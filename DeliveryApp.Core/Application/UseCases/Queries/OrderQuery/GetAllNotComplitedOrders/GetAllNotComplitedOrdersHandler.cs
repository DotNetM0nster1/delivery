using DeliveryApp.Core.Application.UseCases.Queries.Dto;
using DeliveryApp.Core.Ports;
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Queries.OrderQuery.GetAllNotComplitedOrders
{
    public sealed class GetAllNotComplitedOrdersHandler(
        IAllActiveOrdersResult allActiveOrdersQuery) 
        : IRequestHandler<GetAllNotComplitedOrdersRequest, List<OrderDto>>
    {
        private readonly IAllActiveOrdersResult _allActiveOrdersResult = allActiveOrdersQuery;

        public async Task<List<OrderDto>> Handle(GetAllNotComplitedOrdersRequest request, CancellationToken cancellationToken)
        {
            if (await _allActiveOrdersResult.GetAllActiveAsync() is
                var allActiveOrders && allActiveOrders == null || allActiveOrders.Orders.Count == 0)
            {
                throw new ArgumentNullException(nameof(allActiveOrders));
            }

            return allActiveOrders.Orders;
        }
    }
}