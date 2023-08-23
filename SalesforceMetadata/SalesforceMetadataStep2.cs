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

namespace SalesforceMetadata
{
    public partial class SalesforceMetadataStep2 : System.Windows.Forms.Form
    {
        private int ONE_SECOND = 1000;
        private int MAX_NUM_POLL_REQUESTS = 50;

        public List<String> selectedItems;

        private HashSet<String> alreadyAdded;

        private String packageXMLFile = "";
        private String zipFile = "";
        private String extractToFolder = "";
        
        public SalesforceMetadataStep2()
        {
            InitializeComponent();
            alreadyAdded = new HashSet<String>();
        }


        private void btnRetrieveMetadataFromSelected_Click(object sender, EventArgs e)
        {
            this.rtMessages.Text = "";
            this.extractToFolder = "";
            UtilityClass.REQUESTINGORG reqOrg = UtilityClass.REQUESTINGORG.FROMORG;
            requestZipFile(reqOrg);
        }

        private void btnToOrgRetrieveMetadata_Click(object sender, EventArgs e)
        {
            this.rtMessages.Text = "";
            this.extractToFolder = "";
            UtilityClass.REQUESTINGORG reqOrg = UtilityClass.REQUESTINGORG.TOORG;
            requestZipFile(reqOrg);
        }


        private Package setUnpackaged(String packageXmlFile)
        {
            Package manifestPkg = null;

            if (File.Exists(@packageXmlFile))
            {
                manifestPkg = parsePackageManifest(@packageXmlFile);
            }

            return manifestPkg;
        }


        private void requestZipFile(UtilityClass.REQUESTINGORG reqOrg)
        {
            Boolean loginSuccess = SalesforceCredentials.salesforceLogin(reqOrg);

            if (loginSuccess == false)
            {
                MessageBox.Show("Please check username, password and/or security token");
                return;
            }

            alreadyAdded.Clear();

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                String[] urlParsed = SalesforceCredentials.fromOrgLR.serverUrl.Split('/');
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

                // Knock off the HTML
            }
            else if (reqOrg == UtilityClass.REQUESTINGORG.TOORG)
            {
                String[] urlParsed = SalesforceCredentials.toOrgLR.serverUrl.Split('/');
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
            }

            // After selecting a Metadata type to get, build the package.xml file
            // If it is a Profile or Permission set, the Object will also be required
            MetadataService ms = null;

            StringBuilder packageXmlSB = new StringBuilder();
            packageXmlSB.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine);
            packageXmlSB.Append("<Package xmlns = \"http://soap.sforce.com/2006/04/metadata\">" + Environment.NewLine);

