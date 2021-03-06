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

namespace SalesforceMetadata
{
    public partial class frmParseDebugLogs : Form
    {

        // SUMMARY: The only instances where a new TAB will be added to the text file are when it is a CODE_UNIT_STARTED and METHOD_ENTRY

        // Key is the event tag, value is the tab number. It can be 0 for no tab, 1 to increment the tab count or -1 to decrement the tab count
        private HashSet<String> debugEventTags;


        // The last event tag placed here is what will be searched for.
        // If the debugEventTags dictionary has a key/value pair and the value is not empty, then as the code parses through the text file, it will be looking for that ending tag
        // Ex: The parsing process comes across BULK_HEAP_ALLOCATE. Since BULK_HEAP_ALLOCATE does not have a value, no value will be added to the eventTagHierarchy list
        // But then it comes across METHOD_ENTRY, and places METHOD_EXIT at the end of the list
        // It then comes across DML_BEGIN and places DML_END at the end of the list.
        // When the code comes across DML_END it writes the text portion of the debug log to the file and removes DML_END from the list
        // When it comes across METHOD_EXIT, it writes the text portion of the debug log to the file and removes METHOD_EXIT from the list
        // The reason for using a List is I need to guarantee the order. A HashSet won't guarantee order
        private List<String> eventTagHierarchy;

        Int32 currentLevel = 0;
        //Int32 lastLevel = 1;

        //List<TreeNode> lastNodes = new List<TreeNode>();
        //TreeNode lastNode = new TreeNode();
        //TreeNode priorToLastNode = new TreeNode();

        //TreeNode variableScopeBeginNode = new TreeNode();

        public frmParseDebugLogs()
        {
            InitializeComponent();
        }

