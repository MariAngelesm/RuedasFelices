using RuedasFelices.Models;
using RuedasFelices.Repositories.Interfaces;

namespace RuedasFelices.Controllers
{
    public class InspectorController
    {
        private readonly IInspectorRepository _inspectorRepository;
        private readonly ICustomerRepository _customerRepository;

        public InspectorController(
            IInspectorRepository inspectorRepository,
            ICustomerRepository customerRepository)
        {
            _inspectorRepository = inspectorRepository;
            _customerRepository = customerRepository;
        }

        public void RegisterInspector()
        {
            try
            {
                Console.WriteLine("\n=== REGISTER NEW INSPECTOR ===");
                
                Console.Write("Name: ");
                string name = Console.ReadLine();
                
                Console.Write("Document ID: ");
                string documentId = Console.ReadLine();

                if (_inspectorRepository.ExistsByDocumentId(documentId))
                {
                    Console.WriteLine("\n❌ Error: An inspector with this document already exists.");
                    return;
                }

                if (_customerRepository.ExistsByDocumentId(documentId))
                {
                    Console.WriteLine("\n❌ Error: This document is already registered as a customer.");
                    return;
                }

                Console.WriteLine("\nInspection Types:");
                Console.WriteLine("1. Motorcycle");
                Console.WriteLine("2. Light Vehicle");
                Console.WriteLine("3. Heavy Vehicle");
                Console.Write("Select type: ");
                int typeOption = int.Parse(Console.ReadLine());

                InspectionType type = typeOption switch
                {
                    1 => InspectionType.Motorcycle,
                    2 => InspectionType.LightVehicle,
                    3 => InspectionType.HeavyVehicle,
                    _ => throw new Exception("Invalid inspection type")
                };

                Console.Write("Phone: ");
                string phone = Console.ReadLine();
                
                Console.Write("Email: ");
                string email = Console.ReadLine();

                var inspector = new Inspector
                {
                    Name = name,
                    DocumentId = documentId,
                    InspectionType = type,
                    Phone = phone,
                    Email = email
                };

                _inspectorRepository.Add(inspector);
                Console.WriteLine("\n✅ Inspector registered successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error: {ex.Message}");
            }
        }

        public void EditInspector()
        {
            try
            {
                Console.WriteLine("\n=== EDIT INSPECTOR ===");
                ListAllInspectors();
                
                Console.Write("\nEnter Inspector ID to edit: ");
                int id = int.Parse(Console.ReadLine());

                var inspector = _inspectorRepository.GetById(id);
                if (inspector == null)
                {
                    Console.WriteLine("\n❌ Inspector not found.");
                    return;
                }

                Console.WriteLine($"\nEditing: {inspector.Name}");
                
                Console.Write("New Name (leave blank to keep current): ");
                string name = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(name)) inspector.Name = name;

                Console.Write("Change inspection type? (Y/N): ");
                if (Console.ReadLine()?.ToUpper() == "Y")
                {
                    Console.WriteLine("\nInspection Types:");
                    Console.WriteLine("1. Motorcycle");
                    Console.WriteLine("2. Light Vehicle");
                    Console.WriteLine("3. Heavy Vehicle");
                    Console.Write("Select type: ");
                    int typeOption = int.Parse(Console.ReadLine());

                    inspector.InspectionType = typeOption switch
                    {
                        1 => InspectionType.Motorcycle,
                        2 => InspectionType.LightVehicle,
                        3 => InspectionType.HeavyVehicle,
                        _ => inspector.InspectionType
                    };
                }

                Console.Write("New Phone (leave blank to keep current): ");
                string phone = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(phone)) inspector.Phone = phone;

                Console.Write("New Email (leave blank to keep current): ");
                string email = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(email)) inspector.Email = email;

                _inspectorRepository.Update(inspector);
                Console.WriteLine("\n✅ Inspector updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error: {ex.Message}");
            }
        }

        public void ListAllInspectors()
        {
            var inspectors = _inspectorRepository.GetAll();
            
            if (inspectors.Count == 0)
            {
                Console.WriteLine("\nNo inspectors registered.");
                return;
            }

            Console.WriteLine("\n=== INSPECTOR LIST ===");
            foreach (var inspector in inspectors)
            {
                Console.WriteLine(inspector);
            }
        }

        public void ListInspectorsByType()
        {
            try
            {
                Console.WriteLine("\n=== FILTER BY INSPECTION TYPE ===");
                Console.WriteLine("1. Motorcycle");
                Console.WriteLine("2. Light Vehicle");
                Console.WriteLine("3. Heavy Vehicle");
                Console.Write("Select type: ");
                int typeOption = int.Parse(Console.ReadLine());

                InspectionType type = typeOption switch
                {
                    1 => InspectionType.Motorcycle,
                    2 => InspectionType.LightVehicle,
                    3 => InspectionType.HeavyVehicle,
                    _ => throw new Exception("Invalid inspection type")
                };

                var inspectors = _inspectorRepository.GetByInspectionType(type);
                
                if (inspectors.Count == 0)
                {
                    Console.WriteLine($"\nNo inspectors found for type: {type}");
                    return;
                }

                Console.WriteLine($"\n=== INSPECTORS - {type} ===");
                foreach (var inspector in inspectors)
                {
                    Console.WriteLine(inspector);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error: {ex.Message}");
            }
        }
    }
}