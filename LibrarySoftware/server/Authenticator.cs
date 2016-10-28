using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace LibrarySoftware.server
{
    class Authenticator
    {
        public static string hashPassword(string password)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            SHA256Managed hashString = new SHA256Managed();
            byte[] hash = hashString.ComputeHash(bytes);
            string ret = "";
            foreach (byte b in hash)
            {
                ret += string.Format("{0:x2}", b);
            }
            return ret;
        }

        public static bool passwordsMatch(string password, string hash)
        {
            return hashPassword(password).Equals(hash);
        }
    }
}
