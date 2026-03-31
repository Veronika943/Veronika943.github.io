using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RussianTraditionsQuiz.Classes
{
    /// <summary>
    /// Класс, представляющий вопрос викторины
    /// </summary>
    public class Question
    {
        /// <summary>
        /// Текст вопроса
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Путь к изображению (может быть пустым)
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Список вариантов ответов
        /// </summary>
        public List<string> Answers { get; set; }

        /// <summary>
        /// Индекс правильного ответа (начинается с 0)
        /// </summary>
        public int CorrectAnswerIndex { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Question()
        {
            Answers = new List<string>();
            Text = string.Empty;
            ImagePath = string.Empty;
            CorrectAnswerIndex = -1;
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        public Question(string text, List<string> answers, int correctIndex, string imagePath = "")
        {
            Text = text;
            Answers = answers;
            CorrectAnswerIndex = correctIndex;
            ImagePath = imagePath;
        }

        /// <summary>
        /// Проверяет, является ли ответ правильным
        /// </summary>
        public bool IsCorrect(int selectedIndex)
        {
            return selectedIndex == CorrectAnswerIndex;
        }

        /// <summary>
        /// Возвращает текст правильного ответа
        /// </summary>
        public string GetCorrectAnswer()
        {
            if (CorrectAnswerIndex >= 0 && CorrectAnswerIndex < Answers.Count)
                return Answers[CorrectAnswerIndex];
            return string.Empty;
        }
    }
}