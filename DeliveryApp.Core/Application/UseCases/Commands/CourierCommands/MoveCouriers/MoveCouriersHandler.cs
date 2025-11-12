using CSharpFunctionalExtensions;
using DeliveryApp.Core.Ports;
using Primitives;
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Commands.CourierCommands.MoveCouriers
{
    public sealed class MoveCouriersHandler(
        ICourierRepository courierRepository, 
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork) 
        : IRequestHandler<MoveCouriersCommand, UnitResult<Error>>
    {
        private readonly ICourierRepository _courierRepository = courierRepository;
        private readonly IOrderRepository _orderRepository = orderRepository; 
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<UnitResult<Error>> Handle(MoveCouriersCommand request, CancellationToken cancellationToken)
        {
            if (await _orderRepository.GetAllAssignedAsync() is
                var allAssignedOrders && allAssignedOrders == null || allAssignedOrders.Count == 0)
            {
                throw new ArgumentNullException(nameof(allAssignedOrders));    
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