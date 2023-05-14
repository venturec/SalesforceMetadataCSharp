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
using static SalesforceMetadata.AutomationReporter;
using iTextSharp.text;
using System.Reflection;


namespace SalesforceMetadata
{
    public partial class AutomationReporter : Form
    {
        // Key = Object API Name
        public Dictionary<String, ObjectToFields> objectToFieldsDictionary;

        // Key = Object API Name + Field Name
        public Dictionary<String, List<ObjectValidations>> objectValidationsDictionary;

        // Key = class name
        public Dictionary<String, ApexClasses> classNmToClass;

        // Variable names used in Triggers and classes
        // Object
        // Trigger Name
        // Trigger Type - insert, update, delete, undelete
        public Dictionary<String, List<ApexTriggers>> objectToTrigger;

        public Dictionary<String, List<FlowProcess>> objectToFlow;

        public Dictionary<String, List<Workflows>> workflowObjToFieldUpdt;

        public AutomationReporter()
        {
            InitializeComponent();
        }

        private void btnRunAutomationReport_Click(object sender, EventArgs e)
        {
            if (this.tbProjectFolder.Text != null && this.tbProjectFolder.Text != "")
            {
                // We need the objects and fields and the apex classes extracted out first
                runObjectFieldExtract();
                
                runApexClassExtract();

                runApexTriggerExtract();

                runFlowProcessExtract();
                
                runWorkflowExtract();

                //runApprovalProcessExtract();

                //runQuickActionExtract();


                // Write the data to an HTML friendly format based on Order of Execution
                // https://developer.salesforce.com/docs/atlas.en-us.apexcode.meta/apexcode/apex_triggers_order_of_execution.htm
                writeAutomationLogicToFile();

                MessageBox.Show("Automation Report Complete");
            }
        }


        // We need the objects and fields and the apex classes extracted out first
        private void runObjectFieldExtract()
        {
            if (Directory.Exists(this.tbProjectFolder.Text + "\\objects"))
            {
                String[] files = Directory.GetFiles(this.tbProjectFolder.Text + "\\objects");

                objectToFieldsDictionary = new Dictionary<string, ObjectToFields>();
                objectValidationsDictionary = new Dictionary<String, List<ObjectValidations>>();

                foreach (String fl in files)
                {
                    String[] flPathSplit = fl.Split('\\');
                    String[] flNameSplit = flPathSplit[flPathSplit.Length - 1].Split('.');

                    XmlDocument objXd = new XmlDocument();
                    objXd.Load(fl);

                    XmlNodeList fieldList = objXd.GetElementsByTagName("fields");
                    XmlNodeList validationRules = objXd.GetElementsByTagName("validationRules");
                    XmlNodeList sharingmodel = objXd.GetElementsByTagName("sharingModel");
                    XmlNodeList objvisibility = objXd.GetElementsByTagName("visibility");

                    // Get all fields into the Dictionary
                    ObjectToFields otf = new ObjectToFields();
                    otf.objectName = flNameSplit[0];

                    otf.fields = new List<String>();
                    foreach (XmlNode fldNd in fieldList)
                    {
                        if (fldNd.ParentNode.Name == "CustomObject")
                        {
                            otf.fields.Add(fldNd.ChildNodes[0].InnerText);
                        }
                    }

                    if (sharingmodel.Count > 0)
                    {
                        otf.sharingModel = sharingmodel[0].ChildNodes[0].InnerText;
                    }

                    if (objvisibility.Count > 0)
                    {
                        otf.visibility = objvisibility[0].ChildNodes[0].InnerText;
                    }

                    objectToFieldsDictionary.Add(flNameSplit[0], otf);


                    // Get all Validation Rules into the Dictionary
                    foreach (XmlNode objVal in validationRules)
                    {
                        if (objVal.ChildNodes[1].InnerText == "true")
                        {
                            ObjectValidations ov = new ObjectValidations();

                            foreach (XmlNode cn in objVal.ChildNodes)
                            {
                                if (cn.Name == "fullName")
                                {
                                    ov.validationName = cn.InnerText;
                                }
                                else if (cn.Name == "errorConditionFormula")
                                {
                                    ov.errorConditionFormula = cn.InnerText;
                                }
                            }

                            if (objectValidationsDictionary.ContainsKey(flNameSplit[0]))
                            {
                                objectValidationsDictionary[flNameSplit[0]].Add(ov);
                            }
                            else
                            {
                                objectValidationsDictionary.Add(flNameSplit[0], new List<ObjectValidations> { ov });
                            }
                        }
                    }
                }
            }
        }

