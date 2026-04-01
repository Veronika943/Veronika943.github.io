using System.Collections.Generic;

namespace RussianQuiz.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Level> Levels { get; set; } = new List<Level>();
    }

    public class Level
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}