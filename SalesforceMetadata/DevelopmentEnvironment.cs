using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

using SalesforceMetadata.MetadataWSDL;
using SalesforceMetadata.ToolingWSDL;

namespace SalesforceMetadata
{
    public partial class DevelopmentEnvironment : Form
    {
        private Dictionary<String, String> usernameToSecurityToken;
        HashSet<String> mainFolderNames;
        Dictionary<String, TreeNode> tnListAddDependencies;
        Dictionary<String, TreeNode> tnListRemoveDependencies;

        //private String orgName;
        private Boolean bypassTextChange = false;
        private Boolean projectValuesChanged = false;
        private Boolean runTreeNodeSelector = true;

        private HashSet<String> standardValueSets = new HashSet<string>();

        public DevelopmentEnvironment()
        {
            InitializeComponent();
            populateCredentialsFile();
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

        public void populateTreeView()
        {
            mainFolderNames = new HashSet<string>();
            tnListAddDependencies = new Dictionary<String, TreeNode>();
            tnListRemoveDependencies = new Dictionary<String, TreeNode>();

            if (this.tbProjectFolder.Text != null
                && this.tbProjectFolder.Text != "")
            {
                this.treeViewMetadata.Nodes.Clear();

                String[] folders = Directory.GetDirectories(this.tbProjectFolder.Text);
                foreach (String folderName in folders)
                {
                    String[] folderNameSplit = folderName.Split('\\');
                    String[] fileNames = Directory.GetFiles(folderName);

                    TreeNode tnd1 = new TreeNode(folderNameSplit[folderNameSplit.Length - 1]);
                    mainFolderNames.Add(tnd1.Text);

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
        private void treeViewMetadata_AfterCheck(object sender, TreeViewEventArgs e)
        {
            treeViewNodeAfterCheck(e.Node);
        }

        public void treeViewNodeAfterCheck(TreeNode tn)
        {
            if (runTreeNodeSelector == true)
            {
                runTreeNodeSelector = false;
            }
            else
            {
                return;
            }

            //Debug.WriteLine("private void treeViewMetadata_AfterCheck(object sender, TreeViewEventArgs e)");

            Boolean blAddDependencies = false;
            Boolean blRemoveDependencies = false;

            TreeNode parentTn = tn.Parent;

            // This means the selected Node is the top node for that group and the sub-nodes will need to be selected.
            if (tn.Checked == true && tn.Nodes.Count > 0)
            {
                //Debug.WriteLine("tn.Checked == true && tn.Nodes.Count > 0 : " + runTreeNodeSelector);

                foreach (TreeNode cNode in tn.Nodes)
                {
                    cNode.Checked = true;
                    blAddDependencies = true;

                    if (cNode.Nodes.Count > 0)
                    {
                        foreach (TreeNode c2Node in cNode.Nodes)
                        {
                            c2Node.Checked = true;
                        }
                    }
                }
            }
            else if (tn.Checked == false && tn.Nodes.Count > 0)
            {
                //Debug.WriteLine("tn.Checked == false && tn.Nodes.Count > 0 : " + runTreeNodeSelector);

                foreach (TreeNode cNode in tn.Nodes)
                {
                    cNode.Checked = false;
                    blRemoveDependencies = true;

                    if (cNode.Nodes.Count > 0)
                    {
                        foreach (TreeNode c2Node in cNode.Nodes)
                        {
                            c2Node.Checked = false;
                        }
                    }
                }
            }
            // Mostly for Aura and LWC components
            // Make sure to check the parent folder and any unchecked
            else if (tn.Checked == true && parentTn != null)
            {
                if (!mainFolderNames.Contains(parentTn.Text))
                {
                    //Debug.WriteLine("tn.Checked == true && parentTn != null && !mainFolderNames.Contains(parentTn.Text) : " + runTreeNodeSelector);

                    parentTn.Checked = true;
                    blAddDependencies = true;

                    String[] nodeFullPath = parentTn.FullPath.Split('\\');

                    if (nodeFullPath[0] != "objects" && nodeFullPath[0] != "objectTranslations")
                    {
                        //Debug.WriteLine("if (nodeFullPath[0] != \"objects\" && nodeFullPath[0] != \"objectTranslations\") " + runTreeNodeSelector);
                        foreach (TreeNode cNode in parentTn.Nodes)
                        {
                            //Debug.WriteLine("foreach (TreeNode cNode in parentTn.Nodes) " + runTreeNodeSelector);
                            cNode.Checked = true;

                            if (cNode.Nodes.Count > 0)
                            {
                                //Debug.WriteLine("if (cNode.Nodes.Count > 0) " + runTreeNodeSelector);
                                foreach (TreeNode c2Node in cNode.Nodes)
                                {
                                    //Debug.WriteLine("foreach (TreeNode c2Node in cNode.Nodes) " + runTreeNodeSelector);
                                    c2Node.Checked = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    blAddDependencies = true;
                }
            }
            else if (tn.Checked == false && parentTn != null)
            {
                blRemoveDependencies = true;
            }

            if (blAddDependencies == true)
            {
                //Debug.WriteLine("blAddDependencies");
                addDependencies(tn);
            }

            if (blRemoveDependencies == true)
            {
                //Debug.WriteLine("");
                removeDependencies(tn);
            }

            runTreeNodeSelector = true;
        }

        public void addDependencies(TreeNode tn)
        {
            //Debug.WriteLine("public void addDependencies(TreeNode tn)");

            foreach (TreeNode treeNd in this.treeViewMetadata.Nodes)
            {
                if (treeNd.Checked == true)
                {
                    foreach (TreeNode tnd2 in treeNd.Nodes)
                    {
                        String[] nodeFullPath = tnd2.FullPath.Split('\\');
                        String[] fileNameSplit = nodeFullPath[1].Split('.');

                        if (!this.tnListAddDependencies.ContainsKey(nodeFullPath[0] + "@" + fileNameSplit[0]))
                        {
                            this.tnListAddDependencies.Add(nodeFullPath[0] + "@" + fileNameSplit[0], tn);
                            selectDependencies(tnd2, nodeFullPath, fileNameSplit);
                        }

                        if (this.tnListRemoveDependencies.ContainsKey(nodeFullPath[0] + "@" + fileNameSplit[0]))
                        {
                            this.tnListRemoveDependencies.Remove(nodeFullPath[0] + "@" + fileNameSplit[0]);
                        }
                    }
                }
                else
                {
                    foreach (TreeNode tnd2 in treeNd.Nodes)
                    {
                        if (tnd2.Checked == true)
                        {
                            String[] nodeFullPath = tnd2.FullPath.Split('\\');
                            String[] fileNameSplit = nodeFullPath[1].Split('.');

                            if (!this.tnListAddDependencies.ContainsKey(nodeFullPath[0] + "@" + fileNameSplit[0]))
                            {
                                this.tnListAddDependencies.Add(nodeFullPath[0] + "@" + fileNameSplit[0], tn);
                                selectDependencies(tnd2, nodeFullPath, fileNameSplit);
                            }

                            if (this.tnListRemoveDependencies.ContainsKey(nodeFullPath[0] + "@" + fileNameSplit[0]))
                            {
                                this.tnListRemoveDependencies.Remove(nodeFullPath[0] + "@" + fileNameSplit[0]);
                            }
                        }
                    }
                }
            }
        }

        private void removeDependencies(TreeNode tn)
        {
            // TODO: This will be very complex especially with custom objects if multiple fields are selected and then one is deselected.
            // Don't want to remove the dependencies if there are more than 1 field or item selected.
            // Then again, what if it is a net new object?
            // Have to think through the logic and processes to make this work.

            String[] nodeFullPath = tn.FullPath.Split('\\');

            if (nodeFullPath.Length > 2)
            { 
                // Do something else
            }
            else if (nodeFullPath.Length == 2)
            {
                String[] fileNameSplit = nodeFullPath[1].Split('.');
                if (this.tnListAddDependencies.ContainsKey(nodeFullPath[0] + "@" + fileNameSplit[0]))
                {
                    this.tnListAddDependencies.Remove(nodeFullPath[0] + "@" + fileNameSplit[0]);
                }

                //if (!this.tnListRemoveDependencies.ContainsKey(nodeFullPath[0] + "@" + fileNameSplit[0]))
                //{
                //    this.tnListRemoveDependencies.Remove(nodeFullPath[0] + "@" + fileNameSplit[0]));
                //}
            }
            else if (nodeFullPath.Length == 1)
            {

            }
        }
        private void selectDependencies(TreeNode tn, String[] nodeFullPath, String[] fileNameSplit)
        {
            //Debug.WriteLine("public void selectDependencies(TreeNode tn, String[] nodeFullPath, String[] fileNameSplit)");

            String[] objectName = new string[2];
            if (nodeFullPath.Length > 1)
            {
                objectName = nodeFullPath[1].Split('.');
            }

            // TODO:
            // If a standard picklist field is selected, make sure to also select the related StandardValueSet. You will need a separate method for determining
            //      if a field selected translates to a StandardValueSet

            if (nodeFullPath[0] == "aura")
            {
                foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
                {
                    if (tnd1.Text == "aura")
                    {
                        foreach (TreeNode tnd2 in tnd1.Nodes)
                        {
                            if (tnd2.Text == nodeFullPath[1])
                            {
                                tnd2.Checked = true;
                                foreach (TreeNode tnd3 in tnd2.Nodes)
                                {
                                    tnd3.Checked = true;
                                }
                            }
                        }
                    }
                }
            }
            else if (nodeFullPath[0] == "lwc")
            {
                foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
                {
                    if (tnd1.Text == "lwc")
                    {
                        foreach (TreeNode tnd2 in tnd1.Nodes)
                        {
                            if (tnd2.Text == nodeFullPath[1])
                            {
                                tnd2.Checked = true;
                                foreach (TreeNode tnd3 in tnd2.Nodes)
                                {
                                    tnd3.Checked = true;
                                }
                            }
                        }
                    }
                }
            }
            // Custom object
            else if (nodeFullPath[0] == "objects"
                && nodeFullPath[1].Contains("__c"))
            {
                selectRequiredObjectFields(nodeFullPath[1]);
            }
            // Custom Metadata object
            else if (nodeFullPath[0] == "objects"
                && nodeFullPath[1].Contains("__mdt"))
            {
                // First check off the sub-nodes from the parent
                foreach (TreeNode tnd3 in tn.Nodes)
                {
                    tnd3.Checked = true;
                }

                // Check off the Metadata Types in the customMetadata folder related to the metadata type checked.
                foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
                {
                    if (tnd1.Text == "customMetadata")
                    {
                        String[] splitObjectFileName = nodeFullPath[1].Split(new String[] { "__" }, StringSplitOptions.None);

                        foreach (TreeNode tnd2 in tnd1.Nodes)
                        {
                            String[] splitTND2Node = tnd2.Text.Split('.');
                            if (splitTND2Node[0] == splitObjectFileName[0])
                            {
                                tnd2.Checked = true;

                                foreach (TreeNode tnd3 in tnd2.Nodes)
                                {
                                    tnd3.Checked = true;
                                }
                            }
                        }
                    }
                }
            }
            // Standard object
            else if (nodeFullPath[0] == "objects")
            {
                if (tn.Text.StartsWith("<fields"))
                {
                    String tnd3XmlString = "<document>" + tn.Text + "</document>";
                    XmlDocument tnd3Xd = new XmlDocument();
                    tnd3Xd.LoadXml(tnd3XmlString);

                    String objectFieldCombo = objectName[0] + "." + tnd3Xd.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;

                    populateStandardValueSetHashSet(objectFieldCombo);
                }
            }
            else if (nodeFullPath[0] == "certs")
            {
                // Get the class name and then make sure the XML file is checked too
                foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
                {
                    if (tnd1.Text == "certs")
                    {
                        foreach (TreeNode tnd2 in tnd1.Nodes)
                        {
                            if (tnd2.Text == nodeFullPath[1] + "-meta.xml")
                            {
                                tnd2.Checked = true;
                            }
                        }
                    }
                }
            }
            else if (nodeFullPath[0] == "classes")
            {
                // Get the class name and then make sure the XML file is checked too
                foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
                {
                    if (tnd1.Text == "classes")
                    {
                        foreach (TreeNode tnd2 in tnd1.Nodes)
                        {
                            if (tnd2.Text == nodeFullPath[1] + "-meta.xml")
                            {
                                tnd2.Checked = true;
                                if (tnd2.Nodes.Count > 0)
                                {
                                    foreach (TreeNode tnd3 in tnd2.Nodes)
                                    {
                                        tnd3.Checked = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (nodeFullPath[0] == "components")
            {
                // Get the class name and then make sure the XML file is checked too
                foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
                {
                    if (tnd1.Text == "components")
                    {
                        foreach (TreeNode tnd2 in tnd1.Nodes)
                        {
                            if (tnd2.Text == nodeFullPath[1] + "-meta.xml")
                            {
                                tnd2.Checked = true;
                            }
                        }
                    }
                }
            }
            else if (nodeFullPath[0] == "contentassets")
            {
                foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
                {
                    if (tnd1.Text == "contentassets")
                    {
                        foreach (TreeNode tnd2 in tnd1.Nodes)
                        {
                            if (tnd2.Text == nodeFullPath[1] + "-meta.xml")
                            {
                                tnd2.Checked = true;
                            }
                        }
                    }
                }
            }
            else if (nodeFullPath[0] == "layouts")
            {
                String[] objectNameSplit = nodeFullPath[1].Split('-');

                String xmlNodes = "<Layout>";

                // First, make sure the entire layout is included
                foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
                {
                    if (tnd1.Text == "layouts")
                    {
                        foreach (TreeNode tnd2 in tnd1.Nodes)
                        {
                            if (tnd2.Text == nodeFullPath[1])
                            {
                                tnd2.Checked = true;

                                foreach (TreeNode tnd3 in tnd2.Nodes)
                                {
                                    tnd3.Checked = true;
                                    xmlNodes = xmlNodes + tnd3.Text;
                                }
                            }
                        }
                    }
                }

                xmlNodes = xmlNodes + "</Layout>";

                // Second, get the related object and fields from the layout
                // Third, go back to the object in the Tree View and select all fields which are on the layout based on the object selected
                selectObjectFieldsFromLayout(objectNameSplit[0], xmlNodes);
            }
            else if (nodeFullPath[0] == "pages")
            {
                // Get the class name and then make sure the XML file is checked too
                foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
                {
                    if (tnd1.Text == "pages")
                    {
                        foreach (TreeNode tnd2 in tnd1.Nodes)
                        {
                            if (tnd2.Text == nodeFullPath[1] + "-meta.xml")
                            {
                                tnd2.Checked = true;
                            }
                        }
                    }
                }
            }
            else if (nodeFullPath[0] == "permissionsets")
            {
                foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
                {
                    if (tnd1.Text == "permissionsets")
                    {
                        foreach (TreeNode tnd2 in tnd1.Nodes)
                        {
                            if (tnd2.Text == nodeFullPath[1])
                            {
                                foreach (TreeNode tnd3 in tnd2.Nodes)
                                {
                                    if (tnd3.Text.StartsWith("<label"))
                                    {
                                        tnd3.Checked = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (nodeFullPath[0] == "staticresources")
            {
                // Get the class name and then make sure the XML file is checked too
                foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
                {
                    if (tnd1.Text == "staticresources")
                    {
                        foreach (TreeNode tnd2 in tnd1.Nodes)
                        {
                            String[] tnd2FileNameSplit = tnd2.Text.Split('.');

                            //if (tnd2FileNameSplit[0] == fileNameSplit[0]
                            //    && tnd2.Text.EndsWith("-meta.xml"))
                            //{
                            //    tnd2.Checked = true;
                            //}
                        }
                    }
                }
            }
            else if (nodeFullPath[0] == "triggers")
            {
                // Get the trigger name and then make sure the XML file is checked too
                foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
                {
                    if (tnd1.Text == "triggers")
                    {
                        foreach (TreeNode tnd2 in tnd1.Nodes)
                        {
                            if (tnd2.Text == nodeFullPath[1] + "-meta.xml")
                            {
                                tnd2.Checked = true;
                            }
                        }
                    }
                }
            }

            if (standardValueSets.Count > 0)
            {
                selectStandardValueSets(standardValueSets);
            }
        }
        private void selectRequiredObjectFields(String objectName)
        {
            foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
            {
                if (tnd1.Text == "objects")
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (tnd2.Text == objectName)
                        {
                            foreach (TreeNode tnd3 in tnd2.Nodes)
                            {
                                if (tnd3.Text.StartsWith("<deploymentStatus"))
                                {
                                    tnd3.Checked = true;
                                }
                                else if (tnd3.Text.StartsWith("<description"))
                                {
                                    tnd3.Checked = true;
                                }
                                else if (tnd3.Text.StartsWith("<label"))
                                {
                                    tnd3.Checked = true;
                                }
                                else if (tnd3.Text.StartsWith("<nameField"))
                                {
                                    tnd3.Checked = true;
                                }
                                else if (tnd3.Text.StartsWith("<pluralLabel"))
                                {
                                    tnd3.Checked = true;
                                }
                                else if (tnd3.Text.StartsWith("<sharingModel"))
                                {
                                    tnd3.Checked = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void populateStandardValueSetHashSet(String objectFieldCombo)
        {
            if (objectFieldCombo == "Account.Industry"
                || objectFieldCombo == "Lead.Industry")
            {
                standardValueSets.Add("Industry");
            }
            else if (objectFieldCombo == "Account.Ownership")
            {
                standardValueSets.Add("AccountOwnership");
            }
            else if (objectFieldCombo == "Account.Rating"
                || objectFieldCombo == "Lead.Rating")
            {
                standardValueSets.Add("AccountRating");
            }
            else if (objectFieldCombo == "Account.Type")
            {
                standardValueSets.Add("AccountType");
            }
            else if (objectFieldCombo == "Account.AccountSource"
                || objectFieldCombo == "Lead.LeadSource"
                || objectFieldCombo == "Opportunity.Source")
            {
                standardValueSets.Add("LeadSource");
            }
            else if (objectFieldCombo == "Case.Origin")
            {
                standardValueSets.Add("CaseOrigin");
            }
            else if (objectFieldCombo == "Case.Priority")
            {
                standardValueSets.Add("CasePriority");
            }
            else if (objectFieldCombo == "Case.Reason")
            {
                standardValueSets.Add("CaseReason");
            }
            else if (objectFieldCombo == "Case.Status")
            {
                standardValueSets.Add("CaseStatus");
            }
            else if (objectFieldCombo == "Case.Type")
            {
                standardValueSets.Add("CaseType");
            }
            else if (objectFieldCombo == "Lead.Status")
            {
                standardValueSets.Add("LeadStatus");
            }
            else if (objectFieldCombo == "Opportunity.StageName")
            {
                standardValueSets.Add("OpportunityStage");
            }
            else if (objectFieldCombo == "Opportunity.Type")
            {
                standardValueSets.Add("OpportunityType");
            }
            else if (objectFieldCombo == "Product2.Family")
            {
                standardValueSets.Add("Product2Family");
            }
        }
        private void selectObjectFieldsFromLayout(String objectName, String xmlDocument)
        {
            HashSet<String> objectFields = new HashSet<string>();

            XmlDocument xd = new XmlDocument();
            xd.LoadXml(xmlDocument);
            XmlNodeList nodeList = xd.GetElementsByTagName("field");
            foreach (XmlNode nd in nodeList)
            {
                if (nd.Name == "field")
                {
                    objectFields.Add(nd.InnerText);
                }
            }

            foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
            {
                if (tnd1.Text == "objects")
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (tnd2.Text == objectName + ".object")
                        {
                            foreach (TreeNode tnd3 in tnd2.Nodes)
                            {
                                if (tnd3.Text.StartsWith("<fields"))
                                {
                                    String fieldXmlDoc = "<document>" + tnd3.Text + "</document>";
                                    XmlDocument fieldXd = new XmlDocument();
                                    fieldXd.LoadXml(fieldXmlDoc);

                                    if (objectFields.Contains(fieldXd.ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText))
                                    {
                                        tnd3.Checked = true;

                                        if (objectName.EndsWith("__c"))
                                        {
                                            selectRequiredObjectFields(objectName + ".object");
                                        }

                                        // If a standard value set is available on the page layout, make sure to include it in the HashSet to add to the deployment package
                                        String tnd3XmlString = "<document>" + tnd3.Text + "</document>";
                                        XmlDocument tnd3Xd = new XmlDocument();
                                        tnd3Xd.LoadXml(tnd3XmlString);

                                        String objectFieldCombo = objectName + "." + tnd3Xd.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;
                                        populateStandardValueSetHashSet(objectFieldCombo);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private void selectStandardValueSets(HashSet<String> standardValueSets)
        {
            foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
            {
                if (tnd1.Text == "standardValueSets")
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        String[] objectNameSplit = tnd2.Text.Split('.');
                        if (standardValueSets.Contains(objectNameSplit[0]))
                        {
                            tnd2.Checked = true;

                            foreach (TreeNode tnd3 in tnd2.Nodes)
                            {
                                tnd3.Checked = true;
                            }
                        }
                    }
                }
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            populateTreeView();
        }

        // Create a zip file with package.xml to deploy to the org
        private void btnDeployToOrg_Click(object sender, EventArgs e)
        {
            if (this.tbDeployFrom.Text == "")
            {
                MessageBox.Show("Please select a folder to save the zip file and deploy from first");
                return;
            }

            if (String.IsNullOrEmpty(this.cmbUserName.Text))
            {
                MessageBox.Show("Please select a Username first and then try the deployment again");
                return;
            }

            String zipFilePath = buildZipFileWithPackageXml();

            DeployMetadata dm = new DeployMetadata();
            dm.cmbUserName.SelectedItem = this.cmbUserName.Text;
            dm.tbZipFileLocation.Text = zipFilePath;
            dm.cbCheckOnly.CheckState = CheckState.Unchecked;
            dm.cbCheckOnly.Checked = false;

            dm.Show();
        }

        public String buildZipFileWithPackageXml()
        {
            String codeArchiveRootPath = this.tbRootFolder.Text + "\\Code Archive";
            String logFile = this.tbRootFolder.Text + "\\Code Archive\\LogFile.txt";

            if (!Directory.Exists(codeArchiveRootPath))
            {
                Directory.CreateDirectory(codeArchiveRootPath);
            }

            Dictionary<String, HashSet<String>> packageXml = new Dictionary<String, HashSet<String>>();
            HashSet<String> directoryFilesDeleted = new HashSet<String>();

            DateTime dtt = DateTime.Now;
            String directoryName = dtt.Year + "_" + dtt.Month + "_" + dtt.Day + "_" + dtt.Hour + "_" + dtt.Minute + "_" + dtt.Second + "_" + dtt.Millisecond;
            String folderPath = this.tbDeployFrom.Text + "\\" + directoryName;

            DirectoryInfo cdDi = Directory.CreateDirectory(folderPath);

            // We want to track the directory and files which will be deployed so we can build the package.xml properly
            List<String> filesDeployed = new List<string>();
            foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
            {
                if (tnd1.Nodes.Count > 0)
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (tnd2.Checked == true)
                        {
                            String[] nodeFullPath = tnd2.FullPath.Split('\\');
                            filesDeployed.Add(tnd1.Text + "\\" + tnd2.Text);

                            DirectoryInfo di;
                            if (!Directory.Exists(folderPath + "\\" + tnd1.Text))
                            {
                                di = Directory.CreateDirectory(folderPath + "\\" + tnd1.Text);
                            }
                            else
                            {
                                di = new DirectoryInfo(folderPath + "\\" + tnd1.Text);
                            }

                            // Copy the directory
                            if (tnd1.Text == "aura" || tnd1.Text == "lwc")
                            {
                                UtilityClass.copyDirectory(this.tbProjectFolder.Text + "\\" + tnd1.Text + "\\" + nodeFullPath[1],
                                                           folderPath + "\\" + tnd1.Text + "\\" + nodeFullPath[1],
                                                           true);

                                if (packageXml.ContainsKey(tnd1.Text))
                                {
                                    packageXml[tnd1.Text].Add(nodeFullPath[1]);
                                }
                                else
                                {
                                    packageXml.Add(tnd1.Text, new HashSet<String> { nodeFullPath[1] });
                                }
                            }
                            else if (tnd1.Text == "objects" || tnd1.Text == "objectTranslations")
                            {
                                // Create the file and write the selected values to the file
                                //Debug.Write("tnd1.Text == \"objects\" || tnd1.Text == \"objectTranslations\"");
                                StreamWriter objSw = new StreamWriter(di.FullName + "\\" + nodeFullPath[1]);

                                objSw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                                objSw.WriteLine("<CustomObject xmlns=\"http://soap.sforce.com/2006/04/metadata\">");

                                foreach (TreeNode tnd3 in tnd2.Nodes)
                                {
                                    if (tnd3.Checked == true)
                                    {
                                        objSw.WriteLine(tnd3.Text);
                                    }
                                }

                                objSw.WriteLine("</CustomObject>");
                                objSw.Close();
                            }
                            else if (tnd1.Text == "profiles")
                            {
                                //Debug.Write("tnd1.Text == \"profiles\"");
                            }
                            else if (tnd1.Text == "permissionsets")
                            {
                                //Debug.Write("tnd1.Text == \"permissionsets\"");
                            }
                            else if (tnd1.Text == "reports")
                            {
                                //Debug.Write("tnd1.Text == \"reports\"");
                            }
                            else
                            {
                                File.Copy(this.tbProjectFolder.Text + "\\" + tnd1.Text + "\\" + nodeFullPath[1],
                                          folderPath + "\\" + tnd1.Text + "\\" + nodeFullPath[1]);
                            }

                            // Build the packageXml dictionary for writing out the actual package.xml file
                            if (!tnd2.Text.EndsWith("-meta.xml")
                                && packageXml.ContainsKey(tnd1.Text))
                            {
                                packageXml[tnd1.Text].Add(nodeFullPath[1]);
                            }
                            else if(!tnd2.Text.EndsWith("-meta.xml"))
                            {
                                packageXml.Add(tnd1.Text, new HashSet<String> { nodeFullPath[1] });
                            }
                        }
                    }
                }
            }

            // Write out the package.xml file and then build the zip file
            buildPackageXmlFile(packageXml, folderPath);

            // Zip up the contents of the folders and package.xml file
            String zipPathAndName = zipUpContents(packageXml, folderPath);

            StreamWriter sw = new StreamWriter(logFile, true);
            foreach (String objName in filesDeployed)
            {
                sw.Write(objName + "\t" +
                         dtt.Year.ToString() + "\t" +
                         dtt.Month.ToString() + "\t" +
                         dtt.Day.ToString() + "\t" +
                         dtt.Hour.ToString() + "\t" +
                         dtt.Minute.ToString() + "\t" +
                         dtt.Second.ToString() + "\t" +
                         "Deployed" + Environment.NewLine);
            }
            sw.Close();

            return zipPathAndName;
        }

        private void buildPackageXmlFile(Dictionary<String, HashSet<String>> packageXml, String folderPath)
        {
            StreamWriter sw = new StreamWriter(folderPath + "\\package.xml", false);

            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sw.WriteLine("<Package xmlns=\"http://soap.sforce.com/2006/04/metadata\">");

            foreach (String typeName in packageXml.Keys)
            {
                sw.WriteLine("<types>");

                foreach (String memberName in packageXml[typeName])
                {
                    String[] objName = memberName.Split('.');

                    sw.WriteLine("<members>" + objName[0] + "</members>");
                }

                sw.WriteLine("<name>" + MetadataDifferenceProcessing.folderToType(typeName, "") + "</name>");
                sw.WriteLine("</types>");
            }

            sw.WriteLine("<version>" + Properties.Settings.Default.DefaultAPI + "</version>");
            sw.WriteLine("</Package>");

            sw.Close();
        }

        private String zipUpContents(Dictionary<String, HashSet<String>> packageXml, String folderPath)
        {
            String[] folderPathSplit = folderPath.Split('\\');

            String zipFileName = folderPathSplit[folderPathSplit.Length - 1] + ".zip";
            String zipPathAndName = this.tbDeployFrom.Text + "\\" + zipFileName;

            ZipFile.CreateFromDirectory(folderPath, zipPathAndName, CompressionLevel.Fastest, false);

            return zipPathAndName;
        }

        private void btnObjectFieldInspector_Click(object sender, EventArgs e)
        {
            ObjectFieldInspector ofi = new ObjectFieldInspector();
            ofi.cmbUserName.SelectedItem = this.cmbUserName.Text;

            ofi.Show();
        }

        private void btnRetrieveFromOrg_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.cmbUserName.Text))
            {
                MessageBox.Show("Please select a Username first and then try the deployment again");
                return;
            }

            Dictionary<String, List<String>> packageXmlMembers = new Dictionary<string, List<string>>();

            // Loop through the treeview to obtain the name and members values
            foreach (TreeNode tn1 in this.treeViewMetadata.Nodes)
            {
                String typeName = MetadataDifferenceProcessing.folderToType(tn1.Text, "");

                foreach (TreeNode tn2 in tn1.Nodes)
                {
                    // Classes, Triggers will need to be handled differently
                    String[] tn2TextSplit = tn2.Text.Split('.');

                    if (tn2.Checked == true)
                    {
                        if (packageXmlMembers.ContainsKey(typeName))
                        {
                            if (!packageXmlMembers[typeName].Contains(tn2TextSplit[0]))
                            {
                                packageXmlMembers[typeName].Add(tn2TextSplit[0]);
                            }
                        }
                        else
                        {
                            packageXmlMembers.Add(typeName, new List<string> { tn2TextSplit[0] });
                        }
                    }
                }
            }

            if (packageXmlMembers.Count > 0
                && this.tbRootFolder.Text != null)
            {
                SalesforceMetadataStep2 sfMetadataStep2 = new SalesforceMetadataStep2();
                sfMetadataStep2.userName = this.cmbUserName.Text;
                sfMetadataStep2.selectedItems = packageXmlMembers;
                sfMetadataStep2.tbFromOrgSaveLocation.Text = this.tbProjectFolder.Text;

                Action act = () => sfMetadataStep2.requestZipFile(UtilityClass.REQUESTINGORG.FROMORG, this.tbRootFolder.Text, false, true, sfMetadataStep2);
                Task tsk = Task.Run(act);
            }
        }

        private void projectSolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bypassTextChange = true;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "sln files (*.sln)|*.sln|All Files (*.*)|*.*";
            ofd.Title = "Please select a Project/Solution file";
            ofd.ShowDialog();

            if (ofd.FileName.Length > 0)
            {
                Properties.Settings.Default.IDEProjectRoot = ofd.FileName;

                StreamReader sr = new StreamReader(ofd.FileName);

                String username = sr.ReadLine();
                if (this.cmbUserName.Items.Contains(username))
                {
                    this.cmbUserName.Text = username;
                }
                
                this.tbProjectFolder.Text = sr.ReadLine();
                this.tbDeployFrom.Text = sr.ReadLine();
                this.tbRepository.Text = sr.ReadLine();
                this.tbRootFolder.Text = sr.ReadLine();

                Properties.Settings.Default.DevelopmentSelectedFolder = this.tbProjectFolder.Text;
                Properties.Settings.Default.DevelopmentDeploymentFolder = this.tbDeployFrom.Text;
                Properties.Settings.Default.RepositoryPath = this.tbRepository.Text;
                Properties.Settings.Default.IDEProjectRoot = this.tbRootFolder.Text;

                //if (!Properties.Settings.Default.RecentProjects.Contains(ofd.FileName))
                //{
                //    //Properties.Settings.Default.RecentProjects.Add(ofd.FileName);
                //    //this.recentToolStripMenuItem.
                //}

                Properties.Settings.Default.Save();

                sr.Close();
            }

            populateTreeView();

            bypassTextChange = false;
        }

        private void tbProjectFolder_DoubleClick(object sender, EventArgs e)
        {
            String selectedPath = UtilityClass.folderBrowserSelectPath("Select Your Project Folder",
                                                                       false,
                                                                       FolderEnum.SaveTo,
                                                                       Properties.Settings.Default.DevelopmentSelectedFolder);

            if (selectedPath != "")
            {
                this.tbProjectFolder.Text = selectedPath;
                Properties.Settings.Default.DevelopmentSelectedFolder = selectedPath;
                Properties.Settings.Default.Save();

                this.projectValuesChanged = true;

                populateTreeView();
            }
        }

        private void tbProjectFolder_TextChanged(object sender, EventArgs e)
        {
            if (bypassTextChange == true) return;

            Properties.Settings.Default.DevelopmentSelectedFolder = this.tbProjectFolder.Text;
            Properties.Settings.Default.Save();

            this.projectValuesChanged = true;

            populateTreeView();
        }

        private void tbDeployFrom_DoubleClick(object sender, EventArgs e)
        {
            String selectedPath = UtilityClass.folderBrowserSelectPath("Select The Deploy From Folder",
                                                                       true,
                                                                       FolderEnum.SaveTo,
                                                                       Properties.Settings.Default.DevelopmentDeploymentFolder);

            if (selectedPath != "")
            {
                this.tbDeployFrom.Text = selectedPath;
                Properties.Settings.Default.DevelopmentDeploymentFolder = selectedPath;
                Properties.Settings.Default.Save();

                this.projectValuesChanged = true;
            }
        }

        private void tbDeployFrom_TextChanged(object sender, EventArgs e)
        {
            if (bypassTextChange == true) return;

            Properties.Settings.Default.DevelopmentDeploymentFolder = this.tbDeployFrom.Text;
            Properties.Settings.Default.Save();
        }

        private void tbRepository_DoubleClick(object sender, EventArgs e)
        {
            String selectedPath = UtilityClass.folderBrowserSelectPath("Select the path to the Repository",
                                                                       false,
                                                                       FolderEnum.SaveTo,
                                                                       Properties.Settings.Default.RepositoryPath);

            if (selectedPath != "")
            {
                this.tbRepository.Text = selectedPath;
                Properties.Settings.Default.RepositoryPath = selectedPath;
                Properties.Settings.Default.Save();

                this.projectValuesChanged = true;
            }
        }

        private void tbRootFolder_DoubleClick(object sender, EventArgs e)
        {
            String selectedPath = UtilityClass.folderBrowserSelectPath("Select the path to the Project\'s Root Folder",
                                                                       false,
                                                                       FolderEnum.SaveTo,
                                                                       Properties.Settings.Default.IDEProjectRoot);

            if (selectedPath != "")
            {
                this.tbRootFolder.Text = selectedPath;
                Properties.Settings.Default.IDEProjectRoot = selectedPath;
                Properties.Settings.Default.Save();

                this.projectValuesChanged = true;
            }
        }

        private void treeViewMetadata_DoubleClick(object sender, EventArgs e)
        {
            System.Windows.Forms.MouseEventArgs evtArgs = (System.Windows.Forms.MouseEventArgs)e;

            if (evtArgs.Button == MouseButtons.Left)
            {
                TreeView tv = (TreeView)sender;

                if (tv.SelectedNode != null)
                {
                    String pathToFile = "\"" + this.tbProjectFolder.Text + "\\" + tv.SelectedNode.FullPath;

                    checkArchiveDirectory(this.tbProjectFolder.Text + "\\" + tv.SelectedNode.FullPath);

                    if (Properties.Settings.Default.DefaultTextEditorPath == "")
                    {
                        Process.Start(@"notepad.exe", pathToFile);
                    }
                    else
                    {
                        Process.Start(@Properties.Settings.Default.DefaultTextEditorPath, pathToFile);
                    }
                }
            }
        }

        // Check if Code Archive directory exists
        private void checkArchiveDirectory(String fileToCopy)
        {
            String[] fileToCopySplit = fileToCopy.Split('\\');
            String codeArchiveRootPath = this.tbRootFolder.Text + "\\Code Archive";
            String logFile = this.tbRootFolder.Text + "\\Code Archive\\LogFile.txt";

            // Confirm if Directory exists
            if (!Directory.Exists(codeArchiveRootPath))
            {
                Directory.CreateDirectory(codeArchiveRootPath);
            }

            String[] fileNameSplit = fileToCopySplit[fileToCopySplit.Length - 1].Split('.');
            String codeArchiveFolder = "";

            if (fileToCopySplit[fileToCopySplit.Length - 3] == "aura" || fileToCopySplit[fileToCopySplit.Length - 3] == "lwc")
            {
                codeArchiveFolder = codeArchiveRootPath + "\\" + fileToCopySplit[fileToCopySplit.Length - 3] + "\\" + fileToCopySplit[fileToCopySplit.Length - 2];
            }
            else
            {
                codeArchiveFolder = codeArchiveRootPath + "\\" + fileToCopySplit[fileToCopySplit.Length - 2];
            }

            if (codeArchiveFolder != "" && !Directory.Exists(codeArchiveFolder))
            {
                Directory.CreateDirectory(codeArchiveFolder);
            }

            DateTime currDtTime = DateTime.Now;
            String copiedFileName = codeArchiveFolder + "\\" + fileNameSplit[0] + "_" +
                                                               currDtTime.Year.ToString() + "_" +
                                                               currDtTime.Month.ToString() + "_" +
                                                               currDtTime.Day.ToString() + "_" +
                                                               currDtTime.Hour.ToString() + "_" +
                                                               currDtTime.Minute.ToString() + "_" +
                                                               currDtTime.Second.ToString() + "." + fileNameSplit[1];

            File.Copy(fileToCopy, copiedFileName);

            StreamWriter sw = new StreamWriter(logFile, true);
            sw.Write(fileNameSplit[0] + "\t" +
                     fileNameSplit[1] + "\t" +
                     currDtTime.Year.ToString() + "\t" +
                     currDtTime.Month.ToString() + "\t" +
                     currDtTime.Day.ToString() + "\t" +
                     currDtTime.Hour.ToString() + "\t" +
                     currDtTime.Minute.ToString() + "\t" +
                     currDtTime.Second.ToString() + "\t" +
                     "Opened" + Environment.NewLine);
            sw.Close();

        }
        private void btnBuildERD_Click(object sender, EventArgs e)
        {

        }

        private void updateProjectFile()
        {
            if (Properties.Settings.Default.IDEProjectRoot != null)
            {
                StreamWriter sw = new StreamWriter(Properties.Settings.Default.IDEProjectRoot + "\\project.sln", false);

                sw.WriteLine(this.cmbUserName.Text);
                sw.WriteLine(Properties.Settings.Default.DevelopmentSelectedFolder);
                sw.WriteLine(Properties.Settings.Default.DevelopmentDeploymentFolder);
                sw.WriteLine(Properties.Settings.Default.RepositoryPath);
                sw.WriteLine(Properties.Settings.Default.IDEProjectRoot);

                sw.Close();
            }
            else
            {
                MessageBox.Show("Please select a Root folder to save your project/solution in, then save again");
            }
        }

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.cmbUserName.Text = "";
            this.tbDeployFrom.Text = "";
            this.tbProjectFolder.Text = "";
            this.tbRepository.Text = "";
            this.tbRootFolder.Text = "";

            Properties.Settings.Default.DevelopmentSelectedFolder = "";
            Properties.Settings.Default.DevelopmentDeploymentFolder = "";
            Properties.Settings.Default.RepositoryPath = "";
            Properties.Settings.Default.IDEProjectRoot = "";

            Properties.Settings.Default.Save();
        }

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            updateProjectFile();
        }

        private void DevelopmentEnvironment_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.projectValuesChanged == true)
            {
                DialogResult mbDr = MessageBox.Show("Would you like to save your changes to the Project/Solution file?", "Save Project Settings", MessageBoxButtons.YesNo);
                if (mbDr.Equals(DialogResult.Yes))
                {
                    if (this.tbRootFolder.Text != "")
                    {
                        updateProjectFile();
                    }
                    else
                    {
                        tbRootFolder_DoubleClick(null, null);
                        updateProjectFile();
                    }
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void AddObject_ObjectAdded(object sender, AddObjectEvent e)
        {
            if (e.refreshParentForm == true)
            {
                this.runTreeNodeSelector = false;

                // Add the new nodes to the tree view with a default of checked so the deployment function can pick up on them.
                foreach (TreeNode tn1 in this.treeViewMetadata.Nodes)
                {
                    if (tn1.Text == e.nodeType)
                    {
                        for (Int32 i = 0; i < e.filesCreated.Count(); i++)
                        {
                            TreeNode tn2 = tn1.Nodes.Add(e.filesCreated[i]);
                            tn2.Checked = true;
                        }
                    }
                }

                this.runTreeNodeSelector = true;
            }
        }
        private void btnSearchMetadata_Click(object sender, EventArgs e)
        {
            if (this.tbProjectFolder.Text == "")
            {
                MessageBox.Show("Please select a project folder to search first");
            }
            else
            {
                frmSearch srch = new frmSearch();
                srch.tbSearchLocation.Text = this.tbProjectFolder.Text;
                srch.Show();
            }
        }

        private void btnDebugLogs_Click(object sender, EventArgs e)
        {
            ParseDebugLogs pdl = new ParseDebugLogs();
            pdl.tbDebugFile.Text = Properties.Settings.Default.DebugLogPath;
            pdl.Show();
        }

        private void treeViewMetadata_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MouseEventArgs evtArgs = (System.Windows.Forms.MouseEventArgs)e;
            if (evtArgs.Button == MouseButtons.Right)
            {
                this.treeViewMetadata.SelectedNode = this.treeViewMetadata.GetNodeAt(evtArgs.X, evtArgs.Y);


            }
        }

        private void addApexTriggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.tbProjectFolder.Text == "")
            {
                MessageBox.Show("Please select a Project Solution / Folder before adding new objects");
            }
            else
            {
                AddObject ao = new AddObject();
                ao.projectFolderPath = this.tbProjectFolder.Text;
                ao.loadSobjectsToCombobox();
                ao.ObjectAdded += AddObject_ObjectAdded;
                ao.Show();
            }
        }

        private void addApexClassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.tbProjectFolder.Text == "")
            {
                MessageBox.Show("Please select a Project Solution / Folder before adding new objects");
            }
            else
            {
                AddObject ao = new AddObject();
                ao.projectFolderPath = this.tbProjectFolder.Text;
                ao.loadSobjectsToCombobox();
                ao.ObjectAdded += AddObject_ObjectAdded;
                ao.Show();
            }
        }

        private void addLightningWebComponentToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void customObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void visualforcePageToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void visualforceComponentToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void delMyDebugLogs_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.cmbUserName.Text))
            {
                MessageBox.Show("Please select a username and enter the password first before continuing");
                return;
            }

            Boolean loginSuccess = SalesforceCredentials.salesforceLogin(UtilityClass.REQUESTINGORG.FROMORG, this.cmbUserName.Text);
            if (loginSuccess == false)
            {
                MessageBox.Show("Please check username, password and/or security token");
                return;
            }

            String selectStatement = "";
            String userId = "";
            userId = SalesforceCredentials.fromOrgLR.userId;

            selectStatement = "SELECT Id FROM ApexLog WHERE LogUserId = \'" + userId + "\'";

            SalesforceMetadata.PartnerWSDL.QueryResult qr = new SalesforceMetadata.PartnerWSDL.QueryResult();

            try
            {
                qr = SalesforceCredentials.fromOrgSS.query(selectStatement);

                if (qr.size > 2000)
                {
                    Boolean done = false;

                    while (!done)
                    {
                        SalesforceMetadata.PartnerWSDL.sObject[] sobjRecords = qr.records;

                        Dictionary<Int32, List<String>> recordIdsToDelete = new Dictionary<Int32, List<String>>();

                        Int32 i = 0;
                        List<String> rtds = new List<String>();

                        if (sobjRecords == null)
                        {

                        }
                        else
                        {
                            foreach (SalesforceMetadata.PartnerWSDL.sObject s in sobjRecords)
                            {
                                if (rtds.Count < 200)
                                {
                                    rtds.Add(s.Id);
                                }
                                else
                                {
                                    recordIdsToDelete.Add(i, rtds);
                                    rtds = new List<String>();
                                    rtds.Add(s.Id);

                                    i++;
                                }
                            }

                            if (rtds.Count > 0)
                            {
                                recordIdsToDelete.Add(i, rtds);
                                rtds = new List<String>();
                            }

                            foreach (Int32 rtd in recordIdsToDelete.Keys)
                            {
                                if (recordIdsToDelete[rtd].Count > 0)
                                {
                                    PartnerWSDL.DeleteResult[] dr = SalesforceCredentials.fromOrgSS.delete(recordIdsToDelete[rtd].ToArray());
                                }
                            }

                            if (!qr.done)
                            {
                                qr = SalesforceCredentials.fromOrgSS.queryMore(qr.queryLocator);
                            }
                            else
                            {
                                done = true;
                            }
                        }
                    }
                }
                else if (qr.records != null)
                {
                    SalesforceMetadata.PartnerWSDL.sObject[] sobjRecords = qr.records;
                    Dictionary<Int32, List<String>> recordIdsToDelete = new Dictionary<Int32, List<String>>();

                    Int32 i = 0;
                    List<String> rtds = new List<String>();
                    foreach (SalesforceMetadata.PartnerWSDL.sObject s in sobjRecords)
                    {
                        if (rtds.Count < 200)
                        {
                            rtds.Add(s.Id);
                        }
                        else
                        {
                            recordIdsToDelete.Add(i, rtds);
                            rtds = new List<String>();
                            rtds.Add(s.Id);

                            i++;
                        }
                    }

                    if (rtds.Count > 0)
                    {
                        recordIdsToDelete.Add(i, rtds);
                        rtds = new List<String>();
                    }

                    foreach (Int32 rtd in recordIdsToDelete.Keys)
                    {
                        if (recordIdsToDelete[rtd].Count > 0)
                        {
                            PartnerWSDL.DeleteResult[] dr = SalesforceCredentials.fromOrgSS.delete(recordIdsToDelete[rtd].ToArray());
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

            MessageBox.Show("Debug log delete process completed");
        }

        private void cmbUserName_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Text = "DevelopmentEnvironment";

            if (SalesforceCredentials.isProduction[this.cmbUserName.Text] == true)
            {
                this.Text = "DevelopmentEnvironment - PRODUCTION";
            }
            else
            {
                String[] userNamesplit = this.cmbUserName.Text.Split('.');
                String orgName = userNamesplit[userNamesplit.Length - 1].ToUpper();
                this.Text = "DevelopmentEnvironment - " + orgName;
            }
        }
    }
}
