using RuedasFelices.Models;

namespace RuedasFelices.Repositories.Interfaces
{
    public interface IVehicleRepository
    {
        void Add(Vehicle vehicle);
        void Update(Vehicle vehicle);
        Vehicle GetById(int id);
        Vehicle GetByLicensePlate(string licensePlate);
        List<Vehicle> GetAll();
        List<Vehicle> GetByCustomerId(int customerId);
        bool ExistsByLicensePlate(string licensePlate);
    }
}