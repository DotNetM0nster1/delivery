using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System;

namespace DeliveryApp.Infrastructure.OutputAdapters.Postgres.EntityConfigurations.CourierAggregate
{
    internal sealed class StoragePlaceEntityTypeConfiguration : IEntityTypeConfiguration<StoragePlace>
    {
        public void Configure(EntityTypeBuilder<StoragePlace> entityBuilder)
        {
            entityBuilder.ToTable("storage_places");

            entityBuilder.HasKey(x => x.Id);

            entityBuilder
                .Property(storagePlace => storagePlace.Id)
                .ValueGeneratedNever()
                .HasColumnName("id")
                .IsRequired();

            entityBuilder
                .Property(storagePlace => storagePlace.Name)
                .HasColumnName("name")
                .IsRequired();

            entityBuilder
                .Property(storagePlace => storagePlace.TotalVolume)
                .HasColumnName("total_volume")
                .IsRequired();

            entityBuilder
                .Property(storagePlace => storagePlace.OrderId)
                .HasColumnName("order_id")
                .IsRequired(false);

            entityBuilder
                .Property(storagePlace => storagePlace.TotalVolume)
                .HasColumnName("total_volume")
                .IsRequired();
        }
    }
}