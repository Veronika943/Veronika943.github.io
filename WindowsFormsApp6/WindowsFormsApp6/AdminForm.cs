using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using RussianTraditionsQuiz.Classes;

namespace RussianTraditionsQuiz
{
    public partial class AdminForm : Form
    {
        private GroupBox grpTopicSelection;
        private Label lblTopic;
        private ComboBox cmbTopics;
        private Button btnAddTopic;
        private Label lblLevel;
        private NumericUpDown nudLevel;

        private GroupBox grpQuestionEditor;
        private Label lblQuestion;
        private TextBox txtQuestion;
        private Label lblImage;
        private TextBox txtImagePath;
        private Button btnBrowseImage;
        private Label lblAnswers;
        private ListBox listAnswers;
        private TextBox txtNewAnswer;
        private Button btnAddAnswer;
        private Button btnRemoveAnswer;
        private Label lblCorrectAnswer;
        private NumericUpDown nudCorrectAnswer;
        private Button btnSaveQuestion;
        private Button btnDeleteQuestion;
        private Button btnBackToMenu;

        private PictureBox pictureBoxPreview;

        public AdminForm()
        {
            InitializeComponent();
            LoadTopics();
            cmbTopics.SelectedIndexChanged += CmbTopics_SelectedIndexChanged;
        }

