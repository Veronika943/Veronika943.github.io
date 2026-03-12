using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        private Rectangle rect;
        private int rectWidth = 40;
        private int rectHeight = 60;
        private int step = 5; 
        private int direction = 1; 
        private Color rectColor = Color.Blue;
        private Timer timer;

        public Form1()
        {
            InitializeComponent();

            this.Text = "Вариант 6: Движение прямоугольника";
            this.Size = new Size(800, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true; // Убирает мерцание
            this.KeyPreview = true; 

            timer = new Timer();
            timer.Interval = 50;
            timer.Tick += Timer_Tick;
            timer.Start();

            int startY = (this.ClientSize.Height - rectHeight) / 2;
            rect = new Rectangle(50, startY, rectWidth, rectHeight);

            // Подписка на события
            this.Paint += Form1_Paint;
            this.Resize += Form1_Resize;
            this.KeyDown += Form1_KeyDown;
            this.FormClosing += Form1_FormClosing;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            rect.X += step * direction;

            if (rect.Right >= this.ClientSize.Width)
            {

                rect.X = this.ClientSize.Width - rect.Width;
                direction = -1; 
                FlipRectangle();
                ChangeColor();
            }
            else if (rect.Left <= 0)
            {
                // Достигли левого края
                rect.X = 0;
                direction = 1; // Меняем направление направо
                FlipRectangle();
                ChangeColor();
            }

            Invalidate();
        }

        private void FlipRectangle()
        {

            int temp = rect.Width;
            rect.Width = rect.Height;
            rect.Height = temp;

            if (rect.Bottom >= this.ClientSize.Height)
            {
                rect.Y = this.ClientSize.Height - rect.Height;
            }
            if (rect.Y < 0)
            {
                rect.Y = 0;
            }
        }

        private void ChangeColor()
        {
            Random rand = new Random();
            rectColor = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            using (SolidBrush brush = new SolidBrush(rectColor))
            {
                e.Graphics.FillRectangle(brush, rect);
            }

            // Рисуем контур
            using (Pen pen = new Pen(Color.Black, 2))
            {
                e.Graphics.DrawRectangle(pen, rect);
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (rect.Right >= this.ClientSize.Width)
            {
                rect.X = this.ClientSize.Width - rect.Width;
            }
            if (rect.Bottom >= this.ClientSize.Height)
            {
                rect.Y = this.ClientSize.Height - rect.Height;
            }
            Invalidate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            timer.Stop();
            this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
        }
    }
}