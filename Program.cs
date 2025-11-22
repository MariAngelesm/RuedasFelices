using DotNetEnv;
using RuedasFelices.Controllers;
using RuedasFelices.Repositories;
using RuedasFelices.Repositories.Interfaces;

namespace RuedasFelices
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize environment variables
            Env.Load();
            
            // Initialize repositories
            ICustomerRepository customerRepository = new CustomerRepository();
            IVehicleRepository vehicleRepository = new VehicleRepository();
            IInspectorRepository inspectorRepository = new InspectorRepository();
            IAppointmentRepository appointmentRepository = new AppointmentRepository();
            IEmailLogRepository emailLogRepository = new EmailLogRepository();

            // Initialize controllers
            var customerController = new CustomerController(customerRepository, vehicleRepository, inspectorRepository);
            var inspectorController = new InspectorController(inspectorRepository, customerRepository);
            var appointmentController = new AppointmentController(
                appointmentRepository,
                vehicleRepository,
                inspectorRepository,
                customerRepository,
                emailLogRepository
            );

            bool running = true;

            while (running)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("╔════════════════════════════════════════╗");
                    Console.WriteLine("║     RUEDAS FELICES - MAIN MENU        ║");
                    Console.WriteLine("║   Technical Inspection Center System   ║");
                    Console.WriteLine("╚════════════════════════════════════════╝");
                    Console.WriteLine("\n┌─ CUSTOMER MANAGEMENT");
                    Console.WriteLine("│  1. Register Customer");
                    Console.WriteLine("│  2. Edit Customer");
                    Console.WriteLine("│  3. List Customers");
                    Console.WriteLine("│  4. Register Vehicle");
                    Console.WriteLine("│");
                    Console.WriteLine("┌─ INSPECTOR MANAGEMENT");
                    Console.WriteLine("│  5. Register Inspector");
                    Console.WriteLine("│  6. Edit Inspector");
                    Console.WriteLine("│  7. List Inspectors");
                    Console.WriteLine("│  8. Filter Inspectors by Type");
                    Console.WriteLine("│");
                    Console.WriteLine("┌─ APPOINTMENT MANAGEMENT");
                    Console.WriteLine("│  9. Schedule Appointment");
                    Console.WriteLine("│ 10. Cancel Appointment");
                    Console.WriteLine("│ 11. Complete Appointment");
                    Console.WriteLine("│ 12. List All Appointments");
                    Console.WriteLine("│ 13. Appointments by Customer");
                    Console.WriteLine("│ 14. Appointments by Vehicle");
                    Console.WriteLine("│ 15. Appointments by Inspector");
                    Console.WriteLine("│");
                    Console.WriteLine("┌─ EMAIL HISTORY");
                    Console.WriteLine("│ 16. View Email History");
                    Console.WriteLine("│");
                    Console.WriteLine("└─ 0. Exit");
                    Console.WriteLine("\n════════════════════════════════════════");
                    Console.Write("Select an option: ");

                    string option = Console.ReadLine();

                    switch (option)
                    {
                        case "1":
                            customerController.RegisterCustomer();
                            break;
                        case "2":
                            customerController.EditCustomer();
                            break;
                        case "3":
                            customerController.ListAllCustomers();
                            break;
                        case "4":
                            customerController.RegisterVehicle();
                            break;
                        case "5":
                            inspectorController.RegisterInspector();
                            break;
                        case "6":
                            inspectorController.EditInspector();
                            break;
                        case "7":
                            inspectorController.ListAllInspectors();
                            break;
                        case "8":
                            inspectorController.ListInspectorsByType();
                            break;
                        case "9":
                            appointmentController.ScheduleAppointment();
                            break;
                        case "10":
                            appointmentController.CancelAppointment();
                            break;
                        case "11":
                            appointmentController.CompleteAppointment();
                            break;
                        case "12":
                            appointmentController.ListAllAppointments();
                            break;
                        case "13":
                            appointmentController.ListAppointmentsByCustomer();
                            break;
                        case "14":
                            appointmentController.ListAppointmentsByVehicle();
                            break;
                        case "15":
                            appointmentController.ListAppointmentsByInspector();
                            break;
                        case "16":
                            appointmentController.ViewEmailHistory();
                            break;
                        case "0":
                            running = false;
                            Console.WriteLine("\nThank you for using Ruedas Felices!");
                            break;
                        default:
                            Console.WriteLine("\n❌ Invalid option. Please try again.");
                            break;
                    }

                    if (running && option != "0")
                    {
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n❌ Unexpected error: {ex.Message}");
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }
    }

}
