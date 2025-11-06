using CSharpFunctionalExtensions;
using Primitives;
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Commands.Order.CreateOrder
{
    public sealed class CreateOrderCommand : IRequest<UnitResult<Error>>
    {
        public CreateOrderCommand(string street, Guid orderId, int volume)
        {
            if (string.IsNullOrWhiteSpace(street))
            {
                throw new ArgumentNullException(nameof(street));
            }

            if (orderId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(orderId));
            }

            if (volume <= 0)
            {
                throw new ArgumentException(nameof(volume));
            }

            OrderId = orderId;
            Street = street;
            Volume = volume;
        }

        public string Street { get; }

        public Guid OrderId { get; }

        public int Volume { get; }
    }
}