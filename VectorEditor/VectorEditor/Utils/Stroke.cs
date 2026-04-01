using System;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace VectorEditor.Utils
{
    /// <summary>
    /// Класс для хранения свойств контура фигуры
    /// </summary>
    [Serializable]
    public class Stroke
    {
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Stroke()
        {
            Color = Color.Black;
            Width = 2f;
            DashStyle = DashStyle.Solid;
        }

        /// <summary>
        /// Цвет контура
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Толщина контура
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Стиль линии
        /// </summary>
        public DashStyle DashStyle { get; set; }

        /// <summary>
        /// Обновление пера по текущим настройкам
        /// </summary>
        public Pen UpdatePen(Pen pen)
        {
            if (pen == null)
                throw new ArgumentNullException(nameof(pen));

            pen.Color = Color;
            pen.Width = Width;
            pen.DashStyle = DashStyle;
            return pen;
        }

        /// <summary>
        /// Создание пера по текущим настройкам
        /// </summary>
        public Pen CreatePen()
        {
            return new Pen(Color, Width) { DashStyle = DashStyle };
        }
    }
}