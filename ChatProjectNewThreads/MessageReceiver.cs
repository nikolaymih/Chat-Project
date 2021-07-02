using System;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using ChatProjectRefactored.Interfaces;


namespace ChatProjectRefactored
{
    class MessageReceiver : IMessageReceiver
    {
        public string stringCreateHandler(TcpClient client)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                do
                {
                    if (client.Available > 0)
                    {
                        while (client.Available > 0)
                        {
                            char ch = (char)client.GetStream().ReadByte();

                            if (ch == '\r')
                            {
                                continue;
                            }
                            if (ch == '\n')
                            {
                                return sb.ToString();
                            }

                            sb.Append(ch);

                        }
                    }

                    Thread.Sleep(10);

                } while (true);
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