        private void allocateEventTagsToDictinary()
        {
            //CheckedListBox.CheckedItemCollection itemsColl = this.parseSpecifics.CheckedItems;
            HashSet<String> checkedDebugItems = new HashSet<string>();
            //for (Int32 i = 0; i < this.parseSpecifics.CheckedItems.Count; i++)
            //{
            //    checkedDebugItems.Add((String)this.parseSpecifics.CheckedItems[i]);
            //}


            debugEventTags = new HashSet<String>();

            debugEventTags.Add("USER_INFO");
            debugEventTags.Add("EXCEPTION_THROWN");
            debugEventTags.Add("FATAL_ERROR");
            debugEventTags.Add("FLOW_CREATE_INTERVIEW_ERROR");
            debugEventTags.Add("USER_DEBUG");

            if (checkedDebugItems.Contains("All") || checkedDebugItems.Contains("HEAP_ALLOCATE"))
            {
                debugEventTags.Add("HEAP_ALLOCATE");
                debugEventTags.Add("HEAP_DEALLOCATE");
            }

            if (checkedDebugItems.Contains("All") || checkedDebugItems.Contains("BULK_HEAP_ALLOCATE"))
            {
                debugEventTags.Add("BULK_HEAP_ALLOCATE");
            }

            if (checkedDebugItems.Contains("All") || checkedDebugItems.Contains("EXECUTION_STARTED"))
            {
                debugEventTags.Add("EXECUTION_STARTED");
                debugEventTags.Add("EXECUTION_FINISHED");
            }

            if (checkedDebugItems.Contains("All") || checkedDebugItems.Contains("STATEMENT_EXECUTE"))
            {
                debugEventTags.Add("STATEMENT_EXECUTE");
            }

            if (checkedDebugItems.Contains("All") || checkedDebugItems.Contains("CALLOUT_REQUEST"))
            {
                debugEventTags.Add("CALLOUT_REQUEST");
                debugEventTags.Add("CALLOUT_RESPONSE");
            }

            if (checkedDebugItems.Contains("All") || checkedDebugItems.Contains("CODE_UNIT_STARTED"))
            {
                debugEventTags.Add("CODE_UNIT_STARTED");
                debugEventTags.Add("CODE_UNIT_FINISHED");
            }

            if (checkedDebugItems.Contains("All") || checkedDebugItems.Contains("CONSTRUCTOR_ENTRY"))
            {

                debugEventTags.Add("CONSTRUCTOR_ENTRY");
                debugEventTags.Add("CONSTRUCTOR_EXIT");
            }

            if (checkedDebugItems.Contains("All") || checkedDebugItems.Contains("METHOD_ENTRY"))
            {
                debugEventTags.Add("CODE_UNIT_STARTED");
                debugEventTags.Add("CODE_UNIT_FINISHED");

                debugEventTags.Add("METHOD_ENTRY");
                debugEventTags.Add("METHOD_EXIT");

                debugEventTags.Add("DML_BEGIN");
                debugEventTags.Add("DML_END");

                debugEventTags.Add("SOQL_EXECUTE_BEGIN");
                debugEventTags.Add("SOQL_EXECUTE_END");

                debugEventTags.Add("WF_FIELD_UPDATE");
            }

            if (checkedDebugItems.Contains("All") || checkedDebugItems.Contains("ENTERING_MANAGED_PKG"))
            {
                debugEventTags.Add("ENTERING_MANAGED_PKG");
            }

            if (checkedDebugItems.Contains("All") || checkedDebugItems.Contains("FLOW_CREATE_INTERVIEW_BEGIN"))
            {
                debugEventTags.Add("WF_FLOW_ACTION_BEGIN");
                debugEventTags.Add("WF_FLOW_ACTION_END");

                debugEventTags.Add("WF_FLOW_ACTION_DETAIL");

                debugEventTags.Add("FLOW_CREATE_INTERVIEW_BEGIN");
                debugEventTags.Add("FLOW_CREATE_INTERVIEW_END");

                debugEventTags.Add("FLOW_START_INTERVIEW_BEGIN");
                debugEventTags.Add("FLOW_START_INTERVIEW_END");

                debugEventTags.Add("FLOW_ELEMENT_BEGIN");
                debugEventTags.Add("FLOW_ELEMENT_END");
                debugEventTags.Add("FLOW_ELEMENT_ERROR");
                debugEventTags.Add("FLOW_ELEMENT_FAULT");

                debugEventTags.Add("FLOW_VALUE_ASSIGNMENT");
            }

            if (checkedDebugItems.Contains("All") || checkedDebugItems.Contains("DML_BEGIN"))
            {
                debugEventTags.Add("DML_BEGIN");
                debugEventTags.Add("DML_END");
            }

            if (checkedDebugItems.Contains("All") || checkedDebugItems.Contains("SOQL_EXECUTE_BEGIN"))
            {
                debugEventTags.Add("SOQL_EXECUTE_BEGIN");
                debugEventTags.Add("SOQL_EXECUTE_END");
            }

            if (checkedDebugItems.Contains("All") || checkedDebugItems.Contains("VALIDATION_RULE"))
            {
                debugEventTags.Add("VALIDATION_RULE");
                debugEventTags.Add("VALIDATION_FORMULA");
                debugEventTags.Add("VALIDATION_PASS");
                debugEventTags.Add("VALIDATION_FAIL");
                debugEventTags.Add("VALIDATION_ERROR");
            }

            if (checkedDebugItems.Contains("All") || checkedDebugItems.Contains("WF_FLOW_ACTION_BEGIN"))
            {
                debugEventTags.Add("WF_FLOW_ACTION_BEGIN");
                debugEventTags.Add("WF_FLOW_ACTION_END");

                debugEventTags.Add("WF_ACTION");
                debugEventTags.Add("WF_ACTIONS_END");

                debugEventTags.Add("WF_APPROVAL");
                debugEventTags.Add("WF_ACTION_TASK");
                debugEventTags.Add("WF_EMAIL_ALERT");
                debugEventTags.Add("WF_FIELD_UPDATE");
                debugEventTags.Add("WF_OUTBOUND_MSG");

                debugEventTags.Add("WF_FLOW_ACTION_DETAIL");

                debugEventTags.Add("FLOW_CREATE_INTERVIEW_BEGIN");
                debugEventTags.Add("FLOW_CREATE_INTERVIEW_END");

                debugEventTags.Add("FLOW_START_INTERVIEW_BEGIN");
                debugEventTags.Add("FLOW_START_INTERVIEW_END");

                debugEventTags.Add("FLOW_ELEMENT_BEGIN");
                debugEventTags.Add("FLOW_ELEMENT_END");
                debugEventTags.Add("FLOW_ELEMENT_ERROR");
                debugEventTags.Add("FLOW_ELEMENT_FAULT");

                debugEventTags.Add("FLOW_VALUE_ASSIGNMENT");
            }

            if (checkedDebugItems.Contains("All") 
                || checkedDebugItems.Contains("VARIABLE_SCOPE_BEGIN")
                || checkedDebugItems.Contains("VARIABLE_ASSIGNMENT"))
            {
                debugEventTags.Add("VARIABLE_SCOPE_BEGIN");
                debugEventTags.Add("VARIABLE_ASSIGNMENT");
            }

        }

