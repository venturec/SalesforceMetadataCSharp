using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SalesforceMetadata
{
    public partial class ObjectModification : Form
    {
        public ObjectModification()
        {
            InitializeComponent();
            this.lbFileNames.Items.Clear();
            //getAllObjectFiles();
        }

        private void tbObjectFolderLocation_DoubleClick(object sender, EventArgs e)
        {
            refreshFileListBox();
        }


        // Now get all of the .object files and start sifting through them to prep them for deployment to another oreg
        // Open each one individually as an Xml Document and add/remove/update the tags.
        // Removed any leading package names (i.e. pse__, ffrr__)
        // Rename the file removing the leading package names as well.
        private void getAllObjectFiles()
        {
            String[] fileNames = Directory.GetFiles(this.tbObjectFolderLocation.Text);
            String[] directorySplit = this.tbObjectFolderLocation.Text.Split('\\');

            if (fileNames != null)
            {
                foreach (String fn in fileNames)
                {
                    String[] fileNameSplit = fn.Split('\\');

                    // I need to get the last element in the array, but won't automatically know how many elements there are
                    // Keep in mind the array is 0 based, so the directorySplit.Length will provide the last element in that array
                    this.lbFileNames.Items.Add(fileNameSplit[directorySplit.Length]);
                }
            }
        }

        private void btnRemovePkgNameFromFile_Click(object sender, EventArgs e)
        {
            if (this.lbFileNames.Items.Count > 0)
            {
                for (Int32 i=0; i<this.lbFileNames.Items.Count; i++)
                {
                    if(File.Exists(this.tbObjectFolderLocation.Text + '\\' + (String)this.lbFileNames.Items[i]))
                    {
                        String oldFileName = (String)this.lbFileNames.Items[i];

                        String[] separator = new String[1];
                        separator[0] = "__";
                        String[] oldFileNameSplit = oldFileName.Split(separator, StringSplitOptions.None);

                        // Example: If the object name is pse__Assignment__c.object, then the split should look similar to:
                        // oldFileNameSplit[0] = "pse"
                        // oldFileNameSplit[1] = Assignment
                        // oldFileNameSplit[2] = "c.object"

                        // But if there is no package name, but still a custom object, it will only be an array of 2

                        // And if there is no package name and it is not custom, it will be an array of 1
                        // Only standard Salesforce objects will have an array of 1. All others, including packages, should be an arry of 2 or more

                        String newFileName = "";
                        Boolean renameFile = false;
                        if (oldFileNameSplit.Length == 3)
                        {
                            newFileName = oldFileNameSplit[1] + "__" + oldFileNameSplit[2];
                            renameFile = true;
                        }
                        else if (oldFileNameSplit.Length == 2)
                        {
                            newFileName = oldFileNameSplit[0] + "__" + oldFileNameSplit[1];
                        }
                        else if (oldFileNameSplit.Length == 1)
                        {
                            newFileName = oldFileNameSplit[0];
                        }

                        if (renameFile == true && newFileName != "")
                        {
                            File.Copy(this.tbObjectFolderLocation.Text + '\\' + oldFileName, this.tbObjectFolderLocation.Text + '\\' + newFileName, true);
                            File.Delete(this.tbObjectFolderLocation.Text + '\\' + oldFileName);
                        }
                    }
                }

                refreshFileListBox();
            }
        }


        private void refreshFileListBox()
        {
            this.lbFileNames.Items.Clear();

            this.tbObjectFolderLocation.Text = UtilityClass.folderBrowserSelectPath("Select the Folder Which Contains the Objects to Parse", false, FolderEnum.ReadFrom);

            if (Directory.Exists(this.tbObjectFolderLocation.Text))
            {
                getAllObjectFiles();
            }
        }

        private void btnRemovePkgNameFromFields_Click(object sender, EventArgs e)
        {
            if (this.tbPkgName.Text != "" &&  this.lbFileNames.Items.Count > 0)
            {
                for (Int32 i = 0; i < this.lbFileNames.Items.Count; i++)
                {
                    if (File.Exists(this.tbObjectFolderLocation.Text + '\\' + (String)this.lbFileNames.Items[i]))
                    {
                        // Find and replace the package name on all inner text fields
                        String fileText = File.ReadAllText(this.tbObjectFolderLocation.Text + '\\' + (String)this.lbFileNames.Items[i]);

                        fileText = Regex.Replace(fileText, this.tbPkgName.Text + "__", "");
                        fileText = Regex.Replace(fileText, "<customHelpPage>(.*)</customHelpPage>", "<customHelpPage></customHelpPage>");
                        fileText = Regex.Replace(fileText, @"<webLinks>(.|\n)*?</webLinks>", String.Empty);
                        fileText = Regex.Replace(fileText, @"^\s+$[\r\n]*", String.Empty, RegexOptions.Multiline);

                        File.WriteAllText(this.tbObjectFolderLocation.Text + '\\' + (String)this.lbFileNames.Items[i], fileText);
                    }
                }
            }
        }

    }
}
