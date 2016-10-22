﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySoftware.utils;
using LibrarySoftware.server;
using LibrarySoftware.network.packets;

namespace LibrarySoftware.network.server
{
    class ServerNetworkManager
    {
        private static Connection conn;

        public static void openSocket(Address address)
        {
            closeConnection();
            conn = new LibrarySoftware.Connection();
            conn.startListening(address, Server.instance);
        }

        public static void receivedPacketFromClient(Client client, IPacket packet)
        {
            switch (packet.getPacketID())
            {
                case Registry.packet_loginData:
                    string username = ((LoginDataPacket)packet).username;
                    string password = ((LoginDataPacket)packet).password;

                    //TODO autentikace

                    sendPacketToClient(client, new LoginReplyPacket(1));
                    return;
                case Registry.packet_loginReply:
                    return; // client-only packet
            }
        }

        public static void sendPacketToClient(Client client, IPacket packet)
        {
            if (client != null)
            {
                client.sendPacket(packet);
            }
        }

        public static void closeConnection()
        {
            if (conn != null)
            {
                conn.closeConnection();
            }
        }
    }
}
