using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class PromptForDropCreateDatabaseWhenModelChanges<TContext> : IDatabaseInitializer<TContext>
           where TContext : DbContext // TContext muss von DbContext abgeleitet sein
    {
        public void InitializeDatabase(TContext context)
        {
            // If the database exists and matches the model there is nothing to do.
            var exists = context.Database.Exists();
            if(exists && context.Database.CompatibleWithModel(true)) 
            {
                return;
            }

            /* If the database exists and dosen't match the model
             *  then prompt for input
             */
            if(exists)
            {

            Console.WriteLine("Existing database dosen't match the model!");
            Console.Write("Do you want to drop and create the database (Y/N): ");
            var res = Console.ReadKey();
            if(!String.Equals("Y", res.KeyChar.ToString(), StringComparison.OrdinalIgnoreCase)) 
            {
                return;
            }

            context.Database.Delete();
            }
            context.Database.Create();
        }
    }
}
