using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using ChatProjectRefactored.Interfaces;
using System.Runtime.InteropServices;

namespace ChatProjectRefactored
{
    class BroadcastMessageLogic : IBroadcastMessageLogic
    {
        private TcpClient tClient;
        private string name;
        private bool done;
        private Dictionary<int, BroadcastMessage> copyOfDictionary;
        private IBroadcastMessage broadcastMessage;

        public BroadcastMessageLogic([Optional] Dictionary<int, BroadcastMessage> copyOfDictionary, [Optional] TcpClient tClient, [Optional] string name, [Optional] bool done)
        {
            this.tClient = tClient;
            this.name = name;
            this.done = done;
            this.copyOfDictionary = copyOfDictionary;
            this.broadcastMessage = new BroadcastMessage();
        }

        public bool CheckForDublicatingName()
        {
            try
            {
                foreach (var client in copyOfDictionary)
                {
                    var state = client.Value;

                    if (state.Name == name)
                    {
                        broadcastMessage.SendInitialMessage(string.Format("This username is already taken, please type a new one= "), tClient);
                        done = false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return done;
        }
        public void BroadcastMessageToAllExceptCurrent(string text, [Optional] Dictionary<int, BroadcastMessage> copyOfDictionary)
        {
            try
            {
                foreach (var clientId in copyOfDictionary)
                {
                    if (clientId.Key != Thread.CurrentThread.ManagedThreadId)
                    {
                        BroadcastMessage user = clientId.Value;
                        user.SendChatMessage(text);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public void SendConnectedUsersToSingleUser(Dictionary<int, BroadcastMessage> CopyOfConnections, string name, TcpClient tClient)
        {
            try
            {
                foreach (var client in CopyOfConnections)
                {
                    BroadcastMessage connectedUser = client.Value;

                    if (connectedUser.Name == name)
                    {
                        List<string> clientsIncluded = new List<string>();

                        foreach (var clName in CopyOfConnections)
                        {
                            BroadcastMessage clientIncluded = clName.Value;
                            clientsIncluded.Add(clientIncluded.Name);
                        }

                        BroadcastMessage user = client.Value;
                        user.SendListWithConnectedUsersToCurrentUser(clientsIncluded, tClient);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
        public void BroadcastMessageToSpecifiedUser(string text, string receiverName, Dictionary<int, BroadcastMessage> copyOfDictionary)
        {
            try
            {
                    bool isValidname = false;

                    foreach (var userConnected in copyOfDictionary)
                    {
                        BroadcastMessage userName = userConnected.Value;
                        int userKey = userConnected.Key;

                        if (userName.Name == receiverName && userKey != Thread.CurrentThread.ManagedThreadId)
                        {
                            isValidname = true;
                            BroadcastMessage user = userConnected.Value;
                            user.SendChatMessage(text);
                        }
                    }

                    if (!isValidname)
                    {
                        foreach (var clientKey in copyOfDictionary)
                        {
                            if (clientKey.Key == Thread.CurrentThread.ManagedThreadId)
                            {
                                BroadcastMessage user = clientKey.Value;
                                user.SendChatMessage(string.Format("Invalid name! And please make sure to follow the following syntax= :whisper <Name>, <Message>\n"));
                            }
                        }
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
        public void SingleMessageToCurrentUser(string text, Dictionary<int, BroadcastMessage> connections)
        {
            try
            {
                    foreach (var clientKey in connections)
                    {
                        if (clientKey.Key == Thread.CurrentThread.ManagedThreadId)
                        {
                            BroadcastMessage user = clientKey.Value;
                            user.SendChatMessage(text);
                        }
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
        public void BroadcastMessageToAllUsers(string text, Dictionary<int, BroadcastMessage> copyOfConnections)
        {
            try
            {
                    foreach (var clientId in copyOfConnections)
                    {
                        BroadcastMessage user = clientId.Value;
                        user.SendChatMessage(text);
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
