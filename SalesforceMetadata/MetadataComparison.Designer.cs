namespace SalesforceMetadata
{
    partial class MetadataComparison
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MetadataComparison));
            this.lblPackageDifferences = new System.Windows.Forms.Label();
            this.RunComparison = new System.Windows.Forms.Button();
            this.tbFromFolder = new System.Windows.Forms.TextBox();
            this.tbToFolder = new System.Windows.Forms.TextBox();
            this.lblFromFolder = new System.Windows.Forms.Label();
            this.lblToFolder = new System.Windows.Forms.Label();
            this.GenerateDeploymentPackage = new System.Windows.Forms.Button();
            this.treeViewDifferences = new System.Windows.Forms.TreeView();
            this.cbExportXML = new System.Windows.Forms.CheckBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.cmbExportType = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lblPackageDifferences
            // 
            this.lblPackageDifferences.AutoSize = true;
            this.lblPackageDifferences.Location = new System.Drawing.Point(12, 194);
            this.lblPackageDifferences.Name = "lblPackageDifferences";
            this.lblPackageDifferences.Size = new System.Drawing.Size(61, 13);
            this.lblPackageDifferences.TabIndex = 9;
            this.lblPackageDifferences.Text = "Differences";
            // 
            // RunComparison
            // 
            this.RunComparison.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RunComparison.Location = new System.Drawing.Point(197, 100);
            this.RunComparison.Name = "RunComparison";
            this.RunComparison.Size = new System.Drawing.Size(182, 23);
            this.RunComparison.TabIndex = 4;
            this.RunComparison.Text = "Run Comparison";
            this.RunComparison.UseVisualStyleBackColor = true;
            this.RunComparison.Click += new System.EventHandler(this.RunComparison_Click);
            // 
            // tbFromFolder
            // 
            this.tbFromFolder.Location = new System.Drawing.Point(197, 16);
            this.tbFromFolder.Name = "tbFromFolder";
            this.tbFromFolder.Size = new System.Drawing.Size(611, 20);
            this.tbFromFolder.TabIndex = 1;
            this.tbFromFolder.DoubleClick += new System.EventHandler(this.tbFromFolder_DoubleClick);
            // 
            // tbToFolder
            // 
            this.tbToFolder.Location = new System.Drawing.Point(197, 54);
            this.tbToFolder.Name = "tbToFolder";
            this.tbToFolder.Size = new System.Drawing.Size(611, 20);
            this.tbToFolder.TabIndex = 3;
            this.tbToFolder.Tag = "Most of the time this will be your production org";
            this.tbToFolder.DoubleClick += new System.EventHandler(this.tbToFolder_DoubleClick);
            // 
            // lblFromFolder
            // 
            this.lblFromFolder.AutoSize = true;
            this.lblFromFolder.Location = new System.Drawing.Point(12, 19);
            this.lblFromFolder.Name = "lblFromFolder";
            this.lblFromFolder.Size = new System.Drawing.Size(136, 13);
            this.lblFromFolder.TabIndex = 0;
            this.lblFromFolder.Tag = "Most of the time this will be from your Sandbox Org";
            this.lblFromFolder.Text = "Read Changes From Folder";
            // 
            // lblToFolder
            // 
            this.lblToFolder.AutoSize = true;
            this.lblToFolder.Location = new System.Drawing.Point(12, 57);
            this.lblToFolder.Name = "lblToFolder";
            this.lblToFolder.Size = new System.Drawing.Size(97, 13);
            this.lblToFolder.TabIndex = 2;
            this.lblToFolder.Text = "Compare-To Folder";
            // 
            // GenerateDeploymentPackage
            // 
            this.GenerateDeploymentPackage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GenerateDeploymentPackage.Location = new System.Drawing.Point(1123, 164);
            this.GenerateDeploymentPackage.Name = "GenerateDeploymentPackage";
            this.GenerateDeploymentPackage.Size = new System.Drawing.Size(182, 23);
            this.GenerateDeploymentPackage.TabIndex = 11;
            this.GenerateDeploymentPackage.Text = "Generate Deployment Package";
            this.GenerateDeploymentPackage.UseVisualStyleBackColor = true;
            this.GenerateDeploymentPackage.Click += new System.EventHandler(this.GenerateDeploymentPackage_Click);
            // 
            // treeViewDifferences
            // 
            this.treeViewDifferences.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewDifferences.CheckBoxes = true;
            this.treeViewDifferences.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeViewDifferences.Location = new System.Drawing.Point(12, 210);
            this.treeViewDifferences.Name = "treeViewDifferences";
            this.treeViewDifferences.Size = new System.Drawing.Size(1293, 578);
            this.treeViewDifferences.TabIndex = 10;
            this.treeViewDifferences.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewDifference_AfterCheck);
            // 
            // cbExportXML
            // 
            this.cbExportXML.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbExportXML.AutoSize = true;
            this.cbExportXML.Checked = true;
            this.cbExportXML.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbExportXML.Enabled = false;
            this.cbExportXML.Location = new System.Drawing.Point(401, 168);
            this.cbExportXML.Name = "cbExportXML";
            this.cbExportXML.Size = new System.Drawing.Size(73, 17);
            this.cbExportXML.TabIndex = 7;
            this.cbExportXML.Text = "With XML";
            this.cbExportXML.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cbExportXML.UseVisualStyleBackColor = true;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(531, 160);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(182, 24);
            this.btnExport.TabIndex = 5;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // cmbExportType
            // 
            this.cmbExportType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbExportType.FormattingEnabled = true;
            this.cmbExportType.Items.AddRange(new object[] {
            "Export All to Excel",
            "Export All to HTML",
            "Export All to CSV",
            "Export Selected to CSV",
            "Export Selected to Excel"});
            this.cmbExportType.Location = new System.Drawing.Point(197, 163);
            this.cmbExportType.Name = "cmbExportType";
            this.cmbExportType.Size = new System.Drawing.Size(182, 21);
            this.cmbExportType.TabIndex = 13;
            this.cmbExportType.Text = "Export All to Excel";
            // 
            // MetadataComparison
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1317, 800);
            this.Controls.Add(this.cmbExportType);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.cbExportXML);
            this.Controls.Add(this.treeViewDifferences);
            this.Controls.Add(this.GenerateDeploymentPackage);
            this.Controls.Add(this.lblToFolder);
            this.Controls.Add(this.lblFromFolder);
            this.Controls.Add(this.tbToFolder);
            this.Controls.Add(this.tbFromFolder);
            this.Controls.Add(this.RunComparison);
            this.Controls.Add(this.lblPackageDifferences);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MetadataComparison";
            this.Text = "Comparison Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblPackageDifferences;
        private System.Windows.Forms.Button RunComparison;
        private System.Windows.Forms.Label lblFromFolder;
        private System.Windows.Forms.Label lblToFolder;
        private System.Windows.Forms.Button GenerateDeploymentPackage;
        public System.Windows.Forms.TextBox tbFromFolder;
        public System.Windows.Forms.TextBox tbToFolder;
        private System.Windows.Forms.TreeView treeViewDifferences;
        private System.Windows.Forms.CheckBox cbExportXML;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.ComboBox cmbExportType;
    }
}