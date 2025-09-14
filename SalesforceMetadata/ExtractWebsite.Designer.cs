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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtractWebsites));
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
            this.btnPDFBookmarks = new System.Windows.Forms.Button();
            this.lblPDFFolder = new System.Windows.Forms.Label();
            this.tbPDFFolder = new System.Windows.Forms.TextBox();
            this.cbSplitFiles = new System.Windows.Forms.CheckBox();
            this.tbSaveTextFilesTo = new System.Windows.Forms.TextBox();
            this.lblSaveTextFilesTo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblURL
            // 
            this.lblURL.AutoSize = true;
            this.lblURL.Location = new System.Drawing.Point(9, 45);
            this.lblURL.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(29, 13);
            this.lblURL.TabIndex = 0;
            this.lblURL.Text = "URL";
            // 
            // tbURL
            // 
            this.tbURL.Location = new System.Drawing.Point(48, 45);
            this.tbURL.Margin = new System.Windows.Forms.Padding(2);
            this.tbURL.Name = "tbURL";
            this.tbURL.Size = new System.Drawing.Size(858, 20);
            this.tbURL.TabIndex = 1;
            // 
            // btnGetPageLinks
            // 
            this.btnGetPageLinks.Location = new System.Drawing.Point(9, 180);
            this.btnGetPageLinks.Margin = new System.Windows.Forms.Padding(2);
            this.btnGetPageLinks.Name = "btnGetPageLinks";
            this.btnGetPageLinks.Size = new System.Drawing.Size(116, 25);
            this.btnGetPageLinks.TabIndex = 7;
            this.btnGetPageLinks.Text = "Get Page Links";
            this.btnGetPageLinks.UseVisualStyleBackColor = true;
            this.btnGetPageLinks.Click += new System.EventHandler(this.btnGetPageLinks_Click);
            // 
            // tbFileSaveLocation
            // 
            this.tbFileSaveLocation.Location = new System.Drawing.Point(117, 77);
            this.tbFileSaveLocation.Margin = new System.Windows.Forms.Padding(2);
            this.tbFileSaveLocation.Name = "tbFileSaveLocation";
            this.tbFileSaveLocation.Size = new System.Drawing.Size(789, 20);
            this.tbFileSaveLocation.TabIndex = 3;
            this.tbFileSaveLocation.DoubleClick += new System.EventHandler(this.tbFileSaveLocation_DoubleClick);
            // 
            // lblSaveURLsToFile
            // 
            this.lblSaveURLsToFile.AutoSize = true;
            this.lblSaveURLsToFile.Location = new System.Drawing.Point(9, 77);
            this.lblSaveURLsToFile.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSaveURLsToFile.Name = "lblSaveURLsToFile";
            this.lblSaveURLsToFile.Size = new System.Drawing.Size(97, 13);
            this.lblSaveURLsToFile.TabIndex = 2;
            this.lblSaveURLsToFile.Text = "Save URLs To File";
            // 
            // btnRetrieveWebsites
            // 
            this.btnRetrieveWebsites.Location = new System.Drawing.Point(142, 180);
            this.btnRetrieveWebsites.Margin = new System.Windows.Forms.Padding(2);
            this.btnRetrieveWebsites.Name = "btnRetrieveWebsites";
            this.btnRetrieveWebsites.Size = new System.Drawing.Size(114, 25);
            this.btnRetrieveWebsites.TabIndex = 8;
            this.btnRetrieveWebsites.Text = "Retrieve Websites";
            this.btnRetrieveWebsites.UseVisualStyleBackColor = true;
            this.btnRetrieveWebsites.Click += new System.EventHandler(this.btnRetrieveWebsites_Click);
            // 
            // cmbLayers
            // 
            this.cmbLayers.FormattingEnabled = true;
            this.cmbLayers.Items.AddRange(new object[] {
            "0",
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
            this.cmbLayers.Location = new System.Drawing.Point(117, 124);
            this.cmbLayers.Margin = new System.Windows.Forms.Padding(2);
            this.cmbLayers.Name = "cmbLayers";
            this.cmbLayers.Size = new System.Drawing.Size(92, 21);
            this.cmbLayers.TabIndex = 5;
            this.cmbLayers.Text = "0";
            // 
            // lblLayers
            // 
            this.lblLayers.AutoSize = true;
            this.lblLayers.Location = new System.Drawing.Point(9, 124);
            this.lblLayers.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLayers.Name = "lblLayers";
            this.lblLayers.Size = new System.Drawing.Size(67, 13);
            this.lblLayers.TabIndex = 4;
            this.lblLayers.Text = "Layers Deep";
            // 
            // btnHTMLToText
            // 
            this.btnHTMLToText.Enabled = false;
            this.btnHTMLToText.Location = new System.Drawing.Point(276, 180);
            this.btnHTMLToText.Name = "btnHTMLToText";
            this.btnHTMLToText.Size = new System.Drawing.Size(114, 25);
            this.btnHTMLToText.TabIndex = 9;
            this.btnHTMLToText.Text = "HTML Page To Text";
            this.btnHTMLToText.UseVisualStyleBackColor = true;
            this.btnHTMLToText.Click += new System.EventHandler(this.btnHTMLToText_Click);
            // 
            // cbStayInSameDomain
            // 
            this.cbStayInSameDomain.AutoSize = true;
            this.cbStayInSameDomain.Checked = true;
            this.cbStayInSameDomain.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbStayInSameDomain.Location = new System.Drawing.Point(256, 126);
            this.cbStayInSameDomain.Name = "cbStayInSameDomain";
            this.cbStayInSameDomain.Size = new System.Drawing.Size(128, 17);
            this.cbStayInSameDomain.TabIndex = 6;
            this.cbStayInSameDomain.Text = "Stay In Same Domain";
            this.cbStayInSameDomain.UseVisualStyleBackColor = true;
            // 
            // tbPDFFileLocation
            // 
            this.tbPDFFileLocation.Location = new System.Drawing.Point(123, 321);
            this.tbPDFFileLocation.Name = "tbPDFFileLocation";
            this.tbPDFFileLocation.Size = new System.Drawing.Size(747, 20);
            this.tbPDFFileLocation.TabIndex = 15;
            this.tbPDFFileLocation.DoubleClick += new System.EventHandler(this.tbPDFFileLocation_DoubleClick);
            // 
            // lblPDFFile
            // 
            this.lblPDFFile.AutoSize = true;
            this.lblPDFFile.Location = new System.Drawing.Point(6, 324);
            this.lblPDFFile.Name = "lblPDFFile";
            this.lblPDFFile.Size = new System.Drawing.Size(47, 13);
            this.lblPDFFile.TabIndex = 14;
            this.lblPDFFile.Text = "PDF File";
            // 
            // btnPDFToText
            // 
            this.btnPDFToText.Location = new System.Drawing.Point(117, 431);
            this.btnPDFToText.Name = "btnPDFToText";
            this.btnPDFToText.Size = new System.Drawing.Size(114, 25);
            this.btnPDFToText.TabIndex = 19;
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
            this.cmbIncludeTextPos.Location = new System.Drawing.Point(123, 351);
            this.cmbIncludeTextPos.Name = "cmbIncludeTextPos";
            this.cmbIncludeTextPos.Size = new System.Drawing.Size(121, 21);
            this.cmbIncludeTextPos.TabIndex = 17;
            this.cmbIncludeTextPos.Text = "Yes";
            // 
            // lblIncludeTextPositions
            // 
            this.lblIncludeTextPositions.AutoSize = true;
            this.lblIncludeTextPositions.Location = new System.Drawing.Point(6, 354);
            this.lblIncludeTextPositions.Name = "lblIncludeTextPositions";
            this.lblIncludeTextPositions.Size = new System.Drawing.Size(111, 13);
            this.lblIncludeTextPositions.TabIndex = 16;
            this.lblIncludeTextPositions.Text = "Include Text Positions";
            // 
            // btnPDFBookmarks
            // 
            this.btnPDFBookmarks.Location = new System.Drawing.Point(256, 431);
            this.btnPDFBookmarks.Name = "btnPDFBookmarks";
            this.btnPDFBookmarks.Size = new System.Drawing.Size(114, 23);
            this.btnPDFBookmarks.TabIndex = 20;
            this.btnPDFBookmarks.Text = "Get PDF Bookmarks";
            this.btnPDFBookmarks.UseVisualStyleBackColor = true;
            this.btnPDFBookmarks.Click += new System.EventHandler(this.btnPDFBookmarks_Click);
            // 
            // lblPDFFolder
            // 
            this.lblPDFFolder.AutoSize = true;
            this.lblPDFFolder.Location = new System.Drawing.Point(6, 297);
            this.lblPDFFolder.Name = "lblPDFFolder";
            this.lblPDFFolder.Size = new System.Drawing.Size(60, 13);
            this.lblPDFFolder.TabIndex = 12;
            this.lblPDFFolder.Text = "PDF Folder";
            // 
            // tbPDFFolder
            // 
            this.tbPDFFolder.Location = new System.Drawing.Point(123, 294);
            this.tbPDFFolder.Name = "tbPDFFolder";
            this.tbPDFFolder.Size = new System.Drawing.Size(747, 20);
            this.tbPDFFolder.TabIndex = 13;
            this.tbPDFFolder.DoubleClick += new System.EventHandler(this.tbPDFFolder_DoubleClick);
            // 
            // cbSplitFiles
            // 
            this.cbSplitFiles.AutoSize = true;
            this.cbSplitFiles.Location = new System.Drawing.Point(123, 388);
            this.cbSplitFiles.Name = "cbSplitFiles";
            this.cbSplitFiles.Size = new System.Drawing.Size(70, 17);
            this.cbSplitFiles.TabIndex = 18;
            this.cbSplitFiles.Text = "Split Files";
            this.cbSplitFiles.UseVisualStyleBackColor = true;
            // 
            // tbSaveTextFilesTo
            // 
            this.tbSaveTextFilesTo.Location = new System.Drawing.Point(123, 268);
            this.tbSaveTextFilesTo.Name = "tbSaveTextFilesTo";
            this.tbSaveTextFilesTo.Size = new System.Drawing.Size(747, 20);
            this.tbSaveTextFilesTo.TabIndex = 11;
            this.tbSaveTextFilesTo.DoubleClick += new System.EventHandler(this.tbSaveTextFilesTo_DoubleClick);
            // 
            // lblSaveTextFilesTo
            // 
            this.lblSaveTextFilesTo.AutoSize = true;
            this.lblSaveTextFilesTo.Location = new System.Drawing.Point(6, 271);
            this.lblSaveTextFilesTo.Name = "lblSaveTextFilesTo";
            this.lblSaveTextFilesTo.Size = new System.Drawing.Size(96, 13);
            this.lblSaveTextFilesTo.TabIndex = 10;
            this.lblSaveTextFilesTo.Text = "Save Text Files To";
            // 
            // ExtractWebsites
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(914, 468);
            this.Controls.Add(this.lblSaveTextFilesTo);
            this.Controls.Add(this.tbSaveTextFilesTo);
            this.Controls.Add(this.cbSplitFiles);
            this.Controls.Add(this.tbPDFFolder);
            this.Controls.Add(this.lblPDFFolder);
            this.Controls.Add(this.btnPDFBookmarks);
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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
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
        private System.Windows.Forms.Button btnPDFBookmarks;
        private System.Windows.Forms.Label lblPDFFolder;
        private System.Windows.Forms.TextBox tbPDFFolder;
        private System.Windows.Forms.CheckBox cbSplitFiles;
        private System.Windows.Forms.TextBox tbSaveTextFilesTo;
        private System.Windows.Forms.Label lblSaveTextFilesTo;
    }
}