using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.View
{
    internal class InputHandler
    {
        public static string GetStringInput(string message)
        {
            string userInput = AnsiConsole.Ask<string>(message);
            return userInput;
        }

        public static int GetIntInput(string message)
        {
            var userInput = AnsiConsole.Ask<string>(message);
            return userInput.Trim().ToLower() == "q" || !Int32.TryParse(userInput, out _) ? -1 : Int32.Parse(userInput);
        }
    }
}
