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
using System.Xml;

namespace SalesforceMetadata
{
    public partial class DevelopmentEnvironment : Form
    {
        // Key = the Metadata Name. Value = the folder the metadata file comes from
        //private Dictionary<String, String> metadataXmlNameToFolder;
        private Dictionary<String, String> usernameToSecurityToken;
        //private frmUserSettings userSetting;
        //private List<String> metadataObjectsList;

        //private UtilityClass.REQUESTINGORG reqOrg;

        private String orgName;

        public DevelopmentEnvironment()
        {
            InitializeComponent();
            populateCredentialsFile();
            populateSelectedAndDeploymentFolder();
            populateTreeView();
        }

        private void tbParentFolder_DoubleClick(object sender, EventArgs e)
        {
            String selectedPath = UtilityClass.folderBrowserSelectPath("Select Your Project Folder", 
                                                                       false, 
                                                                       FolderEnum.SaveTo,
                                                                       Properties.Settings.Default.DevelopmentSelectedFolder);

            if (selectedPath != "")
            {
                this.tbParentFolder.Text = selectedPath;
                Properties.Settings.Default.DevelopmentSelectedFolder = selectedPath;
                Properties.Settings.Default.Save();
                populateTreeView();
            }
        }

        private void tbDeployFrom_DoubleClick(object sender, EventArgs e)
        {
            String selectedPath = UtilityClass.folderBrowserSelectPath("Select The Deploy From Folder", 
                                                                       false, 
                                                                       FolderEnum.SaveTo,
                                                                       Properties.Settings.Default.DevelopmentDeploymentFolder);

            if (selectedPath != "")
            {
                this.tbDeployFrom.Text = selectedPath;
                Properties.Settings.Default.DevelopmentDeploymentFolder = selectedPath;
                Properties.Settings.Default.Save();
            }
        }

        private void populateCredentialsFile()
        {
            Boolean encryptionFileSettingsPopulated = true;
            if (Properties.Settings.Default.UserAndAPIFileLocation == ""
            || Properties.Settings.Default.SharedSecretLocation == "")
            {
                encryptionFileSettingsPopulated = false;
            }

            if (encryptionFileSettingsPopulated == false)
            {
                MessageBox.Show("Please populate the fields in the Settings from the Landing Page first, then use this form to download the Metadata.");
                return;
            }

            SalesforceCredentials.usernamePartnerUrl = new Dictionary<String, String>();
            SalesforceCredentials.usernameMetadataUrl = new Dictionary<String, String>();
            SalesforceCredentials.usernameToolingWsdlUrl = new Dictionary<String, String>();
            SalesforceCredentials.isProduction = new Dictionary<String, Boolean>();
            SalesforceCredentials.defaultWsdlObjects = new Dictionary<String, List<String>>();

            // Decrypt the contents of the file and place in an XML Document format
            StreamReader encryptedContents = new StreamReader(Properties.Settings.Default.UserAndAPIFileLocation);
            StreamReader sharedSecret = new StreamReader(Properties.Settings.Default.SharedSecretLocation);
            String decryptedContents = Crypto.DecryptString(encryptedContents.ReadToEnd(),
                                                            sharedSecret.ReadToEnd(),
                                                            Properties.Settings.Default.Salt);

            encryptedContents.Close();
            sharedSecret.Close();

            XmlDocument sfUser = new XmlDocument();
            sfUser.LoadXml(decryptedContents);

            XmlNodeList documentNodes = sfUser.GetElementsByTagName("usersetting");

            this.usernameToSecurityToken = new Dictionary<string, string>();

            for (int i = 0; i < documentNodes.Count; i++)
            {
                String username = "";
                String partnerWsdlUrl = "";
                String metadataWdldUrl = "";
                String toolingWsdlUrl = "";
                Boolean isProd = false;
                List<String> defaultWsdlObjectList = new List<String>();
                foreach (XmlNode childNode in documentNodes[i].ChildNodes)
                {
                    if (childNode.Name == "username")
                    {
                        username = childNode.InnerText;
                    }

                    if (childNode.Name == "securitytoken")
                    {
                        usernameToSecurityToken.Add(username, childNode.InnerText);
                    }

                    if (childNode.Name == "isproduction")
                    {
                        isProd = Convert.ToBoolean(childNode.InnerText);
                    }

                    if (childNode.Name == "partnerwsdlurl")
                    {
                        partnerWsdlUrl = childNode.InnerText;
                    }

                    if (childNode.Name == "metadatawsdlurl")
                    {
                        metadataWdldUrl = childNode.InnerText;
                    }

                    if (childNode.Name == "toolingwsdlurl")
                    {
                        toolingWsdlUrl = childNode.InnerText;
                    }

                    if (childNode.Name == "defaultpackages" && childNode.HasChildNodes)
                    {
                        XmlNodeList defObjects = childNode.ChildNodes;
                        foreach (XmlNode obj in defObjects)
                        {
                            defaultWsdlObjectList.Add(obj.InnerText);
                        }
                    }
                }

                SalesforceCredentials.usernamePartnerUrl.Add(username, partnerWsdlUrl);
                SalesforceCredentials.usernameMetadataUrl.Add(username, metadataWdldUrl);
                SalesforceCredentials.isProduction.Add(username, isProd);

                if (defaultWsdlObjectList.Count > 0)
                {
                    SalesforceCredentials.defaultWsdlObjects.Add(username, defaultWsdlObjectList);
                }

                if (toolingWsdlUrl != "")
                {
                    SalesforceCredentials.usernameToolingWsdlUrl.Add(username, toolingWsdlUrl);
                }
            }

            populateUserNames();
        }


