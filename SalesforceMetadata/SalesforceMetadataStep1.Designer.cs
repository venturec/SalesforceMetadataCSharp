namespace SalesforceMetadata
{
    partial class SalesforceMetadataStep1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SalesforceMetadataStep1));
            this.btnGetMetadataTypes = new System.Windows.Forms.Button();
            this.lblSFMetadataPackage = new System.Windows.Forms.Label();
            this.lbMetadataTypes = new System.Windows.Forms.CheckedListBox();
            this.lblSFUsername = new System.Windows.Forms.Label();
            this.lblSalesforce = new System.Windows.Forms.Label();
            this.cmbUserName = new System.Windows.Forms.ComboBox();
            this.btnRetrieveMetadata = new System.Windows.Forms.Button();
            this.btnDeleteDebugLogs = new System.Windows.Forms.Button();
            this.btnDeploy = new System.Windows.Forms.Button();
            this.fromOrgGroup = new System.Windows.Forms.GroupBox();
            this.btnDevSBSeeding = new System.Windows.Forms.Button();
            this.btnSobjectFieldInspector = new System.Windows.Forms.Button();
            this.btnFromGenerateToolingChangeReport = new System.Windows.Forms.Button();
            this.btnConfigurationWorkbook = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.lblSelectAll = new System.Windows.Forms.Label();
            this.btnSelectNone = new System.Windows.Forms.Button();
            this.lblSelectNone = new System.Windows.Forms.Label();
            this.btnSelectDefaults = new System.Windows.Forms.Button();
            this.lblSelectDefaults = new System.Windows.Forms.Label();
            this.cbAllDebugLogs = new System.Windows.Forms.CheckBox();
            this.fromOrgGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGetMetadataTypes
            // 
            this.btnGetMetadataTypes.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnGetMetadataTypes.Location = new System.Drawing.Point(821, 214);
            this.btnGetMetadataTypes.Margin = new System.Windows.Forms.Padding(2);
            this.btnGetMetadataTypes.Name = "btnGetMetadataTypes";
            this.btnGetMetadataTypes.Size = new System.Drawing.Size(165, 25);
            this.btnGetMetadataTypes.TabIndex = 10;
            this.btnGetMetadataTypes.Text = "Get Metadata Types";
            this.btnGetMetadataTypes.UseVisualStyleBackColor = false;
            this.btnGetMetadataTypes.Click += new System.EventHandler(this.btnGetMetadataTypes_Click);
            // 
            // lblSFMetadataPackage
            // 
            this.lblSFMetadataPackage.AutoSize = true;
            this.lblSFMetadataPackage.Location = new System.Drawing.Point(23, 192);
            this.lblSFMetadataPackage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSFMetadataPackage.Name = "lblSFMetadataPackage";
            this.lblSFMetadataPackage.Size = new System.Drawing.Size(243, 13);
            this.lblSFMetadataPackage.TabIndex = 2;
            this.lblSFMetadataPackage.Text = "Select the Salesforce Metadata Types to Retrieve";
            // 
            // lbMetadataTypes
            // 
            this.lbMetadataTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbMetadataTypes.FormattingEnabled = true;
            this.lbMetadataTypes.HorizontalScrollbar = true;
            this.lbMetadataTypes.Location = new System.Drawing.Point(26, 214);
            this.lbMetadataTypes.Margin = new System.Windows.Forms.Padding(2);
            this.lbMetadataTypes.Name = "lbMetadataTypes";
            this.lbMetadataTypes.Size = new System.Drawing.Size(725, 499);
            this.lbMetadataTypes.TabIndex = 9;
            // 
            // lblSFUsername
            // 
            this.lblSFUsername.AutoSize = true;
            this.lblSFUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSFUsername.Location = new System.Drawing.Point(5, 34);
            this.lblSFUsername.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSFUsername.Name = "lblSFUsername";
            this.lblSFUsername.Size = new System.Drawing.Size(143, 17);
            this.lblSFUsername.TabIndex = 1;
            this.lblSFUsername.Text = "Username (from Org)";
            // 
            // lblSalesforce
            // 
            this.lblSalesforce.AutoSize = true;
            this.lblSalesforce.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalesforce.Location = new System.Drawing.Point(11, 13);
            this.lblSalesforce.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSalesforce.Name = "lblSalesforce";
            this.lblSalesforce.Size = new System.Drawing.Size(133, 29);
            this.lblSalesforce.TabIndex = 0;
            this.lblSalesforce.Text = "Salesforce";
            // 
            // cmbUserName
            // 
            this.cmbUserName.DropDownWidth = 325;
            this.cmbUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbUserName.FormattingEnabled = true;
            this.cmbUserName.Location = new System.Drawing.Point(152, 33);
            this.cmbUserName.Margin = new System.Windows.Forms.Padding(2);
            this.cmbUserName.Name = "cmbUserName";
            this.cmbUserName.Size = new System.Drawing.Size(346, 24);
            this.cmbUserName.TabIndex = 2;
            this.cmbUserName.SelectedIndexChanged += new System.EventHandler(this.cmbUserName_SelectedIndexChanged);
            // 
            // btnRetrieveMetadata
            // 
            this.btnRetrieveMetadata.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnRetrieveMetadata.Location = new System.Drawing.Point(1012, 214);
            this.btnRetrieveMetadata.Margin = new System.Windows.Forms.Padding(2);
            this.btnRetrieveMetadata.Name = "btnRetrieveMetadata";
            this.btnRetrieveMetadata.Size = new System.Drawing.Size(165, 25);
            this.btnRetrieveMetadata.TabIndex = 11;
            this.btnRetrieveMetadata.Text = "Retrieve Metadata Package";
            this.btnRetrieveMetadata.UseVisualStyleBackColor = false;
            this.btnRetrieveMetadata.Click += new System.EventHandler(this.btnRetrieveMetadata_Click);
            // 
            // btnDeleteDebugLogs
            // 
            this.btnDeleteDebugLogs.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnDeleteDebugLogs.Location = new System.Drawing.Point(821, 685);
            this.btnDeleteDebugLogs.Margin = new System.Windows.Forms.Padding(2);
            this.btnDeleteDebugLogs.Name = "btnDeleteDebugLogs";
            this.btnDeleteDebugLogs.Size = new System.Drawing.Size(165, 28);
            this.btnDeleteDebugLogs.TabIndex = 11;
            this.btnDeleteDebugLogs.Text = "Delete Debug Logs";
            this.btnDeleteDebugLogs.UseVisualStyleBackColor = false;
            this.btnDeleteDebugLogs.Click += new System.EventHandler(this.btnDeleteDebugLogs_Click);
            // 
            // btnDeploy
            // 
            this.btnDeploy.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnDeploy.Location = new System.Drawing.Point(821, 441);
            this.btnDeploy.Name = "btnDeploy";
            this.btnDeploy.Size = new System.Drawing.Size(165, 23);
            this.btnDeploy.TabIndex = 15;
            this.btnDeploy.Text = "Deploy";
            this.btnDeploy.UseVisualStyleBackColor = false;
            this.btnDeploy.Click += new System.EventHandler(this.btnDeploy_Click);
            // 
            // fromOrgGroup
            // 
            this.fromOrgGroup.BackColor = System.Drawing.SystemColors.Control;
            this.fromOrgGroup.Controls.Add(this.lblSFUsername);
            this.fromOrgGroup.Controls.Add(this.cmbUserName);
            this.fromOrgGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fromOrgGroup.Location = new System.Drawing.Point(26, 45);
            this.fromOrgGroup.Name = "fromOrgGroup";
            this.fromOrgGroup.Size = new System.Drawing.Size(523, 91);
            this.fromOrgGroup.TabIndex = 1;
            this.fromOrgGroup.TabStop = false;
            this.fromOrgGroup.Text = "From Org";
            // 
            // btnDevSBSeeding
            // 
            this.btnDevSBSeeding.BackColor = System.Drawing.SystemColors.Control;
            this.btnDevSBSeeding.Location = new System.Drawing.Point(1012, 441);
            this.btnDevSBSeeding.Margin = new System.Windows.Forms.Padding(2);
            this.btnDevSBSeeding.Name = "btnDevSBSeeding";
            this.btnDevSBSeeding.Size = new System.Drawing.Size(165, 24);
            this.btnDevSBSeeding.TabIndex = 16;
            this.btnDevSBSeeding.Text = "Open Dev Sandbox Seeding Form";
            this.btnDevSBSeeding.UseVisualStyleBackColor = false;
            this.btnDevSBSeeding.Click += new System.EventHandler(this.btnDevSBSeeding_Click);
            // 
            // btnSobjectFieldInspector
            // 
            this.btnSobjectFieldInspector.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnSobjectFieldInspector.Location = new System.Drawing.Point(821, 244);
            this.btnSobjectFieldInspector.Name = "btnSobjectFieldInspector";
            this.btnSobjectFieldInspector.Size = new System.Drawing.Size(165, 27);
            this.btnSobjectFieldInspector.TabIndex = 12;
            this.btnSobjectFieldInspector.Text = "Sobject Field Inspector";
            this.btnSobjectFieldInspector.UseVisualStyleBackColor = false;
            this.btnSobjectFieldInspector.Click += new System.EventHandler(this.btnSobjectFieldInspector_Click);
            // 
            // btnFromGenerateToolingChangeReport
            // 
            this.btnFromGenerateToolingChangeReport.Location = new System.Drawing.Point(821, 277);
            this.btnFromGenerateToolingChangeReport.Name = "btnFromGenerateToolingChangeReport";
            this.btnFromGenerateToolingChangeReport.Size = new System.Drawing.Size(165, 25);
            this.btnFromGenerateToolingChangeReport.TabIndex = 13;
            this.btnFromGenerateToolingChangeReport.Text = "Generate Tooling Report";
            this.btnFromGenerateToolingChangeReport.UseVisualStyleBackColor = true;
            this.btnFromGenerateToolingChangeReport.Click += new System.EventHandler(this.btnFromGenerateToolingChangeReport_Click);
            // 
            // btnConfigurationWorkbook
            // 
            this.btnConfigurationWorkbook.Location = new System.Drawing.Point(821, 308);
            this.btnConfigurationWorkbook.Name = "btnConfigurationWorkbook";
            this.btnConfigurationWorkbook.Size = new System.Drawing.Size(165, 25);
            this.btnConfigurationWorkbook.TabIndex = 14;
            this.btnConfigurationWorkbook.Text = "Configuration Workbook";
            this.btnConfigurationWorkbook.UseVisualStyleBackColor = true;
            this.btnConfigurationWorkbook.Click += new System.EventHandler(this.btnConfigurationWorkbook_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(311, 188);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(18, 17);
            this.btnSelectAll.TabIndex = 3;
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // lblSelectAll
            // 
            this.lblSelectAll.AutoSize = true;
            this.lblSelectAll.Location = new System.Drawing.Point(335, 192);
            this.lblSelectAll.Name = "lblSelectAll";
            this.lblSelectAll.Size = new System.Drawing.Size(51, 13);
            this.lblSelectAll.TabIndex = 4;
            this.lblSelectAll.Text = "Select All";
            // 
            // btnSelectNone
            // 
            this.btnSelectNone.Location = new System.Drawing.Point(428, 188);
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.Size = new System.Drawing.Size(18, 17);
            this.btnSelectNone.TabIndex = 5;
            this.btnSelectNone.UseVisualStyleBackColor = true;
            this.btnSelectNone.Click += new System.EventHandler(this.btnSelectNone_Click);
            // 
            // lblSelectNone
            // 
            this.lblSelectNone.AutoSize = true;
            this.lblSelectNone.Location = new System.Drawing.Point(452, 192);
            this.lblSelectNone.Name = "lblSelectNone";
            this.lblSelectNone.Size = new System.Drawing.Size(66, 13);
            this.lblSelectNone.TabIndex = 6;
            this.lblSelectNone.Text = "Select None";
            // 
            // btnSelectDefaults
            // 
            this.btnSelectDefaults.Location = new System.Drawing.Point(565, 188);
            this.btnSelectDefaults.Name = "btnSelectDefaults";
            this.btnSelectDefaults.Size = new System.Drawing.Size(18, 17);
            this.btnSelectDefaults.TabIndex = 7;
            this.btnSelectDefaults.UseVisualStyleBackColor = true;
            this.btnSelectDefaults.Click += new System.EventHandler(this.btnSelectDefaults_Click);
            // 
            // lblSelectDefaults
            // 
            this.lblSelectDefaults.AutoSize = true;
            this.lblSelectDefaults.Location = new System.Drawing.Point(589, 192);
            this.lblSelectDefaults.Name = "lblSelectDefaults";
            this.lblSelectDefaults.Size = new System.Drawing.Size(79, 13);
            this.lblSelectDefaults.TabIndex = 8;
            this.lblSelectDefaults.Text = "Select Defaults";
            // 
            // cbAllDebugLogs
            // 
            this.cbAllDebugLogs.AutoSize = true;
            this.cbAllDebugLogs.Location = new System.Drawing.Point(1012, 692);
            this.cbAllDebugLogs.Name = "cbAllDebugLogs";
            this.cbAllDebugLogs.Size = new System.Drawing.Size(132, 17);
            this.cbAllDebugLogs.TabIndex = 17;
            this.cbAllDebugLogs.Text = "Delete All Debug Logs";
            this.cbAllDebugLogs.UseVisualStyleBackColor = true;
            // 
            // SalesforceMetadataStep1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(1221, 733);
            this.Controls.Add(this.cbAllDebugLogs);
            this.Controls.Add(this.lblSelectDefaults);
            this.Controls.Add(this.btnSelectDefaults);
            this.Controls.Add(this.lblSelectNone);
            this.Controls.Add(this.btnSelectNone);
            this.Controls.Add(this.lblSelectAll);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.btnConfigurationWorkbook);
            this.Controls.Add(this.btnFromGenerateToolingChangeReport);
            this.Controls.Add(this.btnSobjectFieldInspector);
            this.Controls.Add(this.btnDevSBSeeding);
            this.Controls.Add(this.fromOrgGroup);
            this.Controls.Add(this.btnDeploy);
            this.Controls.Add(this.btnDeleteDebugLogs);
            this.Controls.Add(this.btnRetrieveMetadata);
            this.Controls.Add(this.lblSalesforce);
            this.Controls.Add(this.lbMetadataTypes);
            this.Controls.Add(this.lblSFMetadataPackage);
            this.Controls.Add(this.btnGetMetadataTypes);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "SalesforceMetadataStep1";
            this.Text = "Salesforce Metadata";
            this.fromOrgGroup.ResumeLayout(false);
            this.fromOrgGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnGetMetadataTypes;
        private System.Windows.Forms.Label lblSFMetadataPackage;
        private System.Windows.Forms.CheckedListBox lbMetadataTypes;
        private System.Windows.Forms.Label lblSFUsername;
        private System.Windows.Forms.Label lblSalesforce;
        private System.Windows.Forms.ComboBox cmbUserName;
        private System.Windows.Forms.Button btnRetrieveMetadata;
        private System.Windows.Forms.Button btnDeleteDebugLogs;
        private System.Windows.Forms.Button btnDeploy;
        private System.Windows.Forms.GroupBox fromOrgGroup;
        private System.Windows.Forms.Button btnDevSBSeeding;
        private System.Windows.Forms.Button btnSobjectFieldInspector;
        private System.Windows.Forms.Button btnFromGenerateToolingChangeReport;
        private System.Windows.Forms.Button btnConfigurationWorkbook;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Label lblSelectAll;
        private System.Windows.Forms.Button btnSelectNone;
        private System.Windows.Forms.Label lblSelectNone;
        private System.Windows.Forms.Button btnSelectDefaults;
        private System.Windows.Forms.Label lblSelectDefaults;
        private System.Windows.Forms.CheckBox cbAllDebugLogs;
    }
}

