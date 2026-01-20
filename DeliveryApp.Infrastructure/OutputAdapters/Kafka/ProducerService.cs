using Confluent.Kafka;
using DeliveryApp.Core.Domain.DomainEvents;
using DeliveryApp.Core.Ports;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz.Logging;
using Queues.Order;

namespace DeliveryApp.Infrastructure.OutputAdapters.Kafka
{
    public sealed class ProducerService : IMessageBusProducer
    {
        private readonly ILogger<ProducerService> _logger;
        private readonly ProducerConfig _producerConfig;
        private readonly string _topicName;

        public ProducerService(ILogger<ProducerService> logger, IOptions<Settings> options) 
        {
            if (logger == null)
            { 
                throw new ArgumentNullException(nameof(logger));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _logger = logger;

            _producerConfig = new ProducerConfig
            {
                BootstrapServers = options.Value.MessageBrokerHost
            };

            _topicName = options.Value.OrderEventsTopic;
        }
        
        public async Task PublishOrderCreatedDomainEvent(OrderCreatedDomainEvent orderCreatedDomainEvent, CancellationToken cancellationToken)
        {
            if (orderCreatedDomainEvent == null)
            {
                throw new ArgumentNullException(nameof(OrderCompletedDomainEvent));
            }

            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogWarning($"[{PublishOrderCreatedDomainEvent}] Publish order created domain event canelled");
                return;
            }

            var orderCreatedIntegrationEvent = GetOrderCompletedDomainEventMapToOrderCompletedIntegrationEvent(orderCreatedDomainEvent);

            var message = new Message<string, string>
            {
                Key = orderCreatedIntegrationEvent.EventId,
                Value = JsonConvert.SerializeObject(orderCreatedIntegrationEvent)
            };

            try
            {
                using var producer = new ProducerBuilder<string, string>(_producerConfig).Build();
                var produceMessageResult = await producer.ProduceAsync(_topicName, message, cancellationToken);

                _logger.LogInformation($"[{PublishOrderCreatedDomainEvent}] Success delivered " +
                    $"'{produceMessageResult.Value}' to '{produceMessageResult.TopicPartitionOffset}'");
            }

            catch (KafkaException kafkaException) 
            {
                _logger.LogInformation($"[{PublishOrderCreatedDomainEvent}] Error delivered '{kafkaException.Error.Reason}'");
            }
        }

        public async Task PublishOrderCompletedDomainEvent(OrderCompletedDomainEvent orderCompletedDomainEvent, CancellationToken cancellationToken)
        {
            if (orderCompletedDomainEvent == null)
            {
                throw new ArgumentNullException(nameof(OrderCompletedDomainEvent));
            }

            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogWarning($"[{PublishOrderCompletedDomainEvent}] Publish order completed domain event canelled");
                return;
            }

            var orderCompletedIntegrationEvent = GetOrderCreatedDomainEventMapToOrderCreatedIntegrationEvent(orderCompletedDomainEvent);

            var message = new Message<string, string>
            {
                Key = orderCompletedIntegrationEvent.EventId,
                Value = JsonConvert.SerializeObject(orderCompletedIntegrationEvent)
            };

            try
            {
                using var producer = new ProducerBuilder<string, string>(_producerConfig).Build();
                var produceMessageResult = await producer.ProduceAsync(_topicName, message, cancellationToken);

                _logger.LogInformation($"[{PublishOrderCreatedDomainEvent}] Success delivered " +
                    $"'{produceMessageResult.Value}' to '{produceMessageResult.TopicPartitionOffset}'");
            }

            catch (KafkaException kafkaException)
            {
                _logger.LogInformation($"[{PublishOrderCreatedDomainEvent}] Error delivered '{kafkaException.Error.Reason}'");
            }
        }

        private OrderCreatedIntegrationEvent GetOrderCompletedDomainEventMapToOrderCompletedIntegrationEvent(OrderCreatedDomainEvent orderCompletedDomainEvent)
        {
            return new()
            {
                OrderId = orderCompletedDomainEvent.Order.Id.ToString()
            };
        }

        private OrderCompletedIntegrationEvent GetOrderCreatedDomainEventMapToOrderCreatedIntegrationEvent(OrderCompletedDomainEvent orderCreatedDomainEvent)
        {
            return new() 
            {
                OrderId = orderCreatedDomainEvent.Order.Id.ToString(),
                CourierId = orderCreatedDomainEvent.Order.CourierId.ToString()
            };
        }
    }
}