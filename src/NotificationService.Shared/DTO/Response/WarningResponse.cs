namespace NotificationService.Shared.DTO.Response
{
    public class WarningResponse
    {
        public long IDWarning { get; set; }

        public long IDUser { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; }
    }
}
