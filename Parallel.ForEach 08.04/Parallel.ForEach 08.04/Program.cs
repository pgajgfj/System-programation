using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        string filePath = "numbers.txt"; 
        List<int> numbers = ReadNumbersFromFile(filePath); 

        
        var factorialResults = new List<string>();

        
        Parallel.ForEach(numbers, (number) =>
        {
            long factorial = CalculateFactorial(number);
            factorialResults.Add($"Factorial of {number} is {factorial}");
        });

        
        foreach (var result in factorialResults)
        {
            Console.WriteLine(result);
        }
    }

   
    static List<int> ReadNumbersFromFile(string filePath)
    {
        var numbers = new List<int>();
        try
        {
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                if (int.TryParse(line, out int number))
                {
                    numbers.Add(number);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
        }
        return numbers;
    }

    
    static long CalculateFactorial(int n)
    {
        if (n == 0 || n == 1)
            return 1;

        long result = 1;
        for (int i = 2; i <= n; i++)
        {
            result *= i;
        }
        return result;
    }
}
