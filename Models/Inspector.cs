namespace RuedasFelices.Models
{
    public enum InspectionType
    {
        Motorcycle,
        LightVehicle,
        HeavyVehicle
    }

    public class Inspector
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DocumentId { get; set; }
        public InspectionType InspectionType { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public override string ToString()
        {
            return $"ID: {Id} | Name: {Name} | Document: {DocumentId} | Type: {InspectionType} | Phone: {Phone}";
        }
    }
}