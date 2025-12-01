using CSharpFunctionalExtensions;
using Primitives;
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Commands.OrderCommands.AssignOrder
{
    public sealed class AssignOrdersCommand : IRequest<UnitResult<Error>>;
}