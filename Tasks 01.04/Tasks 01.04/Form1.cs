using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tasks_01_04
{
    public class Form1 : Form
    {
        private TextBox txtInput;
        private Button btnAnalyze;
        private Button btnSaveToFile;
        private Label lblReport;

        public Form1()
        {
            this.Text = "Аналізатор тексту";
            this.Size = new System.Drawing.Size(500, 400);

            CreateUI();
        }

        private void CreateUI()
        {
            txtInput = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Size = new System.Drawing.Size(450, 100),
                Location = new System.Drawing.Point(20, 20)
            };
            this.Controls.Add(txtInput);

            btnAnalyze = new Button
            {
                Text = "Аналізувати",
                Size = new System.Drawing.Size(120, 30),
                Location = new System.Drawing.Point(20, 140)
            };
            btnAnalyze.Click += async (sender, e) => await AnalyzeTextAsync();
            this.Controls.Add(btnAnalyze);

            btnSaveToFile = new Button
            {
                Text = "Зберегти у файл",
                Size = new System.Drawing.Size(120, 30),
                Location = new System.Drawing.Point(160, 140)
            };
            btnSaveToFile.Click += async (sender, e) => await SaveReportToFileAsync();
            this.Controls.Add(btnSaveToFile);

            lblReport = new Label
            {
                Text = "Результат буде тут",
                AutoSize = true,
                Location = new System.Drawing.Point(20, 190),
                Size = new System.Drawing.Size(450, 100)
            };
            this.Controls.Add(lblReport);
        }

        private async Task AnalyzeTextAsync()
        {
            string text = txtInput.Text;
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Введіть текст для аналізу!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var report = await Task.Run(() => AnalyzeText(text));
            lblReport.Text = report;
        }

        private string AnalyzeText(string text)
        {
            int sentenceCount = Regex.Matches(text, @"[.!?]").Count;
            int wordCount = Regex.Matches(text, @"\b\w+\b").Count;
            int charCount = text.Length;
            int questionCount = Regex.Matches(text, @"\?").Count;
            int exclamationCount = Regex.Matches(text, @"!").Count;

            return $"Кількість речень: {sentenceCount}\n" +
                   $"Кількість символів: {charCount}\n" +
                   $"Кількість слів: {wordCount}\n" +
                   $"Кількість питальних речень: {questionCount}\n" +
                   $"Кількість окличних речень: {exclamationCount}";
        }

        private async Task SaveReportToFileAsync()
        {
            string text = txtInput.Text;
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Введіть текст перед збереженням!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var report = await Task.Run(() => AnalyzeText(text));

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt",
                Title = "Збереження звіту"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                await File.WriteAllTextAsync(saveFileDialog.FileName, report);
                MessageBox.Show("Звіт успішно збережено!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
