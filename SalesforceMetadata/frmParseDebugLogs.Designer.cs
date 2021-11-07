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
            this.btnParseDebugLogFile = new System.Windows.Forms.Button();
            this.btnDebugReplay = new System.Windows.Forms.Button();
            this.tvDebugReplay = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // tbDebugFile
            // 
            this.tbDebugFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.horizontalLine1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalLine1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.horizontalLine1.Location = new System.Drawing.Point(0, 94);
            this.horizontalLine1.MaxLength = 0;
            this.horizontalLine1.Name = "horizontalLine1";
            this.horizontalLine1.Size = new System.Drawing.Size(1163, 20);
            this.horizontalLine1.TabIndex = 2;
            // 
            // btnParseDebugLogFile
            // 
            this.btnParseDebugLogFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnParseDebugLogFile.Location = new System.Drawing.Point(1030, 45);
            this.btnParseDebugLogFile.Name = "btnParseDebugLogFile";
            this.btnParseDebugLogFile.Size = new System.Drawing.Size(121, 34);
            this.btnParseDebugLogFile.TabIndex = 5;
            this.btnParseDebugLogFile.Text = "Parse Debug Log File";
            this.btnParseDebugLogFile.UseVisualStyleBackColor = true;
            this.btnParseDebugLogFile.Click += new System.EventHandler(this.btnParseDebugLogFile_Click);
            // 
            // btnDebugReplay
            // 
            this.btnDebugReplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDebugReplay.Location = new System.Drawing.Point(1030, 120);
            this.btnDebugReplay.Name = "btnDebugReplay";
            this.btnDebugReplay.Size = new System.Drawing.Size(121, 23);
            this.btnDebugReplay.TabIndex = 6;
            this.btnDebugReplay.Text = "Debug Replay";
            this.btnDebugReplay.UseVisualStyleBackColor = true;
            this.btnDebugReplay.Click += new System.EventHandler(this.btnDebugReplay_Click);
            // 
            // tvDebugReplay
            // 
            this.tvDebugReplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvDebugReplay.Location = new System.Drawing.Point(12, 120);
            this.tvDebugReplay.Name = "tvDebugReplay";
            this.tvDebugReplay.Size = new System.Drawing.Size(995, 760);
            this.tvDebugReplay.TabIndex = 7;
            this.tvDebugReplay.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvDebugReplay_AfterExpand);
            // 
            // frmParseDebugLogs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1163, 884);
            this.Controls.Add(this.tvDebugReplay);
            this.Controls.Add(this.btnDebugReplay);
            this.Controls.Add(this.btnParseDebugLogFile);
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
        private System.Windows.Forms.Button btnParseDebugLogFile;
        private System.Windows.Forms.Button btnDebugReplay;
        private System.Windows.Forms.TreeView tvDebugReplay;
    }
}