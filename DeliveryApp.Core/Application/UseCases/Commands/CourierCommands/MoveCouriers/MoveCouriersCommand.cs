using CSharpFunctionalExtensions;
using Primitives;
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Commands.CourierCommands.MoveCouriers
{
    public sealed class MoveCouriersCommand : IRequest<UnitResult<Error>>;
}