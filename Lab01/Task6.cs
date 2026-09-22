using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab01
{
    internal class Task6
    {
        public static void Run()
        {
            Console.Write("Введіть номер студента: ");
            int number = int.Parse(Console.ReadLine()!);

            string group = (number % 10) switch
            {
                0 or 1 => "Група 1",
                2 or 3 => "Група 2",
                4 or 5 => "Група 3",
                6 or 7 => "Група 4",
                8 or 9 => "Група 5"
            };

            string educationForm = number % 2 == 0 ? "денна" : "вечірня";

            string scholarship = number % 3 == 0 ? "так" : "ні";

            Console.WriteLine($"Група: {group}");
            Console.WriteLine($"Форма навчання: {educationForm}");
            Console.WriteLine($"Стипендіат: {scholarship}");
        }
    }
}
