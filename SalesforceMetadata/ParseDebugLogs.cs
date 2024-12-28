using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesforceMetadata
{
    public partial class ParseDebugLogs : Form
    {
        // SUMMARY: The only instances where a new TAB will be added to the text file are when it is a CODE_UNIT_STARTED and METHOD_ENTRY

        // The last event tag placed here is what will be searched for.
        // If the debugEventTags dictionary has a key/value pair and the value is not empty, then as the code parses through the text file, it will be looking for that ending tag
        // Ex: The parsing process comes across BULK_HEAP_ALLOCATE. Since BULK_HEAP_ALLOCATE does not have a value, no value will be added to the eventTagHierarchy list
        // But then it comes across METHOD_ENTRY, and places METHOD_EXIT at the end of the list
        // It then comes across DML_BEGIN and places DML_END at the end of the list.
        // When the code comes across DML_END it writes the text portion of the debug log to the file and removes DML_END from the list
        // When it comes across METHOD_EXIT, it writes the text portion of the debug log to the file and removes METHOD_EXIT from the list
        // The reason for using a List is I need to guarantee the order. A HashSet won't guarantee order

        private SalesforceCredentials sc;

        public ParseDebugLogs()
        {
            InitializeComponent();
            populateDefaultDebugLogPath();
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

        private void tbFolderPath_DoubleClick(object sender, EventArgs e)
        {
            this.tbFolderPath.Text = UtilityClass.folderBrowserSelectPath("Select the folder containing the debug log files", false, FolderEnum.ReadFrom, Properties.Settings.Default.DebugLogPath);
            
            Properties.Settings.Default.DebugLogPath = this.tbFolderPath.Text;
            Properties.Settings.Default.Save();
        }

        private void tbDebugFile_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "All Files (*.*)|*.*";
            ofd.Title = "Debug Log to Parse";
            ofd.InitialDirectory = Properties.Settings.Default.DebugLogPath;

            DialogResult dr = ofd.ShowDialog();

            if (dr == DialogResult.OK)
            {
                this.tbDebugFile.Text = ofd.FileName;
            }
        }

        private void btnParseDebugLogFile_Click(object sender, EventArgs e)
        {
            if (this.tbDebugFile.Text == null || this.tbDebugFile.Text == "")
            {
                MessageBox.Show("Please select a debug file to parse to continue");
                return;
            }

            String debugFileName = "DebugLog_Aggregations.txt";
            String[] fileNameSplit = this.tbDebugFile.Text.Split(char.Parse("\\"));
            String fileSaveLocation = "";

            Boolean continueProcessing = true;
            for (Int32 i = 0; i < fileNameSplit.Length - 1; i++)
            {
                if (fileNameSplit[i].Contains(" "))
                {
                    continueProcessing = false;
                    MessageBox.Show("Folder cannot contain spaces");
                    break;
                }

                fileSaveLocation += fileNameSplit[i] + "\\";
            }

            fileSaveLocation = fileSaveLocation + debugFileName;

            StreamWriter debugSW = new StreamWriter(fileSaveLocation);

            parseDebugLogFile(this.tbDebugFile.Text, debugSW);

            if (debugSW != null) debugSW.Close();

            if (Properties.Settings.Default.DefaultTextEditorPath == "")
            {
                Process.Start(@"notepad.exe", fileSaveLocation);
            }
            else
            {
                Process.Start(@Properties.Settings.Default.DefaultTextEditorPath, fileSaveLocation);
            }
        }

        private void btnParseFolderDebubLogs_Click(object sender, EventArgs e)
        {
            if (this.tbFolderPath.Text == "")
            {
                MessageBox.Show("Please select a folder with debug logs to parse to continue");
                return;
            }

            String debugFileName = "DebugLog_Aggregations.txt";
            String[] folderNameSplit = this.tbFolderPath.Text.Split(char.Parse("\\"));

            Boolean continueProcessing = true;
            for (Int32 i = 0; i < folderNameSplit.Length; i++)
            {
                if (folderNameSplit[i].Contains(" "))
                {
                    continueProcessing = false;
                    MessageBox.Show("Folder cannot contain spaces");
                    break;
                }
            }

            if (continueProcessing == false) { return; }

            String[] files = Directory.GetFiles(this.tbFolderPath.Text, "*.log");

            if (files == null || files.Length == 0)
            {
                MessageBox.Show("The directory selected does not contain debug logs to parse");
                return;
            }

            String fileSaveLocation = this.tbFolderPath.Text + "\\" + debugFileName;
            StreamWriter debugSW = new StreamWriter(fileSaveLocation);

            foreach (String file in files)
            {
                parseDebugLogFile(file, debugSW);
            }

            if (debugSW != null) debugSW.Close();

            if (Properties.Settings.Default.DefaultTextEditorPath == "")
            {
                Process.Start(@"notepad.exe", fileSaveLocation);
            }
            else
            {
                Process.Start(@Properties.Settings.Default.DefaultTextEditorPath, fileSaveLocation);
            }
        }

        private void parseDebugLogFile(String filePath, StreamWriter debugSW)
        {
            Boolean firstCodeUnitReached = true;
            Int32 tabCount = 0;
            //String milSecStart = "";
            //String milSecEnd = "";

            // Open file for reading
            StreamReader debugSR = new StreamReader(filePath);
            while (debugSR.EndOfStream == false)
            {
                String line = debugSR.ReadLine();
                String[] columnElements = line.Split(char.Parse("|"));

                if (columnElements.Length > 1)
                {
                    if (columnElements[1] == DebugEventTags.BULK_HEAP_ALLOCATE)
                    {

                    }
                    else if (columnElements[1] == DebugEventTags.CODE_UNIT_STARTED)
                    {
                        if (firstCodeUnitReached == true)
                        {
                            firstCodeUnitReached = false;

                            writeHierarchy(debugSW, tabCount, columnElements[0].ToString());

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
                            writeHierarchy(debugSW, tabCount, columnElements[0].ToString());

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

                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());

                        debugSW.Write("CODE_UNIT_FINISHED");
                        debugSW.Write(Environment.NewLine);
                        debugSW.Write(Environment.NewLine);
                    }
                    else if (columnElements[1] == DebugEventTags.CONSTRUCTOR_ENTRY)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        writeDebugLineDetail(debugSW, 2, "CONSTRUCTOR_ENTRY", columnElements);
                        tabCount++;
                    }
                    else if (columnElements[1] == DebugEventTags.CONSTRUCTOR_EXIT)
                    {
                        if (tabCount > 0) tabCount--;
                    }
                    else if (columnElements[1] == DebugEventTags.DML_BEGIN)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        writeDebugLineDetail(debugSW, 1, "DML_BEGIN", columnElements);
                        tabCount++;
                    }
                    else if (columnElements[1] == DebugEventTags.DML_END)
                    {
                        if (tabCount > 0) tabCount--;
                    }
                    else if (columnElements[1] == DebugEventTags.ENTERING_MANAGED_PKG)
                    {
                        //writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        //debugSW.Write("ENTERING_MANAGED_PKG: " + columnElements[2] + Environment.NewLine);
                    }
                    else if (columnElements[1] == DebugEventTags.EXCEPTION_THROWN)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        writeDebugLineDetail(debugSW, 2, "EXCEPTION_THROWN", columnElements);
                    }
                    else if (columnElements[1] == DebugEventTags.EXECUTION_STARTED)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());

                        debugSW.Write("EXECUTION_STARTED" + Environment.NewLine);
                    }
                    else if (columnElements[1] == DebugEventTags.EXECUTION_FINISHED)
                    {
                        //if (tabCount > 0) tabCount--;
                    }
                    else if (columnElements[1] == DebugEventTags.FATAL_ERROR)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());

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
                    //else if (columnElements[1] == DebugEventTags.FLOW_CREATE_INTERVIEW_BEGIN)
                    //{
                    //    if (this.cbIncludeHierarchy.Checked == true)
                    //    {
                    //        debugSW.Write(tabCount.ToString() + " | " + columnElements[0].ToString() + " | ");
                    //    }

                    //    for (Int32 tc = 0; tc < tabCount; tc++)
                    //    {
                    //        debugSW.Write("\t");
                    //    }

                    //    debugSW.Write("FLOW_CREATE_INTERVIEW_BEGIN" + Environment.NewLine);

                    //    tabCount++;
                    //}
                    //else if (columnElements[1] == DebugEventTags.FLOW_CREATE_INTERVIEW_END)
                    //{
                    //    debugSW.Write("FLOW_CREATE_INTERVIEW_END" + Environment.NewLine);

                    //    if (tabCount > 0) tabCount--;
                    //}
                    else if (columnElements[1] == DebugEventTags.FLOW_VALUE_ASSIGNMENT)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        writeDebugLineDetail(debugSW, 2, "FLOW_VALUE_ASSIGNMENT", columnElements);
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_ELEMENT_BEGIN)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        writeDebugLineDetail(debugSW, 2, "FLOW_ELEMENT_BEGIN", columnElements);
                        tabCount++;
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_ELEMENT_END)
                    {
                        if (tabCount > 0) tabCount--;
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        debugSW.Write("FLOW_ELEMENT_END");
                        debugSW.Write(Environment.NewLine);
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_ELEMENT_DEFERRED)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        writeDebugLineDetail(debugSW, 2, "FLOW_ELEMENT_DEFERRED", columnElements);
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_ACTIONCALL_DETAIL)
                    {
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_ASSIGNMENT_DETAIL)
                    {
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_BULK_ELEMENT_BEGIN)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        writeDebugLineDetail(debugSW, 2, "FLOW_BULK_ELEMENT_BEGIN", columnElements);
                        tabCount++;
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_BULK_ELEMENT_DETAIL)
                    {
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_BULK_ELEMENT_END)
                    {
                        if (tabCount > 0) tabCount--;
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        debugSW.Write("FLOW_BULK_ELEMENT_END");
                        debugSW.Write(Environment.NewLine);
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_CREATE_INTERVIEW_ERROR)
                    {
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_ELEMENT_ERROR)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());

                        debugSW.Write("FLOW_ELEMENT_ERROR: " + columnElements[2] + Environment.NewLine);

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
                    else if (columnElements[1] == DebugEventTags.FLOW_ELEMENT_FAULT)
                    {
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_LOOP_DETAIL)
                    {
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_RULE_DETAIL)
                    {
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_START_INTERVIEWS_BEGIN)
                    {
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_START_INTERVIEWS_END)
                    {
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_START_INTERVIEWS_ERROR)
                    {
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_START_INTERVIEW_BEGIN)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        writeDebugLineDetail(debugSW, 2, "FLOW_START_INTERVIEW_BEGIN", columnElements);
                        tabCount++;
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_START_INTERVIEW_END)
                    {
                        if (tabCount > 0) tabCount--;
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        debugSW.Write("FLOW_START_INTERVIEW_END");
                        debugSW.Write(Environment.NewLine);
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_SUBFLOW_DETAIL)
                    {
                    }
                    else if (columnElements[1] == DebugEventTags.HEAP_ALLOCATE)
                    {
                    }
                    else if (columnElements[1] == DebugEventTags.METHOD_ENTRY)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        writeDebugLineDetail(debugSW, 1, "METHOD_ENTRY", columnElements);
                        tabCount++;
                    }
                    else if (columnElements[1] == DebugEventTags.METHOD_EXIT)
                    {
                        if (tabCount > 0) tabCount--;

                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());

                        debugSW.Write("METHOD_EXIT");
                        debugSW.Write(Environment.NewLine);
                    }
                    else if (columnElements[1] == DebugEventTags.SAVEPOINT_ROLLBACK)
                    {
                    }
                    else if (columnElements[1] == DebugEventTags.SAVEPOINT_SET)
                    {
                    }
                    else if (columnElements[1] == DebugEventTags.SOQL_EXECUTE_BEGIN)
                    {
                        debugSW.Write(Environment.NewLine);
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        writeDebugLineDetail(debugSW, 1, "SOQL_EXECUTE_BEGIN", columnElements);

                        // Keep this additional NewLine. Makes reading and extracting the SOQL Statements a lot easier
                        debugSW.Write(Environment.NewLine);

                        tabCount++;
                    }
                    else if (columnElements[1] == DebugEventTags.SOQL_EXECUTE_END)
                    {
                        if (tabCount > 0) tabCount--;
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        writeDebugLineDetail(debugSW, 1, "SOQL_EXECUTE_END", columnElements);

                        // Keep this additional NewLine. Makes reading and extracting the SOQL Statements a lot easier
                        debugSW.Write(Environment.NewLine);
                    }
                    else if (columnElements[1] == DebugEventTags.SOSL_EXECUTE_BEGIN)
                    {
                    }
                    else if (columnElements[1] == DebugEventTags.SOSL_EXECUTE_END)
                    {
                    }
                    else if (columnElements[1] == DebugEventTags.STATEMENT_EXECUTE)
                    {
                    }
                    else if (columnElements[1] == DebugEventTags.USER_DEBUG)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());

                        debugSW.Write("USER_DEBUG: ");

                        for (Int32 ce = 0; ce < columnElements.Length; ce++)
                        {
                            if (ce == 2) debugSW.Write(columnElements[ce] + " ");
                            if (ce > 3) debugSW.Write(columnElements[ce] + " ");
                        }

                        debugSW.Write(Environment.NewLine);
                    }
                    else if (columnElements[1] == DebugEventTags.USER_INFO)
                    {

                    }
                    else if (columnElements[1] == DebugEventTags.VALIDATION_RULE)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        writeDebugLineDetail(debugSW, 1, "VALIDATION_RULE", columnElements);
                        //tabCount++;
                    }
                    else if (columnElements[1] == DebugEventTags.VALIDATION_ERROR)
                    {
                        writeHierarchy(debugSW, tabCount + 1, columnElements[0].ToString());
                        writeDebugLineDetail(debugSW, 1, "VALIDATION_ERROR", columnElements);
                    }
                    else if (columnElements[1] == DebugEventTags.VALIDATION_FAIL)
                    {
                        //if (tabCount > 0) tabCount--;
                        writeHierarchy(debugSW, tabCount + 1, columnElements[0].ToString());
                        writeDebugLineDetail(debugSW, 1, "VALIDATION_FAIL", columnElements);
                    }
                    else if (columnElements[1] == DebugEventTags.VALIDATION_PASS)
                    {
                        writeHierarchy(debugSW, tabCount + 1, columnElements[0].ToString());
                        writeDebugLineDetail(debugSW, 1, "VALIDATION_PASS", columnElements);
                    }
                    else if (columnElements[1] == DebugEventTags.VARIABLE_SCOPE_BEGIN)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        writeDebugLineDetail(debugSW, 1, "VARIABLE_SCOPE_BEGIN", columnElements);
                    }
                    else if (columnElements[1] == DebugEventTags.VARIABLE_ASSIGNMENT)
                    {
                        writeHierarchy(debugSW, tabCount + 1, columnElements[0].ToString());
                        writeDebugLineDetail(debugSW, 1, "VARIABLE_ASSIGNMENT", columnElements);
                    }
                    else if (columnElements[1] == DebugEventTags.WF_CRITERIA_BEGIN)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        writeDebugLineDetail(debugSW, 1, "WF_CRITERIA_BEGIN", columnElements);
                        tabCount++;
                    }
                    else if (columnElements[1] == DebugEventTags.WF_RULE_NOT_EVALUATED)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        debugSW.Write("WF_RULE_NOT_EVALUATED");
                        debugSW.Write(Environment.NewLine);
                        if (tabCount > 0) tabCount--;

                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        debugSW.Write("WF_CRITERIA_END");
                        debugSW.Write(Environment.NewLine);
                    }
                    else if (columnElements[1] == DebugEventTags.WF_CRITERIA_END)
                    {
                        if (tabCount > 0) tabCount--;

                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());

                        debugSW.Write("WF_CRITERIA_END");
                        debugSW.Write(Environment.NewLine);
                    }
                    else if (columnElements[1] == DebugEventTags.WF_EMAIL_ALERT)
                    {
                    }
                    else if (columnElements[1] == DebugEventTags.WF_EMAIL_SENT)
                    {
                    }
                    else if (columnElements[1] == DebugEventTags.WF_FIELD_UPDATE)
                    {
                    }
                    else if (columnElements[1] == DebugEventTags.WF_FLOW_ACTION_BEGIN)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        debugSW.Write("WF_FLOW_ACTION_BEGIN" + Environment.NewLine);
                        tabCount++;
                    }
                    else if (columnElements[1] == DebugEventTags.WF_FLOW_ACTION_END)
                    {
                        if (tabCount > 0) tabCount--;

                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());

                        debugSW.Write("WF_FLOW_ACTION_END");
                        debugSW.Write(Environment.NewLine);
                    }
                    else if (columnElements[1] == DebugEventTags.WF_FORMULA)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        writeDebugLineDetail(debugSW, 1, "WF_FORMULA", columnElements);
                    }
                    else if (columnElements[1] == DebugEventTags.WF_RULE_FILTER)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        writeDebugLineDetail(debugSW, 1, "WF_RULE_FILTER", columnElements);
                    }
                }
            }

            debugSR.Close();
        }

        private void btnParseCodeUnits_Click(object sender, EventArgs e)
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

            folderSaveLocation = folderSaveLocation + "DebugLog_CodeUnits.txt";

            //Boolean firstCodeUnitReached = true;
            //String milSecStart = "";
            //String milSecEnd = "";

            Boolean firstCodeUnitReached = true;
            Int32 tabCount = 0;

            StreamWriter debugSW = new StreamWriter(folderSaveLocation);

            // Open file for reading
            StreamReader debugSR = new StreamReader(this.tbDebugFile.Text);

            while (debugSR.EndOfStream == false)
            {
                String line = debugSR.ReadLine();
                String[] columnElements = line.Split(char.Parse("|"));

                if (columnElements.Length > 1)
                {
                    if (columnElements[1] == DebugEventTags.CODE_UNIT_STARTED)
                    {
                        if (firstCodeUnitReached == true)
                        {
                            firstCodeUnitReached = false;

                            writeHierarchy(debugSW, tabCount, columnElements[0].ToString());

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
                            writeHierarchy(debugSW, tabCount, columnElements[0].ToString());

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

                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());

                        debugSW.Write("CODE_UNIT_FINISHED");
                        debugSW.Write(Environment.NewLine);
                        debugSW.Write(Environment.NewLine);
                    }
                    else if (columnElements[1] == DebugEventTags.DML_BEGIN)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        writeDebugLineDetail(debugSW, 1, "DML_BEGIN", columnElements);
                        tabCount++;
                    }
                    else if (columnElements[1] == DebugEventTags.DML_END)
                    {
                        if (tabCount > 0) tabCount--;
                    }
                    //else if (columnElements[1] == DebugEventTags.ENTERING_MANAGED_PKG)
                    //{
                    //    writeHierarchy(debugSW, 1, columnElements[0].ToString());
                    //    debugSW.Write("ENTERING_MANAGED_PKG: " + columnElements[2] + Environment.NewLine);
                    //}
                    else if (columnElements[1] == DebugEventTags.EXCEPTION_THROWN)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        writeDebugLineDetail(debugSW, 2, "EXCEPTION_THROWN", columnElements);
                    }
                    else if (columnElements[1] == DebugEventTags.FATAL_ERROR)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());

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
                    else if (columnElements[1] == DebugEventTags.FLOW_BULK_ELEMENT_BEGIN)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        writeDebugLineDetail(debugSW, 2, "FLOW_BULK_ELEMENT_BEGIN", columnElements);
                        tabCount++;
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_BULK_ELEMENT_END)
                    {
                        if (tabCount > 0) tabCount--;
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        debugSW.Write("FLOW_BULK_ELEMENT_END");
                        debugSW.Write(Environment.NewLine);
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_ELEMENT_BEGIN)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        writeDebugLineDetail(debugSW, 2, "FLOW_ELEMENT_BEGIN", columnElements);
                        tabCount++;
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_ELEMENT_END)
                    {
                        if (tabCount > 0) tabCount--;
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        debugSW.Write("FLOW_ELEMENT_END");
                        debugSW.Write(Environment.NewLine);
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_CREATE_INTERVIEW_ERROR)
                    {
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_ELEMENT_ERROR)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());

                        debugSW.Write("FLOW_ELEMENT_ERROR: " + columnElements[2] + Environment.NewLine);

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
                    else if (columnElements[1] == DebugEventTags.FLOW_ELEMENT_FAULT)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        debugSW.Write("FLOW_ELEMENT_FAULT");
                        debugSW.Write(Environment.NewLine);
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_START_INTERVIEW_BEGIN)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        writeDebugLineDetail(debugSW, 2, "FLOW_START_INTERVIEW_BEGIN", columnElements);
                        tabCount++;
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_START_INTERVIEW_END)
                    {
                        if (tabCount > 0) tabCount--;
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        debugSW.Write("FLOW_START_INTERVIEW_END");
                        debugSW.Write(Environment.NewLine);
                    }
                    else if (columnElements[1] == DebugEventTags.FLOW_START_INTERVIEWS_ERROR)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        debugSW.Write("FLOW_START_INTERVIEWS_ERROR");
                        debugSW.Write(Environment.NewLine);
                    }
                    else if (columnElements[1] == DebugEventTags.METHOD_ENTRY)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        writeDebugLineDetail(debugSW, 1, "METHOD_ENTRY", columnElements);
                        tabCount++;
                    }
                    else if (columnElements[1] == DebugEventTags.METHOD_EXIT)
                    {
                        if (tabCount > 0) tabCount--;

                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());

                        debugSW.Write("METHOD_EXIT");
                        debugSW.Write(Environment.NewLine);
                    }
                    else if (columnElements[1] == DebugEventTags.SOQL_EXECUTE_BEGIN)
                    {
                        debugSW.Write(Environment.NewLine);
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        writeDebugLineDetail(debugSW, 1, "SOQL_EXECUTE_BEGIN", columnElements);

                        // Keep this additional NewLine. Makes reading and extracting the SOQL Statements a lot easier
                        debugSW.Write(Environment.NewLine);

                        tabCount++;
                    }
                    else if (columnElements[1] == DebugEventTags.SOQL_EXECUTE_END)
                    {
                        if (tabCount > 0) tabCount--;
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());
                        writeDebugLineDetail(debugSW, 1, "SOQL_EXECUTE_END", columnElements);

                        // Keep this additional NewLine. Makes reading and extracting the SOQL Statements a lot easier
                        debugSW.Write(Environment.NewLine);
                    }
                    else if (columnElements[1] == DebugEventTags.USER_DEBUG)
                    {
                        writeHierarchy(debugSW, tabCount, columnElements[0].ToString());

                        debugSW.Write("USER_DEBUG: ");

                        for (Int32 ce = 0; ce < columnElements.Length; ce++)
                        {
                            if (ce == 2) debugSW.Write(columnElements[ce] + " ");
                            if (ce > 3) debugSW.Write(columnElements[ce] + " ");
                        }

                        debugSW.Write(Environment.NewLine);
                    }
                }
            }

            debugSR.Close();
            if (debugSW != null) debugSW.Close();

            //MessageBox.Show("Debug Parsing Complete");

            if (Properties.Settings.Default.DefaultTextEditorPath == "")
            {
                Process.Start(@"notepad.exe", folderSaveLocation);
            }
            else
            {
                Process.Start(@Properties.Settings.Default.DefaultTextEditorPath, folderSaveLocation);
            }
        }

        private void writeHierarchy(StreamWriter debugSW, Int32 tabCount, String columnElements)
        {
            debugSW.Write(tabCount.ToString() + " | ");

            if (this.cbIncludeHierarchy.Checked == true)
            {
                debugSW.Write(columnElements + " | ");
            }

            for (Int32 tc = 0; tc < tabCount; tc++)
            {
                debugSW.Write("\t");
            }
        }

        private void writeDebugLineDetail(StreamWriter debugSW, Int32 colElemIdxCheck, String debugEvent, String[] columnElements)
        {
            debugSW.Write(debugEvent + ": ");

            for (Int32 ce = 0; ce < columnElements.Length; ce++)
            {
                if (ce > colElemIdxCheck) debugSW.Write(columnElements[ce] + " ");
            }

            debugSW.Write(Environment.NewLine);
        }

        public class DebugLogWrapper
        {
            Double timeBlockInMilliseconds;
            List<String> codeBlock;
        }

        /*
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

            //allocateEventTagsToDictinary();

            //String milSecStart = "";
            //String milSecEnd = "";

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
        */

        private void btnDeleteDebugLogs_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.cmbUserName.Text))
            {
                MessageBox.Show("Please select a username and enter the password first before continuing");
                return;
            }

            try
            {
                sc.salesforceLogin(UtilityClass.REQUESTINGORG.FROMORG, this.cmbUserName.Text);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                return;
            }

            if (sc.loginSuccess == false)
            {
                MessageBox.Show("Please check username, password and/or security token");
                return;
            }

            String selectStatement = "";
            if (cbAllDebugLogs.Checked)
            {
                // TODO: message box confirmation that they are going to delete everyone's debug log files including their own

                selectStatement = "SELECT Id FROM ApexLog";
            }
            else
            {
                String userId = "";
                userId = sc.fromOrgLR.userId;

                selectStatement = "SELECT Id FROM ApexLog WHERE LogUserId = \'" + userId + "\'";
            }

            SalesforceMetadata.PartnerWSDL.QueryResult qr = new SalesforceMetadata.PartnerWSDL.QueryResult();

            try
            {
                qr = sc.fromOrgSS.query(selectStatement);

                if (qr.size > 2000)
                {
                    Boolean done = false;

                    while (!done)
                    {
                        SalesforceMetadata.PartnerWSDL.sObject[] sobjRecords = qr.records;

                        Dictionary<Int32, List<String>> recordIdsToDelete = new Dictionary<Int32, List<String>>();

                        Int32 i = 0;
                        List<String> rtds = new List<String>();

                        if (sobjRecords == null)
                        {

                        }
                        else
                        {
                            foreach (SalesforceMetadata.PartnerWSDL.sObject s in sobjRecords)
                            {
                                if (rtds.Count < 200)
                                {
                                    rtds.Add(s.Id);
                                }
                                else
                                {
                                    recordIdsToDelete.Add(i, rtds);
                                    rtds = new List<String>();
                                    rtds.Add(s.Id);

                                    i++;
                                }
                            }

                            if (rtds.Count > 0)
                            {
                                recordIdsToDelete.Add(i, rtds);
                                rtds = new List<String>();
                            }

                            foreach (Int32 rtd in recordIdsToDelete.Keys)
                            {
                                if (recordIdsToDelete[rtd].Count > 0)
                                {
                                    PartnerWSDL.DeleteResult[] dr = sc.fromOrgSS.delete(recordIdsToDelete[rtd].ToArray());
                                }
                            }

                            if (!qr.done)
                            {
                                qr = sc.fromOrgSS.queryMore(qr.queryLocator);
                            }
                            else
                            {
                                done = true;
                            }
                        }
                    }
                }
                else if (qr.records != null)
                {
                    SalesforceMetadata.PartnerWSDL.sObject[] sobjRecords = qr.records;
                    Dictionary<Int32, List<String>> recordIdsToDelete = new Dictionary<Int32, List<String>>();

                    Int32 i = 0;
                    List<String> rtds = new List<String>();
                    foreach (SalesforceMetadata.PartnerWSDL.sObject s in sobjRecords)
                    {
                        if (rtds.Count < 200)
                        {
                            rtds.Add(s.Id);
                        }
                        else
                        {
                            recordIdsToDelete.Add(i, rtds);
                            rtds = new List<String>();
                            rtds.Add(s.Id);

                            i++;
                        }
                    }

                    if (rtds.Count > 0)
                    {
                        recordIdsToDelete.Add(i, rtds);
                        rtds = new List<String>();
                    }

                    foreach (Int32 rtd in recordIdsToDelete.Keys)
                    {
                        if (recordIdsToDelete[rtd].Count > 0)
                        {
                            PartnerWSDL.DeleteResult[] dr = sc.fromOrgSS.delete(recordIdsToDelete[rtd].ToArray());
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

            MessageBox.Show("Debug log delete process completed");

        }

        private void populateDefaultDebugLogPath()
        {
            this.tbFolderPath.Text = Properties.Settings.Default.DebugLogPath;
        }

        private void btnGetDebugLogs_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.cmbUserName.Text))
            {
                MessageBox.Show("Please select a username and enter the password first before continuing");
                return;
            }

            try
            {
                sc.salesforceLogin(UtilityClass.REQUESTINGORG.FROMORG, this.cmbUserName.Text);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                return;
            }

            if (sc.loginSuccess == false)
            {
                MessageBox.Show("Please check username, password and/or security token");
                return;
            }

            String apexLogQuery = "SELECT Id, StartTime, LastModifiedDate, LogLength, DurationMilliseconds, LogUserId, LogUser.Name, Location, Request, Operation, Application, Status, RequestIdentifier FROM ApexLog";

            SalesforceMetadata.PartnerWSDL.QueryResult qr = new SalesforceMetadata.PartnerWSDL.QueryResult();

            Boolean continueExecution = true;
            try
            {
                qr = sc.fromOrgSS.query(apexLogQuery);
            }
            catch (Exception exc)
            {
                continueExecution = false;
                MessageBox.Show(exc.Message);
            }

            if (continueExecution == false) { return; }

            Boolean done = false;
            Dictionary<String, String> debugLogIds = new Dictionary<String, String>();

            if (this.debugLogsDataTable.Columns.Count > 0) 
            {
                this.debugLogsDataTable.Columns.Clear();
            }

            if (this.debugLogsDataTable.Rows.Count > 0)
            {
                this.debugLogsDataTable.Rows.Clear();
            }

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("StartTime");
            dt.Columns.Add("LastModifiedDate");
            dt.Columns.Add("LogLength");
            dt.Columns.Add("DurationMilliseconds");
            dt.Columns.Add("LogUserId");
            dt.Columns.Add("LogUser");
            dt.Columns.Add("Location");
            dt.Columns.Add("Request");
            dt.Columns.Add("Operation");
            dt.Columns.Add("Application");
            dt.Columns.Add("Status");
            //dt.Columns.Add("RequestIdentifier");

            while (!done)
            {
                SalesforceMetadata.PartnerWSDL.sObject[] sobjRecords = qr.records;

                if (qr.records == null)
                {
                    done = true;
                    break;
                }

                foreach (SalesforceMetadata.PartnerWSDL.sObject s in sobjRecords)
                {
                    debugLogIds.Add(s.Id, s.Id);

                    DataRow dr = dt.NewRow();
                    dr[0] = s.Any[0].ChildNodes[0].InnerText;
                    dr[1] = s.Any[1].ChildNodes[0].InnerText;
                    dr[2] = s.Any[2].ChildNodes[0].InnerText;
                    dr[3] = s.Any[3].ChildNodes[0].InnerText;
                    dr[4] = s.Any[4].ChildNodes[0].InnerText;
                    dr[5] = s.Any[5].ChildNodes[0].InnerText;
                    dr[6] = s.Any[6].ChildNodes[2].InnerText;
                    dr[7] = s.Any[7].ChildNodes[0].InnerText;
                    dr[8] = s.Any[8].ChildNodes[0].InnerText;
                    dr[9] = s.Any[9].ChildNodes[0].InnerText;
                    dr[10] = s.Any[10].ChildNodes[0].InnerText;
                    dr[11] = s.Any[11].ChildNodes[0].InnerText;
                    //dr[12] = s.Any[12].ChildNodes[0].InnerText;

                    dt.Rows.Add(dr);

                    //Debug.WriteLine("");
                }

                this.debugLogsDataTable.DataSource = dt;

                if (!qr.done)
                {
                    qr = sc.fromOrgSS.queryMore(qr.queryLocator);
                }
                else
                {
                    done = true;
                }
            }

            /**** REST URL /sobjects/ApexLog/{id}/Body/  ****/
            //for (int i = 0; i < debugLogIds.Count; i++)
            //{
            //
            //}


        }