            foreach (String selected in selectedItems)
            {
                if (selected == "CustomObject" && !alreadyAdded.Contains(selected))
                {
                    List<String> members = new List<String>();
                    members.AddRange(getSObjectMembers(reqOrg));
                    getMetadataTypes("CustomObject", packageXmlSB, members.ToArray());
                }
                else if (selected == "CustomTab")
                {
                    List<String> members = new List<String>();
                    if (!alreadyAdded.Contains("CustomTab"))
                    {
                        members.AddRange(getTabDescribe(reqOrg));
                        getMetadataTypes("CustomTab", packageXmlSB, members.ToArray());
                    }
                }
                else if (selected == "EmailTemplate" && !alreadyAdded.Contains(selected))
                {
                    packageXmlSB.Append("<types>" + Environment.NewLine);

                    // List all the folders using EmailFolder 
                    // and then run another listmetadata() for each folder name and include type = 'EmailTemplate'.
                    List<ListMetadataQuery> mdqFolderList = new List<ListMetadataQuery>();
                    ListMetadataQuery mdqFolderQuery = new ListMetadataQuery();
                    mdqFolderQuery.type = "EmailFolder";
                    mdqFolderList.Add(mdqFolderQuery);

                    List<FileProperties> sfEmailFolders = new List<FileProperties>();
                    List<FileProperties> sfEmailFiles = new List<FileProperties>();

                    ms = SalesforceCredentials.getMetadataService(reqOrg);
                    FileProperties[] sfFolders = ms.listMetadata(mdqFolderList.ToArray(), Convert.ToDouble(Properties.Settings.Default.DefaultAPI));

                    foreach (FileProperties fp in sfFolders)
                    {
                        sfEmailFolders.Add(fp);
                    }

                    QueryResult qr = new QueryResult();
                    qr = SalesforceCredentials.fromOrgSS.query("SELECT Folder.DeveloperName, DeveloperName FROM EmailTemplate");

                    sObject[] sobjRecordsToProcess = qr.records;

                    foreach (sObject s in sobjRecordsToProcess)
                    {
                        if (s.Any == null) break;

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

                        packageXmlSB.Append("<members>" + emailFileName + "</members>" + Environment.NewLine);
                    }

                    packageXmlSB.Append("<name>EmailTemplate</name>" + Environment.NewLine);
                    packageXmlSB.Append("</types>" + Environment.NewLine);

                    alreadyAdded.Add(selected);
                }
                else if ((selected == "PermissionSet"
                            || selected == "Profile")
                            && !alreadyAdded.Contains(selected))
                {
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
                    if (!alreadyAdded.Contains("CustomTab"))
                    {
                        members.AddRange(getTabDescribe(reqOrg));
                        getMetadataTypes("CustomTab", packageXmlSB, members.ToArray());
                    }

                    // Clear the * to allow for accessing all Sobjects in the org
                    members.Clear();
                    if (!alreadyAdded.Contains("CustomObject"))
                    {
                        members = getSObjectMembers(reqOrg);
                        getMetadataTypes("CustomObject", packageXmlSB, members.ToArray());
                    }

                    // Reset the default members to the flag for all members and add the additional types selected.
                    members.Clear();
                    members.Add("*");
                    getMetadataTypes(selected, packageXmlSB, members.ToArray());

                    alreadyAdded.Add(selected);
                }
                else if (selected == "Report")
                {
                    packageXmlSB.Append("<types>" + Environment.NewLine);

                    List<ListMetadataQuery> mdqFolderList = new List<ListMetadataQuery>();
                    ListMetadataQuery mdqFolderQuery = new ListMetadataQuery();
                    mdqFolderQuery.type = "ReportFolder";
                    mdqFolderList.Add(mdqFolderQuery);

                    List<FileProperties> sfReportFolders = new List<FileProperties>();
                    List<FileProperties> sfReportFiles = new List<FileProperties>();

                    ms = SalesforceCredentials.getMetadataService(reqOrg);
                    FileProperties[] sfFolders = ms.listMetadata(mdqFolderList.ToArray(), Convert.ToDouble(Properties.Settings.Default.DefaultAPI));

                    foreach (FileProperties fp in sfFolders)
                    {
                        sfReportFolders.Add(fp);
                    }

                    // Now loop through the sfReportFolders and retrieve the report names in those folders
                    mdqFolderList.Clear();
                    foreach (FileProperties fp in sfReportFolders)
                    {
                        // No more than 3 folders are allowed in the Folder List
                        ListMetadataQuery mdqf = new ListMetadataQuery();
                        mdqf.folder = fp.fullName;
                        mdqf.type = "Report";
                        mdqFolderList.Add(mdqf);

                        FileProperties[] sfFiles = getFolderItems(mdqFolderList, ms);

                        // now add these to the XML
                        if (sfFiles != null)
                        {
                            foreach (FileProperties sff in sfFiles)
                            {
                                packageXmlSB.Append("<members>" + sff.fullName + "</members>" + Environment.NewLine);
                            }
                        }

                        mdqFolderList.Clear();
                    }

                    packageXmlSB.Append("<name>Report</name>" + Environment.NewLine);
                    packageXmlSB.Append("</types>" + Environment.NewLine);
                }
                else if (selected == "StandardValueSet")
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

                }
                else
                {
                    String[] members = new String[1];
                    members[0] = "*";
                    getMetadataTypes(selected, packageXmlSB, members);
                }

