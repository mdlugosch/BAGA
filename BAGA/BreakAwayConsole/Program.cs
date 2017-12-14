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

            Console.ReadKey();
        }
    }
}
