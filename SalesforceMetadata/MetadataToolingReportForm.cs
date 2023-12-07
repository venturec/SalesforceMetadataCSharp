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
using static SalesforceMetadata.AutomationReporter;

namespace SalesforceMetadata
{
    public partial class MetadataToolingReportForm : Form
    {
        public String userName;

        public MetadataToolingReportForm()
        {
            InitializeComponent();
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
            else
            {
                // Run a thread for obtaining the custom objects and classes
                Action act = () => buildToolingReport(this);
                Task tsk = Task.Run(act);
            }
        }

        private void buildToolingReport(MetadataToolingReportForm mtrFrm)
        {
            HashSet<String> bypassObjects = new HashSet<String> {"ApexClass", "CustomObject"};

            Boolean loginSuccess = SalesforceCredentials.salesforceToolingLogin(UtilityClass.REQUESTINGORG.FROMORG, userName);

            if (loginSuccess == false)
            {
                MessageBox.Show("Please check username, password and/or security token");
                return;
            }

            DateTime dt = DateTime.Now;
            String processingMsg = "Tooling Report Started at: " + dt.Year.ToString() + "_" + dt.Month.ToString() + "_" + dt.Day.ToString() + "_" + dt.Hour.ToString() + "_" + dt.Minute.ToString() + "_" + dt.Second.ToString() + "_" + dt.Millisecond.ToString() + Environment.NewLine + Environment.NewLine;
            var threadParameters = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg); });
            var thread2 = new System.Threading.Thread(threadParameters);
            thread2.Start();

            Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlapp.Workbooks.Add();
            xlapp.Visible = true;

            // Key = ObjectName.FieldName, Value = Label, Type

            // Salesforce does not return custom object and custom field api names with the __c so we can't match easily to what is in the metadata
            // Some orgs have duplicate Case Type fields: one of them is the standard one, and the other is a custom one, but the Tooling API returns
            // both with the same DeveloperName - Type and the custom one does not have the __c


            Dictionary<String, List<String>> objectFieldNameToLabel = new Dictionary<String, List<String>>();
            parseObjectFiles(objectFieldNameToLabel);


            Dictionary<String, ToolingApiHelper.WorkflowRule> workflowRules = new Dictionary<String, ToolingApiHelper.WorkflowRule>();
            Dictionary<String, ToolingApiHelper.WorkflowFieldUpdate> workflowFieldUpdatesByFullName = new Dictionary<String, ToolingApiHelper.WorkflowFieldUpdate>();
            Dictionary<String, ToolingApiHelper.WorkflowFieldUpdate> workflowFieldUpdatesByName = new Dictionary<String, ToolingApiHelper.WorkflowFieldUpdate>();
            //Dictionary<String, WorkflowAlert> workflowAlerts = new Dictionary<String, WorkflowAlert>();
            //Dictionary<String, WorkflowOutboundMessage> workflowOutboundMsgs = new Dictionary<String, WorkflowOutboundMessage>();
            parseWorkflowRules(workflowRules, workflowFieldUpdatesByFullName, workflowFieldUpdatesByName);


            Dictionary<String, String> customObjIdToName = new Dictionary<String, String>();
            Dictionary<String, String> classIdToClassName = new Dictionary<String, String>();

            getCustomObject(xlWorkbook, customObjIdToName);
            getCustomField(xlWorkbook, customObjIdToName, objectFieldNameToLabel);
            getApexClass(xlWorkbook, customObjIdToName, mtrFrm.cbRetrieveApexClassCoverage.Checked);

            // Run a new Thread for the rest of the objects
            foreach (String objType in mtrFrm.lbToolingObjects.SelectedItems)
            {
                if (!bypassObjects.Contains(objType))
                {
                    getToolingObject(objType,
                                     xlWorkbook,
                                     customObjIdToName,
                                     classIdToClassName,
                                     workflowRules,
                                     workflowFieldUpdatesByName,
                                     mtrFrm.cbRetrieveApexClassCoverage.Checked);
                }
            }

            dt = DateTime.Now;
            processingMsg = "Tooling Retrieval Completed at: " + dt.Year.ToString() + "_" + dt.Month.ToString() + "_" + dt.Day.ToString() + "_" + dt.Hour.ToString() + "_" + dt.Minute.ToString() + "_" + dt.Second.ToString() + "_" + dt.Millisecond.ToString() + Environment.NewLine + Environment.NewLine;
            threadParameters = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg); });
            thread2 = new System.Threading.Thread(threadParameters);
            thread2.Start();
        }


        private void getCustomObject(Microsoft.Office.Interop.Excel.Workbook xlWorkbook, 
                                     Dictionary<String, String> customObjIdToName)
        {
            DateTime dt = DateTime.Now;
            String processingMsg = "    CustomObject: Tooling Retrieval Started at: " + dt.Year.ToString() + "_" + dt.Month.ToString() + "_" + dt.Day.ToString() + "_" + dt.Hour.ToString() + "_" + dt.Minute.ToString() + "_" + dt.Second.ToString() + "_" + dt.Millisecond.ToString() + Environment.NewLine;
            var threadParameters = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg); });
            var thread2 = new System.Threading.Thread(threadParameters);
            thread2.Start();

            String query = ToolingApiHelper.CustomObjectQuery();
            ToolingApiHelper.customObjectToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG, customObjIdToName);

            dt = DateTime.Now;
            processingMsg = "    CustomObject: Tooling Retrieval Completed at: " + dt.Year.ToString() + "_" + dt.Month.ToString() + "_" + dt.Day.ToString() + "_" + dt.Hour.ToString() + "_" + dt.Minute.ToString() + "_" + dt.Second.ToString() + "_" + dt.Millisecond.ToString() + Environment.NewLine + Environment.NewLine;
            threadParameters = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg); });
            thread2 = new System.Threading.Thread(threadParameters);
            thread2.Start();
        }

        private void getCustomField(Microsoft.Office.Interop.Excel.Workbook xlWorkbook, 
                                    Dictionary<String, String> customObjIdToName, 
                                    Dictionary<String, List<String>> objectFieldNameToLabel)
        {
            DateTime dt = DateTime.Now;
            String processingMsg = "    CustomField: Tooling Retrieval Started at: " + dt.Year.ToString() + "_" + dt.Month.ToString() + "_" + dt.Day.ToString() + "_" + dt.Hour.ToString() + "_" + dt.Minute.ToString() + "_" + dt.Second.ToString() + "_" + dt.Millisecond.ToString() + Environment.NewLine;
            var threadParameters = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg); });
            var thread2 = new System.Threading.Thread(threadParameters);
            thread2.Start();

            String query = ToolingApiHelper.CustomFieldQuery();
            ToolingApiHelper.customFieldToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG, customObjIdToName, objectFieldNameToLabel);

            dt = DateTime.Now;
            processingMsg = "    CustomField: Tooling Retrieval Completed at: " + dt.Year.ToString() + "_" + dt.Month.ToString() + "_" + dt.Day.ToString() + "_" + dt.Hour.ToString() + "_" + dt.Minute.ToString() + "_" + dt.Second.ToString() + "_" + dt.Millisecond.ToString() + Environment.NewLine + Environment.NewLine;
            threadParameters = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg); });
            thread2 = new System.Threading.Thread(threadParameters);
            thread2.Start();
        }

        private void getApexClass(Microsoft.Office.Interop.Excel.Workbook xlWorkbook, 
                                  Dictionary<String, String> classIdToClassName,
                                  Boolean retrieveApexCoverage)
        {
            DateTime dt = DateTime.Now;
            String processingMsg = "    ApexClass: Tooling Retrieval Started at: " + dt.Year.ToString() + "_" + dt.Month.ToString() + "_" + dt.Day.ToString() + "_" + dt.Hour.ToString() + "_" + dt.Minute.ToString() + "_" + dt.Second.ToString() + "_" + dt.Millisecond.ToString() + Environment.NewLine;
            var threadParameters = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg); });
            var thread2 = new System.Threading.Thread(threadParameters);
            thread2.Start();

            String query = ToolingApiHelper.ApexClassQuery("");
            ToolingApiHelper.getApexClasses(query, UtilityClass.REQUESTINGORG.FROMORG, classIdToClassName);
            ToolingApiHelper.apexClassToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG, classIdToClassName, retrieveApexCoverage);

            dt = DateTime.Now;
            processingMsg = "    ApexClass: Tooling Retrieval Completed at: " + dt.Year.ToString() + "_" + dt.Month.ToString() + "_" + dt.Day.ToString() + "_" + dt.Hour.ToString() + "_" + dt.Minute.ToString() + "_" + dt.Second.ToString() + "_" + dt.Millisecond.ToString() + Environment.NewLine + Environment.NewLine;
            threadParameters = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg); });
            thread2 = new System.Threading.Thread(threadParameters);
            thread2.Start();
        }

        private void getToolingObject(String toolingObject, 
                                      Microsoft.Office.Interop.Excel.Workbook xlWorkbook, 
                                      Dictionary<String, String> customObjIdToName, 
                                      Dictionary<String, String> classIdToClassName,
                                      Dictionary<String, ToolingApiHelper.WorkflowRule> workflowRules,
                                      Dictionary<String, ToolingApiHelper.WorkflowFieldUpdate> workflowFieldUpdatesByName,
                                      Boolean retrieveApexCoverage)
        {
            DateTime dt = DateTime.Now;
            String processingMsg = "    " + toolingObject + ": Tooling Retrieval Started at: " + dt.Year.ToString() + "_" + dt.Month.ToString() + "_" + dt.Day.ToString() + "_" + dt.Hour.ToString() + "_" + dt.Minute.ToString() + "_" + dt.Second.ToString() + "_" + dt.Millisecond.ToString() + Environment.NewLine;
            var threadParameters = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg); });
            var thread2 = new System.Threading.Thread(threadParameters);
            thread2.Start();

            String query = "";

            if (toolingObject == "ApexComponent")
            {
                query = ToolingApiHelper.ApexComponentQuery();
                ToolingApiHelper.apexComponentToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG, classIdToClassName);
            }
            else if (toolingObject == "ApexPage")
            {
                query = ToolingApiHelper.ApexPageQuery();
                ToolingApiHelper.apexPageToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG, classIdToClassName);
            }
            else if (toolingObject == "ApexTrigger")
            {
                query = ToolingApiHelper.ApexTriggerQuery("");
                ToolingApiHelper.apexTriggerToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG, classIdToClassName, customObjIdToName, retrieveApexCoverage);
            }
            else if (toolingObject == "CompactLayout")
            {
                query = ToolingApiHelper.CompactLayoutQuery("");
                ToolingApiHelper.compactLayoutToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG);
            }
            else if (toolingObject == "EmailTemplate")
            {
                query = ToolingApiHelper.EmailTemplateQuery();
                ToolingApiHelper.emailTemplateToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG);
            }
            else if (toolingObject == "FlexiPage")
            {
                query = ToolingApiHelper.FlexiPageQuery("");
                ToolingApiHelper.flexiPageToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG, customObjIdToName);
            }
            else if (toolingObject == "Flow")
            {
                query = ToolingApiHelper.FlowQuery();
                ToolingApiHelper.flowToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG);
            }
            else if (toolingObject == "Layout")
            {
                query = ToolingApiHelper.LayoutQuery("");
                ToolingApiHelper.layoutToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG, customObjIdToName);
            }
            else if (toolingObject == "LightningComponentBundle")
            {
                query = ToolingApiHelper.LightningComponentBundleQuery();
                ToolingApiHelper.lwcToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG);
            }
            else if (toolingObject == "LightningComponentBundle")
            {
                query = ToolingApiHelper.LightningComponentBundleQuery();
                ToolingApiHelper.lwcToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG);
            }
            else if (toolingObject == "RecordType")
            {
                query = ToolingApiHelper.RecordTypeQuery();
                ToolingApiHelper.recordTypesToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG);
            }
            else if (toolingObject == "ValidationRule")
            {
                query = ToolingApiHelper.ValidationRuleQuery("", "");
                ToolingApiHelper.validationRuleToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG, customObjIdToName);
            }
            else if (toolingObject == "WorkflowRule")
            {
                query = ToolingApiHelper.WorkflowAlertQuery();
                ToolingApiHelper.workflowAlertToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG, workflowRules);
            }
            else if (toolingObject == "WorkflowFieldUpdate")
            {
                query = ToolingApiHelper.WorkflowFieldUpdateQuery();
                ToolingApiHelper.workflowFieldUpdateToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG, customObjIdToName, workflowFieldUpdatesByName);
            }
            else if (toolingObject == "WorkflowOutboundMessage")
            {
                //query = ToolingApiHelper.WorkflowOutboundMessageQuery();
                //ToolingApiHelper.workflowOutboundMessageToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG);
            }
            else if (toolingObject == "WorkflowTask")
            {
                //query = ToolingApiHelper.WorkflowTaskQuery();
                //ToolingApiHelper.workflowTaskToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG);
            }
            else if (toolingObject == "WorkSkillRouting")
            {
                //query = ToolingApiHelper.WorkSkillRoutingQuery();
                //ToolingApiHelper.workSkillRoutingToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG);
            }

            dt = DateTime.Now;
            processingMsg = "    " + toolingObject + ": Tooling Retrieval Completed at: " + dt.Year.ToString() + "_" + dt.Month.ToString() + "_" + dt.Day.ToString() + "_" + dt.Hour.ToString() + "_" + dt.Minute.ToString() + "_" + dt.Second.ToString() + "_" + dt.Millisecond.ToString() + Environment.NewLine + Environment.NewLine;
            threadParameters = new System.Threading.ThreadStart(delegate { tsWriteToTextbox(processingMsg); });
            thread2 = new System.Threading.Thread(threadParameters);
            thread2.Start();
        }


        // To match the Tooling API Object + Field DeveloperName:
        // The Tooling API does not return standard fields, only custom fields.
        // Remove the __c from the object name
        // Do not add standard fields.
        // Check if the field name contains a __c first, remove the __c from the field name and then add it
        private void parseObjectFiles(Dictionary<String, List<String>> objectFieldNameToLabel)
        {
            if (Directory.Exists(this.tbMetadataFolderLocation.Text + "\\objects"))
            {
                String[] objectFiles = Directory.GetFiles(this.tbMetadataFolderLocation.Text + "\\objects");
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
                                        Dictionary<String, ToolingApiHelper.WorkflowFieldUpdate> workflowFieldUpdatesByName)
        {
            if (Directory.Exists(this.tbMetadataFolderLocation.Text + "\\workflows"))
            {
                String[] workflowFiles = Directory.GetFiles(this.tbMetadataFolderLocation.Text + "\\workflows");

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
        public void tsWriteToTextbox(String tbValue)
        {
            if (this.rtStatus.InvokeRequired)
            {
                Action safeWrite = delegate { tsWriteToTextbox($"{tbValue}"); };
                this.rtStatus.Invoke(safeWrite);
            }
            else
            {
                this.rtStatus.Text = this.rtStatus.Text + tbValue;
            }
        }

    }
}
