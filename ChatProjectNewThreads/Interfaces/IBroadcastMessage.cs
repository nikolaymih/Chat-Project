using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace ChatProjectRefactored.Interfaces
{
    interface IBroadcastMessage
    {
        void SendInitialMessage(string text, [Optional] TcpClient client);
        void SendListWithConnectedUsersToCurrentUser(List<string> connectedClients, TcpClient tClient);
    }
}
