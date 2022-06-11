using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using Microsoft.VisualBasic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

using System.Data.OleDb;

using SalesforceMetadata.PartnerWSDL;
using SalesforceMetadata.ToolingWSDL;

namespace SalesforceMetadata
{
    // When the SandboxSeedingStep2 form opens, I want to have the XML file load, parsed, and auto-select the objects in the XML file.
    // If there is a change to the list, then I will re-save the XML file.
    // TODO: Establish a default folder / filename and location. The user can select a different folder, but for the initial purposes, 
    // the XML file with the objects to bring over from production should be available in a default file folder.

    // TODO:
    // lbSalesforceSobjects: Find a way to change the height for each entry
    // Using a ListView box, I can add an additional column for the number of records to retrieve
    // "All", 150, 200, 300
    // These will be stored in the XML file.

    public partial class ConfigurationWorkbook : System.Windows.Forms.Form
    {
        private Dictionary<String, String> usernameToSecurityToken;


        //private Dictionary<String, List<String>> memberNameToFiles;

        // Columns 1,2,3,4 will contains the ObjectName, ObjectType, PrimaryKey and ForeignKey
        // when the columns are written
        private Int32 colNumber1 = 6;
        private Int32 colNumber2 = 6;
        private Int32 colNumber3 = 6;
        private Int32 colNumber4 = 6;
        private Int32 colNumber5 = 6;
        private Int32 colNumber6 = 6;
        private Int32 colNumber7 = 6;
        private Int32 colNumber8 = 6;


        // Column Name - Column Position
        private Dictionary<String, Int32> colNameToColPos1;
        private Dictionary<String, Int32> colNameToColPos2;
        private Dictionary<String, Int32> colNameToColPos3;
        private Dictionary<String, Int32> colNameToColPos4;
        private Dictionary<String, Int32> colNameToColPos5;
        private Dictionary<String, Int32> colNameToColPos6;
        private Dictionary<String, Int32> colNameToColPos7;
        private Dictionary<String, Int32> colNameToColPos8;

        // Row Primary Keys
        private Int32 primaryKey1 = 1;
        private Int32 primaryKey2 = 1;
        private Int32 primaryKey3 = 1;
        private Int32 primaryKey4 = 1;
        private Int32 primaryKey5 = 1;
        private Int32 primaryKey6 = 1;
        private Int32 primaryKey7 = 1;
        //private Int32 primaryKey8 = 1;

        private Dictionary<String, Int32> node1PrimaryKey;
        private Dictionary<String, Int32> node2PrimaryKey;
        private Dictionary<String, Int32> node3PrimaryKey;
        private Dictionary<String, Int32> node4PrimaryKey;
        private Dictionary<String, Int32> node5PrimaryKey;
        private Dictionary<String, Int32> node6PrimaryKey;
        private Dictionary<String, Int32> node7PrimaryKey;
        //private Dictionary<String, Int32> node8PrimaryKey;

        private List<ExcelBorderFormat> excelBorderFormatList;
        private List<ExcelRangeFormat> excelRangeFormatList;

        public ConfigurationWorkbook()
        {
            InitializeComponent();
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
                this.lblSalesforce.Text = "Salesforce";
                this.Text = "Salesforce Production";
            }
            else
            {
                this.lblSalesforce.Text = "Salesforce Sandbox";
                String[] userNamesplit = this.cmbUserName.Text.Split('.');
                this.Text = "Salesforce " + userNamesplit[userNamesplit.Length - 1].ToUpper();
            }

            this.tbSecurityToken.Text = "";
            if (this.usernameToSecurityToken.ContainsKey(this.cmbUserName.Text))
            {
                this.tbSecurityToken.Text = this.usernameToSecurityToken[cmbUserName.Text];
            }

            this.tbPassword.Text = "";
        }

        private void tbSelectedFolder_DoubleClick(object sender, EventArgs e)
        {
            this.tbSelectedFolder.Text = UtilityClass.folderBrowserSelectPath("Select Project Folder to Parse", false, FolderEnum.ReadFrom);
        }

        private void tbSaveResultsTo_DoubleClick(object sender, EventArgs e)
        {
            this.tbSaveResultsTo.Text = UtilityClass.folderBrowserSelectPath("Save Results to...", true, FolderEnum.SaveTo);
        }

        private void btnGenerateConfigReportAsCSV_Click(object sender, EventArgs e)
        {
            if (tbSelectedFolder.Text == null || tbSelectedFolder.Text == "")
            {
                MessageBox.Show("Please select a folder with the metadata to parse");
                return;
            }

            if (tbSaveResultsTo.Text == null || tbSaveResultsTo.Text == "")
            {
                MessageBox.Show("Please select a file to save the results to");
                return;
            }


            // Bypass Aura and LWC folders
            String[] folders = Directory.GetDirectories(tbSelectedFolder.Text);

            foreach (String fldr in folders)
            {
                buildCSVMetadataDictionaries(fldr);
            }

            MessageBox.Show("Configuration Report as CSV Complete");
        }

        private void btnGenerateConfigReport_Excel_Click(object sender, EventArgs e)
        {
            if (tbSelectedFolder.Text == null || tbSelectedFolder.Text == "")
            {
                MessageBox.Show("Please select a folder with the metadata to parse");
                return;
            }

            buildExcelMetadataDictionaries();

            MessageBox.Show("Extraction Complete");
        }

        private String fullObjectNameExtract(String fileNameWithPath)
        {
            String objectName = "";
            String[] fileParse = fileNameWithPath.Split('\\');
            String[] fileName = fileParse[fileParse.Length - 1].Split('.');

            if (fileName.Length > 2)
            {
                for (Int32 i = 0; i < fileName.Length - 1; i++)
                {
                    objectName = objectName + fileName[i] + "_";
                }
            }
            else
            {
                objectName = fileName[0];
            }

            if (objectName.EndsWith("_"))
            {
                objectName = objectName.Substring(0, objectName.Length - 1);
            }

            return objectName;
        }

        private String directoryNameExtract(String folderNameWithPath)
        {
            String directoryName = "";
            String[] folderParse = folderNameWithPath.Split('\\');
            directoryName = folderParse[folderParse.Length - 1];

            return directoryName;
        }

        private String fileExtension(String fileNameWithPath)
        {
            String fileExt = "";

            String[] fileParse = fileNameWithPath.Split('.');
            fileExt = fileParse[fileParse.Length - 1];

            return fileExt;
        }

        public String stringColumnPosition(Int32 colNumber, String value)
        {
            String stringValue = "";

            for (Int32 i = 0; i < colNumber - 1; i++)
            {
                stringValue = stringValue + ",";
            }

            stringValue = stringValue + "\"" + value + "\"";

            return stringValue;
        }

        public void buildExcelMetadataDictionaries()
        {
            this.cwProgressBar.Visible = true;
            this.cwProgressBar.Minimum = 1;
            this.cwProgressBar.Value = 1;
            this.cwProgressBar.Step = 1;

            this.chkListBoxTasks.Items.Clear();
            this.chkListBoxTasks.Items.Add("Extract Configuration Files to Excel", false);
            this.chkListBoxTasks.Items.Add("Apply Borders to Cells", false);
            this.chkListBoxTasks.Items.Add("Apply Cell Formatting", false);

            excelBorderFormatList = new List<ExcelBorderFormat>();
            excelRangeFormatList = new List<ExcelRangeFormat>();

            String[] directorySubdirectories = Directory.GetDirectories(this.tbSelectedFolder.Text);

            List<String> subdirectories = new List<string>();

            if (directorySubdirectories.Length == 0)
            {
                subdirectories.Add(this.tbSelectedFolder.Text);
            }
            else
            {
                subdirectories = directorySubdirectories.ToList<String>();
            }

            String searchPattern = "*.*";

            // Set the progress bar maximumn
            Int32 pbInt32 = 0;
            foreach (String subDir in subdirectories)
            {
                String[] dirFiles = Directory.GetFiles(subDir + '\\', searchPattern);
                pbInt32 += dirFiles.Length;
            }

            this.lblProgressIndicator.Text = "Extract Configuration Files to Excel";
            this.cwProgressBar.Maximum = pbInt32;

            Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
            xlapp.Visible = true;

            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlapp.Workbooks.Add();

            Dictionary<String, Int32> fileNames = new Dictionary<String, Int32>();

            foreach (String subDir in subdirectories)
            {
                String directoryName = directoryNameExtract(subDir);

                String[] dirFiles = Directory.GetFiles(subDir + '\\', searchPattern);

                if (directoryName == "aura")
                {

                }
                else if (directoryName == "classes")
                {
                    Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                            (System.Reflection.Missing.Value,
                                                                                xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                                System.Reflection.Missing.Value,
                                                                                System.Reflection.Missing.Value);

                    xlWorksheet.Name = "Classes";

                    Int32 rowStart = 2;
                    Int32 rowEnd = 2;
                    Int32 colStart = 2;
                    Int32 colEnd = 2;
                    Int32 lastRowNumber = 2;

                    List<String> mdtFields = getObjectTopFieldNames(directoryName);
                    Dictionary<String, Int32> nodeNameToCol = new Dictionary<String, Int32>();

                    writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, "Apex Classes");

                    addFormatToExcelRange(xlWorksheet,
                                        rowStart,
                                        rowEnd,
                                        colStart,
                                        colEnd,
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

                    rowEnd += 2;
                    rowStart = rowEnd;
                    lastRowNumber = rowEnd;

                    nodeNameToCol.Add("Name", colEnd);
                    colEnd++;

                    foreach (String topFieldName in mdtFields)
                    {
                        nodeNameToCol.Add(topFieldName, colEnd);
                        colEnd++;
                    }

                    nodeNameToCol.Add("IsTestClass", colEnd);

                    foreach (String colName in nodeNameToCol.Keys)
                    {
                        writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol[colName], colName);
                    }

                    addFormatToExcelRange(xlWorksheet,
                                        rowStart,
                                        rowEnd,
                                        colStart,
                                        colEnd,
                                        10,
                                        255,
                                        255,
                                        255,
                                        0,
                                        51,
                                        102,
                                        true,
                                        false,
                                        "");

                    rowEnd++;
                    lastRowNumber = rowEnd;

                    foreach (String fl in dirFiles)
                    {
                        String fullObjectName = fullObjectNameExtract(@fl);

                        // Parse the XML Name and pull out the class name
                        String objectNamePath = fl.Split('-')[0];
                        String[] filePathSplit = fl.Split('\\');
                        String[] fileNameSplit = filePathSplit[filePathSplit.Length - 1].Split('-');
                        String objectName = fileNameSplit[0].Split('.')[0];

                        if (fl.EndsWith(".xml"))
                        {
                            writeDataToExcelSheet(xlWorksheet, rowEnd, 2, objectName);
                            addFormatToExcelRange(xlWorksheet,
                                                rowEnd,
                                                rowEnd,
                                                2,
                                                2,
                                                10,
                                                0,
                                                0,
                                                0,
                                                192,
                                                192,
                                                192,
                                                false,
                                                true,
                                                "");

                            XmlDocument xd = new XmlDocument();
                            xd.Load(fl);

                            foreach (XmlNode nd1 in xd.ChildNodes)
                            {
                                if (nd1.LocalName == "xml")
                                {
                                    continue;
                                }
                                else
                                {
                                    foreach (XmlNode nd2 in nd1.ChildNodes)
                                    {
                                        writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol[nd2.Name], nd2.InnerText);
                                    }
                                }
                            }

                            // Open the Apex Class to determine if it is a test class and write the value to the last row / last column
                            StreamReader sr = new StreamReader(objectNamePath);
                            String fileContents = sr.ReadToEnd();

                            if (fileContents.ToLower().Contains("@istest"))
                            {
                                writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, "TRUE");
                                addFormatToExcelRange(xlWorksheet,
                                                    rowEnd,
                                                    rowEnd,
                                                    colEnd,
                                                    colEnd,
                                                    10,
                                                    0,
                                                    0,
                                                    0,
                                                    255,
                                                    255,
                                                    255,
                                                    false,
                                                    false,
                                                    "TRUE");

                            }
                            else
                            {
                                writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, "FALSE");
                            }

                            sr.Close();

                            rowEnd++;
                        }

