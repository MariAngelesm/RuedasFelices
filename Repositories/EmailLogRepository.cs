using RuedasFelices.Models;
using RuedasFelices.Repositories.Interfaces;

namespace RuedasFelices.Repositories
{
    public class EmailLogRepository : IEmailLogRepository
    {
        private readonly List<EmailLog> _emailLogs;
        private int _nextId;

        public EmailLogRepository()
        {
            _emailLogs = new List<EmailLog>();
            _nextId = 1;
        }

        public void Add(EmailLog emailLog)
        {
            emailLog.Id = _nextId++;
            _emailLogs.Add(emailLog);
        }

        public List<EmailLog> GetAll()
        {
            return _emailLogs.ToList();
        }

        public List<EmailLog> GetByAppointmentId(int appointmentId)
        {
            return _emailLogs.Where(e => e.AppointmentId == appointmentId).ToList();
        }
    }
}