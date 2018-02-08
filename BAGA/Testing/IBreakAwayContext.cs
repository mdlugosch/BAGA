using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
    public interface IBreakAwayContext
    {
        IDbSet<Destination> Destinations { get; }
        IDbSet<Lodging> Lodgings { get; }
        IDbSet<Trip> Trips { get; }
        IDbSet<Person> People { get; }
        IDbSet<Reservation> Reservations { get; }
        IDbSet<Payment> Payments { get; }
        IDbSet<Activity> Activities { get; }
    }
}
