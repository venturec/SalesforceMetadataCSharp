using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SalesforceMetadata
{
    public partial class AddObject : Form
    {
        public String projectFolderPath;

        public event EventHandler<AddObjectEvent> ObjectAdded;

        public AddObject()
        {
            InitializeComponent();
        }

        public void loadSobjectsToCombobox()
        {
            if (Directory.Exists(projectFolderPath)) 
            {
                String[] SobjectFiles = Directory.GetFiles(this.projectFolderPath + "\\objects");
                foreach(String sobjFile in SobjectFiles) 
                {
                    String[] fileNameSplit = sobjFile.Split('.');
                    String[] objectName = fileNameSplit[0].Split('\\');

                    this.cmbSobject.Items.Add(objectName[objectName.Length - 1]);
                }
            }
        }

        private void btnAddClass_Click(object sender, EventArgs e)
        {
            String[] filesCreated = new string[2];

            StreamWriter sw = new StreamWriter(projectFolderPath + "\\classes\\" + this.tbClassName.Text + ".cls");
            sw.Write("public");

            if (this.cbWithSharing.Checked == true)
            {
                sw.Write(" with sharing");
            }
            else
            {
                sw.Write(" without sharing");
            }

            sw.Write(" class " + this.tbClassName.Text);

            if (this.tbExtends.Text != "")
            {
                sw.Write(" " + this.tbExtends.Text);
            }

            if (this.tbImplements.Text != "")
            {
                sw.Write(" " + this.tbImplements.Text);
            }

            sw.Write(Environment.NewLine + "{" + Environment.NewLine + Environment.NewLine + "}");
            sw.Close();

            sw = new StreamWriter(projectFolderPath + "\\classes\\" + this.tbClassName.Text + ".cls-meta.xml");
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sw.WriteLine("<ApexClass xmlns=\"http://soap.sforce.com/2006/04/metadata\">");
            sw.WriteLine("<apiVersion>" + Properties.Settings.Default.DefaultAPI + "</apiVersion>");
            sw.WriteLine("<status>Active</status>");
            sw.WriteLine("</ApexClass>");
            sw.Close();

            filesCreated[0] = this.tbClassName.Text + ".cls";
            filesCreated[1] = this.tbClassName.Text + ".cls-meta.xml";

            refreshDevelopmentForm("classes", filesCreated);
        }

        private void btnSaveTrigger_Click(object sender, EventArgs e)
        {
            String[] filesCreated = new string[2];

            if (this.tbTriggerName.Text == "" || this.cmbSobject.Text == "")
            {
                MessageBox.Show("Please make sure the trigger name and sobject field is populated");
            }
            else
            {
                StreamWriter sw = new StreamWriter(projectFolderPath + "\\triggers\\" + this.tbTriggerName.Text + ".trigger");
                sw.Write("trigger ");
                sw.Write(this.tbTriggerName.Text);
                sw.Write(" on ");
                sw.Write(this.cmbSobject.Text);
                sw.Write(" (before insert, before update, before delete, after insert, after update, after delete, after undelete) ");
                sw.Write(Environment.NewLine + "{" + Environment.NewLine + Environment.NewLine + "}");
                sw.Close();

                sw = new StreamWriter(projectFolderPath + "\\triggers\\" + this.tbTriggerName.Text + ".trigger-meta.xml");
                sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                sw.WriteLine("<ApexTrigger xmlns=\"http://soap.sforce.com/2006/04/metadata\">");
                sw.WriteLine("<apiVersion>" + Properties.Settings.Default.DefaultAPI + "</apiVersion>");
                sw.WriteLine("<status>Active</status>");
                sw.WriteLine("</ApexTrigger>");
                sw.Close();

                filesCreated[0] = this.tbTriggerName.Text + ".trigger";
                filesCreated[1] = this.tbTriggerName.Text + ".trigger-meta.xml";

                refreshDevelopmentForm("triggers", filesCreated);
            }
        }

        private void refreshDevelopmentForm(String nodeType, String[] filesCreated)
        {
            FormCollection fc = Application.OpenForms;
            foreach (System.Windows.Forms.Form openFrm in fc)
            {
                if (openFrm.Name == "DevelopmentEnvironment")
                {
                    openFrm.Refresh();
                    openFrm.Show();
                    openFrm.Location = this.Location;
                    openFrm.BringToFront();
                }
            }

            ObjectAdded.Invoke(this, new AddObjectEvent(true, nodeType, filesCreated));

            this.Close();
        }
    }
}
