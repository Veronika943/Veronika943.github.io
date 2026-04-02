using System;
using System.Drawing;
using System.Windows.Forms;

namespace PairGame
{
    public class FormAuth : Form
    {
        private TextBox textBoxLogin;
        private Button buttonOK;
        private Button buttonCancel;

        public string Login { get; private set; }

        public FormAuth()
        {
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "Авторизация";
            this.Size = new Size(300, 150);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Label
            Label label = new Label();
            label.Text = "Введите логин:";
            label.Location = new Point(20, 20);
            label.Size = new Size(100, 25);

            // TextBox
            textBoxLogin = new TextBox();
            textBoxLogin.Location = new Point(20, 50);
            textBoxLogin.Size = new Size(240, 25);

            // Кнопка OK
            buttonOK = new Button();
            buttonOK.Text = "OK";
            buttonOK.Location = new Point(100, 90);
            buttonOK.Size = new Size(75, 25);
            buttonOK.Click += ButtonOK_Click;

            // Кнопка Cancel
            buttonCancel = new Button();
            buttonCancel.Text = "Отмена";
            buttonCancel.Location = new Point(185, 90);
            buttonCancel.Size = new Size(75, 25);
            buttonCancel.Click += (s, e) => DialogResult = DialogResult.Cancel;

            // Добавляем элементы
            this.Controls.Add(label);
            this.Controls.Add(textBoxLogin);
            this.Controls.Add(buttonOK);
            this.Controls.Add(buttonCancel);
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxLogin.Text))
            {
                Login = textBoxLogin.Text.Trim();
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Введите логин!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}