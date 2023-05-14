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
            this.SuspendLayout();
            // 
            // ProjectFolder
            // 
            this.ProjectFolder.AutoSize = true;
            this.ProjectFolder.Location = new System.Drawing.Point(12, 30);
            this.ProjectFolder.Name = "ProjectFolder";
            this.ProjectFolder.Size = new System.Drawing.Size(72, 13);
            this.ProjectFolder.TabIndex = 1;
            this.ProjectFolder.Text = "Project Folder";
            // 
            // SaveResultsTo
            // 
            this.SaveResultsTo.AutoSize = true;
            this.SaveResultsTo.Location = new System.Drawing.Point(12, 66);
            this.SaveResultsTo.Name = "SaveResultsTo";
            this.SaveResultsTo.Size = new System.Drawing.Size(86, 13);
            this.SaveResultsTo.TabIndex = 3;
            this.SaveResultsTo.Text = "Save Results To";
            // 
            // btnRunAutomationReport
            // 
            this.btnRunAutomationReport.Location = new System.Drawing.Point(127, 128);
            this.btnRunAutomationReport.Name = "btnRunAutomationReport";
            this.btnRunAutomationReport.Size = new System.Drawing.Size(199, 23);
            this.btnRunAutomationReport.TabIndex = 7;
            this.btnRunAutomationReport.Text = "Run Automation Report";
            this.btnRunAutomationReport.UseVisualStyleBackColor = true;
            this.btnRunAutomationReport.Click += new System.EventHandler(this.btnRunAutomationReport_Click);
            // 
            // tbProjectFolder
            // 
            this.tbProjectFolder.Location = new System.Drawing.Point(127, 30);
            this.tbProjectFolder.Name = "tbProjectFolder";
            this.tbProjectFolder.Size = new System.Drawing.Size(521, 20);
            this.tbProjectFolder.TabIndex = 2;
            this.tbProjectFolder.DoubleClick += new System.EventHandler(this.tbProjectFolder_DoubleClick);
            // 
            // tbFileSaveTo
            // 
            this.tbFileSaveTo.Location = new System.Drawing.Point(127, 66);
            this.tbFileSaveTo.Name = "tbFileSaveTo";
            this.tbFileSaveTo.Size = new System.Drawing.Size(521, 20);
            this.tbFileSaveTo.TabIndex = 4;
            this.tbFileSaveTo.Text = "C:\\Users\\marcu\\Documents\\Projects\\Florida DEM\\Reports";
            this.tbFileSaveTo.DoubleClick += new System.EventHandler(this.tbFileSaveTo_DoubleClick);
            // 
            // AutomationReporter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(940, 551);
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
    }
}