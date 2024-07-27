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
    internal class StudySessionView
    {
        private DatabaseController _databaseController;
        private StackView _stackView;

        public StudySessionView(DatabaseController databaseController, StackView stackView)
        {
            _databaseController = databaseController;
            _stackView = stackView;
        }

        public void Study()
        {
            AnsiConsole.Clear();
            var stacks = _databaseController.StackController.GetStacks();
            _stackView.ViewStacks(stacks);
            var stackName = InputHandler.GetStringInput("[yellow]Stack Name or press Q to quit:[/]\n");
            if (Validator.CheckForExit(stackName)) return;
            var stack = new FlashcardStackDTO { StackName = stackName};
            while (!_databaseController.StackController.CheckForStack(stack))
            {
                stackName = InputHandler.GetStringInput("[yellow]Provide correct stack name or press Q to quit:[/]\n");
                if (Validator.CheckForExit(stackName)) return;
                stack.StackName = stackName;
            }
            var stackId = stacks.First(x => x.StackName.ToLower() == stackName.ToLower()).StackId;
            stack.StackId = stackId;
            var flashcardDTOs = _databaseController.StackController.GetFlashcards(stack);
            var startingTime = DateTime.Now;
            var score = 0;
            TimeSpan duration;
            StudySession studySession;
            foreach (FlashcardDTO flashcard in flashcardDTOs)
            {
                AnsiConsole.Clear();
                var table = new Table();
                table.AddColumn("Front");
                table.AddRow(new Markup($"[red]{flashcard.Front}[/]"));
                AnsiConsole.Write(table);
                var userInput = InputHandler.GetStringInput($"[yellow]Back or press Q to quit:[/]\n");
                if (Validator.CheckForExit(userInput))
                {
                    duration = DateTime.Now - startingTime;
                    studySession = new StudySession { StackId = stack.StackId, Duration = duration, Score = score, StackName = stack.StackName };
                    _databaseController.StudySessionController.AddSession(studySession);
                    return;
                }
                if (userInput.Trim().ToLower() == flashcard.Back.Trim().ToLower())
                {
                    score++;
                    var newUserInput = InputHandler.GetStringInput("[green]Correct![/] to continue press any button or Q to quit");
                    if (Validator.CheckForExit(newUserInput))
                    {
                        duration = DateTime.Now - startingTime;
                        studySession = new StudySession { StackId = stack.StackId, Duration = duration, Score = score, StackName = stack.StackName };
                        _databaseController.StudySessionController.AddSession(studySession);
                        return;
                    }
                }
                else
                {
                    var newUserInput = InputHandler.GetStringInput("[red]Incorrect![/] to continue press any button or Q to quit");
                    if (Validator.CheckForExit(newUserInput))
                    {
                        duration = DateTime.Now - startingTime;
                        studySession = new StudySession { StackId = stack.StackId, Duration = duration, Score = score, StackName = stack.StackName };
                        _databaseController.StudySessionController.AddSession(studySession);
                        return;
                    }
                }
            }
            duration = DateTime.Now - startingTime;
            studySession = new StudySession { StackId = stack.StackId, Duration = duration, Score = score, StackName = stack.StackName };
            _databaseController.StudySessionController.AddSession(studySession);
            var userInp = InputHandler.GetStringInput($"Your score was: [green]{score}[/] press any button to go back");
        }

        public void ViewSessions(List<StudySessionDTO> sessionDTOs)
        {
            AnsiConsole.Clear();
            var table = new Table();
            table.AddColumn("Id");
            table.AddColumn("StackName");
            table.AddColumn("Duration");
            table.AddColumn("Score");
            foreach(StudySessionDTO sessionDTO in sessionDTOs)
            {
                table.AddRow(new Markup($"[green]{sessionDTO.SessionId}[/])"), new Markup($"{sessionDTO.StackName}"), new Markup($"{sessionDTO.Duration}"), new Markup($"{sessionDTO.Score}"));
            }
            AnsiConsole.Write(table);
        }
        public void DeleteSession()
        {
            AnsiConsole.Clear();
            ViewSessions(_databaseController.StudySessionController.GetSessions());
            var idToDelete = InputHandler.GetIntInput("[yellow]Input ID you want to delete[/] or Q to Quit:\n");
            if (Validator.CheckForExit(idToDelete)) return;
            var studySessionDTO = new StudySessionDTO { SessionId =  idToDelete };
            while (!_databaseController.StudySessionController.CheckSession(studySessionDTO))
            {
                idToDelete = InputHandler.GetIntInput("[yellow]Please provide correct ID to delete[/] or Q to Quit:\n");
                if (Validator.CheckForExit(idToDelete)) return;
                studySessionDTO.SessionId = idToDelete;
            }
            if (!_databaseController.StudySessionController.DeleteSession(studySessionDTO))
            {
                AnsiConsole.Write("[red]Apologies, an error has occurred while deleting.\n");
            }
        }
    }
}
