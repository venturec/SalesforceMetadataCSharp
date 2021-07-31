namespace SalesforceMetadata
{
    partial class EncryptDecryptText
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
            this.tbSharedSecret = new System.Windows.Forms.TextBox();
            this.tbSalt = new System.Windows.Forms.TextBox();
            this.tbEncryptedText = new System.Windows.Forms.TextBox();
            this.btnEncryptDecrypt = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblEncryptedText = new System.Windows.Forms.Label();
            this.tbDecryptedText = new System.Windows.Forms.TextBox();
            this.lblDecryptedText = new System.Windows.Forms.Label();
            this.btnSaveEncryptedText = new System.Windows.Forms.Button();
            this.btnDecryptText = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbSharedSecret
            // 
            this.tbSharedSecret.Location = new System.Drawing.Point(149, 9);
            this.tbSharedSecret.Margin = new System.Windows.Forms.Padding(4);
            this.tbSharedSecret.Name = "tbSharedSecret";
            this.tbSharedSecret.Size = new System.Drawing.Size(584, 22);
            this.tbSharedSecret.TabIndex = 1;
            // 
            // tbSalt
            // 
            this.tbSalt.Location = new System.Drawing.Point(149, 41);
            this.tbSalt.Margin = new System.Windows.Forms.Padding(4);
            this.tbSalt.Name = "tbSalt";
            this.tbSalt.Size = new System.Drawing.Size(584, 22);
            this.tbSalt.TabIndex = 3;
            // 
            // tbEncryptedText
            // 
            this.tbEncryptedText.AcceptsReturn = true;
            this.tbEncryptedText.Location = new System.Drawing.Point(149, 156);
            this.tbEncryptedText.Margin = new System.Windows.Forms.Padding(4);
            this.tbEncryptedText.Multiline = true;
            this.tbEncryptedText.Name = "tbEncryptedText";
            this.tbEncryptedText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbEncryptedText.Size = new System.Drawing.Size(1052, 296);
            this.tbEncryptedText.TabIndex = 5;
            this.tbEncryptedText.WordWrap = false;
            // 
            // btnEncryptDecrypt
            // 
            this.btnEncryptDecrypt.Location = new System.Drawing.Point(385, 95);
            this.btnEncryptDecrypt.Margin = new System.Windows.Forms.Padding(4);
            this.btnEncryptDecrypt.Name = "btnEncryptDecrypt";
            this.btnEncryptDecrypt.Size = new System.Drawing.Size(199, 28);
            this.btnEncryptDecrypt.TabIndex = 8;
            this.btnEncryptDecrypt.Text = "Encrypt Text";
            this.btnEncryptDecrypt.UseVisualStyleBackColor = true;
            this.btnEncryptDecrypt.Click += new System.EventHandler(this.btnEncryptClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 9);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Shared Secret";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 41);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Salt";
            // 
            // lblEncryptedText
            // 
            this.lblEncryptedText.AutoSize = true;
            this.lblEncryptedText.Location = new System.Drawing.Point(12, 159);
            this.lblEncryptedText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEncryptedText.Name = "lblEncryptedText";
            this.lblEncryptedText.Size = new System.Drawing.Size(103, 17);
            this.lblEncryptedText.TabIndex = 4;
            this.lblEncryptedText.Text = "Encrypted Text";
            // 
            // tbDecryptedText
            // 
            this.tbDecryptedText.Location = new System.Drawing.Point(149, 480);
            this.tbDecryptedText.Margin = new System.Windows.Forms.Padding(4);
            this.tbDecryptedText.Multiline = true;
            this.tbDecryptedText.Name = "tbDecryptedText";
            this.tbDecryptedText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbDecryptedText.Size = new System.Drawing.Size(1052, 412);
            this.tbDecryptedText.TabIndex = 10;
            this.tbDecryptedText.TextChanged += new System.EventHandler(this.tbDecryptedText_TextChanged);
            // 
            // lblDecryptedText
            // 
            this.lblDecryptedText.AutoSize = true;
            this.lblDecryptedText.Location = new System.Drawing.Point(11, 480);
            this.lblDecryptedText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDecryptedText.Name = "lblDecryptedText";
            this.lblDecryptedText.Size = new System.Drawing.Size(104, 17);
            this.lblDecryptedText.TabIndex = 9;
            this.lblDecryptedText.Text = "Decrypted Text";
            // 
            // btnSaveEncryptedText
            // 
            this.btnSaveEncryptedText.Location = new System.Drawing.Point(617, 95);
            this.btnSaveEncryptedText.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveEncryptedText.Name = "btnSaveEncryptedText";
            this.btnSaveEncryptedText.Size = new System.Drawing.Size(199, 28);
            this.btnSaveEncryptedText.TabIndex = 11;
            this.btnSaveEncryptedText.Text = "Save Encrypted To File";
            this.btnSaveEncryptedText.UseVisualStyleBackColor = true;
            this.btnSaveEncryptedText.Click += new System.EventHandler(this.btnSaveResults_Click);
            // 
            // btnDecryptText
            // 
            this.btnDecryptText.Location = new System.Drawing.Point(149, 95);
            this.btnDecryptText.Margin = new System.Windows.Forms.Padding(4);
            this.btnDecryptText.Name = "btnDecryptText";
            this.btnDecryptText.Size = new System.Drawing.Size(199, 28);
            this.btnDecryptText.TabIndex = 12;
            this.btnDecryptText.Text = "Decrypt Text";
            this.btnDecryptText.UseVisualStyleBackColor = true;
            this.btnDecryptText.Click += new System.EventHandler(this.btnDecryptText_Click);
            // 
            // EncryptDecryptText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1224, 905);
            this.Controls.Add(this.btnDecryptText);
            this.Controls.Add(this.btnSaveEncryptedText);
            this.Controls.Add(this.lblDecryptedText);
            this.Controls.Add(this.tbDecryptedText);
            this.Controls.Add(this.lblEncryptedText);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnEncryptDecrypt);
            this.Controls.Add(this.tbEncryptedText);
            this.Controls.Add(this.tbSalt);
            this.Controls.Add(this.tbSharedSecret);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EncryptDecryptText";
            this.Text = "Encrypt / Decrypt Text";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EncryptDecryptText_FormClosing);
            this.Leave += new System.EventHandler(this.tbDecryptedText_TextChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tbSharedSecret;
        private System.Windows.Forms.TextBox tbSalt;
        private System.Windows.Forms.TextBox tbEncryptedText;
        private System.Windows.Forms.Button btnEncryptDecrypt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblEncryptedText;
        private System.Windows.Forms.TextBox tbDecryptedText;
        private System.Windows.Forms.Label lblDecryptedText;
        private System.Windows.Forms.Button btnSaveEncryptedText;
        private System.Windows.Forms.Button btnDecryptText;
    }
}