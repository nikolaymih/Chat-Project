using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices;


namespace ChatProjectRefactored.Interfaces
{
    interface IBroadcastMessageLogic
    {
        bool CheckForDublicatingName();
        void BroadcastMessageToAllExceptCurrent(string text, [Optional] Dictionary<int, BroadcastMessage> copyOfDictionary);
        void SendConnectedUsersToSingleUser(Dictionary<int, BroadcastMessage> CopyOfConnections, string name, TcpClient tcpClient);
        void BroadcastMessageToSpecifiedUser(string text, string receiverName, Dictionary<int, BroadcastMessage> copyOfDictionary);
        void SingleMessageToCurrentUser(string text, Dictionary<int, BroadcastMessage> connections);
        void BroadcastMessageToAllUsers(string text, Dictionary<int, BroadcastMessage> copyOfConnections);
    }
}
