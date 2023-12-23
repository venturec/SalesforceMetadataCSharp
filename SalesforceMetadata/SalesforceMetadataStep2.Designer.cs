namespace SalesforceMetadata
{
    partial class SalesforceMetadataStep2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SalesforceMetadataStep2));
            this.lblSaveResultsTo = new System.Windows.Forms.Label();
            this.tbFromOrgSaveLocation = new System.Windows.Forms.TextBox();
            this.btnRetrieveMetadataFromSelected = new System.Windows.Forms.Button();
            this.rtMessages = new System.Windows.Forms.RichTextBox();
            this.horizontalLine2 = new System.Windows.Forms.Label();
            this.lblExistingPackageXml = new System.Windows.Forms.Label();
            this.tbExistingPackageXml = new System.Windows.Forms.TextBox();
            this.horizontalLine3 = new System.Windows.Forms.Label();
            this.btnRetrieveMetadata = new System.Windows.Forms.Button();
            this.lblRetrieveFromOrg = new System.Windows.Forms.Label();
            this.cbConvertToVSCodeStyle = new System.Windows.Forms.CheckBox();
            this.horizontalLine1 = new System.Windows.Forms.Label();
            this.cbRebuildFolder = new System.Windows.Forms.CheckBox();
            this.lblMessages = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblSaveResultsTo
            // 
            this.lblSaveResultsTo.AutoSize = true;
            this.lblSaveResultsTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSaveResultsTo.Location = new System.Drawing.Point(4, 70);
            this.lblSaveResultsTo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSaveResultsTo.Name = "lblSaveResultsTo";
            this.lblSaveResultsTo.Size = new System.Drawing.Size(200, 17);
            this.lblSaveResultsTo.TabIndex = 4;
            this.lblSaveResultsTo.Text = "Retrieve and Save Zip File To:";
            // 
            // tbFromOrgSaveLocation
            // 
            this.tbFromOrgSaveLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbFromOrgSaveLocation.Location = new System.Drawing.Point(208, 70);
            this.tbFromOrgSaveLocation.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbFromOrgSaveLocation.Name = "tbFromOrgSaveLocation";
            this.tbFromOrgSaveLocation.Size = new System.Drawing.Size(760, 23);
            this.tbFromOrgSaveLocation.TabIndex = 5;
            this.tbFromOrgSaveLocation.DoubleClick += new System.EventHandler(this.tbPackageXMLLocation_DoubleClick);
            // 
            // btnRetrieveMetadataFromSelected
            // 
            this.btnRetrieveMetadataFromSelected.Location = new System.Drawing.Point(296, 224);
            this.btnRetrieveMetadataFromSelected.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnRetrieveMetadataFromSelected.Name = "btnRetrieveMetadataFromSelected";
            this.btnRetrieveMetadataFromSelected.Size = new System.Drawing.Size(241, 24);
            this.btnRetrieveMetadataFromSelected.TabIndex = 6;
            this.btnRetrieveMetadataFromSelected.Text = "Retrieve Metadata from Selected";
            this.btnRetrieveMetadataFromSelected.UseVisualStyleBackColor = true;
            this.btnRetrieveMetadataFromSelected.Click += new System.EventHandler(this.btnRetrieveMetadataFromSelected_Click);
            // 
            // rtMessages
            // 
            this.rtMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtMessages.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtMessages.Location = new System.Drawing.Point(7, 316);
            this.rtMessages.Name = "rtMessages";
            this.rtMessages.Size = new System.Drawing.Size(1376, 429);
            this.rtMessages.TabIndex = 12;
            this.rtMessages.Text = "";
            // 
            // horizontalLine2
            // 
            this.horizontalLine2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalLine2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.horizontalLine2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.horizontalLine2.Location = new System.Drawing.Point(-4, 108);
            this.horizontalLine2.Name = "horizontalLine2";
            this.horizontalLine2.Size = new System.Drawing.Size(1732, 10);
            this.horizontalLine2.TabIndex = 7;
            // 
            // lblExistingPackageXml
            // 
            this.lblExistingPackageXml.AutoSize = true;
            this.lblExistingPackageXml.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExistingPackageXml.Location = new System.Drawing.Point(4, 147);
            this.lblExistingPackageXml.Name = "lblExistingPackageXml";
            this.lblExistingPackageXml.Size = new System.Drawing.Size(168, 17);
            this.lblExistingPackageXml.TabIndex = 8;
            this.lblExistingPackageXml.Text = "Existing Package Xml File";
            // 
            // tbExistingPackageXml
            // 
            this.tbExistingPackageXml.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbExistingPackageXml.Location = new System.Drawing.Point(208, 147);
            this.tbExistingPackageXml.Name = "tbExistingPackageXml";
            this.tbExistingPackageXml.Size = new System.Drawing.Size(760, 23);
            this.tbExistingPackageXml.TabIndex = 9;
            this.tbExistingPackageXml.TextChanged += new System.EventHandler(this.tbExistingPackageXml_TextChanged);
            this.tbExistingPackageXml.DoubleClick += new System.EventHandler(this.tbExistingPackageXml_DoubleClick);
            // 
            // horizontalLine3
            // 
            this.horizontalLine3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalLine3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.horizontalLine3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.horizontalLine3.Location = new System.Drawing.Point(-4, 192);
            this.horizontalLine3.Name = "horizontalLine3";
            this.horizontalLine3.Size = new System.Drawing.Size(1732, 23);
            this.horizontalLine3.TabIndex = 11;
            // 
            // btnRetrieveMetadata
            // 
            this.btnRetrieveMetadata.Location = new System.Drawing.Point(296, 256);
            this.btnRetrieveMetadata.Name = "btnRetrieveMetadata";
            this.btnRetrieveMetadata.Size = new System.Drawing.Size(241, 23);
            this.btnRetrieveMetadata.TabIndex = 10;
            this.btnRetrieveMetadata.Text = "Retrieve Metadata with Existing Package XML";
            this.btnRetrieveMetadata.UseVisualStyleBackColor = true;
            this.btnRetrieveMetadata.Click += new System.EventHandler(this.btnRetrieveMetadataWithPackageXML_Click);
            // 
            // lblRetrieveFromOrg
            // 
            this.lblRetrieveFromOrg.AutoSize = true;
            this.lblRetrieveFromOrg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRetrieveFromOrg.Location = new System.Drawing.Point(3, 7);
            this.lblRetrieveFromOrg.Name = "lblRetrieveFromOrg";
            this.lblRetrieveFromOrg.Size = new System.Drawing.Size(157, 20);
            this.lblRetrieveFromOrg.TabIndex = 0;
            this.lblRetrieveFromOrg.Text = "Retrieve Metadata";
            // 
            // cbConvertToVSCodeStyle
            // 
            this.cbConvertToVSCodeStyle.AutoSize = true;
            this.cbConvertToVSCodeStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbConvertToVSCodeStyle.Location = new System.Drawing.Point(8, 248);
            this.cbConvertToVSCodeStyle.Name = "cbConvertToVSCodeStyle";
            this.cbConvertToVSCodeStyle.Size = new System.Drawing.Size(215, 21);
            this.cbConvertToVSCodeStyle.TabIndex = 2;
            this.cbConvertToVSCodeStyle.Text = "Convert Files to VSCode Style";
            this.cbConvertToVSCodeStyle.UseVisualStyleBackColor = true;
            // 
            // horizontalLine1
            // 
            this.horizontalLine1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalLine1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.horizontalLine1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.horizontalLine1.Location = new System.Drawing.Point(-4, 32);
            this.horizontalLine1.Name = "horizontalLine1";
            this.horizontalLine1.Size = new System.Drawing.Size(1732, 10);
            this.horizontalLine1.TabIndex = 1;
            // 
            // cbRebuildFolder
            // 
            this.cbRebuildFolder.AutoSize = true;
            this.cbRebuildFolder.Location = new System.Drawing.Point(8, 224);
            this.cbRebuildFolder.Name = "cbRebuildFolder";
            this.cbRebuildFolder.Size = new System.Drawing.Size(94, 17);
            this.cbRebuildFolder.TabIndex = 13;
            this.cbRebuildFolder.Text = "Rebuild Folder";
            this.cbRebuildFolder.UseVisualStyleBackColor = true;
            // 
            // lblMessages
            // 
            this.lblMessages.AutoSize = true;
            this.lblMessages.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessages.Location = new System.Drawing.Point(8, 296);
            this.lblMessages.Name = "lblMessages";
            this.lblMessages.Size = new System.Drawing.Size(218, 17);
            this.lblMessages.TabIndex = 14;
            this.lblMessages.Text = "Retrieve Metadata Messages";
            // 
            // SalesforceMetadataStep2
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1395, 757);
            this.Controls.Add(this.lblMessages);
            this.Controls.Add(this.cbRebuildFolder);
            this.Controls.Add(this.horizontalLine1);
            this.Controls.Add(this.cbConvertToVSCodeStyle);
            this.Controls.Add(this.lblRetrieveFromOrg);
            this.Controls.Add(this.btnRetrieveMetadata);
            this.Controls.Add(this.horizontalLine3);
            this.Controls.Add(this.tbExistingPackageXml);
            this.Controls.Add(this.lblExistingPackageXml);
            this.Controls.Add(this.horizontalLine2);
            this.Controls.Add(this.rtMessages);
            this.Controls.Add(this.lblSaveResultsTo);
            this.Controls.Add(this.tbFromOrgSaveLocation);
            this.Controls.Add(this.btnRetrieveMetadataFromSelected);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "SalesforceMetadataStep2";
            this.Text = "Retrieve Metadata";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblSaveResultsTo;
        public System.Windows.Forms.TextBox tbFromOrgSaveLocation;
        public System.Windows.Forms.Button btnRetrieveMetadataFromSelected;
        private System.Windows.Forms.RichTextBox rtMessages;
        private System.Windows.Forms.Label horizontalLine2;
        private System.Windows.Forms.Label lblExistingPackageXml;
        private System.Windows.Forms.TextBox tbExistingPackageXml;
        private System.Windows.Forms.Label horizontalLine3;
        private System.Windows.Forms.Button btnRetrieveMetadata;
        private System.Windows.Forms.Label lblRetrieveFromOrg;
        private System.Windows.Forms.CheckBox cbConvertToVSCodeStyle;
        private System.Windows.Forms.Label horizontalLine1;
        private System.Windows.Forms.CheckBox cbRebuildFolder;
        private System.Windows.Forms.Label lblMessages;
    }
}