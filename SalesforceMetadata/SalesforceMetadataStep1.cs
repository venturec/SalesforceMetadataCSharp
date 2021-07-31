using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

using SalesforceMetadata.PartnerWSDL;
using SalesforceMetadata.MetadataWSDL;
using SalesforceMetadata.ToolingWSDL;

namespace SalesforceMetadata
{
    public partial class SalesforceMetadataStep1 : System.Windows.Forms.Form
    {
        // Key = the Metadata Name. Value = the folder the metadata file comes from
        private Dictionary<String, String> metadataXmlNameToFolder;
        private Dictionary<String, String> usernameToSecurityToken;
        private frmUserSettings userSetting;
        private List<String> metadataObjectsList;

        private UtilityClass.REQUESTINGORG reqOrg;

        public SalesforceMetadataStep1()
        {
            InitializeComponent();
            populateCredentialsFile();
        }


        private void btnGetMetadataTypes_Click(object sender, EventArgs e)
        {
            this.lbMetadataTypes.Items.Clear();
            this.reqOrg = UtilityClass.REQUESTINGORG.FROMORG;

            if (this.cmbUserName.Text == "" || this.tbPassword.Text == "")
            {
                MessageBox.Show("Please enter your credentials before continuing");
            }
            else
            {
                SalesforceCredentials.fromOrgUsername = this.cmbUserName.Text;
                SalesforceCredentials.fromOrgPassword = this.tbPassword.Text;
                SalesforceCredentials.fromOrgSecurityToken = this.tbSecurityToken.Text;
                getMetadataTypes(this.cmbUserName.Text);
            }
        }

        private void btnGetMetadataTypesToOrg_Click(object sender, EventArgs e)
        {
            this.lbToMetadataTypes.Items.Clear();
            this.reqOrg = UtilityClass.REQUESTINGORG.TOORG;

            if (this.cmbToOrgUsername.Text == "" || this.tbToOrgPassword.Text == "")
            {
                MessageBox.Show("Please enter your credentials before continuing");
            }
            else
            {
                SalesforceCredentials.toOrgUsername = this.cmbToOrgUsername.Text;
                SalesforceCredentials.toOrgPassword = this.tbToOrgPassword.Text;
                SalesforceCredentials.toOrgSecurityToken = this.tbToOrgSecurityToken.Text;
                getMetadataTypes(this.cmbToOrgUsername.Text);
            }
        }


        private void getMetadataTypes(String userName)
        {
            Boolean loginSuccess = SalesforceCredentials.salesforceLogin(reqOrg);
            if (loginSuccess == false)
            {
                MessageBox.Show("Please check username, password and/or security token");
                return;
            }

            DescribeMetadataResult dmd = new DescribeMetadataResult();
            dmd = SalesforceCredentials.getDescribeMetadataResult(reqOrg);

            DescribeMetadataObject[] dmdObjects = dmd.metadataObjects;

            // Populate the picklist with the XML name
            metadataObjectsList = new List<String>();
            metadataXmlNameToFolder = new Dictionary<String, String>();
            foreach (DescribeMetadataObject obj in dmdObjects)
            {
                metadataObjectsList.Add(obj.xmlName);
                metadataXmlNameToFolder.Add(obj.xmlName, obj.directoryName);
            }

            metadataObjectsList.Sort();

            List<String> defObjectList = new List<String>();
            if (SalesforceCredentials.defaultWsdlObjects.ContainsKey(userName))
            {
                defObjectList = SalesforceCredentials.defaultWsdlObjects[userName];
            }

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                foreach (String s in metadataObjectsList)
                {
                    if (defObjectList.Contains(s))
                    {
                        lbMetadataTypes.Items.Add(s, true);
                    }
                    else
                    {
                        lbMetadataTypes.Items.Add(s, false);
                    }
                }
            }
            else if (reqOrg == UtilityClass.REQUESTINGORG.TOORG)
            {
                foreach (String s in metadataObjectsList)
                {
                    if (defObjectList.Contains(s))
                    {
                        lbToMetadataTypes.Items.Add(s, true);
                    }
                    else
                    {
                        lbToMetadataTypes.Items.Add(s, false);
                    }
                }
            }

