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
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbSecurityToken = new System.Windows.Forms.TextBox();
            this.lblSFUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblSecurityToken = new System.Windows.Forms.Label();
            this.lblSalesforce = new System.Windows.Forms.Label();
            this.cmbUserName = new System.Windows.Forms.ComboBox();
            this.btnRetrieveMetadata = new System.Windows.Forms.Button();
            this.btnDeleteDebugLogs = new System.Windows.Forms.Button();
            this.btnDeploy = new System.Windows.Forms.Button();
            this.fromOrgGroup = new System.Windows.Forms.GroupBox();
            this.deployToGroup = new System.Windows.Forms.GroupBox();
            this.lblToOrgSecurityToken = new System.Windows.Forms.Label();
            this.lblToOrgPassword = new System.Windows.Forms.Label();
            this.lblToOrgUsername = new System.Windows.Forms.Label();
            this.tbToOrgSecurityToken = new System.Windows.Forms.TextBox();
            this.tbToOrgPassword = new System.Windows.Forms.TextBox();
            this.cmbToOrgUsername = new System.Windows.Forms.ComboBox();
            this.SectionSplitter = new System.Windows.Forms.TextBox();
            this.btnDevSBSeeding = new System.Windows.Forms.Button();
            this.btnRetrieveMetadataFromDeployTo = new System.Windows.Forms.Button();
            this.btnSobjectFieldInspector = new System.Windows.Forms.Button();
            this.cbSelectAll = new System.Windows.Forms.CheckBox();
            this.cbSelectNone = new System.Windows.Forms.CheckBox();
            this.lbToMetadataTypes = new System.Windows.Forms.CheckedListBox();
            this.btnGetMetadataTypesToOrg = new System.Windows.Forms.Button();
            this.cbToSelectNone = new System.Windows.Forms.CheckBox();
            this.cbToSelectAll = new System.Windows.Forms.CheckBox();
            this.btnFromGenerateToolingChangeReport = new System.Windows.Forms.Button();
            this.btnConfigurationWorkbook = new System.Windows.Forms.Button();
            this.fromOrgGroup.SuspendLayout();
            this.deployToGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGetMetadataTypes
            // 
            this.btnGetMetadataTypes.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnGetMetadataTypes.Location = new System.Drawing.Point(508, 214);
            this.btnGetMetadataTypes.Margin = new System.Windows.Forms.Padding(2);
            this.btnGetMetadataTypes.Name = "btnGetMetadataTypes";
            this.btnGetMetadataTypes.Size = new System.Drawing.Size(165, 25);
            this.btnGetMetadataTypes.TabIndex = 6;
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
            this.lbMetadataTypes.Size = new System.Drawing.Size(451, 454);
            this.lbMetadataTypes.TabIndex = 5;
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(176, 53);
            this.tbPassword.Margin = new System.Windows.Forms.Padding(2);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(215, 20);
            this.tbPassword.TabIndex = 4;
            // 
            // tbSecurityToken
            // 
            this.tbSecurityToken.Location = new System.Drawing.Point(176, 80);
            this.tbSecurityToken.Margin = new System.Windows.Forms.Padding(2);
            this.tbSecurityToken.Name = "tbSecurityToken";
            this.tbSecurityToken.Size = new System.Drawing.Size(215, 20);
            this.tbSecurityToken.TabIndex = 6;
            this.tbSecurityToken.UseSystemPasswordChar = true;
            // 
            // lblSFUsername
            // 
            this.lblSFUsername.AutoSize = true;
            this.lblSFUsername.Location = new System.Drawing.Point(55, 30);
            this.lblSFUsername.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSFUsername.Name = "lblSFUsername";
            this.lblSFUsername.Size = new System.Drawing.Size(104, 13);
            this.lblSFUsername.TabIndex = 1;
            this.lblSFUsername.Text = "Username (from Org)";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(55, 56);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(102, 13);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "Password (from Org)";
            // 
            // lblSecurityToken
            // 
            this.lblSecurityToken.AutoSize = true;
            this.lblSecurityToken.Location = new System.Drawing.Point(31, 84);
            this.lblSecurityToken.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSecurityToken.Name = "lblSecurityToken";
            this.lblSecurityToken.Size = new System.Drawing.Size(128, 13);
            this.lblSecurityToken.TabIndex = 5;
            this.lblSecurityToken.Text = "Security Token (from Org)";
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
            this.cmbUserName.FormattingEnabled = true;
            this.cmbUserName.Location = new System.Drawing.Point(176, 27);
            this.cmbUserName.Margin = new System.Windows.Forms.Padding(2);
            this.cmbUserName.Name = "cmbUserName";
            this.cmbUserName.Size = new System.Drawing.Size(324, 21);
            this.cmbUserName.TabIndex = 2;
            this.cmbUserName.SelectedIndexChanged += new System.EventHandler(this.cmbUserName_SelectedIndexChanged);
            // 
            // btnRetrieveMetadata
            // 
            this.btnRetrieveMetadata.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnRetrieveMetadata.Location = new System.Drawing.Point(508, 248);
            this.btnRetrieveMetadata.Margin = new System.Windows.Forms.Padding(2);
            this.btnRetrieveMetadata.Name = "btnRetrieveMetadata";
            this.btnRetrieveMetadata.Size = new System.Drawing.Size(165, 25);
            this.btnRetrieveMetadata.TabIndex = 7;
            this.btnRetrieveMetadata.Text = "Retrieve Metadata Package";
            this.btnRetrieveMetadata.UseVisualStyleBackColor = false;
            this.btnRetrieveMetadata.Click += new System.EventHandler(this.btnRetrieveMetadata_Click);
            // 
            // btnDeleteDebugLogs
            // 
            this.btnDeleteDebugLogs.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnDeleteDebugLogs.Location = new System.Drawing.Point(508, 638);
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
            this.btnDeploy.Location = new System.Drawing.Point(1221, 698);
            this.btnDeploy.Name = "btnDeploy";
            this.btnDeploy.Size = new System.Drawing.Size(164, 23);
            this.btnDeploy.TabIndex = 20;
            this.btnDeploy.Text = "Deploy";
            this.btnDeploy.UseVisualStyleBackColor = false;
            this.btnDeploy.Click += new System.EventHandler(this.btnDeploy_Click);
            // 
            // fromOrgGroup
            // 
            this.fromOrgGroup.BackColor = System.Drawing.SystemColors.Control;
            this.fromOrgGroup.Controls.Add(this.lblSFUsername);
            this.fromOrgGroup.Controls.Add(this.tbPassword);
            this.fromOrgGroup.Controls.Add(this.tbSecurityToken);
            this.fromOrgGroup.Controls.Add(this.lblPassword);
            this.fromOrgGroup.Controls.Add(this.lblSecurityToken);
            this.fromOrgGroup.Controls.Add(this.cmbUserName);
            this.fromOrgGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fromOrgGroup.Location = new System.Drawing.Point(26, 45);
            this.fromOrgGroup.Name = "fromOrgGroup";
            this.fromOrgGroup.Size = new System.Drawing.Size(523, 115);
            this.fromOrgGroup.TabIndex = 1;
            this.fromOrgGroup.TabStop = false;
            this.fromOrgGroup.Text = "From Org";
            // 
            // deployToGroup
            // 
            this.deployToGroup.BackColor = System.Drawing.SystemColors.Control;
            this.deployToGroup.Controls.Add(this.lblToOrgSecurityToken);
            this.deployToGroup.Controls.Add(this.lblToOrgPassword);
            this.deployToGroup.Controls.Add(this.lblToOrgUsername);
            this.deployToGroup.Controls.Add(this.tbToOrgSecurityToken);
            this.deployToGroup.Controls.Add(this.tbToOrgPassword);
            this.deployToGroup.Controls.Add(this.cmbToOrgUsername);
            this.deployToGroup.Location = new System.Drawing.Point(745, 45);
            this.deployToGroup.Name = "deployToGroup";
            this.deployToGroup.Size = new System.Drawing.Size(523, 115);
            this.deployToGroup.TabIndex = 13;
            this.deployToGroup.TabStop = false;
            this.deployToGroup.Text = "Deploy To ";
            // 
            // lblToOrgSecurityToken
            // 
            this.lblToOrgSecurityToken.AutoSize = true;
            this.lblToOrgSecurityToken.Location = new System.Drawing.Point(41, 83);
            this.lblToOrgSecurityToken.Name = "lblToOrgSecurityToken";
            this.lblToOrgSecurityToken.Size = new System.Drawing.Size(117, 13);
            this.lblToOrgSecurityToken.TabIndex = 4;
            this.lblToOrgSecurityToken.Text = "Security Token (to Org)";
            // 
            // lblToOrgPassword
            // 
            this.lblToOrgPassword.AutoSize = true;
            this.lblToOrgPassword.Location = new System.Drawing.Point(67, 56);
            this.lblToOrgPassword.Name = "lblToOrgPassword";
            this.lblToOrgPassword.Size = new System.Drawing.Size(91, 13);
            this.lblToOrgPassword.TabIndex = 2;
            this.lblToOrgPassword.Text = "Password (to Org)";
            // 
            // lblToOrgUsername
            // 
            this.lblToOrgUsername.AutoSize = true;
            this.lblToOrgUsername.Location = new System.Drawing.Point(67, 33);
            this.lblToOrgUsername.Name = "lblToOrgUsername";
            this.lblToOrgUsername.Size = new System.Drawing.Size(93, 13);
            this.lblToOrgUsername.TabIndex = 0;
            this.lblToOrgUsername.Text = "Username (to Org)";
            this.lblToOrgUsername.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tbToOrgSecurityToken
            // 
            this.tbToOrgSecurityToken.Location = new System.Drawing.Point(190, 80);
            this.tbToOrgSecurityToken.Margin = new System.Windows.Forms.Padding(2);
            this.tbToOrgSecurityToken.Name = "tbToOrgSecurityToken";
            this.tbToOrgSecurityToken.Size = new System.Drawing.Size(215, 20);
            this.tbToOrgSecurityToken.TabIndex = 5;
            this.tbToOrgSecurityToken.UseSystemPasswordChar = true;
            // 
            // tbToOrgPassword
            // 
            this.tbToOrgPassword.Location = new System.Drawing.Point(190, 56);
            this.tbToOrgPassword.Margin = new System.Windows.Forms.Padding(2);
            this.tbToOrgPassword.Name = "tbToOrgPassword";
            this.tbToOrgPassword.PasswordChar = '*';
            this.tbToOrgPassword.Size = new System.Drawing.Size(215, 20);
            this.tbToOrgPassword.TabIndex = 3;
            // 
            // cmbToOrgUsername
            // 
            this.cmbToOrgUsername.DropDownWidth = 325;
            this.cmbToOrgUsername.FormattingEnabled = true;
            this.cmbToOrgUsername.Location = new System.Drawing.Point(190, 30);
            this.cmbToOrgUsername.Margin = new System.Windows.Forms.Padding(2);
            this.cmbToOrgUsername.Name = "cmbToOrgUsername";
            this.cmbToOrgUsername.Size = new System.Drawing.Size(300, 21);
            this.cmbToOrgUsername.TabIndex = 1;
            this.cmbToOrgUsername.SelectedIndexChanged += new System.EventHandler(this.cmbToOrgUsername_SelectedIndexChanged);
            // 
            // SectionSplitter
            // 
            this.SectionSplitter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.SectionSplitter.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.SectionSplitter.Enabled = false;
            this.SectionSplitter.Location = new System.Drawing.Point(696, -4);
            this.SectionSplitter.Multiline = true;
            this.SectionSplitter.Name = "SectionSplitter";
            this.SectionSplitter.Size = new System.Drawing.Size(22, 742);
            this.SectionSplitter.TabIndex = 12;
            // 
            // btnDevSBSeeding
            // 
            this.btnDevSBSeeding.BackColor = System.Drawing.SystemColors.Control;
            this.btnDevSBSeeding.Location = new System.Drawing.Point(1222, 637);
            this.btnDevSBSeeding.Margin = new System.Windows.Forms.Padding(2);
            this.btnDevSBSeeding.Name = "btnDevSBSeeding";
            this.btnDevSBSeeding.Size = new System.Drawing.Size(165, 28);
            this.btnDevSBSeeding.TabIndex = 19;
            this.btnDevSBSeeding.Text = "Open Dev Sandbox Seeding Form";
            this.btnDevSBSeeding.UseVisualStyleBackColor = false;
            this.btnDevSBSeeding.Click += new System.EventHandler(this.btnDevSBSeeding_Click);
            // 
            // btnRetrieveMetadataFromDeployTo
            // 
            this.btnRetrieveMetadataFromDeployTo.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnRetrieveMetadataFromDeployTo.Location = new System.Drawing.Point(1220, 242);
            this.btnRetrieveMetadataFromDeployTo.Margin = new System.Windows.Forms.Padding(2);
            this.btnRetrieveMetadataFromDeployTo.Name = "btnRetrieveMetadataFromDeployTo";
            this.btnRetrieveMetadataFromDeployTo.Size = new System.Drawing.Size(165, 24);
            this.btnRetrieveMetadataFromDeployTo.TabIndex = 18;
            this.btnRetrieveMetadataFromDeployTo.Text = "Retrieve Metadata Package";
            this.btnRetrieveMetadataFromDeployTo.UseVisualStyleBackColor = false;
            this.btnRetrieveMetadataFromDeployTo.Click += new System.EventHandler(this.btnRetrieveMetadataFromDeployTo_Click);
            // 
            // btnSobjectFieldInspector
            // 
            this.btnSobjectFieldInspector.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnSobjectFieldInspector.Location = new System.Drawing.Point(508, 596);
            this.btnSobjectFieldInspector.Name = "btnSobjectFieldInspector";
            this.btnSobjectFieldInspector.Size = new System.Drawing.Size(164, 27);
            this.btnSobjectFieldInspector.TabIndex = 10;
            this.btnSobjectFieldInspector.Text = "Sobject Field Inspector";
            this.btnSobjectFieldInspector.UseVisualStyleBackColor = false;
            this.btnSobjectFieldInspector.Click += new System.EventHandler(this.btnSobjectFieldInspector_Click);
            // 
            // cbSelectAll
            // 
            this.cbSelectAll.AutoSize = true;
            this.cbSelectAll.Location = new System.Drawing.Point(296, 188);
            this.cbSelectAll.Name = "cbSelectAll";
            this.cbSelectAll.Size = new System.Drawing.Size(70, 17);
            this.cbSelectAll.TabIndex = 3;
            this.cbSelectAll.Text = "Select All";
            this.cbSelectAll.UseVisualStyleBackColor = true;
            this.cbSelectAll.CheckedChanged += new System.EventHandler(this.cbSelectAll_CheckedChanged);
            // 
            // cbSelectNone
            // 
            this.cbSelectNone.AutoSize = true;
            this.cbSelectNone.Location = new System.Drawing.Point(392, 188);
            this.cbSelectNone.Name = "cbSelectNone";
            this.cbSelectNone.Size = new System.Drawing.Size(85, 17);
            this.cbSelectNone.TabIndex = 4;
            this.cbSelectNone.Text = "Select None";
            this.cbSelectNone.UseVisualStyleBackColor = true;
            this.cbSelectNone.CheckedChanged += new System.EventHandler(this.cbSelectNone_CheckedChanged);
            // 
            // lbToMetadataTypes
            // 
            this.lbToMetadataTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbToMetadataTypes.FormattingEnabled = true;
            this.lbToMetadataTypes.HorizontalScrollbar = true;
            this.lbToMetadataTypes.Location = new System.Drawing.Point(745, 214);
            this.lbToMetadataTypes.Margin = new System.Windows.Forms.Padding(2);
            this.lbToMetadataTypes.Name = "lbToMetadataTypes";
            this.lbToMetadataTypes.Size = new System.Drawing.Size(451, 454);
            this.lbToMetadataTypes.TabIndex = 16;
            // 
            // btnGetMetadataTypesToOrg
            // 
            this.btnGetMetadataTypesToOrg.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnGetMetadataTypesToOrg.Location = new System.Drawing.Point(1220, 214);
            this.btnGetMetadataTypesToOrg.Margin = new System.Windows.Forms.Padding(2);
            this.btnGetMetadataTypesToOrg.Name = "btnGetMetadataTypesToOrg";
            this.btnGetMetadataTypesToOrg.Size = new System.Drawing.Size(165, 24);
            this.btnGetMetadataTypesToOrg.TabIndex = 17;
            this.btnGetMetadataTypesToOrg.Text = "Get Metadata Types";
            this.btnGetMetadataTypesToOrg.UseVisualStyleBackColor = false;
            this.btnGetMetadataTypesToOrg.Click += new System.EventHandler(this.btnGetMetadataTypesToOrg_Click);
            // 
            // cbToSelectNone
            // 
            this.cbToSelectNone.AutoSize = true;
            this.cbToSelectNone.Location = new System.Drawing.Point(1103, 192);
            this.cbToSelectNone.Name = "cbToSelectNone";
            this.cbToSelectNone.Size = new System.Drawing.Size(85, 17);
            this.cbToSelectNone.TabIndex = 15;
            this.cbToSelectNone.Text = "Select None";
            this.cbToSelectNone.UseVisualStyleBackColor = true;
            this.cbToSelectNone.CheckedChanged += new System.EventHandler(this.cbToSelectNone_CheckedChanged);
            // 
            // cbToSelectAll
            // 
            this.cbToSelectAll.AutoSize = true;
            this.cbToSelectAll.Location = new System.Drawing.Point(1007, 192);
            this.cbToSelectAll.Name = "cbToSelectAll";
            this.cbToSelectAll.Size = new System.Drawing.Size(70, 17);
            this.cbToSelectAll.TabIndex = 14;
            this.cbToSelectAll.Text = "Select All";
            this.cbToSelectAll.UseVisualStyleBackColor = true;
            this.cbToSelectAll.CheckedChanged += new System.EventHandler(this.cbToSelectAll_CheckedChanged);
            // 
            // btnFromGenerateToolingChangeReport
            // 
            this.btnFromGenerateToolingChangeReport.Location = new System.Drawing.Point(507, 305);
            this.btnFromGenerateToolingChangeReport.Name = "btnFromGenerateToolingChangeReport";
            this.btnFromGenerateToolingChangeReport.Size = new System.Drawing.Size(165, 25);
            this.btnFromGenerateToolingChangeReport.TabIndex = 8;
            this.btnFromGenerateToolingChangeReport.Text = "Generate Tooling Report";
            this.btnFromGenerateToolingChangeReport.UseVisualStyleBackColor = true;
            this.btnFromGenerateToolingChangeReport.Click += new System.EventHandler(this.btnFromGenerateToolingChangeReport_Click);
            // 
            // btnConfigurationWorkbook
            // 
            this.btnConfigurationWorkbook.Location = new System.Drawing.Point(508, 336);
            this.btnConfigurationWorkbook.Name = "btnConfigurationWorkbook";
            this.btnConfigurationWorkbook.Size = new System.Drawing.Size(165, 25);
            this.btnConfigurationWorkbook.TabIndex = 9;
            this.btnConfigurationWorkbook.Text = "Configuration Workbook";
            this.btnConfigurationWorkbook.UseVisualStyleBackColor = true;
            this.btnConfigurationWorkbook.Click += new System.EventHandler(this.btnConfigurationWorkbook_Click);
            // 
            // SalesforceMetadataStep1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(1397, 733);
            this.Controls.Add(this.btnConfigurationWorkbook);
            this.Controls.Add(this.btnFromGenerateToolingChangeReport);
            this.Controls.Add(this.cbToSelectNone);
            this.Controls.Add(this.cbToSelectAll);
            this.Controls.Add(this.btnGetMetadataTypesToOrg);
            this.Controls.Add(this.lbToMetadataTypes);
            this.Controls.Add(this.cbSelectNone);
            this.Controls.Add(this.cbSelectAll);
            this.Controls.Add(this.btnSobjectFieldInspector);
            this.Controls.Add(this.btnRetrieveMetadataFromDeployTo);
            this.Controls.Add(this.btnDevSBSeeding);
            this.Controls.Add(this.SectionSplitter);
            this.Controls.Add(this.deployToGroup);
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
            this.deployToGroup.ResumeLayout(false);
            this.deployToGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnGetMetadataTypes;
        private System.Windows.Forms.Label lblSFMetadataPackage;
        private System.Windows.Forms.CheckedListBox lbMetadataTypes;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbSecurityToken;
        private System.Windows.Forms.Label lblSFUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblSecurityToken;
        private System.Windows.Forms.Label lblSalesforce;
        private System.Windows.Forms.ComboBox cmbUserName;
        private System.Windows.Forms.Button btnRetrieveMetadata;
        private System.Windows.Forms.Button btnDeleteDebugLogs;
        private System.Windows.Forms.Button btnDeploy;
        private System.Windows.Forms.GroupBox fromOrgGroup;
        private System.Windows.Forms.GroupBox deployToGroup;
        private System.Windows.Forms.Label lblToOrgSecurityToken;
        private System.Windows.Forms.Label lblToOrgPassword;
        private System.Windows.Forms.Label lblToOrgUsername;
        private System.Windows.Forms.TextBox tbToOrgSecurityToken;
        private System.Windows.Forms.TextBox tbToOrgPassword;
        private System.Windows.Forms.ComboBox cmbToOrgUsername;
        private System.Windows.Forms.TextBox SectionSplitter;
        private System.Windows.Forms.Button btnDevSBSeeding;
        private System.Windows.Forms.Button btnRetrieveMetadataFromDeployTo;
        private System.Windows.Forms.Button btnSobjectFieldInspector;
        private System.Windows.Forms.CheckBox cbSelectAll;
        private System.Windows.Forms.CheckBox cbSelectNone;
        private System.Windows.Forms.CheckedListBox lbToMetadataTypes;
        private System.Windows.Forms.Button btnGetMetadataTypesToOrg;
        private System.Windows.Forms.CheckBox cbToSelectNone;
        private System.Windows.Forms.CheckBox cbToSelectAll;
        private System.Windows.Forms.Button btnFromGenerateToolingChangeReport;
        private System.Windows.Forms.Button btnConfigurationWorkbook;
    }
}

