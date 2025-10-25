using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.Model.OrderAggregate
{
    public sealed class Order : Aggregate<Guid>
    {
        [ExcludeFromCodeCoverage]
        private Order() { }

        private Order(Guid orderId, Location location, int volume)
        {
            Id = orderId;
            Location = location;
            Volume = volume;
            Status = OrderStatus.Created;
        }

        public OrderStatus Status { get; private set; }

        public Location Location { get; private set; }

        public Guid? CourierId { get; private set; }

        public int Volume { get; private set; }

        public static Result<Order, Error> Create(Guid orderId, Location location, int volume)
        {
            if (orderId == Guid.Empty)
            {
                return GeneralErrors.ValueIsRequired(nameof(orderId));
            }

            if (location == null)
            {
                return GeneralErrors.ValueIsRequired(nameof(location));
            }

            if (volume <= 0)
            {
                return GeneralErrors.ValueIsInvalid(nameof(volume));
            }

            return new Order(orderId, location, volume);
        }

        public UnitResult<Error> Assign(Courier courier)
        {
            if (courier == null)
            {
                return GeneralErrors.ValueIsRequired(nameof(courier));
            }

            if (Status != OrderStatus.Created)
            {
                return GeneralErrors.ValueIsInvalid(nameof(Status));
            }

            CourierId = courier.Id;
            Status = OrderStatus.Assigned;

            return UnitResult.Success<Error>();
        }

        public UnitResult<Error> Complete()
        {
            if (Status != OrderStatus.Assigned)
            {
                return GeneralErrors.ValueIsInvalid(nameof(Status));
            }

            if (CourierId == null)
            {
                return GeneralErrors.ValueIsRequired(nameof(CourierId));
            }

            Status = OrderStatus.Completed;

            return UnitResult.Success<Error>();
        }
    }
}