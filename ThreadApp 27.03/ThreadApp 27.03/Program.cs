using System;
using System.Threading;

class Program
{
    static bool isRunning = true;

    static void Main()
    {
        Console.WriteLine("Виберіть алгоритм: 1 - Фібоначчі, 2 - Факторіал, 3 - Прості числа");
        int choice = int.Parse(Console.ReadLine() ?? "1");

        Configurator config;
        switch (choice)
        {
            case 1:
                config = new Configurator("Fibonacci", 10, 50);
                new Thread(() => Fibonacci(config)).Start();
                break;
            case 2:
                config = new Configurator("Factorial", 10, 50);
                new Thread(() => Factorial(config)).Start();
                break;
            case 3:
                config = new Configurator("Primes", 10, 50);
                new Thread(() => PrimeNumbers(config)).Start();
                break;
            default:
                Console.WriteLine("Невірний вибір.");
                return;
        }

        Console.WriteLine("Натисніть Enter, щоб зупинити потік...");
        Console.ReadLine();
        isRunning = false;
    }

    static void Fibonacci(Configurator config)
    {
        int a = 0, b = 1;
        for (int i = 0; i < config.Count && isRunning; i++)
        {
            Console.WriteLine($"Фібоначчі[{i}]: {a}");
            int temp = a + b;
            a = b;
            b = temp;
            Thread.Sleep(config.Delay);
        }
    }

    static void Factorial(Configurator config)
    {
        long result = 1;
        for (int i = 1; i <= config.Count && isRunning; i++)
        {
            result *= i;
            Console.WriteLine($"Факторіал[{i}]: {result}");
            Thread.Sleep(config.Delay);
        }
    }

    static void PrimeNumbers(Configurator config)
    {
        int num = 2, count = 0;
        while (count < config.Count && isRunning)
        {
            if (IsPrime(num))
            {
                Console.WriteLine($"Просте число[{count}]: {num}");
                count++;
                Thread.Sleep(config.Delay);
            }
            num++;
        }
    }

    static bool IsPrime(int num)
    {
        if (num < 2) return false;
        for (int i = 2; i * i <= num; i++)
            if (num % i == 0) return false;
        return true;
    }
}

class Configurator
{
    public string Name { get; set; }
    public int Count { get; set; }
    public int Delay { get; set; } // у мілісекундах

    public Configurator(string name, int count, int delay)
    {
        Name = name;
        Count = count;
        Delay = delay;
    }
}
