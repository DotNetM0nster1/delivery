using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryApp.Core.Ports
{
    public interface IOrderRepository
    {
        public Task AddAsync(Order order);

        public void Update(Order order);

        public Task<Maybe<Order>> GetAsync(Guid id);

        public Task<Maybe<Order>> GetFirstWithCreatedStatusAsync();

        public Task<List<Order>> GetAllAssignedAsync();
    }
}