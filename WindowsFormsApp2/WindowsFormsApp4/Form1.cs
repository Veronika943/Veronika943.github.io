using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace task2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBoxOp.Items.Add("Сложение (+)");
            comboBoxOp.Items.Add("Вычитание (-)");
            comboBoxOp.Items.Add("Увеличить на 1");
            comboBoxOp.Items.Add("Уменьшить на 1");
            comboBoxOp.SelectedIndex = 0;
        }

        private void buttonCalc_Click(object sender, EventArgs e)
        {
            try
            {
                string inputA = textBoxA.Text.Trim();
                string inputB = textBoxB.Text.Trim();
                string result = "";
                if (comboBoxOp.SelectedIndex < 2 && string.IsNullOrEmpty(inputB))
                {
                    throw new ArgumentException("Для выбранной операции необходимо ввести второе число (Число B).");
                }

                switch (comboBoxOp.SelectedIndex)
                {
                    case 0: // Сложение
                        result = BigNumberLogic.Add(inputA, inputB);
                        break;
                    case 1: // Вычитание
                        result = BigNumberLogic.Subtract(inputA, inputB);
                        break;
                    case 2: // Увеличить на 1
                        result = BigNumberLogic.Increment(inputA);
                        break;
                    case 3: // Уменьшить на 1
                        result = BigNumberLogic.Decrement(inputA);
                        break;
                }

                labelResult.Text = "Результат: " + result;
                labelResult.ForeColor = System.Drawing.Color.Black;
            }
            catch (ArgumentException ex)
            {
                labelResult.Text = "Ошибка: " + ex.Message;
                labelResult.ForeColor = System.Drawing.Color.Red;
            }
            catch (Exception ex)
            {
                labelResult.Text = "Ошибка: " + ex.Message;
                labelResult.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBoxA.Clear();
            textBoxB.Clear();
            labelResult.Text = "Результат: ";
            comboBoxOp.SelectedIndex = 0;
            textBoxA.Focus();
        }
    }
}


