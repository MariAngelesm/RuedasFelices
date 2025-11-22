using RuedasFelices.Models;

namespace RuedasFelices.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        void Add(Appointment appointment);
        void Update(Appointment appointment);
        Appointment GetById(int id);
        List<Appointment> GetAll();
        List<Appointment> GetByCustomerId(int customerId);
        List<Appointment> GetByVehicleId(int vehicleId);
        List<Appointment> GetByInspectorId(int inspectorId);
        bool HasConflictForInspector(int inspectorId, DateTime date);
        bool HasConflictForVehicle(int vehicleId, DateTime date);
    }
}
