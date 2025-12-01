using DeliveryApp.Core.Ports;
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Queries.OrderQuery.GetAllNotComplitedOrders
{
    public sealed class GetAllNotComplitedOrdersHandler(
        IAllActiveOrdersResult allActiveOrdersQuery) 
        : IRequestHandler<GetAllNotComplitedOrdersQuery, GetAllNotComplitedOrdersResponse>
    {
        private readonly IAllActiveOrdersResult _allActiveOrdersResult = allActiveOrdersQuery;

        public async Task<GetAllNotComplitedOrdersResponse> Handle(GetAllNotComplitedOrdersQuery request, CancellationToken cancellationToken)
        {
            if (await _allActiveOrdersResult.GetAllActiveAsync() is
                var allActiveOrders && allActiveOrders == null || allActiveOrders.Orders.Count == 0)
            {
                throw new ArgumentNullException(nameof(allActiveOrders));
            }

            return new GetAllNotComplitedOrdersResponse(allActiveOrders.Orders);
        }
    }
}