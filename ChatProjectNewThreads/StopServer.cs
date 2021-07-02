using System;
using System.Collections.Generic;
using System.Threading;
using ChatProjectRefactored.Interfaces;
using System.Runtime.InteropServices;

namespace ChatProjectRefactored
{
    class StopServer
    {
        private string text;
        private delegate void EventHandler(CtrlType sig);
        static EventHandler _handler;
        private Dictionary<int, BroadcastMessage> connections;
        private Dictionary<int, BroadcastMessage> copyOfConnections;


        #region Trap application termination
        [DllImport("Kernel32")]

        private static extern void SetConsoleCtrlHandler(EventHandler handler, bool add);
        public enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }
        public void onExitEventHandler()
        {
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);
        }
        public void Handler(CtrlType sig)
        {
            Console.WriteLine("Exiting system due to external CTRL-C, or process kill, or shutdown");

            Thread.Sleep(5000);

            Environment.Exit(0);
        }
        #endregion
    }
}
