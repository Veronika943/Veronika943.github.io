using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using RussianTraditionsQuiz.Classes;

namespace RussianTraditionsQuiz
{
    public partial class QuizForm : Form
    {
        private System.Collections.Generic.List<Question> allQuestions;
        private System.Collections.Generic.List<Question> currentQuestions;
        private int currentIndex = 0;
        private int score = 0;
        private int questionsPerSession = 5;
        private Timer timer;
        private int timeLeft = 60;
        private string currentTopic;
        private int currentLevel;

        // Элементы управления
        private Panel panelTop;
        private Label lblScore;
        private Label lblTimer;
        private Label lblProgress;
        private PictureBox pictureBox;
        private Label lblQuestion;
        private GroupBox groupAnswers;
        private RadioButton rbAnswer1;
        private RadioButton rbAnswer2;
        private RadioButton rbAnswer3;
        private RadioButton rbAnswer4;
        private Button btnSubmit;
        private Button btnExitMenu;

        public QuizForm(string topic, int level)
        {
            currentTopic = topic;
            currentLevel = level;
            InitializeComponent();
            LoadQuestions();
            StartTimer();
        }

        private void InitializeComponent()
        {
            // Настройка формы
            this.Text = "Викторина";
            this.Size = new Size(900, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            // Верхняя панель
            panelTop = new Panel();
            panelTop.Dock = DockStyle.Top;
            panelTop.Height = 80;
            panelTop.BackColor = Color.FromArgb(70, 130, 180);

            // Счет баллов
            lblScore = new Label();
            lblScore.Text = "Баллы: 0";
            lblScore.Font = new Font("Arial", 14, FontStyle.Bold);
            lblScore.ForeColor = Color.White;
            lblScore.Location = new Point(20, 25);
            lblScore.AutoSize = true;
            lblScore.BackColor = Color.Transparent;

            // Таймер
            lblTimer = new Label();
            lblTimer.Text = "Осталось: 60 сек";
            lblTimer.Font = new Font("Arial", 14, FontStyle.Bold);
            lblTimer.ForeColor = Color.White;
            lblTimer.Location = new Point(730, 25);
            lblTimer.AutoSize = true;
            lblTimer.BackColor = Color.Transparent;

            // Прогресс
            lblProgress = new Label();
            lblProgress.Text = "Вопрос 1 из 5";
            lblProgress.Font = new Font("Arial", 12, FontStyle.Bold);
            lblProgress.ForeColor = Color.White;
            lblProgress.Location = new Point(380, 30);
            lblProgress.AutoSize = true;
            lblProgress.BackColor = Color.Transparent;

            panelTop.Controls.Add(lblScore);
            panelTop.Controls.Add(lblTimer);
            panelTop.Controls.Add(lblProgress);

            // Изображение
            pictureBox = new PictureBox();
            pictureBox.Location = new Point(50, 120);
            pictureBox.Size = new Size(200, 200);
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.Visible = false;
            pictureBox.BackColor = Color.LightGray;

            // Текст вопроса
            lblQuestion = new Label();
            lblQuestion.Location = new Point(300, 120);
            lblQuestion.Size = new Size(550, 100);
            lblQuestion.Font = new Font("Arial", 12, FontStyle.Regular);
            lblQuestion.BackColor = Color.White;
            lblQuestion.AutoSize = false;

            // Группа для ответов
            groupAnswers = new GroupBox();
            groupAnswers.Text = "Выберите ответ:";
            groupAnswers.Font = new Font("Arial", 12, FontStyle.Bold);
            groupAnswers.Location = new Point(50, 350);
            groupAnswers.Size = new Size(800, 200);
            groupAnswers.BackColor = Color.White;

            // Варианты ответов
            rbAnswer1 = new RadioButton();
            rbAnswer1.Location = new Point(20, 40);
            rbAnswer1.Size = new Size(750, 30);
            rbAnswer1.Font = new Font("Arial", 10);
            rbAnswer1.AutoSize = true;

            rbAnswer2 = new RadioButton();
            rbAnswer2.Location = new Point(20, 80);
            rbAnswer2.Size = new Size(750, 30);
            rbAnswer2.Font = new Font("Arial", 10);
            rbAnswer2.AutoSize = true;

            rbAnswer3 = new RadioButton();
            rbAnswer3.Location = new Point(20, 120);
            rbAnswer3.Size = new Size(750, 30);
            rbAnswer3.Font = new Font("Arial", 10);
            rbAnswer3.AutoSize = true;

            rbAnswer4 = new RadioButton();
            rbAnswer4.Location = new Point(20, 160);
            rbAnswer4.Size = new Size(750, 30);
            rbAnswer4.Font = new Font("Arial", 10);
            rbAnswer4.AutoSize = true;

            groupAnswers.Controls.Add(rbAnswer1);
            groupAnswers.Controls.Add(rbAnswer2);
            groupAnswers.Controls.Add(rbAnswer3);
            groupAnswers.Controls.Add(rbAnswer4);

            // Кнопка "Ответить"
            btnSubmit = new Button();
            btnSubmit.Text = "Ответить";
            btnSubmit.Font = new Font("Arial", 12, FontStyle.Bold);
            btnSubmit.Location = new Point(350, 580);
            btnSubmit.Size = new Size(150, 50);
            btnSubmit.BackColor = Color.LightGreen;
            btnSubmit.FlatStyle = FlatStyle.Flat;
            btnSubmit.Cursor = Cursors.Hand;
            btnSubmit.Click += new EventHandler(btnSubmit_Click);

            // Кнопка "Выход в меню"
            btnExitMenu = new Button();
            btnExitMenu.Text = "Выход в меню";
            btnExitMenu.Font = new Font("Arial", 10, FontStyle.Bold);
            btnExitMenu.Location = new Point(550, 580);
            btnExitMenu.Size = new Size(150, 50);
            btnExitMenu.BackColor = Color.LightCoral;
            btnExitMenu.FlatStyle = FlatStyle.Flat;
            btnExitMenu.Cursor = Cursors.Hand;
            btnExitMenu.Click += new EventHandler(btnExitMenu_Click);

            // Добавление элементов на форму
            this.Controls.Add(panelTop);
            this.Controls.Add(pictureBox);
            this.Controls.Add(lblQuestion);
            this.Controls.Add(groupAnswers);
            this.Controls.Add(btnSubmit);
            this.Controls.Add(btnExitMenu);
        }

        private void LoadQuestions()
        {
            allQuestions = XMLHelper.GetQuestions(currentTopic, currentLevel);

            if (allQuestions.Count == 0)
            {
                MessageBox.Show("Вопросы не найдены!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            Random rnd = new Random();
            currentQuestions = allQuestions
                .OrderBy(x => rnd.Next())
                .Take(questionsPerSession)
                .ToList();

            lblProgress.Text = $"Вопрос 1 из {currentQuestions.Count}";
            ShowQuestion(0);
        }

        private void ShowQuestion(int index)
        {
            if (index < currentQuestions.Count)
            {
                Question q = currentQuestions[index];

                lblQuestion.Text = q.Text;

                ClearAnswers();
                for (int i = 0; i < q.Answers.Count; i++)
                {
                    RadioButton rb = GetAnswerRadioButton(i);
                    if (rb != null)
                    {
                        rb.Text = q.Answers[i];
                        rb.Visible = true;
                        rb.Checked = false;
                    }
                }

                for (int i = q.Answers.Count; i < 4; i++)
                {
                    RadioButton rb = GetAnswerRadioButton(i);
                    if (rb != null) rb.Visible = false;
                }

                if (!string.IsNullOrEmpty(q.ImagePath) && File.Exists(q.ImagePath))
                {
                    try
                    {
                        pictureBox.Image = Image.FromFile(q.ImagePath);
                        pictureBox.Visible = true;
                    }
                    catch
                    {
                        pictureBox.Visible = false;
                    }
                }
                else
                {
                    pictureBox.Visible = false;
                }

                lblProgress.Text = $"Вопрос {index + 1} из {currentQuestions.Count}";
            }
        }

        private RadioButton GetAnswerRadioButton(int index)
        {
            switch (index)
            {
                case 0: return rbAnswer1;
                case 1: return rbAnswer2;
                case 2: return rbAnswer3;
                case 3: return rbAnswer4;
                default: return null;
            }
        }

        private void ClearAnswers()
        {
            rbAnswer1.Visible = false;
            rbAnswer2.Visible = false;
            rbAnswer3.Visible = false;
            rbAnswer4.Visible = false;
        }

        private void CheckAnswer()
        {
            int selectedIndex = -1;
            for (int i = 0; i < 4; i++)
            {
                RadioButton rb = GetAnswerRadioButton(i);
                if (rb != null && rb.Visible && rb.Checked)
                {
                    selectedIndex = i;
                    break;
                }
            }

            if (selectedIndex == -1)
            {
                MessageBox.Show("Пожалуйста, выберите вариант ответа!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Question currentQ = currentQuestions[currentIndex];
            if (currentQ.IsCorrect(selectedIndex))
            {
                score += 20;
                MessageBox.Show("Правильно! +20 баллов", "Отлично!",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string correctAnswer = currentQ.GetCorrectAnswer();
                MessageBox.Show($"Неправильно!\nПравильный ответ: {correctAnswer}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            lblScore.Text = $"Баллы: {score}";
            currentIndex++;

            if (currentIndex < currentQuestions.Count)
            {
                ShowQuestion(currentIndex);
            }
            else
            {
                EndQuiz();
            }
        }

        private void EndQuiz()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }

            string message = $"Викторина завершена!\n\nВаш результат: {score} баллов из 100\n";

            if (score >= 80)
            {
                message += "\nПоздравляем! Вы набрали достаточно баллов для перехода на следующий уровень!";

                if (currentLevel < 3)
                {
                    message += $"\n\nОткрыт уровень {currentLevel + 1} в теме '{currentTopic}'.";
                    MessageBox.Show(message, "Поздравляем!",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    message += "\n\nВы прошли все уровни в этой теме! Поздравляем с завершением!";
                    MessageBox.Show(message, "Абсолютная победа!",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                message += "\n\nК сожалению, вы не набрали достаточно баллов для перехода на следующий уровень.\n" +
                          "Попробуйте еще раз!";
                MessageBox.Show(message, "Результат",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.Close();
        }

        private void StartTimer()
        {
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            lblTimer.Text = $"Осталось: {timeLeft} сек";

            if (timeLeft <= 0)
            {
                timer.Stop();
                MessageBox.Show("Время вышло! Викторина завершается.", "Время истекло",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                EndQuiz();
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            CheckAnswer();
        }

        private void btnExitMenu_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите выйти в главное меню?\n" +
                "Прогресс будет потерян!", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (timer != null)
                {
                    timer.Stop();
                    timer.Dispose();
                }
                this.Close();
            }
        }

        // Удаляем переопределение Dispose, если оно есть
    }
}