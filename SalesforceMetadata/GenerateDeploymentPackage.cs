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

using SalesforceMetadata.PartnerWSDL;
using SalesforceMetadata.MetadataWSDL;
using SalesforceMetadata.ToolingWSDL;

namespace SalesforceMetadata
{
    public partial class GenerateDeploymentPackage : Form
    {
        private Boolean runTreeNodeSelector = true;

        private Dictionary<String, HashSet<String>> packageXmlObjectMembers = new Dictionary<String, HashSet<String>>();
        private Dictionary<String, HashSet<String>> packageXmlDestructiveChangeMembers = new Dictionary<String, HashSet<String>>();

        private HashSet<String> standardValueSets = new HashSet<string>();

        public TreeNodeCollection treeNodeCollFromDiff;


        public GenerateDeploymentPackage()
        {
            InitializeComponent();
            loadDefaultApis();
            formSizeResolution();
        }

        private void formSizeResolution()
        {
            Rectangle rect = Screen.FromControl(this).Bounds;
            if (rect.Width < 1200)
            {
                this.ClientSize = new System.Drawing.Size(rect.Size.Width - 100, rect.Size.Height - 100);
                this.treeViewMetadata.Size = new System.Drawing.Size(rect.Size.Width - 150, rect.Size.Height - 350);
            }
        }

        private void tbDeploymentPackageLocation_DoubleClick(object sender, EventArgs e)
        {
            this.tbDeploymentPackageLocation.Text = UtilityClass.folderBrowserSelectPath("Select folder to save Deployment items to", true, FolderEnum.SaveTo);

            Boolean isEmpty = true;
            String[] dirs = Directory.GetDirectories(this.tbDeploymentPackageLocation.Text);
            String[] fls = Directory.GetFiles(this.tbDeploymentPackageLocation.Text);

            if (dirs.Length > 0 || fls.Length > 0) isEmpty = false;

            if (isEmpty == false)
            {
                // read from the items and mark the deployable items
                // Which means you have to open the XML files and 

            }
        }

        private void tbMetadataFolderToReadFrom_DoubleClick(object sender, EventArgs e)
        {
            this.tbMetadataFolderToReadFrom.Text = UtilityClass.folderBrowserSelectPath("Select folder to read the Deployment items to", true, FolderEnum.SaveTo);

            if (this.tbMetadataFolderToReadFrom.Text != "")
            {
                populateMetadataTreeView();
            }
        }

