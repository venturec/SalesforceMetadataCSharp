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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenerateDeploymentPackage));
            this.treeViewMetadata = new System.Windows.Forms.TreeView();
            this.tbDeployFrom = new System.Windows.Forms.TextBox();
            this.lblDeploymentFolder = new System.Windows.Forms.Label();
            this.tbProjectFolder = new System.Windows.Forms.TextBox();
            this.lblProjectFolder = new System.Windows.Forms.Label();
            this.btnBuildZipFile = new System.Windows.Forms.Button();
            this.btnBuildProfilesAndPermissionSets = new System.Windows.Forms.Button();
            this.tbInformation = new System.Windows.Forms.TextBox();
            this.cmbDestructiveChange = new System.Windows.Forms.ComboBox();
            this.lblDestructiveChangeType = new System.Windows.Forms.Label();
            this.lblDestructiveChangesFirst = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.lblDefaultAPI = new System.Windows.Forms.Label();
            this.cmbDefaultAPI = new System.Windows.Forms.ComboBox();
            this.tbOutboundChangeSetName = new System.Windows.Forms.TextBox();
            this.lblOutboundChangeSetName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // treeViewMetadata
            // 
            this.treeViewMetadata.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewMetadata.CheckBoxes = true;
            this.treeViewMetadata.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeViewMetadata.Location = new System.Drawing.Point(15, 209);
            this.treeViewMetadata.Name = "treeViewMetadata";
            this.treeViewMetadata.Size = new System.Drawing.Size(1267, 604);
            this.treeViewMetadata.TabIndex = 9;
            this.treeViewMetadata.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewMetadata_AfterCheck);
            // 
            // tbDeployFrom
            // 
            this.tbDeployFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbDeployFrom.Location = new System.Drawing.Point(312, 44);
            this.tbDeployFrom.Name = "tbDeployFrom";
            this.tbDeployFrom.Size = new System.Drawing.Size(607, 23);
            this.tbDeployFrom.TabIndex = 3;
            this.tbDeployFrom.DoubleClick += new System.EventHandler(this.tbDeploymentPackageLocation_DoubleClick);
            // 
            // lblDeploymentFolder
            // 
            this.lblDeploymentFolder.AutoSize = true;
            this.lblDeploymentFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeploymentFolder.Location = new System.Drawing.Point(12, 47);
            this.lblDeploymentFolder.Name = "lblDeploymentFolder";
            this.lblDeploymentFolder.Size = new System.Drawing.Size(144, 17);
            this.lblDeploymentFolder.TabIndex = 2;
            this.lblDeploymentFolder.Text = "Deployment Folder";
            // 
            // tbProjectFolder
            // 
            this.tbProjectFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbProjectFolder.Location = new System.Drawing.Point(312, 12);
            this.tbProjectFolder.Name = "tbProjectFolder";
            this.tbProjectFolder.Size = new System.Drawing.Size(607, 23);
            this.tbProjectFolder.TabIndex = 1;
            this.tbProjectFolder.DoubleClick += new System.EventHandler(this.tbMetadataFolderToReadFrom_DoubleClick);
            // 
            // lblProjectFolder
            // 
            this.lblProjectFolder.AutoSize = true;
            this.lblProjectFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProjectFolder.Location = new System.Drawing.Point(12, 15);
            this.lblProjectFolder.Name = "lblProjectFolder";
            this.lblProjectFolder.Size = new System.Drawing.Size(110, 17);
            this.lblProjectFolder.TabIndex = 0;
            this.lblProjectFolder.Text = "Project Folder";
            // 
            // btnBuildZipFile
            // 
            this.btnBuildZipFile.Location = new System.Drawing.Point(880, 146);
            this.btnBuildZipFile.Name = "btnBuildZipFile";
            this.btnBuildZipFile.Size = new System.Drawing.Size(215, 23);
            this.btnBuildZipFile.TabIndex = 13;
            this.btnBuildZipFile.Text = "Build Zip File and Deploy";
            this.btnBuildZipFile.UseVisualStyleBackColor = true;
            this.btnBuildZipFile.Click += new System.EventHandler(this.btnBuildPackageXml_Click);
            // 
            // btnBuildProfilesAndPermissionSets
            // 
            this.btnBuildProfilesAndPermissionSets.Location = new System.Drawing.Point(15, 146);
            this.btnBuildProfilesAndPermissionSets.Name = "btnBuildProfilesAndPermissionSets";
            this.btnBuildProfilesAndPermissionSets.Size = new System.Drawing.Size(215, 23);
            this.btnBuildProfilesAndPermissionSets.TabIndex = 10;
            this.btnBuildProfilesAndPermissionSets.Text = "Build Profiles and Permission Sets";
            this.btnBuildProfilesAndPermissionSets.UseVisualStyleBackColor = true;
            this.btnBuildProfilesAndPermissionSets.Click += new System.EventHandler(this.btnBuildProfilesAndPermissionSets_Click);
            // 
            // tbInformation
            // 
            this.tbInformation.Location = new System.Drawing.Point(1004, 15);
            this.tbInformation.Multiline = true;
            this.tbInformation.Name = "tbInformation";
            this.tbInformation.ReadOnly = true;
            this.tbInformation.Size = new System.Drawing.Size(278, 58);
            this.tbInformation.TabIndex = 15;
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
            this.cmbDestructiveChange.Location = new System.Drawing.Point(648, 146);
            this.cmbDestructiveChange.Name = "cmbDestructiveChange";
            this.cmbDestructiveChange.Size = new System.Drawing.Size(206, 21);
            this.cmbDestructiveChange.TabIndex = 12;
            this.cmbDestructiveChange.Text = "--none--";
            this.cmbDestructiveChange.SelectedIndexChanged += new System.EventHandler(this.cmbDestructiveChange_SelectedIndexChanged);
            // 
            // lblDestructiveChangeType
            // 
            this.lblDestructiveChangeType.AutoSize = true;
            this.lblDestructiveChangeType.Location = new System.Drawing.Point(514, 150);
            this.lblDestructiveChangeType.Name = "lblDestructiveChangeType";
            this.lblDestructiveChangeType.Size = new System.Drawing.Size(128, 13);
            this.lblDestructiveChangeType.TabIndex = 11;
            this.lblDestructiveChangeType.Text = "Destructive Change Type";
            // 
            // lblDestructiveChangesFirst
            // 
            this.lblDestructiveChangesFirst.AutoSize = true;
            this.lblDestructiveChangesFirst.BackColor = System.Drawing.SystemColors.Window;
            this.lblDestructiveChangesFirst.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDestructiveChangesFirst.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.lblDestructiveChangesFirst.Location = new System.Drawing.Point(12, 188);
            this.lblDestructiveChangesFirst.Name = "lblDestructiveChangesFirst";
            this.lblDestructiveChangesFirst.Size = new System.Drawing.Size(732, 18);
            this.lblDestructiveChangesFirst.TabIndex = 8;
            this.lblDestructiveChangesFirst.Text = "Select your destructive changes first, then click Next to select your Pre / Post " +
    "Deployment Items";
            this.lblDestructiveChangesFirst.Visible = false;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(880, 176);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(215, 23);
            this.btnNext.TabIndex = 14;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Visible = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lblDefaultAPI
            // 
            this.lblDefaultAPI.AutoSize = true;
            this.lblDefaultAPI.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDefaultAPI.Location = new System.Drawing.Point(12, 106);
            this.lblDefaultAPI.Name = "lblDefaultAPI";
            this.lblDefaultAPI.Size = new System.Drawing.Size(97, 17);
            this.lblDefaultAPI.TabIndex = 6;
            this.lblDefaultAPI.Text = "Default APIs";
            // 
            // cmbDefaultAPI
            // 
            this.cmbDefaultAPI.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbDefaultAPI.FormattingEnabled = true;
            this.cmbDefaultAPI.Location = new System.Drawing.Point(312, 103);
            this.cmbDefaultAPI.Name = "cmbDefaultAPI";
            this.cmbDefaultAPI.Size = new System.Drawing.Size(121, 24);
            this.cmbDefaultAPI.TabIndex = 7;
            // 
            // tbOutboundChangeSetName
            // 
            this.tbOutboundChangeSetName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbOutboundChangeSetName.Location = new System.Drawing.Point(312, 73);
            this.tbOutboundChangeSetName.Name = "tbOutboundChangeSetName";
            this.tbOutboundChangeSetName.Size = new System.Drawing.Size(607, 23);
            this.tbOutboundChangeSetName.TabIndex = 5;
            this.tbOutboundChangeSetName.MouseHover += new System.EventHandler(this.tbOutboundChangeSetName_MouseHover);
            // 
            // lblOutboundChangeSetName
            // 
            this.lblOutboundChangeSetName.AutoSize = true;
            this.lblOutboundChangeSetName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutboundChangeSetName.Location = new System.Drawing.Point(12, 76);
            this.lblOutboundChangeSetName.Name = "lblOutboundChangeSetName";
            this.lblOutboundChangeSetName.Size = new System.Drawing.Size(214, 17);
            this.lblOutboundChangeSetName.TabIndex = 4;
            this.lblOutboundChangeSetName.Text = "Outbound Change Set Name";
            // 
            // GenerateDeploymentPackage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1294, 825);
            this.Controls.Add(this.lblOutboundChangeSetName);
            this.Controls.Add(this.tbOutboundChangeSetName);
            this.Controls.Add(this.cmbDefaultAPI);
            this.Controls.Add(this.lblDefaultAPI);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.lblDestructiveChangesFirst);
            this.Controls.Add(this.lblDestructiveChangeType);
            this.Controls.Add(this.cmbDestructiveChange);
            this.Controls.Add(this.tbInformation);
            this.Controls.Add(this.btnBuildProfilesAndPermissionSets);
            this.Controls.Add(this.btnBuildZipFile);
            this.Controls.Add(this.lblProjectFolder);
            this.Controls.Add(this.tbProjectFolder);
            this.Controls.Add(this.lblDeploymentFolder);
            this.Controls.Add(this.tbDeployFrom);
            this.Controls.Add(this.treeViewMetadata);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GenerateDeploymentPackage";
            this.Text = "GenerateDeploymentPackage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TreeView treeViewMetadata;
        public System.Windows.Forms.TextBox tbDeployFrom;
        private System.Windows.Forms.Label lblDeploymentFolder;
        public System.Windows.Forms.TextBox tbProjectFolder;
        private System.Windows.Forms.Label lblProjectFolder;
        private System.Windows.Forms.Button btnBuildZipFile;
        private System.Windows.Forms.Button btnBuildProfilesAndPermissionSets;
        private System.Windows.Forms.TextBox tbInformation;
        private System.Windows.Forms.ComboBox cmbDestructiveChange;
        private System.Windows.Forms.Label lblDestructiveChangeType;
        private System.Windows.Forms.Label lblDestructiveChangesFirst;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblDefaultAPI;
        private System.Windows.Forms.ComboBox cmbDefaultAPI;
        private System.Windows.Forms.TextBox tbOutboundChangeSetName;
        private System.Windows.Forms.Label lblOutboundChangeSetName;
    }
}