using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryApp.Infrastructure.OutputAdapters.Postgres.EntityConfigurations.CourierAggregate
{
    internal sealed class CourierEntityTypeConfiguration : IEntityTypeConfiguration<Courier>
    {
        public void Configure(EntityTypeBuilder<Courier> entityBuilder)
        {
            entityBuilder.ToTable("couriers");

            entityBuilder.HasKey(courier => courier.Id);

            entityBuilder
                .Property(courier => courier.Id)
                .ValueGeneratedNever()
                .HasColumnName("id")
                .IsRequired();

            entityBuilder
                .Property(courier => courier.Name)
                .HasColumnName("name")
                .IsRequired();

            entityBuilder
                .Property(courier => courier.Speed)
                .HasColumnName("speed")
                .IsRequired();

            entityBuilder.OwnsOne(courier => courier.Location, location =>
            {
                location.Property(x => x.X).HasColumnName("x").IsRequired();
                location.Property(y => y.Y).HasColumnName("y").IsRequired();
                location.WithOwner();
            });

            entityBuilder.Navigation(courier => courier.Location).IsRequired();
        }
    }
}