/*
// Get the EventLogFile
String eventLogQuery = "SELECT Id, ApiVersion, CreatedDate, LogDate, EventType, LogFileContentType, LogFileFieldNames, LogFileFieldTypes, LogFileLength, LogFile FROM EventLogFile";

qr = new SalesforceMetadata.PartnerWSDL.QueryResult();

continueExecution = true;
try
{
    qr = sc.fromOrgSS.query(eventLogQuery);
}
catch (Exception exc)
{
    continueExecution = false;
    MessageBox.Show(exc.Message);
}

if (continueExecution == false) { return; }

done = false;
while (!done)
{
    SalesforceMetadata.PartnerWSDL.sObject[] sobjRecords = qr.records;

    if (qr.records == null)
    {
        done = true;
        break;
    }

    foreach (SalesforceMetadata.PartnerWSDL.sObject s in sobjRecords)
    {
        Debug.WriteLine("");
    }

    if (!qr.done)
    {
        qr = sc.fromOrgSS.queryMore(qr.queryLocator);
    }
    else
    {
        done = true;
    }
}

// Get the EventLogFile
//for (Int32 i = 0; i < debugLogIds.Count; i++)
//{

//}
*/

        private void cbAllDebugLogs_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbAllDebugLogs.Checked == true)
            {
                this.btnDeleteDebugLogs.Text = "Delete All Debug Logs";
            }
            else
            {
                this.btnDeleteDebugLogs.Text = "Delete Your Debug Logs";
            }
        }

        private void ParseDebugLogs_Load(object sender, EventArgs e)
        {

        }


    }
}