        private void InitializeComponent()
        {
            // Настройка формы
            this.Text = "Панель администратора";
            this.Size = new Size(950, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            // ==================== Группа выбора темы ====================
            grpTopicSelection = new GroupBox();
            grpTopicSelection.Text = "Выбор темы";
            grpTopicSelection.Font = new Font("Arial", 10, FontStyle.Bold);
            grpTopicSelection.Location = new Point(20, 20);
            grpTopicSelection.Size = new Size(450, 140);
            grpTopicSelection.BackColor = Color.White;

            lblTopic = new Label();
            lblTopic.Text = "Выберите тему:";
            lblTopic.Font = new Font("Arial", 9);
            lblTopic.Location = new Point(20, 30);
            lblTopic.Size = new Size(100, 25);

            cmbTopics = new ComboBox();
            cmbTopics.Location = new Point(20, 55);
            cmbTopics.Size = new Size(250, 25);
            cmbTopics.DropDownStyle = ComboBoxStyle.DropDownList;

            btnAddTopic = new Button();
            btnAddTopic.Text = "Добавить тему";
            btnAddTopic.Location = new Point(280, 55);
            btnAddTopic.Size = new Size(100, 27);
            btnAddTopic.BackColor = Color.LightGreen;
            btnAddTopic.Cursor = Cursors.Hand;
            btnAddTopic.Click += new EventHandler(btnAddTopic_Click);

            lblLevel = new Label();
            lblLevel.Text = "Уровень сложности:";
            lblLevel.Font = new Font("Arial", 9);
            lblLevel.Location = new Point(20, 95);
            lblLevel.Size = new Size(120, 25);

            nudLevel = new NumericUpDown();
            nudLevel.Location = new Point(150, 92);
            nudLevel.Size = new Size(60, 25);
            nudLevel.Minimum = 1;
            nudLevel.Maximum = 3;
            nudLevel.Value = 1;
            nudLevel.ValueChanged += new EventHandler(nudLevel_ValueChanged);

            grpTopicSelection.Controls.Add(lblTopic);
            grpTopicSelection.Controls.Add(cmbTopics);
            grpTopicSelection.Controls.Add(btnAddTopic);
            grpTopicSelection.Controls.Add(lblLevel);
            grpTopicSelection.Controls.Add(nudLevel);

            // ==================== Группа редактора вопросов ====================
            grpQuestionEditor = new GroupBox();
            grpQuestionEditor.Text = "Редактор вопросов";
            grpQuestionEditor.Font = new Font("Arial", 10, FontStyle.Bold);
            grpQuestionEditor.Location = new Point(20, 170);
            grpQuestionEditor.Size = new Size(900, 520);
            grpQuestionEditor.BackColor = Color.White;

            lblQuestion = new Label();
            lblQuestion.Text = "Текст вопроса:";
            lblQuestion.Font = new Font("Arial", 9);
            lblQuestion.Location = new Point(20, 30);
            lblQuestion.Size = new Size(100, 25);

            txtQuestion = new TextBox();
            txtQuestion.Location = new Point(20, 55);
            txtQuestion.Size = new Size(850, 80);
            txtQuestion.Multiline = true;
            txtQuestion.Font = new Font("Arial", 9);

            lblImage = new Label();
            lblImage.Text = "Путь к изображению:";
            lblImage.Font = new Font("Arial", 9);
            lblImage.Location = new Point(20, 150);
            lblImage.Size = new Size(120, 25);

            txtImagePath = new TextBox();
            txtImagePath.Location = new Point(20, 175);
            txtImagePath.Size = new Size(650, 25);
            txtImagePath.ReadOnly = true;

            btnBrowseImage = new Button();
            btnBrowseImage.Text = "Обзор...";
            btnBrowseImage.Location = new Point(680, 175);
            btnBrowseImage.Size = new Size(80, 27);
            btnBrowseImage.Cursor = Cursors.Hand;
            btnBrowseImage.Click += new EventHandler(btnBrowseImage_Click);

            lblAnswers = new Label();
            lblAnswers.Text = "Варианты ответов:";
            lblAnswers.Font = new Font("Arial", 9);
            lblAnswers.Location = new Point(20, 220);
            lblAnswers.Size = new Size(120, 25);

            listAnswers = new ListBox();
            listAnswers.Location = new Point(20, 245);
            listAnswers.Size = new Size(550, 120);
            listAnswers.Font = new Font("Arial", 9);

            txtNewAnswer = new TextBox();
            txtNewAnswer.Location = new Point(20, 375);
            txtNewAnswer.Size = new Size(450, 25);
            txtNewAnswer.Font = new Font("Arial", 9);

            btnAddAnswer = new Button();
            btnAddAnswer.Text = "Добавить";
            btnAddAnswer.Location = new Point(480, 375);
            btnAddAnswer.Size = new Size(90, 27);
            btnAddAnswer.BackColor = Color.LightBlue;
            btnAddAnswer.Cursor = Cursors.Hand;
            btnAddAnswer.Click += new EventHandler(btnAddAnswer_Click);

            btnRemoveAnswer = new Button();
            btnRemoveAnswer.Text = "Удалить";
            btnRemoveAnswer.Location = new Point(580, 375);
            btnRemoveAnswer.Size = new Size(90, 27);
            btnRemoveAnswer.BackColor = Color.LightCoral;
            btnRemoveAnswer.Cursor = Cursors.Hand;
            btnRemoveAnswer.Click += new EventHandler(btnRemoveAnswer_Click);

            lblCorrectAnswer = new Label();
            lblCorrectAnswer.Text = "Правильный ответ (номер):";
            lblCorrectAnswer.Font = new Font("Arial", 9);
            lblCorrectAnswer.Location = new Point(20, 420);
            lblCorrectAnswer.Size = new Size(170, 25);

            nudCorrectAnswer = new NumericUpDown();
            nudCorrectAnswer.Location = new Point(200, 417);
            nudCorrectAnswer.Size = new Size(60, 25);
            nudCorrectAnswer.Minimum = 1;
            nudCorrectAnswer.Maximum = 10;
            nudCorrectAnswer.Value = 1;

            btnSaveQuestion = new Button();
            btnSaveQuestion.Text = "Сохранить вопрос";
            btnSaveQuestion.Location = new Point(20, 470);
            btnSaveQuestion.Size = new Size(150, 40);
            btnSaveQuestion.BackColor = Color.LightGreen;
            btnSaveQuestion.Font = new Font("Arial", 9, FontStyle.Bold);
            btnSaveQuestion.Cursor = Cursors.Hand;
            btnSaveQuestion.Click += new EventHandler(btnSaveQuestion_Click);

            btnDeleteQuestion = new Button();
            btnDeleteQuestion.Text = "Удалить вопрос";
            btnDeleteQuestion.Location = new Point(190, 470);
            btnDeleteQuestion.Size = new Size(150, 40);
            btnDeleteQuestion.BackColor = Color.LightCoral;
            btnDeleteQuestion.Font = new Font("Arial", 9, FontStyle.Bold);
            btnDeleteQuestion.Cursor = Cursors.Hand;
            btnDeleteQuestion.Click += new EventHandler(btnDeleteQuestion_Click);

            btnBackToMenu = new Button();
            btnBackToMenu.Text = "Назад в меню";
            btnBackToMenu.Location = new Point(760, 470);
            btnBackToMenu.Size = new Size(120, 40);
            btnBackToMenu.BackColor = Color.LightGray;
            btnBackToMenu.Font = new Font("Arial", 9, FontStyle.Bold);
            btnBackToMenu.Cursor = Cursors.Hand;
            btnBackToMenu.Click += new EventHandler(btnBackToMenu_Click);

            grpQuestionEditor.Controls.Add(lblQuestion);
            grpQuestionEditor.Controls.Add(txtQuestion);
            grpQuestionEditor.Controls.Add(lblImage);
            grpQuestionEditor.Controls.Add(txtImagePath);
            grpQuestionEditor.Controls.Add(btnBrowseImage);
            grpQuestionEditor.Controls.Add(lblAnswers);
            grpQuestionEditor.Controls.Add(listAnswers);
            grpQuestionEditor.Controls.Add(txtNewAnswer);
            grpQuestionEditor.Controls.Add(btnAddAnswer);
            grpQuestionEditor.Controls.Add(btnRemoveAnswer);
            grpQuestionEditor.Controls.Add(lblCorrectAnswer);
            grpQuestionEditor.Controls.Add(nudCorrectAnswer);
            grpQuestionEditor.Controls.Add(btnSaveQuestion);
            grpQuestionEditor.Controls.Add(btnDeleteQuestion);
            grpQuestionEditor.Controls.Add(btnBackToMenu);

            // ==================== PictureBox для предпросмотра ====================
            pictureBoxPreview = new PictureBox();
            pictureBoxPreview.Location = new Point(700, 55);
            pictureBoxPreview.Size = new Size(180, 180);
            pictureBoxPreview.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxPreview.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxPreview.BackColor = Color.LightGray;
            pictureBoxPreview.Visible = false;
            grpQuestionEditor.Controls.Add(pictureBoxPreview);

            // Добавление групп на форму
            this.Controls.Add(grpTopicSelection);
            this.Controls.Add(grpQuestionEditor);
        }

        private void LoadTopics()
        {
            cmbTopics.Items.Clear();
            var topics = XMLHelper.GetTopics();
            foreach (var topic in topics)
            {
                cmbTopics.Items.Add(topic);
            }
            if (cmbTopics.Items.Count > 0)
                cmbTopics.SelectedIndex = 0;
        }

        private void CmbTopics_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadQuestionsForTopic();
        }

