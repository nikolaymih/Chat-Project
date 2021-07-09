using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChatProjectRefactored.Interfaces;
using System.Runtime.InteropServices;

namespace ChatProjectRefactored
{
    public class ConnectionManager : IConnectionManager
    {
        private TcpListener tcpListener;
        private int maxConnections;
        private string welcomeMessage;
        private Dictionary<int, BroadcastMessage> connections = new Dictionary<int, BroadcastMessage>();

        private volatile bool exitServer = false;

        public ConnectionManager([Optional] int maxConnections, [Optional] string welcomeMessage, [Optional] TcpListener tcpListener)
        {
            this.tcpListener = tcpListener;
            this.maxConnections = maxConnections;
            this.welcomeMessage = welcomeMessage;
        }

        public void OnSocketReceive()
        {
            while (!this.exitServer)
            {
                if (!tcpListener.Pending())
                {
                    Thread.Sleep(10);
                    continue;
                }

                Console.WriteLine("The server is waiting for a connection...\n");

                TcpClient client = tcpListener.AcceptTcpClient();
                Console.WriteLine("Someone was attached to the server\n");

                bool serverFull = connections.Count >= maxConnections;

                if (serverFull)
                {
                    Console.WriteLine("The server has reached it's maximum capacity");
                    byte[] b = Encoding.ASCII.GetBytes("We are very sorry but the server has reached its maximum users. Please try again later :(");
                    client.GetStream().Write(b, 0, b.Length);

                    Console.WriteLine("Client (Thread: {0}): Terminated!\n", Thread.CurrentThread.ManagedThreadId);
                    
                    Task.Delay(5000).ContinueWith(t => client.Close());
                    continue;
                }

                IManageConnectedUser chatLogic = new ManageConnectedUser(connections, welcomeMessage);

                Thread clientThread = new Thread(new ParameterizedThreadStart(chatLogic.OnClientConnection));
                clientThread.Start(client);
            }
        }
        
    }
}
