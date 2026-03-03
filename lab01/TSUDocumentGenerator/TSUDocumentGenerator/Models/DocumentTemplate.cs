namespace UniversityReports.Models
{
    public class DocumentTemplate : IDocumentPrototype
    {
        public string TemplateName { get; set; }
        public string TemplateFilePath { get; set; }

        // Встроенные данные шаблона
        public UserData Data { get; set; } = new UserData();

        // Глубокое клонирование: копируем и данные
        public IDocumentPrototype Clone()
        {
            var clone = (DocumentTemplate)this.MemberwiseClone();
            clone.Data = this.Data?.Copy(); // глубокое копирование UserData
            return clone;
        }
    }
}