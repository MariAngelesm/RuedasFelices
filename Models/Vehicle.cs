namespace RuedasFelices.Models
{
    public enum VehicleType
    {
        Motorcycle,
        LightVehicle,
        HeavyVehicle
    }

    public class Vehicle
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public VehicleType Type { get; set; }
        public int CustomerId { get; set; }

        public override string ToString()
        {
            return $"Plate: {LicensePlate} | Brand: {Brand} | Model: {Model} | Year: {Year} | Type: {Type}";
        }
    }
}