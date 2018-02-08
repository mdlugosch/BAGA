using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
    public class TripRepository
    {
        IBreakAwayContext _context;

        public TripRepository(IBreakAwayContext context)
        {
            _context = context;
        }

        public List<Person> GetTravelersOnFutureTrip(Trip trip)
        {
            if (trip.StartDate <= DateTime.Today)
            {
                return null;
            }

            return _context.Reservations
                .Where(r => r.Trip.Identifier == trip.Identifier)
                .Select(r => r.Traveler)
                .ToList();
        }
    }
}
