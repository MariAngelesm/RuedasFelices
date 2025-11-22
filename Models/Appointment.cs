namespace RuedasFelices.Models
{
    public enum AppointmentStatus
    {
        Scheduled,
        Completed,
        Cancelled
    }

    public class Appointment
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public int InspectorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public AppointmentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public Appointment()
        {
            Status = AppointmentStatus.Scheduled;
            CreatedAt = DateTime.Now;
        }

        public override string ToString()
        {
            return $"ID: {Id} | Vehicle: {VehicleId} | Inspector: {InspectorId} | Date: {AppointmentDate:yyyy-MM-dd HH:mm} | Status: {Status}";
        }
    }
}