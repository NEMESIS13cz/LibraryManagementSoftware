using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySoftware.utils;

namespace LibrarySoftware.server
{
    class Config
    {
        public static int serverPort = Registry.serverPort;
        public static string adminPassword = Registry.defaultAdminPass;
    }
}