            SalesforceCredentials.salesforceLogout();
            btnGetMetadataTypes.Enabled = false;
        }


        private void cmbUserName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SalesforceCredentials.isProduction[this.cmbUserName.Text] == true)
            {
                this.lblSalesforce.Text = "Salesforce";
                this.Text = "Salesforce Metadata - Production";
            }
            else
            {
                this.lblSalesforce.Text = "Salesforce Sandbox";
                String[] userNamesplit = this.cmbUserName.Text.Split('.');
                this.Text = "Salesforce Metadata - " + userNamesplit[userNamesplit.Length - 1].ToUpper();
            }

            this.tbSecurityToken.Text = "";
            if (this.usernameToSecurityToken.ContainsKey(this.cmbUserName.Text))
            {
                this.tbSecurityToken.Text = this.usernameToSecurityToken[cmbUserName.Text];
            }

            this.btnGetMetadataTypes.Enabled = true;
            this.cbSelectAll.Checked = false;
            this.cbSelectNone.Checked = false;
            this.tbPassword.Text = "";
            this.lbMetadataTypes.Items.Clear();
        }


        private void btnRetrieveMetadata_Click(object sender, EventArgs e)
        {
            if (this.cmbUserName.Text == "" || this.tbPassword.Text == "")
            {
                MessageBox.Show("Please enter your credentials before continuing");
            }
            else
            {
                this.reqOrg = UtilityClass.REQUESTINGORG.FROMORG;

                SalesforceCredentials.fromOrgUsername = this.cmbUserName.Text;
                SalesforceCredentials.fromOrgPassword = this.tbPassword.Text;
                SalesforceCredentials.fromOrgSecurityToken = this.tbSecurityToken.Text;

                CheckedListBox.CheckedItemCollection selItems = this.lbMetadataTypes.CheckedItems;

                // Validate if the form is already opened and bring it to the front if it is.
                Boolean isAlreadyOpen = false;
                FormCollection fc = Application.OpenForms;
                foreach (System.Windows.Forms.Form openFrm in fc)
                {
                    if (openFrm.Name == "SalesforceMetadataStep2")
                    {
                        SalesforceMetadataStep2 sfMetadataStep2 = (SalesforceMetadataStep2)Application.OpenForms["SalesforceMetadataStep2"];
                        sfMetadataStep2.selectedItems.Clear();

                        foreach (String si in selItems)
                        {
                            sfMetadataStep2.selectedItems.Add(si);
                        }

                        foreach (Control ctrl in openFrm.Controls)
                        {
                            if (ctrl.Name == "tbFromOrgSaveLocation"
                                || ctrl.Name == "btnRetrieveMetadataFromSelected"
                                || ctrl.Name == "tbExistingPackageXml"
                                || ctrl.Name == "btnRetrieveMetadata")
                            {
                                ctrl.Enabled = true;
                            }
                            else if (ctrl.Name == "tbToOrgSaveLocation"
                                    || ctrl.Name == "btnToOrgRetrieveMetadata"
                                    || ctrl.Name == "tbToOrgExistingPackageXml"
                                    || ctrl.Name == "btnToOrgRetrieveMetadataWPkg")
                            {
                                ctrl.Enabled = false;
                            }
                        }
                            
                        openFrm.Show();
                        openFrm.Location = this.Location;
                        openFrm.BringToFront();
                        isAlreadyOpen = true;
                    }
                }

                if (isAlreadyOpen == false)
                {
                    SalesforceMetadataStep2 sfMetadataStep2 = new SalesforceMetadataStep2();
                    sfMetadataStep2.btnRetrieveMetadataFromSelected.Enabled = false;
                    sfMetadataStep2.btnToOrgRetrieveMetadata.Enabled = false;
                    sfMetadataStep2.selectedItems = new List<String>();

                    foreach (String si in selItems)
                    {
                        sfMetadataStep2.selectedItems.Add(si);
                    }

                    foreach (Control ctrl in sfMetadataStep2.Controls)
                    {
                        if (ctrl.Name == "tbFromOrgSaveLocation"
                            || ctrl.Name == "btnRetrieveMetadataFromSelected"
                            || ctrl.Name == "tbExistingPackageXml"
                            || ctrl.Name == "btnRetrieveMetadata")
                        {
                            ctrl.Enabled = true;
                        }
                        else if (ctrl.Name == "tbToOrgSaveLocation"
                                || ctrl.Name == "btnToOrgRetrieveMetadata"
                                || ctrl.Name == "tbToOrgExistingPackageXml"
                                || ctrl.Name == "btnToOrgRetrieveMetadataWPkg")
                        {
                            ctrl.Enabled = false;
                        }
                    }

                    sfMetadataStep2.Show();
                    sfMetadataStep2.Location = this.Location;
                }
            }
        }


        private void btnRetrieveMetadataFromDeployTo_Click(object sender, EventArgs e)
        {
            if (this.cmbToOrgUsername.Text == "" || this.tbToOrgPassword.Text == "")
            {
                MessageBox.Show("Please enter the Deploy To credentials before continuing");
            }
            else
            {
                this.reqOrg = UtilityClass.REQUESTINGORG.TOORG;

                SalesforceCredentials.toOrgUsername = this.cmbToOrgUsername.Text;
                SalesforceCredentials.toOrgPassword = this.tbToOrgPassword.Text;
                SalesforceCredentials.toOrgSecurityToken = this.tbToOrgSecurityToken.Text;

                CheckedListBox.CheckedItemCollection selItems = this.lbToMetadataTypes.CheckedItems;

                // Validate if the form is already opened and bring it to the front if it is.
                Boolean isAlreadyOpen = false;
                FormCollection fc = Application.OpenForms;
                foreach (System.Windows.Forms.Form openFrm in fc)
                {
                    if (openFrm.Name == "SalesforceMetadataStep2")
                    {
                        SalesforceMetadataStep2 sfMetadataStep2 = (SalesforceMetadataStep2)Application.OpenForms["SalesforceMetadataStep2"];
                        sfMetadataStep2.selectedItems.Clear();

                        foreach (String si in selItems)
                        {
                            sfMetadataStep2.selectedItems.Add(si);
                        }

                        foreach (Control ctrl in openFrm.Controls)
                        {
                            if (ctrl.Name == "tbFromOrgSaveLocation"
                                || ctrl.Name == "btnRetrieveMetadataFromSelected"
                                || ctrl.Name == "tbExistingPackageXml"
                                || ctrl.Name == "btnRetrieveMetadata")
                            {
                                ctrl.Enabled = false;
                            }
                            else if (ctrl.Name == "tbToOrgSaveLocation"
                                    || ctrl.Name == "btnToOrgRetrieveMetadata"
                                    || ctrl.Name == "tbToOrgExistingPackageXml"
                                    || ctrl.Name == "btnToOrgRetrieveMetadataWPkg")
                            {
                                ctrl.Enabled = true;
                            }
                        }

                        openFrm.Show();
                        openFrm.Location = this.Location;
                        openFrm.BringToFront();
                        isAlreadyOpen = true;
                    }
                }

                if (isAlreadyOpen == false)
                {
                    SalesforceMetadataStep2 sfMetadataStep2 = new SalesforceMetadataStep2();
                    sfMetadataStep2.btnRetrieveMetadataFromSelected.Enabled = false;
                    sfMetadataStep2.btnToOrgRetrieveMetadata.Enabled = false;
                    sfMetadataStep2.selectedItems = new List<String>();

                    foreach (String si in selItems)
                    {
                        sfMetadataStep2.selectedItems.Add(si);
                    }

                    foreach (Control ctrl in sfMetadataStep2.Controls)
                    {
                        if (ctrl.Name == "tbFromOrgSaveLocation"
                            || ctrl.Name == "btnRetrieveMetadataFromSelected"
                            || ctrl.Name == "tbExistingPackageXml"
                            || ctrl.Name == "btnRetrieveMetadata")
                        {
                            ctrl.Enabled = false;
                        }
                        else if (ctrl.Name == "tbToOrgSaveLocation"
                                || ctrl.Name == "btnToOrgRetrieveMetadata"
                                || ctrl.Name == "tbToOrgExistingPackageXml"
                                || ctrl.Name == "btnToOrgRetrieveMetadataWPkg")
                        {
                            ctrl.Enabled = true;
                        }
                    }

                    sfMetadataStep2.Show();
                    sfMetadataStep2.Location = this.Location;
                }
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

            SalesforceCredentials.usernameWsdlUrl = new Dictionary<String, String>();
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

                SalesforceCredentials.usernameWsdlUrl.Add(username, partnerWsdlUrl);
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
            foreach (String un in SalesforceCredentials.usernameWsdlUrl.Keys)
            {
                this.cmbUserName.Items.Add(un);
                this.cmbToOrgUsername.Items.Add(un);
            }
        }


        private void btnDeleteDebugLogs_Click(object sender, EventArgs e)
        {
            if (this.cmbUserName.Text == "" || this.tbPassword.Text == "")
            {
                MessageBox.Show("Please select a username and enter the password first before continuing");
                return;
            }
            else
            {
                SalesforceCredentials.fromOrgUsername = this.cmbUserName.Text;
                SalesforceCredentials.fromOrgPassword = this.tbPassword.Text;
                SalesforceCredentials.fromOrgSecurityToken = this.tbSecurityToken.Text;
            }

            Boolean loginSuccess = SalesforceCredentials.salesforceLogin(UtilityClass.REQUESTINGORG.FROMORG);
            if (loginSuccess == false)
            {
                MessageBox.Show("Please check username, password and/or security token");
                return;
            }

            String userId = "";
            userId = SalesforceCredentials.fromOrgLR.userId;

            String selectStatement = "SELECT Id FROM ApexLog WHERE LogUserId = \'" + userId + "\'";

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
                else if(qr.records != null)
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


        private void addUserAndSOAPAPIAddress()
        {
            userSetting = new frmUserSettings();

            if (userSetting.IsDisposed)
            {
                userSetting.Show();
                userSetting.Location = this.Location;
                userSetting.TopMost = true;
            }
            else
            {
                userSetting.Show();
                userSetting.Location = this.Location;
                userSetting.TopMost = true;
            }
        }

        private void btnDeploy_Click(object sender, EventArgs e)
        {
            if (this.cmbToOrgUsername.Text == "" || this.tbToOrgPassword.Text == "")
            {
                MessageBox.Show("Please enter your credentials before continuing");
            }
            else
            {
                SalesforceCredentials.fromOrgUsername = null;
                SalesforceCredentials.fromOrgPassword = null;
                SalesforceCredentials.fromOrgSecurityToken = null;
                SalesforceCredentials.toOrgUsername = this.cmbToOrgUsername.Text;
                SalesforceCredentials.toOrgPassword = this.tbToOrgPassword.Text;
                SalesforceCredentials.toOrgSecurityToken = this.tbToOrgSecurityToken.Text;

                // Validate if the form is already opened and bring it to the front if it is.
                Boolean isAlreadyOpen = false;
                FormCollection fc = Application.OpenForms;
                foreach (System.Windows.Forms.Form openFrm in fc)
                {
                    if (openFrm.Name == "DeployMetadata")
                    {
                        openFrm.Show();
                        openFrm.Location = this.Location;
                        openFrm.BringToFront();
                        isAlreadyOpen = true;
                    }
                }

                if (isAlreadyOpen == false)
                {
                    DeployMetadata dm = new DeployMetadata();
                    dm.isProduction = SalesforceCredentials.isProduction[this.cmbToOrgUsername.Text];
                    dm.Show();
                    dm.Location = this.Location;
                }
            }
        }


        private void btnDevSBSeeding_Click(object sender, EventArgs e)
        {
            if (this.cmbUserName.Text == ""
                || this.tbPassword.Text == ""
                || this.cmbToOrgUsername.Text == ""
                || this.tbToOrgPassword.Text == "")
            {
                MessageBox.Show("Please enter credentials in the From/To areas before continuing");
            }
            else
            {
                RESTService sndBoxSeeding = new RESTService();
                sndBoxSeeding.Show();
                sndBoxSeeding.Location = this.Location;
            }
        }

        private void btnSobjectFieldInspector_Click(object sender, EventArgs e)
        {
            if (this.cmbUserName.Text == ""
                || this.tbPassword.Text == "")
            {
                MessageBox.Show("Please enter credentials in the From/To areas before continuing");
            }
            else
            {
                SalesforceCredentials.fromOrgUsername = this.cmbUserName.Text;
                SalesforceCredentials.fromOrgPassword = this.tbPassword.Text;
                SalesforceCredentials.fromOrgSecurityToken = this.tbSecurityToken.Text;

                ObjectFieldInspector ofi = new ObjectFieldInspector();
                ofi.reqOrg = UtilityClass.REQUESTINGORG.FROMORG;
                ofi.Show();
                ofi.Location = this.Location;
            }
        }

        private void cbSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            if (cb.Checked == true)
            {
                this.cbSelectNone.Checked = false;
                lbMetadataTypes.Items.Clear();
                foreach (String s in metadataObjectsList)
                {
                    if (s == "Report"
                        || s == "ReportType"
                        || s == "ContentAsset")
                    {
                        lbMetadataTypes.Items.Add(s, false);
                    }
                    else
                    {
                        lbMetadataTypes.Items.Add(s, true);
                    }
                }
            }
        }

        private void cbSelectNone_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if(cb.Checked == true)
            {
                this.cbSelectAll.Checked = false;
                lbMetadataTypes.Items.Clear();
                foreach (String s in metadataObjectsList)
                {
                    lbMetadataTypes.Items.Add(s, false);
                }
            }
        }

        private void cmbToOrgUsername_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.tbToOrgSecurityToken.Text = "";
            if (this.usernameToSecurityToken.ContainsKey(this.cmbToOrgUsername.Text))
            {
                this.tbToOrgSecurityToken.Text = this.usernameToSecurityToken[cmbToOrgUsername.Text];
            }

            this.btnGetMetadataTypesToOrg.Enabled = true;
            this.cbToSelectAll.Checked = false;
            this.cbToSelectNone.Checked = false;
            this.tbToOrgPassword.Text = "";
            this.lbToMetadataTypes.Items.Clear();
        }



        private void cbToSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            if (cb.Checked == true)
            {
                this.cbToSelectNone.Checked = false;
                lbToMetadataTypes.Items.Clear();
                foreach (String s in metadataObjectsList)
                {
                    lbToMetadataTypes.Items.Add(s, true);
                }
            }
        }

        private void cbToSelectNone_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if (cb.Checked == true)
            {
                this.cbToSelectAll.Checked = false;
                lbToMetadataTypes.Items.Clear();
                foreach (String s in metadataObjectsList)
                {
                    lbToMetadataTypes.Items.Add(s, false);
                }
            }
        }

        private void btnFromGenerateToolingChangeReport_Click(object sender, EventArgs e)
        {
            if (this.cmbUserName.Text == ""
                || this.tbPassword.Text == ""
                || this.tbSecurityToken.Text == "")
            {
                MessageBox.Show("Please provide the credentials necessary to continue");
            }
            else
            {
                MetadataToolingReportForm mtrf = new MetadataToolingReportForm();
                mtrf.userName = this.cmbUserName.Text;
                mtrf.password = this.tbPassword.Text;
                mtrf.securityToken = this.tbSecurityToken.Text;

                mtrf.Show();
            }
        }

        private void btnConfigurationWorkbook_Click(object sender, EventArgs e)
        {
            ConfigurationWorkbook cw = new ConfigurationWorkbook();

            cw.Show();
        }
    }
}
