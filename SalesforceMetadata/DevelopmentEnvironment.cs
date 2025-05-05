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

namespace SalesforceMetadata
{
    public partial class DevelopmentEnvironment : Form
    {
        private SalesforceCredentials sc;

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
            sc = new SalesforceCredentials();
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

            populateUserNames();
        }
        private void populateUserNames()
        {
            foreach (String un in sc.usernamePartnerUrl.Keys)
            {
                this.cmbUserName.Items.Add(un);
            }
        }
        public void populateTreeView()
        {
            if (Directory.Exists(this.tbProjectFolder.Text))
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

                                // Check for a __test__ directory
                                String[] testDirs = Directory.GetDirectories(subFolder);
                                if (testDirs.Length > 0)
                                {
                                    String[] testDirsSplit = testDirs[0].Split('\\');
                                    TreeNode tnd3 = new TreeNode(testDirsSplit[testDirsSplit.Length - 1]);

                                    String[] testDirFiles = Directory.GetFiles(testDirs[0]);
                                    foreach (String testFile in testDirFiles)
                                    {
                                        String[] testFileSplit = testFile.Split('\\');

                                        TreeNode tnd4 = new TreeNode(testFileSplit[testFileSplit.Length - 1]);
                                        tnd3.Nodes.Add(tnd4);
                                    }

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
                        Process proc = Process.Start(@"notepad.exe", pathToFile);
                    }
                    else
                    {
                        Process proc = Process.Start(@Properties.Settings.Default.DefaultTextEditorPath, pathToFile);
                    }
                }
            }
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
                            if (tnd2.Text == fileNameSplit[0] + ".crt"
                                || tnd2.Text == fileNameSplit[0] + ".crt-meta.xml")
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
                            if (tnd2.Text == fileNameSplit[0] + ".cls"
                                || tnd2.Text == fileNameSplit[0] + ".cls-meta.xml")
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
                            if (tnd2.Text == fileNameSplit[0] + ".component"
                                || tnd2.Text == fileNameSplit[0] + ".component-meta.xml")
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
                            if (tnd2.Text == fileNameSplit[0] + ".asset"
                                || tnd2.Text == fileNameSplit[0] + ".asset-meta.xml")
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
                            if (tnd2.Text == fileNameSplit[0] + ".page"
                                || tnd2.Text == fileNameSplit[0] + ".page-meta.xml")
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
                            if (tnd2.Text == fileNameSplit[0] + ".resource"
                                || tnd2.Text == fileNameSplit[0] + ".resource-meta.xml")
                            {
                                tnd2.Checked = true;
                                tnd1.Checked = true;
                            }
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
                            if (tnd2.Text == fileNameSplit[0] + ".trigger"
                                || tnd2.Text == fileNameSplit[0] + ".trigger-meta.xml")
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

            this.copySelectedToRepository();

            String zipFilePath = buildZipFileWithPackageXml();

            DeployMetadata dm = new DeployMetadata();
            dm.cmbUserName.SelectedItem = this.cmbUserName.Text;
            dm.tbZipFileLocation.Text = zipFilePath;
            dm.cbCheckOnly.CheckState = CheckState.Unchecked;
            dm.cbCheckOnly.Checked = false;

            dm.Show();
        }

        private String buildZipFileWithPackageXml()
        {
            String codeArchiveRootPath = this.tbProjectFile.Text + "\\Code Archive";
            String logFile = this.tbProjectFile.Text + "\\Code Archive\\LogFile.txt";

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
                String metadataType = MetadataDifferenceProcessing.folderToType(tnd1.Text, "");

                if (tnd1.Nodes.Count > 0)
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (tnd2.Checked == true)
                        {
                            String[] tnd2NodeFullPath = tnd2.FullPath.Split('\\');
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
                            if (metadataType == "AuraDefinitionBundle" || metadataType == "LightningComponentBundle")
                            {
                                UtilityClass.copyDirectory(this.tbProjectFolder.Text + "\\" + tnd1.Text + "\\" + tnd2NodeFullPath[1],
                                                           folderPath + "\\" + tnd1.Text + "\\" + tnd2NodeFullPath[1],
                                                           true);

                                if (packageXml.ContainsKey(metadataType))
                                {
                                    packageXml[metadataType].Add(tnd2NodeFullPath[1]);
                                }
                                else
                                {
                                    packageXml.Add(metadataType, new HashSet<String> { tnd2NodeFullPath[1] });
                                }
                            }
                            else if (metadataType == "CustomMetadata")
                            {
                                // Loop through the child nodes, get the CMT names and then add an .md before copying to the deployment folder
                                if (tnd2.Checked == true)
                                {
                                    foreach (TreeNode tnd3 in tnd2.Nodes)
                                    {
                                        if (tnd3.Checked == true)
                                        {
                                            String[] cmtRecordSplit = tnd3.Text.Split('.');

                                            File.Copy(this.tbProjectFolder.Text + "\\" + tnd1.Text + "\\" + tnd2.Text + "." + tnd3.Text,
                                                folderPath + "\\" + tnd1.Text + "\\" + tnd2.Text + "." + tnd3.Text);

                                            if (packageXml.ContainsKey(metadataType))
                                            {
                                                packageXml[metadataType].Add(tnd2.Text + '.' + cmtRecordSplit[0]);
                                            }
                                            else
                                            {
                                                packageXml.Add(metadataType, new HashSet<string> { tnd2.Text + '.' + cmtRecordSplit[0] });
                                            }
                                        }
                                    }
                                }
                            }
                            else if (metadataType == "CustomObject" || metadataType == "CustomObjectTranslation")
                            {
                                // Create the file and write the selected values to the file

                                StreamWriter objSw = new StreamWriter(di.FullName + "\\" + tnd2NodeFullPath[1]);

                                objSw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                                objSw.WriteLine("<CustomObject xmlns=\"http://soap.sforce.com/2006/04/metadata\">");

                                foreach (TreeNode tnd3 in tnd2.Nodes)
                                {
                                    if (tnd3.Checked == true)
                                    {
                                        objSw.WriteLine(tnd3.Text);

                                        String[] tnd3NodeFullPath = tnd3.FullPath.Split('\\');
                                        directoryName = tnd3NodeFullPath[0];

                                        String[] objectNameSplit = tnd3NodeFullPath[1].Split('.');

                                        //String parentNode = MetadataDifferenceProcessing.folderToType(tnd3NodeFullPath[0], "");

                                        // Add the custom field to the dictionary
                                        if (tnd3NodeFullPath.Length == 3)
                                        {
                                            if (tnd3NodeFullPath[0] == "objects"
                                                && tnd3NodeFullPath[2].StartsWith("<fields"))
                                            {
                                                String xmlString = "<document>" + tnd3NodeFullPath[2] + "</document>";
                                                XmlDocument xd = new XmlDocument();
                                                xd.LoadXml(xmlString);

                                                String objectFieldCombo = objectNameSplit[0] + "." + xd.ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;

                                                // Add the custom field to the packagexml dictionary
                                                if (packageXml.ContainsKey("CustomField"))
                                                {
                                                    packageXml["CustomField"].Add(objectFieldCombo);
                                                }
                                                else
                                                {
                                                    packageXml.Add("CustomField", new HashSet<string> { objectFieldCombo });
                                                }

                                                // Add the custom object to the packagexml dictionary
                                                if (packageXml.ContainsKey(metadataType))
                                                {
                                                    packageXml[metadataType].Add(objectNameSplit[0]);
                                                }
                                                else
                                                {
                                                    packageXml.Add(metadataType, new HashSet<string> { objectNameSplit[0] });
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (packageXml.ContainsKey(metadataType))
                                            {
                                                packageXml[metadataType].Add(objectNameSplit[0]);
                                            }
                                            else
                                            {
                                                packageXml.Add(metadataType, new HashSet<string> { objectNameSplit[0] });
                                            }
                                        }
                                    }
                                }

                                objSw.WriteLine("</CustomObject>");
                                objSw.Close();
                            }
                            else if (metadataType == "Profile")
                            {
                                //Debug.Write("tnd1.Text == \"profiles\"");
                            }
                            else if (metadataType == "PermissionSet")
                            {
                                //Debug.Write("tnd1.Text == \"permissionsets\"");
                            }
                            else if (metadataType == "Report")
                            {
                                //Debug.Write("tnd1.Text == \"reports\"");
                            }
                            else
                            {
                                File.Copy(this.tbProjectFolder.Text + "\\" + tnd1.Text + "\\" + tnd2NodeFullPath[1],
                                          folderPath + "\\" + tnd1.Text + "\\" + tnd2NodeFullPath[1]);


                                String[] objectNameSplit = tnd2NodeFullPath[1].Split('.');

                                if (metadataType == "ApprovalProcess")
                                {
                                    if (packageXml.ContainsKey(metadataType))
                                    {
                                        packageXml[metadataType].Add(objectNameSplit[0] + "." + objectNameSplit[1]);
                                    }
                                    else
                                    {
                                        packageXml.Add(metadataType, new HashSet<string> { objectNameSplit[0] + "." + objectNameSplit[1] });
                                    }
                                }
                                else if (metadataType == "ApexClass")
                                {
                                    if (packageXml.ContainsKey(metadataType)
                                        && !tnd2.Text.EndsWith("-meta.xml"))
                                    {
                                        packageXml[metadataType].Add(objectNameSplit[0]);
                                    }
                                    else if (!tnd2.Text.EndsWith("-meta.xml"))
                                    {
                                        packageXml.Add(metadataType, new HashSet<string> { objectNameSplit[0] });
                                    }
                                }
                                else if (metadataType == "ApexTrigger")
                                {
                                    if (packageXml.ContainsKey(metadataType)
                                        && !tnd2.Text.EndsWith("-meta.xml"))
                                    {
                                        packageXml[metadataType].Add(objectNameSplit[0]);
                                    }
                                    else if (!tnd2.Text.EndsWith("-meta.xml"))
                                    {
                                        packageXml.Add(metadataType, new HashSet<string> { objectNameSplit[0] });
                                    }
                                }
                                else if (metadataType == "QuickAction")
                                {
                                    if (objectNameSplit.Length == 2)
                                    {
                                        if (packageXml.ContainsKey(metadataType))
                                        {
                                            packageXml[metadataType].Add(objectNameSplit[0]);
                                        }
                                        else
                                        {
                                            packageXml.Add(metadataType, new HashSet<string> { objectNameSplit[0] });
                                        }
                                    }
                                    else if (objectNameSplit.Length == 3)
                                    {
                                        if (packageXml.ContainsKey(metadataType))
                                        {
                                            packageXml[metadataType].Add(objectNameSplit[0] + "." + objectNameSplit[1]);
                                        }
                                        else
                                        {
                                            packageXml.Add(metadataType, new HashSet<string> { objectNameSplit[0] + "." + objectNameSplit[1] });
                                        }
                                    }
                                }
                                else
                                {
                                    if (packageXml.ContainsKey(metadataType))
                                    {
                                        packageXml[metadataType].Add(objectNameSplit[0]);
                                    }
                                    else
                                    {
                                        packageXml.Add(metadataType, new HashSet<string> { objectNameSplit[0] });
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Write out the package.xml file and then build the zip file
            buildPackageXmlFile(packageXml, folderPath);

            // Zip up the contents of the folders and package.xml file
            String zipPathAndName = zipUpContents(folderPath);

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
                    sw.WriteLine("<members>" + memberName + "</members>");
                }

                sw.WriteLine("<name>" + typeName + "</name>");
                sw.WriteLine("</types>");
            }

            sw.WriteLine("<version>" + Properties.Settings.Default.DefaultAPI + "</version>");

            if (this.tbOutboundChangeSetName.Text != "")
            {
                sw.WriteLine("<fullName>" + this.tbOutboundChangeSetName.Text + "</fullName>");
            }

            sw.WriteLine("</Package>");

            sw.Close();
        }

        private String zipUpContents(String folderPath)
        {
            String[] folderPathSplit = folderPath.Split('\\');

            String zipFileName = folderPathSplit[folderPathSplit.Length - 1] + ".zip";
            String zipPathAndName = this.tbDeployFrom.Text + "\\" + zipFileName;

            ZipFile.CreateFromDirectory(folderPath, zipPathAndName, CompressionLevel.Fastest, false);

            return zipPathAndName;
        }

        private void copySelectedToRepository()
        {
            // Check if the tbRepositoryPath is populated and valid first

            if(this.tbRepository.Text == "") { return; }

            String folderPath = this.tbRepository.Text;

            foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
            {
                String metadataType = MetadataDifferenceProcessing.folderToType(tnd1.Text, "");

                if (tnd1.Nodes.Count > 0)
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (tnd2.Checked == true)
                        {
                            String[] tnd2NodeFullPath = tnd2.FullPath.Split('\\');

                            DirectoryInfo di;
                            if (!Directory.Exists(folderPath + "\\" + tnd1.Text))
                            {
                                di = Directory.CreateDirectory(folderPath + "\\" + tnd1.Text);
                            }
                            else
                            {
                                di = new DirectoryInfo(folderPath + "\\" + tnd1.Text);
                            }

                            // tnd2.FullPath will be something like this: "classes\\AccOppTerritoryBatch.cls"
                            // Copy the file into the folder
                            // If it is an aura / lwc folder, then follow the same processes as you have for the buildMetadataPackage.
                            if (metadataType == "AuraDefinitionBundle" || metadataType == "LightningComponentBundle")
                            {
                                UtilityClass.copyDirectory(this.tbProjectFolder.Text + "\\" + tnd1.Text + "\\" + tnd2NodeFullPath[1],
                                                           folderPath + "\\" + tnd1.Text + "\\" + tnd2NodeFullPath[1],
                                                           true);
                            }
                            else
                            {
                                File.Copy(this.tbProjectFolder.Text + "\\" + tnd1.Text + "\\" + tnd2NodeFullPath[1],
                                          folderPath + "\\" + tnd1.Text + "\\" + tnd2NodeFullPath[1], 
                                          true);
                            }
                        }
                    }
                }
            }
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
                && this.tbProjectFile.Text != null)
            {
                SalesforceMetadataStep2 sfMetadataStep2 = new SalesforceMetadataStep2();
                sfMetadataStep2.userName = this.cmbUserName.Text;
                sfMetadataStep2.selectedItems = packageXmlMembers;
                sfMetadataStep2.tbFromOrgSaveLocation.Text = this.tbProjectFolder.Text;

                Action act = () => sfMetadataStep2.requestZipFile(UtilityClass.REQUESTINGORG.FROMORG, this.tbProjectFile.Text, sfMetadataStep2);
                Task tsk = Task.Run(act);
            }
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

            this.projectValuesChanged = true;
        }

        private void tbRepository_DoubleClick(object sender, EventArgs e)
        {
            String selectedPath = UtilityClass.folderBrowserSelectPath("Select the path to the Repository",
                                                                       true,
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

        private void tbRepository_TextChanged(object sender, EventArgs e)
        {
            if (bypassTextChange == true) return;

            Properties.Settings.Default.RepositoryPath = this.tbRepository.Text;
            Properties.Settings.Default.Save();

            this.projectValuesChanged = true;
        }

        // Check if Code Archive directory exists
        private void checkArchiveDirectory(String fileToCopy)
        {
            String[] fileToCopySplit = fileToCopy.Split('\\');
            String[] fileNameSplit = fileToCopySplit[fileToCopySplit.Length - 1].Split('.');

            String codeArchiveRootPath = this.tbProjectFile.Text + "\\Code Archive";
            String logFile = this.tbProjectFile.Text + "\\Code Archive\\LogFile.txt";

            if (fileNameSplit.Length == 1)
            {
                String[] files = Directory.GetFiles(fileToCopy);
                foreach (String file in files)
                {
                    checkArchiveDirectory(file);
                }
            }
            else
            {
                // Confirm if Directory exists
                if (!Directory.Exists(codeArchiveRootPath))
                {
                    Directory.CreateDirectory(codeArchiveRootPath);
                }

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
        }

        private void checkRepositoryForFile()
        {

        }

        private void btnBuildERD_Click(object sender, EventArgs e)
        {

        }
        private void DevelopmentEnvironment_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.projectValuesChanged == true)
            {
                DialogResult mbDr = MessageBox.Show("Would you like to save your changes to the Project/Solution file?", "Save Project Settings", MessageBoxButtons.YesNo);
                if (mbDr.Equals(DialogResult.Yes))
                {
                    updateProjectFile();
                }
            }
        }
        private void AddObject_ObjectAdded(object sender, AddObjectEvent e)
        {
            if (e.refreshParentForm == true)
            {
                this.runTreeNodeSelector = false;

                // Add the new nodes to the tree view with a default of checked so the deployment function can pick up on them.
                foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
                {
                    if (tnd1.Text == e.nodeType)
                    {
                        if (tnd1.Text == "aura")
                        {
                            break;
                        }
                        else if (tnd1.Text == "lwc")
                        {
                            TreeNode tnd2 = tnd1.Nodes.Add(e.filesCreated[0]);
                            String[] lwcfiles = Directory.GetFiles(this.tbProjectFolder.Text + "\\lwc\\" + e.filesCreated[0]);

                            foreach (String lwcfile in lwcfiles)
                            {
                                String[] fileNameSplit = lwcfile.Split('\\');
                                TreeNode tnd3 = tnd2.Nodes.Add(fileNameSplit[fileNameSplit.Length - 1]);
                                tnd3.Checked = true;
                            }

                            tnd2.Checked = true;

                            break;
                        }
                        else
                        {
                            for (Int32 i = 0; i < e.filesCreated.Count(); i++)
                            {
                                TreeNode tnd2 = tnd1.Nodes.Add(e.filesCreated[i]);
                                tnd2.Checked = true;
                            }

                            break;
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
                SearchForm srch = new SearchForm();
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
        private void delMyDebugLogs_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.cmbUserName.Text))
            {
                MessageBox.Show("Please select a username and enter the password first before continuing");
                return;
            }

            try
            {
                sc.salesforceLogin(UtilityClass.REQUESTINGORG.FROMORG, this.cmbUserName.Text);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                return;
            }

            if (sc.loginSuccess == false)
            {
                MessageBox.Show("Please check username, password and/or security token");
                return;
            }

            String selectStatement = "";
            String userId = "";
            userId = sc.fromOrgLR.userId;

            selectStatement = "SELECT Id FROM ApexLog WHERE LogUserId = \'" + userId + "\'";

            SalesforceMetadata.PartnerWSDL.QueryResult qr = new SalesforceMetadata.PartnerWSDL.QueryResult();

            try
            {
                qr = sc.fromOrgSS.query(selectStatement);

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
                                    PartnerWSDL.DeleteResult[] dr = sc.fromOrgSS.delete(recordIdsToDelete[rtd].ToArray());
                                }
                            }

                            if (!qr.done)
                            {
                                qr = sc.fromOrgSS.queryMore(qr.queryLocator);
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
                            PartnerWSDL.DeleteResult[] dr = sc.fromOrgSS.delete(recordIdsToDelete[rtd].ToArray());
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

            if (sc.isProduction[this.cmbUserName.Text] == true)
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

        private void tbOutboundChangeSetName_MouseHover(object sender, EventArgs e)
        {
            TextBox TB = (TextBox)sender;
            int VisibleTime = 10000;  //in milliseconds

            ToolTip tt = new ToolTip();
            tt.Show("If you have a open Salesforce Outbound Change Set, add the Change Set Name here. When deployed, your changes will be added to the change set.", TB, 0, 0, VisibleTime);
        }

        // Toolstrip Menu Items
        private void projectSolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bypassTextChange = true;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "sln files (*.sln)|*.sln|All Files (*.*)|*.*";
            ofd.Title = "Please select a Project/Solution file";
            ofd.ShowDialog();

            if (ofd.FileName.Length > 0)
            {
                this.loadProject(ofd.FileName);
            }
        }

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bypassTextChange = true;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "sln files (*.sln)|*.sln|All Files (*.*)|*.*";
            ofd.Title = "Please select a Project/Solution file";
            ofd.ShowDialog();

            if (ofd.FileName.Length == 0) 
            {
                MessageBox.Show("Please select a Project Solution file (*.sln)");
                return;
            }

            this.tbProjectFile.Text = ofd.FileName;
            this.cmbUserName.Text = "";
            this.tbDeployFrom.Text = "";
            this.tbProjectFolder.Text = "";
            this.tbRepository.Text = "";

            Properties.Settings.Default.ProjectFilePath = this.tbProjectFile.Text;
            Properties.Settings.Default.DevelopmentSelectedFolder = "";
            Properties.Settings.Default.DevelopmentDeploymentFolder = "";
            Properties.Settings.Default.RepositoryPath = "";
            Properties.Settings.Default.RecentProjectPath = "";

            Properties.Settings.Default.Save();
        }

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            updateProjectFile();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void loadRecentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Properties.Settings.Default.RecentProjectPath) == false)
            {
                this.loadProject(Properties.Settings.Default.RecentProjectPath);
            }
        }
        private void updateProjectFile()
        {
            if (this.tbProjectFile.Text != "")
            {
                StreamWriter sw = new StreamWriter(this.tbProjectFile.Text, false);

                sw.WriteLine(this.tbProjectFile.Text);
                sw.WriteLine(this.cmbUserName.Text);
                sw.WriteLine(Properties.Settings.Default.DevelopmentSelectedFolder);
                sw.WriteLine(Properties.Settings.Default.DevelopmentDeploymentFolder);
                sw.WriteLine(Properties.Settings.Default.RepositoryPath);

                if (this.tbOutboundChangeSetName.Text != "")
                {
                    sw.WriteLine(this.tbOutboundChangeSetName.Text);
                }
                else
                {
                    sw.WriteLine("");
                }

                sw.Close();
            }
            else
            {
                MessageBox.Show("Please select a Root folder to save your project/solution in, then save again");
            }
        }

        private void loadProject(String projectFilePath)
        {
            Properties.Settings.Default.RecentProjectPath = projectFilePath;

            StreamReader sr = new StreamReader(projectFilePath);

            this.tbProjectFile.Text = sr.ReadLine();

            String username = sr.ReadLine();
            if (this.cmbUserName.Items.Contains(username))
            {
                this.cmbUserName.Text = username;
            }

            this.tbProjectFolder.Text = sr.ReadLine();
            this.tbDeployFrom.Text = sr.ReadLine();
            this.tbRepository.Text = sr.ReadLine();

            this.tbOutboundChangeSetName.Text = sr.ReadLine();

            Properties.Settings.Default.DevelopmentSelectedFolder = this.tbProjectFolder.Text;
            Properties.Settings.Default.DevelopmentDeploymentFolder = this.tbDeployFrom.Text;
            Properties.Settings.Default.RepositoryPath = this.tbRepository.Text;
            Properties.Settings.Default.ProjectFilePath = this.tbProjectFile.Text;

            //if (!Properties.Settings.Default.RecentProjects.Contains(ofd.FileName))
            //{
            //    //Properties.Settings.Default.RecentProjects.Add(ofd.FileName);
            //    //this.recentToolStripMenuItem.
            //}

            Properties.Settings.Default.Save();

            sr.Close();

            populateTreeView();

            bypassTextChange = false;
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
                ao.tbTriggerName.Select();
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
                ao.tbClassName.Select();
                ao.ObjectAdded += AddObject_ObjectAdded;
                ao.Show();
            }
        }

        private void addLightningWebComponentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddObject ao = new AddObject();
            ao.projectFolderPath = this.tbProjectFolder.Text;
            ao.tbLWCName.Select();
            ao.ObjectAdded += AddObject_ObjectAdded;
            ao.Show();
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

        private void btnCopySelectedToRepository_Click(object sender, EventArgs e)
        {
            this.copySelectedToRepository();
        }


    }
}
