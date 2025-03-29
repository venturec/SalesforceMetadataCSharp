using SalesforceMetadata.MetadataWSDL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SalesforceMetadata
{
    public partial class PermissionsUpdater : Form
    {

        private Dictionary<String, String> applicationVisible = new Dictionary<String, String>();
        private Dictionary<String, String> classAccesses = new Dictionary<String, String>();
        private Dictionary<String, String> customMetadataTypeAccesses = new Dictionary<String, String>();
        private Dictionary<String, String> customPermissions = new Dictionary<String, String>();
        private Dictionary<String, String> customSettingAccesses = new Dictionary<String, String>();
        private List<String> description = new List<String>();
        private Dictionary<String, String> emailRoutingAddressAccesses = new Dictionary<String, String>();
        private Dictionary<String, String> externalCredentialPrincipalAccesses = new Dictionary<String, String>();
        private Dictionary<String, String> externalDataSourceAccesses = new Dictionary<String, String>();
        private Dictionary<String, FieldLevelSecurity> fieldPermissions = new Dictionary<String, FieldLevelSecurity>();
        private Dictionary<String, String> flowAccesses = new Dictionary<String, String>();
        private List<String> hasActivationRequired = new List<String>();
        private List<String> label = new List<String>();
        private List<String> license = new List<String>();
        private Dictionary<String, ObjectLevelSecurity> objectPermissions = new Dictionary<String, ObjectLevelSecurity>();
        private Dictionary<String, String> pageAccesses = new Dictionary<String, String>();
        private Dictionary<String, String> recordTypeVisibilities = new Dictionary<String, String>();
        private Dictionary<String, String> tabSettings = new Dictionary<String, String>();
        private List<String> userLicense = new List<String>();
        private Dictionary<String, String> userPermissions = new Dictionary<String, String>();

        public PermissionsUpdater()
        {
            InitializeComponent();
        }

        private void tbSelectFolder_DoubleClick(object sender, EventArgs e)
        {
            this.tbSelectFolder.Text = UtilityClass.folderBrowserSelectPath("Select the Profiles/Permission Sets Folder", false, FolderEnum.ReadFrom, Properties.Settings.Default.DevelopmentDeploymentFolder);
        }

        private void tbSaveChangesTo_DoubleClick(object sender, EventArgs e)
        {
            this.tbSaveChangesTo.Text = UtilityClass.folderBrowserSelectPath("Save Updates to Folder", true, FolderEnum.SaveTo, Properties.Settings.Default.DevelopmentDeploymentFolder);
        }

        private void btnConsolidatePermissions_Click(object sender, EventArgs e)
        {
            if (this.tbSelectFolder.Text == "")
            {
                return;
            }

            String[] dirFiles = Directory.GetFiles(this.tbSelectFolder.Text);

            //Boolean overrideObjectPerms = true;

            if (this.tbDoNotOverride.Text != null)
            {
                // At this point, just get the objects and load them into the objectPermissions dictionary
                XmlDocument xd = new XmlDocument();
                xd.Load(this.tbDoNotOverride.Text);
                foreach (XmlNode nd1 in xd.ChildNodes)
                {
                    if (nd1.Name == "PermissionSet")
                    {
                        parsePermissionsXML(nd1);
                    }
                }

                //overrideObjectPerms = false;
            }

            foreach (String dirFile in dirFiles) 
            {
                XmlDocument xd = new XmlDocument();
                xd.Load(dirFile);

                foreach (XmlNode nd1 in xd.ChildNodes)
                {
                    if (nd1.Name == "PermissionSet")
                    {
                        parsePermissionsXML(nd1);
                    }
                    else if (nd1.Name == "Profile")
                    {
                        
                    }
                }
            }

            // Write all of the XML to the file
            StreamWriter sw = new StreamWriter(this.tbSaveChangesTo.Text + "\\ConsolidatedPerms.xml");
            //sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            //sw.WriteLine("<PermissionSet xmlns=\"http://soap.sforce.com/2006/04/metadata\">");

            if (this.applicationVisible.Count > 0)
            {
                foreach (String key in this.applicationVisible.Keys) 
                {
                    sw.WriteLine(this.applicationVisible[key]);
                }
            }

            if (this.classAccesses.Count > 0)
            {
                foreach (String key in this.classAccesses.Keys)
                {
                    sw.WriteLine(this.classAccesses[key]);
                }
            }

            if (this.customMetadataTypeAccesses.Count > 0)
            {
                foreach (String key in this.customMetadataTypeAccesses.Keys)
                {
                    sw.WriteLine(this.customMetadataTypeAccesses[key]);
                }
            }

            if (this.customPermissions.Count > 0)
            {
                foreach (String key in this.customPermissions.Keys)
                {
                    sw.WriteLine(this.customPermissions[key]);
                }
            }

            if (this.customSettingAccesses.Count > 0)
            {
                foreach (String key in this.customSettingAccesses.Keys)
                {
                    sw.WriteLine(this.customSettingAccesses[key]);
                }
            }

            if (this.emailRoutingAddressAccesses.Count > 0)
            {
                foreach (String key in this.emailRoutingAddressAccesses.Keys)
                {
                    sw.WriteLine(this.emailRoutingAddressAccesses[key]);
                }
            }

            if (this.externalCredentialPrincipalAccesses.Count > 0)
            {
                foreach (String key in this.externalCredentialPrincipalAccesses.Keys)
                {
                    sw.WriteLine(this.externalCredentialPrincipalAccesses[key]);
                }
            }

            if (this.externalDataSourceAccesses.Count > 0)
            {
                foreach (String key in this.externalDataSourceAccesses.Keys)
                {
                    sw.WriteLine(this.externalDataSourceAccesses[key]);
                }
            }

            if (this.fieldPermissions.Count > 0)
            {
                foreach (String key in this.fieldPermissions.Keys)
                {
                    FieldLevelSecurity fls = this.fieldPermissions[key];
                    sw.Write("<fieldPermissions>");
                    sw.Write("<editable>" + fls.editable + "</editable>"); 
                    sw.Write("<field>" + fls.fieldName + "</field>");
                    sw.Write("<readable>" + fls.readable + "</readable>");
                    sw.Write("</fieldPermissions>");
                    sw.Write(Environment.NewLine);
                }
            }

            if (this.flowAccesses.Count > 0)
            {
                foreach (String key in this.flowAccesses.Keys)
                {
                    sw.WriteLine(this.flowAccesses[key]);
                }
            }

            if (this.objectPermissions.Count > 0)
            {
                foreach (String key in this.objectPermissions.Keys)
                {
                    ObjectLevelSecurity ols = this.objectPermissions[key];
                    sw.Write("<objectPermissions>");
                    
                    sw.Write("<allowCreate>" + ols.allowCreate + "</allowCreate>");
                    sw.Write("<allowDelete>" + ols.allowDelete + "</allowDelete>");
                    sw.Write("<allowEdit>" + ols.allowEdit + "</allowEdit>");
                    sw.Write("<allowRead>" + ols.allowRead + "</allowRead>");
                    sw.Write("<modifyAllRecords>" + ols.modifyAllRecords + "</modifyAllRecords>");
                    sw.Write("<object>" + ols.objectName + "</object>");
                    //sw.Write("<viewAllFields>" + ols.viewAllFields + "</viewAllFields>"); // TODO: Holding off on this for now as our DevOps center is still using API vs. 60.0
                    sw.Write("<viewAllRecords>" + ols.viewAllRecords + "</viewAllRecords>");

                    sw.Write("</objectPermissions>");
                    sw.Write(Environment.NewLine);

                }
            }

            if (this.pageAccesses.Count > 0)
            {
                foreach (String key in this.pageAccesses.Keys)
                {
                    sw.WriteLine(this.pageAccesses[key]);
                }
            }

            if (this.recordTypeVisibilities.Count > 0)
            {
                foreach (String key in this.recordTypeVisibilities.Keys)
                {
                    sw.WriteLine(this.recordTypeVisibilities[key]);
                }
            }

            if (this.tabSettings.Count > 0)
            {
                foreach (String key in this.tabSettings.Keys)
                {
                    sw.WriteLine(this.tabSettings[key]);
                }
            }

            if (this.userPermissions.Count > 0)
            {
                foreach (String key in this.userPermissions.Keys)
                {
                    sw.WriteLine(this.userPermissions[key]);
                }
            }

            //sw.WriteLine("</PermissionSet>");
            sw.Close();
        }

        private void parsePermissionsXML(XmlNode nd1)
        {
            foreach (XmlNode nd2 in nd1.ChildNodes)
            {
                if (nd2.Name == "applicationVisibilities")
                {
                    if (!this.applicationVisible.ContainsKey(nd2.ChildNodes[0].InnerText) &&
                        nd2.ChildNodes[1].InnerText == "true")
                    {
                        this.applicationVisible.Add(nd2.ChildNodes[0].InnerText, nd2.OuterXml);
                    }
                }
                else if (nd2.Name == "classAccesses")
                {
                    if (!this.classAccesses.ContainsKey(nd2.ChildNodes[0].InnerText) &&
                        nd2.ChildNodes[1].InnerText == "true")
                    {
                        this.classAccesses.Add(nd2.ChildNodes[0].InnerText, nd2.OuterXml);
                    }
                }
                else if (nd2.Name == "customMetadataTypeAccesses")
                {
                    if (!this.customMetadataTypeAccesses.ContainsKey(nd2.ChildNodes[1].InnerText) &&
                        nd2.ChildNodes[0].InnerText == "true")
                    {
                        this.customMetadataTypeAccesses.Add(nd2.ChildNodes[1].InnerText, nd2.OuterXml);
                    }
                }
                else if (nd2.Name == "customPermissions")
                {
                    if (!this.customPermissions.ContainsKey(nd2.ChildNodes[1].InnerText) &&
                        nd2.ChildNodes[0].InnerText == "true")
                    {
                        this.customPermissions.Add(nd2.ChildNodes[1].InnerText, nd2.OuterXml);
                    }
                }
                else if (nd2.Name == "customSettingAccesses")
                {
                    if (!this.customSettingAccesses.ContainsKey(nd2.ChildNodes[1].InnerText) &&
                        nd2.ChildNodes[0].InnerText == "true")
                    {
                        this.customSettingAccesses.Add(nd2.ChildNodes[1].InnerText, nd2.OuterXml);
                    }
                }
                else if (nd2.Name == "description")
                {
                    // TODO or leave block empty
                }
                else if (nd2.Name == "emailRoutingAddressAccesses")
                {
                    if (!this.emailRoutingAddressAccesses.ContainsKey(nd2.ChildNodes[1].InnerText) &&
                        nd2.ChildNodes[0].InnerText == "true")
                    {
                        this.emailRoutingAddressAccesses.Add(nd2.ChildNodes[1].InnerText, nd2.OuterXml);
                    }
                }
                else if (nd2.Name == "externalCredentialPrincipalAccesses")
                {
                    if (!this.externalCredentialPrincipalAccesses.ContainsKey(nd2.ChildNodes[1].InnerText) &&
                        nd2.ChildNodes[0].InnerText == "true")
                    {
                        this.externalCredentialPrincipalAccesses.Add(nd2.ChildNodes[1].InnerText, nd2.OuterXml);
                    }
                }
                else if (nd2.Name == "externalDataSourceAccesses")
                {
                    if (!this.externalDataSourceAccesses.ContainsKey(nd2.ChildNodes[1].InnerText) &&
                        nd2.ChildNodes[0].InnerText == "true")
                    {
                        this.externalDataSourceAccesses.Add(nd2.ChildNodes[1].InnerText, nd2.OuterXml);
                    }
                }
                else if (nd2.Name == "fieldPermissions")
                {
                    // TODO:
                    if (this.fieldPermissions.ContainsKey(nd2.ChildNodes[1].InnerText))
                    {
                        FieldLevelSecurity pfls = this.fieldPermissions[nd2.ChildNodes[1].InnerText];
                        if (nd2.ChildNodes[0].InnerText == "true" && pfls.editable == "false")
                        {
                            this.fieldPermissions[nd2.ChildNodes[1].InnerText].editable = "true";
                        }
                    }
                    else
                    {
                        FieldLevelSecurity pfls = new FieldLevelSecurity();
                        pfls.fieldName = nd2.ChildNodes[1].InnerText;
                        pfls.readable = nd2.ChildNodes[2].InnerText;
                        pfls.editable = nd2.ChildNodes[0].InnerText;

                        this.fieldPermissions.Add(pfls.fieldName, pfls);
                    }
                }
                else if (nd2.Name == "flowAccesses")
                {
                    if (!this.externalDataSourceAccesses.ContainsKey(nd2.ChildNodes[1].InnerText) &&
                        nd2.ChildNodes[0].InnerText == "true")
                    {
                        this.externalDataSourceAccesses.Add(nd2.ChildNodes[1].InnerText, nd2.OuterXml);
                    }
                }
                else if (nd2.Name == "hasActivationRequired")
                {
                    // TODO or leave block empty
                }
                else if (nd2.Name == "label")
                {
                    // TODO or leave block empty
                }
                else if (nd2.Name == "license")
                {
                    // TODO or leave block empty
                }
                else if (nd2.Name == "objectPermissions")
                {
                    if (!this.objectPermissions.ContainsKey(nd2.ChildNodes[5].InnerText))
                    {
                        ObjectLevelSecurity ols = new ObjectLevelSecurity();
                        ols.objectName       = nd2.ChildNodes[5].InnerText;
                        ols.allowCreate      = nd2.ChildNodes[0].InnerText;
                        ols.allowDelete      = nd2.ChildNodes[1].InnerText;
                        ols.allowEdit        = nd2.ChildNodes[2].InnerText;
                        ols.allowRead        = nd2.ChildNodes[3].InnerText;
                        ols.modifyAllRecords = nd2.ChildNodes[4].InnerText;

                        if (nd2.ChildNodes.Count == 7)
                        {
                            ols.viewAllFields = "false";
                            ols.viewAllRecords = nd2.ChildNodes[6].InnerText;
                        }
                        else if (nd2.ChildNodes.Count == 8)
                        {
                            ols.viewAllFields = nd2.ChildNodes[6].InnerText;
                            ols.viewAllRecords = nd2.ChildNodes[7].InnerText;
                        }

                        this.objectPermissions.Add(nd2.ChildNodes[5].InnerText, ols);
                    }
                    else if (this.objectPermissions.ContainsKey(nd2.ChildNodes[5].InnerText))
                    {
                        ObjectLevelSecurity ols = this.objectPermissions[nd2.ChildNodes[5].InnerText];

                        // Determine if the permissions on the object are greater or not
                        if (ols.allowCreate == "false" && nd2.ChildNodes[0].InnerText == "true")
                        {
                            ols.allowCreate = "true";
                        }

                        if (ols.allowDelete == "false" && nd2.ChildNodes[1].InnerText == "true")
                        {
                            ols.allowDelete = "true";
                        }

                        if (ols.allowEdit == "false" && nd2.ChildNodes[2].InnerText == "true")
                        {
                            ols.allowEdit = "true";
                        }

                        if (ols.allowRead == "false" && nd2.ChildNodes[3].InnerText == "true")
                        {
                            ols.allowRead = "true";
                        }

                        if (ols.modifyAllRecords == "false" && nd2.ChildNodes[4].InnerText == "true")
                        {
                            ols.modifyAllRecords = "true";
                        }

                        if (nd2.ChildNodes.Count == 7)
                        {
                            if (ols.viewAllRecords == "false" && nd2.ChildNodes[6].InnerText == "true")
                            {
                                ols.viewAllRecords = "true";
                            }
                        }
                        else if (nd2.ChildNodes.Count == 8)
                        {
                            if (ols.viewAllFields == "false" && nd2.ChildNodes[6].InnerText == "true")
                            {
                                ols.viewAllFields = "true";
                            }

                            if (ols.viewAllRecords == "false" && nd2.ChildNodes[7].InnerText == "true")
                            {
                                ols.viewAllRecords = "true";
                            }
                        }
                    }
                }
                else if (nd2.Name == "pageAccesses")
                {
                    if (!this.pageAccesses.ContainsKey(nd2.ChildNodes[0].InnerText) &&
                        nd2.ChildNodes[1].InnerText == "true")
                    {
                        this.pageAccesses.Add(nd2.ChildNodes[0].InnerText, nd2.OuterXml);
                    }
                }
                else if (nd2.Name == "recordTypeVisibilities")
                {
                    if (!this.recordTypeVisibilities.ContainsKey(nd2.ChildNodes[0].InnerText) &&
                        nd2.ChildNodes[1].InnerText == "true")
                    {
                        this.recordTypeVisibilities.Add(nd2.ChildNodes[0].InnerText, nd2.OuterXml);
                    }
                }
                else if (nd2.Name == "tabSettings")
                {
                    if (!this.tabSettings.ContainsKey(nd2.ChildNodes[0].InnerText) &&
                        (nd2.ChildNodes[1].InnerText == "Available" || nd2.ChildNodes[1].InnerText == "Visible"))
                    {
                        this.tabSettings.Add(nd2.ChildNodes[0].InnerText, nd2.OuterXml);
                    }
                }
                else if (nd2.Name == "userLicense")
                {
                    // TODO or leave block empty
                }
                else if (nd2.Name == "userPermissions")
                {
                    if (!this.userPermissions.ContainsKey(nd2.ChildNodes[1].InnerText) &&
                        nd2.ChildNodes[0].InnerText == "true")
                    {
                        this.userPermissions.Add(nd2.ChildNodes[1].InnerText, nd2.OuterXml);
                    }
                }
            }
        }

        public class FieldLevelSecurity
        {
            public String fieldName;
            public String readable;
            public String editable;
        }

        public class ObjectLevelSecurity
        {
            public String objectName;
            public String allowCreate;
            public String allowDelete;
            public String allowEdit;
            public String allowRead;
            public String modifyAllRecords;
            public String viewAllFields;
            public String viewAllRecords;
        }

    }
}
