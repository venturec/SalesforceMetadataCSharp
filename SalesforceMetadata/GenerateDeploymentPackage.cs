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
using System.Diagnostics;

using SalesforceMetadata.PartnerWSDL;
using SalesforceMetadata.MetadataWSDL;
using SalesforceMetadata.ToolingWSDL;
using System.Diagnostics.Eventing.Reader;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Excel;
using System.Security.Cryptography;
using System.Xml.Linq;
using iTextSharp.text.xml;

namespace SalesforceMetadata
{
    public partial class GenerateDeploymentPackage : Form
    {
        public Boolean runTreeNodeSelector = true;

        private Dictionary<String, String> usernameToSecurityToken;
        HashSet<String> mainFolderNames;
        Dictionary<String, TreeNode> tnListAddDependencies;
        Dictionary<String, TreeNode> tnListRemoveDependencies;

        //private String orgName;
        private Boolean bypassTextChange = false;
        private Boolean projectValuesChanged = false;

        private HashSet<String> standardValueSets = new HashSet<string>();

        public HashSet<String> treeNodeFromDiff;

        public GenerateDeploymentPackage()
        {
            InitializeComponent();
            loadDefaultApis();
            formSizeResolution();
        }

        private void formSizeResolution()
        {
            System.Drawing.Rectangle rect = Screen.FromControl(this).Bounds;
            if (rect.Width < 1200)
            {
                this.ClientSize = new System.Drawing.Size(rect.Size.Width - 100, rect.Size.Height - 100);
                this.treeViewMetadata.Size = new System.Drawing.Size(rect.Size.Width - 150, rect.Size.Height - 350);
            }
        }

        private void tbDeploymentPackageLocation_DoubleClick(object sender, EventArgs e)
        {
            String selectedPath = UtilityClass.folderBrowserSelectPath("Select folder to save Deployment items to", 
                                                                       true, 
                                                                       FolderEnum.SaveTo,
                                                                       Properties.Settings.Default.DeploymentPackageLastSaveLocation);

            if (selectedPath != "")
            {
                this.tbDeployFrom.Text = selectedPath;
                Properties.Settings.Default.DeploymentPackageLastSaveLocation = selectedPath;
                Properties.Settings.Default.Save();

                Boolean isEmpty = true;
                String[] dirs = Directory.GetDirectories(this.tbDeployFrom.Text);
                String[] fls = Directory.GetFiles(this.tbDeployFrom.Text);

                if (dirs.Length > 0 || fls.Length > 0) isEmpty = false;

                if (isEmpty == false)
                {
                    // read from the items and mark the deployable items
                    // Which means you have to open the XML files and 
                }
            }
        }

        private void tbMetadataFolderToReadFrom_DoubleClick(object sender, EventArgs e)
        {
            String selectedPath = UtilityClass.folderBrowserSelectPath("Select folder to read the Deployment items to", 
                                                                                        true, 
                                                                                        FolderEnum.SaveTo,
                                                                                        Properties.Settings.Default.DeploymentPackageLastReadLocation);

            if(selectedPath != "")
            {
                this.tbProjectFolder.Text = selectedPath;
                Properties.Settings.Default.DeploymentPackageLastReadLocation = selectedPath;
                Properties.Settings.Default.Save();
                populateTreeView();
            }
        }

