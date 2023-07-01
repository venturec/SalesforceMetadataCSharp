using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SalesforceMetadata
{
    public partial class VersionUpdater : Form
    {
        public VersionUpdater()
        {
            InitializeComponent();
            loadDefaultApis();
        }

        private void tbComponentsLocation_DoubleClick(object sender, EventArgs e)
        {
            String selectedPath = UtilityClass.folderBrowserSelectPath("Select Top Level Directory or Sub-directory to update API Versions",
                                                                                  true,
                                                                                  FolderEnum.ReadFrom,
                                                                                  Properties.Settings.Default.ApiVersionUpdaterLastReadLocation);
            if(selectedPath != "")
            {
                this.tbComponentsLocation.Text = selectedPath;
                Properties.Settings.Default.ApiVersionUpdaterLastReadLocation = selectedPath;
                Properties.Settings.Default.Save();
                this.btnUpdateAPI.Enabled = true;
            }
        }

        public void loadDefaultApis()
        {
            foreach (String api in UtilityClass.generateAPIArray())
            {
                this.cmbDefaultAPI.Items.Add(api);
            }

            this.cmbDefaultAPI.Text = Properties.Settings.Default.DefaultAPI;
        }

        private void btnUpdateAPI_Click(object sender, EventArgs e)
        {
            List<String> subdirectorySearchCompleted = new List<String>();

            // Escape any characters in the search String first
            // Get each folder and subfolder
            List<String> subDirectoryList = new List<String>();
            subDirectoryList.Add(this.tbComponentsLocation.Text);
            subDirectoryList.AddRange(getSubdirectories(this.tbComponentsLocation.Text));

            //Boolean resultsFound = false;
            Boolean subdirectoriesExist = false;
            if (subDirectoryList.Count > 0)
            {
                subdirectoriesExist = true;
            }

            while (subdirectoriesExist == true)
            {
                if (subDirectoryList.Count == 0) subdirectoriesExist = false;

                for (Int32 i = 0; i < subDirectoryList.Count; i++)
                {
                    try
                    {
                        // Get all files in the current directory
                        String[] files = Directory.GetFiles(subDirectoryList[i]);
                        if (files.Length > 0)
                        {
                            for (Int32 j = 0; j < files.Length; j++)
                            {
                                if (files[j].EndsWith(".xml"))
                                {
                                    XmlDocument xd = new XmlDocument();
                                    xd.Load(files[j]);

                                    XmlNodeList nodeList = xd.GetElementsByTagName("apiVersion");
                                    foreach (XmlNode nd in nodeList)
                                    {
                                        if (nd.InnerText != this.cmbDefaultAPI.Text)
                                        {
                                            nd.InnerText = this.cmbDefaultAPI.Text;
                                            xd.Save(files[j]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception exc)
                    {

                    }

                    subdirectorySearchCompleted.Add(subDirectoryList[i]);
                }

                // Check if there are any additional sub directories in the current directory and add them to the list
                List<String> subDirectories = new List<String>();
                for (Int32 i = 0; i < subDirectoryList.Count; i++)
                {
                    if (subDirectoryList[i] != this.tbComponentsLocation.Text)
                    {
                        List<String> sds = getSubdirectories(subDirectoryList[i]);
                        if (sds.Count > 0)
                        {
                            foreach (String s in sds)
                            {
                                if (!subdirectorySearchCompleted.Contains(s))
                                {
                                    subDirectories.Add(s);
                                }
                            }
                        }
                    }
                }

                // Remove the current directories in subDirectoriesList before adding the additional subdirectories
                subDirectoryList.Clear();

                if (subDirectories.Count > 0)
                {
                    foreach (String s in subDirectories)
                    {
                        if (!subDirectoryList.Contains(s))
                        {
                            subDirectoryList.Add(s);
                        }
                    }

                    subDirectories.Clear();
                }
            }
        }

        private List<String> getSubdirectories(String folderLocation)
        {
            // Check for additional subdirectories in the current subdirectory list and add them to the list
            List<String> subDirectoryList = new List<String>();
            String[] subDirectories = new String[0];
            subDirectories = Directory.GetDirectories(folderLocation);
            foreach (String sub in subDirectories)
            {
                subDirectoryList.Add(sub);
            }

            return subDirectoryList;
        }

    }
}
