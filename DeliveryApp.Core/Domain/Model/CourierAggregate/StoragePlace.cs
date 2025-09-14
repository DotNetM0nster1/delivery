using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Primitives;
using System.Diagnostics.CodeAnalysis;

namespace DeliveryApp.Core.Domain.Model.CourierAggregate
{
    public sealed class StoragePlace : Entity<Guid>
    {
        [ExcludeFromCodeCoverage]
        private StoragePlace() { }

        private StoragePlace(string name, int volume, Guid? orderId = null)
        { 
            Id = Guid.NewGuid();
            Name = name;
            TotalVolume = volume;
            OrderId = orderId;
        }

        public static StoragePlace Bag => new(nameof(Bag).ToLowerInvariant(), 10);

        public string Name { get; }

        public int TotalVolume { get; private set; }

        public Guid? OrderId { get; private set; }

        public static Result<StoragePlace, Error> Create(
            string name, 
            int volume, 
            Guid? orderId = null) 
        {
            if (string.IsNullOrWhiteSpace(name))
                return GeneralErrors.ValueIsInvalid(nameof(name));

            if (volume <= 0)
                return GeneralErrors.ValueIsInvalid(nameof(volume));

            return new StoragePlace(name, volume, orderId);
        }

        public Result<bool, Error> IsOrderCorrectForAdd(Order order)
        {
            if (order == null)
                return GeneralErrors.ValueIsRequired(nameof(order));

            return order.Volume <= TotalVolume && OrderId == null;
        }

        public UnitResult<Error> AddOrderInStoragePlace(Order order)
        {
            if (IsOrderCorrectForAdd(order) is var isCorrect && (!isCorrect.Value || isCorrect.IsFailure))
                return GeneralErrors.OrderCannotBeStored(isCorrect.IsFailure ? isCorrect.Error : null);

            OrderId = order.Id;

            return UnitResult.Success<Error>();
        }

        public UnitResult<Error> RemoveOrderOfStorage()
        {
            if(OrderId == null)
                return GeneralErrors.ValueIsInvalid(nameof(OrderId));

            OrderId = null;

            return UnitResult.Success<Error>();
        }

        private bool IsCourierBusy() => OrderId != null;
    }
}