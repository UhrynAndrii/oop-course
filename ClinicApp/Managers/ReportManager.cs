using System.Collections.Generic;
using System.Linq;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class ReportManager
{
    private AppointmentManager _appointments;
    private DoctorManager _doctors;
    private PatientManager _patients;
    private AnalyticsManager _analytics;

    public ReportManager(
        AppointmentManager appointments,
        DoctorManager doctors,
        PatientManager patients)
    {
        _appointments = appointments;
        _doctors = doctors;
        _patients = patients;

        _analytics = new AnalyticsManager(
            appointments,
            doctors,
            patients);
    }

    public IEnumerable<SpecialityReport> GetSpecialityStats()
    {
        Doctor[] doctors = _doctors.GetAll();
        Appointment[] appointments = _appointments.GetAll();

        return doctors
            .GroupBy(d => d.Speciality)
            .Select(g =>
            {
                int[] doctorIds = g
                    .Select(d => d.Id)
                    .ToArray();

                Appointment[] specialityAppointments = appointments
                    .Where(a => doctorIds.Contains(a.DoctorId))
                    .ToArray();

                return new SpecialityReport(
                    g.Key,
                    g.Count(),
                    specialityAppointments.Length,
                    specialityAppointments.Sum(a => a.GetCost()));
            })
            .OrderByDescending(r => r.TotalRevenue);
    }

    public string? FindBusiestDoctorName()
    {
        return _analytics.ComputeDoctorStats()
            .OrderByDescending(s => s.AppointmentCount)
            .Select(s => s.FullName)
            .FirstOrDefault();
    }

    public IEnumerable<string> GetPatientsWithMultipleVisits(int minVisits)
    {
        Appointment[] appointments = _appointments.GetAll();

        Patient[] patients = new int[_patients.Count]
            .Select((value, index) => _patients[index]!)
            .ToArray();

        return appointments
            .GroupBy(a => a.PatientId)
            .Where(g => g.Count() >= minVisits)
            .Join(
                patients,
                g => g.Key,
                p => p.Id,
                (g, p) => p.FullName);
    }

    public IEnumerable<DoctorStats> GetTopEarners(int n)
    {
        return _analytics.ComputeDoctorStats()
            .OrderByDescending(s => s.TotalRevenue)
            .Take(n);
    }

    public bool HasAnyUrgentAppointments()
    {
        Appointment[] appointments = _appointments.GetAll();

        return appointments.Any(a => a is UrgentAppointment);
    }

    public IEnumerable<Speciality> GetActiveSpecialities()
    {
        Doctor[] doctors = _doctors.GetAll();

        return doctors
            .Select(d => d.Speciality)
            .Distinct()
            .OrderBy(s => s);
    }

    public IEnumerable<(int Year, int Month, decimal Total)> GetMonthlyRevenue()
    {
        Appointment[] appointments = _appointments.GetAll();

        return appointments
            .GroupBy(a => new
            {
                a.ScheduledAt.Year,
                a.ScheduledAt.Month
            })
            .OrderBy(g => g.Key.Year)
            .ThenBy(g => g.Key.Month)
            .Select(g => (
                Year: g.Key.Year,
                Month: g.Key.Month,
                Total: g.Sum(a => a.GetCost())));
    }
}