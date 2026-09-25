namespace ClinicApp;

public class GrowablePatientManager
{
    private Patient[] _patients = new Patient[4];
    private int _count;

    public int Count
    {
        get { return _count; }
    }

    public int Capacity
    {
        get { return _patients.Length; }
    }

    private void Grow()
    {
        int oldCapacity = _patients.Length;
        int newCapacity = oldCapacity * 2;

        Patient[] newPatients = new Patient[newCapacity];

        for (int i = 0; i < _count; i++)
        {
            newPatients[i] = _patients[i];
        }

        _patients = newPatients;

        Console.WriteLine(
            $"  Масив заповнений! Розширення: {oldCapacity} → {newCapacity}");
    }

    public void Add(Patient patient)
    {
        if (_count == _patients.Length)
        {
            Grow();
        }

        _patients[_count] = patient;
        _count++;

        Console.WriteLine(
            $"  Додано [{patient.Id}]. Розмір: {_count} / {Capacity}");
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
        Console.WriteLine(
            $"=== Пацієнти ({_count} / {Capacity}) ===");

        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_patients[i]);
        }

        Console.WriteLine(
            $"Поточна ємність: {Capacity}");
    }
}