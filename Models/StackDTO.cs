using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.Models
{
    internal class FlashcardStackDTO
    {
        public int StackId { get; set; }
        public string StackName { get; set; }
        public List<FlashcardDTO> Flashcards { get; set; }
    }
}
