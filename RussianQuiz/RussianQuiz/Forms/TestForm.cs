using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using RussianQuiz.Models;

namespace RussianQuiz.Forms
{
    public class TestForm : Form
    {
        private Topic currentTopic;
        private Level currentLevel;
        private List<Question> currentQuestions;
        private int currentQuestionIndex = 0;
        private int correctAnswers = 0;
        private int totalQuestionsInSession = 5;
        private int currentScore = 0;
        private int timeRemaining = 60;
        private Timer timer;

        private Panel mainPanel;
        private Label lblTopicName;
        private Label lblLevelName;
        private Label lblQuestionNumber;
        private Label lblQuestionText;
        private PictureBox pictureBox;
        private Panel answersPanel;
        private Button btnNext;
        private Button btnFinish;
        private Label lblTime;
        private Label lblScore;
        private ProgressBar progressBar;

        private List<RadioButton> answerButtons = new List<RadioButton>();

        public TestForm(Topic topic)
        {
            currentTopic = topic;
            currentLevel = topic.Levels[0];
            InitializeComponent();
            LoadQuestionsForLevel();
        }

        private void InitializeComponent()
        {
            this.Text = $"Тестирование: {currentTopic.Name}";
            this.Size = new Size(700, 550);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosing += TestForm_FormClosing;

            mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Padding = new Padding(20);
            mainPanel.BackColor = Color.Beige;

            lblTopicName = new Label();
            lblTopicName.Text = $"Тема: {currentTopic.Name}";
            lblTopicName.Font = new Font("Arial", 12, FontStyle.Bold);
            lblTopicName.Dock = DockStyle.Top;
            lblTopicName.Height = 30;

            lblLevelName = new Label();
            lblLevelName.Text = $"Уровень: {currentLevel.Name}";
            lblLevelName.Font = new Font("Arial", 10);
            lblLevelName.Dock = DockStyle.Top;
            lblLevelName.Height = 25;

            Panel infoPanel = new Panel();
            infoPanel.Dock = DockStyle.Top;
            infoPanel.Height = 40;

            lblTime = new Label();
            lblTime.Text = $"Время: {timeRemaining} сек";
            lblTime.Font = new Font("Arial", 12, FontStyle.Bold);
            lblTime.ForeColor = Color.DarkRed;
            lblTime.Location = new Point(10, 5);
            lblTime.AutoSize = true;

            lblScore = new Label();
            lblScore.Text = $"Очки: 0";
            lblScore.Font = new Font("Arial", 12, FontStyle.Bold);
            lblScore.ForeColor = Color.DarkGreen;
            lblScore.Location = new Point(200, 5);
            lblScore.AutoSize = true;

            progressBar = new ProgressBar();
            progressBar.Location = new Point(400, 8);
            progressBar.Size = new Size(250, 25);
            progressBar.Maximum = totalQuestionsInSession;
            progressBar.Value = 0;

            infoPanel.Controls.Add(lblTime);
            infoPanel.Controls.Add(lblScore);
            infoPanel.Controls.Add(progressBar);

            lblQuestionNumber = new Label();
            lblQuestionNumber.Font = new Font("Arial", 10, FontStyle.Bold);
            lblQuestionNumber.Dock = DockStyle.Top;
            lblQuestionNumber.Height = 25;

            lblQuestionText = new Label();
            lblQuestionText.Font = new Font("Arial", 12);
            lblQuestionText.Dock = DockStyle.Top;
            lblQuestionText.Height = 80;

            pictureBox = new PictureBox();
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Dock = DockStyle.Top;
            pictureBox.Height = 150;

            answersPanel = new Panel();
            answersPanel.Dock = DockStyle.Top;
            answersPanel.Height = 150;
            answersPanel.AutoScroll = true;

            Panel buttonPanel = new Panel();
            buttonPanel.Dock = DockStyle.Top;
            buttonPanel.Height = 60;

            btnNext = new Button();
            btnNext.Text = "Следующий вопрос";
            btnNext.Size = new Size(150, 40);
            btnNext.Location = new Point(200, 10);
            btnNext.Click += BtnNext_Click;
            btnNext.Enabled = false;

            btnFinish = new Button();
            btnFinish.Text = "Завершить тест";
            btnFinish.Size = new Size(150, 40);
            btnFinish.Location = new Point(360, 10);
            btnFinish.Click += BtnFinish_Click;

            buttonPanel.Controls.Add(btnNext);
            buttonPanel.Controls.Add(btnFinish);

            mainPanel.Controls.Add(buttonPanel);
            mainPanel.Controls.Add(answersPanel);
            mainPanel.Controls.Add(pictureBox);
            mainPanel.Controls.Add(lblQuestionText);
            mainPanel.Controls.Add(lblQuestionNumber);
            mainPanel.Controls.Add(infoPanel);
            mainPanel.Controls.Add(lblLevelName);
            mainPanel.Controls.Add(lblTopicName);

            this.Controls.Add(mainPanel);

            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void LoadQuestionsForLevel()
        {
            var random = new Random();
            var allQuestions = currentLevel.Questions.ToList();

            if (allQuestions.Count >= totalQuestionsInSession)
            {
                currentQuestions = allQuestions
                    .OrderBy(x => random.Next())
                    .Take(totalQuestionsInSession)
                    .ToList();
            }
            else
            {
                currentQuestions = allQuestions;
                totalQuestionsInSession = currentQuestions.Count;
                progressBar.Maximum = totalQuestionsInSession;
            }

            currentQuestionIndex = 0;
            correctAnswers = 0;
            currentScore = 0;
            ShowQuestion();
        }

        private void ShowQuestion()
        {
            if (currentQuestionIndex < currentQuestions.Count)
            {
                var question = currentQuestions[currentQuestionIndex];
                lblQuestionNumber.Text = $"Вопрос {currentQuestionIndex + 1} из {totalQuestionsInSession}";
                lblQuestionText.Text = question.Text;

                if (!string.IsNullOrEmpty(question.ImagePath) && File.Exists(question.ImagePath))
                {
                    try
                    {
                        pictureBox.Image = Image.FromFile(question.ImagePath);
                    }
                    catch
                    {
                        pictureBox.Image = null;
                    }
                }
                else
                {
                    pictureBox.Image = null;
                }

                answersPanel.Controls.Clear();
                answerButtons.Clear();

                var shuffledAnswers = question.Answers
                    .OrderBy(x => Guid.NewGuid())
                    .ToList();

                int y = 10;
                foreach (var answer in shuffledAnswers)
                {
                    var radio = new RadioButton();
                    radio.Text = answer.Text;
                    radio.Tag = answer.IsRight;
                    radio.Location = new Point(20, y);
                    radio.AutoSize = true;
                    radio.CheckedChanged += Radio_CheckedChanged;
                    answersPanel.Controls.Add(radio);
                    answerButtons.Add(radio);
                    y += 35;
                }

                btnNext.Enabled = false;
            }
            else
            {
                EndTest();
            }
        }

        private void Radio_CheckedChanged(object sender, EventArgs e)
        {
            var radio = sender as RadioButton;
            if (radio != null && radio.Checked)
            {
                btnNext.Enabled = true;
            }
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            var selectedAnswer = answerButtons.FirstOrDefault(r => r.Checked);
            if (selectedAnswer != null)
            {
                bool isCorrect = (bool)selectedAnswer.Tag;
                if (isCorrect)
                {
                    correctAnswers++;
                    currentScore = (correctAnswers * 100) / totalQuestionsInSession;
                    lblScore.Text = $"Очки: {currentScore}";
                }

                currentQuestionIndex++;
                progressBar.Value = currentQuestionIndex;
                ShowQuestion();
            }
        }

        private void BtnFinish_Click(object sender, EventArgs e)
        {
            EndTest();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (timeRemaining > 0)
            {
                timeRemaining--;
                lblTime.Text = $"Время: {timeRemaining} сек";

                if (timeRemaining <= 10)
                {
                    lblTime.ForeColor = Color.Red;
                }
            }
            else
            {
                timer.Stop();
                EndTest();
            }
        }

        private void EndTest()
        {
            timer.Stop();

            currentScore = (correctAnswers * 100) / totalQuestionsInSession;

            string message = $"Результат тестирования:\n\n" +
                            $"Тема: {currentTopic.Name}\n" +
                            $"Уровень: {currentLevel.Name}\n" +
                            $"Правильных ответов: {correctAnswers} из {totalQuestionsInSession}\n" +
                            $"Набрано очков: {currentScore} из 100\n" +
                            $"Затрачено времени: {60 - timeRemaining} сек\n\n";

            if (currentScore >= 80)
            {
                message += "Поздравляем! Вы набрали достаточно баллов!\n";

                int nextLevelId = currentLevel.Id + 1;
                var nextLevel = currentTopic.Levels.FirstOrDefault(l => l.Id == nextLevelId);

                if (nextLevel != null)
                {
                    message += "Вы можете перейти на следующий уровень сложности.";
                    DialogResult dr = MessageBox.Show(message + "\n\nХотите продолжить на следующем уровне?",
                        "Отлично!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dr == DialogResult.Yes)
                    {
                        currentLevel = nextLevel;
                        lblLevelName.Text = $"Уровень: {currentLevel.Name}";
                        LoadQuestionsForLevel();
                        timeRemaining = 60;
                        lblTime.ForeColor = Color.DarkRed;
                        timer.Start();
                        return;
                    }
                }
                else
                {
                    message += "Вы прошли все уровни! Поздравляем с завершением темы!";
                }
            }
            else
            {
                message += "К сожалению, вы не набрали 80 баллов.\n" +
                          "Попробуйте еще раз, чтобы перейти на следующий уровень.";
            }

            MessageBox.Show(message, "Результат тестирования",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        private void TestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer?.Stop();
        }
    }
}