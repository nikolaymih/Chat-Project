using System;
using System.Net.Sockets;

namespace ChatProjectRefactored.Interfaces
{
    interface IMessageReceiver
    {
        string stringCreateHandler(TcpClient client);
    }
}
