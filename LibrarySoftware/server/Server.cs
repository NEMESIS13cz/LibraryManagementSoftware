using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySoftware.utils;

namespace LibrarySoftware
{
    class Server
    {
        private static Server instance;

        static void Main(string[] args)
        {
            Side.isClient = false;
            instance = new LibrarySoftware.Server();
            instance.start();
        }

        public void start()
        {

        }

        private void loadConfigFiles()
        {

        }

        private void initializeMySQL()
        {

        }
    }
}
