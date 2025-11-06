using DeliveryApp.Core.Domain.Model.SharedKernel;

namespace DeliveryApp.Core.Application.UseCases.Queries.Dto
{
    public class CourierDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Location Location { get; set; }
    }
}