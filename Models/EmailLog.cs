namespace RuedasFelices.Models
{
    public class EmailLog
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public string RecipientEmail { get; set; }
        public string Subject { get; set; }
        public DateTime SentAt { get; set; }
        public bool WasSent { get; set; }
        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            string status = WasSent ? "Sent" : "Failed";
            return $"ID: {Id} | Appointment: {AppointmentId} | To: {RecipientEmail} | Status: {status} | Date: {SentAt:yyyy-MM-dd HH:mm}";
        }
    }
}