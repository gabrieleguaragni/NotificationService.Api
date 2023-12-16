namespace NotificationService.Repository.Models
{
    public class CriticalTable
    {
        public long IDCritical { get; set; }

        public long IDUser { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; }
    }
}
