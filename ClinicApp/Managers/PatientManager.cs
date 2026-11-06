using System.Collections.Generic;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class PatientManager
{
    private List<Patient> _patients = new List<Patient>();

    public int Count
    {
        get { return _patients.Count; }
    }

    public Patient? this[int index]
    {
        get
        {
            if (index < 0 || index >= _patients.Count)
            {
                return null;
            }

            return _patients[index];
        }
    }

    public void Add(Patient patient)
    {
        _patients.Add(patient);

        Console.WriteLine($"Пацієнта [{patient.Id}] {patient.FullName} додано.");
    }

    public Patient? FindById(int id)
    {
        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].Id == id)
            {
                return _patients[i];
            }
        }

        return null;
    }

    public bool TryFindById(int id, out Patient patient)
    {
        patient = FindById(id)!;

        return patient != null;
    }

    public Patient[] FindByName(string name)
    {
        string searchName = name.ToLower();
        List<Patient> result = new List<Patient>();

        for (int i = 0; i < _patients.Count; i++)
        {
            string firstName = _patients[i].FirstName.ToLower();
            string lastName = _patients[i].LastName.ToLower();

            if (firstName.Contains(searchName) || lastName.Contains(searchName))
            {
                result.Add(_patients[i]);
            }
        }

        return result.ToArray();
    }

    public Patient[] FindByBloodType(BloodType bloodType)
    {
        List<Patient> result = new List<Patient>();

        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].BloodType == bloodType)
            {
                result.Add(_patients[i]);
            }
        }

        return result.ToArray();
    }

    public bool Remove(int id)
    {
        int removeIndex = -1;

        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].Id == id)
            {
                removeIndex = i;
                break;
            }
        }

        if (removeIndex == -1)
        {
            return false;
        }

        _patients.RemoveAt(removeIndex);

        return true;
    }

    public void DisplayAll()
    {
        if (_patients.Count == 0)
        {
            Console.WriteLine("Список пацієнтів порожній.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine($"=== Пацієнти ({_patients.Count}) ===");

        for (int i = 0; i < _patients.Count; i++)
        {
            Console.WriteLine(_patients[i]);
        }

        Console.WriteLine("────────────────────────────────────────────────────────────");
    }

    public void DisplayStats()
    {
        if (_patients.Count == 0)
        {
            Console.WriteLine("Немає пацієнтів для статистики.");
            return;
        }

        int ageSum = 0;
        int youngestIndex = 0;
        int oldestIndex = 0;
        int adultsCount = 0;

        for (int i = 0; i < _patients.Count; i++)
        {
            ageSum += _patients[i].Age;

            if (_patients[i].Age < _patients[youngestIndex].Age)
            {
                youngestIndex = i;
            }

            if (_patients[i].Age > _patients[oldestIndex].Age)
            {
                oldestIndex = i;
            }

            if (_patients[i].IsAdult)
            {
                adultsCount++;
            }
        }

        double averageAge = (double)ageSum / _patients.Count;

        Console.WriteLine();
        Console.WriteLine("=== Статистика пацієнтів ===");
        Console.WriteLine($"Всього:       {_patients.Count}");
        Console.WriteLine($"Середній вік: {averageAge:F1} р.");
        Console.WriteLine($"Наймолодший:  {_patients[youngestIndex].FullName} ({_patients[youngestIndex].Age} р.)");
        Console.WriteLine($"Найстарший:   {_patients[oldestIndex].FullName} ({_patients[oldestIndex].Age} р.)");
        Console.WriteLine($"Дорослих:     {adultsCount} з {_patients.Count}");
        Console.WriteLine("============================");
    }
}