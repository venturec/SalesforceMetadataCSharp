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
using System.Xml.Linq;

namespace SalesforceMetadata
{
    public partial class LandingPage : System.Windows.Forms.Form
    {
        private frmUserSettings userSetting;

        public LandingPage()
        {
            InitializeComponent();
            checkCredentialFiles();
        }

        private void btnMetadataForm_Click(object sender, EventArgs e)
        {
            SalesforceMetadataStep1 sfMetadata = new SalesforceMetadataStep1();
            sfMetadata.Show();
            sfMetadata.Location = this.Location;
        }

        private void checkCredentialFiles()
        { 
            String fileName = Properties.Settings.Default.UserAndAPIFileLocation;

            if (fileName == "" || !File.Exists(fileName))
            {
                addUserAndSOAPAPIAddress_Click(null, null);
            }
        }

        private void addUserAndSOAPAPIAddress_Click(object sender, EventArgs e)
        {
            if (userSetting == null)
            {
                userSetting = new frmUserSettings();
                userSetting.Show();
                userSetting.Location = this.Location;
            }
            else if (userSetting.IsDisposed)
            {
                userSetting = new frmUserSettings();
                userSetting.Show();
                userSetting.Location = this.Location;
                userSetting.BringToFront();
            }
            else
            {
                userSetting.Show();
                userSetting.Location = this.Location;
                userSetting.BringToFront();
            }
        }

        private void btnSearchMetadata_Click(object sender, EventArgs e)
        {
            frmSearch srch = new frmSearch();
            srch.Show();
            srch.Location = this.Location;
        }

        private void BtnParseDebugLogs_Click(object sender, EventArgs e)
        {
            frmParseDebugLogs parseDebugs = new frmParseDebugLogs();
            parseDebugs.Show();
            parseDebugs.Location = this.Location;
        }

        private void btnRetrieveRecords_Click(object sender, EventArgs e)
        {
            RESTService sms1 = new RESTService();
            sms1.Show();
        }

        private void btnOpenObjectFieldInspector_Click(object sender, EventArgs e)
        {
            ObjectFieldInspector ofi = new ObjectFieldInspector();
            ofi.Show();
        }

        private void btnOpenObjectModification_Click(object sender, EventArgs e)
        {
            ObjectModification omd = new ObjectModification();
            omd.Show();
        }

        private void btnGenerateConfigWorkbook_Click(object sender, EventArgs e)
        {
            ConfigurationWorkbook cw = new ConfigurationWorkbook();
            cw.Show();
        }

        private void btnExtractHTMLPages_Click(object sender, EventArgs e)
        {
            ExtractWebsites ew = new ExtractWebsites();
            ew.Show();
        }

        private void btnExtractClassesMethods_Click(object sender, EventArgs e)
        {
            ClassMethodExtractor cme = new ClassMethodExtractor();
            cme.Show();
        }

        private void MetadataComparison_Click(object sender, EventArgs e)
        {
            MetadataComparison mc = new MetadataComparison();
            mc.Show();
        }

        private void btnUpdateAPIVersion_Click(object sender, EventArgs e)
        {
            VersionUpdater vu = new VersionUpdater();
            vu.Show();
        }

        private void btnGenerateDeploymentPackage_Click(object sender, EventArgs e)
        {
            GenerateDeploymentPackage gdp = new GenerateDeploymentPackage();
            gdp.Show();
        }

        private void btnGenerateObjectFieldsFromXML_Click(object sender, EventArgs e)
        {
            ExtractFieldsFromMetadata efm = new ExtractFieldsFromMetadata();
            efm.Show();
        }

        private void btnParseLWC_Click(object sender, EventArgs e)
        {
            LWCInspector lwci = new LWCInspector();
            lwci.Show();
        }
    }
}
