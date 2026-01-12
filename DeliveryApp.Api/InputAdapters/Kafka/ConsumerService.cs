using DeliveryApp.Core.Application.UseCases.Commands.OrderCommands.ChangeOrder;
using Newtonsoft.Json;
using Confluent.Kafka;
using Queues.Basket;
using MediatR;

namespace DeliveryApp.Api.InputAdapters.Kafka
{
    public sealed class ConsumerService : BackgroundService
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly IMediator _mediator;
        private readonly string _topic;

        public ConsumerService(IMediator mediator, string messageBrockerHost, string topic)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            if (string.IsNullOrWhiteSpace(messageBrockerHost))
            {
                throw new ArgumentNullException(nameof(messageBrockerHost));
            }

            if (string.IsNullOrWhiteSpace(topic))
            {
                throw new ArgumentNullException(nameof(topic));
            }

            var config = new ConsumerConfig
            {
                BootstrapServers = messageBrockerHost,
                GroupId = "DeliveryConsumerGroup",
                EnableAutoOffsetStore = false,
                EnableAutoCommit = true,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnablePartitionEof = true
            };

            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            _topic = topic;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(_topic);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);

                    if (_consumer.Consume(stoppingToken) is
                        var consumeResult && consumeResult != null && consumeResult.IsPartitionEOF)
                    {
                        continue;
                    }

                    var orderUpdateEventIntegration =
                        JsonConvert.DeserializeObject<BasketConfirmedIntegrationEvent>(consumeResult.Message.Value);

                    var orderUpdateCommand = new CreateOrderCommand
                    (
                        Guid.Parse(orderUpdateEventIntegration.BasketId),
                        orderUpdateEventIntegration.Address?.Street,
                        orderUpdateEventIntegration.Volume
                    );

                    var commandResult = await _mediator.Send(orderUpdateCommand, stoppingToken);

                    try
                    {
                        _consumer.StoreOffset(consumeResult);
                    }
                    catch (KafkaException exception)
                    {
                        Console.WriteLine($"Kafka exception: {exception.Message}");
                    }
                }
            }
            catch (OperationCanceledException exception)
            {
                Console.WriteLine($"Operation canceled exception: {exception.Message}");
            }

            return;
        }
    }
}