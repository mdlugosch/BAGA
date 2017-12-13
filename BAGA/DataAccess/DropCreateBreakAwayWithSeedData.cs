using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DropCreateBreakAwayWithSeedData : DropCreateDatabaseAlways<BreakAwayContext>
    {
        protected override void Seed(BreakAwayContext context)
        {
            context.Destinations.Add(new Model.Destination { Name = "Great Barrier Reef" });
            context.Destinations.Add(new Model.Destination { Name = "Grand Canyon" });
        }
    }
}
