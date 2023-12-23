namespace SalesforceMetadata
{
    partial class ObjectModification
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ObjectModification));
            this.tbObjectFolderLocation = new System.Windows.Forms.TextBox();
            this.lblObjectFolder = new System.Windows.Forms.Label();
            this.lbFileNames = new System.Windows.Forms.ListBox();
            this.btnRemovePkgNameFromFile = new System.Windows.Forms.Button();
            this.btnRemovePkgNameFromFields = new System.Windows.Forms.Button();
            this.tbPkgName = new System.Windows.Forms.TextBox();
            this.lblPkgName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbObjectFolderLocation
            // 
            this.tbObjectFolderLocation.Location = new System.Drawing.Point(12, 38);
            this.tbObjectFolderLocation.Name = "tbObjectFolderLocation";
            this.tbObjectFolderLocation.Size = new System.Drawing.Size(704, 20);
            this.tbObjectFolderLocation.TabIndex = 0;
            this.tbObjectFolderLocation.DoubleClick += new System.EventHandler(this.tbObjectFolderLocation_DoubleClick);
            // 
            // lblObjectFolder
            // 
            this.lblObjectFolder.AutoSize = true;
            this.lblObjectFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblObjectFolder.Location = new System.Drawing.Point(12, 13);
            this.lblObjectFolder.Name = "lblObjectFolder";
            this.lblObjectFolder.Size = new System.Drawing.Size(106, 17);
            this.lblObjectFolder.TabIndex = 1;
            this.lblObjectFolder.Text = "Object Folder";
            // 
            // lbFileNames
            // 
            this.lbFileNames.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbFileNames.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFileNames.FormattingEnabled = true;
            this.lbFileNames.ItemHeight = 16;
            this.lbFileNames.Location = new System.Drawing.Point(12, 64);
            this.lbFileNames.Name = "lbFileNames";
            this.lbFileNames.Size = new System.Drawing.Size(704, 532);
            this.lbFileNames.TabIndex = 2;
            // 
            // btnRemovePkgNameFromFile
            // 
            this.btnRemovePkgNameFromFile.Location = new System.Drawing.Point(747, 94);
            this.btnRemovePkgNameFromFile.Name = "btnRemovePkgNameFromFile";
            this.btnRemovePkgNameFromFile.Size = new System.Drawing.Size(184, 24);
            this.btnRemovePkgNameFromFile.TabIndex = 3;
            this.btnRemovePkgNameFromFile.Text = "Remove Pkg Name from Files";
            this.btnRemovePkgNameFromFile.UseVisualStyleBackColor = true;
            this.btnRemovePkgNameFromFile.Click += new System.EventHandler(this.btnRemovePkgNameFromFile_Click);
            // 
            // btnRemovePkgNameFromFields
            // 
            this.btnRemovePkgNameFromFields.Location = new System.Drawing.Point(747, 62);
            this.btnRemovePkgNameFromFields.Name = "btnRemovePkgNameFromFields";
            this.btnRemovePkgNameFromFields.Size = new System.Drawing.Size(184, 24);
            this.btnRemovePkgNameFromFields.TabIndex = 4;
            this.btnRemovePkgNameFromFields.Text = "Remove Pkg Name From Fields";
            this.btnRemovePkgNameFromFields.UseVisualStyleBackColor = true;
            this.btnRemovePkgNameFromFields.Click += new System.EventHandler(this.btnRemovePkgNameFromFields_Click);
            // 
            // tbPkgName
            // 
            this.tbPkgName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tbPkgName.Location = new System.Drawing.Point(747, 37);
            this.tbPkgName.Name = "tbPkgName";
            this.tbPkgName.Size = new System.Drawing.Size(100, 20);
            this.tbPkgName.TabIndex = 5;
            // 
            // lblPkgName
            // 
            this.lblPkgName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPkgName.AutoSize = true;
            this.lblPkgName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPkgName.Location = new System.Drawing.Point(745, 9);
            this.lblPkgName.Name = "lblPkgName";
            this.lblPkgName.Size = new System.Drawing.Size(146, 17);
            this.lblPkgName.TabIndex = 6;
            this.lblPkgName.Text = "Pkg Name To  Find";
            // 
            // ObjectModification
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(947, 610);
            this.Controls.Add(this.lblPkgName);
            this.Controls.Add(this.tbPkgName);
            this.Controls.Add(this.btnRemovePkgNameFromFields);
            this.Controls.Add(this.btnRemovePkgNameFromFile);
            this.Controls.Add(this.lbFileNames);
            this.Controls.Add(this.lblObjectFolder);
            this.Controls.Add(this.tbObjectFolderLocation);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ObjectModification";
            this.Text = "ObjectModification";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbObjectFolderLocation;
        private System.Windows.Forms.Label lblObjectFolder;
        private System.Windows.Forms.ListBox lbFileNames;
        private System.Windows.Forms.Button btnRemovePkgNameFromFile;
        private System.Windows.Forms.Button btnRemovePkgNameFromFields;
        private System.Windows.Forms.TextBox tbPkgName;
        private System.Windows.Forms.Label lblPkgName;
    }
}