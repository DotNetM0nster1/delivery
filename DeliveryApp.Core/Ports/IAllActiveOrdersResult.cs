using DeliveryApp.Core.Application.UseCases.Queries.OrderQuery.GetAllNotComplitedOrders;

namespace DeliveryApp.Core.Ports
{
    public interface IAllActiveOrdersResult
    {
        public Task<GetAllNotComplitedOrdersResponse> GetAllActiveAsync();
    }
}