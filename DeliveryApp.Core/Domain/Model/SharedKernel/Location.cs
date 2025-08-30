using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.Model.SharedKernel
{
    public sealed class Location : ValueObject
    {
        public int X { get; }

        public int Y { get; }

        private Location() { }

        private Location(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Result<Location, Error> CreateLocation(int x, int y)
        {
            const int maxBorder = 10;
            const int minBorder = 0;

            if (x > maxBorder || x <= minBorder)
                return GeneralErrors.ValueIsInvalid(nameof(x));

            if (y > maxBorder || y <= minBorder)
                return GeneralErrors.ValueIsInvalid(nameof(y));

            return new Location(x, y);
        }

        public static Result<Location, Error> CreateRandomLocation()
        {
            const int maxValueBorder = 11;
            const int minValueBorder = 1;

            var random = new Random();

            var randomX = random.Next(minValueBorder, maxValueBorder);
            var randomY = random.Next(minValueBorder, maxValueBorder);

            return CreateLocation(randomX, randomY);
        }

        public Result<int, Error> CalculateCurierDistanceToTargetLocation(Location target)
        {
            var stepsCountByCoordinateX = CalculateCurierStepsByCoordinate(X, target.X);
            var stepsCountByCoordinateY = CalculateCurierStepsByCoordinate(Y, target.Y);

            var curierStepsCountByTargetLocation = stepsCountByCoordinateX + stepsCountByCoordinateY;

            return curierStepsCountByTargetLocation;
        }

        private int CalculateCurierStepsByCoordinate(int coordinateFrom, int coordinateTo)
        {
            var coordinateMoveResult = 0;

            while (coordinateFrom != coordinateTo)
            { 
                if (coordinateTo > coordinateFrom) coordinateFrom++;
                if (coordinateTo < coordinateFrom) coordinateFrom--;

                coordinateMoveResult++;    
            }

            return coordinateMoveResult;
        }

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return X;
            yield return Y;
        }
    }
}