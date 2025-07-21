namespace SalesforceMetadata
{
    partial class ObjectFieldInspector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ObjectFieldInspector));
            this.cmbSalesforceSObjects = new System.Windows.Forms.ComboBox();
            this.lblSalesforceSobjects = new System.Windows.Forms.Label();
            this.lblSobjectFieldList = new System.Windows.Forms.Label();
            this.btnGetSobjects = new System.Windows.Forms.Button();
            this.listViewSobjectFields = new System.Windows.Forms.ListView();
            this.btnSaveFieldsToFile = new System.Windows.Forms.Button();
            this.salesforceOrgCredentials = new System.Windows.Forms.GroupBox();
            this.lblSFUsername = new System.Windows.Forms.Label();
            this.btnSaveObjectsToFile = new System.Windows.Forms.Button();
            this.cmbUserName = new System.Windows.Forms.ComboBox();
            this.btnSaveSelectedToExcel = new System.Windows.Forms.Button();
            this.sobjectListBox = new System.Windows.Forms.CheckedListBox();
            this.saveToExcelGrpBox = new System.Windows.Forms.GroupBox();
            this.cbSelectNone = new System.Windows.Forms.CheckBox();
            this.cbSelectAll = new System.Windows.Forms.CheckBox();
            this.btnGetReferenceFields = new System.Windows.Forms.Button();
            this.salesforceSobjectsGrpBox = new System.Windows.Forms.GroupBox();
            this.btnExportSelected = new System.Windows.Forms.Button();
            this.cbFilterManagedPkg = new System.Windows.Forms.CheckBox();
            this.cbCustomObjectsOnly = new System.Windows.Forms.CheckBox();
            this.salesforceOrgCredentials.SuspendLayout();
            this.saveToExcelGrpBox.SuspendLayout();
            this.salesforceSobjectsGrpBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbSalesforceSObjects
            // 
            this.cmbSalesforceSObjects.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSalesforceSObjects.FormattingEnabled = true;
            this.cmbSalesforceSObjects.Location = new System.Drawing.Point(10, 18);
            this.cmbSalesforceSObjects.Name = "cmbSalesforceSObjects";
            this.cmbSalesforceSObjects.Size = new System.Drawing.Size(585, 24);
            this.cmbSalesforceSObjects.TabIndex = 1;
            this.cmbSalesforceSObjects.SelectedValueChanged += new System.EventHandler(this.cmbSalesforceSObjects_SelectedValueChanged);
            // 
            // lblSalesforceSobjects
            // 
            this.lblSalesforceSobjects.AutoSize = true;
            this.lblSalesforceSobjects.Location = new System.Drawing.Point(8, 0);
            this.lblSalesforceSobjects.Name = "lblSalesforceSobjects";
            this.lblSalesforceSobjects.Size = new System.Drawing.Size(133, 17);
            this.lblSalesforceSobjects.TabIndex = 0;
            this.lblSalesforceSobjects.Text = "Salesforce Sobjects";
            // 
            // lblSobjectFieldList
            // 
            this.lblSobjectFieldList.AutoSize = true;
            this.lblSobjectFieldList.Location = new System.Drawing.Point(14, 384);
            this.lblSobjectFieldList.Name = "lblSobjectFieldList";
            this.lblSobjectFieldList.Size = new System.Drawing.Size(87, 13);
            this.lblSobjectFieldList.TabIndex = 3;
            this.lblSobjectFieldList.Text = "Sobject Field List";
            // 
            // btnGetSobjects
            // 
            this.btnGetSobjects.Location = new System.Drawing.Point(530, 30);
            this.btnGetSobjects.Name = "btnGetSobjects";
            this.btnGetSobjects.Size = new System.Drawing.Size(115, 25);
            this.btnGetSobjects.TabIndex = 6;
            this.btnGetSobjects.Text = "Get Sobjects";
            this.btnGetSobjects.UseVisualStyleBackColor = true;
            this.btnGetSobjects.Click += new System.EventHandler(this.btnGetSobjects_Click);
            // 
            // listViewSobjectFields
            // 
            this.listViewSobjectFields.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewSobjectFields.CausesValidation = false;
            this.listViewSobjectFields.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewSobjectFields.HideSelection = false;
            this.listViewSobjectFields.LabelEdit = true;
            this.listViewSobjectFields.Location = new System.Drawing.Point(16, 401);
            this.listViewSobjectFields.Name = "listViewSobjectFields";
            this.listViewSobjectFields.Size = new System.Drawing.Size(1222, 408);
            this.listViewSobjectFields.TabIndex = 4;
            this.listViewSobjectFields.UseCompatibleStateImageBehavior = false;
            this.listViewSobjectFields.View = System.Windows.Forms.View.Details;
            this.listViewSobjectFields.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewSobjectFields_ColumnClick);
            this.listViewSobjectFields.SelectedIndexChanged += new System.EventHandler(this.listViewSobjectFields_SelectedIndexChanged);
            this.listViewSobjectFields.DoubleClick += new System.EventHandler(this.listViewSobjectFields_DoubleClick);
            // 
            // btnSaveFieldsToFile
            // 
            this.btnSaveFieldsToFile.Location = new System.Drawing.Point(11, 112);
            this.btnSaveFieldsToFile.Name = "btnSaveFieldsToFile";
            this.btnSaveFieldsToFile.Size = new System.Drawing.Size(138, 32);
            this.btnSaveFieldsToFile.TabIndex = 4;
            this.btnSaveFieldsToFile.Text = "Save Fields To File";
            this.btnSaveFieldsToFile.UseVisualStyleBackColor = true;
            this.btnSaveFieldsToFile.Click += new System.EventHandler(this.btnSaveFieldsToFile_Click);
            // 
            // salesforceOrgCredentials
            // 
            this.salesforceOrgCredentials.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.salesforceOrgCredentials.Controls.Add(this.lblSFUsername);
            this.salesforceOrgCredentials.Controls.Add(this.btnSaveObjectsToFile);
            this.salesforceOrgCredentials.Controls.Add(this.cmbUserName);
            this.salesforceOrgCredentials.Controls.Add(this.btnGetSobjects);
            this.salesforceOrgCredentials.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.salesforceOrgCredentials.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.salesforceOrgCredentials.Location = new System.Drawing.Point(9, 17);
            this.salesforceOrgCredentials.Margin = new System.Windows.Forms.Padding(2);
            this.salesforceOrgCredentials.Name = "salesforceOrgCredentials";
            this.salesforceOrgCredentials.Padding = new System.Windows.Forms.Padding(2);
            this.salesforceOrgCredentials.Size = new System.Drawing.Size(1064, 129);
            this.salesforceOrgCredentials.TabIndex = 0;
            this.salesforceOrgCredentials.TabStop = false;
            this.salesforceOrgCredentials.Text = "Salesforce Org Credentials";
            // 
            // lblSFUsername
            // 
            this.lblSFUsername.AutoSize = true;
            this.lblSFUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSFUsername.Location = new System.Drawing.Point(14, 31);
            this.lblSFUsername.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSFUsername.Name = "lblSFUsername";
            this.lblSFUsername.Size = new System.Drawing.Size(143, 17);
            this.lblSFUsername.TabIndex = 0;
            this.lblSFUsername.Text = "Username (from Org)";
            // 
            // btnSaveObjectsToFile
            // 
            this.btnSaveObjectsToFile.Enabled = false;
            this.btnSaveObjectsToFile.Location = new System.Drawing.Point(530, 67);
            this.btnSaveObjectsToFile.Margin = new System.Windows.Forms.Padding(2);
            this.btnSaveObjectsToFile.Name = "btnSaveObjectsToFile";
            this.btnSaveObjectsToFile.Size = new System.Drawing.Size(115, 25);
            this.btnSaveObjectsToFile.TabIndex = 7;
            this.btnSaveObjectsToFile.Text = "Save Objects to File";
            this.btnSaveObjectsToFile.UseVisualStyleBackColor = true;
            this.btnSaveObjectsToFile.Click += new System.EventHandler(this.btnSaveObjectsToFile_Click);
            // 
            // cmbUserName
            // 
            this.cmbUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbUserName.Location = new System.Drawing.Point(176, 28);
            this.cmbUserName.Margin = new System.Windows.Forms.Padding(2);
            this.cmbUserName.Name = "cmbUserName";
            this.cmbUserName.Size = new System.Drawing.Size(312, 24);
            this.cmbUserName.TabIndex = 1;
            this.cmbUserName.SelectedIndexChanged += new System.EventHandler(this.cmbUserName_SelectedIndexChanged);
            // 
            // btnSaveSelectedToExcel
            // 
            this.btnSaveSelectedToExcel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnSaveSelectedToExcel.Location = new System.Drawing.Point(245, 17);
            this.btnSaveSelectedToExcel.Margin = new System.Windows.Forms.Padding(2);
            this.btnSaveSelectedToExcel.Name = "btnSaveSelectedToExcel";
            this.btnSaveSelectedToExcel.Size = new System.Drawing.Size(196, 31);
            this.btnSaveSelectedToExcel.TabIndex = 3;
            this.btnSaveSelectedToExcel.Text = "Save Selected to Excel";
            this.btnSaveSelectedToExcel.UseVisualStyleBackColor = false;
            this.btnSaveSelectedToExcel.Click += new System.EventHandler(this.btnSaveSelectedToExcel_Click);
            // 
            // sobjectListBox
            // 
            this.sobjectListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.sobjectListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sobjectListBox.FormattingEnabled = true;
            this.sobjectListBox.Location = new System.Drawing.Point(15, 89);
            this.sobjectListBox.Margin = new System.Windows.Forms.Padding(2);
            this.sobjectListBox.Name = "sobjectListBox";
            this.sobjectListBox.Size = new System.Drawing.Size(426, 697);
            this.sobjectListBox.TabIndex = 0;
            // 
            // saveToExcelGrpBox
            // 
            this.saveToExcelGrpBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.saveToExcelGrpBox.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.saveToExcelGrpBox.Controls.Add(this.cbSelectNone);
            this.saveToExcelGrpBox.Controls.Add(this.cbSelectAll);
            this.saveToExcelGrpBox.Controls.Add(this.btnGetReferenceFields);
            this.saveToExcelGrpBox.Controls.Add(this.sobjectListBox);
            this.saveToExcelGrpBox.Controls.Add(this.btnSaveSelectedToExcel);
            this.saveToExcelGrpBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveToExcelGrpBox.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.saveToExcelGrpBox.Location = new System.Drawing.Point(1243, 17);
            this.saveToExcelGrpBox.Margin = new System.Windows.Forms.Padding(2);
            this.saveToExcelGrpBox.Name = "saveToExcelGrpBox";
            this.saveToExcelGrpBox.Padding = new System.Windows.Forms.Padding(2);
            this.saveToExcelGrpBox.Size = new System.Drawing.Size(458, 793);
            this.saveToExcelGrpBox.TabIndex = 2;
            this.saveToExcelGrpBox.TabStop = false;
            this.saveToExcelGrpBox.Text = "Save Selected To Excel";
            // 
            // cbSelectNone
            // 
            this.cbSelectNone.AutoSize = true;
            this.cbSelectNone.Location = new System.Drawing.Point(15, 50);
            this.cbSelectNone.Name = "cbSelectNone";
            this.cbSelectNone.Size = new System.Drawing.Size(104, 21);
            this.cbSelectNone.TabIndex = 2;
            this.cbSelectNone.Text = "Select None";
            this.cbSelectNone.UseVisualStyleBackColor = true;
            this.cbSelectNone.CheckedChanged += new System.EventHandler(this.cbSelectNone_CheckedChanged);
            // 
            // cbSelectAll
            // 
            this.cbSelectAll.AutoSize = true;
            this.cbSelectAll.Location = new System.Drawing.Point(15, 27);
            this.cbSelectAll.Name = "cbSelectAll";
            this.cbSelectAll.Size = new System.Drawing.Size(85, 21);
            this.cbSelectAll.TabIndex = 1;
            this.cbSelectAll.Text = "Select All";
            this.cbSelectAll.UseVisualStyleBackColor = true;
            this.cbSelectAll.CheckedChanged += new System.EventHandler(this.cbSelectAll_CheckedChanged);
            // 
            // btnGetReferenceFields
            // 
            this.btnGetReferenceFields.Location = new System.Drawing.Point(245, 50);
            this.btnGetReferenceFields.Name = "btnGetReferenceFields";
            this.btnGetReferenceFields.Size = new System.Drawing.Size(196, 34);
            this.btnGetReferenceFields.TabIndex = 4;
            this.btnGetReferenceFields.Text = "Get Reference Fields Only";
            this.btnGetReferenceFields.UseVisualStyleBackColor = true;
            this.btnGetReferenceFields.Click += new System.EventHandler(this.btnGetReferenceFields_Click);
            // 
            // salesforceSobjectsGrpBox
            // 
            this.salesforceSobjectsGrpBox.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.salesforceSobjectsGrpBox.Controls.Add(this.btnExportSelected);
            this.salesforceSobjectsGrpBox.Controls.Add(this.cbFilterManagedPkg);
            this.salesforceSobjectsGrpBox.Controls.Add(this.cbCustomObjectsOnly);
            this.salesforceSobjectsGrpBox.Controls.Add(this.btnSaveFieldsToFile);
            this.salesforceSobjectsGrpBox.Controls.Add(this.lblSalesforceSobjects);
            this.salesforceSobjectsGrpBox.Controls.Add(this.cmbSalesforceSObjects);
            this.salesforceSobjectsGrpBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.salesforceSobjectsGrpBox.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.salesforceSobjectsGrpBox.Location = new System.Drawing.Point(9, 156);
            this.salesforceSobjectsGrpBox.Margin = new System.Windows.Forms.Padding(2);
            this.salesforceSobjectsGrpBox.Name = "salesforceSobjectsGrpBox";
            this.salesforceSobjectsGrpBox.Padding = new System.Windows.Forms.Padding(2);
            this.salesforceSobjectsGrpBox.Size = new System.Drawing.Size(601, 223);
            this.salesforceSobjectsGrpBox.TabIndex = 1;
            this.salesforceSobjectsGrpBox.TabStop = false;
            this.salesforceSobjectsGrpBox.Text = "groupBox1";
            // 
            // btnExportSelected
            // 
            this.btnExportSelected.Enabled = false;
            this.btnExportSelected.Location = new System.Drawing.Point(165, 112);
            this.btnExportSelected.Name = "btnExportSelected";
            this.btnExportSelected.Size = new System.Drawing.Size(174, 32);
            this.btnExportSelected.TabIndex = 6;
            this.btnExportSelected.Text = "Export Selected to Excel";
            this.btnExportSelected.UseVisualStyleBackColor = true;
            this.btnExportSelected.Click += new System.EventHandler(this.btnExportSelected_Click);
            // 
            // cbFilterManagedPkg
            // 
            this.cbFilterManagedPkg.AutoSize = true;
            this.cbFilterManagedPkg.Location = new System.Drawing.Point(10, 68);
            this.cbFilterManagedPkg.Name = "cbFilterManagedPkg";
            this.cbFilterManagedPkg.Size = new System.Drawing.Size(187, 21);
            this.cbFilterManagedPkg.TabIndex = 5;
            this.cbFilterManagedPkg.Text = "Filter Managed Packages";
            this.cbFilterManagedPkg.UseVisualStyleBackColor = true;
            this.cbFilterManagedPkg.CheckedChanged += new System.EventHandler(this.cbFilterManagedPkg_CheckedChanged);
            // 
            // cbCustomObjectsOnly
            // 
            this.cbCustomObjectsOnly.AutoSize = true;
            this.cbCustomObjectsOnly.Location = new System.Drawing.Point(11, 45);
            this.cbCustomObjectsOnly.Name = "cbCustomObjectsOnly";
            this.cbCustomObjectsOnly.Size = new System.Drawing.Size(161, 21);
            this.cbCustomObjectsOnly.TabIndex = 2;
            this.cbCustomObjectsOnly.Text = "Filter Custom Objects";
            this.cbCustomObjectsOnly.UseVisualStyleBackColor = true;
            this.cbCustomObjectsOnly.CheckedChanged += new System.EventHandler(this.cbCustomObjectsOnly_CheckedChanged);
            // 
            // ObjectFieldInspector
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1712, 821);
            this.Controls.Add(this.salesforceSobjectsGrpBox);
            this.Controls.Add(this.saveToExcelGrpBox);
            this.Controls.Add(this.salesforceOrgCredentials);
            this.Controls.Add(this.listViewSobjectFields);
            this.Controls.Add(this.lblSobjectFieldList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ObjectFieldInspector";
            this.Text = "ObjectFieldInspector";
            this.salesforceOrgCredentials.ResumeLayout(false);
            this.salesforceOrgCredentials.PerformLayout();
            this.saveToExcelGrpBox.ResumeLayout(false);
            this.saveToExcelGrpBox.PerformLayout();
            this.salesforceSobjectsGrpBox.ResumeLayout(false);
            this.salesforceSobjectsGrpBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblSFUsername;
        public System.Windows.Forms.ComboBox cmbUserName;
        private System.Windows.Forms.ComboBox cmbSalesforceSObjects;
        private System.Windows.Forms.Label lblSalesforceSobjects;
        private System.Windows.Forms.Label lblSobjectFieldList;
        private System.Windows.Forms.Button btnGetSobjects;
        private System.Windows.Forms.ListView listViewSobjectFields;
        private System.Windows.Forms.Button btnSaveFieldsToFile;
        private System.Windows.Forms.GroupBox salesforceOrgCredentials;
        private System.Windows.Forms.Button btnSaveObjectsToFile;
        private System.Windows.Forms.Button btnSaveSelectedToExcel;
        private System.Windows.Forms.CheckedListBox sobjectListBox;
        private System.Windows.Forms.GroupBox saveToExcelGrpBox;
        private System.Windows.Forms.GroupBox salesforceSobjectsGrpBox;
        private System.Windows.Forms.Button btnGetReferenceFields;
        private System.Windows.Forms.CheckBox cbSelectNone;
        private System.Windows.Forms.CheckBox cbSelectAll;
        private System.Windows.Forms.CheckBox cbCustomObjectsOnly;
        private System.Windows.Forms.CheckBox cbFilterManagedPkg;
        private System.Windows.Forms.Button btnExportSelected;
    }
}