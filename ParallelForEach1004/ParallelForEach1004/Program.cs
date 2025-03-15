using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

class Program
{
    static async Task Main()
    {
        Console.Write("Введіть шлях до директорії: ");
        string directoryPath = Console.ReadLine()!;

        Console.Write("Введіть слово для пошуку: ");
        string searchWord = Console.ReadLine()!;

        if (string.IsNullOrWhiteSpace(directoryPath) || string.IsNullOrWhiteSpace(searchWord))
        {
            Console.WriteLine("❌ Помилка! Вкажіть правильний шлях та слово.");
            return;
        }

        if (!Directory.Exists(directoryPath))
        {
            Console.WriteLine("❌ Помилка! Вказана директорія не існує.");
            return;
        }

        List<string> results = await SearchWordInDirectory(directoryPath, searchWord);

        if (results.Count > 0)
        {
            Console.WriteLine("\n🔍 Результати пошуку:");
            results.ForEach(Console.WriteLine);

            Console.Write("\nЗберегти результати у файл? (так/ні): ");
            if (Console.ReadLine()?.ToLower() == "так")
            {
                File.WriteAllLines("search_results.txt", results);
                Console.WriteLine("✅ Результати збережено у 'search_results.txt'.");
            }
        }
        else
        {
            Console.WriteLine("❌ Слово не знайдено у жодному файлі.");
        }
    }

    static async Task<List<string>> SearchWordInDirectory(string directory, string searchWord)
    {
        List<string> results = new();
        string[] files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
        int totalFiles = files.Length;
        int processed = 0;

        await Task.Run(() =>
        {
            Parallel.ForEach(files, file =>
            {
                int count = CountWordOccurrences(file, searchWord);
                if (count > 0)
                {
                    string result = $"Файл: {Path.GetFileName(file)} | Входження: {count}";
                    lock (results) { results.Add(result); }
                }

                lock (Console.Out)
                {
                    processed++;
                    Console.Write($"\rОброблено файлів: {processed}/{totalFiles}");
                }
            });
        });

        return results;
    }

    static int CountWordOccurrences(string filePath, string searchWord)
    {
        try
        {
            string content = File.ReadAllText(filePath);
            return content.Split(new[] { searchWord }, StringSplitOptions.None).Length - 1;
        }
        catch
        {
            return 0;
        }
    }
}
