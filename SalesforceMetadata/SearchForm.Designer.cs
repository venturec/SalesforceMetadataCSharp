namespace SalesforceMetadata
{
    partial class SearchForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchForm));
            this.tbSearchString = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.rtbResults = new System.Windows.Forms.RichTextBox();
            this.lblSearchText = new System.Windows.Forms.Label();
            this.tbSearchLocation = new System.Windows.Forms.TextBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.lblResults = new System.Windows.Forms.Label();
            this.cbIncludeFileName = new System.Windows.Forms.CheckBox();
            this.lblSearchFileExtension = new System.Windows.Forms.Label();
            this.tbFileExtension = new System.Windows.Forms.TextBox();
            this.btnSearchFileExtension = new System.Windows.Forms.Button();
            this.searchProgressBar = new System.Windows.Forms.ProgressBar();
            this.cmbSearchFilter = new System.Windows.Forms.ComboBox();
            this.lblSearchFilter = new System.Windows.Forms.Label();
            this.cbSearchAll = new System.Windows.Forms.CheckBox();
            this.cbMetadataFolderAndAPINameOnly = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // tbSearchString
            // 
            this.tbSearchString.Location = new System.Drawing.Point(11, 81);
            this.tbSearchString.Margin = new System.Windows.Forms.Padding(2);
            this.tbSearchString.Name = "tbSearchString";
            this.tbSearchString.Size = new System.Drawing.Size(789, 23);
            this.tbSearchString.TabIndex = 3;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(1112, 181);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(171, 28);
            this.btnSearch.TabIndex = 12;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // rtbResults
            // 
            this.rtbResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbResults.Location = new System.Drawing.Point(74, 217);
            this.rtbResults.Margin = new System.Windows.Forms.Padding(2);
            this.rtbResults.Name = "rtbResults";
            this.rtbResults.Size = new System.Drawing.Size(1346, 594);
            this.rtbResults.TabIndex = 14;
            this.rtbResults.Text = "";
            this.rtbResults.WordWrap = false;
            // 
            // lblSearchText
            // 
            this.lblSearchText.AutoSize = true;
            this.lblSearchText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchText.Location = new System.Drawing.Point(9, 62);
            this.lblSearchText.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSearchText.Name = "lblSearchText";
            this.lblSearchText.Size = new System.Drawing.Size(95, 17);
            this.lblSearchText.TabIndex = 2;
            this.lblSearchText.Text = "Search Text";
            // 
            // tbSearchLocation
            // 
            this.tbSearchLocation.Location = new System.Drawing.Point(11, 30);
            this.tbSearchLocation.Margin = new System.Windows.Forms.Padding(2);
            this.tbSearchLocation.Name = "tbSearchLocation";
            this.tbSearchLocation.Size = new System.Drawing.Size(789, 23);
            this.tbSearchLocation.TabIndex = 1;
            this.tbSearchLocation.DoubleClick += new System.EventHandler(this.tbLocation_DoubleClick);
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocation.Location = new System.Drawing.Point(9, 10);
            this.lblLocation.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(70, 17);
            this.lblLocation.TabIndex = 0;
            this.lblLocation.Text = "Location";
            // 
            // lblResults
            // 
            this.lblResults.AutoSize = true;
            this.lblResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResults.Location = new System.Drawing.Point(8, 217);
            this.lblResults.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblResults.Name = "lblResults";
            this.lblResults.Size = new System.Drawing.Size(62, 17);
            this.lblResults.TabIndex = 13;
            this.lblResults.Text = "Results";
            // 
            // cbIncludeFileName
            // 
            this.cbIncludeFileName.AutoSize = true;
            this.cbIncludeFileName.Location = new System.Drawing.Point(1112, 30);
            this.cbIncludeFileName.Name = "cbIncludeFileName";
            this.cbIncludeFileName.Size = new System.Drawing.Size(210, 21);
            this.cbIncludeFileName.TabIndex = 4;
            this.cbIncludeFileName.Text = "Include Files Being Searched";
            this.cbIncludeFileName.UseVisualStyleBackColor = true;
            // 
            // lblSearchFileExtension
            // 
            this.lblSearchFileExtension.AutoSize = true;
            this.lblSearchFileExtension.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchFileExtension.Location = new System.Drawing.Point(9, 121);
            this.lblSearchFileExtension.Name = "lblSearchFileExtension";
            this.lblSearchFileExtension.Size = new System.Drawing.Size(182, 17);
            this.lblSearchFileExtension.TabIndex = 7;
            this.lblSearchFileExtension.Text = "Search File Names Only";
            // 
            // tbFileExtension
            // 
            this.tbFileExtension.Location = new System.Drawing.Point(12, 140);
            this.tbFileExtension.Name = "tbFileExtension";
            this.tbFileExtension.Size = new System.Drawing.Size(176, 23);
            this.tbFileExtension.TabIndex = 8;
            // 
            // btnSearchFileExtension
            // 
            this.btnSearchFileExtension.Location = new System.Drawing.Point(444, 138);
            this.btnSearchFileExtension.Name = "btnSearchFileExtension";
            this.btnSearchFileExtension.Size = new System.Drawing.Size(171, 27);
            this.btnSearchFileExtension.TabIndex = 11;
            this.btnSearchFileExtension.Text = "Search By File Extension";
            this.btnSearchFileExtension.UseVisualStyleBackColor = true;
            this.btnSearchFileExtension.Click += new System.EventHandler(this.btnSearchFileExtension_Click);
            // 
            // searchProgressBar
            // 
            this.searchProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchProgressBar.Location = new System.Drawing.Point(74, 839);
            this.searchProgressBar.Name = "searchProgressBar";
            this.searchProgressBar.Size = new System.Drawing.Size(1342, 23);
            this.searchProgressBar.TabIndex = 15;
            // 
            // cmbSearchFilter
            // 
            this.cmbSearchFilter.FormattingEnabled = true;
            this.cmbSearchFilter.Items.AddRange(new object[] {
            "--none--",
            "Starts With",
            "Ends With",
            "By File Extension"});
            this.cmbSearchFilter.Location = new System.Drawing.Point(232, 140);
            this.cmbSearchFilter.Name = "cmbSearchFilter";
            this.cmbSearchFilter.Size = new System.Drawing.Size(171, 24);
            this.cmbSearchFilter.TabIndex = 10;
            this.cmbSearchFilter.Text = "--none--";
            // 
            // lblSearchFilter
            // 
            this.lblSearchFilter.AutoSize = true;
            this.lblSearchFilter.Location = new System.Drawing.Point(229, 120);
            this.lblSearchFilter.Name = "lblSearchFilter";
            this.lblSearchFilter.Size = new System.Drawing.Size(88, 17);
            this.lblSearchFilter.TabIndex = 9;
            this.lblSearchFilter.Text = "Search Filter";
            // 
            // cbSearchAll
            // 
            this.cbSearchAll.AutoSize = true;
            this.cbSearchAll.Location = new System.Drawing.Point(1112, 57);
            this.cbSearchAll.Name = "cbSearchAll";
            this.cbSearchAll.Size = new System.Drawing.Size(249, 21);
            this.cbSearchAll.TabIndex = 5;
            this.cbSearchAll.Text = "Search All (Even Excluded Folders)";
            this.cbSearchAll.UseVisualStyleBackColor = true;
            // 
            // cbMetadataFolderAndAPINameOnly
            // 
            this.cbMetadataFolderAndAPINameOnly.AutoSize = true;
            this.cbMetadataFolderAndAPINameOnly.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMetadataFolderAndAPINameOnly.Location = new System.Drawing.Point(1112, 84);
            this.cbMetadataFolderAndAPINameOnly.Name = "cbMetadataFolderAndAPINameOnly";
            this.cbMetadataFolderAndAPINameOnly.Size = new System.Drawing.Size(304, 21);
            this.cbMetadataFolderAndAPINameOnly.TabIndex = 6;
            this.cbMetadataFolderAndAPINameOnly.Text = "Return Metadata Folder and API Name Only";
            this.cbMetadataFolderAndAPINameOnly.UseVisualStyleBackColor = true;
            // 
            // SearchForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1428, 869);
            this.Controls.Add(this.cbMetadataFolderAndAPINameOnly);
            this.Controls.Add(this.cbSearchAll);
            this.Controls.Add(this.lblSearchFilter);
            this.Controls.Add(this.cmbSearchFilter);
            this.Controls.Add(this.searchProgressBar);
            this.Controls.Add(this.btnSearchFileExtension);
            this.Controls.Add(this.tbFileExtension);
            this.Controls.Add(this.lblSearchFileExtension);
            this.Controls.Add(this.cbIncludeFileName);
            this.Controls.Add(this.lblResults);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.tbSearchLocation);
            this.Controls.Add(this.lblSearchText);
            this.Controls.Add(this.rtbResults);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.tbSearchString);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "SearchForm";
            this.Text = "Search Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbSearchString;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.RichTextBox rtbResults;
        private System.Windows.Forms.Label lblSearchText;
        public System.Windows.Forms.TextBox tbSearchLocation;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Label lblResults;
        private System.Windows.Forms.CheckBox cbIncludeFileName;
        private System.Windows.Forms.Label lblSearchFileExtension;
        private System.Windows.Forms.TextBox tbFileExtension;
        private System.Windows.Forms.Button btnSearchFileExtension;
        private System.Windows.Forms.ProgressBar searchProgressBar;
        private System.Windows.Forms.ComboBox cmbSearchFilter;
        private System.Windows.Forms.Label lblSearchFilter;
        private System.Windows.Forms.CheckBox cbSearchAll;
        private System.Windows.Forms.CheckBox cbMetadataFolderAndAPINameOnly;
    }
}