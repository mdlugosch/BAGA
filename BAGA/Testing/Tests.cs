using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void FakeGetCustomersOnPastTripReturnNull()
        {
            var trip = new Trip { StartDate = DateTime.Today.AddDays(-1) };
            var context = new FakeBreakAwayContext();
            var rep = new TripRepository(context);
            Assert.IsNull(rep.GetTravelersOnFutureTrip(trip));
        }

        [TestMethod]
        public void FakeGetCustomersOnFutureTripsDoesNotReturnNull()
        {
            var trip = new Trip { StartDate = DateTime.Today.AddDays(1) };
            var context = new FakeBreakAwayContext();
            var rep = new TripRepository(context);
            Assert.IsNull(rep.GetTravelersOnFutureTrip(trip));
        }
    }
}
