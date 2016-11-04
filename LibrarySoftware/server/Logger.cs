using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LibrarySoftware.server
{
    class Logger
    {
        private static StreamWriter writer;
        private static DateTime creationTime;

        public static void open()
        {
            creationTime = DateTime.Now;
            writer = new StreamWriter(creationTime.Year + "-" + creationTime.Month + "-" + creationTime.Day + ".log", true, Encoding.Default);
        }

        public static void close()
        {
            writer.Close();
        }

        public static void log(string msg)
        {
            DateTime now = DateTime.Now;
            if (creationTime.Day != now.Day)
            {
                close();
                open();
            }
            msg = "|" + now.Hour.ToString("00") + ":" + now.Minute.ToString("00") + ":" + now.Second.ToString("00") + "| " + msg;
            writer.WriteLineAsync(msg);
            Console.WriteLine(msg);
        }
    }
}
