using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySoftware.utils;
using LibrarySoftware.network;

namespace LibrarySoftware
{
    class Server
    {
        private static Server instance;

        private Connection conn;
        private Address addr;

        public bool isRunning = true;
        public List<Client> clients = new List<Client>();
        static void Main(string[] args)
        {
            Side.isClient = false;
            instance = new LibrarySoftware.Server();
            instance.start();
        }

        public void start()
        {
            loadConfigFiles();
            initializeMySQL();

            conn = new Connection();
            addr = new Address("localhost");
            conn.startListening(addr, this);
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
