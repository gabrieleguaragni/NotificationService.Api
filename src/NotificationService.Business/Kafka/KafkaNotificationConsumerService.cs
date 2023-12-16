using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotificationService.Business.Abstractions.Services;
using NotificationService.Shared.DTO.Kafka;
using NotificationService.Shared.Enums;
using System.Text.Json;

namespace NotificationService.Business.Kafka
{
    public class KafkaNotificationConsumerService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IConsumer<string, string> _consumer;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<KafkaNotificationConsumerService> _logger;

        public KafkaNotificationConsumerService(IConfiguration configuration, IServiceProvider serviceProvider, ILogger<KafkaNotificationConsumerService> logger)
        {
            _configuration = configuration;
            _consumer = new ConsumerBuilder<string, string>(new ConsumerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],
                GroupId = _configuration["Kafka:SendNotification:GroupId"],
                AutoOffsetReset = AutoOffsetReset.Earliest
            }).Build();
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            using var scope = _serviceProvider.CreateScope();
            var _messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();

            var topic = _configuration["Kafka:SendNotification:Topic"];
            _consumer.Subscribe(topic);
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                SendNotificationKafka? sendNotificationKafka = JsonSerializer.Deserialize<SendNotificationKafka>(consumeResult.Message.Value, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                });

                if (sendNotificationKafka != null)
                {
                    try
                    {
                        switch (sendNotificationKafka.Type)
                        {
                            case NotificationType.Info:
                                await _messageService.SendInfoMessage(sendNotificationKafka.IDUser, sendNotificationKafka.Message);
                                break;
                            case NotificationType.Warning:
                                await _messageService.SendWarningMessage(sendNotificationKafka.IDUser, sendNotificationKafka.Message);
                                break;
                            case NotificationType.Critical:
                                await _messageService.SendCriticalMessage(sendNotificationKafka.IDUser, sendNotificationKafka.Message);
                                break;
                            default:
                                break;
                        }

                        _consumer.Commit(consumeResult);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Error occurred in KafkaNotificationConsumerService");
                    }
                }
            }
        }

        public override void Dispose()
        {
            _consumer.Dispose();
            base.Dispose();
        }
    }
}
