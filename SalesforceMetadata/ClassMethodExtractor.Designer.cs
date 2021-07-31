namespace SalesforceMetadata
{
    partial class ClassMethodExtractor
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
            this.ProjectFolder = new System.Windows.Forms.Label();
            this.SaveResultsTo = new System.Windows.Forms.Label();
            this.btnExtractNamespacesClasses = new System.Windows.Forms.Button();
            this.tbProjectFolder = new System.Windows.Forms.TextBox();
            this.tbFileSaveTo = new System.Windows.Forms.TextBox();
            this.cmbFileExtensions = new System.Windows.Forms.ComboBox();
            this.lblFileExtension = new System.Windows.Forms.Label();
            this.fromOrgGroup = new System.Windows.Forms.GroupBox();
            this.lblSFUsername = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbSecurityToken = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblSecurityToken = new System.Windows.Forms.Label();
            this.cmbUserName = new System.Windows.Forms.ComboBox();
            this.btnFindWhereClassUsed = new System.Windows.Forms.Button();
            this.btnAddLoggingToMethod = new System.Windows.Forms.Button();
            this.fromOrgGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProjectFolder
            // 
            this.ProjectFolder.AutoSize = true;
            this.ProjectFolder.Location = new System.Drawing.Point(12, 163);
            this.ProjectFolder.Name = "ProjectFolder";
            this.ProjectFolder.Size = new System.Drawing.Size(72, 13);
            this.ProjectFolder.TabIndex = 1;
            this.ProjectFolder.Text = "Project Folder";
            // 
            // SaveResultsTo
            // 
            this.SaveResultsTo.AutoSize = true;
            this.SaveResultsTo.Location = new System.Drawing.Point(12, 199);
            this.SaveResultsTo.Name = "SaveResultsTo";
            this.SaveResultsTo.Size = new System.Drawing.Size(86, 13);
            this.SaveResultsTo.TabIndex = 3;
            this.SaveResultsTo.Text = "Save Results To";
            // 
            // btnExtractNamespacesClasses
            // 
            this.btnExtractNamespacesClasses.Location = new System.Drawing.Point(16, 316);
            this.btnExtractNamespacesClasses.Name = "btnExtractNamespacesClasses";
            this.btnExtractNamespacesClasses.Size = new System.Drawing.Size(199, 23);
            this.btnExtractNamespacesClasses.TabIndex = 7;
            this.btnExtractNamespacesClasses.Text = "Class Symbol Extractor ";
            this.btnExtractNamespacesClasses.UseVisualStyleBackColor = true;
            this.btnExtractNamespacesClasses.Click += new System.EventHandler(this.btnExtractNamespacesClasses_Click);
            // 
            // tbProjectFolder
            // 
            this.tbProjectFolder.Location = new System.Drawing.Point(127, 163);
            this.tbProjectFolder.Name = "tbProjectFolder";
            this.tbProjectFolder.Size = new System.Drawing.Size(521, 20);
            this.tbProjectFolder.TabIndex = 2;
            this.tbProjectFolder.DoubleClick += new System.EventHandler(this.tbProjectFolder_DoubleClick);
            // 
            // tbFileSaveTo
            // 
            this.tbFileSaveTo.Location = new System.Drawing.Point(127, 199);
            this.tbFileSaveTo.Name = "tbFileSaveTo";
            this.tbFileSaveTo.Size = new System.Drawing.Size(521, 20);
            this.tbFileSaveTo.TabIndex = 4;
            // 
            // cmbFileExtensions
            // 
            this.cmbFileExtensions.FormattingEnabled = true;
            this.cmbFileExtensions.Location = new System.Drawing.Point(127, 233);
            this.cmbFileExtensions.Name = "cmbFileExtensions";
            this.cmbFileExtensions.Size = new System.Drawing.Size(120, 21);
            this.cmbFileExtensions.TabIndex = 6;
            this.cmbFileExtensions.Text = "cs";
            // 
            // lblFileExtension
            // 
            this.lblFileExtension.AutoSize = true;
            this.lblFileExtension.Location = new System.Drawing.Point(12, 236);
            this.lblFileExtension.Name = "lblFileExtension";
            this.lblFileExtension.Size = new System.Drawing.Size(72, 13);
            this.lblFileExtension.TabIndex = 5;
            this.lblFileExtension.Text = "File Extension";
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
            this.fromOrgGroup.Location = new System.Drawing.Point(15, 5);
            this.fromOrgGroup.Name = "fromOrgGroup";
            this.fromOrgGroup.Size = new System.Drawing.Size(523, 115);
            this.fromOrgGroup.TabIndex = 0;
            this.fromOrgGroup.TabStop = false;
            this.fromOrgGroup.Text = "From Org";
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
            this.tbSecurityToken.Location = new System.Drawing.Point(176, 81);
            this.tbSecurityToken.Margin = new System.Windows.Forms.Padding(2);
            this.tbSecurityToken.Name = "tbSecurityToken";
            this.tbSecurityToken.Size = new System.Drawing.Size(215, 20);
            this.tbSecurityToken.TabIndex = 6;
            this.tbSecurityToken.UseSystemPasswordChar = true;
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
            // cmbUserName
            // 
            this.cmbUserName.FormattingEnabled = true;
            this.cmbUserName.Location = new System.Drawing.Point(176, 27);
            this.cmbUserName.Margin = new System.Windows.Forms.Padding(2);
            this.cmbUserName.Name = "cmbUserName";
            this.cmbUserName.Size = new System.Drawing.Size(215, 21);
            this.cmbUserName.TabIndex = 2;
            this.cmbUserName.SelectedIndexChanged += new System.EventHandler(this.cmbUserName_SelectedIndexChanged);
            // 
            // btnFindWhereClassUsed
            // 
            this.btnFindWhereClassUsed.Location = new System.Drawing.Point(240, 316);
            this.btnFindWhereClassUsed.Margin = new System.Windows.Forms.Padding(2);
            this.btnFindWhereClassUsed.Name = "btnFindWhereClassUsed";
            this.btnFindWhereClassUsed.Size = new System.Drawing.Size(199, 23);
            this.btnFindWhereClassUsed.TabIndex = 8;
            this.btnFindWhereClassUsed.Text = "Find Where Class Used";
            this.btnFindWhereClassUsed.UseVisualStyleBackColor = true;
            this.btnFindWhereClassUsed.Click += new System.EventHandler(this.btnFindWhereClassUsed_Click);
            // 
            // btnAddLoggingToMethod
            // 
            this.btnAddLoggingToMethod.Location = new System.Drawing.Point(467, 316);
            this.btnAddLoggingToMethod.Name = "btnAddLoggingToMethod";
            this.btnAddLoggingToMethod.Size = new System.Drawing.Size(199, 23);
            this.btnAddLoggingToMethod.TabIndex = 9;
            this.btnAddLoggingToMethod.Text = "Add Logging to Method";
            this.btnAddLoggingToMethod.UseVisualStyleBackColor = true;
            this.btnAddLoggingToMethod.Click += new System.EventHandler(this.btnAddLoggingToMethod_Click);
            // 
            // ClassMethodExtractor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(940, 551);
            this.Controls.Add(this.btnAddLoggingToMethod);
            this.Controls.Add(this.btnFindWhereClassUsed);
            this.Controls.Add(this.fromOrgGroup);
            this.Controls.Add(this.lblFileExtension);
            this.Controls.Add(this.cmbFileExtensions);
            this.Controls.Add(this.tbFileSaveTo);
            this.Controls.Add(this.tbProjectFolder);
            this.Controls.Add(this.btnExtractNamespacesClasses);
            this.Controls.Add(this.SaveResultsTo);
            this.Controls.Add(this.ProjectFolder);
            this.Name = "ClassMethodExtractor";
            this.Text = "Class / Method Extractor";
            this.fromOrgGroup.ResumeLayout(false);
            this.fromOrgGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ProjectFolder;
        private System.Windows.Forms.Label SaveResultsTo;
        private System.Windows.Forms.Button btnExtractNamespacesClasses;
        private System.Windows.Forms.TextBox tbProjectFolder;
        private System.Windows.Forms.TextBox tbFileSaveTo;
        private System.Windows.Forms.ComboBox cmbFileExtensions;
        private System.Windows.Forms.Label lblFileExtension;
        private System.Windows.Forms.GroupBox fromOrgGroup;
        private System.Windows.Forms.Label lblSFUsername;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbSecurityToken;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblSecurityToken;
        private System.Windows.Forms.ComboBox cmbUserName;
        private System.Windows.Forms.Button btnFindWhereClassUsed;
        private System.Windows.Forms.Button btnAddLoggingToMethod;
    }
}