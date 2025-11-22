namespace RuedasFelices.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DocumentId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public List<Vehicle> Vehicles { get; set; }

        public Customer()
        {
            Vehicles = new List<Vehicle>();
        }

        public override string ToString()
        {
            return $"ID: {Id} | Name: {Name} | Document: {DocumentId} | Phone: {Phone} | Email: {Email}";
        }
    }
}