        private void populateUserNames()
        {
            foreach (String un in SalesforceCredentials.usernamePartnerUrl.Keys)
            {
                this.cmbUserName.Items.Add(un);
            }
        }

        private void populateSelectedAndDeploymentFolder()
        {
            this.tbParentFolder.Text = Properties.Settings.Default.DevelopmentSelectedFolder;
            this.tbDeployFrom.Text = Properties.Settings.Default.DevelopmentDeploymentFolder;
        }

        private void populateTreeView()
        {
            if (this.tbParentFolder.Text != null
                && this.tbParentFolder.Text != "")
            {
                this.treeViewMetadata.Nodes.Clear();

                String[] folders = Directory.GetDirectories(this.tbParentFolder.Text);
                foreach (String folderName in folders)
                {
                    String[] folderNameSplit = folderName.Split('\\');
                    String[] fileNames = Directory.GetFiles(folderName);

                    TreeNode tnd1 = new TreeNode(folderNameSplit[folderNameSplit.Length - 1]);

                    if (folderNameSplit[folderNameSplit.Length - 1] == "aura"
                        || folderNameSplit[folderNameSplit.Length - 1] == "lwc")
                    {
                        String[] subFolders = Directory.GetDirectories(folderName);
                        foreach (String subFolder in subFolders)
                        {
                            String[] subfolderSplit = subFolder.Split('\\');
                            TreeNode tnd2 = new TreeNode(subfolderSplit[subfolderSplit.Length - 1]);

                            String[] subFolderFiles = Directory.GetFiles(subFolder);
                            foreach (String subFolderFile in subFolderFiles)
                            {
                                String[] subFolderFileSplit = subFolderFile.Split('\\');

                                TreeNode tnd3 = new TreeNode(subFolderFileSplit[subFolderFileSplit.Length - 1]);
                                tnd2.Nodes.Add(tnd3);
                            }

                            tnd1.Nodes.Add(tnd2);
                        }

                        this.treeViewMetadata.Nodes.Add(tnd1);
                    }
                    else
                    {
                        foreach (String fileName in fileNames)
                        {
                            String[] fileNameSplit = fileName.Split('\\');
                            String[] objectNameSplit = fileNameSplit[fileNameSplit.Length - 1].Split('.');
                            TreeNode tnd2 = new TreeNode(fileNameSplit[fileNameSplit.Length - 1]);

                            try
                            {
                                XmlDocument xd = new XmlDocument();
                                xd.Load(fileName);

                                foreach (XmlNode nd1 in xd.ChildNodes)
                                {
                                    foreach (XmlNode nd2 in nd1.ChildNodes)
                                    {
                                        TreeNode tnd3 = new TreeNode(nd2.OuterXml);
                                        tnd2.Nodes.Add(tnd3);
                                    }
                                }

                                tnd1.Nodes.Add(tnd2);
                            }
                            catch (Exception e)
                            {
                                tnd1.Nodes.Add(tnd2);
                            }
                        }

                        this.treeViewMetadata.Nodes.Add(tnd1);
                    }
                }
            }
        }

        private void cmbUserName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SalesforceCredentials.isProduction[this.cmbUserName.Text] == true)
            {
                //this.lblSalesforce.Text = "Salesforce";
                //this.Text = "Salesforce Metadata - Production";
            }
            else
            {
                //this.lblSalesforce.Text = "Salesforce Sandbox";
                String[] userNamesplit = this.cmbUserName.Text.Split('.');
                this.Text = "Salesforce Metadata - " + userNamesplit[userNamesplit.Length - 1].ToUpper();
            }

            this.tbSecurityToken.Text = "";
            if (this.usernameToSecurityToken.ContainsKey(this.cmbUserName.Text))
            {
                this.tbSecurityToken.Text = this.usernameToSecurityToken[cmbUserName.Text];
            }
        }

    }
}
