namespace NotificationService.Repository.Models
{
    public class InfoTable
    {
        public long IDInfo { get; set; }

        public long IDUser { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; }
    }
}
