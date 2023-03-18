namespace SalesforceMetadata
{
    partial class ExtractFieldsFromMetadata
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtractFieldsFromMetadata));
            this.tbSelectedFolder = new System.Windows.Forms.TextBox();
            this.btnExtractFields = new System.Windows.Forms.Button();
            this.lblSelectedFolder = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbSelectedFolder
            // 
            this.tbSelectedFolder.Location = new System.Drawing.Point(125, 26);
            this.tbSelectedFolder.Name = "tbSelectedFolder";
            this.tbSelectedFolder.Size = new System.Drawing.Size(751, 20);
            this.tbSelectedFolder.TabIndex = 1;
            this.tbSelectedFolder.DoubleClick += new System.EventHandler(this.tbSelectedFolder_DoubleClick);
            // 
            // btnExtractFields
            // 
            this.btnExtractFields.Location = new System.Drawing.Point(720, 66);
            this.btnExtractFields.Name = "btnExtractFields";
            this.btnExtractFields.Size = new System.Drawing.Size(156, 23);
            this.btnExtractFields.TabIndex = 2;
            this.btnExtractFields.Text = "Extract Fields to Excel";
            this.btnExtractFields.UseVisualStyleBackColor = true;
            this.btnExtractFields.Click += new System.EventHandler(this.btnExtractFields_Click);
            // 
            // lblSelectedFolder
            // 
            this.lblSelectedFolder.AutoSize = true;
            this.lblSelectedFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedFolder.Location = new System.Drawing.Point(12, 29);
            this.lblSelectedFolder.Name = "lblSelectedFolder";
            this.lblSelectedFolder.Size = new System.Drawing.Size(96, 13);
            this.lblSelectedFolder.TabIndex = 0;
            this.lblSelectedFolder.Text = "Selected Folder";
            // 
            // ExtractFieldsFromMetadata
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 487);
            this.Controls.Add(this.lblSelectedFolder);
            this.Controls.Add(this.btnExtractFields);
            this.Controls.Add(this.tbSelectedFolder);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExtractFieldsFromMetadata";
            this.Text = "ExtractFieldsFromMetadata";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbSelectedFolder;
        private System.Windows.Forms.Button btnExtractFields;
        private System.Windows.Forms.Label lblSelectedFolder;
    }
}