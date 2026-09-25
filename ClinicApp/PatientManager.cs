namespace ClinicApp;

public class PatientManager
{
    private const int MaxPatients = 100;
    private Patient[] _patients = new Patient[MaxPatients];
    private int _count;

    public int Count
    {
        get { return _count; }
    }

    public void Add(Patient patient)
    {
        if (_count >= MaxPatients)
        {
            Console.WriteLine("Досягнуто максимальну кількість пацієнтів.");
            return;
        }

        _patients[_count] = patient;
        _count++;

        Console.WriteLine($"Пацієнта [{patient.Id}] {patient.FullName} додано.");
    }

    public Patient? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].Id == id)
            {
                return _patients[i];
            }
        }

        return null;
    }

    public Patient[] FindByName(string name)
    {
        string searchName = name.ToLower();
        int foundCount = 0;

        for (int i = 0; i < _count; i++)
        {
            string firstName = _patients[i].FirstName.ToLower();
            string lastName = _patients[i].LastName.ToLower();

            if (firstName.Contains(searchName) || lastName.Contains(searchName))
            {
                foundCount++;
            }
        }

        Patient[] result = new Patient[foundCount];
        int resultIndex = 0;

        for (int i = 0; i < _count; i++)
        {
            string firstName = _patients[i].FirstName.ToLower();
            string lastName = _patients[i].LastName.ToLower();

            if (firstName.Contains(searchName) || lastName.Contains(searchName))
            {
                result[resultIndex] = _patients[i];
                resultIndex++;
            }
        }

        return result;
    }

    public bool Remove(int id)
    {
        int removeIndex = -1;

        for (int i = 0; i < _count; i++)
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

        for (int i = removeIndex; i < _count - 1; i++)
        {
            _patients[i] = _patients[i + 1];
        }

        _patients[_count - 1] = null!;
        _count--;

        return true;
    }

    public void DisplayAll()
    {
        if (_count == 0)
        {
            Console.WriteLine("Список пацієнтів порожній.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine($"=== Пацієнти ({_count} / {MaxPatients}) ===");

        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_patients[i]);
        }

        Console.WriteLine("────────────────────────────────────────────────────────────");
    }

    public void DisplayStats()
    {
        if (_count == 0)
        {
            Console.WriteLine("Немає пацієнтів для статистики.");
            return;
        }

        int ageSum = 0;
        int youngestIndex = 0;
        int oldestIndex = 0;
        int adultsCount = 0;

        for (int i = 0; i < _count; i++)
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

        double averageAge = (double)ageSum / _count;

        Console.WriteLine();
        Console.WriteLine("=== Статистика пацієнтів ===");
        Console.WriteLine($"Всього:       {_count}");
        Console.WriteLine($"Середній вік: {averageAge:F1} р.");
        Console.WriteLine($"Наймолодший:  {_patients[youngestIndex].FullName} ({_patients[youngestIndex].Age} р.)");
        Console.WriteLine($"Найстарший:   {_patients[oldestIndex].FullName} ({_patients[oldestIndex].Age} р.)");
        Console.WriteLine($"Дорослих:     {adultsCount} з {_count}");
        Console.WriteLine("============================");
    }
}