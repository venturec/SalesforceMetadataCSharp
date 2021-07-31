namespace SalesforceMetadata
{
    partial class RESTService
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RESTService));
            this.lblRESTExample = new System.Windows.Forms.Label();
            this.tbURI = new System.Windows.Forms.TextBox();
            this.URI = new System.Windows.Forms.Label();
            this.fromOrgGroup = new System.Windows.Forms.GroupBox();
            this.lblSecurityToken = new System.Windows.Forms.Label();
            this.tbSecurityToken = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbUsername = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.cmbVerb = new System.Windows.Forms.ComboBox();
            this.lblVerb = new System.Windows.Forms.Label();
            this.tbFileSaveLocation = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSendRequest = new System.Windows.Forms.Button();
            this.tbObjectName = new System.Windows.Forms.TextBox();
            this.lblObject = new System.Windows.Forms.Label();
            this.tbFilter = new System.Windows.Forms.TextBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.fromOrgGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblRESTExample
            // 
            this.lblRESTExample.AutoSize = true;
            this.lblRESTExample.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRESTExample.Location = new System.Drawing.Point(12, 9);
            this.lblRESTExample.Name = "lblRESTExample";
            this.lblRESTExample.Size = new System.Drawing.Size(226, 35);
            this.lblRESTExample.TabIndex = 0;
            this.lblRESTExample.Text = "REST Example";
            // 
            // tbURI
            // 
            this.tbURI.Location = new System.Drawing.Point(99, 128);
            this.tbURI.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbURI.Name = "tbURI";
            this.tbURI.Size = new System.Drawing.Size(737, 22);
            this.tbURI.TabIndex = 2;
            this.tbURI.Text = "/services/data/";
            // 
            // URI
            // 
            this.URI.AutoSize = true;
            this.URI.Location = new System.Drawing.Point(15, 130);
            this.URI.Name = "URI";
            this.URI.Size = new System.Drawing.Size(72, 17);
            this.URI.TabIndex = 1;
            this.URI.Text = "REST URI";
            // 
            // fromOrgGroup
            // 
            this.fromOrgGroup.BackColor = System.Drawing.SystemColors.Control;
            this.fromOrgGroup.Controls.Add(this.lblSecurityToken);
            this.fromOrgGroup.Controls.Add(this.tbSecurityToken);
            this.fromOrgGroup.Controls.Add(this.label1);
            this.fromOrgGroup.Controls.Add(this.tbUsername);
            this.fromOrgGroup.Controls.Add(this.tbPassword);
            this.fromOrgGroup.Controls.Add(this.lblPassword);
            this.fromOrgGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fromOrgGroup.Location = new System.Drawing.Point(19, 191);
            this.fromOrgGroup.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.fromOrgGroup.Name = "fromOrgGroup";
            this.fromOrgGroup.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.fromOrgGroup.Size = new System.Drawing.Size(697, 142);
            this.fromOrgGroup.TabIndex = 3;
            this.fromOrgGroup.TabStop = false;
            this.fromOrgGroup.Text = "From Org";
            // 
            // lblSecurityToken
            // 
            this.lblSecurityToken.AutoSize = true;
            this.lblSecurityToken.Location = new System.Drawing.Point(77, 96);
            this.lblSecurityToken.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSecurityToken.Name = "lblSecurityToken";
            this.lblSecurityToken.Size = new System.Drawing.Size(103, 17);
            this.lblSecurityToken.TabIndex = 4;
            this.lblSecurityToken.Text = "Security Token";
            // 
            // tbSecurityToken
            // 
            this.tbSecurityToken.Location = new System.Drawing.Point(235, 96);
            this.tbSecurityToken.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbSecurityToken.Name = "tbSecurityToken";
            this.tbSecurityToken.Size = new System.Drawing.Size(453, 23);
            this.tbSecurityToken.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(77, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username";
            // 
            // tbUsername
            // 
            this.tbUsername.Location = new System.Drawing.Point(235, 37);
            this.tbUsername.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new System.Drawing.Size(453, 23);
            this.tbUsername.TabIndex = 1;
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(235, 65);
            this.tbPassword.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(453, 23);
            this.tbPassword.TabIndex = 3;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(77, 68);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(69, 17);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Password";
            // 
            // cmbVerb
            // 
            this.cmbVerb.FormattingEnabled = true;
            this.cmbVerb.Items.AddRange(new object[] {
            "GET",
            "POST",
            "PATCH",
            "DELETE"});
            this.cmbVerb.Location = new System.Drawing.Point(1000, 126);
            this.cmbVerb.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbVerb.Name = "cmbVerb";
            this.cmbVerb.Size = new System.Drawing.Size(121, 24);
            this.cmbVerb.TabIndex = 5;
            this.cmbVerb.Text = "GET";
            // 
            // lblVerb
            // 
            this.lblVerb.AutoSize = true;
            this.lblVerb.Location = new System.Drawing.Point(931, 129);
            this.lblVerb.Name = "lblVerb";
            this.lblVerb.Size = new System.Drawing.Size(38, 17);
            this.lblVerb.TabIndex = 4;
            this.lblVerb.Text = "Verb";
            // 
            // tbFileSaveLocation
            // 
            this.tbFileSaveLocation.Location = new System.Drawing.Point(133, 421);
            this.tbFileSaveLocation.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbFileSaveLocation.Name = "tbFileSaveLocation";
            this.tbFileSaveLocation.Size = new System.Drawing.Size(703, 22);
            this.tbFileSaveLocation.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 423);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 17);
            this.label2.TabIndex = 10;
            this.label2.Text = "Save Results To";
            // 
            // btnSendRequest
            // 
            this.btnSendRequest.Location = new System.Drawing.Point(933, 417);
            this.btnSendRequest.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSendRequest.Name = "btnSendRequest";
            this.btnSendRequest.Size = new System.Drawing.Size(187, 31);
            this.btnSendRequest.TabIndex = 12;
            this.btnSendRequest.Text = "Send Request";
            this.btnSendRequest.UseVisualStyleBackColor = true;
            this.btnSendRequest.Click += new System.EventHandler(this.btnSendRequest_Click);
            // 
            // tbObjectName
            // 
            this.tbObjectName.Location = new System.Drawing.Point(1000, 191);
            this.tbObjectName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbObjectName.Name = "tbObjectName";
            this.tbObjectName.Size = new System.Drawing.Size(203, 22);
            this.tbObjectName.TabIndex = 7;
            // 
            // lblObject
            // 
            this.lblObject.AutoSize = true;
            this.lblObject.Location = new System.Drawing.Point(904, 194);
            this.lblObject.Name = "lblObject";
            this.lblObject.Size = new System.Drawing.Size(90, 17);
            this.lblObject.TabIndex = 6;
            this.lblObject.Text = "Object Name";
            // 
            // tbFilter
            // 
            this.tbFilter.Location = new System.Drawing.Point(1000, 226);
            this.tbFilter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbFilter.Name = "tbFilter";
            this.tbFilter.Size = new System.Drawing.Size(203, 22);
            this.tbFilter.TabIndex = 9;
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(904, 229);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(39, 17);
            this.lblFilter.TabIndex = 8;
            this.lblFilter.Text = "Filter";
            // 
            // RESTService
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1241, 496);
            this.Controls.Add(this.lblFilter);
            this.Controls.Add(this.tbFilter);
            this.Controls.Add(this.lblObject);
            this.Controls.Add(this.tbObjectName);
            this.Controls.Add(this.btnSendRequest);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbFileSaveLocation);
            this.Controls.Add(this.lblVerb);
            this.Controls.Add(this.cmbVerb);
            this.Controls.Add(this.fromOrgGroup);
            this.Controls.Add(this.URI);
            this.Controls.Add(this.tbURI);
            this.Controls.Add(this.lblRESTExample);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "RESTService";
            this.Text = "Sandbox Seeding Step 1";
            this.fromOrgGroup.ResumeLayout(false);
            this.fromOrgGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblRESTExample;
        private System.Windows.Forms.TextBox tbURI;
        private System.Windows.Forms.Label URI;
        private System.Windows.Forms.GroupBox fromOrgGroup;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbUsername;
        private System.Windows.Forms.ComboBox cmbVerb;
        private System.Windows.Forms.Label lblVerb;
        private System.Windows.Forms.TextBox tbFileSaveLocation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSendRequest;
        private System.Windows.Forms.TextBox tbObjectName;
        private System.Windows.Forms.Label lblObject;
        private System.Windows.Forms.TextBox tbFilter;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.Label lblSecurityToken;
        private System.Windows.Forms.TextBox tbSecurityToken;
    }
}