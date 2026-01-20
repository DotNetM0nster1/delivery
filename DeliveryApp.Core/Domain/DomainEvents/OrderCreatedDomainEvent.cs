using DeliveryApp.Core.Domain.Model.OrderAggregate;
using MediatR;
using Primitives;

namespace DeliveryApp.Core.Domain.DomainEvents
{
    public record class OrderCreatedDomainEvent(Order Order) : DomainEvent { }
}