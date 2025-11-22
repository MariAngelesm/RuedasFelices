using RuedasFelices.Models;

namespace RuedasFelices.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        void Add(Customer customer);
        void Update(Customer customer);
        Customer GetById(int id);
        Customer GetByDocumentId(string documentId);
        List<Customer> GetAll();
        bool ExistsByDocumentId(string documentId);
    }
}