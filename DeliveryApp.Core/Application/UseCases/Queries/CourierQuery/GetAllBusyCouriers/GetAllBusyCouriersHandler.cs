using DeliveryApp.Core.Application.UseCases.Queries.Dto;
using DeliveryApp.Core.Ports;
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Queries.CourierQuery.GetAllBusyCouriers
{
    public sealed class GetAllBusyCouriersHandler(IAllBusyCouriersModelProvider busyCouriersResult) 
        : IRequestHandler<GetAllBusyCouriersRequest, List<CourierDto>>
    {
        private readonly IAllBusyCouriersModelProvider _busyCouriersResult = busyCouriersResult;

        public async Task<List<CourierDto>> Handle(GetAllBusyCouriersRequest request, CancellationToken cancellationToken)
        {
            if (await _busyCouriersResult.GetAllBusyCouriersAsync() is
                var allBusyCouriers && (allBusyCouriers == null || allBusyCouriers.Couriers.Count == 0))
            {
                throw new ArgumentNullException(nameof(allBusyCouriers));
            }

            return allBusyCouriers.Couriers;
        }
    }
}