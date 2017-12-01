using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DataAccess;
using System.Data.Entity;

namespace BreakAwayConsole
{
    class Program
    {
        private static void InsertDestination()
        {
            var destination = new Destination()
            {
                Country = "Indonesia",
                Description = "EcoTourism at its best in exquisite Bali",
                Name = "Bali"
            };

            using (var context = new BreakAwayContext())
            {
                context.Destinations.Add(destination);
                context.SaveChanges();
            }
        }

        public static void ShowDestination()
        {
            using (var context = new BreakAwayContext())
            {
                var query = from row in context.Destinations
                            select row;

                foreach (var element in query)
                {
                    Console.WriteLine(element.DestinationId + " / " + element.Name + " / " + element.Description);
                }
            }
        }

        public static void ClearDestinations()
        {
            using (var context = new BreakAwayContext())
            {
                var query = from row in context.Destinations
                            where row.DestinationId>0
                            select row;

                foreach (var element in query)
                {
                    context.Destinations.Remove(element);
                }
                context.SaveChanges();
            }
        }


        static void Main(string[] args)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<BreakAwayContext>());
            //InsertDestination();
            //ClearDestinations();
            ShowDestination();
            Console.ReadKey();
        }
    }
}
