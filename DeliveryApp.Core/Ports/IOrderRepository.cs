﻿using DeliveryApp.Core.Domain.Model.OrderAggregate;
using CSharpFunctionalExtensions;

namespace DeliveryApp.Core.Ports
{
    public interface IOrderRepository
    {
        public Task AddAsync(Order order);

        public Task<Maybe<Order>> GetByIdAsync(Guid id);

        public Task<Maybe<Order>> GetFirstWithCreatedStatusAsync();

        public Task<List<Order>> GetAllAssignedAsync();

        public void Update(Order order);
    }
}