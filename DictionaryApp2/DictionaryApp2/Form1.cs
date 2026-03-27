using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DictionaryApp
{
    public partial class Form1 : Form
    {
        private Slovar dict;
        private List<string> lastResults = new List<string>();

        // Элементы управления
        private ComboBox cmbDict;
        private Button btnCreate;
        private Button btnDelete;
        private Button btnSave;
        private TextBox txtWord;
        private Button btnAdd;
        private Button btnRemove;
        private ListBox listBox;
        private GroupBox groupBox;
        private RadioButton rbVowels;
        private RadioButton rbConsonants;
        private NumericUpDown numCount;
        private Button btnSearch;
        private TextBox txtFuzzy;
        private Button btnFuzzy;
        private Button btnSaveResults;
        private Label lblStatus;

        public Form1()
        {
            // Создаём форму и элементы
            this.Text = "Словарь (Вариант 6)";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(800, 550);
            this.BackColor = Color.White;

            // ComboBox для выбора словаря
            cmbDict = new ComboBox();
            cmbDict.Location = new Point(12, 12);
            cmbDict.Size = new Size(200, 24);
            cmbDict.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDict.SelectedIndexChanged += cmbDict_SelectedIndexChanged;

            // Кнопка Создать словарь
            btnCreate = new Button();
            btnCreate.Location = new Point(220, 10);
            btnCreate.Size = new Size(90, 28);
            btnCreate.Text = "Создать";
            btnCreate.Click += btnCreate_Click;

            // Кнопка Удалить словарь
            btnDelete = new Button();
            btnDelete.Location = new Point(318, 10);
            btnDelete.Size = new Size(90, 28);
            btnDelete.Text = "Удалить";
            btnDelete.Click += btnDelete_Click;

            // Кнопка Сохранить словарь
            btnSave = new Button();
            btnSave.Location = new Point(416, 10);
            btnSave.Size = new Size(90, 28);
            btnSave.Text = "Сохранить";
            btnSave.Click += btnSave_Click;

            // Поле ввода слова
            txtWord = new TextBox();
            txtWord.Location = new Point(12, 55);
            txtWord.Size = new Size(200, 23);
            txtWord.Font = new Font("Microsoft Sans Serif", 10);

            // Кнопка Добавить слово
            btnAdd = new Button();
            btnAdd.Location = new Point(220, 53);
            btnAdd.Size = new Size(90, 28);
            btnAdd.Text = "Добавить";
            btnAdd.Click += btnAdd_Click;

            // Кнопка Удалить слово
            btnRemove = new Button();
            btnRemove.Location = new Point(318, 53);
            btnRemove.Size = new Size(90, 28);
            btnRemove.Text = "Удалить слово";
            btnRemove.Click += btnRemove_Click;

            // Список слов
            listBox = new ListBox();
            listBox.Location = new Point(12, 95);
            listBox.Size = new Size(380, 360);
            listBox.Font = new Font("Microsoft Sans Serif", 10);

            // Группа поиска по варианту 6
            groupBox = new GroupBox();
            groupBox.Text = "Поиск по количеству (Вариант 6)";
            groupBox.Location = new Point(410, 95);
            groupBox.Size = new Size(350, 130);
            groupBox.Font = new Font("Microsoft Sans Serif", 9);

            // RadioButton Гласные
            rbVowels = new RadioButton();
            rbVowels.Text = "Гласные";
            rbVowels.Location = new Point(15, 25);
            rbVowels.Size = new Size(80, 20);
            rbVowels.Checked = true;

            // RadioButton Согласные
            rbConsonants = new RadioButton();
            rbConsonants.Text = "Согласные";
            rbConsonants.Location = new Point(15, 50);
            rbConsonants.Size = new Size(90, 20);

            // NumericUpDown
            numCount = new NumericUpDown();
            numCount.Location = new Point(15, 80);
            numCount.Size = new Size(60, 22);
            numCount.Minimum = 1;
            numCount.Value = 2;

            // Кнопка Найти
            btnSearch = new Button();
            btnSearch.Location = new Point(90, 78);
            btnSearch.Size = new Size(80, 28);
            btnSearch.Text = "Найти";
            btnSearch.Click += btnSearch_Click;

            // Добавляем элементы в группу
            groupBox.Controls.Add(rbVowels);
            groupBox.Controls.Add(rbConsonants);
            groupBox.Controls.Add(numCount);
            groupBox.Controls.Add(btnSearch);

            // Поле для нечёткого поиска
            txtFuzzy = new TextBox();
            txtFuzzy.Location = new Point(410, 240);
            txtFuzzy.Size = new Size(260, 23);
            txtFuzzy.Font = new Font("Microsoft Sans Serif", 10);

            // Кнопка нечёткого поиска
            btnFuzzy = new Button();
            btnFuzzy.Location = new Point(678, 238);
            btnFuzzy.Size = new Size(82, 28);
            btnFuzzy.Text = "Нечёткий поиск";
            btnFuzzy.Click += btnFuzzy_Click;

            // Кнопка сохранения результатов
            btnSaveResults = new Button();
            btnSaveResults.Location = new Point(410, 280);
            btnSaveResults.Size = new Size(150, 32);
            btnSaveResults.Text = "Сохранить результаты";
            btnSaveResults.Click += btnSaveResults_Click;

            // Статус
            lblStatus = new Label();
            lblStatus.Location = new Point(12, 470);
            lblStatus.Size = new Size(760, 25);
            lblStatus.Text = "Готово";
            lblStatus.BackColor = Color.LightGray;

            // Добавляем все элементы на форму
            this.Controls.Add(cmbDict);
            this.Controls.Add(btnCreate);
            this.Controls.Add(btnDelete);
            this.Controls.Add(btnSave);
            this.Controls.Add(txtWord);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnRemove);
            this.Controls.Add(listBox);
            this.Controls.Add(groupBox);
            this.Controls.Add(txtFuzzy);
            this.Controls.Add(btnFuzzy);
            this.Controls.Add(btnSaveResults);
            this.Controls.Add(lblStatus);

            // Загружаем словари
            LoadDictionaries();
        }

        private void LoadDictionaries()
        {
            cmbDict.Items.Clear();
            string dir = Path.Combine(Application.StartupPath, "Dictionaries");
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            foreach (var f in Directory.GetFiles(dir, "*.txt"))
                cmbDict.Items.Add(Path.GetFileName(f));

            if (cmbDict.Items.Count > 0)
                cmbDict.SelectedIndex = 0;
            else
                lblStatus.Text = "Нет словарей. Нажмите 'Создать'";
        }

        private void cmbDict_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDict.SelectedItem == null) return;
            string file = Path.Combine(Application.StartupPath, "Dictionaries", cmbDict.SelectedItem.ToString());
            dict = new Slovar(file);
            UpdateList(dict.GetByPrefix(""));
            lblStatus.Text = $"Загружен: {cmbDict.SelectedItem}, слов: {dict.Count}";
        }

        private void UpdateList(List<string> list)
        {
            listBox.DataSource = null;
            listBox.DataSource = list;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (dict == null) { MessageBox.Show("Выберите словарь"); return; }
            string w = txtWord.Text.Trim();
            if (string.IsNullOrEmpty(w)) { MessageBox.Show("Введите слово"); return; }
            if (dict.Add(w))
            {
                UpdateList(dict.GetByPrefix(""));
                txtWord.Clear();
                lblStatus.Text = $"Добавлено: {w}";
            }
            else MessageBox.Show("Слово уже есть в словаре");
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dict == null) { MessageBox.Show("Выберите словарь"); return; }
            if (listBox.SelectedItem == null) { MessageBox.Show("Выберите слово"); return; }
            string w = listBox.SelectedItem.ToString();
            if (dict.Remove(w))
            {
                UpdateList(dict.GetByPrefix(""));
                lblStatus.Text = $"Удалено: {w}";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dict == null) return;
            SaveFileDialog dlg = new SaveFileDialog { Filter = "Текстовые файлы|*.txt" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                dict.Save(dlg.FileName);
                MessageBox.Show("Словарь сохранён");
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog { Filter = "Текстовые файлы|*.txt" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                File.Create(dlg.FileName).Close();
                LoadDictionaries();
                cmbDict.SelectedItem = Path.GetFileName(dlg.FileName);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (cmbDict.SelectedItem == null) return;
            string file = Path.Combine(Application.StartupPath, "Dictionaries", cmbDict.SelectedItem.ToString());
            if (File.Exists(file)) File.Delete(file);
            LoadDictionaries();
            dict = null;
            listBox.DataSource = null;
            lblStatus.Text = "Словарь удалён";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (dict == null) { MessageBox.Show("Выберите словарь"); return; }
            int cnt = (int)numCount.Value;
            var type = rbVowels.Checked ? Slovar.SearchType.Vowels : Slovar.SearchType.Consonants;
            string typeName = rbVowels.Checked ? "гласных" : "согласных";
            lastResults = dict.SearchByCount(cnt, type);
            if (lastResults.Count == 0)
            {
                MessageBox.Show($"Нет слов с {cnt} {typeName}");
                UpdateList(new List<string>());
                lblStatus.Text = "Результатов не найдено";
            }
            else
            {
                UpdateList(lastResults);
                lblStatus.Text = $"Найдено: {lastResults.Count} слов с {cnt} {typeName}";
            }
        }

        private void btnFuzzy_Click(object sender, EventArgs e)
        {
            if (dict == null) { MessageBox.Show("Выберите словарь"); return; }
            string pat = txtFuzzy.Text.Trim();
            if (string.IsNullOrEmpty(pat)) { MessageBox.Show("Введите слово для поиска"); return; }
            lastResults = dict.FuzzySearch(pat);
            if (lastResults.Count == 0)
            {
                MessageBox.Show($"Нет слов, похожих на '{pat}'");
                UpdateList(new List<string>());
                lblStatus.Text = "Результатов не найдено";
            }
            else
            {
                UpdateList(lastResults);
                lblStatus.Text = $"Найдено похожих слов: {lastResults.Count}";
            }
        }

        private void btnSaveResults_Click(object sender, EventArgs e)
        {
            if (lastResults.Count == 0) { MessageBox.Show("Нет результатов для сохранения"); return; }
            SaveFileDialog dlg = new SaveFileDialog { Filter = "Текстовые файлы|*.txt" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllLines(dlg.FileName, lastResults);
                MessageBox.Show($"Сохранено {lastResults.Count} слов");
            }
        }
    }

    // КЛАСС СЛОВАРЯ
    public class Slovar
    {
        private List<string> words = new List<string>();
        private string path;

        public Slovar(string filePath)
        {
            path = filePath;
            Load();
        }

        public int Count => words.Count;
        public List<string> GetAll() => new List<string>(words);

        private void Load()
        {
            words.Clear();
            if (!File.Exists(path)) return;
            var lines = File.ReadAllLines(path, System.Text.Encoding.UTF8);
            foreach (var line in lines)
            {
                var w = line.Trim();
                if (!string.IsNullOrEmpty(w)) words.Add(w);
            }
        }

        public void Save(string savePath)
        {
            File.WriteAllLines(savePath, words.OrderBy(w => w), System.Text.Encoding.UTF8);
        }

        public bool Add(string word)
        {
            if (string.IsNullOrWhiteSpace(word)) return false;
            word = word.Trim();
            if (words.Contains(word)) return false;
            words.Add(word);
            return true;
        }

        public bool Remove(string word)
        {
            return words.Remove(word);
        }

        public List<string> GetByPrefix(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
                return words.OrderBy(w => w).ToList();
            return words.Where(w => w.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                        .OrderBy(w => w)
                        .ToList();
        }

        private int Levenshtein(string a, string b)
        {
            int n = a.Length, m = b.Length;
            if (n == 0) return m;
            if (m == 0) return n;
            int[,] d = new int[n + 1, m + 1];
            for (int i = 0; i <= n; i++) d[i, 0] = i;
            for (int j = 0; j <= m; j++) d[0, j] = j;
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = a[i - 1] == b[j - 1] ? 0 : 1;
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }
            return d[n, m];
        }

        public List<string> FuzzySearch(string pattern)
        {
            if (string.IsNullOrEmpty(pattern)) return new List<string>();
            return words.Where(w => Levenshtein(w, pattern) <= 3).ToList();
        }

        public enum SearchType { Vowels, Consonants }

        private static readonly HashSet<char> RussianVowels = new HashSet<char>
        {
            'а','е','ё','и','о','у','ы','э','ю','я',
            'А','Е','Ё','И','О','У','Ы','Э','Ю','Я'
        };

        private static readonly HashSet<char> EnglishVowels = new HashSet<char>
        {
            'a','e','i','o','u','y','A','E','I','O','U','Y'
        };

        private bool IsRussian(char c)
        {
            return (c >= 'а' && c <= 'я') || (c >= 'А' && c <= 'Я') || c == 'ё' || c == 'Ё';
        }

        private int CountVowels(string word)
        {
            int cnt = 0;
            foreach (char c in word)
            {
                if (IsRussian(c))
                {
                    if (RussianVowels.Contains(c)) cnt++;
                }
                else
                {
                    if (EnglishVowels.Contains(c)) cnt++;
                }
            }
            return cnt;
        }

        private int CountConsonants(string word)
        {
            return word.Count(char.IsLetter) - CountVowels(word);
        }

        public List<string> SearchByCount(int target, SearchType type)
        {
            var result = new List<string>();
            foreach (var w in words)
            {
                int val = type == SearchType.Vowels ? CountVowels(w) : CountConsonants(w);
                if (val == target) result.Add(w);
            }
            return result;
        }
    }
}