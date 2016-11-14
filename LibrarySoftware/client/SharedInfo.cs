using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySoftware.data;
using LibrarySoftware.utils;
using System.IO;

namespace LibrarySoftware.client
{
    class SharedInfo
    {
        // Sdílené informace pro předávání mezi okny
        public static byte userType;
        public static Reader currentUser;
        public static Book currentlyEditingBook;
        public static Reader currentlyEditingUser;
        public static bool admin = false; // zda dotyčný je admin
        public static int Port = Registry.serverPort;
        public static string ServerAddress = Registry.serverAddress;
        private static string NameOfFile = "Data.msd";

        public static void reset()
        {
            userType = 0;
            currentUser = null;
            currentlyEditingBook = null;
            currentlyEditingUser = null;
        }
        
        public static void RememberOnIPAndPort()
        {
            StreamReader sw;
            string pom;

            using (sw = new StreamReader(NameOfFile))
            {
                if ((pom = sw.ReadLine()) != null)
                {
                    string[] pole = pom.Split(':');
                    ServerAddress = pole[0];
                    Port = Convert.ToInt32(pole[1]);
                }
            }
        }

        public static void WriteChangeIP ()
        {
            using(StreamWriter sw = new StreamWriter(NameOfFile, false))
            {
                sw.WriteLine(ServerAddress + ":" + Port.ToString());
            }
        }
    }
}
