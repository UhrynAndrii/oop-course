using ClinicApp;
using ClinicApp.Enums;
using ClinicApp.Interfaces;
using ClinicApp.Managers;
using ClinicApp.Models;
using System;
using System.Globalization;
using System.Threading;
using ClinicApp.Comparators;
using System.Collections.Generic;
using ClinicApp.Utils;

Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Clinic clinic = new Clinic("Медична Клініка");

Patient patient1 = new Patient(
    "Андрій",
    "Мельник",
    new DateTime(1988, 4, 18),
    BloodType.APositive,
    "0504567891");

Patient patient2 = new Patient(
    "Софія",
    "Романюк",
    new DateTime(1995, 9, 7),
    BloodType.BNegative,
    "0678123456");

Patient patient3 = new Patient(
    "Данило",
    "Шевчук",
    new DateTime(2011, 2, 25),
    BloodType.OPositive,
    "0935678123");

Patient patient4 = new Patient();

Patient patient5 = new Patient(
    "Катерина",
    "Лисенко");

clinic.Patients.Add(patient1);
clinic.Patients.Add(patient2);
clinic.Patients.Add(patient3);
clinic.Patients.Add(patient4);
clinic.Patients.Add(patient5);

Doctor doctor1 = new Doctor(
    "Віктор",
    "Кравченко",
    Speciality.Cardiology,
    "LIC-101",
    "0444567890");

Doctor doctor2 = new Doctor(
    "Ірина",
    "Бондар",
    Speciality.Neurology,
    "LIC-102",
    "0445678901");

Doctor doctor3 = new Doctor(
    "Тарас",
    "Гнатюк",
    Speciality.Pediatrics,
    "LIC-103",
    "0446789012");

clinic.Doctors.Add(doctor1);
clinic.Doctors.Add(doctor2);
clinic.Doctors.Add(doctor3);

// Медичні записи пацієнта №1
clinic.MedicalRecords.Add(new Diagnosis(
    patient1.Id,
    doctor1.Id,
    DateTime.Today.AddDays(-30),
    "I10",
    "Гіпертонічна хвороба",
    true));

clinic.MedicalRecords.Add(new Diagnosis(
    patient1.Id,
    doctor2.Id,
    DateTime.Today.AddDays(-5),
    "J06.9",
    "Гострий ринофарингіт",
    false));

clinic.MedicalRecords.Add(new LabResult(
    patient1.Id,
    doctor1.Id,
    DateTime.Today.AddDays(-4),
    "Гемоглобін",
    145,
    "г/л",
    "120–160",
    true));

clinic.MedicalRecords.Add(new LabResult(
    patient1.Id,
    doctor2.Id,
    DateTime.Today.AddDays(-3),
    "Холестерин",
    6.2,
    "ммоль/л",
    "< 5.2",
    false));

clinic.MedicalRecords.Add(new Prescription(
    patient1.Id,
    doctor1.Id,
    DateTime.Today.AddDays(-5),
    "Лізиноприл",
    "10 мг",
    30,
    "1 раз на добу вранці"));

// Медичні записи пацієнта №2
clinic.MedicalRecords.Add(new Prescription(
    patient2.Id,
    doctor2.Id,
    DateTime.Today.AddDays(-40),
    "Парацетамол",
    "500 мг",
    10,
    "За потреби"));

clinic.MedicalRecords.Add(new Diagnosis(
    patient2.Id,
    doctor2.Id,
    DateTime.Today.AddDays(-2),
    "M54.5",
    "Біль у попереку",
    false));

// Медичний запис пацієнта №3
clinic.MedicalRecords.Add(new LabResult(
    patient3.Id,
    doctor3.Id,
    DateTime.Today.AddDays(-1),
    "Глюкоза",
    5.1,
    "ммоль/л",
    "3.9–5.5",
    true));

clinic.Appointments.Book(
    patient1.Id,
    doctor1.Id,
    new DateTime(2026, 9, 26, 10, 0, 0));

clinic.Appointments.BookUrgent(
    patient2.Id,
    doctor2.Id,
    new DateTime(2026, 9, 26, 11, 0, 0),
    "терміновий огляд",
    45);

clinic.Appointments.BookSpecialist(
    patient3.Id,
    doctor3.Id,
    new DateTime(2026, 9, 27, 9, 0, 0),
    "кардіологія",
    20);

UrgentAppointment urgentAppointment = new UrgentAppointment(
    patient1.Id,
    doctor1.Id,
    new DateTime(2026, 9, 27, 10, 0, 0),
    "тест");

Appointment appointmentReference = urgentAppointment;
UrgentAppointment urgentReference = urgentAppointment;

// GetDescription() є virtual/override, тому виконується реалізація UrgentAppointment.
// GetPriority() прихований через new, тому вибір методу залежить від типу змінної.
// Щоб GetPriority() був поліморфним, у Appointment потрібно зробити його virtual,
// а в UrgentAppointment замінити new на override.

Console.WriteLine();
Console.WriteLine("== new vs override ==");
Console.WriteLine(
    $"Через Appointment:       {appointmentReference.GetDescription()} | " +
    $"пріоритет {appointmentReference.GetPriority()}");
Console.WriteLine(
    $"Через UrgentAppointment: {urgentReference.GetDescription()} | " +
    $"пріоритет {urgentReference.GetPriority()}");

