using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using RussianQuiz.Models;

namespace RussianQuiz.Helpers
{
    public static class XmlHelper
    {
        private static string xmlFilePath;

        public static void Initialize(string filePath)
        {
            xmlFilePath = filePath;
        }

        public static List<Topic> LoadTopics()
        {
            var topics = new List<Topic>();

            if (!File.Exists(xmlFilePath))
            {
                throw new FileNotFoundException($"XML файл не найден: {xmlFilePath}");
            }

            XDocument doc = XDocument.Load(xmlFilePath);

            foreach (var topicElement in doc.Descendants("topic"))
            {
                var topic = new Topic
                {
                    Id = int.Parse(topicElement.Attribute("id")?.Value ?? "0"),
                    Name = topicElement.Attribute("name")?.Value ?? "",
                    Description = topicElement.Element("description")?.Value ?? "",
                    Levels = new List<Level>()
                };

                foreach (var levelElement in topicElement.Descendants("level"))
                {
                    var level = new Level
                    {
                        Id = int.Parse(levelElement.Attribute("id")?.Value ?? "0"),
                        Name = levelElement.Attribute("name")?.Value ?? "",
                        Questions = new List<Question>()
                    };

                    foreach (var questionElement in levelElement.Descendants("question"))
                    {
                        var question = new Question
                        {
                            Id = int.Parse(questionElement.Attribute("id")?.Value ?? "0"),
                            Text = questionElement.Attribute("text")?.Value ?? "",
                            ImagePath = questionElement.Attribute("image")?.Value ?? "",
                            Answers = new List<Answer>()
                        };

                        foreach (var answerElement in questionElement.Descendants("answer"))
                        {
                            var answer = new Answer
                            {
                                Text = answerElement.Value,
                                IsRight = answerElement.Attribute("right")?.Value == "true"
                            };
                            question.Answers.Add(answer);
                        }

                        level.Questions.Add(question);
                    }

                    topic.Levels.Add(level);
                }

                topics.Add(topic);
            }

            return topics;
        }

        public static void SaveTopics(List<Topic> topics)
        {
            var doc = new XDocument();
            var root = new XElement("quiz");
            var topicsElement = new XElement("topics");

            foreach (var topic in topics)
            {
                var topicElement = new XElement("topic",
                    new XAttribute("id", topic.Id),
                    new XAttribute("name", topic.Name),
                    new XElement("description", topic.Description ?? ""));

                var levelsElement = new XElement("levels");

                foreach (var level in topic.Levels)
                {
                    var levelElement = new XElement("level",
                        new XAttribute("id", level.Id),
                        new XAttribute("name", level.Name));

                    var questionsElement = new XElement("questions");

                    foreach (var question in level.Questions)
                    {
                        var questionElement = new XElement("question",
                            new XAttribute("id", question.Id),
                            new XAttribute("text", question.Text),
                            new XAttribute("image", question.ImagePath ?? ""));

                        foreach (var answer in question.Answers)
                        {
                            var answerElement = new XElement("answer",
                                new XAttribute("right", answer.IsRight ? "true" : "false"),
                                answer.Text);
                            questionElement.Add(answerElement);
                        }

                        questionsElement.Add(questionElement);
                    }

                    levelElement.Add(questionsElement);
                    levelsElement.Add(levelElement);
                }

                topicElement.Add(levelsElement);
                topicsElement.Add(topicElement);
            }

            root.Add(topicsElement);
            doc.Add(root);
            doc.Save(xmlFilePath);
        }

        public static List<string> GetTopicNames()
        {
            var topics = LoadTopics();
            return topics.Select(t => t.Name).ToList();
        }

        public static Topic GetTopicByName(string name)
        {
            var topics = LoadTopics();
            return topics.FirstOrDefault(t => t.Name == name);
        }
    }
}

