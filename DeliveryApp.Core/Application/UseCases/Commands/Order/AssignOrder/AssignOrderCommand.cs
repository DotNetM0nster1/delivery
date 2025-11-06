using CSharpFunctionalExtensions;
using Primitives;
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Commands.Order.AssignOrder
{
    public sealed class AssignOrderCommand : IRequest<UnitResult<Error>>;
}