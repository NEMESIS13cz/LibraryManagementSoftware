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

        private string Serializace (Book book)
        {
            XmlSerializer serializer = new XmlSerializer(book.GetType());

            using(StringWriter textWriter = new StringWriter()) // ošetření chyby
            {
                serializer.Serialize(textWriter, book);
                return textWriter.ToString();
            }
        }

        private string Serializace (Reader reader)
        {
            XmlSerializer serializer = new XmlSerializer(reader.GetType());

            using (StringWriter textWriter = new StringWriter()) // ošetření chyby
            {
                serializer.Serialize(textWriter, reader);
                return textWriter.ToString();
            }
        }
        /*
        // Potřeba domyslet, jak rozlišit co přichází, jestli info o čtenáři nebo kniha
        
        samo o sobě nemůžou být 2 funkce se stejným parametrem - musíme nějak rozlišit
        co přišlo

        private Book Deserializace (string inComingData)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Book));

            using(StringReader textReader = new StringReader(inComingData))
            {
                return serializer.Deserialize(textReader) as Book;
            }
        }

        private Reader Deserializace (string inComingData)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Reader));

            using (StringReader textReader = new StringReader(inComingData))
            {
                return serializer.Deserialize(textReader) as Reader;
            }
        } */
    }
}
