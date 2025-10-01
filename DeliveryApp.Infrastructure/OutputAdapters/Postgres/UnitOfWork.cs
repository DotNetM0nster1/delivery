using DeliveryApp.Infrastructure.OutputAdapters.Postgres.ApplicationContext;
using Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryApp.Infrastructure.OutputAdapters.Postgres
{
    public class UnitOfWork(ApplicationDatabaseContext applicationContext) : IUnitOfWork
    {
        private readonly ApplicationDatabaseContext _applicationContext = applicationContext;

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _applicationContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}