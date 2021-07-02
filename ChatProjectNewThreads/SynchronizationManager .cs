using System;
using System.Collections.Generic;
using ChatProjectRefactored.Interfaces;
using System.Net.Sockets;
using System.Threading;

namespace ChatProjectRefactored
{
    class SynchronizationManager : ISynchronizationManager
    {
        private Dictionary<int, BroadcastMessage> connections;
        private static object _lockObject = new object();

        public SynchronizationManager(Dictionary<int, BroadcastMessage> connections)
        {
            this.connections = connections;
        }

        public Dictionary<int, BroadcastMessage> GetCopy()
        {
            lock (_lockObject)
            {
                Dictionary<int, BroadcastMessage> copyDictionary = new Dictionary<int, BroadcastMessage>(connections);

                return copyDictionary;
            }
        }

        public void AddConnection(string name, TcpClient tClient )
        {
            lock (_lockObject)
            {
                connections.Add(Thread.CurrentThread.ManagedThreadId, new BroadcastMessage(connections, name, tClient));
            }
        }

        public void RemoveConnection(TcpClient tClient)
        {
            lock (_lockObject)
            {
                connections.Remove(Thread.CurrentThread.ManagedThreadId);
            }
            tClient.Close();
        }
    }
}
