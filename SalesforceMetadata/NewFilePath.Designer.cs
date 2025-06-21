namespace SalesforceMetadata
{
    partial class NewFilePath
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
            this.lblSolutionFolderPath = new System.Windows.Forms.Label();
            this.tbSolutionFolderPath = new System.Windows.Forms.TextBox();
            this.lblSolutionFileName = new System.Windows.Forms.Label();
            this.tbSolutionFileName = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblBaseFolderPath = new System.Windows.Forms.Label();
            this.tbBaseFolderPath = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblSolutionFolderPath
            // 
            this.lblSolutionFolderPath.AutoSize = true;
            this.lblSolutionFolderPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSolutionFolderPath.Location = new System.Drawing.Point(16, 70);
            this.lblSolutionFolderPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSolutionFolderPath.Name = "lblSolutionFolderPath";
            this.lblSolutionFolderPath.Size = new System.Drawing.Size(156, 17);
            this.lblSolutionFolderPath.TabIndex = 2;
            this.lblSolutionFolderPath.Text = "Solution Folder Path";
            // 
            // tbSolutionFolderPath
            // 
            this.tbSolutionFolderPath.Location = new System.Drawing.Point(180, 67);
            this.tbSolutionFolderPath.Margin = new System.Windows.Forms.Padding(4);
            this.tbSolutionFolderPath.Name = "tbSolutionFolderPath";
            this.tbSolutionFolderPath.Size = new System.Drawing.Size(1002, 23);
            this.tbSolutionFolderPath.TabIndex = 3;
            this.tbSolutionFolderPath.DoubleClick += new System.EventHandler(this.tbSolutionFolderPath_DoubleClick);
            // 
            // lblSolutionFileName
            // 
            this.lblSolutionFileName.AutoSize = true;
            this.lblSolutionFileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSolutionFileName.Location = new System.Drawing.Point(16, 112);
            this.lblSolutionFileName.Name = "lblSolutionFileName";
            this.lblSolutionFileName.Size = new System.Drawing.Size(144, 17);
            this.lblSolutionFileName.TabIndex = 4;
            this.lblSolutionFileName.Text = "Solution File Name";
            // 
            // tbSolutionFileName
            // 
            this.tbSolutionFileName.Location = new System.Drawing.Point(180, 109);
            this.tbSolutionFileName.Name = "tbSolutionFileName";
            this.tbSolutionFileName.Size = new System.Drawing.Size(418, 23);
            this.tbSolutionFileName.TabIndex = 5;
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(19, 293);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(173, 33);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(198, 293);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(173, 33);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblBaseFolderPath
            // 
            this.lblBaseFolderPath.AutoSize = true;
            this.lblBaseFolderPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBaseFolderPath.Location = new System.Drawing.Point(16, 27);
            this.lblBaseFolderPath.Name = "lblBaseFolderPath";
            this.lblBaseFolderPath.Size = new System.Drawing.Size(133, 17);
            this.lblBaseFolderPath.TabIndex = 0;
            this.lblBaseFolderPath.Text = "Base Folder Path";
            // 
            // tbBaseFolderPath
            // 
            this.tbBaseFolderPath.Location = new System.Drawing.Point(180, 27);
            this.tbBaseFolderPath.Name = "tbBaseFolderPath";
            this.tbBaseFolderPath.Size = new System.Drawing.Size(1002, 23);
            this.tbBaseFolderPath.TabIndex = 1;
            this.tbBaseFolderPath.DoubleClick += new System.EventHandler(this.tbBaseFolderPath_DoubleClick);
            // 
            // NewFilePath
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1319, 554);
            this.Controls.Add(this.tbBaseFolderPath);
            this.Controls.Add(this.lblBaseFolderPath);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tbSolutionFileName);
            this.Controls.Add(this.lblSolutionFileName);
            this.Controls.Add(this.tbSolutionFolderPath);
            this.Controls.Add(this.lblSolutionFolderPath);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "NewFilePath";
            this.Text = "New File";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSolutionFolderPath;
        public System.Windows.Forms.TextBox tbSolutionFolderPath;
        private System.Windows.Forms.Label lblSolutionFileName;
        private System.Windows.Forms.TextBox tbSolutionFileName;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblBaseFolderPath;
        public System.Windows.Forms.TextBox tbBaseFolderPath;
    }
}