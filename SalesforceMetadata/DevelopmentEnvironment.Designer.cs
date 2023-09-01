namespace SalesforceMetadata
{
    partial class DevelopmentEnvironment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DevelopmentEnvironment));
            this.lblNote = new System.Windows.Forms.Label();
            this.treeViewMetadata = new System.Windows.Forms.TreeView();
            this.tbParentFolder = new System.Windows.Forms.TextBox();
            this.lblFolder = new System.Windows.Forms.Label();
            this.lblDeploymentFolder = new System.Windows.Forms.Label();
            this.tbDeployFrom = new System.Windows.Forms.TextBox();
            this.btnRetrieveFromOrg = new System.Windows.Forms.Button();
            this.btnDeployToOrg = new System.Windows.Forms.Button();
            this.fromOrgGroup = new System.Windows.Forms.GroupBox();
            this.lblSFUsername = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbSecurityToken = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblSecurityToken = new System.Windows.Forms.Label();
            this.cmbUserName = new System.Windows.Forms.ComboBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.fromOrgGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic) 
                | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNote.Location = new System.Drawing.Point(12, 9);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(883, 17);
            this.lblNote.TabIndex = 0;
            this.lblNote.Text = "This form allows you to use the editor of your choice, checks for changes, and se" +
    "lects the elements which have changed";
            // 
            // treeViewMetadata
            // 
            this.treeViewMetadata.CheckBoxes = true;
            this.treeViewMetadata.Location = new System.Drawing.Point(14, 142);
            this.treeViewMetadata.Name = "treeViewMetadata";
            this.treeViewMetadata.Size = new System.Drawing.Size(880, 681);
            this.treeViewMetadata.TabIndex = 5;
            this.treeViewMetadata.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewMetadata_AfterCheck);
            this.treeViewMetadata.DoubleClick += new System.EventHandler(this.treeViewMetadata_DoubleClick);
            // 
            // tbParentFolder
            // 
            this.tbParentFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbParentFolder.Location = new System.Drawing.Point(191, 43);
            this.tbParentFolder.Name = "tbParentFolder";
            this.tbParentFolder.Size = new System.Drawing.Size(703, 23);
            this.tbParentFolder.TabIndex = 2;
            this.tbParentFolder.TextChanged += new System.EventHandler(this.tbParentFolder_TextChanged);
            this.tbParentFolder.DoubleClick += new System.EventHandler(this.tbParentFolder_DoubleClick);
            // 
            // lblFolder
            // 
            this.lblFolder.AutoSize = true;
            this.lblFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFolder.Location = new System.Drawing.Point(17, 44);
            this.lblFolder.Name = "lblFolder";
            this.lblFolder.Size = new System.Drawing.Size(122, 17);
            this.lblFolder.TabIndex = 1;
            this.lblFolder.Text = "Selected Folder";
            // 
            // lblDeploymentFolder
            // 
            this.lblDeploymentFolder.AutoSize = true;
            this.lblDeploymentFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeploymentFolder.Location = new System.Drawing.Point(17, 76);
            this.lblDeploymentFolder.Name = "lblDeploymentFolder";
            this.lblDeploymentFolder.Size = new System.Drawing.Size(150, 17);
            this.lblDeploymentFolder.TabIndex = 3;
            this.lblDeploymentFolder.Text = "Deploy From Folder";
            // 
            // tbDeployFrom
            // 
            this.tbDeployFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbDeployFrom.Location = new System.Drawing.Point(191, 76);
            this.tbDeployFrom.Name = "tbDeployFrom";
            this.tbDeployFrom.Size = new System.Drawing.Size(703, 23);
            this.tbDeployFrom.TabIndex = 4;
            this.tbDeployFrom.TextChanged += new System.EventHandler(this.tbDeployFrom_TextChanged);
            this.tbDeployFrom.DoubleClick += new System.EventHandler(this.tbDeployFrom_DoubleClick);
            // 
            // btnRetrieveFromOrg
            // 
            this.btnRetrieveFromOrg.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRetrieveFromOrg.Location = new System.Drawing.Point(1324, 142);
            this.btnRetrieveFromOrg.Name = "btnRetrieveFromOrg";
            this.btnRetrieveFromOrg.Size = new System.Drawing.Size(138, 34);
            this.btnRetrieveFromOrg.TabIndex = 7;
            this.btnRetrieveFromOrg.Text = "Retrieve from Org";
            this.btnRetrieveFromOrg.UseVisualStyleBackColor = true;
            // 
            // btnDeployToOrg
            // 
            this.btnDeployToOrg.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeployToOrg.Location = new System.Drawing.Point(1324, 191);
            this.btnDeployToOrg.Name = "btnDeployToOrg";
            this.btnDeployToOrg.Size = new System.Drawing.Size(138, 34);
            this.btnDeployToOrg.TabIndex = 8;
            this.btnDeployToOrg.Text = "Deploy to Org";
            this.btnDeployToOrg.UseVisualStyleBackColor = true;
            this.btnDeployToOrg.Click += new System.EventHandler(this.btnDeployToOrg_Click);
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
            this.fromOrgGroup.Location = new System.Drawing.Point(939, 12);
            this.fromOrgGroup.Name = "fromOrgGroup";
            this.fromOrgGroup.Size = new System.Drawing.Size(523, 115);
            this.fromOrgGroup.TabIndex = 9;
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
            this.tbSecurityToken.Location = new System.Drawing.Point(176, 80);
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
            this.cmbUserName.DropDownWidth = 325;
            this.cmbUserName.FormattingEnabled = true;
            this.cmbUserName.Location = new System.Drawing.Point(176, 27);
            this.cmbUserName.Margin = new System.Windows.Forms.Padding(2);
            this.cmbUserName.Name = "cmbUserName";
            this.cmbUserName.Size = new System.Drawing.Size(324, 21);
            this.cmbUserName.TabIndex = 2;
            this.cmbUserName.SelectedIndexChanged += new System.EventHandler(this.cmbUserName_SelectedIndexChanged);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(743, 113);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(151, 23);
            this.btnRefresh.TabIndex = 6;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // DevelopmentEnvironment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1481, 835);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.fromOrgGroup);
            this.Controls.Add(this.btnDeployToOrg);
            this.Controls.Add(this.btnRetrieveFromOrg);
            this.Controls.Add(this.tbDeployFrom);
            this.Controls.Add(this.lblDeploymentFolder);
            this.Controls.Add(this.lblFolder);
            this.Controls.Add(this.tbParentFolder);
            this.Controls.Add(this.treeViewMetadata);
            this.Controls.Add(this.lblNote);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DevelopmentEnvironment";
            this.Text = "DevelopmentEnvironment";
            this.fromOrgGroup.ResumeLayout(false);
            this.fromOrgGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNote;
        public System.Windows.Forms.TreeView treeViewMetadata;
        public System.Windows.Forms.TextBox tbParentFolder;
        private System.Windows.Forms.Label lblFolder;
        private System.Windows.Forms.Label lblDeploymentFolder;
        public System.Windows.Forms.TextBox tbDeployFrom;
        private System.Windows.Forms.Button btnRetrieveFromOrg;
        private System.Windows.Forms.Button btnDeployToOrg;
        private System.Windows.Forms.GroupBox fromOrgGroup;
        private System.Windows.Forms.Label lblSFUsername;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbSecurityToken;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblSecurityToken;
        private System.Windows.Forms.ComboBox cmbUserName;
        private System.Windows.Forms.Button btnRefresh;
    }
}