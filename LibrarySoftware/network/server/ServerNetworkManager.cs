using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySoftware.utils;
using LibrarySoftware.server;
using LibrarySoftware.network.packets;
using LibrarySoftware.data;

namespace LibrarySoftware.network.server
{
    class ServerNetworkManager
    {
        private static Connection conn;

        public static void openSocket(Address address)
        {
            closeConnection();
            conn = new Connection();
            conn.startListening(address, Server.instance);
        }

        public static void receivedPacketFromClient(Client client, IPacket packet)
        {
            if (packet == null)
            {
                return;
            }
            switch (packet.getPacketID())
            {
                case Registry.packet_loginData:
                    string username = ((LoginDataPacket)packet).username;
                    string password = ((LoginDataPacket)packet).password;

                    string ID = Database.getUserID(username);
                    if (ID.Equals("-"))
                    {
                        // neexistuje
                        sendPacketToClient(client, new LoginReplyPacket(null, 4));
                        return;
                    }
                    Reader r = Database.getUser(ID);
                    if (r == null || r.ID.Equals("-"))
                    {
                        // chyba
                        sendPacketToClient(client, new LoginReplyPacket(null, 0));
                        return;
                    }
                    if (Authenticator.passwordsMatch(password, r.password))
                    {
                        r.password = ""; // neposílat hash hesla
                        if (r.administrator)
                        {
                            // admin
                            sendPacketToClient(client, new LoginReplyPacket(r, 2));
                            Logger.log("[Network]: Administrátor " + r.name + " (" + r.email + ") se přihlásil.");
                            return;
                        }
                        else
                        {
                            // normální čtenář
                            sendPacketToClient(client, new LoginReplyPacket(r, 1));
                            Logger.log("[Network]: Čtenář " + r.name + " (" + r.email + ") se přihlásil.");
                            return;
                        }
                    }
                    // špatné heslo
                    sendPacketToClient(client, new LoginReplyPacket(null, 3));
                    return;
                case Registry.packet_deleteBook:
                    Database.deleteBook(((DeleteBookPacket)packet).ISBN);
                    return;
                case Registry.packet_addBook:
                    Database.addBook(((AddBookPacket)packet).book);
                    return;
                case Registry.packet_modifyBook:
                    Database.updateBook(((ModifyBookPacket)packet).ISBN, ((ModifyBookPacket)packet).book);
                    return;
                case Registry.packet_requestBook:
                    Book b = Database.getBook(((BookRequestPacket)packet).ISBN);
                    sendPacketToClient(client, new BookPacket(b));
                    return;
                case Registry.packet_searchBooks:
                    int count = ((SearchBooksPacket)packet).amountOfBooks;
                    int offset = ((SearchBooksPacket)packet).offsetOfBooks;
                    List<Book> books = Database.getBooks(((SearchBooksPacket)packet).keyword, ((SearchBooksPacket)packet).category);
                    if (count + offset > books.Count)
                    {
                        count = books.Count - offset;
                    }
                    if (count <= 0)
                    {
                        sendPacketToClient(client, new SearchBooksReplyPacket(new Book[0]));
                        return;
                    }
                    Book[] booksArray = new Book[count];
                    for (int i = offset, j = 0; j < count; i++, j++)
                    {
                        booksArray[j] = books[i];
                    }
                    sendPacketToClient(client, new SearchBooksReplyPacket(booksArray));
                    return;
                case Registry.packet_addUser:
                    if (((AddUserPacket)packet).reader.email.Contains("@"))
                    {
                        Database.addUser(((AddUserPacket)packet).reader);
                    }
                    return;
                case Registry.packet_deleteUser:
                    Database.deleteUser(((DeleteUserPacket)packet).ID);
                    return;
                case Registry.packet_modifyUser:
                    if (((ModifyUserPacket)packet).reader.email.Contains("@"))
                    {
                        Database.updateUser(((ModifyUserPacket)packet).ID, ((ModifyUserPacket)packet).reader);
                    }
                    return;
                case Registry.packet_requestUser:
                    Reader reader = Database.getUser(((ReaderRequestPacket)packet).ID);
                    sendPacketToClient(client, new ReaderPacket(reader));
                    return;
                case Registry.packet_searchUsers:
                    count = ((SearchUsersPacket)packet).amountOfUsers;
                    offset = ((SearchUsersPacket)packet).offsetOfUsers;
                    List<Reader> readers = Database.getReaders(((SearchUsersPacket)packet).keyword, ((SearchUsersPacket)packet).category, ((SearchUsersPacket)packet).getAdmins);
                    if (count + offset > readers.Count)
                    {
                        count = readers.Count - offset;
                    }
                    if (count <= 0)
                    {
                        sendPacketToClient(client, new SearchUsersReplyPacket(new Reader[0]));
                        return;
                    }
                    Reader[] readersArray = new Reader[count];
                    for (int i = offset, j = 0; j < count; i++, j++)
                    {
                        readersArray[j] = readers[i];
                    }
                    sendPacketToClient(client, new SearchUsersReplyPacket(readersArray));
                    return;
                default:
                    return;
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
                if (!Side.isClient)
                {
                    Logger.log("[Network]: Server-Socket vypnut");
                }
            }
        }
    }
}
