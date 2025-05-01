namespace SalesforceMetadata
{
    partial class DevelopmentEnvironment
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
            System.Windows.Forms.MenuStrip msIDE;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DevelopmentEnvironment));
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectSolutionFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectSolutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.loadRecentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addApexTriggerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addApexClassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addLightningWebComponentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addOtherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.visualforcePageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.visualforceComponentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblNote = new System.Windows.Forms.Label();
            this.treeViewMetadata = new System.Windows.Forms.TreeView();
            this.tbProjectFolder = new System.Windows.Forms.TextBox();
            this.lblProjectFolder = new System.Windows.Forms.Label();
            this.lblDeploymentFolder = new System.Windows.Forms.Label();
            this.tbDeployFrom = new System.Windows.Forms.TextBox();
            this.btnRetrieveFromOrg = new System.Windows.Forms.Button();
            this.btnDeployToOrg = new System.Windows.Forms.Button();
            this.fromOrgGroup = new System.Windows.Forms.GroupBox();
            this.lblSFUsername = new System.Windows.Forms.Label();
            this.cmbUserName = new System.Windows.Forms.ComboBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnObjectFieldInspector = new System.Windows.Forms.Button();
            this.lblRepository = new System.Windows.Forms.Label();
            this.tbRepository = new System.Windows.Forms.TextBox();
            this.btnBuildERD = new System.Windows.Forms.Button();
            this.lblRootFolder = new System.Windows.Forms.Label();
            this.tbRootFolder = new System.Windows.Forms.TextBox();
            this.btnSearchMetadata = new System.Windows.Forms.Button();
            this.btnDebugLogs = new System.Windows.Forms.Button();
            this.delMyDebugLogs = new System.Windows.Forms.Button();
            this.lblOutboundChangeSetName = new System.Windows.Forms.Label();
            this.tbOutboundChangeSetName = new System.Windows.Forms.TextBox();
            this.btnCopySelectedToRepository = new System.Windows.Forms.Button();
            msIDE = new System.Windows.Forms.MenuStrip();
            msIDE.SuspendLayout();
            this.fromOrgGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // msIDE
            // 
            msIDE.BackColor = System.Drawing.SystemColors.InactiveCaption;
            msIDE.Font = new System.Drawing.Font("Segoe UI", 10F);
            msIDE.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.addApexTriggerToolStripMenuItem,
            this.addApexClassToolStripMenuItem,
            this.addLightningWebComponentToolStripMenuItem,
            this.addOtherToolStripMenuItem});
            msIDE.Location = new System.Drawing.Point(0, 0);
            msIDE.Name = "msIDE";
            msIDE.Size = new System.Drawing.Size(1479, 27);
            msIDE.TabIndex = 20;
            msIDE.Text = "IDE Menu";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectSolutionFileToolStripMenuItem,
            this.newProjectToolStripMenuItem,
            this.saveProjectToolStripMenuItem,
            this.exitToolStripMenuItem,
            this.toolStripMenuItem2,
            this.loadRecentToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(41, 23);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // selectSolutionFileToolStripMenuItem
            // 
            this.selectSolutionFileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.projectSolutionToolStripMenuItem});
            this.selectSolutionFileToolStripMenuItem.Name = "selectSolutionFileToolStripMenuItem";
            this.selectSolutionFileToolStripMenuItem.Size = new System.Drawing.Size(252, 24);
            this.selectSolutionFileToolStripMenuItem.Text = "Open";
            // 
            // projectSolutionToolStripMenuItem
            // 
            this.projectSolutionToolStripMenuItem.Name = "projectSolutionToolStripMenuItem";
            this.projectSolutionToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.projectSolutionToolStripMenuItem.Text = "Project/Solution";
            this.projectSolutionToolStripMenuItem.Click += new System.EventHandler(this.projectSolutionToolStripMenuItem_Click);
            // 
            // newProjectToolStripMenuItem
            // 
            this.newProjectToolStripMenuItem.Name = "newProjectToolStripMenuItem";
            this.newProjectToolStripMenuItem.Size = new System.Drawing.Size(252, 24);
            this.newProjectToolStripMenuItem.Text = "New Project";
            this.newProjectToolStripMenuItem.Click += new System.EventHandler(this.newProjectToolStripMenuItem_Click);
            // 
            // saveProjectToolStripMenuItem
            // 
            this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
            this.saveProjectToolStripMenuItem.Size = new System.Drawing.Size(252, 24);
            this.saveProjectToolStripMenuItem.Text = "Save Project";
            this.saveProjectToolStripMenuItem.Click += new System.EventHandler(this.saveProjectToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(252, 24);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(252, 24);
            this.toolStripMenuItem2.Text = "-----------------------------";
            // 
            // loadRecentToolStripMenuItem
            // 
            this.loadRecentToolStripMenuItem.Name = "loadRecentToolStripMenuItem";
            this.loadRecentToolStripMenuItem.Size = new System.Drawing.Size(252, 24);
            this.loadRecentToolStripMenuItem.Text = "Load Recent Project";
            this.loadRecentToolStripMenuItem.Click += new System.EventHandler(this.loadRecentToolStripMenuItem_Click);
            // 
            // addApexTriggerToolStripMenuItem
            // 
            this.addApexTriggerToolStripMenuItem.Name = "addApexTriggerToolStripMenuItem";
            this.addApexTriggerToolStripMenuItem.Size = new System.Drawing.Size(126, 23);
            this.addApexTriggerToolStripMenuItem.Text = "Add Apex Trigger";
            this.addApexTriggerToolStripMenuItem.Click += new System.EventHandler(this.addApexTriggerToolStripMenuItem_Click);
            // 
            // addApexClassToolStripMenuItem
            // 
            this.addApexClassToolStripMenuItem.Name = "addApexClassToolStripMenuItem";
            this.addApexClassToolStripMenuItem.Size = new System.Drawing.Size(115, 23);
            this.addApexClassToolStripMenuItem.Text = "Add Apex Class";
            this.addApexClassToolStripMenuItem.Click += new System.EventHandler(this.addApexClassToolStripMenuItem_Click);
            // 
            // addLightningWebComponentToolStripMenuItem
            // 
            this.addLightningWebComponentToolStripMenuItem.Name = "addLightningWebComponentToolStripMenuItem";
            this.addLightningWebComponentToolStripMenuItem.Size = new System.Drawing.Size(216, 23);
            this.addLightningWebComponentToolStripMenuItem.Text = "Add Lightning Web Component";
            this.addLightningWebComponentToolStripMenuItem.Click += new System.EventHandler(this.addLightningWebComponentToolStripMenuItem_Click);
            // 
            // addOtherToolStripMenuItem
            // 
            this.addOtherToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.customObjectToolStripMenuItem,
            this.visualforcePageToolStripMenuItem,
            this.visualforceComponentToolStripMenuItem});
            this.addOtherToolStripMenuItem.Name = "addOtherToolStripMenuItem";
            this.addOtherToolStripMenuItem.Size = new System.Drawing.Size(86, 23);
            this.addOtherToolStripMenuItem.Text = "Add Other";
            // 
            // customObjectToolStripMenuItem
            // 
            this.customObjectToolStripMenuItem.Name = "customObjectToolStripMenuItem";
            this.customObjectToolStripMenuItem.Size = new System.Drawing.Size(221, 24);
            this.customObjectToolStripMenuItem.Text = "Custom Object";
            this.customObjectToolStripMenuItem.Click += new System.EventHandler(this.customObjectToolStripMenuItem_Click);
            // 
            // visualforcePageToolStripMenuItem
            // 
            this.visualforcePageToolStripMenuItem.Name = "visualforcePageToolStripMenuItem";
            this.visualforcePageToolStripMenuItem.Size = new System.Drawing.Size(221, 24);
            this.visualforcePageToolStripMenuItem.Text = "Visualforce Page";
            this.visualforcePageToolStripMenuItem.Click += new System.EventHandler(this.visualforcePageToolStripMenuItem_Click);
            // 
            // visualforceComponentToolStripMenuItem
            // 
            this.visualforceComponentToolStripMenuItem.Name = "visualforceComponentToolStripMenuItem";
            this.visualforceComponentToolStripMenuItem.Size = new System.Drawing.Size(221, 24);
            this.visualforceComponentToolStripMenuItem.Text = "Visualforce Component";
            this.visualforceComponentToolStripMenuItem.Click += new System.EventHandler(this.visualforceComponentToolStripMenuItem_Click);
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic) 
                | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNote.Location = new System.Drawing.Point(12, 41);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(883, 17);
            this.lblNote.TabIndex = 0;
            this.lblNote.Text = "This form allows you to use the editor of your choice, checks for changes, and se" +
    "lects the elements which have changed";
            // 
            // treeViewMetadata
            // 
            this.treeViewMetadata.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewMetadata.CheckBoxes = true;
            this.treeViewMetadata.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeViewMetadata.Location = new System.Drawing.Point(14, 310);
            this.treeViewMetadata.Name = "treeViewMetadata";
            this.treeViewMetadata.Size = new System.Drawing.Size(988, 639);
            this.treeViewMetadata.TabIndex = 14;
            this.treeViewMetadata.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewMetadata_AfterCheck);
            this.treeViewMetadata.Click += new System.EventHandler(this.treeViewMetadata_Click);
            this.treeViewMetadata.DoubleClick += new System.EventHandler(this.treeViewMetadata_DoubleClick);
            // 
            // tbProjectFolder
            // 
            this.tbProjectFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbProjectFolder.Location = new System.Drawing.Point(184, 80);
            this.tbProjectFolder.Name = "tbProjectFolder";
            this.tbProjectFolder.Size = new System.Drawing.Size(703, 23);
            this.tbProjectFolder.TabIndex = 2;
            this.tbProjectFolder.TextChanged += new System.EventHandler(this.tbProjectFolder_TextChanged);
            this.tbProjectFolder.DoubleClick += new System.EventHandler(this.tbProjectFolder_DoubleClick);
            // 
            // lblProjectFolder
            // 
            this.lblProjectFolder.AutoSize = true;
            this.lblProjectFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProjectFolder.Location = new System.Drawing.Point(16, 80);
            this.lblProjectFolder.Name = "lblProjectFolder";
            this.lblProjectFolder.Size = new System.Drawing.Size(110, 17);
            this.lblProjectFolder.TabIndex = 1;
            this.lblProjectFolder.Text = "Project Folder";
            // 
            // lblDeploymentFolder
            // 
            this.lblDeploymentFolder.AutoSize = true;
            this.lblDeploymentFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeploymentFolder.Location = new System.Drawing.Point(16, 112);
            this.lblDeploymentFolder.Name = "lblDeploymentFolder";
            this.lblDeploymentFolder.Size = new System.Drawing.Size(150, 17);
            this.lblDeploymentFolder.TabIndex = 3;
            this.lblDeploymentFolder.Text = "Deploy From Folder";
            // 
            // tbDeployFrom
            // 
            this.tbDeployFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbDeployFrom.Location = new System.Drawing.Point(184, 112);
            this.tbDeployFrom.Name = "tbDeployFrom";
            this.tbDeployFrom.Size = new System.Drawing.Size(703, 23);
            this.tbDeployFrom.TabIndex = 4;
            this.tbDeployFrom.TextChanged += new System.EventHandler(this.tbDeployFrom_TextChanged);
            this.tbDeployFrom.DoubleClick += new System.EventHandler(this.tbDeployFrom_DoubleClick);
            // 
            // btnRetrieveFromOrg
            // 
            this.btnRetrieveFromOrg.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRetrieveFromOrg.Location = new System.Drawing.Point(16, 262);
            this.btnRetrieveFromOrg.Name = "btnRetrieveFromOrg";
            this.btnRetrieveFromOrg.Size = new System.Drawing.Size(159, 34);
            this.btnRetrieveFromOrg.TabIndex = 11;
            this.btnRetrieveFromOrg.Text = "Retrieve from Org";
            this.btnRetrieveFromOrg.UseVisualStyleBackColor = true;
            this.btnRetrieveFromOrg.Click += new System.EventHandler(this.btnRetrieveFromOrg_Click);
            // 
            // btnDeployToOrg
            // 
            this.btnDeployToOrg.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeployToOrg.Location = new System.Drawing.Point(192, 262);
            this.btnDeployToOrg.Name = "btnDeployToOrg";
            this.btnDeployToOrg.Size = new System.Drawing.Size(159, 34);
            this.btnDeployToOrg.TabIndex = 12;
            this.btnDeployToOrg.Text = "Deploy to Org";
            this.btnDeployToOrg.UseVisualStyleBackColor = true;
            this.btnDeployToOrg.Click += new System.EventHandler(this.btnDeployToOrg_Click);
            // 
            // fromOrgGroup
            // 
            this.fromOrgGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fromOrgGroup.BackColor = System.Drawing.SystemColors.Control;
            this.fromOrgGroup.Controls.Add(this.lblSFUsername);
            this.fromOrgGroup.Controls.Add(this.cmbUserName);
            this.fromOrgGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fromOrgGroup.Location = new System.Drawing.Point(944, 70);
            this.fromOrgGroup.Name = "fromOrgGroup";
            this.fromOrgGroup.Size = new System.Drawing.Size(523, 81);
            this.fromOrgGroup.TabIndex = 21;
            this.fromOrgGroup.TabStop = false;
            this.fromOrgGroup.Text = "From Org";
            // 
            // lblSFUsername
            // 
            this.lblSFUsername.AutoSize = true;
            this.lblSFUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSFUsername.Location = new System.Drawing.Point(35, 30);
            this.lblSFUsername.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSFUsername.Name = "lblSFUsername";
            this.lblSFUsername.Size = new System.Drawing.Size(143, 17);
            this.lblSFUsername.TabIndex = 1;
            this.lblSFUsername.Text = "Username (from Org)";
            // 
            // cmbUserName
            // 
            this.cmbUserName.DropDownWidth = 325;
            this.cmbUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbUserName.FormattingEnabled = true;
            this.cmbUserName.Location = new System.Drawing.Point(194, 27);
            this.cmbUserName.Margin = new System.Windows.Forms.Padding(2);
            this.cmbUserName.Name = "cmbUserName";
            this.cmbUserName.Size = new System.Drawing.Size(324, 24);
            this.cmbUserName.TabIndex = 2;
            this.cmbUserName.SelectedIndexChanged += new System.EventHandler(this.cmbUserName_SelectedIndexChanged);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(744, 262);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(151, 31);
            this.btnRefresh.TabIndex = 13;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnObjectFieldInspector
            // 
            this.btnObjectFieldInspector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnObjectFieldInspector.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnObjectFieldInspector.Location = new System.Drawing.Point(1287, 213);
            this.btnObjectFieldInspector.Name = "btnObjectFieldInspector";
            this.btnObjectFieldInspector.Size = new System.Drawing.Size(175, 35);
            this.btnObjectFieldInspector.TabIndex = 15;
            this.btnObjectFieldInspector.Text = "Object Field Inspector";
            this.btnObjectFieldInspector.UseVisualStyleBackColor = true;
            this.btnObjectFieldInspector.Click += new System.EventHandler(this.btnObjectFieldInspector_Click);
            // 
            // lblRepository
            // 
            this.lblRepository.AutoSize = true;
            this.lblRepository.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRepository.Location = new System.Drawing.Point(17, 145);
            this.lblRepository.Name = "lblRepository";
            this.lblRepository.Size = new System.Drawing.Size(124, 17);
            this.lblRepository.TabIndex = 5;
            this.lblRepository.Text = "Repository Path";
            // 
            // tbRepository
            // 
            this.tbRepository.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbRepository.Location = new System.Drawing.Point(184, 144);
            this.tbRepository.Name = "tbRepository";
            this.tbRepository.Size = new System.Drawing.Size(703, 23);
            this.tbRepository.TabIndex = 6;
            this.tbRepository.TextChanged += new System.EventHandler(this.tbDeployFrom_TextChanged);
            this.tbRepository.DoubleClick += new System.EventHandler(this.tbRepository_DoubleClick);
            // 
            // btnBuildERD
            // 
            this.btnBuildERD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBuildERD.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuildERD.Location = new System.Drawing.Point(1287, 295);
            this.btnBuildERD.Name = "btnBuildERD";
            this.btnBuildERD.Size = new System.Drawing.Size(175, 35);
            this.btnBuildERD.TabIndex = 17;
            this.btnBuildERD.Text = "Build ERD";
            this.btnBuildERD.UseVisualStyleBackColor = true;
            this.btnBuildERD.Click += new System.EventHandler(this.btnBuildERD_Click);
            // 
            // lblRootFolder
            // 
            this.lblRootFolder.AutoSize = true;
            this.lblRootFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRootFolder.Location = new System.Drawing.Point(16, 182);
            this.lblRootFolder.Name = "lblRootFolder";
            this.lblRootFolder.Size = new System.Drawing.Size(93, 17);
            this.lblRootFolder.TabIndex = 7;
            this.lblRootFolder.Text = "Root Folder";
            // 
            // tbRootFolder
            // 
            this.tbRootFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbRootFolder.Location = new System.Drawing.Point(184, 182);
            this.tbRootFolder.Name = "tbRootFolder";
            this.tbRootFolder.Size = new System.Drawing.Size(704, 23);
            this.tbRootFolder.TabIndex = 8;
            this.tbRootFolder.DoubleClick += new System.EventHandler(this.tbRootFolder_DoubleClick);
            // 
            // btnSearchMetadata
            // 
            this.btnSearchMetadata.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearchMetadata.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearchMetadata.Location = new System.Drawing.Point(1287, 254);
            this.btnSearchMetadata.Name = "btnSearchMetadata";
            this.btnSearchMetadata.Size = new System.Drawing.Size(175, 35);
            this.btnSearchMetadata.TabIndex = 16;
            this.btnSearchMetadata.Text = "Search Metadata";
            this.btnSearchMetadata.UseVisualStyleBackColor = true;
            this.btnSearchMetadata.Click += new System.EventHandler(this.btnSearchMetadata_Click);
            // 
            // btnDebugLogs
            // 
            this.btnDebugLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDebugLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDebugLogs.Location = new System.Drawing.Point(1287, 336);
            this.btnDebugLogs.Name = "btnDebugLogs";
            this.btnDebugLogs.Size = new System.Drawing.Size(175, 35);
            this.btnDebugLogs.TabIndex = 18;
            this.btnDebugLogs.Text = "Debug Log Parsing";
            this.btnDebugLogs.UseVisualStyleBackColor = true;
            this.btnDebugLogs.Click += new System.EventHandler(this.btnDebugLogs_Click);
            // 
            // delMyDebugLogs
            // 
            this.delMyDebugLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.delMyDebugLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.delMyDebugLogs.Location = new System.Drawing.Point(1287, 377);
            this.delMyDebugLogs.Name = "delMyDebugLogs";
            this.delMyDebugLogs.Size = new System.Drawing.Size(175, 35);
            this.delMyDebugLogs.TabIndex = 19;
            this.delMyDebugLogs.Text = "Delete My Debug Logs";
            this.delMyDebugLogs.UseVisualStyleBackColor = true;
            this.delMyDebugLogs.Click += new System.EventHandler(this.delMyDebugLogs_Click);
            // 
            // lblOutboundChangeSetName
            // 
            this.lblOutboundChangeSetName.AutoSize = true;
            this.lblOutboundChangeSetName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutboundChangeSetName.Location = new System.Drawing.Point(17, 217);
            this.lblOutboundChangeSetName.Name = "lblOutboundChangeSetName";
            this.lblOutboundChangeSetName.Size = new System.Drawing.Size(214, 17);
            this.lblOutboundChangeSetName.TabIndex = 9;
            this.lblOutboundChangeSetName.Text = "Outbound Change Set Name";
            // 
            // tbOutboundChangeSetName
            // 
            this.tbOutboundChangeSetName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbOutboundChangeSetName.Location = new System.Drawing.Point(254, 214);
            this.tbOutboundChangeSetName.Name = "tbOutboundChangeSetName";
            this.tbOutboundChangeSetName.Size = new System.Drawing.Size(633, 23);
            this.tbOutboundChangeSetName.TabIndex = 10;
            this.tbOutboundChangeSetName.MouseHover += new System.EventHandler(this.tbOutboundChangeSetName_MouseHover);
            // 
            // btnCopySelectedToRepository
            // 
            this.btnCopySelectedToRepository.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopySelectedToRepository.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCopySelectedToRepository.Location = new System.Drawing.Point(1008, 310);
            this.btnCopySelectedToRepository.Name = "btnCopySelectedToRepository";
            this.btnCopySelectedToRepository.Size = new System.Drawing.Size(175, 44);
            this.btnCopySelectedToRepository.TabIndex = 22;
            this.btnCopySelectedToRepository.Text = "Copy Selected to Repository";
            this.btnCopySelectedToRepository.UseVisualStyleBackColor = true;
            this.btnCopySelectedToRepository.Click += new System.EventHandler(this.btnCopySelectedToRepository_Click);
            // 
            // DevelopmentEnvironment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1479, 961);
            this.Controls.Add(this.btnCopySelectedToRepository);
            this.Controls.Add(this.lblOutboundChangeSetName);
            this.Controls.Add(this.tbOutboundChangeSetName);
            this.Controls.Add(this.delMyDebugLogs);
            this.Controls.Add(this.btnDebugLogs);
            this.Controls.Add(this.btnSearchMetadata);
            this.Controls.Add(this.tbRootFolder);
            this.Controls.Add(this.lblRootFolder);
            this.Controls.Add(this.btnBuildERD);
            this.Controls.Add(this.tbRepository);
            this.Controls.Add(this.lblRepository);
            this.Controls.Add(this.btnObjectFieldInspector);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.fromOrgGroup);
            this.Controls.Add(this.btnDeployToOrg);
            this.Controls.Add(this.btnRetrieveFromOrg);
            this.Controls.Add(this.tbDeployFrom);
            this.Controls.Add(this.lblDeploymentFolder);
            this.Controls.Add(this.lblProjectFolder);
            this.Controls.Add(this.tbProjectFolder);
            this.Controls.Add(this.treeViewMetadata);
            this.Controls.Add(this.lblNote);
            this.Controls.Add(msIDE);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = msIDE;
            this.Name = "DevelopmentEnvironment";
            this.Text = "DevelopmentEnvironment";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DevelopmentEnvironment_FormClosing);
            msIDE.ResumeLayout(false);
            msIDE.PerformLayout();
            this.fromOrgGroup.ResumeLayout(false);
            this.fromOrgGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNote;
        public System.Windows.Forms.TreeView treeViewMetadata;
        public System.Windows.Forms.TextBox tbProjectFolder;
        private System.Windows.Forms.Label lblProjectFolder;
        private System.Windows.Forms.Label lblDeploymentFolder;
        public System.Windows.Forms.TextBox tbDeployFrom;
        private System.Windows.Forms.Button btnRetrieveFromOrg;
        private System.Windows.Forms.Button btnDeployToOrg;
        private System.Windows.Forms.GroupBox fromOrgGroup;
        private System.Windows.Forms.Label lblSFUsername;
        private System.Windows.Forms.ComboBox cmbUserName;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnObjectFieldInspector;
        private System.Windows.Forms.Label lblRepository;
        private System.Windows.Forms.TextBox tbRepository;
        private System.Windows.Forms.Button btnBuildERD;
        private System.Windows.Forms.Label lblRootFolder;
        private System.Windows.Forms.TextBox tbRootFolder;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectSolutionFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem projectSolutionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button btnSearchMetadata;
        private System.Windows.Forms.Button btnDebugLogs;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem loadRecentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addApexTriggerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addApexClassToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addOtherToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customObjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addLightningWebComponentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem visualforcePageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem visualforceComponentToolStripMenuItem;
        private System.Windows.Forms.Button delMyDebugLogs;
        private System.Windows.Forms.Label lblOutboundChangeSetName;
        private System.Windows.Forms.TextBox tbOutboundChangeSetName;
        private System.Windows.Forms.Button btnCopySelectedToRepository;
    }
}