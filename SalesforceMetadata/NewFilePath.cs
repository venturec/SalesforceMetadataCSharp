using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesforceMetadata
{
    public partial class NewFilePath : Form
    {
        public String lastSolutionFolder;
        public String baseFolderPath;
        public String solutionFolderPath;
        public String solutionFilePath;

        public NewFilePath()
        {
            InitializeComponent();
        }

        private void tbBaseFolderPath_DoubleClick(object sender, EventArgs e)
        {
            String selectedPath = UtilityClass.folderBrowserSelectPath("Select Base Folder", 
                                                                       true, 
                                                                       FolderEnum.SaveTo, 
                                                                       Properties.Settings.Default.BaseFolderPath);

            if (selectedPath != "")
            {
                this.tbBaseFolderPath.Text = selectedPath;
                Properties.Settings.Default.BaseFolderPath = this.tbBaseFolderPath.Text;
                Properties.Settings.Default.Save();
            }
        }

        private void tbSolutionFolderPath_DoubleClick(object sender, EventArgs e)
        {
            String selectedPath = UtilityClass.folderBrowserSelectPath("Select Solution Folder",
                                                                       true,
                                                                       FolderEnum.SaveTo,
                                                                       Properties.Settings.Default.BaseFolderPath);

            if (selectedPath != "")
            {
                this.solutionFolderPath = selectedPath;
                this.tbSolutionFolderPath.Text = selectedPath;
                this.lastSolutionFolder = selectedPath;
            }
        }

        public void btnOK_Click(object sender, EventArgs e)
        {
            if (this.tbSolutionFolderPath.Text == null || this.tbSolutionFileName.Text == null)
            {
                MessageBox.Show("Please popule the Solution Folder and/or Solution File Name first before clicking OK");
            }
            else
            {
                this.solutionFilePath = this.tbSolutionFolderPath.Text + "\\" + this.tbSolutionFileName.Text + ".sln";
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
