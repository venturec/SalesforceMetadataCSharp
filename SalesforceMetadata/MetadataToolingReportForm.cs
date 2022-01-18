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

namespace SalesforceMetadata
{
    public partial class MetadataToolingReportForm : Form
    {
        public String userName;
        public String password;
        public String securityToken;


        public MetadataToolingReportForm()
        {
            InitializeComponent();
        }

        private void tbMetadataFolderLocation_DoubleClick(object sender, EventArgs e)
        {
            this.tbMetadataFolderLocation.Text = UtilityClass.folderBrowserSelectPath("Select Directory to read the Metadata from", true, FolderEnum.ReadFrom);
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            if (this.tbMetadataFolderLocation.Text == "")
            {
                MessageBox.Show("Please choose a location to read the metadata from for the proper reporting");
            }
            else
            {
                SalesforceCredentials.fromOrgUsername = userName;
                SalesforceCredentials.fromOrgPassword = password;
                SalesforceCredentials.fromOrgSecurityToken = securityToken;

                Boolean loginSuccess = SalesforceCredentials.salesforceToolingLogin();

                if (loginSuccess == false)
                {
                    MessageBox.Show("Please check username, password and/or security token");
                    return;
                }


                Dictionary<String, String> customObjIdToName18 = new Dictionary<String, String>();
                Dictionary<String, String> customObjIdToName15 = new Dictionary<String, String>();
                Dictionary<String, String> classIdToClassName = new Dictionary<String, String>();

                // Key = ObjectName.FieldName, Value = Label, Type
                Dictionary<String, List<String>> objectFieldNameToLabel = new Dictionary<String, List<String>>();

                Dictionary<String, ToolingApiHelper.WorkflowRule> workflowRules = new Dictionary<String, ToolingApiHelper.WorkflowRule>();
                Dictionary<String, ToolingApiHelper.WorkflowFieldUpdate> workflowFieldUpdatesByFullName = new Dictionary<String, ToolingApiHelper.WorkflowFieldUpdate>();
                Dictionary<String, ToolingApiHelper.WorkflowFieldUpdate> workflowFieldUpdatesByName = new Dictionary<String, ToolingApiHelper.WorkflowFieldUpdate>();
                //Dictionary<String, WorkflowAlert> workflowAlerts = new Dictionary<String, WorkflowAlert>();
                //Dictionary<String, WorkflowOutboundMessage> workflowOutboundMsgs = new Dictionary<String, WorkflowOutboundMessage>();

                // Salesforce does not return custom object and custom field api names with the __c so we can't match easily to what is in the metadata
                // Some orgs have duplicate Case Type fields: one of them is the standard one, and the other is a custom one, but the Tooling API returns
                // both with the same DeveloperName - Type and the custom one does not have the __c
                
                //parseObjectFiles(objectFieldNameToLabel);
                parseWorkflowRules(workflowRules, workflowFieldUpdatesByFullName, workflowFieldUpdatesByName);

                Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
                xlapp.Visible = false;

                Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlapp.Workbooks.Add();

                String query = "";

                query = ToolingApiHelper.CustomObjectQuery();
                ToolingApiHelper.customObjectToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG, customObjIdToName18, customObjIdToName15);

                //query = ToolingApiHelper.CustomFieldQuery();
                //ToolingApiHelper.customFieldToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG, customObjIdToName18, customObjIdToName15, objectFieldNameToLabel);

                query = ToolingApiHelper.ApexClassQuery("");
                ToolingApiHelper.getApexClasses(query, UtilityClass.REQUESTINGORG.FROMORG, classIdToClassName);
                ToolingApiHelper.apexClassToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG, classIdToClassName);

                query = ToolingApiHelper.ApexTriggerQuery("");
                ToolingApiHelper.apexTriggerToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG, classIdToClassName, customObjIdToName18, customObjIdToName15);

