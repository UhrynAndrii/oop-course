using System;
using System.Globalization;
using System.IO;
using System.Text;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class SessionManager
{
    private readonly string _sessionPath;

    public SessionManager(string sessionPath = "session.dat")
    {
        _sessionPath = sessionPath;
    }

    public bool Exists()
    {
        return File.Exists(_sessionPath);
    }

    public void Save(Clinic clinic)
    {
        using (StreamWriter writer =
            new StreamWriter(_sessionPath, false, Encoding.UTF8))
        {
            writer.WriteLine("[PATIENTS]");

            for (int i = 0; i < clinic.Patients.Count; i++)
            {
                Patient? patient = clinic.Patients[i];

                if (patient != null)
                {
                    writer.WriteLine(
                        $"{patient.FirstName},{patient.LastName}," +
                        $"{patient.DateOfBirth:dd.MM.yyyy}," +
                        $"{patient.BloodType},{patient.Phone}");
                }
            }

            writer.WriteLine("[END]");
        }
    }

    public int Load(Clinic clinic)
    {
        if (!File.Exists(_sessionPath))
        {
            return 0;
        }

        string[] lines = File.ReadAllLines(_sessionPath, Encoding.UTF8);

        string currentSection = "";
        int loadedCount = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (line.StartsWith("["))
            {
                currentSection = line;

                if (currentSection == "[END]")
                {
                    break;
                }

                continue;
            }

            if (currentSection != "[PATIENTS]")
            {
                continue;
            }

            try
            {
                string[] parts = line.Split(',');

                if (parts.Length != 5)
                {
                    continue;
                }

                DateTime dateOfBirth = DateTime.ParseExact(
                    parts[2],
                    "dd.MM.yyyy",
                    CultureInfo.InvariantCulture);

                BloodType bloodType = (BloodType)Enum.Parse(
                    typeof(BloodType),
                    parts[3]);

                bool alreadyExists = false;

                for (int j = 0; j < clinic.Patients.Count; j++)
                {
                    Patient? existingPatient = clinic.Patients[j];

                    if (existingPatient != null &&
                        existingPatient.FirstName == parts[0] &&
                        existingPatient.LastName == parts[1] &&
                        existingPatient.DateOfBirth == dateOfBirth)
                    {
                        alreadyExists = true;
                        break;
                    }
                }

                if (alreadyExists)
                {
                    continue;
                }

                Patient patient = new Patient(
                    parts[0],
                    parts[1],
                    dateOfBirth,
                    bloodType,
                    parts[4]);

                clinic.Patients.Add(patient);

                loadedCount++;
            }
            catch (Exception)
            {
                continue;
            }
        }

        return loadedCount;
    }
}