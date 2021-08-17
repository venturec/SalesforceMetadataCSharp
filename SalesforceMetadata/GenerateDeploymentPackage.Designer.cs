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
            this.treeViewFiltered = new System.Windows.Forms.TreeView();
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
            // treeViewFiltered
            // 
            this.treeViewFiltered.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewFiltered.CheckBoxes = true;
            this.treeViewFiltered.Location = new System.Drawing.Point(15, 174);
            this.treeViewFiltered.Name = "treeViewFiltered";
            this.treeViewFiltered.Size = new System.Drawing.Size(1266, 593);
            this.treeViewFiltered.TabIndex = 7;
            this.treeViewFiltered.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewFiltered_AfterCheck);
            // 
            // tbDeploymentPackageLocation
            // 
            this.tbDeploymentPackageLocation.Location = new System.Drawing.Point(246, 38);
            this.tbDeploymentPackageLocation.Name = "tbDeploymentPackageLocation";
            this.tbDeploymentPackageLocation.Size = new System.Drawing.Size(518, 20);
            this.tbDeploymentPackageLocation.TabIndex = 3;
            this.tbDeploymentPackageLocation.DoubleClick += new System.EventHandler(this.tbDeploymentPackageLocation_DoubleClick);
            // 
            // lblDeploymentFolder
            // 
            this.lblDeploymentFolder.AutoSize = true;
            this.lblDeploymentFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeploymentFolder.Location = new System.Drawing.Point(12, 41);
            this.lblDeploymentFolder.Name = "lblDeploymentFolder";
            this.lblDeploymentFolder.Size = new System.Drawing.Size(112, 13);
            this.lblDeploymentFolder.TabIndex = 2;
            this.lblDeploymentFolder.Text = "Deployment Folder";
            // 
            // tbMetadataFolderToReadFrom
            // 
            this.tbMetadataFolderToReadFrom.Location = new System.Drawing.Point(246, 12);
            this.tbMetadataFolderToReadFrom.Name = "tbMetadataFolderToReadFrom";
            this.tbMetadataFolderToReadFrom.Size = new System.Drawing.Size(518, 20);
            this.tbMetadataFolderToReadFrom.TabIndex = 1;
            this.tbMetadataFolderToReadFrom.DoubleClick += new System.EventHandler(this.tbMetadataFolderToReadFrom_DoubleClick);
            // 
            // lblMetadataToReadFrom
            // 
            this.lblMetadataToReadFrom.AutoSize = true;
            this.lblMetadataToReadFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMetadataToReadFrom.Location = new System.Drawing.Point(12, 15);
            this.lblMetadataToReadFrom.Name = "lblMetadataToReadFrom";
            this.lblMetadataToReadFrom.Size = new System.Drawing.Size(228, 13);
            this.lblMetadataToReadFrom.TabIndex = 0;
            this.lblMetadataToReadFrom.Text = "Metadata Parent Folder (to Read From)";
            // 
            // btnBuildPackageXml
            // 
            this.btnBuildPackageXml.Location = new System.Drawing.Point(1066, 111);
            this.btnBuildPackageXml.Name = "btnBuildPackageXml";
            this.btnBuildPackageXml.Size = new System.Drawing.Size(215, 23);
            this.btnBuildPackageXml.TabIndex = 11;
            this.btnBuildPackageXml.Text = "Build Package XML";
            this.btnBuildPackageXml.UseVisualStyleBackColor = true;
            this.btnBuildPackageXml.Click += new System.EventHandler(this.btnBuildPackageXml_Click);
            // 
            // btnBuildProfilesAndPermissionSets
            // 
            this.btnBuildProfilesAndPermissionSets.Location = new System.Drawing.Point(15, 111);
            this.btnBuildProfilesAndPermissionSets.Name = "btnBuildProfilesAndPermissionSets";
            this.btnBuildProfilesAndPermissionSets.Size = new System.Drawing.Size(215, 23);
            this.btnBuildProfilesAndPermissionSets.TabIndex = 8;
            this.btnBuildProfilesAndPermissionSets.Text = "Build Profiles and Permission Sets";
            this.btnBuildProfilesAndPermissionSets.UseVisualStyleBackColor = true;
            this.btnBuildProfilesAndPermissionSets.Click += new System.EventHandler(this.btnBuildProfilesAndPermissionSets_Click);
            // 
            // tbInformation
            // 
            this.tbInformation.Location = new System.Drawing.Point(893, 12);
            this.tbInformation.Multiline = true;
            this.tbInformation.Name = "tbInformation";
            this.tbInformation.ReadOnly = true;
            this.tbInformation.Size = new System.Drawing.Size(388, 38);
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
            this.cmbDestructiveChange.Location = new System.Drawing.Point(842, 113);
            this.cmbDestructiveChange.Name = "cmbDestructiveChange";
            this.cmbDestructiveChange.Size = new System.Drawing.Size(206, 21);
            this.cmbDestructiveChange.TabIndex = 10;
            this.cmbDestructiveChange.Text = "--none--";
            this.cmbDestructiveChange.SelectedIndexChanged += new System.EventHandler(this.cmbDestructiveChange_SelectedIndexChanged);
            // 
            // lblDestructiveChangeType
            // 
            this.lblDestructiveChangeType.AutoSize = true;
            this.lblDestructiveChangeType.Location = new System.Drawing.Point(708, 116);
            this.lblDestructiveChangeType.Name = "lblDestructiveChangeType";
            this.lblDestructiveChangeType.Size = new System.Drawing.Size(128, 13);
            this.lblDestructiveChangeType.TabIndex = 9;
            this.lblDestructiveChangeType.Text = "Destructive Change Type";
            // 
            // lblDestructiveChangesFirst
            // 
            this.lblDestructiveChangesFirst.AutoSize = true;
            this.lblDestructiveChangesFirst.BackColor = System.Drawing.SystemColors.Window;
            this.lblDestructiveChangesFirst.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDestructiveChangesFirst.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.lblDestructiveChangesFirst.Location = new System.Drawing.Point(12, 153);
            this.lblDestructiveChangesFirst.Name = "lblDestructiveChangesFirst";
            this.lblDestructiveChangesFirst.Size = new System.Drawing.Size(732, 18);
            this.lblDestructiveChangesFirst.TabIndex = 6;
            this.lblDestructiveChangesFirst.Text = "Select your destructive changes first, then click Next to select your Pre / Post " +
    "Deployment Items";
            this.lblDestructiveChangesFirst.Visible = false;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(1066, 141);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(215, 23);
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
            this.lblDefaultAPI.Location = new System.Drawing.Point(12, 69);
            this.lblDefaultAPI.Name = "lblDefaultAPI";
            this.lblDefaultAPI.Size = new System.Drawing.Size(78, 13);
            this.lblDefaultAPI.TabIndex = 4;
            this.lblDefaultAPI.Text = "Default APIs";
            // 
            // cmbDefaultAPI
            // 
            this.cmbDefaultAPI.FormattingEnabled = true;
            this.cmbDefaultAPI.Location = new System.Drawing.Point(246, 65);
            this.cmbDefaultAPI.Name = "cmbDefaultAPI";
            this.cmbDefaultAPI.Size = new System.Drawing.Size(121, 21);
            this.cmbDefaultAPI.TabIndex = 5;
            // 
            // GenerateDeploymentPackage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1293, 778);
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
            this.Controls.Add(this.treeViewFiltered);
            this.Name = "GenerateDeploymentPackage";
            this.Text = "GenerateDeploymentPackage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TreeView treeViewFiltered;
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