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


using SalesforceMetadata.PartnerWSDL;
using SalesforceMetadata.MetadataWSDL;
using SalesforceMetadata.ToolingWSDL;


namespace SalesforceMetadata
{
    public partial class AutomationReporter : Form
    {
        //private Dictionary<String, String> metadataXmlNameToFolder;
        private Dictionary<String, String> usernameToSecurityToken;
        //private frmUserSettings userSetting;

        public List<String> subdirectorySearchCompleted;
        public List<String> fileNames;

        public Dictionary<String, String> classIdToClassName;

        public AutomationReporter()
        {
            InitializeComponent();
            populateCredentials();
        }


        private void populateCredentials()
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
                //String enterpriseWsdlUrl = "";
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


        private void cmbUserName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SalesforceCredentials.isProduction[this.cmbUserName.Text] == true)
            {
                //this.lblSalesforce.Text = "Salesforce";
                //this.Text = "Salesforce Metadata - Production";
            }
            else
            {
                //this.lblSalesforce.Text = "Salesforce Sandbox";
                String[] userNamesplit = this.cmbUserName.Text.Split('.');
                this.Text = "Salesforce Metadata - " + userNamesplit[userNamesplit.Length - 1].ToUpper();
            }

            this.tbSecurityToken.Text = "";
            if (this.usernameToSecurityToken.ContainsKey(this.cmbUserName.Text))
            {
                this.tbSecurityToken.Text = this.usernameToSecurityToken[cmbUserName.Text];
            }
        }

        private void btnRunAutomationOptimizationReport_Click(object sender, EventArgs e)
        {
            SalesforceCredentials.fromOrgUsername = this.cmbUserName.Text;
            SalesforceCredentials.fromOrgPassword = this.tbPassword.Text;
            SalesforceCredentials.fromOrgSecurityToken = this.tbSecurityToken.Text;
            Boolean loginSuccess = SalesforceCredentials.salesforceToolingLogin();

            //Boolean loginSuccess = true;
            if (loginSuccess == false)
            {
                MessageBox.Show("Please check username, password and/or security token");
                return;
            }
            else
            {
                //runApexTriggerToolingReport();
                //runApexClassToolingReport();
                //runFlowProcessAutomationReport();
                runWorkflowAutomationReport();
            }

            MessageBox.Show("Class Extraction Complete");
        }

        private void btnFindWhereClassUsed_Click(object sender, EventArgs e)
        {
            Boolean excelIsInstalled = UtilityClass.microsoftExcelInstalledCheck();

            Dictionary<String, Dictionary<String, List<String>>> searchResultsDict = new Dictionary<String, Dictionary<String, List<String>>>();

            if (this.tbProjectFolder.Text != "")
            {
                String[] directoryPathParse = this.tbProjectFolder.Text.Split('\\');

                // See if the Project Folder contains a subfolder called classes
                String[] subdirectoriesList = Directory.GetDirectories(this.tbProjectFolder.Text);

                if (subdirectoriesList.Length > 0)
                {
                    foreach (String sd in subdirectoriesList)
                    {
                        String[] subDirectoryPathParse = sd.Split('\\');

                        if (subDirectoryPathParse[subDirectoryPathParse.Length - 1] == "classes")
                        {
                            searchResultsDict.Add("Classes", new Dictionary<String, List<String>>());
                            String[] classFiles = Directory.GetFiles(sd);

                            // Loop through the files and find the class names which end in cls
                            foreach (String classFileName in classFiles)
                            {
                                if (classFileName.EndsWith("cls"))
                                {
                                    // Get the class name then search for where it is used avoiding the current folder, profiles and permission sets
                                    String[] classNamePath = classFileName.Split('\\');
                                    String[] className = classNamePath[classNamePath.Length - 1].Split('.');

                                    // Search for the values in the subfolders
                                    List<String> searchResults = SearchUtilityClass.searchForObjectName(this.tbProjectFolder.Text, sd, classNamePath[classNamePath.Length - 1]);
                                    if (searchResults.Count > 0)
                                    {
                                        searchResultsDict["Classes"].Add(className[0], searchResults);
                                    }
                                    else
                                    {
                                        searchResultsDict["Classes"].Add(className[0], new List<string>());
                                    }
                                }
                            }
                        }
                        else if (subDirectoryPathParse[subDirectoryPathParse.Length - 1] == "flows")
                        {
                            searchResultsDict.Add("Flows", new Dictionary<String, List<String>>());
                            String[] flowFiles = Directory.GetFiles(sd);

                            // Loop through the files and find the class names which end in cls
                            foreach (String flowFileName in flowFiles)
                            {
                                // Get the class name then search for where it is used avoiding the current folder, profiles and permission sets
                                String[] flowNamePath = flowFileName.Split('\\');
                                String[] flowName = flowNamePath[flowNamePath.Length - 1].Split('.');

                                // Search for the values in the subfolders
                                List<String> searchResults = SearchUtilityClass.searchForObjectName(this.tbProjectFolder.Text, sd, flowNamePath[flowNamePath.Length - 1]);
                                if (searchResults.Count > 0)
                                {
                                    searchResultsDict["Flows"].Add(flowName[0], searchResults);
                                }
                                else
                                {
                                    searchResultsDict["Flows"].Add(flowName[0], new List<string>());
                                }
                            }
                        }
                        else if (subDirectoryPathParse[subDirectoryPathParse.Length - 1] == "lwc")
                        {
                            searchResultsDict.Add("LWCs", new Dictionary<String, List<String>>());
                            String[] lwcFolders = Directory.GetDirectories(sd);

                            // Loop through the files and find the class names which end in cls
                            foreach (String lwcFolderPath in lwcFolders)
                            {
                                // Get the class name then search for where it is used avoiding the current folder, profiles and permission sets
                                String[] lwcNamePath = lwcFolderPath.Split('\\');
                                String lwcFolderName = lwcNamePath[lwcNamePath.Length - 1];

                                // Search for the values in the subfolders
                                List<String> searchResults = SearchUtilityClass.searchForObjectName(this.tbProjectFolder.Text, sd, lwcNamePath[lwcNamePath.Length - 1]);
                                if (searchResults.Count > 0)
                                {
                                    searchResultsDict["LWCs"].Add(lwcFolderName, searchResults);
                                }
                                else
                                {
                                    searchResultsDict["LWCs"].Add(lwcFolderName, new List<string>());
                                }
                            }
                        }
                        else if (subDirectoryPathParse[subDirectoryPathParse.Length - 1] == "pages")
                        {
                            searchResultsDict.Add("Pages", new Dictionary<String, List<String>>());
                            String[] vfPageFiles = Directory.GetFiles(sd);

                            // Loop through the files and find the class names which end in cls
                            foreach (String vfPageFileName in vfPageFiles)
                            {
                                if (vfPageFileName.EndsWith("page"))
                                {
                                    // Get the class name then search for where it is used avoiding the current folder, profiles and permission sets
                                    String[] vfPageNamePath = vfPageFileName.Split('\\');
                                    String[] vfPageName = vfPageNamePath[vfPageNamePath.Length - 1].Split('.');

                                    // Search for the values in the subfolders
                                    List<String> searchResults = SearchUtilityClass.searchForObjectName(this.tbProjectFolder.Text, sd, vfPageNamePath[vfPageNamePath.Length - 1]);
                                    if (searchResults.Count > 0)
                                    {
                                        searchResultsDict["Pages"].Add(vfPageName[0], searchResults);
                                    }
                                    else
                                    {
                                        searchResultsDict["Pages"].Add(vfPageName[0], new List<string>());
                                    }
                                }
                            }
                        }
                        //else if (subDirectoryPathParse[subDirectoryPathParse.Length - 1] == "triggers")
                        //{
                        //    searchResultsDict.Add("triggers", new Dictionary<String, List<String>>());
                        //    String[] triggerFiles = Directory.GetDirectories(sd);

                        //    // Loop through the files and find the class names which end in cls
                        //    foreach (String triggerFileName in triggerFiles)
                        //    {
                        //        // Get the class name then search for where it is used avoiding the current folder, profiles and permission sets
                        //        String[] triggerNamePath = triggerFileName.Split('\\');
                        //        String[] triggerName = triggerNamePath[triggerNamePath.Length - 1].Split('.');

                        //        // Search for the values in the subfolders
                        //        List<String> searchResults = SearchUtilityClass.searchForObjectName(this.tbProjectFolder.Text, sd, triggerNamePath[triggerNamePath.Length - 1]);
                        //        if (searchResults.Count > 0)
                        //        {
                        //            searchResultsDict["triggers"].Add(triggerName[0], searchResults);
                        //        }
                        //        else
                        //        {
                        //            searchResultsDict["triggers"].Add(triggerName[0], new List<string>());
                        //        }
                        //    }
                        //}
                    }
                }
            }

            // Write contents to Excel
            if (searchResultsDict.Count > 0)
            {
                Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
                xlapp.Visible = false;

                Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlapp.Workbooks.Add();

                foreach (String folderName in searchResultsDict.Keys)
                {
                    Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                                (System.Reflection.Missing.Value,
                                                                                 xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                                 System.Reflection.Missing.Value,
                                                                                 System.Reflection.Missing.Value);

                    xlWorksheet.Name = folderName;

                    //Int32 rowStart = 2;
                    Int32 rowEnd = 2;
                    //Int32 colStart = 2;
                    Int32 colEnd = 2;
                    //Int32 lastRowNumber = 2;

                    foreach (String objName in searchResultsDict[folderName].Keys)
                    {
                        writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, objName);
                        writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd + 1, searchResultsDict[folderName][objName].Count.ToString());

                        formatExcelRange(xlWorksheet,
                                            rowEnd,
                                            rowEnd,
                                            colEnd,
                                            colEnd + 1,
                                            14,
                                            255,
                                            255,
                                            255,
                                            63,
                                            98,
                                            174,
                                            true,
                                            false,
                                            "");

                        rowEnd++;

                        if (searchResultsDict[folderName][objName].Count > 0)
                        {
                            foreach (String relatedName in searchResultsDict[folderName][objName])
                            {
                                writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd + 2, relatedName);
                                rowEnd++;
                            }
                        }

                        rowEnd++;
                    }
                }

                xlapp.Visible = true;
            }
        }

        private void runApexTriggerToolingReport()
        {
            Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
            xlapp.Visible = true;

            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlapp.Workbooks.Add();

            Int32 apexTriggerRowId = 1;

            Microsoft.Office.Interop.Excel.Worksheet xlApexTriggerWrksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                           (System.Reflection.Missing.Value,
                                                                           xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                           System.Reflection.Missing.Value,
                                                                           System.Reflection.Missing.Value);
            xlApexTriggerWrksheet.Name = "ApexTriggers";

            xlApexTriggerWrksheet.Cells[apexTriggerRowId, 1].Value = "ApexTriggerId";
            xlApexTriggerWrksheet.Cells[apexTriggerRowId, 2].Value = "ApexTriggerName";
            xlApexTriggerWrksheet.Cells[apexTriggerRowId, 3].Value = "ApiVersion";
            xlApexTriggerWrksheet.Cells[apexTriggerRowId, 4].Value = "Status";
            xlApexTriggerWrksheet.Cells[apexTriggerRowId, 5].Value = "NamespacePrefix";
            xlApexTriggerWrksheet.Cells[apexTriggerRowId, 6].Value = "TableEnumOrId";
            xlApexTriggerWrksheet.Cells[apexTriggerRowId, 7].Value = "EntityDefinitionId";
            xlApexTriggerWrksheet.Cells[apexTriggerRowId, 8].Value = "LengthWithoutComments";
            xlApexTriggerWrksheet.Cells[apexTriggerRowId, 9].Value = "ManageableState";

            xlApexTriggerWrksheet.Cells[apexTriggerRowId, 10].Value = "UsageBeforeInsert";
            xlApexTriggerWrksheet.Cells[apexTriggerRowId, 11].Value = "UsageBeforeUpdate";
            xlApexTriggerWrksheet.Cells[apexTriggerRowId, 12].Value = "UsageBeforeDelete";
            xlApexTriggerWrksheet.Cells[apexTriggerRowId, 13].Value = "UsageAfterInsert";
            xlApexTriggerWrksheet.Cells[apexTriggerRowId, 14].Value = "UsageAfterUpdate";
            xlApexTriggerWrksheet.Cells[apexTriggerRowId, 15].Value = "UsageAfterDelete";
            xlApexTriggerWrksheet.Cells[apexTriggerRowId, 16].Value = "UsageAfterUndelete";
            xlApexTriggerWrksheet.Cells[apexTriggerRowId, 17].Value = "UsageIsBulk";

            apexTriggerRowId++;

            String query = ToolingApiHelper.ApexTriggerQuery("");
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            toolingQr = SalesforceCredentials.fromOrgToolingSvc.query(query);

            if (toolingQr.records == null) return;

            toolingRecords = toolingQr.records;

            foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
            {
                ApexTrigger1 at = new ApexTrigger1();
                at = (ApexTrigger1)toolingRecord;

                xlApexTriggerWrksheet.Cells[apexTriggerRowId, 1].Value = at.Id;
                xlApexTriggerWrksheet.Cells[apexTriggerRowId, 2].Value = at.Name;
                xlApexTriggerWrksheet.Cells[apexTriggerRowId, 3].Value = at.ApiVersion;
                xlApexTriggerWrksheet.Cells[apexTriggerRowId, 4].Value = at.Status;
                xlApexTriggerWrksheet.Cells[apexTriggerRowId, 5].Value = at.NamespacePrefix;
                xlApexTriggerWrksheet.Cells[apexTriggerRowId, 6].Value = at.TableEnumOrId;
                xlApexTriggerWrksheet.Cells[apexTriggerRowId, 7].Value = at.EntityDefinitionId;
                xlApexTriggerWrksheet.Cells[apexTriggerRowId, 8].Value = at.LengthWithoutComments;
                xlApexTriggerWrksheet.Cells[apexTriggerRowId, 9].Value = at.ManageableState;
                xlApexTriggerWrksheet.Cells[apexTriggerRowId, 10].Value = at.UsageBeforeInsert;
                xlApexTriggerWrksheet.Cells[apexTriggerRowId, 11].Value = at.UsageBeforeUpdate;
                xlApexTriggerWrksheet.Cells[apexTriggerRowId, 12].Value = at.UsageBeforeDelete;
                xlApexTriggerWrksheet.Cells[apexTriggerRowId, 13].Value = at.UsageAfterInsert;
                xlApexTriggerWrksheet.Cells[apexTriggerRowId, 14].Value = at.UsageAfterUpdate;
                xlApexTriggerWrksheet.Cells[apexTriggerRowId, 15].Value = at.UsageAfterDelete;
                xlApexTriggerWrksheet.Cells[apexTriggerRowId, 16].Value = at.UsageAfterUndelete;
                xlApexTriggerWrksheet.Cells[apexTriggerRowId, 17].Value = at.UsageIsBulk;

                apexTriggerRowId++;
            }
        }

        private void runApexClassToolingReport()
        {
            classIdToClassName = new Dictionary<String, String>();

            Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
            xlapp.Visible = true;

            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlapp.Workbooks.Add();

            Int32 apexClassRowId = 1;
            Int32 apexConstructorRowId = 1;
            Int32 apexExternalRefRowId = 1;
            Int32 apexInnerClassRowId = 1;
            Int32 apexInterfaceRowId = 1;
            Int32 apexMethodRowId = 1;
            Int32 apexPropertyRowId = 1;
            Int32 apexVariableRowId = 1;
            Int32 apexTableDeclRowId = 1;

            Microsoft.Office.Interop.Excel.Worksheet xlApexClassWrksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                           (System.Reflection.Missing.Value,
                                                                           xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                           System.Reflection.Missing.Value,
                                                                           System.Reflection.Missing.Value);
            xlApexClassWrksheet.Name = "ApexClasses";

            xlApexClassWrksheet.Cells[apexClassRowId, 1].Value = "ApexClassId";
            xlApexClassWrksheet.Cells[apexClassRowId, 2].Value = "ApexClassName";
            xlApexClassWrksheet.Cells[apexClassRowId, 3].Value = "ApiVersion";
            xlApexClassWrksheet.Cells[apexClassRowId, 4].Value = "Status";
            xlApexClassWrksheet.Cells[apexClassRowId, 5].Value = "ParentClass";
            xlApexClassWrksheet.Cells[apexClassRowId, 6].Value = "NamespacePrefix";
            apexClassRowId++;


            Microsoft.Office.Interop.Excel.Worksheet xlApexConstructorWrksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                           (System.Reflection.Missing.Value,
                                                                           xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                           System.Reflection.Missing.Value,
                                                                           System.Reflection.Missing.Value);
            xlApexConstructorWrksheet.Name = "Constructors";
            xlApexConstructorWrksheet.Cells[apexConstructorRowId, 1].Value = "ApexClassId";
            xlApexConstructorWrksheet.Cells[apexConstructorRowId, 2].Value = "ApexClassName";
            xlApexConstructorWrksheet.Cells[apexConstructorRowId, 3].Value = "ConstructorType";
            xlApexConstructorWrksheet.Cells[apexConstructorRowId, 4].Value = "Annotations";
            xlApexConstructorWrksheet.Cells[apexConstructorRowId, 5].Value = "Modifiers";
            xlApexConstructorWrksheet.Cells[apexConstructorRowId, 6].Value = "ConstructorName";
            xlApexConstructorWrksheet.Cells[apexConstructorRowId, 7].Value = "Parameters";
            apexConstructorRowId++;


            Microsoft.Office.Interop.Excel.Worksheet xlApexExternalRefWrksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                           (System.Reflection.Missing.Value,
                                                                           xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                           System.Reflection.Missing.Value,
                                                                           System.Reflection.Missing.Value);
            xlApexExternalRefWrksheet.Name = "ExternalReferences";
            xlApexExternalRefWrksheet.Cells[apexExternalRefRowId, 1].Value = "ApexClassId";
            xlApexExternalRefWrksheet.Cells[apexExternalRefRowId, 2].Value = "ApexClassName";
            xlApexExternalRefWrksheet.Cells[apexExternalRefRowId, 3].Value = "Namespace";
            xlApexExternalRefWrksheet.Cells[apexExternalRefRowId, 4].Value = "ExternalReferenceName";
            xlApexExternalRefWrksheet.Cells[apexExternalRefRowId, 5].Value = "ExternalMethodName";
            xlApexExternalRefWrksheet.Cells[apexExternalRefRowId, 6].Value = "IsStatic";
            xlApexExternalRefWrksheet.Cells[apexExternalRefRowId, 7].Value = "Parameters";
            xlApexExternalRefWrksheet.Cells[apexExternalRefRowId, 8].Value = "ArgTypes";
            xlApexExternalRefWrksheet.Cells[apexExternalRefRowId, 9].Value = "ReturnType";
            xlApexExternalRefWrksheet.Cells[apexExternalRefRowId, 10].Value = "ExternalSymbol";
            apexExternalRefRowId++;


            Microsoft.Office.Interop.Excel.Worksheet xlApexInnerClassWrksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                           (System.Reflection.Missing.Value,
                                                                           xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                           System.Reflection.Missing.Value,
                                                                           System.Reflection.Missing.Value);
            xlApexInnerClassWrksheet.Name = "InnerClasses";
            xlApexInnerClassWrksheet.Cells[apexInnerClassRowId, 1].Value = "ApexClassId";
            xlApexInnerClassWrksheet.Cells[apexInnerClassRowId, 2].Value = "ApexClassName";
            xlApexInnerClassWrksheet.Cells[apexInnerClassRowId, 3].Value = "Namespace";
            xlApexInnerClassWrksheet.Cells[apexInnerClassRowId, 4].Value = "InnerClassName";
            apexInnerClassRowId++;


            Microsoft.Office.Interop.Excel.Worksheet xlApexInterfaceWrksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                           (System.Reflection.Missing.Value,
                                                                           xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                           System.Reflection.Missing.Value,
                                                                           System.Reflection.Missing.Value);
            xlApexInterfaceWrksheet.Name = "Interfaces";
            xlApexInterfaceWrksheet.Cells[apexInterfaceRowId, 1].Value = "ApexClassId";
            xlApexInterfaceWrksheet.Cells[apexInterfaceRowId, 2].Value = "ApexClassName";
            xlApexInterfaceWrksheet.Cells[apexInterfaceRowId, 3].Value = "Interfaces";
            apexInterfaceRowId++;


            Microsoft.Office.Interop.Excel.Worksheet xlApexMethodWrksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                           (System.Reflection.Missing.Value,
                                                                           xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                           System.Reflection.Missing.Value,
                                                                           System.Reflection.Missing.Value);
            xlApexMethodWrksheet.Name = "Methods";
            xlApexMethodWrksheet.Cells[apexMethodRowId, 1].Value = "ApexClassId";
            xlApexMethodWrksheet.Cells[apexMethodRowId, 2].Value = "ApexClassName";
            xlApexMethodWrksheet.Cells[apexMethodRowId, 3].Value = "MethodType";
            xlApexMethodWrksheet.Cells[apexMethodRowId, 4].Value = "Annotations";
            xlApexMethodWrksheet.Cells[apexMethodRowId, 5].Value = "Modifiers";
            xlApexMethodWrksheet.Cells[apexMethodRowId, 6].Value = "MethodName";
            xlApexMethodWrksheet.Cells[apexMethodRowId, 7].Value = "Parameters";
            xlApexMethodWrksheet.Cells[apexMethodRowId, 8].Value = "ReturnType";
            apexMethodRowId++;


            Microsoft.Office.Interop.Excel.Worksheet xlApexPropertyWrksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                           (System.Reflection.Missing.Value,
                                                                           xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                           System.Reflection.Missing.Value,
                                                                           System.Reflection.Missing.Value);
            xlApexPropertyWrksheet.Name = "Properties";
            xlApexPropertyWrksheet.Cells[apexPropertyRowId, 1].Value = "ApexClassId";
            xlApexPropertyWrksheet.Cells[apexPropertyRowId, 2].Value = "ApexClassName";
            xlApexPropertyWrksheet.Cells[apexPropertyRowId, 3].Value = "Annotations";
            xlApexPropertyWrksheet.Cells[apexPropertyRowId, 4].Value = "Modifiers";
            xlApexPropertyWrksheet.Cells[apexPropertyRowId, 5].Value = "PropertyName";
            xlApexPropertyWrksheet.Cells[apexPropertyRowId, 6].Value = "PropertyType";
            apexPropertyRowId++;

            Microsoft.Office.Interop.Excel.Worksheet xlApexVariableWrksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                           (System.Reflection.Missing.Value,
                                                                           xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                           System.Reflection.Missing.Value,
                                                                           System.Reflection.Missing.Value);
            xlApexVariableWrksheet.Name = "Variables";
            xlApexVariableWrksheet.Cells[apexVariableRowId, 1].Value = "ApexClassId";
            xlApexVariableWrksheet.Cells[apexVariableRowId, 2].Value = "ApexClassName";
            xlApexVariableWrksheet.Cells[apexVariableRowId, 3].Value = "Annotations";
            xlApexVariableWrksheet.Cells[apexVariableRowId, 4].Value = "Modifiers";
            xlApexVariableWrksheet.Cells[apexVariableRowId, 5].Value = "VariableName";
            xlApexVariableWrksheet.Cells[apexVariableRowId, 6].Value = "VariableType";
            apexVariableRowId++;


            Microsoft.Office.Interop.Excel.Worksheet xlApexTableDeclWrksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                           (System.Reflection.Missing.Value,
                                                                           xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                           System.Reflection.Missing.Value,
                                                                           System.Reflection.Missing.Value);
            xlApexTableDeclWrksheet.Name = "TableDeclaration";
            xlApexTableDeclWrksheet.Cells[apexTableDeclRowId, 1].Value = "ApexClassId";
            xlApexTableDeclWrksheet.Cells[apexTableDeclRowId, 2].Value = "ApexClassName";
            xlApexTableDeclWrksheet.Cells[apexTableDeclRowId, 3].Value = "Annotations";
            xlApexTableDeclWrksheet.Cells[apexTableDeclRowId, 4].Value = "Modifiers";
            xlApexTableDeclWrksheet.Cells[apexTableDeclRowId, 5].Value = "TableDeclarationName";
            xlApexTableDeclWrksheet.Cells[apexTableDeclRowId, 6].Value = "TableDeclarationType";
            apexTableDeclRowId++;


            // Make a call to the Tooling API to retrieve the ApexClassMember passing in the ApexClass IDs
            String query = ToolingApiHelper.ApexClassQuery("");
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            toolingQr = SalesforceCredentials.fromOrgToolingSvc.query(query);

            if (toolingQr.records == null) return;

            toolingRecords = toolingQr.records;

            foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
            {
                SalesforceMetadata.ToolingWSDL.ApexClass1 apexClass = (SalesforceMetadata.ToolingWSDL.ApexClass1)toolingRecord;
                classIdToClassName.Add(apexClass.Id, apexClass.Name);

                xlApexClassWrksheet.Cells[apexClassRowId, 1].Value = apexClass.Id;
                xlApexClassWrksheet.Cells[apexClassRowId, 2].Value = apexClass.Name;
                xlApexClassWrksheet.Cells[apexClassRowId, 3].Value = apexClass.ApiVersion;
                xlApexClassWrksheet.Cells[apexClassRowId, 4].Value = apexClass.Status;
                xlApexClassWrksheet.Cells[apexClassRowId, 4].Value = "";
                xlApexClassWrksheet.Cells[apexClassRowId, 6].Value = apexClass.NamespacePrefix;

                if (apexClass.SymbolTable != null)
                {
                    SalesforceMetadata.ToolingWSDL.SymbolTable apexClassSymbolTbl = apexClass.SymbolTable;

                    xlApexClassWrksheet.Cells[apexClassRowId, 5].Value = apexClassSymbolTbl.parentClass;

                    // Constructors
                    if (apexClassSymbolTbl.constructors != null)
                    {
                        foreach (SalesforceMetadata.ToolingWSDL.Constructor constr in apexClassSymbolTbl.constructors)
                        {
                            xlApexConstructorWrksheet.Cells[apexConstructorRowId, 1].Value = apexClass.Id;
                            xlApexConstructorWrksheet.Cells[apexConstructorRowId, 2].Value = apexClass.Name;

                            xlApexConstructorWrksheet.Cells[apexConstructorRowId, 3].Value = constr.type;

                            if (constr.annotations != null)
                            {
                                String annotations = "";

                                foreach (SalesforceMetadata.ToolingWSDL.Annotation annot in constr.annotations)
                                {
                                    annotations = annotations + annot.name + ", ";
                                }

                                xlApexConstructorWrksheet.Cells[apexConstructorRowId, 4].Value = annotations.Substring(0, annotations.Length - 2);
                            }

                            if (constr.modifiers != null)
                            {
                                String modifiers = "";

                                foreach (String modifier in constr.modifiers)
                                {
                                    modifiers = modifiers + modifier + ", ";
                                }

                                xlApexConstructorWrksheet.Cells[apexConstructorRowId, 5].Value = modifiers.Substring(0, modifiers.Length - 2);
                            }

                            xlApexConstructorWrksheet.Cells[apexConstructorRowId, 6].Value = constr.name;

                            if (constr.parameters != null)
                            {
                                String parameters = "";

                                foreach (SalesforceMetadata.ToolingWSDL.Parameter param in constr.parameters)
                                {
                                    parameters = parameters + param.type + " - " + param.name + ", ";
                                }

                                xlApexConstructorWrksheet.Cells[apexConstructorRowId, 7].Value = parameters.Substring(0, parameters.Length - 2);
                            }

                            apexConstructorRowId++;
                        }
                    }

                    // External References
                    if (apexClassSymbolTbl.externalReferences != null)
                    {
                        foreach (SalesforceMetadata.ToolingWSDL.ExternalReference extRef in apexClassSymbolTbl.externalReferences)
                        {
                            xlApexExternalRefWrksheet.Cells[apexExternalRefRowId, 1].Value = apexClass.Id;
                            xlApexExternalRefWrksheet.Cells[apexExternalRefRowId, 2].Value = apexClass.Name;

                            xlApexExternalRefWrksheet.Cells[apexExternalRefRowId, 3].Value = extRef.@namespace;
                            xlApexExternalRefWrksheet.Cells[apexExternalRefRowId, 4].Value = extRef.name;

                            if (extRef.methods != null)
                            {
                                foreach (SalesforceMetadata.ToolingWSDL.ExternalMethod extMethod in extRef.methods)
                                {
                                    xlApexExternalRefWrksheet.Cells[apexExternalRefRowId, 5].Value = extMethod.name;
                                    xlApexExternalRefWrksheet.Cells[apexExternalRefRowId, 6].Value = extMethod.isStatic.ToString();

                                    if (extMethod.parameters != null)
                                    {
                                        String parameters = "";
                                        foreach (SalesforceMetadata.ToolingWSDL.Parameter param in extMethod.parameters)
                                        {
                                            parameters = parameters + param.type + " - " + param.name + ", ";
                                        }

                                        xlApexExternalRefWrksheet.Cells[apexExternalRefRowId, 7].Value = parameters.Substring(0, parameters.Length - 2);
                                    }

                                    if (extMethod.argTypes != null)
                                    {
                                        String argTypes = "";
                                        foreach (String argType in extMethod.argTypes)
                                        {
                                            argTypes = argTypes + argType + ", ";
                                        }

                                        xlApexExternalRefWrksheet.Cells[apexExternalRefRowId, 8].Value = argTypes.Substring(0, argTypes.Length - 2);
                                    }

                                    xlApexExternalRefWrksheet.Cells[apexExternalRefRowId, 9].Value = extMethod.returnType;
                                }
                            }

                            if (extRef.variables != null)
                            {
                                String extSymbols = "";
                                foreach (SalesforceMetadata.ToolingWSDL.ExternalSymbol extSymb in extRef.variables)
                                {
                                    extSymbols = extSymbols + extSymb + ", ";
                                }

                                xlApexExternalRefWrksheet.Cells[apexExternalRefRowId, 10].Value = extSymbols.Substring(0, extSymbols.Length - 2);
                            }

                            apexExternalRefRowId++;
                        }
                    }

                    // Inner Classes
                    if (apexClassSymbolTbl.innerClasses != null)
                    {
                        foreach (SalesforceMetadata.ToolingWSDL.SymbolTable innerCls in apexClassSymbolTbl.innerClasses)
                        {
                            xlApexInnerClassWrksheet.Cells[apexInnerClassRowId, 1].Value = apexClass.Id;
                            xlApexInnerClassWrksheet.Cells[apexInnerClassRowId, 2].Value = apexClass.Name;

                            xlApexInnerClassWrksheet.Cells[apexInnerClassRowId, 3].Value = innerCls.@namespace;
                            xlApexInnerClassWrksheet.Cells[apexInnerClassRowId, 4].Value = innerCls.name;

                            //if (extRef.methods != null)
                            //{
                            //    foreach (SalesforceMetadata.ToolingWSDL.ExternalMethod extMethod in extRef.methods)
                            //    {
                            //        xlApexExternalRefWrksheet.Cells[apexConstructorRowId, 5].Value = extMethod.name;
                            //        xlApexExternalRefWrksheet.Cells[apexConstructorRowId, 6].Value = extMethod.isStatic.ToString();

                            //        if (extMethod.parameters != null)
                            //        {
                            //            String parameters = "";
                            //            foreach (SalesforceMetadata.ToolingWSDL.Parameter param in extMethod.parameters)
                            //            {
                            //                parameters = parameters + param.type + " - " + param.name + ", ";
                            //            }

                            //            xlApexExternalRefWrksheet.Cells[apexConstructorRowId, 7].Value = parameters.Substring(0, parameters.Length - 2);
                            //        }

                            //        if (extMethod.argTypes != null)
                            //        {
                            //            String argTypes = "";
                            //            foreach (String argType in extMethod.argTypes)
                            //            {
                            //                argTypes = argTypes + argType + ", ";
                            //            }

                            //            xlApexExternalRefWrksheet.Cells[apexConstructorRowId, 8].Value = argTypes.Substring(0, argTypes.Length - 2);
                            //        }

                            //        xlApexExternalRefWrksheet.Cells[apexConstructorRowId, 9].Value = extMethod.returnType;

                            //    }
                            //}

                            apexInnerClassRowId++;
                        }
                    }

                    // Interfaces
                    if (apexClassSymbolTbl.interfaces != null)
                    {
                        xlApexInterfaceWrksheet.Cells[apexInterfaceRowId, 1].Value = apexClass.Id;
                        xlApexInterfaceWrksheet.Cells[apexInterfaceRowId, 2].Value = apexClass.Name;

                        String interfaces = "";
                        foreach (String interfc in apexClassSymbolTbl.interfaces)
                        {
                            interfaces = interfaces + interfc + ", ";
                        }

                        xlApexInterfaceWrksheet.Cells[apexInterfaceRowId, 3].Value = interfaces.Substring(0, interfaces.Length - 2);

                        apexInterfaceRowId++;
                    }

                    // Methods
                    if (apexClassSymbolTbl.methods != null)
                    {
                        foreach (SalesforceMetadata.ToolingWSDL.Method meth in apexClassSymbolTbl.methods)
                        {
                            xlApexMethodWrksheet.Cells[apexMethodRowId, 1].Value = apexClass.Id;
                            xlApexMethodWrksheet.Cells[apexMethodRowId, 2].Value = apexClass.Name;

                            xlApexMethodWrksheet.Cells[apexMethodRowId, 3].Value = meth.type;

                            if (meth.annotations != null)
                            {
                                String annotations = "";

                                foreach (SalesforceMetadata.ToolingWSDL.Annotation annot in meth.annotations)
                                {
                                    annotations = annotations + annot.name + ", ";
                                }

                                xlApexMethodWrksheet.Cells[apexMethodRowId, 4].Value = annotations.Substring(0, annotations.Length - 2);
                            }

                            if (meth.modifiers != null)
                            {
                                String modifiers = "";

                                foreach (String modifier in meth.modifiers)
                                {
                                    modifiers = modifiers + modifier + ", ";
                                }

                                xlApexMethodWrksheet.Cells[apexMethodRowId, 5].Value = modifiers.Substring(0, modifiers.Length - 2);
                            }

                            xlApexMethodWrksheet.Cells[apexMethodRowId, 6].Value = meth.name;

                            if (meth.parameters != null)
                            {
                                String parameters = "";

                                foreach (SalesforceMetadata.ToolingWSDL.Parameter param in meth.parameters)
                                {
                                    parameters = parameters + param.type + " - " + param.name + ", ";
                                }

                                xlApexMethodWrksheet.Cells[apexMethodRowId, 7].Value = parameters.Substring(0, parameters.Length - 2);
                            }

                            xlApexMethodWrksheet.Cells[apexMethodRowId, 8].Value = meth.returnType;

                            apexMethodRowId++;
                        }
                    }

                    // Properties
                    if (apexClassSymbolTbl.properties != null)
                    {
                        foreach (SalesforceMetadata.ToolingWSDL.VisibilitySymbol visSymb in apexClassSymbolTbl.properties)
                        {
                            xlApexPropertyWrksheet.Cells[apexPropertyRowId, 1].Value = apexClass.Id;
                            xlApexPropertyWrksheet.Cells[apexPropertyRowId, 2].Value = apexClass.Name;

                            if (visSymb.annotations != null)
                            {
                                String annotations = "";

                                foreach (SalesforceMetadata.ToolingWSDL.Annotation annot in visSymb.annotations)
                                {
                                    annotations = annotations + annot.name + ", ";
                                }

                                xlApexPropertyWrksheet.Cells[apexPropertyRowId, 3].Value = annotations.Substring(0, annotations.Length - 2);
                            }

                            if (visSymb.modifiers != null)
                            {
                                String modifiers = "";

                                foreach (String modifier in visSymb.modifiers)
                                {
                                    modifiers = modifiers + modifier + ", ";
                                }

                                xlApexPropertyWrksheet.Cells[apexPropertyRowId, 4].Value = modifiers.Substring(0, modifiers.Length - 2);
                            }


                            xlApexPropertyWrksheet.Cells[apexPropertyRowId, 5].Value = visSymb.name;

                            xlApexPropertyWrksheet.Cells[apexPropertyRowId, 6].Value = visSymb.type;

                            apexPropertyRowId++;
                        }
                    }

                    //Variables
                    if (apexClassSymbolTbl.variables != null)
                    {
                        foreach (SalesforceMetadata.ToolingWSDL.Symbol symb in apexClassSymbolTbl.variables)
                        {
                            xlApexVariableWrksheet.Cells[apexVariableRowId, 1].Value = apexClass.Id;
                            xlApexVariableWrksheet.Cells[apexVariableRowId, 2].Value = apexClass.Name;

                            if (symb.annotations != null)
                            {
                                String annotations = "";

                                foreach (SalesforceMetadata.ToolingWSDL.Annotation annot in symb.annotations)
                                {
                                    annotations = annotations + annot.name + ", ";
                                }

                                xlApexVariableWrksheet.Cells[apexVariableRowId, 3].Value = annotations.Substring(0, annotations.Length - 2);
                            }

                            if (symb.modifiers != null)
                            {
                                String modifiers = "";

                                foreach (String modifier in symb.modifiers)
                                {
                                    modifiers = modifiers + modifier + ", ";
                                }

                                xlApexVariableWrksheet.Cells[apexVariableRowId, 4].Value = modifiers.Substring(0, modifiers.Length - 2);
                            }

                            xlApexVariableWrksheet.Cells[apexVariableRowId, 5].Value = symb.name;

                            xlApexVariableWrksheet.Cells[apexVariableRowId, 6].Value = symb.type;

                            apexVariableRowId++;
                        }
                    }

                    // Table Declaration
                    if (apexClassSymbolTbl.tableDeclaration != null)
                    {
                        SalesforceMetadata.ToolingWSDL.Symbol symb = apexClassSymbolTbl.tableDeclaration;

                        xlApexTableDeclWrksheet.Cells[apexTableDeclRowId, 1].Value = apexClass.Id;
                        xlApexTableDeclWrksheet.Cells[apexTableDeclRowId, 2].Value = apexClass.Name;

                        if (symb.annotations != null)
                        {
                            String annotations = "";

                            foreach (SalesforceMetadata.ToolingWSDL.Annotation annot in symb.annotations)
                            {
                                annotations = annotations + annot.name + ", ";
                            }

                            xlApexTableDeclWrksheet.Cells[apexTableDeclRowId, 3].Value = annotations.Substring(0, annotations.Length - 2);
                        }

                        if (symb.modifiers != null)
                        {
                            String modifiers = "";

                            foreach (String modifier in symb.modifiers)
                            {
                                modifiers = modifiers + modifier + ", ";
                            }

                            xlApexTableDeclWrksheet.Cells[apexTableDeclRowId, 4].Value = modifiers.Substring(0, modifiers.Length - 2);
                        }

                        xlApexTableDeclWrksheet.Cells[apexTableDeclRowId, 5].Value = symb.name;

                        xlApexTableDeclWrksheet.Cells[apexTableDeclRowId, 6].Value = symb.type;

                        apexTableDeclRowId++;
                    }
                }


                apexClassRowId++;
            }

            xlapp.Visible = true;
        }

        private void runFlowProcessAutomationReport()
        {
            if (this.tbProjectFolder.Text != "")
            {
                if (Directory.Exists(this.tbProjectFolder.Text + "\\Flows"))
                {
                    Dictionary<String, List<FlowProcess>> declarativeTypeToName = new Dictionary<String, List<FlowProcess>>();

                    String[] fileNames = Directory.GetFiles(this.tbProjectFolder.Text + "\\Flows");

                    foreach (String fl in fileNames)
                    {
                        String[] filePathSplit = fl.Split('\\');
                        String fileApiName = filePathSplit[filePathSplit.Length - 1].Split('.')[0];

                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(fl);

                        // start contains information about the flow and specifically the Run In Mode
                        // Need to extract out the Run In Mode
                        XmlNodeList flowStart = xmlDoc.GetElementsByTagName("start");
                        XmlNodeList flowLabel = xmlDoc.GetElementsByTagName("label");
                        XmlNodeList flowProcessType = xmlDoc.GetElementsByTagName("processType");
                        XmlNodeList flowApiVersion = xmlDoc.GetElementsByTagName("apiVersion");
                        XmlNodeList status = xmlDoc.GetElementsByTagName("status");
                        XmlNodeList processType = xmlDoc.GetElementsByTagName("processType");

                        XmlNodeList recordCreates = xmlDoc.GetElementsByTagName("recordCreates");
                        XmlNodeList recordUpdates = xmlDoc.GetElementsByTagName("recordUpdates");
                        XmlNodeList recordDeletes = xmlDoc.GetElementsByTagName("recordDeletes");

                        // For Process Builder
                        //XmlNodeList actionCalls = xmlDoc.GetElementsByTagName("actionCalls");

                        // AutoLaunched Flow
                        if (status[0].InnerText != "Obsolete"
                            && processType[0].InnerText == "AutoLaunchedFlow")
                        {
                            //MessageBox.Show("Hello1");
                            FlowProcess fp = new FlowProcess();

                            fp.processName = fileApiName;

                            foreach (XmlElement elem in flowLabel)
                            {
                                if (elem.ParentNode.Name == "Flow")
                                {
                                    fp.processLabel = elem.InnerText;
                                }
                            }

                            fp.processType = "AutoLaunchedFlow";
                            fp.apiVersion = flowApiVersion[0].InnerText;
                            fp.status = flowApiVersion[0].InnerText;

                            foreach (XmlElement elem in recordCreates)
                            {
                                String ruLabel = "";
                                String ruObject = "";

                                foreach (XmlElement childElem in elem.ChildNodes)
                                {
                                    if (childElem.Name == "label")
                                    {
                                        ruLabel = childElem.InnerText;
                                    }
                                    else if (childElem.Name == "object")
                                    {
                                        ruObject = childElem.InnerText;
                                    }
                                }

                                if (fp.recordCreates.ContainsKey(ruObject))
                                {
                                    fp.recordCreates[ruObject].Add(ruLabel);
                                }
                                else
                                {
                                    List<String> tempList = new List<string> { ruLabel };
                                    Dictionary<String, List<String>> tempDictionary = new Dictionary<string, List<string>>();
                                    tempDictionary.Add(ruObject, tempList);
                                    fp.recordCreates = tempDictionary;
                                }
                            }

                            foreach (XmlElement elem in recordUpdates)
                            {
                                String ruLabel = "";
                                String ruObject = "";

                                foreach (XmlElement childElem in elem.ChildNodes)
                                {
                                    if (childElem.Name == "label")
                                    {
                                        ruLabel = childElem.InnerText;
                                    }
                                    else if (childElem.Name == "object")
                                    {
                                        ruObject = childElem.InnerText;
                                    }
                                }

                                if (fp.recordUpdates.ContainsKey(ruObject))
                                {
                                    fp.recordUpdates[ruObject].Add(ruLabel);
                                }
                                else
                                {
                                    List<String> tempList = new List<string> { ruLabel };
                                    Dictionary<String, List<String>> tempDictionary = new Dictionary<string, List<string>>();
                                    tempDictionary.Add(ruObject, tempList);
                                    fp.recordUpdates = tempDictionary;
                                }
                            }

                            foreach (XmlElement elem in recordDeletes)
                            {
                                String ruLabel = "";
                                String ruObject = "";

                                foreach (XmlElement childElem in elem.ChildNodes)
                                {
                                    if (childElem.Name == "label")
                                    {
                                        ruLabel = childElem.InnerText;
                                    }
                                    else if (childElem.Name == "object")
                                    {
                                        ruObject = childElem.InnerText;
                                    }
                                }

                                if (fp.recordDeletes.ContainsKey(ruObject))
                                {
                                    fp.recordDeletes[ruObject].Add(ruLabel);
                                }
                                else
                                {
                                    List<String> tempList = new List<string> { ruLabel };
                                    Dictionary<String, List<String>> tempDictionary = new Dictionary<string, List<string>>();
                                    tempDictionary.Add(ruObject, tempList);
                                    fp.recordDeletes = tempDictionary;
                                }
                            }


                            if (declarativeTypeToName.ContainsKey("AutoLaunchedFlow"))
                            {
                                declarativeTypeToName["AutoLaunchedFlow"].Add(fp);
                            }
                            else
                            {
                                declarativeTypeToName.Add("AutoLaunchedFlow", new List<FlowProcess> { fp });
                            }
                        }
                        // Process Builder
                        else if (status[0].InnerText != "Obsolete"
                            && processType[0].InnerText == "Workflow")
                        {
                            FlowProcess fp = new FlowProcess();

                            fp.processName = fileApiName;

                            foreach (XmlElement elem in flowLabel)
                            {
                                if (elem.ParentNode.Name == "Flow")
                                {
                                    fp.processLabel = elem.InnerText;
                                }
                            }

                            fp.processType = "Process Builder";
                            fp.apiVersion = flowApiVersion[0].InnerText;
                            fp.status = flowApiVersion[0].InnerText;

                            foreach (XmlElement elem in recordCreates)
                            {
                                String ruLabel = "";
                                String ruObject = "";

                                foreach (XmlElement childElem in elem.ChildNodes)
                                {
                                    if (childElem.Name == "label")
                                    {
                                        ruLabel = childElem.InnerText;
                                    }
                                    else if (childElem.Name == "object")
                                    {
                                        ruObject = childElem.InnerText;
                                    }
                                }

                                if (fp.recordCreates.ContainsKey(ruObject))
                                {
                                    fp.recordCreates[ruObject].Add(ruLabel);
                                }
                                else
                                {
                                    List<String> tempList = new List<string> { ruLabel };
                                    Dictionary<String, List<String>> tempDictionary = new Dictionary<string, List<string>>();
                                    tempDictionary.Add(ruObject, tempList);
                                    fp.recordCreates = tempDictionary;
                                }
                            }

                            foreach (XmlElement elem in recordUpdates)
                            {
                                String ruLabel = "";
                                String ruObject = "";

                                foreach (XmlElement childElem in elem.ChildNodes)
                                {
                                    if (childElem.Name == "label")
                                    {
                                        ruLabel = childElem.InnerText;
                                    }
                                    else if (childElem.Name == "object")
                                    {
                                        ruObject = childElem.InnerText;
                                    }
                                }

                                if (fp.recordUpdates.ContainsKey(ruObject))
                                {
                                    fp.recordUpdates[ruObject].Add(ruLabel);
                                }
                                else
                                {
                                    List<String> tempList = new List<string> { ruLabel };
                                    Dictionary<String, List<String>> tempDictionary = new Dictionary<string, List<string>>();
                                    tempDictionary.Add(ruObject, tempList);
                                    fp.recordUpdates = tempDictionary;
                                }
                            }

                            foreach (XmlElement elem in recordDeletes)
                            {
                                String ruLabel = "";
                                String ruObject = "";

                                foreach (XmlElement childElem in elem.ChildNodes)
                                {
                                    if (childElem.Name == "label")
                                    {
                                        ruLabel = childElem.InnerText;
                                    }
                                    else if (childElem.Name == "object")
                                    {
                                        ruObject = childElem.InnerText;
                                    }
                                }

                                if (fp.recordDeletes.ContainsKey(ruObject))
                                {
                                    fp.recordDeletes[ruObject].Add(ruLabel);
                                }
                                else
                                {
                                    List<String> tempList = new List<string> { ruLabel };
                                    Dictionary<String, List<String>> tempDictionary = new Dictionary<string, List<string>>();
                                    tempDictionary.Add(ruObject, tempList);
                                    fp.recordDeletes = tempDictionary;
                                }
                            }

                            if (declarativeTypeToName.ContainsKey("Process Builder"))
                            {
                                declarativeTypeToName["Process Builder"].Add(fp);
                            }
                            else
                            {
                                declarativeTypeToName.Add("Process Builder", new List<FlowProcess> { fp });
                            }
                        }
                    }

                    // Write the values in the Dictionary to an Excel file
                    Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
                    xlapp.Visible = true;

                    Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlapp.Workbooks.Add();

                    Int32 processBuilderRowId = 1;
                    Int32 flowRowId = 1;


                    // Process Builder
                    Microsoft.Office.Interop.Excel.Worksheet xlProcessBuilderWrksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                                        (System.Reflection.Missing.Value,
                                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                                        System.Reflection.Missing.Value,
                                                                                        System.Reflection.Missing.Value);
                    xlProcessBuilderWrksheet.Name = "Process Builder";

                    xlProcessBuilderWrksheet.Cells[processBuilderRowId, 1].Value = "ProcessBuilderName";
                    xlProcessBuilderWrksheet.Cells[processBuilderRowId, 2].Value = "ProcessBuilderLabel";
                    xlProcessBuilderWrksheet.Cells[processBuilderRowId, 3].Value = "ProcessBuilderType";
                    xlProcessBuilderWrksheet.Cells[processBuilderRowId, 4].Value = "Status";
                    xlProcessBuilderWrksheet.Cells[processBuilderRowId, 5].Value = "API Version";
                    xlProcessBuilderWrksheet.Cells[processBuilderRowId, 6].Value = "Run In Mode";
                    xlProcessBuilderWrksheet.Cells[processBuilderRowId, 7].Value = "Record Creates";
                    xlProcessBuilderWrksheet.Cells[processBuilderRowId, 8].Value = "Record Updates";
                    xlProcessBuilderWrksheet.Cells[processBuilderRowId, 9].Value = "Record Deletes";
                    processBuilderRowId++;

                    List<FlowProcess> processBuilders = declarativeTypeToName["Process Builder"];
                    foreach (FlowProcess fp in processBuilders)
                    {
                        xlProcessBuilderWrksheet.Cells[processBuilderRowId, 1].Value = fp.processName;
                        xlProcessBuilderWrksheet.Cells[processBuilderRowId, 2].Value = fp.processLabel;
                        xlProcessBuilderWrksheet.Cells[processBuilderRowId, 3].Value = fp.processType;
                        xlProcessBuilderWrksheet.Cells[processBuilderRowId, 4].Value = fp.status;
                        xlProcessBuilderWrksheet.Cells[processBuilderRowId, 5].Value = fp.apiVersion;
                        xlProcessBuilderWrksheet.Cells[processBuilderRowId, 6].Value = fp.runInMode;

                        if (fp.recordCreates.Count > 0)
                        {
                            String dictionaryKeys = "";

                            foreach (String key in fp.recordCreates.Keys)
                            {
                                dictionaryKeys = dictionaryKeys + key + ",";
                            }

                            xlProcessBuilderWrksheet.Cells[processBuilderRowId, 7].Value = dictionaryKeys;
                        }

                        if (fp.recordUpdates.Count > 0)
                        {
                            String dictionaryKeys = "";

                            foreach (String key in fp.recordCreates.Keys)
                            {
                                dictionaryKeys = dictionaryKeys + key + ",";
                            }

                            xlProcessBuilderWrksheet.Cells[processBuilderRowId, 8].Value = dictionaryKeys;
                        }

                        if (fp.recordDeletes.Count > 0)
                        {
                            String dictionaryKeys = "";

                            foreach (String key in fp.recordCreates.Keys)
                            {
                                dictionaryKeys = dictionaryKeys + key + ",";
                            }

                            xlProcessBuilderWrksheet.Cells[processBuilderRowId, 9].Value = dictionaryKeys;
                        }

                        processBuilderRowId++;
                    }


                    // AutoLaunchedFlow
                    Microsoft.Office.Interop.Excel.Worksheet xlAutolaunchedFlowWrksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                                        (System.Reflection.Missing.Value,
                                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                                        System.Reflection.Missing.Value,
                                                                                        System.Reflection.Missing.Value);
                    xlAutolaunchedFlowWrksheet.Name = "AutoLaunchedFlow";

                    xlAutolaunchedFlowWrksheet.Cells[flowRowId, 1].Value = "FlowName";
                    xlAutolaunchedFlowWrksheet.Cells[flowRowId, 2].Value = "FlowLabel";
                    xlAutolaunchedFlowWrksheet.Cells[flowRowId, 3].Value = "FlowType";
                    xlAutolaunchedFlowWrksheet.Cells[flowRowId, 4].Value = "Status";
                    xlAutolaunchedFlowWrksheet.Cells[flowRowId, 5].Value = "API Version";
                    xlAutolaunchedFlowWrksheet.Cells[flowRowId, 6].Value = "Run In Mode";
                    xlAutolaunchedFlowWrksheet.Cells[flowRowId, 7].Value = "Record Creates";
                    xlAutolaunchedFlowWrksheet.Cells[flowRowId, 8].Value = "Record Updates";
                    xlAutolaunchedFlowWrksheet.Cells[flowRowId, 9].Value = "Record Deletes";
                    flowRowId++;

                    List<FlowProcess> autoLaunchedFlows = declarativeTypeToName["AutoLaunchedFlow"];
                    foreach (FlowProcess fp in autoLaunchedFlows)
                    {
                        xlAutolaunchedFlowWrksheet.Cells[flowRowId, 1].Value = fp.processName;
                        xlAutolaunchedFlowWrksheet.Cells[flowRowId, 2].Value = fp.processLabel;
                        xlAutolaunchedFlowWrksheet.Cells[flowRowId, 3].Value = fp.processType;
                        xlAutolaunchedFlowWrksheet.Cells[flowRowId, 4].Value = fp.status;
                        xlAutolaunchedFlowWrksheet.Cells[flowRowId, 5].Value = fp.apiVersion;
                        xlAutolaunchedFlowWrksheet.Cells[flowRowId, 6].Value = fp.runInMode;

                        if (fp.recordCreates.Count > 0)
                        {
                            String dictionaryKeys = "";

                            foreach (String key in fp.recordCreates.Keys)
                            {
                                dictionaryKeys = dictionaryKeys + key + ",";
                            }

                            xlAutolaunchedFlowWrksheet.Cells[flowRowId, 7].Value = dictionaryKeys;
                        }

                        if (fp.recordUpdates.Count > 0)
                        {
                            String dictionaryKeys = "";

                            foreach (String key in fp.recordCreates.Keys)
                            {
                                dictionaryKeys = dictionaryKeys + key + ",";
                            }

                            xlAutolaunchedFlowWrksheet.Cells[flowRowId, 8].Value = dictionaryKeys;
                        }

                        if (fp.recordDeletes.Count > 0)
                        {
                            String dictionaryKeys = "";

                            foreach (String key in fp.recordCreates.Keys)
                            {
                                dictionaryKeys = dictionaryKeys + key + ",";
                            }

                            xlAutolaunchedFlowWrksheet.Cells[flowRowId, 9].Value = dictionaryKeys;
                        }

                        flowRowId++;
                    }
                }
                else
                {
                    MessageBox.Show("Please make sure the \"Flows\" folder is in the path " + this.tbProjectFolder.Text + " or choose another path");
                }
            }
            else
            {
                
            }
        }

        private void runWorkflowAutomationReport()
        {
            if (this.tbProjectFolder.Text != "")
            {
                if (Directory.Exists(this.tbProjectFolder.Text + "\\Workflows"))
                {
                    // Key = sObject, Values = FlowProcess
                    Dictionary<String, List<FlowProcess>> flowProcessList = new Dictionary<String, List<FlowProcess>>();

                    String[] fileNames = Directory.GetFiles(this.tbProjectFolder.Text + "\\Workflows");

                    foreach (String fileName in fileNames)
                    {
                        String[] filePathSplit = fileName.Split('\\');
                        String objectName = filePathSplit[filePathSplit.Length - 1].Split('.')[0];

                        XmlDocument xd = new XmlDocument();
                        xd.Load(fileName);


                        XmlNodeList fieldUpdates = xd.GetElementsByTagName("fieldUpdates");
                        Dictionary<String, String> fieldUpdateNameToObject = new Dictionary<String, String>();

                        foreach (XmlElement elem in fieldUpdates)
                        {
                            fieldUpdateNameToObject.Add(elem.ChildNodes[0].InnerText, objectName);
                        }

                        XmlNodeList workflowRules = xd.GetElementsByTagName("rules");

                        foreach (XmlElement elem in workflowRules)
                        {
                            FlowProcess fp = new FlowProcess();
                            fp.objectName = objectName;

                            if (elem.ChildNodes.Count > 0)
                            {
                                foreach (XmlElement ce1 in elem.ChildNodes)
                                {
                                    if (ce1.Name == "fullName")
                                    {
                                        fp.processName = ce1.InnerText;
                                    }
                                    else if (ce1.Name == "active")
                                    {
                                        fp.status = ce1.InnerText;
                                    }
                                    else if (ce1.Name == "triggerType")
                                    {
                                        fp.triggerType = ce1.InnerText;
                                    }

                                    XmlNodeList actions = elem.GetElementsByTagName("actions");

                                    foreach (XmlElement actionElem in actions)
                                    {
                                        if (actionElem.ChildNodes[1].InnerText == "FieldUpdate")
                                        {
                                            if (fp.triggerType == "onCreateOnly")
                                            {
                                                if (fp.recordCreates.ContainsKey(objectName))
                                                {
                                                    fp.recordCreates[objectName].Add(actionElem.ChildNodes[0].InnerText);
                                                }
                                                else
                                                {
                                                    fp.recordCreates.Add(objectName, new List<String> { actionElem.ChildNodes[0].InnerText });
                                                }
                                            }
                                            else if (fp.triggerType == "onCreateOrTriggeringUpdate")
                                            {
                                                if (fp.recordCreates.ContainsKey(objectName))
                                                {
                                                    fp.recordCreates[objectName].Add(actionElem.ChildNodes[0].InnerText);
                                                }
                                                else
                                                {
                                                    fp.recordCreates.Add(objectName, new List<String> { actionElem.ChildNodes[0].InnerText });
                                                }

                                                if (fp.recordUpdates.ContainsKey(objectName))
                                                {
                                                    fp.recordUpdates[objectName].Add(actionElem.ChildNodes[0].InnerText);
                                                }
                                                else
                                                {
                                                    fp.recordUpdates.Add(objectName, new List<String> { actionElem.ChildNodes[0].InnerText });
                                                }
                                            }
                                            else if (fp.triggerType == "onAllChanges")
                                            {
                                                if (fp.recordCreates.ContainsKey(objectName))
                                                {
                                                    fp.recordCreates[objectName].Add(actionElem.ChildNodes[0].InnerText);
                                                }
                                                else
                                                {
                                                    fp.recordCreates.Add(objectName, new List<String> { actionElem.ChildNodes[0].InnerText });
                                                }

                                                if (fp.recordUpdates.ContainsKey(objectName))
                                                {
                                                    fp.recordUpdates[objectName].Add(actionElem.ChildNodes[0].InnerText);
                                                }
                                                else
                                                {
                                                    fp.recordUpdates.Add(objectName, new List<String> { actionElem.ChildNodes[0].InnerText });
                                                }
                                            }
                                        }
                                    }
                                }

                                if (flowProcessList.ContainsKey(objectName))
                                {
                                    flowProcessList[objectName].Add(fp);
                                }
                                else
                                {
                                    flowProcessList.Add(objectName, new List<FlowProcess> { fp });
                                }
                            }
                        }
                    }

                    // Now write the values to an Excel file
                    Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
                    xlapp.Visible = true;

                    Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlapp.Workbooks.Add();

                    //Int32 workflowRuleRowId = 1;
                    Int32 flowRowId = 1;

                    // AutoLaunchedFlow
                    Microsoft.Office.Interop.Excel.Worksheet xlWorkflowWrksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                                        (System.Reflection.Missing.Value,
                                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                                        System.Reflection.Missing.Value,
                                                                                        System.Reflection.Missing.Value);
                    xlWorkflowWrksheet.Name = "Workflow Rules";

                    xlWorkflowWrksheet.Cells[flowRowId, 1].Value = "Workflow Rule Name";
                    xlWorkflowWrksheet.Cells[flowRowId, 2].Value = "Object";
                    xlWorkflowWrksheet.Cells[flowRowId, 3].Value = "Trigger Type";
                    xlWorkflowWrksheet.Cells[flowRowId, 4].Value = "Is Active";
                    xlWorkflowWrksheet.Cells[flowRowId, 5].Value = "Field Updates on Record Create";
                    xlWorkflowWrksheet.Cells[flowRowId, 6].Value = "Field Updates on Record Updates";
                    flowRowId++;

                    foreach (String objectName in flowProcessList.Keys)
                    {
                        foreach (FlowProcess fp in flowProcessList[objectName])
                        {
                            if (fp.status == "FALSE") continue;

                            xlWorkflowWrksheet.Cells[flowRowId, 1].Value = fp.processName;
                            xlWorkflowWrksheet.Cells[flowRowId, 2].Value = fp.objectName;
                            xlWorkflowWrksheet.Cells[flowRowId, 3].Value = fp.triggerType;
                            xlWorkflowWrksheet.Cells[flowRowId, 4].Value = fp.status;

                            if (fp.recordCreates.Count > 0)
                            {
                                String dictionaryFieldUpdates = "";

                                foreach (String key in fp.recordCreates.Keys)
                                {
                                    foreach (String fu in fp.recordCreates[key])
                                    {
                                        dictionaryFieldUpdates = dictionaryFieldUpdates + fu + ",";
                                    }
                                }

                                xlWorkflowWrksheet.Cells[flowRowId, 5].Value = dictionaryFieldUpdates;
                            }

                            if (fp.recordUpdates.Count > 0)
                            {
                                String dictionaryFieldUpdates = "";

                                foreach (String key in fp.recordCreates.Keys)
                                {
                                    foreach (String fu in fp.recordUpdates[key])
                                    {
                                        dictionaryFieldUpdates = dictionaryFieldUpdates + fu + ",";
                                    }
                                }

                                xlWorkflowWrksheet.Cells[flowRowId, 6].Value = dictionaryFieldUpdates;
                            }

                            flowRowId++;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please make sure the \"Workflows\" folder is in the path " + this.tbProjectFolder.Text + " or choose another path");
                }
            }
        }


        public void writeDataToExcelSheet(Microsoft.Office.Interop.Excel.Worksheet xlWorksheet,
                                          Int32 rowNumber,
                                          Int32 colNumber,
                                          String value)
        {
            xlWorksheet.Cells[rowNumber, colNumber].Value = value;
        }

        public void formatExcelRange(Microsoft.Office.Interop.Excel.Worksheet xlWorksheet,
                                             Int32 startRowNumber,
                                             Int32 endRowNumber,
                                             Int32 startColNumber,
                                             Int32 endColNumber,
                                             Int32 fontSize,
                                             Int32 fontColorRed,
                                             Int32 fontColorGreen,
                                             Int32 fontColorBlue,
                                             Int32 interiorColorRed,
                                             Int32 interiorColorGreen,
                                             Int32 interiorColorBlue,
                                             Boolean boldText,
                                             Boolean italicText,
                                             String fieldValues)
        {
            Microsoft.Office.Interop.Excel.Range rng;
            rng = xlWorksheet.Range[xlWorksheet.Cells[startRowNumber, startColNumber], xlWorksheet.Cells[endRowNumber, endColNumber]];
            rng.Font.Bold = boldText;
            rng.Font.Italic = italicText;
            rng.Font.Size = fontSize;
            rng.Font.Color = System.Drawing.Color.FromArgb(fontColorRed, fontColorGreen, fontColorBlue);

            if (fieldValues.ToLower() == "true")
            {
                rng.Interior.Color = System.Drawing.Color.FromArgb(220, 230, 241);
            }
            else if (fieldValues.ToLower() == "false")
            {
                rng.Interior.Color = System.Drawing.Color.FromArgb(250, 191, 143);
            }
            else
            {
                rng.Interior.Color = System.Drawing.Color.FromArgb(interiorColorRed, interiorColorGreen, interiorColorBlue);
            }
        }

        private void btnAddLoggingToMethod_Click(object sender, EventArgs e)
        {
            foreach (String fileName in fileNames)
            {
                String[] fileNameParts = fileName.Split('\\');

                StreamReader sr = new StreamReader(fileName);
                String fileContents = sr.ReadToEnd();
                sr.Close();

                StreamWriter sw = new StreamWriter(fileName);

                String currentMethodName = "";
                String previousMethodName = "";

                Boolean insideMultiLineComment = false;
                Boolean insideInlineComment = false;

                Char[] fileCharArray = fileContents.ToCharArray();

                // provides the length of the method name if the checkIfMethod returns a value so that the loop through the characters
                // does not continue writing the same material
                Int32 j = 0;

                for (Int32 i = 0; i < fileCharArray.Length - 1; i++)
                {
                    if (fileCharArray[i] == '/' && fileCharArray[i + 1] == '*')
                    {
                        insideMultiLineComment = true;
                    }
                    else if (fileCharArray[i] == '*' && fileCharArray[i + 1] == '/')
                    {
                        insideMultiLineComment = false;
                    }
                    else if (fileCharArray[i] == '/' && fileCharArray[i + 1] == '/')
                    {
                        insideInlineComment = true;
                    }
                    else if (fileCharArray[i] == '\n' && insideInlineComment == true)
                    {
                        insideInlineComment = false;
                    }

                    // Get whether this is a method
                    if (j == 0)
                    {
                        currentMethodName = checkIfMethod(fileCharArray, i);
                    }

                    if (currentMethodName != "" && insideMultiLineComment == false && insideInlineComment == false)
                    {
                        j = currentMethodName.Length; // Decrement this
                        previousMethodName = currentMethodName;
                        currentMethodName = "";
                    }

                    // While j is greater than 0, write the method out into the file character by character
                    // When a { is hit, then write the { and then write the line Console.WriteLine(xxx)
                    if (j > 0 && fileCharArray[i] == '{')
                    {
                        sw.Write(fileCharArray[i].ToString());
                        j--;
                    }
                    else if (j > 0 && fileCharArray[i] != '{')
                    {
                        sw.Write(fileCharArray[i].ToString());
                        j--;
                    }
                    else
                    {
                        sw.Write(fileCharArray[i].ToString());
                    }
                }

                sw.Close();
            }
        }

        public String checkIfMethod(Char[] fileCharArray, Int32 arrayPosition)
        {
            String methodName = "";

            // protected inernal - 17
            // private protected - 17
            // protected - 9
            // internal - 8
            // private - 7
            // public - 6

            String startValue = "";
            if (arrayPosition + 17 > fileCharArray.Length)
            {
                for (Int32 i = arrayPosition; i < fileCharArray.Length; i++)
                {
                    startValue = startValue + fileCharArray[i].ToString();
                }
            }
            else
            {
                for (Int32 i = arrayPosition; i < arrayPosition + 17; i++)
                {
                    startValue = startValue + fileCharArray[i].ToString();
                }
            }

            Boolean methodStart = false;
            if (startValue == "protected inernal")
            {
                methodStart = true;
            }
            else if (startValue == "private protected")
            {
                methodStart = true;
            }
            else if (startValue.StartsWith("protected"))
            {
                methodStart = true;
            }
            else if (startValue.StartsWith("internal"))
            {
                methodStart = true;
            }
            else if (startValue.StartsWith("private"))
            {
                methodStart = true;
            }
            else if (startValue.StartsWith("public"))
            {
                methodStart = true;
            }

            Int32 parenthesesCount = 0;
            Int32 braceCount = 0;
            if (methodStart == true)
            {
                for (Int32 i = arrayPosition; i < fileCharArray.Length - 1; i++)
                {
                    if (fileCharArray[i] == '(')
                    {
                        parenthesesCount++;
                    }
                    else if (fileCharArray[i] == ')')
                    {
                        parenthesesCount++;
                    }
                    else if (fileCharArray[i] == '{')
                    {
                        methodName += fileCharArray[i].ToString();
                        braceCount++;
                        break;
                    }
                    else if (fileCharArray[i] == '}')
                    {
                        methodName += fileCharArray[i].ToString();
                        break;
                    }
                    else if (fileCharArray[i] == ';')
                    {
                        methodName += fileCharArray[i].ToString();
                        break;
                    }

                    methodName += fileCharArray[i].ToString();
                }
            }

            if (parenthesesCount == 2 && braceCount == 1)
            {
                // Do nothing
                methodName = methodName.Replace("\n", "");
                methodName = methodName.Replace("\r", "");
                methodName = methodName.Replace("  ", " ");
                methodName = methodName.Replace("  ", " ");
                methodName = methodName.Replace("  ", " ");
                methodName = methodName.Replace("  ", " ");
                methodName = methodName.Replace("  ", " ");
            }
            else
            {
                methodName = "";
            }

            return methodName;
        }

        private void tbProjectFolder_DoubleClick(object sender, EventArgs e)
        {
            this.tbProjectFolder.Text = UtilityClass.folderBrowserSelectPath("Select Project Folder", false, FolderEnum.ReadFrom);
        }


        public class FlowProcess
        {
            public String processName;
            public String processLabel;
            public String processType;
            public String apiVersion;
            public String status;
            public String runInMode;
            public String triggerType;
            public String objectName;
            // Key = object, values = the element names in the flows which initiate the DMLs 
            public Dictionary<String, List<String>> recordCreates;
            public Dictionary<String, List<String>> recordUpdates;
            public Dictionary<String, List<String>> recordDeletes;
            public Dictionary<String, List<String>> fieldUpdates;

            public FlowProcess()
            {
                processName = "";
                processLabel = "";
                processType = "";
                apiVersion = "";
                status = "";
                runInMode = "";
                triggerType = "";
                objectName = "";

                recordCreates = new Dictionary<String, List<String>>();
                recordUpdates = new Dictionary<String, List<String>>();
                recordDeletes = new Dictionary<String, List<String>>();
                fieldUpdates = new Dictionary<string, List<string>>();
            }
        }

    }
}
