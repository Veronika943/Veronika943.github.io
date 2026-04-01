using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using RussianQuiz.Helpers;
using RussianQuiz.Models;

namespace RussianQuiz.Forms
{
    public class AdminForm : Form
    {
        private TabControl tabControl;
        private List<Topic> topics;

        // Вкладка "Темы"
        private ListBox lstTopics;
        private TextBox txtTopicName;
        private TextBox txtTopicDesc;
        private Button btnAddTopic;
        private Button btnEditTopic;
        private Button btnDeleteTopic;

        // Вкладка "Вопросы"
        private ComboBox cmbTopicsForQuestions;
        private ComboBox cmbLevelsForQuestions;
        private ListBox lstQuestions;
        private TextBox txtQuestionText;
        private TextBox txtImagePath;
        private Button btnBrowseImage;
        private Panel answersEditPanel;
        private List<TextBox> answerTexts = new List<TextBox>();
        private List<CheckBox> answerChecks = new List<CheckBox>();
        private Button btnAddAnswer;
        private Button btnRemoveAnswer;
        private Button btnAddQuestion;
        private Button btnEditQuestion;
        private Button btnDeleteQuestion;
        private Button btnSaveAll;

        private Topic selectedTopic;
        private Level selectedLevel;
        private Question selectedQuestion;

        public AdminForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = "Панель администратора";
            this.Size = new Size(900, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;

            // Вкладка "Темы"
            TabPage topicsPage = new TabPage("Управление темами");
            SetupTopicsPage(topicsPage);
            tabControl.TabPages.Add(topicsPage);

            // Вкладка "Вопросы"
            TabPage questionsPage = new TabPage("Управление вопросами");
            SetupQuestionsPage(questionsPage);
            tabControl.TabPages.Add(questionsPage);

            // Кнопка сохранения
            Button btnSave = new Button();
            btnSave.Text = "Сохранить все изменения";
            btnSave.Size = new Size(150, 30);
            btnSave.Location = new Point(this.Width - 160, this.Height - 45);
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.Click += BtnSaveAll_Click;

            this.Controls.Add(tabControl);
            this.Controls.Add(btnSave);
        }

        private void SetupTopicsPage(TabPage page)
        {
            Panel leftPanel = new Panel();
            leftPanel.Dock = DockStyle.Left;
            leftPanel.Width = 300;
            leftPanel.Padding = new Padding(10);

            Label lblTopics = new Label();
            lblTopics.Text = "Список тем:";
            lblTopics.Dock = DockStyle.Top;
            lblTopics.Height = 25;

            lstTopics = new ListBox();
            lstTopics.Dock = DockStyle.Fill;
            lstTopics.SelectedIndexChanged += LstTopics_SelectedIndexChanged;

            leftPanel.Controls.Add(lstTopics);
            leftPanel.Controls.Add(lblTopics);

            Panel rightPanel = new Panel();
            rightPanel.Dock = DockStyle.Fill;
            rightPanel.Padding = new Padding(10);

            Label lblName = new Label();
            lblName.Text = "Название темы:";
            lblName.Location = new Point(10, 10);
            lblName.AutoSize = true;

            txtTopicName = new TextBox();
            txtTopicName.Location = new Point(10, 35);
            txtTopicName.Width = 250;

            Label lblDesc = new Label();
            lblDesc.Text = "Описание:";
            lblDesc.Location = new Point(10, 70);
            lblDesc.AutoSize = true;

            txtTopicDesc = new TextBox();
            txtTopicDesc.Location = new Point(10, 95);
            txtTopicDesc.Width = 250;
            txtTopicDesc.Height = 80;
            txtTopicDesc.Multiline = true;

            btnAddTopic = new Button();
            btnAddTopic.Text = "Добавить тему";
            btnAddTopic.Location = new Point(10, 190);
            btnAddTopic.Size = new Size(120, 30);
            btnAddTopic.Click += BtnAddTopic_Click;

            btnEditTopic = new Button();
            btnEditTopic.Text = "Редактировать";
            btnEditTopic.Location = new Point(140, 190);
            btnEditTopic.Size = new Size(120, 30);
            btnEditTopic.Click += BtnEditTopic_Click;

            btnDeleteTopic = new Button();
            btnDeleteTopic.Text = "Удалить тему";
            btnDeleteTopic.Location = new Point(10, 230);
            btnDeleteTopic.Size = new Size(250, 30);
            btnDeleteTopic.Click += BtnDeleteTopic_Click;

            rightPanel.Controls.Add(btnDeleteTopic);
            rightPanel.Controls.Add(btnEditTopic);
            rightPanel.Controls.Add(btnAddTopic);
            rightPanel.Controls.Add(txtTopicDesc);
            rightPanel.Controls.Add(lblDesc);
            rightPanel.Controls.Add(txtTopicName);
            rightPanel.Controls.Add(lblName);

            page.Controls.Add(rightPanel);
            page.Controls.Add(leftPanel);
        }

