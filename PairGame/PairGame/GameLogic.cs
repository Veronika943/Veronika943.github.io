using System;
using System.Windows.Forms;

namespace PairGame
{
    public class GameLogic
    {
        public int Rows { get; private set; }
        public int Cols { get; private set; }
        public Button[,] Buttons { get; private set; }
        public int[,] CardValues { get; private set; }
        public bool[,] CardMatched { get; private set; }
        public bool IsWaiting { get; private set; }
        public int FirstIndexRow, FirstIndexCol;
        public int Moves { get; private set; }
        public int PairsFound { get; private set; }

        public event Action<int, int> OnCardOpen;
        public event Action OnMatch;
        public event Action<int, int, int, int> OnMismatch;
        public event Action OnGameFinished;

        public GameLogic(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            Buttons = new Button[rows, cols];
            CardValues = new int[rows, cols];
            CardMatched = new bool[rows, cols];
            IsWaiting = false;
            FirstIndexRow = -1;
            Moves = 0;
            PairsFound = 0;
        }

        public void InitializeCards()
        {
            int totalCards = Rows * Cols;
            int[] values = new int[totalCards];
            for (int i = 0; i < totalCards; i++)
                values[i] = i / 2;

            Random rnd = new Random();
            for (int i = totalCards - 1; i > 0; i--)
            {
                int j = rnd.Next(i + 1);
                int temp = values[i];
                values[i] = values[j];
                values[j] = temp;
            }

            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                {
                    CardValues[i, j] = values[i * Cols + j];
                    CardMatched[i, j] = false;
                }

            PairsFound = 0;
            Moves = 0;
        }

        public void CardClick(int row, int col)
        {
            if (IsWaiting) return;
            if (CardMatched[row, col]) return;

            if (FirstIndexRow == -1)
            {
                FirstIndexRow = row;
                FirstIndexCol = col;
                OnCardOpen?.Invoke(row, col);
            }
            else
            {
                int firstVal = CardValues[FirstIndexRow, FirstIndexCol];
                int secondVal = CardValues[row, col];

                Moves++;

                if (firstVal == secondVal && (FirstIndexRow != row || FirstIndexCol != col))
                {
                    CardMatched[FirstIndexRow, FirstIndexCol] = true;
                    CardMatched[row, col] = true;
                    PairsFound++;
                    OnMatch?.Invoke();
                    FirstIndexRow = -1;

                    if (PairsFound == (Rows * Cols) / 2)
                        OnGameFinished?.Invoke();
                }
                else
                {
                    IsWaiting = true;
                    OnMismatch?.Invoke(FirstIndexRow, FirstIndexCol, row, col);

                    Timer timer = new Timer();
                    timer.Interval = 700;
                    timer.Tick += (s, e) =>
                    {
                        IsWaiting = false;
                        FirstIndexRow = -1;
                        timer.Stop();
                    };
                    timer.Start();
                }
            }
        }
    }
}