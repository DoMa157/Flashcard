using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.View
{
    internal class Validator
    {
        public static bool CheckForExit(string message)
        {
            return message.Trim().ToLower() == "q";
        }
        public static bool CheckForExit(int message)
        {
            return message == -1;
        }
    }
}
