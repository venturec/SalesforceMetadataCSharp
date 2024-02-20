namespace SalesforceMetadata
{
    partial class AddObject
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("Add CSS File");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("Add SVG Icon");
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("Add LWC Test");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddObject));
            this.tbClassName = new System.Windows.Forms.TextBox();
            this.lblClassName = new System.Windows.Forms.Label();
            this.cbWithSharing = new System.Windows.Forms.CheckBox();
            this.lblImplements = new System.Windows.Forms.Label();
            this.tbImplements = new System.Windows.Forms.TextBox();
            this.lblExtends = new System.Windows.Forms.Label();
            this.tbExtends = new System.Windows.Forms.TextBox();
            this.btnSaveClass = new System.Windows.Forms.Button();
            this.btnSaveAndAddClass = new System.Windows.Forms.Button();
            this.lblClassSectionHeader = new System.Windows.Forms.Label();
            this.lblApexTriggers = new System.Windows.Forms.Label();
            this.tbTriggerName = new System.Windows.Forms.TextBox();
            this.lblTriggerName = new System.Windows.Forms.Label();
            this.lblSobject = new System.Windows.Forms.Label();
            this.cmbSobject = new System.Windows.Forms.ComboBox();
            this.btnSaveTrigger = new System.Windows.Forms.Button();
            this.btnSaveAndAddTrig = new System.Windows.Forms.Button();
            this.lblLightningWebComponent = new System.Windows.Forms.Label();
            this.lwcName = new System.Windows.Forms.Label();
            this.tbLWCName = new System.Windows.Forms.TextBox();
            this.btnSaveLWC = new System.Windows.Forms.Button();
            this.lvTargets = new System.Windows.Forms.ListView();
            this.lvAdditionalLWCFiles = new System.Windows.Forms.ListView();
            this.lblAdditionalLWCFiles = new System.Windows.Forms.Label();
            this.lblTargets = new System.Windows.Forms.Label();
            this.lblMasterLabel = new System.Windows.Forms.Label();
            this.tbMasterLabel = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbClassName
            // 
            this.tbClassName.Location = new System.Drawing.Point(88, 40);
            this.tbClassName.Name = "tbClassName";
            this.tbClassName.Size = new System.Drawing.Size(608, 20);
            this.tbClassName.TabIndex = 2;
            // 
            // lblClassName
            // 
            this.lblClassName.AutoSize = true;
            this.lblClassName.Location = new System.Drawing.Point(8, 44);
            this.lblClassName.Name = "lblClassName";
            this.lblClassName.Size = new System.Drawing.Size(63, 13);
            this.lblClassName.TabIndex = 1;
            this.lblClassName.Text = "Class Name";
            // 
            // cbWithSharing
            // 
            this.cbWithSharing.AutoSize = true;
            this.cbWithSharing.Location = new System.Drawing.Point(744, 42);
            this.cbWithSharing.Name = "cbWithSharing";
            this.cbWithSharing.Size = new System.Drawing.Size(87, 17);
            this.cbWithSharing.TabIndex = 3;
            this.cbWithSharing.Text = "With Sharing";
            this.cbWithSharing.UseVisualStyleBackColor = true;
            // 
            // lblImplements
            // 
            this.lblImplements.AutoSize = true;
            this.lblImplements.Location = new System.Drawing.Point(8, 72);
            this.lblImplements.Name = "lblImplements";
            this.lblImplements.Size = new System.Drawing.Size(60, 13);
            this.lblImplements.TabIndex = 4;
            this.lblImplements.Text = "Implements";
            // 
            // tbImplements
            // 
            this.tbImplements.Location = new System.Drawing.Point(88, 70);
            this.tbImplements.Name = "tbImplements";
            this.tbImplements.Size = new System.Drawing.Size(608, 20);
            this.tbImplements.TabIndex = 5;
            // 
            // lblExtends
            // 
            this.lblExtends.AutoSize = true;
            this.lblExtends.Location = new System.Drawing.Point(8, 104);
            this.lblExtends.Name = "lblExtends";
            this.lblExtends.Size = new System.Drawing.Size(45, 13);
            this.lblExtends.TabIndex = 6;
            this.lblExtends.Text = "Extends";
            // 
            // tbExtends
            // 
            this.tbExtends.Location = new System.Drawing.Point(88, 100);
            this.tbExtends.Name = "tbExtends";
            this.tbExtends.Size = new System.Drawing.Size(608, 20);
            this.tbExtends.TabIndex = 7;
            // 
            // btnSaveClass
            // 
            this.btnSaveClass.Location = new System.Drawing.Point(428, 132);
            this.btnSaveClass.Name = "btnSaveClass";
            this.btnSaveClass.Size = new System.Drawing.Size(103, 23);
            this.btnSaveClass.TabIndex = 8;
            this.btnSaveClass.Text = "Save Class";
            this.btnSaveClass.UseVisualStyleBackColor = true;
            this.btnSaveClass.Click += new System.EventHandler(this.btnAddClass_Click);
            // 
            // btnSaveAndAddClass
            // 
            this.btnSaveAndAddClass.Location = new System.Drawing.Point(544, 132);
            this.btnSaveAndAddClass.Name = "btnSaveAndAddClass";
            this.btnSaveAndAddClass.Size = new System.Drawing.Size(152, 23);
            this.btnSaveAndAddClass.TabIndex = 9;
            this.btnSaveAndAddClass.Text = "Save and Add Class";
            this.btnSaveAndAddClass.UseVisualStyleBackColor = true;
            // 
            // lblClassSectionHeader
            // 
            this.lblClassSectionHeader.AutoSize = true;
            this.lblClassSectionHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClassSectionHeader.Location = new System.Drawing.Point(4, 8);
            this.lblClassSectionHeader.Name = "lblClassSectionHeader";
            this.lblClassSectionHeader.Size = new System.Drawing.Size(104, 17);
            this.lblClassSectionHeader.TabIndex = 0;
            this.lblClassSectionHeader.Text = "Apex Classes";
            // 
            // lblApexTriggers
            // 
            this.lblApexTriggers.AutoSize = true;
            this.lblApexTriggers.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApexTriggers.Location = new System.Drawing.Point(4, 208);
            this.lblApexTriggers.Name = "lblApexTriggers";
            this.lblApexTriggers.Size = new System.Drawing.Size(109, 17);
            this.lblApexTriggers.TabIndex = 10;
            this.lblApexTriggers.Text = "Apex Triggers";
            // 
            // tbTriggerName
            // 
            this.tbTriggerName.Location = new System.Drawing.Point(88, 236);
            this.tbTriggerName.Name = "tbTriggerName";
            this.tbTriggerName.Size = new System.Drawing.Size(608, 20);
            this.tbTriggerName.TabIndex = 12;
            // 
            // lblTriggerName
            // 
            this.lblTriggerName.AutoSize = true;
            this.lblTriggerName.Location = new System.Drawing.Point(4, 240);
            this.lblTriggerName.Name = "lblTriggerName";
            this.lblTriggerName.Size = new System.Drawing.Size(71, 13);
            this.lblTriggerName.TabIndex = 11;
            this.lblTriggerName.Text = "Trigger Name";
            // 
            // lblSobject
            // 
            this.lblSobject.AutoSize = true;
            this.lblSobject.Location = new System.Drawing.Point(8, 268);
            this.lblSobject.Name = "lblSobject";
            this.lblSobject.Size = new System.Drawing.Size(43, 13);
            this.lblSobject.TabIndex = 13;
            this.lblSobject.Text = "sObject";
            // 
            // cmbSobject
            // 
            this.cmbSobject.FormattingEnabled = true;
            this.cmbSobject.Location = new System.Drawing.Point(88, 268);
            this.cmbSobject.Name = "cmbSobject";
            this.cmbSobject.Size = new System.Drawing.Size(608, 21);
            this.cmbSobject.TabIndex = 14;
            // 
            // btnSaveTrigger
            // 
            this.btnSaveTrigger.Location = new System.Drawing.Point(428, 304);
            this.btnSaveTrigger.Name = "btnSaveTrigger";
            this.btnSaveTrigger.Size = new System.Drawing.Size(104, 23);
            this.btnSaveTrigger.TabIndex = 15;
            this.btnSaveTrigger.Text = "Save Trigger";
            this.btnSaveTrigger.UseVisualStyleBackColor = true;
            this.btnSaveTrigger.Click += new System.EventHandler(this.btnSaveTrigger_Click);
            // 
            // btnSaveAndAddTrig
            // 
            this.btnSaveAndAddTrig.Location = new System.Drawing.Point(544, 304);
            this.btnSaveAndAddTrig.Name = "btnSaveAndAddTrig";
            this.btnSaveAndAddTrig.Size = new System.Drawing.Size(152, 23);
            this.btnSaveAndAddTrig.TabIndex = 16;
            this.btnSaveAndAddTrig.Text = "Save and Add Trigger";
            this.btnSaveAndAddTrig.UseVisualStyleBackColor = true;
            // 
            // lblLightningWebComponent
            // 
            this.lblLightningWebComponent.AutoSize = true;
            this.lblLightningWebComponent.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLightningWebComponent.Location = new System.Drawing.Point(4, 377);
            this.lblLightningWebComponent.Name = "lblLightningWebComponent";
            this.lblLightningWebComponent.Size = new System.Drawing.Size(198, 17);
            this.lblLightningWebComponent.TabIndex = 17;
            this.lblLightningWebComponent.Text = "Lightning Web Component";
            // 
            // lwcName
            // 
            this.lwcName.AutoSize = true;
            this.lwcName.Location = new System.Drawing.Point(4, 406);
            this.lwcName.Name = "lwcName";
            this.lwcName.Size = new System.Drawing.Size(62, 13);
            this.lwcName.TabIndex = 18;
            this.lwcName.Text = "LWC Name";
            // 
            // tbLWCName
            // 
            this.tbLWCName.Location = new System.Drawing.Point(88, 406);
            this.tbLWCName.Name = "tbLWCName";
            this.tbLWCName.Size = new System.Drawing.Size(608, 20);
            this.tbLWCName.TabIndex = 19;
            // 
            // btnSaveLWC
            // 
            this.btnSaveLWC.Location = new System.Drawing.Point(575, 475);
            this.btnSaveLWC.Name = "btnSaveLWC";
            this.btnSaveLWC.Size = new System.Drawing.Size(121, 23);
            this.btnSaveLWC.TabIndex = 22;
            this.btnSaveLWC.Text = "Save LWC";
            this.btnSaveLWC.UseVisualStyleBackColor = true;
            this.btnSaveLWC.Click += new System.EventHandler(this.btnSaveLWC_Click);
            // 
            // lvTargets
            // 
            this.lvTargets.CheckBoxes = true;
            this.lvTargets.HideSelection = false;
            this.lvTargets.Location = new System.Drawing.Point(926, 406);
            this.lvTargets.Name = "lvTargets";
            this.lvTargets.Size = new System.Drawing.Size(219, 274);
            this.lvTargets.TabIndex = 26;
            this.lvTargets.UseCompatibleStateImageBehavior = false;
            // 
            // lvAdditionalLWCFiles
            // 
            this.lvAdditionalLWCFiles.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.lvAdditionalLWCFiles.BackColor = System.Drawing.SystemColors.Window;
            this.lvAdditionalLWCFiles.CheckBoxes = true;
            this.lvAdditionalLWCFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvAdditionalLWCFiles.HideSelection = false;
            listViewItem1.StateImageIndex = 0;
            listViewItem2.StateImageIndex = 0;
            listViewItem3.StateImageIndex = 0;
            this.lvAdditionalLWCFiles.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3});
            this.lvAdditionalLWCFiles.LabelWrap = false;
            this.lvAdditionalLWCFiles.Location = new System.Drawing.Point(744, 406);
            this.lvAdditionalLWCFiles.Name = "lvAdditionalLWCFiles";
            this.lvAdditionalLWCFiles.Size = new System.Drawing.Size(158, 151);
            this.lvAdditionalLWCFiles.TabIndex = 24;
            this.lvAdditionalLWCFiles.UseCompatibleStateImageBehavior = false;
            this.lvAdditionalLWCFiles.View = System.Windows.Forms.View.List;
            // 
            // lblAdditionalLWCFiles
            // 
            this.lblAdditionalLWCFiles.AutoSize = true;
            this.lblAdditionalLWCFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAdditionalLWCFiles.Location = new System.Drawing.Point(741, 381);
            this.lblAdditionalLWCFiles.Name = "lblAdditionalLWCFiles";
            this.lblAdditionalLWCFiles.Size = new System.Drawing.Size(124, 13);
            this.lblAdditionalLWCFiles.TabIndex = 23;
            this.lblAdditionalLWCFiles.Text = "Additional LWC Files";
            // 
            // lblTargets
            // 
            this.lblTargets.AutoSize = true;
            this.lblTargets.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTargets.Location = new System.Drawing.Point(923, 379);
            this.lblTargets.Name = "lblTargets";
            this.lblTargets.Size = new System.Drawing.Size(50, 13);
            this.lblTargets.TabIndex = 25;
            this.lblTargets.Text = "Targets";
            // 
            // lblMasterLabel
            // 
            this.lblMasterLabel.AutoSize = true;
            this.lblMasterLabel.Location = new System.Drawing.Point(4, 441);
            this.lblMasterLabel.Name = "lblMasterLabel";
            this.lblMasterLabel.Size = new System.Drawing.Size(68, 13);
            this.lblMasterLabel.TabIndex = 20;
            this.lblMasterLabel.Text = "Master Label";
            // 
            // tbMasterLabel
            // 
            this.tbMasterLabel.Location = new System.Drawing.Point(88, 438);
            this.tbMasterLabel.Name = "tbMasterLabel";
            this.tbMasterLabel.Size = new System.Drawing.Size(608, 20);
            this.tbMasterLabel.TabIndex = 21;
            // 
            // AddObject
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1157, 692);
            this.Controls.Add(this.tbMasterLabel);
            this.Controls.Add(this.lblMasterLabel);
            this.Controls.Add(this.lblTargets);
            this.Controls.Add(this.lblAdditionalLWCFiles);
            this.Controls.Add(this.lvAdditionalLWCFiles);
            this.Controls.Add(this.lvTargets);
            this.Controls.Add(this.btnSaveLWC);
            this.Controls.Add(this.tbLWCName);
            this.Controls.Add(this.lwcName);
            this.Controls.Add(this.lblLightningWebComponent);
            this.Controls.Add(this.btnSaveAndAddTrig);
            this.Controls.Add(this.btnSaveTrigger);
            this.Controls.Add(this.cmbSobject);
            this.Controls.Add(this.lblSobject);
            this.Controls.Add(this.lblTriggerName);
            this.Controls.Add(this.tbTriggerName);
            this.Controls.Add(this.lblApexTriggers);
            this.Controls.Add(this.lblClassSectionHeader);
            this.Controls.Add(this.btnSaveAndAddClass);
            this.Controls.Add(this.btnSaveClass);
            this.Controls.Add(this.tbExtends);
            this.Controls.Add(this.lblExtends);
            this.Controls.Add(this.tbImplements);
            this.Controls.Add(this.lblImplements);
            this.Controls.Add(this.cbWithSharing);
            this.Controls.Add(this.lblClassName);
            this.Controls.Add(this.tbClassName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddObject";
            this.Text = "AddObject";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox tbClassName;
        public System.Windows.Forms.Label lblClassName;
        public System.Windows.Forms.CheckBox cbWithSharing;
        public System.Windows.Forms.Label lblImplements;
        public System.Windows.Forms.TextBox tbImplements;
        public System.Windows.Forms.Label lblExtends;
        public System.Windows.Forms.TextBox tbExtends;
        public System.Windows.Forms.Button btnSaveClass;
        public System.Windows.Forms.Button btnSaveAndAddClass;
        private System.Windows.Forms.Label lblClassSectionHeader;
        private System.Windows.Forms.Label lblApexTriggers;
        private System.Windows.Forms.Label lblTriggerName;
        private System.Windows.Forms.Label lblSobject;
        private System.Windows.Forms.ComboBox cmbSobject;
        private System.Windows.Forms.Button btnSaveTrigger;
        private System.Windows.Forms.Button btnSaveAndAddTrig;
        private System.Windows.Forms.Label lblLightningWebComponent;
        private System.Windows.Forms.Label lwcName;
        private System.Windows.Forms.Button btnSaveLWC;
        private System.Windows.Forms.ListView lvTargets;
        private System.Windows.Forms.ListView lvAdditionalLWCFiles;
        private System.Windows.Forms.Label lblAdditionalLWCFiles;
        private System.Windows.Forms.Label lblTargets;
        private System.Windows.Forms.Label lblMasterLabel;
        private System.Windows.Forms.TextBox tbMasterLabel;
        public System.Windows.Forms.TextBox tbTriggerName;
        public System.Windows.Forms.TextBox tbLWCName;
    }
}