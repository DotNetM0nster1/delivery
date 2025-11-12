using DeliveryApp.Core.Application.UseCases.Queries.CourierQuery.GetAllBusyCouriers;

namespace DeliveryApp.Core.Ports
{
    public interface IAllBusyCouriersModelProvider
    {
        public Task<GetAllBusyCouriersRequest> GetAllBusyCouriersAsync();
    }
}