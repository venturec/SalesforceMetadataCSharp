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

using SalesforceMetadata.PartnerWSDL;
using SalesforceMetadata.MetadataWSDL;
using System.Xml;

namespace SalesforceMetadata
{
    public partial class DeployMetadata : System.Windows.Forms.Form
    {
        public Boolean isProduction;

        //public String salesforceUserName;
        //public String salesforcePassword;
        //public String salesforceSecurityToken;

        private int ONE_SECOND = 1000;
        private int MAX_NUM_POLL_REQUESTS = 50;

        private Dictionary<String, String> usernameToSecurityToken;

        private UtilityClass.REQUESTINGORG reqOrg;

        private String orgName;

        public DeployMetadata()
        {
            InitializeComponent();
            populateCredentialsFile();
        }

        private void tbZipFileLocation_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "zip files (*.zip)|*.zip|All Files (*.*)|*.*";
            ofd.Title = "Please select a zip file";
            ofd.ShowDialog();

            this.tbZipFileLocation.Text = ofd.FileName;
        }

        private void btnDeployMetadata_Click(object sender, EventArgs e)
        {
            if (this.cmbUserName.Text == "" || this.tbPassword.Text == "")
            {
                MessageBox.Show("Please enter your credentials before continuing");
                return;
            }

            Boolean quickDeploySuccessful = false;
            this.rtMessages.Text = "";

            SalesforceCredentials.fromOrgUsername = null;
            SalesforceCredentials.fromOrgPassword = null;
            SalesforceCredentials.fromOrgSecurityToken = null;
            //SalesforceCredentials.toOrgUsername = this.salesforceUserName;
            //SalesforceCredentials.toOrgPassword = this.salesforcePassword;
            //SalesforceCredentials.toOrgSecurityToken = this.salesforceSecurityToken;

            SalesforceCredentials.toOrgUsername = this.cmbUserName.Text;
            SalesforceCredentials.toOrgPassword = this.tbPassword.Text;
            SalesforceCredentials.toOrgSecurityToken = this.tbSecurityToken.Text;

            Boolean loginSuccess = SalesforceCredentials.salesforceLogin(UtilityClass.REQUESTINGORG.TOORG);
            if (loginSuccess == false)
            {
                MessageBox.Show("Please check username, password and/or security token");
                return;
            }

            if (this.tbZipFileLocation.Text == null
                || this.tbZipFileLocation.Text == "")
            {
                MessageBox.Show("Please select the location where the ZIP file + package.xml is located first. Then click Deploy Metadata Package again.");
            }
            else if (this.cbCheckOnly.Checked == false
                && this.tbDeploymentValidationId.Text != "")
            {
                String asyncResultId = "";
                if (this.isProduction == true)
                {
                    try
                    {
                        asyncResultId = SalesforceCredentials.toOrgMS.deployRecentValidation(this.tbDeploymentValidationId.Text);
                        quickDeploySuccessful = true;
                    }
                    catch (Exception exc1)
                    {
                        quickDeploySuccessful = false;
                    }
                }
                else
                {
                    try
                    {
                        asyncResultId = SalesforceCredentials.toOrgMS.deployRecentValidation(this.tbDeploymentValidationId.Text);
                        quickDeploySuccessful = true;
                    }
                    catch (Exception exc2)
                    {
                        quickDeploySuccessful = false;
                    }
                }

                DeployResult result = waitForDeployCompletion(this.tbDeploymentValidationId.Text);
            }


            if(quickDeploySuccessful == false)
            {
                // If the username ends with .com, then use 'ms' for Production deployment
                // If it does not end in .com, then use ms2 for sandbox deployment
                // Set the deployment options as well for production (run all tests), or for the sandbox 

                String zipFileLocation = this.tbZipFileLocation.Text;

                DeployOptions dopt = new DeployOptions();
                dopt.ignoreWarnings = true;
                dopt.singlePackage = true;

                if (this.cbPurgeOnDelete.Checked == true)
                {
                    dopt.purgeOnDelete = true;
                }

                if (this.isProduction == true)
                {
                    dopt.rollbackOnError = true;
                    dopt.testLevel = TestLevel.RunLocalTests;
                }
                else
                {
                    dopt.allowMissingFiles = true;
                    dopt.autoUpdatePackage = true;
                    dopt.rollbackOnError = false;

                    if (this.cbRunTests.Checked)
                    {
                        dopt.testLevel = TestLevel.RunLocalTests;
                    }
                    else
                    {
                        dopt.testLevel = TestLevel.NoTestRun;
                    }
                }

                if (this.tbTestsToRun.Text != "")
                {
                    char[] splitChars = new char[2];
                    splitChars[0] = '\r';
                    splitChars[1] = '\n';

                    List<String> splitValues = new List<String>(); ;
                    foreach (String s in this.tbTestsToRun.Text.Split(splitChars))
                    {
                        if (s != "") splitValues.Add(s);
                    }

                    dopt.runTests = splitValues.ToArray();
                    dopt.testLevel = TestLevel.RunSpecifiedTests;
                }

                if (this.cbCheckOnly.Checked == true)
                {
                    dopt.checkOnly = true;
                }
                else
                {
                    dopt.checkOnly = false;
                }

                // Is the zip file base64?
                // Encode the folder selected as base64 binary data. This folder should include a package.xml file as well
                byte[] byteArray = File.ReadAllBytes(this.tbZipFileLocation.Text);

                AsyncResult ar = new AsyncResult();
                if (this.isProduction == true)
                {
                    ar = SalesforceCredentials.toOrgMS.deploy(byteArray, dopt);
                }
                else
                {
                    ar = SalesforceCredentials.toOrgMS.deploy(byteArray, dopt);
                }

                DeployResult result = waitForDeployCompletion(ar.id);
            }
        }

