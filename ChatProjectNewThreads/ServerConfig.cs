using System;
using System.Linq;
using System.Collections.Generic;

namespace ChatProjectRefactored
{
    class ServerConfig
    {
        private int port = 3456;
        private int? maxConnections = null;
        private string welcomeMessage;
        private string[] args;
        public static List<string> sortedArgs = new List<string>();

        public ServerConfig(string[] args)
        {
            this.args = args;
        }

        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        public int MaxConnections
        {
            get { return maxConnections.Value; }
            set { maxConnections = value; }
        }

        public string WelcomeMessage
        {
            get { return welcomeMessage; }
            set { welcomeMessage = value; }
        }

        public void configurationHandler()
        {
            try
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (i % 2 != 0)
                    {
                        sortedArgs.Add(args[i - 1] + " " + args[i]);
                    }
                }

                foreach (var argument in sortedArgs)
                {
                    string[] separateArgument = argument.Split(" ");
                    string command = separateArgument[0];
                    string commandArgument = string.Join(" ", separateArgument.Skip(1));

                    switch (command)
                    {
                        case "-p":
                            Port = Convert.ToInt32(commandArgument);
                            Console.WriteLine($"Port successfully changed to {port}");
                            break;
                        case "-n":
                            MaxConnections = Convert.ToInt32(commandArgument);
                            Console.WriteLine($"Max connections successfully set to {maxConnections}");
                            break;

                        case "-m":
                            WelcomeMessage = commandArgument;
                            Console.WriteLine($"Welcome Message successfully set to {WelcomeMessage}\n");
                            break;

                        default:
                            Console.WriteLine("Wrong command! Please try again!");
                            break;
                    }

                }

                if (string.IsNullOrEmpty(maxConnections.ToString()) || string.IsNullOrEmpty(welcomeMessage))
                {
                    throw new Exception();
                }

            }
            catch (Exception)
            {
                Console.WriteLine("Wrong arguments! Please type the correct format");
                Environment.Exit(1);
            }
        }
    }
}
