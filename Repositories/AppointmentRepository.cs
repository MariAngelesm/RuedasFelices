using RuedasFelices.Models;
using RuedasFelices.Repositories.Interfaces;

namespace RuedasFelices.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly List<Appointment> _appointments;
        private int _nextId;

        public AppointmentRepository()
        {
            _appointments = new List<Appointment>();
            _nextId = 1;
        }

        public void Add(Appointment appointment)
        {
            appointment.Id = _nextId++;
            _appointments.Add(appointment);
        }

        public void Update(Appointment appointment)
        {
            var existing = GetById(appointment.Id);
            if (existing != null)
            {
                existing.Status = appointment.Status;
                existing.AppointmentDate = appointment.AppointmentDate;
            }
        }

        public Appointment GetById(int id)
        {
            return _appointments.FirstOrDefault(a => a.Id == id);
        }

        public List<Appointment> GetAll()
        {
            return _appointments.ToList();
        }

        public List<Appointment> GetByCustomerId(int customerId)
        {
            return _appointments.Where(a => a.VehicleId == customerId).ToList();
        }

        public List<Appointment> GetByVehicleId(int vehicleId)
        {
            return _appointments.Where(a => a.VehicleId == vehicleId).ToList();
        }

        public List<Appointment> GetByInspectorId(int inspectorId)
        {
            return _appointments.Where(a => a.InspectorId == inspectorId).ToList();
        }

        public bool HasConflictForInspector(int inspectorId, DateTime date)
        {
            return _appointments.Any(a => 
                a.InspectorId == inspectorId && 
                a.AppointmentDate == date && 
                a.Status == AppointmentStatus.Scheduled);
        }

        public bool HasConflictForVehicle(int vehicleId, DateTime date)
        {
            return _appointments.Any(a => 
                a.VehicleId == vehicleId && 
                a.AppointmentDate == date && 
                a.Status == AppointmentStatus.Scheduled);
        }
    }
}