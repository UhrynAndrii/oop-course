using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab01
{
    internal class Task7
    {
        public static void Run()
        {
            Console.Write("Введіть кількість оцінок: ");
            int n = int.Parse(Console.ReadLine()!);

            int[] scores = new int[n];

            for (int i = 0; i < n; i++)
            {
                Console.Write($"Введіть оцінку {i + 1}: ");
                scores[i] = int.Parse(Console.ReadLine()!);
            }

            int sum = 0;
            int min = scores[0];
            int max = scores[0];

            foreach (int score in scores)
            {
                sum += score;

                if (score < min)
                {
                    min = score;
                }

                if (score > max)
                {
                    max = score;
                }
            }

            decimal average = (decimal)sum / n;

            int aboveAverage = 0;

            for (int i = 0; i < scores.Length; i++)
            {
                if (scores[i] > average)
                {
                    aboveAverage++;
                }
            }

            int firstExcellentIndex = -1;
            int index = 0;

            while (index < scores.Length)
            {
                if (scores[index] >= 90)
                {
                    firstExcellentIndex = index;
                    break;
                }

                index++;
            }

            Console.WriteLine("=== Звіт по оцінках ===");
            Console.WriteLine($"Кількість:        {n}");
            Console.WriteLine($"Сума балів:       {sum}");
            Console.WriteLine($"Середня:          {average:F2}");
            Console.WriteLine($"Мін / Макс:       {min} / {max}");
            Console.WriteLine($"Вище середнього:  {aboveAverage} з {n}");

            if (firstExcellentIndex == -1)
            {
                Console.WriteLine("Перша >= 90:      немає");
            }
            else
            {
                Console.WriteLine(
                    $"Перша >= 90:      #{firstExcellentIndex + 1} — {scores[firstExcellentIndex]}"
                );
            }

            Console.WriteLine("=======================");
        }
    }
}
