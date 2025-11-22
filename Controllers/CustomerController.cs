using RuedasFelices.Models;
using RuedasFelices.Repositories.Interfaces;

namespace RuedasFelices.Controllers
{
    public class CustomerController
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IInspectorRepository _inspectorRepository;

        public CustomerController(
            ICustomerRepository customerRepository,
            IVehicleRepository vehicleRepository,
            IInspectorRepository inspectorRepository)
        {
            _customerRepository = customerRepository;
            _vehicleRepository = vehicleRepository;
            _inspectorRepository = inspectorRepository;
        }

        public void RegisterCustomer()
        {
            try
            {
                Console.WriteLine("\n=== REGISTER NEW CUSTOMER ===");
                
                Console.Write("Name: ");
                string name = Console.ReadLine();
                
                Console.Write("Document ID: ");
                string documentId = Console.ReadLine();

                if (_customerRepository.ExistsByDocumentId(documentId))
                {
                    Console.WriteLine("\n❌ Error: A customer with this document already exists.");
                    return;
                }

                Console.Write("Phone: ");
                string phone = Console.ReadLine();
                
                Console.Write("Email: ");
                string email = Console.ReadLine();
                
                Console.Write("Address: ");
                string address = Console.ReadLine();

                var customer = new Customer
                {
                    Name = name,
                    DocumentId = documentId,
                    Phone = phone,
                    Email = email,
                    Address = address
                };

                _customerRepository.Add(customer);
                Console.WriteLine("\n✅ Customer registered successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error: {ex.Message}");
            }
        }

        public void EditCustomer()
        {
            try
            {
                Console.WriteLine("\n=== EDIT CUSTOMER ===");
                ListAllCustomers();
                
                Console.Write("\nEnter Customer ID to edit: ");
                int id = int.Parse(Console.ReadLine());

                var customer = _customerRepository.GetById(id);
                if (customer == null)
                {
                    Console.WriteLine("\n❌ Customer not found.");
                    return;
                }

                Console.WriteLine($"\nEditing: {customer.Name}");
                Console.Write("New Name (leave blank to keep current): ");
                string name = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(name)) customer.Name = name;

                Console.Write("New Phone (leave blank to keep current): ");
                string phone = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(phone)) customer.Phone = phone;

                Console.Write("New Email (leave blank to keep current): ");
                string email = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(email)) customer.Email = email;

                Console.Write("New Address (leave blank to keep current): ");
                string address = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(address)) customer.Address = address;

                _customerRepository.Update(customer);
                Console.WriteLine("\n✅ Customer updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error: {ex.Message}");
            }
        }

        public void ListAllCustomers()
        {
            var customers = _customerRepository.GetAll();
            
            if (customers.Count == 0)
            {
                Console.WriteLine("\nNo customers registered.");
                return;
            }

            Console.WriteLine("\n=== CUSTOMER LIST ===");
            foreach (var customer in customers)
            {
                Console.WriteLine(customer);
                
                var vehicles = _vehicleRepository.GetByCustomerId(customer.Id);
                if (vehicles.Count > 0)
                {
                    Console.WriteLine($"  Vehicles: {vehicles.Count}");
                }
            }
        }

        public void RegisterVehicle()
        {
            try
            {
                Console.WriteLine("\n=== REGISTER NEW VEHICLE ===");
                ListAllCustomers();
                
                Console.Write("\nEnter Customer ID: ");
                int customerId = int.Parse(Console.ReadLine());

                var customer = _customerRepository.GetById(customerId);
                if (customer == null)
                {
                    Console.WriteLine("\n❌ Customer not found.");
                    return;
                }

                Console.Write("License Plate: ");
                string plate = Console.ReadLine();

                if (_vehicleRepository.ExistsByLicensePlate(plate))
                {
                    Console.WriteLine("\n❌ Error: A vehicle with this license plate already exists.");
                    return;
                }

                Console.Write("Brand: ");
                string brand = Console.ReadLine();
                
                Console.Write("Model: ");
                string model = Console.ReadLine();
                
                Console.Write("Year: ");
                int year = int.Parse(Console.ReadLine());

                Console.WriteLine("\nVehicle Types:");
                Console.WriteLine("1. Motorcycle");
                Console.WriteLine("2. Light Vehicle");
                Console.WriteLine("3. Heavy Vehicle");
                Console.Write("Select type: ");
                int typeOption = int.Parse(Console.ReadLine());

                VehicleType type = typeOption switch
                {
                    1 => VehicleType.Motorcycle,
                    2 => VehicleType.LightVehicle,
                    3 => VehicleType.HeavyVehicle,
                    _ => throw new Exception("Invalid vehicle type")
                };

                var vehicle = new Vehicle
                {
                    LicensePlate = plate,
                    Brand = brand,
                    Model = model,
                    Year = year,
                    Type = type,
                    CustomerId = customerId
                };

                _vehicleRepository.Add(vehicle);
                Console.WriteLine("\n✅ Vehicle registered successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error: {ex.Message}");
            }
        }
    }
}