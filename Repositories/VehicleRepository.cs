using RuedasFelices.Models;
using RuedasFelices.Repositories.Interfaces;

namespace RuedasFelices.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly List<Vehicle> _vehicles;
        private int _nextId;

        public VehicleRepository()
        {
            _vehicles = new List<Vehicle>();
            _nextId = 1;
        }

        public void Add(Vehicle vehicle)
        {
            vehicle.Id = _nextId++;
            _vehicles.Add(vehicle);
        }

        public void Update(Vehicle vehicle)
        {
            var existing = GetById(vehicle.Id);
            if (existing != null)
            {
                existing.Brand = vehicle.Brand;
                existing.Model = vehicle.Model;
                existing.Year = vehicle.Year;
            }
        }

        public Vehicle GetById(int id)
        {
            return _vehicles.FirstOrDefault(v => v.Id == id);
        }

        public Vehicle GetByLicensePlate(string licensePlate)
        {
            return _vehicles.FirstOrDefault(v => v.LicensePlate == licensePlate);
        }

        public List<Vehicle> GetAll()
        {
            return _vehicles.ToList();
        }

        public List<Vehicle> GetByCustomerId(int customerId)
        {
            return _vehicles.Where(v => v.CustomerId == customerId).ToList();
        }

        public bool ExistsByLicensePlate(string licensePlate)
        {
            return _vehicles.Any(v => v.LicensePlate == licensePlate);
        }
    }
}