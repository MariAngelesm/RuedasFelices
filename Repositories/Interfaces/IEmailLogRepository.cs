using RuedasFelices.Models;

namespace RuedasFelices.Repositories.Interfaces
{
    public interface IEmailLogRepository
    {
        void Add(EmailLog emailLog);
        List<EmailLog> GetAll();
        List<EmailLog> GetByAppointmentId(int appointmentId);
    }
}