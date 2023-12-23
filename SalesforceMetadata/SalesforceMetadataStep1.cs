using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

using SalesforceMetadata.MetadataWSDL;

using System.Runtime.CompilerServices;

namespace SalesforceMetadata
{
    public partial class SalesforceMetadataStep1 : System.Windows.Forms.Form
    {
        private SalesforceCredentials sc;

        // Key = the Metadata Name. Value = the folder the metadata file comes from
        private Dictionary<String, String> metadataXmlNameToFolder;
        private UserSettings userSetting;
        private List<String> metadataObjectsList;

        //private UtilityClass.REQUESTINGORG reqOrg;

        private String orgName;

        public SalesforceMetadataStep1()
        {
            InitializeComponent();
            this.orgName = "";
            sc = new SalesforceCredentials();
            populateCredentialsFile();
        }

        private void btnGetMetadataTypes_Click(object sender, EventArgs e)
        {
            this.lbMetadataTypes.Items.Clear();

            if (String.IsNullOrEmpty(this.cmbUserName.Text))
            {
                MessageBox.Show("Please enter your credentials before continuing");
            }
            else
            {
                getMetadataTypes(this.cmbUserName.Text);
            }
        }

        private void getMetadataTypes(String userName)
        {
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

            DescribeMetadataResult dmd = new DescribeMetadataResult();
            dmd = sc.getDescribeMetadataResult(UtilityClass.REQUESTINGORG.FROMORG);

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
            if (sc.defaultWsdlObjects.ContainsKey(userName))
            {
                defObjectList = sc.defaultWsdlObjects[userName];
            }

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

            sc.salesforceLogout();
            btnGetMetadataTypes.Enabled = false;
            btnExportMetadataTypes.Enabled = true;
        }


        private void cmbUserName_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Text = "Salesforce Metadata";

            if (sc.isProduction[this.cmbUserName.Text] == true)
            {
                this.lblSalesforce.Text = "Salesforce";
                this.Text = "Salesforce Metadata - Production";
                this.orgName = "Production";

                this.btnGetMetadataTypes.Enabled = true;
            }
            else
            {
                this.lblSalesforce.Text = "Salesforce Sandbox";
                String[] userNamesplit = this.cmbUserName.Text.Split('.');
                this.orgName = userNamesplit[userNamesplit.Length - 1].ToUpper();
                this.Text = "Salesforce Metadata - " + this.orgName;

                this.btnGetMetadataTypes.Enabled = true;

            }

            this.lbMetadataTypes.Items.Clear();
        }


