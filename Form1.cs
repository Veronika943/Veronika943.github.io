using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        // Элементы интерфейса
        private TextBox txtX;
        private TextBox txtEpsilon;
        private Button btnCalculate;
        private Button btnClear;
        private Label lblResultLeft;
        private Label lblResultRight;
        private Label lblIterations;
        private Label lblX;
        private Label lblEpsilon;
        private Label lblLeftFunc;
        private Label lblRightFunc;
        private Label lblCount;
        private ErrorProvider errorProvider;

        public Form1()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            // Настройка формы
            this.Text = "Вариант 6: ln((x+1)/(x-1)) в ряд";
            this.Size = new System.Drawing.Size(550, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Создание элементов управления
            lblX = new Label()
            {
                Text = "x (|x| > 1):",
                Location = new System.Drawing.Point(20, 20),
                AutoSize = true
            };
            txtX = new TextBox()
            {
                Location = new System.Drawing.Point(120, 17),
                Width = 150
            };

            lblEpsilon = new Label()
            {
                Text = "Точность ε:",
                Location = new System.Drawing.Point(20, 60),
                AutoSize = true
            };
            txtEpsilon = new TextBox()
            {
                Location = new System.Drawing.Point(120, 57),
                Width = 150
            };

            btnCalculate = new Button()
            {
                Text = "Вычислить",
                Location = new System.Drawing.Point(120, 100),
                Width = 100,
                Enabled = false
            };
            btnClear = new Button()
            {
                Text = "Очистить",
                Location = new System.Drawing.Point(230, 100),
                Width = 100
            };

            lblLeftFunc = new Label()
            {
                Text = "Левая часть (Math.Log):",
                Location = new System.Drawing.Point(20, 150),
                AutoSize = true
            };
            lblResultLeft = new Label()
            {
                Text = "",
                Location = new System.Drawing.Point(180, 150),
                AutoSize = true,
                Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold)
            };

            lblRightFunc = new Label()
            {
                Text = "Правая часть (сумма ряда):",
                Location = new System.Drawing.Point(20, 180),
                AutoSize = true
            };
            lblResultRight = new Label()
            {
                Text = "",
                Location = new System.Drawing.Point(180, 180),
                AutoSize = true,
                Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold)
            };

            lblCount = new Label()
            {
                Text = "Количество членов ряда:",
                Location = new System.Drawing.Point(20, 210),
                AutoSize = true
            };
            lblIterations = new Label()
            {
                Text = "",
                Location = new System.Drawing.Point(180, 210),
                AutoSize = true,
                Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold)
            };

            errorProvider = new ErrorProvider();

            // Добавление элементов на форму
            this.Controls.AddRange(new Control[] {
                lblX, txtX,
                lblEpsilon, txtEpsilon,
                btnCalculate, btnClear,
                lblLeftFunc, lblResultLeft,
                lblRightFunc, lblResultRight,
                lblCount, lblIterations
            });

            // Подписка на события
            txtX.KeyPress += Txt_KeyPress;
            txtEpsilon.KeyPress += Txt_KeyPress;
            txtX.TextChanged += Txt_TextChanged;
            txtEpsilon.TextChanged += Txt_TextChanged;
            btnCalculate.Click += BtnCalculate_Click;
            btnClear.Click += BtnClear_Click;
        }

        // Задание 2: Проверка правильности ввода (KeyPress)
        private void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем цифры, запятую, минус и backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                e.KeyChar != ',' && e.KeyChar != '.' && e.KeyChar != '-')
            {
                e.Handled = true;
            }

            // Замена точки на запятую для корректного парсинга
            if (e.KeyChar == '.')
            {
                e.KeyChar = ',';
            }

            // Запрет на несколько минусов и запятых
            TextBox textBox = sender as TextBox;
            if (e.KeyChar == '-' && (textBox.SelectionStart != 0 || textBox.Text.Contains("-")))
            {
                e.Handled = true;
            }
            if (e.KeyChar == ',' && textBox.Text.Contains(","))
            {
                e.Handled = true;
            }
        }

        private void Txt_TextChanged(object sender, EventArgs e)
        {
            bool isValid = !string.IsNullOrWhiteSpace(txtX.Text) &&
                          !string.IsNullOrWhiteSpace(txtEpsilon.Text);

            if (isValid)
            {
                try
                {
                    double x = Convert.ToDouble(txtX.Text);
                    double eps = Convert.ToDouble(txtEpsilon.Text);

                    if (Math.Abs(x) <= 1)
                    {
                        errorProvider.SetError(txtX, "Ошибка: |x| должен быть больше 1");
                        isValid = false;
                    }

                    else if (eps <= 0 || eps >= 1)
                    {
                        errorProvider.SetError(txtEpsilon, "Точность должна быть в интервале (0, 1)");
                        isValid = false;
                    }
                    else
                    {
                        errorProvider.Clear();
                    }
                }
                catch
                {
                    errorProvider.SetError(txtX, "Некорректный формат числа");
                    isValid = false;
                }
            }

            btnCalculate.Enabled = isValid;
        }

        // Задание 1: Основные вычисления
        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                double x = Convert.ToDouble(txtX.Text);
                double epsilon = Convert.ToDouble(txtEpsilon.Text);

                // Левая часть - через встроенную функцию
                double leftValue = Math.Log((x + 1) / (x - 1));

                // Вычисление суммы ряда
                double sum = 0;
                int n = 0;
                double term;
                double previousTerm = 0;

                // Первый член ряда для n=0: 1/x
                term = 1.0 / x;

                do
                {
                    previousTerm = term;  // Запоминаем предыдущий член
                    sum += term;

                    n++;

                    if (n < 1000000) // Защита от бесконечного цикла
                    {
                        term = term / (x * x) * (2.0 * n - 1) / (2.0 * n + 1);
                    }

                    // Условие выхода: разность между соседними членами меньше epsilon
                } while (Math.Abs(term - previousTerm) > epsilon && n < 1000000);

                // Умножаем на 2 согласно формуле
                double rightValue = 2 * sum;

                // Вывод результатов
                lblResultLeft.Text = leftValue.ToString("F10");
                lblResultRight.Text = rightValue.ToString("F10");
                lblIterations.Text = n.ToString();

                // Проверка сходимости
                if (n >= 1000000)
                {
                    MessageBox.Show("Достигнуто максимальное количество итераций.\n" +
                        "Проверьте сходимость ряда или уменьшите точность.",
                        "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                // Задание 3.2: Обработка исключений
                MessageBox.Show($"Произошла ошибка: {ex.Message}\n\n" +
                    $"Тип ошибки: {ex.GetType().Name}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtX.Clear();
            txtEpsilon.Clear();
            lblResultLeft.Text = "";
            lblResultRight.Text = "";
            lblIterations.Text = "";
            errorProvider.Clear();
            btnCalculate.Enabled = false;
        }
    }
}