                query = ToolingApiHelper.FlowQuery();
                ToolingApiHelper.flowToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG);

                //query = ToolingApiHelper.CompactLayoutQuery("");
                //ToolingApiHelper.compactLayoutToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG);

                //query = ToolingApiHelper.FlexiPageQuery("");
                //ToolingApiHelper.flexiPageToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG, customObjIdToName18, customObjIdToName15);

                //query = ToolingApiHelper.LayoutQuery("");
                //ToolingApiHelper.layoutToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG, customObjIdToName18, customObjIdToName15);

                //query = ToolingApiHelper.EmailTemplateQuery();
                //ToolingApiHelper.emailTemplateToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG);

                query = ToolingApiHelper.WorkflowAlertQuery();
                ToolingApiHelper.workflowAlertToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG, workflowRules);

                query = ToolingApiHelper.WorkflowFieldUpdateQuery();
                ToolingApiHelper.workflowFieldUpdateToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG, customObjIdToName18, customObjIdToName15, workflowFieldUpdatesByName);

                query = ToolingApiHelper.ValidationRuleQuery("", "");
                ToolingApiHelper.validationRuleToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG, customObjIdToName18, customObjIdToName15);

                /*
                query = ToolingApiHelper.WorkflowOutboundMessageQuery();
                ToolingApiHelper.workflowOutboundMessageToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG);

                query = ToolingApiHelper.WorkflowTaskQuery();
                ToolingApiHelper.workflowTaskToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG);

                query = ToolingApiHelper.WorkSkillRoutingQuery();
                ToolingApiHelper.workSkillRoutingToExcel(xlWorkbook, query, UtilityClass.REQUESTINGORG.FROMORG);
                */

                // Now go through the metadata to find all configuration items and generate a lines report for each one:
                // The main areas to focus on are:
                // Approval Processes - active only
                // Assignment Rules
                // Auto Response Rules
                // Escalation Rules
                // Flows (active only)
                // Object - Validation Rules
                // Quick Actions
                // Workflow rules (active only): with field updates, email alerts, callouts, task creation
                // Work Skill Routing

                //getLinesOfConfiguration(xlWorkbook, this.tbMetadataFolderLocation.Text, "approvalProcesses", "ApprovalProcessesLinesReport");
                //getLinesOfConfiguration(xlWorkbook, this.tbMetadataFolderLocation.Text, "assignmentRules", "AssignmentRulesLinesReport");
                //getLinesOfConfiguration(xlWorkbook, this.tbMetadataFolderLocation.Text, "autoResponseRules", "AutoResponseLinesReport");
                //getLinesOfConfiguration(xlWorkbook, this.tbMetadataFolderLocation.Text, "escalationRules", "EscalationRulesLinesReport");
                //getLinesOfConfiguration(xlWorkbook, this.tbMetadataFolderLocation.Text, "flows", "FlowsLinesReport");
                //getLinesOfConfiguration(xlWorkbook, this.tbMetadataFolderLocation.Text, "quickActions", "QuckActionsLinesReport");

                //getLinesOfConfiguration(xlWorkbook, this.tbMetadataFolderLocation.Text, "workflows", "WorkflowEvents");
                //getLinesOfConfiguration(xlWorkbook, this.tbMetadataFolderLocation.Text, "workSkillRoutings", "SkillRoutingLinesReport");

                xlapp.Visible = true;
            }
        }

        //private void getLinesOfConfiguration(Microsoft.Office.Interop.Excel.Workbook xlWorkbook, String directory, String folder, String tabName)
        //{

        //    String[] files = Directory.GetFiles(directory + "\\" + folder);

        //    Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
        //                                                (System.Reflection.Missing.Value,
        //                                                    xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
        //                                                    System.Reflection.Missing.Value,
        //                                                    System.Reflection.Missing.Value);

        //    xlWorksheet.Name = tabName;
        //    xlWorksheet.Cells[1, 1].Value = "Name";
        //    xlWorksheet.Cells[1, 2].Value = "LinesOfConfiguration";

        //    Int32 rowNumber = 2;
        //    foreach (String file in files)
        //    {
        //        Int32 linesOfConfiguration = 0;

        //        String[] fileNamePath = file.Split('\\');
        //        String[] fileName = fileNamePath[fileNamePath.Length - 1].Split('.');

        //        XmlDocument xd = new XmlDocument();
        //        xd.Load(file);

        //        XmlNodeList isActiveCheck = xd.GetElementsByTagName("active");

        //        if (isActiveCheck.Count > 0 && isActiveCheck[0].InnerText == "false") continue;

        //        foreach (XmlNode nd1 in xd.ChildNodes)
        //        {
        //            if (nd1.HasChildNodes)
        //            {
        //                foreach (XmlNode nd2 in nd1.ChildNodes)
        //                {
        //                    if (nd2.HasChildNodes)
        //                    {
        //                        foreach (XmlNode nd3 in nd2.ChildNodes)
        //                        {
        //                            if (nd3.HasChildNodes)
        //                            {
        //                                foreach (XmlNode nd4 in nd3.ChildNodes)
        //                                {
        //                                    if (nd4.HasChildNodes)
        //                                    {
        //                                        foreach (XmlNode nd5 in nd4.ChildNodes)
        //                                        {
        //                                            if (nd5.HasChildNodes)
        //                                            {
        //                                                foreach (XmlNode nd6 in nd5.ChildNodes)
        //                                                {
        //                                                    if (nd6.HasChildNodes)
        //                                                    {
        //                                                        foreach (XmlNode nd7 in nd6.ChildNodes)
        //                                                        {
        //                                                            if (nd7.HasChildNodes)
        //                                                            {
        //                                                                foreach (XmlNode nd8 in nd7.ChildNodes)
        //                                                                {
        //                                                                    if (nd8.HasChildNodes)
        //                                                                    {
        //                                                                        foreach (XmlNode nd9 in nd8.ChildNodes)
        //                                                                        {
        //                                                                            if (nd9.HasChildNodes)
        //                                                                            {
        //                                                                                foreach (XmlNode nd10 in nd9.ChildNodes)
        //                                                                                {
        //                                                                                    if (nd10.HasChildNodes)
        //                                                                                    {
        //                                                                                        foreach (XmlNode nd11 in nd10.ChildNodes)
        //                                                                                        {
        //                                                                                            if (nd11.HasChildNodes)
        //                                                                                            {
        //                                                                                            }
        //                                                                                            else if (nd10.Name != "description" && nd11.Name == "#text")
        //                                                                                            {
        //                                                                                                linesOfConfiguration++;
        //                                                                                            }
        //                                                                                        }
        //                                                                                    }
        //                                                                                    else if (nd9.Name != "description" && nd10.Name == "#text")
        //                                                                                    {
        //                                                                                        linesOfConfiguration++;
        //                                                                                    }
        //                                                                                }
        //                                                                            }
        //                                                                            else if (nd8.Name != "description" && nd9.Name == "#text")
        //                                                                            {
        //                                                                                linesOfConfiguration++;
        //                                                                            }
        //                                                                        }
        //                                                                    }
        //                                                                    else if (nd7.Name != "description" && nd8.Name == "#text")
        //                                                                    {
        //                                                                        linesOfConfiguration++;
        //                                                                    }
        //                                                                }
        //                                                            }
        //                                                            else if (nd6.Name != "description" && nd7.Name == "#text")
        //                                                            {
        //                                                                linesOfConfiguration++;
        //                                                            }
        //                                                        }
        //                                                    }
        //                                                    else if (nd5.Name != "description" && nd6.Name == "#text")
        //                                                    {
        //                                                        linesOfConfiguration++;
        //                                                    }
        //                                                }
        //                                            }
        //                                            else if (nd4.Name != "description" && nd5.Name == "#text")
        //                                            {
        //                                                linesOfConfiguration++;
        //                                            }
        //                                        }
        //                                    }
        //                                    else if (nd3.Name != "description" && nd4.Name == "#text")
        //                                    {
        //                                        linesOfConfiguration++;
        //                                    }
        //                                }
        //                            }
        //                            else if (nd2.Name != "description" && nd3.Name == "#text")
        //                            {
        //                                linesOfConfiguration++;
        //                            }
        //                        }
        //                    }
        //                    else if (nd1.Name != "description" && nd2.Name == "#text")
        //                    {
        //                        linesOfConfiguration++;
        //                    }
        //                }
        //            }
        //            else if (nd1.Name == "#text")
        //            {
        //                linesOfConfiguration++;
        //            }
        //        }

        //        xlWorksheet.Cells[rowNumber, 1].Value = fileName[fileName.Length - 2];
        //        xlWorksheet.Cells[rowNumber, 2].Value = linesOfConfiguration;

        //        rowNumber++;
        //    }


        //    //return linesOfConfiguration;
        //}


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
                                String fieldApiName = "";
                                String label = "";
                                String type = "";

                                foreach (XmlNode nd2 in nd1.ChildNodes)
                                {
                                    if (nd2.Name == "fullName"
                                        && nd2.InnerText.EndsWith("__c"))
                                    {
                                        fieldApiName = nd2.InnerText.Substring(0, nd2.InnerText.Length - 3);
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

                                if (fieldApiName != "")
                                {
                                    List<String> tempList = new List<string>();
                                    tempList.Add(label);
                                    tempList.Add(type);

                                    objectFieldNameToLabel.Add(objectName + "." + fieldApiName, tempList);
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
                            workflowFieldUpdatesByName.Add(wrkflowFldUpdate.objectName + "|" + wrkflowFldUpdate.name, wrkflowFldUpdate);
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
    }
}
