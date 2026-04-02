using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace PairGame
{
    public class FormResults : Form
    {
        private ListBox listBoxResults;
        private Button buttonClose;

        public FormResults(string login)
        {
            InitializeForm();
            LoadResults(login);
        }

        private void InitializeForm()
        {
            this.Text = "Результаты игрока";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterParent;

            listBoxResults = new ListBox();
            listBoxResults.Location = new Point(10, 10);
            listBoxResults.Size = new Size(460, 320);
            listBoxResults.Font = new Font("Consolas", 10);

            buttonClose = new Button();
            buttonClose.Text = "Закрыть";
            buttonClose.Location = new Point(200, 340);
            buttonClose.Size = new Size(100, 30);
            buttonClose.Click += (s, e) => Close();

            this.Controls.Add(listBoxResults);
            this.Controls.Add(buttonClose);
        }

        private void LoadResults(string login)
        {
            string file = "results.dat";
            if (!File.Exists(file))
            {
                listBoxResults.Items.Add("Нет сохранённых результатов.");
                return;
            }

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream(file, FileMode.Open))
                {
                    var allResults = (List<UserResult>)formatter.Deserialize(fs);
                    var userResults = allResults.FindAll(r => r.Login == login);
                    userResults.Reverse();

                    if (userResults.Count == 0)
                    {
                        listBoxResults.Items.Add($"Нет результатов для игрока {login}");
                    }
                    else
                    {
                        listBoxResults.Items.Add($"=== Результаты игрока: {login} ===\n");
                        foreach (var res in userResults)
                        {
                            listBoxResults.Items.Add($"{res.Date:dd.MM.yyyy HH:mm:ss}");
                            listBoxResults.Items.Add($"  Пар найдено: {res.PairsFound}/8");
                            listBoxResults.Items.Add($"  Сделано ходов: {res.TotalMoves}");
                            listBoxResults.Items.Add($"  Осталось времени: {res.TimeLeft} сек");
                            listBoxResults.Items.Add(new string('-', 40));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listBoxResults.Items.Add($"Ошибка загрузки: {ex.Message}");
            }
        }
    }
}