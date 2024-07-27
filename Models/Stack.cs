using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.Models
{
    internal class FlashcardStack
    {
        public int StackId {  get; set; }
        public string Name { get; set; }
        public List<Flashcard> Flashcards { get; set; }

        public void AddFlashcard(Flashcard flashcard)
        {
            Flashcards.Add(flashcard);
        }

        public void DeleteFlashcard(Flashcard flashcard) 
        {
            Flashcards.Remove(flashcard);
        }
    }
}
