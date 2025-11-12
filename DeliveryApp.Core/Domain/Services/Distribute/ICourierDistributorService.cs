using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.Services.Distribute
{
    public interface ICourierDistributorService
    {
        public Result<Courier, Error> DistributeOrderOnCouriers(Order order, List<Courier> couriers);
    }
}