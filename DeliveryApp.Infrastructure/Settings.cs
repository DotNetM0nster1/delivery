﻿namespace DeliveryApp.Infrastructure;

public sealed class Settings
{
    public string ConnectionString { get; set; }
    public string GeoServiceGrpcHost { get; set; }
    public string MessageBrokerHost { get; set; }
    public string OrderStatusChangedTopic { get; set; }
    public string BasketConfirmedTopic { get; set; }
}