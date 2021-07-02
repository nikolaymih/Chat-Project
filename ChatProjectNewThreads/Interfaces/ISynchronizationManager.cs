using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace ChatProjectRefactored.Interfaces
{
    interface ISynchronizationManager
    {
        Dictionary<int, BroadcastMessage> GetCopy();
        void AddConnection(string name, TcpClient tClient);
        void RemoveConnection(TcpClient tClient);
    }
}
