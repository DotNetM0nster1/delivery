using DeliveryApp.Core.Domain.Model.OrderAggregate;
using Microsoft.Extensions.Logging;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Ports;
using Primitives;
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Commands.OrderCommands.CreateOrder
{
    public sealed class CreateOrderHandler(
        ILogger<CreateOrderHandler> logger,
        IOrderRepository orderRepository, 
        IUnitOfWork unitOfWork, 
        IGeoClient geoClient) 
        : IRequestHandler<CreateOrderCommand, UnitResult<Error>>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly ILogger<CreateOrderHandler> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IGeoClient _geoClient = geoClient;

        public async Task<UnitResult<Error>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            if (await _geoClient.GetLocation(request.Street)
                is var locationResult && (locationResult.IsFailure || locationResult.Value == null))
            {
                _logger.LogWarning($"[{nameof(Handle)}] Not found location by name {request.Street}");
                return UnitResult.Failure(GeneralErrors.NotFound());
            }

            var location = locationResult.Value;    

            var createOrderResult = Order.Create(request.OrderId, location, request.Volume).Value;

            await _orderRepository.AddAsync(createOrderResult);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return UnitResult.Success<Error>();
        }
    }
}