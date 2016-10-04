using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySoftware.utils;
using LibrarySoftware.network;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace LibrarySoftware
{
    class Connection
    {
        private byte[] buffer = new byte[1024];
        private Socket sock;
        private Server server;

        public void startListening(Address address, Server server)
        {
            this.server = server;
            try
            {
                IPHostEntry info = Dns.GetHostEntry(address.host);
                IPAddress ip = info.AddressList[0];
                IPEndPoint localEnd = new IPEndPoint(ip, address.port);

                sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                sock.Bind(localEnd);
                sock.Listen(16);

                ThreadStart start = new ThreadStart(connectionListener);
                Thread thread = new Thread(start);
                thread.Start();
            }
            catch (SocketException e)
            {
                Console.Error.WriteLine("Nepodařilo se vytvořit socket!");
                Console.Error.WriteLine(e.StackTrace);
                Environment.Exit(-1);
            }
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
        
        private void connectionListener()
        {
            Console.WriteLine("Socket vytvořen...");

            // TODO check if server is running
            while (server.isRunning)
            {
                Socket clientSocket = sock.Accept();
                Client client = new Client(clientSocket);

                server.clients.Add(client);
            }
        }
    }
}
