using DeliveryApp.Core.Application.UseCases.Queries.Courier.GetAllBusyCouriers;

namespace DeliveryApp.Core.Ports
{
    public interface IAllBusyCouriersModelProvider
    {
        public Task<GetAllBusyCouriersRequest> GetAllBusyCouriersAsync();
    }
}