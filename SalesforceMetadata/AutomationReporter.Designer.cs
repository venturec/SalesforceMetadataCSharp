namespace SalesforceMetadata
{
    partial class AutomationReporter
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutomationReporter));
            this.ProjectFolder = new System.Windows.Forms.Label();
            this.SaveResultsTo = new System.Windows.Forms.Label();
            this.btnRunAutomationReport = new System.Windows.Forms.Button();
            this.tbProjectFolder = new System.Windows.Forms.TextBox();
            this.tbFileSaveTo = new System.Windows.Forms.TextBox();
            this.cbWriteToDataDictionary = new System.Windows.Forms.CheckBox();
            this.btnParseObjectsAndFields = new System.Windows.Forms.Button();
            this.tbSearchFilter = new System.Windows.Forms.TextBox();
            this.btnFieldReferences = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ProjectFolder
            // 
            this.ProjectFolder.AutoSize = true;
            this.ProjectFolder.Location = new System.Drawing.Point(12, 30);
            this.ProjectFolder.Name = "ProjectFolder";
            this.ProjectFolder.Size = new System.Drawing.Size(72, 13);
            this.ProjectFolder.TabIndex = 0;
            this.ProjectFolder.Text = "Project Folder";
            // 
            // SaveResultsTo
            // 
            this.SaveResultsTo.AutoSize = true;
            this.SaveResultsTo.Location = new System.Drawing.Point(12, 66);
            this.SaveResultsTo.Name = "SaveResultsTo";
            this.SaveResultsTo.Size = new System.Drawing.Size(86, 13);
            this.SaveResultsTo.TabIndex = 2;
            this.SaveResultsTo.Text = "Save Results To";
            // 
            // btnRunAutomationReport
            // 
            this.btnRunAutomationReport.Location = new System.Drawing.Point(127, 207);
            this.btnRunAutomationReport.Name = "btnRunAutomationReport";
            this.btnRunAutomationReport.Size = new System.Drawing.Size(199, 23);
            this.btnRunAutomationReport.TabIndex = 6;
            this.btnRunAutomationReport.Text = "Run Automation Report";
            this.btnRunAutomationReport.UseVisualStyleBackColor = true;
            this.btnRunAutomationReport.Visible = false;
            // 
            // tbProjectFolder
            // 
            this.tbProjectFolder.Location = new System.Drawing.Point(127, 30);
            this.tbProjectFolder.Name = "tbProjectFolder";
            this.tbProjectFolder.Size = new System.Drawing.Size(521, 20);
            this.tbProjectFolder.TabIndex = 1;
            this.tbProjectFolder.DoubleClick += new System.EventHandler(this.tbProjectFolder_DoubleClick);
            // 
            // tbFileSaveTo
            // 
            this.tbFileSaveTo.Location = new System.Drawing.Point(127, 66);
            this.tbFileSaveTo.Name = "tbFileSaveTo";
            this.tbFileSaveTo.Size = new System.Drawing.Size(521, 20);
            this.tbFileSaveTo.TabIndex = 3;
            this.tbFileSaveTo.DoubleClick += new System.EventHandler(this.tbFileSaveTo_DoubleClick);
            // 
            // cbWriteToDataDictionary
            // 
            this.cbWriteToDataDictionary.AutoSize = true;
            this.cbWriteToDataDictionary.Checked = true;
            this.cbWriteToDataDictionary.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbWriteToDataDictionary.Location = new System.Drawing.Point(675, 130);
            this.cbWriteToDataDictionary.Name = "cbWriteToDataDictionary";
            this.cbWriteToDataDictionary.Size = new System.Drawing.Size(143, 17);
            this.cbWriteToDataDictionary.TabIndex = 8;
            this.cbWriteToDataDictionary.Text = "Write To Data Dictionary";
            this.cbWriteToDataDictionary.UseVisualStyleBackColor = true;
            // 
            // btnParseObjectsAndFields
            // 
            this.btnParseObjectsAndFields.Location = new System.Drawing.Point(127, 130);
            this.btnParseObjectsAndFields.Name = "btnParseObjectsAndFields";
            this.btnParseObjectsAndFields.Size = new System.Drawing.Size(199, 23);
            this.btnParseObjectsAndFields.TabIndex = 4;
            this.btnParseObjectsAndFields.Text = "Parse Objects and Fields";
            this.btnParseObjectsAndFields.UseVisualStyleBackColor = true;
            this.btnParseObjectsAndFields.Click += new System.EventHandler(this.btnParseObjectsAndFields_Click);
            // 
            // tbSearchFilter
            // 
            this.tbSearchFilter.Location = new System.Drawing.Point(486, 128);
            this.tbSearchFilter.Name = "tbSearchFilter";
            this.tbSearchFilter.Size = new System.Drawing.Size(162, 20);
            this.tbSearchFilter.TabIndex = 7;
            // 
            // btnFieldReferences
            // 
            this.btnFieldReferences.Location = new System.Drawing.Point(127, 168);
            this.btnFieldReferences.Name = "btnFieldReferences";
            this.btnFieldReferences.Size = new System.Drawing.Size(199, 23);
            this.btnFieldReferences.TabIndex = 5;
            this.btnFieldReferences.Text = "Find Field References";
            this.btnFieldReferences.UseVisualStyleBackColor = true;
            this.btnFieldReferences.Click += new System.EventHandler(this.btnFieldReferences_Click);
            // 
            // AutomationReporter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(940, 551);
            this.Controls.Add(this.btnFieldReferences);
            this.Controls.Add(this.tbSearchFilter);
            this.Controls.Add(this.btnParseObjectsAndFields);
            this.Controls.Add(this.cbWriteToDataDictionary);
            this.Controls.Add(this.tbFileSaveTo);
            this.Controls.Add(this.tbProjectFolder);
            this.Controls.Add(this.btnRunAutomationReport);
            this.Controls.Add(this.SaveResultsTo);
            this.Controls.Add(this.ProjectFolder);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AutomationReporter";
            this.Text = "Automation Reporter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ProjectFolder;
        private System.Windows.Forms.Label SaveResultsTo;
        private System.Windows.Forms.Button btnRunAutomationReport;
        private System.Windows.Forms.TextBox tbProjectFolder;
        private System.Windows.Forms.TextBox tbFileSaveTo;
        private System.Windows.Forms.CheckBox cbWriteToDataDictionary;
        private System.Windows.Forms.Button btnParseObjectsAndFields;
        private System.Windows.Forms.TextBox tbSearchFilter;
        private System.Windows.Forms.Button btnFieldReferences;
    }
}