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
            this.lblSalesforce = new System.Windows.Forms.Label();
            this.tbSelectedFolder = new System.Windows.Forms.TextBox();
            this.lblFolderLocation = new System.Windows.Forms.Label();
            this.lblSaveResultsTo = new System.Windows.Forms.Label();
            this.tbSaveResultsTo = new System.Windows.Forms.TextBox();
            this.btnGenerateConfigReportAsHTML = new System.Windows.Forms.Button();
            this.linkUnsupportedMetadataTypes = new System.Windows.Forms.LinkLabel();
            this.lblUnsupportedMetadataTypes = new System.Windows.Forms.Label();
            this.SuspendLayout();
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
            // tbSelectedFolder
            // 
            this.tbSelectedFolder.Location = new System.Drawing.Point(138, 12);
            this.tbSelectedFolder.Margin = new System.Windows.Forms.Padding(2);
            this.tbSelectedFolder.Name = "tbSelectedFolder";
            this.tbSelectedFolder.Size = new System.Drawing.Size(624, 20);
            this.tbSelectedFolder.TabIndex = 2;
            this.tbSelectedFolder.DoubleClick += new System.EventHandler(this.tbSelectedFolder_DoubleClick);
            // 
            // lblFolderLocation
            // 
            this.lblFolderLocation.AutoSize = true;
            this.lblFolderLocation.Location = new System.Drawing.Point(7, 15);
            this.lblFolderLocation.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFolderLocation.Name = "lblFolderLocation";
            this.lblFolderLocation.Size = new System.Drawing.Size(127, 13);
            this.lblFolderLocation.TabIndex = 1;
            this.lblFolderLocation.Text = "Selected Folder To Parse";
            // 
            // lblSaveResultsTo
            // 
            this.lblSaveResultsTo.AutoSize = true;
            this.lblSaveResultsTo.Location = new System.Drawing.Point(7, 43);
            this.lblSaveResultsTo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSaveResultsTo.Name = "lblSaveResultsTo";
            this.lblSaveResultsTo.Size = new System.Drawing.Size(86, 13);
            this.lblSaveResultsTo.TabIndex = 3;
            this.lblSaveResultsTo.Text = "Save Results To";
            // 
            // tbSaveResultsTo
            // 
            this.tbSaveResultsTo.Location = new System.Drawing.Point(138, 40);
            this.tbSaveResultsTo.Margin = new System.Windows.Forms.Padding(2);
            this.tbSaveResultsTo.Name = "tbSaveResultsTo";
            this.tbSaveResultsTo.Size = new System.Drawing.Size(624, 20);
            this.tbSaveResultsTo.TabIndex = 4;
            this.tbSaveResultsTo.DoubleClick += new System.EventHandler(this.tbSaveResultsTo_DoubleClick);
            // 
            // btnGenerateConfigReportAsHTML
            // 
            this.btnGenerateConfigReportAsHTML.Location = new System.Drawing.Point(138, 97);
            this.btnGenerateConfigReportAsHTML.Name = "btnGenerateConfigReportAsHTML";
            this.btnGenerateConfigReportAsHTML.Size = new System.Drawing.Size(225, 28);
            this.btnGenerateConfigReportAsHTML.TabIndex = 5;
            this.btnGenerateConfigReportAsHTML.Text = "Save Configuration Workbook To HTML";
            this.btnGenerateConfigReportAsHTML.UseVisualStyleBackColor = true;
            this.btnGenerateConfigReportAsHTML.Click += new System.EventHandler(this.btnGenerateConfigReportAsHTML_Click);
            // 
            // linkUnsupportedMetadataTypes
            // 
            this.linkUnsupportedMetadataTypes.AutoSize = true;
            this.linkUnsupportedMetadataTypes.Location = new System.Drawing.Point(243, 408);
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
            this.lblUnsupportedMetadataTypes.Location = new System.Drawing.Point(7, 408);
            this.lblUnsupportedMetadataTypes.Name = "lblUnsupportedMetadataTypes";
            this.lblUnsupportedMetadataTypes.Size = new System.Drawing.Size(201, 13);
            this.lblUnsupportedMetadataTypes.TabIndex = 8;
            this.lblUnsupportedMetadataTypes.Text = "Salesforce Metadata Unsupported Types";
            // 
            // ConfigurationWorkbook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(927, 553);
            this.Controls.Add(this.lblUnsupportedMetadataTypes);
            this.Controls.Add(this.linkUnsupportedMetadataTypes);
            this.Controls.Add(this.btnGenerateConfigReportAsHTML);
            this.Controls.Add(this.tbSaveResultsTo);
            this.Controls.Add(this.lblSaveResultsTo);
            this.Controls.Add(this.lblFolderLocation);
            this.Controls.Add(this.tbSelectedFolder);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ConfigurationWorkbook";
            this.Text = "Generate Configuration Workbook";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox tbSelectedFolder;
        private System.Windows.Forms.Label lblSalesforce;
        private System.Windows.Forms.Label lblFolderLocation;
        private System.Windows.Forms.Label lblSaveResultsTo;
        private System.Windows.Forms.TextBox tbSaveResultsTo;
        private System.Windows.Forms.Button btnGenerateConfigReportAsHTML;
        private System.Windows.Forms.LinkLabel linkUnsupportedMetadataTypes;
        private System.Windows.Forms.Label lblUnsupportedMetadataTypes;
    }
}