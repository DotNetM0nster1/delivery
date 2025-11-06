using DeliveryApp.Infrastructure.OutputAdapters.Postgres.ApplicationContext;
using Primitives;

namespace DeliveryApp.Infrastructure.OutputAdapters.Postgres
{
    public sealed class UnitOfWork(ApplicationDatabaseContext applicationContext) : IUnitOfWork
    {
        private readonly ApplicationDatabaseContext _applicationContext = applicationContext;

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _applicationContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}