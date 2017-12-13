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

        private static void InsertTrip()
        {
            var trip = new Trip
            {
                CostUSD = 800,
                StartDate = new DateTime(2011, 9, 1),
                EndDate = new DateTime(2011, 9, 14)
            };

            using (var context = new BreakAwayContext())
            {
                context.Trips.Add(trip);
                context.SaveChanges();
            }
        }


        public static void ShowTrip()
        {
            using (var context = new BreakAwayContext())
            {
                var query = from row in context.Trips
                            select row;

                foreach (var element in query)
                {
                    Console.WriteLine(element.Identifier + " / " + element.StartDate + " / " + element.EndDate + " / " + element.CostUSD);
                    foreach (int i in element.RowVersion) { Console.Write(i); }
                    Console.WriteLine();
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

        static void GreatBarrierReefTest()
        {
            using (var context = new BreakAwayContext())
            {
                var reef = from destination in context.Destinations
                           where destination.Name == "Great Barrier Reef"
                           select destination;

                if(reef.Count() == 1)
                {
                    Console.WriteLine("Test Passed: 1 'Great Barrier Reef' destination found");
                }
                else
                {
                    Console.WriteLine("Test Failed: {0} 'Great Barrier Reef' destination found", context.Destinations.Count());
                }
            }
        }

        static void Main(string[] args)
        {
            Database.SetInitializer(new DropCreateBreakAwayWithSeedData());
            GreatBarrierReefTest();
            
            //InsertDestination();

            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<BreakAwayContext>());
            //using(var context = new BreakAwayContext())
            //{
            //    try
            //    {
            //        context.Database.Initialize(force: false);
            //        Console.WriteLine("Initialization successfully");
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine("Initialization failed...");
            //        Console.WriteLine(ex.Message);
            //    }
            //}
            //InsertTrip();
            //ShowTrip();
            //ClearDestinations();
            //InsertDestination();
            //ShowDestination();

            Console.ReadKey();
        }
    }
}
