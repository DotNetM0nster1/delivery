using DeliveryApp.Infrastructure.OutputAdapters.Postgres.EntityConfigurations.CourierAggregate;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres.EntityConfigurations.OrderAggregate;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres.EntityConfigurations.Outbox;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres.Entities;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Infrastructure.OutputAdapters.Postgres.ApplicationContext
{
    public sealed class ApplicationDatabaseContext : DbContext
    {
        public ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options) : base(options) { }

        public DbSet<Courier> Couriers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OutboxEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CourierEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new StoragePlaceEntityTypeConfiguration());
        }
    }
}