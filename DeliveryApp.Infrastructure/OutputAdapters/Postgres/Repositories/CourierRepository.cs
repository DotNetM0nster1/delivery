using DeliveryApp.Infrastructure.OutputAdapters.Postgres.ApplicationContext;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using Microsoft.EntityFrameworkCore;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Ports;

namespace DeliveryApp.Infrastructure.OutputAdapters.Postgres.Repositories
{
    public sealed class CourierRepository(ApplicationDatabaseContext applicationContext) : ICourierRepository
    {
        private readonly ApplicationDatabaseContext _applicationContext = applicationContext;

        public async Task AddAsync(Courier courier)
        {
            if (courier == null)
            {
                throw new ArgumentNullException(nameof(courier));
            }

            await _applicationContext.Couriers.AddAsync(courier);
        }

        public async Task AddRangeAsync(IEnumerable<Courier> couriers)
        {
            if (couriers == null || couriers.Count() == 0)
            {
                throw new ArgumentNullException(nameof(couriers));
            }

            await _applicationContext.Couriers.AddRangeAsync(couriers);
        }

        public async Task<Maybe<Courier>> GetByIdAsync(Guid courierId)
        {
            if (courierId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(Courier));
            }

            return await _applicationContext
                .Couriers
                .Include(courier => courier.StoragePlaces)
                .FirstOrDefaultAsync(courier => courier.Id == courierId);
        }

        public async Task<List<Courier>> GetAllFreeCouriersAsync()
        {
            return await _applicationContext
                .Couriers
                .Include(courier => courier.StoragePlaces)
                .Where(courier => courier.StoragePlaces.All(couriers => couriers.OrderId == null))
                .ToListAsync();
        }

        public void Update(Courier courier)
        {
            if (courier == null)
            {
                throw new ArgumentNullException(nameof(courier));
            }

            _applicationContext.Couriers.Update(courier);
        }
    }
}