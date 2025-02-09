using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Security.AccessControl;

namespace SalesforceMetadata
{
    public partial class ParseSalesforceMetadata : Form
    {
        public ParseSalesforceMetadata()
        {
            InitializeComponent();
            loadRecentPaths();
        }

        private void loadRecentPaths()
        {
            this.tbMetadataFolderPath.Text = Properties.Settings.Default.ParseMetadataFolderaPath;
            this.tbReportFolderPath.Text = Properties.Settings.Default.RecentReportFolderPath;
        }

        private void btnConvertToMDAPI_Click(object sender, EventArgs e)
        {
            if (this.tbMetadataFolderPath.Text == "")
            {
                MessageBox.Show("Please select a path to the metadata folder");
                return;
            }

            //String folderPath = this.tbMetadataFolderPath.Text;

            SalesforceMetadataStep2 sfMetadataStep2 = new SalesforceMetadataStep2();
            sfMetadataStep2.addVSCodeFileExtension(this.tbMetadataFolderPath.Text);
        }

        private void tbMetadataFolderPath_DoubleClick(object sender, EventArgs e)
        {
            this.tbMetadataFolderPath.Text = UtilityClass.folderBrowserSelectPath("Select the Metadata Directory to Parse",
                                                                                  false,
                                                                                  FolderEnum.SaveTo,
                                                                                  Properties.Settings.Default.ParseMetadataFolderaPath);

            Properties.Settings.Default.ParseMetadataFolderaPath = this.tbMetadataFolderPath.Text;
            Properties.Settings.Default.Save();
        }

        private void tbReportFolderPath_DoubleClick(object sender, EventArgs e)
        {
            this.tbReportFolderPath.Text = UtilityClass.folderBrowserSelectPath("Select the Report Folder", 
                                                                                true, 
                                                                                FolderEnum.SaveTo, 
                                                                                Properties.Settings.Default.RecentReportFolderPath);

            Properties.Settings.Default.RecentReportFolderPath = this.tbReportFolderPath.Text;
            Properties.Settings.Default.Save();
        }

