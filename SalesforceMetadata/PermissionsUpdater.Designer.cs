namespace SalesforceMetadata
{
    partial class PermissionsUpdater
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PermissionsUpdater));
            this.tbSelectFolder = new System.Windows.Forms.TextBox();
            this.lblSelectFolder = new System.Windows.Forms.Label();
            this.lblSaveChangesTo = new System.Windows.Forms.Label();
            this.tbSaveChangesTo = new System.Windows.Forms.TextBox();
            this.tvPermissions = new System.Windows.Forms.TreeView();
            this.btnConsolidatePermissions = new System.Windows.Forms.Button();
            this.lblDoNotOverride = new System.Windows.Forms.Label();
            this.tbDoNotOverride = new System.Windows.Forms.TextBox();
            this.btnPopulateTreeView = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbSelectFolder
            // 
            this.tbSelectFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSelectFolder.Location = new System.Drawing.Point(181, 24);
            this.tbSelectFolder.Name = "tbSelectFolder";
            this.tbSelectFolder.Size = new System.Drawing.Size(877, 23);
            this.tbSelectFolder.TabIndex = 1;
            this.tbSelectFolder.DoubleClick += new System.EventHandler(this.tbSelectFolder_DoubleClick);
            // 
            // lblSelectFolder
            // 
            this.lblSelectFolder.AutoSize = true;
            this.lblSelectFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectFolder.Location = new System.Drawing.Point(14, 27);
            this.lblSelectFolder.Name = "lblSelectFolder";
            this.lblSelectFolder.Size = new System.Drawing.Size(104, 17);
            this.lblSelectFolder.TabIndex = 0;
            this.lblSelectFolder.Text = "Select Folder";
            // 
            // lblSaveChangesTo
            // 
            this.lblSaveChangesTo.AutoSize = true;
            this.lblSaveChangesTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSaveChangesTo.Location = new System.Drawing.Point(14, 64);
            this.lblSaveChangesTo.Name = "lblSaveChangesTo";
            this.lblSaveChangesTo.Size = new System.Drawing.Size(136, 17);
            this.lblSaveChangesTo.TabIndex = 2;
            this.lblSaveChangesTo.Text = "Save Changes To";
            // 
            // tbSaveChangesTo
            // 
            this.tbSaveChangesTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSaveChangesTo.Location = new System.Drawing.Point(181, 61);
            this.tbSaveChangesTo.Name = "tbSaveChangesTo";
            this.tbSaveChangesTo.Size = new System.Drawing.Size(877, 23);
            this.tbSaveChangesTo.TabIndex = 3;
            this.tbSaveChangesTo.DoubleClick += new System.EventHandler(this.tbSaveChangesTo_DoubleClick);
            // 
            // tvPermissions
            // 
            this.tvPermissions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvPermissions.CheckBoxes = true;
            this.tvPermissions.Location = new System.Drawing.Point(12, 202);
            this.tvPermissions.Name = "tvPermissions";
            this.tvPermissions.Size = new System.Drawing.Size(1473, 546);
            this.tvPermissions.TabIndex = 4;
            // 
            // btnConsolidatePermissions
            // 
            this.btnConsolidatePermissions.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConsolidatePermissions.Location = new System.Drawing.Point(1261, 24);
            this.btnConsolidatePermissions.Name = "btnConsolidatePermissions";
            this.btnConsolidatePermissions.Size = new System.Drawing.Size(224, 32);
            this.btnConsolidatePermissions.TabIndex = 5;
            this.btnConsolidatePermissions.Text = "Consolidate Permissions";
            this.btnConsolidatePermissions.UseVisualStyleBackColor = true;
            this.btnConsolidatePermissions.Click += new System.EventHandler(this.btnConsolidatePermissions_Click);
            // 
            // lblDoNotOverride
            // 
            this.lblDoNotOverride.AutoSize = true;
            this.lblDoNotOverride.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDoNotOverride.Location = new System.Drawing.Point(14, 101);
            this.lblDoNotOverride.Name = "lblDoNotOverride";
            this.lblDoNotOverride.Size = new System.Drawing.Size(126, 17);
            this.lblDoNotOverride.TabIndex = 6;
            this.lblDoNotOverride.Text = "Do Not Override";
            // 
            // tbDoNotOverride
            // 
            this.tbDoNotOverride.Location = new System.Drawing.Point(181, 101);
            this.tbDoNotOverride.Name = "tbDoNotOverride";
            this.tbDoNotOverride.Size = new System.Drawing.Size(877, 20);
            this.tbDoNotOverride.TabIndex = 7;
            // 
            // btnPopulateTreeView
            // 
            this.btnPopulateTreeView.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPopulateTreeView.Location = new System.Drawing.Point(12, 165);
            this.btnPopulateTreeView.Name = "btnPopulateTreeView";
            this.btnPopulateTreeView.Size = new System.Drawing.Size(183, 31);
            this.btnPopulateTreeView.TabIndex = 8;
            this.btnPopulateTreeView.Text = "Populate Tree View";
            this.btnPopulateTreeView.UseVisualStyleBackColor = true;
            this.btnPopulateTreeView.Click += new System.EventHandler(this.btnPopulateTreeView_Click);
            // 
            // PermissionsUpdater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1497, 780);
            this.Controls.Add(this.btnPopulateTreeView);
            this.Controls.Add(this.tbDoNotOverride);
            this.Controls.Add(this.lblDoNotOverride);
            this.Controls.Add(this.btnConsolidatePermissions);
            this.Controls.Add(this.tvPermissions);
            this.Controls.Add(this.tbSaveChangesTo);
            this.Controls.Add(this.lblSaveChangesTo);
            this.Controls.Add(this.lblSelectFolder);
            this.Controls.Add(this.tbSelectFolder);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PermissionsUpdater";
            this.Text = "Permissions Updater";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbSelectFolder;
        private System.Windows.Forms.Label lblSelectFolder;
        private System.Windows.Forms.Label lblSaveChangesTo;
        private System.Windows.Forms.TextBox tbSaveChangesTo;
        private System.Windows.Forms.TreeView tvPermissions;
        private System.Windows.Forms.Button btnConsolidatePermissions;
        private System.Windows.Forms.Label lblDoNotOverride;
        private System.Windows.Forms.TextBox tbDoNotOverride;
        private System.Windows.Forms.Button btnPopulateTreeView;
    }
}