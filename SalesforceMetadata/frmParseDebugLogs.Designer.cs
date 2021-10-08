namespace SalesforceMetadata
{
    partial class frmParseDebugLogs
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
            this.tbDebugFile = new System.Windows.Forms.TextBox();
            this.lblFileToParse = new System.Windows.Forms.Label();
            this.horizontalLine1 = new System.Windows.Forms.TextBox();
            this.parseSpecifics = new System.Windows.Forms.CheckedListBox();
            this.lblParseSpecifics = new System.Windows.Forms.Label();
            this.btnParseDebugLogFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbDebugFile
            // 
            this.tbDebugFile.Location = new System.Drawing.Point(111, 53);
            this.tbDebugFile.Name = "tbDebugFile";
            this.tbDebugFile.Size = new System.Drawing.Size(723, 20);
            this.tbDebugFile.TabIndex = 0;
            this.tbDebugFile.DoubleClick += new System.EventHandler(this.tbDebugFile_DoubleClick);
            // 
            // lblFileToParse
            // 
            this.lblFileToParse.AutoSize = true;
            this.lblFileToParse.Location = new System.Drawing.Point(25, 53);
            this.lblFileToParse.Name = "lblFileToParse";
            this.lblFileToParse.Size = new System.Drawing.Size(65, 13);
            this.lblFileToParse.TabIndex = 1;
            this.lblFileToParse.Text = "File to Parse";
            // 
            // horizontalLine1
            // 
            this.horizontalLine1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalLine1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.horizontalLine1.Location = new System.Drawing.Point(-1, 425);
            this.horizontalLine1.MaxLength = 0;
            this.horizontalLine1.Name = "horizontalLine1";
            this.horizontalLine1.Size = new System.Drawing.Size(1163, 20);
            this.horizontalLine1.TabIndex = 2;
            // 
            // parseSpecifics
            // 
            this.parseSpecifics.FormattingEnabled = true;
            this.parseSpecifics.Items.AddRange(new object[] {
            "All",
            "EXCEPTION_THROWN",
            "FATAL_ERROR",
            "USER_DEBUG",
            "CODE_UNIT_STARTED",
            "CONSTRUCTOR_ENTRY",
            "METHOD_ENTRY",
            "ENTERING_MANAGED_PKG",
            "FLOW_CREATE_INTERVIEW_BEGIN",
            "FLOW_START_INTERVIEW_BEGIN",
            "DML_BEGIN",
            "SOQL_EXECUTE_BEGIN",
            "SOSL_EXECUTE_BEGIN",
            "WF_FLOW_ACTION_BEGIN",
            "VARIABLE_ASSIGNMENT",
            "VARIABLE_SCOPE_BEGIN"});
            this.parseSpecifics.Location = new System.Drawing.Point(111, 127);
            this.parseSpecifics.Name = "parseSpecifics";
            this.parseSpecifics.Size = new System.Drawing.Size(231, 289);
            this.parseSpecifics.TabIndex = 3;
            // 
            // lblParseSpecifics
            // 
            this.lblParseSpecifics.AutoSize = true;
            this.lblParseSpecifics.Location = new System.Drawing.Point(25, 127);
            this.lblParseSpecifics.Name = "lblParseSpecifics";
            this.lblParseSpecifics.Size = new System.Drawing.Size(80, 13);
            this.lblParseSpecifics.TabIndex = 4;
            this.lblParseSpecifics.Text = "Parse Specifics";
            // 
            // btnParseDebugLogFile
            // 
            this.btnParseDebugLogFile.Location = new System.Drawing.Point(1030, 262);
            this.btnParseDebugLogFile.Name = "btnParseDebugLogFile";
            this.btnParseDebugLogFile.Size = new System.Drawing.Size(121, 34);
            this.btnParseDebugLogFile.TabIndex = 5;
            this.btnParseDebugLogFile.Text = "Parse Debug Log File";
            this.btnParseDebugLogFile.UseVisualStyleBackColor = true;
            this.btnParseDebugLogFile.Click += new System.EventHandler(this.btnParseDebugLogFile_Click);
            // 
            // frmParseDebugLogs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1163, 603);
            this.Controls.Add(this.btnParseDebugLogFile);
            this.Controls.Add(this.lblParseSpecifics);
            this.Controls.Add(this.parseSpecifics);
            this.Controls.Add(this.horizontalLine1);
            this.Controls.Add(this.lblFileToParse);
            this.Controls.Add(this.tbDebugFile);
            this.Name = "frmParseDebugLogs";
            this.Text = "Parse Debug Logs";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbDebugFile;
        private System.Windows.Forms.Label lblFileToParse;
        private System.Windows.Forms.TextBox horizontalLine1;
        private System.Windows.Forms.CheckedListBox parseSpecifics;
        private System.Windows.Forms.Label lblParseSpecifics;
        private System.Windows.Forms.Button btnParseDebugLogFile;
    }
}