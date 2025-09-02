using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

using SalesforceMetadata.PartnerWSDL;
using SalesforceMetadata.MetadataWSDL;
using System.Diagnostics.Eventing.Reader;
using System.Text.RegularExpressions;
using System.Web.UI.MobileControls.Adapters;
using System.Security.Cryptography;
using System.Web.Services.Protocols;
using System.Web.UI.MobileControls;
//using SalesforceMetadata.ToolingWSDL;

namespace SalesforceMetadata
{
    public partial class SalesforceMetadataStep2 : System.Windows.Forms.Form
    {
        public SalesforceCredentials metaRetrieveSFCreds;

        private int ONE_SECOND = 20000;
        private int MAX_NUM_POLL_REQUESTS = 500;

        public String userName;

        // This has to be left as a Dictionary to allow for deploying a portion of the metadata members from the DevelopmentEnvironment
        // Key = metadata name, Values = members
        // Values can = * if allowed by the metadata api
        public Dictionary<String, List<String>> selectedItems;

        private HashSet<String> alreadyAdded;

        private String zipFile = "";
        private String extractToFolder = "";
        
        public SalesforceMetadataStep2()
        {
            InitializeComponent();
            metaRetrieveSFCreds = new SalesforceCredentials();
            alreadyAdded = new HashSet<String>();
        }

        private void btnRetrieveMetadata_Click(object sender, EventArgs e)
        {
            if (this.tbFromOrgSaveLocation.Text == "")
            {
                MessageBox.Show("Please select a directory to save the results to");
            }
            else if (this.tbExistingPackageXml.Text != "")
            {
                if (!this.tbExistingPackageXml.Text.EndsWith("package.xml"))
                {
                    MessageBox.Show("The file selected must be in XML format with the naming format of package.xml. Please select a file with the name package.xml");
                }
                else
                {
                    retrieveMetadataWithPackageXML();
                }
            }
            else if(this.selectedItems.Count > 0)
            {
                try
                {
                    metaRetrieveSFCreds.salesforceLogin(UtilityClass.REQUESTINGORG.FROMORG, userName);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                    return;
                }

                if (metaRetrieveSFCreds.loginSuccess == false)
                {
                    MessageBox.Show("Please check username, password and/or security token");
                    return;
                }

                this.rtMessages.Text = "";
                this.extractToFolder = "";
                String target_dir = this.tbFromOrgSaveLocation.Text;

                // Now build the target_dir and extractToFolder
                String[] urlParsed = metaRetrieveSFCreds.fromOrgLR.serverUrl.Split('/');
                urlParsed = urlParsed[2].Split('.');
                extractToFolder = urlParsed[0];

                if (extractToFolder.Contains("--"))
                {
                    extractToFolder = extractToFolder.Replace("--", "__");
                }
                else
                {
                    extractToFolder = extractToFolder + "__production";
                }

                target_dir = target_dir + '\\' + extractToFolder;

                if (!Directory.Exists(target_dir))
                {
                    DirectoryInfo di = Directory.CreateDirectory(target_dir);
                }
                else if (this.cbRebuildFolder.Checked == true)
                {
                    Directory.Delete(target_dir, true);
                    DirectoryInfo di = Directory.CreateDirectory(target_dir);
                }

                Action act = () => requestZipFile(UtilityClass.REQUESTINGORG.FROMORG, target_dir, this);
                System.Threading.Tasks.Task tsk = System.Threading.Tasks.Task.Run(act);
            }
        }

        private void btnRetrieveProfilesPermSets_Click(object sender, EventArgs e)
        {
            if (this.tbFromOrgSaveLocation.Text == "")
            {
                MessageBox.Show("Please select a directory to save the results to");
            }
            else if (this.selectedItems.Count > 0)
            {
                try
                {
                    metaRetrieveSFCreds.salesforceLogin(UtilityClass.REQUESTINGORG.FROMORG, userName);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                    return;
                }

                if (metaRetrieveSFCreds.loginSuccess == false)
                {
                    MessageBox.Show("Please check username, password and/or security token");
                    return;
                }

                this.rtMessages.Text = "";
                this.extractToFolder = "";
                String target_dir = this.tbFromOrgSaveLocation.Text;

                // Now build the target_dir and extractToFolder
                String[] urlParsed = metaRetrieveSFCreds.fromOrgLR.serverUrl.Split('/');
                urlParsed = urlParsed[2].Split('.');
                extractToFolder = urlParsed[0];

                if (extractToFolder.Contains("--"))
                {
                    extractToFolder = extractToFolder.Replace("--", "__");
                }
                else
                {
                    extractToFolder = extractToFolder + "__production";
                }

                target_dir = target_dir + '\\' + extractToFolder;

                if (!Directory.Exists(target_dir))
                {
                    DirectoryInfo di = Directory.CreateDirectory(target_dir);
                }
                else if (this.cbRebuildFolder.Checked == true)
                {
                    Directory.Delete(target_dir, true);
                    DirectoryInfo di = Directory.CreateDirectory(target_dir);
                }

                // Build the Package XML for retrieval
                // In order to retrieve the Profiles/Permission sets, we have to include the metadata objects related to them, otherwise it only returns the system values.
                if (this.selectedItems.ContainsKey("Profile"))
                { 
                    List<String> allProfileNames = new List<string>();
                    int waitTimeMilliSecs = 6000;

                    QueryResult qr = new QueryResult();
                    qr = metaRetrieveSFCreds.fromOrgSS.query("SELECT Id, Name FROM Profile ORDER BY Name");

                    Boolean done = false;
                    while (done == false)
                    {
                        if (qr.size > 0)
                        {
                            sObject[] sobjRecordsToProcess = qr.records;

                            foreach (sObject s in sobjRecordsToProcess)
                            {
                                if (s.Any == null)
                                {
                                    continue;
                                }
                                else if (s.Any[1].InnerText == "Contract Manager")
                                {
                                    allProfileNames.Add("ContractManager");
                                }
                                else if (s.Any[1].InnerText == "High Volume Customer Portal")
                                {
                                    allProfileNames.Add("HighVolumePortal");
                                }
                                else if (s.Any[1].InnerText == "Marketing User")
                                {
                                    allProfileNames.Add("MarketingProfile");
                                }
                                else if (s.Any[1].InnerText == "Read Only")
                                {
                                    allProfileNames.Add("ReadOnly");
                                }
                                else if (s.Any[1].InnerText == "Solution Manager")
                                {
                                    allProfileNames.Add("SolutionManager");
                                }
                                else if (s.Any[1].InnerText == "Standard Platform User")
                                {
                                    allProfileNames.Add("StandardAul");
                                }
                                else if (s.Any[1].InnerText == "Standard User")
                                {
                                    allProfileNames.Add("Standard");
                                }
                                else if (s.Any[1].InnerText == "System Administrator")
                                {
                                    allProfileNames.Add("Admin");
                                }
                                else
                                {
                                    allProfileNames.Add(replaceStringValue(s.Any[1].InnerText));
                                }
                            }
                        }

                        if (qr.done)
                        {
                            done = true;
                        }
                        else if (qr.size == 0)
                        {
                            done = true;
                        }
                        else
                        {
                            qr = metaRetrieveSFCreds.fromOrgSS.queryMore(qr.queryLocator);
                        }
                    }

                    done = false;

                    // Build the partial package XML 
                    List<String> partialProfileList = new List<String>();
                    StringBuilder partialPackageXMLXB = buildProfilePermissionSetPackageXml(UtilityClass.REQUESTINGORG.FROMORG, "Profile", partialProfileList, false);
                    String partialPackageXML = partialPackageXMLXB.ToString();

                    Int32 maxProfilesPerGroup = 10;
                    Int32 lastPosition = 0;
                    while (done == false)
                    {
                        partialProfileList.Clear();

                        Int32 profileListCounter = 0;
                        for (Int32 i = lastPosition; i < allProfileNames.Count; i++)
                        {
                            partialProfileList.Add(allProfileNames[i]);
                            profileListCounter++;

                            lastPosition = i + 1;
                            if (profileListCounter == maxProfilesPerGroup)
                            {
                                break;
                            }
                        }

                        // Build the RetrieveRequest and send this off to retrieve the profiles which could take a while
                        // so will need to add a wait on the retrieval process
                        // When the retrieval is complete, build the next set of retrieval requests
                        StringBuilder packageXmlSB = new StringBuilder();
                        packageXmlSB.Append(partialPackageXML);

                        foreach (String prof in partialProfileList)
                        {
                            packageXmlSB.Append("<members>" + prof + "</members>" + Environment.NewLine);
                        }

                        packageXmlSB.Append("<name>Profile</name>" + Environment.NewLine);
                        packageXmlSB.Append("</types>" + Environment.NewLine);
                        packageXmlSB.Append("<version>" + Properties.Settings.Default.DefaultAPI + "</version>" + Environment.NewLine);
                        packageXmlSB.Append("</Package>");

                        RetrieveRequest retrieveRequest = new RetrieveRequest();
                        retrieveRequest.apiVersion = Convert.ToDouble(Properties.Settings.Default.DefaultAPI);
                        retrieveRequest.unpackaged = parsePackageManifest(packageXmlSB.ToString());

                        Action act = () => retrieveZipFile(UtilityClass.REQUESTINGORG.FROMORG, target_dir, retrieveRequest, "Profile", this);
                        System.Threading.Tasks.Task tsk = System.Threading.Tasks.Task.Run(act);

                        while (tsk.IsCanceled == false && tsk.IsCompleted == false && tsk.IsFaulted == false)
                        {
                            tsk.Wait(waitTimeMilliSecs);
                        }

                        if (lastPosition == allProfileNames.Count)
                        {
                            done = true;
                        }

                        packageXmlSB.Clear();
                    }
                }

                // Build the Package XML for retrieval
                // In order to retrieve the Profiles/Permission sets, we have to include the metadata objects related to them, otherwise it only returns the system values.
                if (this.selectedItems.ContainsKey("PermissionSet"))
                {
                    int waitTimeMilliSecs = 6000;

                    // Get all Perm Set names into an array (list)
                    List<String> allPermSetNames = new List<string>();

                    QueryResult qr = new QueryResult();
                    qr = metaRetrieveSFCreds.fromOrgSS.query("SELECT Id, NamespacePrefix, Name, Type FROM PermissionSet ORDER BY Name");

                    Boolean done = false;
                    while (done == false)
                    {
                        if (qr.size > 0)
                        {
                            sObject[] sobjRecordsToProcess = qr.records;

                            foreach (sObject s in sobjRecordsToProcess)
                            {
                                if (s.Any == null)
                                {
                                    continue;
                                }
                                else
                                {
                                    if (s.Any[1].InnerText == "")
                                    {
                                        allPermSetNames.Add(replaceStringValue(s.Any[2].InnerText));
                                    }
                                    else
                                    {
                                        String managedPermSet = "";
                                        managedPermSet = (s.Any[1].InnerText + "__" + replaceStringValue(s.Any[2].InnerText));
                                        allPermSetNames.Add(managedPermSet);
                                    }
                                }
                            }
                        }

                        if (qr.done)
                        {
                            done = true;
                        }
                        else if (qr.size == 0)
                        {
                            done = true;
                        }
                        else
                        {
                            qr = metaRetrieveSFCreds.fromOrgSS.queryMore(qr.queryLocator);
                        }
                    }

                    done = false;

                    // Build the partial Perm Set list from the allPermSetNames list to retrieve only the 
                    // a subset of perm sets and prevent timeouts
                    List<String> partialPermSetList = new List<String>();
                    StringBuilder partialPackageXMLSB = buildProfilePermissionSetPackageXml(UtilityClass.REQUESTINGORG.FROMORG, "PermissionSet", partialPermSetList, false);
                    String partialPackageXML = partialPackageXMLSB.ToString();

                    // Why 1000? May need review
                    Int32 maxPermSetsPerGroup = 1000;
                    Int32 lastPosition = 0;
                    while (done == false)
                    {
                        partialPermSetList.Clear();
                        Int32 permSetListCounter = 0;

                        for (Int32 i = lastPosition; i < allPermSetNames.Count; i++)
                        {
                            partialPermSetList.Add(allPermSetNames[i]);
                            permSetListCounter++;

                            lastPosition = i + 1;
                            if (permSetListCounter > allPermSetNames.Count)
                            {
                                break;
                            }
                            else if (permSetListCounter == maxPermSetsPerGroup)
                            {
                                break;
                            }
                        }

                        // Build the RetrieveRequest and send this off to retrieve the profiles which could take a while
                        // so will need to add a wait on the retrieval process
                        // When the retrieval is complete, build the next set of retrieval requests
                        StringBuilder packageXmlSB = new StringBuilder();
                        packageXmlSB.Append(partialPackageXML);

                        foreach (String ps in partialPermSetList)
                        {
                            packageXmlSB.Append("<members>" + ps + "</members>" + Environment.NewLine);
                        }

                        packageXmlSB.Append("<name>PermissionSet</name>" + Environment.NewLine);
                        packageXmlSB.Append("</types>" + Environment.NewLine);
                        packageXmlSB.Append("<version>" + Properties.Settings.Default.DefaultAPI + "</version>" + Environment.NewLine);
                        packageXmlSB.Append("</Package>");

                        RetrieveRequest retrieveRequest = new RetrieveRequest();
                        retrieveRequest.apiVersion = Convert.ToDouble(Properties.Settings.Default.DefaultAPI);
                        retrieveRequest.unpackaged = parsePackageManifest(packageXmlSB.ToString());

                        Action act = () => retrieveZipFile(UtilityClass.REQUESTINGORG.FROMORG, target_dir, retrieveRequest, "PermissionSet", this);
                        System.Threading.Tasks.Task tsk = System.Threading.Tasks.Task.Run(act);

                        while (tsk.IsCanceled == false && tsk.IsCompleted == false && tsk.IsFaulted == false)
                        {
                            tsk.Wait(waitTimeMilliSecs);
                        }

                        if (lastPosition == allPermSetNames.Count)
                        {
                            done = true;
                        }

                        packageXmlSB.Clear();
                    }
                }
            }
        }

