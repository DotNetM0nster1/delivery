using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using Primitives;
using System;

namespace DeliveryApp.Core.Domain.Services.Distribute
{
    public interface ICourierDistributorService
    {
        public Result<UnitResult<Error>, Error> DistributeOrderOnCouriers(Order order, List<Courier> couriers);
    }
}