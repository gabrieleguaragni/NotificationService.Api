namespace NotificationService.Shared.DTO.Response
{
    public class CriticalResponse
    {
        public long IDCritical { get; set; }

        public long IDUser { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; }
    }
}