        private void btnRetrieveMetadata_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.cmbUserName.Text))
            {
                MessageBox.Show("Please select a Username before continuing");
                return;
            }

            CheckedListBox.CheckedItemCollection selItems = this.lbMetadataTypes.CheckedItems;

            // Validate if the form is already opened and bring it to the front if it is.
            Boolean isAlreadyOpen = false;
            FormCollection fc = Application.OpenForms;
            foreach (System.Windows.Forms.Form openFrm in fc)
            {
                if (openFrm.Name == "SalesforceMetadataStep2")
                {
                    SalesforceMetadataStep2 sfMetadataStep2 = (SalesforceMetadataStep2)Application.OpenForms["SalesforceMetadataStep2"];
                    sfMetadataStep2.userName = this.cmbUserName.Text;
                    sfMetadataStep2.selectedItems.Clear();

                    foreach (String si in selItems)
                    {
                        sfMetadataStep2.selectedItems.Add(si, new List<string> { "*" });
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
                        else if (ctrl.Name == "lblRetrieveFromOrg")
                        {
                            ctrl.Text = "Retrieve Metadata from " + this.orgName;
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
                sfMetadataStep2.userName = this.cmbUserName.Text;
                sfMetadataStep2.btnRetrieveMetadataFromSelected.Enabled = false;
                sfMetadataStep2.selectedItems = new Dictionary<String, List<String>>();
                sfMetadataStep2.tbFromOrgSaveLocation.Text = Properties.Settings.Default.MetadataLastSaveToLocation;

                foreach (String si in selItems)
                {
                    sfMetadataStep2.selectedItems.Add(si, new List<string> { "*" });
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
                    else if (ctrl.Name == "lblRetrieveFromOrg")
                    {
                        ctrl.Text = "Retrieve Metadata from " + this.orgName;
                    }
                }

                sfMetadataStep2.Show();
                sfMetadataStep2.Location = this.Location;
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

            populateUserNames();
        }

        private void populateUserNames()
        {
            foreach (String un in sc.usernamePartnerUrl.Keys)
            {
                this.cmbUserName.Items.Add(un);
            }
        }

        private void btnDeleteDebugLogs_Click(object sender, EventArgs e)
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
            if (cbAllDebugLogs.Checked)
            {
                // TODO: message box confirmation that they are going to delete everyone's debug log files including their own

                selectStatement = "SELECT Id FROM ApexLog";
            }
            else
            {
                String userId = "";
                userId = sc.fromOrgLR.userId;

                selectStatement = "SELECT Id FROM ApexLog WHERE LogUserId = \'" + userId + "\'";
            }

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


        private void addUserAndSOAPAPIAddress()
        {
            userSetting = new UserSettings();

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
            // Validate if the form is already opened and bring it to the front if it is.
            Boolean isAlreadyOpen = false;
            FormCollection fc = Application.OpenForms;
            foreach (System.Windows.Forms.Form openFrm in fc)
            {
                if (openFrm.Name == "DeployMetadata")
                {
                    DeployMetadata dm = (DeployMetadata)openFrm.Tag;
                    dm.cmbUserName.SelectedItem = this.cmbUserName.Text;
                    openFrm.Show();
                    openFrm.Location = this.Location;
                    openFrm.BringToFront();
                    isAlreadyOpen = true;
                }
            }

            if (isAlreadyOpen == false)
            {
                DeployMetadata dm = new DeployMetadata();

                dm.cmbUserName.SelectedItem = this.cmbUserName.Text;
                dm.Show();
                dm.Location = this.Location;
            }
        }


        private void btnDevSBSeeding_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.cmbUserName.Text))
            {
                MessageBox.Show("Please enter credentials in the From/To areas before continuing");
                return;
            }

            RESTService sndBoxSeeding = new RESTService();
            sndBoxSeeding.Show();
            sndBoxSeeding.Location = this.Location;
        }

        private void btnSobjectFieldInspector_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.cmbUserName.Text))
            {
                MessageBox.Show("Please enter credentials in the From/To areas before continuing");
                return;
            }

            ObjectFieldInspector ofi = new ObjectFieldInspector();
            //ofi.reqOrg = UtilityClass.REQUESTINGORG.FROMORG;
            ofi.cmbUserName.SelectedItem = this.cmbUserName.Text;

            ofi.Show();
            ofi.Location = this.Location;
        }


        private void btnGenerateToolingReport_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.cmbUserName.Text))
            {
                MessageBox.Show("Please provide the credentials necessary to continue");
                return;
            }

            MetadataToolingReportForm mtrf = new MetadataToolingReportForm();
            mtrf.userName = this.cmbUserName.Text;
            mtrf.Show();
        }

        private void btnConfigurationWorkbook_Click(object sender, EventArgs e)
        {
            ConfigurationWorkbook cw = new ConfigurationWorkbook();

            cw.Show();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            HashSet<String> bypassItems = new HashSet<string> { 
                "AuthProvider", 
                "ContentAsset", 
                "InboundCertificate", 
                "PermissionSet", 
                "PermissionSetGroup",
                "Profile",
                "Report", 
                "ReportType",
                "Scontrol",
                "StaticResource"
             };

            lbMetadataTypes.Items.Clear();
            foreach (String s in metadataObjectsList)
            {
                if (bypassItems.Contains(s))
                {
                    lbMetadataTypes.Items.Add(s, false);
                }
                else
                {
                    lbMetadataTypes.Items.Add(s, true);
                }
            }
        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            lbMetadataTypes.Items.Clear();
            foreach (String s in metadataObjectsList)
            {
                lbMetadataTypes.Items.Add(s, false);
            }
        }

        private void btnSelectDefaults_Click(object sender, EventArgs e)
        {
            lbMetadataTypes.Items.Clear();

            List<String> defObjectList = new List<String>();
            if (sc.defaultWsdlObjects.ContainsKey(this.cmbUserName.Text))
            {
                defObjectList = sc.defaultWsdlObjects[this.cmbUserName.Text];
            }

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

        private void btnExportMetadataTypes_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
            xlapp.Visible = false;

            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlapp.Workbooks.Add();

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                        (System.Reflection.Missing.Value,
                                                            xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                            System.Reflection.Missing.Value,
                                                            System.Reflection.Missing.Value);

            xlWorksheet.Name = "MetadataTypes";

            xlWorksheet.Cells[1, 1].Value = "MetadataType";
            xlWorksheet.Cells[1, 2].Value = "Selected";

            Int32 rowNumber = 2;

            CheckedListBox.ObjectCollection allItems = this.lbMetadataTypes.Items;
            CheckedListBox.CheckedItemCollection checkedItems = this.lbMetadataTypes.CheckedItems;

            HashSet<String> itemsChecked = new HashSet<string>();
            foreach (String ci in checkedItems)
            {
                itemsChecked.Add(ci);

                xlWorksheet.Cells[rowNumber, 1].Value = ci;
                xlWorksheet.Cells[rowNumber, 2].Value = "True";

                rowNumber++;
            }

            foreach (String ci in allItems)
            {
                if (!itemsChecked.Contains(ci))
                {
                    xlWorksheet.Cells[rowNumber, 1].Value = ci;
                    xlWorksheet.Cells[rowNumber, 2].Value = "False";
                    rowNumber++;
                }
            }

            xlapp.Visible = true;
        }
    }
}
