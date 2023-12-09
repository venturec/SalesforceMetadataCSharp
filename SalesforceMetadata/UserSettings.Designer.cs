namespace SalesforceMetadata
{
    partial class UserSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserSettings));
            this.lblSelectLocation = new System.Windows.Forms.Label();
            this.tbXmlFileLocation = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.encryptDecrypt = new System.Windows.Forms.Button();
            this.tbSharedSecret = new System.Windows.Forms.TextBox();
            this.lblSharedSecretLocation = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSalt = new System.Windows.Forms.Label();
            this.tbSalt = new System.Windows.Forms.TextBox();
            this.lblDefaultAPI = new System.Windows.Forms.Label();
            this.cmbDefaultAPI = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblDefaultTextEditor = new System.Windows.Forms.Label();
            this.tbDefaultTextEditor = new System.Windows.Forms.TextBox();
            this.tbAsynchronousThreads = new System.Windows.Forms.TextBox();
            this.lblMetadataRetrievalAynchronousThreads = new System.Windows.Forms.Label();
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
            this.tbXmlFileLocation.Size = new System.Drawing.Size(700, 20);
            this.tbXmlFileLocation.TabIndex = 1;
            this.tbXmlFileLocation.DoubleClick += new System.EventHandler(this.tbXmlFileLocation_DoubleClick);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(9, 415);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(101, 415);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // encryptDecrypt
            // 
            this.encryptDecrypt.Location = new System.Drawing.Point(589, 128);
            this.encryptDecrypt.Name = "encryptDecrypt";
            this.encryptDecrypt.Size = new System.Drawing.Size(126, 34);
            this.encryptDecrypt.TabIndex = 7;
            this.encryptDecrypt.Text = "Encrypt Decrypt";
            this.encryptDecrypt.UseVisualStyleBackColor = true;
            this.encryptDecrypt.Click += new System.EventHandler(this.encryptDecrypt_Click);
            // 
            // tbSharedSecret
            // 
            this.tbSharedSecret.Location = new System.Drawing.Point(15, 89);
            this.tbSharedSecret.Name = "tbSharedSecret";
            this.tbSharedSecret.Size = new System.Drawing.Size(700, 20);
            this.tbSharedSecret.TabIndex = 4;
            this.tbSharedSecret.DoubleClick += new System.EventHandler(this.tbSharedSecret_DoubleClick);
            // 
            // lblSharedSecretLocation
            // 
            this.lblSharedSecretLocation.AutoSize = true;
            this.lblSharedSecretLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSharedSecretLocation.Location = new System.Drawing.Point(12, 70);
            this.lblSharedSecretLocation.Name = "lblSharedSecretLocation";
            this.lblSharedSecretLocation.Size = new System.Drawing.Size(141, 13);
            this.lblSharedSecretLocation.TabIndex = 3;
            this.lblSharedSecretLocation.Text = "Shared Secret Location";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label2.Location = new System.Drawing.Point(1, 178);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(802, 10);
            this.label2.TabIndex = 8;
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblSalt
            // 
            this.lblSalt.AutoSize = true;
            this.lblSalt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalt.Location = new System.Drawing.Point(12, 126);
            this.lblSalt.Name = "lblSalt";
            this.lblSalt.Size = new System.Drawing.Size(29, 13);
            this.lblSalt.TabIndex = 5;
            this.lblSalt.Text = "Salt";
            // 
            // tbSalt
            // 
            this.tbSalt.Location = new System.Drawing.Point(15, 142);
            this.tbSalt.Name = "tbSalt";
            this.tbSalt.Size = new System.Drawing.Size(352, 20);
            this.tbSalt.TabIndex = 6;
            // 
            // lblDefaultAPI
            // 
            this.lblDefaultAPI.AutoSize = true;
            this.lblDefaultAPI.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDefaultAPI.Location = new System.Drawing.Point(11, 202);
            this.lblDefaultAPI.Name = "lblDefaultAPI";
            this.lblDefaultAPI.Size = new System.Drawing.Size(72, 13);
            this.lblDefaultAPI.TabIndex = 11;
            this.lblDefaultAPI.Text = "Default API";
            // 
            // cmbDefaultAPI
            // 
            this.cmbDefaultAPI.FormattingEnabled = true;
            this.cmbDefaultAPI.Location = new System.Drawing.Point(89, 199);
            this.cmbDefaultAPI.Name = "cmbDefaultAPI";
            this.cmbDefaultAPI.Size = new System.Drawing.Size(121, 21);
            this.cmbDefaultAPI.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label3.Location = new System.Drawing.Point(1, 231);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(802, 10);
            this.label3.TabIndex = 13;
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblDefaultTextEditor
            // 
            this.lblDefaultTextEditor.AutoSize = true;
            this.lblDefaultTextEditor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDefaultTextEditor.Location = new System.Drawing.Point(11, 250);
            this.lblDefaultTextEditor.Name = "lblDefaultTextEditor";
            this.lblDefaultTextEditor.Size = new System.Drawing.Size(144, 13);
            this.lblDefaultTextEditor.TabIndex = 14;
            this.lblDefaultTextEditor.Text = "Default Text Edtior Path";
            // 
            // tbDefaultTextEditor
            // 
            this.tbDefaultTextEditor.Location = new System.Drawing.Point(12, 266);
            this.tbDefaultTextEditor.Name = "tbDefaultTextEditor";
            this.tbDefaultTextEditor.Size = new System.Drawing.Size(703, 20);
            this.tbDefaultTextEditor.TabIndex = 15;
            this.tbDefaultTextEditor.DoubleClick += new System.EventHandler(this.tbDefaultTextEditor_DoubleClick);
            // 
            // tbAsynchronousThreads
            // 
            this.tbAsynchronousThreads.Location = new System.Drawing.Point(14, 323);
            this.tbAsynchronousThreads.Name = "tbAsynchronousThreads";
            this.tbAsynchronousThreads.Size = new System.Drawing.Size(122, 20);
            this.tbAsynchronousThreads.TabIndex = 16;
            // 
            // lblMetadataRetrievalAynchronousThreads
            // 
            this.lblMetadataRetrievalAynchronousThreads.AutoSize = true;
            this.lblMetadataRetrievalAynchronousThreads.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMetadataRetrievalAynchronousThreads.Location = new System.Drawing.Point(12, 307);
            this.lblMetadataRetrievalAynchronousThreads.Name = "lblMetadataRetrievalAynchronousThreads";
            this.lblMetadataRetrievalAynchronousThreads.Size = new System.Drawing.Size(248, 13);
            this.lblMetadataRetrievalAynchronousThreads.TabIndex = 17;
            this.lblMetadataRetrievalAynchronousThreads.Text = "Metadata Retrieval Asynchronous Threads";
            // 
            // UserSettings
            // 
            this.ClientSize = new System.Drawing.Size(805, 450);
            this.Controls.Add(this.lblMetadataRetrievalAynchronousThreads);
            this.Controls.Add(this.tbAsynchronousThreads);
            this.Controls.Add(this.tbDefaultTextEditor);
            this.Controls.Add(this.lblDefaultTextEditor);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbDefaultAPI);
            this.Controls.Add(this.lblDefaultAPI);
            this.Controls.Add(this.tbSalt);
            this.Controls.Add(this.lblSalt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblSharedSecretLocation);
            this.Controls.Add(this.tbSharedSecret);
            this.Controls.Add(this.encryptDecrypt);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbXmlFileLocation);
            this.Controls.Add(this.lblSelectLocation);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UserSettings";
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSalt;
        private System.Windows.Forms.TextBox tbSalt;
        private System.Windows.Forms.Label lblDefaultAPI;
        private System.Windows.Forms.ComboBox cmbDefaultAPI;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblDefaultTextEditor;
        private System.Windows.Forms.TextBox tbDefaultTextEditor;
        private System.Windows.Forms.TextBox tbAsynchronousThreads;
        private System.Windows.Forms.Label lblMetadataRetrievalAynchronousThreads;
    }
}