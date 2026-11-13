using System;
using System.IO;
using System.Text;
using ClinicApp.Interfaces;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class ClinicExporter
{
    private readonly Clinic _clinic;
    private readonly string _baseDir;

    public ClinicExporter(Clinic clinic, string baseDir = "reports")
    {
        _clinic = clinic;
        _baseDir = baseDir;
    }

    private string CreateReportDirectory()
    {
        string dateDirectory = DateTime.Today.ToString("yyyy-MM-dd");
        string directory = Path.Combine(_baseDir, dateDirectory);

        Directory.CreateDirectory(directory);

        return directory;
    }

    public string ExportPatients()
    {
        string directory = CreateReportDirectory();
        string path = Path.Combine(directory, "patients.txt");

        using (StreamWriter writer =
            new StreamWriter(path, false, Encoding.UTF8))
        {
            writer.WriteLine("=== Пацієнти — звіт ===");
            writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
            writer.WriteLine("----------------------------------------");

            for (int i = 0; i < _clinic.Patients.Count; i++)
            {
                Patient? patient = _clinic.Patients[i];

                if (patient != null)
                {
                    writer.WriteLine($"{i + 1,3}. {patient}");
                }
            }

            writer.WriteLine("----------------------------------------");
            writer.WriteLine($"Всього: {_clinic.Patients.Count}");
        }

        return path;
    }

    public string ExportAppointments()
    {
        string directory = CreateReportDirectory();
        string path = Path.Combine(directory, "appointments.txt");

        Appointment[] appointments = _clinic.Appointments.GetAll();

        using (StreamWriter writer =
            new StreamWriter(path, false, Encoding.UTF8))
        {
            writer.WriteLine("=== Записи на прийом — звіт ===");
            writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
            writer.WriteLine("----------------------------------------");

            for (int i = 0; i < appointments.Length; i++)
            {
                writer.WriteLine($"{i + 1,3}. {appointments[i]}");
            }

            writer.WriteLine("----------------------------------------");
            writer.WriteLine($"Всього: {appointments.Length}");
        }

        return path;
    }

    public string ExportBilling()
    {
        string directory = CreateReportDirectory();
        string path = Path.Combine(directory, "billing.txt");

        IPayable[] unpaid = _clinic.Billing.GetAllUnpaid();
        decimal totalDebt = _clinic.Billing.GetTotalDebt();

        using (StreamWriter writer =
            new StreamWriter(path, false, Encoding.UTF8))
        {
            writer.WriteLine("=== Неоплачені записи — звіт ===");
            writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
            writer.WriteLine("----------------------------------------");

            for (int i = 0; i < unpaid.Length; i++)
            {
                Appointment appointment = (Appointment)unpaid[i];

                writer.WriteLine($"{i + 1,3}. {appointment}");
            }

            writer.WriteLine("----------------------------------------");
            writer.WriteLine($"Неоплачених записів: {unpaid.Length}");
            writer.WriteLine($"Загальний борг: {totalDebt:F2} грн");
        }

        return path;
    }

    public string ExportTreatmentPlans()
    {
        string directory = CreateReportDirectory();
        string path = Path.Combine(directory, "treatment_plans.txt");

        TreatmentPlan[] plans = _clinic.TreatmentPlans.GetAll();

        using (StreamWriter writer =
            new StreamWriter(path, false, Encoding.UTF8))
        {
            writer.WriteLine("=== Плани лікування — звіт ===");
            writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
            writer.WriteLine("----------------------------------------");

            for (int i = 0; i < plans.Length; i++)
            {
                TreatmentPlan plan = plans[i];

                writer.WriteLine(
                    $"{i + 1,3}. [{plan.Id}] Пацієнт #{plan.PatientId} | " +
                    $"Діагноз: {plan.Diagnosis} | " +
                    $"Тривалість: {plan.DurationDays} днів | " +
                    $"Статус: {plan.Status}");
            }

            writer.WriteLine("----------------------------------------");
            writer.WriteLine($"Всього: {plans.Length}");
        }

        return path;
    }

    public string[] ExportAll()
    {
        string[] paths = new string[4];

        paths[0] = ExportPatients();
        paths[1] = ExportAppointments();
        paths[2] = ExportBilling();
        paths[3] = ExportTreatmentPlans();

        return paths;
    }
}