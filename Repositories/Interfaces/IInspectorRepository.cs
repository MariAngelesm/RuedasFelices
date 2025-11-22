using RuedasFelices.Models;

namespace RuedasFelices.Repositories.Interfaces
{
    public interface IInspectorRepository
    {
        void Add(Inspector inspector);
        void Update(Inspector inspector);
        Inspector GetById(int id);
        Inspector GetByDocumentId(string documentId);
        List<Inspector> GetAll();
        List<Inspector> GetByInspectionType(InspectionType type);
        bool ExistsByDocumentId(string documentId);
    }
}