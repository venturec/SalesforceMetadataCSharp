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
using System.Xml;
using static SalesforceMetadata.AutomationReporter;

namespace SalesforceMetadata
{
    public partial class MetadataToolingReportForm : Form
    {
        private SalesforceCredentials sc;

        public MetadataToolingReportForm()
        {
            InitializeComponent();
            sc = new SalesforceCredentials();
            populateCredentialsFile();
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

        private void tbMetadataFolderLocation_DoubleClick(object sender, EventArgs e)
        {
            String selectedPath = UtilityClass.folderBrowserSelectPath("Select Directory to read the Metadata from",
                                                                                      true,
                                                                                      FolderEnum.ReadFrom,
                                                                                      Properties.Settings.Default.MetadataToolingLastReadFrom);

            if (selectedPath != "")
            {
                this.tbMetadataFolderLocation.Text = selectedPath;
                Properties.Settings.Default.MetadataToolingLastReadFrom = selectedPath;
                Properties.Settings.Default.Save();
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            if (this.tbMetadataFolderLocation.Text == "")
            {
                MessageBox.Show("Please choose a location to read the metadata from for the proper reporting");
            }
            else if (String.IsNullOrEmpty(this.cmbUserName.Text))
            {
                MessageBox.Show("Please select a user in the picklist before continuing");
            }
            else
            {
                try
                {
                    this.sc.salesforceToolingLogin(UtilityClass.REQUESTINGORG.FROMORG, this.cmbUserName.Text);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                    return;
                }

                HashSet<String> selectedItems = new HashSet<string>();
                foreach (String itm in this.lbToolingObjects.CheckedItems)
                {
                    selectedItems.Add(itm);
                }

                // Run a thread for obtaining the custom objects and classes
                Action act = () => buildToolingReport(selectedItems, this);
                Task tsk = Task.Run(act);
            }
        }

        private void buildToolingReport(HashSet<String> selectedItems, MetadataToolingReportForm toolingForm)
        {
            // We are going to retrieve these in this method so adding a filter to bypass in case another method requests to retrieve these items, causing a duplicate retrieval
            HashSet<String> bypassObjects = new HashSet<String> { "ApexClass", "CustomObject" };

            DateTime dt = DateTime.Now;
            String processingMsg1 = "Tooling Report Started at: " + dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString() + " " + dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString() + "." + dt.Millisecond.ToString() + Environment.NewLine + Environment.NewLine;
            var threadParameters1 = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg1, toolingForm); });
            var thread1 = new System.Threading.Thread(threadParameters1);
            thread1.Start();
            while (thread1.ThreadState == System.Threading.ThreadState.Running)
            {
                // do nothing. Just want for the thread to complete
            }

            Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlapp.Workbooks.Add();
            xlapp.Visible = true;

            // Key = ObjectName.FieldName, Value = Label, Type

            // Salesforce does not return custom object and custom field api names with the __c so we can't match easily to what is in the metadata
            // Some orgs have duplicate Case Type fields: one of them is the standard one, and the other is a custom one, but the Tooling API returns
            // both with the same DeveloperName - Type and the custom one does not have the __c

            Dictionary<String, List<String>> objectFieldNameToLabel = new Dictionary<String, List<String>>();
            parseObjectFiles(objectFieldNameToLabel, toolingForm);

            Dictionary<String, ToolingApiHelper.WorkflowRule> workflowRules = new Dictionary<String, ToolingApiHelper.WorkflowRule>();
            Dictionary<String, ToolingApiHelper.WorkflowFieldUpdate> workflowFieldUpdatesByFullName = new Dictionary<String, ToolingApiHelper.WorkflowFieldUpdate>();
            Dictionary<String, ToolingApiHelper.WorkflowFieldUpdate> workflowFieldUpdatesByName = new Dictionary<String, ToolingApiHelper.WorkflowFieldUpdate>();
            //Dictionary<String, WorkflowAlert> workflowAlerts = new Dictionary<String, WorkflowAlert>();
            //Dictionary<String, WorkflowOutboundMessage> workflowOutboundMsgs = new Dictionary<String, WorkflowOutboundMessage>();
            parseWorkflowRules(workflowRules, workflowFieldUpdatesByFullName, workflowFieldUpdatesByName, this);

