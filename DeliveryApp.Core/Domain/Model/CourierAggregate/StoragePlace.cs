using CSharpFunctionalExtensions;
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

        public string Name { get; }

        public int TotalVolume { get; private set; }

        public Guid? OrderId { get; private set; }

        public static Result<StoragePlace, Error> CreateStorage(
            string name, 
            int capacity, 
            Guid? orderId = null) 
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                return GeneralErrors.ValueIsInvalid(nameof(name));

            if (capacity <= 0)
                return GeneralErrors.ValueIsInvalid(nameof(capacity));

            return new StoragePlace(name, capacity, orderId);
        }

        public Result<bool, Error> IsOrderCorrectForPutInStorage(
            Guid orderId, 
            int itemsCount)
        {
            if (OrderId != null)
                return GeneralErrors.ValueIsInvalid(nameof(orderId));

            if (TotalVolume - itemsCount < 0)
                return GeneralErrors.ValueIsInvalid(nameof(itemsCount));

            return true;
        }

        public Result<UnitResult<Error>, Error> PutOrderInStorage(
            Guid orderId, 
            int itemsCount)
        {
            var isOrderCorrect = IsOrderCorrectForPutInStorage(orderId, itemsCount);

            if (!isOrderCorrect.IsSuccess)
                return GeneralErrors.ValueIsInvalid(nameof(isOrderCorrect));

            OrderId = orderId;

            return UnitResult.Success<Error>();
        }

        public Result<UnitResult<Error>, Error> OutputOrderOfStorage()
        {
            if(OrderId == null)
                return GeneralErrors.ValueIsInvalid(nameof(OrderId));

            OrderId = null;

            return UnitResult.Success<Error>();
        }

        private bool IsBusy() => OrderId != null;
    }
}