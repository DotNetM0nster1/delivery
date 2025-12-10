using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.Model.SharedKernel
{
    public sealed class Location : ValueObject
    {
        [ExcludeFromCodeCoverage]
        private Location() { }

        private Location(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }

        public int Y { get; }

        private const int MaxCoordinateValue = 10;
        private const int MinCoordinateValue = 1;

        public static Result<Location, Error> Create(int x, int y)
        {
            if (x > MaxCoordinateValue || x < MinCoordinateValue)
            {
                return GeneralErrors.ValueIsInvalid(nameof(x));
            }

            if (y > MaxCoordinateValue || y < MinCoordinateValue)
            {
                return GeneralErrors.ValueIsInvalid(nameof(y));
            }

            return new Location(x, y);
        }

        public Result<int, Error> CalculateDistanceToTargetLocation(Location targetLocation)
        {
            if (targetLocation == null)
            {
                return GeneralErrors.ValueIsRequired(nameof(targetLocation));
            }

            var stepsCountByCoordinateX = Math.Abs(X - targetLocation.X);
            var stepsCountByCoordinateY = Math.Abs(Y - targetLocation.Y);

            var courierStepsCountByTargetLocation = stepsCountByCoordinateX + stepsCountByCoordinateY;

            return courierStepsCountByTargetLocation;
        }

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return X;
            yield return Y;
        }
    }
}