using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProjectRefactored.Interfaces
{
    interface IManageConnectedUser
    {
         void OnClientConnection(Object client);
    }
}
