
using System;
using System.Collections.Generic;
using System.Linq;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class AnalyticsManager
{
    private AppointmentManager _appointments;
    private DoctorManager _doctors;
    private PatientManager _patients;

    public AnalyticsManager(
        AppointmentManager appointments,
        DoctorManager doctors,
        PatientManager patients)
    {
        _appointments = appointments;
        _doctors = doctors;
        _patients = patients;
    }

    public IEnumerable<DoctorStats> ComputeDoctorStats()
    {
        Doctor[] doctors = _doctors.GetAll();
        Appointment[] appointments = _appointments.GetAll();

        return doctors.Select(d =>
        {
            IEnumerable<Appointment> doctorAppointments =
                appointments.Where(a => a.DoctorId == d.Id);

            int appointmentCount = doctorAppointments.Count();
            decimal totalRevenue = doctorAppointments.Sum(a => a.GetCost());

            DateTime lastAppointmentDate = doctorAppointments.Any()
                ? doctorAppointments.Max(a => a.ScheduledAt)
                : DateTime.MinValue;

            return new DoctorStats(
                d.Id,
                d.FullName,
                appointmentCount,
                totalRevenue,
                lastAppointmentDate);
        });
    }

    public IEnumerable<PatientStats> ComputePatientStats()
    {
        Appointment[] appointments = _appointments.GetAll();

        Patient[] patients = new Patient[_patients.Count];

        for (int i = 0; i < _patients.Count; i++)
        {
            patients[i] = _patients[i]!;
        }

        return patients.Select(p =>
        {
            IEnumerable<Appointment> patientAppointments =
                appointments.Where(a => a.PatientId == p.Id);

            int visitCount = patientAppointments.Count();
            decimal totalSpent = patientAppointments.Sum(a => a.GetCost());

            DateTime lastVisitDate = patientAppointments.Any()
                ? patientAppointments.Max(a => a.ScheduledAt)
                : DateTime.MinValue;

            return new PatientStats(
                p.Id,
                p.FullName,
                visitCount,
                totalSpent,
                lastVisitDate);
        });
    }
}
