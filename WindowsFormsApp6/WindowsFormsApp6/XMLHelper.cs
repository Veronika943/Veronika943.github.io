using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;
using System.IO;

namespace RussianTraditionsQuiz.Classes
{
    /// <summary>
    /// Вспомогательный класс для работы с XML файлом
    /// </summary>
    public static class XMLHelper
    {
        private static string filePath = Path.Combine(Application.StartupPath, "Data", "quiz_data.xml");

        /// <summary>
        /// Получить список всех тем
        /// </summary>
        public static List<string> GetTopics()
        {
            List<string> topics = new List<string>();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);

                XmlNodeList topicNodes = doc.SelectNodes("/game/topics/topic");
                foreach (XmlNode node in topicNodes)
                {
                    if (node.Attributes["name"] != null)
                    {
                        topics.Add(node.Attributes["name"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки тем: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return topics;
        }

        /// <summary>
        /// Получить вопросы для указанной темы и уровня сложности
        /// </summary>
        public static List<Question> GetQuestions(string topic, int level)
        {
            List<Question> questions = new List<Question>();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);

                string xpath = $"/game/topics/topic[@name='{topic}']/level[@difficulty='{level}']/question";
                XmlNodeList questionNodes = doc.SelectNodes(xpath);

                foreach (XmlNode qNode in questionNodes)
                {
                    Question q = new Question();

                    // Получаем текст вопроса
                    if (qNode.Attributes["text"] != null)
                    {
                        q.Text = qNode.Attributes["text"].Value;
                    }

                    // Получаем путь к изображению (если есть)
                    if (qNode.Attributes["src"] != null)
                    {
                        q.ImagePath = qNode.Attributes["src"].Value;
                    }

                    // Получаем варианты ответов
                    int answerIndex = 0;
                    foreach (XmlNode aNode in qNode.SelectNodes("answer"))
                    {
                        q.Answers.Add(aNode.InnerText);

                        // Проверяем, является ли ответ правильным
                        if (aNode.Attributes["right"] != null &&
                            aNode.Attributes["right"].Value == "yes")
                        {
                            q.CorrectAnswerIndex = answerIndex;
                        }

                        answerIndex++;
                    }

                    questions.Add(q);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки вопросов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return questions;
        }

        /// <summary>
        /// Добавить новую тему
        /// </summary>
        public static bool AddTopic(string topicName)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);

                // Создаем узел темы
                XmlElement topicNode = doc.CreateElement("topic");
                topicNode.SetAttribute("name", topicName);

                // Создаем три уровня сложности
                for (int i = 1; i <= 3; i++)
                {
                    XmlElement levelNode = doc.CreateElement("level");
                    levelNode.SetAttribute("difficulty", i.ToString());
                    topicNode.AppendChild(levelNode);
                }

                // Добавляем тему в документ
                XmlNode topicsNode = doc.SelectSingleNode("/game/topics");
                topicsNode.AppendChild(topicNode);

                doc.Save(filePath);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления темы: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Сохранить вопрос
        /// </summary>
        public static bool SaveQuestion(string topic, int level, Question question)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);

                string xpath = $"/game/topics/topic[@name='{topic}']/level[@difficulty='{level}']";
                XmlNode levelNode = doc.SelectSingleNode(xpath);

                if (levelNode == null)
                {
                    MessageBox.Show("Уровень не найден", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // Создаем узел вопроса
                XmlElement questionNode = doc.CreateElement("question");
                questionNode.SetAttribute("text", question.Text);

                if (!string.IsNullOrEmpty(question.ImagePath))
                {
                    questionNode.SetAttribute("src", question.ImagePath);
                }

                // Добавляем ответы
                for (int i = 0; i < question.Answers.Count; i++)
                {
                    XmlElement answerNode = doc.CreateElement("answer");
                    answerNode.InnerText = question.Answers[i];

                    if (i == question.CorrectAnswerIndex)
                    {
                        answerNode.SetAttribute("right", "yes");
                    }

                    questionNode.AppendChild(answerNode);
                }

                levelNode.AppendChild(questionNode);
                doc.Save(filePath);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения вопроса: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Получить количество вопросов в теме и уровне
        /// </summary>
        public static int GetQuestionsCount(string topic, int level)
        {
            return GetQuestions(topic, level).Count;
        }

        /// <summary>
        /// Проверить существует ли тема
        /// </summary>
        public static bool TopicExists(string topicName)
        {
            List<string> topics = GetTopics();
            return topics.Contains(topicName);
        }
    }
}