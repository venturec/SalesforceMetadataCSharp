namespace SalesforceMetadata
{
    partial class LandingPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LandingPage));
            this.btnMetadataForm = new System.Windows.Forms.Button();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.settingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usernameAndSOAPXmlFile = new System.Windows.Forms.ToolStripMenuItem();
            this.horizontalRule1 = new System.Windows.Forms.Label();
            this.btnSearchMetadata = new System.Windows.Forms.Button();
            this.btnParseDebugLogs = new System.Windows.Forms.Button();
            this.btnRESTService = new System.Windows.Forms.Button();
            this.btnGenerateConfigWorkbook = new System.Windows.Forms.Button();
            this.btnOpenObjectFieldInspector = new System.Windows.Forms.Button();
            this.btnOpenObjectModification = new System.Windows.Forms.Button();
            this.btnExtractHTMLPages = new System.Windows.Forms.Button();
            this.btnExtractClassesMethods = new System.Windows.Forms.Button();
            this.MetadataComparison = new System.Windows.Forms.Button();
            this.btnUpdateAPIVersion = new System.Windows.Forms.Button();
            this.btnGenerateDeploymentPackage = new System.Windows.Forms.Button();
            this.btnParseLWC = new System.Windows.Forms.Button();
            this.btnDevelopmentIde = new System.Windows.Forms.Button();
            this.btnDeployments = new System.Windows.Forms.Button();
            this.btnParseMetadataXml = new System.Windows.Forms.Button();
            this.verticalRule1 = new System.Windows.Forms.Label();
            this.btnGenerateToolingReport = new System.Windows.Forms.Button();
            this.mainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnMetadataForm
            // 
            this.btnMetadataForm.Location = new System.Drawing.Point(9, 46);
            this.btnMetadataForm.Margin = new System.Windows.Forms.Padding(2);
            this.btnMetadataForm.Name = "btnMetadataForm";
            this.btnMetadataForm.Size = new System.Drawing.Size(190, 35);
            this.btnMetadataForm.TabIndex = 2;
            this.btnMetadataForm.Text = "Metadata Form";
            this.btnMetadataForm.UseVisualStyleBackColor = true;
            this.btnMetadataForm.Click += new System.EventHandler(this.btnMetadataForm_Click);
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.mainMenuStrip.Size = new System.Drawing.Size(937, 27);
            this.mainMenuStrip.TabIndex = 0;
            this.mainMenuStrip.Text = "Main Menu";
            // 
            // settingsMenuItem
            // 
            this.settingsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.usernameAndSOAPXmlFile});
            this.settingsMenuItem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.settingsMenuItem.Name = "settingsMenuItem";
            this.settingsMenuItem.Size = new System.Drawing.Size(74, 23);
            this.settingsMenuItem.Text = "Settings";
            // 
            // usernameAndSOAPXmlFile
            // 
            this.usernameAndSOAPXmlFile.Name = "usernameAndSOAPXmlFile";
            this.usernameAndSOAPXmlFile.Size = new System.Drawing.Size(325, 24);
            this.usernameAndSOAPXmlFile.Text = "User and SOAP Address File Location";
            this.usernameAndSOAPXmlFile.Click += new System.EventHandler(this.addUserAndSOAPAPIAddress_Click);
            // 
            // horizontalRule1
            // 
            this.horizontalRule1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalRule1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.horizontalRule1.Location = new System.Drawing.Point(-12, 27);
            this.horizontalRule1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.horizontalRule1.Name = "horizontalRule1";
            this.horizontalRule1.Size = new System.Drawing.Size(949, 8);
            this.horizontalRule1.TabIndex = 1;
            // 
            // btnSearchMetadata
            // 
            this.btnSearchMetadata.Location = new System.Drawing.Point(399, 46);
            this.btnSearchMetadata.Margin = new System.Windows.Forms.Padding(2);
            this.btnSearchMetadata.Name = "btnSearchMetadata";
            this.btnSearchMetadata.Size = new System.Drawing.Size(190, 35);
            this.btnSearchMetadata.TabIndex = 7;
            this.btnSearchMetadata.Text = "Search Metadata";
            this.btnSearchMetadata.UseVisualStyleBackColor = true;
            this.btnSearchMetadata.Click += new System.EventHandler(this.btnSearchMetadata_Click);
            // 
            // btnParseDebugLogs
            // 
            this.btnParseDebugLogs.Location = new System.Drawing.Point(400, 86);
            this.btnParseDebugLogs.Name = "btnParseDebugLogs";
            this.btnParseDebugLogs.Size = new System.Drawing.Size(190, 35);
            this.btnParseDebugLogs.TabIndex = 8;
            this.btnParseDebugLogs.Text = "Parse Debug Logs";
            this.btnParseDebugLogs.UseVisualStyleBackColor = true;
            this.btnParseDebugLogs.Click += new System.EventHandler(this.BtnParseDebugLogs_Click);
            // 
            // btnRESTService
            // 
            this.btnRESTService.Location = new System.Drawing.Point(9, 496);
            this.btnRESTService.Margin = new System.Windows.Forms.Padding(2);
            this.btnRESTService.Name = "btnRESTService";
            this.btnRESTService.Size = new System.Drawing.Size(187, 32);
            this.btnRESTService.TabIndex = 18;
            this.btnRESTService.Text = "REST Service Form";
            this.btnRESTService.UseVisualStyleBackColor = true;
            this.btnRESTService.Click += new System.EventHandler(this.btnRetrieveRecords_Click);
            // 
            // btnGenerateConfigWorkbook
            // 
            this.btnGenerateConfigWorkbook.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnGenerateConfigWorkbook.Location = new System.Drawing.Point(736, 210);
            this.btnGenerateConfigWorkbook.Margin = new System.Windows.Forms.Padding(2);
            this.btnGenerateConfigWorkbook.Name = "btnGenerateConfigWorkbook";
            this.btnGenerateConfigWorkbook.Size = new System.Drawing.Size(190, 35);
            this.btnGenerateConfigWorkbook.TabIndex = 14;
            this.btnGenerateConfigWorkbook.Text = "Generate Configuration Workbook";
            this.btnGenerateConfigWorkbook.UseVisualStyleBackColor = true;
            this.btnGenerateConfigWorkbook.Click += new System.EventHandler(this.btnGenerateConfigWorkbook_Click);
            // 
            // btnOpenObjectFieldInspector
            // 
            this.btnOpenObjectFieldInspector.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnOpenObjectFieldInspector.Location = new System.Drawing.Point(735, 46);
            this.btnOpenObjectFieldInspector.Margin = new System.Windows.Forms.Padding(2);
            this.btnOpenObjectFieldInspector.Name = "btnOpenObjectFieldInspector";
            this.btnOpenObjectFieldInspector.Size = new System.Drawing.Size(190, 35);
            this.btnOpenObjectFieldInspector.TabIndex = 10;
            this.btnOpenObjectFieldInspector.Text = "Object / Field Inspector";
            this.btnOpenObjectFieldInspector.UseVisualStyleBackColor = true;
            this.btnOpenObjectFieldInspector.Click += new System.EventHandler(this.btnOpenObjectFieldInspector_Click);
            // 
            // btnOpenObjectModification
            // 
            this.btnOpenObjectModification.Location = new System.Drawing.Point(9, 458);
            this.btnOpenObjectModification.Margin = new System.Windows.Forms.Padding(2);
            this.btnOpenObjectModification.Name = "btnOpenObjectModification";
            this.btnOpenObjectModification.Size = new System.Drawing.Size(188, 32);
            this.btnOpenObjectModification.TabIndex = 17;
            this.btnOpenObjectModification.Text = "Object Modifications";
            this.btnOpenObjectModification.UseVisualStyleBackColor = true;
            this.btnOpenObjectModification.Click += new System.EventHandler(this.btnOpenObjectModification_Click);
            // 
            // btnExtractHTMLPages
            // 
            this.btnExtractHTMLPages.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnExtractHTMLPages.Location = new System.Drawing.Point(736, 421);
            this.btnExtractHTMLPages.Margin = new System.Windows.Forms.Padding(2);
            this.btnExtractHTMLPages.Name = "btnExtractHTMLPages";
            this.btnExtractHTMLPages.Size = new System.Drawing.Size(190, 35);
            this.btnExtractHTMLPages.TabIndex = 15;
            this.btnExtractHTMLPages.Text = "Extract HTML Pages";
            this.btnExtractHTMLPages.UseVisualStyleBackColor = true;
            this.btnExtractHTMLPages.Click += new System.EventHandler(this.btnExtractHTMLPages_Click);
            // 
            // btnExtractClassesMethods
            // 
            this.btnExtractClassesMethods.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnExtractClassesMethods.Location = new System.Drawing.Point(735, 88);
            this.btnExtractClassesMethods.Name = "btnExtractClassesMethods";
            this.btnExtractClassesMethods.Size = new System.Drawing.Size(190, 35);
            this.btnExtractClassesMethods.TabIndex = 11;
            this.btnExtractClassesMethods.Tag = "Includes Code Coverage";
            this.btnExtractClassesMethods.Text = "Automation Reporter";
            this.btnExtractClassesMethods.UseVisualStyleBackColor = true;
            this.btnExtractClassesMethods.Click += new System.EventHandler(this.btnExtractClassesMethods_Click);
            // 
            // MetadataComparison
            // 
            this.MetadataComparison.Location = new System.Drawing.Point(204, 46);
            this.MetadataComparison.Name = "MetadataComparison";
            this.MetadataComparison.Size = new System.Drawing.Size(190, 35);
            this.MetadataComparison.TabIndex = 4;
            this.MetadataComparison.Text = "Metadata Comparison";
            this.MetadataComparison.UseVisualStyleBackColor = true;
            this.MetadataComparison.Click += new System.EventHandler(this.MetadataComparison_Click);
            // 
            // btnUpdateAPIVersion
            // 
            this.btnUpdateAPIVersion.Location = new System.Drawing.Point(213, 421);
            this.btnUpdateAPIVersion.Name = "btnUpdateAPIVersion";
            this.btnUpdateAPIVersion.Size = new System.Drawing.Size(190, 35);
            this.btnUpdateAPIVersion.TabIndex = 19;
            this.btnUpdateAPIVersion.Text = "Update API Versions";
            this.btnUpdateAPIVersion.UseVisualStyleBackColor = true;
            this.btnUpdateAPIVersion.Click += new System.EventHandler(this.btnUpdateAPIVersion_Click);
            // 
            // btnGenerateDeploymentPackage
            // 
            this.btnGenerateDeploymentPackage.Location = new System.Drawing.Point(204, 87);
            this.btnGenerateDeploymentPackage.Name = "btnGenerateDeploymentPackage";
            this.btnGenerateDeploymentPackage.Size = new System.Drawing.Size(190, 35);
            this.btnGenerateDeploymentPackage.TabIndex = 5;
            this.btnGenerateDeploymentPackage.Text = "Generate Deployment Package";
            this.btnGenerateDeploymentPackage.UseVisualStyleBackColor = true;
            this.btnGenerateDeploymentPackage.Click += new System.EventHandler(this.btnGenerateDeploymentPackage_Click);
            // 
            // btnParseLWC
            // 
            this.btnParseLWC.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnParseLWC.Location = new System.Drawing.Point(736, 129);
            this.btnParseLWC.Name = "btnParseLWC";
            this.btnParseLWC.Size = new System.Drawing.Size(190, 35);
            this.btnParseLWC.TabIndex = 12;
            this.btnParseLWC.Text = "Parse LWC Files";
            this.btnParseLWC.UseVisualStyleBackColor = true;
            this.btnParseLWC.Click += new System.EventHandler(this.btnParseLWC_Click);
            // 
            // btnDevelopmentIde
            // 
            this.btnDevelopmentIde.Location = new System.Drawing.Point(9, 88);
            this.btnDevelopmentIde.Name = "btnDevelopmentIde";
            this.btnDevelopmentIde.Size = new System.Drawing.Size(190, 34);
            this.btnDevelopmentIde.TabIndex = 3;
            this.btnDevelopmentIde.Text = "Development IDE";
            this.btnDevelopmentIde.UseVisualStyleBackColor = true;
            this.btnDevelopmentIde.Click += new System.EventHandler(this.btnDevelopmentIde_Click);
            // 
            // btnDeployments
            // 
            this.btnDeployments.Location = new System.Drawing.Point(204, 127);
            this.btnDeployments.Name = "btnDeployments";
            this.btnDeployments.Size = new System.Drawing.Size(190, 35);
            this.btnDeployments.TabIndex = 6;
            this.btnDeployments.Text = "Deployments";
            this.btnDeployments.UseVisualStyleBackColor = true;
            this.btnDeployments.Click += new System.EventHandler(this.btnDeployments_Click);
            // 
            // btnParseMetadataXml
            // 
            this.btnParseMetadataXml.Location = new System.Drawing.Point(9, 421);
            this.btnParseMetadataXml.Name = "btnParseMetadataXml";
            this.btnParseMetadataXml.Size = new System.Drawing.Size(188, 32);
            this.btnParseMetadataXml.TabIndex = 16;
            this.btnParseMetadataXml.Text = "Parse Metadata XML";
            this.btnParseMetadataXml.UseVisualStyleBackColor = true;
            this.btnParseMetadataXml.Click += new System.EventHandler(this.btnParseMetadataXml_Click);
            // 
            // verticalRule1
            // 
            this.verticalRule1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.verticalRule1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.verticalRule1.Location = new System.Drawing.Point(700, 27);
            this.verticalRule1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.verticalRule1.Name = "verticalRule1";
            this.verticalRule1.Size = new System.Drawing.Size(15, 510);
            this.verticalRule1.TabIndex = 9;
            // 
            // btnGenerateToolingReport
            // 
            this.btnGenerateToolingReport.Location = new System.Drawing.Point(736, 170);
            this.btnGenerateToolingReport.Name = "btnGenerateToolingReport";
            this.btnGenerateToolingReport.Size = new System.Drawing.Size(189, 35);
            this.btnGenerateToolingReport.TabIndex = 13;
            this.btnGenerateToolingReport.Text = "Generate Tooling Report";
            this.btnGenerateToolingReport.UseVisualStyleBackColor = true;
            this.btnGenerateToolingReport.Click += new System.EventHandler(this.btnGenerateToolingReport_Click);
            // 
            // LandingPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(937, 536);
            this.Controls.Add(this.btnGenerateToolingReport);
            this.Controls.Add(this.verticalRule1);
            this.Controls.Add(this.btnParseMetadataXml);
            this.Controls.Add(this.btnDeployments);
            this.Controls.Add(this.btnDevelopmentIde);
            this.Controls.Add(this.btnParseLWC);
            this.Controls.Add(this.btnGenerateDeploymentPackage);
            this.Controls.Add(this.btnUpdateAPIVersion);
            this.Controls.Add(this.MetadataComparison);
            this.Controls.Add(this.btnExtractClassesMethods);
            this.Controls.Add(this.btnExtractHTMLPages);
            this.Controls.Add(this.btnOpenObjectModification);
            this.Controls.Add(this.btnOpenObjectFieldInspector);
            this.Controls.Add(this.btnGenerateConfigWorkbook);
            this.Controls.Add(this.btnRESTService);
            this.Controls.Add(this.btnParseDebugLogs);
            this.Controls.Add(this.btnSearchMetadata);
            this.Controls.Add(this.horizontalRule1);
            this.Controls.Add(this.mainMenuStrip);
            this.Controls.Add(this.btnMetadataForm);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenuStrip;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "LandingPage";
            this.Text = "Landing Page";
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnMetadataForm;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem settingsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem usernameAndSOAPXmlFile;
        private System.Windows.Forms.Label horizontalRule1;
        private System.Windows.Forms.Button btnSearchMetadata;
        private System.Windows.Forms.Button btnParseDebugLogs;
        private System.Windows.Forms.Button btnRESTService;
        private System.Windows.Forms.Button btnGenerateConfigWorkbook;
        private System.Windows.Forms.Button btnOpenObjectFieldInspector;
        private System.Windows.Forms.Button btnOpenObjectModification;
        private System.Windows.Forms.Button btnExtractHTMLPages;
        private System.Windows.Forms.Button btnExtractClassesMethods;
        private System.Windows.Forms.Button MetadataComparison;
        private System.Windows.Forms.Button btnUpdateAPIVersion;
        private System.Windows.Forms.Button btnGenerateDeploymentPackage;
        private System.Windows.Forms.Button btnParseLWC;
        private System.Windows.Forms.Button btnDevelopmentIde;
        private System.Windows.Forms.Button btnDeployments;
        private System.Windows.Forms.Button btnParseMetadataXml;
        private System.Windows.Forms.Label verticalRule1;
        private System.Windows.Forms.Button btnGenerateToolingReport;
    }
}