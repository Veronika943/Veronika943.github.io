using System;
using System.Drawing;
using System.Windows.Forms;

namespace PairGame
{
    public class FormSettings : Form
    {
        private NumericUpDown numericUpDownTime;
        private ComboBox comboBoxTheme;
        private Button buttonSave;
        private Button buttonCancel;

        public int GameTime { get; private set; }
        public string Theme { get; private set; }

        public FormSettings(int currentTime, string currentTheme)
        {
            GameTime = currentTime;
            Theme = currentTheme;
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "Настройки игры";
            this.Size = new Size(320, 200);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            int y = 20;

            // Время игры
            Label labelTime = new Label();
            labelTime.Text = "Время игры (секунд):";
            labelTime.Location = new Point(20, y);
            labelTime.Size = new Size(150, 25);

            numericUpDownTime = new NumericUpDown();
            numericUpDownTime.Location = new Point(180, y);
            numericUpDownTime.Size = new Size(100, 25);
            numericUpDownTime.Minimum = 30;
            numericUpDownTime.Maximum = 180;
            numericUpDownTime.Value = GameTime;

            y += 40;

            // Тема картинок
            Label labelTheme = new Label();
            labelTheme.Text = "Тема картинок:";
            labelTheme.Location = new Point(20, y);
            labelTheme.Size = new Size(150, 25);

            comboBoxTheme = new ComboBox();
            comboBoxTheme.Location = new Point(180, y);
            comboBoxTheme.Size = new Size(100, 25);
            comboBoxTheme.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxTheme.Items.AddRange(new string[] { "Animals", "Fruits", "Shapes" });
            comboBoxTheme.SelectedItem = Theme;

            y += 50;

            // Кнопки
            buttonSave = new Button();
            buttonSave.Text = "Сохранить";
            buttonSave.Location = new Point(80, y);
            buttonSave.Size = new Size(90, 30);
            buttonSave.Click += ButtonSave_Click;

            buttonCancel = new Button();
            buttonCancel.Text = "Отмена";
            buttonCancel.Location = new Point(190, y);
            buttonCancel.Size = new Size(90, 30);
            buttonCancel.Click += (s, e) => DialogResult = DialogResult.Cancel;

            // Добавляем элементы
            this.Controls.Add(labelTime);
            this.Controls.Add(numericUpDownTime);
            this.Controls.Add(labelTheme);
            this.Controls.Add(comboBoxTheme);
            this.Controls.Add(buttonSave);
            this.Controls.Add(buttonCancel);
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            GameTime = (int)numericUpDownTime.Value;
            Theme = comboBoxTheme.SelectedItem.ToString();
            DialogResult = DialogResult.OK;
        }
    }
}