using System.Drawing;

namespace VectorEditor.Utils
{
    /// <summary>
    /// Класс для отрисовки маркеров выделения фигуры
    /// </summary>
    public static class SelectionMarkers
    {
        private const int MarkerSize = 6;
        private static readonly Pen MarkerPen = new Pen(Color.Blue, 2);
        private static readonly SolidBrush MarkerBrush = new SolidBrush(Color.White);

        /// <summary>
        /// Отрисовка маркеров вокруг прямоугольной области
        /// </summary>
        public static void DrawMarkers(Graphics g, Rectangle bounds)
        {
            // Восемь маркеров по углам и серединам сторон
            Point[] markers = {
                new Point(bounds.Left - MarkerSize/2, bounds.Top - MarkerSize/2),     // левый верхний
                new Point(bounds.Left + bounds.Width/2 - MarkerSize/2, bounds.Top - MarkerSize/2), // верхний центр
                new Point(bounds.Right - MarkerSize/2, bounds.Top - MarkerSize/2),    // правый верхний
                new Point(bounds.Right - MarkerSize/2, bounds.Top + bounds.Height/2 - MarkerSize/2), // правый центр
                new Point(bounds.Right - MarkerSize/2, bounds.Bottom - MarkerSize/2), // правый нижний
                new Point(bounds.Left + bounds.Width/2 - MarkerSize/2, bounds.Bottom - MarkerSize/2), // нижний центр
                new Point(bounds.Left - MarkerSize/2, bounds.Bottom - MarkerSize/2),  // левый нижний
                new Point(bounds.Left - MarkerSize/2, bounds.Top + bounds.Height/2 - MarkerSize/2)   // левый центр
            };

            // Рисуем прямоугольную рамку
            g.DrawRectangle(new Pen(Color.Blue, 1), bounds);

            // Рисуем маркеры
            foreach (var point in markers)
            {
                g.FillRectangle(MarkerBrush, point.X, point.Y, MarkerSize, MarkerSize);
                g.DrawRectangle(MarkerPen, point.X, point.Y, MarkerSize, MarkerSize);
            }
        }

        /// <summary>
        /// Проверка, попал ли клик в маркер
        /// </summary>
        public static int HitTest(Point clickPoint, Rectangle bounds)
        {
            Point[] markers = {
                new Point(bounds.Left - MarkerSize/2, bounds.Top - MarkerSize/2),
                new Point(bounds.Left + bounds.Width/2 - MarkerSize/2, bounds.Top - MarkerSize/2),
                new Point(bounds.Right - MarkerSize/2, bounds.Top - MarkerSize/2),
                new Point(bounds.Right - MarkerSize/2, bounds.Top + bounds.Height/2 - MarkerSize/2),
                new Point(bounds.Right - MarkerSize/2, bounds.Bottom - MarkerSize/2),
                new Point(bounds.Left + bounds.Width/2 - MarkerSize/2, bounds.Bottom - MarkerSize/2),
                new Point(bounds.Left - MarkerSize/2, bounds.Bottom - MarkerSize/2),
                new Point(bounds.Left - MarkerSize/2, bounds.Top + bounds.Height/2 - MarkerSize/2)
            };

            for (int i = 0; i < markers.Length; i++)
            {
                Rectangle markerRect = new Rectangle(markers[i].X, markers[i].Y, MarkerSize, MarkerSize);
                if (markerRect.Contains(clickPoint))
                    return i;
            }
            return -1;
        }
    }
}