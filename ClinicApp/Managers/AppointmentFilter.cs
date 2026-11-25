using System;
using System.Collections.Generic;
using System.Linq;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class AppointmentFilter
{
    private Func<Appointment, bool>? _predicate;

    public AppointmentFilter Add(Func<Appointment, bool> predicate)
    {
        if (_predicate == null)
        {
            _predicate = predicate;
        }
        else
        {
            Func<Appointment, bool> current = _predicate;
            _predicate = a => current(a) && predicate(a);
        }

        return this;
    }

    public AppointmentFilter And(Func<Appointment, bool> predicate)
    {
        return Add(predicate);
    }

    public AppointmentFilter Or(Func<Appointment, bool> predicate)
    {
        if (_predicate == null)
        {
            return this;
        }

        Func<Appointment, bool> current = _predicate;
        _predicate = a => current(a) || predicate(a);

        return this;
    }

    public AppointmentFilter Negate()
    {
        if (_predicate == null)
        {
            _predicate = a => false;
        }
        else
        {
            Func<Appointment, bool> current = _predicate;
            _predicate = a => !current(a);
        }

        return this;
    }

    public IEnumerable<Appointment> Apply(IEnumerable<Appointment> source)
    {
        if (_predicate == null)
        {
            return source;
        }

        return source.Where(_predicate);
    }

    public AppointmentFilter Reset()
    {
        _predicate = null;
        return this;
    }
}