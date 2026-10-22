using ClinicApp.Interfaces;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class BillingManager
{
    private readonly AppointmentManager _appointments;

    public BillingManager(AppointmentManager appointments)
    {
        _appointments = appointments;
    }

    public IPayable[] GetAllUnpaid()
    {
        return FilterUnpaid(_appointments.GetAll());
    }

    public IPayable[] GetUnpaidByPatient(int patientId)
    {
        return FilterUnpaid(_appointments.GetByPatient(patientId));
    }

    private IPayable[] FilterUnpaid(Appointment[] appointments)
    {
        int count = 0;

        for (int i = 0; i < appointments.Length; i++)
        {
            if (!appointments[i].IsPaid && !appointments[i].IsCancelled)
            {
                count++;
            }
        }

        IPayable[] result = new IPayable[count];
        int resultIndex = 0;

        for (int i = 0; i < appointments.Length; i++)
        {
            if (!appointments[i].IsPaid && !appointments[i].IsCancelled)
            {
                result[resultIndex] = appointments[i];
                resultIndex++;
            }
        }

        return result;
    }

    public decimal GetTotalDebt()
    {
        IPayable[] unpaid = GetAllUnpaid();
        decimal total = 0m;

        for (int i = 0; i < unpaid.Length; i++)
        {
            total += unpaid[i].GetCost();
        }

        return total;
    }

    public decimal GetPatientDebt(int patientId)
    {
        IPayable[] unpaid = GetUnpaidByPatient(patientId);
        decimal total = 0m;

        for (int i = 0; i < unpaid.Length; i++)
        {
            total += unpaid[i].GetCost();
        }

        return total;
    }

    public bool PayAppointment(int appointmentId)
    {
        Appointment[] appointments = _appointments.GetAll();

        for (int i = 0; i < appointments.Length; i++)
        {
            Appointment appointment = appointments[i];

            if (appointment.Id == appointmentId)
            {
                if (appointment.IsPaid || appointment.IsCancelled)
                {
                    return false;
                }

                appointment.MarkPaid();
                return true;
            }
        }

        return false;
    }

    public void DisplayUnpaid(IPayable[] items)
    {
        if (items.Length == 0)
        {
            Console.WriteLine("Немає неоплачених записів.");
            return;
        }

        for (int i = 0; i < items.Length; i++)
        {
            string details;

            if (items[i] is Appointment appointment)
            {
                details = appointment.ToString();
            }
            else
            {
                details = $"[{i + 1}]";
            }

            Console.WriteLine(
                $"{details} | Сума: {items[i].GetCost().ToString("F2")} грн");
        }
    }
}