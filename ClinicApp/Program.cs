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

AppointmentManager appointmentManager = new AppointmentManager(
    patientManager,
    doctorManager);

Appointment appointment1 = new Appointment(
    patient1.Id,
    doctor1.Id,
    new DateTime(2026, 9, 26, 10, 0, 0));

Appointment appointment2 = new Appointment(
    patient2.Id,
    doctor2.Id,
    new DateTime(2026, 9, 26, 11, 0, 0),
    45);

Appointment appointment3 = new Appointment(
    patient3.Id,
    doctor3.Id,
    new DateTime(2026, 9, 27, 9, 0, 0),
    20);

Console.WriteLine();
Console.WriteLine("=== Записи на прийом ===");
Console.WriteLine(appointment1);
Console.WriteLine(appointment2);
Console.WriteLine(appointment3);

Console.WriteLine();
Console.WriteLine("=== Перевірка статусів ===");

if (appointment1.Cancel("Пацієнт не зміг прийти"))
{
    Console.WriteLine("Запис №1 скасовано.");
}
else
{
    Console.WriteLine("Запис №1 вже не має статус Scheduled.");
}

if (appointment2.Complete())
{
    Console.WriteLine("Запис №2 завершено.");
}
else
{
    Console.WriteLine("Запис №2 вже не має статус Scheduled.");
}

Console.WriteLine();
Console.WriteLine(appointment1);
Console.WriteLine(appointment2);

Console.WriteLine();
Console.WriteLine("=== Перевірка повторного переходу ===");

if (appointment1.Complete())
{
    Console.WriteLine("Запис №1 завершено.");
}
else
{
    Console.WriteLine("Не можна завершити скасований запис.");
}

if (appointment2.Cancel("Скасування після завершення"))
{
    Console.WriteLine("Запис №2 скасовано.");
}
else
{
    Console.WriteLine("Не можна скасувати завершений запис.");
}

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

void AppointmentsMenu()
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("=== Записи ===");
        Console.WriteLine("1. Показати майбутні записи");
        Console.WriteLine("2. Записати пацієнта");
        Console.WriteLine("3. Записи пацієнта");
        Console.WriteLine("4. Записи лікаря");
        Console.WriteLine("5. Записи на дату");
        Console.WriteLine("6. Скасувати запис");
        Console.WriteLine("7. Завершити запис");
        Console.WriteLine("0. Назад");
        Console.Write("Ваш вибір: ");

        string choice = Console.ReadLine()!;

        Console.WriteLine();

        if (choice == "1")
        {
            Console.WriteLine("Майбутні записи:");

            Appointment[] appointments = appointmentManager.GetUpcoming();

            appointmentManager.DisplayList(appointments);
        }
        else if (choice == "2")
        {
            Console.WriteLine("=== Пацієнти ===");
            patientManager.DisplayAll();

            Console.WriteLine();
            Console.WriteLine("=== Лікарі ===");
            doctorManager.DisplayAll();

            Console.WriteLine();

            Console.Write("Введіть ID пацієнта: ");
            string patientInput = Console.ReadLine()!;

            if (!int.TryParse(patientInput, out int patientId))
            {
                Console.WriteLine("Некоректний ID пацієнта.");
                continue;
            }

            Console.Write("Введіть ID лікаря: ");
            string doctorInput = Console.ReadLine()!;

            if (!int.TryParse(doctorInput, out int doctorId))
            {
                Console.WriteLine("Некоректний ID лікаря.");
                continue;
            }

            Console.Write("Дата та час прийому (рррр-мм-дд гг:хх): ");
            string dateInput = Console.ReadLine()!;

            if (!DateTime.TryParse(dateInput, out DateTime scheduledAt))
            {
                Console.WriteLine("Некоректна дата або час.");
                continue;
            }

            Console.Write("Тривалість у хвилинах (за замовчуванням 30): ");
            string durationInput = Console.ReadLine()!;

            int durationMinutes = 30;

            if (durationInput.Length > 0)
            {
                if (!int.TryParse(durationInput, out durationMinutes))
                {
                    Console.WriteLine("Некоректна тривалість.");
                    continue;
                }
            }

            appointmentManager.Book(
                patientId,
                doctorId,
                scheduledAt,
                durationMinutes);
        }
        else if (choice == "3")
        {
            patientManager.DisplayAll();

            Console.WriteLine();
            Console.Write("Введіть ID пацієнта: ");
            string input = Console.ReadLine()!;

            if (int.TryParse(input, out int patientId))
            {
                Appointment[] appointments =
                    appointmentManager.GetByPatient(patientId);

                Console.WriteLine();
                Console.WriteLine($"Записи пацієнта #{patientId}:");

                appointmentManager.DisplayList(appointments);
            }
            else
            {
                Console.WriteLine("Некоректний ID.");
            }
        }
        else if (choice == "4")
        {
            doctorManager.DisplayAll();

            Console.WriteLine();
            Console.Write("Введіть ID лікаря: ");
            string input = Console.ReadLine()!;

            if (int.TryParse(input, out int doctorId))
            {
                Appointment[] appointments =
                    appointmentManager.GetByDoctor(doctorId);

                Console.WriteLine();
                Console.WriteLine($"Записи лікаря #{doctorId}:");

                appointmentManager.DisplayList(appointments);
            }
            else
            {
                Console.WriteLine("Некоректний ID.");
            }
        }
        else if (choice == "5")
        {
            Console.Write("Введіть дату (рррр-мм-дд): ");
            string input = Console.ReadLine()!;

            if (DateTime.TryParse(input, out DateTime date))
            {
                Appointment[] appointments =
                    appointmentManager.GetByDate(date);

                Console.WriteLine();
                Console.WriteLine($"Записи на {date:dd.MM.yyyy}:");

                appointmentManager.DisplayList(appointments);
            }
            else
            {
                Console.WriteLine("Некоректна дата.");
            }
        }
        else if (choice == "6")
        {
            Console.Write("Введіть ID запису: ");
            string input = Console.ReadLine()!;

            if (!int.TryParse(input, out int id))
            {
                Console.WriteLine("Некоректний ID.");
                continue;
            }

            Console.Write("Причина скасування: ");
            string reason = Console.ReadLine()!;

            if (appointmentManager.Cancel(id, reason))
            {
                Console.WriteLine($"Запис [{id}] скасовано.");
            }
            else
            {
                Console.WriteLine(
                    "Не вдалося скасувати запис. " +
                    "Можливо, запис не знайдено або він уже завершений/скасований.");
            }
        }
        else if (choice == "7")
        {
            Console.Write("Введіть ID запису: ");
            string input = Console.ReadLine()!;

            if (!int.TryParse(input, out int id))
            {
                Console.WriteLine("Некоректний ID.");
                continue;
            }

            if (appointmentManager.Complete(id))
            {
                Console.WriteLine($"Запис [{id}] завершено.");
            }
            else
            {
                Console.WriteLine(
                    "Не вдалося завершити запис. " +
                    "Можливо, запис не знайдено або він уже завершений/скасований.");
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
AppointmentsMenu();