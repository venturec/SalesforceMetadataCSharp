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
        private Dictionary<String, Int32> debugEventTags;


        // The last event tag placed here is what will be searched for.
        // If the debugEventTags dictionary has a key/value pair and the value is not empty, then as the code parses through the text file, it will be looking for that ending tag
        // Ex: The parsing process comes across BULK_HEAP_ALLOCATE. Since BULK_HEAP_ALLOCATE does not have a value, no value will be added to the eventTagHierarchy list
        // But then it comes across METHOD_ENTRY, and places METHOD_EXIT at the end of the list
        // It then comes across DML_BEGIN and places DML_END at the end of the list.
        // When the code comes across DML_END it writes the text portion of the debug log to the file and removes DML_END from the list
        // When it comes across METHOD_EXIT, it writes the text portion of the debug log to the file and removes METHOD_EXIT from the list
        // The reason for using a List is I need to guarantee the order. A HashSet won't guarantee order
        private List<String> eventTagHierarchy;


        public frmParseDebugLogs()
        {
            InitializeComponent();
            this.parseSpecifics.SetItemChecked(0, true);
            allocateEventTagsToDictinary();
        }

        private void allocateEventTagsToDictinary()
        {
            debugEventTags = new Dictionary<String, Int32>();

            debugEventTags.Add("BULK_HEAP_ALLOCATE", 0);
            debugEventTags.Add("EXCEPTION_THROWN", 0);
            debugEventTags.Add("EXECUTION_STARTED", 0);
            debugEventTags.Add("EXECUTION_FINISHED", 0);
            debugEventTags.Add("FATAL_ERROR", 0);
            debugEventTags.Add("HEAP_ALLOCATE", 0);
            debugEventTags.Add("STATEMENT_EXECUTE", 0);
            debugEventTags.Add("USER_DEBUG", 0);
            debugEventTags.Add("USER_INFO", 0);
            debugEventTags.Add("VARIABLE_ASSIGNMENT", 0);
            debugEventTags.Add("VARIABLE_SCOPE_BEGIN", 0);

            debugEventTags.Add("CODE_UNIT_STARTED", 1);
            debugEventTags.Add("CODE_UNIT_FINISHED", -1);

            debugEventTags.Add("CONSTRUCTOR_ENTRY", 1);
            debugEventTags.Add("CONSTRUCTOR_EXIT", -1);

            debugEventTags.Add("METHOD_ENTRY", 1);
            debugEventTags.Add("METHOD_EXIT", -1);

            debugEventTags.Add("ENTERING_MANAGED_PKG", 1);

            debugEventTags.Add("FLOW_CREATE_INTERVIEW_BEGIN", 1);
            debugEventTags.Add("FLOW_CREATE_INTERVIEW_END", -1);

            debugEventTags.Add("FLOW_START_INTERVIEW_BEGIN", 1);
            debugEventTags.Add("FLOW_START_INTERVIEW_END", -1);

            debugEventTags.Add("DML_BEGIN", 1);
            debugEventTags.Add("DML_END", -1);

            debugEventTags.Add("SOQL_EXECUTE_BEGIN", 1);
            debugEventTags.Add("SOQL_EXECUTE_END", -1);

            debugEventTags.Add("WF_FLOW_ACTION_BEGIN", 1);
            debugEventTags.Add("WF_FLOW_ACTION_END", 1);


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

                foreach (String evtTag in debugEventTags.Keys)
                {
                    if (line.Contains(evtTag) && evtTag == DebugEventTags.BULK_HEAP_ALLOCATE)
                    {

                    }
                    else if (line.Contains(evtTag) && evtTag == DebugEventTags.CODE_UNIT_STARTED)
                    {
                        if (firstCodeUnitReached == true)
                        {
                            firstCodeUnitReached = false;

                            String[] columnElements = line.Split(char.Parse("|"));

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
                            String[] columnElements = line.Split(char.Parse("|"));

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
                    else if (line.Contains(evtTag) && evtTag == DebugEventTags.CODE_UNIT_FINISHED)
                    {
                        if(tabCount > 0) tabCount--;
                    }
                    else if (line.Contains(evtTag) && evtTag == DebugEventTags.CONSTRUCTOR_ENTRY)
                    {
                        String[] columnElements = line.Split(char.Parse("|"));

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
                    else if (line.Contains(evtTag) && evtTag == DebugEventTags.CONSTRUCTOR_EXIT)
                    {
                        if (tabCount > 0) tabCount--;
                    }
                    else if (line.Contains(evtTag) && evtTag == DebugEventTags.DML_BEGIN)
                    {
                        String[] columnElements = line.Split(char.Parse("|"));

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
                    else if (line.Contains(evtTag) && evtTag == DebugEventTags.DML_END)
                    {
                        if (tabCount > 0) tabCount--;
                    }
                    else if (line.Contains(evtTag) && evtTag == DebugEventTags.ENTERING_MANAGED_PKG)
                    {
                        String[] columnElements = line.Split(char.Parse("|"));

                        debugSW.Write(tabCount.ToString() + " | " + columnElements[0].ToString() + " | ");

                        for (Int32 tc = 0; tc < tabCount; tc++)
                        {
                            debugSW.Write("\t");
                        }

                        debugSW.Write("ENTERING_MANAGED_PKG: " + columnElements[2] + Environment.NewLine);

                        //tabCount++;
                    }
                    else if (line.Contains(evtTag) && evtTag == DebugEventTags.EXCEPTION_THROWN)
                    {
                        String[] columnElements = line.Split(char.Parse("|"));

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
                    else if (line.Contains(evtTag) && evtTag == DebugEventTags.EXECUTION_STARTED)
                    {
                        String[] columnElements = line.Split(char.Parse("|"));

                        debugSW.Write(tabCount.ToString() + " | " + columnElements[0].ToString() + " | ");

                        for (Int32 tc = 0; tc < tabCount; tc++)
                        {
                            debugSW.Write("\t");
                        }

                        debugSW.Write("EXECUTION_STARTED" + Environment.NewLine);

                    }
                    else if (line.Contains(evtTag) && evtTag == DebugEventTags.EXECUTION_FINISHED)
                    {
                        //if (tabCount > 0) tabCount--;
                    }
                    else if (line.Contains(evtTag) && evtTag == DebugEventTags.FATAL_ERROR)
                    {
                        String[] columnElements = line.Split(char.Parse("|"));

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
                    else if (line.Contains(evtTag) && evtTag == DebugEventTags.FLOW_CREATE_INTERVIEW_BEGIN)
                    {
                        String[] columnElements = line.Split(char.Parse("|"));

                        debugSW.Write(tabCount.ToString() + " | " + columnElements[0].ToString() + " | ");

                        for (Int32 tc = 0; tc < tabCount; tc++)
                        {
                            debugSW.Write("\t");
                        }

                        debugSW.Write("FLOW_CREATE_INTERVIEW_BEGIN" + Environment.NewLine);

                        tabCount++;
                    }
                    else if (line.Contains(evtTag) && evtTag == DebugEventTags.FLOW_CREATE_INTERVIEW_END)
                    {
                        if (tabCount > 0) tabCount--;
                    }
                    else if (line.Contains(evtTag) && evtTag == DebugEventTags.FLOW_START_INTERVIEW_BEGIN)
                    {
                        String[] columnElements = line.Split(char.Parse("|"));

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
                    else if (line.Contains(evtTag) && evtTag == DebugEventTags.FLOW_START_INTERVIEW_END)
                    {
                        if (tabCount > 0) tabCount--;
                    }
                    else if (line.Contains(evtTag) && evtTag == DebugEventTags.HEAP_ALLOCATE)
                    {

                    }
                    else if (line.Contains(evtTag) && evtTag == DebugEventTags.METHOD_ENTRY)
                    {
                        String[] columnElements = line.Split(char.Parse("|"));

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
                    else if (line.Contains(evtTag) && evtTag == DebugEventTags.METHOD_EXIT)
                    {
                        if (tabCount > 0) tabCount--;
                    }
                    else if (line.Contains(evtTag) && evtTag == DebugEventTags.SOQL_EXECUTE_BEGIN)
                    {
                        String[] columnElements = line.Split(char.Parse("|"));

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

                        tabCount++;
                    }
                    else if (line.Contains(evtTag) && evtTag == DebugEventTags.SOQL_EXECUTE_END)
                    {
                        if (tabCount > 0) tabCount--;
                    }
                    else if (line.Contains(evtTag) && evtTag == DebugEventTags.STATEMENT_EXECUTE)
                    {

                    }
                    else if (line.Contains(evtTag) && evtTag == DebugEventTags.USER_DEBUG)
                    {
                        String[] columnElements = line.Split(char.Parse("|"));

                        debugSW.Write(tabCount.ToString() + " | " + columnElements[0].ToString() + " | ");

                        for (Int32 tc = 0; tc < tabCount; tc++)
                        {
                            debugSW.Write("\t");
                        }

                        debugSW.Write("USER_DEBUG: ");

                        for (Int32 ce = 0; ce < columnElements.Length; ce++)
                        {
                            if (ce > 3) debugSW.Write(columnElements[ce] + " ");
                        }

                        debugSW.Write(Environment.NewLine);
                    }
                    else if (line.Contains(evtTag) && evtTag == DebugEventTags.USER_INFO)
                    {

                    }
                    else if (line.Contains(evtTag) && evtTag == DebugEventTags.VARIABLE_ASSIGNMENT)
                    {
                        String[] columnElements = line.Split(char.Parse("|"));

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
                    else if (line.Contains(evtTag) && evtTag == DebugEventTags.VARIABLE_SCOPE_BEGIN)
                    {
                        String[] columnElements = line.Split(char.Parse("|"));

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
                    else if (line.Contains(evtTag) && evtTag == DebugEventTags.WF_FLOW_ACTION_BEGIN)
                    {
                        String[] columnElements = line.Split(char.Parse("|"));

                        debugSW.Write(tabCount.ToString() + " | " + columnElements[0].ToString() + " | ");

                        for (Int32 tc = 0; tc < tabCount; tc++)
                        {
                            debugSW.Write("\t");
                        }

                        debugSW.Write("WF_FLOW_ACTION_BEGIN" + Environment.NewLine);

                        tabCount++;

                    }
                    else if (line.Contains(evtTag) && evtTag == DebugEventTags.WF_FLOW_ACTION_END)
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

    }
}
