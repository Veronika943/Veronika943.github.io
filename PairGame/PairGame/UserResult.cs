using System;

namespace PairGame
{
    [Serializable]
    public class UserResult
    {
        public string Login { get; set; }
        public int PairsFound { get; set; }
        public int TotalMoves { get; set; }
        public int TimeLeft { get; set; }
        public DateTime Date { get; set; }
    }
}