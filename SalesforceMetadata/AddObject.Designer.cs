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
            this.cmbSobject.TabIndex = 15;
            // 
            // btnSaveTrigger
            // 
            this.btnSaveTrigger.Location = new System.Drawing.Point(428, 304);
            this.btnSaveTrigger.Name = "btnSaveTrigger";
            this.btnSaveTrigger.Size = new System.Drawing.Size(104, 23);
            this.btnSaveTrigger.TabIndex = 16;
            this.btnSaveTrigger.Text = "Save Trigger";
            this.btnSaveTrigger.UseVisualStyleBackColor = true;
            this.btnSaveTrigger.Click += new System.EventHandler(this.btnSaveTrigger_Click);
            // 
            // btnSaveAndAddTrig
            // 
            this.btnSaveAndAddTrig.Location = new System.Drawing.Point(544, 304);
            this.btnSaveAndAddTrig.Name = "btnSaveAndAddTrig";
            this.btnSaveAndAddTrig.Size = new System.Drawing.Size(152, 23);
            this.btnSaveAndAddTrig.TabIndex = 17;
            this.btnSaveAndAddTrig.Text = "Save and Add Trigger";
            this.btnSaveAndAddTrig.UseVisualStyleBackColor = true;
            // 
            // AddObject
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1157, 544);
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
        private System.Windows.Forms.TextBox tbTriggerName;
        private System.Windows.Forms.Label lblTriggerName;
        private System.Windows.Forms.Label lblSobject;
        private System.Windows.Forms.ComboBox cmbSobject;
        private System.Windows.Forms.Button btnSaveTrigger;
        private System.Windows.Forms.Button btnSaveAndAddTrig;
    }
}