using System;
using System.Net.Sockets;
using System.Threading;
using ChatProjectRefactored.Interfaces;

namespace ChatProjectRefactored
{
    class Server : IServer
    {
        private TcpListener tcpListener;
        private Thread serverthread;
        private int port;
        private int maxConnections;
        private string welcomeMessage;
        
        public Server(int port, int maxConnections, string welcomeMessage)
        {
            this.tcpListener = new TcpListener(System.Net.IPAddress.Parse("0.0.0.0"), port);
            this.serverthread = new Thread(new ThreadStart(onServerStart));
            this.port = port;
            this.maxConnections = maxConnections;
            this.welcomeMessage = welcomeMessage;
        }

        public void ServerStartAndStop()
        {
            this.serverthread.Start();

            StopServer stopServer = new StopServer();

            stopServer.onExitEventHandler();
        }

        private void onServerStart()
        {
            tcpListener.Start();

            Console.WriteLine($"The server has started listening on port {port}...");

            IConnectionManager handleConnection = new ConnectionManager(maxConnections, welcomeMessage, tcpListener);
            
            handleConnection.OnSocketReceive();
        }
    }
}
