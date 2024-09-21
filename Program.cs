using System;
using System.Linq;
using System.Threading;

class Program
{
    static int totalCount = 0;
    static object lockObject = new object();

    static void FindWords(object text)
    {
        string[] words = text.ToString().Split(new[] { ' ', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

        int count = words.Count(word =>
        {
            char firstChar = char.ToLower(word[0]);
            char lastChar = char.ToLower(word[word.Length - 1]);
            return firstChar == lastChar;
        });

        lock (lockObject)
        {
            totalCount += count;
        }

        Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} обработал часть текста. Найдено {count} слов.");
    }

    static void Main()
    {
        Console.WriteLine("Введите текст:");
        string inputText = Console.ReadLine();

        int midPoint = inputText.Length / 2;
        string part1 = inputText.Substring(0, midPoint);
        string part2 = inputText.Substring(midPoint);

        Thread thread1 = new Thread(FindWords);
        Thread thread2 = new Thread(FindWords);

        thread1.Start(part1);
        thread2.Start(part2);

        thread1.Join();
        thread2.Join();

        Console.WriteLine($"Общее количество слов, начинающихся и заканчивающихся на одну и ту же букву: {totalCount}");
    }
}
