
namespace SalesforceMetadata
{
    partial class LWCInspector
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
            this.tbLWCFolderPath = new System.Windows.Forms.TextBox();
            this.btnParseLWC = new System.Windows.Forms.Button();
            this.lblLWCFolder = new System.Windows.Forms.Label();
            this.tbSaveResultsTo = new System.Windows.Forms.TextBox();
            this.lblSaveTo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbLWCFolderPath
            // 
            this.tbLWCFolderPath.Location = new System.Drawing.Point(120, 35);
            this.tbLWCFolderPath.Name = "tbLWCFolderPath";
            this.tbLWCFolderPath.Size = new System.Drawing.Size(780, 20);
            this.tbLWCFolderPath.TabIndex = 1;
            this.tbLWCFolderPath.DoubleClick += new System.EventHandler(this.tbLWCFolderPath_DoubleClick);
            // 
            // btnParseLWC
            // 
            this.btnParseLWC.Location = new System.Drawing.Point(922, 35);
            this.btnParseLWC.Name = "btnParseLWC";
            this.btnParseLWC.Size = new System.Drawing.Size(143, 23);
            this.btnParseLWC.TabIndex = 4;
            this.btnParseLWC.Text = "Parse LWC";
            this.btnParseLWC.UseVisualStyleBackColor = true;
            this.btnParseLWC.Click += new System.EventHandler(this.btnParseLWC_Click);
            // 
            // lblLWCFolder
            // 
            this.lblLWCFolder.AutoSize = true;
            this.lblLWCFolder.Location = new System.Drawing.Point(12, 38);
            this.lblLWCFolder.Name = "lblLWCFolder";
            this.lblLWCFolder.Size = new System.Drawing.Size(88, 13);
            this.lblLWCFolder.TabIndex = 0;
            this.lblLWCFolder.Text = "LWC Folder Path";
            // 
            // tbSaveResultsTo
            // 
            this.tbSaveResultsTo.Location = new System.Drawing.Point(120, 96);
            this.tbSaveResultsTo.Name = "tbSaveResultsTo";
            this.tbSaveResultsTo.Size = new System.Drawing.Size(780, 20);
            this.tbSaveResultsTo.TabIndex = 3;
            this.tbSaveResultsTo.Tag = "";
            this.tbSaveResultsTo.Text = "C:\\Users\\marcu\\Documents\\Projects\\Stripe\\Documentation";
            this.tbSaveResultsTo.DoubleClick += new System.EventHandler(this.tbSaveResultsTo_DoubleClick);
            // 
            // lblSaveTo
            // 
            this.lblSaveTo.AutoSize = true;
            this.lblSaveTo.Location = new System.Drawing.Point(12, 99);
            this.lblSaveTo.Name = "lblSaveTo";
            this.lblSaveTo.Size = new System.Drawing.Size(86, 13);
            this.lblSaveTo.TabIndex = 2;
            this.lblSaveTo.Text = "Save Results To";
            // 
            // LWCInspector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1293, 604);
            this.Controls.Add(this.lblSaveTo);
            this.Controls.Add(this.tbSaveResultsTo);
            this.Controls.Add(this.lblLWCFolder);
            this.Controls.Add(this.btnParseLWC);
            this.Controls.Add(this.tbLWCFolderPath);
            this.Name = "LWCInspector";
            this.Text = "LWCInspector";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbLWCFolderPath;
        private System.Windows.Forms.Button btnParseLWC;
        private System.Windows.Forms.Label lblLWCFolder;
        private System.Windows.Forms.TextBox tbSaveResultsTo;
        private System.Windows.Forms.Label lblSaveTo;
    }
}