        private void retrieveMetadataWithPackageXML()
        {
            this.rtMessages.Text = "";
            this.extractToFolder = "";
            String target_dir = this.tbFromOrgSaveLocation.Text;

            // TODO: Add try/catch to this?
            metaRetrieveSFCreds.salesforceLogin(UtilityClass.REQUESTINGORG.FROMORG, userName);

            String[] urlParsed = metaRetrieveSFCreds.fromOrgLR.serverUrl.Split('/');
            urlParsed = urlParsed[2].Split('.');
            extractToFolder = urlParsed[0];

            if (extractToFolder.Contains("--"))
            {
                extractToFolder = extractToFolder.Replace("--", "__");
            }
            else
            {
                extractToFolder = extractToFolder + "__production";
            }

            target_dir = target_dir + '\\' + extractToFolder;

            if (!Directory.Exists(target_dir))
            {
                DirectoryInfo di = Directory.CreateDirectory(target_dir);
            }
            else if (this.cbRebuildFolder.Checked == true)
            {
                Directory.Delete(target_dir, true);
                DirectoryInfo di = Directory.CreateDirectory(target_dir);
            }

            // TODO: Break the RetrieveRequest down into blocks or chunks if the package.xml contains Profiles and/or Permission Sets
            // Then split out the retrieval requests by the number of threads available

            RetrieveRequest retrieveRequest = new RetrieveRequest();
            retrieveRequest.apiVersion = Convert.ToDouble(Properties.Settings.Default.DefaultAPI);
            retrieveRequest.unpackaged = parsePackageManifest(File.ReadAllText(this.tbFromOrgSaveLocation.Text + "\\package.xml"));

            Action act = () => retrieveZipFile(UtilityClass.REQUESTINGORG.FROMORG, target_dir, retrieveRequest, "Existing Package XML", this);
            System.Threading.Tasks.Task tsk = System.Threading.Tasks.Task.Run(act);
        }

