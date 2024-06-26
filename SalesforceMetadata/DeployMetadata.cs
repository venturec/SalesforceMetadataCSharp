﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SalesforceMetadata.MetadataWSDL;
using System.Xml;

namespace SalesforceMetadata
{
    public partial class DeployMetadata : System.Windows.Forms.Form
    {
        private SalesforceCredentials sc;
        private int ONE_SECOND = 20000;
        private int MAX_NUM_POLL_REQUESTS = 250;

        public DeployMetadata()
        {
            InitializeComponent();
            sc = new SalesforceCredentials();
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
            if (String.IsNullOrEmpty(this.cmbUserName.Text))
            {
                MessageBox.Show("Please enter your credentials before continuing");
                return;
            }

            Boolean quickDeploySuccessful = false;
            this.rtMessages.Text = "";

            try
            {
                sc.salesforceLogin(UtilityClass.REQUESTINGORG.TOORG, this.cmbUserName.Text);
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

            if (this.tbZipFileLocation.Text == null
                || this.tbZipFileLocation.Text == "")
            {
                MessageBox.Show("Please select the location where the ZIP file + package.xml is located first. Then click Deploy Metadata Package again.");
            }
            else if (this.cbCheckOnly.Checked == false
                && this.tbDeploymentValidationId.Text != "")
            {
                String asyncResultId = "";
                if (sc.isProduction[this.cmbUserName.Text] == true)
                {
                    try
                    {
                        asyncResultId = sc.toOrgMS.deployRecentValidation(this.tbDeploymentValidationId.Text);
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
                        asyncResultId = sc.toOrgMS.deployRecentValidation(this.tbDeploymentValidationId.Text);
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

                if (this.cbCheckOnly.Checked == true)
                {
                    dopt.checkOnly = true;
                }
                else
                {
                    dopt.checkOnly = false;
                }

                if (this.cbAutoUpdatePackage.Checked)
                {
                    dopt.autoUpdatePackage = true;
                }
                else
                {
                    dopt.autoUpdatePackage = false;
                }

                if (this.cbAllowMissingFiles.Checked)
                {
                    dopt.allowMissingFiles = true;
                }
                else
                {
                    dopt.allowMissingFiles = false;
                }

                if (this.cbRollbackOnError.Checked
                    || sc.isProduction[this.cmbUserName.Text] == true)
                {
                    dopt.rollbackOnError = true;
                }
                else
                {
                    dopt.rollbackOnError = false;
                }

                if (this.cbIgnoreWarnings.Checked)
                {
                    dopt.ignoreWarnings = true;
                }
                else
                {
                    dopt.ignoreWarnings = false;
                }

                if (this.cbSinglePackage.Checked)
                {
                    dopt.singlePackage = true;
                }
                else
                {
                    dopt.singlePackage = false;
                }

                if (this.cbPurgeOnDelete.Checked == true)
                {
                    dopt.purgeOnDelete = true;
                }
                else
                {
                    dopt.purgeOnDelete = false;
                }

                // Quick additional check for production deployment
                if (sc.isProduction[this.cmbUserName.Text] == true)
                {
                    dopt.allowMissingFiles = false;
                    dopt.autoUpdatePackage = false;
                    dopt.testLevel = TestLevel.RunLocalTests;
                }
                else
                {
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



                // Is the zip file base64?
                // Encode the folder selected as base64 binary data. This folder should include a package.xml file as well
                byte[] byteArray = File.ReadAllBytes(this.tbZipFileLocation.Text);

                AsyncResult ar = new AsyncResult();
                if (sc.isProduction[this.cmbUserName.Text] == true)
                {
                    ar = sc.toOrgMS.deploy(byteArray, dopt);
                }
                else
                {
                    ar = sc.toOrgMS.deploy(byteArray, dopt);
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

            result = sc.toOrgMS.checkDeployStatus(asyncResultId, true);

            while (!result.done)
            {
                System.Threading.Thread.Sleep(waitTimeMilliSecs);

                if (poll++ > this.MAX_NUM_POLL_REQUESTS)
                {
                    MessageBox.Show("Request timed out.If this is a large set of metadata components, check that the time allowed by MAX_NUM_POLL_REQUESTS is sufficient.");
                }
                else
                {
                    result = sc.toOrgMS.checkDeployStatus(asyncResultId, true);
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
            this.lblSalesforce.Text = "";
            this.Text = "Deploy Salesforce Metadata";

            if (sc.isProduction[this.cmbUserName.Text] == true)
            {
                this.lblSalesforce.Text = "Deploy to Salesforce Production";
                this.Text = "Deploy Salesforce Metadata - PRODUCTION";
            }
            else
            {
                this.lblSalesforce.Text = "Deploy to Salesforce Sandbox";
                String[] userNamesplit = this.cmbUserName.Text.Split('.');
                String orgName = userNamesplit[userNamesplit.Length - 1].ToUpper();
                this.Text = "Deploy Salesforce Metadata - " + orgName;
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
    }
}
