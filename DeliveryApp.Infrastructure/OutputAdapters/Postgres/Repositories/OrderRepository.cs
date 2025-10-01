using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Ports;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres.ApplicationContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryApp.Infrastructure.OutputAdapters.Postgres.Repositories
{
    public class OrderRepository(ApplicationDatabaseContext  applicationContext) : IOrderRepository
    {
        private readonly ApplicationDatabaseContext _databaseContext = applicationContext;

        public async Task AddAsync(Order order)
        {
            if(order == null) throw new ArgumentNullException(nameof(order));

            await _databaseContext.Orders.AddAsync(order);
        }

        public async Task<List<Order>> GetAllAssignedAsync()
        {
            var assignedOrders = await _databaseContext
                .Orders
                .Where(order => order.Status.Name == OrderStatus.Assigned.Name)
                .ToListAsync();

            if(assignedOrders == null || assignedOrders.Count == 0) 
                throw new ArgumentNullException(nameof(OrderStatus.Assigned));

            return assignedOrders;
        }

        public async Task<Order> GetAsync(Guid orderId)
        {
            if(orderId == Guid.Empty) throw new ArgumentNullException(nameof(orderId));

            var order = await _databaseContext
                .Orders
                .FirstOrDefaultAsync(order => order.Id == orderId);

            if(order == null) throw new ArgumentNullException(nameof(Order));

            return order;
        }

        public async Task<Order> GetFirstWithCreatedStatusAsync()
        {
            var order = await _databaseContext
                .Orders
                .FirstOrDefaultAsync(order => order.Status.Name == OrderStatus.Created.Name);

            if(order == null) throw new ArgumentNullException(nameof(Order));

            return order;
        }

        public void Update(Order order)
        {
            if(order == null) throw new ArgumentNullException(nameof(order));

            _databaseContext.Orders.Update(order);
        }
    }
}