        private void btnParseProfilesPermissionSetsByObject_Click(object sender, EventArgs e)
        {
            if (this.tbMetadataFolderPath.Text == "")
            {
                MessageBox.Show("Please select a folder with the Metadata files");
                return;
            }

            if (this.tbReportFolderPath.Text == "")
            {
                MessageBox.Show("Please select a folder to save the results to");
                return;
            }

            String[] files = Directory.GetFiles(this.tbMetadataFolderPath.Text);

            // Key = object name
            Dictionary<String, ProfilePermSetByObject> profPermSetByObjectDict = new Dictionary<string, ProfilePermSetByObject>();

            // Key = object.fieldName
            Dictionary<String, ProfilePermSetByField> profPermSetByFieldDict = new Dictionary<string, ProfilePermSetByField>();

            foreach (String file in files) 
            {
                String[] fileSplit = file.Split('\\');

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(file);

                Debug.WriteLine("");

                XmlNodeList label = xmlDoc.GetElementsByTagName("label");
                XmlNodeList license = xmlDoc.GetElementsByTagName("license");
                XmlNodeList activationRequired = xmlDoc.GetElementsByTagName("hasActivationRequired");

                XmlNodeList objectPermissions = xmlDoc.GetElementsByTagName("objectPermissions");
                XmlNodeList fieldPermission = xmlDoc.GetElementsByTagName("fieldPermissions");
                
                foreach(XmlNode objNode in objectPermissions)
                {
                    String objName = objNode.ChildNodes[5].InnerText;
                    if (profPermSetByObjectDict.ContainsKey(objName))
                    {
                        ProfilePermSetObjectPerms pps = new ProfilePermSetObjectPerms();
                        pps.profilePermSetApiName = fileSplit[fileSplit.Length - 1];
                        pps.profilePermSetLabel = label[0].InnerText;
                        if (license.Count > 0)
                        {
                            pps.licence = license[0].InnerText;
                        }
                        if (activationRequired.Count > 0)
                        {
                            pps.activationRequired = activationRequired[0].InnerText;
                        }
                        pps.read = objNode.ChildNodes[3].InnerText;
                        pps.create = objNode.ChildNodes[0].InnerText;
                        pps.edit = objNode.ChildNodes[2].InnerText;
                        pps.delete = objNode.ChildNodes[1].InnerText;
                        pps.viewAll = objNode.ChildNodes[4].InnerText;
                        pps.modifyAll = objNode.ChildNodes[6].InnerText;

                        profPermSetByObjectDict[objName].profPermSetObjectPermsList.Add(pps);
                    }
                    else
                    {
                        ProfilePermSetByObject ppso = new ProfilePermSetByObject();
                        ppso.objectName = objName;

                        ProfilePermSetObjectPerms pps = new ProfilePermSetObjectPerms();
                        pps.profilePermSetApiName = fileSplit[fileSplit.Length - 1];
                        pps.profilePermSetLabel = label[0].InnerText;
                        if (license.Count > 0)
                        {
                            pps.licence = license[0].InnerText;
                        }
                        if (activationRequired.Count > 0)
                        {
                            pps.activationRequired = activationRequired[0].InnerText;
                        }
                        pps.read = objNode.ChildNodes[3].InnerText;
                        pps.create = objNode.ChildNodes[0].InnerText;
                        pps.edit = objNode.ChildNodes[2].InnerText;
                        pps.delete = objNode.ChildNodes[1].InnerText;
                        pps.viewAll = objNode.ChildNodes[4].InnerText;
                        pps.modifyAll = objNode.ChildNodes[6].InnerText;

                        ppso.profPermSetObjectPermsList.Add(pps);
                        profPermSetByObjectDict.Add(objName, ppso);
                    }
                }

                foreach (XmlNode fieldNode in fieldPermission)
                {
                    String fieldName = fieldNode.ChildNodes[1].InnerText;

                    if (profPermSetByFieldDict.ContainsKey(fieldName))
                    {
                        ProfilePermSetFieldPerms ppf = new ProfilePermSetFieldPerms();
                        ppf.profilePermSetApiName = fileSplit[fileSplit.Length - 1];
                        ppf.profilePermSetLabel = label[0].InnerText;
                        if (license.Count > 0)
                        {
                            ppf.licence = license[0].InnerText;
                        }
                        if (activationRequired.Count > 0)
                        {
                            ppf.activationRequired = activationRequired[0].InnerText;
                        }
                        ppf.read = fieldNode.ChildNodes[2].InnerText;
                        ppf.edit = fieldNode.ChildNodes[0].InnerText;

                        profPermSetByFieldDict[fieldName].profilePermSetFieldPermsList.Add(ppf);
                    }
                    else
                    {
                        ProfilePermSetByField ppfo = new ProfilePermSetByField();
                        ppfo.fieldName = fieldName;

                        ProfilePermSetFieldPerms ppf = new ProfilePermSetFieldPerms();
                        ppf.profilePermSetApiName = fileSplit[fileSplit.Length - 1];
                        ppf.profilePermSetLabel = label[0].InnerText;
                        if (license.Count > 0)
                        {
                            ppf.licence = license[0].InnerText;
                        }
                        if (activationRequired.Count > 0)
                        {
                            ppf.activationRequired = activationRequired[0].InnerText;
                        }
                        ppf.read = fieldNode.ChildNodes[2].InnerText;
                        ppf.edit = fieldNode.ChildNodes[0].InnerText;

                        ppfo.profilePermSetFieldPermsList.Add(ppf);
                        profPermSetByFieldDict.Add(fieldName, ppfo);
                    }
                }
            }

            StreamWriter swObjPerms = new StreamWriter(this.tbReportFolderPath.Text + "\\ObjPermsByProfilePermissionSet.txt");
            swObjPerms.WriteLine("ObjectName\tProfilePermSetApiName\tProfilePermSetLabel\tLicense\tActivationRequired\tRead\tCreate\tEdit\tDelete\tViewAll\tModifyAll");

            foreach (String objName in profPermSetByObjectDict.Keys)
            {
                swObjPerms.WriteLine(objName);

                foreach (ProfilePermSetObjectPerms objPerms in profPermSetByObjectDict[objName].profPermSetObjectPermsList)
                {
                    swObjPerms.Write('\t');
                    swObjPerms.Write(objPerms.profilePermSetApiName + '\t');
                    swObjPerms.Write(objPerms.profilePermSetLabel + '\t');
                    swObjPerms.Write(objPerms.licence + '\t');
                    swObjPerms.Write(objPerms.activationRequired + '\t');
                    swObjPerms.Write(objPerms.read + '\t');
                    swObjPerms.Write(objPerms.create + '\t');
                    swObjPerms.Write(objPerms.edit + '\t');
                    swObjPerms.Write(objPerms.delete + '\t');
                    swObjPerms.Write(objPerms.viewAll + '\t');
                    swObjPerms.Write(objPerms.modifyAll);
                    swObjPerms.Write(Environment.NewLine);
                }
            }

            swObjPerms.Close();
            profPermSetByObjectDict.Clear();

            // Now write out the Permissions by Field
            StreamWriter swFieldPerms = new StreamWriter(this.tbReportFolderPath.Text + "\\FieldPermsByProfilePermissionSet.txt");
            swFieldPerms.WriteLine("ObjectFieldName\tProfilePermSetApiName\tProfilePermSetLabel\tLicense\tActivationRequired\tRead\tEdit");

            foreach (String fieldName in profPermSetByFieldDict.Keys)
            {
                swFieldPerms.WriteLine(fieldName);

                foreach (ProfilePermSetFieldPerms fldPerms in profPermSetByFieldDict[fieldName].profilePermSetFieldPermsList)
                {
                    swFieldPerms.Write('\t');
                    swFieldPerms.Write(fldPerms.profilePermSetApiName + '\t');
                    swFieldPerms.Write(fldPerms.profilePermSetLabel + '\t');
                    swFieldPerms.Write(fldPerms.licence + '\t');
                    swFieldPerms.Write(fldPerms.activationRequired + '\t');
                    swFieldPerms.Write(fldPerms.read + '\t');
                    swFieldPerms.Write(fldPerms.edit + '\t');
                    swFieldPerms.Write(Environment.NewLine);
                }
            }

            swFieldPerms.Close();
            profPermSetByFieldDict.Clear();
        }

        public class ProfilePermSetByObject
        {
            public String objectName;
            public List<ProfilePermSetObjectPerms> profPermSetObjectPermsList;

            public ProfilePermSetByObject()
            {
                objectName = "";
                profPermSetObjectPermsList = new List<ProfilePermSetObjectPerms>();
            }
        }

        public class ProfilePermSetByField
        {
            public String fieldName;
            public List<ProfilePermSetFieldPerms> profilePermSetFieldPermsList;

            public ProfilePermSetByField()
            {
                fieldName = "";
                profilePermSetFieldPermsList = new List<ProfilePermSetFieldPerms>();
            }
        }
        public class ProfilePermSetObjectPerms
        {
            public String profilePermSetApiName = "";
            public String profilePermSetLabel = "";
            public String licence = "";
            public String activationRequired = "";
            public String read = "false";
            public String create = "false";
            public String edit = "false";
            public String delete = "false";
            public String viewAll = "false";
            public String modifyAll = "false";
        }
        public class ProfilePermSetFieldPerms
        {
            public String profilePermSetApiName = "";
            public String profilePermSetLabel = "";
            public String licence = "";
            public String activationRequired = "";
            public String read = "false";
            public String edit = "false";
        }
    }
}
