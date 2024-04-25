using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Dynamic;
using System.Runtime.InteropServices.ObjectiveC;
using Microsoft.Extensions.Options;

namespace test.Models
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"));

            // Add logging to the DbContext options
            options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                .EnableSensitiveDataLogging(); // Optional: Enables logging of parameters with values
        }

        public DbSet<Customer> customer { get; set; }
        //public DbSet<Employee> employee { get; set; }
        //public DbSet<SparePart> spare_part { get; set; }
        //public DbSet<Department> department { get; set; }
        //public DbSet<TaskDetail> task_detail { get; set; }
        //public DbSet<SpareType> spare_type { get; set; }
        //public DbSet<Dbtask> dbtask { get; set; }
        //public DbSet<Vehicle> vehicle { get; set; }
        //public DbSet<Status> status { get; set; }
        //public DbSet<Province> province { get; set; }
    }
}
