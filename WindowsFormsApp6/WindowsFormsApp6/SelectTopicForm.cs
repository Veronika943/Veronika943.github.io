using System;
using System.Drawing;
using System.Windows.Forms;
using RussianTraditionsQuiz.Classes;

namespace RussianTraditionsQuiz
{
    public partial class SelectTopicForm : Form
    {
        private Label lblTopic;
        private ComboBox cmbTopics;
        private Label lblLevel;
        private ComboBox cmbLevel;
        private Button btnStartQuiz;
        private Button btnBack;
        private Panel panelHeader;
        private Label lblHeader;

        public SelectTopicForm()
        {
            InitializeComponent();
            LoadTopics();
        }

        private void InitializeComponent()
        {
            // Настройка формы
            this.Text = "Выбор темы и уровня";
            this.Size = new Size(600, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            // Панель заголовка
            panelHeader = new Panel();
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Height = 80;
            panelHeader.BackColor = Color.DarkRed;

            lblHeader = new Label();
            lblHeader.Text = "Выберите тему и уровень сложности";
            lblHeader.Font = new Font("Arial", 18, FontStyle.Bold);
            lblHeader.ForeColor = Color.White;
            lblHeader.Location = new Point(150, 25);
            lblHeader.AutoSize = true;
            lblHeader.BackColor = Color.Transparent;
            panelHeader.Controls.Add(lblHeader);

            // Метка для темы
            lblTopic = new Label();
            lblTopic.Text = "Выберите тему:";
            lblTopic.Font = new Font("Arial", 12, FontStyle.Bold);
            lblTopic.Location = new Point(100, 120);
            lblTopic.Size = new Size(150, 30);
            lblTopic.BackColor = Color.Transparent;

            // Выпадающий список тем
            cmbTopics = new ComboBox();
            cmbTopics.Location = new Point(100, 155);
            cmbTopics.Size = new Size(350, 30);
            cmbTopics.Font = new Font("Arial", 10);
            cmbTopics.DropDownStyle = ComboBoxStyle.DropDownList;

            // Метка для уровня
            lblLevel = new Label();
            lblLevel.Text = "Выберите уровень сложности:";
            lblLevel.Font = new Font("Arial", 12, FontStyle.Bold);
            lblLevel.Location = new Point(100, 210);
            lblLevel.Size = new Size(250, 30);
            lblLevel.BackColor = Color.Transparent;

            // Выпадающий список уровней
            cmbLevel = new ComboBox();
            cmbLevel.Location = new Point(100, 245);
            cmbLevel.Size = new Size(200, 30);
            cmbLevel.Font = new Font("Arial", 10);
            cmbLevel.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLevel.Items.AddRange(new string[] { "1 - Легкий", "2 - Средний", "3 - Сложный" });
            cmbLevel.SelectedIndex = 0;

            // Кнопка "Начать"
            btnStartQuiz = new Button();
            btnStartQuiz.Text = "Начать викторину";
            btnStartQuiz.Font = new Font("Arial", 12, FontStyle.Bold);
            btnStartQuiz.Location = new Point(100, 320);
            btnStartQuiz.Size = new Size(160, 45);
            btnStartQuiz.BackColor = Color.LightGreen;
            btnStartQuiz.FlatStyle = FlatStyle.Flat;
            btnStartQuiz.Cursor = Cursors.Hand;
            btnStartQuiz.Click += new EventHandler(btnStartQuiz_Click);

            // Кнопка "Назад"
            btnBack = new Button();
            btnBack.Text = "Назад";
            btnBack.Font = new Font("Arial", 12, FontStyle.Bold);
            btnBack.Location = new Point(290, 320);
            btnBack.Size = new Size(160, 45);
            btnBack.BackColor = Color.LightGray;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Cursor = Cursors.Hand;
            btnBack.Click += new EventHandler(btnBack_Click);

            // Добавление элементов на форму
            this.Controls.Add(panelHeader);
            this.Controls.Add(lblTopic);
            this.Controls.Add(cmbTopics);
            this.Controls.Add(lblLevel);
            this.Controls.Add(cmbLevel);
            this.Controls.Add(btnStartQuiz);
            this.Controls.Add(btnBack);
        }

        private void LoadTopics()
        {
            try
            {
                cmbTopics.Items.Clear();
                var topics = XMLHelper.GetTopics();

                if (topics.Count == 0)
                {
                    MessageBox.Show("Нет доступных тем. Пожалуйста, добавьте темы через панель администратора.",
                        "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnStartQuiz.Enabled = false;
                }
                else
                {
                    foreach (var topic in topics)
                    {
                        cmbTopics.Items.Add(topic);
                    }
                    cmbTopics.SelectedIndex = 0;
                    btnStartQuiz.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки тем: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStartQuiz_Click(object sender, EventArgs e)
        {
            if (cmbTopics.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите тему", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedTopic = cmbTopics.SelectedItem.ToString();
            int selectedLevel = cmbLevel.SelectedIndex + 1;

            int questionsCount = XMLHelper.GetQuestionsCount(selectedTopic, selectedLevel);

            if (questionsCount == 0)
            {
                MessageBox.Show($"В выбранной теме '{selectedTopic}' уровне {selectedLevel} нет вопросов.\n" +
                    "Пожалуйста, добавьте вопросы через панель администратора.",
                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            QuizForm quizForm = new QuizForm(selectedTopic, selectedLevel);
            this.Hide();
            quizForm.ShowDialog();
            this.Show();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Удаляем переопределение Dispose, если оно есть
    }
}