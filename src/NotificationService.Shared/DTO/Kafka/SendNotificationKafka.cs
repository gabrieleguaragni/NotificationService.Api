using NotificationService.Shared.Enums;

namespace NotificationService.Shared.DTO.Kafka
{
    public class SendNotificationKafka
    {
        public long IDUser { get; set; }

        public string Message { get; set; }

        public NotificationType Type { get; set; }
    }
}
