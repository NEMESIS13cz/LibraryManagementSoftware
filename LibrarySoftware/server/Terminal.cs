using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using LibrarySoftware.network.server;
using LibrarySoftware.data;

namespace LibrarySoftware.server
{
    class Terminal
    {
        public static void start()
        {
            Thread t = new Thread(new ThreadStart(getInput));
            t.Start();
        }

        private static void getInput()
        {
            while (Server.instance.isRunning)
            {
                string command = Console.ReadLine();
                switch (command)
                {
                    case "stop":
                        Logger.log("[Server]: Vypínání");
                        Server.instance.isRunning = false;
                        ServerNetworkManager.closeConnection();
                        Database.close();
                        Logger.close();
                        break;
                    case "help":
                        Console.WriteLine("books - ukáže jména všech knih");
                        Console.WriteLine("users - ukáže jména a e-maily čtenářů");
                        Console.WriteLine("admins - ukáže jména a e-maily administrátorů");
                        Console.WriteLine("stop - vypne server");
                        Console.WriteLine("help - úkáže pomoc");
                        break;
                    case "books":
                        foreach (Book b in Database.getBooks("", 0))
                        {
                            Console.WriteLine(b.name + " - " + b.author + " - " + b.ISBN);
                        }
                        break;
                    case "users":
                        foreach (Reader r in Database.getReaders("", 0, false))
                        {
                            Console.WriteLine(r.name + " - " + r.email + " ID: " + r.ID);
                        }
                        break;
                    case "admins":
                        foreach (Reader r in Database.getReaders("", 0, true))
                        {
                            Console.WriteLine(r.name + " - " + r.email + " ID: " + r.ID);
                        }
                        break;
                    default:
                        Console.WriteLine("Neznámý příkaz, použijte 'help' pro více informací");
                        break;
                }
                Thread.Sleep(1);
            }
        }
    }
}
