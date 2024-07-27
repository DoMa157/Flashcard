using FlashCards.Controllers;
using FlashCards.View;
using System.Configuration;

namespace FlashCards
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.AppSettings.Get("Dbstring");
            var DBController = new DatabaseController(connectionString);
            var consoleView = new ConsoleView(DBController);
            var stackView = new StackView(DBController);
            var flashcardView = new FlashcardView(DBController);
            var studySessionView = new StudySessionView(DBController, stackView);
            Run(consoleView, stackView, flashcardView, studySessionView);
        }
        internal static void Run(ConsoleView consoleView, StackView stackView, FlashcardView flashcardView, StudySessionView studySessionView)
        {
            var closeApp = false;
            while (!closeApp)
            {
                switch (consoleView.PromptMainMenu())
                {
                    case "MODIFY STACKS":
                        switch (stackView.PromptStacks())
                        {
                            case "ADD STACK":
                                stackView.AddStack();
                                break;
                            case "DELETE STACK":
                                stackView.DeleteStack();
                                break;
                            case "MODIFY STACK":
                                stackView.ModifyStack();
                                break;
                            case "EXIT":
                                break;
                        }
                        break;
                    case "MODIFY FLASHCARDS":
                        switch (flashcardView.PromptFlashcards())
                        {
                            case "ADD FLASHCARD":
                                flashcardView.AddFlashcard();
                                break;
                            case "DELETE FLASHCARD":
                                flashcardView.DeleteFlashcard();
                                break;
                            case "MODIFY FLASHCARD":
                                flashcardView.ModifyFlashcard();
                                break;
                            case "EXIT":
                                break;
                        }
                        break;
                    case "STUDY":
                        studySessionView.Study();
                        break;
                    case "DELETE STUDY SESSION":
                        studySessionView.DeleteSession();
                        break;
                    case "EXIT":
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}