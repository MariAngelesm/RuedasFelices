using RuedasFelices.Models;
using RuedasFelices.Repositories.Interfaces;

namespace RuedasFelices.Repositories
{
    public class InspectorRepository : IInspectorRepository
    {
        private readonly List<Inspector> _inspectors;
        private int _nextId;

        public InspectorRepository()
        {
            _inspectors = new List<Inspector>();
            _nextId = 1;
        }

        public void Add(Inspector inspector)
        {
            inspector.Id = _nextId++;
            _inspectors.Add(inspector);
        }

        public void Update(Inspector inspector)
        {
            var existing = GetById(inspector.Id);
            if (existing != null)
            {
                existing.Name = inspector.Name;
                existing.InspectionType = inspector.InspectionType;
                existing.Phone = inspector.Phone;
                existing.Email = inspector.Email;
            }
        }

        public Inspector GetById(int id)
        {
            return _inspectors.FirstOrDefault(i => i.Id == id);
        }

        public Inspector GetByDocumentId(string documentId)
        {
            return _inspectors.FirstOrDefault(i => i.DocumentId == documentId);
        }

        public List<Inspector> GetAll()
        {
            return _inspectors.ToList();
        }

        public List<Inspector> GetByInspectionType(InspectionType type)
        {
            return _inspectors.Where(i => i.InspectionType == type).ToList();
        }

        public bool ExistsByDocumentId(string documentId)
        {
            return _inspectors.Any(i => i.DocumentId == documentId);
        }
    }
}