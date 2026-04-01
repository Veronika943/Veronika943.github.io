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
    public class MainForm : Form
    {
        private MenuStrip mainMenu;
        private ToolStripMenuItem fileMenu;
        private ToolStripMenuItem startTestItem;
        private ToolStripMenuItem exitItem;
        private ToolStripMenuItem adminMenu;
        private ToolStripMenuItem adminPanelItem;
        private ToolStripMenuItem helpMenu;
        private ToolStripMenuItem aboutItem;

        private Panel mainPanel;
        private Label lblTitle;
        private Label lblWelcome;
        private ComboBox cmbTopics;
        private Label lblTopic;
        private Button btnStartTest;
        private Button btnAdmin;
        private Label lblInfo;
        private PictureBox pictureBox;

        private List<Topic> topics;

        public MainForm()
        {
            InitializeComponent();
            LoadTopics();
        }

        private void InitializeComponent()
        {
            this.Text = "Русские загадки и обычаи - Викторина";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Beige;

            // Меню
            mainMenu = new MenuStrip();

            fileMenu = new ToolStripMenuItem("Файл");
            startTestItem = new ToolStripMenuItem("Начать тестирование");
            startTestItem.Click += StartTestItem_Click;
            exitItem = new ToolStripMenuItem("Выход");
            exitItem.Click += (s, e) => Application.Exit();
            fileMenu.DropDownItems.Add(startTestItem);
            fileMenu.DropDownItems.Add(new ToolStripMenuItem("-"));
            fileMenu.DropDownItems.Add(exitItem);

            adminMenu = new ToolStripMenuItem("Администрирование");
            adminPanelItem = new ToolStripMenuItem("Панель администратора");
            adminPanelItem.Click += AdminPanelItem_Click;
            adminMenu.DropDownItems.Add(adminPanelItem);

            helpMenu = new ToolStripMenuItem("Справка");
            aboutItem = new ToolStripMenuItem("О программе");
            aboutItem.Click += AboutItem_Click;
            helpMenu.DropDownItems.Add(aboutItem);

            mainMenu.Items.Add(fileMenu);
            mainMenu.Items.Add(adminMenu);
            mainMenu.Items.Add(helpMenu);

            // Основная панель
            mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Padding = new Padding(20);

            // Заголовок
            lblTitle = new Label();
            lblTitle.Text = "Русские загадки и обычаи";
            lblTitle.Font = new Font("Times New Roman", 24, FontStyle.Bold);
            lblTitle.ForeColor = Color.DarkRed;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Height = 80;

            // Приветствие
            lblWelcome = new Label();
            lblWelcome.Text = "Добро пожаловать в викторину!\nПроверьте свои знания о русских загадках, традициях и обычаях.";
            lblWelcome.Font = new Font("Arial", 12);
            lblWelcome.TextAlign = ContentAlignment.MiddleCenter;
            lblWelcome.Dock = DockStyle.Top;
            lblWelcome.Height = 80;

            // Картинка
            pictureBox = new PictureBox();
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Height = 200;
            pictureBox.Dock = DockStyle.Top;
            pictureBox.Image = CreatePlaceholderImage();

            // Выбор темы
            lblTopic = new Label();
            lblTopic.Text = "Выберите тему:";
            lblTopic.Font = new Font("Arial", 12, FontStyle.Bold);
            lblTopic.Dock = DockStyle.Top;
            lblTopic.Height = 30;

            cmbTopics = new ComboBox();
            cmbTopics.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTopics.Dock = DockStyle.Top;
            cmbTopics.Height = 30;
            cmbTopics.SelectedIndexChanged += CmbTopics_SelectedIndexChanged;

            // Информация о теме
            lblInfo = new Label();
            lblInfo.Font = new Font("Arial", 10);
            lblInfo.ForeColor = Color.Gray;
            lblInfo.Dock = DockStyle.Top;
            lblInfo.Height = 60;
            lblInfo.Text = "";

            // Кнопки
            btnStartTest = new Button();
            btnStartTest.Text = "Начать тестирование";
            btnStartTest.Font = new Font("Arial", 12, FontStyle.Bold);
            btnStartTest.BackColor = Color.DarkGreen;
            btnStartTest.ForeColor = Color.White;
            btnStartTest.Size = new Size(200, 50);
            btnStartTest.Location = new Point(300, 450);
            btnStartTest.Click += BtnStartTest_Click;

            btnAdmin = new Button();
            btnAdmin.Text = "Панель администратора";
            btnAdmin.Font = new Font("Arial", 10);
            btnAdmin.Size = new Size(150, 35);
            btnAdmin.Location = new Point(625, 520);
            btnAdmin.Click += AdminPanelItem_Click;

            // Добавление элементов
            mainPanel.Controls.Add(btnAdmin);
            mainPanel.Controls.Add(btnStartTest);
            mainPanel.Controls.Add(lblInfo);
            mainPanel.Controls.Add(cmbTopics);
            mainPanel.Controls.Add(lblTopic);
            mainPanel.Controls.Add(pictureBox);
            mainPanel.Controls.Add(lblWelcome);
            mainPanel.Controls.Add(lblTitle);

            this.Controls.Add(mainPanel);
            this.Controls.Add(mainMenu);
        }

        private Bitmap CreatePlaceholderImage()
        {
            Bitmap bmp = new Bitmap(400, 200);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.LightYellow);
                g.DrawRectangle(Pens.Brown, 10, 10, 380, 180);
                using (Font font = new Font("Arial", 14))
                {
                    g.DrawString("Русские загадки", font, Brushes.DarkRed, 120, 80);
                    g.DrawString("и обычаи", font, Brushes.DarkRed, 150, 110);
                }
            }
            return bmp;
        }

        private void LoadTopics()
        {
            try
            {
                string dataDir = Path.Combine(Application.StartupPath, "Data");
                if (!Directory.Exists(dataDir))
                    Directory.CreateDirectory(dataDir);

                string xmlPath = Path.Combine(dataDir, "questions.xml");

                if (!File.Exists(xmlPath))
                {
                    CreateDemoXml(xmlPath);
                }

                XmlHelper.Initialize(xmlPath);
                topics = XmlHelper.LoadTopics();

                cmbTopics.Items.Clear();
                foreach (var topic in topics)
                {
                    cmbTopics.Items.Add(topic.Name);
                }

                if (cmbTopics.Items.Count > 0)
                    cmbTopics.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateDemoXml(string filePath)
        {
            string xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<quiz>
  <topics>
    <topic id=""1"" name=""Русские загадки"">
      <description>Загадки русского народа о природе, животных и быте</description>
      <levels>
        <level id=""1"" name=""Легкий"">
          <questions>
            <question id=""1"" text=""Сидит дед, в сто шуб одет, кто его раздевает, тот слезы проливает"" image="""">
              <answer right=""true"">Лук</answer>
              <answer right=""false"">Чеснок</answer>
              <answer right=""false"">Картофель</answer>
              <answer right=""false"">Капуста</answer>
            </question>
            <question id=""2"" text=""Зимой и летом одним цветом"" image="""">
              <answer right=""true"">Елка</answer>
              <answer right=""false"">Сосна</answer>
              <answer right=""false"">Кедр</answer>
              <answer right=""false"">Пихта</answer>
            </question>
            <question id=""3"" text=""Не лает, не кусает, а в дом не пускает"" image="""">
              <answer right=""true"">Замок</answer>
              <answer right=""false"">Собака</answer>
              <answer right=""false"">Охрана</answer>
              <answer right=""false"">Дверь</answer>
            </question>
          </questions>
        </level>
        <level id=""2"" name=""Средний"">
          <questions>
            <question id=""4"" text=""Что можно приготовить, но нельзя съесть?"" image="""">
              <answer right=""true"">Уроки</answer>
              <answer right=""false"">Обед</answer>
              <answer right=""false"">Завтрак</answer>
              <answer right=""false"">Ужин</answer>
            </question>
          </questions>
        </level>
        <level id=""3"" name=""Сложный"">
          <questions>
            <question id=""5"" text=""Какое слово начинается с трех букв «г» и заканчивается тремя буквами «я»?"" image="""">
              <answer right=""true"">Тригонометрия</answer>
              <answer right=""false"">География</answer>
              <answer right=""false"">Грамматика</answer>
              <answer right=""false"">Геометрия</answer>
            </question>
          </questions>
        </level>
      </levels>
    </topic>
    <topic id=""2"" name=""Русские обычаи и традиции"">
      <description>Традиции и обычаи русского народа</description>
      <levels>
        <level id=""1"" name=""Легкий"">
          <questions>
            <question id=""6"" text=""Как называется русский праздник проводов зимы?"" image="""">
              <answer right=""true"">Масленица</answer>
              <answer right=""false"">Пасха</answer>
              <answer right=""false"">Рождество</answer>
              <answer right=""false"">Иван Купала</answer>
            </question>
          </questions>
        </level>
        <level id=""2"" name=""Средний"">
          <questions>
            <question id=""7"" text=""В какой праздник принято красить яйца?"" image="""">
              <answer right=""true"">Пасха</answer>
              <answer right=""false"">Рождество</answer>
              <answer right=""false"">Троица</answer>
              <answer right=""false"">Крещение</answer>
            </question>
          </questions>
        </level>
        <level id=""3"" name=""Сложный"">
          <questions>
            <question id=""8"" text=""Как назывался обряд, когда жених похищал невесту?"" image="""">
              <answer right=""true"">Умыкание</answer>
              <answer right=""false"">Сватовство</answer>
              <answer right=""false"">Сговор</answer>
              <answer right=""false"">Помолвка</answer>
            </question>
          </questions>
        </level>
      </levels>
    </topic>
  </topics>
</quiz>";

            File.WriteAllText(filePath, xmlContent);
        }

        private void CmbTopics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTopics.SelectedIndex >= 0 && topics != null)
            {
                var topic = topics[cmbTopics.SelectedIndex];
                lblInfo.Text = topic.Description;
            }
        }

        private void BtnStartTest_Click(object sender, EventArgs e)
        {
            if (cmbTopics.SelectedIndex >= 0 && topics != null)
            {
                var topic = topics[cmbTopics.SelectedIndex];
                var testForm = new TestForm(topic);
                testForm.ShowDialog();
            }
        }

        private void StartTestItem_Click(object sender, EventArgs e)
        {
            BtnStartTest_Click(sender, e);
        }

        private void AdminPanelItem_Click(object sender, EventArgs e)
        {
            var adminForm = new AdminForm();
            adminForm.ShowDialog();
            LoadTopics();
        }

        private void AboutItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Викторина 'Русские загадки и обычаи'\n\n" +
                "Версия 1.0\n\n" +
                "Программа для проверки знаний о русских загадках,\n" +
                "традициях и обычаях русского народа.\n\n" +
                "© 2024",
                "О программе",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}