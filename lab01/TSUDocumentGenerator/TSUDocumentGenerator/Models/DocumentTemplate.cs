namespace UniversityReports.Models
{
    public class DocumentTemplate : IDocumentPrototype
    {
        public string TemplateName { get; set; }
        public string TemplateFilePath { get; set; }

        public IDocumentPrototype Clone()
        {
            return (IDocumentPrototype)this.MemberwiseClone();
        }

        public string GetTemplateName() => TemplateName;
        public string GetTemplatePath() => TemplateFilePath;
    }
}