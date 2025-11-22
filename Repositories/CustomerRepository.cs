using RuedasFelices.Models;
using RuedasFelices.Repositories.Interfaces;

namespace RuedasFelices.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly List<Customer> _customers;
        private int _nextId;

        public CustomerRepository()
        {
            _customers = new List<Customer>();
            _nextId = 1;
        }

        public void Add(Customer customer)
        {
            customer.Id = _nextId++;
            _customers.Add(customer);
        }

        public void Update(Customer customer)
        {
            var existing = GetById(customer.Id);
            if (existing != null)
            {
                existing.Name = customer.Name;
                existing.Phone = customer.Phone;
                existing.Email = customer.Email;
                existing.Address = customer.Address;
            }
        }

        public Customer GetById(int id)
        {
            return _customers.FirstOrDefault(c => c.Id == id);
        }

        public Customer GetByDocumentId(string documentId)
        {
            return _customers.FirstOrDefault(c => c.DocumentId == documentId);
        }

        public List<Customer> GetAll()
        {
            return _customers.ToList();
        }

        public bool ExistsByDocumentId(string documentId)
        {
            return _customers.Any(c => c.DocumentId == documentId);
        }
    }
}