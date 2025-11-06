namespace DeliveryApp.Core.Application.UseCases.Queries.Dto
{
    public sealed class OrderDto
    {
        public Guid Id { get; set; }

        public LocationDto LocationDto { get; set; }
    }
}