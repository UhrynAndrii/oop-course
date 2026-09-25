namespace ClinicApp;

public class DoctorManager
{
    private const int MaxDoctors = 50;
    private Doctor[] _doctors = new Doctor[MaxDoctors];
    private int _count;

    public int Count
    {
        get { return _count; }
    }

    public void Add(Doctor doctor)
    {
        if (_count >= MaxDoctors)
        {
            Console.WriteLine("Досягнуто максимальну кількість лікарів.");
            return;
        }

        _doctors[_count] = doctor;
        _count++;

        Console.WriteLine($"Лікаря [{doctor.Id}] {doctor.FullName} додано.");
    }

    public Doctor? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Id == id)
            {
                return _doctors[i];
            }
        }

        return null;
    }

    public Doctor[] FindBySpeciality(string speciality)
    {
        string searchSpeciality = speciality.ToLower();
        int foundCount = 0;

        for (int i = 0; i < _count; i++)
        {
            string currentSpeciality = _doctors[i].Speciality.ToLower();

            if (currentSpeciality.Contains(searchSpeciality))
            {
                foundCount++;
            }
        }

        Doctor[] result = new Doctor[foundCount];
        int resultIndex = 0;

        for (int i = 0; i < _count; i++)
        {
            string currentSpeciality = _doctors[i].Speciality.ToLower();

            if (currentSpeciality.Contains(searchSpeciality))
            {
                result[resultIndex] = _doctors[i];
                resultIndex++;
            }
        }

        return result;
    }

    public Doctor[] GetAll()
    {
        Doctor[] result = new Doctor[_count];

        Array.Copy(_doctors, result, _count);

        return result;
    }

    public bool Remove(int id)
    {
        int removeIndex = -1;

        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Id == id)
            {
                removeIndex = i;
                break;
            }
        }

        if (removeIndex == -1)
        {
            return false;
        }

        for (int i = removeIndex; i < _count - 1; i++)
        {
            _doctors[i] = _doctors[i + 1];
        }

        _doctors[_count - 1] = null!;
        _count--;

        return true;
    }

    public void DisplayAll()
    {
        if (_count == 0)
        {
            Console.WriteLine("Список лікарів порожній.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine($"=== Лікарі ({_count} / {MaxDoctors}) ===");

        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_doctors[i]);
        }

        Console.WriteLine("────────────────────────────────────────────────────────────");
    }

    public void DisplayStats()
    {
        if (_count == 0)
        {
            Console.WriteLine("Немає лікарів для статистики.");
            return;
        }

        int availableCount = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].IsAvailableNow)
            {
                availableCount++;
            }
        }

        Console.WriteLine();
        Console.WriteLine("=== Статистика лікарів ===");
        Console.WriteLine($"Всього:         {_count}");
        Console.WriteLine($"Доступні зараз: {availableCount}");
        Console.WriteLine("По спеціальностях:");

        for (int i = 0; i < _count; i++)
        {
            bool alreadyDisplayed = false;

            for (int j = 0; j < i; j++)
            {
                if (_doctors[j].Speciality.ToLower() == _doctors[i].Speciality.ToLower())
                {
                    alreadyDisplayed = true;
                    break;
                }
            }

            if (alreadyDisplayed)
            {
                continue;
            }

            int specialityCount = 0;

            for (int j = 0; j < _count; j++)
            {
                if (_doctors[j].Speciality.ToLower() == _doctors[i].Speciality.ToLower())
                {
                    specialityCount++;
                }
            }

            Console.WriteLine($"  {_doctors[i].Speciality}: {specialityCount}");
        }

        Console.WriteLine("==========================");
    }
}