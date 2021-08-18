namespace SalesforceMetadata
{
    partial class GenerateDeploymentPackage
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
            this.treeViewMetadata = new System.Windows.Forms.TreeView();
            this.tbDeploymentPackageLocation = new System.Windows.Forms.TextBox();
            this.lblDeploymentFolder = new System.Windows.Forms.Label();
            this.tbMetadataFolderToReadFrom = new System.Windows.Forms.TextBox();
            this.lblMetadataToReadFrom = new System.Windows.Forms.Label();
            this.btnBuildPackageXml = new System.Windows.Forms.Button();
            this.btnBuildProfilesAndPermissionSets = new System.Windows.Forms.Button();
            this.tbInformation = new System.Windows.Forms.TextBox();
            this.cmbDestructiveChange = new System.Windows.Forms.ComboBox();
            this.lblDestructiveChangeType = new System.Windows.Forms.Label();
            this.lblDestructiveChangesFirst = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.lblDefaultAPI = new System.Windows.Forms.Label();
            this.cmbDefaultAPI = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // treeViewMetadata
            // 
            this.treeViewMetadata.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewMetadata.CheckBoxes = true;
            this.treeViewMetadata.Location = new System.Drawing.Point(20, 214);
            this.treeViewMetadata.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.treeViewMetadata.Name = "treeViewMetadata";
            this.treeViewMetadata.Size = new System.Drawing.Size(1687, 729);
            this.treeViewMetadata.TabIndex = 7;
            this.treeViewMetadata.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewMetadata_AfterCheck);
            // 
            // tbDeploymentPackageLocation
            // 
            this.tbDeploymentPackageLocation.Location = new System.Drawing.Point(328, 47);
            this.tbDeploymentPackageLocation.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbDeploymentPackageLocation.Name = "tbDeploymentPackageLocation";
            this.tbDeploymentPackageLocation.Size = new System.Drawing.Size(689, 22);
            this.tbDeploymentPackageLocation.TabIndex = 3;
            this.tbDeploymentPackageLocation.DoubleClick += new System.EventHandler(this.tbDeploymentPackageLocation_DoubleClick);
            // 
            // lblDeploymentFolder
            // 
            this.lblDeploymentFolder.AutoSize = true;
            this.lblDeploymentFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeploymentFolder.Location = new System.Drawing.Point(16, 50);
            this.lblDeploymentFolder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDeploymentFolder.Name = "lblDeploymentFolder";
            this.lblDeploymentFolder.Size = new System.Drawing.Size(144, 17);
            this.lblDeploymentFolder.TabIndex = 2;
            this.lblDeploymentFolder.Text = "Deployment Folder";
            // 
            // tbMetadataFolderToReadFrom
            // 
            this.tbMetadataFolderToReadFrom.Location = new System.Drawing.Point(328, 15);
            this.tbMetadataFolderToReadFrom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbMetadataFolderToReadFrom.Name = "tbMetadataFolderToReadFrom";
            this.tbMetadataFolderToReadFrom.Size = new System.Drawing.Size(689, 22);
            this.tbMetadataFolderToReadFrom.TabIndex = 1;
            this.tbMetadataFolderToReadFrom.DoubleClick += new System.EventHandler(this.tbMetadataFolderToReadFrom_DoubleClick);
            // 
            // lblMetadataToReadFrom
            // 
            this.lblMetadataToReadFrom.AutoSize = true;
            this.lblMetadataToReadFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMetadataToReadFrom.Location = new System.Drawing.Point(16, 18);
            this.lblMetadataToReadFrom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMetadataToReadFrom.Name = "lblMetadataToReadFrom";
            this.lblMetadataToReadFrom.Size = new System.Drawing.Size(294, 17);
            this.lblMetadataToReadFrom.TabIndex = 0;
            this.lblMetadataToReadFrom.Text = "Metadata Parent Folder (to Read From)";
            // 
            // btnBuildPackageXml
            // 
            this.btnBuildPackageXml.Location = new System.Drawing.Point(1421, 137);
            this.btnBuildPackageXml.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBuildPackageXml.Name = "btnBuildPackageXml";
            this.btnBuildPackageXml.Size = new System.Drawing.Size(287, 28);
            this.btnBuildPackageXml.TabIndex = 11;
            this.btnBuildPackageXml.Text = "Build Package XML";
            this.btnBuildPackageXml.UseVisualStyleBackColor = true;
            this.btnBuildPackageXml.Click += new System.EventHandler(this.btnBuildPackageXml_Click);
            // 
            // btnBuildProfilesAndPermissionSets
            // 
            this.btnBuildProfilesAndPermissionSets.Location = new System.Drawing.Point(20, 137);
            this.btnBuildProfilesAndPermissionSets.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBuildProfilesAndPermissionSets.Name = "btnBuildProfilesAndPermissionSets";
            this.btnBuildProfilesAndPermissionSets.Size = new System.Drawing.Size(287, 28);
            this.btnBuildProfilesAndPermissionSets.TabIndex = 8;
            this.btnBuildProfilesAndPermissionSets.Text = "Build Profiles and Permission Sets";
            this.btnBuildProfilesAndPermissionSets.UseVisualStyleBackColor = true;
            this.btnBuildProfilesAndPermissionSets.Click += new System.EventHandler(this.btnBuildProfilesAndPermissionSets_Click);
            // 
            // tbInformation
            // 
            this.tbInformation.Location = new System.Drawing.Point(1191, 15);
            this.tbInformation.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbInformation.Multiline = true;
            this.tbInformation.Name = "tbInformation";
            this.tbInformation.ReadOnly = true;
            this.tbInformation.Size = new System.Drawing.Size(516, 46);
            this.tbInformation.TabIndex = 13;
            this.tbInformation.Text = "This uses a TreeView and the TreeView object has not kept up with the current dat" +
    "a intensive needs. Performance is very slow.";
            // 
            // cmbDestructiveChange
            // 
            this.cmbDestructiveChange.FormattingEnabled = true;
            this.cmbDestructiveChange.Items.AddRange(new object[] {
            "--none--",
            "destructiveChanges",
            "destructiveChangesPre",
            "destructiveChangesPost"});
            this.cmbDestructiveChange.Location = new System.Drawing.Point(1123, 139);
            this.cmbDestructiveChange.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbDestructiveChange.Name = "cmbDestructiveChange";
            this.cmbDestructiveChange.Size = new System.Drawing.Size(273, 24);
            this.cmbDestructiveChange.TabIndex = 10;
            this.cmbDestructiveChange.Text = "--none--";
            this.cmbDestructiveChange.SelectedIndexChanged += new System.EventHandler(this.cmbDestructiveChange_SelectedIndexChanged);
            // 
            // lblDestructiveChangeType
            // 
            this.lblDestructiveChangeType.AutoSize = true;
            this.lblDestructiveChangeType.Location = new System.Drawing.Point(944, 143);
            this.lblDestructiveChangeType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDestructiveChangeType.Name = "lblDestructiveChangeType";
            this.lblDestructiveChangeType.Size = new System.Drawing.Size(168, 17);
            this.lblDestructiveChangeType.TabIndex = 9;
            this.lblDestructiveChangeType.Text = "Destructive Change Type";
            // 
            // lblDestructiveChangesFirst
            // 
            this.lblDestructiveChangesFirst.AutoSize = true;
            this.lblDestructiveChangesFirst.BackColor = System.Drawing.SystemColors.Window;
            this.lblDestructiveChangesFirst.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDestructiveChangesFirst.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.lblDestructiveChangesFirst.Location = new System.Drawing.Point(16, 188);
            this.lblDestructiveChangesFirst.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDestructiveChangesFirst.Name = "lblDestructiveChangesFirst";
            this.lblDestructiveChangesFirst.Size = new System.Drawing.Size(895, 24);
            this.lblDestructiveChangesFirst.TabIndex = 6;
            this.lblDestructiveChangesFirst.Text = "Select your destructive changes first, then click Next to select your Pre / Post " +
    "Deployment Items";
            this.lblDestructiveChangesFirst.Visible = false;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(1421, 174);
            this.btnNext.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(287, 28);
            this.btnNext.TabIndex = 12;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Visible = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lblDefaultAPI
            // 
            this.lblDefaultAPI.AutoSize = true;
            this.lblDefaultAPI.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDefaultAPI.Location = new System.Drawing.Point(16, 85);
            this.lblDefaultAPI.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDefaultAPI.Name = "lblDefaultAPI";
            this.lblDefaultAPI.Size = new System.Drawing.Size(97, 17);
            this.lblDefaultAPI.TabIndex = 4;
            this.lblDefaultAPI.Text = "Default APIs";
            // 
            // cmbDefaultAPI
            // 
            this.cmbDefaultAPI.FormattingEnabled = true;
            this.cmbDefaultAPI.Location = new System.Drawing.Point(328, 80);
            this.cmbDefaultAPI.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbDefaultAPI.Name = "cmbDefaultAPI";
            this.cmbDefaultAPI.Size = new System.Drawing.Size(160, 24);
            this.cmbDefaultAPI.TabIndex = 5;
            // 
            // GenerateDeploymentPackage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1724, 958);
            this.Controls.Add(this.cmbDefaultAPI);
            this.Controls.Add(this.lblDefaultAPI);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.lblDestructiveChangesFirst);
            this.Controls.Add(this.lblDestructiveChangeType);
            this.Controls.Add(this.cmbDestructiveChange);
            this.Controls.Add(this.tbInformation);
            this.Controls.Add(this.btnBuildProfilesAndPermissionSets);
            this.Controls.Add(this.btnBuildPackageXml);
            this.Controls.Add(this.lblMetadataToReadFrom);
            this.Controls.Add(this.tbMetadataFolderToReadFrom);
            this.Controls.Add(this.lblDeploymentFolder);
            this.Controls.Add(this.tbDeploymentPackageLocation);
            this.Controls.Add(this.treeViewMetadata);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "GenerateDeploymentPackage";
            this.Text = "GenerateDeploymentPackage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TreeView treeViewMetadata;
        public System.Windows.Forms.TextBox tbDeploymentPackageLocation;
        private System.Windows.Forms.Label lblDeploymentFolder;
        public System.Windows.Forms.TextBox tbMetadataFolderToReadFrom;
        private System.Windows.Forms.Label lblMetadataToReadFrom;
        private System.Windows.Forms.Button btnBuildPackageXml;
        private System.Windows.Forms.Button btnBuildProfilesAndPermissionSets;
        private System.Windows.Forms.TextBox tbInformation;
        private System.Windows.Forms.ComboBox cmbDestructiveChange;
        private System.Windows.Forms.Label lblDestructiveChangeType;
        private System.Windows.Forms.Label lblDestructiveChangesFirst;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblDefaultAPI;
        private System.Windows.Forms.ComboBox cmbDefaultAPI;
    }
}