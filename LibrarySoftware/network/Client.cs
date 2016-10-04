using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;

namespace LibrarySoftware.network
{
    class Client
    {
        private Socket sock;
        private Thread receiver;
        private Thread transmitter;
        private List<IPacket> toSend = new List<IPacket>();
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
            // TODO synchronized lists
            toSend.Add(packet);
        }

        private void receiverRun()
        {
            while (sock.Connected)
            {

            }
        }

        private void transmitterRun()
        {
            byte[] buffer;
            while (sock.Connected)
            {
                // TODO synchronize lists
                try
                {
                    if (toSend.Count > 0)
                    {
                        IPacket packet = toSend[0];
                        toSend.RemoveAt(0);
                        buffer = Encoding.ASCII.GetBytes(packet.ToString());
                        sock.Send(buffer);
                    }
                }
                catch (ArgumentOutOfRangeException) {
                }
            }
        }
    }
}
