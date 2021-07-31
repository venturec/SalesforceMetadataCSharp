namespace SalesforceMetadata
{
    partial class frmSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSearch));
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
            this.SuspendLayout();
            // 
            // tbSearchString
            // 
            this.tbSearchString.Location = new System.Drawing.Point(12, 78);
            this.tbSearchString.Margin = new System.Windows.Forms.Padding(2);
            this.tbSearchString.Name = "tbSearchString";
            this.tbSearchString.Size = new System.Drawing.Size(558, 20);
            this.tbSearchString.TabIndex = 4;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(847, 76);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(135, 23);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // rtbResults
            // 
            this.rtbResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbResults.Location = new System.Drawing.Point(74, 190);
            this.rtbResults.Margin = new System.Windows.Forms.Padding(2);
            this.rtbResults.Name = "rtbResults";
            this.rtbResults.Size = new System.Drawing.Size(1096, 571);
            this.rtbResults.TabIndex = 13;
            this.rtbResults.Text = "";
            // 
            // lblSearchText
            // 
            this.lblSearchText.AutoSize = true;
            this.lblSearchText.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchText.Location = new System.Drawing.Point(9, 62);
            this.lblSearchText.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSearchText.Name = "lblSearchText";
            this.lblSearchText.Size = new System.Drawing.Size(76, 13);
            this.lblSearchText.TabIndex = 3;
            this.lblSearchText.Text = "Search Text";
            // 
            // tbSearchLocation
            // 
            this.tbSearchLocation.Location = new System.Drawing.Point(11, 30);
            this.tbSearchLocation.Margin = new System.Windows.Forms.Padding(2);
            this.tbSearchLocation.Name = "tbSearchLocation";
            this.tbSearchLocation.Size = new System.Drawing.Size(555, 20);
            this.tbSearchLocation.TabIndex = 1;
            this.tbSearchLocation.DoubleClick += new System.EventHandler(this.tbLocation_DoubleClick);
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocation.Location = new System.Drawing.Point(9, 14);
            this.lblLocation.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(56, 13);
            this.lblLocation.TabIndex = 0;
            this.lblLocation.Text = "Location";
            // 
            // lblResults
            // 
            this.lblResults.AutoSize = true;
            this.lblResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResults.Location = new System.Drawing.Point(6, 190);
            this.lblResults.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblResults.Name = "lblResults";
            this.lblResults.Size = new System.Drawing.Size(49, 13);
            this.lblResults.TabIndex = 12;
            this.lblResults.Text = "Results";
            // 
            // cbIncludeFileName
            // 
            this.cbIncludeFileName.AutoSize = true;
            this.cbIncludeFileName.Location = new System.Drawing.Point(998, 32);
            this.cbIncludeFileName.Name = "cbIncludeFileName";
            this.cbIncludeFileName.Size = new System.Drawing.Size(164, 17);
            this.cbIncludeFileName.TabIndex = 2;
            this.cbIncludeFileName.Text = "Include Files Being Searched";
            this.cbIncludeFileName.UseVisualStyleBackColor = true;
            // 
            // lblSearchFileExtension
            // 
            this.lblSearchFileExtension.AutoSize = true;
            this.lblSearchFileExtension.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchFileExtension.Location = new System.Drawing.Point(9, 109);
            this.lblSearchFileExtension.Name = "lblSearchFileExtension";
            this.lblSearchFileExtension.Size = new System.Drawing.Size(142, 13);
            this.lblSearchFileExtension.TabIndex = 7;
            this.lblSearchFileExtension.Text = "Search File Names Only";
            // 
            // tbFileExtension
            // 
            this.tbFileExtension.Location = new System.Drawing.Point(9, 128);
            this.tbFileExtension.Name = "tbFileExtension";
            this.tbFileExtension.Size = new System.Drawing.Size(152, 20);
            this.tbFileExtension.TabIndex = 8;
            // 
            // btnSearchFileExtension
            // 
            this.btnSearchFileExtension.Location = new System.Drawing.Point(847, 125);
            this.btnSearchFileExtension.Name = "btnSearchFileExtension";
            this.btnSearchFileExtension.Size = new System.Drawing.Size(135, 23);
            this.btnSearchFileExtension.TabIndex = 11;
            this.btnSearchFileExtension.Text = "Search By File Extension";
            this.btnSearchFileExtension.UseVisualStyleBackColor = true;
            this.btnSearchFileExtension.Click += new System.EventHandler(this.btnSearchFileExtension_Click);
            // 
            // searchProgressBar
            // 
            this.searchProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchProgressBar.Location = new System.Drawing.Point(74, 789);
            this.searchProgressBar.Name = "searchProgressBar";
            this.searchProgressBar.Size = new System.Drawing.Size(1092, 23);
            this.searchProgressBar.TabIndex = 14;
            // 
            // cmbSearchFilter
            // 
            this.cmbSearchFilter.FormattingEnabled = true;
            this.cmbSearchFilter.Items.AddRange(new object[] {
            "--none--",
            "Starts With",
            "Ends With",
            "By File Extension"});
            this.cmbSearchFilter.Location = new System.Drawing.Point(191, 128);
            this.cmbSearchFilter.Name = "cmbSearchFilter";
            this.cmbSearchFilter.Size = new System.Drawing.Size(171, 21);
            this.cmbSearchFilter.TabIndex = 10;
            this.cmbSearchFilter.Text = "--none--";
            // 
            // lblSearchFilter
            // 
            this.lblSearchFilter.AutoSize = true;
            this.lblSearchFilter.Location = new System.Drawing.Point(191, 109);
            this.lblSearchFilter.Name = "lblSearchFilter";
            this.lblSearchFilter.Size = new System.Drawing.Size(66, 13);
            this.lblSearchFilter.TabIndex = 9;
            this.lblSearchFilter.Text = "Search Filter";
            // 
            // cbSearchAll
            // 
            this.cbSearchAll.AutoSize = true;
            this.cbSearchAll.Location = new System.Drawing.Point(998, 76);
            this.cbSearchAll.Name = "cbSearchAll";
            this.cbSearchAll.Size = new System.Drawing.Size(74, 17);
            this.cbSearchAll.TabIndex = 6;
            this.cbSearchAll.Text = "Search All";
            this.cbSearchAll.UseVisualStyleBackColor = true;
            // 
            // frmSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1178, 819);
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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmSearch";
            this.Text = "Search";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbSearchString;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.RichTextBox rtbResults;
        private System.Windows.Forms.Label lblSearchText;
        private System.Windows.Forms.TextBox tbSearchLocation;
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
    }
}