using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace PairGame
{
    public class FormMain : Form
    {
        // Элементы управления
        private MenuStrip menuStrip1;
        private TableLayoutPanel tableLayoutPanel1;
        private Label labelTimer;
        private Label labelMoves;
        private Label labelStatus;
        private Timer gameTimer;

        // Логика игры
        private GameLogic game;
        private int timeLeft;
        private string currentLogin;
        private string currentTheme = "Animals";
        private List<UserResult> allResults;

        // Массив с нарисованными картинками (8 уникальных)
        private Image[] cardImages;
        private int cardSize = 80;

        public FormMain()
        {
            InitializeForm();
            LoadResultsFile();
            ShowAuth();
            CreateCardImages();
            InitGame();
        }

        #region Инициализация формы и элементов управления

        private void InitializeForm()
        {
            this.Text = "Игра «Парные картинки» — найди пары животных";
            this.Size = new Size(700, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.LightGray;

            // MenuStrip
            menuStrip1 = new MenuStrip();

            ToolStripMenuItem gameMenu = new ToolStripMenuItem("Игра");
            ToolStripMenuItem newGameItem = new ToolStripMenuItem("Новая игра");
            newGameItem.Click += (s, e) => NewGame();
            ToolStripMenuItem exitItem = new ToolStripMenuItem("Выход");
            exitItem.Click += (s, e) => Application.Exit();
            gameMenu.DropDownItems.Add(newGameItem);
            gameMenu.DropDownItems.Add(new ToolStripSeparator());
            gameMenu.DropDownItems.Add(exitItem);

            ToolStripMenuItem settingsMenu = new ToolStripMenuItem("Настройки");
            settingsMenu.Click += (s, e) => OpenSettings();

            ToolStripMenuItem resultsMenu = new ToolStripMenuItem("Результаты");
            resultsMenu.Click += (s, e) => ShowResults();

            ToolStripMenuItem helpMenu = new ToolStripMenuItem("Справка");
            ToolStripMenuItem aboutItem = new ToolStripMenuItem("О программе");
            aboutItem.Click += (s, e) => ShowAbout();
            helpMenu.DropDownItems.Add(aboutItem);

            menuStrip1.Items.Add(gameMenu);
            menuStrip1.Items.Add(settingsMenu);
            menuStrip1.Items.Add(resultsMenu);
            menuStrip1.Items.Add(helpMenu);

            // Игровое поле
            Panel gamePanel = new Panel();
            gamePanel.Location = new Point(10, 60);
            gamePanel.Size = new Size(420, 420);
            gamePanel.BackColor = Color.White;
            gamePanel.BorderStyle = BorderStyle.FixedSingle;

            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel1.Dock = DockStyle.Fill;
            gamePanel.Controls.Add(tableLayoutPanel1);

            // Панель информации
            Panel infoPanel = new Panel();
            infoPanel.Location = new Point(440, 60);
            infoPanel.Size = new Size(230, 420);
            infoPanel.BackColor = Color.Beige;
            infoPanel.BorderStyle = BorderStyle.FixedSingle;

            labelTimer = new Label();
            labelTimer.Text = "Время: 60";
            labelTimer.Font = new Font("Arial", 14, FontStyle.Bold);
            labelTimer.Location = new Point(15, 30);
            labelTimer.Size = new Size(200, 40);
            labelTimer.TextAlign = ContentAlignment.MiddleCenter;

            labelMoves = new Label();
            labelMoves.Text = "Ходы: 0";
            labelMoves.Font = new Font("Arial", 14, FontStyle.Bold);
            labelMoves.Location = new Point(15, 100);
            labelMoves.Size = new Size(200, 40);
            labelMoves.TextAlign = ContentAlignment.MiddleCenter;

            labelStatus = new Label();
            labelStatus.Text = "Готов к игре!";
            labelStatus.Font = new Font("Arial", 10);
            labelStatus.Location = new Point(15, 180);
            labelStatus.Size = new Size(200, 80);
            labelStatus.TextAlign = ContentAlignment.MiddleCenter;

            infoPanel.Controls.Add(labelTimer);
            infoPanel.Controls.Add(labelMoves);
            infoPanel.Controls.Add(labelStatus);

            gameTimer = new Timer();
            gameTimer.Interval = 1000;
            gameTimer.Tick += Timer_Tick;

            this.Controls.Add(menuStrip1);
            this.Controls.Add(gamePanel);
            this.Controls.Add(infoPanel);
        }

        #endregion

        #region Работа с файлом результатов

        private void LoadResultsFile()
        {
            string file = "results.dat";
            if (File.Exists(file))
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (FileStream fs = new FileStream(file, FileMode.Open))
                    allResults = (List<UserResult>)bf.Deserialize(fs);
            }
            else
                allResults = new List<UserResult>();
        }

        private void SaveResultsToFile()
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream("results.dat", FileMode.Create))
                bf.Serialize(fs, allResults);
        }

        #endregion

        #region Авторизация

        private void ShowAuth()
        {
            FormAuth auth = new FormAuth();
            if (auth.ShowDialog() == DialogResult.OK)
                currentLogin = auth.Login;
            else
                Application.Exit();

            this.Text = $"Игра «Парные картинки» — игрок: {currentLogin}";
        }

        #endregion

        #region Создание картинок программно

        private void CreateCardImages()
        {
            cardImages = new Image[8];

            for (int i = 0; i < 8; i++)
            {
                cardImages[i] = DrawCardImage(i);
            }

            labelStatus.Text = "Картинки готовы! Начинайте игру.";
        }

        private Image DrawCardImage(int cardType)
        {
            Bitmap bmp = new Bitmap(cardSize, cardSize);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                g.Clear(Color.White);

                using (Pen borderPen = new Pen(Color.Black, 3))
                {
                    g.DrawRectangle(borderPen, 2, 2, cardSize - 5, cardSize - 5);
                }

                using (Pen innerPen = new Pen(Color.LightGray, 1))
                {
                    g.DrawRectangle(innerPen, 5, 5, cardSize - 11, cardSize - 11);
                }

                int centerX = cardSize / 2;
                int centerY = cardSize / 2;
                int size = cardSize - 30;

                switch (cardType)
                {
                    case 0: DrawDog(g, centerX, centerY, size); break;
                    case 1: DrawCat(g, centerX, centerY, size); break;
                    case 2: DrawMouse(g, centerX, centerY, size); break;
                    case 3: DrawRabbit(g, centerX, centerY, size); break;
                    case 4: DrawFox(g, centerX, centerY, size); break;
                    case 5: DrawBear(g, centerX, centerY, size); break;
                    case 6: DrawPenguin(g, centerX, centerY, size); break;
                    case 7: DrawLion(g, centerX, centerY, size); break;
                }
            }
            return bmp;
        }

        #region Рисование животных

        private void DrawDog(Graphics g, int x, int y, int size)
        {
            int r = size / 2;
            g.FillEllipse(Brushes.SandyBrown, x - r, y - r, size, size);
            g.FillEllipse(Brushes.SaddleBrown, x - r - 10, y - r - 5, 20, 25);
            g.FillEllipse(Brushes.SaddleBrown, x + r - 10, y - r - 5, 20, 25);
            g.FillEllipse(Brushes.Black, x - 12, y - 8, 8, 8);
            g.FillEllipse(Brushes.Black, x + 4, y - 8, 8, 8);
            g.FillEllipse(Brushes.Black, x - 5, y + 2, 10, 8);
            g.DrawArc(Pens.Black, x - 8, y + 5, 16, 12, 0, 180);
        }

        private void DrawCat(Graphics g, int x, int y, int size)
        {
            int r = size / 2;
            g.FillEllipse(Brushes.Gray, x - r, y - r, size, size);
            Point[] leftEar = { new Point(x - r - 5, y - r + 10), new Point(x - r + 15, y - r - 5), new Point(x - r + 15, y - r + 20) };
            Point[] rightEar = { new Point(x + r + 5, y - r + 10), new Point(x + r - 15, y - r - 5), new Point(x + r - 15, y - r + 20) };
            g.FillPolygon(Brushes.Gray, leftEar);
            g.FillPolygon(Brushes.Gray, rightEar);
            g.FillEllipse(Brushes.YellowGreen, x - 14, y - 8, 10, 10);
            g.FillEllipse(Brushes.YellowGreen, x + 4, y - 8, 10, 10);
            g.FillEllipse(Brushes.Black, x - 12, y - 6, 6, 6);
            g.FillEllipse(Brushes.Black, x + 6, y - 6, 6, 6);
            g.FillEllipse(Brushes.Pink, x - 4, y + 2, 8, 6);
            g.DrawLine(Pens.Black, x - 20, y + 5, x - 8, y + 5);
            g.DrawLine(Pens.Black, x + 8, y + 5, x + 20, y + 5);
        }

        private void DrawMouse(Graphics g, int x, int y, int size)
        {
            int r = size / 2;
            g.FillEllipse(Brushes.LightGray, x - r, y - r, size, size);
            g.FillEllipse(Brushes.LightGray, x - r - 8, y - r - 5, 18, 18);
            g.FillEllipse(Brushes.LightGray, x + r - 10, y - r - 5, 18, 18);
            g.FillEllipse(Brushes.Pink, x - r - 4, y - r - 1, 10, 10);
            g.FillEllipse(Brushes.Pink, x + r - 6, y - r - 1, 10, 10);
            g.FillEllipse(Brushes.Black, x - 6, y - 4, 5, 5);
            g.FillEllipse(Brushes.Black, x + 1, y - 4, 5, 5);
            g.FillEllipse(Brushes.Pink, x - 3, y + 2, 6, 5);
        }

        private void DrawRabbit(Graphics g, int x, int y, int size)
        {
            int r = size / 2;
            g.FillEllipse(Brushes.White, x - r, y - r, size, size);
            g.FillEllipse(Brushes.White, x - 15, y - r - 20, 12, 30);
            g.FillEllipse(Brushes.White, x + 3, y - r - 20, 12, 30);
            g.FillEllipse(Brushes.Pink, x - 13, y - r - 18, 8, 20);
            g.FillEllipse(Brushes.Pink, x + 5, y - r - 18, 8, 20);
            g.FillEllipse(Brushes.Black, x - 8, y - 4, 6, 6);
            g.FillEllipse(Brushes.Black, x + 2, y - 4, 6, 6);
            g.FillEllipse(Brushes.Pink, x - 4, y + 2, 8, 6);
        }

        private void DrawFox(Graphics g, int x, int y, int size)
        {
            int r = size / 2;
            g.FillEllipse(Brushes.Orange, x - r, y - r, size, size);
            Point[] leftEar = { new Point(x - r - 5, y - r + 5), new Point(x - r + 12, y - r - 10), new Point(x - r + 15, y - r + 15) };
            Point[] rightEar = { new Point(x + r + 5, y - r + 5), new Point(x + r - 12, y - r - 10), new Point(x + r - 15, y - r + 15) };
            g.FillPolygon(Brushes.Orange, leftEar);
            g.FillPolygon(Brushes.Orange, rightEar);
            g.FillEllipse(Brushes.White, x - 18, y, 15, 12);
            g.FillEllipse(Brushes.White, x + 3, y, 15, 12);
            g.FillEllipse(Brushes.Black, x - 10, y - 4, 5, 5);
            g.FillEllipse(Brushes.Black, x + 5, y - 4, 5, 5);
            g.FillEllipse(Brushes.Black, x - 3, y + 2, 6, 5);
        }

        private void DrawBear(Graphics g, int x, int y, int size)
        {
            int r = size / 2;
            g.FillEllipse(Brushes.SaddleBrown, x - r, y - r, size, size);
            g.FillEllipse(Brushes.SaddleBrown, x - r - 5, y - r - 5, 15, 15);
            g.FillEllipse(Brushes.SaddleBrown, x + r - 10, y - r - 5, 15, 15);
            g.FillEllipse(Brushes.Black, x - 10, y - 4, 6, 6);
            g.FillEllipse(Brushes.Black, x + 4, y - 4, 6, 6);
            g.FillEllipse(Brushes.Black, x - 5, y + 2, 10, 8);
        }

        private void DrawPenguin(Graphics g, int x, int y, int size)
        {
            int r = size / 2;
            g.FillEllipse(Brushes.Black, x - r, y - r, size, size);
            g.FillEllipse(Brushes.White, x - r + 5, y - r + 5, size - 10, size - 10);
            g.FillEllipse(Brushes.White, x - 10, y - 6, 8, 8);
            g.FillEllipse(Brushes.White, x + 2, y - 6, 8, 8);
            g.FillEllipse(Brushes.Black, x - 8, y - 5, 4, 4);
            g.FillEllipse(Brushes.Black, x + 4, y - 5, 4, 4);
            Point[] beak = { new Point(x - 3, y + 2), new Point(x + 3, y + 2), new Point(x, y + 10) };
            g.FillPolygon(Brushes.Orange, beak);
        }

        private void DrawLion(Graphics g, int x, int y, int size)
        {
            int r = size / 2;
            g.FillEllipse(Brushes.Goldenrod, x - r - 8, y - r - 8, size + 16, size + 16);
            g.FillEllipse(Brushes.Gold, x - r, y - r, size, size);
            g.FillEllipse(Brushes.Gold, x - r - 3, y - r - 3, 12, 12);
            g.FillEllipse(Brushes.Gold, x + r - 9, y - r - 3, 12, 12);
            g.FillEllipse(Brushes.Black, x - 10, y - 4, 6, 6);
            g.FillEllipse(Brushes.Black, x + 4, y - 4, 6, 6);
            g.FillEllipse(Brushes.Brown, x - 4, y + 2, 8, 6);
        }

        #endregion

        #endregion

        #region Инициализация игры

        private void InitGame()
        {
            game = new GameLogic(4, 4);
            game.OnCardOpen += Game_OnCardOpen;
            game.OnMatch += Game_OnMatch;
            game.OnMismatch += Game_OnMismatch;
            game.OnGameFinished += Game_OnGameFinished;

            game.InitializeCards();
            CreateBoard();
            StartGameTimer();
            UpdateMoveLabel();
        }

        private void CreateBoard()
        {
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowCount = game.Rows;
            tableLayoutPanel1.ColumnCount = game.Cols;
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.ColumnStyles.Clear();

            for (int i = 0; i < game.Rows; i++)
            {
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / game.Rows));
            }
            for (int j = 0; j < game.Cols; j++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / game.Cols));
            }

            for (int i = 0; i < game.Rows; i++)
            {
                for (int j = 0; j < game.Cols; j++)
                {
                    Button btn = new Button();
                    btn.Dock = DockStyle.Fill;
                    btn.BackgroundImageLayout = ImageLayout.Stretch;
                    btn.Tag = new Tuple<int, int>(i, j);
                    btn.Click += CardButton_Click;
                    btn.Text = "?";
                    btn.Font = new Font("Arial", 20, FontStyle.Bold);

                    tableLayoutPanel1.Controls.Add(btn, j, i);
                    game.Buttons[i, j] = btn;
                }
            }
        }

        private void SetButtonBackImage(Button btn, Image img)
        {
            if (btn.InvokeRequired)
            {
                btn.Invoke(new Action(() => SetButtonBackImage(btn, img)));
                return;
            }

            if (img != null)
            {
                btn.BackgroundImage = img;
                btn.Text = "";
            }
            else
            {
                btn.BackgroundImage = null;
                btn.Text = "?";
            }
        }

        private void RefreshBoardImages()
        {
            if (game == null) return;
            for (int i = 0; i < game.Rows; i++)
            {
                for (int j = 0; j < game.Cols; j++)
                {
                    if (!game.CardMatched[i, j])
                    {
                        SetButtonBackImage(game.Buttons[i, j], null);
                    }
                }
            }
        }

        #endregion

        #region Обработчики игры

        private void CardButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            var pos = (Tuple<int, int>)btn.Tag;
            game.CardClick(pos.Item1, pos.Item2);
            UpdateMoveLabel();
        }

        private void Game_OnCardOpen(int row, int col)
        {
            int cardType = game.CardValues[row, col];
            if (cardImages != null && cardType < cardImages.Length && cardImages[cardType] != null)
            {
                SetButtonBackImage(game.Buttons[row, col], cardImages[cardType]);
            }
            else
            {
                game.Buttons[row, col].Text = cardType.ToString();
                game.Buttons[row, col].BackgroundImage = null;
            }
            game.Buttons[row, col].Enabled = !game.CardMatched[row, col];
        }

        private void Game_OnMatch()
        {
            labelStatus.Text = "✅ Пара найдена! ✅";
            UpdateMoveLabel();

            if (game.PairsFound == 8)
                EndGame(true);
        }

        private void Game_OnMismatch(int row1, int col1, int row2, int col2)
        {
            labelStatus.Text = "❌ Не совпало! ❌";

            Timer timer = new Timer();
            timer.Interval = 700;
            timer.Tick += (s, e) =>
            {
                if (!game.CardMatched[row1, col1])
                    SetButtonBackImage(game.Buttons[row1, col1], null);
                if (!game.CardMatched[row2, col2])
                    SetButtonBackImage(game.Buttons[row2, col2], null);
                timer.Stop();
            };
            timer.Start();
        }

        private void Game_OnGameFinished()
        {
            EndGame(true);
        }

        private void UpdateMoveLabel()
        {
            labelMoves.Text = $"Ходы: {game.Moves}";
        }

        #endregion

        #region Таймер и окончание игры

        private void StartGameTimer()
        {
            timeLeft = 60;
            labelTimer.Text = $"Время: {timeLeft}";
            gameTimer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                timeLeft--;
                labelTimer.Text = $"Время: {timeLeft}";
            }
            else
            {
                gameTimer.Stop();
                EndGame(false);
            }
        }

        private void EndGame(bool won)
        {
            gameTimer.Stop();

            if (won && game.PairsFound == 8)
            {
                labelStatus.Text = "🏆 ПОБЕДА! 🏆";
                MessageBox.Show($"Поздравляем, {currentLogin}!\nВы нашли все пары за {game.Moves} ходов!",
                    "Победа", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                labelStatus.Text = "⏰ Время вышло! ⏰";
                MessageBox.Show($"Время вышло, {currentLogin}!\nНайдено пар: {game.PairsFound} из 8",
                    "Поражение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            SaveResult();

            for (int i = 0; i < game.Rows; i++)
                for (int j = 0; j < game.Cols; j++)
                    game.Buttons[i, j].Enabled = false;
        }

        private void SaveResult()
        {
            UserResult res = new UserResult
            {
                Login = currentLogin,
                PairsFound = game.PairsFound,
                TotalMoves = game.Moves,
                TimeLeft = timeLeft,
                Date = DateTime.Now
            };
            allResults.Add(res);
            SaveResultsToFile();
        }

        #endregion

        #region Обработчики меню

        private void NewGame()
        {
            if (MessageBox.Show("Начать новую игру? Текущий прогресс будет потерян.",
                "Новая игра", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                gameTimer.Stop();
                InitGame();
            }
        }

        private void OpenSettings()
        {
            FormSettings settings = new FormSettings(60, currentTheme);
            if (settings.ShowDialog() == DialogResult.OK)
            {
                timeLeft = settings.GameTime;
                currentTheme = settings.Theme;
                labelStatus.Text = "Настройки применены. Начните новую игру.";
            }
        }

        private void ShowResults()
        {
            new FormResults(currentLogin).ShowDialog();
        }

        private void ShowAbout()
        {
            MessageBox.Show(
                "Игра «Парные картинки»\nВерсия 2.0\n\n" +
                "Правила: найдите все пары одинаковых картинок.\n" +
                "Картинки рисуются программно с помощью Graphics.\n\n" +
                "© Лабораторная работа по разработке игр",
                "О программе",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        #endregion
    }
}