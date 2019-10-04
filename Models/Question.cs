using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFNMUSiteCore.Models
{
    [Serializable]
    public class Question
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public List<Answer> Answers { get; set; }

        public Question()
        {
            Answers = new List<Answer>();
        }
    }
    [Serializable]
    public class Answer
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public byte TrueAnswer { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