        private void tbDebugFile_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "All Files (*.*)|*.*";
            ofd.Title = "Debug Log to Parse";

            DialogResult dr = ofd.ShowDialog();

            if (dr == DialogResult.OK) this.tbDebugFile.Text = ofd.FileName;
        }

        private void btnParseDebugLogFile_Click(object sender, EventArgs e)
        {
            if (this.tbDebugFile.Text == null || this.tbDebugFile.Text == "")
            {
                MessageBox.Show("Please select a debug file to parse to continue");
                return;
            }

            allocateEventTagsToDictinary();

            String[] fileNameSplit = this.tbDebugFile.Text.Split(char.Parse("\\"));
            String folderSaveLocation = "";

            for (Int32 i = 0; i < fileNameSplit.Length - 1; i++)
            {
                folderSaveLocation += fileNameSplit[i] + "\\";
            }


            Boolean firstCodeUnitReached = true;
            Int32 tabCount = 0;
            String milSecStart = "";
            String milSecEnd = "";

            StreamWriter debugSW = new StreamWriter(folderSaveLocation + "DebugLog_Aggregations.txt");

            // Open file for reading
            StreamReader debugSR = new StreamReader(this.tbDebugFile.Text);
            while (debugSR.EndOfStream == false)
            {
                String line = debugSR.ReadLine();
                String[] columnElements = line.Split(char.Parse("|"));

                if(columnElements.Length > 1)
                {
                    if (columnElements[1] == DebugEventTags.BULK_HEAP_ALLOCATE)
                    {

                    }
                    else if (columnElements[1] == DebugEventTags.CODE_UNIT_STARTED)
                    {
                        if (firstCodeUnitReached == true)
                        {
                            firstCodeUnitReached = false;

                            debugSW.Write(tabCount.ToString() + " | " + columnElements[0].ToString() + " | ");

                            for (Int32 tc = 0; tc < tabCount; tc++)
                            {
                                debugSW.Write("\t");
                            }

                            debugSW.Write("CODE_UNIT_STARTED: ");

                            for (Int32 ce = 0; ce < columnElements.Length; ce++)
                            {
                                if (ce > 2) debugSW.Write(columnElements[ce] + " ");
                            }

                            debugSW.Write(Environment.NewLine);

                            tabCount++;

                        }
                        else
                        {
                            debugSW.Write(tabCount.ToString() + " | " + columnElements[0].ToString() + " | ");

                            for (Int32 tc = 0; tc < tabCount; tc++)
                            {
                                debugSW.Write("\t");
                            }

                            debugSW.Write("CODE_UNIT_STARTED: ");

                            for (Int32 ce = 0; ce < columnElements.Length; ce++)
                            {
                                if (ce > 2) debugSW.Write(columnElements[ce] + " ");
                            }

                            debugSW.Write(Environment.NewLine);

                            tabCount++;
                        }
                    }
                    else if (columnElements[1] == DebugEventTags.CODE_UNIT_FINISHED)
                    {
                        if (tabCount > 0) tabCount--;

                        debugSW.Write(tabCount.ToString() + " | " + columnElements[0].ToString() + " | ");

                        for (Int32 tc = 0; tc < tabCount; tc++)
                        {
                            debugSW.Write("\t");
                        }

                        debugSW.Write("CODE_UNIT_FINISHED");
                        debugSW.Write(Environment.NewLine);
                        debugSW.Write(Environment.NewLine);

                    }
                    else if (columnElements[1] == DebugEventTags.CONSTRUCTOR_ENTRY)
                    {
                        debugSW.Write(tabCount.ToString() + " | " + columnElements[0].ToString() + " | ");

                        for (Int32 tc = 0; tc < tabCount; tc++)
                        {
                            debugSW.Write("\t");
                        }

                        debugSW.Write("CONSTRUCTOR_ENTRY: ");

                        for (Int32 ce = 0; ce < columnElements.Length; ce++)
                        {
                            if (ce > 2) debugSW.Write(columnElements[ce] + " ");
                        }

                        debugSW.Write(Environment.NewLine);

                        tabCount++;
                    }
                    else if (columnElements[1] == DebugEventTags.CONSTRUCTOR_EXIT)
                    {
                        if (tabCount > 0) tabCount--;
                    }
                    else if (columnElements[1] == DebugEventTags.DML_BEGIN)
                    {
                        debugSW.Write(tabCount.ToString() + " | " + columnElements[0].ToString() + " | ");

                        for (Int32 tc = 0; tc < tabCount; tc++)
                        {
                            debugSW.Write("\t");
                        }

                        debugSW.Write("DML_BEGIN: ");

                        for (Int32 ce = 0; ce < columnElements.Length; ce++)
                        {
                            if (ce > 1) debugSW.Write(columnElements[ce] + " ");
                        }

                        debugSW.Write(Environment.NewLine);

                        tabCount++;
                    }
                    else if (columnElements[1] == DebugEventTags.DML_END)
                    {
                        if (tabCount > 0) tabCount--;
                    }
                    else if (columnElements[1] == DebugEventTags.ENTERING_MANAGED_PKG)
                    {
                        debugSW.Write(tabCount.ToString() + " | " + columnElements[0].ToString() + " | ");

                        for (Int32 tc = 0; tc < tabCount; tc++)
                        {
                            debugSW.Write("\t");
                        }

                        debugSW.Write("ENTERING_MANAGED_PKG: " + columnElements[2] + Environment.NewLine);

                        //tabCount++;
                    }
                    else if (columnElements[1] == DebugEventTags.EXCEPTION_THROWN)
                    {
                        debugSW.Write(tabCount.ToString() + " | " + columnElements[0].ToString() + " | ");

                        for (Int32 tc = 0; tc < tabCount; tc++)
                        {
                            debugSW.Write("\t");
                        }

                        debugSW.Write("EXCEPTION_THROWN: ");

                        for (Int32 ce = 0; ce < columnElements.Length; ce++)
                        {
                            if (ce > 2) debugSW.Write(columnElements[ce] + " ");
                        }

                        debugSW.Write(Environment.NewLine);

                        //tabCount++;
                    }
                    else if (columnElements[1] == DebugEventTags.EXECUTION_STARTED)
                    {
                        debugSW.Write(tabCount.ToString() + " | " + columnElements[0].ToString() + " | ");

                        for (Int32 tc = 0; tc < tabCount; tc++)
                        {
                            debugSW.Write("\t");
                        }

                        debugSW.Write("EXECUTION_STARTED" + Environment.NewLine);

                    }
                    else if (columnElements[1] == DebugEventTags.EXECUTION_FINISHED)
                    {
                        //if (tabCount > 0) tabCount--;
                    }
                    else if (columnElements[1] == DebugEventTags.FATAL_ERROR)
                    {
                        debugSW.Write(tabCount.ToString() + " | " + columnElements[0].ToString() + " | ");

                        for (Int32 tc = 0; tc < tabCount; tc++)
                        {
                            debugSW.Write("\t");
                        }

                        debugSW.Write("FATAL_ERROR: " + columnElements[2] + Environment.NewLine);

                        line = debugSR.ReadLine();
                        columnElements = line.Split(char.Parse("|"));
                        while (columnElements.Length == 1)
                        {
                            if (line != "")
                            {
                                for (Int32 tc = 0; tc < tabCount + 9; tc++)
                                {
                                    debugSW.Write("\t");
                                }

                                debugSW.WriteLine(line);
                            }

                            line = debugSR.ReadLine();
                            columnElements = line.Split(char.Parse("|"));
                        }
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_CREATE_INTERVIEW_BEGIN)
                    {
                        debugSW.Write(tabCount.ToString() + " | " + columnElements[0].ToString() + " | ");

                        for (Int32 tc = 0; tc < tabCount; tc++)
                        {
                            debugSW.Write("\t");
                        }

                        debugSW.Write("FLOW_CREATE_INTERVIEW_BEGIN" + Environment.NewLine);

                        tabCount++;
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_CREATE_INTERVIEW_END)
                    {
                        if (tabCount > 0) tabCount--;
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_START_INTERVIEW_BEGIN)
                    {
                        debugSW.Write(tabCount.ToString() + " | " + columnElements[0].ToString() + " | ");

                        for (Int32 tc = 0; tc < tabCount; tc++)
                        {
                            debugSW.Write("\t");
                        }

                        debugSW.Write("FLOW_START_INTERVIEW_BEGIN: ");

                        for (Int32 ce = 0; ce < columnElements.Length; ce++)
                        {
                            if (ce > 2) debugSW.Write(columnElements[ce] + " ");
                        }

                        debugSW.Write(Environment.NewLine);

                        tabCount++;
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_START_INTERVIEW_END)
                    {
                        if (tabCount > 0) tabCount--;
                    }
                    else if (columnElements[1] == DebugEventTags.HEAP_ALLOCATE)
                    {

                    }
                    else if (columnElements[1] == DebugEventTags.METHOD_ENTRY)
                    {
                        debugSW.Write(tabCount.ToString() + " | " + columnElements[0].ToString() + " | ");

                        for (Int32 tc = 0; tc < tabCount; tc++)
                        {
                            debugSW.Write("\t");
                        }

                        debugSW.Write("METHOD_ENTRY: ");

                        for (Int32 ce = 0; ce < columnElements.Length; ce++)
                        {
                            if (ce > 1) debugSW.Write(columnElements[ce] + " ");
                        }

                        debugSW.Write(Environment.NewLine);

                        tabCount++;
                    }
                    else if (columnElements[1] == DebugEventTags.METHOD_EXIT)
                    {
                        if (tabCount > 0) tabCount--;

                        debugSW.Write(tabCount.ToString() + " | " + columnElements[0].ToString() + " | ");

                        for (Int32 tc = 0; tc < tabCount; tc++)
                        {
                            debugSW.Write("\t");
                        }

                        debugSW.Write("METHOD_EXIT");
                        debugSW.Write(Environment.NewLine);
                    }
                    else if (columnElements[1] == DebugEventTags.SOQL_EXECUTE_BEGIN)
                    {
                        debugSW.Write(tabCount.ToString() + " | " + columnElements[0].ToString() + " | ");

                        for (Int32 tc = 0; tc < tabCount; tc++)
                        {
                            debugSW.Write("\t");
                        }

                        debugSW.Write("SOQL_EXECUTE_BEGIN: ");

                        for (Int32 ce = 0; ce < columnElements.Length; ce++)
                        {
                            if (ce > 1) debugSW.Write(columnElements[ce] + " ");
                        }

                        debugSW.Write(Environment.NewLine);
                        debugSW.Write(Environment.NewLine);

                        tabCount++;
                    }
                    else if (columnElements[1] == DebugEventTags.SOQL_EXECUTE_END)
                    {
                        if (tabCount > 0) tabCount--;
                    }
                    else if (columnElements[1] == DebugEventTags.STATEMENT_EXECUTE)
                    {

                    }
                    else if (columnElements[1] == DebugEventTags.USER_DEBUG)
                    {
                        debugSW.Write(tabCount.ToString() + " | " + columnElements[0].ToString() + " | ");

                        for (Int32 tc = 0; tc < tabCount; tc++)
                        {
                            debugSW.Write("\t");
                        }

                        debugSW.Write("USER_DEBUG: ");

                        for (Int32 ce = 0; ce < columnElements.Length; ce++)
                        {
                            if(ce == 2) debugSW.Write(columnElements[ce] + " ");
                            if (ce > 3) debugSW.Write(columnElements[ce] + " ");
                        }

                        debugSW.Write(Environment.NewLine);
                    }
                    else if (columnElements[1] == DebugEventTags.USER_INFO)
                    {

                    }
                    else if (columnElements[1] == DebugEventTags.VARIABLE_ASSIGNMENT)
                    {
                        Int32 tCnt = tabCount + 2;

                        debugSW.Write(tCnt.ToString() + " | " + columnElements[0].ToString() + " | ");

                        for (Int32 tc = 0; tc < tCnt; tc++)
                        {
                            debugSW.Write("\t");
                        }

                        debugSW.Write("VARIABLE_ASSIGNMENT: ");

                        for (Int32 ce = 0; ce < columnElements.Length; ce++)
                        {
                            if(ce > 1) debugSW.Write(columnElements[ce] + " ");
                        }

                        debugSW.Write(Environment.NewLine);

                    }
                    else if (columnElements[1] == DebugEventTags.VARIABLE_SCOPE_BEGIN)
                    {
                        Int32 tCnt = tabCount + 1;

                        debugSW.Write(tCnt.ToString() + " | " + columnElements[0].ToString() + " | ");

                        for (Int32 tc = 0; tc < tCnt; tc++)
                        {
                            debugSW.Write("\t");
                        }

                        debugSW.Write("VARIABLE_SCOPE_BEGIN: ");

                        for (Int32 ce = 0; ce < columnElements.Length; ce++)
                        {
                            if(ce > 1) debugSW.Write(columnElements[ce] + " ");
                        }

                        debugSW.Write(Environment.NewLine);

                    }
                    else if (columnElements[1] == DebugEventTags.WF_FLOW_ACTION_BEGIN)
                    {
                        debugSW.Write(tabCount.ToString() + " | " + columnElements[0].ToString() + " | ");

                        for (Int32 tc = 0; tc < tabCount; tc++)
                        {
                            debugSW.Write("\t");
                        }

                        debugSW.Write("WF_FLOW_ACTION_BEGIN" + Environment.NewLine);

                        tabCount++;

                    }
                    else if (columnElements[1] == DebugEventTags.WF_FLOW_ACTION_END)
                    {
                        if (tabCount > 0) tabCount--;
                    }
                }
            }

            debugSR.Close();
            if (debugSW != null) debugSW.Close();

            MessageBox.Show("Debug Parsing Complete");

        }


