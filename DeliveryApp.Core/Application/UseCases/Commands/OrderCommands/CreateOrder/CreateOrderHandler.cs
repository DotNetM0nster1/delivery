using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using Microsoft.Extensions.Logging;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Ports;
using Primitives;
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Commands.OrderCommands.ChangeOrder
{
    public sealed class CreateOrderHandler(
        ILogger<CreateOrderHandler> logger,
        IOrderRepository orderRepository, 
        IUnitOfWork unitOfWork) 
        : IRequestHandler<CreateOrderCommand, UnitResult<Error>>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly ILogger<CreateOrderHandler> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<UnitResult<Error>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var randomLocation = Location.CreateRandomLocation().Value;

            if (Order.Create(request.BasketId, randomLocation, request.Volume) is 
                var orderResult && orderResult.IsFailure)
            {
                _logger.LogError($"[{nameof(Handle)}] Cant create order");
                
                UnitResult.Failure(GeneralErrors.CreateOrderError(orderResult.Error));
            }

            var order = orderResult.Value;

            await _orderRepository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return UnitResult.Success<Error>();
        }
    }
}