using DeliveryApp.Infrastructure.OutputAdapters.Postgres.ApplicationContext;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Ports;

namespace DeliveryApp.Infrastructure.OutputAdapters.Postgres.Repositories
{
    public sealed class OrderRepository(ApplicationDatabaseContext applicationContext) : IOrderRepository
    {
        private readonly ApplicationDatabaseContext _databaseContext = applicationContext;

        public async Task AddAsync(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            await _databaseContext.Orders.AddAsync(order);
        }

        public async Task<Maybe<Order>> GetByIdAsync(Guid orderId)
        {
            if (orderId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(orderId));
            }

            return await _databaseContext
                .Orders
                .FirstOrDefaultAsync(order => order.Id == orderId);
        }

        public async Task<Maybe<Order>> GetFirstOrderWithCreatedStatusAsync()
        {
            return await _databaseContext
                .Orders
                .FirstOrDefaultAsync(order => order.Status.Name == OrderStatus.Created.Name);
        }

        public async Task<List<Order>> GetAllAssignedOrdersAsync()
        {
            return await _databaseContext
                .Orders
                .Where(order => order.Status.Name == OrderStatus.Assigned.Name)
                .ToListAsync();
        }

        public void Update(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            _databaseContext.Orders.Update(order);
        }
    }
}