using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab01
{
    internal class Task5
    {
        public static void Run()
        {
            Console.Write("Введіть рівень програми (B/M/P): ");
            string program = Console.ReadLine()!;

            string level = program switch
            {
                "B" => "Бакалавр",
                "M" => "Магістр",
                "P" => "Аспірант",
                _ => "невідома програма"
            };

            Console.WriteLine($"Рівень програми: {level}");
        }
    }
}
