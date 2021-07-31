namespace SalesforceMetadata
{
    partial class LandingPageTemplate
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
            // 
            // LandingPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 478);
            this.Controls.Add(this.horizontalRule);
            this.Controls.Add(this.mainMenuStrip);
            this.Controls.Add(this.btnDevSBSeeding);
            this.Controls.Add(this.btnMetadataForm);
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "LandingPage";
            this.Text = "Landing Page";
            this.contextMenuStrip1.ResumeLayout(false);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnMetadataForm;
        private System.Windows.Forms.Button btnDevSBSeeding;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateUserAndSOAPAPIToolStripMenuItem;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem addUserAndSOAPAPIAddressToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateUserAndSOAPAPIAddressToolStripMenuItem;
        private System.Windows.Forms.Label horizontalRule;
    }
}