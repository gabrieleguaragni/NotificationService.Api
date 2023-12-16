using NotificationService.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NotificationService.Shared.DTO.Request
{
    public class SendMessageRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public NotificationType Type { get; set; }

        [Required]
        public long IDUser { get; set; }

        public string Message { get; set; }
    }
}
