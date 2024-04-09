using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Tabel.Model
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Organization> Organizations { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Division> Divisions { get; set; } = null!;
        public DbSet<DayType> DayTypes { get; set; } = null!;
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Tabel.db");
        }
    }
}
