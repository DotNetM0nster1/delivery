using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Ports;
using Primitives;
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Commands.Order.CreateOrder
{
    public sealed class CreateOrderHandler(IOrderRepository databaseContext, IUnitOfWork unitOfWork) : IRequestHandler<CreateOrderCommand, UnitResult<Error>>
    {
        private readonly IOrderRepository _databaseContext = databaseContext;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<UnitResult<Error>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            if (Location.CreateRandomLocation() is 
                var createRandomLocationResult && (createRandomLocationResult.IsFailure || createRandomLocationResult.Error != null))
            {
                throw new ArgumentNullException(nameof(createRandomLocationResult));
            }

            if (Order.Create(request.OrderId, createRandomLocationResult.Value, request.Volume) is 
                var createOrderResult && createOrderResult.IsFailure || createOrderResult.Value == null)
            {
                throw new ArgumentNullException(nameof(createOrderResult));
            }

            await _databaseContext.AddAsync(createOrderResult.Value);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return UnitResult.Success<Error>();
        }
    }
}