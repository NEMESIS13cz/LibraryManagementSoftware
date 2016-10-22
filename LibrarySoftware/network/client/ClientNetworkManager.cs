using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySoftware.utils;
using LibrarySoftware.network.packets;

namespace LibrarySoftware.network.client
{
    class ClientNetworkManager
    {
        private static Client client;
        private static Connection conn;

        public static bool connectToServer(Address address)
        {
            disconnect();
            conn = new Connection();
            client = conn.connect(address);
            if (client == null)
            {
                disconnect();
                return false;
            }
            return true;
        }
        
        public static void receivedPacketFromServer(IPacket packet)
        {
            switch (packet.getPacketID())
            {
                case Registry.packet_loginData:
                    return; // server-only packet
                case Registry.packet_loginReply:
                    byte returnCode = ((LoginReplyPacket)packet).status;
                    // TODO magik
                    return;
            }
        }

        public static void sendPacketToServer(IPacket packet)
        {
            if (client != null)
            {
                client.sendPacket(packet);
            }
        }

        public static void disconnect()
        {
            if (conn != null)
            {
                conn.closeConnection();
                conn = null;
                client = null;
            }
        }
    }
}
