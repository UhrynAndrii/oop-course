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