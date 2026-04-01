using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace VectorEditor.Utils
{
    /// <summary>
    /// Класс для хранения истории изменений
    /// </summary>
    [Serializable]
    public class StackMemory
    {
        private readonly int _stackDepth;
        private readonly List<byte[]> _list = new List<byte[]>();

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="depth">Глубина стека</param>
        public StackMemory(int depth)
        {
            _stackDepth = depth < 1 ? 1 : depth;
            _list.Clear();
        }

        /// <summary>
        /// Поместить данные в стек
        /// </summary>
        public void Push(MemoryStream stream)
        {
            if (_list.Count >= _stackDepth)
                _list.RemoveAt(0);
            _list.Add(stream.ToArray());
        }

        /// <summary>
        /// Очистить стек
        /// </summary>
        public void Clear()
        {
            _list.Clear();
        }

        /// <summary>
        /// Количество сохранённых версий
        /// </summary>
        public int Count => _list.Count;

        /// <summary>
        /// Извлечь данные из стека
        /// </summary>
        public void Pop(MemoryStream stream)
        {
            if (_list.Count == 0) return;

            var buff = _list[_list.Count - 1];
            stream.Write(buff, 0, buff.Length);
            _list.RemoveAt(_list.Count - 1);
        }

        /// <summary>
        /// Получить последнюю версию без удаления
        /// </summary>
        public byte[] Peek()
        {
            return _list.Count > 0 ? _list[_list.Count - 1] : null;
        }
    }
}
