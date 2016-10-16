using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Xml.Serialization;
using System.IO;
using LibrarySoftware.allAboutBook;
using LibrarySoftware.utils;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using LibrarySoftware.network.client;
using LibrarySoftware.network.server;
using System.Net;

namespace LibrarySoftware.network
{
    class Client
    {
        private Socket sock;
        private Thread receiver;
        private Thread transmitter;
        private List<IPacket> toSend = new List<IPacket>();
        private object lck = new object(); // na zabezpečení vlákna
        private IFormatter serializer = new SoapFormatter();

        public Client(Socket sock)
        {
            this.sock = sock;
            ThreadStart start = new ThreadStart(receiverRun);
            receiver = new Thread(start);
            start = new ThreadStart(transmitterRun);
            transmitter = new Thread(start);
            receiver.Start();
            transmitter.Start();
        }

        public void sendPacket(IPacket packet)
        {
            lock (lck)
            {
                toSend.Add(packet);
            }
        }

        private void receiverRun()
        {
            IPEndPoint ep = (IPEndPoint)sock.RemoteEndPoint;
            List<byte> incomingData = new List<byte>();
            byte[] buffer = null;
            byte[] temp = null;
            int received = 0;

            while (sock.Connected)
            {
                try
                {
                    while (true)
                    {
                        buffer = new byte[1024];
                        received = sock.Receive(buffer);
                        if (received == 1024)
                        {
                            incomingData.AddRange(buffer);
                        }
                        else
                        {
                            temp = new byte[received];
                            Array.Copy(buffer, temp, received);
                            incomingData.AddRange(temp);
                        }
                        if (Enumerable.SequenceEqual(incomingData.GetRange(incomingData.Count - Registry.endOfPacket.Length, Registry.endOfPacket.Length).ToArray(), Registry.endOfPacket))
                        {
                            break;
                        }
                    }
                    if (Side.isClient)
                    {
                        ClientNetworkManager.receivedPacketFromServer(deserialize(incomingData.ToArray()));
                    }
                    else
                    {
                        ServerNetworkManager.receivedPacketFromClient(this, deserialize(incomingData.ToArray()));
                    }
                    incomingData.Clear();
                }
                catch (Exception)
                {
                    if (sock.Poll(1000, SelectMode.SelectRead) && sock.Available == 0)
                    {
                        break;
                    }
                    Console.WriteLine("Chyba při přijímání packetu! Ignoruji...");
                }
            }
            while (transmitter.IsAlive)
            {
                Thread.Sleep(1);
            }
            Console.WriteLine("Pripojení ztraceno (" + ep.Address + ":" + ep.Port + ")");
        }

        private void transmitterRun()
        {
            while (sock.Connected)
            {
                lock (lck)
                {
                    try
                    {
                        if (toSend.Count > 0)
                        {
                            IPacket packet = toSend[0];
                            toSend.RemoveAt(0);
                            sock.Send(serialize(packet));
                        }
                        Thread.Sleep(1);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Chyba při posílání packetu!");
                    }
                }
            }
        }

        private byte[] serialize(IPacket packet)
        {
            MemoryStream mem = new MemoryStream();
            serializer.Serialize(mem, packet);
            mem.Write(Registry.endOfPacket, 0, Registry.endOfPacket.Length);
            return mem.ToArray();
        }

        private IPacket deserialize(byte[] data)
        {
            MemoryStream mem = new MemoryStream(data);
            return (IPacket) serializer.Deserialize(mem);
        }
    }
}
