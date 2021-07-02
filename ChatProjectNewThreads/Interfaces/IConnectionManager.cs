using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProjectRefactored.Interfaces
{
    public interface IConnectionManager
    {
        void OnSocketReceive();
    }
}
