using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using Primitives;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace DeliveryApp.Core.Domain.Model.CourierAggregate
{
    public sealed class Courier : Aggregate<Guid>
    {
        [ExcludeFromCodeCoverage]
        private Courier() { }

        private Courier(int courierSpeed, string courierName, Location currentCourierLocation)
        {
            Id = Guid.NewGuid();
            CourierName = courierName;
            CourierSpeed = courierSpeed;
            CurrentCourierLocation = currentCourierLocation;
            CourierStoragePlaces = [StoragePlace.Bag];
        }

        public int CourierSpeed { get; }

        public string CourierName { get; }

        public List<StoragePlace> CourierStoragePlaces { get; }

        public Location CurrentCourierLocation { get; private set; }

        public static Result<Courier, Error> Create(
            int courierSpeed,
            string courierName,
            Location courierLocation)
        {
            if (string.IsNullOrWhiteSpace(courierName))
                return GeneralErrors.ValueIsRequired(nameof(courierName));

            if (courierSpeed <= 0)
                return GeneralErrors.ValueIsRequired(nameof(courierSpeed));

            if (courierLocation == null)
                return GeneralErrors.ValueIsRequired(nameof(courierLocation));

            return new Courier(courierSpeed, courierName, courierLocation);
        }

        public Result<UnitResult<Error>, Error> AddNewStoragePlace(string storageName, int storageVolume)
        {
            if (StoragePlace.Create(storageName, storageVolume) is var createStorageResult && createStorageResult.IsFailure)
                return GeneralErrors.StorageCannotBeCreated(createStorageResult.Error);

            CourierStoragePlaces.Add(createStorageResult.Value);
            CourierStoragePlaces.TrimExcess();

            return UnitResult.Success<Error>();
        }

        public Result<bool, Error> IsCanTakeOrder(Order order)
        {
            foreach (var storage in CourierStoragePlaces.Select(storage => storage.IsOrderCorrectForAdd(order)))
            {
                if (storage.IsFailure) return GeneralErrors.IsCourierCanTakeOrderError(storage.Error);
                if (storage.Value) return true;
            }

            return false;
        }

        public UnitResult<Error> TakeOrder(Order order)
        {
            if (IsCanTakeOrder(order) is var takeOrderResult && (takeOrderResult.IsFailure || !takeOrderResult.Value))
                return GeneralErrors.TakeOrderError(takeOrderResult.IsFailure ? takeOrderResult.Error : null);

            foreach (var storage in CourierStoragePlaces)
            {
                if (storage.AddOrder(order) is var storeOrderResult && storeOrderResult.IsSuccess)
                    return UnitResult.Success<Error>();
            }

            return GeneralErrors.NotFoundMatchingStorageForStoreOrder();
        }

        public Result<UnitResult<Error>, Error> FinishOrder(Guid orderId)
        {
            if (orderId == Guid.Empty)
                return GeneralErrors.ValueIsRequired(nameof(orderId));

            if(CourierStoragePlaces.Count == 0)
                return GeneralErrors.ValueIsRequired(nameof(CourierStoragePlaces.Count));

            var storagePlace = CourierStoragePlaces.FirstOrDefault(order => order.OrderId == orderId);
            
            if(storagePlace == null)
                return GeneralErrors.ValueIsRequired(nameof(storagePlace));

            storagePlace.RemoveOrder();

            return UnitResult.Success<Error>();
        }

        public Result<double, Error> GetStepsCountToTargetLocation(Location targetLocation)
        {
            if (targetLocation == null)
                return GeneralErrors.ValueIsRequired(nameof(targetLocation));

            if (targetLocation.CalculateDistanceToTargetLocation(CurrentCourierLocation) is var calculateDistanceResult && calculateDistanceResult.IsFailure)
                return GeneralErrors.CourierCannotCalculateDistanceToTargetLocation(calculateDistanceResult.Error);

            return (double)calculateDistanceResult.Value / CourierSpeed;
        }

        public UnitResult<Error> Move(Location targetLocation)
        {
            if (targetLocation == null) 
                return GeneralErrors.ValueIsRequired(nameof(targetLocation));

            var offsetByX = targetLocation.X - CurrentCourierLocation.X;
            var offsetByY = targetLocation.Y - CurrentCourierLocation.Y;
            var cruisingRange = CourierSpeed;

            var xMove = Math.Clamp(offsetByX, -cruisingRange, cruisingRange);
            cruisingRange -= Math.Abs(xMove);

            var yMove = Math.Clamp(offsetByY, -cruisingRange, cruisingRange);

            CurrentCourierLocation = Location.Create(CurrentCourierLocation.X + xMove, CurrentCourierLocation.Y + yMove).Value;

            return UnitResult.Success<Error>();
        }
    }
}