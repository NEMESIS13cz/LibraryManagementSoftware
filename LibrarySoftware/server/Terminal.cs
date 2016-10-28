using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using LibrarySoftware.network.server;

namespace LibrarySoftware.server
{
    class Terminal
    {
        public static void start()
        {
            Thread t = new Thread(new ThreadStart(getInput));
            t.Start();
        }

        private static void getInput()
        {
            while (Server.instance.isRunning)
            {
                string command = Console.ReadLine();
                switch (command)
                {
                    case "stop":
                        Server.instance.isRunning = false;
                        ServerNetworkManager.closeConnection();
                        Database.close();
                        break;
                    default:
                        break;
                }
                Thread.Sleep(1);
            }
        }
    }
}
