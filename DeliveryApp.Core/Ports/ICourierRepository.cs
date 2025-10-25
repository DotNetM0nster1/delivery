using DeliveryApp.Core.Domain.Model.CourierAggregate;
using CSharpFunctionalExtensions;

namespace DeliveryApp.Core.Ports
{
    public interface ICourierRepository
    {
        public Task AddAsync(Courier courier);

        public Task AddRangeAsync(IEnumerable<Courier> couriers);

        public Task<Maybe<Courier>> GetByIdAsync(Guid id);

        public Task<List<Courier>> GetAllFreeCouriersAsync();

        public void Update(Courier courier);
    }
}