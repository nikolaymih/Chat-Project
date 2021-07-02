using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using ChatProjectRefactored.Interfaces;


namespace ChatProjectRefactored
{
    public class ManageConnectedUser : IManageConnectedUser
    {
        private Dictionary<int, BroadcastMessage> connections;
        private string welcomeMessage;

        public ManageConnectedUser(Dictionary<int, BroadcastMessage> connections, string welcomeMessage)
        {
            this.connections = connections;
            this.welcomeMessage = welcomeMessage;
        }

        public void OnClientConnection(Object client)
        {
            TcpClient tClient = (TcpClient)client;

            Console.WriteLine("This someone is now Connected on (Thread: {0})", Thread.CurrentThread.ManagedThreadId);

            IBroadcastMessage message = new BroadcastMessage();

            message.SendInitialMessage(string.Format("First enter your name (type :meet <client name>, without <>)= "), tClient);

            string name = string.Empty;
            bool done = false;

            IMessageParserAndValidation messageParser = new MessageParserAndValidation(tClient, name, done, connections);

            name = messageParser.ClientNameParserAndValidation();

            message.SendInitialMessage(string.Format($"{welcomeMessage} {name}! Please type :help to display all the available commands\n\r"), tClient);

            ISynchronizationManager syncronizeSharedMemory = new SynchronizationManager(connections);
            syncronizeSharedMemory.AddConnection(name, tClient);

            Console.WriteLine("Total connections: {0}", connections.Count);

            Dictionary<int, BroadcastMessage> copyOfDictionary = syncronizeSharedMemory.GetCopy();

            IBroadcastMessageLogic prepareMessage = new BroadcastMessageLogic(copyOfDictionary, tClient, name);
            prepareMessage.BroadcastMessageToAllExceptCurrent(string.Format("\t{0} is connected to the global chat \n", name), copyOfDictionary);

            IChatLogic logicForConnectedUsers = new ChatLogic(tClient, name, connections);
            logicForConnectedUsers.clientMessageHandler();

            Console.WriteLine("Client (Thread: {0}): Terminated!", Thread.CurrentThread.ManagedThreadId);

            Console.WriteLine("Total connections: {0}", connections.Count);

            tClient.Close();
            Thread.CurrentThread.Interrupt();
        }
    }
}
