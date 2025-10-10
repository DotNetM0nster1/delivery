using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Infrastructure.OutputAdapters.Postgres.EntityConfigurations.CourierAggregate
{
    internal sealed class StoragePlaceEntityTypeConfiguration : IEntityTypeConfiguration<StoragePlace>
    {
        public void Configure(EntityTypeBuilder<StoragePlace> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("storage_places");

            entityTypeBuilder.HasKey(entity => entity.Id);

            entityTypeBuilder
                .Property(entity => entity.Id)
                .ValueGeneratedNever()
                .HasColumnName("id")
                .IsRequired();

            entityTypeBuilder
                .Property(entity => entity.Name)
                .HasColumnName("name")
                .IsRequired();

            entityTypeBuilder
                .Property(entity => entity.TotalVolume)
                .HasColumnName("total_volume")
                .IsRequired();

            entityTypeBuilder
                .Property(entity => entity.OrderId)
                .HasColumnName("order_id")
                .IsRequired(false);

            entityTypeBuilder
                .Property("CourierId")
                .HasColumnName("courier_id")
                .IsRequired();
        }
    }
}