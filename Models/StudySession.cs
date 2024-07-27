using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.Models
{
    internal class StudySession
    {
        public int SessionId { get; set; }
        public int StackId { get; set; }

        public string StackName { get; set; }

        public TimeSpan Duration { get; set; }
        public int Score { get; set; }
    }
}