        public void writeTablesToStream(Int32 tabCount)
        {

        }

        public class DebugLogWrapper
        {
            Double timeBlockInMilliseconds;
            List<String> codeBlock;
        }

        private void btnDebugReplay_Click(object sender, EventArgs e)
        {
            //this.lastNode = new TreeNode();
            this.currentLevel = 0;

            List<String> debugLogWithLevels = new List<string>();

            if (this.tbDebugFile.Text == null || this.tbDebugFile.Text == "")
            {
                MessageBox.Show("Please select a debug file to parse to continue");
                return;
            }

            allocateEventTagsToDictinary();

            String milSecStart = "";
            String milSecEnd = "";

            StreamReader debugSR = new StreamReader(this.tbDebugFile.Text);

            while (debugSR.EndOfStream == false)
            {
                String line = debugSR.ReadLine();
                String[] columnElements = line.Split(char.Parse("|"));

                if (columnElements.Length > 1)
                {
                    // Check if we have a 

                    if (columnElements[1] == DebugEventTags.BULK_HEAP_ALLOCATE)
                    {

                    }
                    else if (columnElements[1] == DebugEventTags.CODE_UNIT_STARTED)
                    {
                        this.currentLevel++;

                        String treeNodeText = "CODE_UNIT_STARTED: ";

                        for (Int32 ce = 0; ce < columnElements.Length; ce++)
                        {
                            if (ce > 2) treeNodeText = treeNodeText + columnElements[ce] + " ";
                        }

                        debugLogWithLevels.Add(this.currentLevel.ToString() + "|" + treeNodeText);
                    }
                    else if (columnElements[1] == DebugEventTags.CODE_UNIT_FINISHED)
                    {
                        if (this.currentLevel > 0) this.currentLevel--;
                    }
                    else if (columnElements[1] == DebugEventTags.CONSTRUCTOR_ENTRY)
                    {
                        this.currentLevel++;

                        String treeNodeText = "CONSTRUCTOR_ENTRY: ";

                        for (Int32 ce = 0; ce < columnElements.Length; ce++)
                        {
                            if (ce > 2) treeNodeText = treeNodeText + columnElements[ce] + " ";
                        }

                        debugLogWithLevels.Add(this.currentLevel.ToString() + "|" + treeNodeText);
                    }
                    else if (columnElements[1] == DebugEventTags.CONSTRUCTOR_EXIT)
                    {
                        if (this.currentLevel > 0) this.currentLevel--;
                    }
                    else if (columnElements[1] == DebugEventTags.DML_BEGIN)
                    {
                        this.currentLevel++;

                        String treeNodeText = "DML_BEGIN: ";

                        for (Int32 ce = 0; ce < columnElements.Length; ce++)
                        {
                            if (ce > 1) treeNodeText = treeNodeText + columnElements[ce] + " ";
                        }

                        debugLogWithLevels.Add(this.currentLevel.ToString() + "|" + treeNodeText);
                    }
                    else if (columnElements[1] == DebugEventTags.DML_END)
                    {
                        if (this.currentLevel > 0) this.currentLevel--;
                    }
                    else if (columnElements[1] == DebugEventTags.ENTERING_MANAGED_PKG)
                    {
                        String treeNodeText = "ENTERING_MANAGED_PKG: " + columnElements[2];
                        TreeNode newNode = new TreeNode(treeNodeText);

                        Int32 tempCurrentLevel = this.currentLevel + 1;
                        debugLogWithLevels.Add(tempCurrentLevel.ToString() + "|" + treeNodeText);
                    }
                    else if (columnElements[1] == DebugEventTags.EXCEPTION_THROWN)
                    {
                        String treeNodeText = "EXCEPTION_THROWN: ";

                        for (Int32 ce = 0; ce < columnElements.Length; ce++)
                        {
                            if (ce > 2) treeNodeText = treeNodeText + columnElements[ce] + " ";
                        }

                        debugLogWithLevels.Add(this.currentLevel.ToString() + "|" + treeNodeText);
                    }
                    else if (columnElements[1] == DebugEventTags.EXECUTION_STARTED)
                    {
                        String treeNodeText = "EXECUTION_STARTED";
                        TreeNode newNode = new TreeNode(treeNodeText);

                        debugLogWithLevels.Add(this.currentLevel.ToString() + "|" + treeNodeText);
                    }
                    else if (columnElements[1] == DebugEventTags.EXECUTION_FINISHED)
                    {

                    }
                    else if (columnElements[1] == DebugEventTags.FATAL_ERROR)
                    {
                        String treeNodeText = "FATAL_ERROR: " + columnElements[2];
                        line = debugSR.ReadLine();
                        columnElements = line.Split(char.Parse("|"));
                        while (columnElements.Length == 1)
                        {
                            if (line != "")
                            {
                                treeNodeText = treeNodeText + ", " + line;
                            }

                            line = debugSR.ReadLine();
                            columnElements = line.Split(char.Parse("|"));
                        }

                        debugLogWithLevels.Add(this.currentLevel.ToString() + "|" + treeNodeText);
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_CREATE_INTERVIEW_BEGIN)
                    {
                        this.currentLevel++;

                        String treeNodeText = "FLOW_CREATE_INTERVIEW_BEGIN";

                        debugLogWithLevels.Add(this.currentLevel.ToString() + "|" + treeNodeText);
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_CREATE_INTERVIEW_END)
                    {
                        if (this.currentLevel > 0) this.currentLevel--;
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_ELEMENT_BEGIN)
                    {
                        this.currentLevel++;
                        
                        String treeNodeText = "FLOW_ELEMENT_BEGIN: " + columnElements[3] + " " + columnElements[4];

                        debugLogWithLevels.Add(this.currentLevel.ToString() + "|" + treeNodeText);

                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_ELEMENT_END)
                    {
                        if (this.currentLevel > 0) this.currentLevel--;
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_START_INTERVIEW_BEGIN)
                    {
                        this.currentLevel++;

                        String treeNodeText = "FLOW_START_INTERVIEW_BEGIN: ";

                        for (Int32 ce = 0; ce < columnElements.Length; ce++)
                        {
                            if (ce > 2) treeNodeText = treeNodeText + columnElements[ce] + " ";
                        }

                        debugLogWithLevels.Add(this.currentLevel.ToString() + "|" + treeNodeText);
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_START_INTERVIEW_END)
                    {
                        if (this.currentLevel > 0) this.currentLevel--;
                    }
                    else if (columnElements[1] == DebugEventTags.HEAP_ALLOCATE)
                    {

                    }
                    else if (columnElements[1] == DebugEventTags.METHOD_ENTRY)
                    {
                        this.currentLevel++;

                        String treeNodeText = "METHOD_ENTRY: ";

                        for (Int32 ce = 0; ce < columnElements.Length; ce++)
                        {
                            if (ce > 1) treeNodeText = treeNodeText + columnElements[ce] + " ";
                        }

                        debugLogWithLevels.Add(this.currentLevel.ToString() + "|" + treeNodeText);
                    }
                    else if (columnElements[1] == DebugEventTags.METHOD_EXIT)
                    {
                        if (this.currentLevel > 0) this.currentLevel--;
                    }
                    else if (columnElements[1] == DebugEventTags.SOQL_EXECUTE_BEGIN)
                    {
                        this.currentLevel++;

                        String treeNodeText = "SOQL_EXECUTE_BEGIN: ";

                        for (Int32 ce = 0; ce < columnElements.Length; ce++)
                        {
                            if (ce > 1) treeNodeText = treeNodeText + columnElements[ce] + " ";
                        }

                        debugLogWithLevels.Add(this.currentLevel.ToString() + "|" + treeNodeText);
                    }
                    else if (columnElements[1] == DebugEventTags.SOQL_EXECUTE_END)
                    {
                        if (this.currentLevel > 0) this.currentLevel--;
                    }
                    else if (columnElements[1] == DebugEventTags.USER_DEBUG)
                    {
                        String treeNodeText = "USER_DEBUG: ";

                        for (Int32 ce = 0; ce < columnElements.Length; ce++)
                        {
                            if (ce == 2) treeNodeText = treeNodeText + columnElements[ce] + " ";
                            if (ce > 3) treeNodeText = treeNodeText + columnElements[ce] + " ";
                        }

                        Int32 tempCurrentLevel = this.currentLevel + 1;
                        debugLogWithLevels.Add(tempCurrentLevel.ToString() + "|" + treeNodeText);
                    }
                    else if (columnElements[1] == DebugEventTags.VARIABLE_ASSIGNMENT)
                    {
                        String treeNodeText = "VARIABLE_ASSIGNMENT: ";

                        for (Int32 ce = 0; ce < columnElements.Length; ce++)
                        {
                            if (ce > 1) treeNodeText = treeNodeText + columnElements[ce] + " ";
                        }

                        Int32 tempCurrentLevel = this.currentLevel + 2;
                        debugLogWithLevels.Add(tempCurrentLevel.ToString() + "|" + treeNodeText);
                    }
                    else if (columnElements[1] == DebugEventTags.VARIABLE_SCOPE_BEGIN)
                    {
                        String treeNodeText = "VARIABLE_SCOPE_BEGIN: ";

                        for (Int32 ce = 0; ce < columnElements.Length; ce++)
                        {
                            if (ce > 1) treeNodeText = treeNodeText + columnElements[ce] + " ";
                        }

                        Int32 tempCurrentLevel = this.currentLevel + 1;
                        debugLogWithLevels.Add(tempCurrentLevel.ToString() + "|" + treeNodeText);
                    }
                    else if (columnElements[1] == DebugEventTags.WF_FLOW_ACTION_BEGIN)
                    {
                        this.currentLevel++;

                        String treeNodeText = "WF_FLOW_ACTION_BEGIN";
                        debugLogWithLevels.Add(this.currentLevel.ToString() + "|" + treeNodeText);

                    }
                    else if (columnElements[1] == DebugEventTags.WF_FLOW_ACTION_END)
                    {
                        if (this.currentLevel > 0) this.currentLevel--;
                    }
                }
            }

            populateTreeNodes(debugLogWithLevels);
        }


