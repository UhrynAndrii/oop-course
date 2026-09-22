using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab01
{
    internal class Task4
    {
        public static void Run()
        {
            Console.Write("Введіть бал: ");
            int score = int.Parse(Console.ReadLine()!);

            Console.Write("Введіть кількість спроб: ");
            int attempts = int.Parse(Console.ReadLine()!);

            string result;

            if (score >= 90)
            {
                result = "відмінно";
            }
            else if (score >= 75 && score <= 89 && attempts == 1)
            {
                result = "добре";
            }
            else if (score >= 60 && score <= 74)
            {
                result = "задовільно";
            }
            else
            {
                result = "незадовільно";
            }

            Console.WriteLine($"Результат: {result}");
        }
    }
}
