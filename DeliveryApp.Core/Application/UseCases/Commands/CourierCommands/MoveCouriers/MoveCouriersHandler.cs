using DeliveryApp.Core.Application.UseCases.Commands.OrderCommands.AssignOrder;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using Microsoft.Extensions.Logging;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Ports;
using Primitives;
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Commands.CourierCommands.MoveCouriers
{
    public sealed class MoveCouriersHandler(
        ICourierRepository courierRepository,
        ILogger<MoveCouriersHandler> logger,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork) 
        : IRequestHandler<MoveCouriersCommand, UnitResult<Error>>
    {
        private readonly ICourierRepository _courierRepository = courierRepository;
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly ILogger<MoveCouriersHandler> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<UnitResult<Error>> Handle(MoveCouriersCommand request, CancellationToken cancellationToken)
        {
            if (await _orderRepository.GetAllAssignedOrdersAsync() is
                var allAssignedOrders && allAssignedOrders == null || allAssignedOrders.Count == 0)
            {
                _logger.LogWarning($"[{nameof(Handle)}] Not found orders with {nameof(OrderStatus.Assigned)} status");

                return UnitResult.Failure(GeneralErrors.NotFound());
            }

            foreach (var assignedOrder in allAssignedOrders)
            {
                if (await _courierRepository.GetByIdAsync(assignedOrder.CourierId.Value) is
                    var mayBeCourierFromAssignedOrder && (mayBeCourierFromAssignedOrder == null || mayBeCourierFromAssignedOrder.HasNoValue))
                {
                    throw new ArgumentNullException(nameof(mayBeCourierFromAssignedOrder));       
                }

                var courierFromAssignedOrder = mayBeCourierFromAssignedOrder.Value;

                var moveCourierResult = courierFromAssignedOrder.Move(assignedOrder.Location);

                if (courierFromAssignedOrder.Location == assignedOrder.Location)
                {
                    courierFromAssignedOrder.FinishOrder(assignedOrder.Id);
                    assignedOrder.Complete();
                }

                _courierRepository.Update(courierFromAssignedOrder);
                _orderRepository.Update(assignedOrder);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return UnitResult.Success<Error>();
        }
    }
}