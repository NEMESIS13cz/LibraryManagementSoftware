using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySoftware.utils;
using LibrarySoftware.network;

namespace LibrarySoftware
{
    class Connection
    {
        public void startListening()
        {

        }

        public void connect(Address address)
        {

        }

        public void closeConnection()
        {

        }

        public void sendPacket(IPacket packet)
        {

        }

        public void onPacketReceived(IPacket packet)
        {
            if (Side.isClient)
            {

            }
            else
            {

            }
        }
    }
}
