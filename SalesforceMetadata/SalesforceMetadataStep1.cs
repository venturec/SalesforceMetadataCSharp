﻿using System;
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
                            || ctrl.Name == "btnRetrieveMetadata"
                            || ctrl.Name == "tbExistingPackageXml")
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
                sfMetadataStep2.btnRetrieveMetadata.Enabled = false;
                sfMetadataStep2.selectedItems = new Dictionary<String, List<String>>();
                sfMetadataStep2.tbFromOrgSaveLocation.Text = Properties.Settings.Default.MetadataLastSaveToLocation;

                foreach (String si in selItems)
                {
                    sfMetadataStep2.selectedItems.Add(si, new List<string> { "*" });
                }

                foreach (Control ctrl in sfMetadataStep2.Controls)
                {
                    if (ctrl.Name == "tbFromOrgSaveLocation"
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
            mtrf.cmbUserName.Text = this.cmbUserName.Text;
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

        private void btnGeneratePackageXML_Click(object sender, EventArgs e)
        {

        }
    }
}