        private void SetupQuestionsPage(TabPage page)
        {
            Panel topPanel = new Panel();
            topPanel.Dock = DockStyle.Top;
            topPanel.Height = 80;
            topPanel.Padding = new Padding(10);

            Label lblTopic = new Label();
            lblTopic.Text = "Выберите тему:";
            lblTopic.Location = new Point(10, 15);
            lblTopic.AutoSize = true;

            cmbTopicsForQuestions = new ComboBox();
            cmbTopicsForQuestions.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTopicsForQuestions.Location = new Point(120, 12);
            cmbTopicsForQuestions.Width = 200;
            cmbTopicsForQuestions.SelectedIndexChanged += CmbTopicsForQuestions_SelectedIndexChanged;

            Label lblLevel = new Label();
            lblLevel.Text = "Уровень сложности:";
            lblLevel.Location = new Point(340, 15);
            lblLevel.AutoSize = true;

            cmbLevelsForQuestions = new ComboBox();
            cmbLevelsForQuestions.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLevelsForQuestions.Location = new Point(470, 12);
            cmbLevelsForQuestions.Width = 150;
            cmbLevelsForQuestions.SelectedIndexChanged += CmbLevelsForQuestions_SelectedIndexChanged;

            topPanel.Controls.Add(lblTopic);
            topPanel.Controls.Add(cmbTopicsForQuestions);
            topPanel.Controls.Add(lblLevel);
            topPanel.Controls.Add(cmbLevelsForQuestions);

            // Левая панель - список вопросов
            Panel leftPanel = new Panel();
            leftPanel.Dock = DockStyle.Left;
            leftPanel.Width = 300;
            leftPanel.Padding = new Padding(10);

            Label lblQuestions = new Label();
            lblQuestions.Text = "Вопросы:";
            lblQuestions.Dock = DockStyle.Top;
            lblQuestions.Height = 25;

            lstQuestions = new ListBox();
            lstQuestions.Dock = DockStyle.Fill;
            lstQuestions.SelectedIndexChanged += LstQuestions_SelectedIndexChanged;

            leftPanel.Controls.Add(lstQuestions);
            leftPanel.Controls.Add(lblQuestions);

            // Правая панель - редактирование вопроса
            Panel rightPanel = new Panel();
            rightPanel.Dock = DockStyle.Fill;
            rightPanel.Padding = new Padding(10);
            rightPanel.AutoScroll = true;

            Label lblQuestionText = new Label();
            lblQuestionText.Text = "Текст вопроса:";
            lblQuestionText.Location = new Point(10, 10);
            lblQuestionText.AutoSize = true;

            txtQuestionText = new TextBox();
            txtQuestionText.Location = new Point(10, 35);
            txtQuestionText.Width = 500;
            txtQuestionText.Multiline = true;
            txtQuestionText.Height = 60;

            Label lblImage = new Label();
            lblImage.Text = "Путь к изображению:";
            lblImage.Location = new Point(10, 105);
            lblImage.AutoSize = true;

            txtImagePath = new TextBox();
            txtImagePath.Location = new Point(10, 130);
            txtImagePath.Width = 400;

            btnBrowseImage = new Button();
            btnBrowseImage.Text = "Обзор...";
            btnBrowseImage.Location = new Point(420, 128);
            btnBrowseImage.Size = new Size(90, 25);
            btnBrowseImage.Click += BtnBrowseImage_Click;

            Label lblAnswers = new Label();
            lblAnswers.Text = "Варианты ответов (отметьте правильный):";
            lblAnswers.Location = new Point(10, 165);
            lblAnswers.AutoSize = true;

            answersEditPanel = new Panel();
            answersEditPanel.Location = new Point(10, 190);
            answersEditPanel.Size = new Size(550, 200);
            answersEditPanel.AutoScroll = true;

            btnAddAnswer = new Button();
            btnAddAnswer.Text = "Добавить ответ";
            btnAddAnswer.Location = new Point(10, 400);
            btnAddAnswer.Size = new Size(120, 30);
            btnAddAnswer.Click += BtnAddAnswer_Click;

            btnRemoveAnswer = new Button();
            btnRemoveAnswer.Text = "Удалить последний";
            btnRemoveAnswer.Location = new Point(140, 400);
            btnRemoveAnswer.Size = new Size(120, 30);
            btnRemoveAnswer.Click += BtnRemoveAnswer_Click;

            btnAddQuestion = new Button();
            btnAddQuestion.Text = "Добавить вопрос";
            btnAddQuestion.Location = new Point(280, 400);
            btnAddQuestion.Size = new Size(120, 30);
            btnAddQuestion.Click += BtnAddQuestion_Click;

            btnEditQuestion = new Button();
            btnEditQuestion.Text = "Редактировать";
            btnEditQuestion.Location = new Point(410, 400);
            btnEditQuestion.Size = new Size(120, 30);
            btnEditQuestion.Click += BtnEditQuestion_Click;

            btnDeleteQuestion = new Button();
            btnDeleteQuestion.Text = "Удалить вопрос";
            btnDeleteQuestion.Location = new Point(10, 440);
            btnDeleteQuestion.Size = new Size(250, 30);
            btnDeleteQuestion.Click += BtnDeleteQuestion_Click;

            rightPanel.Controls.Add(btnDeleteQuestion);
            rightPanel.Controls.Add(btnEditQuestion);
            rightPanel.Controls.Add(btnAddQuestion);
            rightPanel.Controls.Add(btnRemoveAnswer);
            rightPanel.Controls.Add(btnAddAnswer);
            rightPanel.Controls.Add(answersEditPanel);
            rightPanel.Controls.Add(lblAnswers);
            rightPanel.Controls.Add(btnBrowseImage);
            rightPanel.Controls.Add(txtImagePath);
            rightPanel.Controls.Add(lblImage);
            rightPanel.Controls.Add(txtQuestionText);
            rightPanel.Controls.Add(lblQuestionText);

            page.Controls.Add(rightPanel);
            page.Controls.Add(leftPanel);
            page.Controls.Add(topPanel);
        }