        private void populateTreeNodes(List<string> debugLogWithLevels)
        {
            Dictionary<Int32, TreeNode> parentTreeNode = new Dictionary<Int32, TreeNode>();

            foreach(String dlText in debugLogWithLevels)
            {
                String[] splitDL = dlText.Split('|');
                Int32 currentLevel = Int32.Parse(splitDL[0]);

                if (Int32.Parse(splitDL[0]) == 1)
                {
                    parentTreeNode.Clear();

                    TreeNode tnd = new TreeNode(splitDL[1]);
                    this.tvDebugReplay.Nodes.Add(tnd);

                    parentTreeNode.Add(currentLevel, tnd);
                }
                else if (parentTreeNode.ContainsKey(Int32.Parse(splitDL[0])))
                {
                    parentTreeNode.Remove(currentLevel);

                    TreeNode parentNode = parentTreeNode[currentLevel - 1];
                    TreeNode tnd = new TreeNode(splitDL[1]);
                    parentNode.Nodes.Add(tnd);

                    parentTreeNode.Add(currentLevel, tnd);
                }
                else
                {
                    TreeNode parentNode = parentTreeNode[currentLevel - 1];
                    TreeNode tnd = new TreeNode(splitDL[1]);
                    parentNode.Nodes.Add(tnd);

                    parentTreeNode.Add(currentLevel, tnd);
                }
            }
        }

        private void tvDebugReplay_AfterExpand(object sender, TreeViewEventArgs e)
        {
            TreeNode currentNode = e.Node;
            expandAllNodes(currentNode);
        }


        private void expandAllNodes(TreeNode currentNode)
        {
            foreach (TreeNode tnd in currentNode.Nodes)
            {
                tnd.Expand();

                if (tnd.Nodes.Count > 0)
                {
                    expandAllNodes(tnd);
                }
            }
        }
    }
}
