using CSharpFunctionalExtensions;
using Primitives;
using System.Diagnostics.CodeAnalysis;

namespace DeliveryApp.Core.Domain.Model.CourierAggregate
{
    public sealed class StoragePlace : Entity<Guid>
    {
        [ExcludeFromCodeCoverage]
        private StoragePlace() { }

        private StoragePlace(string name, int capacity, Guid? orderId = null)
        { 
            Id = Guid.NewGuid();
            Name = name;
            MaxCapacity = capacity;
            OrderId = orderId;
        }

        public static StoragePlace Backpack = CreateStorage("Backpack", 10).Value;
        public static StoragePlace BikeRake = CreateStorage("Bike rake", 15).Value;
        public static StoragePlace CarTrunk = CreateStorage("Car trunk", 100).Value;
        public static StoragePlace MotobikeRake = CreateStorage("Motobike Rake", 20).Value;

        public string Name { get; }

        public int MaxCapacity { get; private set; }

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

            if (MaxCapacity - itemsCount < 0)
                return GeneralErrors.ValueIsInvalid(nameof(itemsCount));

            return true;
        }

        public Result<StoragePlace, Error> PutOrderInStorage(
            Guid orderId, 
            int itemsCount)
        {
            var isOrderCorrect = IsOrderCorrectForPutInStorage(orderId, itemsCount).IsSuccess;

            if (!isOrderCorrect)
                return GeneralErrors.ValueIsInvalid(nameof(isOrderCorrect));

            OrderId = orderId;

            return this;
        }

        public Result<StoragePlace, Error> OutputOrderOfStorage()
        {
            if(OrderId == null)
                return GeneralErrors.ValueIsInvalid(nameof(OrderId));

            OrderId = null;

            return this;
        }

        private bool IsBusy() => OrderId != null;
    }
}