        private void runApexClassExtract()
        {
            if (Directory.Exists(this.tbProjectFolder.Text + "\\classes"))
            {
                String[] files = Directory.GetFiles(this.tbProjectFolder.Text + "\\classes");

                this.classNmToClass = new Dictionary<string, ApexClasses>();

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
            this.objectToTrigger = new Dictionary<string, List<ApexTriggers>>();

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

                    XmlNodeList ndListStatus = objXd.GetElementsByTagName("status");
                    XmlNodeList ndListLabel = objXd.GetElementsByTagName("label");
                    XmlNodeList ndListProcType = objXd.GetElementsByTagName("processType");
                    XmlNodeList ndListObjProc = objXd.GetElementsByTagName("processMetadataValues");
                    XmlNodeList ndListStart = objXd.GetElementsByTagName("start");
                    XmlNodeList ndListApiVs = objXd.GetElementsByTagName("apiVersion");
                    XmlNodeList ndListRunInMd = objXd.GetElementsByTagName("runInMode");

                    XmlNodeList ndListRecordCreates = objXd.GetElementsByTagName("recordCreates");
                    XmlNodeList ndListRecordUpdates = objXd.GetElementsByTagName("recordUpdates");
                    XmlNodeList ndListRecordDeletes = objXd.GetElementsByTagName("recordDeletes");

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
                            Debug.WriteLine(" ");
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
                            foreach (XmlNode cn1 in xn.ChildNodes)
                            {
                                if (cn1.Name == "object")
                                {
                                    flowObj.objectName = cn1.InnerText;
                                }
                                else if (cn1.Name == "recordTriggerType")
                                {
                                    flowObj.recordTriggerTrype = cn1.InnerText;
                                }
                                else if (cn1.Name == "triggerType")
                                {
                                    flowObj.triggerType = cn1.InnerText;
                                }
                                else if (cn1.Name == "connector")
                                {
                                    //Debug.WriteLine(" ");
                                }
                            }
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
                if (Directory.Exists(this.tbProjectFolder.Text + "\\workflows"))
                {
                    workflowObjToFieldUpdt = new Dictionary<string, List<Workflows>>();

                    String[] files = Directory.GetFiles(this.tbProjectFolder.Text + "\\workflows");

                    foreach (String fl in files)
                    {
                        String[] flPathSplit = fl.Split('\\');
                        String[] flNameSplit = flPathSplit[flPathSplit.Length - 1].Split('.');

                        XmlDocument objXd = new XmlDocument();
                        objXd.Load(fl);

                        XmlNodeList wfRules = objXd.GetElementsByTagName("rules");
                        XmlNodeList wfFieldUpdates = objXd.GetElementsByTagName("fieldUpdates");

                        foreach (XmlNode nd1 in wfRules)
                        {
                            if (nd1.ParentNode.Name == "Workflow")
                            {
                                Workflows wrkFlowObj = new Workflows();
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
                                            fieldUpdates.Add(cn.ChildNodes[0].InnerText);
                                        }
                                    }
                                }

                                if (wrkFlowObj.isActive == true
                                    && fieldUpdates.Count > 0)
                                {
                                    foreach (XmlNode cn in wfFieldUpdates)
                                    {
                                        if (fieldUpdates.Contains(cn.ChildNodes[0].InnerText))
                                        {
                                            WorkflowFieldUpdate wfu = new WorkflowFieldUpdate();
                                            wfu.fieldUpdateName = cn.ChildNodes[0].InnerText;

                                            foreach (XmlNode cn2 in cn.ChildNodes)
                                            {
                                                if (cn2.Name == "field")
                                                {
                                                    wfu.fieldName = cn2.InnerText;
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

                                            // Add the field update to the workflow object
                                            wrkFlowObj.fieldUpdates.Add(wfu);
                                        }
                                    }

                                    // Add the workflow to the dictionary
                                    if (this.workflowObjToFieldUpdt.ContainsKey(wrkFlowObj.objectName))
                                    {
                                        this.workflowObjToFieldUpdt[wrkFlowObj.objectName].Add(wrkFlowObj);
                                    }
                                    else
                                    {
                                        this.workflowObjToFieldUpdt.Add(wrkFlowObj.objectName, new List<Workflows> { wrkFlowObj });
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void runApprovalProcessExtract()
        {
            if (Directory.Exists(this.tbProjectFolder.Text + "\\approvalProcesses"))
            {

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

        private void tbProjectFolder_DoubleClick(object sender, EventArgs e)
        {
            this.tbProjectFolder.Text = UtilityClass.folderBrowserSelectPath("Select Project Folder", 
                                                                             false, 
                                                                             FolderEnum.ReadFrom,
                                                                             Properties.Settings.Default.AutomationReportLastReadLocation);

            Properties.Settings.Default.AutomationReportLastReadLocation = this.tbProjectFolder.Text;
            Properties.Settings.Default.Save();
        }

        private void tbFileSaveTo_DoubleClick(object sender, EventArgs e)
        {
            this.tbFileSaveTo.Text = UtilityClass.folderBrowserSelectPath("Select Folder To Save Report To",
                                                                             true,
                                                                             FolderEnum.SaveTo,
                                                                             Properties.Settings.Default.AutomationReportLastSaveLocation);

            Properties.Settings.Default.AutomationReportLastSaveLocation = this.tbFileSaveTo.Text;
            Properties.Settings.Default.Save();
        }

        private String reduceWhitespace(String strValue)
        {
            // Remove any comments and commented code
            String fileContents1 = "";
            Char[] charArray = strValue.ToCharArray();
            Boolean inInlineComment = false;

            Boolean inMultilineComment = false;
            Int32 mlCommentNotationCount = 0;

            Boolean skipOne = false;

            for (Int32 i = 0; i < charArray.Length; i++)
            {
                if (skipOne == true)
                {
                    skipOne = false;
                }
                else if (inInlineComment == false
                    && charArray[i].ToString() == "/" && charArray[i + 1].ToString() == "*")
                {
                    inMultilineComment = true;
                    mlCommentNotationCount++;
                }
                else if (inMultilineComment == false
                    && charArray[i].ToString() == "/" && charArray[i + 1].ToString() == "/")
                {
                    inInlineComment = true;
                }
                else if (inInlineComment == true
                    && charArray[i] == '\n')
                {
                    inInlineComment = false;
                }
                else if (inInlineComment == false
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
            fileContents1 = fileContents1.Replace(" <", "<");

            fileContents1 = fileContents1.Trim();


            String filecontents2 = "";

            // reformat for Map, List, Set
            Boolean inCollection = false;
            String collectionVar = "";
            Boolean firstLessThanFound = false;
            Int32 lessThanCount = 0;

            charArray = fileContents1.ToCharArray();
            for (Int32 i = 0; i < charArray.Length; i++)
            {
                if (inCollection == true && charArray[i].ToString().ToLower() != " ")
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
                else if (charArray[i].ToString().ToLower() == "m"
                   && charArray[i + 1].ToString().ToLower() == "a"
                   && charArray[i + 2].ToString().ToLower() == "p"
                   && charArray[i + 3].ToString().ToLower() == "<")
                {
                    inCollection = true;
                    collectionVar = collectionVar + charArray[i].ToString();
                }
                else if (charArray[i].ToString().ToLower() == "l"
                   && charArray[i + 1].ToString().ToLower() == "i"
                   && charArray[i + 2].ToString().ToLower() == "s"
                   && charArray[i + 3].ToString().ToLower() == "t"
                   && charArray[i + 4].ToString().ToLower() == "<")
                {
                    inCollection = true;
                    collectionVar = collectionVar + charArray[i].ToString();
                }
                else if (charArray[i].ToString().ToLower() == "s"
                   && charArray[i + 1].ToString().ToLower() == "e"
                   && charArray[i + 2].ToString().ToLower() == "t"
                   && charArray[i + 3].ToString().ToLower() == "<")
                {
                    inCollection = true;
                    collectionVar = collectionVar + charArray[i].ToString();
                }
                else if (inCollection == false)
                {
                    filecontents2 = filecontents2 + charArray[i].ToString();
                }
            }

            return filecontents2;
        }

        private void parseApexTrigger(String[] filearray)
        {
            ApexTriggers at = new ApexTriggers();

            Boolean inTriggerEvents = false;
            Boolean inSOQLStatement = false;
            String soqlStatement = "";
            String soqlObject = "";

            for (Int32 i = 0; i < filearray.Length - 1; i++)
            {
                if (filearray[i].ToLower() == "trigger")
                {
                    at.triggerName = filearray[i + 1];
                    at.objectName = filearray[i + 3];

                    inTriggerEvents = true;
                }

                if (inTriggerEvents == true
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
                    }
                    else if(triggerevt == "before update")
                    {
                        at.isBeforeUpdate = true;
                    }
                    else if (triggerevt == "before delete")
                    {
                        at.isBeforeDelete = true;
                    }
                    else if (triggerevt == "after insert")
                    {
                        at.isAfterInsert = true;
                    }
                    else if (triggerevt == "after update")
                    {
                        at.isAfterUpdate = true;
                    }
                    else if (triggerevt == "after delete")
                    {
                        at.isAfterDelete = true;
                    }
                    else if (triggerevt == "after undelete")
                    {
                        at.isAfterUndelete = true;
                    }
                }
                else if (inTriggerEvents == true
                        && filearray[i].ToLower() == ")")
                {
                    inTriggerEvents = false;
                }

                if (filearray[i].ToLower() == "select" && inSOQLStatement == false)
                {
                    inSOQLStatement = true;
                    at.logicContainedInTrigger = true;
                }

                if (inSOQLStatement == true && filearray[i].ToLower() == "]")
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

                if (filearray[i].ToLower() == "database.insert"
                    || filearray[i].ToLower() == "database.update"
                    || filearray[i].ToLower() == "database.delete"
                    || filearray[i].ToLower() == "database.undelete")
                {
                    at.logicContainedInTrigger = true;

                    String varName = filearray[i + 2].ToLower();
                    ObjectVarToType ovt = parseOutDmlVars(filearray, varName, filearray[i]);

                    if (ovt.objectName != null)
                    {
                        if (at.objVarToType.ContainsKey(ovt.objectName))
                        {
                            at.objVarToType[ovt.objectName].Add(ovt);
                        }
                        else
                        {
                            at.objVarToType.Add(ovt.objectName, new List<ObjectVarToType> { ovt });
                        }
                    }
                }
                
                if (filearray[i].ToLower() == "insert"
                    || filearray[i].ToLower() == "update"
                    || filearray[i].ToLower() == "delete"
                    || filearray[i].ToLower() == "undelete")
                {
                    at.logicContainedInTrigger = true;

                    String varName = filearray[i + 1].ToLower();
                    ObjectVarToType ovt = parseOutDmlVars(filearray, varName, filearray[i]);

                    if (ovt.objectName != null)
                    {
                        if (at.objVarToType.ContainsKey(ovt.objectName))
                        {
                            at.objVarToType[ovt.objectName].Add(ovt);
                        }
                        else
                        {
                            at.objVarToType.Add(ovt.objectName, new List<ObjectVarToType> { ovt });
                        }
                    }
                }
            }

            if (this.objectToTrigger.ContainsKey(at.objectName))
            {
                this.objectToTrigger[at.objectName].Add(at);
            }
            else
            {
                this.objectToTrigger.Add(at.objectName, new List<ApexTriggers> { at });
            }
        }

        private void parseApexClass(String[] filearray)
        {
            if (filearray[0].ToLower() == "@istest") return;

            ApexClasses apexCls = new ApexClasses();

            Boolean inClassName = true;
            Int32 classDeclarationCount = 1;

            Int32 skipTo = 0;
            Boolean skipOver = false;

            for (Int32 i = 0; i < filearray.Length - 1; i++)
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
                //else if (filearray[i].ToLower() == "class"
                //    && classDeclarationCount == 0)
                //{
                //    classDeclarationCount++;
                //    inClassName = true;
                //}
                //else if (filearray[i].ToLower() == "interface"
                //    && classDeclarationCount == 0)
                //{
                //    classDeclarationCount++;
                //    inClassName = true;
                //}
                else if (inClassName == true
                         && classDeclarationCount == 1)
                {
                    if (filearray[i] == "{")
                    {
                        inClassName = false;
                    }
                    else if (filearray[i].ToLower() == "private"
                            || filearray[i].ToLower() == "public"
                            || filearray[i].ToLower() == "global")
                    {
                        apexCls.accessModifier = filearray[i].ToLower(); ;
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
                    }
                    else if (filearray[i].ToLower() == "interface")
                    {
                        apexCls.className = filearray[i + 1];
                        apexCls.isInterface = true;
                    }
                    else if (filearray[i].ToLower() == "implements")
                    {
                        //apexCls.className = filearray[i];
                    }
                    else if (filearray[i].ToLower() == "extends")
                    {
                        apexCls.extendsClassName = filearray[i];
                    }
                }
                // TODO: Skipping over inner classes for now
                else if (inClassName == false
                        && (filearray[i].ToLower() == "protected"
                            || filearray[i].ToLower() == "private"
                            || filearray[i].ToLower() == "public"
                            || filearray[i].ToLower() == "global")
                        && filearray[i + 1] == "class")
                {
                    skipTo = i;
                    skipOver = true;

                    Boolean icFirstBraceReached = false;
                    Int32 braceCount = 0;
                    for (Int32 j = i; j < filearray.Length - 1; j++)
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
                        }
                        else if (braceCount == 0
                            && icFirstBraceReached == true)
                        {
                            skipTo = j - 1;
                            break;
                        }
                    }
                }
                else if (inClassName == false
                        && (filearray[i].ToLower() == "protected"
                || filearray[i].ToLower() == "private"
                || filearray[i].ToLower() == "public"
                || filearray[i].ToLower() == "global"))
                {
                    skipTo = parsePropertyOrMethod(filearray, apexCls, i);
                    skipOver = true;
                }
            }

            //classNmToClass = new Dictionary<string, ApexClasses>();
            this.classNmToClass.Add(apexCls.className, apexCls);
        }

        public Int32 parsePropertyOrMethod(String[] filearray, ApexClasses ac, Int32 ap)
        {
            String propertyMethodName = "";
            String propertyMethodQualifier = "";
            String propertyMethodRtnDataType = "";
            Boolean isStatic = false;
            Boolean isFinal = false;
            Boolean isOverride = false;

            Int32 parenthesesCount = 0; // ( )
            Int32 braceCount = 0;       // { }

            Boolean isMethod = false;
            Boolean isConstructor = false;

            Boolean inSOQLStatement = false;
            String soqlObject = "";
            String soqlStatement = "";

            ClassMethods cm = new ClassMethods();
            ClassProperties cp = new ClassProperties();

            // Key = Method Name - Value = SOQL Statements
            Dictionary<String, List<String>> soqlStatements = new Dictionary<String, List<String>>();

            Int32 lastCharLocation = 0;
            
            for (Int32 i = ap; i < filearray.Length - 1; i++)
            {
                // TODO: Bypass the constructors
                if (filearray[i] == "(")
                {
                    if (propertyMethodName != "" && propertyMethodQualifier != "" && propertyMethodRtnDataType != "")
                    {
                        isMethod = true;
                    }
                    else
                    {
                        isConstructor = true;
                    }

                    parenthesesCount++;
                }
                else if (filearray[i] == ")")
                {
                    parenthesesCount--;
                }
                else if (filearray[i] == "{")
                {
                    braceCount++;
                }
                else if (filearray[i] == "}")
                {
                    braceCount--;

                    if (braceCount == 0
                        && isMethod == true)
                    {
                        cm.methodName = propertyMethodName;
                        cm.qualifier = propertyMethodQualifier;
                        cm.returnDataType = propertyMethodRtnDataType;
                        cm.isOverride = isOverride;
                        cm.isStatic = isStatic;
                        cm.soqlStatements = soqlStatements;

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
                else if (filearray[i].ToLower() == "protected"
                    || filearray[i].ToLower() == "private"
                    || filearray[i].ToLower() == "public"
                    || filearray[i].ToLower() == "global")
                {
                    propertyMethodQualifier = filearray[i].ToLower();
                }
                else if (filearray[i].ToLower() == "override")
                {
                    isOverride = true;
                }
                else if (filearray[i].ToLower() == "static")
                {
                    isStatic = true;
                }
                else if (filearray[i].ToLower() == "final")
                {
                    isFinal = true;
                }
                else if (parenthesesCount == 0
                    && braceCount == 0
                    && isConstructor == false
                    && propertyMethodRtnDataType == "")
                {
                    propertyMethodRtnDataType = filearray[i];
                }
                else if (parenthesesCount == 0
                    && braceCount == 0
                    && isConstructor == false
                    && propertyMethodName == "")
                {
                    propertyMethodName = filearray[i];
                }
                else if (filearray[i] == ";"
                        && isMethod == false
                        && braceCount == 0
                        && parenthesesCount == 0)
                {
                    // This is a property
                    cp.propertyName = propertyMethodName;
                    cp.qualifier = propertyMethodQualifier;
                    cp.dataType = propertyMethodRtnDataType;
                    cp.isFinal = isFinal;
                    cp.isStatic = isStatic;

                    ac.clsProperties.Add(propertyMethodName, cp);
                    lastCharLocation = i;
                    break;
                }
                else if (filearray[i].ToLower() == "database.insert"
                    || filearray[i].ToLower() == "database.update"
                    || filearray[i].ToLower() == "database.delete"
                    || filearray[i].ToLower() == "database.undelete")
                {
                    String[] varNameSplit = filearray[i + 2].ToLower().Split('.');
                    String varName = varNameSplit[0];
                    ObjectVarToType ovt = parseOutDmlVars(filearray, varName, filearray[i]);

                    if (ovt.objectName != null)
                    {
                        if (cm.objVarToType.ContainsKey(ovt.objectName))
                        {
                            cm.objVarToType[ovt.objectName].Add(ovt);
                        }
                        else
                        {
                            cm.objVarToType.Add(ovt.objectName, new List<ObjectVarToType> { ovt });
                        }
                    }
                }
                else if (filearray[i].ToLower() == "insert"
                    || filearray[i].ToLower() == "update"
                    || filearray[i].ToLower() == "delete"
                    || filearray[i].ToLower() == "undelete")
                {
                    String[] varNameSplit = filearray[i + 2].ToLower().Split('.');
                    String varName = varNameSplit[0];
                    ObjectVarToType ovt = parseOutDmlVars(filearray, varName, filearray[i]);

                    if (ovt.objectName != null)
                    {
                        if (cm.objVarToType.ContainsKey(ovt.objectName))
                        {
                            cm.objVarToType[ovt.objectName].Add(ovt);
                        }
                        else
                        {
                            cm.objVarToType.Add(ovt.objectName, new List<ObjectVarToType> { ovt });
                        }
                    }
                }

                // SOQL query handlers
                if (filearray[i].ToLower() == "select" && inSOQLStatement == false)
                {
                    inSOQLStatement = true;
                }

                if (inSOQLStatement == true && filearray[i].ToLower() == "]")
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
                else if (inSOQLStatement == true)
                {
                    soqlStatement = soqlStatement + filearray[i] + " ";
                    if (filearray[i].ToLower() == "from")
                    {
                        soqlObject = filearray[i + 1];
                    }
                }
            }

            return lastCharLocation;
        }

        public ObjectVarToType parseOutDmlVars(String[] filearray, String varName, String dmlType)
        {
            ObjectVarToType ovt = new ObjectVarToType();

            // Find the variable in the string array and determine what it is 
            for (Int32 j = 0; j < filearray.Length - 1; j++)
            {
                if (filearray[j].ToLower() == varName)
                {
                    String[] varObjectAndType = filearray[j - 1].Split(new String[] { "<", ">" }, StringSplitOptions.None);
                    // List or Map
                    if (varObjectAndType.Length == 3)
                    {
                        // Accounts for a Map / Dictionary
                        if (varObjectAndType[0].ToLower() == "map")
                        {
                            ovt.objectName = varObjectAndType[1].Split(',')[1];
                        }
                        else
                        {
                            ovt.objectName = varObjectAndType[1];
                        }

                        ovt.varType = varObjectAndType[0];
                        ovt.varName = varName;
                        ovt.dmlType = dmlType;
                    }
                    // Single object
                    else if (varObjectAndType.Length == 2)
                    {
                        // Accounts for a Map / Dictionary
                        if (varObjectAndType[0].ToLower() == "map")
                        {
                            ovt.objectName = varObjectAndType[1].Split(',')[1];
                        }
                        else
                        {
                            ovt.objectName = varObjectAndType[1];
                        }

                        ovt.varType = "";
                        ovt.varName = varName;
                        ovt.dmlType = dmlType;
                    }

                    break;
                }
            }

            return ovt;
        }


        public void writeAutomationLogicToFile()
        {
            StreamWriter sw = new StreamWriter(this.tbFileSaveTo.Text + "\\AutomationReport.txt");

            sw.WriteLine("Salesforce Automation Report");
            sw.WriteLine("Description");
            sw.WriteLine("This report shows which objects are potentially impacted by Triggers, Class Methods, Flows and Workflow-Field Updates");
            sw.WriteLine(Environment.NewLine);
            sw.WriteLine("Report Run Date: " + DateTime.Now);
            sw.Write(Environment.NewLine);

            foreach (String obj in objectToFieldsDictionary.Keys)
            {
                Boolean objectNameWritten = false;

                foreach (String clsNm in classNmToClass.Keys)
                {
                    Boolean clsNameWritten = false;

                    foreach (ClassMethods am in classNmToClass[clsNm].classMethods)
                    {
                        Boolean methodNameWritten = false;

                        foreach (String ovtObjName in am.objVarToType.Keys)
                        {
                            if (ovtObjName == obj)
                            {
                                if (objectNameWritten == false)
                                {
                                    sw.WriteLine("Object Name: " + obj);
                                    objectNameWritten = true;
                                }

                                if (clsNameWritten == false)
                                {
                                    sw.WriteLine("\tClass Name: " + clsNm);
                                    clsNameWritten = true;
                                }

                                if (methodNameWritten == false)
                                {
                                    sw.WriteLine("\t\tMethod Name: " + am.methodName + "\tQualifier: " + am.qualifier + "\tReturn Data Type: " + am.returnDataType);
                                }

                                sw.WriteLine("\t\t\tData Manimpulations (DMLs)");
                                foreach (ObjectVarToType ovt in am.objVarToType[ovtObjName])
                                {
                                    sw.WriteLine("\t\t\tVar Type: " + ovt.varType + "\t" + ovt.varName + "\t" + ovt.dmlType);
                                }

                                // Write triggers potentially impacted by DML statements
                                if (objectToTrigger.ContainsKey(ovtObjName))
                                {
                                    sw.Write(Environment.NewLine);
                                    sw.Write(Environment.NewLine);

                                    foreach (ApexTriggers at in objectToTrigger[obj])
                                    {
                                        sw.WriteLine("\tTrigger Name: " + at.triggerName);
                                        sw.WriteLine("\t\tBefore Insert\tBefore Update\tBefore Delete\tAfter Insert\tAfter Update\tAfter Delete\tAfter Undelete");
                                        sw.WriteLine("\t\t"
                                            + at.isBeforeInsert
                                            + "\t" + at.isBeforeUpdate
                                            + "\t" + at.isBeforeDelete
                                            + "\t" + at.isAfterInsert
                                            + "\t" + at.isAfterUpdate
                                            + "\t" + at.isAfterDelete
                                            + "\t" + at.isAfterUndelete);
                                    }
                                }

                                // Write Flows potentially impacted by DML statements
                                if (objectToFlow.ContainsKey(ovtObjName))
                                {
                                    sw.Write(Environment.NewLine);
                                    sw.Write(Environment.NewLine);

                                    foreach (FlowProcess fl in objectToFlow[ovtObjName])
                                    {
                                        sw.WriteLine("\tFlow Name: " + fl.label + "(" + fl.apiName + ")" + "\tTrigger Type: " + fl.triggerType + "\tRun In Mode: " + fl.runInMode);
                                    }
                                }


                                // Write Workflow field updates
                                if (workflowObjToFieldUpdt.ContainsKey(ovtObjName))
                                {
                                    sw.Write(Environment.NewLine);
                                    sw.Write(Environment.NewLine);

                                    foreach (Workflows wf in workflowObjToFieldUpdt[ovtObjName])
                                    {
                                        sw.WriteLine("\tWorkflow Rule Name: " + wf.workflowRuleName + "\tTrigger Type: " + wf.triggerType);
                                        sw.WriteLine("\t\tFields Being Updated");

                                        foreach (WorkflowFieldUpdate wfu in wf.fieldUpdates)
                                        {
                                            sw.WriteLine("\t\tField Name: " + wfu.fieldName + "\tField Update Name: " + wfu.fieldUpdateName);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (objectToTrigger.ContainsKey(obj))
                {
                    sw.Write(Environment.NewLine);
                    sw.Write(Environment.NewLine);

                    if (objectNameWritten == false)
                    {
                        sw.WriteLine("Object Name: " + obj);
                        objectNameWritten = true;
                    }

                    foreach (ApexTriggers at in objectToTrigger[obj])
                    {
                        sw.WriteLine("\tTrigger Name: " + at.triggerName);
                        sw.WriteLine("\t\tBefore Insert\tBefore Update\tBefore Delete\tAfter Insert\tAfter Update\tAfter Delete\tAfter Undelete");
                        sw.WriteLine("\t\t"
                            + at.isBeforeInsert
                            + "\t" + at.isBeforeUpdate
                            + "\t" + at.isBeforeDelete
                            + "\t" + at.isAfterInsert
                            + "\t" + at.isAfterUpdate
                            + "\t" + at.isAfterDelete
                            + "\t" + at.isAfterUndelete);
                    }
                }

                foreach (ApexClasses ac in classNmToClass.Values)
                {
                    foreach (ClassMethods cm in ac.classMethods)
                    {
                        if (cm.objVarToType.ContainsKey(obj))
                        {
                            sw.WriteLine("\t\tMethod Name: " + cm.methodName + "\tQualifier: " + cm.qualifier + "\tReturn Data Type: " + cm.returnDataType);

                            foreach (ObjectVarToType ovt in cm.objVarToType[obj])
                            {
                                sw.WriteLine("\t\t\tData Manimpulations (DMLs)");
                                sw.WriteLine("\t\t\tVar Type: " + ovt.varType + "\t" + ovt.varName + "\t" + ovt.dmlType);
                            }
                        }
                    }
                }

                if (objectToFlow.ContainsKey(obj))
                {
                    sw.Write(Environment.NewLine);
                    sw.Write(Environment.NewLine);

                    if (objectNameWritten == false)
                    {
                        sw.WriteLine("Object Name: " + obj);
                        objectNameWritten = true;
                    }

                    foreach (FlowProcess fl in objectToFlow[obj])
                    {
                        sw.WriteLine("\tFlow Name: " + fl.label + "(" + fl.apiName + ")" + "\tTrigger Type: " + fl.triggerType + "\tRun In Mode: " + fl.runInMode);
                    }
                }

                // Write Workflow field updates
                if (workflowObjToFieldUpdt.ContainsKey(obj))
                {
                    sw.Write(Environment.NewLine);
                    sw.Write(Environment.NewLine);

                    if (objectNameWritten == false)
                    {
                        sw.WriteLine("Object Name: " + obj);
                        objectNameWritten = true;
                    }

                    foreach (Workflows wf in workflowObjToFieldUpdt[obj])
                    {
                        sw.WriteLine("\tWorkflow Rule Name: " + wf.workflowRuleName + "\tTrigger Type: " + wf.triggerType);
                        sw.WriteLine("\t\tFields Being Updated");

                        foreach (WorkflowFieldUpdate wfu in wf.fieldUpdates)
                        {
                            sw.WriteLine("\t\tField Name: " + wfu.fieldName + "\tField Update Name: " + wfu.fieldUpdateName);
                        }
                    }
                }
            }

            sw.Close();
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

        public class ObjectToFields 
        {
            public String objectName;
            public List<String> fields;
            public String sharingModel;
            public String visibility;
        }

        public class ObjectValidations
        {
            public String validationName;
            public String errorConditionFormula;
        }

        public class ApexTriggers 
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

            // Objects being updated
            public Dictionary<String, List<ObjectVarToType>> objVarToType;
            public ApexTriggers()
            {
                triggerEvents = new HashSet<string>();
                soqlStatements = new Dictionary<String, List<String>>();
                classMethodCalls = new Dictionary<string, List<string>>();
                objVarToType = new Dictionary<String, List<ObjectVarToType>>();
            }
        }

        public class ApexClasses
        {
            public string className = "";
            public string accessModifier = "";
            public string optionalModifier = "";

            public Boolean isInterface = false;

            public List<String> interfaceClassNames;
            public String extendsClassName = "";
            public Dictionary<String, ClassProperties> clsProperties;
            public Dictionary<String, ApexClasses> innerClasses;
            public List<ClassMethods> classMethods;
            public Dictionary<String, String> objToFieldsReferenced;

            // Objects being updated
            //public Dictionary<String, List<ObjectVarToType>> objVarToType;

            public ApexClasses()
            {
                interfaceClassNames = new List<String>();
                clsProperties = new Dictionary<string, ClassProperties>();
                innerClasses = new Dictionary<string, ApexClasses>();
                classMethods = new List<ClassMethods>();
                objToFieldsReferenced = new Dictionary<String, String>();
            }
        }

        public class ObjectVarToType
        {
            public String objectName = "";
            public String varType = "";
            public String varName = "";
            public String dmlType = "";
        }

        public class ClassProperties 
        {
            public String propertyName = "";
            public String qualifier = "";
            public String dataType = "";
            public Boolean isStatic = false;
            public Boolean isFinal = false;
        }

        public class ClassMethods
        {
            public String annotation = "";
            public String methodName = "";
            public String qualifier = "";
            public Boolean isStatic = false;
            public Boolean isOverride = false;
            public String returnDataType = "";

            // Key = sObject API Name, Value = SOQL Statements
            public Dictionary<String, List<String>> soqlStatements;
            public Dictionary<String, String> objToFieldsReferenced;

            public Dictionary<String, List<ObjectVarToType>> objVarToType;

            public ClassMethods() 
            {
                this.soqlStatements = new Dictionary<String, List<String>>();
                this.objToFieldsReferenced = new Dictionary<String, String>();
                this.objVarToType = new Dictionary<String, List<ObjectVarToType>>();
            }
        }

        public class FlowProcess 
        {
            public String apiName = "";
            public String label = "";
            public String objectName = "";
            public String flowProcessType = "";
            public String recordTriggerTrype = "";
            public String triggerType = "";
            public Boolean isActive = false;
            public String apiVersion = "";
            public String runInMode = "";
            public Dictionary<String, List<String>> recordCreates;
            public Dictionary<String, List<String>> recordUpdates;
            public Dictionary<String, List<String>> recordDeletes;

            public FlowProcess()
            {
                recordCreates = new Dictionary<string, List<string>>();
                recordUpdates = new Dictionary<string, List<string>>();
                recordDeletes = new Dictionary<string, List<string>>();
            }
        }

        public class Workflows 
        {
            // These are the rules
            public String objectName = "";
            public String workflowRuleName = "";
            public String triggerType = "";
            public Boolean isActive = false;

            // These are the field updates
            // Value = List<String> fields being updated
            public List<WorkflowFieldUpdate> fieldUpdates;

            public Workflows()
            {
                fieldUpdates = new List<WorkflowFieldUpdate>();
            }
        }

        public class WorkflowFieldUpdate 
        {
            // Object being updated
            public String fieldUpdateName = "";
            public String fieldUpdateLabel = "";
            public String fieldName = "";
            public Boolean notifyAssignee = false;
            public Boolean reevaluateOnChange = false;
        }
    }
}
