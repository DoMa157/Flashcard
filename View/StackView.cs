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
    internal class StackView
    {
        private DatabaseController _databaseController;

        public StackView(DatabaseController databaseController)
        {

            _databaseController = databaseController;
        }
        public string PromptStacks()
        {
            AnsiConsole.Clear();
            var userChoice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("[yellow]Choose a command:[/]").AddChoices(new[]
            {"ADD STACK", "DELETE STACK", "MODIFY STACK", "EXIT"}).HighlightStyle(new Style(Color.Black, Color.White)));
            return userChoice;
        }
        public void ViewFlashcards()
        {
            AnsiConsole.Clear();
            var flashcards = _databaseController.FlashcardController.GetAllFlashcards();
            var table = new Table();
            table.AddColumn("Id");
            table.AddColumn("Front");
            table.AddColumn("Back");
            table.AddColumn("Stack");
            foreach (var flashcard in flashcards)
            {
                table.AddRow(new Markup($"[green]{flashcard.Id}[/]"), new Markup($"{flashcard.Front}"), new Markup($"{flashcard.Back}"),
                    new Markup($"{flashcard.StackName}"));
            }
            AnsiConsole.Write(table);
        }
        public void ViewStacks(List<FlashcardStackDTO> stacks)
        {
            AnsiConsole.Clear();
            var table = new Table();
            table.AddColumn("Name");

            foreach (var stack in stacks)
            {
                table.AddRow(new Markup($"[green]{stack.StackName}[/]"));
            }
            AnsiConsole.Write(table);
        }
        public void ViewStackFlashcards(FlashcardStackDTO stack)
        {
            AnsiConsole.Clear();
            var table = new Table();
            table.AddColumn("Id");
            table.AddColumn("Front");
            table.AddColumn("Back");

            foreach (var flashcard in stack.Flashcards)
            {
                table.AddRow(new Markup($"[green]{flashcard.Id}[/]"), new Markup($"{flashcard.Front}"), new Markup($"{flashcard.Back}"));
            }
            AnsiConsole.Write(table);
        }
        public void AddStack()
        {
            AnsiConsole.Clear();
            var stacks = _databaseController.StackController.GetStacks();
            ViewStacks(stacks);
            var stackName = InputHandler.GetStringInput("[yellow]Stack Name[/] or type Q to exit:\n");
            if (Validator.CheckForExit(stackName))
            {
                return;
            }
            var stack = new FlashcardStackDTO { StackName = stackName };
            while (!_databaseController.StackController.AddStack(stack))
            {
                stackName = InputHandler.GetStringInput("[yellow]Please provide correct Stack Name[/] or type Q to exit:\n");
                if (Validator.CheckForExit(stackName))
                {
                    return;
                }
                stack = new FlashcardStackDTO { StackName = stackName };
            }
        }

        public void DeleteStack()
        {
            AnsiConsole.Clear();
            ViewStacks(_databaseController.StackController.GetStacks());
            var stackName = InputHandler.GetStringInput("[yellow]Stack name to delete[/] or type Q to exit:\n");
            if (Validator.CheckForExit(stackName))
            {
                return;
            }
            var stack = new FlashcardStackDTO { StackName = stackName };
            while (!_databaseController.StackController.DeleteStack(stack))
            {
                stackName = InputHandler.GetStringInput("[yellow]Please provide correct Stack Name:[/]\n");
                stack = new FlashcardStackDTO { StackName = stackName };
            }
        }
        public void ModifyStack()
        {
            AnsiConsole.Clear();
            ViewStacks(_databaseController.StackController.GetStacks());
            var stackName = InputHandler.GetStringInput("[yellow]Stack name to modify:[/] or type Q to exit:\n");
            if (Validator.CheckForExit(stackName))
            {
                return;
            }
            var oldStack = new FlashcardStackDTO { StackName = stackName };
            while (!_databaseController.StackController.CheckForStack(oldStack))
            {
                stackName = InputHandler.GetStringInput("[yellow]Please provide correct stack name[/] or type Q to exit:\n");
                if (Validator.CheckForExit(stackName))
                {
                    return;
                }
                oldStack = new FlashcardStackDTO { StackName = stackName };
            }
            var newStackName = InputHandler.GetStringInput("[yellow]New stack name[/] or type Q to exit:\n");
            if (Validator.CheckForExit(newStackName))
            {
                return;
            }
            var newStack = new FlashcardStackDTO { StackName = newStackName };
            if (!_databaseController.StackController.ModifyStackName(oldStack, newStack))
            {
                AnsiConsole.Write("We apologise, an error occurred.");
            }
        }
    }
}

