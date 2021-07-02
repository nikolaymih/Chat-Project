using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Linq;
using System.Threading;
using ChatProjectRefactored.Interfaces;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;


namespace ChatProjectRefactored
{
    class ChatLogic : IChatLogic
    {
        private TcpClient tClient;
        private string name;
        private Dictionary<int, BroadcastMessage> connections;
        Dictionary<int, BroadcastMessage> copyOfDictionary;

        public ChatLogic([Optional]TcpClient tClient, [Optional] string name, [Optional] Dictionary<int, BroadcastMessage> connections)
        {
            this.tClient = tClient;
            this.name = name;
            this.connections = connections;
        }
        public Dictionary<int, BroadcastMessage> Connections { get => connections; }

        public void clientMessageHandler()
        {
            do
            {
                IMessageReceiver stringBuild = new MessageReceiver();
                string text = stringBuild.stringCreateHandler(tClient);

                ISynchronizationManager syncronizeSharedMemory = new SynchronizationManager(connections);
                IBroadcastMessageLogic transmitMessage = new BroadcastMessageLogic();

                try
                {
                    char command = text[0];

                    if (command == ':')
                    {
                        switch (text)
                        {
                            case ":who":
                                copyOfDictionary = syncronizeSharedMemory.GetCopy();

                                transmitMessage.SendConnectedUsersToSingleUser(copyOfDictionary, name, tClient);
                                break;
                            case ":quit":
                                copyOfDictionary = syncronizeSharedMemory.GetCopy();

                                transmitMessage.BroadcastMessageToAllExceptCurrent(string.Format("\t{0} disconnected the global chat\n", name), copyOfDictionary);

                                syncronizeSharedMemory.RemoveConnection(tClient);
                                break;
                            case string whisper when new Regex(@"(:whisper) \w+, .*").IsMatch(text):
                                string[] whisperSeparator = whisper.Split(", ");
                                string[] whisperLeftSeparator = whisperSeparator[0].Split(" ");
                                string messageReceiver = whisperLeftSeparator[1];
                                text = string.Join(", ", whisperSeparator.Skip(1)); ;

                                copyOfDictionary = syncronizeSharedMemory.GetCopy();

                                transmitMessage.BroadcastMessageToSpecifiedUser(string.Format("\t\t{0}: {1}(Private Message) \n", name, text), messageReceiver, copyOfDictionary);

                                break;
                            case ":help":
                                copyOfDictionary = syncronizeSharedMemory.GetCopy();

                                transmitMessage.SingleMessageToCurrentUser(string.Format(":who(asks the server to print all of connected users),\n\r:quit(terminate connection),\n\r:whisper <client name>, <message>\n"), copyOfDictionary);
                                break;
                            default:
                                copyOfDictionary = syncronizeSharedMemory.GetCopy();

                                transmitMessage.SingleMessageToCurrentUser(string.Format("Please type the correct command syntax\n"), copyOfDictionary);
                                break;
                        }
                    }
                    else
                    {
                        copyOfDictionary = syncronizeSharedMemory.GetCopy();

                        transmitMessage.BroadcastMessageToAllExceptCurrent(string.Format("\t\t{0}: {1} \n", name, text), copyOfDictionary);
                    }

                    if (!tClient.Connected)
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Wrong user input! Another attempt message was sent to the client");

                    copyOfDictionary = syncronizeSharedMemory.GetCopy();

                    transmitMessage.SingleMessageToCurrentUser(string.Format("Something went wrong! Please try again!\n"), copyOfDictionary);
                }

            } while (true);
        }
    }
}
