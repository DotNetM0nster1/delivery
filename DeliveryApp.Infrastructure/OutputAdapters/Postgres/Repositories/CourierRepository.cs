using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Ports;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres.ApplicationContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryApp.Infrastructure.OutputAdapters.Postgres.Repositories
{
    public class CourierRepository(ApplicationDatabaseContext applicationContext) : ICourierRepository
    {
        private readonly ApplicationDatabaseContext _applicationContext = applicationContext;

        public async Task AddAsync(Courier courier)
        {
            if (courier == null) throw new ArgumentNullException(nameof(courier));

            await _applicationContext.Couriers.AddAsync(courier);
        }

        public async Task AddRangeAsync(IEnumerable<Courier> couriers)
        {
            if (couriers == null || couriers.Count() == 0) 
                throw new ArgumentNullException(nameof(couriers));

            await _applicationContext.Couriers.AddRangeAsync(couriers);
        }

        public async Task<List<Courier>> GetAllFreeCouriers()
        {
            var couriers = await _applicationContext.Couriers.Where(courier => 
                courier.StoragePlaces.Where(storagePlace => 
                    storagePlace.OrderId == null).Count() == courier.StoragePlaces.Count)
                .ToListAsync();

            if(couriers == null || couriers.Count == 0)
                throw new ArgumentNullException(nameof(Courier));

            return couriers;
        }

        public async Task<Courier> GetByIdAsync(Guid courierId)
        {
            if(courierId == Guid.Empty) throw new ArgumentNullException(nameof(Courier));

            var courier = await _applicationContext.Couriers.FirstOrDefaultAsync(courier => courier.Id == courierId)
                ?? throw new ArgumentNullException(nameof(Courier));

            return courier;
        }

        public void Update(Courier courier) => _applicationContext.Couriers.Update(courier);
    }
}