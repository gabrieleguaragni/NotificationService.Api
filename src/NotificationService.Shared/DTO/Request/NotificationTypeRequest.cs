using NotificationService.Shared.Enums;
using System.Text.Json.Serialization;

namespace NotificationService.Shared.DTO.Request
{
    public class NotificationTypeRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public NotificationType Type { get; set; }
    }
}
