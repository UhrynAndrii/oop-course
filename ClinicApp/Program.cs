using ClinicApp;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Patient patient1 = new Patient(
    "Андрій",
    "Мельник",
    new DateTime(1988, 4, 18),
    "A+",
    "0504567891");

Patient patient2 = new Patient(
    "Софія",
    "Романюк",
    new DateTime(1995, 9, 7),
    "B-",
    "0678123456");

Patient patient3 = new Patient(
    "Данило",
    "Шевчук",
    new DateTime(2011, 2, 25),
    "O+",
    "0935678123");

Patient patient4 = new Patient();

Patient patient5 = new Patient(
    "Катерина",
    "Лисенко");

Console.WriteLine(patient1);
Console.WriteLine(patient2);
Console.WriteLine(patient3);
Console.WriteLine(patient4);
Console.WriteLine(patient5);

Console.WriteLine();

Doctor doctor1 = new Doctor(
    "Віктор",
    "Кравченко",
    "Кардіологія",
    "LIC-101",
    "0444567890");

Doctor doctor2 = new Doctor(
    "Ірина",
    "Бондар",
    "Неврологія",
    "LIC-102",
    "0445678901");

Doctor doctor3 = new Doctor(
    "Тарас",
    "Гнатюк",
    "Педіатрія",
    "LIC-103",
    "0446789012");

Console.WriteLine(doctor1);
Console.WriteLine(doctor2);
Console.WriteLine(doctor3);

PatientManager patientManager = new PatientManager();

patientManager.Add(patient1);
patientManager.Add(patient2);
patientManager.Add(patient3);
patientManager.Add(patient4);
patientManager.Add(patient5);

DoctorManager doctorManager = new DoctorManager();

doctorManager.Add(doctor1);
doctorManager.Add(doctor2);
doctorManager.Add(doctor3);

void PatientsMenu()
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("=== Пацієнти ===");
        Console.WriteLine("1. Показати всіх");
        Console.WriteLine("2. Додати");
        Console.WriteLine("3. Знайти за ім'ям");
        Console.WriteLine("4. Видалити");
        Console.WriteLine("5. Статистика");
        Console.WriteLine("0. Назад");
        Console.Write("Ваш вибір: ");

        string choice = Console.ReadLine()!;

        Console.WriteLine();

        if (choice == "1")
        {
            patientManager.DisplayAll();
        }
        else if (choice == "2")
        {
            Console.Write("Ім'я: ");
            string firstName = Console.ReadLine()!;

            Console.Write("Прізвище: ");
            string lastName = Console.ReadLine()!;

            Console.Write("Дата народження (рррр-мм-дд): ");
            DateTime dateOfBirth = DateTime.Parse(Console.ReadLine()!);

            Console.Write("Група крові: ");
            string bloodType = Console.ReadLine()!;

            Console.Write("Телефон: ");
            string phone = Console.ReadLine()!;

            Patient patient = new Patient(
                firstName,
                lastName,
                dateOfBirth,
                bloodType,
                phone);

            patientManager.Add(patient);
        }
        else if (choice == "3")
        {
            Console.Write("Введіть ім'я або прізвище: ");
            string searchName = Console.ReadLine()!;

            Patient[] results = patientManager.FindByName(searchName);

            if (results.Length == 0)
            {
                Console.WriteLine("Пацієнтів не знайдено.");
            }
            else
            {
                Console.WriteLine("Знайдені пацієнти:");

                for (int i = 0; i < results.Length; i++)
                {
                    Console.WriteLine(results[i]);
                }
            }
        }
        else if (choice == "4")
        {
            Console.Write("Введіть ID пацієнта для видалення: ");
            int id = int.Parse(Console.ReadLine()!);

            if (patientManager.Remove(id))
            {
                Console.WriteLine("Пацієнта видалено.");
            }
            else
            {
                Console.WriteLine("Пацієнта з таким ID не знайдено.");
            }
        }
        else if (choice == "5")
        {
            patientManager.DisplayStats();
        }
        else if (choice == "0")
        {
            break;
        }
        else
        {
            Console.WriteLine("Невірний вибір.");
        }
    }
}

void DoctorsMenu()
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("=== Лікарі ===");
        Console.WriteLine("1. Показати всіх");
        Console.WriteLine("2. Додати");
        Console.WriteLine("3. Знайти за спеціальністю");
        Console.WriteLine("4. Видалити");
        Console.WriteLine("5. Статистика");
        Console.WriteLine("6. Перевірити доступність на годину");
        Console.WriteLine("0. Назад");
        Console.Write("Ваш вибір: ");

        string choice = Console.ReadLine()!;

        Console.WriteLine();

        if (choice == "1")
        {
            doctorManager.DisplayAll();
        }
        else if (choice == "2")
        {
            Console.Write("Ім'я: ");
            string firstName = Console.ReadLine()!;

            Console.Write("Прізвище: ");
            string lastName = Console.ReadLine()!;

            Console.Write("Спеціальність: ");
            string speciality = Console.ReadLine()!;

            Console.Write("Номер ліцензії: ");
            string licenseNumber = Console.ReadLine()!;

            Console.Write("Телефон: ");
            string phone = Console.ReadLine()!;

            Doctor doctor = new Doctor(
                firstName,
                lastName,
                speciality,
                licenseNumber,
                phone);

            doctorManager.Add(doctor);
        }
        else if (choice == "3")
        {
            Console.Write("Введіть спеціальність: ");
            string speciality = Console.ReadLine()!;

            Doctor[] results = doctorManager.FindBySpeciality(speciality);

            if (results.Length == 0)
            {
                Console.WriteLine("Лікарів не знайдено.");
            }
            else
            {
                Console.WriteLine("Знайдені лікарі:");

                for (int i = 0; i < results.Length; i++)
                {
                    Console.WriteLine(results[i]);
                }
            }
        }
        else if (choice == "4")
        {
            Console.Write("Введіть ID лікаря для видалення: ");
            int id = int.Parse(Console.ReadLine()!);

            if (doctorManager.Remove(id))
            {
                Console.WriteLine("Лікаря видалено.");
            }
            else
            {
                Console.WriteLine("Лікаря з таким ID не знайдено.");
            }
        }
        else if (choice == "5")
        {
            doctorManager.DisplayStats();
        }
        else if (choice == "6")
        {
            Console.Write("Введіть годину (0-23): ");
            string input = Console.ReadLine()!;

            if (int.TryParse(input, out int hour) && hour >= 0 && hour <= 23)
            {
                Doctor[] doctors = doctorManager.GetAll();

                if (doctors.Length == 0)
                {
                    Console.WriteLine("Список лікарів порожній.");
                }
                else
                {
                    Console.WriteLine($"Доступність лікарів о {hour:D2}:00:");

                    for (int i = 0; i < doctors.Length; i++)
                    {
                        if (doctors[i].CanAcceptAt(hour))
                        {
                            Console.WriteLine($"{doctors[i].FullName} — доступний");
                        }
                        else
                        {
                            Console.WriteLine($"{doctors[i].FullName} — недоступний");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Некоректна година. Введіть число від 0 до 23.");
            }
        }
        else if (choice == "0")
        {
            break;
        }
        else
        {
            Console.WriteLine("Невірний вибір.");
        }
    }
}

PatientsMenu();
DoctorsMenu();