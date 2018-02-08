using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
    // FakeContext that didnt use the database.
    public class FakeBreakAwayContext : IBreakAwayContext
    {
        public FakeBreakAwayContext()
        {
            // Instanzieren eines FakeDBSets vom Typ Reservations
            Reservations = new ReservationDbSet();
        }
        public IDbSet<Destination> Destinations { get; set; }
        public IDbSet<Lodging> Lodgings { get; set; }
        public IDbSet<Trip> Trips { get; set; }
        public IDbSet<Person> People { get; set; }
        public IDbSet<Reservation> Reservations { get; set; }
        public IDbSet<Payment> Payments { get; set; }
        public IDbSet<Activity> Activities { get; set; }

        public int SaveChanges()
        {
            return 0;
        }
    }
}
