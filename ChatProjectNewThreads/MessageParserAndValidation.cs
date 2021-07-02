using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Text.RegularExpressions;
using ChatProjectRefactored.Interfaces;

namespace ChatProjectRefactored
{
    class MessageParserAndValidation : IMessageParserAndValidation
    {
        private TcpClient tClient;
        private string name;
        private bool done;
        private ISynchronizationManager syncronizeSharedMemory;

        public MessageParserAndValidation(TcpClient tclient, string name, bool done, Dictionary<int, BroadcastMessage> connections)
        {
            this.tClient = tclient;
            this.name = name;
            this.done = done;
            this.syncronizeSharedMemory = new SynchronizationManager(connections);
        }

        public string ClientNameParserAndValidation()
        {
            do
            {
                if (!tClient.Connected) //killing the thread if the user disconnects
                {
                    Console.WriteLine("Client (Thread: {0}): Terminated!", Thread.CurrentThread.ManagedThreadId);
                    tClient.Close();
                    Thread.CurrentThread.Interrupt();
                }

                try
                {
                    IMessageReceiver stringBuild = new MessageReceiver();

                    name = stringBuild.stringCreateHandler(tClient);
                    done = true;

                    Regex r = new Regex(@"(:meet) (\w+)");
                    Match m = r.Match(name);

                    if (!m.Success)
                    {
                        IBroadcastMessage message = new BroadcastMessage();
                        message.SendInitialMessage(string.Format("Wrong command! Please type :meet <client name>, without <>= "), tClient);
                        done = false;
                        continue;
                    }

                    name = name.Remove(0, 6);

                    if (done)
                    {
                        Dictionary<int, BroadcastMessage> copyOfDictionary = syncronizeSharedMemory.GetCopy();

                        IBroadcastMessageLogic middleware = new BroadcastMessageLogic(copyOfDictionary, tClient, name, done);
                        done = middleware.CheckForDublicatingName();
                    }
                }

                catch (Exception)
                {
                    IBroadcastMessage message = new BroadcastMessage();
                    message.SendInitialMessage(string.Format("Something went wrong! Please type :meet <client name>, without <>= "), tClient);

                    Console.WriteLine("Wrong user input! Another attempt message was sent to the client");

                    continue;
                }

            } while (!done);

            return name;
        }
    }
}
