namespace SalesforceMetadata
{
    partial class DeployMetadata
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeployMetadata));
            this.lblRunSpecificTests = new System.Windows.Forms.Label();
            this.tbTestsToRun = new System.Windows.Forms.TextBox();
            this.tbDeploymentName = new System.Windows.Forms.TextBox();
            this.lblDeploymentName = new System.Windows.Forms.Label();
            this.cbCheckOnly = new System.Windows.Forms.CheckBox();
            this.btnDeployMetadata = new System.Windows.Forms.Button();
            this.lblZipFileLocation = new System.Windows.Forms.Label();
            this.tbZipFileLocation = new System.Windows.Forms.TextBox();
            this.rtMessages = new System.Windows.Forms.RichTextBox();
            this.lblDeploymentMessage = new System.Windows.Forms.Label();
            this.cbPurgeOnDelete = new System.Windows.Forms.CheckBox();
            this.lblDeploymentID = new System.Windows.Forms.Label();
            this.tbDeploymentValidationId = new System.Windows.Forms.TextBox();
            this.cbRunTests = new System.Windows.Forms.CheckBox();
            this.fromOrgGroup = new System.Windows.Forms.GroupBox();
            this.lblSFUsername = new System.Windows.Forms.Label();
            this.cmbUserName = new System.Windows.Forms.ComboBox();
            this.lblSalesforce = new System.Windows.Forms.Label();
            this.fromOrgGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblRunSpecificTests
            // 
            this.lblRunSpecificTests.AutoSize = true;
            this.lblRunSpecificTests.Location = new System.Drawing.Point(79, 152);
            this.lblRunSpecificTests.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRunSpecificTests.Name = "lblRunSpecificTests";
            this.lblRunSpecificTests.Size = new System.Drawing.Size(97, 13);
            this.lblRunSpecificTests.TabIndex = 6;
            this.lblRunSpecificTests.Text = "Run Specific Tests";
            // 
            // tbTestsToRun
            // 
            this.tbTestsToRun.AcceptsReturn = true;
            this.tbTestsToRun.Location = new System.Drawing.Point(205, 152);
            this.tbTestsToRun.Margin = new System.Windows.Forms.Padding(2);
            this.tbTestsToRun.Multiline = true;
            this.tbTestsToRun.Name = "tbTestsToRun";
            this.tbTestsToRun.Size = new System.Drawing.Size(373, 132);
            this.tbTestsToRun.TabIndex = 7;
            // 
            // tbDeploymentName
            // 
            this.tbDeploymentName.Location = new System.Drawing.Point(206, 43);
            this.tbDeploymentName.Margin = new System.Windows.Forms.Padding(2);
            this.tbDeploymentName.Name = "tbDeploymentName";
            this.tbDeploymentName.Size = new System.Drawing.Size(497, 20);
            this.tbDeploymentName.TabIndex = 1;
            // 
            // lblDeploymentName
            // 
            this.lblDeploymentName.AutoSize = true;
            this.lblDeploymentName.Location = new System.Drawing.Point(82, 46);
            this.lblDeploymentName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDeploymentName.Name = "lblDeploymentName";
            this.lblDeploymentName.Size = new System.Drawing.Size(94, 13);
            this.lblDeploymentName.TabIndex = 0;
            this.lblDeploymentName.Text = "Deployment Name";
            // 
            // cbCheckOnly
            // 
            this.cbCheckOnly.AutoSize = true;
            this.cbCheckOnly.Checked = true;
            this.cbCheckOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCheckOnly.Location = new System.Drawing.Point(637, 155);
            this.cbCheckOnly.Margin = new System.Windows.Forms.Padding(2);
            this.cbCheckOnly.Name = "cbCheckOnly";
            this.cbCheckOnly.Size = new System.Drawing.Size(160, 17);
            this.cbCheckOnly.TabIndex = 8;
            this.cbCheckOnly.Text = "Check Only - Do Not Deploy";
            this.cbCheckOnly.UseVisualStyleBackColor = true;
            // 
            // btnDeployMetadata
            // 
            this.btnDeployMetadata.Location = new System.Drawing.Point(619, 260);
            this.btnDeployMetadata.Margin = new System.Windows.Forms.Padding(2);
            this.btnDeployMetadata.Name = "btnDeployMetadata";
            this.btnDeployMetadata.Size = new System.Drawing.Size(193, 24);
            this.btnDeployMetadata.TabIndex = 12;
            this.btnDeployMetadata.Text = "Deploy Metadata Package";
            this.btnDeployMetadata.UseVisualStyleBackColor = true;
            this.btnDeployMetadata.Click += new System.EventHandler(this.btnDeployMetadata_Click);
            // 
            // lblZipFileLocation
            // 
            this.lblZipFileLocation.AutoSize = true;
            this.lblZipFileLocation.Location = new System.Drawing.Point(26, 69);
            this.lblZipFileLocation.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblZipFileLocation.Name = "lblZipFileLocation";
            this.lblZipFileLocation.Size = new System.Drawing.Size(150, 13);
            this.lblZipFileLocation.TabIndex = 2;
            this.lblZipFileLocation.Text = "Deploy Zip File From Location:";
            // 
            // tbZipFileLocation
            // 
            this.tbZipFileLocation.Location = new System.Drawing.Point(205, 69);
            this.tbZipFileLocation.Margin = new System.Windows.Forms.Padding(2);
            this.tbZipFileLocation.Name = "tbZipFileLocation";
            this.tbZipFileLocation.Size = new System.Drawing.Size(497, 20);
            this.tbZipFileLocation.TabIndex = 3;
            this.tbZipFileLocation.DoubleClick += new System.EventHandler(this.tbZipFileLocation_DoubleClick);
            // 
            // rtMessages
            // 
            this.rtMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtMessages.Location = new System.Drawing.Point(12, 325);
            this.rtMessages.Name = "rtMessages";
            this.rtMessages.Size = new System.Drawing.Size(1349, 357);
            this.rtMessages.TabIndex = 14;
            this.rtMessages.Text = "";
            // 
            // lblDeploymentMessage
            // 
            this.lblDeploymentMessage.AutoSize = true;
            this.lblDeploymentMessage.Location = new System.Drawing.Point(12, 309);
            this.lblDeploymentMessage.Name = "lblDeploymentMessage";
            this.lblDeploymentMessage.Size = new System.Drawing.Size(109, 13);
            this.lblDeploymentMessage.TabIndex = 13;
            this.lblDeploymentMessage.Text = "Deployment Message";
            // 
            // cbPurgeOnDelete
            // 
            this.cbPurgeOnDelete.AutoSize = true;
            this.cbPurgeOnDelete.Location = new System.Drawing.Point(637, 201);
            this.cbPurgeOnDelete.Name = "cbPurgeOnDelete";
            this.cbPurgeOnDelete.Size = new System.Drawing.Size(103, 17);
            this.cbPurgeOnDelete.TabIndex = 10;
            this.cbPurgeOnDelete.Text = "Purge on Delete";
            this.cbPurgeOnDelete.UseVisualStyleBackColor = true;
            // 
            // lblDeploymentID
            // 
            this.lblDeploymentID.AutoSize = true;
            this.lblDeploymentID.Location = new System.Drawing.Point(50, 98);
            this.lblDeploymentID.Name = "lblDeploymentID";
            this.lblDeploymentID.Size = new System.Drawing.Size(126, 13);
            this.lblDeploymentID.TabIndex = 4;
            this.lblDeploymentID.Text = "Deployment Validation ID";
            // 
            // tbDeploymentValidationId
            // 
            this.tbDeploymentValidationId.Location = new System.Drawing.Point(206, 98);
            this.tbDeploymentValidationId.Name = "tbDeploymentValidationId";
            this.tbDeploymentValidationId.Size = new System.Drawing.Size(496, 20);
            this.tbDeploymentValidationId.TabIndex = 5;
            // 
            // cbRunTests
            // 
            this.cbRunTests.AutoSize = true;
            this.cbRunTests.Location = new System.Drawing.Point(637, 178);
            this.cbRunTests.Name = "cbRunTests";
            this.cbRunTests.Size = new System.Drawing.Size(104, 17);
            this.cbRunTests.TabIndex = 9;
            this.cbRunTests.Text = "Run Local Tests";
            this.cbRunTests.UseVisualStyleBackColor = true;
            // 
            // fromOrgGroup
            // 
            this.fromOrgGroup.BackColor = System.Drawing.SystemColors.Control;
            this.fromOrgGroup.Controls.Add(this.lblSFUsername);
            this.fromOrgGroup.Controls.Add(this.cmbUserName);
            this.fromOrgGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fromOrgGroup.Location = new System.Drawing.Point(838, 41);
            this.fromOrgGroup.Name = "fromOrgGroup";
            this.fromOrgGroup.Size = new System.Drawing.Size(523, 77);
            this.fromOrgGroup.TabIndex = 11;
            this.fromOrgGroup.TabStop = false;
            this.fromOrgGroup.Text = "From Org";
            // 
            // lblSFUsername
            // 
            this.lblSFUsername.AutoSize = true;
            this.lblSFUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSFUsername.Location = new System.Drawing.Point(14, 30);
            this.lblSFUsername.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSFUsername.Name = "lblSFUsername";
            this.lblSFUsername.Size = new System.Drawing.Size(143, 17);
            this.lblSFUsername.TabIndex = 1;
            this.lblSFUsername.Text = "Username (from Org)";
            // 
            // cmbUserName
            // 
            this.cmbUserName.DropDownWidth = 325;
            this.cmbUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbUserName.FormattingEnabled = true;
            this.cmbUserName.Location = new System.Drawing.Point(176, 27);
            this.cmbUserName.Margin = new System.Windows.Forms.Padding(2);
            this.cmbUserName.Name = "cmbUserName";
            this.cmbUserName.Size = new System.Drawing.Size(324, 24);
            this.cmbUserName.TabIndex = 2;
            this.cmbUserName.SelectedIndexChanged += new System.EventHandler(this.cmbUserName_SelectedIndexChanged);
            // 
            // lblSalesforce
            // 
            this.lblSalesforce.AutoSize = true;
            this.lblSalesforce.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalesforce.Location = new System.Drawing.Point(12, 9);
            this.lblSalesforce.Name = "lblSalesforce";
            this.lblSalesforce.Size = new System.Drawing.Size(0, 17);
            this.lblSalesforce.TabIndex = 15;
            // 
            // DeployMetadata
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1379, 694);
            this.Controls.Add(this.lblSalesforce);
            this.Controls.Add(this.fromOrgGroup);
            this.Controls.Add(this.cbRunTests);
            this.Controls.Add(this.tbDeploymentValidationId);
            this.Controls.Add(this.lblDeploymentID);
            this.Controls.Add(this.cbPurgeOnDelete);
            this.Controls.Add(this.lblDeploymentMessage);
            this.Controls.Add(this.rtMessages);
            this.Controls.Add(this.lblRunSpecificTests);
            this.Controls.Add(this.tbTestsToRun);
            this.Controls.Add(this.tbDeploymentName);
            this.Controls.Add(this.lblDeploymentName);
            this.Controls.Add(this.cbCheckOnly);
            this.Controls.Add(this.btnDeployMetadata);
            this.Controls.Add(this.lblZipFileLocation);
            this.Controls.Add(this.tbZipFileLocation);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DeployMetadata";
            this.Text = "DeployMetadata";
            this.fromOrgGroup.ResumeLayout(false);
            this.fromOrgGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblRunSpecificTests;
        private System.Windows.Forms.TextBox tbTestsToRun;
        private System.Windows.Forms.TextBox tbDeploymentName;
        private System.Windows.Forms.Label lblDeploymentName;
        public System.Windows.Forms.CheckBox cbCheckOnly;
        public System.Windows.Forms.Button btnDeployMetadata;
        private System.Windows.Forms.Label lblZipFileLocation;
        public System.Windows.Forms.TextBox tbZipFileLocation;
        private System.Windows.Forms.RichTextBox rtMessages;
        private System.Windows.Forms.Label lblDeploymentMessage;
        private System.Windows.Forms.CheckBox cbPurgeOnDelete;
        private System.Windows.Forms.Label lblDeploymentID;
        private System.Windows.Forms.TextBox tbDeploymentValidationId;
        private System.Windows.Forms.CheckBox cbRunTests;
        private System.Windows.Forms.GroupBox fromOrgGroup;
        private System.Windows.Forms.Label lblSFUsername;
        public System.Windows.Forms.ComboBox cmbUserName;
        private System.Windows.Forms.Label lblSalesforce;
    }
}