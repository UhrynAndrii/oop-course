using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab01
{
    internal class Task2
    {
        public static void Run()
        {
            Console.Write("Введіть ціну одного кредиту: ");
            double price = double.Parse(Console.ReadLine()!);

            Console.Write("Введіть кількість кредитів: ");
            int credits = int.Parse(Console.ReadLine()!);

            Console.Write("Введіть пільгу (%): ");
            int benefit = int.Parse(Console.ReadLine()!);

            double cost = price * credits * (1 - benefit / 100.0);

            Console.WriteLine($"Вартість: {cost:F2} грн");
        }
    }
}
