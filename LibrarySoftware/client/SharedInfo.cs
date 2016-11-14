using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySoftware.data;

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

        public static void reset()
        {
            userType = 0;
            currentUser = null;
            currentlyEditingBook = null;
            currentlyEditingUser = null;
        }
    }
}
