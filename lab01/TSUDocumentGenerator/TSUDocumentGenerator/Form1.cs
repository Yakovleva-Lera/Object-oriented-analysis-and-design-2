using System;
using System.IO;
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
        private UserData _data;

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
            { TemplateName = "Практика", TemplateFilePath = @"C:\Templates\Template_Practice.docx" });
            _registry.Register("Лабораторная", new DocumentTemplate
            { TemplateName = "Лабораторная", TemplateFilePath = @"C:\Templates\Template_Lab.docx" });
            _registry.Register("Курсовая", new DocumentTemplate
            { TemplateName = "Курсовая", TemplateFilePath = @"C:\Templates\Template_Coursework.docx" });
        }

        private void LoadDefaultData()
        {
            _data = new UserData();

            txtStudentName.Text = _data.StudentName;
            txtGroup.Text = _data.GroupNumber;
            txtDepartment.Text = _data.Department;
            txtDirectionCode.Text = _data.DirectionCode;
            txtDirectionName.Text = _data.DirectionName;
            txtProfile.Text = _data.Profile;
            txtSupervisorName.Text = _data.SupervisorName;
            txtSupervisorDegree.Text = _data.SupervisorDegree;
            txtOrgSupervisorName.Text = _data.OrgSupervisorName;
            txtOrgSupervisorInfo.Text = _data.OrgSupervisorInfo;
            txtPracticeView.Text = _data.PracticeView;
            txtPracticeType.Text = _data.PracticeType;
            txtDisciplineName.Text = _data.DisciplineName;
            txtLabWorkName.Text = _data.LabWorkName;
            txtTopicName.Text = _data.TopicName;
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSpecificFields();
        }

        private void UpdateSpecificFields()
        {
            var type = comboBoxType.SelectedItem?.ToString();

            txtPracticeView.Visible = txtPracticeType.Visible = false;
            txtDisciplineName.Visible = false;
            txtLabWorkName.Visible = false;
            txtTopicName.Visible = false;
            labelPracticeView.Visible = labelPracticeType.Visible = false;
            labelDisciplineName.Visible = false;
            labelLabWorkName.Visible = false;
            labelTopicName.Visible = false;

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
            // Обновляем данные из полей формы перед генерацией
            _data.StudentName = txtStudentName.Text;
            _data.GroupNumber = txtGroup.Text;
            _data.Department = txtDepartment.Text;
            _data.DirectionCode = txtDirectionCode.Text;
            _data.DirectionName = txtDirectionName.Text;
            _data.Profile = txtProfile.Text;
            _data.SupervisorName = txtSupervisorName.Text;
            _data.SupervisorDegree = txtSupervisorDegree.Text;
            _data.OrgSupervisorName = txtOrgSupervisorName.Text;
            _data.OrgSupervisorInfo = txtOrgSupervisorInfo.Text;
            _data.PracticeView = txtPracticeView.Text;
            _data.PracticeType = txtPracticeType.Text;
            _data.DisciplineName = txtDisciplineName.Text;
            _data.LabWorkName = txtLabWorkName.Text;
            _data.TopicName = txtTopicName.Text;

            var type = comboBoxType.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(type)) { MessageBox.Show("Выберите тип работы!"); return; }

            // === ПАТТЕРН ПРОТОТИП ===
            var template = (DocumentTemplate)_registry.CreateFromTemplate(type);

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Word Document|*.docx";
                saveDialog.Title = "Сохранить отчет";
                saveDialog.FileName = $"Отчет_{type}_{_data.StudentName}.docx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.Copy(template.GetTemplatePath(), saveDialog.FileName, true);
                        ReplacePlaceholdersInDocx(saveDialog.FileName);

                        txtResult.Text = $"✓ Документ сохранен:\n{saveDialog.FileName}";
                        MessageBox.Show($"Отчет создан!\n\n{saveDialog.FileName}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = saveDialog.FileName,
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}");
                    }
                }
            }
        }

        private void ReplacePlaceholdersInDocx(string filePath)
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, true))
            {
                var body = doc.MainDocumentPart.Document.Body;

                var replacements = new System.Collections.Generic.Dictionary<string, string>
                {
                    { "{{StudentName}}", _data.StudentName },
                    { "{{GroupNumber}}", _data.GroupNumber },
                    { "{{Department}}", _data.Department },
                    { "{{DirectionCode}}", _data.DirectionCode },
                    { "{{DirectionName}}", _data.DirectionName },
                    { "{{Profile}}", _data.Profile },
                    { "{{SupervisorName}}", _data.SupervisorName },
                    { "{{SupervisorDegree}}", _data.SupervisorDegree },
                    { "{{OrgSupervisorName}}", _data.OrgSupervisorName },
                    { "{{OrgSupervisorInfo}}", _data.OrgSupervisorInfo },
                    { "{{PracticeView}}", _data.PracticeView },
                    { "{{PracticeType}}", _data.PracticeType },
                    { "{{DisciplineName}}", _data.DisciplineName },
                    { "{{LabWorkName}}", _data.LabWorkName },
                    { "{{TopicName}}", _data.TopicName }
                };

                foreach (var text in body.Descendants<Text>().ToList())
                {
                    if (text.Text != null && text.Text.Contains("{{"))
                    {
                        foreach (var kvp in replacements)
                        {
                            if (text.Text.Contains(kvp.Key))
                            {
                                text.Text = text.Text.Replace(kvp.Key, kvp.Value);
                            }
                        }
                    }
                }

                doc.Save();
            }
        }
    }
}