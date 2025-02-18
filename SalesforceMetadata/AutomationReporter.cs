﻿using Microsoft.Office.Interop.Excel;
using SalesforceMetadata.ToolingWSDL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SalesforceMetadata
{
    public partial class AutomationReporter : Form
    {
        public const String FLOW = "Flow";
        public const String APEX_CLASS = "ApexClass";
        public const String APEX_CLASS_METHOD = "ApexClassMethod";

        // Key = class name
        public Dictionary<String, ApexClassStructure> classNmToClass;

        // Variable names used in Triggers and classes
        // Object
        // Trigger Name
        // Trigger Type - insert, update, delete, undelete
        public Dictionary<String, List<ApexTriggerStructure>> objectToTrigger;

        public Dictionary<String, List<FlowProcess>> objectToFlow;

        public Dictionary<String, List<WorkflowRule>> workflowObj;
        public Dictionary<String, List<WorkflowFieldUpdates>> workflowFldUpdates;

        List<FieldExtractor> fieldExtractorList = new List<FieldExtractor>();

        public AutomationReporter()
        {
            InitializeComponent();
        }

        // We need the objects and fields and the apex classes extracted out first
        private void runObjectFieldExtract()
        {
            if (Directory.Exists(this.tbProjectFolder.Text + "\\objects")
                || this.tbProjectFolder.Text == this.tbProjectFolder.Text + "\\objects")
            {

            }
        }

        private void runApexClassExtract()
        {
            if (Directory.Exists(this.tbProjectFolder.Text + "\\classes"))
            {
                String[] files = Directory.GetFiles(this.tbProjectFolder.Text + "\\classes");

                this.classNmToClass = new Dictionary<string, ApexClassStructure>();

                foreach (String fl in files)
                {
                    String[] flPathSplit = fl.Split('\\');
                    String[] flNameSplit = flPathSplit[flPathSplit.Length - 1].Split('.');

                    if (flNameSplit[1] == "cls-meta") { continue; }

                    StreamReader sr = new StreamReader(fl);
                    String flread = sr.ReadToEnd();
                    sr.Close();

                    flread = reduceWhitespace(flread);

                    // Break the file contents into an array of string to parse through
                    String[] filearray = flread.Split(' ');

                    parseApexClass(filearray);
                }
            }
        }

        // This one will be a little more complex
        // Extract out the SOQL comments filtering on SELECT and FROM
        // Extract out the List of objects
        // Extract out the Map of objects
        // Extract methods
        // Extract inner classes
        // Extract the trigger handler classes
        // Extract the extends and implements
        // Extract out the database.insert, database.update, database.delete, database.undelete
        // Extract out the insert, update, delete, undelete
        // Bypass the test classes and methods as we don't need these in the current mapping
        // Map the object to what automation pieces are around these as well as the variable names uses so that you can determine when an instantiated variable
        // is used to trigger other automation such as insert, update, delete, undelete

        // For triggers only:
        // before insert,
        // before update,
        // before delete,
        // after insert,
        // after update,
        // after delete,
        // after undelete,

        // Note: The , may or may not be there in the trigger types
        private void runApexTriggerExtract()
        {
            this.objectToTrigger = new Dictionary<string, List<ApexTriggerStructure>>();

            if (Directory.Exists(this.tbProjectFolder.Text + "\\triggers"))
            {
                String[] files = Directory.GetFiles(this.tbProjectFolder.Text + "\\triggers");

                foreach (String fl in files)
                {
                    String[] flPathSplit = fl.Split('\\');
                    String[] flNameSplit = flPathSplit[flPathSplit.Length - 1].Split('.');

                    if (flNameSplit[1] == "trigger-meta") { continue; }

                    StreamReader sr = new StreamReader(fl);
                    String flread = sr.ReadToEnd();
                    sr.Close();

                    flread = reduceWhitespace(flread);

                    // Break the file contents into an array of string to parse through
                    String[] filearray = flread.Split(' ');

                    parseApexTrigger(filearray);
                }
            }
        }

        // TODO: Needs work
        private void runQuickActionExtract()
        {
            if (Directory.Exists(this.tbProjectFolder.Text + "\\quickActions"))
            {

            }
        }

        private void runFlowProcessExtract()
        {
            if (Directory.Exists(this.tbProjectFolder.Text + "\\flows"))
            {
                objectToFlow = new Dictionary<string, List<FlowProcess>>();

                String[] files = Directory.GetFiles(this.tbProjectFolder.Text + "\\flows");

                foreach (String fl in files)
                {
                    String[] flPathSplit = fl.Split('\\');
                    String[] flNameSplit = flPathSplit[flPathSplit.Length - 1].Split('.');

                    FlowProcess flowObj = new FlowProcess();
                    flowObj.apiName = flNameSplit[0];

                    XmlDocument objXd = new XmlDocument();
                    objXd.Load(fl);

                    XmlNodeList ndListStatus   = objXd.GetElementsByTagName("status");
                    XmlNodeList ndListLabel    = objXd.GetElementsByTagName("label");
                    XmlNodeList ndListProcType = objXd.GetElementsByTagName("processType");
                    XmlNodeList ndListObjProc  = objXd.GetElementsByTagName("processMetadataValues");

                    XmlNodeList ndListStart        = objXd.GetElementsByTagName("start");
                    XmlNodeList ndListStartElemRef = objXd.GetElementsByTagName("startElementReference");

                    XmlNodeList ndListApiVs       = objXd.GetElementsByTagName("apiVersion");
                    XmlNodeList ndListRunInMd     = objXd.GetElementsByTagName("runInMode");
                    XmlNodeList ndListVariables   = objXd.GetElementsByTagName("variables");
                    XmlNodeList ndListAssignments = objXd.GetElementsByTagName("assignments");

                    XmlNodeList ndListRecordLookups = objXd.GetElementsByTagName("recordLookups");
                    XmlNodeList ndListRecordCreates = objXd.GetElementsByTagName("recordCreates");
                    XmlNodeList ndListRecordUpdates = objXd.GetElementsByTagName("recordUpdates");
                    XmlNodeList ndListRecordDeletes = objXd.GetElementsByTagName("recordDeletes");

                    XmlNodeList ndListActionCalls = objXd.GetElementsByTagName("actionCalls");

                    Boolean continueLoop = true;
                    foreach (XmlNode xn in ndListStatus)
                    {
                        if (xn.ParentNode.LocalName == "Flow")
                        {
                            if (xn.InnerText == "Active")
                            {
                                flowObj.isActive = true;
                            }
                            else
                            {
                                continueLoop = false;
                            }
                        }
                    }

                    if (continueLoop == false)
                    {
                        continue;
                    }

                    foreach (XmlNode xn in ndListLabel)
                    {
                        if (xn.ParentNode.LocalName == "Flow")
                        {
                            flowObj.label = xn.InnerText;
                        }
                    }

                    foreach (XmlNode xn in ndListProcType)
                    {
                        if (xn.ParentNode.LocalName == "Flow")
                        {
                            flowObj.flowProcessType = xn.InnerText;
                        }
                    }

                    foreach (XmlNode xn in ndListObjProc)
                    {
                        if (xn.ParentNode.LocalName == "Flow")
                        {
                            //Debug.WriteLine(" ");
                            if (xn.ChildNodes[0].InnerText == "ObjectType")
                            {
                                flowObj.objectName = xn.ChildNodes[1].InnerText;
                            }
                            else if (xn.ChildNodes[0].InnerText == "TriggerType")
                            {
                                flowObj.triggerType = xn.ChildNodes[1].InnerText;
                            }
                        }
                    }

                    foreach (XmlNode xn in ndListApiVs)
                    {
                        flowObj.apiVersion = xn.InnerText;
                    }

                    foreach (XmlNode xn in ndListStart)
                    {
                        if (xn.ParentNode.LocalName == "Flow")
                        {
                            String startReference = "";

                            foreach (XmlNode cn1 in xn.ChildNodes)
                            {
                                if (cn1.Name == "object")
                                {
                                    flowObj.objectName = cn1.InnerText;
                                }
                                else if (cn1.Name == "recordTriggerType")
                                {
                                    flowObj.recordTriggerType = cn1.InnerText;
                                }
                                else if (cn1.Name == "triggerType")
                                {
                                    flowObj.triggerType = cn1.InnerText;
                                }
                                else if (cn1.Name == "connector")
                                {
                                    startReference = cn1.InnerText;
                                }
                            }

                            Boolean rlNameMatch = false;
                            if (startReference != "" && flowObj.objectName == "")
                            {
                                foreach (XmlNode rl in ndListRecordLookups)
                                {
                                    foreach (XmlNode cn in rl.ChildNodes)
                                    {
                                        if (cn.Name == "name" && startReference == cn.InnerText)
                                        {
                                            rlNameMatch = true;
                                        }

                                        if (cn.Name == "object" && rlNameMatch == true)
                                        {
                                            flowObj.objectName = cn.InnerText;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    foreach (XmlNode xn in ndListStartElemRef)
                    {
                        String startReference = "";

                        if (xn.ParentNode.LocalName == "Flow")
                        {
                            startReference = xn.InnerText;
                        }

                        // Now find the object related to the start variable
                        Boolean rlNameMatch = false;
                        foreach (XmlNode rl in ndListRecordLookups)
                        {
                            foreach (XmlNode cn in rl.ChildNodes)
                            {
                                if (cn.Name == "name" && startReference == cn.InnerText)
                                {
                                    rlNameMatch = true;
                                }

                                if (cn.Name == "object" && rlNameMatch == true)
                                {
                                    flowObj.objectName = cn.InnerText;
                                }
                            }
                        }
                    }

                    foreach (XmlNode xn in ndListVariables)
                    {
                        if (xn.ParentNode.LocalName == "Flow")
                        {
                            //Debug.WriteLine("");
                            FlowVariable fv = new FlowVariable();
                            foreach (XmlNode cn in xn.ChildNodes)
                            {
                                if (cn.Name == "name")
                                {
                                    fv.varName = cn.InnerText;
                                }
                                else if (cn.Name == "apexClass")
                                {
                                    fv.apexClass = cn.InnerText;
                                }
                                else if (cn.Name == "dataType")
                                {
                                    fv.dataType = cn.InnerText;
                                }
                                else if (cn.Name == "objectType")
                                {
                                    fv.objectType = cn.InnerText;
                                }
                                else if (cn.Name == "isCollection")
                                {
                                    fv.isCollection = Boolean.Parse(cn.InnerText);
                                }
                                else if (cn.Name == "isInput")
                                {
                                    fv.isInput = Boolean.Parse(cn.InnerText);
                                }
                                else if (cn.Name == "isOutput")
                                {
                                    fv.isOutput = Boolean.Parse(cn.InnerText);
                                }
                            }

                            flowObj.variableToObject.Add(fv.varName, fv);
                        }
                    }

                    foreach (XmlNode xn in ndListRunInMd)
                    {
                        if (xn.ParentNode.LocalName == "Flow")
                        {
                            flowObj.runInMode = xn.InnerText;
                        }
                    }

                    foreach (XmlNode xn in ndListRecordCreates)
                    {
                        if (xn.ParentNode.LocalName == "Flow")
                        {
                            String objName = "";
                            String varName = "";

                            foreach (XmlNode nd1 in xn.ChildNodes)
                            {
                                if (nd1.Name == "object")
                                {
                                    objName = nd1.InnerText;
                                }
                                else if (nd1.Name == "name")
                                {
                                    varName = nd1.InnerText;
                                }
                            }

                            if (objName != "")
                            {
                                if (flowObj.recordCreates.ContainsKey(objName))
                                {
                                    flowObj.recordCreates[objName].Add(varName);
                                }
                                else
                                {
                                    flowObj.recordCreates.Add(objName, new List<String> { varName });
                                }
                            }
                        }
                    }

                    foreach (XmlNode xn in ndListRecordUpdates)
                    {
                        if (xn.ParentNode.LocalName == "Flow")
                        {
                            String objName = "";
                            String varName = "";

                            foreach (XmlNode nd1 in xn.ChildNodes)
                            {
                                if (nd1.Name == "object")
                                {
                                    objName = nd1.InnerText;
                                }
                                else if (nd1.Name == "name")
                                {
                                    varName = nd1.InnerText;
                                }
                            }

                            if (objName != "")
                            {
                                if (flowObj.recordUpdates.ContainsKey(objName))
                                {
                                    flowObj.recordUpdates[objName].Add(varName);
                                }
                                else
                                {
                                    flowObj.recordUpdates.Add(objName, new List<String> { varName });
                                }
                            }
                        }
                    }

                    foreach (XmlNode xn in ndListRecordDeletes)
                    {
                        if (xn.ParentNode.LocalName == "Flow")
                        {
                            String objName = "";
                            String varName = "";

                            foreach (XmlNode nd1 in xn.ChildNodes)
                            {
                                if (nd1.Name == "object")
                                {
                                    objName = nd1.InnerText;
                                }
                                else if (nd1.Name == "name")
                                {
                                    varName = nd1.InnerText;
                                }
                            }

                            if (objName != "")
                            {
                                if (flowObj.recordDeletes.ContainsKey(objName))
                                {
                                    flowObj.recordDeletes[objName].Add(varName);
                                }
                                else
                                {
                                    flowObj.recordDeletes.Add(objName, new List<String> { varName });
                                }
                            }
                        }
                    }

                    foreach (XmlNode xn in ndListActionCalls)
                    {
                        if (xn.ParentNode.LocalName == "Flow")
                        {
                            FlowActionCalls fac = new FlowActionCalls();
                            //Debug.WriteLine("");
                            foreach (XmlNode nd1 in xn.ChildNodes)
                            {
                                if (nd1.Name == "name")
                                {
                                    fac.actionCallAPIName = nd1.InnerText;
                                }
                                else if (nd1.Name == "label")
                                {
                                    fac.actionCallLabel = nd1.InnerText;
                                }
                                else if (nd1.Name == "actionName")
                                {
                                    fac.actionName = nd1.InnerText;
                                }
                                else if (nd1.Name == "actionType")
                                {
                                    fac.actionType = nd1.InnerText;
                                }
                                else if (nd1.Name == "inputParameters")
                                {
                                    FlowActionInputParameters faip = new FlowActionInputParameters();
                                    if (nd1.ChildNodes.Count == 1)
                                    {
                                        faip.apiName = nd1.ChildNodes[0].InnerText;
                                        faip.value = "";
                                    }
                                    else
                                    {
                                        faip.apiName = nd1.ChildNodes[0].InnerText;
                                        faip.value = nd1.ChildNodes[1].InnerText;
                                    }

                                    fac.inputParameters.Add(faip);
                                }
                            }

                            flowObj.actionCalls.Add(fac);
                        }
                    }

                    if (this.objectToFlow.ContainsKey(flowObj.objectName))
                    {
                        this.objectToFlow[flowObj.objectName].Add(flowObj);
                    }
                    else
                    {
                        this.objectToFlow.Add(flowObj.objectName, new List<FlowProcess> { flowObj });
                    }
                }
            }
        }

        private void runWorkflowExtract()
        {
            if (Directory.Exists(this.tbProjectFolder.Text + "\\workflows"))
            {
                this.workflowObj = new Dictionary<string, List<WorkflowRule>>();
                this.workflowFldUpdates = new Dictionary<String, List<WorkflowFieldUpdates>>();

                String[] files = Directory.GetFiles(this.tbProjectFolder.Text + "\\workflows");

                foreach (String fl in files)
                {
                    String[] flPathSplit = fl.Split('\\');
                    String[] flNameSplit = flPathSplit[flPathSplit.Length - 1].Split('.');

                    XmlDocument objXd = new XmlDocument();
                    objXd.Load(fl);

                    XmlNodeList wfRules = objXd.GetElementsByTagName("rules");
                    XmlNodeList wfFieldUpdates = objXd.GetElementsByTagName("fieldUpdates");

                    if (wfRules.Count > 0)
                    {
                        foreach (XmlNode nd1 in wfRules)
                        {
                            if (nd1.ParentNode.Name == "Workflow")
                            {
                                WorkflowRule wrkFlowObj = new WorkflowRule();
                                wrkFlowObj.objectName = flNameSplit[0];

                                HashSet<String> fieldUpdates = new HashSet<String>();

                                // Make sure the rule is active before adding the values
                                foreach (XmlNode cn in nd1.ChildNodes)
                                {
                                    if (cn.Name == "active")
                                    {
                                        wrkFlowObj.isActive = Boolean.Parse(cn.InnerText);
                                    }
                                    else if (cn.Name == "fullName")
                                    {
                                        wrkFlowObj.workflowRuleName = cn.InnerText;
                                    }
                                    else if (cn.Name == "triggerType")
                                    {
                                        wrkFlowObj.triggerType = cn.InnerText;
                                    }
                                    else if (cn.Name == "actions")
                                    {
                                        if (cn.ChildNodes[1].InnerText == "FieldUpdate")
                                        {
                                            wrkFlowObj.wfFieldUpdates.Add(cn.ChildNodes[0].InnerText);
                                        }
                                    }
                                }

                                // Add the workflow to the dictionary
                                if (this.workflowObj.ContainsKey(wrkFlowObj.objectName))
                                {
                                    this.workflowObj[wrkFlowObj.objectName].Add(wrkFlowObj);
                                }
                                else
                                {
                                    this.workflowObj.Add(wrkFlowObj.objectName, new List<WorkflowRule> { wrkFlowObj });
                                }
                            }
                        }
                    }

                    if (wfFieldUpdates.Count > 0)
                    {
                        WorkflowRule wrkFlowObj = new WorkflowRule();
                        wrkFlowObj.objectName = flNameSplit[0];

                        foreach (XmlNode nd1 in wfFieldUpdates)
                        {
                            if (nd1.ParentNode.Name == "Workflow")
                            {
                                WorkflowFieldUpdates wfu = new WorkflowFieldUpdates();
                                wfu.objectName = flNameSplit[0];

                                foreach (XmlNode cn2 in nd1.ChildNodes)
                                {
                                    if (cn2.Name == "fullName")
                                    {
                                        wfu.fieldUpdateName = cn2.InnerText;
                                    }
                                    else if (cn2.Name == "field")
                                    {
                                        wfu.fieldName = cn2.InnerText;
                                    }
                                    else if (cn2.Name == "literalValue")
                                    {
                                        wfu.literalValue = cn2.InnerText;
                                    }
                                    else if (cn2.Name == "name")
                                    {
                                        wfu.fieldUpdateLabel = cn2.InnerText;
                                    }
                                    else if (cn2.Name == "notifyAssignee")
                                    {
                                        wfu.notifyAssignee = Boolean.Parse(cn2.InnerText);
                                    }
                                    else if (cn2.Name == "reevaluateOnChange")
                                    {
                                        wfu.reevaluateOnChange = Boolean.Parse(cn2.InnerText);
                                    }
                                }

                                if (this.workflowFldUpdates.ContainsKey(wfu.objectName))
                                {
                                    this.workflowFldUpdates[wfu.objectName].Add(wfu);
                                }
                                else
                                {
                                    this.workflowFldUpdates.Add(wfu.objectName, new List<WorkflowFieldUpdates> { wfu });
                                }
                            }
                        }
                    }
                }
            }
        }

        // TODO: Needs work
        private void runApprovalProcessExtract()
        {
            if (Directory.Exists(this.tbProjectFolder.Text + "\\approvalProcesses"))
            {

            }
        }

        private void tbProjectFolder_DoubleClick(object sender, EventArgs e)
        {
            String selectedPath = UtilityClass.folderBrowserSelectPath("Select Project Folder",
                                                                       false,
                                                                       FolderEnum.ReadFrom,
                                                                       Properties.Settings.Default.AutomationReportLastReadLocation);

            if (selectedPath != "")
            {
                this.tbProjectFolder.Text = selectedPath;
                Properties.Settings.Default.AutomationReportLastReadLocation = selectedPath;
                Properties.Settings.Default.Save();
            }
        }

        private void tbFileSaveTo_DoubleClick(object sender, EventArgs e)
        {
            String selectedPath = UtilityClass.folderBrowserSelectPath("Select Folder To Save Report To",
                                                                       true,
                                                                       FolderEnum.SaveTo,
                                                                       Properties.Settings.Default.AutomationReportLastSaveLocation);

            if (selectedPath != "")
            {
                this.tbFileSaveTo.Text = selectedPath;
                Properties.Settings.Default.AutomationReportLastSaveLocation = selectedPath;
                Properties.Settings.Default.Save();
            }
        }

        private String reduceWhitespace(String strValue)
        {
            // Remove any comments and commented code
            String fileContents1 = "";
            Char[] charArray = strValue.ToCharArray();
            Boolean inInlineComment = false;

            Boolean inMultilineComment = false;
            Int32 mlCommentNotationCount = 0;

            Boolean restResource = false;

            Int32 skipTo = 0;
            Boolean skipOne = false;
            Boolean skipOver = false;

            // Remove inline and multi-line comments
            // Consolidate @RestResource (url=
            for (Int32 i = 0; i < charArray.Length; i++)
            {
                if (skipOne == true)
                {
                    skipOne = false;
                }
                else if (skipOver == true && skipTo > i)
                {
                    // Do nothing
                }
                else if (skipOver == true && skipTo == i)
                {
                    skipTo = 0;
                    skipOver = false;
                }
                else if (charArray[i].ToString().ToLower() == "@"
                    && charArray[i + 1].ToString().ToLower() == "r"
                    && charArray[i + 2].ToString().ToLower() == "e"
                    && charArray[i + 3].ToString().ToLower() == "s"
                    && charArray[i + 4].ToString().ToLower() == "t"
                    && charArray[i + 5].ToString().ToLower() == "r"
                    && charArray[i + 6].ToString().ToLower() == "e"
                    && charArray[i + 7].ToString().ToLower() == "s")
                {
                    restResource = true;
                }
                else if (charArray[i].ToString().ToLower() == "'"
                    && charArray[i + 1].ToString().ToLower() == "*"
                    && charArray[i + 2].ToString().ToLower() == "/"
                    && charArray[i + 3].ToString().ToLower() == "*"
                    && charArray[i + 4].ToString().ToLower() == "'")
                {
                    fileContents1 = fileContents1 + charArray[i].ToString();
                    fileContents1 = fileContents1 + charArray[i + 1].ToString();
                    fileContents1 = fileContents1 + charArray[i + 2].ToString();
                    fileContents1 = fileContents1 + charArray[i + 3].ToString();
                    fileContents1 = fileContents1 + charArray[i + 4].ToString();

                    skipTo = i + 4;
                    skipOver = true;
                }
                else if (restResource == true
                    && charArray[i].ToString().ToLower() == ")")
                {
                    restResource = false;
                }
                else if (inInlineComment == false
                    && restResource == false
                    && charArray[i].ToString() == "/" && charArray[i + 1].ToString() == "*")
                {
                    inMultilineComment = true;
                    mlCommentNotationCount++;
                }
                // Inline comment, but avoid skipping over urls
                else if (i == 0
                    && inMultilineComment == false
                    && restResource == false
                    && charArray[i].ToString() == "/"
                    && charArray[i + 1].ToString() == "/")
                {
                    inInlineComment = true;
                }
                else if (i > 0
                    && inMultilineComment == false
                    && restResource == false
                    && charArray[i].ToString() == "/"
                    && charArray[i + 1].ToString() == "/"
                    && charArray[i - 1].ToString() != ":")
                {
                    inInlineComment = true;
                }
                else if (inInlineComment == true
                    && restResource == false
                    && charArray[i] == '\n')
                {
                    inInlineComment = false;
                }
                else if (inInlineComment == false
                    && restResource == false
                    && charArray[i].ToString() == "*" && charArray[i + 1].ToString() == "/")
                {
                    mlCommentNotationCount--;
                    if (mlCommentNotationCount == 0)
                    {
                        inMultilineComment = false;
                        skipOne = true;
                    }
                }
                else if (inInlineComment == false && inMultilineComment == false)
                {
                    fileContents1 = fileContents1 + charArray[i].ToString();
                }
            }

            fileContents1 = fileContents1.Replace('\t', ' ');
            fileContents1 = fileContents1.Replace('\r', ' ');
            fileContents1 = fileContents1.Replace('\n', ' ');
            fileContents1 = fileContents1.Replace("(", " ( ");
            fileContents1 = fileContents1.Replace(")", " ) ");
            fileContents1 = fileContents1.Replace("[", " [ ");
            fileContents1 = fileContents1.Replace("]", " ] ");
            fileContents1 = fileContents1.Replace("{", " { ");
            fileContents1 = fileContents1.Replace("}", " } ");
            fileContents1 = fileContents1.Replace(",", " , ");
            fileContents1 = fileContents1.Replace(":", " : ");
            fileContents1 = fileContents1.Replace(";", " ; ");
            fileContents1 = fileContents1.Replace("\\\\", " \\\\ ");
            fileContents1 = fileContents1.Replace("\\'", " \\' ");
            fileContents1 = fileContents1.Replace("'*/*'", " '*/*' ");
            fileContents1 = fileContents1.Replace("'", " ' ");
            fileContents1 = fileContents1.Replace("  ", " ");
            fileContents1 = fileContents1.Replace("  ", " ");
            fileContents1 = fileContents1.Replace("  ", " ");
            fileContents1 = fileContents1.Replace("  ", " ");
            fileContents1 = fileContents1.Replace("  ", " ");
            fileContents1 = fileContents1.Replace("  ", " ");
            fileContents1 = fileContents1.Replace("  ", " ");
            fileContents1 = fileContents1.Replace("  ", " ");
            fileContents1 = fileContents1.Replace("  ", " ");
            fileContents1 = fileContents1.Replace("  ", " ");
            fileContents1 = fileContents1.Replace("  ", " ");
            fileContents1 = fileContents1.Replace("  ", " ");
            fileContents1 = fileContents1.Replace("  ", " ");
            fileContents1 = fileContents1.Replace("  ", " ");
            fileContents1 = fileContents1.Replace("  ", " ");

            // Adjust for any space for List, Map, Set collection
            // The Character loop below will handle any additional spacing.
            // We only care about the < at this point to make sure a potential
            // collection is not Map < but is Map<
            fileContents1 = fileContents1.Replace("P <", "P<");
            fileContents1 = fileContents1.Replace("T <", "T<");
            fileContents1 = fileContents1.Replace("p <", "p<");
            fileContents1 = fileContents1.Replace("t <", "t<");

            fileContents1 = fileContents1.Trim();

            String filecontents2 = "";

            // reformat for Map, List, Set and String values
            Boolean inCollection = false;
            String collectionVar = "";
            Boolean inString = false;
            Boolean firstLessThanFound = false;
            Int32 lessThanCount = 0;

            skipTo = 0;
            skipOver = false;

            charArray = fileContents1.ToCharArray();
            for (Int32 i = 0; i < charArray.Length; i++)
            {
                if (skipOver == true && skipTo > i)
                {
                    // Don't do anything
                }
                else if (skipOver == true && skipTo == i)
                {
                    skipTo = 0;
                    skipOver = false;
                }
                else if (charArray[i].ToString() == "'"
                    && inString == false)
                {
                    filecontents2 = filecontents2 + charArray[i].ToString();
                    inString = true;
                }
                else if (charArray[i].ToString() == "'"
                    && charArray[i - 2].ToString() != "\\"
                    && inString == true)
                {
                    filecontents2 = filecontents2 + charArray[i].ToString();
                    inString = false;
                }
                else if (inString == false
                    && inCollection == true
                    && charArray[i].ToString().ToLower() != " ")
                {
                    collectionVar = collectionVar + charArray[i].ToString();

                    if (charArray[i].ToString() == "<" && firstLessThanFound == false)
                    {
                        firstLessThanFound = true;
                        lessThanCount++;
                    }
                    else if (charArray[i].ToString() == "<" && firstLessThanFound == true)
                    {
                        lessThanCount++;
                    }
                    else if (charArray[i].ToString() == ">")
                    {
                        lessThanCount--;

                        if (firstLessThanFound == true && lessThanCount == 0)
                        {
                            filecontents2 = filecontents2 + " " + collectionVar;

                            collectionVar = "";
                            inCollection = false;
                            firstLessThanFound = false;
                            lessThanCount = 0;
                        }
                    }
                }
                else if (inString == false
                    && charArray.Length - i - 1 > 4
                    && charArray[i].ToString().ToLower() == "s"
                    && charArray[i + 1].ToString().ToLower() == "e"
                    && charArray[i + 2].ToString().ToLower() == "t"
                    && charArray[i + 3].ToString().ToLower() == "<")
                {
                    inCollection = true;
                    collectionVar = collectionVar + charArray[i].ToString();
                }
                else if (inString == false
                    && charArray.Length - i - 1 > 4
                    && charArray[i].ToString().ToLower() == "m"
                    && charArray[i + 1].ToString().ToLower() == "a"
                    && charArray[i + 2].ToString().ToLower() == "p"
                    && charArray[i + 3].ToString().ToLower() == "<")
                {
                    inCollection = true;
                    collectionVar = collectionVar + charArray[i].ToString();
                }
                else if (inString == false
                   && charArray.Length - i - 1 > 5
                   && charArray[i].ToString().ToLower() == "l"
                   && charArray[i + 1].ToString().ToLower() == "i"
                   && charArray[i + 2].ToString().ToLower() == "s"
                   && charArray[i + 3].ToString().ToLower() == "t"
                   && charArray[i + 4].ToString().ToLower() == "<")
                {
                    inCollection = true;
                    collectionVar = collectionVar + charArray[i].ToString();
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == ">"
                    && charArray[i + 1].ToString() == ">"
                    && charArray[i + 2].ToString() == ">"
                    && charArray[i + 3].ToString() == "=")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + charArray[i + 1].ToString() + charArray[i + 2].ToString() + charArray[i + 3].ToString() + " ";
                    skipTo = i + 3;
                    skipOver = true;
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == ">"
                    && charArray[i + 1].ToString() == ">"
                    && charArray[i + 2].ToString() == ">")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + charArray[i + 1].ToString() + charArray[i + 2].ToString() + " ";
                    skipTo = i + 2;
                    skipOver = true;
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == ">"
                    && charArray[i + 1].ToString() == ">"
                    && charArray[i + 2].ToString() == "=")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + charArray[i + 1].ToString() + charArray[i + 2].ToString() + " ";
                    skipTo = i + 2;
                    skipOver = true;
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "="
                    && charArray[i + 1].ToString() == "="
                    && charArray[i + 2].ToString() == "=")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + charArray[i + 1].ToString() + charArray[i + 2].ToString() + " ";
                    skipTo = i + 2;
                    skipOver = true;
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "<"
                    && charArray[i + 1].ToString() == "<"
                    && charArray[i + 2].ToString() == "=")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + charArray[i + 1].ToString() + charArray[i + 2].ToString() + " ";
                    skipTo = i + 2;
                    skipOver = true;
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "!"
                    && charArray[i + 1].ToString() == "="
                    && charArray[i + 2].ToString() == "=")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + charArray[i + 1].ToString() + charArray[i + 2].ToString() + " ";
                    skipTo = i + 2;
                    skipOver = true;
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == ">"
                    && charArray[i + 1].ToString() == ">")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + charArray[i + 1].ToString() + " ";
                    skipTo = i + 1;
                    skipOver = true;
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == ">"
                    && charArray[i + 1].ToString() == "=")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + charArray[i + 1].ToString() + " ";
                    skipTo = i + 1;
                    skipOver = true;
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "<"
                    && charArray[i + 1].ToString() == "=")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + charArray[i + 1].ToString() + " ";
                    skipTo = i + 1;
                    skipOver = true;
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "="
                    && charArray[i + 1].ToString() == ">")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + charArray[i + 1].ToString() + " ";
                    skipTo = i + 1;
                    skipOver = true;
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "="
                    && charArray[i + 1].ToString() == "=")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + charArray[i + 1].ToString() + " ";
                    skipTo = i + 1;
                    skipOver = true;
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "-"
                    && charArray[i + 1].ToString() == "=")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + charArray[i + 1].ToString() + " ";
                    skipTo = i + 1;
                    skipOver = true;
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "<"
                    && charArray[i + 1].ToString() == "<")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + charArray[i + 1].ToString() + " ";
                    skipTo = i + 1;
                    skipOver = true;
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "+"
                    && charArray[i + 1].ToString() == "=")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + charArray[i + 1].ToString() + " ";
                    skipTo = i + 1;
                    skipOver = true;
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "+"
                    && charArray[i + 1].ToString() == "+")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + charArray[i + 1].ToString() + " ";
                    skipTo = i + 1;
                    skipOver = true;
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "|"
                    && charArray[i + 1].ToString() == "=")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + charArray[i + 1].ToString() + " ";
                    skipTo = i + 1;
                    skipOver = true;
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "|"
                    && charArray[i + 1].ToString() == "|")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + charArray[i + 1].ToString() + " ";
                    skipTo = i + 1;
                    skipOver = true;
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "^"
                    && charArray[i + 1].ToString() == "=")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + charArray[i + 1].ToString() + " ";
                    skipTo = i + 1;
                    skipOver = true;
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "?"
                    && charArray[i + 1].ToString() == ".")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + charArray[i + 1].ToString() + " ";
                    skipTo = i + 1;
                    skipOver = true;
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "/"
                    && charArray[i + 1].ToString() == "=")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + charArray[i + 1].ToString() + " ";
                    skipTo = i + 1;
                    skipOver = true;
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "*"
                    && charArray[i + 1].ToString() == "=")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + charArray[i + 1].ToString() + " ";
                    skipTo = i + 1;
                    skipOver = true;
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "&"
                    && charArray[i + 1].ToString() == "=")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + charArray[i + 1].ToString() + " ";
                    skipTo = i + 1;
                    skipOver = true;
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "&"
                    && charArray[i + 1].ToString() == "&")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + charArray[i + 1].ToString() + " ";
                    skipTo = i + 1;
                    skipOver = true;
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "!"
                    && charArray[i + 1].ToString() == "=")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + charArray[i + 1].ToString() + " ";
                    skipTo = i + 1;
                    skipOver = true;
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "-"
                    && charArray[i + 1].ToString() == "-")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + charArray[i + 1].ToString() + " ";
                    skipTo = i + 1;
                    skipOver = true;
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == ">")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + " ";
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "=")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + " ";
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "<")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + " ";
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "+")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + " ";
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "~")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + " ";
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "|")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + " ";
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "^")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + " ";
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "/")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + " ";
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "*")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + " ";
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "&")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + " ";
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "!")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + " ";
                }
                else if (inString == false
                    && inCollection == false
                    && charArray[i].ToString() == "-")
                {
                    filecontents2 = filecontents2 + " " + charArray[i].ToString() + " ";
                }
                else if (inCollection == false)
                {
                    filecontents2 = filecontents2 + charArray[i].ToString();
                }
            }

            filecontents2 = filecontents2.Replace("  ", " ");
            filecontents2 = filecontents2.Replace("  ", " ");
            filecontents2 = filecontents2.Replace("  ", " ");
            filecontents2 = filecontents2.Replace("  ", " ");
            filecontents2 = filecontents2.Replace("  ", " ");
            filecontents2 = filecontents2.Replace("  ", " ");
            filecontents2 = filecontents2.Replace("  ", " ");
            filecontents2 = filecontents2.Replace("  ", " ");
            filecontents2 = filecontents2.Replace("  ", " ");
            filecontents2 = filecontents2.Replace("  ", " ");

            return filecontents2;
        }

        private void parseApexTrigger(String[] filearray)
        {
            ApexTriggerStructure at = new ApexTriggerStructure();

            Boolean inTriggerEvents = false;
            Boolean inTriggerBody = false;

            Boolean inSOQLStatement = false;
            String soqlStatement = "";
            String soqlObject = "";

            // Used for Database.SaveResult srVarName = 
            String saveResultType = "";
            String saveResultVar = "";

            Int32 skipTo = 0;
            Boolean skipOver = false;

            for (Int32 i = 0; i < filearray.Length; i++)
            {
                if (skipOver == true && skipTo > i)
                {
                    // Don't do anything
                }
                else if (skipOver == true && skipTo == i)
                {
                    skipTo = 0;
                    skipOver = false;
                }
                else if (filearray[i].ToLower() == "trigger")
                {
                    at.triggerName = filearray[i + 1];
                    at.objectName = filearray[i + 3];

                    inTriggerEvents = true;
                }
                else if (inTriggerEvents == true
                    && (filearray[i].ToLower() == "before" || filearray[i].ToLower() == "after"))
                {
                    String triggerevt = filearray[i] + " " + filearray[i + 1];
                    if (triggerevt.EndsWith(","))
                    {
                        triggerevt = triggerevt.Substring(0, triggerevt.Length - 1);
                    }

                    at.triggerEvents.Add(triggerevt);

                    if (triggerevt == "before insert")
                    {
                        at.isBeforeInsert = true;
                        skipTo = i + 1;
                        skipOver = true;
                    }
                    else if (triggerevt == "before update")
                    {
                        at.isBeforeUpdate = true;
                        skipTo = i + 1;
                        skipOver = true;
                    }
                    else if (triggerevt == "before delete")
                    {
                        at.isBeforeDelete = true;
                        skipTo = i + 1;
                        skipOver = true;
                    }
                    else if (triggerevt == "after insert")
                    {
                        at.isAfterInsert = true;
                        skipTo = i + 1;
                        skipOver = true;
                    }
                    else if (triggerevt == "after update")
                    {
                        at.isAfterUpdate = true;
                        skipTo = i + 1;
                        skipOver = true;
                    }
                    else if (triggerevt == "after delete")
                    {
                        at.isAfterDelete = true;
                        skipTo = i + 1;
                        skipOver = true;
                    }
                    else if (triggerevt == "after undelete")
                    {
                        at.isAfterUndelete = true;
                        skipTo = i + 1;
                        skipOver = true;
                    }
                }
                else if (filearray[i] == "\'")
                {
                    //Debug.WriteLine(i + " " + filearray[i] + " " + filearray[i + 1] + " " + filearray[i + 2]);

                    Int32 jCount = i + 1;
                    Boolean inString = true;

                    String stringValue = filearray[i];
                    for (Int32 j = jCount; j < filearray.Length; j++)
                    {
                        stringValue = stringValue + " " + filearray[j];
                        jCount = j;

                        if (filearray[j] == "'"
                            && filearray[j - 1] != "\\"
                            && inString == true)
                        {
                            break;
                        }
                    }

                    skipTo = jCount;
                    skipOver = true;
                }
                else if (inTriggerEvents == true
                        && filearray[i].ToLower() == ")")
                {
                    inTriggerEvents = false;
                    inTriggerBody = true;
                }
                else if (filearray[i] == "="
                    && inTriggerBody == true)
                {
                    // Left side / right side method variable
                    skipTo = parseTriggerVariables(filearray, at, i);
                    skipOver = true;
                }
                //else if (filearray[i] == "!="
                //    && inTriggerBody == true)
                //{
                //    // Left side / right side method variable
                //    skipTo = parseTriggerVariables(filearray, at, i);
                //    skipOver = true;
                //}
                else if (filearray[i].ToLower() == "for"
                    && inTriggerBody == true)
                {
                    // Left side / right side method variable
                    ObjectVarToType ovt = new ObjectVarToType();
                    skipTo = parseForLoop(filearray, ovt, i);
                    skipOver = true;

                    if (at.triggerForLoops.ContainsKey(ovt.leftSideObject))
                    {
                        at.triggerForLoops[ovt.leftSideObject].Add(ovt);
                    }
                    else
                    {
                        at.triggerForLoops.Add(ovt.leftSideObject, new List<ObjectVarToType> { ovt });
                    }
                }
                else if (filearray[i].ToLower() == "throw"
                    && inTriggerBody == true)
                {
                    skipTo = bypassCharacters(filearray, i);
                    skipOver = true;
                }
                // bypass the rest of the system.assert and assertEquals
                else if (filearray[i].ToLower() == "system.assert"
                    && inTriggerBody == true)
                {
                    skipTo = bypassCharacters(filearray, i);
                    skipOver = true;
                }
                else if (filearray[i].ToLower() == "system.assertequals"
                    && inTriggerBody == true)
                {
                    skipTo = bypassCharacters(filearray, i);
                    skipOver = true;
                }
                else if (filearray[i].ToLower() == "system.assertnotequals"
                    && inTriggerBody == true)
                {
                    skipTo = bypassCharacters(filearray, i);
                    skipOver = true;
                }
                else if (filearray[i].ToLower() == "system.debug"
                    && inTriggerBody == true)
                {
                    skipTo = bypassCharacters(filearray, i);
                    skipOver = true;
                }

                else if ((filearray[i].ToLower() == "database.deleteresult"
                    || filearray[i].ToLower() == "database.mergeresult"
                    || filearray[i].ToLower() == "database.undeleteresult"
                    || filearray[i].ToLower() == "database.upsertresult"
                    || filearray[i].ToLower() == "database.saveresult")
                    && inTriggerBody == true)
                {
                    // Possible options include:
                    // Database.SaveResult[] sr = Database.update(updateBillingDocuments, false);
                    //
                    // OR another perfectly acceptable way is to declare a null Database.SaveResult[] array
                    // 
                    // Database.SaveResult[] sr;
                    // try { sr = database.update(updateBillingDocuments) } catch(Exception e) {};
                    if (filearray[i + 1] == "[")
                    {
                        saveResultType = filearray[i] + filearray[i + 1] + filearray[i + 2];
                        saveResultVar = filearray[i + 3];
                        skipTo = i + 4;
                        skipOver = true;
                    }
                    else
                    {
                        saveResultType = filearray[i];
                        saveResultVar = filearray[i + 1];
                        skipTo = i + 2;
                        skipOver = true;
                    }
                }
                else if (filearray[i].ToLower() == "select" && inSOQLStatement == false)
                {
                    inSOQLStatement = true;
                    at.logicContainedInTrigger = true;
                }
                else if (inSOQLStatement == true && filearray[i].ToLower() == "]")
                {
                    inSOQLStatement = false;

                    if (at.soqlStatements.ContainsKey(soqlObject))
                    {
                        at.soqlStatements[soqlObject].Add(soqlStatement);
                    }
                    else
                    {
                        at.soqlStatements.Add(soqlObject, new List<string> { soqlStatement });
                    }

                    soqlObject = "";
                    soqlStatement = "";
                }
                else if (inSOQLStatement == true)
                {
                    soqlStatement = soqlStatement + filearray[i] + " ";
                    if (filearray[i].ToLower() == "from")
                    {
                        soqlObject = filearray[i + 1];
                    }
                }
                else if ((filearray[i].ToLower() == "database.insert"
                    || filearray[i].ToLower() == "database.update"
                    || filearray[i].ToLower() == "database.delete"
                    || filearray[i].ToLower() == "database.undelete")
                    && inTriggerBody == true)
                {
                    at.logicContainedInTrigger = true;

                    String varName = filearray[i + 2];
                    skipTo = parseTriggerDmlVars(filearray, varName, filearray[i], saveResultType, saveResultVar, at, i + 2);
                    skipOver = true;
                }
                else if ((filearray[i].ToLower() == "insert"
                    || filearray[i].ToLower() == "update"
                    || filearray[i].ToLower() == "delete"
                    || filearray[i].ToLower() == "undelete")
                    && inTriggerBody == true)
                {
                    at.logicContainedInTrigger = true;

                    String varName = filearray[i + 1];
                    skipTo = parseTriggerDmlVars(filearray, varName, filearray[i], saveResultType, saveResultVar, at, i + 1);
                    skipOver = true;
                }
            }

            if (this.objectToTrigger.ContainsKey(at.objectName))
            {
                this.objectToTrigger[at.objectName].Add(at);
            }
            else
            {
                this.objectToTrigger.Add(at.objectName, new List<ApexTriggerStructure> { at });
            }
        }

        public Int32 parseTriggerVariables(String[] filearray, ApexTriggerStructure at, Int32 arraystart)
        {
            Int32 lastCharLocation = arraystart;

            Int32 distanceFromEqual = 0;

            // Get the left side of the equation
            // Find the distance between the = sign or what is in flarraystart
            String triggerObject = "";
            String triggerVar = "";
            for (Int32 i = arraystart - 1; i >= 0; i--)
            {
                // We also need to account for inline IF statements
                if (filearray[i] == ";"
                    || filearray[i] == "{"
                    || filearray[i] == "}"
                    || filearray[i] == ")")
                {
                    // Example: testBillingSubList[0].Billing_Contact__c = testConList1[0].Id;
                    if (arraystart - distanceFromEqual == 5)
                    {
                        triggerObject = filearray[arraystart - 5] + filearray[arraystart - 4] + filearray[arraystart - 3] + filearray[arraystart - 2];
                        triggerVar = filearray[arraystart - 1];
                    }
                    // Example: cont.MailingZip = acct.BillingZip;
                    else if (arraystart - distanceFromEqual == 4)
                    {
                        triggerObject = filearray[arraystart - 4] + filearray[arraystart - 3] + filearray[arraystart - 2];
                        triggerVar = filearray[arraystart - 1];
                    }
                    else if (arraystart - distanceFromEqual == 3)
                    {
                        //Debug.WriteLine("parseMethodVariables: arraystart - distanceFromEqual == 3 ");
                    }
                    else if (arraystart - distanceFromEqual == 2)
                    {
                        triggerObject = filearray[arraystart - 2];
                        triggerVar = filearray[arraystart - 1];
                    }
                    else if (arraystart - distanceFromEqual == 1)
                    {
                        triggerObject = filearray[arraystart - 1];
                    }

                    break;
                }
                else
                {
                    distanceFromEqual = i;
                }
            }

            // Now get the right side of the equation
            String rightSide = "";
            Boolean inStringValue = false;
            for (Int32 i = arraystart + 1; i < filearray.Length; i++)
            {
                if (filearray[i] == "'"
                    && inStringValue == false)
                {
                    inStringValue = true;
                    rightSide = rightSide + filearray[i] + " ";
                }
                else if (filearray[i] == "'"
                    && inStringValue == true)
                {
                    inStringValue = false;
                    rightSide = rightSide + filearray[i] + " ";
                }
                else if (filearray[i] == ";"
                    && inStringValue == false)
                {
                    lastCharLocation = i;

                    ObjectVarToType ovt = new ObjectVarToType();
                    ovt.leftSide = triggerObject;
                    ovt.varName = triggerVar;
                    ovt.rightSide = rightSide.Trim();

                    findObjectNameFromVar(ovt, at);

                    if (at.triggerBodyVars.ContainsKey(triggerObject))
                    {
                        at.triggerBodyVars[triggerObject].Add(ovt);
                    }
                    else
                    {
                        at.triggerBodyVars.Add(triggerObject, new List<ObjectVarToType> { ovt });
                    }

                    break;
                }
                else
                {
                    rightSide = rightSide + filearray[i] + " ";
                }
            }

            return lastCharLocation;
        }

        public Int32 parseTriggerDmlVars(String[] filearray,
            String varName,
            String dmlType,
            String saveResultType,
            String saveResultVar,
            ApexTriggerStructure at,
            Int32 arraystart)
        {
            ObjectVarToType ovt = new ObjectVarToType();
            ovt.varName = varName;
            ovt.dmlType = dmlType;
            ovt.saveResultType = saveResultType;
            ovt.saveResultVar = saveResultVar;

            String[] varNameSplit = varName.Split('.');

            Int32 lastCharLocation = arraystart;

            Boolean objectNameFound = false;

            foreach (String tpObj in at.triggerBodyVars.Keys)
            {
                foreach (ObjectVarToType objNm in at.triggerBodyVars[tpObj])
                {
                    if (objNm.varName.ToLower() == varNameSplit[0].ToLower())
                    {
                        ovt.leftSideObject = objNm.leftSideObject;
                        objectNameFound = true;
                    }

                    if (objectNameFound == true) break;
                }

                if (objectNameFound == true)
                {
                    lastCharLocation++;
                    break;
                }
            }

            if (objectNameFound == false)
            {
                foreach (String tpObj in at.triggerForLoops.Keys)
                {
                    foreach (ObjectVarToType objNm in at.triggerForLoops[tpObj])
                    {
                        if (objNm.varName.ToLower() == varNameSplit[0].ToLower())
                        {
                            ovt.leftSideObject = objNm.leftSideObject;
                            objectNameFound = true;
                        }

                        if (objectNameFound == true) break;
                    }

                    if (objectNameFound == true)
                    {
                        lastCharLocation++;
                        break;
                    }
                }
            }

            // Now loop backwards through the array to find the property if it still has not been found to get the object type this dml var is referencing
            if (objectNameFound == false)
            {
                if (filearray[arraystart].ToLower() == "[")
                {
                    ovt.varName = "";

                    String rightSide = "";
                    List<String> rightSideList = new List<String>();
                    for (Int32 i = arraystart; i < filearray.Length; i++)
                    {
                        if (filearray[i] == ";")
                        {
                            ovt.rightSide = rightSide.Trim();
                            lastCharLocation = i;

                            // Loop through the rightSideList to capture the object
                            for (Int32 s = 0; s < rightSideList.Count; s++)
                            {
                                if (rightSideList[s].ToLower() == "from")
                                {
                                    ovt.rightSideObject = rightSideList[s + 1];
                                    break;
                                }
                            }

                            break;
                        }
                        else if (filearray[i] == "[")
                        {
                            rightSide = rightSide + filearray[i] + " ";
                        }
                        else if (filearray[i] == "]")
                        {
                            rightSide = rightSide + filearray[i] + " ";
                        }
                        else
                        {
                            rightSideList.Add(filearray[i]);
                            rightSide = rightSide + filearray[i] + " ";
                        }
                    }
                }
                // Inline variable dynamically created
                else if (filearray[arraystart].ToLower() == "new")
                {
                    ovt.leftSideObject = filearray[arraystart + 1];
                    ovt.varType = filearray[arraystart + 1];

                    String rightSide = "";
                    for (Int32 i = arraystart + 2; i < filearray.Length; i++)
                    {
                        if (filearray[i] == ";")
                        {
                            ovt.rightSide = rightSide.Trim();

                            lastCharLocation = i;
                            break;
                        }
                        else
                        {
                            rightSide = rightSide + filearray[i] + " ";
                        }
                    }
                }
                // Examples:
                // insert new Account_Group__c(Name = 'Account Group 1');
                else if (filearray[arraystart - 1].ToLower() == "new")
                {
                    ovt.leftSideObject = filearray[arraystart];
                    ovt.varType = filearray[arraystart];

                    String rightSide = "";
                    for (Int32 i = arraystart + 1; i < filearray.Length; i++)
                    {
                        if (filearray[i] == ";")
                        {
                            ovt.rightSide = rightSide.Trim();

                            lastCharLocation = i;
                            break;
                        }
                        else
                        {
                            rightSide = rightSide + filearray[i] + " ";
                        }
                    }
                }
            }

            //if (filearray[lastCharLocation] == ";")
            //{
            //    lastCharLocation++;
            //}
            // Account for the allOrNone parameter in the Database.insert/update/delete
            //else
            //{
            for (Int32 i = arraystart + 1; i < filearray.Length; i++)
            {
                if (filearray[i] == ";")
                {
                    ovt.allOrNoneParameter = filearray[i - 2];
                    lastCharLocation = i;
                    break;
                }
            }
            //}

            at.triggerDmls.Add(ovt);

            return lastCharLocation;
        }

        private void parseApexClass(String[] filearray)
        {
            ApexClassStructure apexCls = new ApexClassStructure();

            HashSet<String> methodStartVariables = new HashSet<string> {"@auraenabled",
                "@deprecated",
                "@future",
                "@invocablemethod",
                "@invocablevariable",
                "@istest",
                "@readonly",
                "@remoteaction",
                "@testsetup",
                "@testvisible",
                "@httpdelete",
                "@httpget",
                "@httppatch",
                "@httppost",
                "@httpput",
                "protected",
                "private",
                "public",
                "global",
                "static"};

            Boolean inClassName = true;
            Int32 classDeclarationCount = 1;

            Int32 skipTo = 0;
            Boolean skipOver = false;

            for (Int32 i = 0; i < filearray.Length; i++)
            {
                // This is to skip over inner classes for now
                if (skipOver == true && skipTo > i)
                {
                    // Don't do anything
                }
                else if (skipOver == true && skipTo == i)
                {
                    skipTo = 0;
                    skipOver = false;
                }
                else if (inClassName == true
                         && classDeclarationCount == 1)
                {
                    if (filearray[i].ToLower() == "@istest")
                    {
                        apexCls.isTestClass = true;
                    }
                    else if (filearray[i].ToLower() == "@restresource")
                    {
                        apexCls.isRestClass = true;
                    }
                    else if (filearray[i] == "{")
                    {
                        inClassName = false;
                    }
                    else if (filearray[i].ToLower() == "protected"
                            || filearray[i].ToLower() == "private"
                            || filearray[i].ToLower() == "public"
                            || filearray[i].ToLower() == "global")
                    {
                        apexCls.accessModifier = filearray[i].ToLower();
                    }
                    else if (filearray[i].ToLower() == "virtual"
                        || filearray[i].ToLower() == "abstract")
                    {
                        apexCls.optionalModifier = filearray[i].ToLower();
                    }
                    else if (filearray[i].ToLower() == "sharing")
                    {
                        apexCls.optionalModifier = filearray[i - 1].ToLower() + " " + filearray[i].ToLower();
                    }
                    else if (filearray[i].ToLower() == "class")
                    {
                        apexCls.className = filearray[i + 1];
                        skipOver = true;
                        skipTo = i + 1;
                    }
                    else if (filearray[i].ToLower() == "interface")
                    {
                        apexCls.className = filearray[i + 1];
                        apexCls.isInterface = true;
                        skipOver = true;
                        skipTo = i + 1;
                    }
                    else if (filearray[i].ToLower() == "implements")
                    {
                        skipTo = parseImplementsInterfaces(filearray, apexCls, i);
                        skipOver = true;
                    }
                    else if (filearray[i].ToLower() == "extends")
                    {
                        apexCls.extendsClassName = filearray[i + 1];
                        skipTo = i + 1;
                        skipOver = true;
                    }
                }
                // TODO: Skipping over inner classes for now
                // Will come back and add these later
                else if (inClassName == false
                        && filearray.Length > i + 5
                        && (filearray[i].ToLower() == "protected"
                            || filearray[i].ToLower() == "private"
                            || filearray[i].ToLower() == "public"
                            || filearray[i].ToLower() == "global")
                        && (filearray[i + 1] == "class"
                            || filearray[i + 2] == "class"
                            || filearray[i + 3] == "class"
                            || filearray[i + 4] == "class"
                            || filearray[i + 5] == "class"))
                {
                    skipTo = i;
                    skipOver = true;

                    Boolean icFirstBraceReached = false;
                    Int32 braceCount = 0;
                    for (Int32 j = i; j < filearray.Length; j++)
                    {
                        if (filearray[j] == "{"
                            && icFirstBraceReached == false)
                        {
                            braceCount++;
                            icFirstBraceReached = true;
                        }
                        else if (filearray[j] == "{")
                        {
                            braceCount++;
                        }
                        else if (filearray[j] == "}")
                        {
                            braceCount--;

                            if (braceCount == 0
                            && icFirstBraceReached == true)
                            {
                                skipTo = j - 1;
                                break;
                            }
                        }
                        //else if (braceCount == 0
                        //    && icFirstBraceReached == true)
                        //{
                        //    skipTo = j - 1;
                        //    break;
                        //}
                    }
                }
                else if (inClassName == false
                    && methodStartVariables.Contains(filearray[i].ToLower()))
                {
                    skipTo = parsePropertyOrMethod(filearray, apexCls, i);
                    skipOver = true;
                }
            }

            this.classNmToClass.Add(apexCls.className, apexCls);
        }

        public Int32 parseImplementsInterfaces(String[] filearray, ApexClassStructure ac, Int32 arraystart)
        {
            Int32 lastCharLocation = arraystart;

            String interfaceName = "";
            String interfaceParameters = "";

            for (Int32 i = arraystart + 1; i < filearray.Length; i++)
            {
                if (filearray[i] == "{")
                {
                    ac.implementsInterfaces.Add(interfaceName, interfaceParameters);

                    // We don't want to skip over the class { so return 1 less than the current filearray i value
                    lastCharLocation = i - 1;
                    break;
                }
                else if (filearray[i] == ",")
                {
                    ac.implementsInterfaces.Add(interfaceName, interfaceParameters);

                    // Don't do anything. Skip over this.
                    interfaceName = "";
                    interfaceParameters = "";
                }
                else
                {
                    //ac.implementsClassNames.Add(filearray[i]);
                    if (interfaceName == "")
                    {
                        interfaceName = filearray[i];
                    }
                    else
                    {
                        interfaceParameters += filearray[i];
                    }
                }
            }

            return lastCharLocation;
        }

        public Int32 parsePropertyOrMethod(String[] filearray, ApexClassStructure ac, Int32 arraystart)
        {
            HashSet<String> methodAnnotations = new HashSet<string> {"@auraenabled",
                "@deprecated",
                "@future",
                "@invocablemethod",
                "@invocablevariable",
                "@istest",
                "@readonly",
                "@remoteaction",
                "@testsetup",
                "@testvisible",
                "@httpdelete",
                "@httpget",
                "@httppatch",
                "@httppost",
                "@httpput"};

            String propertyMethodAnnotation = "";
            String propertyMethodAnnotationParameters = "";
            String propertyMethodName = "";
            String propertyMethodQualifier = "";
            String propertyMethodRtnDataType = "";
            String propertyValue = "";

            // Used for Database.SaveResult srVarName = 
            // Key = Variable name
            // Value = Save Result Type
            //Dictionary<String, String> saveResultType = new Dictionary<String, String>();
            String saveResultType = "";
            String saveResultVar = "";

            Boolean isStatic = false;
            Boolean isFinal = false;
            Boolean isOverride = false;
            Boolean isVirtual = false;
            Boolean isTestMethod = false;

            Int32 parenthesesCount = 0; // ( )
            Int32 braceCount = 0;       // { }

            Boolean isMethod = false;
            Boolean inMethodBody = false;
            String methodParameters = "";

            Boolean isConstructor = false;

            Boolean inSOQLStatement = false;
            String soqlObject = "";
            String soqlStatement = "";

            ClassMethod cm = new ClassMethod();
            ClassProperty cp = new ClassProperty();

            // Key = Method Name - Value = SOQL Statements
            Dictionary<String, List<String>> soqlStatements = new Dictionary<String, List<String>>();

            Int32 lastCharLocation = 0;

            Int32 skipTo = 0;
            Boolean skipOver = false;

            for (Int32 i = arraystart; i < filearray.Length; i++)
            {
                //Debug.WriteLine("");

                // TODO: Bypass filearray values as they have been processed in sub-routines
                if (skipOver == true && skipTo > i)
                {
                    // Don't do anything. This is the catch-all when a skip is needed.
                }
                else if (skipOver == true && skipTo == i)
                {
                    skipTo = 0;
                    skipOver = false;
                }
                // Check for annotations and add them to the Class Method
                else if (methodAnnotations.Contains(filearray[i].ToLower()))
                {
                    propertyMethodAnnotation = filearray[i];

                    Int32 jCount = i + 1;
                    // This accounts for labels with the possibility of a ( and ) inside the single quote marks
                    // possibly throwing off when the below loop should stop and missing the real end of the annotation paremeter
                    // Ex: InvocableMethod(label='Backorder Date Change: Build and Call Requests (Email Service)')
                    Int32 parenthCount = 0;
                    String annotationParameters = "";

                    if (filearray[jCount] == "(")
                    {
                        for (Int32 j = jCount; j < filearray.Length; j++)
                        {
                            if (filearray[j] == "(")
                            {
                                parenthCount++;
                            }
                            else if (filearray[j] == ")")
                            {
                                parenthCount--;

                                if (parenthCount == 0)
                                {
                                    annotationParameters = annotationParameters + filearray[j];
                                    jCount = j;
                                    break;
                                }
                            }
                            else
                            {
                                annotationParameters = annotationParameters + filearray[j];
                            }
                        }

                        propertyMethodAnnotationParameters = annotationParameters;

                        skipTo = jCount;
                        skipOver = true;
                    }
                }
                // Loop through text values and then skip over those blocks
                else if (filearray[i] == "\'")
                {
                    //Debug.WriteLine(i + " " + filearray[i] + " " + filearray[i + 1] + " " + filearray[i + 2]);

                    Int32 jCount = i + 1;
                    Boolean inString = true;

                    String stringValue = filearray[i];
                    for (Int32 j = jCount; j < filearray.Length; j++)
                    {
                        stringValue = stringValue + " " + filearray[j];
                        jCount = j;

                        if (filearray[j] == "'"
                            && filearray[j - 1] != "\\"
                            && inString == true)
                        {
                            //Debug.WriteLine(stringValue);
                            break;
                        }
                    }

                    skipTo = jCount;
                    skipOver = true;
                }
                else if (filearray[i] == "(")
                {
                    parenthesesCount++;
                    //Debug.WriteLine(i + " parenthCount: " + parenthesesCount + " filearray[i] == (");

                    if (propertyMethodName != ""
                        && propertyMethodRtnDataType != ""
                        && inMethodBody == false)
                    {
                        isMethod = true;
                        inMethodBody = true;
                    }
                    else if (inMethodBody == false)
                    {
                        isConstructor = true;
                    }
                }
                else if (filearray[i] == ")")
                {
                    parenthesesCount--;
                    //Debug.WriteLine(i + " parenthCount: " + parenthesesCount + " filearray[i] == )");

                    // METHOD PARAMETERS
                    // Add the parameters to the methodParameters list
                    if (parenthesesCount == 0
                        && braceCount == 0
                        && methodParameters.Length > 0)
                    {
                        String[] mpSplit = methodParameters.Split(new String[] { ",", " " }, StringSplitOptions.None);

                        String methodParameters2 = "";
                        foreach (String mp in mpSplit)
                        {
                            if (mp == "[")
                            {
                                methodParameters2 = methodParameters2.Trim();
                                methodParameters2 = methodParameters2 + mp;
                            }
                            else if (mp == "]")
                            {
                                methodParameters2 = methodParameters2 + mp + " ";
                            }
                            else
                            {
                                methodParameters2 = methodParameters2 + mp + " ";
                            }
                        }

                        methodParameters2 = methodParameters2.Trim();

                        Int32 m = 0;

                        String varName = "";
                        String varType = "";
                        Boolean varIsMap = false;

                        mpSplit = methodParameters2.Split(new String[] { ",", " " }, StringSplitOptions.None);
                        foreach (String mp in mpSplit)
                        {
                            if (mp != "")
                            {
                                if (mp.ToLower().StartsWith("map<"))
                                {
                                    varType = mp + ",";
                                    varIsMap = true;
                                }
                                else if (varIsMap == true
                                    && m == 0)
                                {
                                    varType = varType + mp;
                                    m++;
                                }
                                else if (varIsMap == true
                                    && m == 1)
                                {
                                    varName = mp;
                                    m = 0;

                                    ObjectVarToType ovt = new ObjectVarToType();
                                    ovt.varType = varType;
                                    ovt.varName = varName;

                                    cm.methodParameters.Add(ovt);

                                    varType = "";
                                    varName = "";
                                    varIsMap = false;
                                }
                                else if (varIsMap == false)
                                {
                                    if (m == 0)
                                    {
                                        varType = mp;
                                        m++;
                                    }
                                    else if (m == 1)
                                    {
                                        varName = mp;
                                        m = 0;

                                        ObjectVarToType ovt = new ObjectVarToType();
                                        ovt.varType = varType;
                                        ovt.varName = varName;

                                        cm.methodParameters.Add(ovt);

                                        varType = "";
                                        varName = "";
                                    }
                                }
                            }
                        }
                    }
                    else if (parenthesesCount == 0
                        && filearray[i + 1] == ";")
                    {
                        skipTo = i + 1;
                        skipOver = true;
                    }
                }
                else if (filearray[i] == "{")
                {
                    braceCount++;
                    //Debug.WriteLine("filearray[i] == \"{\" reached: " + braceCount + " i: " + i.ToString() + " skipOver:" + skipOver + " skipTo:" + skipTo + " character:" + filearray[i].ToString());
                }
                else if (filearray[i] == "}")
                {
                    braceCount--;
                    //Debug.WriteLine("filearray[i] == \"}\" reached: " + braceCount + " i: " + i.ToString() + " skipOver:" + skipOver + " skipTo:" + skipTo + " character:" + filearray[i].ToString());

                    if (braceCount == 0
                        && isMethod == true)
                    {
                        cm.methodName = propertyMethodName;
                        cm.methodAnnotation = propertyMethodAnnotation;
                        cm.annotationParameters = propertyMethodAnnotationParameters;
                        cm.qualifier = propertyMethodQualifier;
                        cm.returnDataType = propertyMethodRtnDataType;
                        cm.isOverride = isOverride;
                        cm.isVirtual = isVirtual;
                        cm.isStatic = isStatic;
                        cm.isTestMethod = isTestMethod;
                        cm.soqlStatements = soqlStatements;

                        // Before adding the method, go through the Method variables to determine if there
                        // is a related Object associated with the left side of the equations.
                        // The only ones to look for is where the objectName is not set

                        foreach (ObjectVarToType ovt in cm.methodBodyVars)
                        {
                            if (ovt.leftSide != "")
                            {
                                findObjectNameFromVar(cm, ovt, ac);
                            }
                        }

                        // Run one last loop through the DML lines to see if there are any which are missing the 
                        // object and variable name association
                        foreach (ObjectVarToType ovt in cm.methodDmls)
                        {
                            if (ovt.leftSideObject == "")
                            {
                                findObjectNameFromVar(cm, ovt, ac);
                            }
                        }

                        ac.classMethods.Add(cm);
                        lastCharLocation = i;
                        break;
                    }
                    else if (braceCount == 0
                        && isConstructor == true)
                    {
                        lastCharLocation = i;
                        break;
                    }
                }
                // Method access
                else if (inMethodBody == false
                    && (filearray[i].ToLower() == "protected"
                    || filearray[i].ToLower() == "private"
                    || filearray[i].ToLower() == "public"
                    || filearray[i].ToLower() == "global"))
                {
                    // Debug stop filters:
                    // filearray[i+1].ToLower() == "static" && filearray[i+2].ToLower() == "void" && filearray[i+3].ToLower() == "createUnitsDecrease"
                    propertyMethodQualifier = filearray[i].ToLower();
                }
                else if (inMethodBody == false && filearray[i].ToLower() == "virtual")
                {
                    isVirtual = true;
                }
                else if (inMethodBody == false && filearray[i].ToLower() == "override")
                {
                    isOverride = true;
                }
                else if (inMethodBody == false && filearray[i].ToLower() == "static")
                {
                    isStatic = true;

                    if (filearray[i + 1].ToLower() == "testmethod")
                    {
                        isTestMethod = true;
                        skipTo = i + 1;
                        skipOver = true;
                    }

                }
                else if (inMethodBody == false && filearray[i].ToLower() == "final")
                {
                    isFinal = true;
                }
                // Method return type
                else if (parenthesesCount == 0
                    && braceCount == 0
                    && isConstructor == false
                    && propertyMethodRtnDataType == "")
                {
                    propertyMethodRtnDataType = filearray[i];

                    if (filearray[i + 1] == "[")
                    {
                        propertyMethodRtnDataType = propertyMethodRtnDataType + filearray[i + 1] + filearray[i + 2];
                        skipTo = i + 2;
                        skipOver = true;
                    }
                }
                // Method name
                else if (parenthesesCount == 0
                    && braceCount == 0
                    && isConstructor == false
                    && propertyMethodName == "")
                {
                    propertyMethodName = filearray[i];
                }
                // Method parameters
                else if (isMethod == true
                    && parenthesesCount == 1
                    && braceCount == 0)
                {
                    methodParameters = methodParameters + filearray[i] + " ";
                }
                // Class Property with value
                else if (filearray[i] == "="
                    && parenthesesCount == 0
                    && braceCount == 0
                    && isConstructor == false
                    && propertyValue == "")
                {
                    if (filearray[i + 2] == ";")
                    {
                        propertyValue = filearray[i + 1];
                        skipTo = i + 1;
                        skipOver = true;
                    }
                    else
                    {
                        for (Int32 j = i + 1; j < filearray.Length; j++)
                        {
                            if (filearray[j] == ";")
                            {
                                // We don't want to skip over the semi-colon, so setting the skipTo to the value just prior in filearray
                                skipTo = j - 1;
                                skipOver = true;
                                break;
                            }
                            else
                            {
                                propertyValue = propertyValue + " " + filearray[j];
                                propertyValue = propertyValue.Trim();
                            }
                        }
                    }
                }
                // Class Property no value
                else if (filearray[i] == ";"
                        && isMethod == false
                        && braceCount == 0
                        && parenthesesCount == 0)
                {
                    // This is a property
                    cp.propertyName = propertyMethodName;
                    cp.propertyAnnotation = propertyMethodAnnotation;
                    cp.annotationParameters = propertyMethodAnnotationParameters;
                    cp.qualifier = propertyMethodQualifier;
                    cp.dataType = propertyMethodRtnDataType;
                    cp.isFinal = isFinal;
                    cp.isStatic = isStatic;
                    cp.propertyValue = propertyValue;

                    ac.clsProperties.Add(propertyMethodName, cp);

                    lastCharLocation = i;
                    break;
                }
                // This is a getter setter property
                else if ((filearray[i].ToLower() == "get"
                    || filearray[i].ToLower() == "set")
                    && braceCount == 1)
                {
                    // Loop to the last brace and reduce the bracecount
                    Boolean inGetter = false;
                    Boolean inSetter = false;
                    String getterRtnValue = "";
                    String setterValue = "";
                    if (filearray[i].ToLower() == "get")
                    {
                        cp.isGetter = true;
                        inGetter = true;
                        inSetter = false;
                    }
                    else if (filearray[i].ToLower() == "set")
                    {
                        cp.isSetter = true;
                        inGetter = false;
                        inSetter = true;
                    }

                    // Get the return and setting values in the getter/setter
                    Int32 gsBraceCount = 1;
                    Int32 jCount = i + 1;
                    while (gsBraceCount > 0)
                    {
                        if (filearray[jCount] == "{")
                        {
                            gsBraceCount++;
                        }
                        else if (filearray[jCount] == "}")
                        {
                            gsBraceCount--;

                            if (gsBraceCount == 0)
                            {
                                break;
                            }
                        }
                        else if (filearray[jCount].ToLower() == "get")
                        {
                            cp.isSetter = true;
                            inGetter = true;
                            inSetter = false;
                        }
                        else if (filearray[jCount].ToLower() == "set")
                        {
                            cp.isSetter = true;
                            inGetter = false;
                            inSetter = true;
                        }
                        else if (filearray[jCount] != ";"
                            && inGetter == true)
                        {
                            getterRtnValue = getterRtnValue + filearray[jCount] + " ";
                        }
                        else if (filearray[jCount] != ";"
                            && inSetter == true)
                        {
                            setterValue = setterValue + filearray[jCount] + " ";
                        }

                        jCount++;
                    }

                    // This is a property
                    cp.propertyName = propertyMethodName;
                    cp.propertyAnnotation = propertyMethodAnnotation;
                    cp.annotationParameters = propertyMethodAnnotationParameters;
                    cp.qualifier = propertyMethodQualifier;
                    cp.dataType = propertyMethodRtnDataType;
                    cp.isFinal = isFinal;
                    cp.isStatic = isStatic;
                    cp.propertyValue = propertyValue;
                    cp.getterReturnValue = getterRtnValue;
                    cp.setterValue = setterValue;

                    ac.clsProperties.Add(propertyMethodName, cp);

                    braceCount = 0;
                    lastCharLocation = jCount;
                    break;
                }
                // Parse the for loops
                // We want to determine if there is a for loop first before the database.saveresult 
                // or =
                // This is because a database.saveresult can have an = to capture the returned save results
                else if (filearray[i].ToLower() == "for"
                    && inMethodBody == true)
                {
                    ObjectVarToType ovt = new ObjectVarToType();

                    // Left side / right side method variable
                    skipTo = parseForLoop(filearray, ovt, i);
                    skipOver = true;

                    if (ovt.leftSideObject != "")
                    {
                        if (cm.methodForLoops.ContainsKey(ovt.leftSideObject))
                        {
                            cm.methodForLoops[ovt.leftSideObject].Add(ovt);
                        }
                        else
                        {
                            cm.methodForLoops.Add(ovt.leftSideObject, new List<ObjectVarToType> { ovt });
                        }
                    }
                }
                else if (filearray[i].ToLower() == "throw"
                    && inMethodBody == true)
                {
                    skipTo = bypassCharacters(filearray, i);
                    skipOver = true;
                }
                else if (filearray[i].ToLower() == "database.deleteresult"
                    || filearray[i].ToLower() == "database.mergeresult"
                    || filearray[i].ToLower() == "database.undeleteresult"
                    || filearray[i].ToLower() == "database.upsertresult"
                    || filearray[i].ToLower() == "database.saveresult")
                {
                    // Possible options include:
                    // Database.SaveResult[] sr = Database.update(updateBillingDocuments, false);
                    //
                    // OR another perfectly acceptable way is to declare a null Database.SaveResult[] array
                    // 
                    // Database.SaveResult[] sr;
                    // try { sr = database.update(updateBillingDocuments) } catch(Exception e) {};

                    if (filearray[i + 1] == "[")
                    {
                        saveResultType = filearray[i] + filearray[i + 1] + filearray[i + 2];
                        saveResultVar = filearray[i + 3];
                        //saveResultType.Add(filearray[i + 3], filearray[i] + filearray[i + 1] + filearray[i + 2]);

                        skipTo = i + 4;
                        skipOver = true;
                    }
                    else
                    {
                        saveResultType = filearray[i];
                        saveResultVar = filearray[i + 1];
                        //saveResultType.Add(filearray[i + 1], filearray[i]);

                        skipTo = i + 2;
                        skipOver = true;
                    }
                }
                // Method variable with value
                else if (filearray[i] == "="
                    && inMethodBody == true)
                {
                    if (filearray[i + 1].ToLower() == "database.insert"
                        || filearray[i + 1].ToLower() == "database.update"
                        || filearray[i + 1].ToLower() == "database.delete"
                        || filearray[i + 1].ToLower() == "database.undelete"
                        || filearray[i + 1].ToLower() == "database.upsert")
                    {
                        // Don't do anything. Just skip to the next array value
                        // This is for the context where a variable is declared for "database.saveresult" before it is being used.
                        // The saveresult type and variable has already been allocated to a local variable and we do not need to capture anything here
                        // Database.SaveResult[] sr;
                        // if (!productsToUpdate.isEmpty())
                        // {
                        //     sr = database.update(productsToUpdate);
                        // }
                    }
                    else
                    {
                        // Left side / right side method variable
                        skipTo = parseMethodVariables(filearray, cm, i);
                        skipOver = true;
                    }
                }
                else if (filearray[i].ToLower() == "return"
                    && inMethodBody == true)
                {
                    skipTo = parseReturnValue(filearray, cm, i);
                    skipOver = true;
                }
                // bypass the rest of the system.assert and assertEquals
                else if (filearray[i].ToLower() == "system.assert"
                    && inMethodBody == true)
                {
                    skipTo = bypassCharacters(filearray, i);
                    skipOver = true;
                }
                else if (filearray[i].ToLower() == "system.assertequals"
                    && inMethodBody == true)
                {
                    skipTo = bypassCharacters(filearray, i);
                    skipOver = true;
                }
                else if (filearray[i].ToLower() == "system.assertnotequals"
                    && inMethodBody == true)
                {
                    skipTo = bypassCharacters(filearray, i);
                    skipOver = true;
                }
                else if (filearray[i].ToLower() == "system.debug"
                    && inMethodBody == true)
                {
                    skipTo = bypassCharacters(filearray, i);
                    skipOver = true;
                }
                else if (filearray[i].ToLower() == "system.runas")
                {
                    Int32 jCount = i + 1;

                    for (Int32 j = jCount; j < filearray.Length; j++)
                    {
                        jCount = j;
                        if (filearray[j] == ")")
                        {
                            break;
                        }
                    }

                    skipTo = jCount;
                    skipOver = true;
                }
                else if (filearray[i].ToLower() == "database.insert"
                    || filearray[i].ToLower() == "database.update"
                    || filearray[i].ToLower() == "database.delete"
                    || filearray[i].ToLower() == "database.undelete"
                    || filearray[i].ToLower() == "database.upsert")
                {
                    String[] varNameSplit = filearray[i + 2].ToLower().Split('.');
                    String varName = varNameSplit[0];

                    if (filearray[i - 1] == "=")
                    {
                        skipTo = parseMethodDmlVars(filearray,
                            varName,
                            filearray[i],
                            saveResultType,
                            saveResultVar,
                            ac,
                            cm,
                            i + 2);
                    }
                    else
                    {
                        skipTo = parseMethodDmlVars(filearray, varName, filearray[i], "", "", ac, cm, i + 2);
                    }

                    skipOver = true;
                    //saveResultType = "";
                    //saveResultVar = "";
                }
                else if (filearray[i].ToLower() == "insert"
                    || filearray[i].ToLower() == "update"
                    || filearray[i].ToLower() == "delete"
                    || filearray[i].ToLower() == "undelete"
                    || filearray[i].ToLower() == "upsert")
                {
                    String varName = "";
                    if (filearray[i + 1].ToLower() == "new")
                    {
                        varName = filearray[i + 2];
                        skipTo = parseMethodDmlVars(filearray, varName, filearray[i], "", "", ac, cm, i + 2);
                    }
                    else
                    {
                        varName = filearray[i + 1];
                        skipTo = parseMethodDmlVars(filearray, varName, filearray[i], "", "", ac, cm, i + 1);
                    }

                    skipOver = true;
                }
                // SOQL query handlers
                else if (inMethodBody == true
                    && filearray[i].ToLower() == "select" && inSOQLStatement == false)
                {
                    inSOQLStatement = true;
                }
                else if (inMethodBody == true
                    && inSOQLStatement == true && filearray[i].ToLower() == "]")
                {
                    inSOQLStatement = false;

                    if (soqlStatements.ContainsKey(soqlObject))
                    {
                        soqlStatements[soqlObject].Add(soqlStatement);
                    }
                    else
                    {
                        soqlStatements.Add(soqlObject, new List<string> { soqlStatement });
                    }

                    soqlObject = "";
                    soqlStatement = "";
                }
                else if (inMethodBody == true
                    && inSOQLStatement == true)
                {
                    soqlStatement = soqlStatement + filearray[i] + " ";
                    if (filearray[i].ToLower() == "from")
                    {
                        soqlObject = filearray[i + 1];
                    }
                }
                else if (filearray[i] == ";"
                    && inMethodBody == true)
                {
                    //Debug.WriteLine(" ");

                    // Determine if this is a declared null variable
                    if (filearray[i - 1] == "break")
                    {
                        // Add nothing
                    }
                    else if (filearray[i - 1] == "continue")
                    {
                        // Add nothing
                    }
                    else if (filearray[i - 3] == "{"
                        || filearray[i - 3] == "}"
                        || filearray[i - 3] == "("
                        || filearray[i - 3] == ")"
                        || filearray[i - 3] == ";")
                    {
                        ObjectVarToType ovt = new ObjectVarToType();
                        ovt.leftSideObject = filearray[i - 2];
                        ovt.varName = filearray[i - 1];
                        ovt.nullVariableValue = true;

                        cm.methodBodyVars.Add(ovt);
                    }
                }
            }

            return lastCharLocation;
        }

        public Int32 parseMethodDmlVars(String[] filearray,
            String varName,
            String dmlType,
            String saveResultType,
            String saveResultVar,
            ApexClassStructure ac,
            ClassMethod cm,
            Int32 arraystart)
        {
            ObjectVarToType ovt = new ObjectVarToType();
            ovt.varName = varName;
            ovt.dmlType = dmlType;
            ovt.saveResultType = saveResultType;
            ovt.saveResultVar = saveResultVar;

            String[] varNameSplit = varName.Split('.');

            Int32 lastCharLocation = arraystart;

            Boolean objectNameFound = false;

            // Loop through the method parameters and variables first
            foreach (ObjectVarToType objNm in cm.methodParameters)
            {
                if (objNm.varName.ToLower() == varNameSplit[0].ToLower())
                {
                    ovt.leftSideObject = objNm.varType;
                    ovt.varType = objNm.varType;
                    objectNameFound = true;
                }

                if (objectNameFound == true) break;
            }

            if (objectNameFound == false)
            {
                foreach (ObjectVarToType methodVar in cm.methodBodyVars)
                {
                    if (methodVar.varName.ToLower() == varNameSplit[0].ToLower())
                    {
                        ovt.leftSideObject = methodVar.leftSideObject;
                        ovt.varType = methodVar.leftSideObject;
                        objectNameFound = true;

                        lastCharLocation++;
                        break;
                    }
                }
            }

            if (objectNameFound == false)
            {
                foreach (String forLoopObj in cm.methodForLoops.Keys)
                {
                    foreach (ObjectVarToType objNm in cm.methodForLoops[forLoopObj])
                    {
                        if (objNm.varName.ToLower() == varNameSplit[0].ToLower())
                        {
                            ovt.leftSideObject = objNm.leftSideObject;
                            objectNameFound = true;
                        }

                        if (objectNameFound == true) break;
                    }

                    if (objectNameFound == true)
                    {
                        lastCharLocation++;
                        break;
                    }
                }
            }

            // Then loop through the class properties next
            if (objectNameFound == false)
            {
                foreach (String clsPropertyName in ac.clsProperties.Keys)
                {
                    if (clsPropertyName.ToLower() == varName)
                    {
                        ovt.leftSideObject = ac.clsProperties[clsPropertyName].dataType;
                        ovt.varType = ac.clsProperties[clsPropertyName].dataType;
                        objectNameFound = true;

                        lastCharLocation++;
                        break;
                    }
                }
            }

            // Now loop backwards through the array to find the property if it still has not been found to get the object type this dml var is referencing
            if (objectNameFound == false)
            {
                // Example: delete [SELECT Id FROM Custom_Field_Definition_To_Account__c WHERE Id = :relationshipId];
                if (filearray[arraystart].ToLower() == "[")
                {
                    ovt.varName = "";

                    String rightSide = "";
                    List<String> rightSideList = new List<String>();
                    for (Int32 i = arraystart; i < filearray.Length; i++)
                    {
                        if (filearray[i] == ";")
                        {
                            ovt.rightSide = rightSide.Trim();
                            lastCharLocation = i;

                            // Loop through the rightSideList to capture the object
                            for (Int32 s = 0; s < rightSideList.Count; s++)
                            {
                                if (rightSideList[s].ToLower() == "from") 
                                {
                                    ovt.rightSideObject = rightSideList[s + 1];
                                    break;
                                }
                            }

                            break;
                        }
                        else if (filearray[i] == "[")
                        {
                            rightSide = rightSide + filearray[i] + " ";
                        }
                        else if (filearray[i] == "]")
                        {
                            rightSide = rightSide + filearray[i] + " ";
                        }
                        else 
                        {
                            rightSideList.Add(filearray[i]);
                            rightSide = rightSide + filearray[i] + " ";
                        }
                    }
                }
                // Inline variable dynamically created
                else if (filearray[arraystart].ToLower() == "new")
                {
                    ovt.leftSideObject = filearray[arraystart + 1];
                    ovt.varType = filearray[arraystart + 1];

                    String rightSide = "";
                    for (Int32 i = arraystart + 2; i < filearray.Length; i++)
                    {
                        if (filearray[i] == ";")
                        {
                            ovt.rightSide = rightSide.Trim();

                            lastCharLocation = i;
                            break;
                        }
                        else
                        {
                            rightSide = rightSide + filearray[i] + " ";
                        }
                    }
                }
                // Examples:
                // insert new Account_Group__c(Name = 'Account Group 1');
                else if (filearray[arraystart - 1].ToLower() == "new")
                {
                    ovt.leftSideObject = filearray[arraystart];
                    ovt.varType = filearray[arraystart];

                    String rightSide = "";
                    for (Int32 i = arraystart + 1; i < filearray.Length; i++)
                    {
                        if (filearray[i] == ";")
                        {
                            ovt.rightSide = rightSide.Trim();

                            lastCharLocation = i;
                            break;
                        }
                        else
                        {
                            rightSide = rightSide + filearray[i] + " ";
                        }
                    }
                }
            }

            //if (filearray[lastCharLocation] == ";")
            //{
            //    lastCharLocation++;
            //}
            // Account for the allOrNone parameter in the Database.insert/update/delete
            //else
            //{
            for (Int32 i = arraystart + 1; i < filearray.Length; i++)
            {
                if (filearray[i] == ";")
                {
                    ovt.allOrNoneParameter = filearray[i - 2];
                    lastCharLocation = i;
                    break;
                }
            }
            //}

            cm.methodDmls.Add(ovt);

            return lastCharLocation;
        }

        public Int32 parseMethodVariables(String[] filearray, ClassMethod cm, Int32 arraystart)
        {
            Int32 lastCharLocation = arraystart;

            Int32 distanceFromEqual = 0;

            // Get the left side of the equation
            // Find the distance between the = sign or what is in flarraystart
            String leftSide = "";
            String rightSide = "";

            String methodObject = "";
            String methodVar = "";
            Boolean isConstant = false;
            String constantKeyword = "";

            Boolean inStringValue = false;
            for (Int32 i = arraystart - 1; i >= 0; i--)
            {
                // We also need to account for inline IF statements
                if (filearray[i] == ";"
                    || filearray[i] == "{"
                    || filearray[i] == "}"
                    || filearray[i] == ")")
                {
                    // Example: testBillingSubList[0].Billing_Contact__c = testConList1[0].Id;
                    if (arraystart - distanceFromEqual == 5)
                    {
                        methodObject = filearray[arraystart - 5] + filearray[arraystart - 4] + filearray[arraystart - 3] + filearray[arraystart - 2];
                        methodVar = filearray[arraystart - 1];
                    }
                    // Example: cont.MailingZip = acct.BillingZip;
                    else if (arraystart - distanceFromEqual == 4)
                    {
                        methodObject = filearray[arraystart - 4] + filearray[arraystart - 3] + filearray[arraystart - 2];
                        methodVar = filearray[arraystart - 1];
                    }
                    // final String ACTIVE_STATUS = ' Active ' ;
                    else if (arraystart - distanceFromEqual == 3)
                    {
                        constantKeyword = filearray[arraystart - 3];
                        if (constantKeyword.ToLower() == "final")
                        {
                            isConstant = true;
                        }

                        methodObject = filearray[arraystart - 2];
                        methodVar = filearray[arraystart - 1];
                    }
                    else if (arraystart - distanceFromEqual == 2)
                    {
                        methodObject = filearray[arraystart - 2];
                        methodVar = filearray[arraystart - 1];
                    }
                    else if (arraystart - distanceFromEqual == 1)
                    {
                        leftSide = filearray[arraystart - 1];
                    }

                    break;
                }
                else
                {
                    distanceFromEqual = i;
                }
            }

            // Now get the right side of the equation
            for (Int32 i = arraystart + 1; i < filearray.Length; i++)
            {
                if (filearray[i] == "'"
                    && inStringValue == false)
                {
                    inStringValue = true;
                    rightSide = rightSide + filearray[i] + " ";
                }
                else if (filearray[i] == "'"
                    && inStringValue == true)
                {
                    inStringValue = false;
                    rightSide = rightSide + filearray[i] + " ";
                }
                else if (filearray[i] == ";"
                    && inStringValue == false)
                {
                    lastCharLocation = i;

                    ObjectVarToType ovt = new ObjectVarToType();
                    ovt.leftSideObject = methodObject;
                    ovt.varName = methodVar;
                    ovt.isConstant = isConstant;
                    ovt.constantKeyword = constantKeyword;
                    ovt.leftSide = leftSide;
                    ovt.rightSide = rightSide.Trim();

                    cm.methodBodyVars.Add(ovt);

                    break;
                }
                else
                {
                    rightSide = rightSide + filearray[i] + " ";
                }
            }

            return lastCharLocation;
        }

        public Int32 parseForLoop(String[] filearray, ObjectVarToType ovt, Int32 arraystart)
        {
            Int32 lastCharLocation = arraystart;

            Boolean concatRightSide = false;
            String rightSide = "";
            for (Int32 i = arraystart; i < filearray.Length; i++)
            {
                if (filearray[i] == ":")
                {
                    ovt.leftSideObject = filearray[i - 2];
                    ovt.varName = filearray[i - 1];
                    ovt.isForLoop = true;

                    concatRightSide = true;
                }
                else if (filearray[i] == "{"
                    && rightSide != "")
                {
                    rightSide = rightSide.Substring(0, rightSide.Length - 2);
                    ovt.rightSide = rightSide.Trim();

                    lastCharLocation = i - 1;

                    break;
                }
                // Bypass for loops with Integer i = 0
                else if (filearray[i] == "{"
                    && rightSide == "")
                {
                    lastCharLocation = i - 1;

                    break;
                }
                else if (concatRightSide == true)
                {
                    rightSide = rightSide + filearray[i] + " ";
                }

                // We do not want to remove the reference to the start of the for loop so that the brace count can be accurate when parsing the class
                // Putting this here as it is safer when bypassing the for loops over Integer vars
                lastCharLocation = i - 1;
            }

            return lastCharLocation;
        }

        public Int32 parseReturnValue(String[] filearray, ClassMethod cm, Int32 arraystart)
        {
            Int32 lastCharLocation = arraystart;
            Boolean inString = false;

            String returnValue = "";

            for (Int32 i = arraystart + 1; i < filearray.Length; i++)
            {
                if (filearray[i] == "'"
                    && filearray[i - 1] != "\\"
                    && inString == false)
                {
                    returnValue = returnValue + filearray[i] + " ";
                    inString = true;
                }
                else if (filearray[i] == "'"
                    && filearray[i - 1] != "\\"
                    && inString == true)
                {
                    returnValue = returnValue + filearray[i] + " ";
                    inString = false;
                }
                else if (filearray[i] == ";"
                    && inString == false)
                {
                    cm.returnStatement = returnValue.Trim();
                    lastCharLocation = i;
                    break;
                }
                else
                {
                    returnValue = returnValue + filearray[i] + " ";
                }
            }

            return lastCharLocation;
        }

        public void findObjectNameFromVar(ObjectVarToType ovt, ApexTriggerStructure at)
        {
            String[] splitLeftSide = ovt.leftSide.Split('.');
            String[] splitRightSide = ovt.rightSide.Split('.');

            Boolean objectNameFound = false;

            // Loop through at.forLoops
            if (objectNameFound == false)
            {
                foreach (String forLoopObj in at.triggerForLoops.Keys)
                {
                    foreach (ObjectVarToType forLoopOvt in at.triggerForLoops[forLoopObj])
                    {
                        if (forLoopOvt.varName == splitLeftSide[0]
                            && forLoopOvt.leftSideObject != "")
                        {
                            ovt.leftSideObject = forLoopOvt.leftSideObject;
                            ovt.varName = ovt.leftSide;
                            if (splitLeftSide.Length > 1)
                            {
                                for (Int32 i = 1; i <= splitLeftSide.Length - 1; i++)
                                {
                                    ovt.varType = ovt.varType + splitLeftSide[i] + '.';
                                }

                                ovt.varType = ovt.varType.Substring(0, ovt.varType.Length - 1);
                            }

                            objectNameFound = true;

                            if (objectNameFound == true) break;
                        }
                    }

                    if (objectNameFound == true) break;
                }
            }

            // Loop through Trigger Properties
            if (objectNameFound == false)
            {
                foreach (String tpObj in at.triggerBodyVars.Keys)
                {
                    foreach (ObjectVarToType tp in at.triggerBodyVars[tpObj])
                    {
                        if (splitLeftSide[0] == tp.varName)
                        {
                            ovt.leftSideObject = tp.leftSideObject;
                            ovt.varName = ovt.leftSide;
                            if (splitLeftSide.Length > 1)
                            {
                                for (Int32 i = 1; i <= splitLeftSide.Length - 1; i++)
                                {
                                    ovt.varType = ovt.varType + splitLeftSide[i] + '.';
                                }

                                ovt.varType = ovt.varType.Substring(0, ovt.varType.Length - 1);
                            }

                            objectNameFound = true;

                            if (objectNameFound == true) break;
                        }
                    }
                }
            }

            if (objectNameFound == false)
            {
                ovt.leftSideObject = ovt.leftSide;
            }

            // Do the same for the right side
            objectNameFound = false;

            if (objectNameFound == false)
            {
                foreach (String forLoopObj in at.triggerForLoops.Keys)
                {
                    foreach (ObjectVarToType forLoopOvt in at.triggerForLoops[forLoopObj])
                    {
                        if (forLoopOvt.varName == splitRightSide[0]
                            && forLoopOvt.leftSideObject != "")
                        {
                            ovt.rightSideObject = forLoopOvt.leftSideObject;
                            objectNameFound = true;
                            if (objectNameFound == true) break;
                        }
                    }

                    if (objectNameFound == true) break;
                }
            }

            // Loop through Trigger Properties
            if (objectNameFound == false)
            {
                foreach (String tpObj in at.triggerBodyVars.Keys)
                {
                    foreach (ObjectVarToType tp in at.triggerBodyVars[tpObj])
                    {
                        if (splitRightSide[0] == tp.varName)
                        {
                            ovt.rightSideObject = tp.leftSideObject;
                            objectNameFound = true;
                            if (objectNameFound == true) break;
                        }
                    }
                }
            }
        }

        public void findObjectNameFromVar(ClassMethod cm, ObjectVarToType ovt, ApexClassStructure ac)
        {
            String[] splitLeftSide = ovt.leftSide.Split('.');
            String[] splitRightSide = ovt.rightSide.Split('.');

            Boolean objectNameFound = false;

            // Loop through cm.methodParameters
            if (objectNameFound == false)
            {
                foreach (ObjectVarToType methodParam in cm.methodParameters)
                {
                    if (splitLeftSide[0] == methodParam.varName.ToLower()
                        && methodParam.leftSideObject != "")
                    {
                        ovt.leftSideObject = methodParam.leftSideObject;
                        ovt.objectFromMethodParameter = true;
                        ovt.varName = ovt.leftSide;
                        if (splitLeftSide.Length > 1)
                        {
                            for (Int32 i = 1; i <= splitLeftSide.Length - 1; i++)
                            {
                                ovt.varType = ovt.varType + splitLeftSide[i] + '.';
                            }

                            ovt.varType = ovt.varType.Substring(0, ovt.varType.Length - 1);
                        }

                        objectNameFound = true;

                        if (objectNameFound == true) break;
                    }
                }
            }

            // Loop through cm.methodBodyVars
            if (objectNameFound == false)
            {
                foreach (ObjectVarToType methodBodyOvt in cm.methodBodyVars)
                {
                    if (splitLeftSide[0] == methodBodyOvt.varName
                        && methodBodyOvt.leftSideObject != "")
                    {
                        ovt.leftSideObject = methodBodyOvt.leftSideObject;
                        ovt.objectFromMethodVariable = true;
                        ovt.varName = ovt.leftSide;
                        if (splitLeftSide.Length > 1)
                        {
                            for (Int32 i = 1; i <= splitLeftSide.Length - 1; i++)
                            {
                                ovt.varType = ovt.varType + splitLeftSide[i] + '.';
                            }

                            ovt.varType = ovt.varType.Substring(0, ovt.varType.Length - 1);
                        }

                        objectNameFound = true;

                        if (objectNameFound == true) break;
                    }
                }
            }

            // Loop through cm.forLoops
            if (objectNameFound == false)
            {
                foreach (String forLoopObj in cm.methodForLoops.Keys)
                {
                    foreach (ObjectVarToType forLoopOvt in cm.methodForLoops[forLoopObj])
                    {
                        if (splitLeftSide[0] == forLoopOvt.varName
                            && forLoopOvt.leftSideObject != "")
                        {
                            ovt.leftSideObject = forLoopOvt.leftSideObject;
                            ovt.objectFromMethodVariable = true;
                            ovt.varName = ovt.leftSide;
                            if (splitLeftSide.Length > 1)
                            {
                                for (Int32 i = 1; i <= splitLeftSide.Length - 1; i++)
                                {
                                    ovt.varType = ovt.varType + splitLeftSide[i] + '.';
                                }

                                ovt.varType = ovt.varType.Substring(0, ovt.varType.Length - 1);
                            }

                            objectNameFound = true;

                            if (objectNameFound == true) break;
                        }
                    }

                    if (objectNameFound == true) break;
                }
            }

            // Loop through classProperties
            if (objectNameFound == false)
            {
                foreach (ClassProperty cp in ac.clsProperties.Values)
                {
                    if (splitLeftSide[0] == cp.propertyName)
                    {
                        ovt.leftSideObject = cp.dataType;
                        ovt.objectFromClassProperty = true;
                        ovt.varName = ovt.leftSide;
                        if (splitLeftSide.Length > 1)
                        {
                            for (Int32 i = 1; i <= splitLeftSide.Length - 1; i++)
                            {
                                ovt.varType = ovt.varType + splitLeftSide[i] + '.';
                            }

                            ovt.varType = ovt.varType.Substring(0, ovt.varType.Length - 1);
                        }

                        objectNameFound = true;

                        if (objectNameFound == true) break;
                    }
                }
            }

            if (objectNameFound == false)
            {
                ovt.leftSideObject = ovt.leftSide;
            }

            // Do the same for the right side
            objectNameFound = false;

            if (objectNameFound == false)
            {
                foreach (ObjectVarToType methodParam in cm.methodParameters)
                {
                    if (splitRightSide[0] == methodParam.varName.ToLower()
                        && methodParam.leftSideObject != "")
                    {
                        ovt.rightSideObject = methodParam.leftSideObject;
                        objectNameFound = true;
                        if (objectNameFound == true) break;
                    }
                }
            }

            // Loop through cm.methodBodyVars
            if (objectNameFound == false)
            {
                foreach (ObjectVarToType methodBodyOvt in cm.methodBodyVars)
                {
                    if (splitRightSide[0] == methodBodyOvt.varName
                        && methodBodyOvt.leftSideObject != "")
                    {
                        ovt.rightSideObject = methodBodyOvt.leftSideObject;
                        objectNameFound = true;
                        if (objectNameFound == true) break;
                    }
                }
            }

            // Loop through cm.forLoops
            if (objectNameFound == false)
            {
                foreach (String forLoopObj in cm.methodForLoops.Keys)
                {
                    foreach (ObjectVarToType forLoopOvt in cm.methodForLoops[forLoopObj])
                    {
                        if (splitLeftSide[0] == forLoopOvt.varName
                            && forLoopOvt.leftSideObject != "")
                        {
                            ovt.rightSideObject = forLoopOvt.leftSideObject;
                            objectNameFound = true;
                            if (objectNameFound == true) break;
                        }
                    }

                    if (objectNameFound == true) break;
                }
            }

            // Loop through classProperties
            if (objectNameFound == false)
            {
                foreach (ClassProperty cp in ac.clsProperties.Values)
                {
                    if (splitLeftSide[0] == cp.propertyName)
                    {
                        ovt.rightSideObject = cp.dataType;
                        ovt.objectFromClassProperty = true;
                        ovt.varName = ovt.leftSide;
                        if (splitLeftSide.Length > 1)
                        {
                            for (Int32 i = 1; i <= splitLeftSide.Length - 1; i++)
                            {
                                ovt.varType = ovt.varType + splitLeftSide[i] + '.';
                            }

                            ovt.varType = ovt.varType.Substring(0, ovt.varType.Length - 1);
                        }

                        objectNameFound = true;

                        if (objectNameFound == true) break;
                    }
                }
            }
        }

        public Int32 bypassCharacters(String[] filearray, Int32 arraystart)
        {
            Int32 lastCharLocation = arraystart;

            Boolean inString = false;

            for (Int32 i = arraystart; i < filearray.Length; i++)
            {
                if (filearray[i] == "'"
                    && filearray[i - 1] != "\\"
                    && inString == false)
                {
                    inString = true;
                }
                else if (filearray[i] == "'"
                    && filearray[i - 1] != "\\"
                    && inString == true)
                {
                    inString = false;
                }
                else if (filearray[i] == ";"
                    && inString == false)
                {
                    lastCharLocation = i;
                    break;
                }
            }

            return lastCharLocation;
        }



        /****
        public void writeClassValuesToDictionary()
        {
            StreamWriter apexClassWriter = new StreamWriter(this.tbFileSaveTo.Text + "\\ApexClasses.txt");

            apexClassWriter.WriteLine(
                "ClassName\t" +
                "AccessModifier\t" +
                "OptionalModifier\t" +
                "IsTestClass\t" +
                "IsInterface\t" +
                "IsRestClass\t" +
                "ExtendsClass\t" +
                "ImplementsClasses\t" +
                "ClassProperties\t" + 
                "PropertyName\t" +
                "PropertyAnnotation\t" +
                "Qualifier\t" +
                "IsStatic\t" +
                "IsFinal\t" +
                "PropertyDataType\t" +
                "PropertyValue\t" +
                "ClassMethods\t" +
                "MethodName\t" +
                "MethodAnnotation\t" +
                "MethodQualifier\t" +
                "IsStatic\t" +
                "IsOverride\t" +
                "IsVirtual\t" +
                "ReturnDataType\t" +
                "ReturnStatement\t" +
                "MethodParameters\t" +
                "MethodParameterVarType\t" +
                "MethodParameterVarName\t" +
                "MethodDMLs\t" +
                "DMLType\t" +
                "DMLObject\t" +
                "DMLVarName\t" +
                "SaveResultType\t" +
                "SaveResultVarName\t" +
                "SOQLStatements\t" +
                "SOQLObject\t" +
                "SOQLStatement"
                );

            foreach (String clsNm in this.classNmToClass.Keys)
            {
                apexClassWriter.Write(classNmToClass[clsNm].className + "\t");
                apexClassWriter.Write(classNmToClass[clsNm].accessModifier + "\t");
                apexClassWriter.Write(classNmToClass[clsNm].optionalModifier + "\t");
                apexClassWriter.Write(classNmToClass[clsNm].isTestClass + "\t");
                apexClassWriter.Write(classNmToClass[clsNm].isInterface + "\t");
                apexClassWriter.Write(classNmToClass[clsNm].isRestClass + "\t");
                apexClassWriter.Write(classNmToClass[clsNm].extendsClassName + "\t");
                
                foreach (String extCls in classNmToClass[clsNm].implementsClassNames)
                {
                    apexClassWriter.Write(extCls + ",");
                }

                apexClassWriter.Write("\t");
                apexClassWriter.Write(Environment.NewLine);

                // Write Class Properties / Variables
                foreach (String clsPropNm in classNmToClass[clsNm].clsProperties.Keys)
                {
                    for (Int32 i = 0; i < 9; i++)
                    {
                        apexClassWriter.Write("\t");
                    }

                    apexClassWriter.Write(classNmToClass[clsNm].clsProperties[clsPropNm].propertyName + "\t");
                    apexClassWriter.Write(classNmToClass[clsNm].clsProperties[clsPropNm].propertyAnnotation + "\t");
                    apexClassWriter.Write(classNmToClass[clsNm].clsProperties[clsPropNm].qualifier + "\t");
                    apexClassWriter.Write(classNmToClass[clsNm].clsProperties[clsPropNm].isStatic + "\t");
                    apexClassWriter.Write(classNmToClass[clsNm].clsProperties[clsPropNm].isFinal + "\t");
                    apexClassWriter.Write(classNmToClass[clsNm].clsProperties[clsPropNm].dataType + "\t");
                    apexClassWriter.Write(classNmToClass[clsNm].clsProperties[clsPropNm].propertyValue + "\t");
                    apexClassWriter.Write(Environment.NewLine);
                }


                // Write Class Methods
                foreach (ClassMethods clsMethod in classNmToClass[clsNm].classMethods)
                {
                    for (Int32 i = 0; i < 17; i++)
                    {
                        apexClassWriter.Write("\t");
                    }

                    apexClassWriter.Write(clsMethod.methodName + "\t");
                    apexClassWriter.Write(clsMethod.methodAnnotation + "\t");
                    apexClassWriter.Write(clsMethod.qualifier + "\t");
                    apexClassWriter.Write(clsMethod.isStatic + "\t");
                    apexClassWriter.Write(clsMethod.isOverride + "\t");
                    apexClassWriter.Write(clsMethod.isVirtual + "\t");
                    apexClassWriter.Write(clsMethod.returnDataType + "\t");
                    apexClassWriter.Write(clsMethod.returnStatement + "\t");
                    apexClassWriter.Write(Environment.NewLine);

                    if (clsMethod.methodParameters.Count > 0)
                    {
                        foreach(ObjectVarToType methParam in clsMethod.methodParameters)
                        {
                            for (Int32 i = 0; i < 25; i++)
                            {
                                apexClassWriter.Write("\t");
                            }

                            apexClassWriter.Write(methParam.varType + "\t");
                            apexClassWriter.Write(methParam.varName + "\t");
                            apexClassWriter.Write(Environment.NewLine);
                        }
                    }

                    if (clsMethod.methodDmls.Count > 0)
                    {
                        foreach (ObjectVarToType methDml in clsMethod.methodDmls)
                        {
                            for (Int32 i = 0; i < 28; i++)
                            {
                                apexClassWriter.Write("\t");
                            }
                            
                            apexClassWriter.Write(methDml.dmlType + "\t");
                            apexClassWriter.Write(methDml.leftSideObject + "\t");
                            apexClassWriter.Write(methDml.varName + "\t");
                            apexClassWriter.Write(methDml.saveResultType + "\t");
                            apexClassWriter.Write(methDml.saveResultVar + "\t");
                            apexClassWriter.Write(Environment.NewLine);
                        }
                    }

                    if (clsMethod.soqlStatements.Count > 0)
                    {
                        foreach (String soqlObj in clsMethod.soqlStatements.Keys)
                        {
                            for (Int32 i = 0; i < 33; i++)
                            {
                                apexClassWriter.Write("\t");
                            }

                            foreach (String soqlStmnt in clsMethod.soqlStatements[soqlObj])
                            {
                                apexClassWriter.Write(soqlObj + "\t");
                                apexClassWriter.Write(soqlStmnt + "\t");
                                apexClassWriter.Write(Environment.NewLine);
                            }
                        }
                    }
                }
            }

            apexClassWriter.Close();
        }
        ****/

        private void writeAutomationFieldExtractedResultsToFile()
        {
            StreamWriter sw = new StreamWriter(this.tbFileSaveTo.Text + "\\AutomationFieldExtractionReport.txt");

            if (this.objectToTrigger != null
                && this.objectToTrigger.Count > 0)
            {
                foreach (String objnm in this.objectToTrigger.Keys)
                {
                    foreach (ApexTriggerStructure at in this.objectToTrigger[objnm])
                    {
                        sw.Write(Environment.NewLine);
                        sw.Write(Environment.NewLine);
                        sw.WriteLine("ApexTrigger\tTriggerName\tObjectType\tBeforeInsert\tBeforeUpdate\tBeforeDelete\tAfterInsert\tAfterUpdate\tAfterDelete\tAfterUndelete");
                        sw.Write("\t" + at.triggerName + "\t" +
                                        objnm + "\t" +
                                        at.isBeforeInsert + "\t" +
                                        at.isBeforeUpdate + "\t" +
                                        at.isBeforeDelete + "\t" +
                                        at.isAfterInsert + "\t" +
                                        at.isAfterUpdate + "\t" +
                                        at.isAfterDelete + "\t" +
                                        at.isAfterUndelete + "\t" +
                                        Environment.NewLine);

                        if (at.triggerBodyVars.Count > 0)
                        {
                            sw.Write(Environment.NewLine);
                            sw.Write(Environment.NewLine);
                            sw.WriteLine("\tTriggerVariables\tLeftSideObject\tLeftSide\tLeftSideVarName\t=\tRightSideObject\tVariableValue");

                            foreach (String tvar in at.triggerBodyVars.Keys)
                            {
                                foreach (ObjectVarToType ovt in at.triggerBodyVars[tvar])
                                {
                                    sw.Write("\t\t" +
                                             ovt.leftSideObject + "\t" +
                                             ovt.leftSide + "\t" +
                                             ovt.varName + "\t=\t" +
                                             ovt.rightSideObject + "\t" +
                                             ovt.rightSide +
                                             Environment.NewLine);
                                }
                            }
                        }

                        if (at.triggerForLoops.Count > 0)
                        {
                            sw.Write(Environment.NewLine);
                            sw.Write(Environment.NewLine);
                            sw.WriteLine("\tTriggerForLoop\tForLoopObject\tForLoopVariable\tForLoopCollection");

                            foreach (String forLoopVar in at.triggerForLoops.Keys)
                            {
                                foreach (ObjectVarToType ovt in at.triggerForLoops[forLoopVar])
                                {
                                    sw.Write("\t\t" +
                                        ovt.leftSideObject + "\t" +
                                        ovt.varName + "\t" +
                                        ovt.rightSide +
                                        Environment.NewLine);
                                }
                            }
                        }

                        if (at.triggerDmls.Count > 0)
                        {
                            sw.Write(Environment.NewLine);
                            sw.Write(Environment.NewLine);
                            sw.WriteLine("\tTriggerDMLs\tDMLType\tObjectType\tDMLVariableName\tSaveResultType\tSaveResultVariable\tDMLValues");

                            foreach (ObjectVarToType ovt in at.triggerDmls)
                            {
                                sw.Write("\t\t" +
                                    ovt.dmlType + "\t" +
                                    ovt.leftSideObject + "\t" +
                                    ovt.varName + "\t" +
                                    ovt.saveResultType + "\t" +
                                    ovt.saveResultVar + "\t" +
                                    ovt.rightSide +
                                    Environment.NewLine);
                            }
                        }

                        if (at.soqlStatements.Count > 0) 
                        {
                            sw.Write(Environment.NewLine);
                            sw.Write(Environment.NewLine);
                            sw.WriteLine("\tTriggerSOQLStatements\tObjectQueried\tSOQLStatement");
                            foreach (String objNm in at.soqlStatements.Keys)
                            {
                                sw.Write("\t\t" +
                                    objNm +
                                    Environment.NewLine);

                                foreach (String soql in at.soqlStatements[objNm])
                                {
                                    sw.Write("\t\t\t" +
                                        soql +
                                        Environment.NewLine);
                                }

                                sw.Write(Environment.NewLine);
                            }
                        }
                    }
                }
            }

            // Write Class / Method results to file
            if (this.classNmToClass != null
                && this.classNmToClass.Count > 0)
            {
                // Skip test classes
                foreach (ApexClassStructure ac in this.classNmToClass.Values)
                {
                    if (ac.isTestClass)
                    {
                        continue;
                    }

                    sw.Write(Environment.NewLine);
                    sw.Write(Environment.NewLine);
                    sw.WriteLine("ApexClass\tClassName\tAccessModifier\tOptional Modifier\tIs Rest Class\tIs Interface\tExtends Class");
                    sw.Write("\t" +
                        ac.className + "\t" +
                        ac.accessModifier + "\t" +
                        ac.optionalModifier + "\t" +
                        ac.isRestClass + "\t" +
                        ac.isInterface + "\t" +
                        ac.extendsClassName +
                        Environment.NewLine);


                    if (ac.clsProperties.Count > 0)
                    {
                        sw.Write(Environment.NewLine);
                        sw.Write(Environment.NewLine);
                        sw.WriteLine("\tClassProperties\tPropertyQualifier\tPropertyDataType\tPropertyName\tIsGetter\tGetterReturnValue\tIsSetter\tSetterValue\tPropertyValue");

                        foreach (String cpKey in ac.clsProperties.Keys)
                        {
                            sw.Write("\t\t" +
                                ac.clsProperties[cpKey].qualifier + "\t" +
                                ac.clsProperties[cpKey].dataType + "\t" +
                                ac.clsProperties[cpKey].propertyName + "\t" +
                                ac.clsProperties[cpKey].isGetter + "\t" +
                                ac.clsProperties[cpKey].getterReturnValue + "\t" +
                                ac.clsProperties[cpKey].isSetter + "\t" +
                                ac.clsProperties[cpKey].setterValue + "\t" +
                                ac.clsProperties[cpKey].propertyValue +
                                Environment.NewLine);
                        }
                    }

                    if (ac.classMethods.Count > 0)
                    {
                        foreach (ClassMethod cm in ac.classMethods)
                        {
                            sw.Write(Environment.NewLine);
                            sw.Write(Environment.NewLine);
                            sw.WriteLine("\tClassMethod\tMethodName\tMethodQualifier\tReturnDataType\tAnnotation");

                            sw.Write("\t\t" +
                            ac.className + "." + cm.methodName + "\t" +
                            cm.qualifier + "\t" +
                            cm.returnDataType + "\t" +
                            cm.methodAnnotation +
                            Environment.NewLine);

                            if (cm.methodParameters.Count > 0)
                            {
                                sw.Write(Environment.NewLine);
                                sw.Write(Environment.NewLine);
                                sw.WriteLine("\t\tMethodParamers\tParamType\tParamName");

                                foreach (ObjectVarToType methParm in cm.methodParameters)
                                {
                                    sw.Write("\t\t\t" +
                                        methParm.varType + "\t" +
                                        methParm.varName +
                                        Environment.NewLine);
                                }
                            }

                            if (cm.methodBodyVars.Count > 0)
                            {
                                sw.Write(Environment.NewLine);
                                sw.Write(Environment.NewLine);
                                sw.WriteLine("\t\t\tMethodVariables\tObjectLeftSide\tVariableName\t=\tObjectRightSide\tVariableValue");

                                foreach (ObjectVarToType ovt in cm.methodBodyVars)
                                {
                                    sw.Write("\t\t\t\t");
                                    sw.Write(ovt.leftSideObject + "\t");
                                    sw.Write(ovt.varName + "\t");
                                    if (ovt.nullVariableValue == false)
                                    {
                                        sw.Write("=\t");
                                        sw.Write(ovt.rightSideObject + "\t");
                                        sw.Write(ovt.rightSide);
                                    }

                                    sw.Write(Environment.NewLine);
                                }
                            }

                            if (cm.methodForLoops.Count > 0)
                            {
                                sw.Write(Environment.NewLine);
                                sw.Write(Environment.NewLine);
                                sw.WriteLine("\t\t\tMethodForLoop\tForLoopObject\tForLoopVariable\tForLoopCollection");

                                foreach (String forLoopVar in cm.methodForLoops.Keys)
                                {
                                    foreach (ObjectVarToType ovt in cm.methodForLoops[forLoopVar])
                                    {
                                        sw.Write("\t\t\t\t" +
                                            ovt.leftSideObject + "\t" +
                                            ovt.varName + "\t" +
                                            ovt.rightSide +
                                            Environment.NewLine);
                                    }
                                }
                            }

                            if (cm.methodDmls.Count > 0)
                            {
                                sw.Write(Environment.NewLine);
                                sw.Write(Environment.NewLine);
                                sw.WriteLine("\t\t\tMethodDMLs\tDMLType\tObjectType\tDMLVariableName\tSaveResultType\tSaveResultVariable\tDMLValues");

                                foreach (ObjectVarToType ovt in cm.methodDmls)
                                {
                                    sw.Write("\t\t\t\t" +
                                        ovt.dmlType + "\t" +
                                        ovt.varType + "\t" +
                                        ovt.varName + "\t" +
                                        ovt.saveResultType + "\t" +
                                        ovt.saveResultVar + "\t" +
                                        ovt.rightSide +
                                        Environment.NewLine);
                                }
                            }

                            if (cm.soqlStatements.Count > 0)
                            {
                                sw.Write(Environment.NewLine);
                                sw.Write(Environment.NewLine);
                                sw.WriteLine("\t\t\tSOQLStatements\tObjectQueried\tSOQLStatement");
                                foreach (String objNm in cm.soqlStatements.Keys)
                                {
                                    sw.Write("\t\t\t\t" +
                                        objNm +
                                        Environment.NewLine);

                                    foreach (String soql in cm.soqlStatements[objNm])
                                    {
                                        sw.Write("\t\t\t\t\t" +
                                            soql +
                                            Environment.NewLine);
                                    }

                                    sw.Write(Environment.NewLine);
                                }
                            }

                        }
                    }

                    sw.Write(Environment.NewLine);
                    sw.Write(Environment.NewLine);
                    sw.WriteLine("\tReferenced In (This is a general search and may contain results which are not exact)");

                    List<String> results = SearchForm.getSearchResults(ac.className, this.tbProjectFolder.Text, false, true, false);
                    foreach (String res in results)
                    {
                        if (res.StartsWith("Line:"))
                        {
                            sw.Write("\t\t\t\t" + res);
                            sw.Write(Environment.NewLine);
                        }
                        else
                        {
                            sw.Write(Environment.NewLine);
                            sw.Write("\t\t" + res);
                            sw.Write(Environment.NewLine);
                        }
                    }
                }
            }

            // Write Flow results to file
            if (this.objectToFlow != null
                && this.objectToFlow.Count > 0)
            {
                foreach (String objName in this.objectToFlow.Keys)
                {
                    sw.Write(Environment.NewLine);
                    sw.Write(Environment.NewLine);
                    sw.Write("Flow_Process_Workflow\t" +
                        "ObjectName\t" +
                        "FlowApiName\t" +
                        "FlowLabel\t" +
                        "ProcessType\t" +
                        "APIVersion" +
                        Environment.NewLine);

                    foreach (FlowProcess fp in objectToFlow[objName])
                    {
                        sw.Write(Environment.NewLine);

                        sw.Write("\t" +
                            objName + "\t" +
                            fp.apiName + "\t" +
                            fp.label + "\t" +
                            fp.flowProcessType + "\t" +
                            fp.apiVersion +
                            Environment.NewLine);

                        if (fp.variableToObject.Count > 0)
                        {
                            sw.Write(Environment.NewLine);
                            sw.Write("\t\tFlowVariables\t" +
                                "FlowVariableName\t" +
                                "ApexClass\t" +
                                "FlowObjectType\t" +
                                "FlowDataType\t" +
                                "IsCollection\t" +
                                "IsInput\t" +
                                "IsOutput" +
                                Environment.NewLine);

                            foreach (FlowVariable fv in fp.variableToObject.Values)
                            {
                                sw.Write("\t\t\t" +
                                    fv.varName + "\t" +
                                    fv.apexClass + "\t" +
                                    fv.objectType + "\t" +
                                    fv.dataType + "\t" +
                                    fv.isCollection + "\t" +
                                    fv.isInput + "\t" +
                                    fv.isOutput +
                                    Environment.NewLine);
                            }
                        }

                        if (fp.actionCalls.Count > 0)
                        {
                            sw.Write(Environment.NewLine);
                            sw.Write("\t\tFlowActionCalls\t" +
                                "ActionCallAPIName\t" +
                                "ActionCallLabel\t" +
                                "ActionName\t" +
                                "ActionType\t" +
                                "InputParameters\t" +
                                "InputParameterName\t" +
                                "InputParameterValue\t" +
                                Environment.NewLine);

                            foreach (FlowActionCalls fac in fp.actionCalls)
                            {
                                sw.Write("\t\t\t" +
                                    fac.actionCallAPIName + "\t" +
                                    fac.actionCallLabel + "\t" +
                                    fac.actionName + "\t" +
                                    fac.actionType + "\t\t");

                                Int32 i = 0;
                                foreach (FlowActionInputParameters faip in fac.inputParameters)
                                {
                                    if (i == 0)
                                    {
                                        sw.Write(faip.apiName + "\t" + faip.value + Environment.NewLine);
                                    }
                                    else
                                    {
                                        sw.Write("\t\t\t\t\t\t\t\t" + faip.apiName + "\t" + faip.value + Environment.NewLine);
                                    }

                                    i++;
                                }

                                sw.Write(Environment.NewLine);
                            }
                        }

                        sw.Write(Environment.NewLine);
                        sw.Write(Environment.NewLine);
                        sw.WriteLine("\t\t\tReferenced In (This is a general search and may contain results which are not exact)");

                        List<String> results = SearchForm.getSearchResults(fp.apiName, this.tbProjectFolder.Text, false, true, false);
                        foreach (String res in results)
                        {
                            if (res.StartsWith("Line:"))
                            {
                                sw.Write("\t\t\t\t\t\t" + res);
                                sw.Write(Environment.NewLine);
                            }
                            else
                            {
                                sw.Write(Environment.NewLine);
                                sw.Write("\t\t\t\t" + res);
                                sw.Write(Environment.NewLine);
                            }
                        }


                        sw.Write(Environment.NewLine);
                    }
                }
            }

            //if (this.workflowObjToFieldUpdt != null
            //    && this.workflowObjToFieldUpdt.Count > 0)
            //{

            //}

            // Write Workflow results to file
            if (this.workflowFldUpdates != null
                && this.workflowFldUpdates.Count > 0)
            {
                sw.Write(Environment.NewLine);
                sw.Write(Environment.NewLine);
                sw.Write("WorkflowFieldUpdate\t" +
                    "ObjectName\t" +
                    "FieldUpdateApiName\t" +
                    "FieldUpdateLabel\t" +
                    "FieldApiName\t" +
                    "LiteralValue\t" +
                    "NotifyAssignee\t" +
                    "ReEvaludateOnChange" +
                    Environment.NewLine);

                foreach (String objName in this.workflowFldUpdates.Keys)
                {
                    sw.Write("\t" + objName + Environment.NewLine);

                    foreach (WorkflowFieldUpdates wfu in this.workflowFldUpdates[objName])
                    {
                        sw.Write("\t\t" +
                            wfu.fieldUpdateName + "\t" +
                            wfu.fieldUpdateLabel + "\t" +
                            wfu.fieldName + "\t" +
                            wfu.literalValue + "\t" +
                            wfu.notifyAssignee + "\t" +
                            wfu.reevaluateOnChange +
                            Environment.NewLine);
                    }
                }
            }

            sw.Close();
        }
        /// <summary>
        /// There will be a lot of recursive loops here so might need to find a way to make this more efficient
        /// Parse the triggers and class-methods (done above)
        /// Loop through the Apex Class Methods and if there are methodDmls, then loop through those
        ///      Upserts will have to be handled as an Insert and Update
        ///         database.insert
        ///         database.update
        ///         database.delete
        ///         database.undelete
        ///         database.upsert
        ///         
        ///         insert
        ///         update
        ///         delete
        ///         undelete
        ///         upsert
        /// 
        /// Find the trigger in the mix with the related sObject and find the related automation pieces in the triggers and related class/method calls for the operation.
        /// </summary>
        private void writeAutomationHierarchyResults()
        {
            StreamWriter sw = new StreamWriter(this.tbFileSaveTo.Text + "\\AutomationHierarchyReport.txt");
            //sw.WriteLine("ObjectName\tDMLType\tIsList\tAutomation Paths");

            foreach (ApexClassStructure ac in classNmToClass.Values)
            {
                if (ac.classMethods.Count > 0) 
                {
                    sw.WriteLine(ac.className + '\t' + ac.accessModifier + '\t' + ac.optionalModifier + '\t' + ac.isTestClass);

                    foreach (ClassMethod cm in ac.classMethods) 
                    {
                        if (cm.methodDmls.Count > 0) 
                        {
                            foreach (ObjectVarToType mdml in cm.methodDmls)
                            {
                                String objectName = "";
                                Boolean isList = false;

                                if (mdml.leftSideObject != "")
                                {
                                    String[] splitObj = mdml.leftSideObject.Split(new char[2] { '<', '>' });
                                    // List
                                    if (splitObj.Length == 3)
                                    {
                                        objectName = splitObj[1];
                                    }
                                    else if (splitObj.Length == 1)
                                    {
                                        objectName = splitObj[0];
                                    }
                                    //else
                                    //{
                                    //    Debug.WriteLine("");
                                    //}
                                }
                                else if (mdml.rightSideObject != "")
                                {
                                    String[] splitObj = mdml.rightSideObject.Split(new char[2] { '<', '>' });
                                    // List
                                    if (splitObj.Length == 3)
                                    {
                                        objectName = splitObj[1];
                                    }
                                    else if (splitObj.Length == 1)
                                    {
                                        objectName = splitObj[0];
                                    }
                                    //else
                                    //{
                                    //    Debug.WriteLine("");
                                    //}
                                }

                                sw.WriteLine('\t' + objectName + '\t' + mdml.dmlType + '\t' + isList.ToString());
                                writeRelatedApexTrigger(objectName, sw, 2);
                            }
                        }
                    }

                    sw.Write(Environment.NewLine);
                }
            }

            sw.Close();
        }

        private void writeRelatedApexTrigger(String objectName, StreamWriter sw, Int32 tabCount)
        {
            //Int32 tabCount = 2;

            if (objectToTrigger.ContainsKey(objectName))
            {
                List<ApexTriggerStructure> triggerStruct = objectToTrigger[objectName];
                foreach (ApexTriggerStructure at in triggerStruct) 
                {
                    writeTabs(tabCount, sw);
                    sw.Write("Apex Trigger" + '\t' + at.triggerName);
                    foreach (String triggerEvt in at.triggerEvents)
                    {
                        sw.Write('\t' + triggerEvt);
                    }

                    sw.Write(Environment.NewLine);
                }
            }

            sw.Write(Environment.NewLine);

            if (objectToFlow.ContainsKey(objectName)) 
            {
                List<FlowProcess> objFlowProc = objectToFlow[objectName];
                foreach (FlowProcess fp in objFlowProc)
                {
                    if (fp.isActive == true && fp.flowProcessType == "AutoLaunchedFlow")
                    {
                        writeTabs(tabCount, sw);
                        sw.WriteLine("Record Triggered Flow" + '\t' + fp.label + '\t' + fp.apiName + '\t' + fp.triggerType + '\t' + fp.recordTriggerType + '\t' + fp.runInMode);
                    }
                    //else if (fp.isActive == true)
                    //{
                    //    Debug.WriteLine("");
                    //}
                }
            }

            sw.Write(Environment.NewLine);
        }
        private void writeTabs(Int32 tabCount, StreamWriter sw)
        {
            for (Int32 i = 0; i < tabCount; i++)
            {
                sw.Write('\t');
            }
        }
        private void writeAutomationNamesToFile()
        {
            StreamWriter sw = new StreamWriter(this.tbFileSaveTo.Text + "\\AutomationNamesExtractionReport.txt");

            if (this.objectToTrigger != null
                && this.objectToTrigger.Count > 0)
            {
                foreach (String objnm in this.objectToTrigger.Keys)
                {
                    foreach (ApexTriggerStructure at in this.objectToTrigger[objnm])
                    {
                        sw.Write(Environment.NewLine);
                        sw.Write(Environment.NewLine);
                        sw.WriteLine("ApexTrigger\tTriggerName\tObjectType\tBeforeInsert\tBeforeUpdate\tBeforeDelete\tAfterInsert\tAfterUpdate\tAfterDelete\tAfterUndelete");
                        sw.Write("\t" + at.triggerName + "\t" +
                                        objnm + "\t" +
                                        at.isBeforeInsert + "\t" +
                                        at.isBeforeUpdate + "\t" +
                                        at.isBeforeDelete + "\t" +
                                        at.isAfterInsert + "\t" +
                                        at.isAfterUpdate + "\t" +
                                        at.isAfterDelete + "\t" +
                                        at.isAfterUndelete + "\t" +
                                        Environment.NewLine);

                        if (at.triggerDmls.Count > 0)
                        {
                            sw.Write(Environment.NewLine);
                            sw.Write(Environment.NewLine);
                            sw.WriteLine("\tTriggerDMLs\tDMLType\tObjectType\tDMLVariableName\tSaveResultType\tSaveResultVariable\tDMLValues");

                            foreach (ObjectVarToType ovt in at.triggerDmls)
                            {
                                sw.Write("\t\t" +
                                    ovt.dmlType + "\t" +
                                    ovt.leftSideObject + "\t" +
                                    ovt.varName + "\t" +
                                    ovt.saveResultType + "\t" +
                                    ovt.saveResultVar + "\t" +
                                    ovt.rightSide +
                                    Environment.NewLine);
                            }
                        }
                    }
                }
            }

            // Write Class / Method results to file
            if (this.classNmToClass != null
                && this.classNmToClass.Count > 0)
            {
                // Skip test classes
                foreach (ApexClassStructure ac in this.classNmToClass.Values)
                {
                    if (ac.isTestClass)
                    {
                        continue;
                    }

                    sw.Write(Environment.NewLine);
                    sw.Write(Environment.NewLine);
                    sw.WriteLine("ApexClass\tClassName\tAccessModifier\tOptional Modifier\tIs Rest Class\tIs Interface\tExtends Class");
                    sw.Write("\t" +
                        ac.className + "\t" +
                        ac.accessModifier + "\t" +
                        ac.optionalModifier + "\t" +
                        ac.isRestClass + "\t" +
                        ac.isInterface + "\t" +
                        ac.extendsClassName +
                        Environment.NewLine);

                    if (ac.clsProperties.Count > 0)
                    {
                        sw.Write(Environment.NewLine);
                        sw.Write(Environment.NewLine);
                        sw.WriteLine("\tClassProperties\tPropertyQualifier\tPropertyDataType\tPropertyName\tIsGetter\tGetterReturnValue\tIsSetter\tSetterValue\tPropertyValue");

                        foreach (String cpKey in ac.clsProperties.Keys)
                        {
                            sw.Write("\t\t" +
                                ac.clsProperties[cpKey].qualifier + "\t" +
                                ac.clsProperties[cpKey].dataType + "\t" +
                                ac.clsProperties[cpKey].propertyName + "\t" +
                                ac.clsProperties[cpKey].isGetter + "\t" +
                                ac.clsProperties[cpKey].getterReturnValue + "\t" +
                                ac.clsProperties[cpKey].isSetter + "\t" +
                                ac.clsProperties[cpKey].setterValue + "\t" +
                                ac.clsProperties[cpKey].propertyValue +
                                Environment.NewLine);
                        }
                    }

                    if (ac.classMethods.Count > 0)
                    {
                        foreach (ClassMethod cm in ac.classMethods)
                        {
                            sw.Write(Environment.NewLine);
                            sw.Write(Environment.NewLine);
                            sw.WriteLine("\tClassMethod\tMethodName\tMethodQualifier\tReturnDataType\tAnnotation");

                            sw.Write("\t\t" +
                            ac.className + "." + cm.methodName + "\t" +
                            cm.qualifier + "\t" +
                            cm.returnDataType + "\t" +
                            cm.methodAnnotation +
                            Environment.NewLine);

                            if (cm.methodParameters.Count > 0)
                            {
                                sw.Write(Environment.NewLine);
                                sw.Write(Environment.NewLine);
                                sw.WriteLine("\t\tMethodParamers\tParamType\tParamName");

                                foreach (ObjectVarToType methParm in cm.methodParameters)
                                {
                                    sw.Write("\t\t\t" +
                                        methParm.varType + "\t" +
                                        methParm.varName +
                                        Environment.NewLine);
                                }
                            }

                            if (cm.methodDmls.Count > 0)
                            {
                                sw.Write(Environment.NewLine);
                                sw.Write(Environment.NewLine);
                                sw.WriteLine("\t\t\tMethodDMLs\tDMLType\tObjectType\tDMLVariableName\tSaveResultType\tSaveResultVariable\tDMLValues");

                                foreach (ObjectVarToType ovt in cm.methodDmls)
                                {
                                    sw.Write("\t\t\t\t" +
                                        ovt.dmlType + "\t" +
                                        ovt.varType + "\t" +
                                        ovt.varName + "\t" +
                                        ovt.saveResultType + "\t" +
                                        ovt.saveResultVar + "\t" +
                                        ovt.rightSide +
                                        Environment.NewLine);
                                }
                            }
                        }
                    }
                }
            }

            // Write Flow results to file
            if (this.objectToFlow != null
                && this.objectToFlow.Count > 0)
            {
                sw.Write(Environment.NewLine);
                sw.Write(Environment.NewLine);
                sw.Write("Flow_Process_Workflow\t" +
                    "ObjectName\t" +
                    "FlowApiName\t" +
                    "FlowLabel\t" +
                    "ProcessType\t" +
                    "APIVersion" +
                    Environment.NewLine);

                foreach (String objName in this.objectToFlow.Keys)
                {
                    if (objName != "")
                    {
                        foreach (FlowProcess fp in objectToFlow[objName])
                        {
                            sw.Write(Environment.NewLine);

                            sw.Write("\t" +
                                objName + "\t" +
                                fp.apiName + "\t" +
                                fp.label + "\t" +
                                fp.flowProcessType + "\t" +
                                fp.apiVersion +
                                Environment.NewLine);
                        }
                    }
                }
            }


            sw.Close();
        }
        public class ObjectToFields 
        {
            public String objectName = "";
            public String objectLabel = "";
            public String pluralLabel = "";
            public List<String> fields;
            // These are the fields which exist on the object itself
            public List<ObjectFields> objFields;
            // These are the fields which are referenced from the current object either to fields on that object itself or in a Field Set or Validation rule
            public List<ObjectFields> objFieldReferences;
            public List<ObjectValidations> objValidations;
            public String sharingModel = "";
            public String visibility = "";
            public Int32 fieldCount = 0;
            public Int32 fieldsTrackedCount = 0;
            public Int32 formulaFieldCount = 0;
            public Boolean isCustomSetting = false;
            public String customSettingType = "";
            public Dictionary<String, List<String>> fieldSetToFieldNames;
            public Dictionary<String, List<String>> compactLayoutToFieldNames;

            public ObjectToFields()
            {
                fields = new List<String>();
                objFields = new List<ObjectFields>();
                objFieldReferences = new List<ObjectFields>();
                objValidations = new List<ObjectValidations>();
                fieldSetToFieldNames = new Dictionary<String, List<String>>();
                compactLayoutToFieldNames = new Dictionary<string, List<string>>();
            }
        }

        public class ObjectFields
        {
            public String sObjectName = "";
            public String fullName = "";
            public String externalId = "";
            public String label = "";
            public String length = "";
            public String required = "";
            public String defaultValue = "";
            public Boolean trackHistory = false;
            public String type = "";
            public String precision = "";
            public String scale = "";
            public String unique = "";
            public String referenceTo = "";
            public String deleteConstraint = "";
            public String relationshipLabel = "";
            public String relationshipName = "";
            public Boolean isFormula = false;
            public String formula = "";
            public String picklistValueSet = "";

            // Object name 
            // public String referenceToObject = "";
            // Field Set, Validation Rule, Formula Field
            // public String referenceFromObjectSection = "";
        }

        public class ObjectValidations
        {
            public String sObjectName = "";
            public String validationName = "";
            public String errorConditionFormula = "";
            public Boolean isActive = false;
        }

        public class ApexTriggerStructure
        {
            public String triggerName = "";
            public String objectName = "";

            public Boolean logicContainedInTrigger = false;

            public Boolean isBeforeInsert = false;
            public Boolean isBeforeUpdate = false;
            public Boolean isBeforeDelete = false;
            
            public Boolean isAfterInsert = false;
            public Boolean isAfterUpdate = false;
            public Boolean isAfterDelete = false;
            public Boolean isAfterUndelete = false;

            public HashSet<String> triggerEvents;
            public Dictionary<String, List<String>> soqlStatements;

            public Dictionary<String, List<String>> classMethodCalls;
            public Dictionary<String, List<ObjectVarToType>> triggerForLoops;
            public Dictionary<String, List<ObjectVarToType>> triggerBodyVars;
            public List<ObjectVarToType> triggerDmls;

            public ApexTriggerStructure()
            {
                triggerEvents = new HashSet<string>();
                soqlStatements = new Dictionary<String, List<String>>();
                classMethodCalls = new Dictionary<string, List<string>>();
                triggerForLoops = new Dictionary<string, List<ObjectVarToType>>();
                triggerBodyVars = new Dictionary<String, List<ObjectVarToType>>();
                triggerDmls = new List<ObjectVarToType>();
            }
        }

        public class ApexClassStructure
        {
            public string className = "";
            public string accessModifier = "";
            public string optionalModifier = "";

            public Boolean isInterface = false;
            public Boolean isVirtual = false;
            public Boolean isTestClass = false;
            public Boolean isRestClass = false;

            //public List<String> interfaceClassNames;
            public Dictionary<String, String> implementsInterfaces;
            public String extendsClassName = "";
            public Dictionary<String, ClassProperty> clsProperties;
            public Dictionary<String, ApexClassStructure> innerClasses;
            public List<ClassMethod> classMethods;
            public Dictionary<String, String> objToFieldsReferenced;

            public ApexClassStructure()
            {
                //this.interfaceClassNames = new List<String>();
                this.implementsInterfaces = new Dictionary<String, String>();
                this.clsProperties = new Dictionary<string, ClassProperty>();
                this.innerClasses = new Dictionary<string, ApexClassStructure>();
                this.classMethods = new List<ClassMethod>();
                this.objToFieldsReferenced = new Dictionary<String, String>();
            }
        }

        public class ObjectVarToType
        {
            // This is the sObject name being referenced if there is one
            public String leftSideObject = "";
            public String rightSideObject = "";

            public String leftSide = "";
            public String rightSide = "";

            // This is a system type being reference: i.e. String, Double, Integer, etc.
            public String varType = "";

            // This is the variable name assigned to the sObject or system type
            public String varName = "";

            public String dmlType = "";
            public String saveResultType = "";
            public String saveResultVar = "";
            public String allOrNoneParameter = "";

            public Boolean isForLoop = false;
            public Boolean isConstant = false;
            public String constantKeyword = "";

            public Boolean nullVariableValue = false;
            public Boolean objectFromClassProperty = false;
            public Boolean objectFromMethodParameter = false;
            public Boolean objectFromMethodVariable = false;
            public Boolean objectFromMethodForLoop = false;
            public Boolean objectFromInlineVariable = false;
        }

        public class ClassProperty 
        {
            public String propertyName = "";
            public String propertyAnnotation = "";
            public String annotationParameters = "";
            public String qualifier = "";
            public String dataType = "";
            public Boolean isStatic = false;
            public Boolean isFinal = false;
            public String propertyValue = "";
            public Boolean isGetter = false;
            public String getterReturnValue = "";
            public Boolean isSetter = false;
            public String setterValue = "";
        }

        public class ClassMethod
        {
            public String methodName = "";
            public String methodAnnotation = "";
            public String annotationParameters = "";
            public String qualifier = "";
            public Boolean isStatic = false;
            public Boolean isOverride = false;
            public Boolean isVirtual = false;
            public Boolean isTestMethod = false;
            public String returnDataType = "";
            public String returnStatement = "";

            // Key = sObject API Name, Value = SOQL Statements
            public List<ObjectVarToType> methodParameters;
            public List<ObjectVarToType> methodBodyVars;
            public Dictionary<String, List<ObjectVarToType>> methodForLoops;
            public List<ObjectVarToType> methodDmls;
            public Dictionary<String, List<String>> soqlStatements;

            public ClassMethod() 
            {
                this.methodParameters = new List<ObjectVarToType>();
                this.methodBodyVars = new List<ObjectVarToType>();
                this.methodForLoops = new Dictionary<String, List<ObjectVarToType>>();
                this.methodDmls = new List<ObjectVarToType>();
                this.soqlStatements = new Dictionary<String, List<String>>();
            }
        }

        public class FlowProcess 
        {
            public String apiName = "";
            public String label = "";
            public String objectName = "";
            public String flowProcessType = "";
            public String recordTriggerType = "";
            public String triggerType = "";
            public Boolean isActive = false;
            public String apiVersion = "";
            public String runInMode = "";

            public Dictionary<String, FlowVariable> variableToObject;
            public Dictionary<String, List<String>> recordCreates;
            public Dictionary<String, List<String>> recordUpdates;
            public Dictionary<String, List<String>> recordDeletes;

            public List<FlowActionCalls> actionCalls;

            public FlowProcess()
            {
                variableToObject = new Dictionary<String, FlowVariable>();
                recordCreates = new Dictionary<string, List<string>>();
                recordUpdates = new Dictionary<string, List<string>>();
                recordDeletes = new Dictionary<string, List<string>>();

                actionCalls = new List<FlowActionCalls>();
            }
        }

        public class FlowVariable
        {
            public String varName = "";
            public String apexClass = "";
            public String dataType = "";
            public String objectType = "";
            public Boolean isCollection = false;
            public Boolean isInput = false;
            public Boolean isOutput = false;
        }

        public class FlowActionCalls
        {
            public String actionCallAPIName = "";
            public String actionCallLabel = "";
            public String actionName = "";
            public String actionType = "";
            public List<FlowActionInputParameters> inputParameters;

            public FlowActionCalls()
            {
                inputParameters = new List<FlowActionInputParameters>();
            }
        }

        public class FlowActionInputParameters
        {
            public String apiName = "";
            public String value = "";
        }

        public class WorkflowRule
        {
            // These are the rules
            public String objectName = "";
            public String workflowRuleName = "";
            public String triggerType = "";
            public Boolean isActive = false;
            public HashSet<String> wfFieldUpdates;

            public WorkflowRule()
            {
                wfFieldUpdates = new HashSet<String>();
            }
        }

        public class WorkflowFieldUpdates
        {
            // Object being updated
            public String objectName = "";
            public String fieldUpdateName = "";
            public String fieldUpdateLabel = "";
            public String fieldName = "";
            public String literalValue = "";
            public Boolean notifyAssignee = false;
            public Boolean reevaluateOnChange = false;
        }

        public class FieldExtractor
        {
            public String automationType = "";
            public String automationName = "";
            public String sObjectName = "";
            public String sObjectField = "";
        }

        private void btnRunAutomationFieldExtraction_Click(object sender, EventArgs e)
        {
            if (this.tbProjectFolder.Text == "")
            {
                MessageBox.Show("Please select a project folder which contains the Salesforce metadata");
                return;
            }

            if (this.tbFileSaveTo.Text == "")
            {
                MessageBox.Show("Please select a folder location to save the report values to");
                return;
            }

            // We need the objects and fields and the apex classes extracted out first
            runObjectFieldExtract();

            // Search through Triggers for field references
            runApexTriggerExtract();

            // Search through Classes for field references
            runApexClassExtract();

            // Search Flows/Processes for field references
            runFlowProcessExtract();

            // Search Workflows for field references
            runWorkflowExtract();

            // Run the custom Quick Action parsing
            // TODO: 


            // Write the results to a file
            writeAutomationFieldExtractedResultsToFile();

            writeAutomationHierarchyResults();

            MessageBox.Show("Automation Field Reference Extraction Complete");
        }

        private void btnParseFlows_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(this.tbProjectFolder.Text + "\\flows")
                || this.tbProjectFolder.Text == this.tbProjectFolder.Text + "\\flows")
            {
                String[] files = Directory.GetFiles(this.tbProjectFolder.Text + "\\flows");

                this.objectToFlow = new Dictionary<string, List<FlowProcess>>();

                foreach (String fl in files)
                {
                    runFlowProcessExtract();
                }

                // Now write out the contents in tabular format
                StreamWriter sw = new StreamWriter(this.tbFileSaveTo.Text + "\\FlowAutomation.txt");
                sw.Write("Triggering sObject\t");
                sw.Write("Flow API Name\t");
                sw.Write("Flow Label\t");
                sw.Write("API Version\t");
                sw.Write("Process Type\t");
                sw.Write("Triggered From\t");
                sw.Write("Run In Mode\t");
                sw.Write("Is Active" + Environment.NewLine);

                foreach (String flowObj in this.objectToFlow.Keys)
                {
                    if (flowObj == "")
                    {
                        sw.Write("No triggering sObject");

                        List<FlowProcess> flowProcessList = this.objectToFlow[flowObj];

                        foreach (FlowProcess fp in flowProcessList)
                        {
                            sw.Write("\t");
                            sw.Write(fp.apiName + "\t");
                            sw.Write(fp.label + "\t");
                            sw.Write(fp.apiVersion + "\t");
                            sw.Write(fp.flowProcessType + "\t");
                            sw.Write(fp.triggerType + "\t");
                            sw.Write(fp.runInMode + "\t");
                            sw.Write(fp.isActive + Environment.NewLine);
                        }

                        sw.Write(Environment.NewLine);
                    }
                    else
                    {
                        sw.WriteLine(flowObj);

                        List<FlowProcess> flowProcessList = this.objectToFlow[flowObj];

                        foreach (FlowProcess fp in flowProcessList) 
                        {
                            sw.Write("\t");
                            sw.Write(fp.apiName + "\t");
                            sw.Write(fp.label + "\t");
                            sw.Write(fp.apiVersion + "\t");
                            sw.Write(fp.flowProcessType + "\t");
                            sw.Write(fp.triggerType + "\t");
                            sw.Write(fp.runInMode + "\t");
                            sw.Write(fp.isActive + Environment.NewLine);
                        }

                        sw.Write(Environment.NewLine);
                    }
                }

                sw.Close();
            }

            MessageBox.Show("Flow Extraction Complete");
        }

        private void btnGetApexClassNamesAndMethods_Click(object sender, EventArgs e)
        {
            if (this.tbProjectFolder.Text == "")
            {
                MessageBox.Show("Please select a project folder which contains the Salesforce metadata");
                return;
            }

            if (this.tbFileSaveTo.Text == "")
            {
                MessageBox.Show("Please select a folder location to save the report values to");
                return;
            }

            // We need the objects and fields and the apex classes extracted out first
            runObjectFieldExtract();

            // Search through Triggers for field references
            runApexTriggerExtract();

            // Search through Classes for field references
            runApexClassExtract();

            // Search Flows/Processes for field references
            runFlowProcessExtract();

            // Search Workflows for field references
            runWorkflowExtract();

            writeAutomationNamesToFile();

            MessageBox.Show("Automation Names Extraction Complete");
        }

        private void btnAutomationHierarchy_Click(object sender, EventArgs e)
        {
            if (this.tbProjectFolder.Text == "")
            {
                MessageBox.Show("Please select a project folder which contains the Salesforce metadata");
                return;
            }

            if (this.tbFileSaveTo.Text == "")
            {
                MessageBox.Show("Please select a folder location to save the report values to");
                return;
            }

            // We need the objects and fields and the apex classes extracted out first
            runObjectFieldExtract();

            // Search through Triggers for field references
            runApexTriggerExtract();

            // Search through Classes for field references
            runApexClassExtract();

            // Search Flows/Processes for field references
            runFlowProcessExtract();

            writeAutomationSequenceToFile();

            MessageBox.Show("Automation Hierarchy Report Complete");
        }

        private void writeAutomationSequenceToFile()
        {
            StreamWriter sw = new StreamWriter(this.tbFileSaveTo.Text + "\\AutomationSequencesReport.txt");

            sw.Write("sObject\t");
            sw.Write("Flow API Name\t");
            sw.Write("Flow Label\t");
            sw.Write("API Version\t");
            sw.Write("Process Type\t");
            sw.Write("Trigger Type\t");
            sw.Write("Record Trigger Type\t");
            sw.Write("Run In Mode\t");
            sw.Write("Is Active" + Environment.NewLine);

            foreach (String sobj in this.objectToFlow.Keys)
            {
                if (sobj != "")
                {
                    foreach (FlowProcess fp in this.objectToFlow[sobj])
                    {
                        sw.Write(sobj + "\t");
                        sw.Write(fp.apiName + "\t");
                        sw.Write(fp.label + "\t");
                        sw.Write(fp.apiVersion + "\t");
                        sw.Write(fp.flowProcessType + "\t");
                        sw.Write(fp.triggerType + "\t");
                        sw.Write(fp.recordTriggerType + "\t");
                        sw.Write(fp.runInMode + "\t");
                        sw.Write(fp.isActive + Environment.NewLine);

                        foreach (FlowActionCalls fac in fp.actionCalls)
                        {
                            if (fac.actionCallAPIName.StartsWith("Call_proxy_to_"))
                            {
                                String subFlow = fac.actionCallAPIName.Substring(14);
                                sw.Write("\t\tSubflow: " + subFlow);
                                sw.Write("\t\tInput Parameters: \t");

                                foreach (FlowActionInputParameters fv in fac.inputParameters)
                                {
                                    sw.Write(fv.apiName + " => " + fv.value + "\t\t");
                                }

                                sw.Write(Environment.NewLine);

                                writeRelatedAutomationToFile(sw, subFlow, "", FLOW, 2);
                            }
                            else
                            {
                                // Custom Class - Find related automation pieces after writing the class name
                                if (this.classNmToClass.ContainsKey(fac.actionName))
                                {
                                    sw.Write("\t\tApexClass: " + fac.actionName + "\t\t");
                                    sw.Write("Input Parameters: \t");

                                    foreach (FlowActionInputParameters fv in fac.inputParameters)
                                    {
                                        sw.Write(fv.apiName + " => " + fv.value + "\t\t");
                                    }

                                    sw.Write(Environment.NewLine);

                                    writeRelatedAutomationToFile(sw, fac.actionName, "", APEX_CLASS, 2);
                                }
                                // Managed Package class: No need to find any related automation pieces from this
                                else if (fac.actionType == "apex")
                                {
                                    sw.Write("\t\tManagedApexClass: " + fac.actionName);
                                    sw.Write(Environment.NewLine);
                                    sw.Write(Environment.NewLine);
                                }
                                else
                                {
                                    sw.Write("\t\tManagedActionCall: " + fac.actionName);
                                    sw.Write(Environment.NewLine);
                                    sw.Write(Environment.NewLine);
                                }
                            }
                        }

                        sw.Write(Environment.NewLine);
                    }
                }
            }

            sw.Write(Environment.NewLine);
            sw.Write(Environment.NewLine);

            foreach (String sobj in this.objectToFlow.Keys)
            {
                if (sobj == "")
                {
                    foreach (FlowProcess fp in this.objectToFlow[sobj])
                    {
                        sw.Write(sobj + "\t");
                        sw.Write(fp.apiName + "\t");
                        sw.Write(fp.label + "\t");
                        sw.Write(fp.apiVersion + "\t");
                        sw.Write(fp.flowProcessType + "\t");
                        sw.Write(fp.triggerType + "\t");
                        sw.Write(fp.recordTriggerType + "\t");
                        sw.Write(fp.runInMode + "\t");
                        sw.Write(fp.isActive + Environment.NewLine);

                        foreach (FlowActionCalls fac in fp.actionCalls)
                        {
                            if (fac.actionCallAPIName.StartsWith("Call_proxy_to_"))
                            {
                                String subFlow = fac.actionCallAPIName.Substring(14);
                                sw.Write("\t\tSubflow: " + subFlow);
                                sw.Write("\t\tInput Parameters: \t");

                                foreach (FlowActionInputParameters fv in fac.inputParameters)
                                {
                                    sw.Write(fv.apiName + " => " + fv.value + "\t\t");
                                }

                                sw.Write(Environment.NewLine);

                                writeRelatedAutomationToFile(sw, subFlow, "", FLOW, 2);
                            }
                            else
                            {
                                // Custom Class - Find related automation pieces after writing the class name
                                if (this.classNmToClass.ContainsKey(fac.actionName))
                                {
                                    sw.Write("\t\tApexClass: " + fac.actionName + "\t\t");
                                    sw.Write("Input Parameters: \t");

                                    foreach (FlowActionInputParameters fv in fac.inputParameters)
                                    {
                                        sw.Write(fv.apiName + " => " + fv.value + "\t\t");
                                    }

                                    sw.Write(Environment.NewLine);

                                    writeRelatedAutomationToFile(sw, fac.actionName, "", APEX_CLASS, 2);
                                }
                                // Managed Package class: No need to find any related automation pieces from this
                                else if (fac.actionType == "apex")
                                {
                                    sw.Write("\t\tManagedApexClass: " + fac.actionName);
                                    sw.Write(Environment.NewLine);
                                    sw.Write(Environment.NewLine);
                                }
                                else
                                {
                                    sw.Write("\t\tManagedActionCall: " + fac.actionName);
                                    sw.Write(Environment.NewLine);
                                    sw.Write(Environment.NewLine);
                                }
                            }
                        }

                        sw.Write(Environment.NewLine);
                    }
                }
            }

            sw.Write(Environment.NewLine);
            sw.Write(Environment.NewLine);

            sw.Write("ClassName\t");
            sw.Write("ExtendsClass\t");
            sw.Write("OptionalModifier\t");
            sw.Write("IsInterface\t");
            sw.Write("IsRestClass\t");
            sw.Write("IsVirtual");
            sw.Write(Environment.NewLine);

            foreach (String clsnm in this.classNmToClass.Keys)
            {
                ApexClassStructure apxClass = this.classNmToClass[clsnm];

                if (apxClass.isTestClass == true)
                {
                    continue;
                }

                sw.Write(apxClass.className + "\t");
                sw.Write(apxClass.extendsClassName + "\t");
                sw.Write(apxClass.optionalModifier + "\t");
                sw.Write(apxClass.isInterface + "\t");
                sw.Write(apxClass.isRestClass + "\t");
                sw.Write(apxClass.isVirtual);

                sw.Write(Environment.NewLine);
                sw.Write(Environment.NewLine);

                sw.Write("\t");
                sw.Write("ClassProperties\t");
                sw.Write("PropertyName\t");
                sw.Write("DateType\t");
                sw.Write("PropertyValue\t");
                sw.Write("PropertyQualifier\t");
                sw.Write("IsStatic\t");
                sw.Write("IsFinal\t");
                sw.Write("IsGetter\t");
                sw.Write("IsSetter");
                sw.Write(Environment.NewLine);

                foreach (ClassProperty cp in apxClass.clsProperties.Values)
                {
                    sw.Write("\t\t");
                    sw.Write(cp.propertyName + "\t");
                    sw.Write(cp.dataType + "\t");
                    sw.Write(cp.propertyValue + "\t");
                    sw.Write(cp.qualifier + "\t");
                    sw.Write(cp.isStatic + "\t");
                    sw.Write(cp.isFinal + "\t");
                    sw.Write(cp.isGetter + "\t");
                    sw.Write(cp.isSetter);
                    sw.Write(Environment.NewLine);
                }

                sw.Write(Environment.NewLine);

                sw.Write("\t");
                sw.Write("ClassMethods\t");
                sw.Write("MethodName\t");
                sw.Write("MethodAnnotation\t");
                sw.Write("MethodQualifier\t");
                sw.Write("ReturnDataType\t");
                sw.Write("IsStatic\t");
                sw.Write("IsVirutual\t");
                sw.Write("IsTestMethod");
                sw.Write(Environment.NewLine);

                foreach (ClassMethod cm in apxClass.classMethods)
                {
                    sw.Write("\t\t");
                    sw.Write(cm.methodName + "\t");
                    sw.Write(cm.methodAnnotation + "\t");
                    sw.Write(cm.qualifier + "\t");
                    sw.Write(cm.returnDataType + "\t");
                    sw.Write(cm.isStatic + "\t");
                    sw.Write(cm.isVirtual + "\t");
                    sw.Write(cm.isTestMethod);
                    sw.Write(Environment.NewLine);

                    writeRelatedAutomationToFile(sw, apxClass.className, cm.methodName, APEX_CLASS_METHOD, 2);
                }

                sw.Write(Environment.NewLine);
                sw.Write(Environment.NewLine);
            }

            sw.Close();
        }

        private void writeRelatedAutomationToFile(StreamWriter sw, String apiName, String methodName, String automationType, Int32 tabCount)
        {
            // Write the incoming value to the file
            if (automationType == FLOW)
            {
                foreach (String sobj in this.objectToFlow.Keys)
                {
                    List<FlowProcess> fpList = this.objectToFlow[sobj];

                    foreach (FlowProcess fp in fpList)
                    {
                        if (fp.apiName == apiName)
                        {
                            tabCount += 2;

                            foreach (FlowActionCalls fac in fp.actionCalls)
                            {
                                if (fac.actionCallAPIName.StartsWith("Call_proxy_to_"))
                                {
                                    for (Int32 i = 0; i < tabCount; i++)
                                    {
                                        sw.Write("\t");
                                    }

                                    String subFlow = fac.actionCallAPIName.Substring(14);
                                    sw.Write("Subflow: " + subFlow);
                                    sw.Write("\t\tInput Parameters: \t");

                                    foreach (FlowActionInputParameters fv in fac.inputParameters)
                                    {
                                        sw.Write(fv.apiName + " => " + fv.value + "\t\t");
                                    }

                                    sw.Write(Environment.NewLine);

                                    writeRelatedAutomationToFile(sw, subFlow, "", FLOW, tabCount);
                                }
                                else
                                {
                                    // Custom Class - Find related automation pieces after writing the class name
                                    if (this.classNmToClass.ContainsKey(fac.actionName))
                                    {
                                        for (Int32 i = 0; i < tabCount; i++)
                                        {
                                            sw.Write("\t");
                                        }

                                        sw.Write("ApexClass: " + fac.actionName + "\t\t");
                                        sw.Write("Input Parameters: \t");

                                        foreach (FlowActionInputParameters fv in fac.inputParameters)
                                        {
                                            sw.Write(fv.apiName + " => " + fv.value + "\t\t");
                                        }

                                        sw.Write(Environment.NewLine);

                                        writeRelatedAutomationToFile(sw, fac.actionName, "", APEX_CLASS, tabCount);
                                    }
                                    // Managed Package class: No need to find any related automation pieces from this
                                    else if (fac.actionType == "apex")
                                    {
                                        for (Int32 i = 0; i < tabCount; i++)
                                        {
                                            sw.Write("\t");
                                        }

                                        sw.Write("ManagedApexClass: " + fac.actionName);
                                        sw.Write(Environment.NewLine);
                                        sw.Write(Environment.NewLine);
                                    }
                                    else
                                    {
                                        for (Int32 i = 0; i < tabCount; i++)
                                        {
                                            sw.Write("\t");
                                        }

                                        sw.Write("ManagedActionCall: " + fac.actionName);
                                        sw.Write(Environment.NewLine);
                                        sw.Write(Environment.NewLine);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (automationType == APEX_CLASS)
            {
                foreach (String clsNm in this.classNmToClass.Keys)
                {
                    ApexClassStructure apxClass = this.classNmToClass[clsNm];

                    if (apxClass.className == apiName)
                    {
                        tabCount += 2;

                        List<ClassMethod> clsMethods = new List<ClassMethod>();
                        clsMethods = apxClass.classMethods;

                        foreach (ClassMethod cm in clsMethods)
                        {
                            for (Int32 i = 0; i < tabCount; i++)
                            {
                                sw.Write("\t");
                            }

                            sw.Write("MethodName: " + cm.methodName + "\t");

                            if (cm.methodAnnotation != "")
                            {
                                sw.Write("Annotation: " + cm.methodAnnotation + "\t");
                            }

                            sw.Write(Environment.NewLine);
                            sw.Write(Environment.NewLine);

                            //String apexClassName = "";
                            //String apexClassVariable = "";

                            foreach (ObjectVarToType ovt in cm.methodBodyVars)
                            {
                                if (ovt.leftSideObject.StartsWith("Flow.Interview"))
                                {
                                    for (Int32 i = 0; i < tabCount; i++)
                                    {
                                        sw.Write("\t");
                                    }

                                    String flowName = ovt.leftSideObject.Substring(15);

                                    sw.Write("Flow API Name: " + flowName);

                                    sw.Write(Environment.NewLine);

                                    writeRelatedAutomationToFile(sw, flowName, "", FLOW, tabCount);
                                }
                                else if (this.classNmToClass.ContainsKey(ovt.leftSideObject))
                                {
                                    if (ovt.leftSide != "")
                                    {
                                        String[] splitClassMethodLeftSide = ovt.leftSide.Split('.');
                                        if (splitClassMethodLeftSide.Length >= 2)
                                        {
                                            writeRelatedAutomationToFile(sw, ovt.leftSideObject, splitClassMethodLeftSide[1], APEX_CLASS_METHOD, tabCount);
                                        }
                                    }

                                    if (ovt.rightSide != "")
                                    {
                                        String[] splitClassMethodRightSide = ovt.rightSide.Split('.');
                                        if (splitClassMethodRightSide.Length >= 2)
                                        {
                                            String[] splitMethod = splitClassMethodRightSide[1].Split(' ');
                                            writeRelatedAutomationToFile(sw, splitClassMethodRightSide[0], splitMethod[0], APEX_CLASS_METHOD, tabCount);
                                        }
                                    }
                                }
                                else if (ovt.rightSide != "")
                                {
                                    String[] splitClassMethodRightSide = ovt.rightSide.Split('.');
                                    if (splitClassMethodRightSide.Length >= 2)
                                    {
                                        String[] splitMethod = splitClassMethodRightSide[1].Split(' ');
                                        writeRelatedAutomationToFile(sw, splitClassMethodRightSide[0], splitMethod[0], APEX_CLASS_METHOD, tabCount);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (automationType == APEX_CLASS_METHOD)
            {
                foreach (String clsNm in this.classNmToClass.Keys)
                {
                    ApexClassStructure apxClass = this.classNmToClass[clsNm];

                    if (apxClass.className == apiName)
                    {
                        List<ClassMethod> clsMethods = new List<ClassMethod>();
                        clsMethods = apxClass.classMethods;

                        foreach (ClassMethod cm in clsMethods)
                        {
                            if (cm.methodName == methodName)
                            {
                                tabCount += 2;

                                for (Int32 i = 0; i < tabCount; i++)
                                {
                                    sw.Write("\t");
                                }

                                sw.Write("Class-MethodName: " + apxClass.className + "." + cm.methodName + "\t");

                                if (cm.methodAnnotation != "")
                                {
                                    sw.Write("Annotation: " + cm.methodAnnotation + "\t");
                                }

                                sw.Write(Environment.NewLine);
                                sw.Write(Environment.NewLine);

                                //String apexClassName = "";
                                //String apexClassVariable = "";

                                foreach (ObjectVarToType ovt in cm.methodBodyVars)
                                {
                                    if (ovt.leftSideObject.StartsWith("Flow.Interview"))
                                    {
                                        for (Int32 i = 0; i < tabCount; i++)
                                        {
                                            sw.Write("\t");
                                        }

                                        String flowName = ovt.leftSideObject.Substring(15);

                                        sw.Write("Flow API Name: " + flowName);

                                        sw.Write(Environment.NewLine);

                                        writeRelatedAutomationToFile(sw, flowName, "", FLOW, tabCount);
                                    }
                                    else if (this.classNmToClass.ContainsKey(ovt.leftSideObject))
                                    {
                                        if (ovt.leftSide != "")
                                        {
                                            String[] splitClassMethodLeftSide = ovt.leftSide.Split('.');
                                            if (splitClassMethodLeftSide.Length >= 2)
                                            {
                                                writeRelatedAutomationToFile(sw, ovt.leftSideObject, splitClassMethodLeftSide[1], APEX_CLASS_METHOD, tabCount);
                                            }
                                        }

                                        if (ovt.rightSide != "")
                                        {
                                            String[] splitClassMethodRightSide = ovt.rightSide.Split('.');
                                            if (splitClassMethodRightSide.Length >= 2)
                                            {
                                                String[] splitMethod = splitClassMethodRightSide[1].Split(' ');
                                                writeRelatedAutomationToFile(sw, splitClassMethodRightSide[0], splitMethod[0], APEX_CLASS_METHOD, tabCount);
                                            }
                                        }
                                    }
                                    else if (ovt.rightSide != "")
                                    {
                                        String[] splitClassMethodRightSide = ovt.rightSide.Split('.');
                                        if (splitClassMethodRightSide.Length >= 2)
                                        {
                                            String[] splitMethod = splitClassMethodRightSide[1].Split(' ');
                                            writeRelatedAutomationToFile(sw, splitClassMethodRightSide[0], splitMethod[0], APEX_CLASS_METHOD, tabCount);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void btnParseApprovalProcesses_Click(object sender, EventArgs e)
        {

        }
    }
}
