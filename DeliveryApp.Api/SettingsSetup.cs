using Microsoft.Extensions.Options;
using DeliveryApp.Infrastructure;

namespace DeliveryApp.Api;

public sealed class SettingsSetup : IConfigureOptions<Settings>
{
    private readonly IConfiguration _configuration;

    public SettingsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(Settings options)
    {
        options.ConnectionString = _configuration["CONNECTION_STRING"];
        options.GeoServiceGrpcHost = _configuration["GEO_SERVICE_GRPC_HOST"];
        options.MessageBrokerHost = _configuration["MESSAGE_BROKER_HOST"];
        options.OrderUpdateTopic = _configuration["ORDER_STATUS_CHANGED_TOPIC"];
        options.BasketConfirmedTopic = _configuration["BASKET_CONFIRMED_TOPIC"];
    }
}