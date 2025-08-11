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
        public String sourceCodeFolderPath;
        public String projectSolutionFolderPath;
        public String projectSolutionFilePath;

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

        private void tbSourceCodeFolder_DoubleClick(object sender, EventArgs e)
        {
            String selectedPath = UtilityClass.folderBrowserSelectPath("Select Source Code Folder",
                                                                       true,
                                                                       FolderEnum.SaveTo,
                                                                       Properties.Settings.Default.BaseFolderPath);
            if (selectedPath != "")
            {
                this.sourceCodeFolderPath = selectedPath;
                this.tbSourceCodeFolder.Text = selectedPath;
            }
        }

        private void tbProjectSolutionFolderPath_DoubleClick(object sender, EventArgs e)
        {
            String selectedPath = UtilityClass.folderBrowserSelectPath("Select Solution Folder",
                                                                       true,
                                                                       FolderEnum.SaveTo,
                                                                       Properties.Settings.Default.BaseFolderPath);

            if (selectedPath != "")
            {
                this.projectSolutionFolderPath = selectedPath;
                this.tbProjectSolutionFolderPath.Text = selectedPath;
                this.lastSolutionFolder = selectedPath;
            }
        }

        public void btnOK_Click(object sender, EventArgs e)
        {
            if (this.tbProjectSolutionFolderPath.Text == null || this.tbSolutionFileName.Text == null)
            {
                MessageBox.Show("Please popule the Solution Folder and/or Solution File Name first before clicking OK");
            }
            else
            {
                this.projectSolutionFilePath = this.tbProjectSolutionFolderPath.Text + "\\" + this.tbSolutionFileName.Text + ".sln";
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
