using DeliveryApp.Infrastructure.OutputAdapters.Postgres.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Infrastructure.OutputAdapters.Postgres.EntityConfigurations.Outbox
{
    public class OutboxEntityTypeConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("outbox");

            entityTypeBuilder
               .Property(entity => entity.Id)
               .HasColumnName("id")
               .IsRequired();

            entityTypeBuilder
               .Property(entity => entity.Type)
               .UsePropertyAccessMode(PropertyAccessMode.Field)
               .HasColumnName("type")
               .IsRequired();

            entityTypeBuilder
               .Property(entity => entity.Content)
               .UsePropertyAccessMode(PropertyAccessMode.Field)
               .HasColumnName("content")
               .IsRequired();

            entityTypeBuilder
               .Property(entity => entity.OccuredAtUtc)
               .UsePropertyAccessMode(PropertyAccessMode.Field)
               .HasColumnName("occured_at_utc")
               .IsRequired();

            entityTypeBuilder
               .Property(entity => entity.ProcessedOnUtc)
               .UsePropertyAccessMode(PropertyAccessMode.Field)
               .HasColumnName("processed_on_utc")
               .IsRequired(false);
        }
    }
}