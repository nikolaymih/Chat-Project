using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace ChatProjectRefactored.Interfaces
{
    public class BroadcastMessage : IBroadcastMessage
    {
        private TcpClient client;
        private Dictionary<int, BroadcastMessage> connections;
        public string Name { get; }

        public BroadcastMessage([Optional] Dictionary<int, BroadcastMessage> connections, [Optional] string name, [Optional] TcpClient client)
        {
            this.client = client;
            Name = name;
            this.connections = connections;
        }

        //having the two almost identical SendChatMessage and SendInitialMessage is because of the putty, sometimes the text is needed at the beginning
        public void SendChatMessage(string text)
        {
            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes(string.Format("{0}\r\r", text));
                this.client.GetStream().Write(buffer, 0, buffer.Length);
            }
            catch (Exception)
            {
                Console.WriteLine("Message was not sent");
            }
        }
        public void SendInitialMessage(string text, [Optional] TcpClient client)
        {
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(text);
                client.GetStream().Write(bytes, 0, bytes.Length);
            }
            catch (Exception)
            {
                Console.WriteLine("Message was not sent");
            }
        }
        public void SendListWithConnectedUsersToCurrentUser(List<string> connectedClients, TcpClient tClient)
        {
            try
            {
                foreach (var clName in connectedClients)
                {
                    //iterate through all connected users and send them to current one
                    byte[] buffer = Encoding.ASCII.GetBytes(string.Format("{0} is connected\n\r", clName));
                    tClient.GetStream().Write(buffer, 0, buffer.Length);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