        private void nudLevel_ValueChanged(object sender, EventArgs e)
        {
            LoadQuestionsForTopic();
        }

        private void LoadQuestionsForTopic()
        {
            if (cmbTopics.SelectedItem != null)
            {
                string topic = cmbTopics.SelectedItem.ToString();
                int level = (int)nudLevel.Value;
                var questions = XMLHelper.GetQuestions(topic, level);

                // Очищаем список ответов и поля
                listAnswers.Items.Clear();
                txtQuestion.Clear();
                txtImagePath.Clear();
                pictureBoxPreview.Visible = false;
            }
        }

        private void btnAddTopic_Click(object sender, EventArgs e)
        {
            // Создаем простую форму для ввода названия темы
            Form inputForm = new Form();
            inputForm.Text = "Добавление темы";
            inputForm.Size = new Size(400, 150);
            inputForm.StartPosition = FormStartPosition.CenterParent;
            inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            inputForm.MaximizeBox = false;

            Label lblInput = new Label();
            lblInput.Text = "Введите название новой темы:";
            lblInput.Location = new Point(20, 20);
            lblInput.Size = new Size(200, 25);

            TextBox txtInput = new TextBox();
            txtInput.Location = new Point(20, 50);
            txtInput.Size = new Size(340, 25);

            Button btnOk = new Button();
            btnOk.Text = "OK";
            btnOk.Location = new Point(100, 85);
            btnOk.Size = new Size(80, 30);
            btnOk.DialogResult = DialogResult.OK;

            Button btnCancel = new Button();
            btnCancel.Text = "Отмена";
            btnCancel.Location = new Point(200, 85);
            btnCancel.Size = new Size(80, 30);
            btnCancel.DialogResult = DialogResult.Cancel;

            inputForm.Controls.Add(lblInput);
            inputForm.Controls.Add(txtInput);
            inputForm.Controls.Add(btnOk);
            inputForm.Controls.Add(btnCancel);

            if (inputForm.ShowDialog() == DialogResult.OK)
            {
                string newTopic = txtInput.Text.Trim();
                if (!string.IsNullOrEmpty(newTopic))
                {
                    if (XMLHelper.TopicExists(newTopic))
                    {
                        MessageBox.Show("Тема с таким названием уже существует!", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (XMLHelper.AddTopic(newTopic))
                    {
                        MessageBox.Show("Тема успешно добавлена!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadTopics();
                    }
                }
                else
                {
                    MessageBox.Show("Название темы не может быть пустым!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            openFileDialog.Title = "Выберите изображение для вопроса";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Создаем папку Images если её нет
                string imagesFolder = Path.Combine(Application.StartupPath, "Images");
                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                string destPath = Path.Combine(imagesFolder, Path.GetFileName(openFileDialog.FileName));

                try
                {
                    File.Copy(openFileDialog.FileName, destPath, true);
                    txtImagePath.Text = destPath;

                    pictureBoxPreview.Image = Image.FromFile(destPath);
                    pictureBoxPreview.Visible = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка копирования файла: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAddAnswer_Click(object sender, EventArgs e)
        {
            string newAnswer = txtNewAnswer.Text.Trim();
            if (!string.IsNullOrEmpty(newAnswer))
            {
                listAnswers.Items.Add(newAnswer);
                txtNewAnswer.Clear();
                nudCorrectAnswer.Maximum = listAnswers.Items.Count;
            }
            else
            {
                MessageBox.Show("Введите текст ответа!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRemoveAnswer_Click(object sender, EventArgs e)
        {
            if (listAnswers.SelectedIndex >= 0)
            {
                listAnswers.Items.RemoveAt(listAnswers.SelectedIndex);

                if (nudCorrectAnswer.Value > listAnswers.Items.Count)
                {
                    nudCorrectAnswer.Value = listAnswers.Items.Count;
                }
                nudCorrectAnswer.Maximum = listAnswers.Items.Count;
            }
            else
            {
                MessageBox.Show("Выберите ответ для удаления!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSaveQuestion_Click(object sender, EventArgs e)
        {
            if (cmbTopics.SelectedItem == null)
            {
                MessageBox.Show("Выберите тему!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string questionText = txtQuestion.Text.Trim();
            if (string.IsNullOrEmpty(questionText))
            {
                MessageBox.Show("Введите текст вопроса!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (listAnswers.Items.Count < 2)
            {
                MessageBox.Show("Добавьте как минимум 2 варианта ответа!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int correctIndex = (int)nudCorrectAnswer.Value - 1;
            if (correctIndex < 0 || correctIndex >= listAnswers.Items.Count)
            {
                MessageBox.Show("Укажите правильный номер ответа!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<string> answers = new List<string>();
            foreach (var item in listAnswers.Items)
            {
                answers.Add(item.ToString());
            }

            Question newQuestion = new Question
            {
                Text = questionText,
                Answers = answers,
                CorrectAnswerIndex = correctIndex,
                ImagePath = txtImagePath.Text
            };

            string topic = cmbTopics.SelectedItem.ToString();
            int level = (int)nudLevel.Value;

            if (XMLHelper.SaveQuestion(topic, level, newQuestion))
            {
                MessageBox.Show("Вопрос успешно сохранен!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtQuestion.Clear();
                listAnswers.Items.Clear();
                txtImagePath.Clear();
                pictureBoxPreview.Visible = false;
                nudCorrectAnswer.Value = 1;
                nudCorrectAnswer.Maximum = 10;
            }
        }

        private void btnDeleteQuestion_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Функция удаления вопросов будет добавлена в следующей версии.",
                "В разработке", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnBackToMenu_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Удаляем переопределение Dispose, если оно есть
    }
}