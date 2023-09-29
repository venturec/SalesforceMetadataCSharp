namespace SalesforceMetadata
{
    partial class ParseSalesforceMetadata
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
            this.cbVSCodeStyle = new System.Windows.Forms.CheckBox();
            this.btnParseMetadata = new System.Windows.Forms.Button();
            this.tbMetadataFolderPath = new System.Windows.Forms.TextBox();
            this.lblMetadataFolderPath = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbVSCodeStyle
            // 
            this.cbVSCodeStyle.AutoSize = true;
            this.cbVSCodeStyle.Checked = true;
            this.cbVSCodeStyle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbVSCodeStyle.Location = new System.Drawing.Point(12, 12);
            this.cbVSCodeStyle.Name = "cbVSCodeStyle";
            this.cbVSCodeStyle.Size = new System.Drawing.Size(94, 17);
            this.cbVSCodeStyle.TabIndex = 0;
            this.cbVSCodeStyle.Text = "VS Code Style";
            this.cbVSCodeStyle.UseVisualStyleBackColor = true;
            // 
            // btnParseMetadata
            // 
            this.btnParseMetadata.Location = new System.Drawing.Point(756, 85);
            this.btnParseMetadata.Name = "btnParseMetadata";
            this.btnParseMetadata.Size = new System.Drawing.Size(75, 23);
            this.btnParseMetadata.TabIndex = 1;
            this.btnParseMetadata.Text = "Parse Metadata";
            this.btnParseMetadata.UseVisualStyleBackColor = true;
            this.btnParseMetadata.Click += new System.EventHandler(this.btnParseMetadata_Click);
            // 
            // tbMetadataFolderPath
            // 
            this.tbMetadataFolderPath.Location = new System.Drawing.Point(124, 49);
            this.tbMetadataFolderPath.Name = "tbMetadataFolderPath";
            this.tbMetadataFolderPath.Size = new System.Drawing.Size(707, 20);
            this.tbMetadataFolderPath.TabIndex = 2;
            this.tbMetadataFolderPath.DoubleClick += new System.EventHandler(this.tbMetadataFolderPath_DoubleClick);
            // 
            // lblMetadataFolderPath
            // 
            this.lblMetadataFolderPath.AutoSize = true;
            this.lblMetadataFolderPath.Location = new System.Drawing.Point(9, 52);
            this.lblMetadataFolderPath.Name = "lblMetadataFolderPath";
            this.lblMetadataFolderPath.Size = new System.Drawing.Size(109, 13);
            this.lblMetadataFolderPath.TabIndex = 3;
            this.lblMetadataFolderPath.Text = "Metadata Folder Path";
            // 
            // ParseSalesforceMetadata
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 475);
            this.Controls.Add(this.lblMetadataFolderPath);
            this.Controls.Add(this.tbMetadataFolderPath);
            this.Controls.Add(this.btnParseMetadata);
            this.Controls.Add(this.cbVSCodeStyle);
            this.Name = "ParseSalesforceMetadata";
            this.Text = "ParseSalesforceMetadata";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbVSCodeStyle;
        private System.Windows.Forms.Button btnParseMetadata;
        private System.Windows.Forms.TextBox tbMetadataFolderPath;
        private System.Windows.Forms.Label lblMetadataFolderPath;
    }
}