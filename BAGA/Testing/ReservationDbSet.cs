using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testing;

namespace Testing
{
    public class ReservationDbSet : FakeDbSet<Reservation>
    {
        public override Reservation Find(params object[] keyValues)
        {
            var keyValue = (int)keyValues.FirstOrDefault();
            return this.SingleOrDefault(r => r.ReservationId == keyValue);
        }
    }
}
