using DeliveryApp.Core.Domain.Services.Distribute;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryApp.Api.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCourierDistributorService(this IServiceCollection service)
        {
            return service.AddScoped<ICourierDistributorService, CourierDistributorService>();
        }
    }
}