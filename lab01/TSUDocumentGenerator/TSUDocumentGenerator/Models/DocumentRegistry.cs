using System.Collections.Generic;

namespace UniversityReports.Models
{
    public class DocumentRegistry
    {
        private Dictionary<string, IDocumentPrototype> _prototypes = new();

        public void Register(string key, IDocumentPrototype p) => _prototypes[key] = p;

        public IDocumentPrototype CreateFromTemplate(string key)
        {
            if (_prototypes.TryGetValue(key, out var p)) return p.Clone();
            throw new KeyNotFoundException($"Шаблон {key} не найден");
        }
    }
}