        public void requestZipFile(UtilityClass.REQUESTINGORG reqOrg, String target_dir, SalesforceMetadataStep2 sfMdFrm)
        {
            String processingMsg1 = "Metadata Components to Retrieve:" + Environment.NewLine;
            var threadParameters1 = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg1, sfMdFrm); });
            var thread1 = new System.Threading.Thread(threadParameters1);
            thread1.Start();
            while (thread1.ThreadState == System.Threading.ThreadState.Running)
            { 
                // do nothing. Just want for the thread to complete
            }

            String componentsToRetrieve = "";
            foreach (String comp in selectedItems.Keys)
            {
                componentsToRetrieve = componentsToRetrieve + "    " + comp + Environment.NewLine;
            }

            String processingMsg2 = componentsToRetrieve + Environment.NewLine;
            var threadParameters2 = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg2, sfMdFrm); });
            var thread2 = new System.Threading.Thread(threadParameters2);
            thread2.Start();
            while (thread2.ThreadState == System.Threading.ThreadState.Running)
            {
                // do nothing. Just wait for the thread to complete
            }

            //if (sfMdFrm.selectedItems.ContainsKey("FlexiPage"))
            //{

            //}

            //if (sfMdFrm.selectedItems.ContainsKey("Layout"))
            //{

            //}

            // Build the Package XML for retrieval
            // In order to retrieve the Profiles/Permission sets, we have to include the metadata objects related to them, otherwise it only returns the system values.
            if (sfMdFrm.selectedItems.ContainsKey("Profile"))
            {
                List<String> allProfileNames = new List<string>();

                try
                {
                    metaRetrieveSFCreds.salesforceLogin(UtilityClass.REQUESTINGORG.FROMORG, userName);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                    return;
                }

                QueryResult qr = new QueryResult();
                qr = metaRetrieveSFCreds.fromOrgSS.query("SELECT Id, Name FROM Profile ORDER BY Name");

                Boolean done = false;
                while (done == false)
                {
                    if (qr.size > 0)
                    {
                        sObject[] sobjRecordsToProcess = qr.records;

                        foreach (sObject s in sobjRecordsToProcess)
                        {
                            if (s.Any == null)
                            {
                                continue;
                            }
                            else if (s.Any[1].InnerText == "Contract Manager")
                            {
                                allProfileNames.Add("ContractManager");
                            }
                            else if (s.Any[1].InnerText == "High Volume Customer Portal")
                            {
                                allProfileNames.Add("HighVolumePortal");
                            }
                            else if (s.Any[1].InnerText == "Marketing User")
                            {
                                allProfileNames.Add("MarketingProfile");
                            }
                            else if (s.Any[1].InnerText == "Read Only")
                            {
                                allProfileNames.Add("ReadOnly");
                            }
                            else if (s.Any[1].InnerText == "Solution Manager")
                            {
                                allProfileNames.Add("SolutionManager");
                            }
                            else if (s.Any[1].InnerText == "Standard Platform User")
                            {
                                allProfileNames.Add("StandardAul");
                            }
                            else if (s.Any[1].InnerText == "Standard User")
                            {
                                allProfileNames.Add("Standard");
                            }
                            else if (s.Any[1].InnerText == "System Administrator")
                            {
                                allProfileNames.Add("Admin");
                            }
                            else
                            {
                               allProfileNames.Add(replaceStringValue(s.Any[1].InnerText));
                            }
                        }
                    }

                    if (qr.done)
                    {
                        done = true;
                    }
                    else if (qr.size == 0)
                    {
                        done = true;
                    }
                    else
                    {
                        qr = metaRetrieveSFCreds.fromOrgSS.queryMore(qr.queryLocator);
                    }
                }

                StringBuilder packageXmlSB = new StringBuilder();
                packageXmlSB = buildProfilePermissionSetPackageXml(UtilityClass.REQUESTINGORG.FROMORG, "Profile", allProfileNames, true);

                File.WriteAllText(this.tbFromOrgSaveLocation.Text + "\\package.xml", packageXmlSB.ToString());

                MessageBox.Show("The Profile Package XML has been built. Please adjust the Package XML to include only those you are interested in and then set the Existing Package Xml field in the Metadata Retrieval Step for processing");
            }

            // Build the Package XML for retrieval
            // In order to retrieve the Profiles/Permission sets, we have to include the metadata objects related to them, otherwise it only returns the system values.
            if (sfMdFrm.selectedItems.ContainsKey("PermissionSet"))
            {
                List<String> allPermSetNames = new List<string>();

                try
                {
                    metaRetrieveSFCreds.salesforceLogin(UtilityClass.REQUESTINGORG.FROMORG, userName);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                    return;
                }

                QueryResult qr = new QueryResult();
                qr = metaRetrieveSFCreds.fromOrgSS.query("SELECT Id, NamespacePrefix, Name, Type FROM PermissionSet ORDER BY Name");

                Boolean done = false;
                while (done == false)
                {
                    if (qr.size > 0)
                    {
                        sObject[] sobjRecordsToProcess = qr.records;

                        foreach (sObject s in sobjRecordsToProcess)
                        {
                            if (s.Any == null)
                            {
                                continue;
                            }
                            else
                            {
                                if (s.Any[1].InnerText == "")
                                {
                                    allPermSetNames.Add(replaceStringValue(s.Any[2].InnerText));
                                }
                                else
                                {
                                    String managedPermSet = "";
                                    managedPermSet = (s.Any[1].InnerText + "__" + replaceStringValue(s.Any[2].InnerText));
                                    allPermSetNames.Add(managedPermSet);
                                }
                            }
                        }
                    }

                    if (qr.done)
                    {
                        done = true;
                    }
                    else if (qr.size == 0)
                    {
                        done = true;
                    }
                    else
                    {
                        qr = metaRetrieveSFCreds.fromOrgSS.queryMore(qr.queryLocator);
                    }
                }

                StringBuilder packageXmlSB = new StringBuilder();
                packageXmlSB = buildProfilePermissionSetPackageXml(UtilityClass.REQUESTINGORG.FROMORG, "PermissionSet", allPermSetNames, true);

                File.WriteAllText(this.tbFromOrgSaveLocation.Text + "\\package.xml", packageXmlSB.ToString());

                MessageBox.Show("The Permission Set Package XML has been built. Please adjust the Package XML to include only those you are interested in and then set the Existing Package Xml field in the Metadata Retrieval Step for processing");
            }
            
            if (sfMdFrm.selectedItems.ContainsKey("EmailTemplate"))
            {
                // Build the package.xml to retrieve both the Profile and Permission Set
                List<String> emailMembers = new List<string>();

                QueryResult qr = new QueryResult();
                qr = metaRetrieveSFCreds.fromOrgSS.query("SELECT Folder.DeveloperName, DeveloperName FROM EmailTemplate");

                Boolean done = false;
                while (!done)
                {
                    if (qr.size > 0)
                    {
                        sObject[] sobjRecordsToProcess = qr.records;

                        foreach (sObject s in sobjRecordsToProcess)
                        {
                            if (s.Any == null)
                            {
                                continue;
                            }

                            String emailFileName = "";

                            if (s.Any[0].InnerText == "")
                            {
                                emailFileName = "unfiled$public";
                            }
                            else
                            {
                                emailFileName = s.Any[0].InnerText;
                            }

                            emailFileName = emailFileName + "/" + s.Any[1].InnerText;
                            emailMembers.Add(emailFileName);
                        }
                    }

                    if (qr.done)
                    {
                        done = true;
                    }
                    else if (qr.size == 0)
                    {
                        done = true;
                    }
                    else
                    {
                        qr = metaRetrieveSFCreds.fromOrgSS.queryMore(qr.queryLocator);
                    }
                }

                StringBuilder packageXmlSB = new StringBuilder();
                packageXmlSB = buildEmailTemplatePacageXml(emailMembers);

                RetrieveRequest retrieveRequest = new RetrieveRequest();
                retrieveRequest.apiVersion = Convert.ToDouble(Properties.Settings.Default.DefaultAPI);
                retrieveRequest.unpackaged = parsePackageManifest(packageXmlSB.ToString());

                int waitTimeMilliSecs = 6000;
                Action act = () => retrieveZipFile(UtilityClass.REQUESTINGORG.FROMORG, target_dir, retrieveRequest, "EmailTemplate", sfMdFrm);
                System.Threading.Tasks.Task tsk = System.Threading.Tasks.Task.Run(act);

                while (tsk.IsCanceled == false && tsk.IsCompleted == false && tsk.IsFaulted == false)
                {
                    tsk.Wait(waitTimeMilliSecs);
                }

                alreadyAdded.Add("EmailTemplate");
            }

            if (sfMdFrm.selectedItems.ContainsKey("Report"))
            {
                Dictionary<String, String> reportFolders = new Dictionary<string, string>();
                List<String> allReportMembers = new List<string>();
                List<String> selectedReportMembers = new List<string>();

                QueryResult rf = new QueryResult();
                rf = metaRetrieveSFCreds.fromOrgSS.query("SELECT Id, DeveloperName FROM Folder");

                Boolean done = false;
                while (!done)
                {
                    if (rf.size > 0)
                    {
                        sObject[] sobjRecordsToProcess = rf.records;

                        foreach (sObject s in sobjRecordsToProcess)
                        {
                            reportFolders.Add(s.Any[0].InnerText, s.Any[1].InnerText);
                        }
                    }

                    if (rf.done)
                    {
                        done = true;
                    }
                    else if (rf.size == 0)
                    {
                        done = true;
                    }
                    else
                    {
                        rf = metaRetrieveSFCreds.fromOrgSS.queryMore(rf.queryLocator);
                    }
                }

                QueryResult qr = new QueryResult();
                qr = metaRetrieveSFCreds.fromOrgSS.query("SELECT OwnerId, DeveloperName FROM Report");

                done = false;
                while (!done)
                {
                    if (qr.size > 0)
                    {
                        sObject[] sobjRecordsToProcess = qr.records;

                        foreach (sObject s in sobjRecordsToProcess)
                        {
                            if (s.Any == null)
                            {
                                continue;
                            }

                            if (reportFolders.ContainsKey(s.Any[0].InnerText))
                            {
                                String reportFileName = reportFolders[s.Any[0].InnerText] + "/" + s.Any[1].InnerText;
                                allReportMembers.Add(reportFileName);
                            }
                        }
                    }

                    if (qr.done)
                    {
                        done = true;
                    }
                    else if (qr.size == 0)
                    {
                        done = true;
                    }
                    else
                    {
                        qr = metaRetrieveSFCreds.fromOrgSS.queryMore(qr.queryLocator);
                    }
                }

                //Debug.WriteLine("");
                List<Task> taskArray = new List<Task>();

                // Now build the package xml requests with only a specific number of members. The max is 10,000 reports
                Boolean retrieveComplete = false;
                while (retrieveComplete == false)
                {
                    Boolean allThreadsComplete = false;

                    // We can only have 10,000 reports requested in the package.xml file
                    // Setting the default size to 5000 reports for now.
                    foreach (String rms in allReportMembers)
                    {
                        selectedReportMembers.Add(rms);

                        if (selectedReportMembers.Count == 5000)
                        {
                            break;
                        }
                    }

                    StringBuilder packageXmlSB = new StringBuilder();
                    packageXmlSB = buildReportPackageXml(selectedReportMembers);

                    RetrieveRequest retrieveRequest = new RetrieveRequest();
                    retrieveRequest.apiVersion = Convert.ToDouble(Properties.Settings.Default.DefaultAPI);
                    retrieveRequest.unpackaged = parsePackageManifest(packageXmlSB.ToString());

                    //int waitTimeMilliSecs = 6000;
                    Action act = () => retrieveZipFile(UtilityClass.REQUESTINGORG.FROMORG, target_dir, retrieveRequest, "Report", sfMdFrm);
                    System.Threading.Tasks.Task tsk = System.Threading.Tasks.Task.Run(act);
                    taskArray.Add(tsk);

                    while (allThreadsComplete == false)
                    {
                        Int32 completedThreads = 0;
                        foreach (Task tskObj in taskArray)
                        {
                            if (tskObj.IsCanceled == true || tskObj.IsCompleted == true || tskObj.IsFaulted == true)
                            {
                                completedThreads++;
                            }
                        }

                        if (completedThreads == taskArray.Count)
                        {
                            allThreadsComplete = true;
                        }
                    }

                    taskArray.Clear();

                    foreach (String selReport in selectedReportMembers)
                    {
                        if (allReportMembers.Contains(selReport))
                        {
                            allReportMembers.Remove(selReport);
                        }
                    }

                    selectedReportMembers.Clear();

                    if (allReportMembers.Count == 0)
                    {
                        retrieveComplete = true;
                    }
                }

                alreadyAdded.Add("Report");
            }


            // Retrieve the remaining if any are left
            List<String> selectedItemsList2 = new List<string>();
            List<String> completedItemsList = new List<String>();
            foreach (String comp in selectedItems.Keys)
            {
                if (!alreadyAdded.Contains(comp))
                {
                    selectedItemsList2.Add(comp);
                }
            }

            // Parallel For processing of components to see if it is faster
            Int32 threadCount = Properties.Settings.Default.MetadataAynchrounsThreads;
            if (threadCount == 0)
            {
                threadCount = 1;
            }

            if(selectedItemsList2.Count > 0)
            {
                Boolean retrieveComplete = false;

                List<Task> taskArray = new List<Task>();

                while (retrieveComplete == false)
                {
                    Boolean allThreadsComplete = false;

                    // Debug.WriteLine("");
                    // Build the threads and then as the threads complete, build the next one

                    for (Int32 i = 0; i <= threadCount - 1; i++)
                    {
                        if (i <= selectedItemsList2.Count - 1)
                        {
                            // Cannot pass the metadata object in by reference, i.e. selectedItemsList2[i]
                            // When the loop continues, the reference to selectedItemsList2[i] changes as the iterator increases throwing off the value for the other actions
                            String metadataObject = selectedItemsList2[i];

                            HashSet<String> mObjSet = new HashSet<string> { selectedItemsList2[i] };
                            StringBuilder packageXmlSB = new StringBuilder();
                            packageXmlSB = buildPackageXml(UtilityClass.REQUESTINGORG.FROMORG, mObjSet);

                            RetrieveRequest retrieveRequest = new RetrieveRequest();
                            retrieveRequest.apiVersion = Convert.ToDouble(Properties.Settings.Default.DefaultAPI);
                            retrieveRequest.unpackaged = parsePackageManifest(packageXmlSB.ToString());

                            Action act = () => retrieveZipFile(UtilityClass.REQUESTINGORG.FROMORG, target_dir, retrieveRequest, metadataObject, sfMdFrm);
                            System.Threading.Tasks.Task tsk = System.Threading.Tasks.Task.Run(act);

                            taskArray.Add(tsk);
                            completedItemsList.Add(selectedItemsList2[i]);
                        }
                    }

                    // Loop through the existing threads to determine if all are complete.
                    // Once all threads are complete, go back to the for loop
                    while (allThreadsComplete == false)
                    {
                        Int32 completedThreads = 0;

                        foreach (Task tsk in taskArray)
                        {
                            if (tsk.IsCanceled == true || tsk.IsCompleted == true || tsk.IsFaulted == true)
                            {
                                completedThreads++;
                            }
                        }

                        if (completedThreads == taskArray.Count)
                        {
                            allThreadsComplete = true;
                        }
                    }

                    taskArray.Clear();

                    // Remove the items from the selectedItemsList2 based on what is in the completedItemsList
                    foreach (String ci in completedItemsList)
                    {
                        selectedItemsList2.Remove(ci);
                    }

                    if (selectedItemsList2.Count == 0)
                    {
                        retrieveComplete = true;
                    }
                }
            }

            String processingMsg3 = "Metadata Components Retrieval Complete:" + Environment.NewLine;
            var threadParameters3 = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg3, sfMdFrm); });
            var thread3 = new System.Threading.Thread(threadParameters3);
            thread3.Start();
            while (thread3.ThreadState == System.Threading.ThreadState.Running)
            {
                // do nothing. Just want for the thread to complete
            }
        }

        // Add metadata types
        private StringBuilder buildEmailTemplatePacageXml(List<String> emailMembers)
        {
            StringBuilder packageXmlSB = new StringBuilder();
            packageXmlSB.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine);
            packageXmlSB.Append("<Package xmlns = \"http://soap.sforce.com/2006/04/metadata\">" + Environment.NewLine);

            packageXmlSB.Append("<types>" + Environment.NewLine);

            foreach (String emailFileName in emailMembers)
            {
                packageXmlSB.Append("<members>" + emailFileName + "</members>" + Environment.NewLine);
            }

            packageXmlSB.Append("<name>EmailTemplate</name>" + Environment.NewLine);
            packageXmlSB.Append("</types>" + Environment.NewLine);

            packageXmlSB.Append("<version>" + Properties.Settings.Default.DefaultAPI + "</version>" + Environment.NewLine);
            packageXmlSB.Append("</Package>");

            return packageXmlSB;
        }

        private StringBuilder buildProfilePermissionSetPackageXml(UtilityClass.REQUESTINGORG reqOrg, String metadataType, List<String> profPermSetMembers, Boolean addClosingTags)
        {
            StringBuilder packageXmlSB = new StringBuilder();
            packageXmlSB.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine);
            packageXmlSB.Append("<Package xmlns = \"http://soap.sforce.com/2006/04/metadata\">" + Environment.NewLine);

            MetadataService ms = metaRetrieveSFCreds.getMetadataService(reqOrg);
            ms.AllowAutoRedirect = true;

            List<String> members = new List<String>();
            members.Add("*");

            getMetadataTypes("ApexClass", packageXmlSB, members.ToArray());
            getMetadataTypes("ApexComponent", packageXmlSB, members.ToArray());
            getMetadataTypes("ApexPage", packageXmlSB, members.ToArray());
            getMetadataTypes("ApexTrigger", packageXmlSB, members.ToArray());
            getMetadataTypes("ConnectedApp", packageXmlSB, members.ToArray());
            getMetadataTypes("CustomApplication", packageXmlSB, members.ToArray());
            getMetadataTypes("CustomApplicationComponent", packageXmlSB, members.ToArray());
            getMetadataTypes("CustomMetadata", packageXmlSB, members.ToArray());
            getMetadataTypes("CustomPermissions", packageXmlSB, members.ToArray());
            getMetadataTypes("ExternalDataSource", packageXmlSB, members.ToArray());
            getMetadataTypes("Flow", packageXmlSB, members.ToArray());
            getMetadataTypes("FlowDefinition", packageXmlSB, members.ToArray());
            getMetadataTypes("Layout", packageXmlSB, members.ToArray());
            getMetadataTypes("NamedCredential", packageXmlSB, members.ToArray());
            getMetadataTypes("ServicePresenceStatus", packageXmlSB, members.ToArray());

            members.Clear();
            members.AddRange(getTabDescribe(reqOrg));
            getMetadataTypes("CustomTab", packageXmlSB, members.ToArray());

            // Clear the * to allow for accessing all Sobjects in the org
            members.Clear();
            members = getSObjectMembers(reqOrg);
            getMetadataTypes("CustomObject", packageXmlSB, members.ToArray());

            // Reset the default members to the flag for all members and add the additional types selected.
            members.Clear();

            packageXmlSB.Append("<types>" + Environment.NewLine);

            if (addClosingTags == true)
            {
                foreach (String prof in profPermSetMembers)
                {
                    packageXmlSB.Append("<members>" + prof + "</members>" + Environment.NewLine);
                }

                packageXmlSB.Append("<name>" + metadataType + "</name>" + Environment.NewLine);
                packageXmlSB.Append("</types>" + Environment.NewLine);
                packageXmlSB.Append("<version>" + Properties.Settings.Default.DefaultAPI + "</version>" + Environment.NewLine);
                packageXmlSB.Append("</Package>");
            }

            alreadyAdded.Add("ApexClass");
            alreadyAdded.Add("ApexComponent");
            alreadyAdded.Add("ApexPage");
            alreadyAdded.Add("ApexTrigger");
            alreadyAdded.Add("ConnectedApp");
            alreadyAdded.Add("CustomApplication");
            alreadyAdded.Add("CustomApplicationComponent");
            alreadyAdded.Add("CustomMetadata");
            alreadyAdded.Add("CustomPermissions");
            alreadyAdded.Add("ExternalDataSource");
            alreadyAdded.Add("Flow");
            alreadyAdded.Add("FlowDefinition");
            alreadyAdded.Add("Layout");
            alreadyAdded.Add("NamedCredential");
            alreadyAdded.Add("ServicePresenceStatus");
            alreadyAdded.Add("CustomTab");
            alreadyAdded.Add("CustomObject");

            return packageXmlSB;
        }

        private StringBuilder buildReportPackageXml(List<String> reportMembers)
        {
            StringBuilder packageXmlSB = new StringBuilder();
            packageXmlSB.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine);
            packageXmlSB.Append("<Package xmlns = \"http://soap.sforce.com/2006/04/metadata\">" + Environment.NewLine);

            packageXmlSB.Append("<types>" + Environment.NewLine);

            foreach (String rm in reportMembers)
            {
                packageXmlSB.Append("<members>" + rm + "</members>" + Environment.NewLine);
            }

            packageXmlSB.Append("<name>Report</name>" + Environment.NewLine);
            packageXmlSB.Append("</types>" + Environment.NewLine);

            packageXmlSB.Append("<version>" + Properties.Settings.Default.DefaultAPI + "</version>" + Environment.NewLine);
            packageXmlSB.Append("</Package>");

            return packageXmlSB;
        }

        // TODO:
        // Document - This metadata type doesn’t support the wildcard character * in the package.xml manifest file
        // Letterhead - This metadata type doesn’t support the wildcard character * in the package.xml manifest file
        private StringBuilder buildPackageXml(UtilityClass.REQUESTINGORG reqOrg, HashSet<String> selectedItems)
        {
            StringBuilder packageXmlSB = new StringBuilder();
            packageXmlSB.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine);
            packageXmlSB.Append("<Package xmlns=\"http://soap.sforce.com/2006/04/metadata\">" + Environment.NewLine);

            MetadataService ms = metaRetrieveSFCreds.getMetadataService(reqOrg);
            ms.AllowAutoRedirect = true;

            foreach (String selected in selectedItems)
            {
                if (selected == "CustomObject" && !alreadyAdded.Contains(selected))
                {
                    List<String> members = new List<String>();
                    members.AddRange(getSObjectMembers(reqOrg));
                    getMetadataTypes("CustomObject", packageXmlSB, members.ToArray());
                    alreadyAdded.Add(selected);
                }
                else if (selected == "CustomTab" && !alreadyAdded.Contains(selected))
                {
                    List<String> members = new List<String>();
                    members.AddRange(getTabDescribe(reqOrg));
                    getMetadataTypes("CustomTab", packageXmlSB, members.ToArray());
                    alreadyAdded.Add(selected);
                }
                //else if (selected == "Layout" && !alreadyAdded.Contains(selected))
                //{
                //    List<String> members = new List<String>();
                //    members.AddRange(getLayoutDescribe(reqOrg));
                //    getMetadataTypes("Layout", packageXmlSB, members.ToArray());
                //    alreadyAdded.Add(selected);
                //}
                else if (selected == "StandardValueSet" && !alreadyAdded.Contains(selected))
                {
                    packageXmlSB.Append("<types>" + Environment.NewLine);
                    packageXmlSB.Append("<members>AccountContactMultiRoles</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>AccountContactRole</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>AccountOwnership</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>AccountRating</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>AccountType</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>AssetStatus</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>CampaignMemberStatus</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>CampaignStatus</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>CampaignType</members>" + Environment.NewLine);
                    //packageXmlSB.Append("<members>CareItemStatus</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>CaseContactRole</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>CaseOrigin</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>CasePriority</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>CaseReason</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>CaseStatus</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>CaseType</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>ContactRole</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>ContractContactRole</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>ContractStatus</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>EntitlementType</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>EventSubject</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>EventType</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>FiscalYearPeriodName</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>FiscalYearPeriodPrefix</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>FiscalYearQuarterName</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>FiscalYearQuarterPrefix</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>FulfillmentStatus</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>FulfillmentType</members>" + Environment.NewLine);
                    //packageXmlSB.Append("<members>IdeaCategory</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>IdeaMultiCategory</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>IdeaStatus</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>IdeaThemeStatus</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>Industry</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>LeadSource</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>LeadStatus</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>OpportunityCompetitor</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>OpportunityStage</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>OpportunityType</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>OrderItemSummaryChgRsn</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>OrderStatus</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>OrderSummaryRoutingSchdRsn</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>OrderSummaryStatus</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>OrderType</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>PartnerRole</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>Product2Family</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>ProcessExceptionCategory</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>ProcessExceptionPriority</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>ProcessExceptionSeverity</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>ProcessExceptionStatus</members>" + Environment.NewLine);
                    //packageXmlSB.Append("<members>QuestionOrigin</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>QuickTextCategory</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>QuickTextChannel</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>QuoteStatus</members>" + Environment.NewLine);
                    //packageXmlSB.Append("<members>RoleInTerritory</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>ResourceAbsenceType</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>ReturnOrderLineItemProcessPlan</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>ReturnOrderLineItemReasonForRejection</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>ReturnOrderLineItemReasonForReturn</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>ReturnOrderLineItemRepaymentMethod</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>ReturnOrderShipmentType</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>ReturnOrderStatus</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>SalesTeamRole</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>Salutation</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>ServiceAppointmentStatus</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>ServiceContractApprovalStatus</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>ServTerrMemRoleType</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>SocialPostClassification</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>SocialPostEngagementLevel</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>SocialPostReviewedStatus</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>SolutionStatus</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>TaskPriority</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>TaskStatus</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>TaskSubject</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>TaskType</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>WorkOrderLineItemStatus</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>WorkOrderPriority</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>WorkOrderStatus</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>WorkTypeDefApptType</members>" + Environment.NewLine);
                    packageXmlSB.Append("<members>WorkTypeGroupAddInfo</members>" + Environment.NewLine);

                    packageXmlSB.Append("<name>StandardValueSet</name>" + Environment.NewLine);
                    packageXmlSB.Append("</types>" + Environment.NewLine);

                    alreadyAdded.Add(selected);
                }
                else if (!alreadyAdded.Contains(selected))
                {
                    SalesforceMetadata.MetadataWSDL.FileProperties[] fpList = metaRetrieveSFCreds.listMetadata(selected, reqOrg);

                    List <String> members = new List<String>();
                    if (fpList[0] == null)
                    {
                        members.Add("*");
                    }
                    // Bypass the managed package objects which cannot be retrieved or modified
                    else if (selected == "ApexClass"
                        || selected == "ApexComponent"
                        || selected == "ApexPage"
                        || selected == "ApexTrigger"
                        || selected == "AuraDefinitionBundle"
                        || selected == "Flow"
                        || selected == "LightningComponentBundle"
                        || selected == "Workflow")
                    {
                        foreach (SalesforceMetadata.MetadataWSDL.FileProperties fp in fpList)
                        {
                            if (fp.namespacePrefix == null)
                            {
                                members.Add(fp.fullName);
                            }
                        }
                    }
                    else
                    {
                        foreach (SalesforceMetadata.MetadataWSDL.FileProperties fp in fpList)
                        {
                            members.Add(fp.fullName);
                        }
                    }
                    
                    getMetadataTypes(selected, packageXmlSB, members.ToArray());
                    alreadyAdded.Add(selected);
                }
            }

            packageXmlSB.Append("<version>" + Properties.Settings.Default.DefaultAPI + "</version>" + Environment.NewLine);
            packageXmlSB.Append("</Package>");

            return packageXmlSB;
        }

        private void retrieveZipFile(UtilityClass.REQUESTINGORG reqOrg, String target_dir, RetrieveRequest retrieveRequest, String metdataObject, SalesforceMetadataStep2 sfMdFrm)
        {
            DateTime dt = DateTime.Now;
            String processingMsg1 = "    " + metdataObject + ": Metadata Retrieval Started at: " + dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString() + " " + dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString() + "." + dt.Millisecond.ToString() + Environment.NewLine;
            var threadParameters1 = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg1, sfMdFrm); });
            var thread1 = new System.Threading.Thread(threadParameters1);
            thread1.Start();
            while (thread1.ThreadState == System.Threading.ThreadState.Running)
            {
                // do nothing. Just want for the thread to complete
            }

            AsyncResult asyncResult = new AsyncResult();

            MetadataService ms = metaRetrieveSFCreds.getMetadataService(reqOrg);
            ms.AllowAutoRedirect = true;

            if (ms != null)
            {
                asyncResult = ms.retrieve(retrieveRequest);
            }

            RetrieveResult result = waitForRetrieveCompletion(ms, asyncResult, reqOrg, sfMdFrm);

            // Do not remove underscores. The zip file's path is dependent on this.
            String timestamp = dt.Year.ToString() + "_" + dt.Month.ToString() + "_" + dt.Day.ToString() + "_" +
                               dt.Hour.ToString() + "_" + dt.Minute.ToString() + "_" + dt.Second.ToString() + "_" + dt.Millisecond.ToString();

            if (result.zipFile != null)
            {
                zipFile = sfMdFrm.tbFromOrgSaveLocation.Text + "\\components_" + extractToFolder + "_" + metdataObject + "_" + timestamp + ".zip";

                try
                {
                    File.WriteAllBytes(@zipFile, result.zipFile);

                    // Unzip the package and store it to the folder specified
                    ZipArchive archive = ZipFile.Open(zipFile, ZipArchiveMode.Read);
                    foreach (ZipArchiveEntry file in archive.Entries)
                    {
                        String completeFileName = Path.GetFullPath(Path.Combine(target_dir, file.FullName));
                        String directoryPath = Path.GetDirectoryName(completeFileName);

                        // Confirm if directory path exists and if not create the directory
                        if (!Directory.Exists(directoryPath))
                        {
                            // Assuming Empty for Directory
                            Directory.CreateDirectory(directoryPath);
                        }

                        file.ExtractToFile(completeFileName, true);
                    }

                    archive.Dispose();

                    // If cbConvertToVSCodeStyle == true then add the -meta.xml to the end of the file for each file in the directories, except for LWC and Aura
                    // Objects will need to be reworked as well as their folder structure is different
                    if (sfMdFrm.cbMDAPISourceStyle.Checked == true)
                    {
                        addVSCodeFileExtension(target_dir);
                    }

                    DateTime dt2 = DateTime.Now;
                    String processingMsg2 = "    " + metdataObject + ": Metadata Retrieval Completed at: " + dt2.Year.ToString() + "-" + dt2.Month.ToString() + "-" + dt2.Day.ToString() + " " + dt2.Hour.ToString() + ":" + dt2.Minute.ToString() + ":" + dt2.Second.ToString() + "." + dt2.Millisecond.ToString() + Environment.NewLine + Environment.NewLine;
                    var threadParameters2 = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg2, sfMdFrm); });
                    var thread2 = new System.Threading.Thread(threadParameters2);
                    thread2.Start();
                    while (thread2.ThreadState == System.Threading.ThreadState.Running)
                    {
                        // do nothing. Just want for the thread to complete
                    }
                }
                catch (System.IO.IOException ioExc)
                {
                    String processingMsg2 = "    " + metdataObject + ": IO Exception: " + ioExc.Message + Environment.NewLine + Environment.NewLine;
                    var threadParameters2 = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg2, sfMdFrm); });
                    var thread2 = new System.Threading.Thread(threadParameters2);
                    thread2.Start();
                    while (thread2.ThreadState == System.Threading.ThreadState.Running)
                    {
                        // do nothing. Just want for the thread to complete
                    }
                }
                catch (Exception exc)
                {
                    String processingMsg2 = "    " + metdataObject + ": There was an error saving the package.zip file to the location specified: " + exc.Message + Environment.NewLine;
                    var threadParameters2 = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg2, sfMdFrm); });
                    var thread2 = new System.Threading.Thread(threadParameters2);
                    thread2.Start();
                    while (thread2.ThreadState == System.Threading.ThreadState.Running)
                    {
                        // do nothing. Just want for the thread to complete
                    }
                }
            }
        }

        private void getMetadataTypes(String metadataObjectName, StringBuilder packageXmlSB, String[] members)
        {
            packageXmlSB.Append("<types>" + Environment.NewLine);

            foreach (String s in members)
            {
                packageXmlSB.Append("<members>");
                packageXmlSB.Append(s);
                packageXmlSB.Append("</members>" + Environment.NewLine);
            }

            packageXmlSB.Append("<name>" + metadataObjectName + "</name>" + Environment.NewLine);
            packageXmlSB.Append("</types>" + Environment.NewLine);
        }

        private Package parsePackageManifest(String packageXmlContents)
        {
            Package packageManifest = null;
            List<PackageTypeMembers> listPackageTypes = new List<PackageTypeMembers>();  // convert this to an array in the package

            XmlDocument sfPackage = new XmlDocument();
            sfPackage.LoadXml(packageXmlContents);

            XmlNodeList documentNodes = sfPackage.GetElementsByTagName("types");

            for (int i = 0; i < documentNodes.Count; i++)
            {
                if (documentNodes[i].LocalName == "types")
                {
                    String typeName = "";
                    List<String> membersList = new List<String>();

                    foreach (XmlNode childNode in documentNodes[i].ChildNodes)
                    {
                        if (childNode.Name == "members")
                        {
                            membersList.Add(childNode.InnerText);
                        }

                        if (childNode.Name == "name")
                        {
                            typeName = childNode.InnerText;
                        }
                    }

                    PackageTypeMembers packageTypes = new PackageTypeMembers();
                    packageTypes.name = typeName;
                    packageTypes.members = membersList.ToArray();
                    listPackageTypes.Add(packageTypes);
                }
            }

            packageManifest = new Package();
            PackageTypeMembers[] packageTypesArray = listPackageTypes.ToArray();
            packageManifest.types = packageTypesArray;
            packageManifest.version = Properties.Settings.Default.DefaultAPI;

            return packageManifest;
        }

        private RetrieveResult waitForRetrieveCompletion(MetadataService ms, AsyncResult asyncResult, UtilityClass.REQUESTINGORG reqOrg, SalesforceMetadataStep2 sfMdFrm)
        {
            // Wait for the retrieve to complete
            int poll = 0;
            int waitTimeMilliSecs = this.ONE_SECOND;
            String asyncResultId = asyncResult.id;
            RetrieveResult result = new RetrieveResult();

            result = ms.checkRetrieveStatus(asyncResultId, true);

            RetrieveStatus rsOld = result.status;
            RetrieveStatus rsNew = result.status;

            while (!result.done)
            {
                System.Threading.Thread.Sleep(waitTimeMilliSecs);
                
                if (asyncResult.statusCodeSpecified == true)
                {
                    result.done = true;

                    String processingMsg1 = "    " + asyncResult.statusCode.ToString() + Environment.NewLine + Environment.NewLine;
                    var threadParameters1 = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg1, sfMdFrm); });
                    var thread1 = new System.Threading.Thread(threadParameters1);
                    thread1.Start();
                    while (thread1.ThreadState == System.Threading.ThreadState.Running)
                    {
                        // do nothing. Just want for the thread to complete
                    }
                }
                else if (poll++ > this.MAX_NUM_POLL_REQUESTS)
                {
                    result.status = RetrieveStatus.Failed;
                    result.done = true;

                    String processingMsg2 = "    Request timed out. If this is a large set of metadata components, check that the time allowed by MAX_NUM_POLL_REQUESTS is sufficient." + Environment.NewLine + Environment.NewLine;
                    var threadParameters2 = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg2, sfMdFrm); });
                    var thread2 = new System.Threading.Thread(threadParameters2);
                    thread2.Start();
                    while (thread2.ThreadState == System.Threading.ThreadState.Running)
                    {
                        // do nothing. Just want for the thread to complete
                    }
                }
                else
                {
                    result = ms.checkRetrieveStatus(asyncResultId, true);
                    rsNew = result.status;

                    if (result.status == RetrieveStatus.Succeeded)
                    {
                        result.done = true;
                    }
                    else if (rsOld == RetrieveStatus.Pending
                        && rsNew == RetrieveStatus.InProgress)
                    {
                        rsOld = result.status;

                        poll = 0;
                        waitTimeMilliSecs = this.ONE_SECOND;
                        //Debug.WriteLine(result.status.ToString() + ": " + waitTimeMilliSecs.ToString());
                    }
                    else if (result.status == RetrieveStatus.Pending
                        || result.status == RetrieveStatus.InProgress)
                    {
                        //Debug.WriteLine(result.status.ToString() + ": " + waitTimeMilliSecs.ToString());
                    }
                    else if (result.status == RetrieveStatus.Failed)
                    {
                        result.done = true;

                        String processingMsg2 = "    " + result.errorMessage + Environment.NewLine + Environment.NewLine;
                        var threadParameters2 = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg2, sfMdFrm); });
                        var thread2 = new System.Threading.Thread(threadParameters2);
                        thread2.Start();
                        while (thread2.ThreadState == System.Threading.ThreadState.Running)
                        {
                            // do nothing. Just want for the thread to complete
                        }
                    }
                }
            }

            return result;
        }

        //private List<String> getLayoutDescribe(UtilityClass.REQUESTINGORG reqOrg)
        //{
        //    DescribeLayout[] descrLayouts = metaRetrieveSFCreds.get(reqOrg);

        //    List<String> members = new List<String>();
        //    for (Int32 i = 0; i < descrTabs.Length; i++)
        //    {
        //        members.Add(descrTabs[i].name);
        //    }

        //    return members;
        //}

        private List<String> getTabDescribe(UtilityClass.REQUESTINGORG reqOrg)
        {
            DescribeTab[] descrTabs = metaRetrieveSFCreds.getDescribeTab(reqOrg);

            List<String> members = new List<String>();
            for (Int32 i = 0; i < descrTabs.Length; i++)
            {
                members.Add(descrTabs[i].name);
            }

            return members;
        }


        private List<String> getSObjectMembers(UtilityClass.REQUESTINGORG reqOrg)
        {
            List<String> members = new List<string>();

            // We need to add these manually since the object itself is not part of the DescribeGlobalResult,
            // but you won't be able to retrieve the custom fields related to Events and Tasks if not included
            members.Add("Activity");

            DescribeGlobalResult dgr = metaRetrieveSFCreds.getDescribeGlobalResult(reqOrg);
            for (Int32 i = 0; i < dgr.sobjects.Length; i++)
            {
                String sobj = dgr.sobjects[i].name;
                if (!sobj.EndsWith("__Tag")
                    && !sobj.EndsWith("__History")
                    && !sobj.EndsWith("__Feed")
                    && !sobj.EndsWith("__ChangeEvent")
                    && !sobj.EndsWith("__Share"))
                {
                    members.Add(sobj);
                }
            }

            return members;
        }


        //private SalesforceMetadata.MetadataWSDL.FileProperties[] listMetadataMembers(String metadataType, UtilityClass.REQUESTINGORG reqOrg)
        //{
        //    SalesforceMetadata.MetadataWSDL.FileProperties[] fp = metaRetrieveSFCreds.listMetadata(metadataType, reqOrg);

        //    return fp;
        //}

        // VS Code style extensions
        public void addVSCodeFileExtension(String targetDirectory)
        {
            List<String> filePathsInDirectory = new List<string>();

            HashSet<String> folderSkips = new HashSet<string>();
            folderSkips.Add("aura");
            folderSkips.Add("classes");
            folderSkips.Add("lwc");
            folderSkips.Add("pages");
            folderSkips.Add("reports");
            folderSkips.Add("triggers");

            // TODO: Come back and refactor these
            // However, you will then have to go into each of the sub-directories to determine the differences
            // At this point, hold off on these updates
            //folderSkips.Add("objects");
            //folderSkips.Add("objectTranslations");

            List<String> subdirectorySearchCompleted = new List<String>();

            List<String> subDirectoryList = new List<String>();
            subDirectoryList.Add(targetDirectory);
            subDirectoryList.AddRange(getSubdirectories(targetDirectory));

            Boolean subdirectoriesExist = false;
            if (subDirectoryList.Count > 0)
            {
                subdirectoriesExist = true;
            }

            while (subdirectoriesExist == true)
            {
                if (subDirectoryList.Count == 0) subdirectoriesExist = false;

                for (Int32 i = 0; i < subDirectoryList.Count; i++)
                {
                    String[] subdirSplit = subDirectoryList[i].Split('\\');

                    if (!folderSkips.Contains(subdirSplit[subdirSplit.Length - 1]))
                    {
                        if (subdirSplit[subdirSplit.Length - 1] == "objects")
                        {
                            // Create a new folder with the object name -> Batch__c
                            // Parse the XML file to separate out the base object XML tags
                            // Save the XML file to the folder -> Batch__c.object-meta.xml

                            // Sub-Folders
                            //      businessProcesses
                            //      compactLayouts
                            //      fields
                            //      fieldSets
                            //      listViews
                            //      recordTypes
                            //      validationRules
                            //      webLinks

                            String[] files = Directory.GetFiles(subDirectoryList[i]);
                            if (files.Length > 0)
                            {
                                for (Int32 j = 0; j < files.Length; j++)
                                {
                                    parseObjectFiles(files[j]);
                                }
                            }
                        }
                        else if (subdirSplit[subdirSplit.Length - 1] == "objectTranslations")
                        {
                            // Create a new folder with the object name -> Batch__c
                            // Parse the XML file to separate out the base object XML tags
                            // Save the XML file to the folder -> Batch__c.object-meta.xml

                            // Sub-Folders
                            //      businessProcesses
                            //      compactLayouts
                            //      fields
                            //      fieldSets
                            //      listViews
                            //      recordTypes
                            //      validationRules
                            //      webLinks

                            String[] files = Directory.GetFiles(subDirectoryList[i]);
                            if (files.Length > 0)
                            {
                                for (Int32 j = 0; j < files.Length; j++)
                                {
                                    //Debug.WriteLine(" ");
                                    //parseObjectTranslationFiles(files[j]);
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                // Get all files in the current directory
                                String[] files = Directory.GetFiles(subDirectoryList[i]);
                                if (files.Length > 0)
                                {
                                    for (Int32 j = 0; j < files.Length; j++)
                                    {
                                        FileInfo fi = new FileInfo(files[j]);
                                        fi.CopyTo(files[j] + "-meta.xml", true);
                                        fi.Delete();
                                    }
                                }
                            }
                            catch (Exception exc)
                            {

                            }
                        }

                        subdirectorySearchCompleted.Add(subDirectoryList[i]);
                    }
                }

                // Check if there are any additional sub directories in the current directory and add them to the list
                List<String> subDirectories = new List<String>();
                for (Int32 j = 0; j < subDirectoryList.Count; j++)
                {
                    String[] subdirectorySplit = subDirectoryList[j].Split('\\');

                    String folderName = subdirectorySplit[subdirectorySplit.Length - 1];

                    if (folderSkips.Contains(folderName))
                    {
                        continue;
                    }
                    else if (folderName == "objects" || folderName == "objectTranslations")
                    {
                        continue;
                    }

                    if (subDirectoryList[j] != targetDirectory)
                    {
                        List<String> sds = getSubdirectories(subDirectoryList[j]);
                        if (sds.Count > 0)
                        {
                            foreach (String s in sds)
                            {
                                if (!subdirectorySearchCompleted.Contains(s))
                                {
                                    subDirectories.Add(s);
                                }
                            }
                        }
                    }
                }

                // Remove the current directories in subDirectoriesList before adding the additional subdirectories
                // so the tool does not review them again
                subDirectoryList.Clear();

                if (subDirectories.Count > 0)
                {
                    foreach (String s in subDirectories)
                    {
                        if (!subDirectoryList.Contains(s))
                        {
                            subDirectoryList.Add(s);
                        }
                    }

                    subDirectories.Clear();
                }

            }
        }

        // VS Code style extensions
        private List<String> getSubdirectories(String folderLocation)
        {
            // Check for additional subdirectories in the current subdirectory list and add them to the list
            List<String> subDirectoryList = new List<String>();
            String[] subDirectories = new String[0];
            try
            {
                subDirectories = Directory.GetDirectories(folderLocation);
                foreach (String sub in subDirectories)
                {
                    subDirectoryList.Add(sub);
                }
            }
            catch (Exception e)
            {

            }

            return subDirectoryList;
        }

        // VS Code style extensions
        private void parseObjectFiles(String objectPath)
        {
            String[] objectPathSplit = objectPath.Split('\\');
            String[] objDirectoryName = objectPathSplit[objectPathSplit.Length - 1].Split('.');

            String objDirectoryPath = "";

            for (Int32 i = 0; i < objectPathSplit.Length - 1; i++)
            {
                objDirectoryPath = objDirectoryPath + objectPathSplit[i] + "\\";
            }

            objDirectoryPath = objDirectoryPath + objDirectoryName[0];

            DirectoryInfo objDir = Directory.CreateDirectory(objDirectoryPath);

            // Sub-Folders
            //      businessProcesses
            //      compactLayouts
            //      fields
            //      fieldSets
            //      listViews
            //      recordTypes
            //      validationRules
            //      webLinks

            XmlDocument xd = new XmlDocument();
            xd.Load(objectPath);

            XmlNodeList bpNodeList = xd.GetElementsByTagName("businessProcesses");
            XmlNodeList clNodeList = xd.GetElementsByTagName("compactLayouts");
            XmlNodeList fldNodeList = xd.GetElementsByTagName("fields");
            XmlNodeList fldsetNodeList = xd.GetElementsByTagName("fieldSets");
            XmlNodeList lvNodeList = xd.GetElementsByTagName("listViews");
            XmlNodeList rtNodeList = xd.GetElementsByTagName("recordTypes");
            XmlNodeList vrNodeList = xd.GetElementsByTagName("validationRules");
            XmlNodeList wlNodeList = xd.GetElementsByTagName("webLinks");

            if (bpNodeList.Count > 0)
            {
                DirectoryInfo dirInfo = Directory.CreateDirectory(objDir.FullName + "\\businessProcesses");

                writeChildNodes(dirInfo,
                                ".businessProcess-meta.xml",
                                bpNodeList,
                                "businessProcesses",
                                "BusinessProcess");
            }

            if (clNodeList.Count > 0)
            {
                DirectoryInfo dirInfo = Directory.CreateDirectory(objDir.FullName + "\\compactLayouts");

                writeChildNodes(dirInfo,
                                ".compactLayout-meta.xml",
                                clNodeList,
                                "compactLayouts",
                                "CompactLayout");
            }

            if (fldNodeList.Count > 0)
            {
                DirectoryInfo dirInfo = Directory.CreateDirectory(objDir.FullName + "\\fields");

                writeChildNodes(dirInfo,
                                ".field-meta.xml",
                                fldNodeList,
                                "fields",
                                "CustomField");
            }

            if (fldsetNodeList.Count > 0)
            {
                DirectoryInfo dirInfo = Directory.CreateDirectory(objDir.FullName + "\\fieldSets");

                writeChildNodes(dirInfo,
                                ".fieldSet-meta.xml",
                                fldsetNodeList,
                                "fieldSets",
                                "FieldSet");
            }

            if (lvNodeList.Count > 0)
            {
                DirectoryInfo dirInfo = Directory.CreateDirectory(objDir.FullName + "\\listViews");

                writeChildNodes(dirInfo,
                                ".listView-meta.xml",
                                lvNodeList,
                                "listViews",
                                "ListView");
            }

            if (rtNodeList.Count > 0)
            {
                DirectoryInfo dirInfo = Directory.CreateDirectory(objDir.FullName + "\\recordTypes");

                writeChildNodes(dirInfo,
                                ".recordType-meta.xml",
                                rtNodeList,
                                "recordTypes",
                                "RecordType");
            }

            if (vrNodeList.Count > 0)
            {
                DirectoryInfo dirInfo = Directory.CreateDirectory(objDir.FullName + "\\validationRules");

                writeChildNodes(dirInfo,
                                ".validationRule-meta.xml",
                                vrNodeList,
                                "validationRules",
                                "ValidationRule");
            }

            if (wlNodeList.Count > 0)
            {
                DirectoryInfo dirInfo = Directory.CreateDirectory(objDir.FullName + "\\webLinks");

                writeChildNodes(dirInfo,
                                ".webLink-meta.xml",
                                wlNodeList,
                                "webLinks",
                                "WebLink");
            }

            File.Move(objectPath, objDirectoryPath + "\\" + objectPathSplit[objectPathSplit.Length - 1] + "-meta.xml");
        }

        // VS Code style extensions
        private void writeChildNodes(DirectoryInfo dirInfo, 
                                     String fileExtension, 
                                     XmlNodeList nodeList,
                                     String parentTagToReplace, 
                                     String newParentTag)
        {
            foreach (XmlNode xn in nodeList)
            {
                if (xn.ParentNode.Name == "CustomObject")
                {
                    XmlDocument xdDocument = new XmlDocument();
                    xdDocument.LoadXml(xn.OuterXml);

                    XmlNodeList nameNode = xdDocument.GetElementsByTagName("fullName");

                    StreamWriter sw = new StreamWriter(dirInfo.FullName + "\\" + nameNode[0].InnerText + fileExtension);
                    sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");

                    //String xmlStructure = "";

                    foreach (XmlNode nd1 in xdDocument.ChildNodes)
                    {
                        if (nd1.Name == parentTagToReplace)
                        {
                            sw.Write("<" + newParentTag + " xmlns=\"http://soap.sforce.com/2006/04/metadata\">");
                        }

                        if (nd1.ChildNodes.Count > 0
                            && nd1.ChildNodes[0].LocalName == "#text")
                        {
                            //sw.Write(nd1.ChildNodes[0].InnerText);
                        }
                        else if (nd1.ChildNodes.Count > 0)
                        {
                            foreach (XmlNode nd2 in nd1.ChildNodes)
                            {
                                if (nd2.ChildNodes.Count > 0
                                    && nd2.ChildNodes[0].LocalName == "#text")
                                {
                                    sw.Write(Environment.NewLine);
                                    sw.Write("    <" + nd2.LocalName + ">" + nd2.ChildNodes[0].InnerText + "</" + nd2.LocalName + ">");
                                }
                                else if (nd2.ChildNodes.Count > 0)
                                {
                                    sw.Write(Environment.NewLine);
                                    sw.Write("    <" + nd2.LocalName + ">");

                                    foreach (XmlNode nd3 in nd2.ChildNodes)
                                    {
                                        if (nd3.ChildNodes.Count > 0
                                            && nd3.ChildNodes[0].LocalName == "#text")
                                        {
                                            sw.Write(Environment.NewLine);
                                            sw.Write("        <" + nd3.LocalName + ">" + nd3.ChildNodes[0].InnerText + "</" + nd3.LocalName + ">");
                                        }
                                        else if (nd3.ChildNodes.Count > 0)
                                        {
                                            sw.Write(Environment.NewLine);
                                            sw.Write("        <" + nd3.LocalName + ">");

                                            foreach (XmlNode nd4 in nd3.ChildNodes)
                                            {
                                                if (nd4.ChildNodes.Count > 0
                                                    && nd4.ChildNodes[0].LocalName == "#text")
                                                {
                                                    sw.Write(Environment.NewLine);
                                                    sw.Write("            <" + nd4.LocalName + ">" + nd4.ChildNodes[0].InnerText + "</" + nd4.LocalName + ">");
                                                }
                                                else if (nd4.ChildNodes.Count > 0)
                                                {
                                                    sw.Write(Environment.NewLine);
                                                    sw.Write("            <" + nd4.LocalName + ">");

                                                    foreach (XmlNode nd5 in nd4.ChildNodes)
                                                    {
                                                        if (nd5.ChildNodes.Count > 0
                                                            && nd5.ChildNodes[0].LocalName == "#text")
                                                        {
                                                            sw.Write(Environment.NewLine);
                                                            sw.Write("                <" + nd5.LocalName + ">" + nd5.ChildNodes[0].InnerText + "</" + nd5.LocalName + ">");
                                                        }
                                                        else if (nd5.ChildNodes.Count > 0)
                                                        {
                                                            foreach (XmlNode nd6 in nd5.ChildNodes)
                                                            {
                                                                if (nd6.ChildNodes.Count > 0
                                                                    && nd6.ChildNodes[0].LocalName == "#text")
                                                                {
                                                                    //Debug.WriteLine(" ");
                                                                }
                                                                else if (nd6.ChildNodes.Count > 0)
                                                                {
                                                                    //Debug.WriteLine(" ");
                                                                }
                                                            }
                                                        }
                                                    }

                                                    sw.Write(Environment.NewLine);
                                                    sw.Write("            </" + nd4.LocalName + ">");
                                                }
                                            }

                                            sw.Write(Environment.NewLine);
                                            sw.Write("        </" + nd3.LocalName + ">");
                                        }
                                    }

                                    sw.Write(Environment.NewLine);
                                    sw.Write("    </" + nd2.LocalName + ">");
                                }
                            }

                            //sw.Write(Environment.NewLine + "    </" + nd1.LocalName + ">");
                        }

                        sw.Write(Environment.NewLine + "</" + newParentTag + ">");
                    }

                    //sw.Write(Environment.NewLine + "</" + newParentTag + ">");

                    sw.Close();
                }
            }
        }

        private FileProperties[] getFolderItems(List<ListMetadataQuery> mdqFolderList, MetadataService ms)
        {
            FileProperties[] sfFolderItems = ms.listMetadata(mdqFolderList.ToArray(), Convert.ToDouble(Properties.Settings.Default.DefaultAPI));
            return sfFolderItems;
        }

        private void tbPackageXMLLocation_DoubleClick(object sender, EventArgs e)
        {
            String selectedPath = UtilityClass.folderBrowserSelectPath("Select Directory to Save the Metadata Results to",
                                                                       true,
                                                                       FolderEnum.SaveTo,
                                                                       Properties.Settings.Default.MetadataLastSaveToLocation);

            if (selectedPath != "")
            {
                this.tbFromOrgSaveLocation.Text = selectedPath;
                Properties.Settings.Default.MetadataLastSaveToLocation = selectedPath;
                Properties.Settings.Default.Save();
                this.btnRetrieveMetadata.Enabled = true;
            }
        }

        private void tbExistingPackageXml_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "xml files (package.xml)|package.xml|All Files (*.*)|*.*";
            ofd.Title = "Please select a package.xml file";

            DialogResult dr = ofd.ShowDialog();

            if(dr == DialogResult.OK) this.tbExistingPackageXml.Text = ofd.FileName;
        }

        private void tbExistingPackageXml_TextChanged(object sender, EventArgs e)
        {
            if (this.tbExistingPackageXml.Text == "")
            {
                this.btnRetrieveMetadata.Enabled = true;
            }
        }

        // Threadsafe way to write back to the form's textbox
        public void tsWriteToTextbox(String tbValue, SalesforceMetadataStep2 sfMdFrm)
        {
            if (sfMdFrm.rtMessages.InvokeRequired)
            {
                Action safeWrite = delegate { tsWriteToTextbox($"{tbValue}", sfMdFrm); };
                sfMdFrm.rtMessages.Invoke(safeWrite);
            }
            else
            {
                sfMdFrm.rtMessages.Text = sfMdFrm.rtMessages.Text + tbValue;
            }
        }

        public String replaceStringValue(String strValue)
        {
            strValue = strValue.Replace("(", "%28");
            strValue = strValue.Replace(")", "%29");
            strValue = strValue.Replace("&", "%26");

            return strValue;
        }

        private void btnGeneratePackageXML_Click(object sender, EventArgs e)
        {
            metaRetrieveSFCreds.salesforceLogin(UtilityClass.REQUESTINGORG.FROMORG, userName);

            HashSet<String> mObjSet = new HashSet<string>();
            foreach (String metaObj in this.selectedItems.Keys)
            {
                mObjSet.Add(metaObj);
            }

            StringBuilder packageXmlSB = new StringBuilder();
            packageXmlSB = buildPackageXml(UtilityClass.REQUESTINGORG.FROMORG, mObjSet);

            File.WriteAllText(this.tbFromOrgSaveLocation.Text + "\\package.xml", packageXmlSB.ToString());

            MessageBox.Show("package.xml file created at: " + this.tbFromOrgSaveLocation.Text + "\\package.xml",
                            "Package.xml Created",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }
    }
}