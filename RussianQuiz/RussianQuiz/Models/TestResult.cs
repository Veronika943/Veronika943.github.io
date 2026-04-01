using System;

namespace RussianQuiz.Models
{
    public class TestResult
    {
        public string TopicName { get; set; }
        public int LevelId { get; set; }
        public string LevelName { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
        public int Score { get; set; }
        public int TimeSpent { get; set; }
        public DateTime TestDate { get; set; }

        public bool CanAdvance => Score >= 80;
    }
}