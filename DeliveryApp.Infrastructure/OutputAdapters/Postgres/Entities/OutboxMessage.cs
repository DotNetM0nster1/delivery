namespace DeliveryApp.Infrastructure.OutputAdapters.Postgres.Entities
{
    public sealed class OutboxMessage
    {
        public Guid Id { get; set; }

        public DateTime OccuredAtUtc { get; set; }
        
        public DateTime? ProcessedOnUtc { get; set; }

        public string Type { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
    }
}