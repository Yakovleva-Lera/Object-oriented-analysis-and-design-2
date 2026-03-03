using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using UniversityReports.Models;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace UniversityReports
{
    public partial class Form1 : Form
    {
        // Словарь с шаблонами (тип работы → шаблон)
        private readonly Dictionary<string, DocumentTemplate> _templates = new();

        // Текущий выбранный шаблон
        private DocumentTemplate _currentTemplate;

        public Form1()
        {
            InitializeComponent();
            InitializeTemplates();
            LoadDefaultData();
            if (comboBoxType.Items.Count > 0) comboBoxType.SelectedIndex = 0;
            UpdateSpecificFields();
        }

        /// <summary>
        /// Инициализация шаблонов документов
        /// </summary>
        private void InitializeTemplates()
        {
            _templates.Clear();

            _templates.Add("Практика", new DocumentTemplate("Практика", @"C:\Templates\Template_Practice.docx"));
            _templates.Add("Лабораторная", new DocumentTemplate("Лабораторная", @"C:\Templates\Template_Lab.docx"));
            _templates.Add("Курсовая", new DocumentTemplate("Курсовая", @"C:\Templates\Template_Coursework.docx"));
        }

        /// <summary>
        /// Загрузка данных по умолчанию из первого шаблона
        /// </summary>
        private void LoadDefaultData()
        {
            if (_templates.Count > 0)
            {
                _currentTemplate = _templates.Values.First();
                BindUIToData(_currentTemplate.Data);
            }
        }

        /// <summary>
        /// Заполнение полей формы данными из UserData
        /// </summary>
        private void BindUIToData(UserData data)
        {
            txtStudentName.Text = data.StudentName;
            txtGroup.Text = data.GroupNumber;
            txtDepartment.Text = data.Department;
            txtDirectionCode.Text = data.DirectionCode;
            txtDirectionName.Text = data.DirectionName;
            txtProfile.Text = data.Profile;
            txtSupervisorName.Text = data.SupervisorName;
            txtSupervisorDegree.Text = data.SupervisorDegree;
            txtOrgSupervisorName.Text = data.OrgSupervisorName;
            txtOrgSupervisorInfo.Text = data.OrgSupervisorInfo;
            txtPracticeView.Text = data.PracticeView;
            txtPracticeType.Text = data.PracticeType;
            txtDisciplineName.Text = data.DisciplineName;
            txtLabWorkName.Text = data.LabWorkName;
            txtTopicName.Text = data.TopicName;
        }

        /// <summary>
        /// Обновление данных шаблона из полей формы
        /// </summary>
        private void UpdateTemplateDataFromUI()
        {
            if (_currentTemplate?.Data == null) return;

            _currentTemplate.Data.StudentName = txtStudentName.Text;
            _currentTemplate.Data.GroupNumber = txtGroup.Text;
            _currentTemplate.Data.Department = txtDepartment.Text;
            _currentTemplate.Data.DirectionCode = txtDirectionCode.Text;
            _currentTemplate.Data.DirectionName = txtDirectionName.Text;
            _currentTemplate.Data.Profile = txtProfile.Text;
            _currentTemplate.Data.SupervisorName = txtSupervisorName.Text;
            _currentTemplate.Data.SupervisorDegree = txtSupervisorDegree.Text;
            _currentTemplate.Data.OrgSupervisorName = txtOrgSupervisorName.Text;
            _currentTemplate.Data.OrgSupervisorInfo = txtOrgSupervisorInfo.Text;
            _currentTemplate.Data.PracticeView = txtPracticeView.Text;
            _currentTemplate.Data.PracticeType = txtPracticeType.Text;
            _currentTemplate.Data.DisciplineName = txtDisciplineName.Text;
            _currentTemplate.Data.LabWorkName = txtLabWorkName.Text;
            _currentTemplate.Data.TopicName = txtTopicName.Text;
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSpecificFields();

            // Переключаем текущий шаблон при смене типа
            var type = comboBoxType.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(type) && _templates.ContainsKey(type))
            {
                _currentTemplate = _templates[type];
                BindUIToData(_currentTemplate.Data);
            }
        }

        /// <summary>
        /// Обновление видимости полей в зависимости от типа работы
        /// </summary>
        private void UpdateSpecificFields()
        {
            var type = comboBoxType.SelectedItem?.ToString();

            // Скрываем все специфичные поля
            txtPracticeView.Visible = false;
            txtPracticeType.Visible = false;
            txtDisciplineName.Visible = false;
            txtLabWorkName.Visible = false;
            txtTopicName.Visible = false;

            labelPracticeView.Visible = false;
            labelPracticeType.Visible = false;
            labelDisciplineName.Visible = false;
            labelLabWorkName.Visible = false;
            labelTopicName.Visible = false;

            // Показываем нужные поля в зависимости от типа
            if (type == "Практика")
            {
                txtPracticeView.Visible = true;
                txtPracticeType.Visible = true;
                labelPracticeView.Visible = true;
                labelPracticeType.Visible = true;
            }
            else if (type == "Лабораторная")
            {
                txtDisciplineName.Visible = true;
                txtLabWorkName.Visible = true;
                labelDisciplineName.Visible = true;
                labelLabWorkName.Visible = true;
            }
            else if (type == "Курсовая")
            {
                txtTopicName.Visible = true;
                labelTopicName.Visible = true;
            }
        }

        /// <summary>
        /// Обработчик кнопки генерации документа
        /// </summary>
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            var type = comboBoxType.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(type))
            {
                MessageBox.Show("Выберите тип работы!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!_templates.ContainsKey(type))
            {
                MessageBox.Show($"Шаблон для типа \"{type}\" не настроен!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _currentTemplate = _templates[type];

            // Проверяем существование файла шаблона
            if (!File.Exists(_currentTemplate.TemplateFilePath))
            {
                MessageBox.Show($"Файл шаблона не найден:\n{_currentTemplate.TemplateFilePath}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Обновляем данные из полей формы
            UpdateTemplateDataFromUI();

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Word Document|*.docx";
                saveDialog.Title = "Сохранить отчет";
                saveDialog.FileName = $"Отчет_{type}_{_currentTemplate.Data.StudentName}.docx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Копируем файл шаблона
                        File.Copy(_currentTemplate.TemplateFilePath, saveDialog.FileName, true);

                        // Заменяем плейсхолдеры в скопированном документе
                        ReplacePlaceholdersInDocx(saveDialog.FileName);

                        txtResult.Text = $"✓ Документ сохранен:\n{saveDialog.FileName}";

                        MessageBox.Show($"Отчет создан!\n\n{saveDialog.FileName}", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Открываем созданный файл
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = saveDialog.FileName,
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при создании документа: {ex.Message}",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Замена плейсхолдеров в документе Word
        /// </summary>
        private void ReplacePlaceholdersInDocx(string filePath)
        {
            if (_currentTemplate?.Data == null) return;

            using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, true))
            {
                var body = doc.MainDocumentPart?.Document?.Body;
                if (body == null) return;

                var replacements = new Dictionary<string, string>
                {
                    { "{{StudentName}}", _currentTemplate.Data.StudentName },
                    { "{{GroupNumber}}", _currentTemplate.Data.GroupNumber },
                    { "{{Department}}", _currentTemplate.Data.Department },
                    { "{{DirectionCode}}", _currentTemplate.Data.DirectionCode },
                    { "{{DirectionName}}", _currentTemplate.Data.DirectionName },
                    { "{{Profile}}", _currentTemplate.Data.Profile },
                    { "{{SupervisorName}}", _currentTemplate.Data.SupervisorName },
                    { "{{SupervisorDegree}}", _currentTemplate.Data.SupervisorDegree },
                    { "{{OrgSupervisorName}}", _currentTemplate.Data.OrgSupervisorName },
                    { "{{OrgSupervisorInfo}}", _currentTemplate.Data.OrgSupervisorInfo },
                    { "{{PracticeView}}", _currentTemplate.Data.PracticeView },
                    { "{{PracticeType}}", _currentTemplate.Data.PracticeType },
                    { "{{DisciplineName}}", _currentTemplate.Data.DisciplineName },
                    { "{{LabWorkName}}", _currentTemplate.Data.LabWorkName },
                    { "{{TopicName}}", _currentTemplate.Data.TopicName }
                };

                // Заменяем плейсхолдеры во всех текстовых элементах документа
                foreach (var text in body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>().ToList())
                {
                    if (string.IsNullOrEmpty(text.Text) || !text.Text.Contains("{{"))
                        continue;

                    foreach (var kvp in replacements)
                    {
                        if (text.Text.Contains(kvp.Key))
                        {
                            text.Text = text.Text.Replace(kvp.Key, kvp.Value);
                        }
                    }
                }

                doc.Save();
            }
        }
    }
}