        public void populateMetadataTreeView()
        {
            this.treeViewMetadata.Nodes.Clear();

            String[] folders = Directory.GetDirectories(this.tbMetadataFolderToReadFrom.Text);
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

        public void selectDefaultsFromDiff()
        {
            foreach (TreeNode tnd1 in treeNodeCollFromDiff)
            {
                if (tnd1.Text == "permissionsets"
                    || tnd1.Text == "profiles")
                {
                    continue;
                }

                // Get the incoming node text from the Differences.
                foreach (TreeNode tnd2 in tnd1.Nodes)
                {
                    String tnd2Text = "";

                    if (tnd2.Text.StartsWith("[New]"))
                    {
                        tnd2Text = tnd2.Text.Substring(6, tnd2.Text.Length - 6);
                    }
                    else if (tnd2.Text.StartsWith("[Updated]"))
                    {
                        tnd2Text = tnd2.Text.Substring(10, tnd2.Text.Length - 10);
                    }
                    else
                    {
                        tnd2Text = tnd2.Text;
                    }


                    if (tnd1.Text == "objects")
                    {
                        foreach (TreeNode tnd3 in tnd2.Nodes)
                        {
                            foreach (TreeNode tnd4 in tnd3.Nodes)
                            {
                                foreach (TreeNode tnd5 in tnd4.Nodes)
                                {
                                    // Actual Name with [Updated] or [New]
                                    String tnd5Text = "";

                                    if (tnd5.Text.StartsWith("[New]"))
                                    {
                                        tnd5Text = tnd5.Text.Substring(6, tnd5.Text.Length - 6);
                                    }
                                    else if (tnd5.Text.StartsWith("[Updated]"))
                                    {
                                        tnd5Text = tnd5.Text.Substring(10, tnd5.Text.Length - 10);
                                    }
                                    else
                                    {
                                        tnd5Text = tnd5.Text;
                                    }

                                    // Now find it in the treeViewMetadata and set the Checked state to true
                                    foreach (TreeNode mdTnd1 in this.treeViewMetadata.Nodes)
                                    {
                                        if (mdTnd1.Text == "objects")
                                        {
                                            foreach (TreeNode mdTnd2 in mdTnd1.Nodes)
                                            {
                                                if (mdTnd2.Text == tnd2Text)
                                                {
                                                    foreach (TreeNode mdTnd3 in mdTnd2.Nodes)
                                                    {
                                                        // Load the XML from mdTnd3 to determine the name of the object types
                                                        XmlDocument xd = new XmlDocument();
                                                        xd.LoadXml("<document>" + mdTnd3.Text + "</document>");

                                                        // Get the Name based on the object element type to see if it is in the differences
                                                        // coming over from the difference report.
                                                        String nodeBlockNameValue = MetadataDifferenceProcessing.getNameField("CustomObject", xd.ChildNodes[0].ChildNodes[0].Name, xd.ChildNodes[0].ChildNodes[0].OuterXml);

                                                        if (nodeBlockNameValue == "")
                                                        {
                                                            nodeBlockNameValue = xd.ChildNodes[0].ChildNodes[0].Name;
                                                        }

                                                        if (nodeBlockNameValue == tnd5Text)
                                                        {
                                                            mdTnd3.Checked = true;
                                                        }
                                                        else if (nodeBlockNameValue == tnd2Text)
                                                        {
                                                            mdTnd3.Checked = true;
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
                    else
                    {
                        foreach (TreeNode mdTnd1 in this.treeViewMetadata.Nodes)
                        {
                            if (mdTnd1.Text == tnd1.Text)
                            {
                                foreach (TreeNode mdTnd2 in mdTnd1.Nodes)
                                {
                                    if (mdTnd2.Text == tnd2Text)
                                    {
                                        mdTnd2.Checked = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void defaultSelectedFromMetadataComp()
        {
            if (runTreeNodeSelector == true)
            {
                runTreeNodeSelector = false;
            }
        }

        private void treeViewMetadata_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (runTreeNodeSelector == true)
            {
                runTreeNodeSelector = false;

                String[] nodeFullPath = e.Node.FullPath.Split('\\');
                String[] objectName = new string[2];
                String[] fileNameSplit = nodeFullPath[1].Split('.');

                if (nodeFullPath.Length > 1)
                {
                    objectName = nodeFullPath[1].Split('.');
                }

                // TODO:
                // If a standard picklist field is selected, make sure to also select the related StandardValueSet. You will need a separate method for determining
                //      if a field selected translates to a StandardValueSet

                if (e.Node.Checked == true)
                {
                    if (nodeFullPath.Length > 2
                        && nodeFullPath[0] == "aura")
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
                    else if (nodeFullPath.Length > 2
                             && nodeFullPath[0] == "lwc")
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
                    else if (nodeFullPath.Length > 2
                        && nodeFullPath[0] == "objects"
                        && nodeFullPath[1].Contains("__c"))
                    {
                        selectRequiredObjectFields(nodeFullPath[1]);
                    }
                    else if (nodeFullPath.Length > 2
                            && nodeFullPath[0] == "objects")
                    {
                        if (e.Node.Text.StartsWith("<fields"))
                        {
                            String tnd3XmlString = "<document>" + e.Node.Text + "</document>";
                            XmlDocument tnd3Xd = new XmlDocument();
                            tnd3Xd.LoadXml(tnd3XmlString);

                            String objectFieldCombo = objectName[0] + "." + tnd3Xd.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;

                            populateStandardValueSetHashSet(objectFieldCombo);
                        }
                    }
                    else if (nodeFullPath.Length == 2
                        && nodeFullPath[0] == "objects"
                        && nodeFullPath[1].Contains("__mdt"))
                    {
                        // First check off the sub-nodes from the parent
                        foreach (TreeNode tnd3 in e.Node.Nodes)
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
                    else if (nodeFullPath.Length == 2
                            && nodeFullPath[0] == "certs")
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
                    else if (nodeFullPath.Length == 2
                            && nodeFullPath[0] == "classes")
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
                                    }
                                }
                            }
                        }
                    }
                    else if (nodeFullPath.Length == 2
                            && nodeFullPath[0] == "components")
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
                    else if (nodeFullPath.Length == 2
                             && nodeFullPath[0] == "contentassets")
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
                    else if (nodeFullPath.Length == 2
                            && nodeFullPath[0] == "pages")
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
                    else if (nodeFullPath.Length == 2
                            && nodeFullPath[0] == "staticresources")
                    {
                        // Get the class name and then make sure the XML file is checked too
                        foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
                        {
                            if (tnd1.Text == "staticresources")
                            {
                                foreach (TreeNode tnd2 in tnd1.Nodes)
                                {
                                    String[] tnd2FileNameSplit = tnd2.Text.Split('.');

                                    if (tnd2FileNameSplit[0] == fileNameSplit[0]
                                        && tnd2.Text.EndsWith("-meta.xml"))
                                    {
                                        tnd2.Checked = true;
                                    }
                                }
                            }
                        }
                    }
                    else if (nodeFullPath.Length == 2
                            && nodeFullPath[0] == "triggers")
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
                    //else if (nodeFullPath[0] == "flexipages")
                    //{ 
                    //    String objectType = 
                    //}
                    else if (nodeFullPath.Length == 2)
                    {
                        foreach (TreeNode tnd3 in e.Node.Nodes)
                        {
                            tnd3.Checked = true;
                        }
                    }
                }
                else if (nodeFullPath.Length == 2
                       && e.Node.Checked == false)
                {
                    foreach (TreeNode tnd3 in e.Node.Nodes)
                    {
                        tnd3.Checked = false;
                    }
                }

                if (standardValueSets.Count > 0)
                {
                    selectStandardValueSets(standardValueSets);
                }

                runTreeNodeSelector = true;
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
                                if (tnd3.Text.StartsWith("<deploymentStatus"))
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

                            foreach (TreeNode tnd3 in tnd2.Nodes)
                            {
                                tnd3.Checked = true;
                            }
                        }
                    }
                }
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
                                if (tnd3.Text.StartsWith("<fields"))
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
                                else if (tnd3.Text.StartsWith("<recordTypes"))
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
            if (this.tbDeploymentPackageLocation.Text == "")
            {
                MessageBox.Show("Please choose a location to save the deployment package contents to");
                return;
            }

            // Example: CustomObject -> Account -> fields
            String xmlHeaderLine = "<?xml version =\"1.0\" encoding=\"UTF-8\"?>";

            if (this.cmbDestructiveChange.Text == "--none--")
            {
                buildDeploymentPackageFiles(packageXmlObjectMembers);
                buildPackageXmlFile(packageXmlObjectMembers);
            }
            else if (this.cmbDestructiveChange.Text == "destructiveChanges")
            {
                StreamWriter swPackageXml = new StreamWriter(this.tbDeploymentPackageLocation.Text + "\\package.xml");
                swPackageXml.WriteLine(xmlHeaderLine);
                swPackageXml.WriteLine("<Package xmlns=\"http://soap.sforce.com/2006/04/metadata\">");
                swPackageXml.WriteLine("<version>" + this.cmbDefaultAPI.Text + "</version>");
                swPackageXml.WriteLine("</Package>");
                swPackageXml.Close();

                buildPackageObjectMembers(packageXmlDestructiveChangeMembers);
                buildDestructivePackageXmlFile(packageXmlDestructiveChangeMembers, this.cmbDestructiveChange.Text);
            }
            else if (this.cmbDestructiveChange.Text == "destructiveChangesPre")
            {
                // Build the destructive changes first
                // Then pre-populate the Tree View with the pre-checked related items which need to be reviewed from the
                // selected items in the destructive changes
                // Example: If you select a custom field to delete, then clicking Next will populate the Tree View with all
                // Items which include that field
                buildPackageObjectMembers(packageXmlObjectMembers);
                buildDestructivePackageXmlFile(packageXmlDestructiveChangeMembers, this.cmbDestructiveChange.Text);
            }
            else if (this.cmbDestructiveChange.Text == "destructiveChangesPost")
            {
                // Build the destructive changes first
                // Then pre-populate the Tree View with the pre-checked related items which need to be reviewed from the
                // selected items in the destructive changes
                // Example: If you select a custom field to delete, then clicking Next will populate the Tree View with all
                // Items which include that field
                buildPackageObjectMembers(packageXmlObjectMembers);
                buildDestructivePackageXmlFile(packageXmlDestructiveChangeMembers, this.cmbDestructiveChange.Text);
            }

            MessageBox.Show("Package XML File Built");
        }

        public void buildDeploymentPackageFiles(Dictionary<String, HashSet<String>> packageXmlObjectMembers)
        {
            String xmlHeaderLine = "<?xml version =\"1.0\" encoding=\"UTF-8\"?>";

            Boolean writeClosingTag = false;
            String parentNode = "";
            String directoryName = "";
            String objectFileName = "";

            foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
            {
                if (tnd1.Text == "aura")
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (tnd2.Checked == true)
                        {
                            if (!Directory.Exists(this.tbDeploymentPackageLocation.Text + "\\aura"))
                            {
                                Directory.CreateDirectory(this.tbDeploymentPackageLocation.Text + "\\aura");
                            }

                            Directory.CreateDirectory(this.tbDeploymentPackageLocation.Text + "\\aura\\" + tnd2.Text);
                            DirectoryInfo dirInfo = new DirectoryInfo(this.tbMetadataFolderToReadFrom.Text + "\\aura\\" + tnd2.Text);
                            FileInfo[] auraSubdirFiles = dirInfo.GetFiles();

                            foreach (FileInfo file in auraSubdirFiles)
                            {
                                file.CopyTo(this.tbDeploymentPackageLocation.Text + "\\aura\\" + tnd2.Text + "\\" + file.Name, true);
                            }

                            if (packageXmlObjectMembers.ContainsKey("AuraDefinitionBundle"))
                            {
                                packageXmlObjectMembers["AuraDefinitionBundle"].Add(tnd2.Text);
                            }
                            else
                            {
                                packageXmlObjectMembers.Add("AuraDefinitionBundle", new HashSet<string> { tnd2.Text });
                            }
                        }
                    }
                }
                else if (tnd1.Text == "certs")
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (tnd2.Checked == true)
                        {
                            String[] objectNameSplit = tnd2.Text.Split('.');

                            if (!Directory.Exists(this.tbDeploymentPackageLocation.Text + "\\certs"))
                            {
                                Directory.CreateDirectory(this.tbDeploymentPackageLocation.Text + "\\certs");
                            }

                            FileInfo file = new FileInfo(this.tbMetadataFolderToReadFrom.Text + "\\certs\\" + tnd2.Text);
                            file.CopyTo(this.tbDeploymentPackageLocation.Text + "\\certs\\" + file.Name, true);

                            if (packageXmlObjectMembers.ContainsKey("Certificate"))
                            {
                                packageXmlObjectMembers["Certificate"].Add(objectNameSplit[0]);
                            }
                            else
                            {
                                packageXmlObjectMembers.Add("Certificate", new HashSet<string> { objectNameSplit[0] });
                            }
                        }
                    }
                }
                else if (tnd1.Text == "classes")
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (tnd2.Checked == true)
                        {
                            String[] objectNameSplit = tnd2.Text.Split('.');

                            if (!Directory.Exists(this.tbDeploymentPackageLocation.Text + "\\classes"))
                            {
                                Directory.CreateDirectory(this.tbDeploymentPackageLocation.Text + "\\classes");
                            }

                            FileInfo file = new FileInfo(this.tbMetadataFolderToReadFrom.Text + "\\classes\\" + tnd2.Text);
                            file.CopyTo(this.tbDeploymentPackageLocation.Text + "\\classes\\" + file.Name, true);

                            if (packageXmlObjectMembers.ContainsKey("ApexClass"))
                            {
                                packageXmlObjectMembers["ApexClass"].Add(objectNameSplit[0]);
                            }
                            else
                            {
                                packageXmlObjectMembers.Add("ApexClass", new HashSet<string> { objectNameSplit[0] });
                            }
                        }
                    }
                }
                else if (tnd1.Text == "components")
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (tnd2.Checked == true)
                        {
                            String[] objectNameSplit = tnd2.Text.Split('.');

                            if (!Directory.Exists(this.tbDeploymentPackageLocation.Text + "\\components"))
                            {
                                Directory.CreateDirectory(this.tbDeploymentPackageLocation.Text + "\\components");
                            }

                            FileInfo file = new FileInfo(this.tbMetadataFolderToReadFrom.Text + "\\components\\" + tnd2.Text);
                            file.CopyTo(this.tbDeploymentPackageLocation.Text + "\\components\\" + file.Name, true);

                            if (packageXmlObjectMembers.ContainsKey("ApexComponent"))
                            {
                                packageXmlObjectMembers["ApexComponent"].Add(objectNameSplit[0]);
                            }
                            else
                            {
                                packageXmlObjectMembers.Add("ApexComponent", new HashSet<string> { objectNameSplit[0] });
                            }
                        }
                    }
                }
                else if (tnd1.Text == "contentassets")
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (tnd2.Checked == true)
                        {
                            String[] objectNameSplit = tnd2.Text.Split('.');

                            if (!Directory.Exists(this.tbDeploymentPackageLocation.Text + "\\contentassets"))
                            {
                                Directory.CreateDirectory(this.tbDeploymentPackageLocation.Text + "\\contentassets");
                            }

                            FileInfo file = new FileInfo(this.tbMetadataFolderToReadFrom.Text + "\\contentassets\\" + tnd2.Text);
                            file.CopyTo(this.tbDeploymentPackageLocation.Text + "\\contentassets\\" + file.Name, true);

                            if (packageXmlObjectMembers.ContainsKey("ContentAsset"))
                            {
                                packageXmlObjectMembers["ContentAsset"].Add(objectNameSplit[0]);
                            }
                            else
                            {
                                packageXmlObjectMembers.Add("ContentAsset", new HashSet<string> { objectNameSplit[0] });
                            }
                        }
                    }
                }
                else if (tnd1.Text == "lwc")
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (tnd2.Checked == true)
                        {
                            if (tnd2.Checked == true)
                            {
                                if (!Directory.Exists(this.tbDeploymentPackageLocation.Text + "\\lwc"))
                                {
                                    Directory.CreateDirectory(this.tbDeploymentPackageLocation.Text + "\\lwc");
                                }

                                Directory.CreateDirectory(this.tbDeploymentPackageLocation.Text + "\\lwc\\" + tnd2.Text);
                                DirectoryInfo dirInfo = new DirectoryInfo(this.tbMetadataFolderToReadFrom.Text + "\\lwc\\" + tnd2.Text);
                                FileInfo[] auraSubdirFiles = dirInfo.GetFiles();

                                foreach (FileInfo file in auraSubdirFiles)
                                {
                                    file.CopyTo(this.tbDeploymentPackageLocation.Text + "\\lwc\\" + tnd2.Text + "\\" + file.Name, true);
                                }

                                if (packageXmlObjectMembers.ContainsKey("LightningComponentBundle"))
                                {
                                    packageXmlObjectMembers["LightningComponentBundle"].Add(tnd2.Text);
                                }
                                else
                                {
                                    packageXmlObjectMembers.Add("LightningComponentBundle", new HashSet<string> { tnd2.Text });
                                }
                            }
                        }
                    }
                }
                else if (tnd1.Text == "pages")
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (tnd2.Checked == true)
                        {
                            String[] objectNameSplit = tnd2.Text.Split('.');

                            if (!Directory.Exists(this.tbDeploymentPackageLocation.Text + "\\pages"))
                            {
                                Directory.CreateDirectory(this.tbDeploymentPackageLocation.Text + "\\pages");
                            }

                            FileInfo file = new FileInfo(this.tbMetadataFolderToReadFrom.Text + "\\pages\\" + tnd2.Text);
                            file.CopyTo(this.tbDeploymentPackageLocation.Text + "\\pages\\" + file.Name, true);

                            if (packageXmlObjectMembers.ContainsKey("ApexPage"))
                            {
                                packageXmlObjectMembers["ApexPage"].Add(objectNameSplit[0]);
                            }
                            else
                            {
                                packageXmlObjectMembers.Add("ApexPage", new HashSet<string> { objectNameSplit[0] });
                            }
                        }
                    }
                }
                else if (tnd1.Text == "staticresources")
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (tnd2.Checked == true)
                        {
                            String[] objectNameSplit = tnd2.Text.Split('.');

                            if (!Directory.Exists(this.tbDeploymentPackageLocation.Text + "\\staticresources"))
                            {
                                Directory.CreateDirectory(this.tbDeploymentPackageLocation.Text + "\\staticresources");
                            }

                            FileInfo file = new FileInfo(this.tbMetadataFolderToReadFrom.Text + "\\staticresources\\" + tnd2.Text);
                            file.CopyTo(this.tbDeploymentPackageLocation.Text + "\\staticresources\\" + file.Name, true);

                            if (packageXmlObjectMembers.ContainsKey("StaticResource"))
                            {
                                packageXmlObjectMembers["StaticResource"].Add(objectNameSplit[0]);
                            }
                            else
                            {
                                packageXmlObjectMembers.Add("StaticResource", new HashSet<string> { objectNameSplit[0] });
                            }
                        }
                    }
                }
                else if (tnd1.Text == "triggers")
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (tnd2.Checked == true)
                        {
                            String[] objectNameSplit = tnd2.Text.Split('.');

                            if (!Directory.Exists(this.tbDeploymentPackageLocation.Text + "\\triggers"))
                            {
                                Directory.CreateDirectory(this.tbDeploymentPackageLocation.Text + "\\triggers");
                            }

                            FileInfo file = new FileInfo(this.tbMetadataFolderToReadFrom.Text + "\\triggers\\" + tnd2.Text);
                            file.CopyTo(this.tbDeploymentPackageLocation.Text + "\\triggers\\" + file.Name, true);

                            if (packageXmlObjectMembers.ContainsKey("ApexTrigger"))
                            {
                                packageXmlObjectMembers["ApexTrigger"].Add(objectNameSplit[0]);
                            }
                            else
                            {
                                packageXmlObjectMembers.Add("ApexTrigger", new HashSet<string> { objectNameSplit[0] });
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
                                writeClosingTag = true;

                                String[] parsedFullPath = tnd3.FullPath.Split('\\');
                                directoryName = parsedFullPath[0];
                                objectFileName = parsedFullPath[1];

                                String[] objectNameSplit = parsedFullPath[1].Split('.');

                                // Handle Fields and Objects differently as you will need to list out the fields and objects in 
                                // the packageXml file since you are generating those individually along with the permission sets and profiles.

                                // Write the XML to the object file
                                if (parsedFullPath.Length == 3)
                                {
                                    // Package XML Dictionary build
                                    parentNode = MetadataDifferenceProcessing.folderToType(parsedFullPath[0], "");

                                    if (parsedFullPath[0] == "objects"
                                        && parsedFullPath[2].StartsWith("<fields"))
                                    {
                                        String xmlString = "<document>" + parsedFullPath[2] + "</document>";
                                        XmlDocument xd = new XmlDocument();
                                        xd.LoadXml(xmlString);

                                        String objectFieldCombo = objectNameSplit[0] + "." + xd.ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;

                                        // Add the custom field to the dictionary
                                        if (packageXmlObjectMembers.ContainsKey("CustomField"))
                                        {
                                            packageXmlObjectMembers["CustomField"].Add(objectFieldCombo);
                                        }
                                        else
                                        {
                                            packageXmlObjectMembers.Add("CustomField", new HashSet<string> { objectFieldCombo });
                                        }

                                        // Add the custom object 
                                        if (packageXmlObjectMembers.ContainsKey(parentNode))
                                        {
                                            packageXmlObjectMembers[parentNode].Add(objectNameSplit[0]);
                                        }
                                        else
                                        {
                                            packageXmlObjectMembers.Add(parentNode, new HashSet<string> { objectNameSplit[0] });
                                        }

                                    }
                                    else
                                    {
                                        if (packageXmlObjectMembers.ContainsKey(parentNode))
                                        {
                                            packageXmlObjectMembers[parentNode].Add(objectNameSplit[0]);
                                        }
                                        else
                                        {
                                            packageXmlObjectMembers.Add(parentNode, new HashSet<string> { objectNameSplit[0] });
                                        }
                                    }

                                    // Write selected contents to deployable xml file
                                    if (Directory.Exists(this.tbDeploymentPackageLocation.Text + "\\" + parsedFullPath[0]))
                                    {
                                        if (File.Exists(this.tbDeploymentPackageLocation.Text + "\\" + parsedFullPath[0] + "\\" + parsedFullPath[1]))
                                        {
                                            StreamWriter sw = new StreamWriter(this.tbDeploymentPackageLocation.Text + "\\" + parsedFullPath[0] + "\\" + parsedFullPath[1], true);
                                            sw.WriteLine(parsedFullPath[2]);
                                            sw.Close();
                                        }
                                        else
                                        {
                                            StreamWriter sw = new StreamWriter(this.tbDeploymentPackageLocation.Text + "\\" + parsedFullPath[0] + "\\" + parsedFullPath[1], false);
                                            sw.WriteLine(xmlHeaderLine);
                                            sw.WriteLine("<" + parentNode + ">");

                                            sw.WriteLine(parsedFullPath[2]);
                                            sw.Close();
                                        }
                                    }
                                    else
                                    {
                                        Directory.CreateDirectory(this.tbDeploymentPackageLocation.Text + "\\" + parsedFullPath[0]);
                                        StreamWriter sw = new StreamWriter(this.tbDeploymentPackageLocation.Text + "\\" + parsedFullPath[0] + "\\" + parsedFullPath[1], false);
                                        sw.WriteLine(xmlHeaderLine);
                                        sw.WriteLine("<" + parentNode + ">");

                                        sw.WriteLine(parsedFullPath[2]);
                                        sw.Close();
                                    }
                                }
                            }
                        }

                        if (writeClosingTag == true)
                        {
                            StreamWriter sw = new StreamWriter(this.tbDeploymentPackageLocation.Text + "\\" + directoryName + "\\" + objectFileName, true);
                            sw.WriteLine("</" + parentNode + ">");
                            sw.Close();

                            writeClosingTag = false;
                            parentNode = "";
                            directoryName = "";
                            objectFileName = "";
                        }
                    }
                }

                parentNode = "";
                directoryName = "";
                objectFileName = "";
            }
        }

        public void buildPackageObjectMembers(Dictionary<String, HashSet<String>> packageXmlObjectMembers)
        {
            String parentNode = "";
            String directoryName = "";
            String objectFileName = "";

            foreach (TreeNode tnd1 in this.treeViewMetadata.Nodes)
            {
                if (tnd1.Text == "aura")
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (tnd2.Checked == true)
                        {
                            if (packageXmlObjectMembers.ContainsKey("AuraDefinitionBundle"))
                            {
                                packageXmlObjectMembers["AuraDefinitionBundle"].Add(tnd2.Text);
                            }
                            else
                            {
                                packageXmlObjectMembers.Add("AuraDefinitionBundle", new HashSet<string> { tnd2.Text });
                            }
                        }
                    }
                }
                else if (tnd1.Text == "certs")
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (tnd2.Checked == true)
                        {
                            String[] objectNameSplit = tnd2.Text.Split('.');

                            if (packageXmlObjectMembers.ContainsKey("Certificate"))
                            {
                                packageXmlObjectMembers["Certificate"].Add(objectNameSplit[0]);
                            }
                            else
                            {
                                packageXmlObjectMembers.Add("Certificate", new HashSet<string> { objectNameSplit[0] });
                            }
                        }
                    }
                }
                else if (tnd1.Text == "classes")
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (tnd2.Checked == true)
                        {
                            String[] objectNameSplit = tnd2.Text.Split('.');

                            if (packageXmlObjectMembers.ContainsKey("ApexClass"))
                            {
                                packageXmlObjectMembers["ApexClass"].Add(objectNameSplit[0]);
                            }
                            else
                            {
                                packageXmlObjectMembers.Add("ApexClass", new HashSet<string> { objectNameSplit[0] });
                            }
                        }
                    }
                }
                else if (tnd1.Text == "components")
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (tnd2.Checked == true)
                        {
                            String[] objectNameSplit = tnd2.Text.Split('.');

                            if (packageXmlObjectMembers.ContainsKey("ApexComponent"))
                            {
                                packageXmlObjectMembers["ApexComponent"].Add(objectNameSplit[0]);
                            }
                            else
                            {
                                packageXmlObjectMembers.Add("ApexComponent", new HashSet<string> { objectNameSplit[0] });
                            }
                        }
                    }
                }
                else if (tnd1.Text == "contentassets")
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (tnd2.Checked == true)
                        {
                            String[] objectNameSplit = tnd2.Text.Split('.');

                            if (packageXmlObjectMembers.ContainsKey("ContentAsset"))
                            {
                                packageXmlObjectMembers["ContentAsset"].Add(objectNameSplit[0]);
                            }
                            else
                            {
                                packageXmlObjectMembers.Add("ContentAsset", new HashSet<string> { objectNameSplit[0] });
                            }
                        }
                    }
                }
                else if (tnd1.Text == "lwc")
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (tnd2.Checked == true)
                        {
                            if (tnd2.Checked == true)
                            {
                                if (packageXmlObjectMembers.ContainsKey("LightningComponentBundle"))
                                {
                                    packageXmlObjectMembers["LightningComponentBundle"].Add(tnd2.Text);
                                }
                                else
                                {
                                    packageXmlObjectMembers.Add("LightningComponentBundle", new HashSet<string> { tnd2.Text });
                                }
                            }
                        }
                    }
                }
                else if (tnd1.Text == "pages")
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (tnd2.Checked == true)
                        {
                            String[] objectNameSplit = tnd2.Text.Split('.');

                            if (packageXmlObjectMembers.ContainsKey("ApexPage"))
                            {
                                packageXmlObjectMembers["ApexPage"].Add(objectNameSplit[0]);
                            }
                            else
                            {
                                packageXmlObjectMembers.Add("ApexPage", new HashSet<string> { objectNameSplit[0] });
                            }
                        }
                    }
                }
                else if (tnd1.Text == "staticresources")
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (tnd2.Checked == true)
                        {
                            String[] objectNameSplit = tnd2.Text.Split('.');

                            if (packageXmlObjectMembers.ContainsKey("StaticResource"))
                            {
                                packageXmlObjectMembers["StaticResource"].Add(objectNameSplit[0]);
                            }
                            else
                            {
                                packageXmlObjectMembers.Add("StaticResource", new HashSet<string> { objectNameSplit[0] });
                            }
                        }
                    }
                }
                else if (tnd1.Text == "triggers")
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (tnd2.Checked == true)
                        {
                            String[] objectNameSplit = tnd2.Text.Split('.');

                            if (packageXmlObjectMembers.ContainsKey("ApexTrigger"))
                            {
                                packageXmlObjectMembers["ApexTrigger"].Add(objectNameSplit[0]);
                            }
                            else
                            {
                                packageXmlObjectMembers.Add("ApexTrigger", new HashSet<string> { objectNameSplit[0] });
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
                                String[] parsedFullPath = tnd3.FullPath.Split('\\');
                                directoryName = parsedFullPath[0];
                                objectFileName = parsedFullPath[1];

                                String[] objectNameParsed = parsedFullPath[1].Split('.');

                                // Handle Fields and Objects differently as you will need to list out the fields and objects in 
                                // the packageXml file since you are generating those individually along with the permission sets and profiles.

                                // Write the XML to the object file
                                if (parsedFullPath.Length == 3)
                                {
                                    // Package XML Dictionary build
                                    parentNode = MetadataDifferenceProcessing.folderToType(parsedFullPath[0], "");

                                    if (parsedFullPath[0] == "objects"
                                        && parsedFullPath[2].StartsWith("<fields"))
                                    {
                                        String xmlString = "<document>" + parsedFullPath[2] + "</document>";
                                        XmlDocument xd = new XmlDocument();
                                        xd.LoadXml(xmlString);

                                        String objectFieldCombo = objectNameParsed[0] + "." + xd.ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;

                                        // Add the custom field to the dictionary
                                        if (packageXmlObjectMembers.ContainsKey("CustomField"))
                                        {
                                            packageXmlObjectMembers["CustomField"].Add(objectFieldCombo);
                                        }
                                        else
                                        {
                                            packageXmlObjectMembers.Add("CustomField", new HashSet<string> { objectFieldCombo });
                                        }

                                        // Add the custom object 
                                        if (packageXmlObjectMembers.ContainsKey(parentNode))
                                        {
                                            packageXmlObjectMembers[parentNode].Add(objectNameParsed[0]);
                                        }
                                        else
                                        {
                                            packageXmlObjectMembers.Add(parentNode, new HashSet<string> { objectNameParsed[0] });
                                        }

                                    }
                                    else
                                    {
                                        if (packageXmlObjectMembers.ContainsKey(parentNode))
                                        {
                                            packageXmlObjectMembers[parentNode].Add(objectNameParsed[0]);
                                        }
                                        else
                                        {
                                            packageXmlObjectMembers.Add(parentNode, new HashSet<string> { objectNameParsed[0] });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                parentNode = "";
                directoryName = "";
                objectFileName = "";
            }
        }

        public void buildPackageXmlFile(Dictionary<String, HashSet<String>> packageXmlObjectMembers)
        {
            String xmlHeaderLine = "<?xml version =\"1.0\" encoding=\"UTF-8\"?>";

            StreamWriter swPackageXml = new StreamWriter(this.tbDeploymentPackageLocation.Text + "\\package.xml");

            swPackageXml.WriteLine(xmlHeaderLine);
            swPackageXml.WriteLine("<Package xmlns=\"http://soap.sforce.com/2006/04/metadata\">");

            foreach (String objectType in packageXmlObjectMembers.Keys)
            {
                swPackageXml.WriteLine("<types>");

                foreach (String memberType in packageXmlObjectMembers[objectType])
                {
                    swPackageXml.WriteLine("<members>" + memberType + "</members>");
                }

                swPackageXml.WriteLine("<name>" + objectType + "</name>");
                swPackageXml.WriteLine("</types>");
            }

            swPackageXml.WriteLine("<version>" + this.cmbDefaultAPI.Text + "</version>");
            swPackageXml.WriteLine("</Package>");

            swPackageXml.Close();
        }

        public void buildDestructivePackageXmlFile(Dictionary<String, HashSet<String>> packageXmlObjectMembers, String destructivePackageType)
        {
            String xmlHeaderLine = "<?xml version =\"1.0\" encoding=\"UTF-8\"?>";

            StreamWriter swDestructivePackageXml = new StreamWriter(this.tbDeploymentPackageLocation.Text + "\\" + destructivePackageType + ".xml");
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

        public class ObjectFieldWithXML
        {
            public String objectName;
            public String fieldApiName;
            public String fieldXml;
        }

        private void cmbDestructiveChange_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbDestructiveChange.Text == "--none--")
            {
                this.lblDestructiveChangesFirst.Visible = false;
                this.btnBuildProfilesAndPermissionSets.Visible = true;
                this.btnBuildPackageXml.Visible = true;
                this.btnNext.Visible = false;
                this.runTreeNodeSelector = true;
            }
            else if (this.cmbDestructiveChange.Text == "destructiveChanges")
            {
                this.lblDestructiveChangesFirst.Visible = false;
                this.btnBuildProfilesAndPermissionSets.Visible = false;
                this.btnBuildPackageXml.Visible = true;
                this.btnNext.Visible = false;
                this.runTreeNodeSelector = false;
            }
            else
            {
                this.lblDestructiveChangesFirst.Visible = true;
                this.btnBuildProfilesAndPermissionSets.Visible = false;
                this.btnBuildPackageXml.Visible = false;
                this.btnNext.Visible = true;
                this.runTreeNodeSelector = false;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.runTreeNodeSelector = true;

            // Build the destructive changes first
            // Then pre-populate the Tree View with the pre-checked related items which need to be reviewed from the
            // selected items in the destructive changes
            // Example: If you select a custom field to delete, then clicking Next will populate the Tree View with all
            // Items which include that field

            // Loop through the selected items and build the Dictionary / HashSet list

            buildPackageObjectMembers(packageXmlDestructiveChangeMembers);

            populateMetadataTreeView();
        }

        public void loadDefaultApis()
        {
            foreach (String api in UtilityClass.generateAPIArray())
            {
                this.cmbDefaultAPI.Items.Add(api);
            }

            this.cmbDefaultAPI.Text = Properties.Settings.Default.DefaultAPI;
        }

    }
}
