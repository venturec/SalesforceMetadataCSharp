namespace SalesforceMetadata
{
    partial class ParseSalesforceMetadata
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
            this.btnConvertMDAPI = new System.Windows.Forms.Button();
            this.tbMetadataFolderPath = new System.Windows.Forms.TextBox();
            this.lblMetadataFolderPath = new System.Windows.Forms.Label();
            this.tbReportFolderPath = new System.Windows.Forms.TextBox();
            this.lblSaveReportTo = new System.Windows.Forms.Label();
            this.btnParseProfilesPermissionSets = new System.Windows.Forms.Button();
            this.btnParseObjectsAndFields = new System.Windows.Forms.Button();
            this.lblFilter = new System.Windows.Forms.Label();
            this.tbSearchFilter = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnConvertMDAPI
            // 
            this.btnConvertMDAPI.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConvertMDAPI.Location = new System.Drawing.Point(870, 166);
            this.btnConvertMDAPI.Name = "btnConvertMDAPI";
            this.btnConvertMDAPI.Size = new System.Drawing.Size(232, 47);
            this.btnConvertMDAPI.TabIndex = 8;
            this.btnConvertMDAPI.Text = "Convert to MDAPI";
            this.btnConvertMDAPI.UseVisualStyleBackColor = true;
            this.btnConvertMDAPI.Click += new System.EventHandler(this.btnConvertToMDAPI_Click);
            // 
            // tbMetadataFolderPath
            // 
            this.tbMetadataFolderPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMetadataFolderPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbMetadataFolderPath.Location = new System.Drawing.Point(180, 12);
            this.tbMetadataFolderPath.Name = "tbMetadataFolderPath";
            this.tbMetadataFolderPath.Size = new System.Drawing.Size(922, 23);
            this.tbMetadataFolderPath.TabIndex = 1;
            this.tbMetadataFolderPath.DoubleClick += new System.EventHandler(this.tbMetadataFolderPath_DoubleClick);
            // 
            // lblMetadataFolderPath
            // 
            this.lblMetadataFolderPath.AutoSize = true;
            this.lblMetadataFolderPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMetadataFolderPath.Location = new System.Drawing.Point(10, 15);
            this.lblMetadataFolderPath.Name = "lblMetadataFolderPath";
            this.lblMetadataFolderPath.Size = new System.Drawing.Size(164, 17);
            this.lblMetadataFolderPath.TabIndex = 0;
            this.lblMetadataFolderPath.Text = "Metadata Folder Path";
            // 
            // tbReportFolderPath
            // 
            this.tbReportFolderPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbReportFolderPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbReportFolderPath.Location = new System.Drawing.Point(180, 52);
            this.tbReportFolderPath.Name = "tbReportFolderPath";
            this.tbReportFolderPath.Size = new System.Drawing.Size(922, 23);
            this.tbReportFolderPath.TabIndex = 3;
            this.tbReportFolderPath.DoubleClick += new System.EventHandler(this.tbReportFolderPath_DoubleClick);
            // 
            // lblSaveReportTo
            // 
            this.lblSaveReportTo.AutoSize = true;
            this.lblSaveReportTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSaveReportTo.Location = new System.Drawing.Point(10, 55);
            this.lblSaveReportTo.Name = "lblSaveReportTo";
            this.lblSaveReportTo.Size = new System.Drawing.Size(130, 17);
            this.lblSaveReportTo.TabIndex = 2;
            this.lblSaveReportTo.Text = "Save Reports To";
            // 
            // btnParseProfilesPermissionSets
            // 
            this.btnParseProfilesPermissionSets.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnParseProfilesPermissionSets.Location = new System.Drawing.Point(269, 166);
            this.btnParseProfilesPermissionSets.Name = "btnParseProfilesPermissionSets";
            this.btnParseProfilesPermissionSets.Size = new System.Drawing.Size(232, 47);
            this.btnParseProfilesPermissionSets.TabIndex = 7;
            this.btnParseProfilesPermissionSets.Text = "Parse Profiles / Permission Sets By Object";
            this.btnParseProfilesPermissionSets.UseVisualStyleBackColor = true;
            this.btnParseProfilesPermissionSets.Click += new System.EventHandler(this.btnParseProfilesPermissionSetsByObject_Click);
            // 
            // btnParseObjectsAndFields
            // 
            this.btnParseObjectsAndFields.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnParseObjectsAndFields.Location = new System.Drawing.Point(13, 166);
            this.btnParseObjectsAndFields.Name = "btnParseObjectsAndFields";
            this.btnParseObjectsAndFields.Size = new System.Drawing.Size(232, 47);
            this.btnParseObjectsAndFields.TabIndex = 6;
            this.btnParseObjectsAndFields.Text = "Parse Objects and Fields";
            this.btnParseObjectsAndFields.UseVisualStyleBackColor = true;
            this.btnParseObjectsAndFields.Click += new System.EventHandler(this.btnParseObjectsAndFields_Click);
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilter.Location = new System.Drawing.Point(10, 96);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(45, 17);
            this.lblFilter.TabIndex = 4;
            this.lblFilter.Text = "Filter";
            // 
            // tbSearchFilter
            // 
            this.tbSearchFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSearchFilter.Location = new System.Drawing.Point(180, 93);
            this.tbSearchFilter.Name = "tbSearchFilter";
            this.tbSearchFilter.Size = new System.Drawing.Size(237, 23);
            this.tbSearchFilter.TabIndex = 5;
            // 
            // ParseSalesforceMetadata
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1202, 532);
            this.Controls.Add(this.lblFilter);
            this.Controls.Add(this.tbSearchFilter);
            this.Controls.Add(this.btnParseObjectsAndFields);
            this.Controls.Add(this.btnParseProfilesPermissionSets);
            this.Controls.Add(this.lblSaveReportTo);
            this.Controls.Add(this.tbReportFolderPath);
            this.Controls.Add(this.lblMetadataFolderPath);
            this.Controls.Add(this.tbMetadataFolderPath);
            this.Controls.Add(this.btnConvertMDAPI);
            this.Name = "ParseSalesforceMetadata";
            this.Text = "ParseSalesforceMetadata";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnConvertMDAPI;
        private System.Windows.Forms.TextBox tbMetadataFolderPath;
        private System.Windows.Forms.Label lblMetadataFolderPath;
        private System.Windows.Forms.TextBox tbReportFolderPath;
        private System.Windows.Forms.Label lblSaveReportTo;
        private System.Windows.Forms.Button btnParseProfilesPermissionSets;
        private System.Windows.Forms.Button btnParseObjectsAndFields;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.TextBox tbSearchFilter;
    }
}