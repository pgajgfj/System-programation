using System;
using System.IO;
using System.Linq;
using System.Threading;

class TextAnalysisResult
{
    private int words;
    private int lines;
    private int punctuation;
    private readonly object lockObject = new object();

    public int Words => words;
    public int Lines => lines;
    public int Punctuation => punctuation;

    public void AddResults(int wordsCount, int linesCount, int punctuationCount)
    {
        Interlocked.Add(ref words, wordsCount);
        Interlocked.Add(ref lines, linesCount);

        
        lock (lockObject)
        {
            punctuation += punctuationCount;
        }
    }
}

class Program
{
    private static readonly char[] PunctuationMarks =
        { '.', ',', ';', ':', '–', '—', '‒', '…', '!', '?', '"', '\'', '«', '»',
          '(', ')', '{', '}', '[', ']', '<', '>', '/' };

    static void Main()
    {
        string directoryPath = @"C:\Users\user\source\repos\TextAnalysisResult 29.03\TextAnalysisResult 29.03\TextFiles"; 

        if (!Directory.Exists(directoryPath))
        {
            Console.WriteLine("Вказана директорія не існує.");
            return;
        }

        string[] files = Directory.GetFiles(directoryPath, "*.txt");
        if (files.Length == 0)
        {
            Console.WriteLine("У директорії немає .txt файлів.");
            return;
        }

        TextAnalysisResult result = new TextAnalysisResult();
        Thread[] threads = new Thread[files.Length];

        for (int i = 0; i < files.Length; i++)
        {
            string filePath = files[i];
            threads[i] = new Thread(() => AnalyzeFile(filePath, result));
            threads[i].Start();
        }

        
        foreach (Thread thread in threads)
        {
            thread.Join();
        }

       
        Console.WriteLine("\nЗагальні результати:");
        Console.WriteLine($"Слів: {result.Words}");
        Console.WriteLine($"Рядків: {result.Lines}");
        Console.WriteLine($"Розділових знаків: {result.Punctuation}");
    }

    static void AnalyzeFile(string filePath, TextAnalysisResult result)
    {
        try
        {
            string[] lines = File.ReadAllLines(filePath);
            int wordCount = lines.Sum(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length);
            int punctuationCount = lines.Sum(line => line.Count(c => PunctuationMarks.Contains(c)));

            result.AddResults(wordCount, lines.Length, punctuationCount);

            Console.WriteLine($"Файл: {Path.GetFileName(filePath)} -> Слова: {wordCount}, Рядки: {lines.Length}, Розділові знаки: {punctuationCount}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при обробці файлу {filePath}: {ex.Message}");
        }
    }
}
