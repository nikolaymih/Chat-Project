using System;
using ChatProjectRefactored.Interfaces;

namespace ChatProjectRefactored
{
    class Program
    {
        static void Main(string[] args)
        {
            StopServer stopServer = new StopServer();

            stopServer.onExitEventHandler();

            ServerConfig config = new ServerConfig(args);

            config.configurationHandler();

            IServer connection = new Server(config.Port, config.MaxConnections, config.WelcomeMessage);
            
            connection.ExecuteTcpListener();
        }
    }
}
