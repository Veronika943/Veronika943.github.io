using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            button1.Enabled = false; // Кнопка заблокирована изначально
            label3.Text = ""; // Очищаем label с результатами
            this.Text = "Вычисление функции разложением в ряд (Вариант 6)";
        }

        /// <summary>
        /// Значение математической функции (левая часть)
        /// ln((x+1)/(x-1))
        /// </summary>
        private double MathFunction(double x)
        {
            return Math.Log((x + 1) / (x - 1));
        }

        /// <summary>
        /// Вычисление суммы ряда (правая часть)
        /// 2 * ∑(1/((2n+1) * x^(2n+1))), n от 0 до ∞
        /// </summary>
        private double SeriesSum(double x, double eps, out int count)
        {
            // Первый член ряда при n=0: 2 * (1/x)
            double a = 2.0 / x;        // первый член (n=0)
            double sum = a;             // сумма начинается с первого члена
            int n = 0;                   // номер члена (начинаем с 0)
            count = 1;                   // количество просуммированных членов

            // Рекуррентная формула: a(n+1) = a(n) * (2n+1)/(2n+3) * 1/x²
            // Выводится из отношения (n+1)-го члена к n-му
            while (Math.Abs(a) >= eps * Math.Abs(sum) && count < 1000000)
            {
                // Вычисляем следующий член через предыдущий
                // a(n+1) = a(n) * (2n+1)/(2n+3) * 1/x²
                a = a * (2 * n + 1) / (2 * n + 3) / (x * x);
                sum += a;
                count++;
                n++;
            }

            return sum;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Чтение данных из полей ввода
                double x = Convert.ToDouble(textBox2.Text);
                double eps = Convert.ToDouble(textBox1.Text);

                // Проверка области допустимых значений
                // Для функции ln((x+1)/(x-1)): |x| > 1
                if (Math.Abs(x) <= 1)
                {
                    label3.Text = "Ошибка: |x| должен быть > 1";
                    return;
                }

                // Проверка точности: 0 < eps < 1
                if (eps <= 0 || eps >= 1)
                {
                    label3.Text = "Ошибка: точность должна быть 0 < eps < 1";
                    return;
                }

                // Вычисление значений
                double mathResult = MathFunction(x);
                int count;
                double seriesResult = SeriesSum(x, eps, out count);

                // Вывод результатов
                label3.Text = $"Математическая функция ln(({x}+1)/({x}-1)) = {mathResult:F10}\n" +
                              $"Сумма ряда = {seriesResult:F10}\n" +
                              $"Количество просуммированных членов = {count}\n" +
                              $"Погрешность = {Math.Abs(mathResult - seriesResult):E2}";
            }
            catch (FormatException)
            {
                label3.Text = "Ошибка: некорректный формат числа. Используйте запятую для дробной части.";
            }
            catch (DivideByZeroException)
            {
                label3.Text = "Ошибка: деление на ноль. Проверьте значение x.";
            }
            catch (Exception ex)
            {
                label3.Text = $"Ошибка: {ex.Message}";
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем: цифры, backspace, запятую (для дробной части)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true; // Запрещаем ввод
            }

            // Разрешаем только одну запятую
            if (e.KeyChar == ',' && (sender as TextBox).Text.Contains(','))
            {
                e.Handled = true;
            }

            // Для точности не разрешаем минус (точность всегда положительная)
            if (e.KeyChar == '-')
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем: цифры, backspace, запятую, минус
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                e.KeyChar != ',' && e.KeyChar != '-')
            {
                e.Handled = true;
            }

            // Разрешаем только одну запятую
            if (e.KeyChar == ',' && (sender as TextBox).Text.Contains(','))
            {
                e.Handled = true;
            }

            // Минус только в начале и только один
            if (e.KeyChar == '-' && ((sender as TextBox).Text.Length > 0 ||
                (sender as TextBox).SelectionStart > 0))
            {
                e.Handled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            CheckFields();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            CheckFields();
        }
        private void CheckFields()
        {
            // Проверяем, что поля не пустые и не содержат только служебные символы
            bool isValid1 = !string.IsNullOrWhiteSpace(textBox1.Text) &&
                           textBox1.Text != "," && textBox1.Text != "-";

            bool isValid2 = !string.IsNullOrWhiteSpace(textBox2.Text) &&
                           textBox2.Text != "-" && textBox2.Text != "," &&
                           textBox2.Text != "-," && textBox2.Text != ",-";

            button1.Enabled = isValid1 && isValid2;
        }
    }
}
