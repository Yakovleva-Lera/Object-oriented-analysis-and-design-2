namespace UniversityReports
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelType = new System.Windows.Forms.Label();
            this.comboBoxType = new System.Windows.Forms.ComboBox();

            this.groupBoxStudent = new System.Windows.Forms.GroupBox();
            this.txtStudentName = new System.Windows.Forms.TextBox();
            this.labelStudent = new System.Windows.Forms.Label();
            this.txtGroup = new System.Windows.Forms.TextBox();
            this.labelGroup = new System.Windows.Forms.Label();

            this.groupBoxEducation = new System.Windows.Forms.GroupBox();
            this.txtDepartment = new System.Windows.Forms.TextBox();
            this.labelDepartment = new System.Windows.Forms.Label();
            this.txtDirectionCode = new System.Windows.Forms.TextBox();
            this.labelDirectionCode = new System.Windows.Forms.Label();
            this.txtDirectionName = new System.Windows.Forms.TextBox();
            this.labelDirectionName = new System.Windows.Forms.Label();
            this.txtProfile = new System.Windows.Forms.TextBox();
            this.labelProfile = new System.Windows.Forms.Label();

            this.groupBoxSupervisor = new System.Windows.Forms.GroupBox();
            this.txtSupervisorName = new System.Windows.Forms.TextBox();
            this.labelSupervisorName = new System.Windows.Forms.Label();
            this.txtSupervisorDegree = new System.Windows.Forms.TextBox();
            this.labelSupervisorDegree = new System.Windows.Forms.Label();
            this.txtOrgSupervisorName = new System.Windows.Forms.TextBox();
            this.labelOrgSupervisorName = new System.Windows.Forms.Label();
            this.txtOrgSupervisorInfo = new System.Windows.Forms.TextBox();
            this.labelOrgSupervisorInfo = new System.Windows.Forms.Label();

            this.groupBoxSpecific = new System.Windows.Forms.GroupBox();
            this.txtPracticeView = new System.Windows.Forms.TextBox();
            this.labelPracticeView = new System.Windows.Forms.Label();
            this.txtPracticeType = new System.Windows.Forms.TextBox();
            this.labelPracticeType = new System.Windows.Forms.Label();
            this.txtDisciplineName = new System.Windows.Forms.TextBox();
            this.labelDisciplineName = new System.Windows.Forms.Label();
            this.txtLabWorkName = new System.Windows.Forms.TextBox();
            this.labelLabWorkName = new System.Windows.Forms.Label();
            this.txtTopicName = new System.Windows.Forms.TextBox();
            this.labelTopicName = new System.Windows.Forms.Label();

            this.btnGenerate = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.TextBox();

            this.groupBoxStudent.SuspendLayout();
            this.groupBoxEducation.SuspendLayout();
            this.groupBoxSupervisor.SuspendLayout();
            this.groupBoxSpecific.SuspendLayout();
            this.SuspendLayout();

            // labelTitle
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(12, 9);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(280, 25);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Генератор отчетов ТГУ";

            // labelType
            this.labelType.AutoSize = true;
            this.labelType.Location = new System.Drawing.Point(12, 45);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(63, 15);
            this.labelType.TabIndex = 1;
            this.labelType.Text = "Тип работы:";

            // comboBoxType
            this.comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Items.AddRange(new object[] { "Практика", "Лабораторная", "Курсовая" });
            this.comboBoxType.Location = new System.Drawing.Point(12, 63);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(560, 23);
            this.comboBoxType.TabIndex = 2;
            this.comboBoxType.SelectedIndexChanged += new System.EventHandler(this.comboBoxType_SelectedIndexChanged);

            // groupBoxStudent
            this.groupBoxStudent.Controls.Add(this.txtStudentName);
            this.groupBoxStudent.Controls.Add(this.labelStudent);
            this.groupBoxStudent.Controls.Add(this.txtGroup);
            this.groupBoxStudent.Controls.Add(this.labelGroup);
            this.groupBoxStudent.Location = new System.Drawing.Point(12, 92);
            this.groupBoxStudent.Name = "groupBoxStudent";
            this.groupBoxStudent.Size = new System.Drawing.Size(560, 80);
            this.groupBoxStudent.TabIndex = 3;
            this.groupBoxStudent.TabStop = false;
            this.groupBoxStudent.Text = "Данные студента";

            // labelStudent
            this.labelStudent.AutoSize = true;
            this.labelStudent.Location = new System.Drawing.Point(6, 20);
            this.labelStudent.Name = "labelStudent";
            this.labelStudent.Size = new System.Drawing.Size(86, 15);
            this.labelStudent.TabIndex = 0;
            this.labelStudent.Text = "ФИО студента:";

            // txtStudentName
            this.txtStudentName.Location = new System.Drawing.Point(6, 38);
            this.txtStudentName.Name = "txtStudentName";
            this.txtStudentName.Size = new System.Drawing.Size(270, 23);
            this.txtStudentName.TabIndex = 1;

            // labelGroup
            this.labelGroup.AutoSize = true;
            this.labelGroup.Location = new System.Drawing.Point(282, 20);
            this.labelGroup.Name = "labelGroup";
            this.labelGroup.Size = new System.Drawing.Size(48, 15);
            this.labelGroup.TabIndex = 2;
            this.labelGroup.Text = "Группа:";

            // txtGroup
            this.txtGroup.Location = new System.Drawing.Point(282, 38);
            this.txtGroup.Name = "txtGroup";
            this.txtGroup.Size = new System.Drawing.Size(270, 23);
            this.txtGroup.TabIndex = 3;

            // groupBoxEducation
            this.groupBoxEducation.Controls.Add(this.txtDepartment);
            this.groupBoxEducation.Controls.Add(this.labelDepartment);
            this.groupBoxEducation.Controls.Add(this.txtDirectionCode);
            this.groupBoxEducation.Controls.Add(this.labelDirectionCode);
            this.groupBoxEducation.Controls.Add(this.txtDirectionName);
            this.groupBoxEducation.Controls.Add(this.labelDirectionName);
            this.groupBoxEducation.Controls.Add(this.txtProfile);
            this.groupBoxEducation.Controls.Add(this.labelProfile);
            this.groupBoxEducation.Location = new System.Drawing.Point(12, 178);
            this.groupBoxEducation.Name = "groupBoxEducation";
            this.groupBoxEducation.Size = new System.Drawing.Size(560, 110);
            this.groupBoxEducation.TabIndex = 4;
            this.groupBoxEducation.TabStop = false;
            this.groupBoxEducation.Text = "Образовательная программа";

            // labelDepartment
            this.labelDepartment.AutoSize = true;
            this.labelDepartment.Location = new System.Drawing.Point(6, 20);
            this.labelDepartment.Name = "labelDepartment";
            this.labelDepartment.Size = new System.Drawing.Size(88, 15);
            this.labelDepartment.TabIndex = 0;
            this.labelDepartment.Text = "Подразделение:";

            // txtDepartment
            this.txtDepartment.Location = new System.Drawing.Point(6, 38);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.Size = new System.Drawing.Size(548, 23);
            this.txtDepartment.TabIndex = 1;

            // labelDirectionCode
            this.labelDirectionCode.AutoSize = true;
            this.labelDirectionCode.Location = new System.Drawing.Point(6, 65);
            this.labelDirectionCode.Name = "labelDirectionCode";
            this.labelDirectionCode.Size = new System.Drawing.Size(118, 15);
            this.labelDirectionCode.TabIndex = 2;
            this.labelDirectionCode.Text = "Код направления:";

            // txtDirectionCode
            this.txtDirectionCode.Location = new System.Drawing.Point(6, 83);
            this.txtDirectionCode.Name = "txtDirectionCode";
            this.txtDirectionCode.Size = new System.Drawing.Size(120, 23);
            this.txtDirectionCode.TabIndex = 3;

            // labelDirectionName
            this.labelDirectionName.AutoSize = true;
            this.labelDirectionName.Location = new System.Drawing.Point(132, 65);
            this.labelDirectionName.Name = "labelDirectionName";
            this.labelDirectionName.Size = new System.Drawing.Size(147, 15);
            this.labelDirectionName.TabIndex = 4;
            this.labelDirectionName.Text = "Наименование направления:";

            // txtDirectionName
            this.txtDirectionName.Location = new System.Drawing.Point(132, 83);
            this.txtDirectionName.Name = "txtDirectionName";
            this.txtDirectionName.Size = new System.Drawing.Size(422, 23);
            this.txtDirectionName.TabIndex = 5;

            // labelProfile
            this.labelProfile.AutoSize = true;
            this.labelProfile.Location = new System.Drawing.Point(6, 112);
            this.labelProfile.Name = "labelProfile";
            this.labelProfile.Size = new System.Drawing.Size(49, 15);
            this.labelProfile.TabIndex = 6;
            this.labelProfile.Text = "Профиль:";

            // txtProfile
            this.txtProfile.Location = new System.Drawing.Point(6, 130);
            this.txtProfile.Name = "txtProfile";
            this.txtProfile.Size = new System.Drawing.Size(548, 23);
            this.txtProfile.TabIndex = 7;

            // groupBoxSupervisor
            this.groupBoxSupervisor.Controls.Add(this.txtSupervisorName);
            this.groupBoxSupervisor.Controls.Add(this.labelSupervisorName);
            this.groupBoxSupervisor.Controls.Add(this.txtSupervisorDegree);
            this.groupBoxSupervisor.Controls.Add(this.labelSupervisorDegree);
            this.groupBoxSupervisor.Controls.Add(this.txtOrgSupervisorName);
            this.groupBoxSupervisor.Controls.Add(this.labelOrgSupervisorName);
            this.groupBoxSupervisor.Controls.Add(this.txtOrgSupervisorInfo);
            this.groupBoxSupervisor.Controls.Add(this.labelOrgSupervisorInfo);
            this.groupBoxSupervisor.Location = new System.Drawing.Point(12, 294);
            this.groupBoxSupervisor.Name = "groupBoxSupervisor";
            this.groupBoxSupervisor.Size = new System.Drawing.Size(560, 110);
            this.groupBoxSupervisor.TabIndex = 5;
            this.groupBoxSupervisor.TabStop = false;
            this.groupBoxSupervisor.Text = "Руководители";

            // labelSupervisorName
            this.labelSupervisorName.AutoSize = true;
            this.labelSupervisorName.Location = new System.Drawing.Point(6, 20);
            this.labelSupervisorName.Name = "labelSupervisorName";
            this.labelSupervisorName.Size = new System.Drawing.Size(168, 15);
            this.labelSupervisorName.TabIndex = 0;
            this.labelSupervisorName.Text = "Руководитель от ТГУ (ФИО):";

            // txtSupervisorName
            this.txtSupervisorName.Location = new System.Drawing.Point(6, 38);
            this.txtSupervisorName.Name = "txtSupervisorName";
            this.txtSupervisorName.Size = new System.Drawing.Size(270, 23);
            this.txtSupervisorName.TabIndex = 1;

            // labelSupervisorDegree
            this.labelSupervisorDegree.AutoSize = true;
            this.labelSupervisorDegree.Location = new System.Drawing.Point(282, 20);
            this.labelSupervisorDegree.Name = "labelSupervisorDegree";
            this.labelSupervisorDegree.Size = new System.Drawing.Size(143, 15);
            this.labelSupervisorDegree.TabIndex = 2;
            this.labelSupervisorDegree.Text = "Степень/звание ТГУ:";

            // txtSupervisorDegree
            this.txtSupervisorDegree.Location = new System.Drawing.Point(282, 38);
            this.txtSupervisorDegree.Name = "txtSupervisorDegree";
            this.txtSupervisorDegree.Size = new System.Drawing.Size(270, 23);
            this.txtSupervisorDegree.TabIndex = 3;

            // labelOrgSupervisorName
            this.labelOrgSupervisorName.AutoSize = true;
            this.labelOrgSupervisorName.Location = new System.Drawing.Point(6, 65);
            this.labelOrgSupervisorName.Name = "labelOrgSupervisorName";
            this.labelOrgSupervisorName.Size = new System.Drawing.Size(205, 15);
            this.labelOrgSupervisorName.TabIndex = 4;
            this.labelOrgSupervisorName.Text = "Руководитель от орг. (ФИО):";

            // txtOrgSupervisorName
            this.txtOrgSupervisorName.Location = new System.Drawing.Point(6, 83);
            this.txtOrgSupervisorName.Name = "txtOrgSupervisorName";
            this.txtOrgSupervisorName.Size = new System.Drawing.Size(270, 23);
            this.txtOrgSupervisorName.TabIndex = 5;

            // labelOrgSupervisorInfo
            this.labelOrgSupervisorInfo.AutoSize = true;
            this.labelOrgSupervisorInfo.Location = new System.Drawing.Point(282, 65);
            this.labelOrgSupervisorInfo.Name = "labelOrgSupervisorInfo";
            this.labelOrgSupervisorInfo.Size = new System.Drawing.Size(210, 15);
            this.labelOrgSupervisorInfo.TabIndex = 6;
            this.labelOrgSupervisorInfo.Text = "Степень/должность от орг.:";

            // txtOrgSupervisorInfo
            this.txtOrgSupervisorInfo.Location = new System.Drawing.Point(282, 83);
            this.txtOrgSupervisorInfo.Name = "txtOrgSupervisorInfo";
            this.txtOrgSupervisorInfo.Size = new System.Drawing.Size(270, 23);
            this.txtOrgSupervisorInfo.TabIndex = 7;

            // groupBoxSpecific
            this.groupBoxSpecific.Controls.Add(this.txtPracticeView);
            this.groupBoxSpecific.Controls.Add(this.labelPracticeView);
            this.groupBoxSpecific.Controls.Add(this.txtPracticeType);
            this.groupBoxSpecific.Controls.Add(this.labelPracticeType);
            this.groupBoxSpecific.Controls.Add(this.txtDisciplineName);
            this.groupBoxSpecific.Controls.Add(this.labelDisciplineName);
            this.groupBoxSpecific.Controls.Add(this.txtLabWorkName);
            this.groupBoxSpecific.Controls.Add(this.labelLabWorkName);
            this.groupBoxSpecific.Controls.Add(this.txtTopicName);
            this.groupBoxSpecific.Controls.Add(this.labelTopicName);
            this.groupBoxSpecific.Location = new System.Drawing.Point(12, 410);
            this.groupBoxSpecific.Name = "groupBoxSpecific";
            this.groupBoxSpecific.Size = new System.Drawing.Size(560, 80);
            this.groupBoxSpecific.TabIndex = 6;
            this.groupBoxSpecific.TabStop = false;
            this.groupBoxSpecific.Text = "Данные работы";

            // labelPracticeView
            this.labelPracticeView.AutoSize = true;
            this.labelPracticeView.Location = new System.Drawing.Point(6, 20);
            this.labelPracticeView.Name = "labelPracticeView";
            this.labelPracticeView.Size = new System.Drawing.Size(78, 15);
            this.labelPracticeView.TabIndex = 0;
            this.labelPracticeView.Text = "Вид практики:";

            // txtPracticeView
            this.txtPracticeView.Location = new System.Drawing.Point(6, 38);
            this.txtPracticeView.Name = "txtPracticeView";
            this.txtPracticeView.Size = new System.Drawing.Size(180, 23);
            this.txtPracticeView.TabIndex = 1;

            // labelPracticeType
            this.labelPracticeType.AutoSize = true;
            this.labelPracticeType.Location = new System.Drawing.Point(192, 20);
            this.labelPracticeType.Name = "labelPracticeType";
            this.labelPracticeType.Size = new System.Drawing.Size(76, 15);
            this.labelPracticeType.TabIndex = 2;
            this.labelPracticeType.Text = "Тип практики:";

            // txtPracticeType
            this.txtPracticeType.Location = new System.Drawing.Point(192, 38);
            this.txtPracticeType.Name = "txtPracticeType";
            this.txtPracticeType.Size = new System.Drawing.Size(180, 23);
            this.txtPracticeType.TabIndex = 3;

            // labelDisciplineName
            this.labelDisciplineName.AutoSize = true;
            this.labelDisciplineName.Location = new System.Drawing.Point(6, 20);
            this.labelDisciplineName.Name = "labelDisciplineName";
            this.labelDisciplineName.Size = new System.Drawing.Size(118, 15);
            this.labelDisciplineName.TabIndex = 4;
            this.labelDisciplineName.Text = "Наименование дисциплины:";

            // txtDisciplineName
            this.txtDisciplineName.Location = new System.Drawing.Point(6, 38);
            this.txtDisciplineName.Name = "txtDisciplineName";
            this.txtDisciplineName.Size = new System.Drawing.Size(548, 23);
            this.txtDisciplineName.TabIndex = 5;

            // labelLabWorkName
            this.labelLabWorkName.AutoSize = true;
            this.labelLabWorkName.Location = new System.Drawing.Point(6, 20);
            this.labelLabWorkName.Name = "labelLabWorkName";
            this.labelLabWorkName.Size = new System.Drawing.Size(168, 15);
            this.labelLabWorkName.TabIndex = 6;
            this.labelLabWorkName.Text = "Наименование работы:";

            // txtLabWorkName
            this.txtLabWorkName.Location = new System.Drawing.Point(6, 38);
            this.txtLabWorkName.Name = "txtLabWorkName";
            this.txtLabWorkName.Size = new System.Drawing.Size(548, 23);
            this.txtLabWorkName.TabIndex = 7;

            // labelTopicName
            this.labelTopicName.AutoSize = true;
            this.labelTopicName.Location = new System.Drawing.Point(6, 20);
            this.labelTopicName.Name = "labelTopicName";
            this.labelTopicName.Size = new System.Drawing.Size(88, 15);
            this.labelTopicName.TabIndex = 8;
            this.labelTopicName.Text = "Тема работы:";

            // txtTopicName
            this.txtTopicName.Location = new System.Drawing.Point(6, 38);
            this.txtTopicName.Name = "txtTopicName";
            this.txtTopicName.Size = new System.Drawing.Size(548, 23);
            this.txtTopicName.TabIndex = 9;

            // btnGenerate
            this.btnGenerate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnGenerate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnGenerate.Location = new System.Drawing.Point(12, 496);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(560, 35);
            this.btnGenerate.TabIndex = 8;
            this.btnGenerate.Text = "📄 Создать отчет";
            this.btnGenerate.UseVisualStyleBackColor = false;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);

            // txtResult
            this.txtResult.Location = new System.Drawing.Point(12, 537);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(560, 60);
            this.txtResult.TabIndex = 9;

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 609);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.groupBoxSpecific);
            this.Controls.Add(this.groupBoxSupervisor);
            this.Controls.Add(this.groupBoxEducation);
            this.Controls.Add(this.groupBoxStudent);
            this.Controls.Add(this.comboBoxType);
            this.Controls.Add(this.labelType);
            this.Controls.Add(this.labelTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Генератор отчетов ТГУ";
            this.groupBoxStudent.ResumeLayout(false);
            this.groupBoxStudent.PerformLayout();
            this.groupBoxEducation.ResumeLayout(false);
            this.groupBoxEducation.PerformLayout();
            this.groupBoxSupervisor.ResumeLayout(false);
            this.groupBoxSupervisor.PerformLayout();
            this.groupBoxSpecific.ResumeLayout(false);
            this.groupBoxSpecific.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.GroupBox groupBoxStudent;
        private System.Windows.Forms.TextBox txtStudentName;
        private System.Windows.Forms.Label labelStudent;
        private System.Windows.Forms.TextBox txtGroup;
        private System.Windows.Forms.Label labelGroup;
        private System.Windows.Forms.GroupBox groupBoxEducation;
        private System.Windows.Forms.TextBox txtDepartment;
        private System.Windows.Forms.Label labelDepartment;
        private System.Windows.Forms.TextBox txtDirectionCode;
        private System.Windows.Forms.Label labelDirectionCode;
        private System.Windows.Forms.TextBox txtDirectionName;
        private System.Windows.Forms.Label labelDirectionName;
        private System.Windows.Forms.TextBox txtProfile;
        private System.Windows.Forms.Label labelProfile;
        private System.Windows.Forms.GroupBox groupBoxSupervisor;
        private System.Windows.Forms.TextBox txtSupervisorName;
        private System.Windows.Forms.Label labelSupervisorName;
        private System.Windows.Forms.TextBox txtSupervisorDegree;
        private System.Windows.Forms.Label labelSupervisorDegree;
        private System.Windows.Forms.TextBox txtOrgSupervisorName;
        private System.Windows.Forms.Label labelOrgSupervisorName;
        private System.Windows.Forms.TextBox txtOrgSupervisorInfo;
        private System.Windows.Forms.Label labelOrgSupervisorInfo;
        private System.Windows.Forms.GroupBox groupBoxSpecific;
        private System.Windows.Forms.TextBox txtPracticeView;
        private System.Windows.Forms.Label labelPracticeView;
        private System.Windows.Forms.TextBox txtPracticeType;
        private System.Windows.Forms.Label labelPracticeType;
        private System.Windows.Forms.TextBox txtDisciplineName;
        private System.Windows.Forms.Label labelDisciplineName;
        private System.Windows.Forms.TextBox txtLabWorkName;
        private System.Windows.Forms.Label labelLabWorkName;
        private System.Windows.Forms.TextBox txtTopicName;
        private System.Windows.Forms.Label labelTopicName;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.TextBox txtResult;
    }
}