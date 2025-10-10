using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System;

namespace DeliveryApp.Infrastructure.OutputAdapters.Postgres.EntityConfigurations.OrderAggregate
{
    internal sealed class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("orders");

            entityTypeBuilder.HasKey(entity => entity.Id);

            entityTypeBuilder
                .Property(entity => entity.Id)
                .ValueGeneratedNever()
                .HasColumnName("id")
                .IsRequired();

            entityTypeBuilder
                .Property(entity => entity.CourierId)
                .HasColumnName("courier_id")
                .IsRequired(false);

            entityTypeBuilder
                .OwnsOne(entity => entity.Status, a =>
                {
                    a.Property(c => c.Name).HasColumnName("status").IsRequired();
                    a.WithOwner();
                });

            entityTypeBuilder.Navigation(entity => entity.Status).IsRequired();

            entityTypeBuilder
                .OwnsOne(entity => entity.Location, l =>
                {
                    l.Property(x => x.X).HasColumnName("location_x").IsRequired();
                    l.Property(y => y.Y).HasColumnName("location_y").IsRequired();
                    l.WithOwner();
                });

            entityTypeBuilder.Navigation(entity => entity.Location).IsRequired();
        }
    }
}