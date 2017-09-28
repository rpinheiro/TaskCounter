using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.SQLite.EF6;
using System.Data.Entity.ModelConfiguration.Conventions;
namespace TaskManagement.BD
{
    public class Context : DbContext
    {
        public Context()
            : base("DefaultConnection")
        {
            Database.SetInitializer<DbContext>(null);
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
