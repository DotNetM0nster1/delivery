using DeliveryApp.Core.Domain.Services.Distribute;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Ports;
using Primitives;
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Commands.Order.AssignOrder
{
    public sealed class AssignOrderHandler(
        ICourierDistributorService courierDistributorService,
        ICourierRepository courierRepository,
        IOrderRepository orderRepository, 
        IUnitOfWork unitOfWork) 
        : IRequestHandler<AssignOrderCommand, UnitResult<Error>>
    {
        private readonly ICourierDistributorService _courierDistributorService = courierDistributorService;
        private readonly ICourierRepository _courierRepository = courierRepository;
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<UnitResult<Error>> Handle(AssignOrderCommand request, CancellationToken cancellationToken)
        {
            if (await _orderRepository.GetFirstWithCreatedStatusAsync() is
                var mayBeFirstOrderWithCreatedStatus && (mayBeFirstOrderWithCreatedStatus == null || mayBeFirstOrderWithCreatedStatus.HasNoValue))
            {
                throw new ArgumentNullException(nameof(mayBeFirstOrderWithCreatedStatus));
            }

            var firstOrderWithCreatedStatus = mayBeFirstOrderWithCreatedStatus.Value;

            if (await _courierRepository.GetAllFreeCouriersAsync() is
                var allFreeCouriers && (allFreeCouriers == null || allFreeCouriers.Count == 0))
            {
                throw new ArgumentNullException(nameof(allFreeCouriers));
            }

            if (_courierDistributorService.DistributeOrderOnCouriers(firstOrderWithCreatedStatus, allFreeCouriers) is
                var distributeResult && (distributeResult.IsFailure || distributeResult.Error != null))
            {
                throw new ArgumentNullException(nameof(distributeResult));
            }

            _orderRepository.Update(firstOrderWithCreatedStatus);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return distributeResult;
        }
    }
}