        private void LoadData()
        {
            try
            {
                string xmlPath = Path.Combine(Application.StartupPath, "Data", "questions.xml");
                XmlHelper.Initialize(xmlPath);
                topics = XmlHelper.LoadTopics();

                // Заполнение списка тем
                lstTopics.Items.Clear();
                foreach (var topic in topics)
                {
                    lstTopics.Items.Add(topic.Name);
                }

                // Заполнение выпадающих списков для вопросов
                cmbTopicsForQuestions.Items.Clear();
                foreach (var topic in topics)
                {
                    cmbTopicsForQuestions.Items.Add(topic.Name);
                }

                if (cmbTopicsForQuestions.Items.Count > 0)
                    cmbTopicsForQuestions.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LstTopics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstTopics.SelectedIndex >= 0 && topics != null)
            {
                selectedTopic = topics[lstTopics.SelectedIndex];
                txtTopicName.Text = selectedTopic.Name;
                txtTopicDesc.Text = selectedTopic.Description;
            }
        }

        private void BtnAddTopic_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtTopicName.Text))
            {
                var newTopic = new Topic
                {
                    Name = txtTopicName.Text,
                    Description = txtTopicDesc.Text,
                    Levels = new List<Level>()
                };

                for (int i = 1; i <= 3; i++)
                {
                    string levelName = i == 1 ? "Легкий" : (i == 2 ? "Средний" : "Сложный");
                    newTopic.Levels.Add(new Level
                    {
                        Id = i,
                        Name = levelName,
                        Questions = new List<Question>()
                    });
                }

                topics.Add(newTopic);
                lstTopics.Items.Add(newTopic.Name);
                cmbTopicsForQuestions.Items.Add(newTopic.Name);

                txtTopicName.Clear();
                txtTopicDesc.Clear();

                MessageBox.Show("Тема добавлена!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnEditTopic_Click(object sender, EventArgs e)
        {
            if (selectedTopic != null)
            {
                selectedTopic.Name = txtTopicName.Text;
                selectedTopic.Description = txtTopicDesc.Text;

                int index = lstTopics.SelectedIndex;
                lstTopics.Items[index] = selectedTopic.Name;

                MessageBox.Show("Тема отредактирована!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnDeleteTopic_Click(object sender, EventArgs e)
        {
            if (selectedTopic != null)
            {
                DialogResult dr = MessageBox.Show($"Удалить тему '{selectedTopic.Name}'?",
                    "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    topics.Remove(selectedTopic);
                    lstTopics.Items.Remove(selectedTopic.Name);
                    cmbTopicsForQuestions.Items.Remove(selectedTopic.Name);
                    selectedTopic = null;
                    txtTopicName.Clear();
                    txtTopicDesc.Clear();

                    MessageBox.Show("Тема удалена!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void CmbTopicsForQuestions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTopicsForQuestions.SelectedIndex >= 0)
            {
                selectedTopic = topics[cmbTopicsForQuestions.SelectedIndex];
                cmbLevelsForQuestions.Items.Clear();

                foreach (var level in selectedTopic.Levels)
                {
                    cmbLevelsForQuestions.Items.Add(level.Name);
                }

                if (cmbLevelsForQuestions.Items.Count > 0)
                    cmbLevelsForQuestions.SelectedIndex = 0;
            }
        }

        private void CmbLevelsForQuestions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLevelsForQuestions.SelectedIndex >= 0 && selectedTopic != null)
            {
                selectedLevel = selectedTopic.Levels[cmbLevelsForQuestions.SelectedIndex];
                lstQuestions.Items.Clear();

                foreach (var question in selectedLevel.Questions)
                {
                    string text = question.Text.Length > 50 ?
                        question.Text.Substring(0, 47) + "..." :
                        question.Text;
                    lstQuestions.Items.Add(text);
                }

                ClearQuestionForm();
            }
        }

        private void LstQuestions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstQuestions.SelectedIndex >= 0 && selectedLevel != null)
            {
                selectedQuestion = selectedLevel.Questions[lstQuestions.SelectedIndex];
                txtQuestionText.Text = selectedQuestion.Text;
                txtImagePath.Text = selectedQuestion.ImagePath;

                answersEditPanel.Controls.Clear();
                answerTexts.Clear();
                answerChecks.Clear();

                int y = 5;
                foreach (var answer in selectedQuestion.Answers)
                {
                    var txtAnswer = new TextBox();
                    txtAnswer.Text = answer.Text;
                    txtAnswer.Location = new Point(5, y);
                    txtAnswer.Width = 400;

                    var chkRight = new CheckBox();
                    chkRight.Text = "Правильный";
                    chkRight.Location = new Point(410, y + 5);
                    chkRight.Checked = answer.IsRight;

                    answersEditPanel.Controls.Add(txtAnswer);
                    answersEditPanel.Controls.Add(chkRight);

                    answerTexts.Add(txtAnswer);
                    answerChecks.Add(chkRight);

                    y += 35;
                }
            }
        }

        private void ClearQuestionForm()
        {
            txtQuestionText.Clear();
            txtImagePath.Clear();
            answersEditPanel.Controls.Clear();
            answerTexts.Clear();
            answerChecks.Clear();
            selectedQuestion = null;
        }

        private void BtnAddAnswer_Click(object sender, EventArgs e)
        {
            int y = answerTexts.Count * 35 + 5;

            var txtAnswer = new TextBox();
            txtAnswer.Location = new Point(5, y);
            txtAnswer.Width = 400;

            var chkRight = new CheckBox();
            chkRight.Text = "Правильный";
            chkRight.Location = new Point(410, y + 5);

            answersEditPanel.Controls.Add(txtAnswer);
            answersEditPanel.Controls.Add(chkRight);

            answerTexts.Add(txtAnswer);
            answerChecks.Add(chkRight);
        }

        private void BtnRemoveAnswer_Click(object sender, EventArgs e)
        {
            if (answerTexts.Count > 0)
            {
                int lastIndex = answerTexts.Count - 1;
                answersEditPanel.Controls.Remove(answerTexts[lastIndex]);
                answersEditPanel.Controls.Remove(answerChecks[lastIndex]);
                answerTexts.RemoveAt(lastIndex);
                answerChecks.RemoveAt(lastIndex);
            }
        }

        private void BtnAddQuestion_Click(object sender, EventArgs e)
        {
            if (selectedLevel != null && !string.IsNullOrWhiteSpace(txtQuestionText.Text))
            {
                var newQuestion = new Question
                {
                    Id = selectedLevel.Questions.Count + 1,
                    Text = txtQuestionText.Text,
                    ImagePath = txtImagePath.Text,
                    Answers = new List<Answer>()
                };

                for (int i = 0; i < answerTexts.Count; i++)
                {
                    if (!string.IsNullOrWhiteSpace(answerTexts[i].Text))
                    {
                        newQuestion.Answers.Add(new Answer
                        {
                            Text = answerTexts[i].Text,
                            IsRight = answerChecks[i].Checked
                        });
                    }
                }

                if (newQuestion.Answers.Count >= 2)
                {
                    selectedLevel.Questions.Add(newQuestion);
                    string displayText = newQuestion.Text.Length > 50 ?
                        newQuestion.Text.Substring(0, 47) + "..." :
                        newQuestion.Text;
                    lstQuestions.Items.Add(displayText);
                    ClearQuestionForm();

                    MessageBox.Show("Вопрос добавлен!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Добавьте минимум 2 варианта ответа!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void BtnEditQuestion_Click(object sender, EventArgs e)
        {
            if (selectedQuestion != null)
            {
                selectedQuestion.Text = txtQuestionText.Text;
                selectedQuestion.ImagePath = txtImagePath.Text;
                selectedQuestion.Answers.Clear();

                for (int i = 0; i < answerTexts.Count; i++)
                {
                    if (!string.IsNullOrWhiteSpace(answerTexts[i].Text))
                    {
                        selectedQuestion.Answers.Add(new Answer
                        {
                            Text = answerTexts[i].Text,
                            IsRight = answerChecks[i].Checked
                        });
                    }
                }

                int index = lstQuestions.SelectedIndex;
                string displayText = selectedQuestion.Text.Length > 50 ?
                    selectedQuestion.Text.Substring(0, 47) + "..." :
                    selectedQuestion.Text;
                lstQuestions.Items[index] = displayText;

                MessageBox.Show("Вопрос отредактирован!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnDeleteQuestion_Click(object sender, EventArgs e)
        {
            if (selectedQuestion != null && selectedLevel != null)
            {
                DialogResult dr = MessageBox.Show("Удалить вопрос?",
                    "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    selectedLevel.Questions.Remove(selectedQuestion);
                    lstQuestions.Items.RemoveAt(lstQuestions.SelectedIndex);
                    ClearQuestionForm();

                    MessageBox.Show("Вопрос удален!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnBrowseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            ofd.Title = "Выберите изображение";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string targetDir = Path.Combine(Application.StartupPath, "Images");
                if (!Directory.Exists(targetDir))
                    Directory.CreateDirectory(targetDir);

                string fileName = Path.GetFileName(ofd.FileName);
                string targetPath = Path.Combine(targetDir, fileName);
                File.Copy(ofd.FileName, targetPath, true);

                txtImagePath.Text = targetPath;
            }
        }

        private void BtnSaveAll_Click(object sender, EventArgs e)
        {
            try
            {
                XmlHelper.SaveTopics(topics);
                MessageBox.Show("Все изменения сохранены!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}