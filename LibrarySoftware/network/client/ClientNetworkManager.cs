using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySoftware.utils;
using LibrarySoftware.network.packets;
using LibrarySoftware.client;
using System.Windows.Threading;
using System.Threading;

namespace LibrarySoftware.network.client
{
    class ClientNetworkManager
    {
        private static Client client;
        private static Connection conn;
        private static List<IPacket> syncPackets = new List<IPacket>();

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
                case Registry.packet_loginReply:
                case Registry.packet_bookData:
                case Registry.packet_readerData:
                case Registry.packet_searchReplyBooks:
                case Registry.packet_searchReplyUsers:
                    lock (syncPackets)
                    {
                        syncPackets.Add(packet);
                    }
                    return;
                default:
                    return;
            }
        }

        private static IPacket receivedPacketSync(IPacket packet)
        {
            switch (packet.getPacketID())
            {
                case Registry.packet_loginReply:
                    SharedInfo.userType = ((LoginReplyPacket)packet).status;
                    SharedInfo.currentUser = ((LoginReplyPacket)packet).reader;
                    return null;
                default:
                    return packet;
            }
        }

        public static IPacket pollSynchronizedPackets()
        {
            while (true)
            {
                lock (syncPackets)
                {
                    if (syncPackets.Count > 0)
                    {
                        IPacket packet = syncPackets[0];
                        syncPackets.RemoveAt(0);
                        return receivedPacketSync(packet);
                    }
                }
                Thread.Sleep(1);
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
