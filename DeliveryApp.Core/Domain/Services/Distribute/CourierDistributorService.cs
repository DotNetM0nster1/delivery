using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryApp.Core.Domain.Services.Distribute
{
    public sealed class CourierDistributorService : ICourierDistributorService
    {
        public Result<UnitResult<Error>, Error> DistributeOrderOnCouriers(Order order, List<Courier> couriers)
        {
            if (order == null)
                return GeneralErrors.ValueIsRequired(nameof(order));

            if (couriers == null || couriers.Count == 0)
                return GeneralErrors.ValueIsRequired(nameof(couriers));

            if (GetMostSituableCourierForOrder(couriers, order) is var situableCourierResult && situableCourierResult.IsFailure)
                return GeneralErrors.OrderCannotBeDistributedError(situableCourierResult.Error);

            if(order.Assign(situableCourierResult.Value) is var assignOrderResult && assignOrderResult.IsFailure)
                return GeneralErrors.OrderCannotBeDistributedError(assignOrderResult.Error);

            if(situableCourierResult.Value.TakeOrder(order) is var takingOrderResult && takingOrderResult.IsFailure) 
                return GeneralErrors.OrderCannotBeDistributedError(takingOrderResult.Error);

            return UnitResult.Success<Error>();
        }

        private Result<Courier, Error> GetMostSituableCourierForOrder(List<Courier> couriers, Order order)
        {
            Dictionary<Courier, double> courierAndStepsPair = [];

            foreach (var courier in couriers)
            {
                if (courier.IsCanTakeOrder(order) is var isCanTakeOrderResult && (isCanTakeOrderResult.IsFailure || !isCanTakeOrderResult.Value))
                    continue;

                if (courier.GetStepsCountToTargetLocation(order.Location) is var courierStepsToTargetLocationResult && !courierAndStepsPair.ContainsKey(courier))
                    courierAndStepsPair[courier] = courierStepsToTargetLocationResult.Value;
            }

            if (courierAndStepsPair.Count == 0)
                return GeneralErrors.NotFoundMatchingCourierForOrder();

            var matchingCourier = courierAndStepsPair.OrderByDescending(steps => steps.Value).Select(courier => courier).Last().Key;

            return matchingCourier;
        }
    }
}