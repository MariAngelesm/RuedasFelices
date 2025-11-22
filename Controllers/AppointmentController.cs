using RuedasFelices.Models;
using RuedasFelices.Repositories.Interfaces;
using RuedasFelices.Services;

namespace RuedasFelices.Controllers
{
    public class AppointmentController
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IInspectorRepository _inspectorRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmailLogRepository _emailLogRepository;
        private readonly EmailService _emailService;

        public AppointmentController(
            IAppointmentRepository appointmentRepository,
            IVehicleRepository vehicleRepository,
            IInspectorRepository inspectorRepository,
            ICustomerRepository customerRepository,
            IEmailLogRepository emailLogRepository)
        {
            _appointmentRepository = appointmentRepository;
            _vehicleRepository = vehicleRepository;
            _inspectorRepository = inspectorRepository;
            _customerRepository = customerRepository;
            _emailLogRepository = emailLogRepository;
            _emailService = new EmailService(emailLogRepository);
        }

        public void ScheduleAppointment()
        {
            try
            {
                Console.WriteLine("\n=== SCHEDULE NEW APPOINTMENT ===");
                
                // Select Vehicle
                var vehicles = _vehicleRepository.GetAll();
                if (vehicles.Count == 0)
                {
                    Console.WriteLine("\n❌ No vehicles registered. Please register a vehicle first.");
                    return;
                }

                Console.WriteLine("\nAvailable Vehicles:");
                foreach (var v in vehicles)
                {
                    Console.WriteLine($"{v.Id}. {v}");
                }
                
                Console.Write("\nSelect Vehicle ID: ");
                int vehicleId = int.Parse(Console.ReadLine());
                var vehicle = _vehicleRepository.GetById(vehicleId);
                
                if (vehicle == null)
                {
                    Console.WriteLine("\n❌ Vehicle not found.");
                    return;
                }

                // Get compatible inspectors
                InspectionType requiredType = vehicle.Type switch
                {
                    VehicleType.Motorcycle => InspectionType.Motorcycle,
                    VehicleType.LightVehicle => InspectionType.LightVehicle,
                    VehicleType.HeavyVehicle => InspectionType.HeavyVehicle,
                    _ => throw new Exception("Invalid vehicle type")
                };

                var compatibleInspectors = _inspectorRepository.GetByInspectionType(requiredType);
                
                if (compatibleInspectors.Count == 0)
                {
                    Console.WriteLine($"\n❌ No inspectors available for {requiredType} inspections.");
                    return;
                }

                Console.WriteLine($"\nCompatible Inspectors for {vehicle.Type}:");
                foreach (var i in compatibleInspectors)
                {
                    Console.WriteLine($"{i.Id}. {i}");
                }

                Console.Write("\nSelect Inspector ID: ");
                int inspectorId = int.Parse(Console.ReadLine());
                var inspector = _inspectorRepository.GetById(inspectorId);
                
                if (inspector == null || inspector.InspectionType != requiredType)
                {
                    Console.WriteLine("\n❌ Invalid inspector or incompatible type.");
                    return;
                }

                // Select date and time with validation loop
                DateTime appointmentDate = default;
                bool validDate = false;
                
                while (!validDate)
                {
                    try
                    {
                        Console.WriteLine($"\n📅 Current date: {DateTime.Now:yyyy-MM-dd HH:mm}");
                        Console.Write("Enter date (yyyy-MM-dd): ");
                        string dateStr = Console.ReadLine();
                        
                        Console.Write("Enter time (HH:mm): ");
                        string timeStr = Console.ReadLine();
                        
                        appointmentDate = DateTime.Parse($"{dateStr} {timeStr}");

                        // Validate date is not in the past
                        if (appointmentDate <= DateTime.Now)
                        {
                            Console.WriteLine("\n⚠️  Warning: The appointment date must be in the future.");
                            Console.WriteLine("Please enter a valid date and time.");
                            continue;
                        }

                        // Validate conflicts for inspector
                        if (_appointmentRepository.HasConflictForInspector(inspectorId, appointmentDate))
                        {
                            Console.WriteLine("\n⚠️  Warning: Inspector already has an appointment at this time.");
                            Console.WriteLine($"Inspector: {inspector.Name}");
                            Console.WriteLine($"Conflicting time: {appointmentDate:yyyy-MM-dd HH:mm}");
                            Console.WriteLine("Please choose a different date/time.");
                            continue;
                        }

                        // Validate conflicts for vehicle
                        if (_appointmentRepository.HasConflictForVehicle(vehicleId, appointmentDate))
                        {
                            Console.WriteLine("\n⚠️  Warning: Vehicle already has an appointment at this time.");
                            Console.WriteLine($"Vehicle: {vehicle.LicensePlate}");
                            Console.WriteLine($"Conflicting time: {appointmentDate:yyyy-MM-dd HH:mm}");
                            Console.WriteLine("Please choose a different date/time.");
                            continue;
                        }

                        // If we reach here, the date is valid
                        validDate = true;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("\n⚠️  Warning: Invalid date/time format.");
                        Console.WriteLine("Please use the format: yyyy-MM-dd for date and HH:mm for time.");
                        Console.WriteLine("Example: 2025-12-25 and 14:30");
                    }
                }

                // Create appointment
                var appointment = new Appointment
                {
                    VehicleId = vehicleId,
                    InspectorId = inspectorId,
                    AppointmentDate = appointmentDate
                };

                _appointmentRepository.Add(appointment);
                Console.WriteLine("\n✅ Appointment scheduled successfully!");
                Console.WriteLine($"Appointment details: {appointmentDate:dddd, MMMM dd, yyyy - HH:mm}");

                // Send confirmation email
                var customer = _customerRepository.GetById(vehicle.CustomerId);
                if (customer != null)
                {
                    _emailService.SendAppointmentConfirmation(appointment, customer, vehicle, inspector);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error: {ex.Message}");
            }
        }

        public void CancelAppointment()
        {
            try
            {
                Console.WriteLine("\n=== CANCEL APPOINTMENT ===");
                ListAllAppointments();
                
                Console.Write("\nEnter Appointment ID to cancel: ");
                int id = int.Parse(Console.ReadLine());

                var appointment = _appointmentRepository.GetById(id);
                if (appointment == null)
                {
                    Console.WriteLine("\n❌ Appointment not found.");
                    return;
                }

                if (appointment.Status != AppointmentStatus.Scheduled)
                {
                    Console.WriteLine($"\n❌ Cannot cancel appointment with status: {appointment.Status}");
                    return;
                }

                appointment.Status = AppointmentStatus.Cancelled;
                _appointmentRepository.Update(appointment);
                Console.WriteLine("\n✅ Appointment cancelled successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error: {ex.Message}");
            }
        }

        public void CompleteAppointment()
        {
            try
            {
                Console.WriteLine("\n=== COMPLETE APPOINTMENT ===");
                ListAllAppointments();
                
                Console.Write("\nEnter Appointment ID to complete: ");
                int id = int.Parse(Console.ReadLine());

                var appointment = _appointmentRepository.GetById(id);
                if (appointment == null)
                {
                    Console.WriteLine("\n❌ Appointment not found.");
                    return;
                }

                if (appointment.Status != AppointmentStatus.Scheduled)
                {
                    Console.WriteLine($"\n❌ Cannot complete appointment with status: {appointment.Status}");
                    return;
                }

                appointment.Status = AppointmentStatus.Completed;
                _appointmentRepository.Update(appointment);
                Console.WriteLine("\n✅ Appointment completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error: {ex.Message}");
            }
        }

        public void ListAllAppointments()
        {
            var appointments = _appointmentRepository.GetAll();
            
            if (appointments.Count == 0)
            {
                Console.WriteLine("\nNo appointments scheduled.");
                return;
            }

            Console.WriteLine("\n=== APPOINTMENT LIST ===");
            foreach (var appointment in appointments)
            {
                var vehicle = _vehicleRepository.GetById(appointment.VehicleId);
                var inspector = _inspectorRepository.GetById(appointment.InspectorId);
                
                Console.WriteLine($"\n{appointment}");
                if (vehicle != null)
                    Console.WriteLine($"  Vehicle: {vehicle.LicensePlate} - {vehicle.Brand} {vehicle.Model}");
                if (inspector != null)
                    Console.WriteLine($"  Inspector: {inspector.Name}");
            }
        }

        public void ListAppointmentsByCustomer()
        {
            try
            {
                Console.Write("\nEnter Customer ID: ");
                int customerId = int.Parse(Console.ReadLine());

                var vehicles = _vehicleRepository.GetByCustomerId(customerId);
                var appointments = new List<Appointment>();

                foreach (var vehicle in vehicles)
                {
                    appointments.AddRange(_appointmentRepository.GetByVehicleId(vehicle.Id));
                }

                if (appointments.Count == 0)
                {
                    Console.WriteLine("\nNo appointments found for this customer.");
                    return;
                }

                Console.WriteLine("\n=== CUSTOMER APPOINTMENTS ===");
                foreach (var appointment in appointments)
                {
                    var vehicle = _vehicleRepository.GetById(appointment.VehicleId);
                    Console.WriteLine($"{appointment}");
                    Console.WriteLine($"  Vehicle: {vehicle?.LicensePlate}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error: {ex.Message}");
            }
        }

        public void ListAppointmentsByVehicle()
        {
            try
            {
                Console.Write("\nEnter Vehicle ID: ");
                int vehicleId = int.Parse(Console.ReadLine());

                var appointments = _appointmentRepository.GetByVehicleId(vehicleId);
                
                if (appointments.Count == 0)
                {
                    Console.WriteLine("\nNo appointments found for this vehicle.");
                    return;
                }

                Console.WriteLine("\n=== VEHICLE APPOINTMENTS ===");
                foreach (var appointment in appointments)
                {
                    Console.WriteLine(appointment);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error: {ex.Message}");
            }
        }

        public void ListAppointmentsByInspector()
        {
            try
            {
                Console.Write("\nEnter Inspector ID: ");
                int inspectorId = int.Parse(Console.ReadLine());

                var appointments = _appointmentRepository.GetByInspectorId(inspectorId);
                
                if (appointments.Count == 0)
                {
                    Console.WriteLine("\nNo appointments found for this inspector.");
                    return;
                }

                Console.WriteLine("\n=== INSPECTOR APPOINTMENTS ===");
                foreach (var appointment in appointments)
                {
                    Console.WriteLine(appointment);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error: {ex.Message}");
            }
        }

        public void ViewEmailHistory()
        {
            var logs = _emailLogRepository.GetAll();
            
            if (logs.Count == 0)
            {
                Console.WriteLine("\nNo email logs found.");
                return;
            }

            Console.WriteLine("\n=== EMAIL HISTORY ===");
            foreach (var log in logs)
            {
                Console.WriteLine(log);
                if (!string.IsNullOrEmpty(log.ErrorMessage))
                {
                    Console.WriteLine($"  Error: {log.ErrorMessage}");
                }
            }
        }
    }
}