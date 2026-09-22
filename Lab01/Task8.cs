using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab01
{
    internal class Task8
    {
        public static double CalculateAverageScore(double sum, int count)
        {
            return sum / count;
        }

        public static double CalculateCost(double price, int credits, int benefit)
        {
            return price * credits * (1 - benefit / 100.0);
        }

        public static string GetCourseLevel(int credits)
        {
            if (credits <= 3)
            {
                return "базовий";
            }
            else if (credits <= 6)
            {
                return "середній";
            }
            else
            {
                return "поглиблений";
            }
        }

        public static string GetResultCategory(int score, int attempts)
        {
            if (score >= 90)
            {
                return "відмінно";
            }
            else if (score >= 75 && score <= 89 && attempts == 1)
            {
                return "добре";
            }
            else if (score >= 60 && score <= 74)
            {
                return "задовільно";
            }
            else
            {
                return "незадовільно";
            }
        }

        public static void Run()
        {
            Console.Write("Введіть суму балів: ");
            double sum = double.Parse(Console.ReadLine()!);

            Console.Write("Введіть кількість предметів: ");
            int count = int.Parse(Console.ReadLine()!);

            Console.Write("Введіть ціну одного кредиту: ");
            double price = double.Parse(Console.ReadLine()!);

            Console.Write("Введіть кількість кредитів: ");
            int credits = int.Parse(Console.ReadLine()!);

            Console.Write("Введіть пільгу (%): ");
            int benefit = int.Parse(Console.ReadLine()!);

            Console.Write("Введіть бал: ");
            int score = int.Parse(Console.ReadLine()!);

            Console.Write("Введіть кількість спроб: ");
            int attempts = int.Parse(Console.ReadLine()!);

            double average = CalculateAverageScore(sum, count);
            double cost = CalculateCost(price, credits, benefit);
            string level = GetCourseLevel(credits);
            string result = GetResultCategory(score, attempts);

            Console.WriteLine($"Середній бал: {average:F2}");
            Console.WriteLine($"Вартість: {cost:F2} грн");
            Console.WriteLine($"Рівень курсу: {level}");
            Console.WriteLine($"Результат: {result}");
        }
    }
}
