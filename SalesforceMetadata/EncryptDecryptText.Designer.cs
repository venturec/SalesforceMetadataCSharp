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
            this.tbSharedSecret.Location = new System.Drawing.Point(112, 7);
            this.tbSharedSecret.Name = "tbSharedSecret";
            this.tbSharedSecret.Size = new System.Drawing.Size(439, 20);
            this.tbSharedSecret.TabIndex = 1;
            // 
            // tbSalt
            // 
            this.tbSalt.Location = new System.Drawing.Point(112, 33);
            this.tbSalt.Name = "tbSalt";
            this.tbSalt.Size = new System.Drawing.Size(439, 20);
            this.tbSalt.TabIndex = 3;
            // 
            // tbEncryptedText
            // 
            this.tbEncryptedText.AcceptsReturn = true;
            this.tbEncryptedText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEncryptedText.Location = new System.Drawing.Point(112, 127);
            this.tbEncryptedText.Multiline = true;
            this.tbEncryptedText.Name = "tbEncryptedText";
            this.tbEncryptedText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbEncryptedText.Size = new System.Drawing.Size(790, 241);
            this.tbEncryptedText.TabIndex = 5;
            this.tbEncryptedText.WordWrap = false;
            // 
            // btnEncryptDecrypt
            // 
            this.btnEncryptDecrypt.Location = new System.Drawing.Point(289, 77);
            this.btnEncryptDecrypt.Name = "btnEncryptDecrypt";
            this.btnEncryptDecrypt.Size = new System.Drawing.Size(149, 23);
            this.btnEncryptDecrypt.TabIndex = 8;
            this.btnEncryptDecrypt.Text = "Encrypt Text";
            this.btnEncryptDecrypt.UseVisualStyleBackColor = true;
            this.btnEncryptDecrypt.Click += new System.EventHandler(this.btnEncryptClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Shared Secret";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Salt";
            // 
            // lblEncryptedText
            // 
            this.lblEncryptedText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEncryptedText.AutoSize = true;
            this.lblEncryptedText.Location = new System.Drawing.Point(9, 129);
            this.lblEncryptedText.Name = "lblEncryptedText";
            this.lblEncryptedText.Size = new System.Drawing.Size(79, 13);
            this.lblEncryptedText.TabIndex = 4;
            this.lblEncryptedText.Text = "Encrypted Text";
            // 
            // tbDecryptedText
            // 
            this.tbDecryptedText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDecryptedText.Location = new System.Drawing.Point(112, 390);
            this.tbDecryptedText.Multiline = true;
            this.tbDecryptedText.Name = "tbDecryptedText";
            this.tbDecryptedText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbDecryptedText.Size = new System.Drawing.Size(790, 327);
            this.tbDecryptedText.TabIndex = 10;
            this.tbDecryptedText.TextChanged += new System.EventHandler(this.tbDecryptedText_TextChanged);
            // 
            // lblDecryptedText
            // 
            this.lblDecryptedText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDecryptedText.AutoSize = true;
            this.lblDecryptedText.Location = new System.Drawing.Point(8, 390);
            this.lblDecryptedText.Name = "lblDecryptedText";
            this.lblDecryptedText.Size = new System.Drawing.Size(80, 13);
            this.lblDecryptedText.TabIndex = 9;
            this.lblDecryptedText.Text = "Decrypted Text";
            // 
            // btnSaveEncryptedText
            // 
            this.btnSaveEncryptedText.Location = new System.Drawing.Point(463, 77);
            this.btnSaveEncryptedText.Name = "btnSaveEncryptedText";
            this.btnSaveEncryptedText.Size = new System.Drawing.Size(149, 23);
            this.btnSaveEncryptedText.TabIndex = 11;
            this.btnSaveEncryptedText.Text = "Save Encrypted To File";
            this.btnSaveEncryptedText.UseVisualStyleBackColor = true;
            this.btnSaveEncryptedText.Click += new System.EventHandler(this.btnSaveResults_Click);
            // 
            // btnDecryptText
            // 
            this.btnDecryptText.Location = new System.Drawing.Point(112, 77);
            this.btnDecryptText.Name = "btnDecryptText";
            this.btnDecryptText.Size = new System.Drawing.Size(149, 23);
            this.btnDecryptText.TabIndex = 12;
            this.btnDecryptText.Text = "Decrypt Text";
            this.btnDecryptText.UseVisualStyleBackColor = true;
            this.btnDecryptText.Click += new System.EventHandler(this.btnDecryptText_Click);
            // 
            // EncryptDecryptText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(918, 726);
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