using DeliveryApp.Core.Application.UseCases.Queries.Order.GetAllNotComplitedOrders;

namespace DeliveryApp.Core.Ports
{
    public interface IAllActiveOrdersResult
    {
        public Task<GetAllNotComplitedOrdersRequest> GetAllActiveAsync();
    }
}