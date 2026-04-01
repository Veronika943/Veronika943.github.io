using System.Collections.Generic;

namespace RussianQuiz.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string ImagePath { get; set; }
        public List<Answer> Answers { get; set; } = new List<Answer>();

        public Question()
        {
            Answers = new List<Answer>();
        }
    }

    public class Answer
    {
        public string Text { get; set; }
        public bool IsRight { get; set; }
    }
}