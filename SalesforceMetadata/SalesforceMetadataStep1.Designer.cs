namespace SalesforceMetadata
{
    partial class SalesforceMetadataStep1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SalesforceMetadataStep1));
            this.btnGetMetadataTypes = new System.Windows.Forms.Button();
            this.lblSFMetadataPackage = new System.Windows.Forms.Label();
            this.lbMetadataTypes = new System.Windows.Forms.CheckedListBox();
            this.lblSFUsername = new System.Windows.Forms.Label();
            this.lblSalesforce = new System.Windows.Forms.Label();
            this.cmbUserName = new System.Windows.Forms.ComboBox();
            this.btnRetrieveMetadata = new System.Windows.Forms.Button();
            this.fromOrgGroup = new System.Windows.Forms.GroupBox();
            this.btnSobjectFieldInspector = new System.Windows.Forms.Button();
            this.btnGenerateToolingReport = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.lblSelectAll = new System.Windows.Forms.Label();
            this.btnSelectNone = new System.Windows.Forms.Button();
            this.lblSelectNone = new System.Windows.Forms.Label();
            this.btnSelectDefaults = new System.Windows.Forms.Button();
            this.lblSelectDefaults = new System.Windows.Forms.Label();
            this.btnExportMetadataTypes = new System.Windows.Forms.Button();
            this.fromOrgGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGetMetadataTypes
            // 
            this.btnGetMetadataTypes.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnGetMetadataTypes.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetMetadataTypes.Location = new System.Drawing.Point(813, 312);
            this.btnGetMetadataTypes.Margin = new System.Windows.Forms.Padding(4);
            this.btnGetMetadataTypes.Name = "btnGetMetadataTypes";
            this.btnGetMetadataTypes.Size = new System.Drawing.Size(194, 46);
            this.btnGetMetadataTypes.TabIndex = 10;
            this.btnGetMetadataTypes.Text = "Get Metadata Types";
            this.btnGetMetadataTypes.UseVisualStyleBackColor = false;
            this.btnGetMetadataTypes.Click += new System.EventHandler(this.btnGetMetadataTypes_Click);
            // 
            // lblSFMetadataPackage
            // 
            this.lblSFMetadataPackage.AutoSize = true;
            this.lblSFMetadataPackage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSFMetadataPackage.Location = new System.Drawing.Point(49, 237);
            this.lblSFMetadataPackage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSFMetadataPackage.Name = "lblSFMetadataPackage";
            this.lblSFMetadataPackage.Size = new System.Drawing.Size(369, 17);
            this.lblSFMetadataPackage.TabIndex = 2;
            this.lblSFMetadataPackage.Text = "Select the Salesforce Metadata Types to Retrieve";
            // 
            // lbMetadataTypes
            // 
            this.lbMetadataTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbMetadataTypes.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMetadataTypes.FormattingEnabled = true;
            this.lbMetadataTypes.HorizontalScrollbar = true;
            this.lbMetadataTypes.Location = new System.Drawing.Point(52, 312);
            this.lbMetadataTypes.Margin = new System.Windows.Forms.Padding(4);
            this.lbMetadataTypes.Name = "lbMetadataTypes";
            this.lbMetadataTypes.Size = new System.Drawing.Size(720, 436);
            this.lbMetadataTypes.TabIndex = 9;
            // 
            // lblSFUsername
            // 
            this.lblSFUsername.AutoSize = true;
            this.lblSFUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSFUsername.Location = new System.Drawing.Point(10, 44);
            this.lblSFUsername.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSFUsername.Name = "lblSFUsername";
            this.lblSFUsername.Size = new System.Drawing.Size(143, 17);
            this.lblSFUsername.TabIndex = 1;
            this.lblSFUsername.Text = "Username (from Org)";
            // 
            // lblSalesforce
            // 
            this.lblSalesforce.AutoSize = true;
            this.lblSalesforce.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalesforce.Location = new System.Drawing.Point(22, 25);
            this.lblSalesforce.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSalesforce.Name = "lblSalesforce";
            this.lblSalesforce.Size = new System.Drawing.Size(133, 29);
            this.lblSalesforce.TabIndex = 0;
            this.lblSalesforce.Text = "Salesforce";
            // 
            // cmbUserName
            // 
            this.cmbUserName.DropDownWidth = 325;
            this.cmbUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbUserName.FormattingEnabled = true;
            this.cmbUserName.Location = new System.Drawing.Point(161, 41);
            this.cmbUserName.Margin = new System.Windows.Forms.Padding(4);
            this.cmbUserName.Name = "cmbUserName";
            this.cmbUserName.Size = new System.Drawing.Size(499, 24);
            this.cmbUserName.TabIndex = 2;
            this.cmbUserName.SelectedIndexChanged += new System.EventHandler(this.cmbUserName_SelectedIndexChanged);
            // 
            // btnRetrieveMetadata
            // 
            this.btnRetrieveMetadata.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnRetrieveMetadata.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRetrieveMetadata.Location = new System.Drawing.Point(1045, 312);
            this.btnRetrieveMetadata.Margin = new System.Windows.Forms.Padding(4);
            this.btnRetrieveMetadata.Name = "btnRetrieveMetadata";
            this.btnRetrieveMetadata.Size = new System.Drawing.Size(194, 46);
            this.btnRetrieveMetadata.TabIndex = 11;
            this.btnRetrieveMetadata.Text = "Retrieve Metadata Package";
            this.btnRetrieveMetadata.UseVisualStyleBackColor = false;
            this.btnRetrieveMetadata.Click += new System.EventHandler(this.btnRetrieveMetadata_Click);
            // 
            // fromOrgGroup
            // 
            this.fromOrgGroup.BackColor = System.Drawing.SystemColors.Control;
            this.fromOrgGroup.Controls.Add(this.lblSFUsername);
            this.fromOrgGroup.Controls.Add(this.cmbUserName);
            this.fromOrgGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fromOrgGroup.Location = new System.Drawing.Point(52, 60);
            this.fromOrgGroup.Margin = new System.Windows.Forms.Padding(6);
            this.fromOrgGroup.Name = "fromOrgGroup";
            this.fromOrgGroup.Padding = new System.Windows.Forms.Padding(6);
            this.fromOrgGroup.Size = new System.Drawing.Size(731, 106);
            this.fromOrgGroup.TabIndex = 1;
            this.fromOrgGroup.TabStop = false;
            this.fromOrgGroup.Text = "From Org";
            // 
            // btnSobjectFieldInspector
            // 
            this.btnSobjectFieldInspector.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnSobjectFieldInspector.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSobjectFieldInspector.Location = new System.Drawing.Point(1045, 368);
            this.btnSobjectFieldInspector.Margin = new System.Windows.Forms.Padding(6);
            this.btnSobjectFieldInspector.Name = "btnSobjectFieldInspector";
            this.btnSobjectFieldInspector.Size = new System.Drawing.Size(194, 46);
            this.btnSobjectFieldInspector.TabIndex = 13;
            this.btnSobjectFieldInspector.Text = "Sobject Field Inspector";
            this.btnSobjectFieldInspector.UseVisualStyleBackColor = false;
            this.btnSobjectFieldInspector.Click += new System.EventHandler(this.btnSobjectFieldInspector_Click);
            // 
            // btnGenerateToolingReport
            // 
            this.btnGenerateToolingReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateToolingReport.Location = new System.Drawing.Point(813, 423);
            this.btnGenerateToolingReport.Margin = new System.Windows.Forms.Padding(6);
            this.btnGenerateToolingReport.Name = "btnGenerateToolingReport";
            this.btnGenerateToolingReport.Size = new System.Drawing.Size(194, 46);
            this.btnGenerateToolingReport.TabIndex = 14;
            this.btnGenerateToolingReport.Text = "Generate Tooling Report";
            this.btnGenerateToolingReport.UseVisualStyleBackColor = true;
            this.btnGenerateToolingReport.Click += new System.EventHandler(this.btnGenerateToolingReport_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectAll.Location = new System.Drawing.Point(65, 273);
            this.btnSelectAll.Margin = new System.Windows.Forms.Padding(6);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(26, 25);
            this.btnSelectAll.TabIndex = 3;
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // lblSelectAll
            // 
            this.lblSelectAll.AutoSize = true;
            this.lblSelectAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectAll.Location = new System.Drawing.Point(103, 277);
            this.lblSelectAll.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblSelectAll.Name = "lblSelectAll";
            this.lblSelectAll.Size = new System.Drawing.Size(66, 17);
            this.lblSelectAll.TabIndex = 4;
            this.lblSelectAll.Text = "Select All";
            // 
            // btnSelectNone
            // 
            this.btnSelectNone.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectNone.Location = new System.Drawing.Point(211, 273);
            this.btnSelectNone.Margin = new System.Windows.Forms.Padding(6);
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.Size = new System.Drawing.Size(23, 25);
            this.btnSelectNone.TabIndex = 5;
            this.btnSelectNone.UseVisualStyleBackColor = true;
            this.btnSelectNone.Click += new System.EventHandler(this.btnSelectNone_Click);
            // 
            // lblSelectNone
            // 
            this.lblSelectNone.AutoSize = true;
            this.lblSelectNone.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectNone.Location = new System.Drawing.Point(246, 277);
            this.lblSelectNone.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblSelectNone.Name = "lblSelectNone";
            this.lblSelectNone.Size = new System.Drawing.Size(85, 17);
            this.lblSelectNone.TabIndex = 6;
            this.lblSelectNone.Text = "Select None";
            // 
            // btnSelectDefaults
            // 
            this.btnSelectDefaults.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectDefaults.Location = new System.Drawing.Point(367, 273);
            this.btnSelectDefaults.Margin = new System.Windows.Forms.Padding(6);
            this.btnSelectDefaults.Name = "btnSelectDefaults";
            this.btnSelectDefaults.Size = new System.Drawing.Size(26, 25);
            this.btnSelectDefaults.TabIndex = 7;
            this.btnSelectDefaults.UseVisualStyleBackColor = true;
            this.btnSelectDefaults.Click += new System.EventHandler(this.btnSelectDefaults_Click);
            // 
            // lblSelectDefaults
            // 
            this.lblSelectDefaults.AutoSize = true;
            this.lblSelectDefaults.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectDefaults.Location = new System.Drawing.Point(405, 277);
            this.lblSelectDefaults.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblSelectDefaults.Name = "lblSelectDefaults";
            this.lblSelectDefaults.Size = new System.Drawing.Size(103, 17);
            this.lblSelectDefaults.TabIndex = 8;
            this.lblSelectDefaults.Text = "Select Defaults";
            // 
            // btnExportMetadataTypes
            // 
            this.btnExportMetadataTypes.Enabled = false;
            this.btnExportMetadataTypes.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportMetadataTypes.Location = new System.Drawing.Point(813, 365);
            this.btnExportMetadataTypes.Margin = new System.Windows.Forms.Padding(6);
            this.btnExportMetadataTypes.Name = "btnExportMetadataTypes";
            this.btnExportMetadataTypes.Size = new System.Drawing.Size(194, 46);
            this.btnExportMetadataTypes.TabIndex = 12;
            this.btnExportMetadataTypes.Text = "Export Metadata Types";
            this.btnExportMetadataTypes.UseVisualStyleBackColor = true;
            this.btnExportMetadataTypes.Click += new System.EventHandler(this.btnExportMetadataTypes_Click);
            // 
            // SalesforceMetadataStep1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(1254, 771);
            this.Controls.Add(this.btnExportMetadataTypes);
            this.Controls.Add(this.lblSelectDefaults);
            this.Controls.Add(this.btnSelectDefaults);
            this.Controls.Add(this.lblSelectNone);
            this.Controls.Add(this.btnSelectNone);
            this.Controls.Add(this.lblSelectAll);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.btnGenerateToolingReport);
            this.Controls.Add(this.btnSobjectFieldInspector);
            this.Controls.Add(this.fromOrgGroup);
            this.Controls.Add(this.btnRetrieveMetadata);
            this.Controls.Add(this.lblSalesforce);
            this.Controls.Add(this.lbMetadataTypes);
            this.Controls.Add(this.lblSFMetadataPackage);
            this.Controls.Add(this.btnGetMetadataTypes);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SalesforceMetadataStep1";
            this.Text = "Salesforce Metadata";
            this.fromOrgGroup.ResumeLayout(false);
            this.fromOrgGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnGetMetadataTypes;
        private System.Windows.Forms.Label lblSFMetadataPackage;
        private System.Windows.Forms.CheckedListBox lbMetadataTypes;
        private System.Windows.Forms.Label lblSFUsername;
        private System.Windows.Forms.Label lblSalesforce;
        private System.Windows.Forms.ComboBox cmbUserName;
        private System.Windows.Forms.Button btnRetrieveMetadata;
        private System.Windows.Forms.GroupBox fromOrgGroup;
        private System.Windows.Forms.Button btnSobjectFieldInspector;
        private System.Windows.Forms.Button btnGenerateToolingReport;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Label lblSelectAll;
        private System.Windows.Forms.Button btnSelectNone;
        private System.Windows.Forms.Label lblSelectNone;
        private System.Windows.Forms.Button btnSelectDefaults;
        private System.Windows.Forms.Label lblSelectDefaults;
        private System.Windows.Forms.Button btnExportMetadataTypes;
    }
}

