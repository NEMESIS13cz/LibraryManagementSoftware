using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySoftware.utils;
using LibrarySoftware.network;
using LibrarySoftware.network.server;
using System.Threading;

namespace LibrarySoftware.server
{
    class Server
    {
        public static Server instance;
        
        private Address addr;

        public bool isRunning = true;
        public List<Client> clients = new List<Client>();

        static void Main(string[] args)
        {
            Side.isClient = false;
            instance = new Server();
            instance.start();
        }

        public void start()
        {
            loadConfigFiles();
            initializeMySQL();
            
            addr = new Address("localhost");
            ServerNetworkManager.openSocket(addr);

            while (true)
            {
                Thread.Sleep(1);
            }
        }

        private void loadConfigFiles()
        {
            //TODO magik
        }

        private void initializeMySQL()
        {
            //TODO magik
        }
    }
}