void PatientsMenu(Clinic clinic)
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
            clinic.Patients.DisplayAll();
        }
        else if (choice == "2")
        {
            try
            {
                Console.Write("Ім'я: ");
                string firstName = Console.ReadLine()!;

                Console.Write("Прізвище: ");
                string lastName = Console.ReadLine()!;

                Console.Write("Дата народження (рррр-мм-дд): ");
                DateTime dateOfBirth = DateTime.Parse(Console.ReadLine()!);

                Console.WriteLine("Оберіть групу крові:");
                Console.WriteLine("0 - Unknown");
                Console.WriteLine("1 - APositive");
                Console.WriteLine("2 - ANegative");
                Console.WriteLine("3 - BPositive");
                Console.WriteLine("4 - BNegative");
                Console.WriteLine("5 - ABPositive");
                Console.WriteLine("6 - ABNegative");
                Console.WriteLine("7 - OPositive");
                Console.WriteLine("8 - ONegative");
                Console.Write("Ваш вибір: ");

                int bloodTypeNumber = int.Parse(Console.ReadLine()!);
                BloodType bloodType = (BloodType)bloodTypeNumber;

                Console.Write("Телефон: ");
                string phone = Console.ReadLine()!;

                Patient patient = new Patient(
                    firstName,
                    lastName,
                    dateOfBirth,
                    bloodType,
                    phone);

                clinic.Patients.Add(patient);

                clinic.Logger.LogInfo(
                    $"Додано пацієнта #{patient.Id}: {patient.FullName}");

                Console.WriteLine("Пацієнта додано.");
            }
            catch (ArgumentOutOfRangeException e)
            {
                clinic.Logger.LogWarning("Помилка введення: " + e.Message);
                Console.WriteLine("Помилка: " + e.Message);
            }
            catch (ArgumentException e)
            {
                clinic.Logger.LogWarning("Помилка введення: " + e.Message);
                Console.WriteLine("Помилка: " + e.Message);
            }
            catch (FormatException)
            {
                clinic.Logger.LogWarning("Помилка введення: некоректний формат даних.");
                Console.WriteLine("Помилка: некоректний формат введених даних.");
            }
        }
        else if (choice == "3")
        {
            Console.Write("Введіть ім'я або прізвище: ");
            string searchName = Console.ReadLine()!;

            Patient[] results = clinic.Patients.FindByName(searchName);

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

            if (clinic.Patients.Remove(id))
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
            clinic.Patients.DisplayStats();
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

void DoctorsMenu(Clinic clinic)
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("=== Лікарі ===");
        Console.WriteLine("1. Показати всіх");
        Console.WriteLine("2. Додати");
        Console.WriteLine("3. Знайти за спеціальністю");
        Console.WriteLine("4. Видалити");
        Console.WriteLine("5. Вільні години лікаря");
        Console.WriteLine("6. Статистика");
        Console.WriteLine("7. Перевірити доступність на годину");
        Console.WriteLine("0. Назад");
        Console.Write("Ваш вибір: ");

        string choice = Console.ReadLine()!;

        Console.WriteLine();

        if (choice == "1")
        {
            clinic.Doctors.DisplayAll();
        }
        else if (choice == "2")
        {
            try
            {
                Console.Write("Ім'я: ");
                string firstName = Console.ReadLine()!;

                Console.Write("Прізвище: ");
                string lastName = Console.ReadLine()!;

                Console.WriteLine("Оберіть спеціальність:");

                Console.WriteLine("0 - General");
                Console.WriteLine("1 - Cardiology");
                Console.WriteLine("2 - Neurology");
                Console.WriteLine("3 - Pediatrics");
                Console.WriteLine("4 - Surgery");
                Console.WriteLine("5 - Orthopedics");
                Console.WriteLine("6 - Dermatology");
                Console.WriteLine("7 - Emergency");

                Console.Write("Ваш вибір: ");

                int specialityNumber = int.Parse(Console.ReadLine()!);
                Speciality speciality = (Speciality)specialityNumber;

                Console.Write("Номер ліцензії: ");
                string licenseNumber = Console.ReadLine()!;

                Console.Write("Телефон: ");
                string phone = Console.ReadLine()!;

                Console.Write("Початок робочого дня (0-23): ");
                int start = int.Parse(Console.ReadLine()!);

                Console.Write("Кінець робочого дня (1-24): ");
                int end = int.Parse(Console.ReadLine()!);

                WorkSchedule schedule = new WorkSchedule(start, end);

                Doctor doctor = new Doctor(
                    firstName,
                    lastName,
                    speciality,
                    licenseNumber,
                    phone);

                doctor.Schedule = schedule;

                clinic.Doctors.Add(doctor);

                clinic.Logger.LogInfo(
                    $"Додано лікаря #{doctor.Id}: {doctor.FullName}");

                Console.WriteLine("Лікаря додано.");
            }
            catch (ArgumentOutOfRangeException e)
            {
                clinic.Logger.LogWarning("Помилка введення: " + e.Message);
                Console.WriteLine("Помилка: " + e.Message);
            }
            catch (ArgumentException e)
            {
                clinic.Logger.LogWarning("Помилка введення: " + e.Message);
                Console.WriteLine("Помилка: " + e.Message);
            }
            catch (FormatException)
            {
                clinic.Logger.LogWarning("Помилка введення: некоректний формат даних.");
                Console.WriteLine("Помилка: некоректний формат введених даних.");
            }
        }
        else if (choice == "3")
        {
            Console.Write("Введіть спеціальність: ");
            string speciality = Console.ReadLine()!;

            Doctor[] results =
                clinic.Doctors.FindBySpeciality(speciality);

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

            if (clinic.Doctors.Remove(id))
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
            Console.Write("Введіть ID лікаря: ");

            if (!int.TryParse(Console.ReadLine(), out int doctorId))
            {
                Console.WriteLine("Некоректний ID лікаря.");
                continue;
            }

            Doctor? doctor = clinic.Doctors.FindById(doctorId);

            if (doctor == null)
            {
                Console.WriteLine("Лікаря не знайдено.");
                continue;
            }

            ISchedulable schedulable = doctor;

            Console.Write("Введіть дату (dd.MM.yyyy): ");

            if (!DateTime.TryParseExact(
                    Console.ReadLine(),
                    "dd.MM.yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime date))
            {
                Console.WriteLine("Некоректна дата. Використовуйте формат dd.MM.yyyy.");
                continue;
            }

            Console.Write("Введіть кількість слотів: ");

            if (!int.TryParse(Console.ReadLine(), out int slotCount))
            {
                Console.WriteLine("Некоректна кількість слотів.");
                continue;
            }

            try
            {
                DateTime[] slots = schedulable.GetAvailableSlots(date, slotCount);

                if (slots.Length == 0)
                {
                    Console.WriteLine("Вільних годин немає.");
                }
                else
                {
                    Console.WriteLine($"Вільні години лікаря {doctor.FullName}:");

                    for (int i = 0; i < slots.Length; i++)
                    {
                        Console.WriteLine(slots[i].ToString("HH:mm"));
                    }
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                clinic.Logger.LogWarning("Помилка введення: " + e.Message);
                Console.WriteLine("Помилка: " + e.Message);
            }
        }
        else if (choice == "6")
        {
            clinic.Doctors.DisplayStats();
        }
        else if (choice == "7")
        {
            Console.Write("Введіть годину (0-23): ");
            string input = Console.ReadLine()!;

            if (int.TryParse(input, out int hour) &&
                hour >= 0 &&
                hour <= 23)
            {
                Doctor[] doctors = clinic.Doctors.GetAll();

                if (doctors.Length == 0)
                {
                    Console.WriteLine("Список лікарів порожній.");
                }
                else
                {
                    Console.WriteLine(
                        $"Доступність лікарів о {hour:D2}:00:");

                    for (int i = 0; i < doctors.Length; i++)
                    {
                        if (doctors[i].CanAcceptAt(hour))
                        {
                            Console.WriteLine(
                                $"{doctors[i].FullName} — доступний");
                        }
                        else
                        {
                            Console.WriteLine(
                                $"{doctors[i].FullName} — недоступний");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine(
                    "Некоректна година. Введіть число від 0 до 23.");
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

void AppointmentsMenu(Clinic clinic)
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
        Console.WriteLine("8. Скасувати всі записи пацієнта");
        Console.WriteLine("9. За типом прийому");
        Console.WriteLine("10. Розклад на дату");
        Console.WriteLine("11. Згенерувати звіт");
        Console.WriteLine("0. Назад");
        Console.Write("Ваш вибір: ");

        string choice = Console.ReadLine()!;

        Console.WriteLine();

        if (choice == "1")
        {
            Console.WriteLine("Майбутні записи:");

            Appointment[] appointments =
                clinic.Appointments.GetUpcoming();

            clinic.Appointments.DisplayList(appointments);
        }
        else if (choice == "2")
        {
            try
            {
                Console.WriteLine("=== Пацієнти ===");
                clinic.Patients.DisplayAll();

                Console.WriteLine();
                Console.WriteLine("=== Лікарі ===");
                clinic.Doctors.DisplayAll();

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

                Console.Write(
                    "Дата та час прийому (рррр-мм-дд гг:хх): ");

                string dateInput = Console.ReadLine()!;

                if (!DateTime.TryParse(
                        dateInput,
                        out DateTime scheduledAt))
                {
                    Console.WriteLine("Некоректна дата або час.");
                    continue;
                }

                Console.Write(
                    "Тривалість у хвилинах " +
                    "(за замовчуванням 30): ");

                string durationInput = Console.ReadLine()!;

                int durationMinutes = 30;

                if (durationInput.Length > 0)
                {
                    if (!int.TryParse(
                            durationInput,
                            out durationMinutes))
                    {
                        Console.WriteLine("Некоректна тривалість.");
                        continue;
                    }
                }

                clinic.Appointments.Book(
                    patientId,
                    doctorId,
                    scheduledAt,
                    durationMinutes);

                clinic.Logger.LogInfo(
                    $"Створено запис: пацієнт #{patientId} → лікар #{doctorId}, " +
                    $"дата {scheduledAt:dd.MM.yyyy HH:mm}");

                Console.WriteLine("Запис створено.");
            }
            catch (ArgumentOutOfRangeException e)
            {
                clinic.Logger.LogWarning("Помилка введення: " + e.Message);
                Console.WriteLine("Помилка: " + e.Message);
            }
            catch (ArgumentException e)
            {
                clinic.Logger.LogWarning("Помилка введення: " + e.Message);
                Console.WriteLine("Помилка: " + e.Message);
            }
        }
        else if (choice == "3")
        {
            clinic.Patients.DisplayAll();

            Console.WriteLine();
            Console.Write("Введіть ID пацієнта: ");

            string input = Console.ReadLine()!;

            if (int.TryParse(input, out int patientId))
            {
                Appointment[] appointments =
                    clinic.Appointments.GetByPatient(patientId);

                Console.WriteLine();
                Console.WriteLine(
                    $"Записи пацієнта #{patientId}:");

                clinic.Appointments.DisplayList(appointments);
            }
            else
            {
                Console.WriteLine("Некоректний ID.");
            }
        }
        else if (choice == "4")
        {
            clinic.Doctors.DisplayAll();

            Console.WriteLine();
            Console.Write("Введіть ID лікаря: ");

            string input = Console.ReadLine()!;

            if (int.TryParse(input, out int doctorId))
            {
                Appointment[] appointments =
                    clinic.Appointments.GetByDoctor(doctorId);

                Console.WriteLine();
                Console.WriteLine(
                    $"Записи лікаря #{doctorId}:");

                clinic.Appointments.DisplayList(appointments);
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
                    clinic.Appointments.GetByDate(date);

                Console.WriteLine();
                Console.WriteLine(
                    $"Записи на {date:dd.MM.yyyy}:");

                clinic.Appointments.DisplayList(appointments);
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

            if (clinic.Appointments.Cancel(id, reason))
            {
                clinic.Logger.LogInfo(
                    $"Запис #{id} скасовано. Причина: {reason}");

                Console.WriteLine(
                    $"Запис [{id}] скасовано.");
            }
            else
            {
                Console.WriteLine(
                    "Не вдалося скасувати запис. " +
                    "Можливо, запис не знайдено або " +
                    "він уже завершений/скасований.");
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

            if (clinic.Appointments.Complete(id))
            {
                clinic.Logger.LogInfo(
                    $"Запис #{id} завершено.");

                Console.WriteLine(
                    $"Запис [{id}] завершено.");
            }
            else
            {
                Console.WriteLine(
                    "Не вдалося завершити запис. " +
                    "Можливо, запис не знайдено або " +
                    "він уже завершений/скасований.");
            }
        }
        else if (choice == "8")
        {
            Console.Write("Введіть ID пацієнта: ");

            if (!int.TryParse(Console.ReadLine(), out int patientId))
            {
                Console.WriteLine("Некоректний ID пацієнта.");
                continue;
            }

            Console.Write("Причина скасування (Enter — без причини): ");
            string reason = Console.ReadLine()!;

            Appointment[] patientAppointments =
                clinic.Appointments.GetByPatient(patientId);

            int cancelledCount = AppointmentManager.CancelAll(
                patientAppointments,
                reason);

            if (cancelledCount > 0)
            {
                clinic.Logger.LogInfo(
                    $"Масово скасовано записів пацієнта #{patientId}: " +
                    $"{cancelledCount}. Причина: {reason}");
            }

            Console.WriteLine($"Скасовано записів: {cancelledCount}");
        }

        else if (choice == "9")
        {
            Console.WriteLine("1. Термінові");
            Console.WriteLine("2. Консультації спеціаліста");
            Console.WriteLine("3. Звичайні");
            Console.Write("Оберіть тип прийому: ");

            string typeChoice = Console.ReadLine()!;

            Appointment[] appointments;

            if (typeChoice == "1")
            {
                appointments = clinic.Appointments.GetUrgent();
                Console.WriteLine("Термінові записи:");
            }
            else if (typeChoice == "2")
            {
                appointments = clinic.Appointments.GetSpecialist();
                Console.WriteLine("Консультації спеціаліста:");
            }
            else if (typeChoice == "3")
            {
                appointments = clinic.Appointments.GetRegular();
                Console.WriteLine("Звичайні записи:");
            }
            else
            {
                Console.WriteLine("Невірний вибір.");
                continue;
            }

            clinic.Appointments.DisplayList(appointments);
        }

        else if (choice == "10")
        {
            Console.Write("Введіть дату (рррр-мм-дд): ");

            string input = Console.ReadLine()!;

            if (DateTime.TryParse(input, out DateTime date))
            {
                clinic.DisplaySchedule(date);
            }
            else
            {
                Console.WriteLine("Некоректна дата.");
            }
        }

        else if (choice == "11")
        {
            clinic.GenerateReport();
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

void MedicalRecordsMenu(Clinic clinic)
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("── Медична картка ────────────");
        Console.WriteLine("1. Картка пацієнта");
        Console.WriteLine("2. Усі записи пацієнта");
        Console.WriteLine("3. Додати діагноз");
        Console.WriteLine("4. Додати аналіз");
        Console.WriteLine("5. Додати рецепт");
        Console.WriteLine("6. Записи лікаря");
        Console.WriteLine("0. Назад");
        Console.Write("Оберіть: ");

        string choice = Console.ReadLine()!;

        Console.WriteLine();

        if (choice == "1")
        {
            clinic.Patients.DisplayAll();

            Console.Write("ID пацієнта: ");

            if (!int.TryParse(Console.ReadLine(), out int patientId))
            {
                Console.WriteLine("Некоректний ID пацієнта.");
                continue;
            }

            if (clinic.Patients.FindById(patientId) == null)
            {
                Console.WriteLine("Пацієнта не знайдено.");
                continue;
            }

            clinic.MedicalRecords.DisplayPatientSummary(patientId);
        }
        else if (choice == "2")
        {
            clinic.Patients.DisplayAll();

            Console.Write("ID пацієнта: ");

            if (!int.TryParse(Console.ReadLine(), out int patientId))
            {
                Console.WriteLine("Некоректний ID пацієнта.");
                continue;
            }

            if (clinic.Patients.FindById(patientId) == null)
            {
                Console.WriteLine("Пацієнта не знайдено.");
                continue;
            }

            MedicalRecord[] records =
                clinic.MedicalRecords.GetByPatient(patientId);

            if (records.Length == 0)
            {
                Console.WriteLine("Медичних записів не знайдено.");
            }
            else
            {
                for (int i = 0; i < records.Length; i++)
                {
                    Console.WriteLine(
                        $"{records[i].GetRecordType()}: {records[i].GetSummary()}");
                }
            }

        }
        else if (choice == "3")
        {
            clinic.Patients.DisplayAll();
            Console.WriteLine();
            clinic.Doctors.DisplayAll();
            Console.WriteLine();

            Console.Write("ID пацієнта: ");

            if (!int.TryParse(Console.ReadLine(), out int patientId))
            {
                Console.WriteLine("Некоректний ID пацієнта.");
                continue;
            }

            if (clinic.Patients.FindById(patientId) == null)
            {
                Console.WriteLine("Пацієнта не знайдено.");
                continue;
            }

            Console.Write("ID лікаря: ");

            if (!int.TryParse(Console.ReadLine(), out int doctorId))
            {
                Console.WriteLine("Некоректний ID лікаря.");
                continue;
            }

            if (clinic.Doctors.FindById(doctorId) == null)
            {
                Console.WriteLine("Лікаря не знайдено.");
                continue;
            }

            Console.Write("Код діагнозу (напр. J06.9): ");
            string diagnosisCode = Console.ReadLine()!;

            Console.Write("Опис: ");
            string description = Console.ReadLine()!;

            Console.Write("Хронічне? (1=так, 0=ні): ");
            string chronicInput = Console.ReadLine()!;

            if (chronicInput != "1" && chronicInput != "0")
            {
                Console.WriteLine("Введіть 1 або 0.");
                continue;
            }

            try
            {
                Diagnosis diagnosis = new Diagnosis(
                    patientId,
                    doctorId,
                    DateTime.Today,
                    diagnosisCode,
                    description,
                    chronicInput == "1");

                clinic.MedicalRecords.Add(diagnosis);
            }
            catch (ArgumentOutOfRangeException e)
            {
                clinic.Logger.LogWarning("Помилка введення: " + e.Message);
                Console.WriteLine("Помилка: " + e.Message);
            }
            catch (ArgumentException e)
            {
                clinic.Logger.LogWarning("Помилка введення: " + e.Message);
                Console.WriteLine("Помилка: " + e.Message);
            }
        }
        else if (choice == "4")
        {
            clinic.Patients.DisplayAll();
            Console.WriteLine();
            clinic.Doctors.DisplayAll();
            Console.WriteLine();

            Console.Write("ID пацієнта: ");

            if (!int.TryParse(Console.ReadLine(), out int patientId))
                {
                    Console.WriteLine("Некоректний ID пацієнта.");
                    continue;
                }

            if (clinic.Patients.FindById(patientId) == null)
            {
                Console.WriteLine("Пацієнта не знайдено.");
                continue;
            }

            Console.Write("ID лікаря: ");

            if (!int.TryParse(Console.ReadLine(), out int doctorId))
            {
                Console.WriteLine("Некоректний ID лікаря.");
                continue;
            }

            if (clinic.Doctors.FindById(doctorId) == null)
            {
                Console.WriteLine("Лікаря не знайдено.");
                continue;
            }

            Console.Write("Назва аналізу: ");
            string testName = Console.ReadLine()!;

            Console.Write("Значення (число): ");

            if (!double.TryParse(
                Console.ReadLine(),
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture,
                out double value))
            {
                Console.WriteLine(
                "Некоректне число. Використовуйте крапку, наприклад 6.2.");
                continue;
            }

            Console.Write("Одиниці виміру: ");
            string unit = Console.ReadLine()!;

            Console.Write("Норма (напр. 4.0–9.0): ");
            string referenceRange = Console.ReadLine()!;

            Console.Write("В нормі? (1=так, 0=ні): ");
            string normalInput = Console.ReadLine()!;

            if (normalInput != "1" && normalInput != "0")
            {
                Console.WriteLine("Введіть 1 або 0.");
                continue;
            }

            try
            {
                LabResult labResult = new LabResult(
                patientId,
                doctorId,
                DateTime.Today,
                testName,
                value,
                unit,
                referenceRange,
                normalInput == "1");

                clinic.MedicalRecords.Add(labResult);
            }
            catch (ArgumentOutOfRangeException e)
            {
                clinic.Logger.LogWarning("Помилка введення: " + e.Message);
                Console.WriteLine("Помилка: " + e.Message);
            }
            catch (ArgumentException e)
            {
                clinic.Logger.LogWarning("Помилка введення: " + e.Message);
                Console.WriteLine("Помилка: " + e.Message);
            }
        }
        else if (choice == "5")
        {
            clinic.Patients.DisplayAll();
            Console.WriteLine();
            clinic.Doctors.DisplayAll();
            Console.WriteLine();

            Console.Write("ID пацієнта: ");

            if (!int.TryParse(Console.ReadLine(), out int patientId))
            {
                Console.WriteLine("Некоректний ID пацієнта.");
                continue;
            }

            if (clinic.Patients.FindById(patientId) == null)
            {
                Console.WriteLine("Пацієнта не знайдено.");
                continue;
            }

            Console.Write("ID лікаря: ");

            if (!int.TryParse(Console.ReadLine(), out int doctorId))
            {
                Console.WriteLine("Некоректний ID лікаря.");
                continue;
            }

            if (clinic.Doctors.FindById(doctorId) == null)
            {
                Console.WriteLine("Лікаря не знайдено.");
                continue;
            }

            Console.Write("Препарат: ");
            string medicationName = Console.ReadLine()!;

            Console.Write("Дозування (напр. 10 мг): ");
            string dosage = Console.ReadLine()!;

            Console.Write("Кількість днів: ");

            if (!int.TryParse(Console.ReadLine(), out int durationDays))
            {
                Console.WriteLine("Некоректна кількість днів.");
                continue;
            }

            Console.Write("Інструкція (Enter — пропустити): ");
            string instructions = Console.ReadLine()!;

            try
            {
                Prescription prescription = new Prescription(
                patientId,
                doctorId,
                DateTime.Today,
                medicationName,
                dosage,
                durationDays,
                instructions);

                clinic.MedicalRecords.Add(prescription);
            }
            catch (ArgumentOutOfRangeException e)
            {
                clinic.Logger.LogWarning("Помилка введення: " + e.Message);
                Console.WriteLine("Помилка: " + e.Message);
            }
            catch (ArgumentException e)
            {
                clinic.Logger.LogWarning("Помилка введення: " + e.Message);
                Console.WriteLine("Помилка: " + e.Message);
            }
        }
        else if (choice == "6")
        {
            clinic.Doctors.DisplayAll();

            Console.Write("ID лікаря: ");

            if (!int.TryParse(Console.ReadLine(), out int doctorId))
            {
                Console.WriteLine("Некоректний ID лікаря.");
                continue;
            }

            if (clinic.Doctors.FindById(doctorId) == null)
            {
                Console.WriteLine("Лікаря не знайдено.");
                continue;
            }

            MedicalRecord[] records =
            clinic.MedicalRecords.GetByDoctor(doctorId);

            clinic.MedicalRecords.DisplayList(records);
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

void WaitingRoomMenu(Clinic clinic)
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("── Черга ───────────────────────");
        Console.WriteLine("1. Додати пацієнта до черги");
        Console.WriteLine("2. Прийняти першого");
        Console.WriteLine("3. Хто перший?");
        Console.WriteLine("4. Переглянути всю чергу");
        Console.WriteLine("0. Назад");
        Console.Write("Оберіть: ");

        string choice = Console.ReadLine()!;

        Console.WriteLine();

        if (choice == "1")
        {
            clinic.Patients.DisplayAll();

            Console.Write("Введіть ID пацієнта: ");

            if (!int.TryParse(Console.ReadLine(), out int patientId))
            {
                Console.WriteLine("Некоректний ID пацієнта.");
                continue;
            }

            Patient? patient = clinic.Patients.FindById(patientId);

            if (patient == null)
            {
                Console.WriteLine("Пацієнта не знайдено.");
            }
            else
            {
                clinic.WaitingRoom.Enqueue(patient);

                Console.WriteLine(
                    $"{patient.FullName} доданий до черги.");
            }
        }
        else if (choice == "2")
        {
            try
            {
                Patient patient = clinic.WaitingRoom.Dequeue();

                Console.WriteLine(
                    $"Прийнято: {patient.FullName}. " +
                    $"У черзі лишилось: {clinic.WaitingRoom.Count}");
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        else if (choice == "3")
        {
            try
            {
                Patient patient = clinic.WaitingRoom.Peek();

                Console.WriteLine(
                    $"Перший у черзі: {patient.FullName}");
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        else if (choice == "4")
        {
            Patient[] patients = clinic.WaitingRoom.ToArray();

            if (patients.Length == 0)
            {
                Console.WriteLine("Черга порожня.");
            }
            else
            {
                Console.WriteLine("Пацієнти в черзі:");

                for (int i = 0; i < patients.Length; i++)
                {
                    Console.WriteLine(
                        $"{i + 1}. {patients[i].FullName}");
                }
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

void TreatmentPlansMenu(Clinic clinic)
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("── Плани лікування ────────────────────");
        Console.WriteLine("1. Показати всі плани");
        Console.WriteLine("2. Додати план лікування");
        Console.WriteLine("3. Плани пацієнта");
        Console.WriteLine("4. Активувати план");
        Console.WriteLine("5. Завершити план");
        Console.WriteLine("6. Скасувати план");
        Console.WriteLine("7. Інформація про тип TreatmentPlan");
        Console.WriteLine("0. Назад");
        Console.Write("Оберіть: ");

        string choice = Console.ReadLine()!;

        Console.WriteLine();

        if (choice == "1")
        {
            TreatmentPlan[] plans = clinic.TreatmentPlans.GetAll();

            if (plans.Length == 0)
            {
                Console.WriteLine("Планів лікування немає.");
            }
            else
            {
                foreach (TreatmentPlan plan in plans)
                {
                    Console.WriteLine(
                        $"ID: {plan.Id} | " +
                        $"Пацієнт: {plan.PatientId} | " +
                        $"Діагноз: {plan.Diagnosis} | " +
                        $"Тривалість: {plan.DurationDays} днів | " +
                        $"Статус: {plan.Status}");
                }
            }
        }
        else if (choice == "2")
        {
            TreatmentPlan plan =
                FormBuilder.Build<TreatmentPlan>();

            if (clinic.TreatmentPlans.Add(plan))
            {
                Console.WriteLine("План лікування додано.");
            }
        }
        else if (choice == "3")
        {
            Console.Write("Введіть ID пацієнта: ");

            if (!int.TryParse(Console.ReadLine(), out int patientId))
            {
                Console.WriteLine("Некоректний ID пацієнта.");
                continue;
            }

            TreatmentPlan[] plans =
                clinic.TreatmentPlans.GetByPatient(patientId);

            if (plans.Length == 0)
            {
                Console.WriteLine("Планів цього пацієнта не знайдено.");
            }
            else
            {
                Console.WriteLine($"Плани пацієнта #{patientId}:");

                foreach (TreatmentPlan plan in plans)
                {
                    Console.WriteLine(
                        $"ID: {plan.Id} | " +
                        $"Діагноз: {plan.Diagnosis} | " +
                        $"Тривалість: {plan.DurationDays} днів | " +
                        $"Статус: {plan.Status}");
                }
            }
        }
        else if (choice == "4")
        {
            Console.Write("Введіть ID плану: ");

            if (!int.TryParse(Console.ReadLine(), out int planId))
            {
                Console.WriteLine("Некоректний ID плану.");
                continue;
            }

            TreatmentPlan? plan =
                clinic.TreatmentPlans.GetById(planId);

            if (plan == null)
            {
                Console.WriteLine("План не знайдено.");
            }
            else if (plan.Activate())
            {
                Console.WriteLine("План успішно активовано.");
            }
            else
            {
                Console.WriteLine("Неможливо активувати цей план.");
            }
        }
        else if (choice == "5")
        {
            Console.Write("Введіть ID плану: ");

            if (!int.TryParse(Console.ReadLine(), out int planId))
            {
                Console.WriteLine("Некоректний ID плану.");
                continue;
            }

            TreatmentPlan? plan =
                clinic.TreatmentPlans.GetById(planId);

            if (plan == null)
            {
                Console.WriteLine("План не знайдено.");
            }
            else if (plan.Complete())
            {
                Console.WriteLine("План успішно завершено.");
            }
            else
            {
                Console.WriteLine("Неможливо завершити цей план.");
            }
        }
        else if (choice == "6")
        {
            Console.Write("Введіть ID плану: ");

            if (!int.TryParse(Console.ReadLine(), out int planId))
            {
                Console.WriteLine("Некоректний ID плану.");
                continue;
            }

            TreatmentPlan? plan =
                clinic.TreatmentPlans.GetById(planId);

            if (plan == null)
            {
                Console.WriteLine("План не знайдено.");
            }
            else if (plan.Cancel())
            {
                Console.WriteLine("План успішно скасовано.");
            }
            else
            {
                Console.WriteLine("Неможливо скасувати цей план.");
            }
        }
        else if (choice == "7")
        {
            ModelValidator.PrintInfo(typeof(TreatmentPlan));
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

void FilesMenu(Clinic clinic)
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("── Файли ─────────────────────");
        Console.WriteLine("1. Останні рядки лога");
        Console.WriteLine("2. Очистити лог");
        Console.WriteLine("3. Експортувати всі звіти");
        Console.WriteLine("0. Назад");
        Console.Write("Оберіть: ");

        string choice = Console.ReadLine()!;

        Console.WriteLine();

        if (choice == "1")
        {
            Console.Write("Скільки останніх рядків показати? ");

            if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
            {
                Console.WriteLine("Введіть додатне ціле число.");
                continue;
            }

            if (!clinic.Logger.Exists())
            {
                Console.WriteLine("Файл лога ще не існує.");
                continue;
            }

            string[] lines = clinic.Logger.GetLastLines(n);

            Console.WriteLine();
            Console.WriteLine("=== Останні рядки лога ===");

            for (int i = 0; i < lines.Length; i++)
            {
                Console.WriteLine(lines[i]);
            }
        }
        else if (choice == "2")
        {
            clinic.Logger.Clear();

            Console.WriteLine("Лог очищено.");
        }
        else if (choice == "3")
        {
            string[] paths = clinic.Exporter.ExportAll();

            Console.WriteLine("Звіти успішно експортовано:");

            for (int i = 0; i < paths.Length; i++)
            {
                Console.WriteLine(paths[i]);
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

void BillingMenu(Clinic clinic)
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("── Рахунки ───────────────────");
        Console.WriteLine("1. Борги пацієнта");
        Console.WriteLine("2. Всі неоплачені записи");
        Console.WriteLine("3. Оплатити запис");
        Console.WriteLine("4. Загальний борг клініки");
        Console.WriteLine("0. Назад");
        Console.Write("Ваш вибір: ");

        string choice = Console.ReadLine()!;

        Console.WriteLine();

        if (choice == "1")
        {
            Console.Write("Введіть ID пацієнта: ");

            if (!int.TryParse(Console.ReadLine(), out int patientId))
            {
                Console.WriteLine("Некоректний ID пацієнта.");
                continue;
            }

            IPayable[] unpaid =
                clinic.Billing.GetUnpaidByPatient(patientId);

            Console.WriteLine(
                $"Неоплачені записи пацієнта #{patientId}:");

            clinic.Billing.DisplayUnpaid(unpaid);

            decimal debt = clinic.Billing.GetPatientDebt(patientId);

            Console.WriteLine(
                $"Борг пацієнта: {debt.ToString("F2")} грн");
        }
        else if (choice == "2")
        {
            IPayable[] unpaid = clinic.Billing.GetAllUnpaid();

            clinic.Billing.DisplayUnpaid(unpaid);
        }
        else if (choice == "3")
        {
            Console.Write("Введіть ID запису: ");

            if (!int.TryParse(Console.ReadLine(), out int appointmentId))
            {
                Console.WriteLine("Некоректний ID запису.");
                continue;
            }

            if (clinic.Billing.PayAppointment(appointmentId))
            {
                clinic.Logger.LogInfo(
                    $"Запис #{appointmentId} оплачено.");

                Console.WriteLine($"Запис [{appointmentId}] оплачено.");
            }
            else
            {
                Console.WriteLine(
                    "Не вдалося оплатити: запис не знайдено, вже оплачено або скасовано.");
            }
        }
        else if (choice == "4")
        {
            decimal debt = clinic.Billing.GetTotalDebt();

            Console.WriteLine(
                $"Загальний борг по клініці: {debt.ToString("F2")} грн");
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
List<DoctorStats> CollectDoctorStats(Clinic clinic)
{
    List<DoctorStats> stats = new List<DoctorStats>();

    foreach (DoctorStats doctorStat in clinic.Analytics.ComputeDoctorStats())
    {
        stats.Add(doctorStat);
    }

    return stats;
}

List<PatientStats> CollectPatientStats(Clinic clinic)
{
    List<PatientStats> stats = new List<PatientStats>();

    foreach (PatientStats patientStat in clinic.Analytics.ComputePatientStats())
    {
        stats.Add(patientStat);
    }

    return stats;
}

void AnalyticsMenu(Clinic clinic)
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("── Аналітика ───────────────────");
        Console.WriteLine("1. Лікарі за навантаженням");
        Console.WriteLine("2. Лікарі за виручкою");
        Console.WriteLine("3. Лікарі за іменем");
        Console.WriteLine("4. Пацієнти за кількістю візитів");
        Console.WriteLine("5. Пацієнти за витратами");
        Console.WriteLine("0. Назад");
        Console.Write("Оберіть: ");

        string choice = Console.ReadLine()!;

        Console.WriteLine();

        if (choice == "1")
        {
            List<DoctorStats> stats = CollectDoctorStats(clinic);
            stats.Sort();

            foreach (DoctorStats stat in stats)
            {
                Console.WriteLine(stat);
            }
        }
        else if (choice == "2")
        {
            List<DoctorStats> stats = CollectDoctorStats(clinic);
            stats.Sort(new DoctorStatsByRevenue());

            foreach (DoctorStats stat in stats)
            {
                Console.WriteLine(stat);
            }
        }
        else if (choice == "3")
        {
            List<DoctorStats> stats = CollectDoctorStats(clinic);
            stats.Sort(new DoctorStatsByName());

            foreach (DoctorStats stat in stats)
            {
                Console.WriteLine(stat);
            }
        }
        else if (choice == "4")
        {
            List<PatientStats> stats = CollectPatientStats(clinic);
            stats.Sort();

            foreach (PatientStats stat in stats)
            {
                Console.WriteLine(stat);
            }
        }
        else if (choice == "5")
        {
            List<PatientStats> stats = CollectPatientStats(clinic);
            stats.Sort(new PatientStatsBySpent());

            foreach (PatientStats stat in stats)
            {
                Console.WriteLine(stat);
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

while (true)
{
    Console.WriteLine();
    Console.WriteLine("╔════════════════════════════════════════════════╗");
    Console.WriteLine("║              МЕДИЧНА КЛІНІКА                  ║");
    Console.WriteLine("╠════════════════════════════════════════════════╣");
    Console.WriteLine("║  1. Пацієнти       — реєстрація, пошук         ║");
    Console.WriteLine("║  2. Лікарі         — персонал, розклад         ║");
    Console.WriteLine("║  3. Записи         — прийоми, скасування       ║");
    Console.WriteLine("║  4. Медична картка — діагнози, рецепти         ║");
    Console.WriteLine("║  5. Рахунки        — оплата, борги             ║");
    Console.WriteLine("║  6. Черга          — очікування, прийом        ║");
    Console.WriteLine("║  7. Звіт           — загальна статистика       ║");
    Console.WriteLine("║  8. Аналітика      — статистика лікарів і пацієнтів ║");
    Console.WriteLine("║  9. Плани лікування — рефлексія, атрибути      ║");
    Console.WriteLine("║ 10. Файли          — журнал дій                ║");
    Console.WriteLine("║  0. Вийти                                      ║");
    Console.WriteLine("╚════════════════════════════════════════════════╝");
    Console.Write("Оберіть: ");

    string choice = Console.ReadLine()!;

    Console.WriteLine();

    if (choice == "1")
    {
        PatientsMenu(clinic);
    }
    else if (choice == "2")
    {
        DoctorsMenu(clinic);
    }
    else if (choice == "3")
    {
        AppointmentsMenu(clinic);
    }
    else if (choice == "4")
    {
        MedicalRecordsMenu(clinic);
    }
    else if (choice == "5")
    {
        BillingMenu(clinic);
    }
    else if (choice == "6")
    {
        WaitingRoomMenu(clinic);
    }
    else if (choice == "7")
    {
        clinic.GenerateReport();
    }
    else if (choice == "8")
    {
        AnalyticsMenu(clinic);
    }
    else if (choice == "9")
    {
        TreatmentPlansMenu(clinic);
    }
    else if (choice == "10")
    {
        FilesMenu(clinic);
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