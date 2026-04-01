using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using VectorEditor.Utils;

namespace VectorEditor.Figures
{
    /// <summary>
    /// Г-образная фигура (состоит из двух прямоугольников)
    /// </summary>
    [Serializable]
    public class LShapeFigure : Figure
    {
        // Для сериализации храним параметры частей
        private Rectangle _part1;
        private Rectangle _part2;

        public LShapeFigure() : base()
        {
            InitializeParts();
        }

        public LShapeFigure(Rectangle bounds, Stroke stroke) : base(bounds, stroke)
        {
            InitializeParts();
        }

        private void InitializeParts()
        {
            int width = Bounds.Width;
            int height = Bounds.Height;
            int partWidth = width / 2;
            int partHeight = height / 2;

            // Вертикальная часть (левая)
            _part1 = new Rectangle(Bounds.X, Bounds.Y, partWidth, height);
            // Горизонтальная часть (нижняя)
            _part2 = new Rectangle(Bounds.X + partWidth, Bounds.Y + partHeight, partWidth, partHeight);
        }

        private void UpdateParts()
        {
            int width = Bounds.Width;
            int height = Bounds.Height;
            int partWidth = width / 2;
            int partHeight = height / 2;

            _part1 = new Rectangle(Bounds.X, Bounds.Y, partWidth, height);
            _part2 = new Rectangle(Bounds.X + partWidth, Bounds.Y + partHeight, partWidth, partHeight);
        }

        public override void Draw(Graphics g)
        {
            UpdateParts();
            using (Pen pen = Stroke.CreatePen())
            {
                g.DrawRectangle(pen, _part1);
                g.DrawRectangle(pen, _part2);
            }
        }

        public override bool HitTest(Point point)
        {
            UpdateParts();
            // Проверяем попадание в любую из частей
            Rectangle expanded1 = new Rectangle(
                _part1.X - (int)Stroke.Width,
                _part1.Y - (int)Stroke.Width,
                _part1.Width + (int)Stroke.Width * 2,
                _part1.Height + (int)Stroke.Width * 2);

            Rectangle expanded2 = new Rectangle(
                _part2.X - (int)Stroke.Width,
                _part2.Y - (int)Stroke.Width,
                _part2.Width + (int)Stroke.Width * 2,
                _part2.Height + (int)Stroke.Width * 2);

            return expanded1.Contains(point) || expanded2.Contains(point);
        }

        public override void Move(int dx, int dy)
        {
            base.Move(dx, dy);
            UpdateParts();
        }

        public override void MoveTo(Point newLocation)
        {
            base.MoveTo(newLocation);
            UpdateParts();
        }

        public override void Resize(int width, int height)
        {
            base.Resize(width, height);
            UpdateParts();
        }

        public override void FlipHorizontally()
        {
            // Отражение по горизонтали
            int newX = Bounds.X;
            int newY = Bounds.Y;
            // Меняем структуру фигуры
            int partWidth = Bounds.Width / 2;
            int partHeight = Bounds.Height / 2;

            _part1 = new Rectangle(newX + partWidth, newY, partWidth, Bounds.Height);
            _part2 = new Rectangle(newX, newY + partHeight, partWidth, partHeight);

            UpdateBoundsFromParts();
        }

        public override void FlipVertically()
        {
            // Отражение по вертикали
            int newX = Bounds.X;
            int newY = Bounds.Y;
            int partWidth = Bounds.Width / 2;
            int partHeight = Bounds.Height / 2;

            _part1 = new Rectangle(newX, newY + partHeight, partWidth, partHeight);
            _part2 = new Rectangle(newX + partWidth, newY, partWidth, Bounds.Height);

            UpdateBoundsFromParts();
        }

        public override void Rotate90()
        {
            // Поворот на 90 градусов
            int newWidth = Bounds.Height;
            int newHeight = Bounds.Width;
            int newX = Bounds.X + (Bounds.Width - newWidth) / 2;
            int newY = Bounds.Y + (Bounds.Height - newHeight) / 2;

            Bounds = new Rectangle(newX, newY, newWidth, newHeight);
            UpdateParts();
        }

        private void UpdateBoundsFromParts()
        {
            int minX = Math.Min(_part1.X, _part2.X);
            int minY = Math.Min(_part1.Y, _part2.Y);
            int maxX = Math.Max(_part1.X + _part1.Width, _part2.X + _part2.Width);
            int maxY = Math.Max(_part1.Y + _part1.Height, _part2.Y + _part2.Height);

            Bounds = new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        public override Figure Clone()
        {
            return new LShapeFigure(Bounds, Stroke);
        }
    }
}