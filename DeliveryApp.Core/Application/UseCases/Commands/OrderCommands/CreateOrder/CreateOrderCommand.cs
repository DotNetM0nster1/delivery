using CSharpFunctionalExtensions;
using Primitives;
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Commands.OrderCommands.ChangeOrder
{
    public sealed class CreateOrderCommand : IRequest<UnitResult<Error>> 
    {
        public CreateOrderCommand(
            Guid basketId, 
            string street,
            int volume)
        {
            if (basketId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(basketId));            
            }

            if (string.IsNullOrWhiteSpace(street))
            {
                throw new ArgumentNullException(nameof(street));
            }

            if (volume <= 0)
            {
                throw new ArgumentException(nameof(volume));
            }

            BasketId = basketId;
            Street = street;
            Volume = volume;
        }

        public Guid BasketId { get; set; }

        public string Street { get; set; }

        public int Volume { get; set; }
    }
}