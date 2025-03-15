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
            this.Text = "��������� ������";
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
                Text = "����������",
                Size = new System.Drawing.Size(120, 30),
                Location = new System.Drawing.Point(20, 140)
            };
            btnAnalyze.Click += async (sender, e) => await AnalyzeTextAsync();
            this.Controls.Add(btnAnalyze);

            btnSaveToFile = new Button
            {
                Text = "�������� � ����",
                Size = new System.Drawing.Size(120, 30),
                Location = new System.Drawing.Point(160, 140)
            };
            btnSaveToFile.Click += async (sender, e) => await SaveReportToFileAsync();
            this.Controls.Add(btnSaveToFile);

            lblReport = new Label
            {
                Text = "��������� ���� ���",
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
                MessageBox.Show("������ ����� ��� ������!", "�������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            return $"ʳ������ ������: {sentenceCount}\n" +
                   $"ʳ������ �������: {charCount}\n" +
                   $"ʳ������ ���: {wordCount}\n" +
                   $"ʳ������ ��������� ������: {questionCount}\n" +
                   $"ʳ������ �������� ������: {exclamationCount}";
        }

        private async Task SaveReportToFileAsync()
        {
            string text = txtInput.Text;
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("������ ����� ����� �����������!", "�������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var report = await Task.Run(() => AnalyzeText(text));

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt",
                Title = "���������� ����"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                await File.WriteAllTextAsync(saveFileDialog.FileName, report);
                MessageBox.Show("��� ������ ���������!", "������", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
