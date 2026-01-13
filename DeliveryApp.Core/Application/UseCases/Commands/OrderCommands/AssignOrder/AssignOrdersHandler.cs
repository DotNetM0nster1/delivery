using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.Services.Distribute;
using Microsoft.Extensions.Logging;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Ports;
using Primitives;
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Commands.OrderCommands.AssignOrder
{
    public sealed class AssignOrdersHandler(
        ICourierDistributorService courierDistributorService,
        ICourierRepository courierRepository,
        ILogger<AssignOrdersHandler> logger,
        IOrderRepository orderRepository, 
        IUnitOfWork unitOfWork) 
        : IRequestHandler<AssignOrdersCommand, UnitResult<Error>>
    {
        private readonly ICourierDistributorService _courierDistributorService = courierDistributorService;
        private readonly ICourierRepository _courierRepository = courierRepository;
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly ILogger<AssignOrdersHandler> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<UnitResult<Error>> Handle(AssignOrdersCommand request, CancellationToken cancellationToken)
        {
            if (await _orderRepository.GetFirstOrderWithCreatedStatusAsync() is
                var mayBeFirstOrderWithCreatedStatus && 
                (mayBeFirstOrderWithCreatedStatus == null || mayBeFirstOrderWithCreatedStatus.HasNoValue))
            {
                _logger.LogWarning($"[{nameof(Handle)}] Not found order with {nameof(OrderStatus.Created)} status");

                return UnitResult.Failure(GeneralErrors.NotFound(nameof(mayBeFirstOrderWithCreatedStatus)));
            }

            var firstOrderWithCreatedStatus = mayBeFirstOrderWithCreatedStatus.Value;

            if (await _courierRepository.GetAllFreeCouriersAsync() is
                var allFreeCouriers && (allFreeCouriers == null || allFreeCouriers.Count == 0))
            {
                _logger.LogWarning($"[{nameof(Handle)}] Not found free couriers");

                return UnitResult.Failure(GeneralErrors.NotFound(nameof(allFreeCouriers)));
            }

            if (_courierDistributorService.DistributeOrderOnCouriers(firstOrderWithCreatedStatus, allFreeCouriers) is
                var distributeResult && distributeResult.IsFailure)
            {
                _logger.LogWarning($"[{nameof(Handle)}] Cant distribute order on courier {distributeResult.Error.Message}");

                return UnitResult.Failure(GeneralErrors.DistributeOrderOnCouriersError(distributeResult.Error));
            }

            _courierRepository.Update(distributeResult.Value);
            _orderRepository.Update(firstOrderWithCreatedStatus);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"[{nameof(Handle)}] " +
                $"Success assign order {firstOrderWithCreatedStatus.Id} " +
                $"to courier {distributeResult.Value.Id}");

            return distributeResult;
        }
    }
}