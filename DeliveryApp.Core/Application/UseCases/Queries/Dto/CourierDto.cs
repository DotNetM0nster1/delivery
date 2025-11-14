namespace DeliveryApp.Core.Application.UseCases.Queries.Dto
{
    public class CourierDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public LocationDto Location { get; set; }
    }
}