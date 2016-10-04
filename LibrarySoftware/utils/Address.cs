using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySoftware.utils
{
    class Address
    {
        public string host { get; }
        public int port { get; }

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
