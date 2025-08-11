namespace SalesforceMetadata
{
    partial class ParseDebugLogs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParseDebugLogs));
            this.tbDebugFile = new System.Windows.Forms.TextBox();
            this.lblFileToParse = new System.Windows.Forms.Label();
            this.horizontalLine1 = new System.Windows.Forms.TextBox();
            this.btnParseSelectedDebugLogs = new System.Windows.Forms.Button();
            this.cbIncludeHierarchy = new System.Windows.Forms.CheckBox();
            this.btnParseCodeUnits = new System.Windows.Forms.Button();
            this.fromOrgGroup = new System.Windows.Forms.GroupBox();
            this.lblSFUsername = new System.Windows.Forms.Label();
            this.cmbUserName = new System.Windows.Forms.ComboBox();
            this.btnDeleteDebugLogs = new System.Windows.Forms.Button();
            this.cbAllDebugLogs = new System.Windows.Forms.CheckBox();
            this.btnGetDebugLogs = new System.Windows.Forms.Button();
            this.debugLogsDataTable = new System.Windows.Forms.DataGridView();
            this.lblFolder = new System.Windows.Forms.Label();
            this.tbFolderPath = new System.Windows.Forms.TextBox();
            this.btnParseFolderDebubLogs = new System.Windows.Forms.Button();
            this.btnDeleteAPILogs = new System.Windows.Forms.Button();
            this.fromOrgGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.debugLogsDataTable)).BeginInit();
            this.SuspendLayout();
            // 
            // tbDebugFile
            // 
            this.tbDebugFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDebugFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbDebugFile.Location = new System.Drawing.Point(141, 247);
            this.tbDebugFile.Name = "tbDebugFile";
            this.tbDebugFile.Size = new System.Drawing.Size(957, 23);
            this.tbDebugFile.TabIndex = 5;
            this.tbDebugFile.DoubleClick += new System.EventHandler(this.tbDebugFile_DoubleClick);
            // 
            // lblFileToParse
            // 
            this.lblFileToParse.AutoSize = true;
            this.lblFileToParse.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFileToParse.Location = new System.Drawing.Point(17, 247);
            this.lblFileToParse.Name = "lblFileToParse";
            this.lblFileToParse.Size = new System.Drawing.Size(87, 17);
            this.lblFileToParse.TabIndex = 4;
            this.lblFileToParse.Text = "File to Parse";
            // 
            // horizontalLine1
            // 
            this.horizontalLine1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalLine1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.horizontalLine1.Location = new System.Drawing.Point(-1, 332);
            this.horizontalLine1.MaxLength = 0;
            this.horizontalLine1.Name = "horizontalLine1";
            this.horizontalLine1.Size = new System.Drawing.Size(1435, 20);
            this.horizontalLine1.TabIndex = 8;
            // 
            // btnParseSelectedDebugLogs
            // 
            this.btnParseSelectedDebugLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnParseSelectedDebugLogs.Location = new System.Drawing.Point(141, 279);
            this.btnParseSelectedDebugLogs.Name = "btnParseSelectedDebugLogs";
            this.btnParseSelectedDebugLogs.Size = new System.Drawing.Size(202, 34);
            this.btnParseSelectedDebugLogs.TabIndex = 6;
            this.btnParseSelectedDebugLogs.Text = "Parse Selected Debug Logs";
            this.btnParseSelectedDebugLogs.UseVisualStyleBackColor = true;
            this.btnParseSelectedDebugLogs.Click += new System.EventHandler(this.btnParseDebugLogFile_Click);
            // 
            // cbIncludeHierarchy
            // 
            this.cbIncludeHierarchy.AutoSize = true;
            this.cbIncludeHierarchy.Checked = true;
            this.cbIncludeHierarchy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIncludeHierarchy.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbIncludeHierarchy.Location = new System.Drawing.Point(1232, 142);
            this.cbIncludeHierarchy.Name = "cbIncludeHierarchy";
            this.cbIncludeHierarchy.Size = new System.Drawing.Size(191, 21);
            this.cbIncludeHierarchy.TabIndex = 12;
            this.cbIncludeHierarchy.Text = "Include Hierarchy Number";
            this.cbIncludeHierarchy.UseVisualStyleBackColor = true;
            // 
            // btnParseCodeUnits
            // 
            this.btnParseCodeUnits.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnParseCodeUnits.Location = new System.Drawing.Point(349, 280);
            this.btnParseCodeUnits.Name = "btnParseCodeUnits";
            this.btnParseCodeUnits.Size = new System.Drawing.Size(202, 33);
            this.btnParseCodeUnits.TabIndex = 7;
            this.btnParseCodeUnits.Text = "Parse Code Units";
            this.btnParseCodeUnits.UseVisualStyleBackColor = true;
            this.btnParseCodeUnits.Click += new System.EventHandler(this.btnParseCodeUnits_Click);
            // 
            // fromOrgGroup
            // 
            this.fromOrgGroup.BackColor = System.Drawing.SystemColors.Control;
            this.fromOrgGroup.Controls.Add(this.lblSFUsername);
            this.fromOrgGroup.Controls.Add(this.cmbUserName);
            this.fromOrgGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fromOrgGroup.Location = new System.Drawing.Point(15, 15);
            this.fromOrgGroup.Margin = new System.Windows.Forms.Padding(6);
            this.fromOrgGroup.Name = "fromOrgGroup";
            this.fromOrgGroup.Padding = new System.Windows.Forms.Padding(6);
            this.fromOrgGroup.Size = new System.Drawing.Size(731, 106);
            this.fromOrgGroup.TabIndex = 0;
            this.fromOrgGroup.TabStop = false;
            this.fromOrgGroup.Text = "From Org";
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
            // 
            // btnDeleteDebugLogs
            // 
            this.btnDeleteDebugLogs.BackColor = System.Drawing.SystemColors.Control;
            this.btnDeleteDebugLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteDebugLogs.Location = new System.Drawing.Point(1042, 44);
            this.btnDeleteDebugLogs.Margin = new System.Windows.Forms.Padding(4);
            this.btnDeleteDebugLogs.Name = "btnDeleteDebugLogs";
            this.btnDeleteDebugLogs.Size = new System.Drawing.Size(194, 46);
            this.btnDeleteDebugLogs.TabIndex = 10;
            this.btnDeleteDebugLogs.Text = "Delete Your Debug Logs";
            this.btnDeleteDebugLogs.UseVisualStyleBackColor = true;
            this.btnDeleteDebugLogs.Click += new System.EventHandler(this.btnDeleteDebugLogs_Click);
            // 
            // cbAllDebugLogs
            // 
            this.cbAllDebugLogs.AutoSize = true;
            this.cbAllDebugLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbAllDebugLogs.Location = new System.Drawing.Point(1252, 59);
            this.cbAllDebugLogs.Margin = new System.Windows.Forms.Padding(6);
            this.cbAllDebugLogs.Name = "cbAllDebugLogs";
            this.cbAllDebugLogs.Size = new System.Drawing.Size(168, 21);
            this.cbAllDebugLogs.TabIndex = 11;
            this.cbAllDebugLogs.Text = "Delete All Debug Logs";
            this.cbAllDebugLogs.UseVisualStyleBackColor = true;
            this.cbAllDebugLogs.CheckedChanged += new System.EventHandler(this.cbAllDebugLogs_CheckedChanged);
            // 
            // btnGetDebugLogs
            // 
            this.btnGetDebugLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetDebugLogs.Location = new System.Drawing.Point(755, 44);
            this.btnGetDebugLogs.Name = "btnGetDebugLogs";
            this.btnGetDebugLogs.Size = new System.Drawing.Size(202, 46);
            this.btnGetDebugLogs.TabIndex = 1;
            this.btnGetDebugLogs.Text = "Get Debug Logs";
            this.btnGetDebugLogs.UseVisualStyleBackColor = true;
            this.btnGetDebugLogs.Click += new System.EventHandler(this.btnGetDebugLogs_Click);
            // 
            // debugLogsDataTable
            // 
            this.debugLogsDataTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.debugLogsDataTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.debugLogsDataTable.Location = new System.Drawing.Point(12, 358);
            this.debugLogsDataTable.Name = "debugLogsDataTable";
            this.debugLogsDataTable.Size = new System.Drawing.Size(1402, 514);
            this.debugLogsDataTable.TabIndex = 9;
            // 
            // lblFolder
            // 
            this.lblFolder.AutoSize = true;
            this.lblFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFolder.Location = new System.Drawing.Point(17, 143);
            this.lblFolder.Name = "lblFolder";
            this.lblFolder.Size = new System.Drawing.Size(105, 17);
            this.lblFolder.TabIndex = 2;
            this.lblFolder.Text = "Folder to Parse";
            // 
            // tbFolderPath
            // 
            this.tbFolderPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbFolderPath.Location = new System.Drawing.Point(141, 140);
            this.tbFolderPath.Name = "tbFolderPath";
            this.tbFolderPath.Size = new System.Drawing.Size(957, 23);
            this.tbFolderPath.TabIndex = 3;
            this.tbFolderPath.DoubleClick += new System.EventHandler(this.tbFolderPath_DoubleClick);
            // 
            // btnParseFolderDebubLogs
            // 
            this.btnParseFolderDebubLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnParseFolderDebubLogs.Location = new System.Drawing.Point(141, 169);
            this.btnParseFolderDebubLogs.Name = "btnParseFolderDebubLogs";
            this.btnParseFolderDebubLogs.Size = new System.Drawing.Size(202, 33);
            this.btnParseFolderDebubLogs.TabIndex = 13;
            this.btnParseFolderDebubLogs.Text = "Parse Folder Debug Logs";
            this.btnParseFolderDebubLogs.UseVisualStyleBackColor = true;
            this.btnParseFolderDebubLogs.Click += new System.EventHandler(this.btnParseFolderDebubLogs_Click);
            // 
            // btnDeleteAPILogs
            // 
            this.btnDeleteAPILogs.Location = new System.Drawing.Point(1042, 97);
            this.btnDeleteAPILogs.Name = "btnDeleteAPILogs";
            this.btnDeleteAPILogs.Size = new System.Drawing.Size(194, 23);
            this.btnDeleteAPILogs.TabIndex = 14;
            this.btnDeleteAPILogs.Text = "Delete API Logs";
            this.btnDeleteAPILogs.UseVisualStyleBackColor = true;
            this.btnDeleteAPILogs.Click += new System.EventHandler(this.btnDeleteAPILogs_Click);
            // 
            // ParseDebugLogs
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1435, 884);
            this.Controls.Add(this.btnDeleteAPILogs);
            this.Controls.Add(this.btnParseFolderDebubLogs);
            this.Controls.Add(this.tbFolderPath);
            this.Controls.Add(this.lblFolder);
            this.Controls.Add(this.debugLogsDataTable);
            this.Controls.Add(this.btnGetDebugLogs);
            this.Controls.Add(this.fromOrgGroup);
            this.Controls.Add(this.btnParseCodeUnits);
            this.Controls.Add(this.cbIncludeHierarchy);
            this.Controls.Add(this.btnParseSelectedDebugLogs);
            this.Controls.Add(this.horizontalLine1);
            this.Controls.Add(this.lblFileToParse);
            this.Controls.Add(this.tbDebugFile);
            this.Controls.Add(this.cbAllDebugLogs);
            this.Controls.Add(this.btnDeleteDebugLogs);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ParseDebugLogs";
            this.Text = "Parse Debug Logs";
            this.Load += new System.EventHandler(this.ParseDebugLogs_Load);
            this.fromOrgGroup.ResumeLayout(false);
            this.fromOrgGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.debugLogsDataTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox tbDebugFile;
        private System.Windows.Forms.Label lblFileToParse;
        private System.Windows.Forms.TextBox horizontalLine1;
        private System.Windows.Forms.Button btnParseSelectedDebugLogs;
        private System.Windows.Forms.CheckBox cbIncludeHierarchy;
        private System.Windows.Forms.Button btnParseCodeUnits;
        private System.Windows.Forms.GroupBox fromOrgGroup;
        private System.Windows.Forms.Label lblSFUsername;
        private System.Windows.Forms.ComboBox cmbUserName;
        private System.Windows.Forms.Button btnDeleteDebugLogs;
        private System.Windows.Forms.CheckBox cbAllDebugLogs;
        private System.Windows.Forms.Button btnGetDebugLogs;
        private System.Windows.Forms.DataGridView debugLogsDataTable;
        private System.Windows.Forms.Label lblFolder;
        private System.Windows.Forms.TextBox tbFolderPath;
        private System.Windows.Forms.Button btnParseFolderDebubLogs;
        private System.Windows.Forms.Button btnDeleteAPILogs;
    }
}