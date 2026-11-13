using System;
using System.Globalization;
using System.IO;
using System.Text;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class CsvImporter
{
    public ImportResult ImportPatients(Clinic clinic, string filePath)
    {
        ImportResult result = new ImportResult();

        if (!File.Exists(filePath))
        {
            result.AddError(0, "Файл не знайдено.");
            return result;
        }

        string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
            {
                continue;
            }

            try
            {
                string[] parts = lines[i].Split(',');

                if (parts.Length != 5)
                {
                    result.AddError(
                        i + 1,
                        "Очікується 5 полів, отримано " + parts.Length + ".");
                    continue;
                }

                DateTime dateOfBirth = DateTime.ParseExact(
                    parts[2],
                    "dd.MM.yyyy",
                    CultureInfo.InvariantCulture);

                BloodType bloodType = (BloodType)Enum.Parse(
                    typeof(BloodType),
                    parts[3]);

                Patient patient = new Patient(
                    parts[0],
                    parts[1],
                    dateOfBirth,
                    bloodType,
                    parts[4]);

                clinic.Patients.Add(patient);

                result.AddSuccess();
            }
            catch (Exception e)
            {
                result.AddError(i + 1, e.Message);
            }
        }

        return result;
    }
}