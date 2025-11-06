using DeliveryApp.Core.Application.UseCases.Queries.Dto;
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Queries.Courier.GetAllBusyCouriers
{
    public sealed class GetAllBusyCouriersRequest : IRequest<List<CourierDto>>
    {
        public GetAllBusyCouriersRequest(List<CourierDto> couriers) 
        {
            if (couriers == null)
            { 
                throw new ArgumentNullException(nameof(couriers));
            }

            Couriers.AddRange(couriers);
        }

        public List<CourierDto> Couriers { get; private set; } = [];
    }
}