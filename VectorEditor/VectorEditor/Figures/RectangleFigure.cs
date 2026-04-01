using System;
using System.Drawing;
using VectorEditor.Utils;

namespace VectorEditor.Figures
{
    /// <summary>
    /// Класс прямоугольника
    /// </summary>
    [Serializable]
    public class RectangleFigure : Figure
    {
        public RectangleFigure() : base() { }

        public RectangleFigure(Rectangle bounds, Stroke stroke) : base(bounds, stroke) { }

        public override void Draw(Graphics g)
        {
            using (Pen pen = Stroke.CreatePen())
            {
                g.DrawRectangle(pen, Bounds);
            }
        }

        public override bool HitTest(Point point)
        {
            // Проверка попадания в контур (с учётом толщины линии)
            Rectangle expandedBounds = new Rectangle(
                Bounds.X - (int)Stroke.Width,
                Bounds.Y - (int)Stroke.Width,
                Bounds.Width + (int)Stroke.Width * 2,
                Bounds.Height + (int)Stroke.Width * 2);

            return expandedBounds.Contains(point);
        }

        public override void FlipHorizontally()
        {
            // Для прямоугольника отражение не меняет форму
        }

        public override void FlipVertically()
        {
            // Для прямоугольника отражение не меняет форму
        }

        public override void Rotate90()
        {
            // Меняем ширину и высоту местами
            Bounds = new Rectangle(Bounds.X, Bounds.Y, Bounds.Height, Bounds.Width);
        }

        public override Figure Clone()
        {
            return new RectangleFigure(Bounds, Stroke);
        }
    }
}