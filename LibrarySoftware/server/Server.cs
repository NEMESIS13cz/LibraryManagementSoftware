using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySoftware.utils;
using LibrarySoftware.network;
using LibrarySoftware.network.server;
using System.Threading;
using System.IO;
using System.Data.SQLite;

namespace LibrarySoftware.server
{
    class Server
    {
        public static Server instance;
        
        private Address addr;

        public bool isRunning = true;

        static void Main(string[] args)
        {
            Side.isClient = false;
            instance = new Server();
            instance.start();
        }

        public void start()
        {
            loadConfigFiles();
            initializeDatabase();
            Terminal.start();
            
            addr = new Address("localhost");
            ServerNetworkManager.openSocket(addr);
        }

        private void loadConfigFiles()
        {
            if (File.Exists("library.cfg"))
            {
                StreamReader reader = new StreamReader("library.cfg", Encoding.Default);
                string buffer = "";

                while ((buffer = reader.ReadLine()) != null)
                {
                    if (buffer.Contains('='))
                    {
                        string[] pair = buffer.Split('=');
                        switch (pair[0])
                        {
                            case "port":
                                try
                                {
                                    Config.serverPort = Convert.ToInt32(pair[1]);
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("Config: Invalid value for option 'port' (not a number).");
                                }
                                if (Config.serverPort <= 0 || Config.serverPort > 65535)
                                {
                                    Config.serverPort = Registry.serverPort;
                                    Console.WriteLine("Config: Invalid value for option 'port' (out of range).");
                                }
                                break;
                            case "admin_pass":
                                Config.adminPassword = Authenticator.hashPassword(pair[1]);
                                break;
                        }
                    }
                }
                reader.Close();
            }
            else
            {
                StreamWriter writer = File.CreateText("library.cfg");
                writer.WriteLine("port=" + Registry.serverPort);
                writer.WriteLine("admin_pass=" + Registry.defaultAdminPass);
                writer.Close();
            }
        }

        private void initializeDatabase()
        {
            if (!File.Exists("database.sql"))
            {
                SQLiteConnection.CreateFile("database.sql");
            }
            Database.connect();
        }
    }
}
