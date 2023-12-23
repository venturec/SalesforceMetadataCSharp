namespace SalesforceMetadata
{
    partial class VersionUpdater
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VersionUpdater));
            this.tbComponentsLocation = new System.Windows.Forms.TextBox();
            this.cmbDefaultAPI = new System.Windows.Forms.ComboBox();
            this.btnUpdateAPI = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbComponentsLocation
            // 
            this.tbComponentsLocation.Location = new System.Drawing.Point(138, 29);
            this.tbComponentsLocation.Name = "tbComponentsLocation";
            this.tbComponentsLocation.Size = new System.Drawing.Size(821, 20);
            this.tbComponentsLocation.TabIndex = 0;
            this.tbComponentsLocation.DoubleClick += new System.EventHandler(this.tbComponentsLocation_DoubleClick);
            // 
            // cmbDefaultAPI
            // 
            this.cmbDefaultAPI.FormattingEnabled = true;
            this.cmbDefaultAPI.Location = new System.Drawing.Point(139, 75);
            this.cmbDefaultAPI.Name = "cmbDefaultAPI";
            this.cmbDefaultAPI.Size = new System.Drawing.Size(121, 21);
            this.cmbDefaultAPI.TabIndex = 1;
            // 
            // btnUpdateAPI
            // 
            this.btnUpdateAPI.Enabled = false;
            this.btnUpdateAPI.Location = new System.Drawing.Point(138, 144);
            this.btnUpdateAPI.Name = "btnUpdateAPI";
            this.btnUpdateAPI.Size = new System.Drawing.Size(156, 31);
            this.btnUpdateAPI.TabIndex = 2;
            this.btnUpdateAPI.Text = "Update API";
            this.btnUpdateAPI.UseVisualStyleBackColor = true;
            this.btnUpdateAPI.Click += new System.EventHandler(this.btnUpdateAPI_Click);
            // 
            // VersionUpdater
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1131, 541);
            this.Controls.Add(this.btnUpdateAPI);
            this.Controls.Add(this.cmbDefaultAPI);
            this.Controls.Add(this.tbComponentsLocation);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "VersionUpdater";
            this.Text = "API Version Updater";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbComponentsLocation;
        private System.Windows.Forms.ComboBox cmbDefaultAPI;
        private System.Windows.Forms.Button btnUpdateAPI;
    }
}