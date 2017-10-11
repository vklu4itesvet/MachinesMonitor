using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Data.Infrastructure.MSSQL
{
    public class MssqlContext : DbContext
    {
        public static string ConnectionString { get; set; } 

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Vehicle> Vehicles { get; set; }

        public async Task<IEnumerable<Vehicle>> GetVehiclesAsync() => await Vehicles.ToListAsync();

        public async Task<IEnumerable<Vehicle>> GetVehiclesDetails() => await Vehicles.Include(v => v.Owner).ToListAsync();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>(e =>
            {
                e.HasKey(v => v.VIN);
                e.HasOne(v => v.Owner).WithMany(customer => customer.Vehicles);
            });

            modelBuilder.Entity<Customer>(e =>
            {
                e.HasKey(c => c.Id);
                e.HasOne(c => c.Address).WithMany(address => address.Customers);
            });
        }
    }
}
