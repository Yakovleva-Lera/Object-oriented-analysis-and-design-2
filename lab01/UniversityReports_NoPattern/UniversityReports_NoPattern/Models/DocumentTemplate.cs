namespace UniversityReports.Models
{
    public class DocumentTemplate
    {
        // Название шаблона
        public string TemplateName { get; set; }

        // Путь к файлу шаблона
        public string TemplateFilePath { get; set; }

        // Данные для подстановки в документ
        public UserData Data { get; set; } = new UserData();

        // Конструктор по умолчанию
        public DocumentTemplate()
        {
            Data = new UserData();
        }

        // Конструктор с параметрами
        public DocumentTemplate(string name, string path)
        {
            TemplateName = name;
            TemplateFilePath = path;
            Data = new UserData();
        }
    }
}