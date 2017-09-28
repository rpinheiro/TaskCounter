using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.BD.Entity
{
    public class TaskDateContext : Context
    {
        public DbSet<TaskDate> TaskDates { get; set; }
    }

    public class TaskDate
    {
        public int Id { get; set; }
        public Issue Task { get; set; }
    }
}
