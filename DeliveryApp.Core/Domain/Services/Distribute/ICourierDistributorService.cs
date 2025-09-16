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
    public interface ICourierDistributorService
    {
        public Result<UnitResult<Error>, Error> DistributeOrderOnCouriers(Order order, List<Courier> couriers);
    }
}