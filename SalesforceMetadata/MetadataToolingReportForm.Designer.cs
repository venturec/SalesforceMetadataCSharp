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
            this.lblSelectNone = new System.Windows.Forms.Label();
            this.btnSelectNone = new System.Windows.Forms.Button();
            this.lblSelectAll = new System.Windows.Forms.Label();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.lblSFToolingTypes = new System.Windows.Forms.Label();
            this.cbDefaultCoreObjects = new System.Windows.Forms.CheckBox();
            this.fromOrgGroup = new System.Windows.Forms.GroupBox();
            this.lblSFUsername = new System.Windows.Forms.Label();
            this.cmbUserName = new System.Windows.Forms.ComboBox();
            this.fromOrgGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbMetadataFolderLocation
            // 
            this.tbMetadataFolderLocation.Location = new System.Drawing.Point(15, 41);
            this.tbMetadataFolderLocation.Name = "tbMetadataFolderLocation";
            this.tbMetadataFolderLocation.Size = new System.Drawing.Size(774, 20);
            this.tbMetadataFolderLocation.TabIndex = 1;
            this.tbMetadataFolderLocation.DoubleClick += new System.EventHandler(this.tbMetadataFolderLocation_DoubleClick);
            // 
            // btnGenerateToolingReport
            // 
            this.btnGenerateToolingReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGenerateToolingReport.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.btnGenerateToolingReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateToolingReport.Location = new System.Drawing.Point(15, 747);
            this.btnGenerateToolingReport.Name = "btnGenerateToolingReport";
            this.btnGenerateToolingReport.Size = new System.Drawing.Size(208, 45);
            this.btnGenerateToolingReport.TabIndex = 14;
            this.btnGenerateToolingReport.Text = "Generate Tooling Report";
            this.btnGenerateToolingReport.UseVisualStyleBackColor = true;
            this.btnGenerateToolingReport.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // lblMetdataFolderSelection
            // 
            this.lblMetdataFolderSelection.AutoSize = true;
            this.lblMetdataFolderSelection.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMetdataFolderSelection.Location = new System.Drawing.Point(12, 21);
            this.lblMetdataFolderSelection.Name = "lblMetdataFolderSelection";
            this.lblMetdataFolderSelection.Size = new System.Drawing.Size(777, 17);
            this.lblMetdataFolderSelection.TabIndex = 0;
            this.lblMetdataFolderSelection.Text = "Select Folder where the Salesforce Metadata Resides. This will be used to read fi" +
    "les from specific folders.";
            this.lblMetdataFolderSelection.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // lbToolingObjects
            // 
            this.lbToolingObjects.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbToolingObjects.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbToolingObjects.FormattingEnabled = true;
            this.lbToolingObjects.Items.AddRange(new object[] {
            "ApexClass",
            "ApexComponent",
            "ApexEmailNotification",
            "ApexPage",
            "ApexTrigger",
            "AuraDefinitionBundle",
            "CompactLayout",
            "CustomApplication",
            "CustomField",
            "CustomObject",
            "CustomTab",
            "EmailTemplate",
            "FieldSet",
            "FlexiPage",
            "Flow",
            "GlobalValueSet",
            "Group",
            "Layout",
            "LightningComponentBundle",
            "PermissionSet",
            "PermissionSetAssignment",
            "PermissionSetGroup",
            "Profile",
            "QuickActionDefinition",
            "RecordType",
            "TabDefinition",
            "ValidationRule",
            "WorkflowRule",
            "WorkflowAlert",
            "WorkflowFieldUpdate",
            "WorkflowOutboundMessage",
            "WorkflowTask"});
            this.lbToolingObjects.Location = new System.Drawing.Point(15, 200);
            this.lbToolingObjects.Name = "lbToolingObjects";
            this.lbToolingObjects.Size = new System.Drawing.Size(437, 526);
            this.lbToolingObjects.TabIndex = 10;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(540, 164);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(54, 17);
            this.lblStatus.TabIndex = 11;
            this.lblStatus.Text = "Status";
            // 
            // rtStatus
            // 
            this.rtStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtStatus.Location = new System.Drawing.Point(543, 200);
            this.rtStatus.Name = "rtStatus";
            this.rtStatus.Size = new System.Drawing.Size(810, 526);
            this.rtStatus.TabIndex = 12;
            this.rtStatus.Text = "";
            // 
            // lblToolingObject
            // 
            this.lblToolingObject.AutoSize = true;
            this.lblToolingObject.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToolingObject.Location = new System.Drawing.Point(18, 180);
            this.lblToolingObject.Name = "lblToolingObject";
            this.lblToolingObject.Size = new System.Drawing.Size(122, 17);
            this.lblToolingObject.TabIndex = 9;
            this.lblToolingObject.Text = "Tooling Objects";
            // 
            // cbRetrieveApexClassCoverage
            // 
            this.cbRetrieveApexClassCoverage.AutoSize = true;
            this.cbRetrieveApexClassCoverage.Location = new System.Drawing.Point(287, 164);
            this.cbRetrieveApexClassCoverage.Name = "cbRetrieveApexClassCoverage";
            this.cbRetrieveApexClassCoverage.Size = new System.Drawing.Size(142, 17);
            this.cbRetrieveApexClassCoverage.TabIndex = 8;
            this.cbRetrieveApexClassCoverage.Text = "Retrieve Apex Coverage";
            this.cbRetrieveApexClassCoverage.UseVisualStyleBackColor = true;
            // 
            // lblSelectNone
            // 
            this.lblSelectNone.AutoSize = true;
            this.lblSelectNone.Location = new System.Drawing.Point(170, 118);
            this.lblSelectNone.Name = "lblSelectNone";
            this.lblSelectNone.Size = new System.Drawing.Size(66, 13);
            this.lblSelectNone.TabIndex = 6;
            this.lblSelectNone.Text = "Select None";
            // 
            // btnSelectNone
            // 
            this.btnSelectNone.Location = new System.Drawing.Point(146, 116);
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.Size = new System.Drawing.Size(18, 17);
            this.btnSelectNone.TabIndex = 5;
            this.btnSelectNone.UseVisualStyleBackColor = true;
            // 
            // lblSelectAll
            // 
            this.lblSelectAll.AutoSize = true;
            this.lblSelectAll.Location = new System.Drawing.Point(41, 118);
            this.lblSelectAll.Name = "lblSelectAll";
            this.lblSelectAll.Size = new System.Drawing.Size(51, 13);
            this.lblSelectAll.TabIndex = 4;
            this.lblSelectAll.Text = "Select All";
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(17, 116);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(18, 17);
            this.btnSelectAll.TabIndex = 3;
            this.btnSelectAll.UseVisualStyleBackColor = true;
            // 
            // lblSFToolingTypes
            // 
            this.lblSFToolingTypes.AutoSize = true;
            this.lblSFToolingTypes.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSFToolingTypes.Location = new System.Drawing.Point(12, 86);
            this.lblSFToolingTypes.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSFToolingTypes.Name = "lblSFToolingTypes";
            this.lblSFToolingTypes.Size = new System.Drawing.Size(367, 17);
            this.lblSFToolingTypes.TabIndex = 2;
            this.lblSFToolingTypes.Text = "Select the Salesforce Tooling Objects to Retrieve";
            // 
            // cbDefaultCoreObjects
            // 
            this.cbDefaultCoreObjects.AutoSize = true;
            this.cbDefaultCoreObjects.Location = new System.Drawing.Point(287, 117);
            this.cbDefaultCoreObjects.Name = "cbDefaultCoreObjects";
            this.cbDefaultCoreObjects.Size = new System.Drawing.Size(215, 17);
            this.cbDefaultCoreObjects.TabIndex = 7;
            this.cbDefaultCoreObjects.Text = "Default - Objects Fields Triggers Classes";
            this.cbDefaultCoreObjects.UseVisualStyleBackColor = true;
            // 
            // fromOrgGroup
            // 
            this.fromOrgGroup.BackColor = System.Drawing.SystemColors.Control;
            this.fromOrgGroup.Controls.Add(this.lblSFUsername);
            this.fromOrgGroup.Controls.Add(this.cmbUserName);
            this.fromOrgGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fromOrgGroup.Location = new System.Drawing.Point(807, 12);
            this.fromOrgGroup.Name = "fromOrgGroup";
            this.fromOrgGroup.Size = new System.Drawing.Size(523, 81);
            this.fromOrgGroup.TabIndex = 13;
            this.fromOrgGroup.TabStop = false;
            this.fromOrgGroup.Text = "From Org";
            // 
            // lblSFUsername
            // 
            this.lblSFUsername.AutoSize = true;
            this.lblSFUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSFUsername.Location = new System.Drawing.Point(29, 30);
            this.lblSFUsername.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSFUsername.Name = "lblSFUsername";
            this.lblSFUsername.Size = new System.Drawing.Size(143, 17);
            this.lblSFUsername.TabIndex = 0;
            this.lblSFUsername.Text = "Username (from Org)";
            // 
            // cmbUserName
            // 
            this.cmbUserName.DropDownWidth = 325;
            this.cmbUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbUserName.FormattingEnabled = true;
            this.cmbUserName.Location = new System.Drawing.Point(194, 27);
            this.cmbUserName.Margin = new System.Windows.Forms.Padding(2);
            this.cmbUserName.Name = "cmbUserName";
            this.cmbUserName.Size = new System.Drawing.Size(324, 24);
            this.cmbUserName.TabIndex = 1;
            // 
            // MetadataToolingReportForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1365, 804);
            this.Controls.Add(this.fromOrgGroup);
            this.Controls.Add(this.cbDefaultCoreObjects);
            this.Controls.Add(this.lblSelectNone);
            this.Controls.Add(this.btnSelectNone);
            this.Controls.Add(this.lblSelectAll);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.lblSFToolingTypes);
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
            this.fromOrgGroup.ResumeLayout(false);
            this.fromOrgGroup.PerformLayout();
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
        private System.Windows.Forms.Label lblSelectNone;
        private System.Windows.Forms.Button btnSelectNone;
        private System.Windows.Forms.Label lblSelectAll;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Label lblSFToolingTypes;
        private System.Windows.Forms.CheckBox cbDefaultCoreObjects;
        private System.Windows.Forms.GroupBox fromOrgGroup;
        private System.Windows.Forms.Label lblSFUsername;
        public System.Windows.Forms.ComboBox cmbUserName;
    }
}