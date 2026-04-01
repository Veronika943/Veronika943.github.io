using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using VectorEditor.Utils;

namespace VectorEditor.Figures
{
    /// <summary>
    /// П-образная фигура (состоит из трёх прямоугольников)
    /// </summary>
    [Serializable]
    public class UShapeFigure : Figure
    {
        private Rectangle _leftPart;
        private Rectangle _rightPart;
        private Rectangle _bottomPart;

        public UShapeFigure() : base()
        {
            InitializeParts();
        }

        public UShapeFigure(Rectangle bounds, Stroke stroke) : base(bounds, stroke)
        {
            InitializeParts();
        }

        private void InitializeParts()
        {
            int width = Bounds.Width;
            int height = Bounds.Height;
            int partWidth = width / 3;
            int partHeight = height / 3;

            // Левая вертикальная часть
            _leftPart = new Rectangle(Bounds.X, Bounds.Y, partWidth, height);
            // Правая вертикальная часть
            _rightPart = new Rectangle(Bounds.X + width - partWidth, Bounds.Y, partWidth, height);
            // Нижняя горизонтальная часть
            _bottomPart = new Rectangle(Bounds.X + partWidth, Bounds.Y + height - partHeight,
                width - 2 * partWidth, partHeight);
        }

        private void UpdateParts()
        {
            int width = Bounds.Width;
            int height = Bounds.Height;
            int partWidth = width / 3;
            int partHeight = height / 3;

            _leftPart = new Rectangle(Bounds.X, Bounds.Y, partWidth, height);
            _rightPart = new Rectangle(Bounds.X + width - partWidth, Bounds.Y, partWidth, height);
            _bottomPart = new Rectangle(Bounds.X + partWidth, Bounds.Y + height - partHeight,
                width - 2 * partWidth, partHeight);
        }

        public override void Draw(Graphics g)
        {
            UpdateParts();
            using (Pen pen = Stroke.CreatePen())
            {
                g.DrawRectangle(pen, _leftPart);
                g.DrawRectangle(pen, _rightPart);
                g.DrawRectangle(pen, _bottomPart);
            }
        }

        public override bool HitTest(Point point)
        {
            UpdateParts();
            Rectangle expandedLeft = ExpandRect(_leftPart);
            Rectangle expandedRight = ExpandRect(_rightPart);
            Rectangle expandedBottom = ExpandRect(_bottomPart);

            return expandedLeft.Contains(point) || expandedRight.Contains(point) || expandedBottom.Contains(point);
        }

        private Rectangle ExpandRect(Rectangle rect)
        {
            return new Rectangle(
                rect.X - (int)Stroke.Width,
                rect.Y - (int)Stroke.Width,
                rect.Width + (int)Stroke.Width * 2,
                rect.Height + (int)Stroke.Width * 2);
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
            int partWidth = Bounds.Width / 3;

            _leftPart = new Rectangle(newX + Bounds.Width - partWidth, Bounds.Y, partWidth, Bounds.Height);
            _rightPart = new Rectangle(newX, Bounds.Y, partWidth, Bounds.Height);

            UpdateBoundsFromParts();
        }

        public override void FlipVertically()
        {
            // Отражение по вертикали
            int newY = Bounds.Y;
            int partHeight = Bounds.Height / 3;

            _leftPart = new Rectangle(Bounds.X, newY + Bounds.Height - partHeight, Bounds.Width / 3, partHeight);
            _rightPart = new Rectangle(Bounds.X + 2 * Bounds.Width / 3, newY + Bounds.Height - partHeight,
                Bounds.Width / 3, partHeight);
            _bottomPart = new Rectangle(Bounds.X + Bounds.Width / 3, newY, Bounds.Width / 3, partHeight);

            UpdateBoundsFromParts();
        }

        public override void Rotate90()
        {
            int newWidth = Bounds.Height;
            int newHeight = Bounds.Width;
            int newX = Bounds.X + (Bounds.Width - newWidth) / 2;
            int newY = Bounds.Y + (Bounds.Height - newHeight) / 2;

            Bounds = new Rectangle(newX, newY, newWidth, newHeight);
            UpdateParts();
        }

        private void UpdateBoundsFromParts()
        {
            int minX = Math.Min(_leftPart.X, Math.Min(_rightPart.X, _bottomPart.X));
            int minY = Math.Min(_leftPart.Y, Math.Min(_rightPart.Y, _bottomPart.Y));
            int maxX = Math.Max(_leftPart.X + _leftPart.Width,
                Math.Max(_rightPart.X + _rightPart.Width, _bottomPart.X + _bottomPart.Width));
            int maxY = Math.Max(_leftPart.Y + _leftPart.Height,
                Math.Max(_rightPart.Y + _rightPart.Height, _bottomPart.Y + _bottomPart.Height));

            Bounds = new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        public override Figure Clone()
        {
            return new UShapeFigure(Bounds, Stroke);
        }
    }
}