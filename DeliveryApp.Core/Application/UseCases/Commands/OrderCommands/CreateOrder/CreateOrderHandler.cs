using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Ports;
using Primitives;
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Commands.OrderCommands.CreateOrder
{
    public sealed class CreateOrderHandler(
        IOrderRepository databaseContext, 
        IUnitOfWork unitOfWork) 
        : IRequestHandler<CreateOrderCommand, UnitResult<Error>>
    {
        private readonly IOrderRepository _orderRepository = databaseContext;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<UnitResult<Error>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var randomLocation = Location.CreateRandomLocation().Value;

            var createOrderResult = Order.Create(request.OrderId, randomLocation, request.Volume).Value;

            await _orderRepository.AddAsync(createOrderResult);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return UnitResult.Success<Error>();
        }
    }
}