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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SalesforceMetadata
{
    public partial class DevelopmentEnvironment : Form
    {
        private SalesforceCredentials devEnvSFCreds;

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
            devEnvSFCreds = new SalesforceCredentials();
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
            foreach (String un in devEnvSFCreds.usernamePartnerUrl.Keys)
            {
                this.cmbUserName.Items.Add(un);
            }
        }
        public void populateTreeView()
        {
            if (Directory.Exists(this.tbSourceCodeFolder.Text))
            {
                mainFolderNames = new HashSet<string>();
                tnListAddDependencies = new Dictionary<String, TreeNode>();
                tnListRemoveDependencies = new Dictionary<String, TreeNode>();

                if (this.tbSourceCodeFolder.Text != null
                    && this.tbSourceCodeFolder.Text != "")
                {
                    this.treeViewMetadata.Nodes.Clear();

                    String[] folders = Directory.GetDirectories(this.tbSourceCodeFolder.Text);
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
        private void treeViewMetadata_DoubleClick(object sender, EventArgs e)
        {
            System.Windows.Forms.MouseEventArgs evtArgs = (System.Windows.Forms.MouseEventArgs)e;

            if (evtArgs.Button == MouseButtons.Left)
            {
                TreeView tv = (TreeView)sender;

                if (tv.SelectedNode != null)
                {
                    TreeNode parentFolderNode = null;
                    TreeNode parentFileNode = null;

                    // Better control over whether the parent nodes are null or not
                    // Leave this structure in place as I don't think it can be simplified further
                    String pathToFile = "";
                    if (tv.SelectedNode.Parent != null)
                    {
                        if (tv.SelectedNode.Parent.Parent != null)
                        {
                            parentFolderNode = tv.SelectedNode.Parent.Parent;
                            parentFileNode = tv.SelectedNode.Parent;
                            pathToFile = this.tbSourceCodeFolder.Text + "\\" + parentFolderNode.Text + "\\" + parentFileNode.Text + "\\" + tv.SelectedNode.Text;
                        }
                        else
                        {
                            parentFolderNode = tv.SelectedNode.Parent;
                            pathToFile = this.tbSourceCodeFolder.Text + "\\" + parentFolderNode.Text + "\\" + tv.SelectedNode.Text;
                        }
                    }

                    //String pathToFile = this.tbProjectFolder.Text + "\\" + parentFolderNode.Text + "\\" + parentFileNode.Text;

                    checkArchiveDirectory(pathToFile);

                    if (Properties.Settings.Default.DefaultTextEditorPath == "")
                    {
                        Process proc = Process.Start(@"notepad.exe", "\"" + pathToFile);
                    }
                    else
                    {
                        Process proc = Process.Start(@Properties.Settings.Default.DefaultTextEditorPath, "\"" + pathToFile);
                    }
                }
            }
        }

        private void treeNodeSelectParentChildNodes(TreeNode tnd)
        {
            if (tnd.Checked == true)
            {
                if (tnd.Nodes.Count > 0)
                {
                    foreach (TreeNode cNode in tnd.Nodes)
                    {
                        cNode.Checked = true;
                    }
                }

                // Now go up the chain to the parent
                if (tnd.Parent != null)
                {
                    tnd.Parent.Checked = true;

                    if (tnd.Parent.Parent != null)
                    {
                        tnd.Parent.Parent.Checked = true;

                        if (tnd.Parent.Parent.Parent != null)
                        {
                            tnd.Parent.Parent.Parent.Checked = true;

                            if (tnd.Parent.Parent.Parent.Parent != null)
                            {
                                tnd.Parent.Parent.Parent.Parent.Checked = true;

                                if (tnd.Parent.Parent.Parent.Parent.Parent != null)
                                {
                                    tnd.Parent.Parent.Parent.Parent.Parent.Checked = true;
                                }
                            }
                        }
                    }
                }

                if (tnd.Nodes.Count > 0)
                {
                    foreach (TreeNode cnd1 in tnd.Nodes)
                    {
                        cnd1.Checked = true;

                        if (cnd1.Nodes.Count > 0)
                        {
                            foreach (TreeNode cnd2 in cnd1.Nodes)
                            {
                                cnd2.Checked = true;

                                if (cnd2.Nodes.Count > 0)
                                {
                                    foreach (TreeNode cnd3 in cnd2.Nodes)
                                    {
                                        cnd3.Checked = true;

                                        if (cnd3.Nodes.Count > 0)
                                        {
                                            foreach (TreeNode cnd4 in cnd3.Nodes)
                                            {
                                                cnd4.Checked = true;

                                                if (cnd4.Nodes.Count > 0)
                                                {
                                                    foreach (TreeNode cnd5 in cnd4.Nodes)
                                                    {
                                                        cnd5.Checked = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (tnd.Checked == false && tnd.Nodes.Count > 0)
            {
                foreach (TreeNode cnd1 in tnd.Nodes)
                {
                    cnd1.Checked = false;

                    if (cnd1.Nodes.Count > 0)
                    {
                        foreach (TreeNode cnd2 in cnd1.Nodes)
                        {
                            cnd2.Checked = false;

                            if (cnd2.Nodes.Count > 0)
                            {
                                foreach (TreeNode cnd3 in cnd2.Nodes)
                                {
                                    cnd3.Checked = false;

                                    if (cnd3.Nodes.Count > 0)
                                    {
                                        foreach (TreeNode cnd4 in cnd3.Nodes)
                                        {
                                            cnd4.Checked = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private void treeViewNodeAfterCheck(TreeNode tnd)
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

            TreeNode parentFolderNode = null;
            TreeNode parentFileNode = null;

            // Better control over whether the parent nodes are null or not
            // Leave this structure in place as I don't think it can be simplified further
            if (tnd.Parent != null)
            {
                if (tnd.Parent.Parent != null)
                {
                    parentFolderNode = tnd.Parent.Parent;
                    parentFileNode = tnd.Parent;
                }
                else
                {
                    parentFolderNode = tnd.Parent;
                    parentFileNode = tnd;
                }
            }

            // This means the selected Node is the top node for that group and the sub-nodes will need to be selected.
            if (tnd.Checked == true)
            {
                //Debug.WriteLine("tnd.Checked == true && tnd.Nodes.Count > 0 : " + runTreeNodeSelector);

                // Select the parent nodes
                if (parentFolderNode != null)
                {
                    parentFolderNode.Checked = true;
                }

                if (parentFileNode != null)
                {
                    parentFileNode.Checked = true;
                    blAddDependencies = true;
                }

                // Select the child nodes and their children
                foreach (TreeNode cNode1 in tnd.Nodes)
                {
                    cNode1.Checked = true;
                    if (cNode1.Nodes.Count > 0)
                    {
                        foreach (TreeNode cNode2 in cNode1.Nodes)
                        {
                            cNode2.Checked = true;
                        }
                    }
                }
            }
            else if (tnd.Checked == false)
            {
                // Make sure to check the parent folder unchecked the parent nodes if no other sub-nodes are selected
                // We need to confirm if there are any other elements checked under the parent node
                // i.e. : If a flow is selected, then the flows parent node needs to be checked.
                // If a flow is unselected, then we need to confirm first if there are any other flows checked before unselecting the parent node

                //Debug.WriteLine("tnd.Checked == false && tnd.Nodes.Count > 0 : " + runTreeNodeSelector);
                // Select the parent nodes
                if (parentFolderNode != null)
                {
                    //parentFolderNode.Checked = true;
                }

                if (parentFileNode != null)
                {
                    //parentFileNode.Checked = true;
                    blRemoveDependencies = true;
                }

                // De-select the child nodes and their children
                foreach (TreeNode cNode1 in tnd.Nodes)
                {
                    cNode1.Checked = false;
                    if (cNode1.Nodes.Count > 0)
                    {
                        foreach (TreeNode cNode2 in cNode1.Nodes)
                        {
                            cNode2.Checked = false;
                        }
                    }
                }
            }

            if (blAddDependencies == true)
            {
                addRemoveDependencies(parentFileNode, true);
            }

            if (blRemoveDependencies == true)
            {
                addRemoveDependencies(parentFileNode, false);
            }

            runTreeNodeSelector = true;
        }
        private void addRemoveDependencies(TreeNode tn, Boolean checkedState)
        {
            //Debug.WriteLine("public void addDependencies(TreeNode tn)");
            String[] nodeFullPath = tn.FullPath.Split('\\');
            String[] fileNameSplit = nodeFullPath[1].Split('.');
            selectDependencies(tn, nodeFullPath, fileNameSplit, checkedState);
        }
        private void selectDependencies(TreeNode tnd, String[] nodeFullPath, String[] fileNameSplit, Boolean checkedState)
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
                                tnd2.Checked = checkedState;
                                foreach (TreeNode tnd3 in tnd2.Nodes)
                                {
                                    tnd3.Checked = checkedState;
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
                                tnd2.Checked = checkedState;
                                foreach (TreeNode tnd3 in tnd2.Nodes)
                                {
                                    tnd3.Checked = checkedState;
                                }
                            }
                        }
                    }
                }
            }
            else if (nodeFullPath[0] == "labels")
            {
                foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
                {
                    if (tnd1.Text == "labels")
                    {
                        foreach (TreeNode tnd2 in tnd1.Nodes)
                        {
                            foreach (TreeNode tnd3 in tnd2.Nodes)
                            {
                                if (tnd3.Checked == true)
                                {
                                    tnd2.Checked = true;
                                    tnd1.Checked = true;
                                }
                            }
                        }
                    }
                }
            }
            else if (nodeFullPath[0] == "customMetadata")
            {
                foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
                {
                    if (tnd1.Text == "customMetadata")
                    {
                        foreach (TreeNode tnd2 in tnd1.Nodes)
                        {
                            if (tnd2.Text == nodeFullPath[1])
                            {
                                tnd2.Checked = checkedState;
                                foreach (TreeNode tnd3 in tnd2.Nodes)
                                {
                                    tnd3.Checked = checkedState;
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
                foreach (TreeNode tnd3 in tnd.Nodes)
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
                                tnd2.Parent.Checked = true;
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
                foreach (TreeNode tndObj in tnd.Nodes)
                {
                    if (tndObj.Checked == true && tndObj.Text.StartsWith("<fields"))
                    {
                        String tndObjXmlString = "<document>" + tndObj.Text + "</document>";
                        XmlDocument tndObjXd = new XmlDocument();
                        tndObjXd.LoadXml(tndObjXmlString);
                        String objectFieldCombo = objectName[0] + "." + tndObjXd.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;
                        populateStandardValueSetHashSet(objectFieldCombo);
                    }
                }
            }
            else if (nodeFullPath[0] == "certs")
            {
                foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
                {
                    if (tnd1.Text == "certs")
                    {
                        foreach (TreeNode tnd2 in tnd1.Nodes)
                        {
                            if (tnd2.Text == fileNameSplit[0] + ".crt"
                                || tnd2.Text == fileNameSplit[0] + ".crt-meta.xml")
                            {
                                tnd2.Checked = checkedState;
                                foreach (TreeNode tnd3 in tnd2.Nodes)
                                {
                                    tnd3.Checked = checkedState;
                                }
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
                                tnd2.Checked = checkedState;
                                foreach (TreeNode tnd3 in tnd2.Nodes)
                                {
                                    tnd3.Checked = checkedState;
                                }
                            }
                        }
                    }
                }
            }
            else if (nodeFullPath[0] == "components")
            {
                foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
                {
                    if (tnd1.Text == "components")
                    {
                        foreach (TreeNode tnd2 in tnd1.Nodes)
                        {
                            if (tnd2.Text == fileNameSplit[0] + ".component"
                                || tnd2.Text == fileNameSplit[0] + ".component-meta.xml")
                            {
                                tnd2.Checked = checkedState;
                                foreach (TreeNode tnd3 in tnd2.Nodes)
                                {
                                    tnd3.Checked = checkedState;
                                }
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
                                tnd2.Checked = checkedState;
                                foreach (TreeNode tnd3 in tnd2.Nodes)
                                {
                                    tnd3.Checked = checkedState;
                                }
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
                                tnd2.Checked = checkedState;
                                foreach (TreeNode tnd3 in tnd2.Nodes)
                                {
                                    tnd3.Checked = checkedState;
                                }
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
                                tnd2.Checked = checkedState;
                                foreach (TreeNode tnd3 in tnd2.Nodes)
                                {
                                    tnd3.Checked = checkedState;
                                }
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
                                tnd2.Checked = checkedState;
                                foreach (TreeNode tnd3 in tnd2.Nodes)
                                {
                                    tnd3.Checked = checkedState;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (tnd2.Text == nodeFullPath[1])
                        {
                            tnd2.Checked = checkedState;
                            foreach (TreeNode tnd3 in tnd2.Nodes)
                            {
                                tnd3.Checked = checkedState;
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
        public void selectObjectFieldsFromLayout(String objectName, String xmlDocument)
        {
            // We only want the custom ones from the layout, not the standard ones
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
                                    String tndObjXmlString = "<document>" + tnd3.Text + "</document>";
                                    XmlDocument tndObjXd = new XmlDocument();
                                    tndObjXd.LoadXml(tndObjXmlString);

                                    String fieldName = tndObjXd.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;

                                    // Don't deploy Standard Fields
                                    // Manage package fields are questionable as well but will include them for now
                                    if (objectFields.Contains(fieldName))
                                    {
                                        String[] fieldNameSplit = fieldName.Split(new String[] { "__" }, StringSplitOptions.None);
                                        if (fieldNameSplit.Length > 3)
                                        {
                                            // Throws are not great, but a good stop gap for now until I can figure something else out.
                                            throw new Exception("Field name split length is greater than 3. This is not expected. Field Name: " + fieldName);
                                        }
                                        else if (fieldNameSplit.Length == 3)
                                        {
                                            tnd3.Checked = true;
                                            treeNodeSelectParentChildNodes(tnd3);
                                        }
                                        else if (fieldNameSplit.Length == 2)
                                        {
                                            tnd3.Checked = true;
                                            treeNodeSelectParentChildNodes(tnd3);
                                        }

                                        // I don't like the below because it means multiple loops within loop iterations, but for now
                                        // I don't want to refactor this
                                        if (objectName.EndsWith("__c"))
                                        {
                                            selectRequiredObjectFields(objectName + ".object");
                                        }
                                        else if (objectName.EndsWith("__mdt"))
                                        {
                                            selectRequiredObjectFields(objectName + ".object");
                                            selectMetadataObjects(objectName);
                                        }

                                        // If a standard value set is available on the page layout, make sure to include it in the HashSet to add to the deployment package
                                        String objectFieldCombo = objectName + "." + fieldName;
                                        populateStandardValueSetHashSet(objectFieldCombo);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public void selectRequiredObjectFields(String objectName)
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
                                XmlDocument tnd3Xd = new XmlDocument();
                                String tnd3XmlString = "<document>" + tnd3.Text + "</document>";
                                tnd3Xd.LoadXml(tnd3XmlString);

                                if (tnd3Xd.ChildNodes[0].ChildNodes[0].LocalName == "customSettingsType")
                                {
                                    tnd3.Checked = true;
                                }
                                else if (tnd3Xd.ChildNodes[0].ChildNodes[0].LocalName == "deploymentStatus")
                                {
                                    tnd3.Checked = true;
                                }
                                else if (tnd3Xd.ChildNodes[0].ChildNodes[0].LocalName == "description")
                                {
                                    tnd3.Checked = true;
                                }
                                else if (tnd3Xd.ChildNodes[0].ChildNodes[0].LocalName == "label")
                                {
                                    tnd3.Checked = true;
                                }
                                else if (tnd3Xd.ChildNodes[0].ChildNodes[0].LocalName == "nameField")
                                {
                                    tnd3.Checked = true;
                                }
                                else if (tnd3Xd.ChildNodes[0].ChildNodes[0].LocalName == "pluralLabel")
                                {
                                    tnd3.Checked = true;
                                }
                                else if (tnd3Xd.ChildNodes[0].ChildNodes[0].LocalName == "sharingModel")
                                {
                                    tnd3.Checked = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        public void selectMetadataObjects(String objectName)
        {
            // Check off the Metadata Types in the customMetadata folder related to the metadata type checked.
            foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
            {
                if (tnd1.Text == "customMetadata")
                {
                    String[] splitObjectFileName = objectName.Split(new String[] { "__" }, StringSplitOptions.None);

                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        String[] splitTND2Node = tnd2.Text.Split('.');
                        if (splitTND2Node[0] == splitObjectFileName[0])
                        {
                            tnd2.Checked = true;
                            tnd1.Checked = true;

                            foreach (TreeNode tnd3 in tnd2.Nodes)
                            {
                                tnd3.Checked = true;

                                foreach (TreeNode tnd4 in tnd3.Nodes)
                                {
                                    tnd4.Checked = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        public void selectStandardValueSets(HashSet<String> standardValueSets)
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
                            treeNodeSelectParentChildNodes(tnd2);
                        }
                    }
                }
            }
        }
        public void populateStandardValueSetHashSet(String objectFieldCombo)
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

            ZipFileWithPackageXML.copySelectedToRepository(this.treeViewMetadata.Nodes,
                                                           this.tbSourceCodeFolder.Text,
                                                           this.tbRepository.Text);

            String zipFilePath = ZipFileWithPackageXML.buildZipFileWithPackageXml(this.treeViewMetadata.Nodes,
                                                                                  this.tbBaseFolderPath.Text,
                                                                                  this.tbDeployFrom.Text,
                                                                                  this.tbSourceCodeFolder.Text,
                                                                                  "");

            DeployMetadata dm = new DeployMetadata();
            dm.cmbUserName.SelectedItem = this.cmbUserName.Text;
            dm.tbZipFileLocation.Text = zipFilePath;
            dm.cbCheckOnly.CheckState = CheckState.Unchecked;
            dm.cbCheckOnly.Checked = false;

            dm.Show();
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
                && this.tbBaseFolderPath.Text != null)
            {
                SalesforceMetadataStep2 sfMetadataStep2 = new SalesforceMetadataStep2();
                sfMetadataStep2.userName = this.cmbUserName.Text;
                sfMetadataStep2.selectedItems = packageXmlMembers;
                sfMetadataStep2.tbFromOrgSaveLocation.Text = this.tbSourceCodeFolder.Text;

                try
                {
                    sfMetadataStep2.metaRetrieveSFCreds.salesforceLogin(UtilityClass.REQUESTINGORG.FROMORG, this.cmbUserName.Text);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                    return;
                }

                if (sfMetadataStep2.metaRetrieveSFCreds.loginSuccess == false)
                {
                    MessageBox.Show("Please check username, password and/or security token");
                    return;
                }

                Action act = () => sfMetadataStep2.requestZipFile(UtilityClass.REQUESTINGORG.FROMORG, this.tbBaseFolderPath.Text, sfMetadataStep2);
                Task tsk = Task.Run(act);
            }
        }

        private void tbProjectFolder_DoubleClick(object sender, EventArgs e)
        {
            String selectedPath = UtilityClass.folderBrowserSelectPath("Select Your Project Folder",
                                                                       false,
                                                                       FolderEnum.SaveTo,
                                                                       Properties.Settings.Default.BaseFolderPath);

            if (selectedPath != "")
            {
                this.tbSourceCodeFolder.Text = selectedPath;
                Properties.Settings.Default.DevelopmentSelectedFolder = selectedPath;
                Properties.Settings.Default.Save();

                this.projectValuesChanged = true;

                populateTreeView();
            }
        }

        private void tbProjectFolder_TextChanged(object sender, EventArgs e)
        {
            if (bypassTextChange == true) return;

            Properties.Settings.Default.DevelopmentSelectedFolder = this.tbSourceCodeFolder.Text;
            Properties.Settings.Default.Save();

            this.projectValuesChanged = true;

            populateTreeView();
        }

        private void tbDeployFrom_DoubleClick(object sender, EventArgs e)
        {
            String selectedPath = "";

            if (String.IsNullOrEmpty(Properties.Settings.Default.DevelopmentDeploymentFolder) == false)
            {
                selectedPath = UtilityClass.folderBrowserSelectPath("Select The Deploy From Folder",
                                                                     true,
                                                                     FolderEnum.SaveTo,
                                                                     Properties.Settings.Default.DevelopmentDeploymentFolder);
            }
            else if (String.IsNullOrEmpty(Properties.Settings.Default.BaseFolderPath) == false)
            {
                selectedPath = UtilityClass.folderBrowserSelectPath("Select The Deploy From Folder",
                                                                     true,
                                                                     FolderEnum.SaveTo,
                                                                     Properties.Settings.Default.BaseFolderPath);
            }
            else
            {
                selectedPath = UtilityClass.folderBrowserSelectPath("Select The Deploy From Folder",
                                                                     true,
                                                                     FolderEnum.SaveTo,
                                                                     "");
            }

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
            String selectedPath = "";

            if (String.IsNullOrEmpty(Properties.Settings.Default.RepositoryPath) == false)
            {
                selectedPath = UtilityClass.folderBrowserSelectPath("Select the path to the Repository",
                                                                     true,
                                                                     FolderEnum.SaveTo,
                                                                     Properties.Settings.Default.RepositoryPath);
            }
            else if (String.IsNullOrEmpty(Properties.Settings.Default.BaseFolderPath) == false)
            {
                selectedPath = UtilityClass.folderBrowserSelectPath("Select the path to the Repository",
                                                                     true,
                                                                     FolderEnum.SaveTo,
                                                                     Properties.Settings.Default.BaseFolderPath);
            }
            else
            {
                selectedPath = UtilityClass.folderBrowserSelectPath("Select the path to the Repository",
                                                                     true,
                                                                     FolderEnum.SaveTo,
                                                                     "");
            }

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

        private void tbBaseFolderPath_DoubleClick(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Base folder for references to related development folders such as the Code Archive folder";
            fbd.ShowNewFolderButton = true;

            fbd.ShowDialog();

            if (fbd.SelectedPath != null)
            {
                this.tbBaseFolderPath.Text = fbd.SelectedPath;
                Properties.Settings.Default.BaseFolderPath = fbd.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }

        private void tbBaseFolderPath_TextChanged(object sender, EventArgs e)
        {
            if (bypassTextChange == true) return;

            Properties.Settings.Default.BaseFolderPath = this.tbBaseFolderPath.Text;
            Properties.Settings.Default.Save();

            this.projectValuesChanged = true;
        }

        // Check if Code Archive directory exists
        private void checkArchiveDirectory(String fileToCopy)
        {
            if (this.tbBaseFolderPath.Text == "")
            {
                MessageBox.Show("Please select a base folder path first to copy the file to the Code Archive folder");
                return;
            }

            String[] fileToCopySplit = fileToCopy.Split('\\');
            String[] fileNameSplit = fileToCopySplit[fileToCopySplit.Length - 1].Split('.');

            String codeArchiveRootPath = this.tbBaseFolderPath.Text + "\\Code Archive";
            String logFile = this.tbBaseFolderPath.Text + "\\Code Archive\\LogFile.txt";

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
                try
                {
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
                catch (Exception exc)
                {
                    MessageBox.Show("Error copying file to Code Archive folder: " + exc.Message);
                    return;
                }
            }
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
                            String[] lwcfiles = Directory.GetFiles(this.tbSourceCodeFolder.Text + "\\lwc\\" + e.filesCreated[0]);

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
            if (this.tbSourceCodeFolder.Text == "")
            {
                MessageBox.Show("Please select a project folder to search first");
            }
            else
            {
                SearchForm srch = new SearchForm();
                srch.tbSearchLocation.Text = this.tbSourceCodeFolder.Text;
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
                devEnvSFCreds.salesforceLogin(UtilityClass.REQUESTINGORG.FROMORG, this.cmbUserName.Text);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                return;
            }

            if (devEnvSFCreds.loginSuccess == false)
            {
                MessageBox.Show("Please check username, password and/or security token");
                return;
            }

            String selectStatement = "";
            String userId = "";
            userId = devEnvSFCreds.fromOrgLR.userId;

            selectStatement = "SELECT Id FROM ApexLog WHERE LogUserId = \'" + userId + "\'";

            SalesforceMetadata.PartnerWSDL.QueryResult qr = new SalesforceMetadata.PartnerWSDL.QueryResult();

            try
            {
                qr = devEnvSFCreds.fromOrgSS.query(selectStatement);

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
                                    PartnerWSDL.DeleteResult[] dr = devEnvSFCreds.fromOrgSS.delete(recordIdsToDelete[rtd].ToArray());
                                }
                            }

                            if (!qr.done)
                            {
                                qr = devEnvSFCreds.fromOrgSS.queryMore(qr.queryLocator);
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
                            PartnerWSDL.DeleteResult[] dr = devEnvSFCreds.fromOrgSS.delete(recordIdsToDelete[rtd].ToArray());
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

            if (devEnvSFCreds.isProduction[this.cmbUserName.Text] == true)
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

        //private void tbOutboundChangeSetName_MouseHover(object sender, EventArgs e)
        //{
        //    TextBox TB = (TextBox)sender;
        //    int VisibleTime = 10000;  //in milliseconds

        //    ToolTip tt = new ToolTip();
        //    tt.Show("If you have a open Salesforce Outbound Change Set, add the Change Set Name here. When deployed, your changes will be added to the change set.", TB, 0, 0, VisibleTime);
        //}

        private void tbBaseFolderPath_MouseHover(object sender, EventArgs e)
        {
            TextBox TB = (TextBox)sender;
            ToolTip tt = new ToolTip();
            tt.Show("The base folder where other related folders will be built from such as the path to the Code Archive folder", TB);
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

            NewFilePath nfp = new NewFilePath();
            nfp.baseFolderPath = Properties.Settings.Default.BaseFolderPath;
            nfp.tbBaseFolderPath.Text = Properties.Settings.Default.BaseFolderPath;
            nfp.Show();

            Action act = () => createNewProjectFile(nfp, this);
            System.Threading.Tasks.Task tsk = System.Threading.Tasks.Task.Run(act);

            //this.populateTreeView();
        }

        private void createNewProjectFile(NewFilePath nfp, DevelopmentEnvironment devEnvFrm)
        {
            Boolean windowOpen = true;

            while (windowOpen)
            {
                if (nfp.Disposing == true)
                {
                    if (nfp.projectSolutionFilePath != null)
                    {
                        StreamWriter sw = new StreamWriter(nfp.projectSolutionFilePath);
                        sw.Close();

                        var threadParameters1 = new System.Threading.ThreadStart(delegate { tsWriteProjectFileToTB(nfp.baseFolderPath, nfp.sourceCodeFolderPath, nfp.projectSolutionFilePath, devEnvFrm); });
                        var thread1 = new System.Threading.Thread(threadParameters1);
                        thread1.Start();
                        while (thread1.ThreadState == System.Threading.ThreadState.Running)
                        {
                            // do nothing. Just want for the thread to complete
                        }
                    }

                    windowOpen = false;
                }
            }
        }

        // Threadsafe way to write back to the form's textbox
        public void tsWriteProjectFileToTB(String baseFolder, String sourceCodeFolderPath, String projectSolutionFilePath, DevelopmentEnvironment devEnvFrm)
        {
            if (devEnvFrm.tbProjectFile.InvokeRequired)
            {
                Action safeWrite = delegate { tsWriteProjectFileToTB($"{baseFolder}", $"{sourceCodeFolderPath}", $"{projectSolutionFilePath}", devEnvFrm); };
                devEnvFrm.tbProjectFile.Invoke(safeWrite);
            }
            else
            {
                devEnvFrm.tbProjectFile.Text = projectSolutionFilePath;
                devEnvFrm.tbSourceCodeFolder.Text = sourceCodeFolderPath;
                devEnvFrm.tbBaseFolderPath.Text = baseFolder;
                devEnvFrm.cmbUserName.Text = "";
                devEnvFrm.tbDeployFrom.Text = "";
                devEnvFrm.tbRepository.Text = "";

                Properties.Settings.Default.BaseFolderPath = baseFolder;
                Properties.Settings.Default.DevelopmentSelectedFolder = sourceCodeFolderPath;
                Properties.Settings.Default.RecentProjectPath = projectSolutionFilePath;

                if(devEnvFrm.tbSourceCodeFolder.Text != "")
                {
                    devEnvFrm.populateTreeView();
                }
            }

            Properties.Settings.Default.DevelopmentDeploymentFolder = "";
            Properties.Settings.Default.RepositoryPath = "";
            Properties.Settings.Default.Save();
        }

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            updateProjectFile();
            this.projectValuesChanged = false;
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
                sw.WriteLine(this.tbBaseFolderPath.Text);

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
            this.tbProjectFile.Text = projectFilePath;

            if (sr.EndOfStream == true)
            {
                return;
            }

            // Make sure this is here so the values in the project file don't get misaligned.
            sr.ReadLine();

            String username = sr.ReadLine();
            if (this.cmbUserName.Items.Contains(username))
            {
                this.cmbUserName.Text = username;
            }

            this.tbSourceCodeFolder.Text = sr.ReadLine();
            this.tbDeployFrom.Text = sr.ReadLine();
            this.tbRepository.Text = sr.ReadLine();
            this.tbBaseFolderPath.Text = sr.ReadLine();

            Properties.Settings.Default.ProjectFilePath = this.tbProjectFile.Text;
            Properties.Settings.Default.DevelopmentSelectedFolder = this.tbSourceCodeFolder.Text;
            Properties.Settings.Default.DevelopmentDeploymentFolder = this.tbDeployFrom.Text;
            Properties.Settings.Default.RepositoryPath = this.tbRepository.Text;
            Properties.Settings.Default.BaseFolderPath = this.tbBaseFolderPath.Text;

            Properties.Settings.Default.Save();

            sr.Close();

            populateTreeView();

            bypassTextChange = false;
        }

        private void addApexTriggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.tbSourceCodeFolder.Text == "")
            {
                MessageBox.Show("Please select a Project Solution / Folder before adding new objects");
            }
            else
            {
                AddObject ao = new AddObject();
                ao.projectFolderPath = this.tbSourceCodeFolder.Text;
                ao.loadSobjectsToCombobox();
                ao.tbTriggerName.Select();
                ao.ObjectAdded += AddObject_ObjectAdded;
                ao.Show();
            }
        }

        private void addApexClassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.tbSourceCodeFolder.Text == "")
            {
                MessageBox.Show("Please select a Project Solution / Folder before adding new objects");
            }
            else
            {
                AddObject ao = new AddObject();
                ao.projectFolderPath = this.tbSourceCodeFolder.Text;
                ao.loadSobjectsToCombobox();
                ao.tbClassName.Select();
                ao.ObjectAdded += AddObject_ObjectAdded;
                ao.Show();
            }
        }

        private void addLightningWebComponentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddObject ao = new AddObject();
            ao.projectFolderPath = this.tbSourceCodeFolder.Text;
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
            ZipFileWithPackageXML.copySelectedToRepository(this.treeViewMetadata.Nodes,
                                                           this.tbSourceCodeFolder.Text,
                                                           this.tbRepository.Text);
        }
    }
}
