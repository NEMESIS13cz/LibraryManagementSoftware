using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySoftware.utils
{
    class Address
    {
        private string host { get; }
        private int port { get; }

        public Address(string host, int port)
        {
            this.host = host;
            this.port = port;
        }

        public Address(string host) : this(host, Registry.serverPort)
        {

        }
    }
}
