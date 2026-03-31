using System;
using System.Drawing;
using System.Windows.Forms;

namespace RussianTraditionsQuiz
{
    public partial class MainForm : Form
    {
        private Label lblTitle;
        private Label lblDescription;
        private Button btnStart;
        private Button btnAdmin;
        private Button btnExit;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Настройка формы
            this.Text = "Викторина 'Загадки и обычаи на Руси'";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            // Создание заголовка
            lblTitle = new Label();
            lblTitle.Text = "Добро пожаловать в викторину!";
            lblTitle.Font = new Font("Arial", 24, FontStyle.Bold);
            lblTitle.ForeColor = Color.DarkRed;
            lblTitle.Location = new Point(250, 50);
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.Transparent;

            // Создание описания
            lblDescription = new Label();
            lblDescription.Text = "Проверьте свои знания о русских традициях, загадках и обычаях";
            lblDescription.Font = new Font("Arial", 12, FontStyle.Regular);
            lblDescription.ForeColor = Color.Brown;
            lblDescription.Location = new Point(150, 120);
            lblDescription.AutoSize = true;
            lblDescription.BackColor = Color.Transparent;

            // Создание кнопки "Начать тестирование"
            btnStart = new Button();
            btnStart.Text = "Начать тестирование";
            btnStart.Font = new Font("Arial", 12, FontStyle.Bold);
            btnStart.Location = new Point(300, 250);
            btnStart.Size = new Size(200, 50);
            btnStart.BackColor = Color.LightGreen;
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.Cursor = Cursors.Hand;
            btnStart.Click += new EventHandler(btnStart_Click);

            // Создание кнопки "Панель администратора"
            btnAdmin = new Button();
            btnAdmin.Text = "Панель администратора";
            btnAdmin.Font = new Font("Arial", 12, FontStyle.Bold);
            btnAdmin.Location = new Point(300, 320);
            btnAdmin.Size = new Size(200, 50);
            btnAdmin.BackColor = Color.LightBlue;
            btnAdmin.FlatStyle = FlatStyle.Flat;
            btnAdmin.Cursor = Cursors.Hand;
            btnAdmin.Click += new EventHandler(btnAdmin_Click);

            // Создание кнопки "Выход"
            btnExit = new Button();
            btnExit.Text = "Выход";
            btnExit.Font = new Font("Arial", 12, FontStyle.Bold);
            btnExit.Location = new Point(300, 390);
            btnExit.Size = new Size(200, 50);
            btnExit.BackColor = Color.LightCoral;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Cursor = Cursors.Hand;
            btnExit.Click += new EventHandler(btnExit_Click);

            // Добавление элементов на форму
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblDescription);
            this.Controls.Add(btnStart);
            this.Controls.Add(btnAdmin);
            this.Controls.Add(btnExit);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            SelectTopicForm selectTopicForm = new SelectTopicForm();
            selectTopicForm.ShowDialog();
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            AdminForm adminForm = new AdminForm();
            adminForm.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите выйти?",
                "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        // Удаляем переопределение Dispose, если оно есть
        // protected override void Dispose(bool disposing) - удалите эту строку, если она есть
    }
}