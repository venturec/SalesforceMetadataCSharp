namespace SalesforceMetadata
{
    partial class ExtractWebsites
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
            this.lblURL = new System.Windows.Forms.Label();
            this.tbURL = new System.Windows.Forms.TextBox();
            this.btnGetPageLinks = new System.Windows.Forms.Button();
            this.tbFileSaveLocation = new System.Windows.Forms.TextBox();
            this.lblSaveURLsToFile = new System.Windows.Forms.Label();
            this.btnRetrieveWebsites = new System.Windows.Forms.Button();
            this.cmbLayers = new System.Windows.Forms.ComboBox();
            this.lblLayers = new System.Windows.Forms.Label();
            this.btnHTMLToText = new System.Windows.Forms.Button();
            this.cbStayInSameDomain = new System.Windows.Forms.CheckBox();
            this.tbPDFFileLocation = new System.Windows.Forms.TextBox();
            this.lblPDFFile = new System.Windows.Forms.Label();
            this.btnPDFToText = new System.Windows.Forms.Button();
            this.cmbIncludeTextPos = new System.Windows.Forms.ComboBox();
            this.lblIncludeTextPositions = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblURL
            // 
            this.lblURL.AutoSize = true;
            this.lblURL.Location = new System.Drawing.Point(12, 55);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(36, 17);
            this.lblURL.TabIndex = 0;
            this.lblURL.Text = "URL";
            // 
            // tbURL
            // 
            this.tbURL.Location = new System.Drawing.Point(64, 55);
            this.tbURL.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbURL.Name = "tbURL";
            this.tbURL.Size = new System.Drawing.Size(1143, 22);
            this.tbURL.TabIndex = 1;
            // 
            // btnGetPageLinks
            // 
            this.btnGetPageLinks.Location = new System.Drawing.Point(12, 222);
            this.btnGetPageLinks.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnGetPageLinks.Name = "btnGetPageLinks";
            this.btnGetPageLinks.Size = new System.Drawing.Size(155, 31);
            this.btnGetPageLinks.TabIndex = 8;
            this.btnGetPageLinks.Text = "Get Page Links";
            this.btnGetPageLinks.UseVisualStyleBackColor = true;
            this.btnGetPageLinks.Click += new System.EventHandler(this.btnGetPageLinks_Click);
            // 
            // tbFileSaveLocation
            // 
            this.tbFileSaveLocation.Location = new System.Drawing.Point(156, 95);
            this.tbFileSaveLocation.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbFileSaveLocation.Name = "tbFileSaveLocation";
            this.tbFileSaveLocation.Size = new System.Drawing.Size(1051, 22);
            this.tbFileSaveLocation.TabIndex = 3;
            this.tbFileSaveLocation.DoubleClick += new System.EventHandler(this.tbFileSaveLocation_DoubleClick);
            // 
            // lblSaveURLsToFile
            // 
            this.lblSaveURLsToFile.AutoSize = true;
            this.lblSaveURLsToFile.Location = new System.Drawing.Point(12, 95);
            this.lblSaveURLsToFile.Name = "lblSaveURLsToFile";
            this.lblSaveURLsToFile.Size = new System.Drawing.Size(126, 17);
            this.lblSaveURLsToFile.TabIndex = 2;
            this.lblSaveURLsToFile.Text = "Save URLs To File";
            // 
            // btnRetrieveWebsites
            // 
            this.btnRetrieveWebsites.Location = new System.Drawing.Point(189, 222);
            this.btnRetrieveWebsites.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRetrieveWebsites.Name = "btnRetrieveWebsites";
            this.btnRetrieveWebsites.Size = new System.Drawing.Size(152, 31);
            this.btnRetrieveWebsites.TabIndex = 9;
            this.btnRetrieveWebsites.Text = "Retrieve Websites";
            this.btnRetrieveWebsites.UseVisualStyleBackColor = true;
            this.btnRetrieveWebsites.Click += new System.EventHandler(this.btnRetrieveWebsites_Click);
            // 
            // cmbLayers
            // 
            this.cmbLayers.FormattingEnabled = true;
            this.cmbLayers.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "15",
            "20",
            "25",
            "30"});
            this.cmbLayers.Location = new System.Drawing.Point(156, 153);
            this.cmbLayers.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbLayers.Name = "cmbLayers";
            this.cmbLayers.Size = new System.Drawing.Size(121, 24);
            this.cmbLayers.TabIndex = 5;
            this.cmbLayers.Text = "1";
            // 
            // lblLayers
            // 
            this.lblLayers.AutoSize = true;
            this.lblLayers.Location = new System.Drawing.Point(12, 153);
            this.lblLayers.Name = "lblLayers";
            this.lblLayers.Size = new System.Drawing.Size(89, 17);
            this.lblLayers.TabIndex = 4;
            this.lblLayers.Text = "Layers Deep";
            // 
            // btnHTMLToText
            // 
            this.btnHTMLToText.Location = new System.Drawing.Point(368, 222);
            this.btnHTMLToText.Margin = new System.Windows.Forms.Padding(4);
            this.btnHTMLToText.Name = "btnHTMLToText";
            this.btnHTMLToText.Size = new System.Drawing.Size(152, 31);
            this.btnHTMLToText.TabIndex = 10;
            this.btnHTMLToText.Text = "HTML Page To Text";
            this.btnHTMLToText.UseVisualStyleBackColor = true;
            this.btnHTMLToText.Click += new System.EventHandler(this.btnHTMLToText_Click);
            // 
            // cbStayInSameDomain
            // 
            this.cbStayInSameDomain.AutoSize = true;
            this.cbStayInSameDomain.Checked = true;
            this.cbStayInSameDomain.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbStayInSameDomain.Location = new System.Drawing.Point(341, 155);
            this.cbStayInSameDomain.Margin = new System.Windows.Forms.Padding(4);
            this.cbStayInSameDomain.Name = "cbStayInSameDomain";
            this.cbStayInSameDomain.Size = new System.Drawing.Size(165, 21);
            this.cbStayInSameDomain.TabIndex = 6;
            this.cbStayInSameDomain.Text = "Stay In Same Domain";
            this.cbStayInSameDomain.UseVisualStyleBackColor = true;
            // 
            // tbPDFFileLocation
            // 
            this.tbPDFFileLocation.Location = new System.Drawing.Point(164, 395);
            this.tbPDFFileLocation.Margin = new System.Windows.Forms.Padding(4);
            this.tbPDFFileLocation.Name = "tbPDFFileLocation";
            this.tbPDFFileLocation.Size = new System.Drawing.Size(995, 22);
            this.tbPDFFileLocation.TabIndex = 12;
            this.tbPDFFileLocation.DoubleClick += new System.EventHandler(this.tbPDFFileLocation_DoubleClick);
            // 
            // lblPDFFile
            // 
            this.lblPDFFile.AutoSize = true;
            this.lblPDFFile.Location = new System.Drawing.Point(8, 399);
            this.lblPDFFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPDFFile.Name = "lblPDFFile";
            this.lblPDFFile.Size = new System.Drawing.Size(61, 17);
            this.lblPDFFile.TabIndex = 11;
            this.lblPDFFile.Text = "PDF File";
            // 
            // btnPDFToText
            // 
            this.btnPDFToText.Location = new System.Drawing.Point(1008, 482);
            this.btnPDFToText.Margin = new System.Windows.Forms.Padding(4);
            this.btnPDFToText.Name = "btnPDFToText";
            this.btnPDFToText.Size = new System.Drawing.Size(152, 31);
            this.btnPDFToText.TabIndex = 15;
            this.btnPDFToText.Text = "PDF to Text";
            this.btnPDFToText.UseVisualStyleBackColor = true;
            this.btnPDFToText.Click += new System.EventHandler(this.btnPDFToText_Click);
            // 
            // cmbIncludeTextPos
            // 
            this.cmbIncludeTextPos.FormattingEnabled = true;
            this.cmbIncludeTextPos.Items.AddRange(new object[] {
            "No",
            "Yes"});
            this.cmbIncludeTextPos.Location = new System.Drawing.Point(164, 438);
            this.cmbIncludeTextPos.Margin = new System.Windows.Forms.Padding(4);
            this.cmbIncludeTextPos.Name = "cmbIncludeTextPos";
            this.cmbIncludeTextPos.Size = new System.Drawing.Size(160, 24);
            this.cmbIncludeTextPos.TabIndex = 14;
            this.cmbIncludeTextPos.Text = "Yes";
            // 
            // lblIncludeTextPositions
            // 
            this.lblIncludeTextPositions.AutoSize = true;
            this.lblIncludeTextPositions.Location = new System.Drawing.Point(8, 442);
            this.lblIncludeTextPositions.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblIncludeTextPositions.Name = "lblIncludeTextPositions";
            this.lblIncludeTextPositions.Size = new System.Drawing.Size(145, 17);
            this.lblIncludeTextPositions.TabIndex = 13;
            this.lblIncludeTextPositions.Text = "Include Text Positions";
            // 
            // ExtractWebsites
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1219, 576);
            this.Controls.Add(this.lblIncludeTextPositions);
            this.Controls.Add(this.cmbIncludeTextPos);
            this.Controls.Add(this.btnPDFToText);
            this.Controls.Add(this.lblPDFFile);
            this.Controls.Add(this.tbPDFFileLocation);
            this.Controls.Add(this.cbStayInSameDomain);
            this.Controls.Add(this.btnHTMLToText);
            this.Controls.Add(this.lblLayers);
            this.Controls.Add(this.cmbLayers);
            this.Controls.Add(this.btnRetrieveWebsites);
            this.Controls.Add(this.lblSaveURLsToFile);
            this.Controls.Add(this.tbFileSaveLocation);
            this.Controls.Add(this.btnGetPageLinks);
            this.Controls.Add(this.tbURL);
            this.Controls.Add(this.lblURL);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ExtractWebsites";
            this.Text = "Extract Website";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblURL;
        private System.Windows.Forms.TextBox tbURL;
        private System.Windows.Forms.Button btnGetPageLinks;
        private System.Windows.Forms.TextBox tbFileSaveLocation;
        private System.Windows.Forms.Label lblSaveURLsToFile;
        private System.Windows.Forms.Button btnRetrieveWebsites;
        private System.Windows.Forms.ComboBox cmbLayers;
        private System.Windows.Forms.Label lblLayers;
        private System.Windows.Forms.Button btnHTMLToText;
        private System.Windows.Forms.CheckBox cbStayInSameDomain;
        private System.Windows.Forms.TextBox tbPDFFileLocation;
        private System.Windows.Forms.Label lblPDFFile;
        private System.Windows.Forms.Button btnPDFToText;
        private System.Windows.Forms.ComboBox cmbIncludeTextPos;
        private System.Windows.Forms.Label lblIncludeTextPositions;
    }
}