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
            this.lblPackageXML = new System.Windows.Forms.Label();
            this.tbFromOrgSaveLocation = new System.Windows.Forms.TextBox();
            this.btnRetrieveMetadataFromSelected = new System.Windows.Forms.Button();
            this.rtMessages = new System.Windows.Forms.RichTextBox();
            this.horizontalLine1 = new System.Windows.Forms.Label();
            this.lblExistingPackageXml = new System.Windows.Forms.Label();
            this.tbExistingPackageXml = new System.Windows.Forms.TextBox();
            this.horizontalLine2 = new System.Windows.Forms.Label();
            this.btnRetrieveMetadata = new System.Windows.Forms.Button();
            this.lblRetrieveFromOrg = new System.Windows.Forms.Label();
            this.cbConvertToVSCodeStyle = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblPackageXML
            // 
            this.lblPackageXML.AutoSize = true;
            this.lblPackageXML.Location = new System.Drawing.Point(4, 107);
            this.lblPackageXML.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPackageXML.Name = "lblPackageXML";
            this.lblPackageXML.Size = new System.Drawing.Size(152, 13);
            this.lblPackageXML.TabIndex = 4;
            this.lblPackageXML.Text = "Retrieve and Save Zip File To:";
            // 
            // tbFromOrgSaveLocation
            // 
            this.tbFromOrgSaveLocation.Location = new System.Drawing.Point(254, 104);
            this.tbFromOrgSaveLocation.Margin = new System.Windows.Forms.Padding(2);
            this.tbFromOrgSaveLocation.Name = "tbFromOrgSaveLocation";
            this.tbFromOrgSaveLocation.Size = new System.Drawing.Size(498, 20);
            this.tbFromOrgSaveLocation.TabIndex = 5;
            this.tbFromOrgSaveLocation.DoubleClick += new System.EventHandler(this.tbPackageXMLLocation_DoubleClick);
            // 
            // btnRetrieveMetadataFromSelected
            // 
            this.btnRetrieveMetadataFromSelected.Location = new System.Drawing.Point(765, 102);
            this.btnRetrieveMetadataFromSelected.Margin = new System.Windows.Forms.Padding(2);
            this.btnRetrieveMetadataFromSelected.Name = "btnRetrieveMetadataFromSelected";
            this.btnRetrieveMetadataFromSelected.Size = new System.Drawing.Size(193, 24);
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
            this.rtMessages.Location = new System.Drawing.Point(7, 254);
            this.rtMessages.Name = "rtMessages";
            this.rtMessages.Size = new System.Drawing.Size(946, 571);
            this.rtMessages.TabIndex = 12;
            this.rtMessages.Text = "";
            // 
            // horizontalLine1
            // 
            this.horizontalLine1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalLine1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.horizontalLine1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.horizontalLine1.Location = new System.Drawing.Point(-4, 145);
            this.horizontalLine1.Name = "horizontalLine1";
            this.horizontalLine1.Size = new System.Drawing.Size(971, 10);
            this.horizontalLine1.TabIndex = 7;
            // 
            // lblExistingPackageXml
            // 
            this.lblExistingPackageXml.AutoSize = true;
            this.lblExistingPackageXml.Location = new System.Drawing.Point(4, 184);
            this.lblExistingPackageXml.Name = "lblExistingPackageXml";
            this.lblExistingPackageXml.Size = new System.Drawing.Size(239, 13);
            this.lblExistingPackageXml.TabIndex = 8;
            this.lblExistingPackageXml.Text = "Retrieve Metadata with Existing Package.xml File";
            // 
            // tbExistingPackageXml
            // 
            this.tbExistingPackageXml.Location = new System.Drawing.Point(254, 181);
            this.tbExistingPackageXml.Name = "tbExistingPackageXml";
            this.tbExistingPackageXml.Size = new System.Drawing.Size(498, 20);
            this.tbExistingPackageXml.TabIndex = 9;
            this.tbExistingPackageXml.DoubleClick += new System.EventHandler(this.tbExistingPackageXml_DoubleClick);
            // 
            // horizontalLine2
            // 
            this.horizontalLine2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalLine2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.horizontalLine2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.horizontalLine2.Location = new System.Drawing.Point(-4, 228);
            this.horizontalLine2.Name = "horizontalLine2";
            this.horizontalLine2.Size = new System.Drawing.Size(971, 23);
            this.horizontalLine2.TabIndex = 11;
            // 
            // btnRetrieveMetadata
            // 
            this.btnRetrieveMetadata.Location = new System.Drawing.Point(765, 180);
            this.btnRetrieveMetadata.Name = "btnRetrieveMetadata";
            this.btnRetrieveMetadata.Size = new System.Drawing.Size(193, 23);
            this.btnRetrieveMetadata.TabIndex = 10;
            this.btnRetrieveMetadata.Text = "Retrieve Metadata";
            this.btnRetrieveMetadata.UseVisualStyleBackColor = true;
            this.btnRetrieveMetadata.Click += new System.EventHandler(this.btnRetrieveMetadata_Click);
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
            this.cbConvertToVSCodeStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbConvertToVSCodeStyle.Location = new System.Drawing.Point(12, 45);
            this.cbConvertToVSCodeStyle.Name = "cbConvertToVSCodeStyle";
            this.cbConvertToVSCodeStyle.Size = new System.Drawing.Size(185, 19);
            this.cbConvertToVSCodeStyle.TabIndex = 2;
            this.cbConvertToVSCodeStyle.Text = "Convert Files to VSCode Style";
            this.cbConvertToVSCodeStyle.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(-4, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(971, 10);
            this.label1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(-4, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(971, 10);
            this.label2.TabIndex = 1;
            // 
            // SalesforceMetadataStep2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(963, 836);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbConvertToVSCodeStyle);
            this.Controls.Add(this.lblRetrieveFromOrg);
            this.Controls.Add(this.btnRetrieveMetadata);
            this.Controls.Add(this.horizontalLine2);
            this.Controls.Add(this.tbExistingPackageXml);
            this.Controls.Add(this.lblExistingPackageXml);
            this.Controls.Add(this.horizontalLine1);
            this.Controls.Add(this.rtMessages);
            this.Controls.Add(this.lblPackageXML);
            this.Controls.Add(this.tbFromOrgSaveLocation);
            this.Controls.Add(this.btnRetrieveMetadataFromSelected);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "SalesforceMetadataStep2";
            this.Text = "Retrieve Metadata";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblPackageXML;
        private System.Windows.Forms.TextBox tbFromOrgSaveLocation;
        public System.Windows.Forms.Button btnRetrieveMetadataFromSelected;
        private System.Windows.Forms.RichTextBox rtMessages;
        private System.Windows.Forms.Label horizontalLine1;
        private System.Windows.Forms.Label lblExistingPackageXml;
        private System.Windows.Forms.TextBox tbExistingPackageXml;
        private System.Windows.Forms.Label horizontalLine2;
        private System.Windows.Forms.Button btnRetrieveMetadata;
        private System.Windows.Forms.Label lblRetrieveFromOrg;
        private System.Windows.Forms.CheckBox cbConvertToVSCodeStyle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}