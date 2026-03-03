using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using UniversityReports.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace UniversityReports
{
    public partial class Form1 : Form
    {
        private DocumentRegistry _registry;
        private DocumentTemplate _currentTemplate; // Храним текущий клон шаблона с данными

        public Form1()
        {
            InitializeComponent();
            InitializeRegistry();
            LoadDefaultData();
            if (comboBoxType.Items.Count > 0) comboBoxType.SelectedIndex = 0;
            UpdateSpecificFields();
        }

        private void InitializeRegistry()
        {
            _registry = new DocumentRegistry();

            _registry.Register("Практика", new DocumentTemplate
            {
                TemplateName = "Практика",
                TemplateFilePath = @"C:\Templates\Template_Practice.docx",
                Data = new UserData()
            });

            _registry.Register("Лабораторная", new DocumentTemplate
            {
                TemplateName = "Лабораторная",
                TemplateFilePath = @"C:\Templates\Template_Lab.docx",
                Data = new UserData()
            });

            _registry.Register("Курсовая", new DocumentTemplate
            {
                TemplateName = "Курсовая",
                TemplateFilePath = @"C:\Templates\Template_Coursework.docx",
                Data = new UserData()
            });
        }

        private void LoadDefaultData()
        {
            // Загружаем дефолтные данные из шаблона "Практика" для инициализации формы
            var defaultTemplate = (DocumentTemplate)_registry.CreateFromTemplate("Практика");
            BindUIToData(defaultTemplate.Data);
        }

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

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSpecificFields();

            // Опционально: подгружать дефолтные данные выбранного типа шаблона
            // var type = comboBoxType.SelectedItem?.ToString();
            // if (!string.IsNullOrEmpty(type))
            // {
            //     var template = (DocumentTemplate)_registry.CreateFromTemplate(type);
            //     BindUIToData(template.Data);
            // }
        }

        private void UpdateSpecificFields()
        {
            var type = comboBoxType.SelectedItem?.ToString();

            // Скрываем все специфичные поля
            txtPracticeView.Visible = txtPracticeType.Visible = false;
            txtDisciplineName.Visible = false;
            txtLabWorkName.Visible = false;
            txtTopicName.Visible = false;
            labelPracticeView.Visible = labelPracticeType.Visible = false;
            labelDisciplineName.Visible = false;
            labelLabWorkName.Visible = false;
            labelTopicName.Visible = false;

            // Показываем нужные поля в зависимости от типа
            if (type == "Практика")
            {
                txtPracticeView.Visible = txtPracticeType.Visible = true;
                labelPracticeView.Visible = labelPracticeType.Visible = true;
            }
            else if (type == "Лабораторная")
            {
                txtDisciplineName.Visible = true;
                txtLabWorkName.Visible = true;
                labelDisciplineName.Visible = labelLabWorkName.Visible = true;
            }
            else if (type == "Курсовая")
            {
                txtTopicName.Visible = true;
                labelTopicName.Visible = true;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            var type = comboBoxType.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(type))
            {
                MessageBox.Show("Выберите тип работы!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // === ПАТТЕРН ПРОТОТИП: создаём независимый клон шаблона ===
            _currentTemplate = (DocumentTemplate)_registry.CreateFromTemplate(type);

            // Синхронизируем данные из UI в клон шаблона
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
                    catch (FileNotFoundException)
                    {
                        MessageBox.Show($"Файл шаблона не найден:\n{_currentTemplate.TemplateFilePath}",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при создании документа: {ex.Message}",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

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