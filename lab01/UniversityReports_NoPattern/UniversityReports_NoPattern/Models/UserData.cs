namespace UniversityReports.Models
{
    public class UserData
    {
        // Данные студента
        public string StudentName { get; set; } = "Яковлева Лерочка";
        public string GroupNumber { get; set; } = "932305";

        // Данные об обучении
        public string Department { get; set; } = "Институт прикладной математики и компьютерных наук";
        public string DirectionCode { get; set; } = "02.03.03";
        public string DirectionName { get; set; } = "Мат. обес.";
        public string Profile { get; set; } = "DevOps";

        // Руководитель от ТГУ
        public string SupervisorName { get; set; } = "Литовченко М.И.";
        public string SupervisorDegree { get; set; } = "к.т.н., доцент";

        // Руководитель от организации (для практики)
        public string OrgSupervisorName { get; set; } = "Салимзянов Р.Р.";
        public string OrgSupervisorInfo { get; set; } = "к.т.н., доцент";

        // Специфичные данные для разных типов работ
        public string PracticeView { get; set; } = "Суперская";
        public string PracticeType { get; set; } = "Крутецкая";
        public string DisciplineName { get; set; } = "Объектно-ориентированный анализ и проектирование";
        public string LabWorkName { get; set; } = "ПАТТЕРНЫ ПРОЕКТИРОВАНИЯ";
        public string TopicName { get; set; } = "Разработка приложения на C#";
    }
}