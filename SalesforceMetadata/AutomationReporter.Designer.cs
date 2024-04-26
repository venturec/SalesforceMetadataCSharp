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
            this.btnRunAutomationFieldExtraction = new System.Windows.Forms.Button();
            this.tbProjectFolder = new System.Windows.Forms.TextBox();
            this.tbFileSaveTo = new System.Windows.Forms.TextBox();
            this.cbWriteToDataDictionary = new System.Windows.Forms.CheckBox();
            this.btnParseObjectsAndFields = new System.Windows.Forms.Button();
            this.tbSearchFilter = new System.Windows.Forms.TextBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.btnParseFlows = new System.Windows.Forms.Button();
            this.btnGetApexClassNamesAndMethods = new System.Windows.Forms.Button();
            this.btnAutomationHierarchy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ProjectFolder
            // 
            this.ProjectFolder.AutoSize = true;
            this.ProjectFolder.Location = new System.Drawing.Point(12, 30);
            this.ProjectFolder.Name = "ProjectFolder";
            this.ProjectFolder.Size = new System.Drawing.Size(96, 17);
            this.ProjectFolder.TabIndex = 0;
            this.ProjectFolder.Text = "Project Folder";
            // 
            // SaveResultsTo
            // 
            this.SaveResultsTo.AutoSize = true;
            this.SaveResultsTo.Location = new System.Drawing.Point(12, 66);
            this.SaveResultsTo.Name = "SaveResultsTo";
            this.SaveResultsTo.Size = new System.Drawing.Size(112, 17);
            this.SaveResultsTo.TabIndex = 2;
            this.SaveResultsTo.Text = "Save Results To";
            // 
            // btnRunAutomationFieldExtraction
            // 
            this.btnRunAutomationFieldExtraction.Location = new System.Drawing.Point(15, 200);
            this.btnRunAutomationFieldExtraction.Name = "btnRunAutomationFieldExtraction";
            this.btnRunAutomationFieldExtraction.Size = new System.Drawing.Size(265, 38);
            this.btnRunAutomationFieldExtraction.TabIndex = 8;
            this.btnRunAutomationFieldExtraction.Text = "Run Automation Field Extraction";
            this.btnRunAutomationFieldExtraction.UseVisualStyleBackColor = true;
            this.btnRunAutomationFieldExtraction.Click += new System.EventHandler(this.btnRunAutomationFieldExtraction_Click);
            // 
            // tbProjectFolder
            // 
            this.tbProjectFolder.Location = new System.Drawing.Point(127, 30);
            this.tbProjectFolder.Name = "tbProjectFolder";
            this.tbProjectFolder.Size = new System.Drawing.Size(895, 23);
            this.tbProjectFolder.TabIndex = 1;
            this.tbProjectFolder.DoubleClick += new System.EventHandler(this.tbProjectFolder_DoubleClick);
            // 
            // tbFileSaveTo
            // 
            this.tbFileSaveTo.Location = new System.Drawing.Point(127, 66);
            this.tbFileSaveTo.Name = "tbFileSaveTo";
            this.tbFileSaveTo.Size = new System.Drawing.Size(895, 23);
            this.tbFileSaveTo.TabIndex = 3;
            this.tbFileSaveTo.DoubleClick += new System.EventHandler(this.tbFileSaveTo_DoubleClick);
            // 
            // cbWriteToDataDictionary
            // 
            this.cbWriteToDataDictionary.AutoSize = true;
            this.cbWriteToDataDictionary.Checked = true;
            this.cbWriteToDataDictionary.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbWriteToDataDictionary.Enabled = false;
            this.cbWriteToDataDictionary.Location = new System.Drawing.Point(1171, 32);
            this.cbWriteToDataDictionary.Name = "cbWriteToDataDictionary";
            this.cbWriteToDataDictionary.Size = new System.Drawing.Size(182, 21);
            this.cbWriteToDataDictionary.TabIndex = 6;
            this.cbWriteToDataDictionary.Text = "Write To Data Dictionary";
            this.cbWriteToDataDictionary.UseVisualStyleBackColor = true;
            // 
            // btnParseObjectsAndFields
            // 
            this.btnParseObjectsAndFields.Location = new System.Drawing.Point(15, 156);
            this.btnParseObjectsAndFields.Name = "btnParseObjectsAndFields";
            this.btnParseObjectsAndFields.Size = new System.Drawing.Size(265, 38);
            this.btnParseObjectsAndFields.TabIndex = 7;
            this.btnParseObjectsAndFields.Text = "Parse Objects and Fields from XML";
            this.btnParseObjectsAndFields.UseVisualStyleBackColor = true;
            this.btnParseObjectsAndFields.Click += new System.EventHandler(this.btnParseObjectsAndFields_Click);
            // 
            // tbSearchFilter
            // 
            this.tbSearchFilter.Location = new System.Drawing.Point(127, 102);
            this.tbSearchFilter.Name = "tbSearchFilter";
            this.tbSearchFilter.Size = new System.Drawing.Size(162, 23);
            this.tbSearchFilter.TabIndex = 5;
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(12, 105);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(39, 17);
            this.lblFilter.TabIndex = 4;
            this.lblFilter.Text = "Filter";
            // 
            // btnParseFlows
            // 
            this.btnParseFlows.Location = new System.Drawing.Point(15, 288);
            this.btnParseFlows.Name = "btnParseFlows";
            this.btnParseFlows.Size = new System.Drawing.Size(265, 38);
            this.btnParseFlows.TabIndex = 10;
            this.btnParseFlows.Text = "Parse Flows";
            this.btnParseFlows.UseVisualStyleBackColor = true;
            this.btnParseFlows.Click += new System.EventHandler(this.btnParseFlows_Click);
            // 
            // btnGetApexClassNamesAndMethods
            // 
            this.btnGetApexClassNamesAndMethods.Location = new System.Drawing.Point(15, 244);
            this.btnGetApexClassNamesAndMethods.Name = "btnGetApexClassNamesAndMethods";
            this.btnGetApexClassNamesAndMethods.Size = new System.Drawing.Size(265, 38);
            this.btnGetApexClassNamesAndMethods.TabIndex = 9;
            this.btnGetApexClassNamesAndMethods.Text = "Get Apex Class Names and Methods";
            this.btnGetApexClassNamesAndMethods.UseVisualStyleBackColor = true;
            this.btnGetApexClassNamesAndMethods.Click += new System.EventHandler(this.btnGetApexClassNamesAndMethods_Click);
            // 
            // btnAutomationHierarchy
            // 
            this.btnAutomationHierarchy.Location = new System.Drawing.Point(306, 200);
            this.btnAutomationHierarchy.Name = "btnAutomationHierarchy";
            this.btnAutomationHierarchy.Size = new System.Drawing.Size(265, 38);
            this.btnAutomationHierarchy.TabIndex = 11;
            this.btnAutomationHierarchy.Text = "Automation Sequence Report";
            this.btnAutomationHierarchy.UseVisualStyleBackColor = true;
            this.btnAutomationHierarchy.Click += new System.EventHandler(this.btnAutomationHierarchy_Click);
            // 
            // AutomationReporter
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1382, 551);
            this.Controls.Add(this.btnAutomationHierarchy);
            this.Controls.Add(this.btnGetApexClassNamesAndMethods);
            this.Controls.Add(this.btnParseFlows);
            this.Controls.Add(this.lblFilter);
            this.Controls.Add(this.tbSearchFilter);
            this.Controls.Add(this.btnParseObjectsAndFields);
            this.Controls.Add(this.cbWriteToDataDictionary);
            this.Controls.Add(this.tbFileSaveTo);
            this.Controls.Add(this.tbProjectFolder);
            this.Controls.Add(this.btnRunAutomationFieldExtraction);
            this.Controls.Add(this.SaveResultsTo);
            this.Controls.Add(this.ProjectFolder);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AutomationReporter";
            this.Text = "Automation Reporter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ProjectFolder;
        private System.Windows.Forms.Label SaveResultsTo;
        private System.Windows.Forms.Button btnRunAutomationFieldExtraction;
        private System.Windows.Forms.TextBox tbProjectFolder;
        private System.Windows.Forms.TextBox tbFileSaveTo;
        private System.Windows.Forms.CheckBox cbWriteToDataDictionary;
        private System.Windows.Forms.Button btnParseObjectsAndFields;
        private System.Windows.Forms.TextBox tbSearchFilter;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.Button btnParseFlows;
        private System.Windows.Forms.Button btnGetApexClassNamesAndMethods;
        private System.Windows.Forms.Button btnAutomationHierarchy;
    }
}