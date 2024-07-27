using FlashCards.Controllers;
using FlashCards.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.View
{
    internal class ConsoleView
    {
        private DatabaseController _databaseController;

        public ConsoleView(DatabaseController databaseController)
        {
            _databaseController = databaseController;
        }
        public void WriteHeader()
        {
            AnsiConsole.Write(new FigletText("FLASHCARDS").Color(Color.White).LeftJustified());
        }
        public string PromptMainMenu()
        {
            AnsiConsole.Clear();
            WriteHeader();
            var userChoice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("[yellow]Choose a command:[/]").AddChoices(new[]
            {"MODIFY STACKS", "MODIFY FLASHCARDS", "STUDY" , "DELETE STUDY SESSION",  "EXIT"}).HighlightStyle(new Style(Color.Black, Color.White)));
            return userChoice;
        }
    }
}