                        this.cwProgressBar.PerformStep();
                    }
                }
                else if (directoryName == "corsWhitelistOrigins")
                {
                    Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                            (System.Reflection.Missing.Value,
                                                                                xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                                System.Reflection.Missing.Value,
                                                                                System.Reflection.Missing.Value);

                    xlWorksheet.Name = "corsWhitelistOrigins";

                    Int32 rowStart = 2;
                    Int32 rowEnd = 2;
                    Int32 colStart = 2;
                    Int32 colEnd = 2;
                    Int32 lastRowNumber = 2;

                    List<String> mdtFields = getObjectTopFieldNames(directoryName);
                    Dictionary<String, Int32> nodeNameToCol = new Dictionary<String, Int32>();

                    writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, "Cors Whitelist Origin");

                    addFormatToExcelRange(xlWorksheet,
                                        rowStart,
                                        rowEnd,
                                        colStart,
                                        colEnd,
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

                    rowEnd += 2;
                    rowStart = rowEnd;
                    lastRowNumber = rowEnd;

                    nodeNameToCol.Add("Name", colEnd);
                    colEnd++;

                    foreach (String topFieldName in mdtFields)
                    {
                        nodeNameToCol.Add(topFieldName, colEnd);
                        colEnd++;
                    }

                    foreach (String colName in nodeNameToCol.Keys)
                    {
                        writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol[colName], colName);
                    }

                    addFormatToExcelRange(xlWorksheet,
                                        rowStart,
                                        rowEnd,
                                        colStart,
                                        colEnd,
                                        10,
                                        255,
                                        255,
                                        255,
                                        0,
                                        51,
                                        102,
                                        true,
                                        false,
                                        "");

                    rowEnd++;
                    lastRowNumber = rowEnd;

                    foreach (String fl in dirFiles)
                    {
                        // Write the file name value in the appropriate column
                        writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol["Name"], fullObjectNameExtract(fl));
                        addFormatToExcelRange(xlWorksheet,
                                            rowEnd,
                                            rowEnd,
                                            nodeNameToCol["Name"],
                                            nodeNameToCol["Name"],
                                            10,
                                            0,
                                            0,
                                            0,
                                            255,
                                            255,
                                            255,
                                            false,
                                            false,
                                            fullObjectNameExtract(fl));

                        XmlDocument xd = new XmlDocument();
                        xd.Load(fl);

                        foreach (XmlNode nd1 in xd.ChildNodes)
                        {
                            foreach (XmlNode nd2 in nd1.ChildNodes)
                            {
                                if (nd2.Name != "xml")
                                {
                                    writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol[nd2.Name], nd2.InnerText);
                                    addFormatToExcelRange(xlWorksheet,
                                                        rowEnd,
                                                        rowEnd,
                                                        nodeNameToCol[nd2.Name],
                                                        nodeNameToCol[nd2.Name],
                                                        10,
                                                        0,
                                                        0,
                                                        0,
                                                        255,
                                                        255,
                                                        255,
                                                        false,
                                                        false,
                                                        nd2.InnerText);

                                    rowEnd++;
                                }
                            }
                        }

                        this.cwProgressBar.PerformStep();
                    }
                }
                else if (directoryName == "components")
                {
                    Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                            (System.Reflection.Missing.Value,
                                                                                xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                                System.Reflection.Missing.Value,
                                                                                System.Reflection.Missing.Value);

                    xlWorksheet.Name = "Components";

                    Int32 rowStart = 2;
                    Int32 rowEnd = 2;
                    Int32 colStart = 2;
                    Int32 colEnd = 2;
                    Int32 lastRowNumber = 2;

                    List<String> mdtFields = getObjectTopFieldNames(directoryName);
                    Dictionary<String, Int32> nodeNameToCol = new Dictionary<String, Int32>();

                    writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, "Apex Components");

                    addFormatToExcelRange(xlWorksheet,
                                        rowStart,
                                        rowEnd,
                                        colStart,
                                        colEnd,
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

                    rowEnd += 2;
                    rowStart = rowEnd;
                    lastRowNumber = rowEnd;

                    nodeNameToCol.Add("Name", colEnd);
                    colEnd++;
                    foreach (String topFieldName in mdtFields)
                    {
                        nodeNameToCol.Add(topFieldName, colEnd);
                        colEnd++;
                    }

                    foreach (String colName in nodeNameToCol.Keys)
                    {
                        writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol[colName], colName);
                    }

                    addFormatToExcelRange(xlWorksheet,
                                        rowStart,
                                        rowEnd,
                                        colStart,
                                        colEnd - 1,
                                        10,
                                        255,
                                        255,
                                        255,
                                        0,
                                        51,
                                        102,
                                        true,
                                        false,
                                        "");

                    rowEnd++;
                    lastRowNumber = rowEnd;

                    foreach (String fl in dirFiles)
                    {
                        String fullObjectName = fullObjectNameExtract(@fl);

                        // Parse the XML Name and pull out the class name
                        String objectNamePath = fl.Split('-')[0];
                        String[] filePathSplit = fl.Split('\\');
                        String[] fileNameSplit = filePathSplit[filePathSplit.Length - 1].Split('-');
                        String objectName = fileNameSplit[0].Split('.')[0];

                        if (fl.EndsWith(".xml"))
                        {
                            writeDataToExcelSheet(xlWorksheet, rowEnd, 2, objectName);
                            addFormatToExcelRange(xlWorksheet,
                                                rowEnd,
                                                rowEnd,
                                                2,
                                                2,
                                                10,
                                                0,
                                                0,
                                                0,
                                                192,
                                                192,
                                                192,
                                                false,
                                                true,
                                                "");

                            XmlDocument xd = new XmlDocument();
                            xd.Load(fl);

                            foreach (XmlNode nd1 in xd.ChildNodes)
                            {
                                if (nd1.LocalName == "xml")
                                {
                                    continue;
                                }
                                else
                                {
                                    foreach (XmlNode nd2 in nd1.ChildNodes)
                                    {
                                        writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol[nd2.Name], nd2.InnerText);
                                    }
                                }
                            }

                            rowEnd++;
                        }

                        this.cwProgressBar.PerformStep();
                    }
                }
                else if (directoryName == "cspTrustedSites")
                {
                    Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                            (System.Reflection.Missing.Value,
                                                                                xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                                System.Reflection.Missing.Value,
                                                                                System.Reflection.Missing.Value);

                    xlWorksheet.Name = "CSPTrustedSites";

                    Int32 rowStart = 2;
                    Int32 rowEnd = 2;
                    Int32 colStart = 2;
                    Int32 colEnd = 2;
                    Int32 lastRowNumber = 2;

                    List<String> mdtFields = getObjectTopFieldNames(directoryName);
                    Dictionary<String, Int32> nodeNameToCol = new Dictionary<String, Int32>();

                    writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, "CSP Trusted Sites");

                    addFormatToExcelRange(xlWorksheet,
                                        rowStart,
                                        rowEnd,
                                        colStart,
                                        colEnd,
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

                    rowEnd += 2;
                    rowStart = rowEnd;
                    lastRowNumber = rowEnd;

                    nodeNameToCol.Add("Name", colEnd);
                    colEnd++;

                    foreach (String topFieldName in mdtFields)
                    {
                        nodeNameToCol.Add(topFieldName, colEnd);
                        colEnd++;
                    }

                    foreach (String colName in nodeNameToCol.Keys)
                    {
                        writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol[colName], colName);
                    }

                    addFormatToExcelRange(xlWorksheet,
                                        rowStart,
                                        rowEnd,
                                        colStart,
                                        colEnd,
                                        10,
                                        255,
                                        255,
                                        255,
                                        0,
                                        51,
                                        102,
                                        true,
                                        false,
                                        "");

                    rowEnd++;
                    lastRowNumber = rowEnd;

                    foreach (String fl in dirFiles)
                    {
                        // Write the file name value in the appropriate column
                        writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol["Name"], fullObjectNameExtract(fl));
                        addFormatToExcelRange(xlWorksheet,
                                            rowEnd,
                                            rowEnd,
                                            nodeNameToCol["Name"],
                                            nodeNameToCol["Name"],
                                            10,
                                            0,
                                            0,
                                            0,
                                            255,
                                            255,
                                            255,
                                            false,
                                            false,
                                            fullObjectNameExtract(fl));

                        XmlDocument xd = new XmlDocument();
                        xd.Load(fl);

                        foreach (XmlNode nd1 in xd.ChildNodes)
                        {
                            foreach (XmlNode nd2 in nd1.ChildNodes)
                            {
                                if (nd2.Name != "xml")
                                {
                                    writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol[nd2.Name], nd2.InnerText);
                                    addFormatToExcelRange(xlWorksheet,
                                                        rowEnd,
                                                        rowEnd,
                                                        nodeNameToCol[nd2.Name],
                                                        nodeNameToCol[nd2.Name],
                                                        10,
                                                        0,
                                                        0,
                                                        0,
                                                        255,
                                                        255,
                                                        255,
                                                        false,
                                                        false,
                                                        nd2.InnerText);
                                }
                            }
                        }

                        rowEnd++;

                        this.cwProgressBar.PerformStep();
                    }
                }
                else if (directoryName == "emailservices")
                {
                    Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                            (System.Reflection.Missing.Value,
                                                                                xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                                System.Reflection.Missing.Value,
                                                                                System.Reflection.Missing.Value);

                    xlWorksheet.Name = "EmailServices";

                    Int32 rowStart = 2;
                    Int32 rowEnd = 2;
                    Int32 colStart = 2;
                    Int32 colEnd = 2;
                    Int32 lastRowNumber = 2;

                    List<String> mdtFields = getObjectTopFieldNames(directoryName);
                    Dictionary<String, Int32> nodeNameToCol = new Dictionary<String, Int32>();

                    writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, "Email Services");

                    addFormatToExcelRange(xlWorksheet,
                                        rowStart,
                                        rowEnd,
                                        colStart,
                                        colEnd,
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

                    rowEnd += 2;
                    rowStart = rowEnd;
                    lastRowNumber = rowEnd;

                    nodeNameToCol.Add("Name", colEnd);
                    colEnd++;

                    foreach (String topFieldName in mdtFields)
                    {
                        nodeNameToCol.Add(topFieldName, colEnd);
                        colEnd++;
                    }

                    foreach (String colName in nodeNameToCol.Keys)
                    {
                        writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol[colName], colName);
                    }

                    addFormatToExcelRange(xlWorksheet,
                                        rowStart,
                                        rowEnd,
                                        colStart,
                                        colEnd,
                                        10,
                                        255,
                                        255,
                                        255,
                                        0,
                                        51,
                                        102,
                                        true,
                                        false,
                                        "");

                    rowEnd++;
                    lastRowNumber = rowEnd;

                    foreach (String fl in dirFiles)
                    {
                        // Write the file name value in the appropriate column
                        writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol["Name"], fullObjectNameExtract(fl));
                        addFormatToExcelRange(xlWorksheet,
                                            rowEnd,
                                            rowEnd,
                                            nodeNameToCol["Name"],
                                            nodeNameToCol["Name"],
                                            10,
                                            0,
                                            0,
                                            0,
                                            255,
                                            255,
                                            255,
                                            false,
                                            false,
                                            fullObjectNameExtract(fl));

                        XmlDocument xd = new XmlDocument();
                        xd.Load(fl);

                        foreach (XmlNode nd1 in xd.ChildNodes)
                        {
                            foreach (XmlNode nd2 in nd1.ChildNodes)
                            {
                                if (nd2.Name != "xml")
                                {
                                    writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol[nd2.Name], nd2.InnerText);
                                    addFormatToExcelRange(xlWorksheet,
                                                        rowEnd,
                                                        rowEnd,
                                                        nodeNameToCol[nd2.Name],
                                                        nodeNameToCol[nd2.Name],
                                                        10,
                                                        0,
                                                        0,
                                                        0,
                                                        255,
                                                        255,
                                                        255,
                                                        false,
                                                        false,
                                                        nd2.InnerText);
                                }
                            }
                        }

                        rowEnd++;

                        this.cwProgressBar.PerformStep();

                    }
                }
                else if (directoryName == "installedPackages")
                {
                    Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                            (System.Reflection.Missing.Value,
                                                                                xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                                System.Reflection.Missing.Value,
                                                                                System.Reflection.Missing.Value);

                    xlWorksheet.Name = "InstalledPackages";

                    Int32 rowStart = 2;
                    Int32 rowEnd = 2;
                    Int32 colStart = 2;
                    Int32 colEnd = 2;
                    Int32 lastRowNumber = 2;

                    List<String> mdtFields = getObjectTopFieldNames(directoryName);
                    Dictionary<String, Int32> nodeNameToCol = new Dictionary<String, Int32>();

                    writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, "Installed Packages");

                    addFormatToExcelRange(xlWorksheet,
                                        rowStart,
                                        rowEnd,
                                        colStart,
                                        colEnd,
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

                    rowEnd += 2;
                    rowStart = rowEnd;
                    lastRowNumber = rowEnd;

                    nodeNameToCol.Add("Name", colEnd);
                    colEnd++;

                    foreach (String topFieldName in mdtFields)
                    {
                        nodeNameToCol.Add(topFieldName, colEnd);
                        colEnd++;
                    }

                    foreach (String colName in nodeNameToCol.Keys)
                    {
                        writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol[colName], colName);
                    }

                    addFormatToExcelRange(xlWorksheet,
                                        rowStart,
                                        rowEnd,
                                        colStart,
                                        colEnd,
                                        10,
                                        255,
                                        255,
                                        255,
                                        0,
                                        51,
                                        102,
                                        true,
                                        false,
                                        "");

                    rowEnd++;
                    lastRowNumber = rowEnd;

                    foreach (String fl in dirFiles)
                    {
                        // Write the file name value in the appropriate column
                        writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol["Name"], fullObjectNameExtract(fl));
                        addFormatToExcelRange(xlWorksheet,
                                            rowEnd,
                                            rowEnd,
                                            nodeNameToCol["Name"],
                                            nodeNameToCol["Name"],
                                            10,
                                            0,
                                            0,
                                            0,
                                            255,
                                            255,
                                            255,
                                            false,
                                            false,
                                            fullObjectNameExtract(fl));

                        XmlDocument xd = new XmlDocument();
                        xd.Load(fl);

                        foreach (XmlNode nd1 in xd.ChildNodes)
                        {
                            foreach (XmlNode nd2 in nd1.ChildNodes)
                            {
                                if (nd2.Name != "xml")
                                {
                                    writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol[nd2.Name], nd2.InnerText);
                                    addFormatToExcelRange(xlWorksheet,
                                                        rowEnd,
                                                        rowEnd,
                                                        nodeNameToCol[nd2.Name],
                                                        nodeNameToCol[nd2.Name],
                                                        10,
                                                        0,
                                                        0,
                                                        0,
                                                        255,
                                                        255,
                                                        255,
                                                        false,
                                                        false,
                                                        nd2.InnerText);
                                }
                            }
                        }

                        rowEnd++;

                        this.cwProgressBar.PerformStep();
                    }
                }
                else if (directoryName == "lwc")
                {

                }
                else if (directoryName == "namedCredentials")
                {
                    Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                            (System.Reflection.Missing.Value,
                                                                                xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                                System.Reflection.Missing.Value,
                                                                                System.Reflection.Missing.Value);

                    xlWorksheet.Name = "NamedCredentials";

                    Int32 rowStart = 2;
                    Int32 rowEnd = 2;
                    Int32 colStart = 2;
                    Int32 colEnd = 2;
                    Int32 lastRowNumber = 2;

                    List<String> mdtFields = getObjectTopFieldNames(directoryName);
                    Dictionary<String, Int32> nodeNameToCol = new Dictionary<String, Int32>();

                    writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, "Named Credentials");

                    addFormatToExcelRange(xlWorksheet,
                                        rowStart,
                                        rowEnd,
                                        colStart,
                                        colEnd,
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

                    rowEnd += 2;
                    rowStart = rowEnd;
                    lastRowNumber = rowEnd;

                    nodeNameToCol.Add("Name", colEnd);
                    colEnd++;

                    foreach (String topFieldName in mdtFields)
                    {
                        nodeNameToCol.Add(topFieldName, colEnd);
                        colEnd++;
                    }

                    foreach (String colName in nodeNameToCol.Keys)
                    {
                        writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol[colName], colName);
                    }

                    addFormatToExcelRange(xlWorksheet,
                                        rowStart,
                                        rowEnd,
                                        colStart,
                                        colEnd,
                                        10,
                                        255,
                                        255,
                                        255,
                                        0,
                                        51,
                                        102,
                                        true,
                                        false,
                                        "");

                    rowEnd++;
                    lastRowNumber = rowEnd;

                    foreach (String fl in dirFiles)
                    {
                        // Write the file name value in the appropriate column
                        writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol["Name"], fullObjectNameExtract(fl));
                        addFormatToExcelRange(xlWorksheet,
                                            rowEnd,
                                            rowEnd,
                                            nodeNameToCol["Name"],
                                            nodeNameToCol["Name"],
                                            10,
                                            0,
                                            0,
                                            0,
                                            255,
                                            255,
                                            255,
                                            false,
                                            false,
                                            fullObjectNameExtract(fl));

                        XmlDocument xd = new XmlDocument();
                        xd.Load(fl);

                        foreach (XmlNode nd1 in xd.ChildNodes)
                        {
                            foreach (XmlNode nd2 in nd1.ChildNodes)
                            {
                                if (nd2.Name != "xml")
                                {
                                    writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol[nd2.Name], nd2.InnerText);
                                    addFormatToExcelRange(xlWorksheet,
                                                        rowEnd,
                                                        rowEnd,
                                                        nodeNameToCol[nd2.Name],
                                                        nodeNameToCol[nd2.Name],
                                                        10,
                                                        0,
                                                        0,
                                                        0,
                                                        255,
                                                        255,
                                                        255,
                                                        false,
                                                        false,
                                                        nd2.InnerText);
                                }
                            }
                        }

                        rowEnd++;

                        this.cwProgressBar.PerformStep();
                    }

                }
                else if (directoryName == "pages")
                {
                    Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                            (System.Reflection.Missing.Value,
                                                                                xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                                System.Reflection.Missing.Value,
                                                                                System.Reflection.Missing.Value);

                    xlWorksheet.Name = "Pages";

                    Int32 rowStart = 2;
                    Int32 rowEnd = 2;
                    Int32 colStart = 2;
                    Int32 colEnd = 2;
                    Int32 lastRowNumber = 2;

                    List<String> mdtFields = getObjectTopFieldNames(directoryName);
                    Dictionary<String, Int32> nodeNameToCol = new Dictionary<String, Int32>();

                    writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, "Visualforce Pages");

                    addFormatToExcelRange(xlWorksheet,
                                        rowStart,
                                        rowEnd,
                                        colStart,
                                        colEnd,
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

                    rowEnd += 2;
                    rowStart = rowEnd;
                    lastRowNumber = rowEnd;

                    nodeNameToCol.Add("Name", colEnd);
                    colEnd++;
                    foreach (String topFieldName in mdtFields)
                    {
                        nodeNameToCol.Add(topFieldName, colEnd);
                        colEnd++;
                    }

                    foreach (String colName in nodeNameToCol.Keys)
                    {
                        writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol[colName], colName);
                    }

                    addFormatToExcelRange(xlWorksheet,
                                        rowStart,
                                        rowEnd,
                                        colStart,
                                        colEnd - 1,
                                        10,
                                        255,
                                        255,
                                        255,
                                        0,
                                        51,
                                        102,
                                        true,
                                        false,
                                        "");

                    rowEnd++;
                    lastRowNumber = rowEnd;

                    foreach (String fl in dirFiles)
                    {
                        String fullObjectName = fullObjectNameExtract(@fl);

                        // Parse the XML Name and pull out the class name
                        String objectNamePath = fl.Split('-')[0];
                        String[] filePathSplit = fl.Split('\\');
                        String[] fileNameSplit = filePathSplit[filePathSplit.Length - 1].Split('-');
                        String objectName = fileNameSplit[0].Split('.')[0];

                        if (fl.EndsWith(".xml"))
                        {
                            writeDataToExcelSheet(xlWorksheet, rowEnd, 2, objectName);
                            addFormatToExcelRange(xlWorksheet,
                                                rowEnd,
                                                rowEnd,
                                                2,
                                                2,
                                                10,
                                                0,
                                                0,
                                                0,
                                                192,
                                                192,
                                                192,
                                                false,
                                                true,
                                                "");

                            XmlDocument xd = new XmlDocument();
                            xd.Load(fl);

                            foreach (XmlNode nd1 in xd.ChildNodes)
                            {
                                if (nd1.LocalName == "xml")
                                {
                                    continue;
                                }
                                else
                                {
                                    foreach (XmlNode nd2 in nd1.ChildNodes)
                                    {
                                        writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol[nd2.Name], nd2.InnerText);
                                    }
                                }
                            }

                            rowEnd++;
                        }

                        this.cwProgressBar.PerformStep();
                    }
                }
                else if (directoryName == "profilePasswordPolicies")
                {
                    Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                            (System.Reflection.Missing.Value,
                                                                                xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                                System.Reflection.Missing.Value,
                                                                                System.Reflection.Missing.Value);

                    xlWorksheet.Name = "ProfilePasswordPolicies";

                    Int32 rowStart = 2;
                    Int32 rowEnd = 2;
                    Int32 colStart = 2;
                    Int32 colEnd = 2;
                    Int32 lastRowNumber = 2;

                    List<String> mdtFields = getObjectTopFieldNames(directoryName);
                    Dictionary<String, Int32> nodeNameToCol = new Dictionary<String, Int32>();

                    writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, "Profile Password Policies");

                    addFormatToExcelRange(xlWorksheet,
                                        rowStart,
                                        rowEnd,
                                        colStart,
                                        colEnd,
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

                    rowEnd += 2;
                    rowStart = rowEnd;
                    lastRowNumber = rowEnd;

                    nodeNameToCol.Add("Name", colEnd);
                    colEnd++;

                    foreach (String topFieldName in mdtFields)
                    {
                        nodeNameToCol.Add(topFieldName, colEnd);
                        colEnd++;
                    }

                    foreach (String colName in nodeNameToCol.Keys)
                    {
                        writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol[colName], colName);
                    }

                    addFormatToExcelRange(xlWorksheet,
                                        rowStart,
                                        rowEnd,
                                        colStart,
                                        colEnd,
                                        10,
                                        255,
                                        255,
                                        255,
                                        0,
                                        51,
                                        102,
                                        true,
                                        false,
                                        "");

                    rowEnd++;
                    lastRowNumber = rowEnd;

                    foreach (String fl in dirFiles)
                    {
                        // Write the file name value in the appropriate column
                        writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol["Name"], fullObjectNameExtract(fl));
                        addFormatToExcelRange(xlWorksheet,
                                            rowEnd,
                                            rowEnd,
                                            nodeNameToCol["Name"],
                                            nodeNameToCol["Name"],
                                            10,
                                            0,
                                            0,
                                            0,
                                            255,
                                            255,
                                            255,
                                            false,
                                            false,
                                            fullObjectNameExtract(fl));

                        XmlDocument xd = new XmlDocument();
                        xd.Load(fl);

                        foreach (XmlNode nd1 in xd.ChildNodes)
                        {
                            foreach (XmlNode nd2 in nd1.ChildNodes)
                            {
                                if (nd2.Name != "xml")
                                {
                                    writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol[nd2.Name], nd2.InnerText);
                                    addFormatToExcelRange(xlWorksheet,
                                                        rowEnd,
                                                        rowEnd,
                                                        nodeNameToCol[nd2.Name],
                                                        nodeNameToCol[nd2.Name],
                                                        10,
                                                        0,
                                                        0,
                                                        0,
                                                        255,
                                                        255,
                                                        255,
                                                        false,
                                                        false,
                                                        nd2.InnerText);
                                }
                            }
                        }

                        rowEnd++;

                        this.cwProgressBar.PerformStep();
                    }
                }
                else if (directoryName == "remoteSiteSettings")
                {
                    Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                            (System.Reflection.Missing.Value,
                                                                                xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                                System.Reflection.Missing.Value,
                                                                                System.Reflection.Missing.Value);

                    xlWorksheet.Name = "RemoteSiteSettings";

                    Int32 rowStart = 2;
                    Int32 rowEnd = 2;
                    Int32 colStart = 2;
                    Int32 colEnd = 2;
                    Int32 lastRowNumber = 2;

                    List<String> mdtFields = getObjectTopFieldNames(directoryName);
                    Dictionary<String, Int32> nodeNameToCol = new Dictionary<String, Int32>();

                    writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, "Remote Site Settings");

                    addFormatToExcelRange(xlWorksheet,
                                        rowStart,
                                        rowEnd,
                                        colStart,
                                        colEnd,
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

                    rowEnd += 2;
                    rowStart = rowEnd;
                    lastRowNumber = rowEnd;

                    nodeNameToCol.Add("Name", colEnd);
                    colEnd++;

                    foreach (String topFieldName in mdtFields)
                    {
                        nodeNameToCol.Add(topFieldName, colEnd);
                        colEnd++;
                    }

                    foreach (String colName in nodeNameToCol.Keys)
                    {
                        writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol[colName], colName);
                    }

                    addFormatToExcelRange(xlWorksheet,
                                        rowStart,
                                        rowEnd,
                                        colStart,
                                        colEnd,
                                        10,
                                        255,
                                        255,
                                        255,
                                        0,
                                        51,
                                        102,
                                        true,
                                        false,
                                        "");

                    rowEnd++;
                    lastRowNumber = rowEnd;

                    foreach (String fl in dirFiles)
                    {
                        // Write the file name value in the appropriate column
                        writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol["Name"], fullObjectNameExtract(fl));
                        addFormatToExcelRange(xlWorksheet,
                                            rowEnd,
                                            rowEnd,
                                            nodeNameToCol["Name"],
                                            nodeNameToCol["Name"],
                                            10,
                                            0,
                                            0,
                                            0,
                                            255,
                                            255,
                                            255,
                                            false,
                                            false,
                                            fullObjectNameExtract(fl));

                        XmlDocument xd = new XmlDocument();
                        xd.Load(fl);

                        foreach (XmlNode nd1 in xd.ChildNodes)
                        {
                            foreach (XmlNode nd2 in nd1.ChildNodes)
                            {
                                if (nd2.Name != "xml")
                                {
                                    writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol[nd2.Name], nd2.InnerText);
                                    addFormatToExcelRange(xlWorksheet,
                                                        rowEnd,
                                                        rowEnd,
                                                        nodeNameToCol[nd2.Name],
                                                        nodeNameToCol[nd2.Name],
                                                        10,
                                                        0,
                                                        0,
                                                        0,
                                                        255,
                                                        255,
                                                        255,
                                                        false,
                                                        false,
                                                        nd2.InnerText);
                                }
                            }
                        }

                        rowEnd++;

                        this.cwProgressBar.PerformStep();
                    }
                }
                else if (directoryName == "staticresource")
                {
                    Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                            (System.Reflection.Missing.Value,
                                                                                xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                                System.Reflection.Missing.Value,
                                                                                System.Reflection.Missing.Value);

                    xlWorksheet.Name = "Static Resources";

                    Int32 rowStart = 2;
                    Int32 rowEnd = 2;
                    Int32 colStart = 2;
                    Int32 colEnd = 2;
                    Int32 lastRowNumber = 2;

                    List<String> mdtFields = getObjectTopFieldNames(directoryName);
                    Dictionary<String, Int32> nodeNameToCol = new Dictionary<String, Int32>();

                    writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, "Static Resources");

                    addFormatToExcelRange(xlWorksheet,
                                        rowStart,
                                        rowEnd,
                                        colStart,
                                        colEnd,
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

                    rowEnd += 2;
                    rowStart = rowEnd;
                    lastRowNumber = rowEnd;

                    nodeNameToCol.Add("Name", colEnd);
                    colEnd++;
                    foreach (String topFieldName in mdtFields)
                    {
                        nodeNameToCol.Add(topFieldName, colEnd);
                        colEnd++;
                    }

                    foreach (String colName in nodeNameToCol.Keys)
                    {
                        writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol[colName], colName);
                    }

                    addFormatToExcelRange(xlWorksheet,
                                        rowStart,
                                        rowEnd,
                                        colStart,
                                        colEnd - 1,
                                        10,
                                        255,
                                        255,
                                        255,
                                        0,
                                        51,
                                        102,
                                        true,
                                        false,
                                        "");

                    rowEnd++;
                    lastRowNumber = rowEnd;

                    foreach (String fl in dirFiles)
                    {
                        String fullObjectName = fullObjectNameExtract(@fl);

                        // Parse the XML Name and pull out the class name
                        String objectNamePath = fl.Split('-')[0];
                        String[] filePathSplit = fl.Split('\\');
                        String[] fileNameSplit = filePathSplit[filePathSplit.Length - 1].Split('-');
                        String objectName = fileNameSplit[0].Split('.')[0];

                        if (fl.EndsWith(".xml"))
                        {
                            writeDataToExcelSheet(xlWorksheet, rowEnd, 2, objectName);
                            addFormatToExcelRange(xlWorksheet,
                                                rowEnd,
                                                rowEnd,
                                                2,
                                                2,
                                                10,
                                                0,
                                                0,
                                                0,
                                                192,
                                                192,
                                                192,
                                                false,
                                                true,
                                                "");

                            XmlDocument xd = new XmlDocument();
                            xd.Load(fl);

                            foreach (XmlNode nd1 in xd.ChildNodes)
                            {
                                if (nd1.LocalName == "xml")
                                {
                                    continue;
                                }
                                else
                                {
                                    foreach (XmlNode nd2 in nd1.ChildNodes)
                                    {
                                        writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol[nd2.Name], nd2.InnerText);
                                    }
                                }
                            }

                            rowEnd++;
                        }

                        this.cwProgressBar.PerformStep();
                    }
                }
                else if (directoryName == "triggers")
                {
                    Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                            (System.Reflection.Missing.Value,
                                                                                xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                                System.Reflection.Missing.Value,
                                                                                System.Reflection.Missing.Value);

                    xlWorksheet.Name = "Triggers";

                    Int32 rowStart = 2;
                    Int32 rowEnd = 2;
                    Int32 colStart = 2;
                    Int32 colEnd = 2;
                    Int32 lastRowNumber = 2;

                    List<String> mdtFields = getObjectTopFieldNames(directoryName);
                    Dictionary<String, Int32> nodeNameToCol = new Dictionary<String, Int32>();

                    writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, "Apex Triggers");

                    addFormatToExcelRange(xlWorksheet,
                                        rowStart,
                                        rowEnd,
                                        colStart,
                                        colEnd,
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

                    rowEnd += 2;
                    rowStart = rowEnd;
                    lastRowNumber = rowEnd;

                    nodeNameToCol.Add("Name", colEnd);
                    colEnd++;

                    nodeNameToCol.Add("Object", colEnd);
                    colEnd++;

                    foreach (String topFieldName in mdtFields)
                    {
                        nodeNameToCol.Add(topFieldName, colEnd);
                        colEnd++;
                    }

                    nodeNameToCol.Add("Before Insert", colEnd);
                    colEnd++;

                    nodeNameToCol.Add("Before Update", colEnd);
                    colEnd++;

                    nodeNameToCol.Add("Before Delete", colEnd);
                    colEnd++;

                    nodeNameToCol.Add("After Insert", colEnd);
                    colEnd++;

                    nodeNameToCol.Add("After Update", colEnd);
                    colEnd++;

                    nodeNameToCol.Add("After Delete", colEnd);
                    colEnd++;

                    nodeNameToCol.Add("After Undelete", colEnd);
                    colEnd++;

                    foreach (String colName in nodeNameToCol.Keys)
                    {
                        writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol[colName], colName);
                    }

                    addFormatToExcelRange(xlWorksheet,
                                        rowStart,
                                        rowEnd,
                                        colStart,
                                        colEnd - 1,
                                        10,
                                        255,
                                        255,
                                        255,
                                        0,
                                        51,
                                        102,
                                        true,
                                        false,
                                        "");

                    rowEnd++;
                    lastRowNumber = rowEnd;

                    foreach (String fl in dirFiles)
                    {
                        String fullObjectName = fullObjectNameExtract(@fl);

                        // Parse the XML Name and pull out the class name
                        String objectNamePath = fl.Split('-')[0];
                        String[] filePathSplit = fl.Split('\\');
                        String[] fileNameSplit = filePathSplit[filePathSplit.Length - 1].Split('-');
                        String objectName = fileNameSplit[0].Split('.')[0];

                        if (fl.EndsWith(".xml"))
                        {
                            writeDataToExcelSheet(xlWorksheet, rowEnd, 2, objectName);
                            addFormatToExcelRange(xlWorksheet,
                                                rowEnd,
                                                rowEnd,
                                                2,
                                                2,
                                                10,
                                                0,
                                                0,
                                                0,
                                                192,
                                                192,
                                                192,
                                                false,
                                                true,
                                                "");

                            XmlDocument xd = new XmlDocument();
                            xd.Load(fl);

                            foreach (XmlNode nd1 in xd.ChildNodes)
                            {
                                if (nd1.LocalName == "xml")
                                {
                                    continue;
                                }
                                else
                                {
                                    foreach (XmlNode nd2 in nd1.ChildNodes)
                                    {
                                        writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol[nd2.Name], nd2.InnerText);
                                    }
                                }
                            }

                            StreamReader sr = new StreamReader(objectNamePath);

                            while (sr.EndOfStream == false)
                            {
                                String lineRead = sr.ReadLine().Trim();
                                if (lineRead.StartsWith("trigger"))
                                {
                                    String[] parsedTriggerLine = lineRead.Split(new char[] { ' ', '(', ')', ',' });
                                    String[] triggerOperations = new string[17];

                                    Int32 i = 0;
                                    foreach (String triggerOpp in parsedTriggerLine)
                                    {
                                        if (triggerOpp != "")
                                        {
                                            triggerOperations[i] = triggerOpp;
                                            i++;
                                        }
                                    }

                                    i = 0;

                                    // Write the object the trigger is on
                                    writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol["Object"], triggerOperations[3]);

                                    foreach (String triggerOpp in triggerOperations)
                                    {
                                        if (triggerOpp == null)
                                        {
                                            continue;
                                        }
                                        else if (triggerOpp.ToLower() == "before" && triggerOperations[i + 1].ToLower() == "insert")
                                        {
                                            writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol["Before Insert"], "TRUE");
                                        }
                                        else if (triggerOpp.ToLower() == "before" && triggerOperations[i + 1].ToLower() == "update")
                                        {
                                            writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol["Before Update"], "TRUE");
                                        }
                                        else if (triggerOpp.ToLower() == "before" && triggerOperations[i + 1].ToLower() == "delete")
                                        {
                                            writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol["Before Delete"], "TRUE");
                                        }
                                        else if (triggerOpp.ToLower() == "after" && triggerOperations[i + 1].ToLower() == "insert")
                                        {
                                            writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol["After Insert"], "TRUE");
                                        }
                                        else if (triggerOpp.ToLower() == "after" && triggerOperations[i + 1].ToLower() == "update")
                                        {
                                            writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol["After Update"], "TRUE");
                                        }
                                        else if (triggerOpp.ToLower() == "after" && triggerOperations[i + 1].ToLower() == "delete")
                                        {
                                            writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol["After Delete"], "TRUE");
                                        }
                                        else if (triggerOpp.ToLower() == "after" && triggerOperations[i + 1].ToLower() == "undelete")
                                        {
                                            writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol["After Undelete"], "TRUE");
                                        }

                                        i++;
                                    }
                                }
                            }

                            sr.Close();

                            rowEnd++;
                        }

                        this.cwProgressBar.PerformStep();
                    }
                }
                else
                {
                    foreach (String fl in dirFiles)
                    {
                        String fullObjectName = fullObjectNameExtract(@fl);
                        String objNameAbbrev = checkObjectNameLength(directoryName, fullObjectName, fileNames);

                        writeTablesAndValuesToExcel(xlWorkbook, fl, directoryName, fullObjectName, objNameAbbrev);

                        this.cwProgressBar.PerformStep();
                    }
                }
            }

            this.chkListBoxTasks.SetItemChecked(0, true);

            this.cwProgressBar.Visible = true;
            this.cwProgressBar.Minimum = 1;
            this.cwProgressBar.Value = 1;
            this.cwProgressBar.Step = 1;
            this.cwProgressBar.Maximum = excelBorderFormatList.Count;

            this.lblProgressIndicator.Text = "Apply Borders to Cells";

            //foreach (ExcelBorderFormat ebf in excelBorderFormatList)
            //{
            //    applyExcelBorder(ebf);
            //    this.cwProgressBar.PerformStep();
            //}

            this.chkListBoxTasks.SetItemChecked(1, true);


            this.cwProgressBar.Visible = true;
            this.cwProgressBar.Minimum = 1;
            this.cwProgressBar.Value = 1;
            this.cwProgressBar.Step = 1;
            this.cwProgressBar.Maximum = excelRangeFormatList.Count;

            this.lblProgressIndicator.Text = "Apply Cell Formatting";

            foreach (ExcelRangeFormat erf in excelRangeFormatList)
            {
                applyExcelRangeFormats(erf);
                this.cwProgressBar.PerformStep();
            }

            this.chkListBoxTasks.SetItemChecked(2, true);

            excelBorderFormatList.Clear();
            excelRangeFormatList.Clear();

            xlapp.Visible = true;
        }

        public void writeTablesAndValuesToExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook, String fileName, String directoryName, String fullObjectName, String objNameAbbrev)
        {
            // Load the XML document
            XmlDocument xd = new XmlDocument();
            Boolean isXmlDocument = true;
            try
            {
                xd.Load(fileName);
            }
            catch (Exception e)
            {
                isXmlDocument = false;
            }

            if (isXmlDocument == false) return;

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                        (System.Reflection.Missing.Value,
                                                                         xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                         System.Reflection.Missing.Value,
                                                                         System.Reflection.Missing.Value);

            xlWorksheet.Name = objNameAbbrev;

            Int32 rowStart = 2;
            Int32 rowEnd = 2;
            Int32 colStart = 2;
            Int32 colEnd = 2;
            Int32 lastRowNumber = 2;

            List<String> columnNames1 = new List<String>();
            List<String> columnNames2 = new List<String>();
            List<String> columnNames3 = new List<String>();
            Dictionary<String, List<String>> columnNames4 = new Dictionary<String, List<String>>();
            Dictionary<String, List<String>> columnNames5 = new Dictionary<String, List<String>>();
            Dictionary<String, List<String>> columnNames6 = new Dictionary<String, List<String>>();
            Dictionary<String, List<String>> columnNames7 = new Dictionary<String, List<String>>();
            Dictionary<String, List<String>> columnNames8 = new Dictionary<String, List<String>>();

            Dictionary<String, Int32> nodeNameToCol2 = new Dictionary<String, Int32>();
            Dictionary<String, Int32> nodeNameToCol3 = new Dictionary<String, Int32>();
            Dictionary<String, Int32> nodeNameToCol4 = new Dictionary<String, Int32>();
            Dictionary<String, Int32> nodeNameToCol5 = new Dictionary<String, Int32>();
            Dictionary<String, Int32> nodeNameToCol6 = new Dictionary<String, Int32>();
            Dictionary<String, Int32> nodeNameToCol7 = new Dictionary<String, Int32>();
            Dictionary<String, Int32> nodeNameToCol8 = new Dictionary<String, Int32>();

            // Holds onto the objects to focus on when writing the fields out for the permission sets so that the task of writing these out does not encumber the 
            // tool with unnecessary field permissions.
            List<String> permissionSetObjectsFocus = new List<string>();

            List<String> mdtFields = new List<string>();
            if (directoryName == "settings")
            {
                mdtFields = getObjectTopFieldNames(directoryName, fileName);
            }
            else
            {
                mdtFields = getObjectTopFieldNames(directoryName);
            }


            foreach (XmlNode nd1 in xd.ChildNodes)
            {
                if (nd1.LocalName == "xml")
                {
                    continue;
                }
                else if (nd1.NodeType == XmlNodeType.Element)
                {
                    writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, nd1.Name);
                    colEnd++;
                    writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, fullObjectName);

                    addFormatToExcelRange(xlWorksheet,
                                        rowStart,
                                        rowEnd,
                                        colStart,
                                        colEnd,
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
                }


                // First Loop - get the Parent Node Names and columns
                // Adjust the row start for the values before writing them to account for multiple header names
                // Add each to the dictionaries related to the appropriate node levels

                foreach (String nodeName in mdtFields)
                {
                    XmlNodeList nodeList = xd.GetElementsByTagName(nodeName);

                    // [BUILD COLUMN NAMES]
                    foreach (XmlNode nd2 in nodeList)
                    {
                        if (nd2.ParentNode.Name == nd1.Name && nd2.NodeType == XmlNodeType.Element)
                        {
                            if (!nodeNameToCol2.ContainsKey(nd2.Name)) nodeNameToCol2.Add(nd2.Name, 0);
                            if (!columnNames2.Contains(nd2.Name)) columnNames2.Add(nd2.Name);

                            foreach (XmlNode nd3 in nd2.ChildNodes)
                            {
                                if (nd3.ParentNode.Name == nd2.Name && nd3.NodeType == XmlNodeType.Element)
                                {
                                    if (!nodeNameToCol3.ContainsKey(nd3.Name)) nodeNameToCol3.Add(nd3.Name, 0);
                                    if (!columnNames3.Contains(nd3.Name)) columnNames3.Add(nd3.Name);

                                    foreach (XmlNode nd4 in nd3.ChildNodes)
                                    {
                                        if (nd4.ParentNode.Name == nd3.Name && nd4.NodeType == XmlNodeType.Element)
                                        {
                                            if (!nodeNameToCol4.ContainsKey(nd4.Name)) nodeNameToCol4.Add(nd4.Name, 0);
                                            if (!columnNames4.ContainsKey(nd3.Name))
                                            {
                                                columnNames4.Add(nd3.Name, new List<String> { nd4.Name });
                                            }
                                            else if (!columnNames4[nd3.Name].Contains(nd4.Name))
                                            {
                                                columnNames4[nd3.Name].Add(nd4.Name);
                                            }

                                            foreach (XmlNode nd5 in nd4.ChildNodes)
                                            {
                                                if (nd5.ParentNode.Name == nd4.Name && nd5.NodeType == XmlNodeType.Element)
                                                {
                                                    if (!nodeNameToCol5.ContainsKey(nd5.Name)) nodeNameToCol5.Add(nd5.Name, 0);
                                                    if (!columnNames5.ContainsKey(nd3.Name))
                                                    {
                                                        columnNames5.Add(nd3.Name, new List<String> { nd5.Name });
                                                    }
                                                    else if (!columnNames5[nd3.Name].Contains(nd5.Name))
                                                    {
                                                        columnNames5[nd3.Name].Add(nd5.Name);
                                                    }

                                                    foreach (XmlNode nd6 in nd5.ChildNodes)
                                                    {
                                                        if (nd6.ParentNode.Name == nd5.Name && nd6.NodeType == XmlNodeType.Element)
                                                        {
                                                            if (!nodeNameToCol6.ContainsKey(nd6.Name)) nodeNameToCol6.Add(nd6.Name, 0);
                                                            if (!columnNames6.ContainsKey(nd3.Name))
                                                            {
                                                                columnNames6.Add(nd3.Name, new List<String> { nd6.Name });
                                                            }
                                                            else if (!columnNames6[nd3.Name].Contains(nd6.Name))
                                                            {
                                                                columnNames6[nd3.Name].Add(nd6.Name);
                                                            }

                                                            foreach (XmlNode nd7 in nd6.ChildNodes)
                                                            {
                                                                if (nd7.ParentNode.Name == nd6.Name && nd7.NodeType == XmlNodeType.Element)
                                                                {
                                                                    if (!nodeNameToCol7.ContainsKey(nd7.Name)) nodeNameToCol7.Add(nd7.Name, 0);
                                                                    if (!columnNames7.ContainsKey(nd3.Name))
                                                                    {
                                                                        columnNames7.Add(nd3.Name, new List<String> { nd7.Name });
                                                                    }
                                                                    else if (!columnNames7[nd3.Name].Contains(nd7.Name))
                                                                    {
                                                                        columnNames7[nd3.Name].Add(nd7.Name);
                                                                    }

                                                                    foreach (XmlNode nd8 in nd7.ChildNodes)
                                                                    {
                                                                        if (nd8.ParentNode.Name == nd7.Name && nd8.NodeType == XmlNodeType.Element)
                                                                        {
                                                                            if (!nodeNameToCol8.ContainsKey(nd8.Name)) nodeNameToCol8.Add(nd8.Name, 0);
                                                                            if (!columnNames8.ContainsKey(nd3.Name))
                                                                            {
                                                                                columnNames8.Add(nd3.Name, new List<String> { nd8.Name });
                                                                            }
                                                                            else if (!columnNames8[nd3.Name].Contains(nd8.Name))
                                                                            {
                                                                                columnNames8[nd3.Name].Add(nd8.Name);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // [WRITE COLUMN NAMES AND SET COLUMN NUMBERS]
                    // Now write the column names
                    // We don't want to increment the rowStart value unless there are values in columnNames2 to write
                    Boolean subHeadersWritten = false;
                    Int32 sectionColumnEnd = 2; // To used with setting the column format
                    if (columnNames2.Count > 0)
                    {
                        rowStart = lastRowNumber + 2;
                        rowEnd = rowStart;
                        colStart = 2;
                        colEnd = 2;

                        foreach (String colHeader2 in columnNames2)
                        {
                            writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, colHeader2);

                            colEnd = getColumnEndValue(colEnd,
                                                       0,
                                                       0,
                                                       columnNames3.Count, 
                                                       columnNames4, 
                                                       columnNames5, 
                                                       columnNames6, 
                                                       columnNames7, 
                                                       columnNames8);

                            addFormatToExcelRange(xlWorksheet,
                                                rowStart,
                                                rowEnd,
                                                colStart,
                                                colEnd,
                                                10,
                                                255,
                                                255,
                                                255,
                                                0,
                                                51,
                                                102,
                                                true,
                                                false,
                                                "");

                            nodeNameToCol2[colHeader2] = colEnd;
                            sectionColumnEnd = colEnd;

                            colEnd++;
                        }


                        if (columnNames3.Count > 0) rowEnd++;
                        rowStart = rowEnd;
                        lastRowNumber = rowEnd;
                        colEnd = 2;

                        foreach (String colHeader3 in columnNames3)
                        {
                            List<String> subHeaderNames = MetadataDifferenceProcessing.getSubHeaderNames(directoryName, nodeName);

                            Int32 colOffset3 = subHeaderNames.IndexOf(colHeader3);

                            subHeadersWritten = true;

                            // Reset the row number for the next column
                            rowEnd = lastRowNumber;

                            if (colOffset3 >= 0)
                            {
                                writeDataToExcelSheet(xlWorksheet, rowEnd, colOffset3, colHeader3);
                                addFormatToExcelRange(xlWorksheet,
                                                    rowEnd,
                                                    rowEnd,
                                                    colOffset3,
                                                    colOffset3,
                                                    10,
                                                    0,
                                                    0,
                                                    0,
                                                    192,
                                                    192,
                                                    192,
                                                    false,
                                                    true,
                                                    "");

                                nodeNameToCol3[colHeader3] = colOffset3;
                            }
                            else
                            {
                                writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, colHeader3);
                                addFormatToExcelRange(xlWorksheet,
                                                    rowEnd,
                                                    rowEnd,
                                                    colEnd,
                                                    sectionColumnEnd,
                                                    10,
                                                    0,
                                                    0,
                                                    0,
                                                    192,
                                                    192,
                                                    192,
                                                    false,
                                                    true,
                                                    "");

                                nodeNameToCol3[colHeader3] = colEnd;
                            }

                            colEnd++;

                            if (columnNames4.ContainsKey(colHeader3))
                            {
                                if (columnNames4[colHeader3].Count > 1)
                                {
                                    rowEnd++;
                                    colStart = colEnd;

                                    foreach (String colHeader4 in columnNames4[colHeader3])
                                    {
                                        writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, colHeader4);
                                        addFormatToExcelRange(xlWorksheet,
                                                            rowEnd,
                                                            rowEnd,
                                                            colEnd,
                                                            sectionColumnEnd,
                                                            10,
                                                            0,
                                                            0,
                                                            0,
                                                            192,
                                                            192,
                                                            192,
                                                            false,
                                                            true,
                                                            "");

                                        nodeNameToCol4[colHeader4] = colEnd;
                                        colEnd++;
                                    }

                                    //addExcelBorderToList(xlWorksheet, rowEnd, rowEnd, colEnd, colEnd);
                                }
                                else
                                {
                                    rowEnd++;

                                    writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, columnNames4[colHeader3][0]);
                                    addFormatToExcelRange(xlWorksheet,
                                                        rowEnd,
                                                        rowEnd,
                                                        colEnd,
                                                        sectionColumnEnd,
                                                        10,
                                                        0,
                                                        0,
                                                        0,
                                                        192,
                                                        192,
                                                        192,
                                                        false,
                                                        true,
                                                        "");

                                    //addExcelBorderToList(xlWorksheet, rowEnd, rowEnd, colEnd, colEnd);

                                    nodeNameToCol4[columnNames4[colHeader3][0]] = colEnd;
                                    colEnd++;
                                }
                            }

                            if (columnNames5.ContainsKey(colHeader3))
                            {
                                if (columnNames5[colHeader3].Count > 1)
                                {
                                    rowEnd++;
                                    colStart = colEnd;

                                    foreach (String colHeader5 in columnNames5[colHeader3])
                                    {
                                        writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, colHeader5);
                                        addFormatToExcelRange(xlWorksheet,
                                                            rowEnd,
                                                            rowEnd,
                                                            colEnd,
                                                            sectionColumnEnd,
                                                            10,
                                                            0,
                                                            0,
                                                            0,
                                                            192,
                                                            192,
                                                            192,
                                                            false,
                                                            true,
                                                            "");

                                        nodeNameToCol5[colHeader5] = colEnd;
                                        colEnd++;
                                    }

                                    //addExcelBorderToList(xlWorksheet, rowEnd, rowEnd, colStart, colEnd - 1);
                                }
                                else
                                {
                                    rowEnd++;

                                    writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, columnNames5[colHeader3][0]);
                                    addFormatToExcelRange(xlWorksheet,
                                                        rowEnd,
                                                        rowEnd,
                                                        colEnd,
                                                        sectionColumnEnd,
                                                        10,
                                                        0,
                                                        0,
                                                        0,
                                                        192,
                                                        192,
                                                        192,
                                                        false,
                                                        true,
                                                        "");

                                    //addExcelBorderToList(xlWorksheet, rowEnd, rowEnd, colEnd, colEnd);

                                    nodeNameToCol5[columnNames5[colHeader3][0]] = colEnd;
                                    colEnd++;
                                }
                            }

                            if (columnNames6.ContainsKey(colHeader3))
                            {
                                if (columnNames6[colHeader3].Count > 1)
                                {
                                    rowEnd++;
                                    colStart = colEnd;

                                    foreach (String colHeader6 in columnNames6[colHeader3])
                                    {
                                        writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, colHeader6);
                                        addFormatToExcelRange(xlWorksheet,
                                                            rowEnd,
                                                            rowEnd,
                                                            colEnd,
                                                            sectionColumnEnd,
                                                            10,
                                                            0,
                                                            0,
                                                            0,
                                                            192,
                                                            192,
                                                            192,
                                                            false,
                                                            true,
                                                            "");

                                        nodeNameToCol6[colHeader6] = colEnd;
                                        colEnd++;
                                    }

                                    //addExcelBorderToList(xlWorksheet, rowEnd, rowEnd, colStart, colEnd - 1);
                                }
                                else
                                {
                                    rowEnd++;

                                    writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, columnNames6[colHeader3][0]);
                                    addFormatToExcelRange(xlWorksheet,
                                                        rowEnd,
                                                        rowEnd,
                                                        colEnd,
                                                        sectionColumnEnd,
                                                        10,
                                                        0,
                                                        0,
                                                        0,
                                                        192,
                                                        192,
                                                        192,
                                                        false,
                                                        true,
                                                        "");

                                    //addExcelBorderToList(xlWorksheet, rowEnd, rowEnd, colEnd, colEnd);

                                    nodeNameToCol6[columnNames6[colHeader3][0]] = colEnd;
                                    colEnd++;
                                }
                            }

                            if (columnNames7.ContainsKey(colHeader3))
                            {
                                if (columnNames7[colHeader3].Count > 1)
                                {
                                    rowEnd++;
                                    colStart = colEnd;

                                    foreach (String colHeader7 in columnNames7[colHeader3])
                                    {
                                        writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, colHeader7);
                                        addFormatToExcelRange(xlWorksheet,
                                                            rowEnd,
                                                            rowEnd,
                                                            colEnd,
                                                            sectionColumnEnd,
                                                            10,
                                                            0,
                                                            0,
                                                            0,
                                                            192,
                                                            192,
                                                            192,
                                                            false,
                                                            true,
                                                            "");

                                        nodeNameToCol7[colHeader7] = colEnd;
                                        colEnd++;
                                    }

                                    //addExcelBorderToList(xlWorksheet, rowEnd, rowEnd, colStart, colEnd - 1);
                                }
                                else
                                {
                                    rowEnd++;

                                    writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, columnNames7[colHeader3][0]);
                                    addFormatToExcelRange(xlWorksheet,
                                                        rowEnd,
                                                        rowEnd,
                                                        colEnd,
                                                        sectionColumnEnd,
                                                        10,
                                                        0,
                                                        0,
                                                        0,
                                                        192,
                                                        192,
                                                        192,
                                                        false,
                                                        true,
                                                        "");

                                    nodeNameToCol7[columnNames7[colHeader3][0]] = colEnd;
                                    colEnd++;
                                }
                            }

                            if (columnNames8.ContainsKey(colHeader3))
                            {
                                if (columnNames8[colHeader3].Count > 1)
                                {
                                    rowEnd++;
                                    colStart = colEnd;

                                    foreach (String colHeader8 in columnNames8[colHeader3])
                                    {
                                        writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, colHeader8);
                                        addFormatToExcelRange(xlWorksheet,
                                                            rowEnd,
                                                            rowEnd,
                                                            colEnd,
                                                            sectionColumnEnd,
                                                            10,
                                                            0,
                                                            0,
                                                            0,
                                                            192,
                                                            192,
                                                            192,
                                                            false,
                                                            true,
                                                            "");

                                        nodeNameToCol8[colHeader8] = colEnd;
                                        colEnd++;
                                    }

                                    //addExcelBorderToList(xlWorksheet, rowEnd, rowEnd, colStart, colEnd - 1);
                                }
                                else
                                {
                                    rowEnd++;

                                    writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, columnNames8[colHeader3][0]);
                                    addFormatToExcelRange(xlWorksheet,
                                                        rowEnd,
                                                        rowEnd,
                                                        colEnd,
                                                        sectionColumnEnd,
                                                        10,
                                                        0,
                                                        0,
                                                        0,
                                                        192,
                                                        192,
                                                        192,
                                                        false,
                                                        true,
                                                        "");

                                    nodeNameToCol8[columnNames8[colHeader3][0]] = colEnd;
                                    colEnd++;
                                }
                            }

                            colStart = colEnd;
                        }


                        // [BUILD VALUES LIST]
                        if (columnNames4.Count > 0) lastRowNumber++;
                        if (columnNames5.Count > 0) lastRowNumber++;
                        if (columnNames6.Count > 0) lastRowNumber++;
                        if (columnNames7.Count > 0) lastRowNumber++;
                        if (columnNames8.Count > 0) lastRowNumber++;

                        // We need to control when this should be set. If we have a scenario where the main column header is set, but there are 
                        // no sub-headers, then we should not set this.
                        if (subHeadersWritten)
                        {
                            rowEnd = lastRowNumber;
                        }

                        Int32 lastRow1 = rowEnd;
                        Int32 lastRow2 = rowEnd;
                        Int32 lastRow3 = rowEnd;
                        Int32 lastRow4 = rowEnd;
                        Int32 lastRow5 = rowEnd;
                        Int32 lastRow6 = rowEnd;
                        Int32 lastRow7 = rowEnd;
                        Int32 lastRow8 = rowEnd;

                        rowEnd++;

                        Dictionary<String, NodeValue> nodeNameAndValues2 = new Dictionary<String, NodeValue>();
                        Dictionary<String, NodeValue> nodeNameAndValues3 = new Dictionary<String, NodeValue>();
                        Dictionary<String, NodeValue> nodeNameAndValues4 = new Dictionary<String, NodeValue>();
                        Dictionary<String, NodeValue> nodeNameAndValues5 = new Dictionary<String, NodeValue>();
                        Dictionary<String, NodeValue> nodeNameAndValues6 = new Dictionary<String, NodeValue>();
                        Dictionary<String, NodeValue> nodeNameAndValues7 = new Dictionary<String, NodeValue>();
                        Dictionary<String, NodeValue> nodeNameAndValues8 = new Dictionary<String, NodeValue>();

                        foreach (XmlNode nd2 in nodeList)
                        {
                            // This IF statement needs to house the rest of the if-then and for loops.
                            // Verifying the ParentNode.Name against nd1.Name is vital to making sure we are not looking at invalid nodes given 
                            // the above method which just gets elements by tag name: xd.GetElementsByTagName(nodeName);
                            if (nd2.ParentNode.Name == nd1.Name)
                            {
                                //Console.WriteLine("foreach (XmlNode nd2 in nodeList) rowEnd start of loop: " + rowEnd);

                                // Find the next row end based on the lastRowNumberX values
                                if (lastRow1 > rowEnd) rowEnd = lastRow1;
                                //Console.WriteLine("if (lastRow1 > rowEnd) rowEnd = lastRow1: " + rowEnd);

                                if (lastRow2 > rowEnd) rowEnd = lastRow2;
                                //Console.WriteLine("if (lastRow2 > rowEnd) rowEnd = lastRow2: " + rowEnd);

                                if (lastRow3 > rowEnd) rowEnd = lastRow3;
                                //Console.WriteLine("if (lastRow3 > rowEnd) rowEnd = lastRow3: " + rowEnd);

                                if (lastRow4 > rowEnd) rowEnd = lastRow4;
                                //Console.WriteLine("if (lastRow4 > rowEnd) rowEnd = lastRow4: " + rowEnd);

                                if (lastRow5 > rowEnd) rowEnd = lastRow5;
                                //Console.WriteLine("if (lastRow5 > rowEnd) rowEnd = lastRow5: " + rowEnd);

                                if (lastRow6 > rowEnd) rowEnd = lastRow6;
                                //Console.WriteLine("if (lastRow6 > rowEnd) rowEnd = lastRow6: " + rowEnd);

                                if (lastRow7 > rowEnd) rowEnd = lastRow7;
                                //Console.WriteLine("if (lastRow7 > rowEnd) rowEnd = lastRow7: " + rowEnd);

                                if (lastRow8 > rowEnd) rowEnd = lastRow8;
                                //Console.WriteLine("if (lastRow8 > rowEnd) rowEnd = lastRow8: " + rowEnd);

                                //Console.WriteLine("");

                                rowStart = rowEnd;

                                if (nd2.ParentNode.Name == nd1.Name && nd2.NodeType == XmlNodeType.Element && !nodeNameAndValues2.ContainsKey(nd2.Name))
                                {
                                    // Add a new Dictionary entry if one does not exist
                                    NodeValue nValues2 = new NodeValue();
                                    nValues2.nodeName = nd2.Name;
                                    nValues2.parentNodeName = nd2.ParentNode.Name;

                                    nodeNameAndValues2.Add(nd2.Name, nValues2);
                                }
                                else if (nd2.ParentNode.Name == nd1.Name && nd2.NodeType == XmlNodeType.Text)
                                {
                                    // Usually there is nothing in here
                                }

                                foreach (XmlNode nd3 in nd2.ChildNodes)
                                {
                                    if (nd3.ParentNode.Name == nd2.Name && nd3.NodeType == XmlNodeType.Element && !nodeNameAndValues3.ContainsKey(nd3.Name))
                                    {
                                        // Add a new Dictionary entry if one does not exist
                                        NodeValue nValues3 = new NodeValue();
                                        nValues3.nodeName = nd3.Name;
                                        nValues3.parentNodeName = nd3.ParentNode.Name;

                                        nodeNameAndValues3.Add(nd3.Name, nValues3);
                                    }
                                    else if (nd3.ParentNode.Name == nd2.Name && nd3.NodeType == XmlNodeType.Text && nodeNameAndValues2.ContainsKey(nd3.ParentNode.Name))
                                    {
                                        // Add values to the Dictionary within the parent node
                                        // Ex: on nd3: Add values to nValues2[nd3.ParentNode.Name].values.Add(nd3.InnerText);
                                        nodeNameAndValues2[nd3.ParentNode.Name].nodeValues.Add(nd3.InnerText);
                                    }

                                    foreach (XmlNode nd4 in nd3.ChildNodes)
                                    {
                                        if (nd4.ParentNode.Name == nd3.Name && nd4.NodeType == XmlNodeType.Element && !nodeNameAndValues4.ContainsKey(nd4.Name))
                                        {
                                            // Add a new Dictionary entry if one does not exist
                                            NodeValue nValues4 = new NodeValue();
                                            nValues4.nodeName = nd4.Name;
                                            nValues4.parentNodeName = nd4.ParentNode.Name;

                                            nodeNameAndValues4.Add(nd4.Name, nValues4);
                                        }
                                        else if (nd4.ParentNode.Name == nd3.Name && nd4.NodeType == XmlNodeType.Text && nodeNameAndValues3.ContainsKey(nd4.ParentNode.Name))
                                        {
                                            // Add values to the Dictionary
                                            nodeNameAndValues3[nd4.ParentNode.Name].nodeValues.Add(nd4.InnerText);
                                        }

                                        foreach (XmlNode nd5 in nd4.ChildNodes)
                                        {
                                            if (nd5.ParentNode.Name == nd4.Name && nd5.NodeType == XmlNodeType.Element && !nodeNameAndValues5.ContainsKey(nd5.Name))
                                            {
                                                // Add a new Dictionary entry if one does not exist
                                                NodeValue nValues5 = new NodeValue();
                                                nValues5.nodeName = nd5.Name;
                                                nValues5.parentNodeName = nd5.ParentNode.Name;

                                                nodeNameAndValues5.Add(nd5.Name, nValues5);
                                            }
                                            else if (nd5.ParentNode.Name == nd4.Name && nd5.NodeType == XmlNodeType.Text && nodeNameAndValues4.ContainsKey(nd5.ParentNode.Name))
                                            {
                                                // Add values to the Dictionary
                                                nodeNameAndValues4[nd5.ParentNode.Name].nodeValues.Add(nd5.InnerText);
                                            }

                                            foreach (XmlNode nd6 in nd5.ChildNodes)
                                            {
                                                if (nd6.ParentNode.Name == nd5.Name && nd6.NodeType == XmlNodeType.Element && !nodeNameAndValues6.ContainsKey(nd6.Name))
                                                {
                                                    // Add a new Dictionary entry if one does not exist
                                                    NodeValue nValues6 = new NodeValue();
                                                    nValues6.nodeName = nd6.Name;
                                                    nValues6.parentNodeName = nd6.ParentNode.Name;

                                                    nodeNameAndValues6.Add(nd6.Name, nValues6);
                                                }
                                                else if (nd6.ParentNode.Name == nd5.Name && nd6.NodeType == XmlNodeType.Text && nodeNameAndValues5.ContainsKey(nd6.ParentNode.Name))
                                                {
                                                    // Add values to the Dictionary
                                                    nodeNameAndValues5[nd6.ParentNode.Name].nodeValues.Add(nd6.InnerText);
                                                }

                                                foreach (XmlNode nd7 in nd6.ChildNodes)
                                                {
                                                    if (nd7.ParentNode.Name == nd6.Name && nd7.NodeType == XmlNodeType.Element && !nodeNameAndValues7.ContainsKey(nd7.Name))
                                                    {
                                                        // Add a new Dictionary entry if one does not exist
                                                        NodeValue nValues7 = new NodeValue();
                                                        nValues7.nodeName = nd7.Name;
                                                        nValues7.parentNodeName = nd7.ParentNode.Name;

                                                        nodeNameAndValues7.Add(nd7.Name, nValues7);
                                                    }
                                                    else if (nd7.ParentNode.Name == nd6.Name && nd7.NodeType == XmlNodeType.Text && nodeNameAndValues6.ContainsKey(nd7.ParentNode.Name))
                                                    {
                                                        // Add values to the Dictionary
                                                        nodeNameAndValues6[nd7.ParentNode.Name].nodeValues.Add(nd7.InnerText);
                                                    }

                                                    foreach (XmlNode nd8 in nd7.ChildNodes)
                                                    {
                                                        if (nd8.ParentNode.Name == nd7.Name && nd8.NodeType == XmlNodeType.Element && !nodeNameAndValues8.ContainsKey(nd8.Name))
                                                        {
                                                            // Add a new Dictionary entry if one does not exist
                                                            NodeValue nValues8 = new NodeValue();
                                                            nValues8.nodeName = nd8.Name;
                                                            nValues8.parentNodeName = nd8.ParentNode.Name;

                                                            nodeNameAndValues8.Add(nd8.Name, nValues8);
                                                        }
                                                        else if (nd8.ParentNode.Name == nd7.Name && nd8.NodeType == XmlNodeType.Text && nodeNameAndValues7.ContainsKey(nd8.ParentNode.Name))
                                                        {
                                                            // Add values to the Dictionary
                                                            nodeNameAndValues7[nd8.ParentNode.Name].nodeValues.Add(nd8.InnerText);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                // [WRITE VALUES TO EXCEL]
                                // Write the values to the Excel Spreadsheet using the node values
                                // The column names should be used as the key and the column number should be retrieved through the nodeNameToColumnX dictionaries
                                // If there aren't any values in the dictionary, don't write anything. 
                                // Your rowStart / rowEnd will still be handled in the same way
                                foreach (String key2 in nodeNameAndValues2.Keys)
                                {
                                    NodeValue nodeValues2 = nodeNameAndValues2[key2];

                                    if (nodeValues2.nodeValues.Count > 0)
                                    {
                                        foreach (String ndValue in nodeValues2.nodeValues)
                                        {
                                            writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol2[key2], ndValue);
                                            addFormatToExcelRange(xlWorksheet,
                                                                rowEnd,
                                                                rowEnd,
                                                                nodeNameToCol2[key2],
                                                                nodeNameToCol2[key2],
                                                                10,
                                                                0,
                                                                0,
                                                                0,
                                                                255,
                                                                255,
                                                                255,
                                                                false,
                                                                false,
                                                                ndValue);

                                            rowEnd++;
                                            if (rowEnd > lastRow2) lastRow2 = rowEnd;
                                        }

                                        rowEnd = rowStart;
                                    }
                                }

                                foreach (String key3 in nodeNameAndValues3.Keys)
                                {
                                    NodeValue nodeValues3 = nodeNameAndValues3[key3];

                                    if (directoryName == "permissionsets"
                                        && key3 == "object")
                                    {
                                        permissionSetObjectsFocus.Add(nodeValues3.nodeValues[0]);
                                    }

                                    if (nodeValues3.nodeValues.Count > 0)
                                    {
                                        // Check on permission sets to determine if the field's object is part of the list
                                        if (directoryName == "permissionsets"
                                            && nodeValues3.parentNodeName == "fieldPermissions")
                                        {
                                            String[] objectFieldSplit = nodeNameAndValues3["field"].nodeValues[0].Split('.');
                                            if (permissionSetObjectsFocus.Contains(objectFieldSplit[0]))
                                            {
                                                foreach (String ndValue in nodeValues3.nodeValues)
                                                {
                                                    writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol3[key3], ndValue);
                                                    addFormatToExcelRange(xlWorksheet,
                                                                        rowEnd,
                                                                        rowEnd,
                                                                        nodeNameToCol3[key3],
                                                                        nodeNameToCol3[key3],
                                                                        10,
                                                                        0,
                                                                        0,
                                                                        0,
                                                                        255,
                                                                        255,
                                                                        255,
                                                                        false,
                                                                        false,
                                                                        ndValue);

                                                    //addExcelBorderToList(xlWorksheet, rowEnd, rowEnd, nodeNameToCol3[key3], colEnd - 1);

                                                    rowEnd++;
                                                    if (rowEnd > lastRow3) lastRow3 = rowEnd;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            foreach (String ndValue in nodeValues3.nodeValues)
                                            {
                                                writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol3[key3], ndValue);
                                                addFormatToExcelRange(xlWorksheet,
                                                                    rowEnd,
                                                                    rowEnd,
                                                                    nodeNameToCol3[key3],
                                                                    nodeNameToCol3[key3],
                                                                    10,
                                                                    0,
                                                                    0,
                                                                    0,
                                                                    255,
                                                                    255,
                                                                    255,
                                                                    false,
                                                                    false,
                                                                    ndValue);

                                                //addExcelBorderToList(xlWorksheet, rowEnd, rowEnd, nodeNameToCol3[key3], colEnd - 1);

                                                rowEnd++;
                                                if (rowEnd > lastRow3) lastRow3 = rowEnd;
                                            }
                                        }

                                        rowEnd = rowStart;
                                    }
                                }

                                foreach (String key4 in nodeNameAndValues4.Keys)
                                {
                                    NodeValue nodeValues4 = nodeNameAndValues4[key4];

                                    if (nodeValues4.nodeValues.Count > 0)
                                    {
                                        foreach (String ndValue in nodeValues4.nodeValues)
                                        {
                                            writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol4[key4], ndValue);
                                            addFormatToExcelRange(xlWorksheet,
                                                                rowEnd,
                                                                rowEnd,
                                                                nodeNameToCol4[key4],
                                                                nodeNameToCol4[key4],
                                                                10,
                                                                0,
                                                                0,
                                                                0,
                                                                255,
                                                                255,
                                                                255,
                                                                false,
                                                                false,
                                                                ndValue);

                                            //addExcelBorderToList(xlWorksheet, rowEnd, rowEnd, nodeNameToCol4[key4], nodeNameToCol4[key4]);

                                            rowEnd++;
                                            if (rowEnd > lastRow4) lastRow4 = rowEnd;
                                        }

                                        //applyExcelBorder(xlWorksheet, rowEnd, rowEnd, nodeNameToCol4[key4], nodeNameToCol4[key4]);
                                        rowEnd = rowStart;
                                    }
                                }

                                foreach (String key5 in nodeNameAndValues5.Keys)
                                {
                                    NodeValue nodeValues5 = nodeNameAndValues5[key5];

                                    if (nodeValues5.nodeValues.Count > 0)
                                    {
                                        foreach (String ndValue in nodeValues5.nodeValues)
                                        {
                                            writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol5[key5], ndValue);
                                            addFormatToExcelRange(xlWorksheet,
                                                                rowEnd,
                                                                rowEnd,
                                                                nodeNameToCol5[key5],
                                                                nodeNameToCol5[key5],
                                                                10,
                                                                0,
                                                                0,
                                                                0,
                                                                255,
                                                                255,
                                                                255,
                                                                false,
                                                                false,
                                                                ndValue);

                                            //addExcelBorderToList(xlWorksheet, rowEnd, rowEnd, nodeNameToCol5[key5], nodeNameToCol5[key5]);

                                            rowEnd++;
                                            if (rowEnd > lastRow5) lastRow5 = rowEnd;
                                        }

                                        rowEnd = rowStart;
                                    }
                                }

                                foreach (String key6 in nodeNameAndValues6.Keys)
                                {
                                    NodeValue nodeValues6 = nodeNameAndValues6[key6];

                                    if (nodeValues6.nodeValues.Count > 0)
                                    {
                                        foreach (String ndValue in nodeValues6.nodeValues)
                                        {
                                            writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol6[key6], ndValue);
                                            addFormatToExcelRange(xlWorksheet,
                                                                rowEnd,
                                                                rowEnd,
                                                                nodeNameToCol6[key6],
                                                                nodeNameToCol6[key6],
                                                                10,
                                                                0,
                                                                0,
                                                                0,
                                                                255,
                                                                255,
                                                                255,
                                                                false,
                                                                false,
                                                                ndValue);

                                            //addExcelBorderToList(xlWorksheet, rowEnd, rowEnd, nodeNameToCol6[key6], nodeNameToCol6[key6]);

                                            rowEnd++;
                                            if (rowEnd > lastRow6) lastRow6 = rowEnd;
                                        }

                                        rowEnd = rowStart;
                                    }
                                }

                                foreach (String key7 in nodeNameAndValues7.Keys)
                                {
                                    NodeValue nodeValues7 = nodeNameAndValues7[key7];

                                    if (nodeValues7.nodeValues.Count > 0)
                                    {
                                        foreach (String ndValue in nodeValues7.nodeValues)
                                        {
                                            writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol7[key7], ndValue);
                                            addFormatToExcelRange(xlWorksheet,
                                                                rowEnd,
                                                                rowEnd,
                                                                nodeNameToCol7[key7],
                                                                nodeNameToCol7[key7],
                                                                10,
                                                                0,
                                                                0,
                                                                0,
                                                                255,
                                                                255,
                                                                255,
                                                                false,
                                                                false,
                                                                ndValue);

                                            //addExcelBorderToList(xlWorksheet, rowEnd, rowEnd, nodeNameToCol7[key7], nodeNameToCol7[key7]);

                                            rowEnd++;
                                            if (rowEnd > lastRow7) lastRow7 = rowEnd;
                                        }

                                        rowEnd = rowStart;
                                    }
                                }

                                foreach (String key8 in nodeNameAndValues8.Keys)
                                {
                                    NodeValue nodeValues8 = nodeNameAndValues8[key8];

                                    if (nodeValues8.nodeValues.Count > 0)
                                    {
                                        foreach (String ndValue in nodeValues8.nodeValues)
                                        {
                                            writeDataToExcelSheet(xlWorksheet, rowEnd, nodeNameToCol8[key8], ndValue);
                                            addFormatToExcelRange(xlWorksheet,
                                                                rowEnd,
                                                                rowEnd,
                                                                nodeNameToCol8[key8],
                                                                nodeNameToCol8[key8],
                                                                10,
                                                                0,
                                                                0,
                                                                0,
                                                                255,
                                                                255,
                                                                255,
                                                                false,
                                                                false,
                                                                ndValue);

                                            //addExcelBorderToList(xlWorksheet, rowEnd, rowEnd, nodeNameToCol8[key8], nodeNameToCol8[key8]);

                                            rowEnd++;
                                            if (rowEnd > lastRow8) lastRow8 = rowEnd;
                                        }

                                        rowEnd = rowStart;
                                    }
                                }

                                //rowEnd = lastRowNumber;
                                nodeNameAndValues2.Clear();
                                nodeNameAndValues3.Clear();
                                nodeNameAndValues4.Clear();
                                nodeNameAndValues5.Clear();
                                nodeNameAndValues6.Clear();
                                nodeNameAndValues7.Clear();
                                nodeNameAndValues8.Clear();
                            }
                        }

                        // Reset the rowEnd, colEnd
                        if (lastRow1 > rowEnd) rowEnd = lastRow1;
                        if (lastRow2 > rowEnd) rowEnd = lastRow2;
                        if (lastRow3 > rowEnd) rowEnd = lastRow3;
                        if (lastRow4 > rowEnd) rowEnd = lastRow4;
                        if (lastRow5 > rowEnd) rowEnd = lastRow5;
                        if (lastRow6 > rowEnd) rowEnd = lastRow6;
                        if (lastRow7 > rowEnd) rowEnd = lastRow7;
                        if (lastRow8 > rowEnd) rowEnd = lastRow8;

                        if (lastRow1 > lastRowNumber) lastRowNumber = lastRow1 - 1;
                        if (lastRow2 > lastRowNumber) lastRowNumber = lastRow2 - 1;
                        if (lastRow3 > lastRowNumber) lastRowNumber = lastRow3 - 1;
                        if (lastRow4 > lastRowNumber) lastRowNumber = lastRow4 - 1;
                        if (lastRow5 > lastRowNumber) lastRowNumber = lastRow5 - 1;
                        if (lastRow6 > lastRowNumber) lastRowNumber = lastRow6 - 1;
                        if (lastRow7 > lastRowNumber) lastRowNumber = lastRow7 - 1;
                        if (lastRow8 > lastRowNumber) lastRowNumber = lastRow8 - 1;

                        rowStart = rowEnd;

                        //Console.WriteLine("foreach (XmlNode nd2 in nodeList) rowEnd end of loop: " + rowEnd);
                        //Console.WriteLine("");
                    }

                    // Clear the lists and dictionaries for the next iteration of Node2
                    columnNames1.Clear();
                    columnNames2.Clear();
                    columnNames3.Clear();
                    columnNames4.Clear();
                    columnNames5.Clear();
                    columnNames6.Clear();
                    columnNames7.Clear();
                    columnNames8.Clear();

                    nodeNameToCol2.Clear();
                    nodeNameToCol3.Clear();
                    nodeNameToCol4.Clear();
                    nodeNameToCol5.Clear();
                    nodeNameToCol6.Clear();
                    nodeNameToCol7.Clear();
                    nodeNameToCol8.Clear();
                }
            }
        }

        public void buildCSVMetadataDictionaries(String folderName)
        {
            // Get all files in the folder excluding XML files as these are related to the Metadata types
            // Get the files in that folder
            String[] files = Directory.GetFiles(folderName);
            String directoryName = directoryNameExtract(folderName);

            colNumber1 = 6;
            colNumber2 = 6;
            colNumber3 = 6;
            colNumber4 = 6;
            colNumber5 = 6;
            colNumber6 = 6;
            colNumber7 = 6;
            colNumber8 = 6;

            this.colNameToColPos1 = new Dictionary<String, Int32>();
            this.colNameToColPos2 = new Dictionary<String, Int32>();
            this.colNameToColPos3 = new Dictionary<String, Int32>();
            this.colNameToColPos4 = new Dictionary<String, Int32>();
            this.colNameToColPos5 = new Dictionary<String, Int32>();
            this.colNameToColPos6 = new Dictionary<String, Int32>();
            this.colNameToColPos7 = new Dictionary<String, Int32>();
            this.colNameToColPos8 = new Dictionary<String, Int32>();

            primaryKey1 = 1;
            primaryKey2 = 1;
            primaryKey3 = 1;
            primaryKey4 = 1;
            primaryKey5 = 1;
            primaryKey6 = 1;
            primaryKey7 = 1;
            //primaryKey8 = 1;

            node1PrimaryKey = new Dictionary<String, Int32>();
            node2PrimaryKey = new Dictionary<String, Int32>();
            node3PrimaryKey = new Dictionary<String, Int32>();
            node4PrimaryKey = new Dictionary<String, Int32>();
            node5PrimaryKey = new Dictionary<String, Int32>();
            node6PrimaryKey = new Dictionary<String, Int32>();
            node7PrimaryKey = new Dictionary<String, Int32>();
            //node8PrimaryKey = new Dictionary<String, Int32>();


            // [Get the Column Names first]
            foreach (String fileName in files)
            {
                // TODO: I can write the values after each file,
                // OR 
                // I can write all values after all of the files have been checked
                // Running through all files will ensure ALL column names will be captured
                // So, the trick then is to write the actual File Name without the extension and I'm thinking
                // the best place to put that is in the colNameToColPos1. Since some of these file names are separated by an '_', 
                // the delimiter should be '*'

                if (!fileName.EndsWith(".xml"))
                {
                    String fullObjectName = fullObjectNameExtract(@fileName);

                    XmlDocument xd = new XmlDocument();
                    Boolean isXmlDocument = true;
                    try
                    {
                        xd.Load(fileName);
                    }
                    catch (Exception e)
                    {
                        isXmlDocument = false;
                    }

                    if (isXmlDocument == false) continue;

                    foreach (XmlNode nd1 in xd.ChildNodes)
                    {
                        if (nd1.LocalName == "xml")
                        {
                            continue;
                        }
                        else if (nd1.NodeType == XmlNodeType.Element)
                        {
                            if (!colNameToColPos1.ContainsKey(nd1.Name))
                            {
                                colNameToColPos1.Add(nd1.Name, colNumber1);
                                colNumber1++;
                            }
                        }

                        foreach(XmlNode nd2 in nd1.ChildNodes)
                        {
                            // [BUILD COLUMN NAMES]
                            if (nd2.ParentNode.Name == nd1.Name && nd2.NodeType == XmlNodeType.Element)
                            {
                                if (!colNameToColPos2.ContainsKey(nd2.Name))
                                {
                                    colNameToColPos2.Add(nd2.Name, colNumber2);
                                    colNumber2++;
                                }
                            }

                            foreach (XmlNode nd3 in nd2.ChildNodes)
                            {
                                if (nd3.ParentNode.Name == nd2.Name && nd3.NodeType == XmlNodeType.Element)
                                {
                                    if (!colNameToColPos3.ContainsKey(nd3.Name))
                                    {
                                        colNameToColPos3.Add(nd3.Name, colNumber3);
                                        colNumber3++;
                                    }
                                }

                                foreach (XmlNode nd4 in nd3.ChildNodes)
                                {
                                    if (nd4.ParentNode.Name == nd3.Name && nd4.NodeType == XmlNodeType.Element)
                                    {
                                        if (!colNameToColPos4.ContainsKey(nd4.Name))
                                        {
                                            colNameToColPos4.Add(nd4.Name, colNumber4);
                                            colNumber4++;
                                        }
                                    }

                                    foreach (XmlNode nd5 in nd4.ChildNodes)
                                    {
                                        if (nd5.ParentNode.Name == nd4.Name && nd5.NodeType == XmlNodeType.Element)
                                        {
                                            if (!colNameToColPos5.ContainsKey(nd5.Name))
                                            {
                                                colNameToColPos5.Add(nd5.Name, colNumber5);
                                                colNumber5++;
                                            }
                                        }

                                        foreach (XmlNode nd6 in nd5.ChildNodes)
                                        {
                                            if (nd6.ParentNode.Name == nd5.Name && nd6.NodeType == XmlNodeType.Element)
                                            {
                                                if (!colNameToColPos6.ContainsKey(nd6.Name))
                                                {
                                                    colNameToColPos6.Add(nd6.Name, colNumber6);
                                                    colNumber6++;
                                                }
                                            }

                                            foreach (XmlNode nd7 in nd6.ChildNodes)
                                            {
                                                if (nd7.ParentNode.Name == nd6.Name && nd7.NodeType == XmlNodeType.Element)
                                                {
                                                    if (!colNameToColPos7.ContainsKey(nd7.Name))
                                                    {
                                                        colNameToColPos7.Add(nd7.Name, colNumber7);
                                                        colNumber7++;
                                                    }
                                                }

                                                foreach (XmlNode nd8 in nd7.ChildNodes)
                                                {
                                                    if (nd8.ParentNode.Name == nd7.Name && nd8.NodeType == XmlNodeType.Element)
                                                    {
                                                        if (!colNameToColPos8.ContainsKey(nd8.Name))
                                                        {
                                                            colNameToColPos8.Add(nd8.Name, colNumber8);
                                                            colNumber8++;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // [Write the Column Names in the colNameToColPosX. Include two to start called ObjectName and ObjectType
            // Name each file after the folderName (not the full path in folderName, but retrieve the directory name using directoryNameExtract method
            String headers = "\"MetadataType\",\"DirectoryName\",\"PrimaryKey\",\"ForeignKey\",\"ParentTag\",";
            foreach (String colName in colNameToColPos1.Keys)
            {
                headers = headers + "\"" + colName + "\",";
            }

            StreamWriter sw1 = new StreamWriter(this.tbSaveResultsTo.Text + "\\" + directoryName + "1.csv", false);
            sw1.Write(headers.Substring(0, headers.Length -1));
            sw1.Write(Environment.NewLine);
            sw1.Close();


            headers = "\"MetadataType\",\"DirectoryName\",\"PrimaryKey\",\"ForeignKey\",\"ParentTag\",";
            if (colNameToColPos2.Count > 0)
            {
                foreach (String colName in colNameToColPos2.Keys)
                {
                    headers = headers + "\"" + colName + "\",";
                }

                StreamWriter sw2 = new StreamWriter(this.tbSaveResultsTo.Text + "\\" + directoryName + "2.csv", false);
                sw2.Write(headers.Substring(0, headers.Length - 1));
                sw2.Write(Environment.NewLine);
                sw2.Close();
            }


            headers = "\"MetadataType\",\"DirectoryName\",\"PrimaryKey\",\"ForeignKey\",\"ParentTag\",";
            if (colNameToColPos3.Count > 0)
            {
                foreach (String colName in colNameToColPos3.Keys)
                {
                    headers = headers + "\"" + colName + "\",";
                }

                StreamWriter sw3 = new StreamWriter(this.tbSaveResultsTo.Text + "\\" + directoryName + "3.csv", false);
                sw3.Write(headers.Substring(0, headers.Length - 1));
                sw3.Write(Environment.NewLine);
                sw3.Close();
            }


            headers = "\"MetadataType\",\"DirectoryName\",\"PrimaryKey\",\"ForeignKey\",\"ParentTag\",";
            if (colNameToColPos4.Count > 0)
            {
                foreach (String colName in colNameToColPos4.Keys)
                {
                    headers = headers + "\"" + colName + "\",";
                }

                StreamWriter sw4 = new StreamWriter(this.tbSaveResultsTo.Text + "\\" + directoryName + "4.csv", false);
                sw4.Write(headers.Substring(0, headers.Length - 1));
                sw4.Write(Environment.NewLine);
                sw4.Close();
            }


            headers = "\"MetadataType\",\"DirectoryName\",\"PrimaryKey\",\"ForeignKey\",\"ParentTag\",";
            if (colNameToColPos5.Count > 0)
            {
                foreach (String colName in colNameToColPos5.Keys)
                {
                    headers = headers + "\"" + colName + "\",";
                }

                StreamWriter sw5 = new StreamWriter(this.tbSaveResultsTo.Text + "\\" + directoryName + "5.csv", false);
                sw5.Write(headers.Substring(0, headers.Length - 1));
                sw5.Write(Environment.NewLine);
                sw5.Close();
            }


            headers = "\"MetadataType\",\"DirectoryName\",\"PrimaryKey\",\"ForeignKey\",\"ParentTag\",";
            if (colNameToColPos6.Count > 0)
            {
                foreach (String colName in colNameToColPos6.Keys)
                {
                    headers = headers + "\"" + colName + "\",";
                }

                StreamWriter sw6 = new StreamWriter(this.tbSaveResultsTo.Text + "\\" + directoryName + "6.csv", false);
                sw6.Write(headers.Substring(0, headers.Length - 1));
                sw6.Write(Environment.NewLine);
                sw6.Close();
            }


            headers = "\"MetadataType\",\"DirectoryName\",\"PrimaryKey\",\"ForeignKey\",\"ParentTag\",";
            if (colNameToColPos7.Count > 0)
            {
                foreach (String colName in colNameToColPos7.Keys)
                {
                    headers = headers + "\"" + colName + "\",";
                }

                StreamWriter sw7 = new StreamWriter(this.tbSaveResultsTo.Text + "\\" + directoryName + "7.csv", false);
                sw7.Write(headers.Substring(0, headers.Length - 1));
                sw7.Write(Environment.NewLine);
                sw7.Close();
            }


            headers = "\"MetadataType\",\"DirectoryName\",\"PrimaryKey\",\"ForeignKey\",\"ParentTag\",";
            if (colNameToColPos8.Count > 0)
            {
                foreach (String colName in colNameToColPos8.Keys)
                {
                    headers = headers + "\"" + colName + "\",";
                }

                StreamWriter sw8 = new StreamWriter(this.tbSaveResultsTo.Text + "\\" + directoryName + "8.csv", false);
                sw8.Write(headers.Substring(0, headers.Length - 1));
                sw8.Write(Environment.NewLine);
                sw8.Close();
            }


            // [WRITE the InnerText values]
            foreach (String fileName in files)
            {
                if (!fileName.EndsWith(".xml"))
                {
                    String fullObjectName = fullObjectNameExtract(@fileName);

                    XmlDocument xd = new XmlDocument();
                    Boolean isXmlDocument = true;
                    try
                    {
                        xd.Load(fileName);
                    }
                    catch (Exception e)
                    {
                        isXmlDocument = false;
                    }

                    if (isXmlDocument == false) continue;

                    // Write InnerText values from Node 1
                    //writeInnerTextValuesNode1(xd, fullObjectName, folderName);

                    // Write InnerText values from Node 2
                    //writeInnerTextValuesNode2(xd, fullObjectName, directoryName);

                    // Write InnerText values from Node 3
                    writeInnerTextValuesNode3(xd, fullObjectName, directoryName);

                    // Write InnerText values from Node 4
                    writeInnerTextValuesNode4(xd, fullObjectName, directoryName);

                    // Write InnerText values from Node 5
                    writeInnerTextValuesNode5(xd, fullObjectName, directoryName);

                    // Write InnerText values from Node 6
                    writeInnerTextValuesNode6(xd, fullObjectName, directoryName);

                    // Write InnerText values from Node 7
                    writeInnerTextValuesNode7(xd, fullObjectName, directoryName);

                    // Write InnerText values from Node 8
                    writeInnerTextValuesNode8(xd, fullObjectName, directoryName);
                }
            }
        }

        private String checkObjectNameLength(String folderName, String objName, Dictionary<String, Int32> fileNames)
        {
            //String combinedName = folderName + "_" + objName;

            String combinedName = objName;
            String shortName = "";

            if (combinedName.Length > 30)
            {
                shortName = combinedName.Substring(0, 28);
            }
            else
            {
                shortName = combinedName;
            }

            if (fileNames.ContainsKey(shortName.ToLower()))
            {
                fileNames[shortName.ToLower()] = fileNames[shortName.ToLower()] + 1;
                shortName = shortName + "_" + fileNames[shortName.ToLower()].ToString();
            }
            else
            {
                fileNames.Add(shortName.ToLower(), 1);
            }

            return shortName;
        }

        private List<String> getObjectTopFieldNames(String directoryName)
        {
            List<String> xmlTopNodeNames = new List<String>();

            if (directoryName == "accountRelationshipShareRules")
            {
                List<MetadataDifferenceProcessing.MetadataFieldTypes> mftList = MetadataDifferenceProcessing.accountRelationshipShareRuleFieldNames();

                foreach(MetadataDifferenceProcessing.MetadataFieldTypes mft in mftList)
                {
                    xmlTopNodeNames.Add(mft.fieldName);
                }

                //xmlTopNodeNames = MetadataDifferenceProcessing.accountRelationshipShareRuleFieldNames();
            }
            else if (directoryName == "actionLinkGroupTemplates")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.actionLinkGroupTemplateFieldNames();
            }
            else if (directoryName == "actionPlanTemplates")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.actionPlanTemplateFieldNames();
            }
            else if (directoryName == "analyticSnapshots")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.analyticSnapshotFieldNames();
            }
            else if (directoryName == "animationRules")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.animationRuleFieldNames();
            }
            else if (directoryName == "classes")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.apexClassFieldNames();
            }
            else if (directoryName == "applications")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.customApplicationFieldNames();
            }
            else if (directoryName == "appMenus")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.appMenuFieldNames();
            }
            else if (directoryName == "appointmentSchedulingPolicies")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.appointmentSchedulingPolicyFieldNames();
            }
            else if (directoryName == "approvalProcesses")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.approvalProcessFieldNames();
            }
            else if (directoryName == "assignmentRules")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.assignmentRulesFieldNames();
            }
            else if (directoryName == "audience")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.audienceFieldNames();
            }
            else if (directoryName == "aura")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.auraDefinitionBundleFieldNames();
            }
            else if (directoryName == "authproviders")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.authProviderFieldNames();
            }
            else if (directoryName == "autoResponseRules")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.autoResponseRulesFieldNames();
            }
            else if (directoryName == "bot")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.botFieldNames();
            }
            else if (directoryName == "brandingSets")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.brandingSetFieldNames();
            }
            else if (directoryName == "cachePartitions")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.platformCachePartitionFieldNames();
            }
            else if (directoryName == "callCenters")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.callCenterFieldNames();
            }
            else if (directoryName == "campaignInfluenceModels")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.campaignInfluenceModelFieldNames();
            }
            else if (directoryName == "CaseSubjectParticles")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.caseSubjectParticleFieldNames();
            }
            else if (directoryName == "certs")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.certificateFieldNames();
            }
            else if (directoryName == "cleanDataServices")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.cleanDataServiceFieldNames();
            }
            else if (directoryName == "cmsConnectSource")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.CMSConnectSourceFieldNames();
            }
            else if (directoryName == "communities")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.communityFieldNames();
            }
            else if (directoryName == "communityTemplateDefinitions")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.communityTemplateDefinitionFieldNames();
            }
            else if (directoryName == "communityThemeDefinitions")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.communityThemeDefinitionFieldNames();
            }
            else if (directoryName == "components")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.apexComponentFieldNames();
            }
            else if (directoryName == "connectedApps")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.connectedAppFieldNames();
            }
            else if (directoryName == "contentassets")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.contentAssetFieldNames();
            }
            else if (directoryName == "corsWhitelistOrigins")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.corsWhitelistOriginFieldNames();
            }
            else if (directoryName == "cspTrustedSites")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.cspTrustedSiteFieldNames();
            }
            else if (directoryName == "customApplicationComponents")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.customApplicationFieldNames();
            }
            else if (directoryName == "customHelpMenuSections")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.customHelpMenuSectionFieldNames();
            }
            else if (directoryName == "customMetadata")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.customMetadataFieldNames();
            }
            else if (directoryName == "customPermissions")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.customPermissionFieldNames();
            }
            else if (directoryName == "dashboards")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.dashboardFieldNames();
            }
            else if (directoryName == "datacategorygroups")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.dataCategoryGroupFieldNames();
            }
            else if (directoryName == "dataSources")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.externalDataSourceFieldNames();
            }
            else if (directoryName == "delegateGroups")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.delegateGroupFieldNames();
            }
            else if (directoryName == "document")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.documentFieldNames();
            }
            else if (directoryName == "duplicateRules")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.duplicateRuleFieldNames();
            }
            else if (directoryName == "eclair")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.eclairGeoDataFieldNames();
            }
            else if (directoryName == "email")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.emailTemplateFieldNames();
            }
            else if (directoryName == "emailservices")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.emailServicesFunctionFieldNames();
            }
            else if (directoryName == "EmbeddedServiceBranding")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.embeddedServiceBrandingFieldNames();
            }
            else if (directoryName == "EmbeddedServiceConfig")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.embeddedServiceConfigFieldNames();
            }
            else if (directoryName == "EmbeddedServiceFieldService")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.embeddedServiceFieldServiceFieldNames();
            }
            else if (directoryName == "EmbeddedServiceFlowConfig")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.embeddedServiceFlowConfigFieldNames();
            }
            else if (directoryName == "EmbeddedServiceLiveAgent")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.embeddedServiceLiveAgentFieldNames();
            }
            else if (directoryName == "EmbeddedServiceMenuSettings")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.embeddedServiceMenuSettingsFieldNames();
            }
            else if (directoryName == "entitlementProcesses")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.entitlementProcessFieldNames();
            }
            else if (directoryName == "entitlementTemplates")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.entitlementTemplateFieldNames();
            }
            else if (directoryName == "escalationRules")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.escalationRulesFieldNames();
            }
            else if (directoryName == "eventDeliveries")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.eventDeliveryFieldNames();
            }
            else if (directoryName == "eventSubscriptions")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.eventSubscriptionFieldNames();
            }
            else if (directoryName == "experiences")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.experienceBundleFieldNames();
            }
            else if (directoryName == "externalServiceRegistrations")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.externalServiceRegistrationFieldNames();
            }
            else if (directoryName == "featureParameters")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.featureParameterFieldNames();
            }
            else if (directoryName == "feedFilters")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.customFeedFilterFieldNames();
            }
            else if (directoryName == "flexipages")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.flexiPageFieldNames();
            }
            else if (directoryName == "flows")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.flowFieldNames();
            }
            else if (directoryName == "flowCategories")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.flowCategoryFieldNames();
            }
            else if (directoryName == "flowDefinitions")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.flowDefinitionFieldNames();
            }
            else if (directoryName == "globalPicklist")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.globalPicklistFieldNames();
            }
            else if (directoryName == "globalValueSets")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.globalValueSetFieldNames();
            }
            else if (directoryName == "globalValueSetTranslations")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.globalValueSetTranslationFieldNames();
            }
            else if (directoryName == "groups")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.groupFieldNames();
            }
            else if (directoryName == "homepagecomponents")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.homePageComponentFieldNames();
            }
            else if (directoryName == "homePageLayouts")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.homePageLayoutFieldNames();
            }
            else if (directoryName == "installedPackages")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.installedPackageFieldNames();
            }
            else if (directoryName == "layouts")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.layoutFieldNames();
            }
            else if (directoryName == "letterhead")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.letterheadFieldNames();
            }
            else if (directoryName == "lightningBolts")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.lightningBoltFieldNames();
            }
            else if (directoryName == "lightningExperienceThemes")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.lightningExperienceThemeFieldNames();
            }
            else if (directoryName == "liveChatAgentConfigs")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.liveChatAgentConfigFieldNames();
            }
            else if (directoryName == "liveChatButtons")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.liveChatButtonFieldNames();
            }
            else if (directoryName == "liveChatDeployments")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.liveChatDeploymentFieldNames();
            }
            else if (directoryName == "liveChatSensitiveDataRule")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.liveChatSensitiveDataRuleFieldNames();
            }
            else if (directoryName == "managedContentTypes")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.managedContentTypeFieldNames();
            }
            else if (directoryName == "managedTopics")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.managedTopicsFieldNames();
            }
            else if (directoryName == "matchingRules")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.matchingRuleFieldNames();
            }
            else if (directoryName == "messageChannels")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.lightningMessageChannelFieldNames();
            }
            else if (directoryName == "milestoneTypes")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.milestoneTypeFieldNames();
            }
            else if (directoryName == "mlDomains")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.mlDomainFieldNames();
            }
            else if (directoryName == "MobileApplicationDetails")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.mobileApplicationDetailFieldNames();
            }
            else if (directoryName == "moderation")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.moderationRuleFieldNames();
            }
            else if (directoryName == "myDomainDiscoverableLogins")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.myDomainDiscoverableLoginFieldNames();
            }
            else if (directoryName == "namedCredentials")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.namedCredentialFieldNames();
            }
            else if (directoryName == "navigationMenus")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.navigationMenuFieldNames();
            }
            else if (directoryName == "networkBranding")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.networkBrandingFieldNames();
            }
            else if (directoryName == "networks")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.networkFieldNames();
            }
            else if (directoryName == "notificationTypeConfig")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.notificationTypeConfigFieldNames();
            }
            else if (directoryName == "notificationtypes")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.customNotificationTypeFieldNames();
            }
            else if (directoryName == "oauthcustomscopes")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.oauthCustomScopeFieldNames();
            }
            else if (directoryName == "objects")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.customObjectFieldNames();
            }
            else if (directoryName == "objectTranslations")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.customObjectTranslationFieldNames();
            }
            else if (directoryName == "pages")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.apexPageFieldNames();
            }
            else if (directoryName == "pathAssistants")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.pathAssistantFieldNames();
            }
            else if (directoryName == "paymentGatewayProviders")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.paymentGatewayProviderFieldNames();
            }
            else if (directoryName == "permissionsetgroups")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.permissionSetGroupFieldNames();
            }
            else if (directoryName == "permissionsets")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.profilePermissionSetFieldNames();
            }
            else if (directoryName == "platformEventChannelMembers")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.platformEventChannelMemberFieldNames();
            }
            else if (directoryName == "platformEventChannels")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.platformEventChannelFieldNames();
            }
            else if (directoryName == "portals")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.portalFieldNames();
            }
            else if (directoryName == "postTemplates")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.postTemplateFieldNames();
            }
            else if (directoryName == "presenceDeclineReasons")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.presenceDeclineReasonFieldNames();
            }
            else if (directoryName == "presenceUserConfigs")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.presenceUserConfigFieldNames();
            }
            else if (directoryName == "profilePasswordPolicies")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.profilePasswordPolicyFieldNames();
            }
            else if (directoryName == "profiles")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.profilePermissionSetFieldNames();
            }
            else if (directoryName == "profileSessionSettings")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.profileSessionSettingFieldNames();
            }
            else if (directoryName == "prompts")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.promptFieldNames();
            }
            else if (directoryName == "queueRoutingConfigs")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.queueRoutingConfigFieldNames();
            }
            else if (directoryName == "queues")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.queueFieldNames();
            }
            else if (directoryName == "quickActions")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.quickActionFieldNames();
            }
            else if (directoryName == "recommendationStrategies")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.recommendationStrategyFieldNames();
            }
            else if (directoryName == "recordActionDeployments")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.recordActionDeploymentFieldNames();
            }
            else if (directoryName == "redirectWhitelistUrls")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.redirectWhitelistUrlFieldNames();
            }
            else if (directoryName == "remoteSiteSettings")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.remoteSiteSettingFieldNames();
            }
            else if (directoryName == "reports")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.reportFieldNames();
            }
            else if (directoryName == "reportTypes")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.reportTypeFieldNames();
            }
            else if (directoryName == "roles")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.roleFieldNames();
            }
            else if (directoryName == "samlssoconfigs")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.samlSsoConfigFieldNames();
            }
            else if (directoryName == "scontrols")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.scontrolFieldNames();
            }
            else if (directoryName == "serviceChannels")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.serviceChannelFieldNames();
            }
            else if (directoryName == "servicePresenceStatuses")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.servicePresenceStatusFieldNames();
            }
            //else if (directoryName == "settings")
            //{
            //    xmlTopNodeNames = MetadataDifferenceProcessing.settingsFieldNames();
            //}
            else if (directoryName == "sharingRules")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.sharingRulesFieldNames();
            }
            else if (directoryName == "sharingSets")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.sharingSetFieldNames();
            }
            else if (directoryName == "siteDotComSites")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.siteDotComFieldNames();
            }
            else if (directoryName == "sites")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.customSiteFieldNames();
            }
            else if (directoryName == "skills")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.skillFieldNames();
            }
            else if (directoryName == "standardValueSets")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.standardValueSetFieldNames();
            }
            else if (directoryName == "standardValueSetTranslations")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.standardValueSetTranslationFieldNames();
            }
            else if (directoryName == "staticresources")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.staticResourceFieldNames();
            }
            else if (directoryName == "synonymDictionaries")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.synonymDictionaryFieldNames();
            }
            else if (directoryName == "tabs")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.customTabFieldNames();
            }
            else if (directoryName == "territories")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.territoryFieldNames();
            }
            else if (directoryName == "territory2Models")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.territory2ModelFieldNames();
            }
            else if (directoryName == "territory2Types")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.territory2TypeFieldNames();
            }
            //else if (directoryName == "testSuites")
            //{

            //}
            else if (directoryName == "timeSheetTemplates")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.timeSheetTemplateFieldNames();
            }
            else if (directoryName == "topicsForObjects")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.topicsForObjectsFieldNames();
            }
            else if (directoryName == "transactionSecurityPolicies")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.transactionSecurityPolicyFieldNames();
            }
            else if (directoryName == "translations")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.translationsFieldNames();
            }
            else if (directoryName == "triggers")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.apexTriggerFieldNames();
            }
            else if (directoryName == "UserCriteria")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.userCriteriaFieldNames();
            }
            else if (directoryName == "wave")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.waveApplicationFieldNames();
            }
            else if (directoryName == "waveTemplates")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.waveTemplateBundleFieldNames();
            }
            else if (directoryName == "weblinks")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.customPageWebLinkFieldNames();
            }
            else if (directoryName == "workflows")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.workflowFieldNames();
            }
            else if (directoryName == "workSkillRoutings")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.workSkillRoutingFieldNames();
            }

            return xmlTopNodeNames;
        }


        private List<String> getObjectTopFieldNames(String directoryName, String fileName)
        {
            Dictionary <String, String> xmlTopNodeNames = new Dictionary<String, String>();

            XmlDocument xd = new XmlDocument();
            xd.Load(fileName);

            foreach (XmlNode nd in xd.ChildNodes)
            {
                foreach (XmlNode nd2 in nd.ChildNodes)
                {
                    if (!xmlTopNodeNames.ContainsKey(nd2.Name))
                    {
                        xmlTopNodeNames.Add(nd2.Name, nd2.Name);
                    }
                }
            }


            return xmlTopNodeNames.Keys.ToList<String>();
        }

        public void writeDataToExcelSheet(Microsoft.Office.Interop.Excel.Worksheet xlWorksheet,
                                          Int32 rowNumber,
                                          Int32 colNumber,
                                          String value)
        {
            xlWorksheet.Cells[rowNumber, colNumber].Value = value;
            ((Microsoft.Office.Interop.Excel.Range)xlWorksheet.Columns[colNumber]).AutoFit();
        }


        //public void addExcelBorderToList(Microsoft.Office.Interop.Excel.Worksheet xlWorksheet,
        //                             Int32 startRowNumber,
        //                             Int32 endRowNumber,
        //                             Int32 startColNumber,
        //                             Int32 endColNumber)
        //{
        //    ExcelBorderFormat ebf = new ExcelBorderFormat();
        //    ebf.xlWorksheet = xlWorksheet;
        //    ebf.startRowNumber = startRowNumber;
        //    ebf.endRowNumber = endRowNumber;
        //    ebf.startColNumber = startColNumber;
        //    ebf.endColNumber = endColNumber;

        //    excelBorderFormatList.Add(ebf);
        //}


        public void addFormatToExcelRange(Microsoft.Office.Interop.Excel.Worksheet xlWorksheet,
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
            ExcelRangeFormat erf = new ExcelRangeFormat();
            erf.xlWorksheet = xlWorksheet;
            erf.startRowNumber = startRowNumber;
            erf.endRowNumber = endRowNumber;
            erf.startColNumber = startColNumber;
            erf.endColNumber = endColNumber;
            erf.fontSize = fontSize;
            erf.fontColorRed = fontColorRed;
            erf.fontColorGreen = fontColorGreen;
            erf.fontColorBlue = fontColorBlue;
            erf.interiorColorRed = interiorColorRed;
            erf.interiorColorGreen = interiorColorGreen;
            erf.interiorColorBlue = interiorColorBlue;
            erf.boldText = boldText;
            erf.italicText = italicText;
            erf.fieldValues = fieldValues;

            excelRangeFormatList.Add(erf);
        }

        //public void applyExcelBorder(ExcelBorderFormat ebf)
        //{
        //    Microsoft.Office.Interop.Excel.Range rng;
        //    rng = ebf.xlWorksheet.Range[ebf.xlWorksheet.Cells[ebf.startRowNumber, ebf.startColNumber], ebf.xlWorksheet.Cells[ebf.endRowNumber, ebf.endColNumber]];

        //    Microsoft.Office.Interop.Excel.Borders border = rng.Borders;
        //    border[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //    border[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //    border[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //    border[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

        //    border.Color = ColorTranslator.ToOle(Color.DarkGray);
        //}

        public void applyExcelRangeFormats(ExcelRangeFormat erf)
        {
            Microsoft.Office.Interop.Excel.Range rng;
            rng = erf.xlWorksheet.Range[erf.xlWorksheet.Cells[erf.startRowNumber, erf.startColNumber], erf.xlWorksheet.Cells[erf.endRowNumber, erf.endColNumber]];
            rng.Font.Bold = erf.boldText;
            rng.Font.Italic = erf.italicText;
            rng.Font.Size = erf.fontSize;

            //rng.Font.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbFloralWhite;
            rng.Font.Color = System.Drawing.Color.FromArgb(erf.fontColorRed, erf.fontColorGreen, erf.fontColorBlue);

            if (erf.fieldValues.ToLower() == "true")
            {
                rng.Interior.Color = System.Drawing.Color.FromArgb(220, 230, 241);
            }
            else if (erf.fieldValues.ToLower() == "false")
            {
                rng.Interior.Color = System.Drawing.Color.FromArgb(250, 191, 143);
            }
            else
            {
                rng.Interior.Color = System.Drawing.Color.FromArgb(erf.interiorColorRed, erf.interiorColorGreen, erf.interiorColorBlue);
            }
        }


        public Int32 getColumnEndValue(Int32 currColEndValue, Int32 c1, Int32 c2, Int32 c3,
                                       Dictionary<String, List<String>> columnNames4,
                                       Dictionary<String, List<String>> columnNames5,
                                       Dictionary<String, List<String>> columnNames6,
                                       Dictionary<String, List<String>> columnNames7,
                                       Dictionary<String, List<String>> columnNames8)
        {
            Int32 newColumnEndValue = currColEndValue;

            if (c1 > 0) newColumnEndValue = newColumnEndValue + c1;
            if (c2 > 0) newColumnEndValue = newColumnEndValue + c2;
            if (c3 > 0) newColumnEndValue = newColumnEndValue = c3 + 1;

            foreach (String key4 in columnNames4.Keys)
            {
                List<String> colNames = columnNames4[key4];
                newColumnEndValue = newColumnEndValue + colNames.Count;
            }

            foreach (String key5 in columnNames5.Keys)
            {
                List<String> colNames = columnNames5[key5];
                newColumnEndValue = newColumnEndValue + colNames.Count;
            }

            foreach (String key6 in columnNames6.Keys)
            {
                List<String> colNames = columnNames6[key6];
                newColumnEndValue = newColumnEndValue + colNames.Count;
            }

            foreach (String key7 in columnNames7.Keys)
            {
                List<String> colNames = columnNames7[key7];
                newColumnEndValue = newColumnEndValue + colNames.Count;
            }

            foreach (String key8 in columnNames8.Keys)
            {
                List<String> colNames = columnNames8[key8];
                newColumnEndValue = newColumnEndValue + colNames.Count;
            }

            return newColumnEndValue;
        }

        private void writeInnerTextValuesNode3(XmlDocument xd, String fullObjectName, String directoryName)
        {
            Int32 parentKey = 0;

            foreach (XmlNode nd1 in xd.ChildNodes)
            {
                String result = "";
                SortedDictionary<Int32, NodeValue> colAndValues = new SortedDictionary<Int32, NodeValue>();

                foreach (XmlNode nd2 in nd1.ChildNodes)
                {
                    foreach (XmlNode nd3 in nd2.ChildNodes)
                    {
                        // Has the nd2.Name changed? If so, it means we are on a different segment and need to write the values
                        // and reset the parentNodeName and result values.
                        if (nd3.NodeType == XmlNodeType.Text)
                        {
                            // Get the Column Position of the current Node
                            Int32 colPos = this.colNameToColPos2[nd3.ParentNode.Name];

                            if (colAndValues.ContainsKey(colPos))
                            {
                                colAndValues[colPos].nodeValues.Add(nd3.InnerText);
                            }
                            else
                            {
                                parentKey = getParentPK(fullObjectName, directoryName, nd1.Name);

                                NodeValue nv = new NodeValue();
                                nv.foreignKey = parentKey;
                                nv.parentNodeName = nd1.Name;
                                nv.nodeValues.Add(nd3.InnerText);

                                colAndValues.Add(colPos, nv);
                            }
                        }
                    }
                }

                // Build the values and Write the result to the file
                // Use the SortedDictionary key to get the column position
                // TODO: Figure out how to retreive the Foreign Key
                if (colAndValues.Count > 0)
                {
                    StreamWriter swChildRecord = getFileStream(directoryName, "2.csv");

                    result = "\"" + fullObjectName + "\",\"" + directoryName + "\",\"" + primaryKey2.ToString() + "\",\"" + parentKey.ToString() + "\",\"" + nd1.Name + "\",";

                    Int32 lastColumn = 6;
                    foreach (Int32 colPos in colAndValues.Keys)
                    {
                        if (lastColumn < colPos)
                        {
                            for (Int32 i = lastColumn; i < colPos; i++)
                            {
                                result = result + "\"\",";
                            }

                            result = result + "\"";
                            foreach (String value in colAndValues[colPos].nodeValues)
                            {
                                result = result + value + ",";
                            }

                            result = result.Substring(0, result.Length - 1);
                            result = result + "\",";
                            lastColumn = colPos + 1;
                        }
                        else
                        {
                            result = result + "\"";
                            foreach (String value in colAndValues[colPos].nodeValues)
                            {
                                result = result + value + ",";
                            }

                            result = result.Substring(0, result.Length - 1);
                            result = result + "\",";
                            lastColumn++;
                        }
                    }

                    swChildRecord.WriteLine(result.Substring(0, result.Length - 1));
                    swChildRecord.Close();

                    primaryKey2++;
                }
            }
        }

        private void writeInnerTextValuesNode4(XmlDocument xd, String fullObjectName, String directoryName)
        {
            Int32 parentKey = 0;

            foreach (XmlNode nd1 in xd.ChildNodes)
            {
                foreach (XmlNode nd2 in nd1.ChildNodes)
                {
                    String result = "";

                    SortedDictionary<Int32, NodeValue> colAndValues = new SortedDictionary<Int32, NodeValue>();

                    foreach (XmlNode nd3 in nd2.ChildNodes)
                    {
                        foreach (XmlNode nd4 in nd3.ChildNodes)
                        {
                            // Has the nd2.Name changed? If so, it means we are on a different segment and need to write the values
                            // and reset the parentNodeName and result values.
                            if (nd4.NodeType == XmlNodeType.Text)
                            {
                                // Get the Column Position of the current Node
                                Int32 colPos = this.colNameToColPos3[nd4.ParentNode.Name];

                                if (colAndValues.ContainsKey(colPos))
                                {
                                    colAndValues[colPos].nodeValues.Add(nd4.InnerText);
                                }
                                else
                                {
                                    parentKey = getParentPK(fullObjectName, directoryName, nd1.Name, nd2.Name);

                                    NodeValue nv = new NodeValue();
                                    nv.foreignKey = parentKey;
                                    nv.parentNodeName = nd2.Name;
                                    nv.nodeValues.Add(nd4.InnerText);

                                    colAndValues.Add(colPos, nv);
                                }
                            }
                        }
                    }

                    // Build the values and Write the result to the file
                    // Use the SortedDictionary key to get the column position
                    // TODO: Figure out how to retreive the Foreign Key
                    if (colAndValues.Count > 0)
                    {
                        StreamWriter swChildRecord = getFileStream(directoryName, "3.csv");

                        result = "\"" + fullObjectName + "\",\"" + directoryName + "\",\"" + primaryKey3.ToString() + "\",\"" + parentKey.ToString() + "\",\"" + nd2.Name + "\",";

                        Int32 lastColumn = 6;
                        foreach (Int32 colPos in colAndValues.Keys)
                        {
                            if (lastColumn < colPos)
                            {
                                for (Int32 i = lastColumn; i < colPos; i++)
                                {
                                    result = result + "\"\",";
                                }

                                result = result + "\"";
                                foreach (String value in colAndValues[colPos].nodeValues)
                                {
                                    result = result + value + ",";
                                }

                                result = result.Substring(0, result.Length - 1);
                                result = result + "\",";
                                lastColumn = colPos + 1;
                            }
                            else
                            {
                                result = result + "\"";
                                foreach (String value in colAndValues[colPos].nodeValues)
                                {
                                    result = result + value + ",";
                                }

                                result = result.Substring(0, result.Length - 1);
                                result = result + "\",";
                                lastColumn++;
                            }
                        }

                        swChildRecord.WriteLine(result.Substring(0, result.Length - 1));
                        swChildRecord.Close();

                        primaryKey3++;
                    }
                }
            }
        }

        private void writeInnerTextValuesNode5(XmlDocument xd, String fullObjectName, String directoryName)
        {
            Int32 parentKey = 0;

            foreach (XmlNode nd1 in xd.ChildNodes)
            {
                foreach (XmlNode nd2 in nd1.ChildNodes)
                {
                    foreach (XmlNode nd3 in nd2.ChildNodes)
                    {
                        String result = "";
                        SortedDictionary<Int32, NodeValue> colAndValues = new SortedDictionary<Int32, NodeValue>();

                        foreach (XmlNode nd4 in nd3.ChildNodes)
                        {
                            foreach (XmlNode nd5 in nd4.ChildNodes)
                            {
                                // Has the nd2.Name changed? If so, it means we are on a different segment and need to write the values
                                // and reset the parentNodeName and result values.
                                if (nd5.NodeType == XmlNodeType.Text)
                                {
                                    // Get the Column Position of the current Node
                                    Int32 colPos = this.colNameToColPos4[nd5.ParentNode.Name];

                                    if (colAndValues.ContainsKey(colPos))
                                    {
                                        colAndValues[colPos].nodeValues.Add(nd5.InnerText);
                                    }
                                    else
                                    {
                                        parentKey = getParentPK(fullObjectName, directoryName, nd1.Name, nd2.Name, nd3.Name);

                                        NodeValue nv = new NodeValue();
                                        nv.foreignKey = parentKey;
                                        nv.parentNodeName = nd3.Name;
                                        nv.nodeValues.Add(nd5.InnerText);

                                        colAndValues.Add(colPos, nv);
                                    }
                                }
                            }
                        }


                        // Build the values and Write the result to the file
                        // Use the SortedDictionary key to get the column position
                        // TODO: Figure out how to retreive the Foreign Key
                        if (colAndValues.Count > 0)
                        {
                            StreamWriter swChildRecord = getFileStream(directoryName, "4.csv");
                            result = "\"" + fullObjectName + "\",\"" + directoryName + "\",\"" + primaryKey4.ToString() + "\",\"" + parentKey.ToString() + "\",\"" + nd3.Name + "\",";

                            Int32 lastColumn = 6;
                            foreach (Int32 colPos in colAndValues.Keys)
                            {
                                if (lastColumn < colPos)
                                {
                                    for (Int32 i = lastColumn; i < colPos; i++)
                                    {
                                        result = result + "\"\",";
                                    }

                                    result = result + "\"";
                                    foreach (String value in colAndValues[colPos].nodeValues)
                                    {
                                        result = result + value + ",";
                                    }

                                    result = result.Substring(0, result.Length - 1);
                                    result = result + "\",";
                                    lastColumn = colPos + 1;
                                }
                                else
                                {
                                    result = result + "\"";
                                    foreach (String value in colAndValues[colPos].nodeValues)
                                    {
                                        result = result + value + ",";
                                    }

                                    result = result.Substring(0, result.Length - 1);
                                    result = result + "\",";
                                    lastColumn++;
                                }
                            }

                            swChildRecord.WriteLine(result.Substring(0, result.Length - 1));
                            swChildRecord.Close();

                            primaryKey4++;
                        }
                    }
                }
            }
        }

        private void writeInnerTextValuesNode6(XmlDocument xd, String fullObjectName, String directoryName)
        {
            Int32 parentKey = 0;

            foreach (XmlNode nd1 in xd.ChildNodes)
            {
                foreach (XmlNode nd2 in nd1.ChildNodes)
                {
                    foreach (XmlNode nd3 in nd2.ChildNodes)
                    {
                        foreach (XmlNode nd4 in nd3.ChildNodes)
                        {
                            String result = "";
                            SortedDictionary<Int32, NodeValue> colAndValues = new SortedDictionary<Int32, NodeValue>();

                            foreach (XmlNode nd5 in nd4.ChildNodes)
                            {
                                foreach (XmlNode nd6 in nd5.ChildNodes)
                                {
                                    if (nd6.NodeType == XmlNodeType.Text)
                                    {
                                        // Get the Column Position of the current Node
                                        Int32 colPos = this.colNameToColPos5[nd6.ParentNode.Name];

                                        if (colAndValues.ContainsKey(colPos))
                                        {
                                            colAndValues[colPos].nodeValues.Add(nd6.InnerText);
                                        }
                                        else
                                        {
                                            parentKey = getParentPK(fullObjectName, directoryName, nd1.Name, nd2.Name, nd3.Name, nd4.Name);

                                            NodeValue nv = new NodeValue();
                                            nv.foreignKey = parentKey;
                                            nv.parentNodeName = nd4.Name;
                                            nv.nodeValues.Add(nd6.InnerText);

                                            colAndValues.Add(colPos, nv);
                                        }
                                    }
                                }
                            }

                            if (colAndValues.Count > 0)
                            {
                                StreamWriter swChildRecord = getFileStream(directoryName, "5.csv");
                                result = "\"" + fullObjectName + "\",\"" + directoryName + "\",\"" + primaryKey5.ToString() + "\",\"" + parentKey.ToString() + "\",\"" + nd4.Name + "\",";

                                Int32 lastColumn = 6;
                                foreach (Int32 colPos in colAndValues.Keys)
                                {
                                    if (lastColumn < colPos)
                                    {
                                        for (Int32 i = lastColumn; i < colPos; i++)
                                        {
                                            result = result + "\"\",";
                                        }

                                        result = result + "\"";
                                        foreach (String value in colAndValues[colPos].nodeValues)
                                        {
                                            result = result + value + ",";
                                        }

                                        result = result.Substring(0, result.Length - 1);
                                        result = result + "\",";
                                        lastColumn = colPos + 1;
                                    }
                                    else
                                    {
                                        result = result + "\"";
                                        foreach (String value in colAndValues[colPos].nodeValues)
                                        {
                                            result = result + value + ",";
                                        }

                                        result = result.Substring(0, result.Length - 1);
                                        result = result + "\",";
                                        lastColumn++;
                                    }
                                }

                                swChildRecord.WriteLine(result.Substring(0, result.Length - 1));
                                swChildRecord.Close();

                                primaryKey5++;
                            }

                        }
                    }
                }
            }
        }

        private void writeInnerTextValuesNode7(XmlDocument xd, String fullObjectName, String directoryName)
        {
            Int32 parentKey = 0;

            foreach (XmlNode nd1 in xd.ChildNodes)
            {
                foreach (XmlNode nd2 in nd1.ChildNodes)
                {
                    foreach (XmlNode nd3 in nd2.ChildNodes)
                    {
                        foreach (XmlNode nd4 in nd3.ChildNodes)
                        {
                            foreach (XmlNode nd5 in nd4.ChildNodes)
                            {
                                String result = "";
                                SortedDictionary<Int32, NodeValue> colAndValues = new SortedDictionary<Int32, NodeValue>();

                                foreach (XmlNode nd6 in nd5.ChildNodes)
                                {
                                    foreach (XmlNode nd7 in nd6.ChildNodes)
                                    {
                                        if (nd7.NodeType == XmlNodeType.Text)
                                        {
                                            // Get the Column Position of the current Node
                                            Int32 colPos = this.colNameToColPos6[nd7.ParentNode.Name];

                                            if (colAndValues.ContainsKey(colPos))
                                            {
                                                colAndValues[colPos].nodeValues.Add(nd7.InnerText);
                                            }
                                            else
                                            {
                                                parentKey = getParentPK(fullObjectName, directoryName, nd1.Name, nd2.Name, nd3.Name, nd4.Name, nd5.Name);

                                                NodeValue nv = new NodeValue();
                                                nv.foreignKey = parentKey;
                                                nv.parentNodeName = nd5.Name;
                                                nv.nodeValues.Add(nd7.InnerText);

                                                colAndValues.Add(colPos, nv);
                                            }
                                        }
                                    }
                                }

                                if (colAndValues.Count > 0)
                                {
                                    StreamWriter swChildRecord = getFileStream(directoryName, "6.csv");

                                    result = "\"" + fullObjectName + "\",\"" + directoryName + "\",\"" + primaryKey6.ToString() + "\",\"" + parentKey.ToString() + "\",\"" + nd5.Name + "\",";

                                    Int32 lastColumn = 6;
                                    foreach (Int32 colPos in colAndValues.Keys)
                                    {
                                        if (lastColumn < colPos)
                                        {
                                            for (Int32 i = lastColumn; i < colPos; i++)
                                            {
                                                result = result + "\"\",";
                                            }

                                            result = result + "\"";
                                            foreach (String value in colAndValues[colPos].nodeValues)
                                            {
                                                result = result + value + ",";
                                            }

                                            result = result.Substring(0, result.Length - 1);
                                            result = result + "\",";
                                            lastColumn = colPos + 1;
                                        }
                                        else
                                        {
                                            result = result + "\"";
                                            foreach (String value in colAndValues[colPos].nodeValues)
                                            {
                                                result = result + value + ",";
                                            }

                                            result = result.Substring(0, result.Length - 1);
                                            result = result + "\",";
                                            lastColumn++;
                                        }
                                    }

                                    swChildRecord.WriteLine(result.Substring(0, result.Length - 1));
                                    swChildRecord.Close();

                                    primaryKey6++;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void writeInnerTextValuesNode8(XmlDocument xd, String fullObjectName, String directoryName)
        {
            Int32 parentKey = 0;

            foreach (XmlNode nd1 in xd.ChildNodes)
            {
                foreach (XmlNode nd2 in nd1.ChildNodes)
                {
                    foreach (XmlNode nd3 in nd2.ChildNodes)
                    {
                        foreach (XmlNode nd4 in nd3.ChildNodes)
                        {
                            foreach (XmlNode nd5 in nd4.ChildNodes)
                            {
                                foreach (XmlNode nd6 in nd5.ChildNodes)
                                {
                                    String result = "";
                                    SortedDictionary<Int32, NodeValue> colAndValues = new SortedDictionary<Int32, NodeValue>();

                                    foreach (XmlNode nd7 in nd6.ChildNodes)
                                    {
                                        foreach (XmlNode nd8 in nd7.ChildNodes)
                                        {
                                            if (nd8.NodeType == XmlNodeType.Text)
                                            {
                                                //Console.WriteLine("Node 8 is Text type");

                                                // Get the Column Position of the current Node
                                                Int32 colPos = this.colNameToColPos7[nd8.ParentNode.Name];

                                                if (colAndValues.ContainsKey(colPos))
                                                {
                                                    colAndValues[colPos].nodeValues.Add(nd7.InnerText);
                                                }
                                                else
                                                {
                                                    parentKey = getParentPK(fullObjectName, directoryName, nd1.Name, nd2.Name, nd3.Name, nd4.Name, nd5.Name, nd6.Name);

                                                    NodeValue nv = new NodeValue();
                                                    nv.foreignKey = parentKey;
                                                    nv.parentNodeName = nd6.Name;
                                                    nv.nodeValues.Add(nd8.InnerText);

                                                    colAndValues.Add(colPos, nv);
                                                }
                                            }
                                        }
                                    }

                                    if (colAndValues.Count > 0)
                                    {
                                        StreamWriter swChildRecord = getFileStream(directoryName, "7.csv");

                                        result = "\"" + fullObjectName + "\",\"" + directoryName + "\",\"" + primaryKey7.ToString() + "\",\"" + parentKey.ToString() + "\",\"" + nd6.Name + "\",";

                                        Int32 lastColumn = 6;
                                        foreach (Int32 colPos in colAndValues.Keys)
                                        {
                                            if (lastColumn < colPos)
                                            {
                                                for (Int32 i = lastColumn; i < colPos; i++)
                                                {
                                                    result = result + "\"\",";
                                                }

                                                result = result + "\"";
                                                foreach (String value in colAndValues[colPos].nodeValues)
                                                {
                                                    result = result + value + ",";
                                                }

                                                result = result.Substring(0, result.Length - 1);
                                                result = result + "\",";
                                                lastColumn = colPos + 1;
                                            }
                                            else
                                            {
                                                result = result + "\"";
                                                foreach (String value in colAndValues[colPos].nodeValues)
                                                {
                                                    result = result + value + ",";
                                                }

                                                result = result.Substring(0, result.Length - 1);
                                                result = result + "\",";
                                                lastColumn++;
                                            }
                                        }

                                        swChildRecord.WriteLine(result.Substring(0, result.Length - 1));
                                        swChildRecord.Close();

                                        primaryKey7++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private StreamWriter getFileStream(String directoryName, String fileName)
        {
            StreamWriter sw;

            if (File.Exists(this.tbSaveResultsTo.Text + "\\" + directoryName + fileName))
            {
                sw = new StreamWriter(this.tbSaveResultsTo.Text + "\\" + directoryName + fileName, true);
            }
            else
            {
                sw = new StreamWriter(this.tbSaveResultsTo.Text + "\\" + directoryName + fileName, true);
                sw.WriteLine("\"ObjectName\",\"ObjectType\",\"PrimaryKey\",\"ForeignKey\",\"ParentTag\"");
            }

            return sw;
        }

        private Int32 getParentPK(String fullObjectName, String directoryName, String nodeName1)
        {
            Int32 pk1 = 0;

            if (node1PrimaryKey.ContainsKey(fullObjectName + "_" + nodeName1))
            {
                pk1 = node1PrimaryKey[fullObjectName + "_" + nodeName1];
            }
            else
            {
                StreamWriter swParent = new StreamWriter(this.tbSaveResultsTo.Text + "\\" + directoryName + "1.csv", true);
                swParent.WriteLine("\"" + fullObjectName + "\",\"" + directoryName + "\",\"" + primaryKey1.ToString() + "\",\"0\",\"" + nodeName1 + "\"");
                swParent.Close();

                node1PrimaryKey.Add(fullObjectName + "_" + nodeName1, primaryKey1);
                pk1 = primaryKey1;
                primaryKey1++;
            }

            return pk1;
        }

        private Int32 getParentPK(String fullObjectName, String directoryName, String nodeName1, String nodeName2)
        {
            Int32 pk1 = 0;
            Int32 pk2 = 0;

            if (!node1PrimaryKey.ContainsKey(fullObjectName + "_" + nodeName1))
            {
                pk1 = getParentPK(fullObjectName, directoryName, nodeName1);
            }
            else
            {
                pk1 = node1PrimaryKey[fullObjectName + "_" + nodeName1];
            }

            if (node2PrimaryKey.ContainsKey(fullObjectName + "_" + nodeName1 + "_" + nodeName2))
            {
                pk2 = node2PrimaryKey[fullObjectName + "_" + nodeName1 + "_" + nodeName2];
            }
            else
            {
                StreamWriter swParent = new StreamWriter(this.tbSaveResultsTo.Text + "\\" + directoryName + "2.csv", true);
                swParent.WriteLine("\"" + fullObjectName + "\",\"" + directoryName + "\",\"" + primaryKey2.ToString() + "\",\"" + pk1.ToString() + "\",\"" + nodeName2 + "\"");
                swParent.Close();

                node2PrimaryKey.Add(fullObjectName + "_" + nodeName1 + "_" + nodeName2, primaryKey2);
                pk2 = primaryKey2;
                primaryKey2++;
            }

            return pk2;
        }

        private Int32 getParentPK(String fullObjectName, String directoryName, String nodeName1, String nodeName2, String nodeName3)
        {
            Int32 pk2 = 0;
            Int32 pk3 = 0;

            if (!node2PrimaryKey.ContainsKey(fullObjectName + "_" + nodeName1 + "_" + nodeName2))
            {
                pk2 = getParentPK(fullObjectName, directoryName, nodeName1, nodeName2);
            }
            else
            {
                pk2 = node2PrimaryKey[fullObjectName + "_" + nodeName1 + "_" + nodeName2];
            }

            if (node3PrimaryKey.ContainsKey(fullObjectName + "_" + nodeName1 + "_" + nodeName2 + "_" + nodeName3))
            {
                pk3 = node3PrimaryKey[fullObjectName + "_" + nodeName1 + "_" + nodeName2 + "_" + nodeName3];
            }
            else
            {
                StreamWriter swParent = new StreamWriter(this.tbSaveResultsTo.Text + "\\" + directoryName + "3.csv", true);
                swParent.WriteLine("\"" + fullObjectName + "\",\"" + directoryName + "\",\"" + primaryKey3.ToString() + "\",\"" + pk2.ToString() + "\",\"" + nodeName3 + "\"");
                swParent.Close();

                node3PrimaryKey.Add(fullObjectName + "_" + nodeName1 + "_" + nodeName2 + "_" + nodeName3, primaryKey3);
                pk3 = primaryKey3;
                primaryKey3++;
            }

            return pk3;
        }

        private Int32 getParentPK(String fullObjectName, String directoryName, String nodeName1, String nodeName2, String nodeName3, String nodeName4)
        {
            Int32 pk3 = 0;
            Int32 pk4 = 0;

            if (!node3PrimaryKey.ContainsKey(fullObjectName + "_" + nodeName1 + "_" + nodeName2 + "_" + nodeName3))
            {
                pk3 = getParentPK(fullObjectName, directoryName, nodeName1, nodeName2, nodeName3);
            }
            else
            {
                pk3 = node3PrimaryKey[fullObjectName + "_" + nodeName1 + "_" + nodeName2 + "_" + nodeName3];
            }

            if (node4PrimaryKey.ContainsKey(fullObjectName + "_" + nodeName1 + "_" + nodeName2 + "_" + nodeName3 + "_" + nodeName4))
            {
                pk4 = node4PrimaryKey[fullObjectName + "_" + nodeName1 + "_" + nodeName2 + "_" + nodeName3 + "_" + nodeName4];
            }
            else
            {
                StreamWriter swParent = new StreamWriter(this.tbSaveResultsTo.Text + "\\" + directoryName + "4.csv", true);
                swParent.WriteLine("\"" + fullObjectName + "\",\"" + directoryName + "\",\"" + primaryKey4.ToString() + "\",\"" + pk3.ToString() + "\",\"" + nodeName4 + "\"");
                swParent.Close();

                node4PrimaryKey.Add(fullObjectName + "_" + nodeName1 + "_" + nodeName2 + "_" + nodeName3 + "_" + nodeName4, primaryKey4);
                pk4 = primaryKey4;
                primaryKey4++;
            }

            return pk4;
        }

        private Int32 getParentPK(String fullObjectName, String directoryName, String nodeName1, String nodeName2, String nodeName3, String nodeName4, String nodeName5)
        {
            Int32 pk4 = 0;
            Int32 pk5 = 0;

            if (!node4PrimaryKey.ContainsKey(fullObjectName + "_" + nodeName1 + "_" + nodeName2 + "_" + nodeName3 + "_" + nodeName4))
            {
                pk4 = getParentPK(fullObjectName, directoryName, nodeName1, nodeName2, nodeName3 + "_" + nodeName4);
            }
            else
            {
                pk4 = node4PrimaryKey[fullObjectName + "_" + nodeName1 + "_" + nodeName2 + "_" + nodeName3 + "_" + nodeName4];
            }

            if (node5PrimaryKey.ContainsKey(fullObjectName + "_" + nodeName1 + "_" + nodeName2 + "_" + nodeName3 + "_" + nodeName4 + "_" + nodeName5))
            {
                pk5 = node5PrimaryKey[fullObjectName + "_" + nodeName1 + "_" + nodeName2 + "_" + nodeName3 + "_" + nodeName4 + "_" + nodeName5];
            }
            else
            {
                StreamWriter swParent = new StreamWriter(this.tbSaveResultsTo.Text + "\\" + directoryName + "5.csv", true);
                swParent.WriteLine("\"" + fullObjectName + "\",\"" + directoryName + "\",\"" + primaryKey5.ToString() + "\",\"" + pk4.ToString() + "\",\"" + nodeName5 + "\"");
                swParent.Close();

                node5PrimaryKey.Add(fullObjectName + "_" + nodeName1 + "_" + nodeName2 + "_" + nodeName3 + "_" + nodeName4 + "_" + nodeName5, primaryKey5);
                pk5 = primaryKey5;
                primaryKey5++;
            }

            return pk5;
        }

        private Int32 getParentPK(String fullObjectName, String directoryName, String nodeName1, String nodeName2, String nodeName3, String nodeName4, String nodeName5, String nodeName6)
        {
            Int32 pk5 = 0;
            Int32 pk6 = 0;

            if (!node5PrimaryKey.ContainsKey(fullObjectName + "_" + nodeName1 + "_" + nodeName2 + "_" + nodeName3 + "_" + nodeName4 + "_" + nodeName5))
            {
                pk5 = getParentPK(fullObjectName, directoryName, nodeName1, nodeName2, nodeName3 + "_" + nodeName4 + "_" + nodeName5);
            }
            else
            {
                pk5 = node5PrimaryKey[fullObjectName + "_" + nodeName1 + "_" + nodeName2 + "_" + nodeName3 + "_" + nodeName4 + "_" + nodeName5];
            }

            if (node6PrimaryKey.ContainsKey(fullObjectName + "_" + nodeName1 + "_" + nodeName2 + "_" + nodeName3 + "_" + nodeName4 + "_" + nodeName5 + "_" + nodeName6))
            {
                pk6 = node6PrimaryKey[fullObjectName + "_" + nodeName1 + "_" + nodeName2 + "_" + nodeName3 + "_" + nodeName4 + "_" + nodeName5 + "_" + nodeName6];
            }
            else
            {
                StreamWriter swParent = new StreamWriter(this.tbSaveResultsTo.Text + "\\" + directoryName + "6.csv", true);
                swParent.WriteLine("\"" + fullObjectName + "\",\"" + directoryName + "\",\"" + primaryKey6.ToString() + "\",\"" + pk5.ToString() + "\",\"" + nodeName6 + "\"");
                swParent.Close();

                node6PrimaryKey.Add(fullObjectName + "_" + nodeName1 + "_" + nodeName2 + "_" + nodeName3 + "_" + nodeName4 + "_" + nodeName5 + "_" + nodeName6, primaryKey6);
                pk6 = primaryKey6;
                primaryKey6++;
            }

            return pk6;
        }


        // [INNER Classes]
        // Single file processing in a folder per Excel Tab
        public class NodeValue
        {
            public Int32 primaryKey;
            public Int32 foreignKey;
            public String parentNodeName;
            public String nodeName;
            public List<String> nodeValues;

            public NodeValue()
            {
                nodeValues = new List<string>();
            }
        }


        public class ObjectNameAndColumn
        {
            public String objectName;
            public String objectType;
            public Int32 colNumber;
            public String nodeName;
        }


        public class ExcelBorderFormat
        {
            public Microsoft.Office.Interop.Excel.Worksheet xlWorksheet;
            public Int32 startRowNumber;
            public Int32 endRowNumber;
            public Int32 startColNumber;
            public Int32 endColNumber;
        }

        public class ExcelRangeFormat
        {
            public Microsoft.Office.Interop.Excel.Worksheet xlWorksheet;
            public Int32 startRowNumber;
            public Int32 endRowNumber;
            public Int32 startColNumber;
            public Int32 endColNumber;
            public Int32 fontSize;
            public Int32 fontColorRed;
            public Int32 fontColorGreen;
            public Int32 fontColorBlue;
            public Int32 interiorColorRed;
            public Int32 interiorColorGreen;
            public Int32 interiorColorBlue;
            public Boolean boldText;
            public Boolean italicText;
            public String fieldValues;
        }

        private void btnObjectAutomationMap_Click(object sender, EventArgs e)
        {
            if (tbSelectedFolder.Text == null || tbSelectedFolder.Text == "")
            {
                MessageBox.Show("Please select a folder with the metadata to parse");
                return;
            }

            Dictionary<String, String> sfClassNameFilePath = new Dictionary<String, String>();
            Dictionary<String, String> sfFlowNameFilePath = new Dictionary<String, String>();
            Dictionary<String, String> sfSobjectNameFilePath = new Dictionary<String, String>();
            Dictionary<String, String> sfTriggerNameFilePath = new Dictionary<String, String>();


            // Key = object name, 
            Dictionary<String, List<SFFlowValues>> objectToFlows = new Dictionary<String, List<SFFlowValues>>();
            Dictionary<String, List<SFTriggerValues>> objectToTrigger = new Dictionary<String, List<SFTriggerValues>>();

            // Automation to consider:
            // approvalProcesses
            // assignmentRules
            // autoResponseRules
            // * classes
            // escalationRules
            // * flows
            // lwc
            // * objects
            // queueRoutingConfigs
            // queues
            // quickActions
            // skills
            // * triggers
            // workflows
            // workSkillRoutings

            if (Directory.Exists(this.tbSelectedFolder.Text + "\\classes")
                && Directory.Exists(this.tbSelectedFolder.Text + "\\flows")
                && Directory.Exists(this.tbSelectedFolder.Text + "\\triggers"))
            {
                SalesforceCredentials.fromOrgUsername = this.cmbUserName.Text;
                SalesforceCredentials.fromOrgPassword = this.tbPassword.Text;
                SalesforceCredentials.fromOrgSecurityToken = this.tbSecurityToken.Text;

                Boolean loginSuccess = SalesforceCredentials.salesforceToolingLogin();

                if (loginSuccess == false)
                {
                    MessageBox.Show("Please check username, password and/or security token");
                    return;
                }


                String[] classesFiles = Directory.GetFiles(this.tbSelectedFolder.Text + "\\classes");
                String[] flowsFiles = Directory.GetFiles(this.tbSelectedFolder.Text + "\\flows");
                String[] triggersFiles = Directory.GetFiles(this.tbSelectedFolder.Text + "\\triggers");


                // BEGIN: Populate the customObjIdToName Map from the Tooling API to be used with the Apex Trigger Tooling API
                Dictionary<String, String> customObjIdToName = new Dictionary<String, String>();

                SalesforceMetadata.ToolingWSDL.QueryResult toolingQr;
                SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

                String objectQuery = ToolingApiHelper.CustomObjectQuery();
                toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
                toolingQr = SalesforceCredentials.fromOrgToolingSvc.query(objectQuery);
                toolingRecords = toolingQr.records;

                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SalesforceMetadata.ToolingWSDL.CustomObject1 customObj = (SalesforceMetadata.ToolingWSDL.CustomObject1)toolingRecord;
                    customObjIdToName.Add(customObj.Id, customObj.DeveloperName);
                }
                // END: Populate the customObjIdToName Map from the Tooling API to be used with the Apex Trigger Tooling API


                // BEGIN: Class information
                foreach (String fileName in classesFiles)
                {
                    String[] parsedFilePath = fileName.Split('\\');
                    String[] parsedFileName = parsedFilePath[parsedFilePath.Length - 1].Split('.');

                    if (parsedFileName.Length == 2)
                    {
                        sfClassNameFilePath.Add(parsedFileName[0], fileName);
                    }
                }

                foreach (String apexClassName in sfClassNameFilePath.Keys)
                {
                    // Use the Tooling ApexClass as this contains the Symbol Table

                }
                // END: Class information


                foreach (String fileName in flowsFiles)
                {
                    String[] parsedFilePath = fileName.Split('\\');
                    String[] parsedFileName = parsedFilePath[parsedFilePath.Length - 1].Split('.');

                    sfFlowNameFilePath.Add(parsedFileName[0], fileName);
                }


                // BEGIN: Trigger information
                foreach (String fileName in triggersFiles)
                {
                    String[] parsedFilePath = fileName.Split('\\');
                    String[] parsedFileName = parsedFilePath[parsedFilePath.Length - 1].Split('.');

                    if (parsedFileName.Length == 2)
                    {
                        sfTriggerNameFilePath.Add(parsedFileName[0], fileName);
                    }
                }


                // Use the Tooling ApexTriggerMember as this contains the Symbol Table
                String apexTriggerQuery = ToolingApiHelper.ApexTriggerQuery("");
                toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
                toolingQr = SalesforceCredentials.fromOrgToolingSvc.query(apexTriggerQuery);

                toolingRecords = toolingQr.records;

                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SFTriggerValues sfTriggerValueObj = new SFTriggerValues();

                    SalesforceMetadata.ToolingWSDL.ApexTrigger1 apexTrigger = (SalesforceMetadata.ToolingWSDL.ApexTrigger1)toolingRecord;

                    if (apexTrigger.NamespacePrefix != null)
                    {
                        sfTriggerValueObj.namespacePrefix = apexTrigger.NamespacePrefix;
                    }

                    if (apexTrigger.EntityDefinitionId != null && customObjIdToName.ContainsKey(apexTrigger.EntityDefinitionId))
                    {
                        sfTriggerValueObj.objectName = customObjIdToName[apexTrigger.EntityDefinitionId];
                    }
                    else if (apexTrigger.TableEnumOrId != null && customObjIdToName.ContainsKey(apexTrigger.TableEnumOrId))
                    {
                        sfTriggerValueObj.objectName = customObjIdToName[apexTrigger.TableEnumOrId];
                    }
                    else if (apexTrigger.TableEnumOrId != null)
                    {
                        sfTriggerValueObj.objectName = apexTrigger.TableEnumOrId;
                    }
                    else
                    {
                        sfTriggerValueObj.objectName = apexTrigger.TableEnumOrId;
                    }


                    sfTriggerValueObj.triggerName        = apexTrigger.Name;
                    sfTriggerValueObj.entityDefinitionId = apexTrigger.EntityDefinitionId;
                    sfTriggerValueObj.usageAfterDelete   = (Boolean)apexTrigger.UsageAfterDelete;
                    sfTriggerValueObj.usageAfterInsert   = (Boolean)apexTrigger.UsageAfterInsert;
                    sfTriggerValueObj.usageAfterUndelete = (Boolean)apexTrigger.UsageAfterUndelete;
                    sfTriggerValueObj.usageAfterUpdate   = (Boolean)apexTrigger.UsageAfterUpdate;
                    sfTriggerValueObj.usageBeforeDelete  = (Boolean)apexTrigger.UsageBeforeDelete;
                    sfTriggerValueObj.usageBeforeInsert  = (Boolean)apexTrigger.UsageBeforeInsert;
                    sfTriggerValueObj.usageBeforeUpdate  = (Boolean)apexTrigger.UsageBeforeUpdate;
                    sfTriggerValueObj.usageIsBulk        = (Boolean)apexTrigger.UsageIsBulk;


                    if (objectToTrigger.ContainsKey(sfTriggerValueObj.objectName))
                    {
                        objectToTrigger[sfTriggerValueObj.objectName].Add(sfTriggerValueObj);
                    }
                    else
                    {
                        objectToTrigger.Add(sfTriggerValueObj.objectName, new List<SFTriggerValues> { sfTriggerValueObj });
                    }
                }
                // END: Trigger information



                foreach (String flowName in sfFlowNameFilePath.Keys)
                {
                    XmlDocument xd = new XmlDocument();
                    xd.Load(sfFlowNameFilePath[flowName]);

                    XmlNodeList screenNodes = xd.GetElementsByTagName("screens");
                    if (screenNodes.Count > 0) continue;

                    XmlNodeList xnListStatus = xd.GetElementsByTagName("status");

                    foreach (XmlNode xnStatus in xnListStatus)
                    {
                        if (xnStatus.ParentNode.Name == "Flow"
                            && xnStatus.InnerText == "Active")
                        {
                            XmlNodeList xnListProcType = xd.GetElementsByTagName("processType");

                            SFFlowValues sfFlowValue = new SFFlowValues();

                            foreach (XmlNode xnProcType in xnListProcType)
                            {
                                Boolean addFlowToDictionary = false;

                                if (xnProcType.ParentNode.Name == "Flow"
                                    && xnProcType.InnerText == "AutoLaunchedFlow")
                                {
                                    sfFlowValue = populateAutolaunchedFlowValues(xd, flowName);
                                    addFlowToDictionary = true;
                                }
                                else if (xnProcType.ParentNode.Name == "Flow"
                                        && xnProcType.InnerText == "Workflow")
                                {
                                    sfFlowValue = populateWorkflowFlowValues(xd, flowName);
                                    addFlowToDictionary = true;
                                }

                                if (addFlowToDictionary == false) continue;

                                // Add the flow inner class object to the Dictionary
                                if (sfFlowValue.objectName == null
                                    && sfFlowValue.triggerType == null
                                    && objectToFlows.ContainsKey("AutolaunchedNoObject"))
                                {
                                    objectToFlows["AutolaunchedNoObject"].Add(sfFlowValue);
                                }
                                else if (sfFlowValue.objectName == null
                                         && sfFlowValue.triggerType == null)
                                {
                                    objectToFlows.Add("AutolaunchedNoObject", new List<SFFlowValues> { sfFlowValue });
                                }
                                else if (sfFlowValue.objectName == null
                                    && sfFlowValue.triggerType == "Scheduled"
                                    && objectToFlows.ContainsKey("ScheduledTriggerFlow"))
                                {
                                    objectToFlows["ScheduledTriggerFlow"].Add(sfFlowValue);
                                }
                                else if (sfFlowValue.objectName == null
                                         && sfFlowValue.triggerType == "Scheduled")
                                {
                                    objectToFlows.Add("ScheduledTriggerFlow", new List<SFFlowValues> { sfFlowValue });
                                }
                                else if (objectToFlows.ContainsKey(sfFlowValue.objectName))
                                {
                                    objectToFlows[sfFlowValue.objectName].Add(sfFlowValue);
                                }
                                else
                                {
                                    objectToFlows.Add(sfFlowValue.objectName, new List<SFFlowValues> { sfFlowValue });
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("The directory objects does not exist in the path provided. Please choose another directory location or retrieve the objects directory from the SalesforceMetadataStep1 process.");
            }


            // Write the values to the Excel File
            if (objectToFlows.Count > 0)
            {
                writeProcessAutomationToExcel(objectToFlows, objectToTrigger);
            }
        }

        public SFFlowValues populateAutolaunchedFlowValues(XmlDocument flowDocument, String flowName)
        {
            SFFlowValues sfFlowValues = new SFFlowValues();

            sfFlowValues.flowName = flowName;

            XmlNodeList nodeList = flowDocument.GetElementsByTagName("start");
            foreach (XmlNode nd in nodeList)
            {
                if (nd.ParentNode.Name == "Flow")
                {
                    foreach (XmlNode nd2 in nd.ChildNodes)
                    {
                        if (nd2.Name == "object")
                        {
                            sfFlowValues.objectName = nd2.InnerText;
                        }
                        else if (nd2.Name == "recordTriggerType")
                        {
                            sfFlowValues.recordTriggerType = nd2.InnerText;
                        }
                        else if (nd2.Name == "triggerType")
                        {
                            sfFlowValues.triggerType = nd2.InnerText;
                        }
                    }
                }
            }

            // Determine the Process Type of the Flow
            if (sfFlowValues.objectName == null && sfFlowValues.triggerType == null)
            {
                sfFlowValues.processType = "Autolaunched Flow (No Trigger)";
            }
            else if (sfFlowValues.objectName == null && sfFlowValues.triggerType == "Scheduled")
            {
                sfFlowValues.processType = "Schedule-Triggered Flow";
            }
            else if (sfFlowValues.triggerType == "PlatformEvent")
            {
                sfFlowValues.processType = "Platform Event-Triggered Flow";
            }
            else if (sfFlowValues.objectName != null)
            {
                sfFlowValues.processType = "Record-Triggered Flow";
            }


            nodeList = flowDocument.GetElementsByTagName("runInMode");
            if (nodeList.Count == 0)
            {
                sfFlowValues.runInMode = "DefaultMode";
            }
            else
            {
                foreach (XmlNode nd in nodeList)
                {
                    if (nd.ParentNode.Name == "Flow")
                    {
                        sfFlowValues.runInMode = nd.InnerText;
                    }
                }
            }

            nodeList = flowDocument.GetElementsByTagName("variables");
            foreach (XmlNode nd in nodeList)
            {
                if (nd.ParentNode.Name == "Flow")
                {
                    String variableName = "";
                    String objectName = "";

                    foreach (XmlNode nd2 in nd.ChildNodes)
                    {
                        if (nd2.Name == "name")
                        {
                            variableName = nd2.InnerText;
                        }
                        else if (nd2.Name == "objectType")
                        {
                            objectName = nd2.InnerText;
                        }
                    }

                    if (objectName != "")
                    {
                        sfFlowValues.varNameToObject.Add(variableName, objectName);
                    }
                }
            }

            nodeList = flowDocument.GetElementsByTagName("recordLookups");
            foreach (XmlNode nd in nodeList)
            {
                if (nd.ParentNode.Name == "Flow")
                {
                    String recordLookupName = "";
                    String objectName = "";

                    foreach (XmlNode nd2 in nd.ChildNodes)
                    {
                        if (nd2.Name == "name")
                        {
                            recordLookupName = nd2.InnerText;
                        }
                        else if (nd2.Name == "object")
                        {
                            objectName = nd2.InnerText;
                        }
                    }

                    if (objectName != "")
                    {
                        sfFlowValues.recordLookupVarToObject.Add(recordLookupName, objectName);
                    }
                }
            }

            nodeList = flowDocument.GetElementsByTagName("recordCreates");
            foreach (XmlNode nd in nodeList)
            {
                if (nd.ParentNode.Name == "Flow")
                {
                    String elementName = "";
                    String objectName = "";

                    // Check if the object exits or is a reference to a varialbe which should be populated
                    foreach (XmlNode nd2 in nd.ChildNodes)
                    {
                        if (nd2.Name == "name")
                        {
                            elementName = nd2.InnerText;
                        }
                        else if (nd2.Name == "object")
                        {
                            objectName = nd2.InnerText;
                        }
                        else if (nd2.Name == "inputReference"
                                && sfFlowValues.varNameToObject.ContainsKey(nd2.InnerText))
                        {
                            objectName = sfFlowValues.varNameToObject[nd2.InnerText];
                        }
                        else if (nd2.Name == "inputReference"
                                && sfFlowValues.recordLookupVarToObject.ContainsKey(nd2.InnerText))
                        {
                            objectName = sfFlowValues.recordLookupVarToObject[nd2.InnerText];
                        }
                    }

                    if (objectName != "")
                    {
                        if (sfFlowValues.objectToCreateUpdate.ContainsKey(objectName))
                        {
                            if (sfFlowValues.objectToCreateUpdate[objectName].ContainsKey("recordCreates"))
                            {
                                sfFlowValues.objectToCreateUpdate[objectName]["recordCreates"].Add(elementName);
                            }
                            else
                            {
                                sfFlowValues.objectToCreateUpdate[objectName].Add("recordCreates", new List<String> { elementName });
                            }
                        }
                        else
                        {
                            sfFlowValues.objectToCreateUpdate.Add(objectName, new Dictionary<String, List<String>>());
                            sfFlowValues.objectToCreateUpdate[objectName].Add("recordCreates", new List<string> { elementName });
                        }
                    }
                }
            }

            nodeList = flowDocument.GetElementsByTagName("recordUpdates");
            foreach (XmlNode nd in nodeList)
            {
                if (nd.ParentNode.Name == "Flow")
                {
                    String elementName = "";
                    String objectName = "";

                    // Check if the object exits or is a reference to a varialbe which should be populated
                    foreach (XmlNode nd2 in nd.ChildNodes)
                    {
                        if (nd2.Name == "name")
                        {
                            elementName = nd2.InnerText;
                        }
                        else if (nd2.Name == "object")
                        {
                            objectName = nd2.InnerText;
                        }
                        else if (nd2.Name == "inputReference"
                                && sfFlowValues.varNameToObject.ContainsKey(nd2.InnerText))
                        {
                            objectName = sfFlowValues.varNameToObject[nd2.InnerText];
                        }
                        else if (nd2.Name == "inputReference"
                                && sfFlowValues.recordLookupVarToObject.ContainsKey(nd2.InnerText))
                        {
                            objectName = sfFlowValues.recordLookupVarToObject[nd2.InnerText];
                        }
                    }

                    if (objectName != "")
                    {
                        if (sfFlowValues.objectToCreateUpdate.ContainsKey(objectName))
                        {
                            if (sfFlowValues.objectToCreateUpdate[objectName].ContainsKey("recordUpdates"))
                            {
                                sfFlowValues.objectToCreateUpdate[objectName]["recordUpdates"].Add(elementName);
                            }
                            else
                            {
                                sfFlowValues.objectToCreateUpdate[objectName].Add("recordUpdates", new List<String> { elementName });
                            }
                        }
                        else
                        {
                            sfFlowValues.objectToCreateUpdate.Add(objectName, new Dictionary<String, List<String>>());
                            sfFlowValues.objectToCreateUpdate[objectName].Add("recordUpdates", new List<string> { elementName });
                        }
                    }
                }
            }

            return sfFlowValues;
        }

        public SFFlowValues populateWorkflowFlowValues(XmlDocument flowDocument, String flowName)
        {
            SFFlowValues sfFlowValues = new SFFlowValues();

            sfFlowValues.processType = "ProcessBuilder";
            sfFlowValues.flowName = flowName;

            XmlNodeList nodeList = flowDocument.GetElementsByTagName("variables");
            foreach (XmlNode nd in nodeList)
            {
                if (nd.ParentNode.Name == "Flow")
                {
                    String variableName = "";
                    String objectName = "";

                    foreach (XmlNode nd2 in nd.ChildNodes)
                    {
                        if (nd2.Name == "name")
                        {
                            variableName = nd2.InnerText;
                        }
                        else if (nd2.Name == "objectType")
                        {
                            objectName = nd2.InnerText;
                        }
                    }

                    if (variableName == "myVariable_current"
                        && objectName != "")
                    {
                        sfFlowValues.objectName = objectName;
                    }
                    else if(objectName != "")
                    {
                        sfFlowValues.varNameToObject.Add(variableName, objectName);
                    }
                }
            }

            nodeList = flowDocument.GetElementsByTagName("processMetadataValues");
            foreach (XmlNode nd in nodeList)
            {
                Boolean getTriggerType = false;
                foreach (XmlNode nd2 in nd.ChildNodes)
                {
                    if (nd2.Name == "name"
                        && nd2.InnerText == "TriggerType")
                    {
                        getTriggerType = true;
                    }

                    if (getTriggerType == true 
                        && nd2.Name == "value")
                    {
                        sfFlowValues.triggerType = nd2.ChildNodes[0].InnerText;
                    }
                }
            }


            return sfFlowValues;
        }


        public void writeProcessAutomationToExcel(Dictionary<String, List<SFFlowValues>> objectToFlows, Dictionary<String, List<SFTriggerValues>> objectToTrigger)
        {
            excelRangeFormatList = new List<ExcelRangeFormat>();

            Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
            xlapp.Visible = true;

            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlapp.Workbooks.Add();

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                    (System.Reflection.Missing.Value,
                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                        System.Reflection.Missing.Value,
                                                                        System.Reflection.Missing.Value);

            xlWorksheet.Name = "ObjectAutomation";

            Int32 rowStart = 1;
            Int32 rowEnd = 1;
            Int32 colStart = 1;
            Int32 colEnd = 1;
            //Int32 lastRowNumber = 1;

            writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, "Object Name");
            colEnd++;

            writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, "Flow Name");
            colEnd++;

            writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, "Flow Run In Mode");
            colEnd++;

            writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, "Flow Process Type");
            colEnd++;

            writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, "Flow Trigger Type");
            colEnd++;

            writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, "Flow Record Trigger Type");
            colEnd++;


            // These values will come from the Dictionary:
            // SFFlowValues.objectToCreateUpdate
            // SFFlowValues
            // SFTriggerValues
            writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, "Object Inserted/Updated");
            colEnd++;

            writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, "Flow DML Type");
            colEnd++;

            writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, "Flow Trigger Name");
            colEnd++;

            writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, "Flow Trigger Type");
            colEnd++;

            writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, "Apex Trigger - Execution Type");
            colEnd++;

            writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, "Apex Trigger Name");

            addFormatToExcelRange(xlWorksheet, rowStart, rowEnd, colStart, colEnd, 11, 0, 0, 0, 255, 255, 255, true, false, "");

            colEnd = 1;
            rowEnd++;

            //foreach (ExcelRangeFormat erf in excelRangeFormatList)
            //{
            //    formatExcelRange(erf);
            //}


            // 2021-05-04: Move to private method for better handling. For right now though, going to develop the entire structure here
            // Write the Record-Triggered Flows which are run Before Insert / Update of a record

            // Used for sorting
            List<String> flowObjectKeys = new List<String>();
            flowObjectKeys.AddRange(objectToFlows.Keys);
            flowObjectKeys.Sort();

            foreach (String sfObject in flowObjectKeys)
            {
                writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, sfObject);
                colEnd++;
                rowEnd++;

                foreach (SFFlowValues flowVals in objectToFlows[sfObject])
                {
                    if (flowVals.triggerType == "RecordBeforeSave")
                    {
                        writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, flowVals.flowName);
                        colEnd++;

                        writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, flowVals.runInMode);
                        colEnd++;

                        writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, flowVals.processType);
                        colEnd++;

                        writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, flowVals.triggerType);
                        colEnd++;

                        writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, flowVals.recordTriggerType);
                        colEnd++;

                        rowEnd++;

                        // This will have to be confirmed on what triggers and other flows fire when a record is inserted/updated in the parent Flow/Trigger
                        foreach (String sfObject2 in flowVals.objectToCreateUpdate.Keys)
                        {
                            writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, sfObject2);
                            colEnd++;

                            foreach (String flowDmlType in flowVals.objectToCreateUpdate[sfObject2].Keys)
                            {
                                writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, flowDmlType);
                                rowEnd++;
                            }

                            colEnd = 7;
                        }

                        rowEnd++;
                    }

                    colEnd = 2;
                }

                foreach (SFFlowValues flowVals in objectToFlows[sfObject])
                {
                    if (flowVals.triggerType == "RecordAfterSave")
                    {
                        writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, flowVals.flowName);
                        colEnd++;

                        writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, flowVals.runInMode);
                        colEnd++;

                        writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, flowVals.processType);
                        colEnd++;

                        writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, flowVals.triggerType);
                        colEnd++;

                        writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, flowVals.recordTriggerType);
                        colEnd++;

                        rowEnd++;

                        // This will have to be confirmed on what triggers and other flows fire when a record is inserted/updated in the parent Flow/Trigger
                        foreach (String sfObject2 in flowVals.objectToCreateUpdate.Keys)
                        {
                            writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, sfObject2);
                            colEnd++;

                            foreach (String flowDmlType in flowVals.objectToCreateUpdate[sfObject2].Keys)
                            {
                                writeDataToExcelSheet(xlWorksheet, rowEnd, colEnd, flowDmlType);
                                rowEnd++;
                            }

                            colEnd = 7;
                        }

                        rowEnd++;
                    }

                    colEnd = 2;
                }

                colEnd = 1;
            }

            xlapp.Visible = true;
        }


        public class SFClassValues
        {

        }


        public class SFFlowValues
        {
            public String flowName;
            public String objectName;
            public String runInMode;
            public String processType;
            public String recordTriggerType;
            public String triggerType;

            // Key = variable name; Value = objectType 
            // Some variables may or may not have objectType, so check this first to determine if the variable contains an object reference
            // We only want the variables which reference an object so we can determine if there is a recordCreates or recordUpdates which uses that variable value
            // Some recordCreates and recordUpdates directly reference the object as well, so check for an inputReference tag first to determine if it is variable value or 
            // a direct create/update

            public SFFlowValues()
            {
                recordLookupVarToObject = new Dictionary<String, String>();
                varNameToObject = new Dictionary<string, string>();
                objectToCreateUpdate = new Dictionary<String, Dictionary<String, List<String>>>();
            }

            public Dictionary<String, String> recordLookupVarToObject;

            // Key = Variable Name, Value = Object Name
            public Dictionary<String, String> varNameToObject;

            // Key1 = Name of object being created/updated, Key2 = recordCreates OR recordUpdates, List values = name of the element which runs the create/update
            public Dictionary<String, Dictionary<String, List<String>>> objectToCreateUpdate;
        }


        public class SFTriggerValues
        {
            public String triggerName;
            public String namespacePrefix;
            public String entityDefinitionId;
            public String objectName;
            public Boolean usageAfterDelete;
            public Boolean usageAfterInsert;
            public Boolean usageAfterUndelete;
            public Boolean usageAfterUpdate;
            public Boolean usageBeforeDelete;
            public Boolean usageBeforeInsert;
            public Boolean usageBeforeUpdate;
            public Boolean usageIsBulk;
        }

    }
}