                alreadyAdded.Add(selected);
            }

            packageXmlSB.Append("<version>" + Properties.Settings.Default.DefaultAPI + "</version>" + Environment.NewLine);
            packageXmlSB.Append("</Package>");

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                packageXMLFile = this.tbFromOrgSaveLocation.Text + "\\package.xml";
            }
            
            File.WriteAllText(@packageXMLFile, packageXmlSB.ToString());

            // Retrieve Zip File
            RetrieveRequest retrieveRequest = new RetrieveRequest();
            retrieveRequest.apiVersion = Convert.ToDouble(Properties.Settings.Default.DefaultAPI);
            retrieveRequest.unpackaged = setUnpackaged(packageXMLFile);

            AsyncResult asyncResult = new AsyncResult();

            ms = SalesforceCredentials.getMetadataService(reqOrg);
            if (ms != null)
            {
                asyncResult = ms.retrieve(retrieveRequest);
            }

            RetrieveResult result = waitForRetrieveCompletion(asyncResult, reqOrg);

            String timestamp = DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + 
                               DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + "_" + DateTime.Now.Millisecond.ToString();

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                zipFile = this.tbFromOrgSaveLocation.Text + "\\components_" + extractToFolder + "_" + timestamp + ".zip";
            }

            if (result.zipFile != null)
            {
                try
                {
                    File.WriteAllBytes(@zipFile, result.zipFile);

                    // Unzip the package and store it to the folder specified
                    if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
                    {
                        String target_dir = this.tbFromOrgSaveLocation.Text + '\\' + extractToFolder;

                        if (!Directory.Exists(target_dir))
                        {
                            DirectoryInfo di = Directory.CreateDirectory(target_dir);
                        }
                        else
                        {
                            Directory.Delete(target_dir, true);
                        }

                        ZipFile.ExtractToDirectory(zipFile, target_dir);


                        // If cbConvertToVSCodeStyle == true then add the -meta.xml to the end of the file for each file in the directories, except for LWC and Aura
                        // Objects will need to be reworked as well as their folder structure is different
                        if (this.cbConvertToVSCodeStyle.Checked == true)
                        {
                            addVSCodeFileExtension(target_dir);
                        }
                    }

                    this.rtMessages.Text = "Metadata Extract Completed Successfully";
                }
                catch (System.IO.IOException ioExc)
                {
                    this.rtMessages.Text = "IO Exception: " + ioExc.Message;
                }
                catch (Exception exc)
                {
                    this.rtMessages.Text = "There was an error saving the package.zip file to the location specified: " + exc.Message;
                }
                finally
                {
                }
            }
        }


        // Add metadata types
        private void getMetadataTypes(String metadataObjectName, StringBuilder packageXmlSB, String[] members)
        {
            if (!alreadyAdded.Contains(metadataObjectName))
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

                alreadyAdded.Add(metadataObjectName);
            }
        }


        private Package parsePackageManifest(String fileName)
        {
            Package packageManifest = null;
            List<PackageTypeMembers> listPackageTypes = new List<PackageTypeMembers>();  // convert this to an array in the package

            XmlDocument sfPackage = new XmlDocument();
            sfPackage.Load(fileName);

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


        private RetrieveResult waitForRetrieveCompletion(AsyncResult asyncResult, UtilityClass.REQUESTINGORG reqOrg)
        {
            // Wait for the retrieve to complete
            int poll = 0;
            int waitTimeMilliSecs = this.ONE_SECOND;
            String asyncResultId = asyncResult.id;
            RetrieveResult result = new RetrieveResult();

            MetadataService ms = SalesforceCredentials.getMetadataService(reqOrg);
            result = ms.checkRetrieveStatus(asyncResultId, true);

            while (!result.done)
            {
                System.Threading.Thread.Sleep(waitTimeMilliSecs);
                waitTimeMilliSecs *= 2;
                if (poll++ > this.MAX_NUM_POLL_REQUESTS)
                {
                    System.Diagnostics.Debug.WriteLine("Request timed out.If this is a large set of metadata components, check that the time allowed by MAX_NUM_POLL_REQUESTS is sufficient.");
                }
                else
                {
                    result = ms.checkRetrieveStatus(asyncResultId, true);
                }
            }

            return result;
        }

        private List<String> getTabDescribe(UtilityClass.REQUESTINGORG reqOrg)
        {
            DescribeTab[] descrTabs = SalesforceCredentials.getDescribeTab(reqOrg);

            List<String> members = new List<String>();
            for (Int32 i = 0; i < descrTabs.Length; i++)
            {
                members.Add(descrTabs[i].name);
            }

            return members;
        }

        private List<String> getSObjectMembers(UtilityClass.REQUESTINGORG reqOrg)
        {
            DescribeGlobalResult dgr = SalesforceCredentials.getDescribeGlobalResult(reqOrg);

            List<String> members = new List<string>();
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


        private void addVSCodeFileExtension(String targetDirectory)
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
            folderSkips.Add("objects");
            folderSkips.Add("objectTranslations");

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

                    if (folderSkips.Contains(subdirSplit[subdirSplit.Length - 1]))
                    {
                        continue;
                    }

                    try
                    {
                        // Get all files in the current directory
                        String[] files = Directory.GetFiles(subDirectoryList[i]);
                        if (files.Length > 0)
                        {
                            for (Int32 j = 0; j < files.Length; j++)
                            {
                                FileInfo fi = new FileInfo(files[j]);
                                fi.CopyTo(files[j] + "-meta.xml");
                                fi.Delete();
                            }
                        }
                    }
                    catch (Exception exc)
                    {

                    }

                    subdirectorySearchCompleted.Add(subDirectoryList[i]);

                }

                // Check if there are any additional sub directories in the current directory and add them to the list
                List<String> subDirectories = new List<String>();
                for (Int32 i = 0; i < subDirectoryList.Count; i++)
                {
                    if (subDirectoryList[i] != targetDirectory)
                    {
                        List<String> sds = getSubdirectories(subDirectoryList[i]);
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
                this.btnRetrieveMetadataFromSelected.Enabled = true;
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


        private void btnRetrieveMetadata_Click(object sender, EventArgs e)
        {
            if (this.tbExistingPackageXml.Text == "")
            {
                MessageBox.Show("Please select a package.xml file");
            }
            else if (!this.tbExistingPackageXml.Text.EndsWith("package.xml"))
            {
                MessageBox.Show("The file selected must be in XML format with the naming format of package.xml. Please select a file with the name package.xml");
            }
            else
            {
                String zipFile = this.tbExistingPackageXml.Text.Substring(0, this.tbExistingPackageXml.Text.Length - 11) + "components.zip";

                Boolean loginSuccess = SalesforceCredentials.salesforceLogin(UtilityClass.REQUESTINGORG.FROMORG);
                if (loginSuccess == false)
                {
                    MessageBox.Show("Please check username, password and/or security token");
                    return;
                }

                // Retrieve Zip File
                RetrieveRequest retrieveRequest = new RetrieveRequest();
                retrieveRequest.apiVersion = Convert.ToDouble(Properties.Settings.Default.DefaultAPI);
                retrieveRequest.unpackaged = setUnpackaged(this.tbExistingPackageXml.Text);

                AsyncResult asyncResult = new AsyncResult();
                if (SalesforceCredentials.fromOrgMS != null)
                {
                    asyncResult = SalesforceCredentials.fromOrgMS.retrieve(retrieveRequest);
                }

                RetrieveResult result = waitForRetrieveCompletion(asyncResult, UtilityClass.REQUESTINGORG.FROMORG);

                if (result.zipFile != null)
                {
                    try
                    {
                        File.WriteAllBytes(@zipFile, result.zipFile);
                        this.rtMessages.Text = "Metadata Extract Completed Successfully";
                    }
                    catch (Exception exc)
                    {
                        this.rtMessages.Text = "There was an error saving the package.zip file to the location specified: " + exc.Message;
                    }
                    finally
                    {
                    }
                }
            }
        }
    }
}
