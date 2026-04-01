using System;
using System.Drawing;
using VectorEditor.Utils;

namespace VectorEditor.Figures
{
    /// <summary>
    /// Базовый абстрактный класс для всех фигур
    /// </summary>
    [Serializable]
    public abstract class Figure
    {
        /// <summary>
        /// Прямоугольная область фигуры (верхний левый угол, ширина, высота)
        /// </summary>
        public Rectangle Bounds { get; set; }

        /// <summary>
        /// Контур фигуры
        /// </summary>
        public Stroke Stroke { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        protected Figure()
        {
            Stroke = new Stroke();
            Bounds = new Rectangle(100, 100, 100, 100);
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        protected Figure(Rectangle bounds, Stroke stroke)
        {
            Bounds = bounds;
            Stroke = stroke ?? new Stroke();
        }

        /// <summary>
        /// Отрисовка фигуры
        /// </summary>
        public abstract void Draw(Graphics g);

        /// <summary>
        /// Проверка попадания точки в фигуру
        /// </summary>
        public abstract bool HitTest(Point point);

        /// <summary>
        /// Сдвиг фигуры
        /// </summary>
        public virtual void Move(int dx, int dy)
        {
            Bounds = new Rectangle(Bounds.X + dx, Bounds.Y + dy, Bounds.Width, Bounds.Height);
        }

        /// <summary>
        /// Перемещение в точку
        /// </summary>
        public virtual void MoveTo(Point newLocation)
        {
            Bounds = new Rectangle(newLocation.X, newLocation.Y, Bounds.Width, Bounds.Height);
        }

        /// <summary>
        /// Изменение размера
        /// </summary>
        public virtual void Resize(int width, int height)
        {
            Bounds = new Rectangle(Bounds.X, Bounds.Y, width, height);
        }

        /// <summary>
        /// Отразить по горизонтали
        /// </summary>
        public abstract void FlipHorizontally();

        /// <summary>
        /// Отразить по вертикали
        /// </summary>
        public abstract void FlipVertically();

        /// <summary>
        /// Повернуть на 90 градусов
        /// </summary>
        public abstract void Rotate90();

        /// <summary>
        /// Создать копию фигуры
        /// </summary>
        public abstract Figure Clone();

        /// <summary>
        /// Получить центр фигуры
        /// </summary>
        public virtual Point GetCenter()
        {
            return new Point(Bounds.X + Bounds.Width / 2, Bounds.Y + Bounds.Height / 2);
        }
    }
}