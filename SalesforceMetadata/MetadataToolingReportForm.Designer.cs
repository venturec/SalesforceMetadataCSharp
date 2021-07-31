namespace SalesforceMetadata
{
    partial class MetadataToolingReportForm
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
            this.tbMetadataFolderLocation = new System.Windows.Forms.TextBox();
            this.btnGenerateToolingReport = new System.Windows.Forms.Button();
            this.lblMetdataFolderSelection = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbMetadataFolderLocation
            // 
            this.tbMetadataFolderLocation.Location = new System.Drawing.Point(12, 37);
            this.tbMetadataFolderLocation.Name = "tbMetadataFolderLocation";
            this.tbMetadataFolderLocation.Size = new System.Drawing.Size(753, 20);
            this.tbMetadataFolderLocation.TabIndex = 1;
            this.tbMetadataFolderLocation.DoubleClick += new System.EventHandler(this.tbMetadataFolderLocation_DoubleClick);
            // 
            // btnGenerateToolingReport
            // 
            this.btnGenerateToolingReport.Location = new System.Drawing.Point(607, 81);
            this.btnGenerateToolingReport.Name = "btnGenerateToolingReport";
            this.btnGenerateToolingReport.Size = new System.Drawing.Size(158, 23);
            this.btnGenerateToolingReport.TabIndex = 2;
            this.btnGenerateToolingReport.Text = "Generate Tooling Report";
            this.btnGenerateToolingReport.UseVisualStyleBackColor = true;
            this.btnGenerateToolingReport.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // lblMetdataFolderSelection
            // 
            this.lblMetdataFolderSelection.AutoSize = true;
            this.lblMetdataFolderSelection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMetdataFolderSelection.Location = new System.Drawing.Point(12, 21);
            this.lblMetdataFolderSelection.Name = "lblMetdataFolderSelection";
            this.lblMetdataFolderSelection.Size = new System.Drawing.Size(608, 13);
            this.lblMetdataFolderSelection.TabIndex = 0;
            this.lblMetdataFolderSelection.Text = "Select Folder where the Salesforce Metadata Resides. This will be used to read fi" +
    "les from specific folders.";
            this.lblMetdataFolderSelection.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // MetadataToolingReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(848, 335);
            this.Controls.Add(this.lblMetdataFolderSelection);
            this.Controls.Add(this.btnGenerateToolingReport);
            this.Controls.Add(this.tbMetadataFolderLocation);
            this.Name = "MetadataToolingReportForm";
            this.Text = "Metadata / Tooling Report Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbMetadataFolderLocation;
        private System.Windows.Forms.Button btnGenerateToolingReport;
        private System.Windows.Forms.Label lblMetdataFolderSelection;
    }
}