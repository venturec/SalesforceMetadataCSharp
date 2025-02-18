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
using static SalesforceMetadata.AutomationReporter;

namespace SalesforceMetadata
{
    public partial class ParseSalesforceMetadata : Form
    {
        // Key = Object API Name
        public Dictionary<String, ObjectToFields> objectToFieldsDictionary;

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

        private void btnParseObjectsAndFields_Click(object sender, EventArgs e)
        {
            if (this.tbMetadataFolderPath.Text == "")
            {
                MessageBox.Show("Please select a project folder with the Metadata files");
                return;
            }

            if (this.tbReportFolderPath.Text == "")
            {
                MessageBox.Show("Please select a folder location to save the report values to");
                return;
            }

            runObjectFieldExtract(this.tbMetadataFolderPath.Text, this.tbSearchFilter.Text);
            writeObjectValuesToDictionary(this.tbReportFolderPath.Text, this.tbSearchFilter.Text);

            MessageBox.Show("Sobject and Fields Extract Complete");
        }


        public void parseObjectsAndFields(String objectFolderPath, String reportRolderPath, String searchFilter)
        {
            runObjectFieldExtract(objectFolderPath, searchFilter);
            writeObjectValuesToDictionary(reportRolderPath, searchFilter);
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

            Dictionary<String, List<String>> profPermSetFolderPaths = new Dictionary<String, List<String>>();
            String[] directories = Directory.GetDirectories(this.tbMetadataFolderPath.Text);
            if (directories != null && directories.Length > 0)
            {
                foreach (String dir in directories)
                {
                    String[] directorySplit = dir.Split('\\');
                    String subFolderName = directorySplit[directorySplit.Length - 1];
                    if (subFolderName == "profiles" || subFolderName == "permissionsets")
                    {
                        String[] dirFiles = Directory.GetFiles(dir);
                        List<string> dirFilesList = new List<string>();
                        dirFilesList.AddRange(dirFiles);

                        if (subFolderName == "profiles")
                        {
                            profPermSetFolderPaths.Add("Profile", dirFilesList);
                        }
                        else if (subFolderName == "permissionsets")
                        {
                            profPermSetFolderPaths.Add("PermissionSet", dirFilesList);
                        }
                    }
                }
            }
            else
            {
                String[] directorySplit = this.tbMetadataFolderPath.Text.Split('\\');
                String subFolderName = directorySplit[directorySplit.Length - 1];

                String[] dirFiles = Directory.GetFiles(this.tbMetadataFolderPath.Text);
                List<string> dirFilesList = new List<string>();
                dirFilesList.AddRange(dirFiles);

                if (subFolderName == "profiles")
                {
                    profPermSetFolderPaths.Add("Profile", dirFilesList);
                }
                else if (subFolderName == "permissionsets")
                {
                    profPermSetFolderPaths.Add("PermissionSet", dirFilesList);
                }
            }

            if (profPermSetFolderPaths.Count == 0)
            {
                MessageBox.Show("There were no Profiles or Permission Set folders found in path selected");
                return;
            }

            // Key = object name
            Dictionary<String, ProfilePermSetByObject> profPermSetByObjectDict = new Dictionary<string, ProfilePermSetByObject>();

            // Key = object.fieldName
            Dictionary<String, ProfilePermSetByField> profPermSetByFieldDict = new Dictionary<string, ProfilePermSetByField>();

            foreach (String ppsType in profPermSetFolderPaths.Keys)
            {
                foreach (String file in profPermSetFolderPaths[ppsType])
                {
                    String[] fileSplit = file.Split('\\');

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(file);

                    XmlNodeList label = xmlDoc.GetElementsByTagName("label");
                    XmlNodeList license = xmlDoc.GetElementsByTagName("license");
                    XmlNodeList activationRequired = xmlDoc.GetElementsByTagName("hasActivationRequired");

                    XmlNodeList objectPermissions = xmlDoc.GetElementsByTagName("objectPermissions");
                    XmlNodeList fieldPermission = xmlDoc.GetElementsByTagName("fieldPermissions");

                    foreach (XmlNode objNode in objectPermissions)
                    {
                        String objName = objNode.ChildNodes[5].InnerText;
                        if (profPermSetByObjectDict.ContainsKey(objName))
                        {
                            ProfilePermSetObjectPerms pps = new ProfilePermSetObjectPerms();
                            pps.type = ppsType;
                            pps.profilePermSetApiName = fileSplit[fileSplit.Length - 1];
                            if (label.Count > 0)
                            {
                                pps.profilePermSetLabel = label[0].InnerText;
                            }
                            if (license.Count > 0)
                            {
                                pps.licence = license[0].InnerText;
                            }
                            if (activationRequired.Count > 0)
                            {
                                pps.activationRequired = activationRequired[0].InnerText;
                            }

                            pps.create = objNode.ChildNodes[0].InnerText;
                            pps.delete = objNode.ChildNodes[1].InnerText;
                            pps.edit = objNode.ChildNodes[2].InnerText;
                            pps.read = objNode.ChildNodes[3].InnerText;
                            pps.modifyAll = objNode.ChildNodes[4].InnerText;
                            pps.viewAll = objNode.ChildNodes[6].InnerText;

                            profPermSetByObjectDict[objName].profPermSetObjectPermsList.Add(pps);
                        }
                        else
                        {
                            ProfilePermSetByObject ppso = new ProfilePermSetByObject();
                            ppso.objectName = objName;

                            ProfilePermSetObjectPerms pps = new ProfilePermSetObjectPerms();
                            pps.type = ppsType;
                            pps.profilePermSetApiName = fileSplit[fileSplit.Length - 1];
                            if (label.Count > 0)
                            {
                                pps.profilePermSetLabel = label[0].InnerText;
                            }
                            if (license.Count > 0)
                            {
                                pps.licence = license[0].InnerText;
                            }
                            if (activationRequired.Count > 0)
                            {
                                pps.activationRequired = activationRequired[0].InnerText;
                            }

                            pps.create = objNode.ChildNodes[0].InnerText;
                            pps.delete = objNode.ChildNodes[1].InnerText;
                            pps.edit = objNode.ChildNodes[2].InnerText;
                            pps.read = objNode.ChildNodes[3].InnerText;
                            pps.modifyAll = objNode.ChildNodes[4].InnerText;
                            pps.viewAll = objNode.ChildNodes[6].InnerText;

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
                            ppf.type = ppsType;
                            ppf.profilePermSetApiName = fileSplit[fileSplit.Length - 1];
                            if (label.Count > 0)
                            {
                                ppf.profilePermSetLabel = label[0].InnerText;
                            }
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
                            ppf.type = ppsType;
                            ppf.profilePermSetApiName = fileSplit[fileSplit.Length - 1];
                            if (label.Count > 0)
                            {
                                ppf.profilePermSetLabel = label[0].InnerText;
                            }
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
            }

            StreamWriter swObjPerms = new StreamWriter(this.tbReportFolderPath.Text + "\\ObjPermsByProfilePermissionSet.txt");
            swObjPerms.WriteLine("ObjectName\tType\tProfilePermSetApiName\tProfilePermSetLabel\tLicense\tActivationRequired\tRead\tCreate\tEdit\tDelete\tViewAll\tModifyAll");

            foreach (String objName in profPermSetByObjectDict.Keys)
            {
                foreach (ProfilePermSetObjectPerms objPerms in profPermSetByObjectDict[objName].profPermSetObjectPermsList)
                {
                    swObjPerms.Write(objName + '\t');
                    swObjPerms.Write(objPerms.type + '\t');
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
            swFieldPerms.WriteLine("ObjectFieldName\tType\tProfilePermSetApiName\tProfilePermSetLabel\tLicense\tActivationRequired\tRead\tEdit");

            foreach (String fieldName in profPermSetByFieldDict.Keys)
            {
                foreach (ProfilePermSetFieldPerms fldPerms in profPermSetByFieldDict[fieldName].profilePermSetFieldPermsList)
                {
                    swFieldPerms.Write(fieldName + '\t');
                    swFieldPerms.Write(fldPerms.type + '\t');
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

            MessageBox.Show("Profiles/Permission Sets By Object/Field Report Extract Complete");
        }

        // We need the objects and fields and the apex classes extracted out first
        private void runObjectFieldExtract(String metadataFolderPath, String searchFilter)
        {
            // Determine if the directory name ends with objects and if not, then add the objects
            String[] directorySplit = metadataFolderPath.Split('\\');
            if (directorySplit[directorySplit.Length - 1] != "objects")
            {
                metadataFolderPath = metadataFolderPath + "\\objects";
            }

            if (Directory.Exists(metadataFolderPath))
            {
                String[] files = Directory.GetFiles(metadataFolderPath);

                objectToFieldsDictionary = new Dictionary<string, ObjectToFields>();

                foreach (String fl in files)
                {
                    String[] flPathSplit = fl.Split('\\');
                    String[] flNameSplit = flPathSplit[flPathSplit.Length - 1].Split('.');

                    XmlDocument objXd = new XmlDocument();
                    objXd.Load(fl);

                    XmlNodeList objLabel = objXd.GetElementsByTagName("label");
                    XmlNodeList pluralLabel = objXd.GetElementsByTagName("pluralLabel");
                    XmlNodeList objectFieldList = objXd.GetElementsByTagName("fields");
                    XmlNodeList validationRules = objXd.GetElementsByTagName("validationRules");
                    XmlNodeList sharingmodel = objXd.GetElementsByTagName("sharingModel");
                    XmlNodeList objvisibility = objXd.GetElementsByTagName("visibility");
                    XmlNodeList customSetting = objXd.GetElementsByTagName("customSettingsType");
                    XmlNodeList fieldSets = objXd.GetElementsByTagName("fieldSets");
                    XmlNodeList compactLayouts = objXd.GetElementsByTagName("compactLayouts");

                    // Get all fields into the Dictionary
                    ObjectToFields otf = new ObjectToFields();
                    otf.objectName = flNameSplit[0];

                    otf.fields = new List<String>();

                    foreach (XmlNode xn in objLabel)
                    {
                        if (xn.ParentNode.Name == "CustomObject")
                        {
                            otf.objectLabel = xn.InnerText;
                        }
                    }

                    foreach (XmlNode xn in pluralLabel)
                    {
                        if (xn.ParentNode.Name == "CustomObject")
                        {
                            otf.pluralLabel = xn.InnerText;
                        }
                    }

                    foreach (XmlNode xn in customSetting)
                    {
                        if (xn.ParentNode.Name == "CustomObject")
                        {
                            otf.isCustomSetting = true;
                            otf.customSettingType = xn.InnerText;
                        }
                    }

                    foreach (XmlNode xn in compactLayouts)
                    {
                        if (xn.ParentNode.Name == "CustomObject")
                        {
                            String clName = "";
                            List<String> clFields = new List<String>();

                            foreach (XmlNode cn in xn.ChildNodes)
                            {
                                if (cn.Name == "fullName")
                                {
                                    clName = cn.InnerText;
                                }
                                else if (cn.Name == "fields")
                                {
                                    clFields.Add(cn.ChildNodes[0].InnerText);
                                }
                            }

                            otf.compactLayoutToFieldNames.Add(clName, new List<String>());
                            otf.compactLayoutToFieldNames[clName].AddRange(clFields);
                        }
                    }

                    foreach (XmlNode xn in fieldSets)
                    {
                        if (xn.ParentNode.Name == "CustomObject")
                        {
                            String fsName = "";
                            List<String> fsFields = new List<String>();

                            foreach (XmlNode cn in xn.ChildNodes)
                            {
                                if (cn.Name == "fullName")
                                {
                                    fsName = cn.InnerText;
                                }
                                else if (cn.Name == "displayedFields")
                                {
                                    fsFields.Add(cn.ChildNodes[0].InnerText);
                                }
                            }

                            otf.fieldSetToFieldNames.Add(fsName, new List<String>());
                            otf.fieldSetToFieldNames[fsName].AddRange(fsFields);
                        }
                    }

                    foreach (XmlNode fldNd in objectFieldList)
                    {
                        if (fldNd.ParentNode.Name == "CustomObject")
                        {
                            otf.fields.Add(fldNd.ChildNodes[0].InnerText);
                            ObjectFields of = new ObjectFields();
                            foreach (XmlNode fldDetailNd in fldNd.ChildNodes)
                            {
                                of.sObjectName = otf.objectName;

                                if (fldDetailNd.Name == "fullName")
                                {
                                    of.fullName = fldDetailNd.InnerText;
                                }
                                else if (fldDetailNd.Name == "externalId")
                                {
                                    of.externalId = fldDetailNd.InnerText;
                                }
                                else if (fldDetailNd.Name == "label")
                                {
                                    of.label = fldDetailNd.InnerText;
                                }
                                else if (fldDetailNd.Name == "length")
                                {
                                    of.length = fldDetailNd.InnerText;
                                }
                                else if (fldDetailNd.Name == "required")
                                {
                                    of.required = fldDetailNd.InnerText;
                                }
                                else if (fldDetailNd.Name == "defaultValue")
                                {
                                    String defval = fldDetailNd.InnerText;
                                    defval = defval.Replace('\t', ' ');
                                    defval = defval.Replace('\r', ' ');
                                    defval = defval.Replace('\n', ' ');
                                    defval = defval.Replace(' ', ' ');    // nbsp
                                    defval = defval.Replace("  ", " ");
                                    defval = defval.Replace("  ", " ");
                                    defval = defval.Replace("  ", " ");
                                    defval = defval.Replace("  ", " ");
                                    defval = defval.Replace("  ", " ");
                                    defval = defval.Replace("  ", " ");
                                    defval = defval.Replace("  ", " ");
                                    defval = defval.Replace("  ", " ");
                                    defval = defval.Replace("  ", " ");
                                    defval = defval.Replace("  ", " ");
                                    defval = defval.Replace("  ", " ");
                                    defval = defval.Replace("  ", " ");
                                    defval = defval.Replace("  ", " ");
                                    defval = defval.Replace("  ", " ");
                                    defval = defval.Replace("  ", " ");

                                    of.defaultValue = defval;
                                }
                                else if (fldDetailNd.Name == "trackHistory")
                                {
                                    of.trackHistory = Boolean.Parse(fldDetailNd.InnerText);
                                    if (of.trackHistory == true)
                                    {
                                        otf.fieldsTrackedCount++;
                                    }
                                }
                                else if (fldDetailNd.Name == "type")
                                {
                                    of.type = fldDetailNd.InnerText;
                                }
                                else if (fldDetailNd.Name == "formula")
                                {
                                    String frm = fldDetailNd.InnerText;
                                    frm = frm.Replace('\t', ' ');
                                    frm = frm.Replace('\r', ' ');
                                    frm = frm.Replace('\n', ' ');
                                    frm = frm.Replace(' ', ' ');    // nbsp
                                    frm = frm.Replace("  ", " ");
                                    frm = frm.Replace("  ", " ");
                                    frm = frm.Replace("  ", " ");
                                    frm = frm.Replace("  ", " ");
                                    frm = frm.Replace("  ", " ");
                                    frm = frm.Replace("  ", " ");
                                    frm = frm.Replace("  ", " ");
                                    frm = frm.Replace("  ", " ");
                                    frm = frm.Replace("  ", " ");
                                    frm = frm.Replace("  ", " ");
                                    frm = frm.Replace("  ", " ");
                                    frm = frm.Replace("  ", " ");
                                    frm = frm.Replace("  ", " ");
                                    frm = frm.Replace("  ", " ");
                                    frm = frm.Replace("  ", " ");

                                    of.isFormula = true;
                                    of.formula = frm;
                                    otf.formulaFieldCount++;
                                }
                                else if (fldDetailNd.Name == "precision")
                                {
                                    of.precision = fldDetailNd.InnerText;
                                }
                                else if (fldDetailNd.Name == "scale")
                                {
                                    of.scale = fldDetailNd.InnerText;
                                }
                                else if (fldDetailNd.Name == "unique")
                                {
                                    of.unique = fldDetailNd.InnerText;
                                }
                                else if (fldDetailNd.Name == "referenceTo")
                                {
                                    of.referenceTo = fldDetailNd.InnerText;
                                }
                                else if (fldDetailNd.Name == "deleteConstraint")
                                {
                                    of.deleteConstraint = fldDetailNd.InnerText;
                                }
                                else if (fldDetailNd.Name == "relationshipLabel")
                                {
                                    of.relationshipLabel = fldDetailNd.InnerText;
                                }
                                else if (fldDetailNd.Name == "relationshipName")
                                {
                                    of.relationshipName = fldDetailNd.InnerText;
                                }
                                else if (fldDetailNd.Name == "valueSet")
                                {
                                    String valueSet = "";

                                    foreach (XmlNode val1 in fldDetailNd.ChildNodes)
                                    {
                                        if (val1.Name == "valueSetDefinition")
                                        {
                                            foreach (XmlNode val2 in val1.ChildNodes)
                                            {
                                                if (val2.Name == "value")
                                                {
                                                    foreach (XmlNode val3 in val2.ChildNodes)
                                                    {
                                                        if (val3.Name == "fullName")
                                                        {
                                                            valueSet = valueSet + val3.InnerText + ";";
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (valueSet != "")
                                    {
                                        of.picklistValueSet = valueSet;
                                    }
                                }
                            }

                            if (searchFilter == "")
                            {
                                otf.objFields.Add(of);
                            }
                            else if (of.fullName.Contains(searchFilter))
                            {
                                otf.objFields.Add(of);
                            }
                            else if (of.formula.Contains(searchFilter))
                            {
                                otf.objFields.Add(of);
                            }
                            else
                            {
                                otf.objFields.Add(of);
                            }

                            otf.fieldCount++;
                        }
                    }

                    if (sharingmodel.Count > 0)
                    {
                        otf.sharingModel = sharingmodel[0].ChildNodes[0].InnerText;
                    }

                    if (objvisibility.Count > 0)
                    {
                        otf.visibility = objvisibility[0].ChildNodes[0].InnerText;
                    }

                    // Get all Validation Rules into the Dictionary
                    foreach (XmlNode objVal in validationRules)
                    {
                        if (objVal.ChildNodes[1].InnerText == "true")
                        {
                            ObjectValidations ov = new ObjectValidations();
                            ov.sObjectName = otf.objectName;

                            foreach (XmlNode cn in objVal.ChildNodes)
                            {
                                if (cn.Name == "fullName")
                                {
                                    ov.validationName = cn.InnerText;
                                }
                                else if (cn.Name == "errorConditionFormula")
                                {
                                    String errCond = cn.InnerText;
                                    errCond = errCond.Replace('\t', ' ');
                                    errCond = errCond.Replace('\r', ' ');
                                    errCond = errCond.Replace('\n', ' ');
                                    errCond = errCond.Replace("  ", " ");
                                    errCond = errCond.Replace("  ", " ");
                                    errCond = errCond.Replace("  ", " ");
                                    errCond = errCond.Replace("  ", " ");
                                    errCond = errCond.Replace("  ", " ");
                                    errCond = errCond.Replace("  ", " ");
                                    errCond = errCond.Replace("  ", " ");
                                    errCond = errCond.Replace("  ", " ");
                                    errCond = errCond.Replace("  ", " ");
                                    errCond = errCond.Replace("  ", " ");
                                    errCond = errCond.Replace("  ", " ");
                                    errCond = errCond.Replace("  ", " ");
                                    errCond = errCond.Replace("  ", " ");
                                    errCond = errCond.Replace("  ", " ");
                                    errCond = errCond.Replace("  ", " ");


                                    ov.errorConditionFormula = errCond;
                                }
                                else if (cn.Name == "active")
                                {
                                    ov.isActive = Boolean.Parse(cn.InnerText);
                                }
                            }

                            if (searchFilter == "")
                            {
                                otf.objValidations.Add(ov);
                            }
                            else if (ov.validationName.Contains(searchFilter))
                            {
                                otf.objValidations.Add(ov);
                            }
                            else if (ov.errorConditionFormula.Contains(searchFilter))
                            {
                                otf.objValidations.Add(ov);
                            }
                            else
                            {
                                otf.objValidations.Add(ov);
                            }
                        }
                    }

                    objectToFieldsDictionary.Add(flNameSplit[0], otf);
                }
            }
        }

        public void writeObjectValuesToDictionary(String saveResultsToFolder, String searchFilter)
        {
            // Write the values

            StreamWriter objWriter = new StreamWriter(saveResultsToFolder + "\\sObjects.txt");
            StreamWriter fieldWriter = new StreamWriter(saveResultsToFolder + "\\sObjectFields.txt");
            StreamWriter validationWriter = new StreamWriter(saveResultsToFolder + "\\sObjectValidations.txt");
            StreamWriter fieldSetWriter = new StreamWriter(saveResultsToFolder + "\\sObjectFieldSets.txt");

            objWriter.WriteLine("sObjectName\t" +
                "Label\t" +
                "PluralLabel\t" +
                "SharingModel\t" +
                "Visibility\t" +
                "FieldCount\t" +
                "FieldsTrackedCount\t" +
                "FormulaFieldCount\t" +
                "IsCustomSetting\t" +
                "CustomSettingType\t" +
                "FieldSets");

            fieldWriter.WriteLine("sObjectName\t" +
                "FullName\t" +
                "Label\t" +
                "Required\t" +
                "DefaultValue\t" +
                "TrackHistory\t" +
                "Type\t" +
                "PicklistValueSet\t" +
                "Length\t" +
                "Precision\t" +
                "Scale\t" +
                "Unique\t" +
                "ExternalId\t" +
                "ReferenceTo\t" +
                "DeleteConstraint\t" +
                "RelationshipLabel\t" +
                "RelationshipName\t" +
                "IsFormula\t" +
                "Formula");

            validationWriter.WriteLine("sObjectName\t" +
                "validationName\t" +
                "isActive\t" +
                "errorConditionFormula");

            fieldSetWriter.WriteLine("sObjectName\t" +
                "FieldSetName\t" +
                "Fields");

            foreach (ObjectToFields otf in this.objectToFieldsDictionary.Values)
            {
                if (searchFilter != ""
                    && (otf.objFields.Count > 0
                        || otf.objValidations.Count > 0))
                {
                    objWriter.Write(otf.objectName + "\t");
                    objWriter.Write(otf.objectLabel + "\t");
                    objWriter.Write(otf.pluralLabel + "\t");
                    objWriter.Write(otf.sharingModel + "\t");
                    objWriter.Write(otf.visibility + "\t");
                    objWriter.Write(otf.fieldCount + "\t");
                    objWriter.Write(otf.fieldsTrackedCount + "\t");
                    objWriter.Write(otf.formulaFieldCount + "\t");
                    objWriter.Write(otf.isCustomSetting + "\t");
                    objWriter.Write(otf.customSettingType + "\t");
                    objWriter.Write(Environment.NewLine);
                }
                else if (searchFilter == "")
                {
                    objWriter.Write(otf.objectName + "\t");
                    objWriter.Write(otf.objectLabel + "\t");
                    objWriter.Write(otf.pluralLabel + "\t");
                    objWriter.Write(otf.sharingModel + "\t");
                    objWriter.Write(otf.visibility + "\t");
                    objWriter.Write(otf.fieldCount + "\t");
                    objWriter.Write(otf.fieldsTrackedCount + "\t");
                    objWriter.Write(otf.formulaFieldCount + "\t");
                    objWriter.Write(otf.isCustomSetting + "\t");
                    objWriter.Write(otf.customSettingType + "\t");
                    objWriter.Write(Environment.NewLine);
                }

                foreach (ObjectFields of in otf.objFields)
                {
                    fieldWriter.Write(of.sObjectName + "\t");
                    fieldWriter.Write(of.fullName + "\t");
                    fieldWriter.Write(of.label + "\t");
                    fieldWriter.Write(of.required + "\t");
                    fieldWriter.Write(of.defaultValue + "\t");
                    fieldWriter.Write(of.trackHistory + "\t");
                    fieldWriter.Write(of.type + "\t");
                    fieldWriter.Write(of.picklistValueSet + "\t");
                    fieldWriter.Write(of.length + "\t");
                    fieldWriter.Write(of.precision + "\t");
                    fieldWriter.Write(of.scale + "\t");
                    fieldWriter.Write(of.unique + "\t");
                    fieldWriter.Write(of.externalId + "\t");
                    fieldWriter.Write(of.referenceTo + "\t");
                    fieldWriter.Write(of.deleteConstraint + "\t");
                    fieldWriter.Write(of.relationshipLabel + "\t");
                    fieldWriter.Write(of.relationshipName + "\t");
                    fieldWriter.Write(of.isFormula + "\t");
                    fieldWriter.Write(of.formula);
                    fieldWriter.Write(Environment.NewLine);
                }

                foreach (ObjectValidations ov in otf.objValidations)
                {
                    validationWriter.Write(ov.sObjectName + "\t");
                    validationWriter.Write(ov.validationName + "\t");
                    validationWriter.Write(ov.isActive + "\t");
                    validationWriter.Write(ov.errorConditionFormula);
                    validationWriter.Write(Environment.NewLine);
                }

                foreach (String fs in otf.fieldSetToFieldNames.Keys)
                {
                    fieldSetWriter.Write(otf.objectName + "\t");
                    fieldSetWriter.Write(fs + "\t");
                    foreach (String fn in otf.fieldSetToFieldNames[fs])
                    {
                        fieldSetWriter.Write(fn + ",");
                    }

                    fieldSetWriter.Write(Environment.NewLine);
                }
            }

            objWriter.Close();
            fieldWriter.Close();
            validationWriter.Close();
            fieldSetWriter.Close();
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
            public String type = "";
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
            public String type = "";
            public String profilePermSetApiName = "";
            public String profilePermSetLabel = "";
            public String licence = "";
            public String activationRequired = "";
            public String read = "false";
            public String edit = "false";
        }
    }
}
