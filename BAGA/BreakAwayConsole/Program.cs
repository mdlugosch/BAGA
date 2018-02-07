using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DataAccess;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace BreakAwayConsole
{
    class Program
    {
        private static void PrintAllDestinations()
        {
            using(var context = new BreakAwayContext())
            {
                foreach(var destination in context.Destinations)
                {
                    Console.WriteLine(destination.Name);
                }
            }
        }

        private static void PrintAllDestinationsTwice()
        {
            using (var context = new BreakAwayContext())
            {
                /*
                 * Destinations werden einmal geladen und dann für die Ausgabe-
                 * schleifen genutzt. Die Datenbank wird hier also nur einmal
                 * abgefragt anstatt für jede Ausgabeschleife. Vorteil: Speicher-
                 * zugriffe sind schneller
                 */
                var allDestinations = context.Destinations.ToList();

                foreach (var destination in allDestinations)
                {
                    Console.WriteLine(destination.Name);
                }

                foreach (var destination in allDestinations)
                {
                    Console.WriteLine(destination.Name);
                }
            }
        }

        private static void PrintAllDestinationsSorted()
        {
            using (var context = new BreakAwayContext())
            {
                // LinQ Variante
                //var query = from d in context.Destinations
                //            orderby d.Name
                //            select d;

                // Lambda Variante
                var query = context.Destinations.OrderBy(d => d.Name);

                foreach (var destination in query)
                {
                    Console.WriteLine(destination.Name);
                }
            }
        }

        private static void PrintAustralianDestinations()
        {
            using (var context = new BreakAwayContext())
            {
                var query = context.Destinations.Where(d => d.Country == "Australia").OrderBy(d => d.Name);

                foreach (var destination in query)
                {
                    Console.WriteLine(destination.Name);
                }
            }
        }

        private static void PrintDestinationNameOnly()
        {
            using (var context = new BreakAwayContext())
            {
                /*
                 * Hier werden nur die Namen der Destinations durch das Selectstatment der Liste hinzugefügt.
                 */
                var query = context.Destinations.Where(d => d.Country == "Australia").OrderBy(d => d.Name).Select(d => d.Name);

                foreach (var name in query)
                {
                    Console.WriteLine(name);
                }
            }
        }

        /*
         * Suchen eines Datensatzes mit der Find-Methode.
         */
        private static void FindDestination()
        {
            Console.Write("Enter id of Destination to find: ");
            var id = int.Parse(Console.ReadLine());

            using (var context = new BreakAwayContext())
            {
                var destination = context.Destinations.Find(id);

                if(destination==null)
                {
                    Console.WriteLine("Destination not found!");
                }
                else
                {
                    Console.WriteLine(destination.Name);
                }
            }
        }

        private static void FindGreatBarrierReef()
        {
            using (var context = new BreakAwayContext())
            {
                var query = from d in context.Destinations
                            where d.Name == "Great Barrier Reef"
                            select d;

                var reef = query.FirstOrDefault();

                if(reef==null)
                {
                    Console.WriteLine("Can't find the reff!");
                }
                else
                {
                    Console.WriteLine(reef.DestinationId + " / " + reef.Description);
                }            
            }
        }

        private static void GetLocalDestinationCount()
        {
            using(var context = new BreakAwayContext())
            {
                foreach (var destination in context.Destinations)
                {
                    Console.WriteLine(destination.Name);
                }

                var count = context.Destinations.Local.Count;
                Console.WriteLine("Destinations in memory: {0}", count);
            }
        }

        private static void GetLocalDestinationCountWithLoad()
        {
            using (var context = new BreakAwayContext())
            {
                context.Destinations.Load();         

                var count = context.Destinations.Local.Count;
                Console.WriteLine("Destinations in memory: {0}", count);
            }
        }

        private static void LoadAustralianDestinations()
        {
            using (var context = new BreakAwayContext())
            {
                var query = from d in context.Destinations
                            where d.Country == "Australia"
                            select d;

                query.Load();

                var count = context.Destinations.Local.Count;
                Console.WriteLine("Destinations in memory: {0}", count);
            }
        }

        private static void LocalLinqQueries()
        {
            using (var context = new BreakAwayContext())
            {
                context.Destinations.Load();

                var sortedDestinations = from d in context.Destinations.Local
                                         orderby d.Name
                                         select d;

                Console.WriteLine("All Destinations:");
                foreach(var destination in sortedDestinations)
                {
                    Console.WriteLine(destination.Name);
                }

                var aussieDestinations = from d in context.Destinations.Local
                                         where d.Country == "Australia"
                                         select d;

                Console.WriteLine();
                Console.WriteLine("Australian Destinations:");
                foreach(var destination in aussieDestinations)
                {
                    Console.WriteLine(destination.Name);
                }
            }
        }

        private static void ListenToLocalChanges()
        {
            using (var context = new BreakAwayContext())
            {
                context.Destinations.Local.
                CollectionChanged += (sender, args) =>
                {
                    if (args.NewItems != null)
                    {
                        foreach (Destination item in args.NewItems)
                        {
                            Console.WriteLine("Added: " + item.Name);
                        }
                    }

                    if (args.OldItems != null)
                    {
                        foreach (Destination item in args.OldItems)
                        {
                            Console.WriteLine("Removed: " + item.Name);
                        }
                    }
                };

                context.Destinations.Load();
            }

        }

        private static void TestLazyLoading()
        {
            using (var context = new BreakAwayContext())
            {
                var query = from d in context.Destinations
                            where d.Name == "Grand Canyon"
                            select d;

                var canyon = query.Single();

                Console.WriteLine("Grand Canyon Lodging:");
                if(canyon.Lodgings!=null)
                {
                    foreach(var lodging in canyon.Lodgings)
                    {
                        Console.WriteLine(lodging.Name);
                    }
                }
            }
        }

        private static void TestEagerLoading()
        {
            using (var context = new BreakAwayContext())
            {
                var allDestinations = context
                    .Destinations
                    .Include(d => d.Lodgings);

                foreach(var destination in allDestinations)
                {
                    Console.WriteLine(destination.Name);

                    foreach (var lodging in destination.Lodgings)
                    {
                        Console.WriteLine(" - " + lodging.Name);
                    }
                }
            }
        }

        private static void TestExplicitLoading()
        {
            using (var context = new BreakAwayContext())
            {
                var query = from d in context.Destinations
                            where d.Name == "Grand Canyon"
                            select d;

                var canyon = query.Single();

                context.Entry(canyon)
                    .Collection(d => d.Lodgings)
                    .Load();

                Console.WriteLine("Grand Canyon Lodging:");
                foreach (var lodging in canyon.Lodgings)
                {
                    Console.WriteLine(lodging.Name);
                }
            }
        }

        private static void TestIsLoaded()
        {
            using (var context = new BreakAwayContext())
            {
                var canyon = (from d in context.Destinations
                              where d.Name == "Grand Canyon"
                              select d).Single();

                var entry = context.Entry(canyon);

                Console.WriteLine("Before Load: {0}", entry.Collection(d => d.Lodgings).IsLoaded);

                entry.Collection(d => d.Lodgings).Load();

                Console.WriteLine("After Load: {0}", entry.Collection(d => d.Lodgings).IsLoaded);
            }
        }

        private static void QueryLodgingDistance()
        {
            using (var context = new BreakAwayContext())
            {
                var canyonQuery = from d in context.Destinations
                             where d.Name == "Grand Canyon"
                             select d;

                var canyon = canyonQuery.Single();

                var lodgingQuery = context.Entry(canyon)
                    .Collection(d => d.Lodgings)
                    .Query();

                var distanceQuery = from l in lodgingQuery
                                    where l.MilesFromNearestAirport <= 10
                                  select l;

                foreach(var lodging in distanceQuery)
                {
                    Console.WriteLine(lodging.Name);
                }
            }
        }

        private static void QueryLodgingCount()
        {
            using (var context = new BreakAwayContext())
            {
                var canyonQuery = from d in context.Destinations
                                  where d.Name == "Grand Canyon"
                                  select d;

                var canyon = canyonQuery.Single();

                var lodgingQuery = context.Entry(canyon)
                    .Collection(d => d.Lodgings)
                    .Query();

                var lodgingCount = lodgingQuery.Count();

                Console.WriteLine("Lodging at Grand Canyon: " + lodgingCount);
            }
        }

        private static void AddMachuPicchu()
        {
            using(var context = new BreakAwayContext())
            {
                var machuPicchu = new Destination
                {
                    Name = "Machu Picchu",
                    Country = "Peru"
                };

                context.Destinations.Add(machuPicchu);
                context.SaveChanges();
            }
        }

        private static void ChangeGrandCanyon()
        {
            using (var context = new BreakAwayContext())
            {
                var canyon = (from d in context.Destinations
                              where d.Name == "Grand Canyon"
                              select d).Single();

                canyon.Description = "227 mile long canyon.";

                context.SaveChanges();
            }
        }

        private static void DeleteWineGlassBay()
        {
            using (var context = new BreakAwayContext())
            {
                var bay = (from d in context.Destinations
                              where d.Name == "Wine Glass Bay"
                              select d).Single();

                context.Destinations.Remove(bay);
                context.SaveChanges();
            }
        }

        private static void DeleteTrip()
        {
            using (var context = new BreakAwayContext())
            {
                var trip = (from t in context.Trips
                            where t.Description == "Trip from the database"
                            select t).Single();

                var res = (from r in context.Reservations
                           where r.Trip.Description == "Trip from the database"
                           select r).Single();

                context.Trips.Remove(trip);
                context.SaveChanges();
            }
        }

        private static void DeleteGrandCanyon()
        {
            using (var context = new BreakAwayContext())
            {
                var canyon = (from t in context.Destinations
                            where t.Name == "Grand Canyon"
                            select t).Single();

                context.Destinations.Remove(canyon);
                context.SaveChanges();
            }
        }

        private static void MakeMultipleChanges()
        {
            using(var context = new BreakAwayContext())
            {
                var niagaraFalls = new Destination
                {
                    Name = "Niagara Falls",
                    Country = "USA"
                };

                context.Destinations.Add(niagaraFalls);

                var wineGlassBay = (from d in context.Destinations
                              where d.Name == "Wine Glass Bay"
                              select d).Single();

                wineGlassBay.Description = "Picturesque bay with beaches.";

                context.SaveChanges();
            }
        }

        private static void NewGrandCanyonResort()
        {
            using (var context = new BreakAwayContext())
            {
                var resort = new Resort
                {
                    Name = "Pete's Luxury Resort"
                };

                context.Lodgings.Add(resort);

                var canyon = (from t in context.Destinations
                              where t.Name == "Grand Canyon"
                              select t).Single();

                canyon.Lodgings.Add(resort);

                context.SaveChanges();
            }
        }

        private static void ManualDetectChanges()
        {
            using (var context = new BreakAwayContext())
            {
                context.Configuration.AutoDetectChangesEnabled = false;

                var reef = (from d in context.Destinations
                            where d.Name == "Great Barrier Reef"
                            select d).Single();

                reef.Description = "The world's largest reef.";

                Console.WriteLine("Before DetectChanges: {0}", context.Entry(reef).State);

                context.ChangeTracker.DetectChanges();

                Console.WriteLine("After DetectChanges: {0}", context.Entry(reef).State);
            }
        }

        private static void CreatingNewProxies()
        {
            using (var context = new BreakAwayContext())
            {
                var nonProxy = new Destination()
                {
                    Name = "Non-proxy Destination",
                    Lodgings = new List<Lodging>()
                };

                var proxy = context.Destinations.Create();
                proxy.Name = "Proxy Destinations";

                context.Destinations.Add(proxy);
                context.Destinations.Add(nonProxy);
                context.SaveChanges();

                var davesDump = (from l in context.Lodgings
                                 where l.Name == "Dave's Dump"
                                 select l).Single();

                context.Entry(davesDump)
                    .Reference(l => l.Destination)
                    .Load();

                Console.WriteLine("Before changes: {0}", davesDump.Destination.Name);

                nonProxy.Lodgings.Add(davesDump);

                Console.WriteLine("Added to non-proxy destination: {0}", davesDump.Destination.Name);

                proxy.Lodgings.Add(davesDump);

                Console.WriteLine("Added to proxy destination: {0}", davesDump.Destination.Name);

                context.SaveChanges();
            }
        }

        // Clientsidesimulation
        private static void TestAddDestination()
        {
            var jacksonHole = new Destination
            {
                Name = "Jackson Hole, Wyoming",
                Description = "Get your skis on."
            };

            AddDestination(jacksonHole);
        }

        private static void AttachDestination(Destination destination)
        {
            using (var context = new BreakAwayContext())
            {
                context.Entry(destination).State = EntityState.Unchanged;
                context.SaveChanges();
            }
        }

        private static void TestUpdateDestination()
        {
            Destination canyon;
            using (var context = new BreakAwayContext())
            {
                canyon = (from d in context.Destinations
                          where d.Name == "Grand Canyon"
                          select d).Single();
            }
            canyon.TravelWarnings = "Don't fall in!";
            UpdateDestination(canyon);
        }

        private static void TestDeleteDestination()
        {
            Destination canyon;
            using (var context = new BreakAwayContext())
            {
                canyon = (from d in context.Destinations
                          where d.Name == "Grand Canyon"
                          select d).Single();
            }

            DeleteDestination(canyon);
        }

        // Serversidesimulation
        private static void AddDestination(Destination destination)
        {
            using (var context = new BreakAwayContext())
            {
                context.Destinations.Add(destination);
                context.SaveChanges();
            }
        }

        private static void TestAttachDestination()
        {
            Destination canyon;
            using (var context = new BreakAwayContext())
            {
                canyon = (from d in context.Destinations
                          where d.Name == "Grand Canyon"
                          select d).Single();
            }
            AttachDestination(canyon);
        }

        private static void UpdateDestination(Destination destination)
        {
            using (var context = new BreakAwayContext())
            {
                context.Entry(destination).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        private static void DeleteDestination(Destination destination)
        {
            using (var context = new BreakAwayContext())
            {
                context.Destinations.Attach(destination);
                context.Destinations.Remove(destination);
                context.SaveChanges();
            }
        }

        private static void SaveDestinationAndLodgings(
            Destination destination,
            List<Lodging> deletedLodgings)
        {
            // TODO: Ensure only Destinations & Lodgings are passed in
            using (var context = new BreakAwayContext())
            {
                context.Destinations.Add(destination);

                if(destination.DestinationId > 0)
                {
                    context.Entry(destination).State = EntityState.Modified;
                }

                foreach(var lodging in destination.Lodgings)
                {
                    if(lodging.LodgingId > 0)
                    {
                        context.Entry(lodging).State = EntityState.Modified;
                    }
                }

                foreach(var lodging in deletedLodgings)
                {
                    context.Entry(lodging).State = EntityState.Deleted;
                }

                context.SaveChanges();
            }
        }

        public static void SaveDestinationGraph(Destination destination)
        {
            using (var context = new BreakAwayContext())
            {
                context.Destinations.Add(destination);

                foreach(var entry in context.ChangeTracker.Entries<IObjectWithState>())
                {
                    IObjectWithState stateInfo = entry.Entity;
                    entry.State = ConverterState(stateInfo.State);
                }

                context.SaveChanges();
            }
        }

        public static EntityState ConverterState(State state)
        {
            switch(state)
            {
                case State.Added:
                    return EntityState.Added;

                case State.Modified:
                    return EntityState.Modified;

                case State.Deleted:
                    return EntityState.Deleted;

                default:
                    return EntityState.Unchanged;
            }
        }

        private static void TestSaveDestinationGraph()
        {
            Destination canyon;
            using (var context = new BreakAwayContext())
            {
                canyon = (from d in context.Destinations.Include(d => d.Lodgings)
                          where d.Name == "Grand Canyon"
                          select d).Single();
            }

            canyon.TravelWarnings = "Carry enough water";
            canyon.State = State.Modified;

            var firstLodging = canyon.Lodgings.First();
            firstLodging.Name = "New Name Holiday Park";
            firstLodging.State = State.Modified;

            var secondLodging = canyon.Lodgings.Last();
            secondLodging.State = State.Deleted;

            canyon.Lodgings.Add(new Lodging
            {
                Name = "Big Canyon Lodge",
                State = State.Added
            });

            ApplyChanges(canyon);
        }

        private static void ApplyChanges<TEntity>(TEntity root) where TEntity : class, IObjectWithState
        {
            using (var context = new BreakAwayContext())
            {
                context.Set<TEntity>().Add(root);

                CheckForEntitiesWithoutStateInterface(context);

                foreach(var entry in context.ChangeTracker
                    .Entries<IObjectWithState>()) 
                {
                    IObjectWithState stateInfo = entry.Entity;
                    if(stateInfo.State == State.Modified)
                    {
                        entry.State = EntityState.Unchanged;
                        foreach(var property in stateInfo.ModifiedProperties)
                        {
                            entry.Property(property).IsModified = true;
                        }
                    }
                    else
                    {
                        entry.State = ConverterState(stateInfo.State);
                    }
                    entry.State = ConverterState(stateInfo.State);
                }

                context.SaveChanges();
            }
        }

        private static void CheckForEntitiesWithoutStateInterface(BreakAwayContext context)
        {
            var entitiesWithoutState =
                from e in context.ChangeTracker.Entries()
                where !(e.Entity is IObjectWithState)
                select e;

            if(entitiesWithoutState.Any())
            {
                throw new NotSupportedException("All entities must implement IObjectWithState");
            }
        }

        private static void PrintState()
        {
            using(var context = new BreakAwayContext())
            {
                var canyon = (from d in context.Destinations
                              where d.Name == "Grand Canyon"
                              select d).Single();

                DbEntityEntry<Destination> entry = context.Entry(canyon);

                Console.WriteLine("Before Edit: {0}", entry.State);
                canyon.TravelWarnings = "Take lots of water.";
                Console.WriteLine("After Edit: {0}", entry.State);
            }
        }

        private static void PrintChangeTrackingInfo(BreakAwayContext context,
            Lodging entity)
        {
            var entry = context.Entry(entity);

            if (entry.State == EntityState.Deleted)
            {
                Console.WriteLine("\nCurrent Values:");
                PrintPropertyValues(entry.CurrentValues);
            }

            if (entry.State == EntityState.Added)
            {
                Console.WriteLine("\nOriginal Values:");
                PrintPropertyValues(entry.OriginalValues);

                Console.WriteLine("\nDatabase Values:");
                PrintPropertyValues(entry.GetDatabaseValues());
            }
        }

        private static void PrintPropertyValues(DbPropertyValues values)
        {
            foreach(var propertyName in values.PropertyNames)
            {
                Console.WriteLine(" - {0}: {1}", propertyName, values[propertyName]);
            }
        }

        private static void PrintLodgingInfo()
        {
            using (var context = new BreakAwayContext())
            {
                var hotel = (from d in context.Lodgings
                             where d.Name == "Grand Hotel"
                             select d).Single();

                hotel.Name = "Super Grand Hotel";

                context.Database.ExecuteSqlCommand(@"UPDATE Lodgings
                                                    Set Name = 'Not-So-Grand Hotel'
                                                    WHERE Name = 'Grand Hotel'");

                PrintChangeTrackingInfo(context, hotel);
            }
        }

        private static void ValidateNewPerson()
        {
            var person = new Person
            {
                FirstName = "Julie",
                LastName = "Lerman-Flynn",
                Photo = new PersonPhoto { Photo = new Byte[] { 0 } }
            };

            using (var context = new BreakAwayContext())
            {
                if(context.Entry(person).GetValidationResult().IsValid)
                {
                    Console.WriteLine("Person is Valid");
                }
                else
                {
                    Console.WriteLine("Person is Invalid");
                }
            }
        }

        // Auslesen der Validationsmeldungen die durch eine Entity produziert werden.
        private static void ConsoleValidationResult(object entity)
        {
            using(var context = new BreakAwayContext())
            {
                var result = context.Entry(entity).GetValidationResult();
                foreach(DbValidationError error in result.ValidationErrors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
        }

        public static void ValidationDestination()
        {
            ConsoleValidationResult(new Destination
            {
                Name = "New York City",
                Country = "U.S.A",
                Description = "Big city! :)"
            });
        }

        private static void ValidateTrip()
        {
            ConsoleValidationResult(new Trip
            {
                EndDate = DateTime.Now,
                StartDate = DateTime.Now.AddDays(2),
                CostUSD = 500.00M,
                Description="You should enjoy this 500 dollar trip",
                Destination = new Destination { Name = "Somewhere Fun" }
            });
        }

        static void Main(string[] args)
        {
            Database.SetInitializer(new InitializeBagaDatabaseWithSeedData());

            using(var context = new BreakAwayContext())
            {
                try
                {
                    /*
                     * force: Zwingt EF dazu die Datenbank an dieser Stelle zu initialisieren.
                     * false: Nur initialisieren wenn das Model noch nicht in der Datenbank ist.
                     * true: Immer neu Initialisieren.
                     */
                    context.Database.Initialize(force: false);
                    Console.WriteLine("Initialization successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Initialization failed...");
                    Console.WriteLine(ex.Message);
                }
            }

            ValidateTrip();
            Console.ReadKey();
        }
    }
}
