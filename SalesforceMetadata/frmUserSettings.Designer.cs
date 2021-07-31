namespace SalesforceMetadata
{
    partial class frmUserSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUserSettings));
            this.lblSelectLocation = new System.Windows.Forms.Label();
            this.tbXmlFileLocation = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.encryptDecrypt = new System.Windows.Forms.Button();
            this.tbSharedSecret = new System.Windows.Forms.TextBox();
            this.lblSharedSecretLocation = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSalt = new System.Windows.Forms.Label();
            this.tbSalt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblSelectLocation
            // 
            this.lblSelectLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectLocation.Location = new System.Drawing.Point(12, 9);
            this.lblSelectLocation.Name = "lblSelectLocation";
            this.lblSelectLocation.Size = new System.Drawing.Size(703, 23);
            this.lblSelectLocation.TabIndex = 0;
            this.lblSelectLocation.Text = "Please select the XML file whcih contains your Username, Enterprise WSDL URL and " +
    "Metadata WSL URL";
            // 
            // tbXmlFileLocation
            // 
            this.tbXmlFileLocation.Location = new System.Drawing.Point(15, 35);
            this.tbXmlFileLocation.Name = "tbXmlFileLocation";
            this.tbXmlFileLocation.Size = new System.Drawing.Size(700, 22);
            this.tbXmlFileLocation.TabIndex = 1;
            this.tbXmlFileLocation.DoubleClick += new System.EventHandler(this.tbXmlFileLocation_DoubleClick);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(15, 310);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(107, 310);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // encryptDecrypt
            // 
            this.encryptDecrypt.Location = new System.Drawing.Point(589, 181);
            this.encryptDecrypt.Name = "encryptDecrypt";
            this.encryptDecrypt.Size = new System.Drawing.Size(126, 34);
            this.encryptDecrypt.TabIndex = 7;
            this.encryptDecrypt.Text = "Encrypt Decrypt";
            this.encryptDecrypt.UseVisualStyleBackColor = true;
            this.encryptDecrypt.Click += new System.EventHandler(this.encryptDecrypt_Click);
            // 
            // tbSharedSecret
            // 
            this.tbSharedSecret.Location = new System.Drawing.Point(15, 134);
            this.tbSharedSecret.Name = "tbSharedSecret";
            this.tbSharedSecret.Size = new System.Drawing.Size(700, 22);
            this.tbSharedSecret.TabIndex = 4;
            this.tbSharedSecret.DoubleClick += new System.EventHandler(this.tbSharedSecret_DoubleClick);
            // 
            // lblSharedSecretLocation
            // 
            this.lblSharedSecretLocation.AutoSize = true;
            this.lblSharedSecretLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSharedSecretLocation.Location = new System.Drawing.Point(12, 115);
            this.lblSharedSecretLocation.Name = "lblSharedSecretLocation";
            this.lblSharedSecretLocation.Size = new System.Drawing.Size(179, 17);
            this.lblSharedSecretLocation.TabIndex = 3;
            this.lblSharedSecretLocation.Text = "Shared Secret Location";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.Location = new System.Drawing.Point(2, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(724, 10);
            this.label1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label2.Location = new System.Drawing.Point(2, 226);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(724, 10);
            this.label2.TabIndex = 8;
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblSalt
            // 
            this.lblSalt.AutoSize = true;
            this.lblSalt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalt.Location = new System.Drawing.Point(12, 171);
            this.lblSalt.Name = "lblSalt";
            this.lblSalt.Size = new System.Drawing.Size(36, 17);
            this.lblSalt.TabIndex = 5;
            this.lblSalt.Text = "Salt";
            // 
            // tbSalt
            // 
            this.tbSalt.Location = new System.Drawing.Point(12, 187);
            this.tbSalt.Name = "tbSalt";
            this.tbSalt.Size = new System.Drawing.Size(352, 22);
            this.tbSalt.TabIndex = 6;
            // 
            // frmUserSettings
            // 
            this.ClientSize = new System.Drawing.Size(727, 345);
            this.Controls.Add(this.tbSalt);
            this.Controls.Add(this.lblSalt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblSharedSecretLocation);
            this.Controls.Add(this.tbSharedSecret);
            this.Controls.Add(this.encryptDecrypt);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbXmlFileLocation);
            this.Controls.Add(this.lblSelectLocation);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmUserSettings";
            this.Text = "User Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSelectLocation;
        private System.Windows.Forms.TextBox tbXmlFileLocation;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button encryptDecrypt;
        private System.Windows.Forms.TextBox tbSharedSecret;
        private System.Windows.Forms.Label lblSharedSecretLocation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSalt;
        private System.Windows.Forms.TextBox tbSalt;
    }
}