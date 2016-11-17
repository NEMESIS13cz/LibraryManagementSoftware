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
using LibrarySoftware.server;

namespace LibrarySoftware
{
    class Connection
    {
        private Socket sock;
        private Server server;

        public void startListening(Address address, Server server)
        {
            this.server = server;
            try
            {
                IPHostEntry info = Dns.GetHostEntry(address.host);
                IPAddress ip = null;
                foreach (IPAddress i in info.AddressList)
                {
                    if (i.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ip = i;
                        break;
                    }
                }
                if (ip == null)
                {
                    Console.WriteLine("[Network]: Nepodařilo se najít IPv4 adresu!");
                    Environment.Exit(-1);
                }
                IPEndPoint localEnd = new IPEndPoint(ip, address.port);

                sock = new Socket(localEnd.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                sock.Bind(localEnd);
                sock.Listen(16);

                ThreadStart start = new ThreadStart(connectionListener);
                Thread thread = new Thread(start);
                thread.Start();
            }
            catch (SocketException e)
            {
                Console.WriteLine("[Network]: Nepodařilo se vytvořit socket!");
                Console.WriteLine(e.ToString());
                Environment.Exit(-1);
            }
        }

        public Client connect(Address address)
        {
            IPAddress ip = null;
            if (!address.host.Contains("."))
            {
                foreach (IPAddress i in Dns.GetHostEntry(address.host).AddressList)
                {
                    if (i.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ip = i;
                        break;
                    }
                }
            }
            else
            {
                ip = IPAddress.Parse(address.host);
            }
            if (ip == null)
            {
                Console.WriteLine("[Network]: Nepodařilo se najít IPv4 adresu!");
                Environment.Exit(-1);
            }
            IPEndPoint remote = new IPEndPoint(ip, address.port);
            sock = new Socket(remote.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                sock.Connect(remote);
            }
            catch (Exception)
            {
                Console.WriteLine("[Network]: Nepodařilo se připojit (" + remote.Address + ":" + remote.Port + ")");
                return null;
            }
            Console.WriteLine("[Network]: Připojeno (" + remote.Address + ":" + remote.Port + ")");
            return new Client(sock);
        }

        public void closeConnection()
        {
            sock.Close();
        }
        
        private void connectionListener()
        {
            Logger.log("[Network]: Server-Socket inicializován");
            
            while (server.isRunning)
            {
                try
                {
                    Socket clientSocket = sock.Accept();
                    Client client = new Client(clientSocket);
                    IPEndPoint ep = ((IPEndPoint)clientSocket.RemoteEndPoint);

                    Logger.log("[Network]: Nové připojení (" + ep.Address + ":" + ep.Port + ")");
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
