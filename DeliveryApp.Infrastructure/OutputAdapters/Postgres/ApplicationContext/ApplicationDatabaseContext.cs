using DeliveryApp.Infrastructure.OutputAdapters.Postgres.EntityConfigurations.CourierAggregate;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres.EntityConfigurations.OrderAggregate;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System;

namespace DeliveryApp.Infrastructure.OutputAdapters.Postgres.ApplicationContext
{
    public sealed class ApplicationDatabaseContext : DbContext
    {
        public ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options) : base(options) { }

        public DbSet<Courier> Couriers { get; set; }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CourierEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new StoragePlaceEntityTypeConfiguration());
        }
    }
}