using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab01
{
    internal class Task1
    {
        public static void Run()
        {
            Console.Write("Введіть суму балів: ");
            double sum = double.Parse(Console.ReadLine()!);
            Console.Write("Введіть кількість предметів: ");
            int count = int.Parse(Console.ReadLine()!);

            double average = sum / count;

            Console.WriteLine($"Середній бал: {average:F2}");
        }
    }
}
