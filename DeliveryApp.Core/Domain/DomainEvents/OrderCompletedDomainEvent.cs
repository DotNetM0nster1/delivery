using DeliveryApp.Core.Domain.Model.OrderAggregate;
using Primitives;

namespace DeliveryApp.Core.Domain.DomainEvents
{
    public sealed record class OrderCompletedDomainEvent(Order Order) : DomainEvent { }
}