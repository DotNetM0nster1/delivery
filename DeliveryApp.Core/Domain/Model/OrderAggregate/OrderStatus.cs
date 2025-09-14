using CSharpFunctionalExtensions;
using System.Diagnostics.CodeAnalysis;

namespace DeliveryApp.Core.Domain.Model.OrderAggregate
{
    public sealed class OrderStatus : ValueObject
    {
        [ExcludeFromCodeCoverage]
        private OrderStatus() { }

        private OrderStatus(string name)
        {
            Name = name;
        }

        public static OrderStatus Created => new(nameof(Created).ToLowerInvariant());

        public static OrderStatus Assigned => new(nameof(Assigned).ToLowerInvariant());

        public static OrderStatus Completed => new(nameof(Completed).ToLowerInvariant());

        public string Name { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }
}