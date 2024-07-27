using FlashCards.Controllers;
using FlashCards.Models;
using Spectre.Console;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.View
{
    internal class FlashcardView
    {
        private DatabaseController _databaseController;

        public FlashcardView(DatabaseController databaseController)
        {
            _databaseController = databaseController;
        } 


        public string PromptFlashcards()
        {
            AnsiConsole.Clear();
            var userChoice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("[yellow]Choose a command:[/]").AddChoices(new[]
            {"ADD FLASHCARD", "DELETE FLASHCARD", "MODIFY FLASHCARD", "EXIT"}).HighlightStyle(new Style(Color.Black, Color.White)));
            return userChoice;
        }



        public string QuizFlashcard(Flashcard flashcard)
        {
            var table = new Table();
            table.AddColumn("Front");
            table.AddRow(new Markup($"{flashcard.Front}"));
            AnsiConsole.Write(table);
            return InputHandler.GetStringInput("[yellow]Answer:[/]\n");
        }

        public void AddFlashcard()
        {
            AnsiConsole.Clear();
            var flashcards = _databaseController.FlashcardController.GetAllFlashcards();
            ViewAllFlashcards(flashcards);
            var stackName = InputHandler.GetStringInput("[yellow]Stack Name[/] or type Q to exit:\n");
            if (Validator.CheckForExit(stackName))
            {
                return;
            }
            var stack = new FlashcardStackDTO { StackName = stackName };
            while (!_databaseController.StackController.CheckForStack(stack))
            {
                stackName = InputHandler.GetStringInput("[yellow]Incorrect stack, please provide a correct stack name[/] or type Q to exit:\n");
                if (Validator.CheckForExit(stackName))
                {
                    return;
                }
                stack.StackName = stackName;
            }
            stack.StackId = _databaseController.StackController.GetStackId(stack);
            var front = InputHandler.GetStringInput("[yellow]Front:[/] or type Q to exit:\n");
            if (Validator.CheckForExit(front))
            {
                return;
            }
            var back = InputHandler.GetStringInput("[yellow]Back:[/] or type Q to exit:\n");
            if (Validator.CheckForExit(back))
            {
                return;
            }
            var flashcard = new FlashcardDTO { StackName = stackName, Back = back, Front = front };
            while (!_databaseController.FlashcardController.AddFlashcardIntoStack(flashcard, stack))
            {
                stackName = InputHandler.GetStringInput("[yellow]Please provide correct Stack Name[/] or type Q to exit:\n");
                if (Validator.CheckForExit(stackName))
                {
                    return;
                }
                stack = new FlashcardStackDTO { StackName = stackName };
            }
        }

        public void DeleteFlashcard()
        {
            AnsiConsole.Clear();
            ViewAllFlashcards(_databaseController.FlashcardController.GetAllFlashcards());
            var idToDelete = InputHandler.GetIntInput("[yellow]Id of flashcard to delete[/] or Q to exit:\n");
            if (Validator.CheckForExit(idToDelete))
            {
                return;
            }
            var flashcard = new FlashcardDTO { Id = idToDelete };
            while (!_databaseController.FlashcardController.CheckForFlashcard(flashcard))
            {
                idToDelete = InputHandler.GetIntInput("[yellow]Incorrect Id, please provide correct id of flashcard to delete[/] or Q to exit:\n");
                if (Validator.CheckForExit(idToDelete))
                {
                    return;
                }
                flashcard = new FlashcardDTO { Id = idToDelete };
            }
            if (!_databaseController.FlashcardController.DeleteFlashcard(flashcard))
            {
                AnsiConsole.Write("We apologize, something went wrong :(");
            }
        }

        public void ModifyFlashcard()
        {
            AnsiConsole.Clear();
            ViewAllFlashcards(_databaseController.FlashcardController.GetAllFlashcards());
            var idToModify = InputHandler.GetIntInput("[yellow]Id of flashcard to modify[/] or Q to exit:\n");
            if (Validator.CheckForExit(idToModify))
            {
                return;
            }
            var flashcard = new FlashcardDTO { Id = idToModify};
            while (!_databaseController.FlashcardController.CheckForFlashcard(flashcard))
            {
                idToModify = InputHandler.GetIntInput("[yellow]Incorrect Id, please provide correct id of flashcard to delete[/] or Q to exit:\n");
                if (Validator.CheckForExit(idToModify))
                {
                    return;
                }
                flashcard = new FlashcardDTO { Id = idToModify};
            }
            var front = InputHandler.GetStringInput("[yellow]Front:[/]\n");
            if (Validator.CheckForExit(front)) return;
            var back = InputHandler.GetStringInput("[yellow]Back:[/]\n");
            if (Validator.CheckForExit(back)) return;
            flashcard.Front = front;
            flashcard.Back = back;
            if (!_databaseController.FlashcardController.ModifyFlashCard(flashcard))
            {
                AnsiConsole.Write("We apologize, something went wrong :(");
            }
        }

        public void ViewAllFlashcards(List<FlashcardDTO> flashcardDTOs)
        {
            AnsiConsole.Clear();
            var table = new Table();
            table.AddColumn("Id");
            table.AddColumn("Front");
            table.AddColumn("Back");
            table.AddColumn("Stack");

            foreach (var flashcard in flashcardDTOs)
            {
                table.AddRow(new Markup($"[green]{flashcard.Id}[/]"), new Markup($"{flashcard.Front}"), new Markup($"{flashcard.Back}"), new Markup($"{flashcard.StackName}"));
            }
            AnsiConsole.Write(table);
        }
    }
}
