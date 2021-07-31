namespace SalesforceMetadata
{
    partial class ConfigurationWorkbook
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationWorkbook));
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbSecurityToken = new System.Windows.Forms.TextBox();
            this.lblSFUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblSecurityToken = new System.Windows.Forms.Label();
            this.lblSalesforce = new System.Windows.Forms.Label();
            this.cmbUserName = new System.Windows.Forms.ComboBox();
            this.fromOrgGroup = new System.Windows.Forms.GroupBox();
            this.tbSelectedFolder = new System.Windows.Forms.TextBox();
            this.lblFolderLocation = new System.Windows.Forms.Label();
            this.lblSaveResultsTo = new System.Windows.Forms.Label();
            this.tbSaveResultsTo = new System.Windows.Forms.TextBox();
            this.btnGenerateConfigReportAsCSV = new System.Windows.Forms.Button();
            this.linkUnsupportedMetadataTypes = new System.Windows.Forms.LinkLabel();
            this.lblUnsupportedMetadataTypes = new System.Windows.Forms.Label();
            this.generateConfigReportExcel = new System.Windows.Forms.Button();
            this.cwProgressBar = new System.Windows.Forms.ProgressBar();
            this.lblProgressIndicator = new System.Windows.Forms.Label();
            this.chkListBoxTasks = new System.Windows.Forms.CheckedListBox();
            this.btnObjectAutomationMap = new System.Windows.Forms.Button();
            this.fromOrgGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(176, 56);
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
            this.fromOrgGroup.Location = new System.Drawing.Point(10, 12);
            this.fromOrgGroup.Name = "fromOrgGroup";
            this.fromOrgGroup.Size = new System.Drawing.Size(523, 115);
            this.fromOrgGroup.TabIndex = 0;
            this.fromOrgGroup.TabStop = false;
            this.fromOrgGroup.Text = "From Org";
            // 
            // tbSelectedFolder
            // 
            this.tbSelectedFolder.Location = new System.Drawing.Point(138, 150);
            this.tbSelectedFolder.Margin = new System.Windows.Forms.Padding(2);
            this.tbSelectedFolder.Name = "tbSelectedFolder";
            this.tbSelectedFolder.Size = new System.Drawing.Size(624, 20);
            this.tbSelectedFolder.TabIndex = 2;
            this.tbSelectedFolder.DoubleClick += new System.EventHandler(this.tbSelectedFolder_DoubleClick);
            // 
            // lblFolderLocation
            // 
            this.lblFolderLocation.AutoSize = true;
            this.lblFolderLocation.Location = new System.Drawing.Point(7, 153);
            this.lblFolderLocation.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFolderLocation.Name = "lblFolderLocation";
            this.lblFolderLocation.Size = new System.Drawing.Size(127, 13);
            this.lblFolderLocation.TabIndex = 1;
            this.lblFolderLocation.Text = "Selected Folder To Parse";
            // 
            // lblSaveResultsTo
            // 
            this.lblSaveResultsTo.AutoSize = true;
            this.lblSaveResultsTo.Location = new System.Drawing.Point(7, 181);
            this.lblSaveResultsTo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSaveResultsTo.Name = "lblSaveResultsTo";
            this.lblSaveResultsTo.Size = new System.Drawing.Size(86, 13);
            this.lblSaveResultsTo.TabIndex = 3;
            this.lblSaveResultsTo.Text = "Save Results To";
            // 
            // tbSaveResultsTo
            // 
            this.tbSaveResultsTo.Location = new System.Drawing.Point(138, 178);
            this.tbSaveResultsTo.Margin = new System.Windows.Forms.Padding(2);
            this.tbSaveResultsTo.Name = "tbSaveResultsTo";
            this.tbSaveResultsTo.Size = new System.Drawing.Size(624, 20);
            this.tbSaveResultsTo.TabIndex = 4;
            this.tbSaveResultsTo.DoubleClick += new System.EventHandler(this.tbSaveResultsTo_DoubleClick);
            // 
            // btnGenerateConfigReportAsCSV
            // 
            this.btnGenerateConfigReportAsCSV.Location = new System.Drawing.Point(10, 248);
            this.btnGenerateConfigReportAsCSV.Name = "btnGenerateConfigReportAsCSV";
            this.btnGenerateConfigReportAsCSV.Size = new System.Drawing.Size(203, 28);
            this.btnGenerateConfigReportAsCSV.TabIndex = 5;
            this.btnGenerateConfigReportAsCSV.Text = "Save Configuration Workbook To CSV";
            this.btnGenerateConfigReportAsCSV.UseVisualStyleBackColor = true;
            this.btnGenerateConfigReportAsCSV.Click += new System.EventHandler(this.btnGenerateConfigReportAsCSV_Click);
            // 
            // linkUnsupportedMetadataTypes
            // 
            this.linkUnsupportedMetadataTypes.AutoSize = true;
            this.linkUnsupportedMetadataTypes.Location = new System.Drawing.Point(246, 341);
            this.linkUnsupportedMetadataTypes.Name = "linkUnsupportedMetadataTypes";
            this.linkUnsupportedMetadataTypes.Size = new System.Drawing.Size(512, 13);
            this.linkUnsupportedMetadataTypes.TabIndex = 9;
            this.linkUnsupportedMetadataTypes.TabStop = true;
            this.linkUnsupportedMetadataTypes.Text = "https://developer.salesforce.com/docs/atlas.en-us.api_meta.meta/api_meta/meta_uns" +
    "upported_types.htm";
            // 
            // lblUnsupportedMetadataTypes
            // 
            this.lblUnsupportedMetadataTypes.AutoSize = true;
            this.lblUnsupportedMetadataTypes.Location = new System.Drawing.Point(10, 341);
            this.lblUnsupportedMetadataTypes.Name = "lblUnsupportedMetadataTypes";
            this.lblUnsupportedMetadataTypes.Size = new System.Drawing.Size(201, 13);
            this.lblUnsupportedMetadataTypes.TabIndex = 8;
            this.lblUnsupportedMetadataTypes.Text = "Salesforce Metadata Unsupported Types";
            // 
            // generateConfigReportExcel
            // 
            this.generateConfigReportExcel.Location = new System.Drawing.Point(228, 248);
            this.generateConfigReportExcel.Name = "generateConfigReportExcel";
            this.generateConfigReportExcel.Size = new System.Drawing.Size(203, 28);
            this.generateConfigReportExcel.TabIndex = 6;
            this.generateConfigReportExcel.Text = "Generate Config Workbook in Excel";
            this.generateConfigReportExcel.UseVisualStyleBackColor = true;
            this.generateConfigReportExcel.Click += new System.EventHandler(this.btnGenerateConfigReport_Excel_Click);
            // 
            // cwProgressBar
            // 
            this.cwProgressBar.Location = new System.Drawing.Point(10, 380);
            this.cwProgressBar.Name = "cwProgressBar";
            this.cwProgressBar.Size = new System.Drawing.Size(741, 23);
            this.cwProgressBar.TabIndex = 11;
            // 
            // lblProgressIndicator
            // 
            this.lblProgressIndicator.AutoSize = true;
            this.lblProgressIndicator.Location = new System.Drawing.Point(10, 364);
            this.lblProgressIndicator.Name = "lblProgressIndicator";
            this.lblProgressIndicator.Size = new System.Drawing.Size(89, 13);
            this.lblProgressIndicator.TabIndex = 10;
            this.lblProgressIndicator.Text = "ProgressIndicator";
            // 
            // chkListBoxTasks
            // 
            this.chkListBoxTasks.Enabled = false;
            this.chkListBoxTasks.FormattingEnabled = true;
            this.chkListBoxTasks.Location = new System.Drawing.Point(10, 418);
            this.chkListBoxTasks.Margin = new System.Windows.Forms.Padding(2);
            this.chkListBoxTasks.Name = "chkListBoxTasks";
            this.chkListBoxTasks.Size = new System.Drawing.Size(301, 79);
            this.chkListBoxTasks.TabIndex = 12;
            // 
            // btnObjectAutomationMap
            // 
            this.btnObjectAutomationMap.Location = new System.Drawing.Point(447, 248);
            this.btnObjectAutomationMap.Name = "btnObjectAutomationMap";
            this.btnObjectAutomationMap.Size = new System.Drawing.Size(203, 28);
            this.btnObjectAutomationMap.TabIndex = 7;
            this.btnObjectAutomationMap.Text = "Generate Object Automation Map";
            this.btnObjectAutomationMap.UseVisualStyleBackColor = true;
            this.btnObjectAutomationMap.Click += new System.EventHandler(this.btnObjectAutomationMap_Click);
            // 
            // ConfigurationWorkbook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(927, 553);
            this.Controls.Add(this.btnObjectAutomationMap);
            this.Controls.Add(this.chkListBoxTasks);
            this.Controls.Add(this.lblProgressIndicator);
            this.Controls.Add(this.cwProgressBar);
            this.Controls.Add(this.fromOrgGroup);
            this.Controls.Add(this.generateConfigReportExcel);
            this.Controls.Add(this.lblUnsupportedMetadataTypes);
            this.Controls.Add(this.linkUnsupportedMetadataTypes);
            this.Controls.Add(this.btnGenerateConfigReportAsCSV);
            this.Controls.Add(this.tbSaveResultsTo);
            this.Controls.Add(this.lblSaveResultsTo);
            this.Controls.Add(this.lblFolderLocation);
            this.Controls.Add(this.tbSelectedFolder);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ConfigurationWorkbook";
            this.Text = "Generate Configuration Workbook";
            this.fromOrgGroup.ResumeLayout(false);
            this.fromOrgGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox tbSelectedFolder;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbSecurityToken;
        private System.Windows.Forms.Label lblSFUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblSecurityToken;
        private System.Windows.Forms.Label lblSalesforce;
        private System.Windows.Forms.ComboBox cmbUserName;
        private System.Windows.Forms.GroupBox fromOrgGroup;
        private System.Windows.Forms.Label lblFolderLocation;
        private System.Windows.Forms.Label lblSaveResultsTo;
        private System.Windows.Forms.TextBox tbSaveResultsTo;
        private System.Windows.Forms.Button btnGenerateConfigReportAsCSV;
        private System.Windows.Forms.LinkLabel linkUnsupportedMetadataTypes;
        private System.Windows.Forms.Label lblUnsupportedMetadataTypes;
        private System.Windows.Forms.Button generateConfigReportExcel;
        private System.Windows.Forms.ProgressBar cwProgressBar;
        private System.Windows.Forms.Label lblProgressIndicator;
        private System.Windows.Forms.CheckedListBox chkListBoxTasks;
        private System.Windows.Forms.Button btnObjectAutomationMap;
    }
}