        public void populateTreeView()
        {
            this.treeViewMetadata.Nodes.Clear();

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

        public void populateMetadataTreeViewFromDiff()
        {
            this.runTreeNodeSelector = false;

            this.treeViewMetadata.Nodes.Clear();
            this.populateTreeView();

            foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
            {
                if (this.treeNodeFromDiff.Contains(tnd1.FullPath))
                {
                    tnd1.Checked = true;
                    treeNodeSelectParentChildNodes(tnd1);
                    treeViewNodeAfterCheck(tnd1);
                }

                foreach(TreeNode tnd2 in tnd1.Nodes)
                {
                    if (this.treeNodeFromDiff.Contains(tnd2.FullPath))
                    {
                        tnd2.Checked = true;
                        treeNodeSelectParentChildNodes(tnd2);
                        treeViewNodeAfterCheck(tnd2);
                    }

                    foreach (TreeNode tnd3 in tnd2.Nodes)
                    {
                        if (this.treeNodeFromDiff.Contains(tnd3.FullPath))
                        {
                            tnd3.Checked = true;
                            treeNodeSelectParentChildNodes(tnd3);
                            treeViewNodeAfterCheck(tnd3);
                        }

                        foreach (TreeNode tnd4 in tnd3.Nodes)
                        {
                            if (this.treeNodeFromDiff.Contains(tnd4.FullPath))
                            {
                                tnd4.Checked = true;
                                treeNodeSelectParentChildNodes(tnd4);
                                treeViewNodeAfterCheck(tnd4);
                            }

                            foreach (TreeNode tnd5 in tnd4.Nodes)
                            {
                                if (this.treeNodeFromDiff.Contains(tnd5.FullPath))
                                {
                                    tnd5.Checked = true;
                                    treeNodeSelectParentChildNodes(tnd5);
                                    treeViewNodeAfterCheck(tnd5);
                                }

                                foreach (TreeNode tnd6 in tnd5.Nodes)
                                {
                                    if (this.treeNodeFromDiff.Contains(tnd6.FullPath))
                                    {
                                        tnd6.Checked = true;
                                        treeNodeSelectParentChildNodes(tnd6);
                                        treeViewNodeAfterCheck(tnd6);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            this.runTreeNodeSelector = true;
        }

        private void parseXmlDocument(TreeNode tnd2, XmlDocument xmlDoc)
        {
            XmlNodeParser ndParser = new XmlNodeParser();

            String nodeBlockNameValue = "";
            foreach (XmlNode nd3 in xmlDoc.ChildNodes) 
            {
                if (nd3.OuterXml == "<?xml version=\"1.0\" encoding=\"UTF-8\"?>")
                {
                    continue;
                }

                // XML top tag
                TreeNode tnd3 = new TreeNode(nd3.Name);

                if (nd3.HasChildNodes)
                {
                    foreach (XmlNode nd4 in nd3.ChildNodes)
                    {
                        /****************************************************************/
                        nodeBlockNameValue = MetadataDifferenceProcessing.getNameField(nd3.Name, nd4.Name, nd4.OuterXml);
                        /****************************************************************/

                        if (nd4.HasChildNodes)
                        {
                            TreeNode tnd4 = new TreeNode();
                            if (nodeBlockNameValue == "")
                            {
                                tnd4.Text = nd4.Name;
                            }
                            else
                            {
                                tnd4.Text = nd4.Name + " | " + nodeBlockNameValue;
                            }

                            List<XmlNodeParser.XmlNodeValue> ndPathAndValues = ndParser.parseXmlChildNodes1(nd4);
                            ndParser.buildTreeNodeWithValues1(tnd4, ndPathAndValues);
                            tnd3.Nodes.Add(tnd4);
                        }
                        else if (nd4.NodeType == XmlNodeType.Text)
                        {
                            TreeNode tnd4 = new TreeNode(nd4.Value);
                            tnd3.Nodes.Add(tnd4);
                        }
                    }
                }
                //else
                //{
                //    Debug.WriteLine("");
                //}

                tnd2.Nodes.Add(tnd3);
            }
        }

        public void defaultSelectedFromMetadataComp()
        {
            if (runTreeNodeSelector == true)
            {
                runTreeNodeSelector = false;
            }
        }

        // TODO: When selecting the top parent Node, the fileNameSplit index throws an Out of Range Error
        // Also, select the top parent node won't select any dependencies as it was not built for that.
        private void treeViewMetadata_AfterCheck(object sender, TreeViewEventArgs e)
        {
            treeViewNodeAfterCheck(e.Node);
        }

        public void treeNodeSelectParentChildNodes(TreeNode tnd)
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

        public void treeViewNodeAfterCheck(TreeNode tnd)
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

            TreeNode parentTn = tnd.Parent;

            // This means the selected Node is the top node for that group and the sub-nodes will need to be selected.
            if (tnd.Checked == true && tnd.Nodes.Count > 0)
            {
                //Debug.WriteLine("tnd.Checked == true && tnd.Nodes.Count > 0 : " + runTreeNodeSelector);

                foreach (TreeNode cNode in tnd.Nodes)
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
            else if (tnd.Checked == false && tnd.Nodes.Count > 0)
            {
                //Debug.WriteLine("tnd.Checked == false && tnd.Nodes.Count > 0 : " + runTreeNodeSelector);

                foreach (TreeNode cNode in tnd.Nodes)
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
            else if (tnd.Checked == true && parentTn != null)
            {
                if (!mainFolderNames.Contains(parentTn.Text))
                {
                    //Debug.WriteLine("tnd.Checked == true && parentTn != null && !mainFolderNames.Contains(parentTn.Text) : " + runTreeNodeSelector);

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
            else if (tnd.Checked == false && parentTn != null)
            {
                blRemoveDependencies = true;
            }

            if (blAddDependencies == true)
            {
                //Debug.WriteLine("blAddDependencies");
                addDependencies(tnd);
            }

            if (blRemoveDependencies == true)
            {
                //Debug.WriteLine("");
                removeDependencies(tnd);
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

        private void removeDependencies(TreeNode tnd)
        {
            // TODO: This will be very complex especially with custom objects if multiple fields are selected and then one is deselected.
            // Don't want to remove the dependencies if there are more than 1 field or item selected.
            // Then again, what if it is a net new object?
            // Have to think through the logic and processes to make this work.

            String[] nodeFullPath = tnd.FullPath.Split('\\');

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

        private void selectDependencies(TreeNode tnd, String[] nodeFullPath, String[] fileNameSplit)
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
                if (tnd.Text.StartsWith("<fields"))
                {
                    String tnd3XmlString = "<document>" + tnd.Text + "</document>";
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

        public void selectObjectFieldsFromLayout(String objectName, String xmlDocument)
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
                                foreach (TreeNode tnd4 in tnd3.Nodes)
                                {
                                    if (tnd4.Text.StartsWith("fields"))
                                    {
                                        String[] fieldSplit = tnd4.Text.Split('|');

                                        if (objectFields.Contains(fieldSplit[1].Trim()))
                                        {
                                            tnd4.Checked = true;
                                            treeNodeSelectParentChildNodes(tnd4);

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
                                            String objectFieldCombo = objectName + "." + fieldSplit[1].Trim();
                                            populateStandardValueSetHashSet(objectFieldCombo);
                                        }
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
                            tnd2.Checked = true;

                            foreach (TreeNode tnd3 in tnd2.Nodes)
                            {
                                foreach (TreeNode tnd4 in tnd3.Nodes)
                                {
                                    if (tnd4.Text == "customSettingsType")
                                    {
                                        tnd4.Checked = true;
                                        treeNodeSelectParentChildNodes(tnd4);
                                    }
                                    else if (tnd4.Text.StartsWith("deploymentStatus"))
                                    {
                                        tnd4.Checked = true;
                                        treeNodeSelectParentChildNodes(tnd4);
                                    }
                                    else if (tnd4.Text.StartsWith("description"))
                                    {
                                        tnd4.Checked = true;
                                        treeNodeSelectParentChildNodes(tnd4);
                                    }
                                    else if (tnd4.Text.StartsWith("label"))
                                    {
                                        tnd4.Checked = true;
                                        treeNodeSelectParentChildNodes(tnd4);
                                    }
                                    else if (tnd4.Text.StartsWith("nameField"))
                                    {
                                        tnd4.Checked = true;
                                        treeNodeSelectParentChildNodes(tnd4);
                                    }
                                    else if (tnd4.Text.StartsWith("pluralLabel"))
                                    {
                                        tnd4.Checked = true;
                                        treeNodeSelectParentChildNodes(tnd4);
                                    }
                                    else if (tnd4.Text.StartsWith("sharingModel"))
                                    {
                                        tnd4.Checked = true;
                                        treeNodeSelectParentChildNodes(tnd4);
                                    }
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

        private void btnBuildProfilesAndPermissionSets_Click(object sender, EventArgs e)
        {
            // Clear all checkboxes as there may have been changes
            foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
            {
                if ((tnd1.Text == "profiles"
                    || tnd1.Text == "permissionsets"))
                {
                    if (tnd1.Checked == true)
                    {
                        tnd1.Checked = false;
                    }

                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (tnd2.Checked == true)
                        {
                            tnd2.Checked = false;
                        }

                        foreach (TreeNode tnd3 in tnd2.Nodes)
                        {
                            if (tnd3.Checked == true)
                            {
                                tnd3.Checked = false;
                            }
                        }
                    }
                }
            }

            // Key = the Profile tag name related to the object type selected in the treenode
            // Example: if the tnd1.Text == "objects", then the Profile or Permission set Key will be objectPermissions

            Dictionary<String, List<String>> profileNodesToObjectsSelected = new Dictionary<String, List<String>>();

            foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
            {
                // We need to review and extract out the permissions for the objects within the detail: object name and permissions, field permissions, record type visibilities
                if (tnd1.Text == "classes")
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (tnd2.Checked == true)
                        {
                            if (!profileNodesToObjectsSelected.ContainsKey("classAccesses"))
                            {
                                profileNodesToObjectsSelected.Add("classAccesses", new List<string> { tnd2.FullPath });
                            }
                            else
                            {
                                profileNodesToObjectsSelected["classAccesses"].Add(tnd2.FullPath);
                            }
                        }
                    }
                }
                else if (tnd1.Text == "objects")
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        foreach (TreeNode tnd3 in tnd2.Nodes)
                        {
                            if (tnd3.Checked == true)
                            {
                                if (tnd3.Text.StartsWith("fields"))
                                {
                                    if (profileNodesToObjectsSelected.ContainsKey("fieldPermissions"))
                                    {
                                        profileNodesToObjectsSelected["fieldPermissions"].Add(tnd3.FullPath);
                                    }
                                    else
                                    {
                                        profileNodesToObjectsSelected.Add("fieldPermissions", new List<string> { tnd3.FullPath });
                                    }
                                }
                                else if (tnd3.Text.StartsWith("recordTypes"))
                                {
                                    if (profileNodesToObjectsSelected.ContainsKey("recordTypeVisibilities"))
                                    {
                                        profileNodesToObjectsSelected["recordTypeVisibilities"].Add(tnd3.FullPath);
                                    }
                                    else
                                    {
                                        profileNodesToObjectsSelected.Add("recordTypeVisibilities", new List<string> { tnd3.FullPath });
                                    }
                                }

                                if (profileNodesToObjectsSelected.ContainsKey("objectPermissions")
                                    && !profileNodesToObjectsSelected["objectPermissions"].Contains(tnd2.FullPath))
                                {
                                    profileNodesToObjectsSelected["objectPermissions"].Add(tnd2.FullPath);
                                }
                                else if (!profileNodesToObjectsSelected.ContainsKey("objectPermissions"))
                                {
                                    profileNodesToObjectsSelected.Add("objectPermissions", new List<string> { tnd2.FullPath });
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        foreach (TreeNode tnd3 in tnd2.Nodes)
                        {
                            if (tnd3.Checked == true)
                            {
                                String[] parsedFullPath = tnd2.FullPath.Split('\\');

                                if (parsedFullPath[0] == "applications")
                                {
                                    if (profileNodesToObjectsSelected.ContainsKey("applicationVisibilities")
                                        && !profileNodesToObjectsSelected["applicationVisibilities"].Contains(tnd2.FullPath))
                                    {
                                        profileNodesToObjectsSelected["applicationVisibilities"].Add(tnd2.FullPath);
                                    }
                                    else if (!profileNodesToObjectsSelected.ContainsKey("applicationVisibilities"))
                                    {
                                        profileNodesToObjectsSelected.Add("applicationVisibilities", new List<string> { tnd2.FullPath });
                                    }
                                }
                                //else if ()
                                //{
                                //"categoryGroupVisibilities"
                                //}
                                // TODO: CustomMetadata types are the XML, but you will also need to include the object and fields
                                else if (parsedFullPath[0] == "customMetadata")
                                {
                                    if (profileNodesToObjectsSelected.ContainsKey("customMetadataTypeAccesses")
                                        && !profileNodesToObjectsSelected["customMetadataTypeAccesses"].Contains(tnd2.FullPath))
                                    {
                                        profileNodesToObjectsSelected["customMetadataTypeAccesses"].Add(tnd2.FullPath);
                                    }
                                    else if (!profileNodesToObjectsSelected.ContainsKey("customMetadataTypeAccesses"))
                                    {
                                        profileNodesToObjectsSelected.Add("customMetadataTypeAccesses", new List<string> { tnd2.FullPath });
                                    }
                                }
                                //else if ()
                                //{
                                //     "customPermissions"
                                //}

                                // TODO: Custom Settings are part of the objects folder
                                //else if ()
                                //{
                                //     customSettingAccesses
                                //}
                                else if (parsedFullPath[0] == "flows")
                                {
                                    if (profileNodesToObjectsSelected.ContainsKey("flowAccesses")
                                        && !profileNodesToObjectsSelected["flowAccesses"].Contains(tnd2.FullPath))
                                    {
                                        profileNodesToObjectsSelected["flowAccesses"].Add(tnd2.FullPath);
                                    }
                                    else if (!profileNodesToObjectsSelected.ContainsKey("flowAccesses"))
                                    {
                                        profileNodesToObjectsSelected.Add("flowAccesses", new List<string> { tnd2.FullPath });
                                    }
                                }
                                else if (parsedFullPath[0] == "layouts")
                                {
                                    if (profileNodesToObjectsSelected.ContainsKey("layoutAssignments")
                                        && !profileNodesToObjectsSelected["layoutAssignments"].Contains(tnd2.FullPath))
                                    {
                                        profileNodesToObjectsSelected["layoutAssignments"].Add(tnd2.FullPath);
                                    }
                                    else if (!profileNodesToObjectsSelected.ContainsKey("layoutAssignments"))
                                    {
                                        profileNodesToObjectsSelected.Add("layoutAssignments", new List<string> { tnd2.FullPath });
                                    }
                                }
                                else if (parsedFullPath[0] == "pages")
                                {
                                    if (profileNodesToObjectsSelected.ContainsKey("pageAccesses")
                                        && !profileNodesToObjectsSelected["pageAccesses"].Contains(tnd2.FullPath))
                                    {
                                        profileNodesToObjectsSelected["pageAccesses"].Add(tnd2.FullPath);
                                    }
                                    else if (!profileNodesToObjectsSelected.ContainsKey("pageAccesses"))
                                    {
                                        profileNodesToObjectsSelected.Add("pageAccesses", new List<string> { tnd2.FullPath });
                                    }
                                }
                                else if (parsedFullPath[0] == "tabs")
                                {
                                    if (profileNodesToObjectsSelected.ContainsKey("tabVisibilities")
                                        && !profileNodesToObjectsSelected["tabVisibilities"].Contains(tnd2.FullPath))
                                    {
                                        profileNodesToObjectsSelected["tabVisibilities"].Add(tnd2.FullPath);
                                    }
                                    else if (!profileNodesToObjectsSelected.ContainsKey("tabVisibilities"))
                                    {
                                        profileNodesToObjectsSelected.Add("tabVisibilities", new List<string> { tnd2.FullPath });
                                    }
                                }

                                continue;
                            }
                        }
                    }
                }
            }

            //this.profilesSelectUserPermissions = true;
            //this.permissionSetsSelectUserPermissions = true;

            buildProfilePermissionSetVisibilities(profileNodesToObjectsSelected);
        }

        // Custom Objects will need to include the field permissions as well
        // compareDetails will be used to sift through only specific folders such as Objects to extract out the fields and record types
        // All others should just reference the folder and name
        private void buildProfilePermissionSetVisibilities(Dictionary<String, List<String>> profileNodesToObjectsSelected)
        {
            Int32 nodesCount = 0;

            //this.progressIndicator.Visible = true;
            //this.progressIndicator.Minimum = 1;
            //this.progressIndicator.Value = 1;
            //this.progressIndicator.Step = 1;

            foreach (String nodeKey in profileNodesToObjectsSelected.Keys)
            {
                foreach (String objectSelected in profileNodesToObjectsSelected[nodeKey])
                {
                    nodesCount++;
                }
            }

            //this.progressIndicator.Maximum = nodesCount;

            // Key = folder: i.e. profiles or permissionsets
            Dictionary<String, HashSet<String>> profilesPermissionSetsUpdated = new Dictionary<String, HashSet<String>>();

            foreach (String nodeKey in profileNodesToObjectsSelected.Keys)
            {
                //this.tbArrayValue.Text = "Profiles / Permission Sets - " + nodeKey;

                // TODO: Instead of calling the buildProfilePermissionSetVisibilities and traversing the tree for each and every treenode + subnode:
                // get the list of nodes and pass these in to the selectProfileNodes method
                // When the TreeNode hits the nodeKey, then you can start matching on the 

                // this is very slow due to having to traverse the treenode for each and every entry.
                foreach (String objectSelected in profileNodesToObjectsSelected[nodeKey])
                {
                    selectProfileNode(nodeKey, objectSelected, profilesPermissionSetsUpdated);
                    //this.progressIndicator.PerformStep();
                }
            }

            updateProfilePermissionSetUserPermission(profilesPermissionSetsUpdated);

            MessageBox.Show("Profiles and Permission Sets Updated");
        }

        private void selectProfileNode(String nodeKey, String nodeFullPath, Dictionary<String, HashSet<String>> profilesPermissionSetsUpdated)
        {
            // Custom objects will have objectPermissions, fieldPermissions, recordTypeVisibilities
            // Page Layouts will need to include layoutAssignments

            String[] parsedNodePath = nodeFullPath.Split('\\');
            String[] parsedObjectName = parsedNodePath[1].Split('.');

            foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
            {
                if (tnd1.Text == "profiles"
                    || tnd1.Text == "permissionsets")
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        foreach (TreeNode tnd3 in tnd2.Nodes)
                        {
                            String tnd3XmlString = "<document>" + tnd3.Text + "</document>";
                            XmlDocument tnd3Xd = new XmlDocument();
                            tnd3Xd.LoadXml(tnd3XmlString);

                            if (nodeKey == "applicationVisibilities"
                                && tnd3Xd.ChildNodes[0].ChildNodes[0].Name == "applicationVisibilities"
                                && tnd3Xd.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText == parsedObjectName[0])
                            {
                                tnd3.Checked = true;
                                if (profilesPermissionSetsUpdated.ContainsKey(tnd1.Text))
                                {
                                    profilesPermissionSetsUpdated[tnd1.Text].Add(tnd2.Text);
                                }
                                else
                                {
                                    profilesPermissionSetsUpdated.Add(tnd1.Text, new HashSet<string> { tnd1.Text });
                                }
                            }

                            //else if (nodeKey == "categoryGroupVisibilities")
                            //{

                            //}

                            else if (nodeKey == "classAccesses"
                                    && tnd3Xd.ChildNodes[0].ChildNodes[0].Name == "classAccesses"
                                    && tnd3Xd.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText == parsedObjectName[0])
                            {
                                tnd3.Checked = true;
                                if (profilesPermissionSetsUpdated.ContainsKey(tnd1.Text))
                                {
                                    profilesPermissionSetsUpdated[tnd1.Text].Add(tnd2.Text);
                                }
                                else
                                {
                                    profilesPermissionSetsUpdated.Add(tnd1.Text, new HashSet<string> { tnd1.Text });
                                }

                                continue;
                            }
                            //else if (nodeKey == "customMetadataTypeAccesses")
                            //{

                            //}
                            //else if (nodeKey == "customPermissions")
                            //{

                            //}
                            //else if (nodeKey == "customSettingAccesses")
                            //{

                            //}
                            //else if (nodeKey == "externalDataSourceAccesses")
                            //{

                            //}
                            //else if (nodeKey == "fieldLevelSecurities")
                            //{
                            //    // API 22.0 and earlier. Use fieldPermissions instead
                            //}
                            else if (nodeKey == "fieldPermissions"
                                        && tnd3Xd.ChildNodes[0].ChildNodes[0].Name == "fieldPermissions")
                            {
                                // Get the field name from the incoming XML passed into this method
                                // Append the object name and a '.'
                                // Result: objectName.fieldApiName
                                String incomingSelectedXml = "<document>" + parsedNodePath[2] + "</document>";
                                XmlDocument incomingSelectedDoc = new XmlDocument();
                                incomingSelectedDoc.LoadXml(incomingSelectedXml);

                                String fieldApiName = parsedObjectName[0] + "." + incomingSelectedDoc.ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;

                                if (tnd3Xd.ChildNodes[0].ChildNodes[0].ChildNodes[1].InnerText == fieldApiName)
                                {
                                    tnd3.Checked = true;
                                    if (profilesPermissionSetsUpdated.ContainsKey(tnd1.Text))
                                    {
                                        profilesPermissionSetsUpdated[tnd1.Text].Add(tnd2.Text);
                                    }
                                    else
                                    {
                                        profilesPermissionSetsUpdated.Add(tnd1.Text, new HashSet<string> { tnd1.Text });
                                    }
                                }
                            }
                            else if (nodeKey == "flowAccesses"
                                    && tnd3Xd.ChildNodes[0].ChildNodes[0].Name == "flowAccesses"
                                    && tnd3Xd.ChildNodes[0].ChildNodes[0].ChildNodes[1].ChildNodes[0].InnerText == parsedObjectName[0])
                            {
                                tnd3.Checked = true;
                                if (profilesPermissionSetsUpdated.ContainsKey(tnd1.Text))
                                {
                                    profilesPermissionSetsUpdated[tnd1.Text].Add(tnd2.Text);
                                }
                                else
                                {
                                    profilesPermissionSetsUpdated.Add(tnd1.Text, new HashSet<string> { tnd1.Text });
                                }
                            }
                            else if (nodeKey == "layoutAssignments"
                                     && tnd3Xd.ChildNodes[0].ChildNodes[0].Name == "layoutAssignments"
                                     && tnd3Xd.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText == parsedObjectName[0])
                            {
                                tnd3.Checked = true;
                                if (profilesPermissionSetsUpdated.ContainsKey(tnd1.Text))
                                {
                                    profilesPermissionSetsUpdated[tnd1.Text].Add(tnd2.Text);
                                }
                                else
                                {
                                    profilesPermissionSetsUpdated.Add(tnd1.Text, new HashSet<string> { tnd1.Text });
                                }
                            }
                            //else if (nodeKey == "loginFlows")
                            //{

                            //}
                            //else if (nodeKey == "loginHours")
                            //{

                            //}
                            //else if (nodeKey == "loginIpRanges")
                            //{

                            //}
                            else if (nodeKey == "objectPermissions"
                                        && tnd3Xd.ChildNodes[0].ChildNodes[0].Name == "objectPermissions"
                                        && tnd3Xd.ChildNodes[0].ChildNodes[0].ChildNodes[5].InnerText == parsedObjectName[0])
                            {
                                tnd3.Checked = true;
                                if (profilesPermissionSetsUpdated.ContainsKey(tnd1.Text))
                                {
                                    profilesPermissionSetsUpdated[tnd1.Text].Add(tnd2.Text);
                                }
                                else
                                {
                                    profilesPermissionSetsUpdated.Add(tnd1.Text, new HashSet<string> { tnd1.Text });
                                }
                            }
                            else if (nodeKey == "pageAccesses"
                                     && tnd3Xd.ChildNodes[0].ChildNodes[0].Name == "pageAccesses"
                                     && tnd3Xd.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText == parsedObjectName[0])
                            {
                                tnd3.Checked = true;
                                if (profilesPermissionSetsUpdated.ContainsKey(tnd1.Text))
                                {
                                    profilesPermissionSetsUpdated[tnd1.Text].Add(tnd2.Text);
                                }
                                else
                                {
                                    profilesPermissionSetsUpdated.Add(tnd1.Text, new HashSet<string> { tnd1.Text });
                                }
                            }
                            //else if (nodeKey == "profileActionOverrides")
                            //{

                            //}
                            else if (nodeKey == "recordTypeVisibilities"
                                        && tnd3Xd.ChildNodes[0].ChildNodes[0].Name == "recordTypeVisibilities")
                            {
                                String incomingSelectedXml = "<document>" + parsedNodePath[2] + "</document>";
                                XmlDocument incomingSelectedDoc = new XmlDocument();
                                incomingSelectedDoc.LoadXml(incomingSelectedXml);

                                String recordTypeApiName = parsedObjectName[0] + "." + incomingSelectedDoc.ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;

                                if (tnd3Xd.ChildNodes[0].ChildNodes[0].ChildNodes[1].InnerText == recordTypeApiName)
                                {
                                    tnd3.Checked = true;
                                    if (profilesPermissionSetsUpdated.ContainsKey(tnd1.Text))
                                    {
                                        profilesPermissionSetsUpdated[tnd1.Text].Add(tnd2.Text);
                                    }
                                    else
                                    {
                                        profilesPermissionSetsUpdated.Add(tnd1.Text, new HashSet<string> { tnd1.Text });
                                    }
                                }
                            }
                            else if (nodeKey == "tabVisibilities"
                                     && (tnd3Xd.ChildNodes[0].ChildNodes[0].Name == "tabVisibilities"
                                     || tnd3Xd.ChildNodes[0].ChildNodes[0].Name == "tabSettings")
                                     && tnd3Xd.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText == parsedObjectName[0])
                            {
                                tnd3.Checked = true;
                                if (profilesPermissionSetsUpdated.ContainsKey(tnd1.Text))
                                {
                                    profilesPermissionSetsUpdated[tnd1.Text].Add(tnd2.Text);
                                }
                                else
                                {
                                    profilesPermissionSetsUpdated.Add(tnd1.Text, new HashSet<string> { tnd1.Text });
                                }
                            }
                            // TODO: Move this processing to a different method. We only want to hit the profiles which have been updated
                            // 
                            //else if (nodeKey == "userPermissions"
                            //        && this.profilesSelectUserPermissions == true)
                            //{
                            //    tnd3.Checked = true;
                            //}
                        }
                    }
                }
            }
        }

        private void updateProfilePermissionSetUserPermission(Dictionary<String, HashSet<String>> profilesPermissionSetsUpdated)
        {
            foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
            {
                if ((tnd1.Text == "profiles"
                    || tnd1.Text == "permissionsets")
                    && profilesPermissionSetsUpdated.ContainsKey(tnd1.Text))
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (profilesPermissionSetsUpdated[tnd1.Text].Contains(tnd2.Text))
                        {
                            foreach (TreeNode tnd3 in tnd2.Nodes)
                            {
                                String tnd3XmlString = "<document>" + tnd3.Text + "</document>";
                                XmlDocument tnd3Xd = new XmlDocument();
                                tnd3Xd.LoadXml(tnd3XmlString);

                                if (tnd3Xd.ChildNodes[0].ChildNodes[0].Name == "custom")
                                {
                                    tnd3.Checked = true;
                                }
                                else if (tnd3Xd.ChildNodes[0].ChildNodes[0].Name == "description")
                                {
                                    tnd3.Checked = true;
                                }
                                else if (tnd3Xd.ChildNodes[0].ChildNodes[0].Name == "fullName")
                                {
                                    tnd3.Checked = true;
                                }
                                else if (tnd3Xd.ChildNodes[0].ChildNodes[0].Name == "hasActivationRequired")
                                {
                                    tnd3.Checked = true;
                                }
                                else if (tnd3Xd.ChildNodes[0].ChildNodes[0].Name == "label")
                                {
                                    tnd3.Checked = true;
                                }
                                else if (tnd3Xd.ChildNodes[0].ChildNodes[0].Name == "license")
                                {
                                    tnd3.Checked = true;
                                }
                                else if (tnd3Xd.ChildNodes[0].ChildNodes[0].Name == "userLicense")
                                {
                                    tnd3.Checked = true;
                                }
                                else if (tnd3Xd.ChildNodes[0].ChildNodes[0].Name == "userPermissions")
                                {
                                    tnd3.Checked = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void btnBuildPackageXml_Click(object sender, EventArgs e)
        {
            if (this.tbDeployFrom.Text == "")
            {
                MessageBox.Show("Please choose a location to save the deployment package contents to");
                return;
            }

            // Example: CustomObject -> Account -> fields
            //String xmlHeaderLine = "<?xml version =\"1.0\" encoding=\"UTF-8\"?>";

            if (this.cmbDestructiveChange.Text == "--none--")
            {
                String zipFilePath = ZipFileWithPackageXML.buildZipFileWithPackageXml(this.treeViewMetadata.Nodes,
                                                                                      "",
                                                                                      this.tbDeployFrom.Text,
                                                                                      this.tbProjectFolder.Text);

                DeployMetadata dm = new DeployMetadata();
                dm.tbZipFileLocation.Text = zipFilePath;
                dm.cbCheckOnly.CheckState = CheckState.Checked;
                dm.cbCheckOnly.Checked = true;

                dm.Show();

            }
            //else if (this.cmbDestructiveChange.Text == "destructiveChanges")
            //{
            //    StreamWriter swPackageXml = new StreamWriter(this.tbDeploymentPackageLocation.Text + "\\package.xml");
            //    swPackageXml.WriteLine(xmlHeaderLine);
            //    swPackageXml.WriteLine("<Package xmlns=\"http://soap.sforce.com/2006/04/metadata\">");
            //    swPackageXml.WriteLine("<version>" + this.cmbDefaultAPI.Text + "</version>");
            //    swPackageXml.WriteLine("</Package>");
            //    swPackageXml.Close();

            //    buildPackageObjectMembers(packageXmlDestructiveChangeMembers);
            //    buildDestructivePackageXmlFile(packageXmlDestructiveChangeMembers, this.cmbDestructiveChange.Text);
            //}
            //else if (this.cmbDestructiveChange.Text == "destructiveChangesPre")
            //{
            //    // Build the destructive changes first
            //    // Then pre-populate the Tree View with the pre-checked related items which need to be reviewed from the
            //    // selected items in the destructive changes
            //    // Example: If you select a custom field to delete, then clicking Next will populate the Tree View with all
            //    // Items which include that field
            //    buildPackageObjectMembers(packageXmlObjectMembers);
            //    buildDestructivePackageXmlFile(packageXmlDestructiveChangeMembers, this.cmbDestructiveChange.Text);
            //}
            //else if (this.cmbDestructiveChange.Text == "destructiveChangesPost")
            //{
            //    // Build the destructive changes first
            //    // Then pre-populate the Tree View with the pre-checked related items which need to be reviewed from the
            //    // selected items in the destructive changes
            //    // Example: If you select a custom field to delete, then clicking Next will populate the Tree View with all
            //    // Items which include that field
            //    buildPackageObjectMembers(packageXmlObjectMembers);
            //    buildDestructivePackageXmlFile(packageXmlDestructiveChangeMembers, this.cmbDestructiveChange.Text);
            //}

            MessageBox.Show("Package XML File Built");
        }

        public String buildXmlFile(String topNodeName, String nodeFullPath)
        {
            String xmlFile = "";

            foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
            {
                if (tnd1.Text == topNodeName)
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (tnd2.Text == nodeFullPath)
                        {
                            foreach (TreeNode tnd3 in tnd2.Nodes)
                            {
                                if (tnd3.Nodes.Count > 0)
                                {
                                    xmlFile = xmlFile + "<" + tnd3.Text + ">";

                                    foreach (TreeNode tnd4 in tnd3.Nodes)
                                    {
                                        if (tnd4.Nodes.Count > 0)
                                        {
                                            String[] tnd4TextSplit = tnd4.Text.Split('|');
                                            xmlFile = xmlFile + "<" + tnd4TextSplit[0].Trim() + ">";

                                            foreach (TreeNode tnd5 in tnd4.Nodes)
                                            {
                                                if (tnd5.Nodes.Count > 0)
                                                {
                                                    String[] tnd5TextSplit = tnd5.Text.Split('|');
                                                    xmlFile = xmlFile + "<" + tnd5TextSplit[0].Trim() + ">";

                                                    foreach (TreeNode tnd6 in tnd5.Nodes)
                                                    {
                                                        xmlFile = xmlFile + tnd6.Text;
                                                    }

                                                    xmlFile = xmlFile + "</" + tnd5TextSplit[0].Trim() + ">";
                                                }
                                                else
                                                {
                                                    xmlFile = xmlFile + tnd5.Text;
                                                }
                                            }

                                            xmlFile = xmlFile + "</" + tnd4TextSplit[0].Trim() + ">";
                                        }
                                        else
                                        {
                                            xmlFile = xmlFile + tnd4.Text;
                                        }
                                    }

                                    xmlFile = xmlFile + "</" + tnd3.Text + ">";
                                }
                                else
                                {
                                    // TODO: Add error handling
                                }
                            }
                        }
                    }
                }
            }

            return xmlFile;
        }

        public void buildDestructivePackageXmlFile(Dictionary<String, HashSet<String>> packageXmlObjectMembers, String destructivePackageType)
        {
            String xmlHeaderLine = "<?xml version =\"1.0\" encoding=\"UTF-8\"?>";

            StreamWriter swDestructivePackageXml = new StreamWriter(this.tbDeployFrom.Text + "\\" + destructivePackageType + ".xml");
            swDestructivePackageXml.WriteLine(xmlHeaderLine);
            swDestructivePackageXml.WriteLine("<Package xmlns=\"http://soap.sforce.com/2006/04/metadata\">");

            foreach (String objectType in packageXmlObjectMembers.Keys)
            {
                swDestructivePackageXml.WriteLine("<types>");

                foreach (String memberType in packageXmlObjectMembers[objectType])
                {
                    swDestructivePackageXml.WriteLine("<members>" + memberType + "</members>");
                }

                swDestructivePackageXml.WriteLine("<name>" + objectType + "</name>");
                swDestructivePackageXml.WriteLine("</types>");
            }

            swDestructivePackageXml.WriteLine("</Package>");
            swDestructivePackageXml.Close();
        }

        private void cmbDestructiveChange_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbDestructiveChange.Text == "--none--")
            {
                this.lblDestructiveChangesFirst.Visible = false;
                this.btnBuildProfilesAndPermissionSets.Visible = true;
                this.btnBuildZipFile.Visible = true;
                //this.btnNext.Visible = false;
                this.runTreeNodeSelector = true;
            }
            else if (this.cmbDestructiveChange.Text == "destructiveChanges")
            {
                this.lblDestructiveChangesFirst.Visible = false;
                this.btnBuildProfilesAndPermissionSets.Visible = false;
                this.btnBuildZipFile.Visible = true;
                //this.btnNext.Visible = false;
                this.runTreeNodeSelector = false;
            }
            else
            {
                this.lblDestructiveChangesFirst.Visible = true;
                this.btnBuildProfilesAndPermissionSets.Visible = false;
                this.btnBuildZipFile.Visible = false;
                //this.btnNext.Visible = true;
                this.runTreeNodeSelector = false;
            }
        }

        //private void btnNext_Click(object sender, EventArgs e)
        //{
        ////    this.runTreeNodeSelector = true;

        ////    // Build the destructive changes first
        ////    // Then pre-populate the Tree View with the pre-checked related items which need to be reviewed from the
        ////    // selected items in the destructive changes
        ////    // Example: If you select a custom field to delete, then clicking Next will populate the Tree View with all
        ////    // Items which include that field

        ////    // Loop through the selected items and build the Dictionary / HashSet list

        ////    buildPackageObjectMembers(packageXmlDestructiveChangeMembers);

        ////    populateMetadataTreeView();
        //}

        public void loadDefaultApis()
        {
            foreach (String api in UtilityClass.generateAPIArray())
            {
                this.cmbDefaultAPI.Items.Add(api);
            }

            this.cmbDefaultAPI.Text = Properties.Settings.Default.DefaultAPI;
        }

        private void tbOutboundChangeSetName_MouseHover(object sender, EventArgs e)
        {
            System.Windows.Forms.TextBox TB = (System.Windows.Forms.TextBox)sender;
            int VisibleTime = 10000;  //in milliseconds

            ToolTip tt = new ToolTip();
            tt.Show("If you have a open Salesforce Outbound Change Set, add the Change Set Name here. When deployed, your changes will be added to the change set.", TB, 0, 0, VisibleTime);
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
    }
}
