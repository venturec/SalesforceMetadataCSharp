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
            this.tbPhysicalProcessors = new System.Windows.Forms.TextBox();
            this.lblPhysicalProcessors = new System.Windows.Forms.Label();
            this.lblCPUCoreCount = new System.Windows.Forms.Label();
            this.tbCPUCoreCount = new System.Windows.Forms.TextBox();
            this.lblLogicalProcessors = new System.Windows.Forms.Label();
            this.tbLogicalProcessors = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblSelectLocation
            // 
            this.lblSelectLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(15, 415);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(108, 31);
            this.btnSave.TabIndex = 21;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(140, 415);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(108, 31);
            this.btnCancel.TabIndex = 22;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // encryptDecrypt
            // 
            this.encryptDecrypt.Location = new System.Drawing.Point(589, 128);
            this.encryptDecrypt.Name = "encryptDecrypt";
            this.encryptDecrypt.Size = new System.Drawing.Size(126, 34);
            this.encryptDecrypt.TabIndex = 6;
            this.encryptDecrypt.Text = "Encrypt Decrypt";
            this.encryptDecrypt.UseVisualStyleBackColor = true;
            this.encryptDecrypt.Click += new System.EventHandler(this.encryptDecrypt_Click);
            // 
            // tbSharedSecret
            // 
            this.tbSharedSecret.Location = new System.Drawing.Point(15, 89);
            this.tbSharedSecret.Name = "tbSharedSecret";
            this.tbSharedSecret.Size = new System.Drawing.Size(700, 20);
            this.tbSharedSecret.TabIndex = 3;
            this.tbSharedSecret.DoubleClick += new System.EventHandler(this.tbSharedSecret_DoubleClick);
            // 
            // lblSharedSecretLocation
            // 
            this.lblSharedSecretLocation.AutoSize = true;
            this.lblSharedSecretLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSharedSecretLocation.Location = new System.Drawing.Point(12, 70);
            this.lblSharedSecretLocation.Name = "lblSharedSecretLocation";
            this.lblSharedSecretLocation.Size = new System.Drawing.Size(179, 17);
            this.lblSharedSecretLocation.TabIndex = 2;
            this.lblSharedSecretLocation.Text = "Shared Secret Location";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label2.Location = new System.Drawing.Point(1, 178);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(802, 10);
            this.label2.TabIndex = 7;
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblSalt
            // 
            this.lblSalt.AutoSize = true;
            this.lblSalt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalt.Location = new System.Drawing.Point(12, 126);
            this.lblSalt.Name = "lblSalt";
            this.lblSalt.Size = new System.Drawing.Size(36, 17);
            this.lblSalt.TabIndex = 4;
            this.lblSalt.Text = "Salt";
            // 
            // tbSalt
            // 
            this.tbSalt.Location = new System.Drawing.Point(15, 146);
            this.tbSalt.Name = "tbSalt";
            this.tbSalt.Size = new System.Drawing.Size(352, 20);
            this.tbSalt.TabIndex = 5;
            // 
            // lblDefaultAPI
            // 
            this.lblDefaultAPI.AutoSize = true;
            this.lblDefaultAPI.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDefaultAPI.Location = new System.Drawing.Point(11, 202);
            this.lblDefaultAPI.Name = "lblDefaultAPI";
            this.lblDefaultAPI.Size = new System.Drawing.Size(89, 17);
            this.lblDefaultAPI.TabIndex = 8;
            this.lblDefaultAPI.Text = "Default API";
            // 
            // cmbDefaultAPI
            // 
            this.cmbDefaultAPI.FormattingEnabled = true;
            this.cmbDefaultAPI.Location = new System.Drawing.Point(106, 201);
            this.cmbDefaultAPI.Name = "cmbDefaultAPI";
            this.cmbDefaultAPI.Size = new System.Drawing.Size(121, 21);
            this.cmbDefaultAPI.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label3.Location = new System.Drawing.Point(1, 231);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(802, 10);
            this.label3.TabIndex = 10;
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblDefaultTextEditor
            // 
            this.lblDefaultTextEditor.AutoSize = true;
            this.lblDefaultTextEditor.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDefaultTextEditor.Location = new System.Drawing.Point(11, 250);
            this.lblDefaultTextEditor.Name = "lblDefaultTextEditor";
            this.lblDefaultTextEditor.Size = new System.Drawing.Size(182, 17);
            this.lblDefaultTextEditor.TabIndex = 11;
            this.lblDefaultTextEditor.Text = "Default Text Edtior Path";
            // 
            // tbDefaultTextEditor
            // 
            this.tbDefaultTextEditor.Location = new System.Drawing.Point(12, 270);
            this.tbDefaultTextEditor.Name = "tbDefaultTextEditor";
            this.tbDefaultTextEditor.Size = new System.Drawing.Size(703, 20);
            this.tbDefaultTextEditor.TabIndex = 12;
            this.tbDefaultTextEditor.DoubleClick += new System.EventHandler(this.tbDefaultTextEditor_DoubleClick);
            // 
            // tbAsynchronousThreads
            // 
            this.tbAsynchronousThreads.Location = new System.Drawing.Point(14, 327);
            this.tbAsynchronousThreads.Name = "tbAsynchronousThreads";
            this.tbAsynchronousThreads.Size = new System.Drawing.Size(129, 20);
            this.tbAsynchronousThreads.TabIndex = 14;
            // 
            // lblMetadataRetrievalAynchronousThreads
            // 
            this.lblMetadataRetrievalAynchronousThreads.AutoSize = true;
            this.lblMetadataRetrievalAynchronousThreads.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMetadataRetrievalAynchronousThreads.Location = new System.Drawing.Point(12, 307);
            this.lblMetadataRetrievalAynchronousThreads.Name = "lblMetadataRetrievalAynchronousThreads";
            this.lblMetadataRetrievalAynchronousThreads.Size = new System.Drawing.Size(317, 17);
            this.lblMetadataRetrievalAynchronousThreads.TabIndex = 13;
            this.lblMetadataRetrievalAynchronousThreads.Text = "Metadata Retrieval Asynchronous Threads";
            // 
            // tbPhysicalProcessors
            // 
            this.tbPhysicalProcessors.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPhysicalProcessors.Location = new System.Drawing.Point(630, 374);
            this.tbPhysicalProcessors.Name = "tbPhysicalProcessors";
            this.tbPhysicalProcessors.ReadOnly = true;
            this.tbPhysicalProcessors.Size = new System.Drawing.Size(124, 23);
            this.tbPhysicalProcessors.TabIndex = 20;
            // 
            // lblPhysicalProcessors
            // 
            this.lblPhysicalProcessors.AutoSize = true;
            this.lblPhysicalProcessors.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhysicalProcessors.Location = new System.Drawing.Point(470, 377);
            this.lblPhysicalProcessors.Name = "lblPhysicalProcessors";
            this.lblPhysicalProcessors.Size = new System.Drawing.Size(154, 17);
            this.lblPhysicalProcessors.TabIndex = 19;
            this.lblPhysicalProcessors.Text = "Physical Processors";
            // 
            // lblCPUCoreCount
            // 
            this.lblCPUCoreCount.AutoSize = true;
            this.lblCPUCoreCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCPUCoreCount.Location = new System.Drawing.Point(470, 319);
            this.lblCPUCoreCount.Name = "lblCPUCoreCount";
            this.lblCPUCoreCount.Size = new System.Drawing.Size(125, 17);
            this.lblCPUCoreCount.TabIndex = 15;
            this.lblCPUCoreCount.Text = "CPU Core Count";
            // 
            // tbCPUCoreCount
            // 
            this.tbCPUCoreCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCPUCoreCount.Location = new System.Drawing.Point(630, 316);
            this.tbCPUCoreCount.Name = "tbCPUCoreCount";
            this.tbCPUCoreCount.ReadOnly = true;
            this.tbCPUCoreCount.Size = new System.Drawing.Size(124, 23);
            this.tbCPUCoreCount.TabIndex = 16;
            // 
            // lblLogicalProcessors
            // 
            this.lblLogicalProcessors.AutoSize = true;
            this.lblLogicalProcessors.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogicalProcessors.Location = new System.Drawing.Point(470, 348);
            this.lblLogicalProcessors.Name = "lblLogicalProcessors";
            this.lblLogicalProcessors.Size = new System.Drawing.Size(146, 17);
            this.lblLogicalProcessors.TabIndex = 17;
            this.lblLogicalProcessors.Text = "Logical Processors";
            // 
            // tbLogicalProcessors
            // 
            this.tbLogicalProcessors.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbLogicalProcessors.Location = new System.Drawing.Point(630, 345);
            this.tbLogicalProcessors.Name = "tbLogicalProcessors";
            this.tbLogicalProcessors.ReadOnly = true;
            this.tbLogicalProcessors.Size = new System.Drawing.Size(124, 23);
            this.tbLogicalProcessors.TabIndex = 18;
            // 
            // UserSettings
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(805, 450);
            this.Controls.Add(this.lblLogicalProcessors);
            this.Controls.Add(this.tbLogicalProcessors);
            this.Controls.Add(this.lblCPUCoreCount);
            this.Controls.Add(this.tbCPUCoreCount);
            this.Controls.Add(this.lblPhysicalProcessors);
            this.Controls.Add(this.tbPhysicalProcessors);
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
        private System.Windows.Forms.TextBox tbPhysicalProcessors;
        private System.Windows.Forms.Label lblPhysicalProcessors;
        private System.Windows.Forms.Label lblCPUCoreCount;
        private System.Windows.Forms.TextBox tbCPUCoreCount;
        private System.Windows.Forms.Label lblLogicalProcessors;
        private System.Windows.Forms.TextBox tbLogicalProcessors;
    }
}