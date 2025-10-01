using DeliveryApp.Core.Domain.Model.CourierAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryApp.Core.Ports
{
    public interface ICourierRepository
    {
        public Task AddAsync(Courier courier);

        public Task AddRangeAsync(IEnumerable<Courier> couriers);

        public void Update(Courier courier);

        public Task<Courier> GetByIdAsync(Guid id);

        public Task<List<Courier>> GetAllFreeCouriers();
    }
}