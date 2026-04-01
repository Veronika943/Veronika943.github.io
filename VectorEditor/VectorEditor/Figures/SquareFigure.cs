using System;
using System.Drawing;

using VectorEditor.Utils;

namespace VectorEditor.Figures
{
    /// <summary>
    /// Класс квадрата
    /// </summary>
    [Serializable]
    public class SquareFigure : Figure
    {
        public SquareFigure() : base()
        {
            // Квадрат: ширина равна высоте
            Bounds = new Rectangle(100, 100, 100, 100);
        }

        public SquareFigure(Rectangle bounds, Stroke stroke) : base(bounds, stroke)
        {
            // Приводим к квадрату
            int size = Math.Min(bounds.Width, bounds.Height);
            Bounds = new Rectangle(bounds.X, bounds.Y, size, size);
        }

        public override void Draw(Graphics g)
        {
            using (Pen pen = Stroke.CreatePen())
            {
                g.DrawRectangle(pen, Bounds);
            }
        }

        public override bool HitTest(Point point)
        {
            Rectangle expandedBounds = new Rectangle(
                Bounds.X - (int)Stroke.Width,
                Bounds.Y - (int)Stroke.Width,
                Bounds.Width + (int)Stroke.Width * 2,
                Bounds.Height + (int)Stroke.Width * 2);

            return expandedBounds.Contains(point);
        }

        public override void FlipHorizontally() { }

        public override void FlipVertically() { }

        public override void Rotate90()
        {
            // Квадрат не меняет форму при повороте
        }

        public override void Resize(int width, int height)
        {
            int size = Math.Min(width, height);
            Bounds = new Rectangle(Bounds.X, Bounds.Y, size, size);
        }

        public override Figure Clone()
        {
            return new SquareFigure(Bounds, Stroke);
        }
    }
}