            Dictionary<String, String> customObjIdToName = new Dictionary<String, String>();
            Dictionary<String, String> classIdToClassName = new Dictionary<String, String>();

            if (toolingForm.cbDefaultCoreObjects.Checked == true || selectedItems.Contains("CustomObject"))
            {
                getCustomObject(xlWorkbook, customObjIdToName, this);
                getCustomField(xlWorkbook, customObjIdToName, objectFieldNameToLabel, this);
            }

            if (toolingForm.cbDefaultCoreObjects.Checked == true || selectedItems.Contains("ApexClass"))
            {
                getApexClass(xlWorkbook, classIdToClassName, this);
            }

            // Run a new Thread for the rest of the objects
            foreach (String objType in selectedItems)
            {
                if (!bypassObjects.Contains(objType))
                {
                    getToolingObject(objType,
                                     xlWorkbook,
                                     customObjIdToName,
                                     classIdToClassName,
                                     workflowRules,
                                     workflowFieldUpdatesByName,
                                     toolingForm);
                }
            }

            dt = DateTime.Now;
            String processingMsg2 = "Tooling Retrieval Completed at: " + dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString() + " " + dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString() + "." + dt.Millisecond.ToString() + Environment.NewLine + Environment.NewLine;
            var threadParameters2 = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg2, toolingForm); });
            var thread2 = new System.Threading.Thread(threadParameters2);
            thread2.Start();
            while (thread2.ThreadState == System.Threading.ThreadState.Running)
            {
                // do nothing. Just want for the thread to complete
            }
        }


        private void getCustomObject(Microsoft.Office.Interop.Excel.Workbook xlWorkbook, 
                                     Dictionary<String, String> customObjIdToName,
                                     MetadataToolingReportForm toolingForm)
        {
            DateTime dt = DateTime.Now;
            String processingMsg1 = "    CustomObject: Tooling Retrieval Started at: " + dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString() + " " + dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString() + "." + dt.Millisecond.ToString() + Environment.NewLine;
            var threadParameters1 = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg1, toolingForm); });
            var thread1 = new System.Threading.Thread(threadParameters1);
            thread1.Start();
            while (thread1.ThreadState == System.Threading.ThreadState.Running)
            {
                // do nothing. Just want for the thread to complete
            }

            String queryString = ToolingApiHelper.CustomObjectQuery();
            ToolingApiHelper.customObjectToExcel(xlWorkbook, sc, queryString, UtilityClass.REQUESTINGORG.FROMORG, customObjIdToName);

            dt = DateTime.Now;
            String processingMsg2 = "    CustomObject: Tooling Retrieval Completed at: " + dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString() + " " + dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString() + "." + dt.Millisecond.ToString() + Environment.NewLine + Environment.NewLine;
            var threadParameters2 = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg2, toolingForm); });
            var thread2 = new System.Threading.Thread(threadParameters2);
            thread2.Start();
            while (thread2.ThreadState == System.Threading.ThreadState.Running)
            {
                // do nothing. Just want for the thread to complete
            }
        }

        private void getCustomField(Microsoft.Office.Interop.Excel.Workbook xlWorkbook, 
                                    Dictionary<String, String> customObjIdToName, 
                                    Dictionary<String, List<String>> objectFieldNameToLabel,
                                    MetadataToolingReportForm toolingForm)
        {
            DateTime dt = DateTime.Now;
            String processingMsg1 = "    CustomField: Tooling Retrieval Started at: " + dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString() + " " + dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString() + "." + dt.Millisecond.ToString() + Environment.NewLine;
            var threadParameters1 = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg1, toolingForm); });
            var thread1 = new System.Threading.Thread(threadParameters1);
            thread1.Start();
            while (thread1.ThreadState == System.Threading.ThreadState.Running)
            {
                // do nothing. Just want for the thread to complete
            }

            String queryString = ToolingApiHelper.CustomFieldQuery();
            ToolingApiHelper.customFieldToExcel(xlWorkbook, sc, queryString, UtilityClass.REQUESTINGORG.FROMORG, customObjIdToName, objectFieldNameToLabel);

            dt = DateTime.Now;
            String processingMsg2 = "    CustomField: Tooling Retrieval Completed at: " + dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString() + " " + dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString() + "." + dt.Millisecond.ToString() + Environment.NewLine + Environment.NewLine;
            var threadParameters2 = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg2, toolingForm); });
            var thread2 = new System.Threading.Thread(threadParameters2);
            thread2.Start();
            while (thread2.ThreadState == System.Threading.ThreadState.Running)
            {
                // do nothing. Just want for the thread to complete
            }
        }

        private void getApexClass(Microsoft.Office.Interop.Excel.Workbook xlWorkbook, 
                                  Dictionary<String, String> classIdToClassName,
                                  MetadataToolingReportForm toolingForm)
        {
            DateTime dt = DateTime.Now;
            String processingMsg1 = "    ApexClass: Tooling Retrieval Started at: " + dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString() + " " + dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString() + "." + dt.Millisecond.ToString() + Environment.NewLine;
            var threadParameters1 = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg1, toolingForm); });
            var thread1 = new System.Threading.Thread(threadParameters1);
            thread1.Start();
            while (thread1.ThreadState == System.Threading.ThreadState.Running)
            {
                // do nothing. Just want for the thread to complete
            }

            String queryString = ToolingApiHelper.ApexClassQuery("");
            ToolingApiHelper.getApexClasses(sc, queryString, UtilityClass.REQUESTINGORG.FROMORG, classIdToClassName);
            ToolingApiHelper.apexClassToExcel(xlWorkbook, sc, queryString, UtilityClass.REQUESTINGORG.FROMORG, classIdToClassName, toolingForm.cbRetrieveApexClassCoverage.Checked);

            dt = DateTime.Now;
            String processingMsg2 = "    ApexClass: Tooling Retrieval Completed at: " + dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString() + " " + dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString() + "." + dt.Millisecond.ToString() + Environment.NewLine + Environment.NewLine;
            var threadParameters2 = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg2, toolingForm); });
            var thread2 = new System.Threading.Thread(threadParameters2);
            thread2.Start();
            while (thread2.ThreadState == System.Threading.ThreadState.Running)
            {
                // do nothing. Just want for the thread to complete
            }
        }

        private void getToolingObject(String toolingObject, 
                                      Microsoft.Office.Interop.Excel.Workbook xlWorkbook, 
                                      Dictionary<String, String> customObjIdToName, 
                                      Dictionary<String, String> classIdToClassName,
                                      Dictionary<String, ToolingApiHelper.WorkflowRule> workflowRules,
                                      Dictionary<String, ToolingApiHelper.WorkflowFieldUpdate> workflowFieldUpdatesByName,
                                      MetadataToolingReportForm toolingForm)
        {
            DateTime dt = DateTime.Now;
            String processingMsg1 = "    " + toolingObject + ": Tooling Retrieval Started at: " + dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString() + " " + dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString() + "." + dt.Millisecond.ToString() + Environment.NewLine;
            var threadParameters1 = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg1, toolingForm); });
            var thread1 = new System.Threading.Thread(threadParameters1);
            thread1.Start();
            while (thread1.ThreadState == System.Threading.ThreadState.Running)
            {
                // do nothing. Just want for the thread to complete
            }

            String queryString = "";

            if (toolingObject == "ApexComponent")
            {
                queryString = ToolingApiHelper.ApexComponentQuery();
                ToolingApiHelper.apexComponentToExcel(xlWorkbook, sc, queryString, UtilityClass.REQUESTINGORG.FROMORG, classIdToClassName);
            }
            //else if (toolingObject == "ApexEmailNotification")
            //{
            //    query = ToolingApiHelper.ApexEmailNotificationQuery();
            //    ToolingApiHelper.apexEmailNotificationToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG);
            //}
            else if (toolingObject == "ApexPage")
            {
                queryString = ToolingApiHelper.ApexPageQuery();
                ToolingApiHelper.apexPageToExcel(xlWorkbook, sc, queryString, UtilityClass.REQUESTINGORG.FROMORG, classIdToClassName);
            }
            else if (toolingObject == "ApexTrigger")
            {
                if (customObjIdToName.Count == 0)
                {
                    getCustomObject(xlWorkbook, customObjIdToName, toolingForm);
                }

                if (classIdToClassName.Count == 0)
                {
                    getApexClass(xlWorkbook, customObjIdToName, toolingForm);
                }

                queryString = ToolingApiHelper.ApexTriggerQuery("");
                ToolingApiHelper.apexTriggerToExcel(xlWorkbook, sc, queryString, UtilityClass.REQUESTINGORG.FROMORG, classIdToClassName, customObjIdToName, toolingForm.cbRetrieveApexClassCoverage.Checked);
            }
            else if (toolingObject == "AuraDefinitionBundle")
            {
                queryString = ToolingApiHelper.AuraDefinitionBundleQuery();
                ToolingApiHelper.auraDefinitionBundleToExcel(xlWorkbook, sc, queryString, UtilityClass.REQUESTINGORG.FROMORG);
            }
            else if (toolingObject == "CompactLayout")
            {
                queryString = ToolingApiHelper.CompactLayoutQuery();
                ToolingApiHelper.compactLayoutToExcel(xlWorkbook, sc, queryString, UtilityClass.REQUESTINGORG.FROMORG);
            }
            else if (toolingObject == "CustomApplication")
            {
                queryString = ToolingApiHelper.CustomApplicationQuery();
                ToolingApiHelper.customApplicationToExcel(xlWorkbook, sc, queryString, UtilityClass.REQUESTINGORG.FROMORG);
            }
            else if (toolingObject == "CustomTab")
            {
                queryString = ToolingApiHelper.CustomTabQuery();
                ToolingApiHelper.customTabToExcel(xlWorkbook, sc, queryString, UtilityClass.REQUESTINGORG.FROMORG);
            }
            else if (toolingObject == "EmailTemplate")
            {
                queryString = ToolingApiHelper.EmailTemplateQuery();
                ToolingApiHelper.emailTemplateToExcel(xlWorkbook, sc, queryString, UtilityClass.REQUESTINGORG.FROMORG);
            }
            else if (toolingObject == "FieldSet")
            {
                queryString = ToolingApiHelper.FieldSetQuery();
                ToolingApiHelper.fieldSetToExcel(xlWorkbook, sc, queryString, UtilityClass.REQUESTINGORG.FROMORG);
            }
            else if (toolingObject == "FlexiPage")
            {
                if (customObjIdToName.Count == 0)
                {
                    getCustomObject(xlWorkbook, customObjIdToName, toolingForm);
                }

                queryString = ToolingApiHelper.FlexiPageQuery("");
                ToolingApiHelper.flexiPageToExcel(xlWorkbook, sc, queryString, UtilityClass.REQUESTINGORG.FROMORG, customObjIdToName);
            }
            else if (toolingObject == "Flow")
            {
                queryString = ToolingApiHelper.FlowQuery();
                ToolingApiHelper.flowToExcel(xlWorkbook, sc, queryString, UtilityClass.REQUESTINGORG.FROMORG);
            }
            else if (toolingObject == "GlobalValueSet")
            {
                queryString = ToolingApiHelper.GlobalValueSetQuery();
                ToolingApiHelper.globalValueSetToExcel(xlWorkbook, sc, queryString, UtilityClass.REQUESTINGORG.FROMORG);
            }
            else if (toolingObject == "Group")
            {
                queryString = ToolingApiHelper.GroupQuery();
                ToolingApiHelper.groupToExcel(xlWorkbook, sc, queryString, UtilityClass.REQUESTINGORG.FROMORG);
            }
            else if (toolingObject == "Layout")
            {
                if (customObjIdToName.Count == 0)
                {
                    getCustomObject(xlWorkbook, customObjIdToName, toolingForm);
                }

                queryString = ToolingApiHelper.LayoutQuery("");
                ToolingApiHelper.layoutToExcel(xlWorkbook, sc, queryString, UtilityClass.REQUESTINGORG.FROMORG, customObjIdToName);
            }
            else if (toolingObject == "LightningComponentBundle")
            {
                queryString = ToolingApiHelper.LightningComponentBundleQuery();
                ToolingApiHelper.lwcToExcel(xlWorkbook, sc, queryString, UtilityClass.REQUESTINGORG.FROMORG);
            }
            else if (toolingObject == "LightningComponentBundle")
            {
                queryString = ToolingApiHelper.LightningComponentBundleQuery();
                ToolingApiHelper.lwcToExcel(xlWorkbook, sc, queryString, UtilityClass.REQUESTINGORG.FROMORG);
            }
            else if (toolingObject == "PermissionSet")
            {
                queryString = ToolingApiHelper.PermissionSetQuery();
                ToolingApiHelper.permissionSetToExcel(xlWorkbook, sc, queryString, UtilityClass.REQUESTINGORG.FROMORG);
            }
            else if (toolingObject == "PermissionSetAssignment")
            {
                //queryString = ToolingApiHelper.PermissionSetAssignmentQuery();
                //ToolingApiHelper.permissionSetAssignmentToExcel(xlWorkbook, queryString, UtilityClass.REQUESTINGORG.FROMORG);
            }
            else if (toolingObject == "PermissionSetGroup")
            {
                //queryString = ToolingApiHelper.PermissionSetGroupQuery();
                //ToolingApiHelper.permissionSetGroupToExcel(xlWorkbook, queryString, UtilityClass.REQUESTINGORG.FROMORG);
            }
            else if (toolingObject == "Profile")
            {
                queryString = ToolingApiHelper.ProfileQuery();
                ToolingApiHelper.profileToExcel(xlWorkbook, sc, queryString, UtilityClass.REQUESTINGORG.FROMORG);
            }
            else if (toolingObject == "QuickActionDefinition")
            {
                queryString = ToolingApiHelper.QuickActionDefinitionQuery();
                ToolingApiHelper.quickActionDefinitionToExcel(xlWorkbook, sc, queryString, UtilityClass.REQUESTINGORG.FROMORG);
            }
            else if (toolingObject == "RecordType")
            {
                queryString = ToolingApiHelper.RecordTypeQuery();
                ToolingApiHelper.recordTypesToExcel(xlWorkbook, sc, queryString, UtilityClass.REQUESTINGORG.FROMORG);
            }
            else if (toolingObject == "TabDefinition")
            {
                queryString = ToolingApiHelper.TabDefinitionQuery();
                ToolingApiHelper.tabDefinitionToExcel(xlWorkbook, sc, queryString, UtilityClass.REQUESTINGORG.FROMORG);
            }
            else if (toolingObject == "ValidationRule")
            {
                if (customObjIdToName.Count == 0)
                {
                    getCustomObject(xlWorkbook, customObjIdToName, toolingForm);
                }

                queryString = ToolingApiHelper.ValidationRuleQuery("", "");
                ToolingApiHelper.validationRuleToExcel(xlWorkbook, sc, queryString, UtilityClass.REQUESTINGORG.FROMORG, customObjIdToName);
            }
            else if (toolingObject == "WorkflowRule")
            {
                if (customObjIdToName.Count == 0)
                {
                    getCustomObject(xlWorkbook, customObjIdToName, toolingForm);
                }

                queryString = ToolingApiHelper.WorkflowRuleQuery();
                ToolingApiHelper.workflowRuleToExcel(xlWorkbook, sc, queryString, UtilityClass.REQUESTINGORG.FROMORG, customObjIdToName);
            }
            else if (toolingObject == "WorkflowAlert")
            {
                queryString = ToolingApiHelper.WorkflowAlertQuery();
                ToolingApiHelper.workflowAlertToExcel(xlWorkbook, sc, queryString, UtilityClass.REQUESTINGORG.FROMORG, workflowRules);
            }
            else if (toolingObject == "WorkflowFieldUpdate")
            {
                if (customObjIdToName.Count == 0)
                {
                    getCustomObject(xlWorkbook, customObjIdToName, toolingForm);
                }

                queryString = ToolingApiHelper.WorkflowFieldUpdateQuery();
                ToolingApiHelper.workflowFieldUpdateToExcel(xlWorkbook, sc, queryString, UtilityClass.REQUESTINGORG.FROMORG, customObjIdToName, workflowFieldUpdatesByName);
            }
            else if (toolingObject == "WorkflowOutboundMessage")
            {
                //queryString = ToolingApiHelper.WorkflowOutboundMessageQuery();
                //ToolingApiHelper.workflowOutboundMessageToExcel(xlWorkbook, queryString, UtilityClass.REQUESTINGORG.FROMORG, customObjIdToName, workflowFieldUpdatesByName);
            }
            else if (toolingObject == "WorkflowTask")
            {
                //queryString = ToolingApiHelper.WorkflowTaskQuery();
                //ToolingApiHelper.workflowTaskToExcel(xlWorkbook, queryString, UtilityClass.REQUESTINGORG.FROMORG);
            }
            //else if (toolingObject == "WorkSkillRouting")
            //{
            //    //queryString = ToolingApiHelper.WorkSkillRoutingQuery();
            //    //ToolingApiHelper.workSkillRoutingToExcel(xlWorkbook, queryString, UtilityClass.REQUESTINGORG.FROMORG);
            //}

            dt = DateTime.Now;
            String processingMsg2 = "    " + toolingObject + ": Tooling Retrieval Completed at: " + dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString() + " " + dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString() + "." + dt.Millisecond.ToString() + Environment.NewLine + Environment.NewLine;
            var threadParameters2 = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg2, toolingForm); });
            var thread2 = new System.Threading.Thread(threadParameters2);
            thread2.Start();
            while (thread2.ThreadState == System.Threading.ThreadState.Running)
            {
                // do nothing. Just want for the thread to complete
            }
        }


        // To match the Tooling API Object + Field DeveloperName:
        // The Tooling API does not return standard fields, only custom fields.
        // Remove the __c from the object name
        // Do not add standard fields.
        // Check if the field name contains a __c first, remove the __c from the field name and then add it
        private void parseObjectFiles(Dictionary<String, List<String>> objectFieldNameToLabel, MetadataToolingReportForm toolingForm)
        {
            if (Directory.Exists(toolingForm.tbMetadataFolderLocation.Text + "\\objects"))
            {
                String[] objectFiles = Directory.GetFiles(toolingForm.tbMetadataFolderLocation.Text + "\\objects");
                if (objectFiles.Length > 0)
                {
                    foreach (String fl in objectFiles)
                    {
                        String[] fileSplit = fl.Split('\\');
                        String[] fileNameSplit = fileSplit[fileSplit.Length - 1].Split('.');
                        String objectName = "";

                        if (fileNameSplit[0].EndsWith("__c"))
                        {
                            objectName = fileNameSplit[0].Substring(0, fileNameSplit[0].Length - 3);
                        }
                        else if (fileNameSplit[0].EndsWith("__kav"))
                        {
                            objectName = fileNameSplit[0].Substring(0, fileNameSplit[0].Length - 5);
                        }
                        else if (fileNameSplit[0].EndsWith("__mdt"))
                        {
                            objectName = fileNameSplit[0].Substring(0, fileNameSplit[0].Length - 5);
                        }
                        else
                        {
                            objectName = fileNameSplit[0];
                        }

                        XmlDocument xd = new XmlDocument();
                        xd.Load(fl);

                        XmlNodeList objectFieldsNodesList = xd.GetElementsByTagName("fields");
                        foreach (XmlNode nd1 in objectFieldsNodesList)
                        {
                            if (nd1.ParentNode.Name == "CustomObject")
                            {
                                String developerName = "";
                                String fieldApiName = "";
                                String label = "";
                                String type = "";

                                foreach (XmlNode nd2 in nd1.ChildNodes)
                                {
                                    // Check if a custom field first and then a standard field
                                    if (nd2.Name == "fullName"
                                        && nd2.InnerText.EndsWith("__c"))
                                    {
                                        developerName = nd2.InnerText.Substring(0, nd2.InnerText.Length - 3);
                                        fieldApiName = nd2.InnerText;
                                    }
                                    if (nd2.Name == "fullName")
                                    {
                                        developerName = nd2.InnerText;
                                        fieldApiName = nd2.InnerText;
                                    }
                                    else if (nd2.Name == "label")
                                    {
                                        label = nd2.InnerText;
                                    }
                                    else if (nd2.Name == "type")
                                    {
                                        type = nd2.InnerText;
                                    }
                                }

                                if (developerName != "")
                                {
                                    List<String> tempList = new List<string>();
                                    tempList.Add(label);
                                    tempList.Add(fieldApiName);
                                    tempList.Add(type);

                                    objectFieldNameToLabel.Add(objectName + "." + developerName, tempList);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void parseWorkflowRules(Dictionary<String, ToolingApiHelper.WorkflowRule> workflowRules,
                                        Dictionary<String, ToolingApiHelper.WorkflowFieldUpdate> workflowFieldUpdatesByFullName,
                                        Dictionary<String, ToolingApiHelper.WorkflowFieldUpdate> workflowFieldUpdatesByName,
                                        MetadataToolingReportForm toolingForm)
        {
            if (Directory.Exists(toolingForm.tbMetadataFolderLocation.Text + "\\workflows"))
            {
                String[] workflowFiles = Directory.GetFiles(toolingForm.tbMetadataFolderLocation.Text + "\\workflows");

                if (workflowFiles.Length > 0)
                {
                    foreach (String fl in workflowFiles)
                    {
                        String[] fileSplit  = fl.Split('\\');
                        String[] fileNameSplit = fileSplit[fileSplit.Length - 1].Split('.');

                        XmlDocument xd = new XmlDocument();
                        xd.Load(fl);

                        XmlNodeList fieldUpdateNodeList = xd.GetElementsByTagName("fieldUpdates");
                        foreach (XmlNode nd1 in fieldUpdateNodeList)
                        {
                            // Since this is being added to both workflowFieldUpdatesByFullName and workflowFieldUpdatesByName,
                            // You will not need to add the workflow rule name to both only one.
                            // The two different maps can utilize the FullName or the Name, but the workflow rule list 
                            // will always reference the same object below.
                            ToolingApiHelper.WorkflowFieldUpdate wrkflowFldUpdate = new ToolingApiHelper.WorkflowFieldUpdate();
                            wrkflowFldUpdate.objectName = fileNameSplit[0];
                            wrkflowFldUpdate.workflowRules = new List<string>();

                            foreach (XmlNode nd2 in nd1.ChildNodes)
                            {
                                if (nd2.Name == "fullName")
                                {
                                    wrkflowFldUpdate.fullName = nd2.InnerText;
                                }
                                else if (nd2.Name == "field")
                                {
                                    wrkflowFldUpdate.field = nd2.InnerText;
                                }
                                else if (nd2.Name == "name")
                                {
                                    wrkflowFldUpdate.name = nd2.InnerText;
                                }
                                else if (nd2.Name == "notifyAssignee")
                                {
                                    wrkflowFldUpdate.notifyAssignee = nd2.InnerText;
                                }
                                else if (nd2.Name == "operation")
                                {
                                    wrkflowFldUpdate.operation = nd2.InnerText;
                                }
                                else if (nd2.Name == "protected")
                                {
                                    wrkflowFldUpdate.isProtected = nd2.InnerText;
                                }
                                else if (nd2.Name == "reevaluateOnChange")
                                {
                                    wrkflowFldUpdate.reevaluateOnChange = nd2.InnerText;
                                }
                            }

                            workflowFieldUpdatesByFullName.Add(wrkflowFldUpdate.objectName + "|" + wrkflowFldUpdate.fullName, wrkflowFldUpdate);
                            if (workflowFieldUpdatesByName.ContainsKey(wrkflowFldUpdate.objectName + "|" + wrkflowFldUpdate.name))
                            {
                                workflowFieldUpdatesByName.Add(wrkflowFldUpdate.objectName + "|" + wrkflowFldUpdate.name + "(2)", wrkflowFldUpdate);
                            }
                            else
                            {
                                workflowFieldUpdatesByName.Add(wrkflowFldUpdate.objectName + "|" + wrkflowFldUpdate.name, wrkflowFldUpdate);
                            }
                        }

                        //XmlNodeList alertNodeList = xd.GetElementsByTagName("alerts");
                        //foreach (XmlNode nd1 in alertNodeList)
                        //{
                        //    foreach (XmlNode nd2 in nd1.ChildNodes)
                        //    {

                        //    }
                        //}

                        //XmlNodeList outboundMsgNodeList = xd.GetElementsByTagName("outboundMessages");
                        //foreach (XmlNode nd1 in outboundMsgNodeList)
                        //{

                        //}

                        //XmlNodeList outboundMsgNodeList = xd.GetElementsByTagName("tasks");
                        //foreach (XmlNode nd1 in outboundMsgNodeList)
                        //{

                        //}


                        XmlNodeList workflowRuleNodes = xd.GetElementsByTagName("rules");
                        foreach (XmlNode nd1 in workflowRuleNodes)
                        {
                            ToolingApiHelper.WorkflowRule wflRule = new ToolingApiHelper.WorkflowRule();
                            wflRule.objectName = fileNameSplit[0];

                            foreach (XmlNode nd2 in nd1.ChildNodes)
                            {
                                if (nd2.Name == "fullName")
                                {
                                    wflRule.fullName = nd2.InnerText;
                                }
                                else if (nd2.Name == "active")
                                {
                                    wflRule.active = nd2.InnerText;
                                }
                                else if (nd2.Name == "triggerType")
                                {
                                    wflRule.triggerType = nd2.InnerText;
                                }
                                else if (nd2.Name == "actions")
                                {
                                    if (nd2.ChildNodes[1].InnerText == "Alert")
                                    {
                                        ToolingApiHelper.WorkflowAlert wflAlert = new ToolingApiHelper.WorkflowAlert();
                                        wflAlert.fullName = nd2.ChildNodes[0].InnerText;
                                        wflRule.wrkFlowAlerts.Add(wflAlert.fullName, wflAlert);
                                    }
                                    else if (nd2.ChildNodes[1].InnerText == "FieldUpdate")
                                    {
                                        ToolingApiHelper.WorkflowFieldUpdate wflFieldUpdt = new ToolingApiHelper.WorkflowFieldUpdate();
                                        wflFieldUpdt.fullName = nd2.ChildNodes[0].InnerText;
                                        wflRule.wrkFlowFieldupdates.Add(wflFieldUpdt.fullName, wflFieldUpdt);

                                        if (workflowFieldUpdatesByFullName.ContainsKey(wflRule.objectName + "|" + wflFieldUpdt.fullName))
                                        {
                                            workflowFieldUpdatesByFullName[wflRule.objectName + "|" + wflFieldUpdt.fullName].workflowRules.Add(wflRule.fullName);

                                            //String nameValue = workflowFieldUpdatesByFullName[wflRule.objectName + "|" + wflFieldUpdt.fullName].name;
                                            //workflowFieldUpdatesByName[wflRule.objectName + "|" + nameValue].workflowRules.Add(wflRule.fullName);
                                        }
                                    }
                                    else if (nd2.ChildNodes[1].InnerText == "OutboundMessage")
                                    {
                                        ToolingApiHelper.WorkflowOutboundMessage wflOutboundMsg = new ToolingApiHelper.WorkflowOutboundMessage();
                                        wflOutboundMsg.fullName = nd2.ChildNodes[0].InnerText;
                                        wflRule.wrkFlowOutboundMsgs.Add(wflOutboundMsg.fullName, wflOutboundMsg);
                                    }
                                    else if (nd2.ChildNodes[1].InnerText == "Task")
                                    {
                                        ToolingApiHelper.WorkflowTask wflTask = new ToolingApiHelper.WorkflowTask();
                                        wflTask.fullName = nd2.ChildNodes[0].InnerText;
                                        wflRule.wrkFlowTasks.Add(wflTask.fullName, wflTask);
                                    }
                                }
                            }

                            workflowRules.Add(wflRule.objectName + "|" + wflRule.fullName, wflRule);
                        }
                    }
                }
            }
        }

        // Threadsafe way to write back to the form's textbox
        public void tsWriteToTextbox(String tbValue, MetadataToolingReportForm toolingForm)
        {
            if (toolingForm.rtStatus.InvokeRequired)
            {
                Action safeWrite = delegate { tsWriteToTextbox($"{tbValue}", toolingForm); };
                toolingForm.rtStatus.Invoke(safeWrite);
            }
            else
            {
                toolingForm.rtStatus.Text = toolingForm.rtStatus.Text + tbValue;
            }
        }
    }
}