        //private DeployResult waitForDeployCompletion(AsyncResult asyncResult, Boolean isProduction)
        private DeployResult waitForDeployCompletion(String asyncResultId)
        {
            // Wait for the retrieve to complete
            int poll = 0;
            int waitTimeMilliSecs = this.ONE_SECOND;
            //String asyncResultId = asyncResult.id;
            DeployResult result = new DeployResult();

            result = SalesforceCredentials.toOrgMS.checkDeployStatus(asyncResultId, true);

            while (!result.done)
            {
                System.Threading.Thread.Sleep(waitTimeMilliSecs);
                waitTimeMilliSecs *= 2;

                if (poll++ > this.MAX_NUM_POLL_REQUESTS)
                {
                    MessageBox.Show("Request timed out.If this is a large set of metadata components, check that the time allowed by MAX_NUM_POLL_REQUESTS is sufficient.");
                }
                else
                {
                    result = SalesforceCredentials.toOrgMS.checkDeployStatus(asyncResultId, true);
                }
            }

            StringBuilder errorMessage = new StringBuilder();

            //result.details
            DeployDetails dd = result.details;
            DeployMessage[] deployMsgs = dd.componentFailures;

            Boolean errors = false;

            if (result.errorMessage != null)
            {
                errorMessage.Append(result.errorMessage);
                errors = true;
            }

            if (deployMsgs != null)
            {
                foreach (DeployMessage dm in deployMsgs)
                {
                    errorMessage.Append(dm.fileName + " - " + dm.lineNumber.ToString() + ":  " + dm.problem + Environment.NewLine + Environment.NewLine);
                }

                errors = true;
            }

            if (errors)
            {
                this.rtMessages.Text = errorMessage.ToString();
            }
            else
            {
                this.rtMessages.Text = "Async Deploy ID: " + asyncResultId + "\n\n" + "Successfully deployed package\n\n";
                this.rtMessages.Text = this.rtMessages.Text + "You can use the Async Deploy ID to run a Quick Deploy if this was a validation.";
            }

            return result;
        }

        private void cmbUserName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbUserName.Text == "")
            {
                this.tbPassword.Text = "";
                this.tbSecurityToken.Text = "";
            }
            else if (SalesforceCredentials.isProduction[this.cmbUserName.Text] == true)
            {
                this.lblSalesforce.Text = "Salesforce";
                this.Text = "Salesforce Metadata - Production";
                this.orgName = "Production";
                this.tbSecurityToken.Text = "";
                if (this.usernameToSecurityToken.ContainsKey(this.cmbUserName.Text))
                {
                    this.tbSecurityToken.Text = this.usernameToSecurityToken[cmbUserName.Text];
                }
            }
            else
            {
                this.lblSalesforce.Text = "Salesforce Sandbox";
                String[] userNamesplit = this.cmbUserName.Text.Split('.');
                this.orgName = userNamesplit[userNamesplit.Length - 1].ToUpper();
                this.Text = "Salesforce Metadata - " + this.orgName;
                this.tbSecurityToken.Text = "";
                if (this.usernameToSecurityToken.ContainsKey(this.cmbUserName.Text))
                {
                    this.tbSecurityToken.Text = this.usernameToSecurityToken[cmbUserName.Text];
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

            SalesforceCredentials.usernamePartnerUrl = new Dictionary<String, String>();
            SalesforceCredentials.usernameMetadataUrl = new Dictionary<String, String>();
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
                String metadataWdldUrl = "";
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

                    if (childNode.Name == "metadatawsdlurl")
                    {
                        metadataWdldUrl = childNode.InnerText;
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

                SalesforceCredentials.usernamePartnerUrl.Add(username, partnerWsdlUrl);
                SalesforceCredentials.usernameMetadataUrl.Add(username, metadataWdldUrl);
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
            foreach (String un in SalesforceCredentials.usernamePartnerUrl.Keys)
            {
                this.cmbUserName.Items.Add(un);
            }
        }

    }
}
