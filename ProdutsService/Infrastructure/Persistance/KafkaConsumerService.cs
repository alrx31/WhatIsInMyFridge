using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistance
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly IConsumer<Null, string> _consumer;
        private readonly ILogger<KafkaConsumerService> _logger;

        public KafkaConsumerService(ILogger<KafkaConsumerService> logger)
        {
            _logger = logger;
            var config = new ConsumerConfig
            {
                GroupId = "test",
                BootstrapServers = "localhost:9093",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true // Автоматическое подтверждение offset
            };

            _consumer = new ConsumerBuilder<Null, string>(config).Build();
            _consumer.Subscribe("test");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Kafka consumer started");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = _consumer.Consume(stoppingToken);

                        if (consumeResult != null)
                        {
                            // Обработка сообщения
                            _logger.LogInformation($"Received message: {consumeResult.Message.Value}");
                        }
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError($"Kafka consume error: {ex.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Kafka consumer stopping...");
            }
            finally
            {
                // Корректно завершить работу Consumer
                _consumer.Close();
            }
        }

        public override void Dispose()
        {
            _consumer.Dispose();
            base.Dispose();
        }
    }
}
