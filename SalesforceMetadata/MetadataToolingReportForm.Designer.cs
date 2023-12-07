namespace SalesforceMetadata
{
    partial class MetadataToolingReportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MetadataToolingReportForm));
            this.tbMetadataFolderLocation = new System.Windows.Forms.TextBox();
            this.btnGenerateToolingReport = new System.Windows.Forms.Button();
            this.lblMetdataFolderSelection = new System.Windows.Forms.Label();
            this.lbToolingObjects = new System.Windows.Forms.CheckedListBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.rtStatus = new System.Windows.Forms.RichTextBox();
            this.lblToolingObject = new System.Windows.Forms.Label();
            this.cbRetrieveApexClassCoverage = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // tbMetadataFolderLocation
            // 
            this.tbMetadataFolderLocation.Location = new System.Drawing.Point(15, 37);
            this.tbMetadataFolderLocation.Name = "tbMetadataFolderLocation";
            this.tbMetadataFolderLocation.Size = new System.Drawing.Size(753, 20);
            this.tbMetadataFolderLocation.TabIndex = 1;
            this.tbMetadataFolderLocation.DoubleClick += new System.EventHandler(this.tbMetadataFolderLocation_DoubleClick);
            // 
            // btnGenerateToolingReport
            // 
            this.btnGenerateToolingReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGenerateToolingReport.Location = new System.Drawing.Point(14, 609);
            this.btnGenerateToolingReport.Name = "btnGenerateToolingReport";
            this.btnGenerateToolingReport.Size = new System.Drawing.Size(158, 23);
            this.btnGenerateToolingReport.TabIndex = 6;
            this.btnGenerateToolingReport.Text = "Generate Tooling Report";
            this.btnGenerateToolingReport.UseVisualStyleBackColor = true;
            this.btnGenerateToolingReport.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // lblMetdataFolderSelection
            // 
            this.lblMetdataFolderSelection.AutoSize = true;
            this.lblMetdataFolderSelection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMetdataFolderSelection.Location = new System.Drawing.Point(12, 21);
            this.lblMetdataFolderSelection.Name = "lblMetdataFolderSelection";
            this.lblMetdataFolderSelection.Size = new System.Drawing.Size(608, 13);
            this.lblMetdataFolderSelection.TabIndex = 0;
            this.lblMetdataFolderSelection.Text = "Select Folder where the Salesforce Metadata Resides. This will be used to read fi" +
    "les from specific folders.";
            this.lblMetdataFolderSelection.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // lbToolingObjects
            // 
            this.lbToolingObjects.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbToolingObjects.FormattingEnabled = true;
            this.lbToolingObjects.Items.AddRange(new object[] {
            "ApexClass",
            "ApexComponent",
            "ApexEmailNotification",
            "ApexPage",
            "ApexTrigger",
            "AssignmentRule",
            "AuraDefinition",
            "AuradefinitionBundle",
            "AutoResponseRule",
            "CompactLayout",
            "CustomObject",
            "CustomTab",
            "EmailTemplate",
            "FlexiPage",
            "Flow",
            "FlowDefinition",
            "GlobalValueSet",
            "Layout",
            "LightningComponentBundle",
            "LightningComponentResource",
            "LookupFilter",
            "PermissionSet",
            "PermissionSetGroup",
            "Profile",
            "RecordType",
            "ValidationRule",
            "WorkflowAlert",
            "WorkflowFieldUpdate",
            "WorkflowOutboundMessage",
            "WorkflowRule",
            "WorkflowTask",
            "WorkSkillRouting"});
            this.lbToolingObjects.Location = new System.Drawing.Point(15, 99);
            this.lbToolingObjects.Name = "lbToolingObjects";
            this.lbToolingObjects.Size = new System.Drawing.Size(387, 484);
            this.lbToolingObjects.TabIndex = 3;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(468, 79);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(54, 17);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Status";
            // 
            // rtStatus
            // 
            this.rtStatus.Location = new System.Drawing.Point(471, 99);
            this.rtStatus.Name = "rtStatus";
            this.rtStatus.Size = new System.Drawing.Size(639, 484);
            this.rtStatus.TabIndex = 5;
            this.rtStatus.Text = "";
            // 
            // lblToolingObject
            // 
            this.lblToolingObject.AutoSize = true;
            this.lblToolingObject.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToolingObject.Location = new System.Drawing.Point(12, 79);
            this.lblToolingObject.Name = "lblToolingObject";
            this.lblToolingObject.Size = new System.Drawing.Size(122, 17);
            this.lblToolingObject.TabIndex = 2;
            this.lblToolingObject.Text = "Tooling Objects";
            // 
            // cbRetrieveApexClassCoverage
            // 
            this.cbRetrieveApexClassCoverage.AutoSize = true;
            this.cbRetrieveApexClassCoverage.Location = new System.Drawing.Point(921, 40);
            this.cbRetrieveApexClassCoverage.Name = "cbRetrieveApexClassCoverage";
            this.cbRetrieveApexClassCoverage.Size = new System.Drawing.Size(142, 17);
            this.cbRetrieveApexClassCoverage.TabIndex = 7;
            this.cbRetrieveApexClassCoverage.Text = "Retrieve Apex Coverage";
            this.cbRetrieveApexClassCoverage.UseVisualStyleBackColor = true;
            // 
            // MetadataToolingReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1122, 667);
            this.Controls.Add(this.cbRetrieveApexClassCoverage);
            this.Controls.Add(this.lblToolingObject);
            this.Controls.Add(this.rtStatus);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lbToolingObjects);
            this.Controls.Add(this.lblMetdataFolderSelection);
            this.Controls.Add(this.btnGenerateToolingReport);
            this.Controls.Add(this.tbMetadataFolderLocation);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MetadataToolingReportForm";
            this.Text = "Metadata / Tooling Report Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbMetadataFolderLocation;
        private System.Windows.Forms.Button btnGenerateToolingReport;
        private System.Windows.Forms.Label lblMetdataFolderSelection;
        private System.Windows.Forms.CheckedListBox lbToolingObjects;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.RichTextBox rtStatus;
        private System.Windows.Forms.Label lblToolingObject;
        private System.Windows.Forms.CheckBox cbRetrieveApexClassCoverage;
    }
}