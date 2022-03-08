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
using System.Diagnostics;

namespace SalesforceMetadata
{
    public partial class LWCInspector : Form
    {
        private Dictionary<String, JSFileHierarchy> jsFileHierarchyDict;
        private Dictionary<String, String> importDictionary;

        // Key = Constant Value, Value = parsed component name
        // Ex: const TIERING_MARGIN_ACTIVE_TIER_CALCS = 'c-tiering-margin-active-tier-calcs';
        // TIERING_MARGIN_ACTIVE_TIER_CALCS, tieringMarginActiveTierCalcs
        private Dictionary<String, String> constToComponent;

        public LWCInspector()
        {
            InitializeComponent();
        }

        private void tbLWCFolderPath_DoubleClick(object sender, EventArgs e)
        {
            this.tbLWCFolderPath.Text = UtilityClass.folderBrowserSelectPath("Select LWC Folder to Parse", false, FolderEnum.ReadFrom);
        }

        private void tbSaveResultsTo_DoubleClick(object sender, EventArgs e)
        {
            this.tbSaveResultsTo.Text = UtilityClass.folderBrowserSelectPath("Save Results to", true, FolderEnum.SaveTo);
        }

        private void btnParseLWC_Click(object sender, EventArgs e)
        {
            if (this.tbLWCFolderPath.Text == "")
            {
                MessageBox.Show("Please select a location to parse in the LWC Folder Path text box");
                return;
            }

            if (this.tbSaveResultsTo.Text == "")
            {
                MessageBox.Show("Please select a location to save the results to in the Save Results To box");
                return;
            }

            this.jsFileHierarchyDict = new Dictionary<string, JSFileHierarchy>();
            this.importDictionary = new Dictionary<String, String>();
            this.constToComponent = new Dictionary<String, String>();

            // Go through all JS Files first
            List<String> jsFiles = new List<string>();

            // Go through all LWC HTML Components to find which components / child components call which functions
            List<String> htmlFiles = new List<string>();


            // Get all subdirectories and files
            List<String> subdirectorySearchCompleted = new List<String>();

            // Add the currently selected folder path so it won't be selected again.
            List<String> subDirectoryList = new List<String>();
            subDirectoryList.Add(this.tbLWCFolderPath.Text);
            subDirectoryList.AddRange(getSubdirectories(this.tbLWCFolderPath.Text));

            Boolean subdirectoriesExist = false;
            if (subDirectoryList.Count > 0)
            {
                subdirectoriesExist = true;
            }

            while (subdirectoriesExist == true)
            {
                if (subDirectoryList.Count == 0) subdirectoriesExist = false;

                for (Int32 i = 0; i < subDirectoryList.Count; i++)
                {
                    try
                    {
                        // Get all files in the current directory
                        String[] files = Directory.GetFiles(subDirectoryList[i]);
                        if (files.Length > 0)
                        {
                            for (Int32 j = 0; j < files.Length; j++)
                            {
                                FileInfo fi = new FileInfo(files[j]);
                                if (fi.Extension == ".html")
                                {
                                    htmlFiles.Add(fi.FullName);
                                }
                                else if (fi.Extension == ".js")
                                {
                                    jsFiles.Add(fi.FullName);
                                }
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                    }

                    subdirectorySearchCompleted.Add(subDirectoryList[i]);
                }

                // Check if there are any additional sub directories in the current directory and add them to the list
                List<String> subDirectories = new List<String>();
                for (Int32 i = 0; i < subDirectoryList.Count; i++)
                {
                    if (subDirectoryList[i] != this.tbLWCFolderPath.Text)
                    {
                        List<String> sds = getSubdirectories(subDirectoryList[i]);
                        if (sds.Count > 0)
                        {
                            foreach (String s in sds)
                            {
                                if (!subdirectorySearchCompleted.Contains(s))
                                {
                                    subDirectories.Add(s);
                                }
                            }
                        }
                    }
                }

                // Remove the current directories in subDirectoriesList before adding the additional subdirectories
                subDirectoryList.Clear();

                if (subDirectories.Count > 0)
                {
                    foreach (String s in subDirectories)
                    {
                        if (!subDirectoryList.Contains(s))
                        {
                            subDirectoryList.Add(s);
                        }
                    }

                    subDirectories.Clear();
                }
            }

            // Now loop through the LWC JS files to get the call hiearchy
            // get the text values into a List<string> and then add them to the 
            Dictionary<String, List<String>> fileToParsedContent = new Dictionary<String, List<String>>();
            Boolean writeToFile = true;
            if (jsFiles.Count > 0)
            {
                StreamWriter swLog = new StreamWriter(this.tbSaveResultsTo.Text + "\\LWCFunctionHierarchyLog.txt");

                foreach (String fileName in jsFiles)
                {
                    String[] filePathSplit = fileName.Split('\\');
                    String[] fileNameSplit = filePathSplit[filePathSplit.Length - 1].Split('.');

                    String folderName = filePathSplit[filePathSplit.Length - 2].ToLower();

                    StreamReader sr = new StreamReader(fileName);


                    // Read the line and add to the stringArrayList to loop through as individual characters
                    // At this point we are just reading the line and adding it to the stringArray
                    // then read the next line and add those additional contents to the stringArray
                    List<String> stringArray = new List<string>();
                    //Boolean isMultiLineComment = false;
                    //Boolean isInlineComment = false;
                    Boolean isStringValue = false;
                    Boolean isConsoleLog = false;

                    String stringValue = "";

                    String[] parsedLine = readFileSplit(sr.ReadToEnd());
                    sr.Close();

                    for (Int32 i = 0; i < parsedLine.Length; i++)
                    {
                        if (isStringValue == false
                            && isConsoleLog == false
                            && (parsedLine[i] == "\'" || parsedLine[i] == "\""))
                        {
                            isStringValue = true;
                        }
                        else if (isStringValue == true
                            && (parsedLine[i] == "\'" || parsedLine[i] == "\""))
                        {
                            isStringValue = false;
                            stringArray.Add(stringValue.Trim());

                            stringValue = "";
                        }
                        else if (parsedLine[i].ToLower().StartsWith("console.log"))
                        {
                            isConsoleLog = true;
                        }
                        else if (isConsoleLog == true && parsedLine[i] == ";")
                        {
                            isConsoleLog = false;
                        }
                        else if (parsedLine[i] == ";")
                        {
                            stringArray.Add(parsedLine[i]);
                        }
                        else if (isStringValue == true)
                        {
                            stringValue = stringValue + parsedLine[i] + " ";
                        }
                        else if (isConsoleLog == false)
                        {
                            if (parsedLine[i] != "")
                            {
                                stringArray.Add(parsedLine[i]);
                            }
                        }
                    }


                    // Now loop through the text array and determine the types, constructing them using the inner classes
                    Boolean isExported = false;
                    Int32 arrayPos = 0;

                    //try
                    //{
                        for (Int32 i = 0; i < stringArray.Count; i++)
                        {
                            if (arrayPos < i) arrayPos = i;

                            if (arrayPos > stringArray.Count - 1) break;

                            if (arrayPos == i)
                            {
                                if (stringArray[i] == ";")
                                {
                                    // Do nothing
                                }
                                else if (stringArray[i].ToLower() == "}"
                                    && isExported == true)
                                {
                                    isExported = false;
                                }
                                else if (stringArray[i].ToLower().EndsWith(".set"))
                                {
                                    for (Int32 j = i; j < stringArray.Count; j++)
                                    {
                                        if (stringArray[j] == ";")
                                        {
                                            arrayPos = j + 1;
                                            break;
                                        }
                                    }
                                }
                                else if (stringArray[i].ToLower() == "@api")
                                {
                                    // Check if is function first, then if all filters fail, assume it is a property

                                    // get / set
                                    if (stringArray[i + 3] == "(")
                                    {
                                        arrayPos = parseFunction(folderName, fileNameSplit[0].ToLower(), stringArray, i, isExported, swLog);
                                    }
                                    else if (stringArray[i + 2] == "(")
                                    {
                                        arrayPos = parseFunction(folderName, fileNameSplit[0].ToLower(), stringArray, i, isExported, swLog);
                                    }
                                    else
                                    {
                                        arrayPos = parseProperty(folderName, fileNameSplit[0].ToLower(), stringArray, i, isExported, swLog);
                                    }
                                }
                                else if (stringArray[i].ToLower() == "@track")
                                {
                                    // I believe this is a variable / property
                                    arrayPos = parseProperty(folderName, fileNameSplit[0].ToLower(), stringArray, i, isExported, swLog);
                                }
                                else if (stringArray[i].ToLower() == "@wire")
                                {
                                    arrayPos = parseWireFunction(folderName, fileNameSplit[0].ToLower(), stringArray, i, isExported, swLog);
                                }
                                else if (stringArray[i].ToLower() == "const")
                                {
                                    arrayPos = parseConstant(folderName, fileNameSplit[0].ToLower(), stringArray, i, swLog);
                                }
                                else if (stringArray[i].ToLower() == "export")
                                {
                                    if (stringArray[i].ToLower() == "export"
                                        && stringArray[i + 1].ToLower() == "default")
                                    {
                                        isExported = true;
                                        arrayPos = parseExportDefault(stringArray, i, swLog);
                                    }
                                    else if (stringArray[i].ToLower() == "export"
                                        && stringArray[i + 1].ToLower() == "class")
                                    {
                                        isExported = true;
                                        arrayPos = parseExportDefault(stringArray, i, swLog);
                                    }
                                    else
                                    {
                                        // Call export function parser
                                        arrayPos = parseExport(folderName, fileNameSplit[0].ToLower(), stringArray, i, swLog);
                                    }
                                }
                                else if (stringArray[i].ToLower() == "function")
                                {
                                    arrayPos = parseFunction(folderName, fileNameSplit[0].ToLower(), stringArray, i, isExported, swLog);
                                }
                                else if (stringArray[i].ToLower() == "let")
                                {
                                    arrayPos = parseProperty(folderName, fileNameSplit[0].ToLower(), stringArray, i, isExported, swLog);
                                }
                                else if (stringArray[i].ToLower() == "import")
                                {
                                    arrayPos = parseImport(folderName, fileNameSplit[0].ToLower(), stringArray, i, swLog);
                                }
                                else if (stringArray[i].ToLower() == "static")
                                {
                                    // Check if is function first, then if all filters fail, assume it is a property
                                    if (stringArray[i + 2] == "(")
                                    {
                                        arrayPos = parseFunction(folderName, fileNameSplit[0].ToLower(), stringArray, i, isExported, swLog);
                                    }
                                    else
                                    {
                                        arrayPos = parseProperty(folderName, fileNameSplit[0].ToLower(), stringArray, i, isExported, swLog);
                                    }
                                }
                                else if (stringArray[i].ToLower() == "get")
                                {
                                    arrayPos = parseFunction(folderName, fileNameSplit[0].ToLower(), stringArray, i, isExported, swLog);
                                }
                                else if (stringArray[i].ToLower() == "set")
                                {
                                    arrayPos = parseFunction(folderName, fileNameSplit[0].ToLower(), stringArray, i, isExported, swLog);
                                }
                                else
                                {
                                    // Check if is function first, then if all filters fail, assume it is a property
                                    if (stringArray.Count - 1 >= i + 1
                                    && stringArray[i + 1] == "(")
                                    {
                                        arrayPos = parseFunction(folderName, fileNameSplit[0].ToLower(), stringArray, i, isExported, swLog);
                                    }
                                    else
                                    {
                                        arrayPos = parseProperty(folderName, fileNameSplit[0].ToLower(), stringArray, i, isExported, swLog);
                                    }
                                }
                            }
                        }
                    //}
                    //catch (Exception parseError)
                    //{
                    //    swErrorLog.WriteLine("Parsing Error in file " + fileName + ". Please check the syntax to make sure each constant, export and/or property has a closing ';'");
                    //    swErrorLog.Close();
                    //    writeToFile = false;
                    //    break;
                    //}
                }

                swLog.Close();
            }

            // Now loop through the jsFileHieararchyDict and associate the calling fumctions to the function itself with a hierarchical value;
            //Boolean loopJSFileHieararchyDict = true;

            // Write the values for both functions and properties to the file
            if(writeToFile == true) writeFunctionsToFile();

        }

        private String[] readFileSplit(String fileContents)
        {
            if (fileContents != null)
            {
                String readFile = fileContents.Trim();

                // Maintain the new line character for this loop so that we can determine the end of the inline comments
                readFile = readFile.Replace("\n", " \n ");
                readFile = readFile.Replace("\r", " ");
                readFile = readFile.Replace("\t", " ");

                // Because we are using the <space> character as a way to break apart the text for easier manipulation
                // we need to add these values to an array of strings where we can add the space character back later.
                String[] rLineSplit = readFile.Split(' ');
                List<String> splitStringList = new List<string>();

                // Remove inline comments first
                Boolean isStringValue = false;
                Boolean isMultilineComment = false;
                Boolean isInlineComment = false;
                String stringValue = "";

                for (Int32 i = 0; i < rLineSplit.Length; i++)
                {
                    if (isInlineComment == false
                        && rLineSplit[i].StartsWith("//"))
                    {
                        isInlineComment = true;
                    }
                    else if (isMultilineComment == false
                        && rLineSplit[i].StartsWith("/*"))
                    {
                        isMultilineComment = true;
                    }
                    else if (isInlineComment == true
                        && rLineSplit[i] == "\n")
                    {
                        isInlineComment = false;
                    }
                    else if (isMultilineComment == true
                        && rLineSplit[i].EndsWith("*/"))
                    {
                        isMultilineComment = false;
                    }
                    else if (isMultilineComment == false && isInlineComment == false)
                    {
                        splitStringList.Add(rLineSplit[i]);
                    }
                }


                // Now go back through and add the characters from the list back to the stringValue to parsed further.
                foreach (String lstChar in splitStringList)
                {
                    stringValue += lstChar + " ";
                }


                // Try to eliminate as many unnecessary combinations of \n as we will be using these for the 
                // the lines which do not have a \n such as properties or constants
                // If a line is terminated with a ; then we are in good shape.

                stringValue = stringValue.Replace("\n", " ");
                stringValue = stringValue.Replace("\r", " ");
                stringValue = stringValue.Replace("\t", " ");
                stringValue = stringValue.Replace("(", " ( ");
                stringValue = stringValue.Replace(")", " ) ");
                stringValue = stringValue.Replace("{", " { ");
                stringValue = stringValue.Replace("}", " } ");
                stringValue = stringValue.Replace("[", " [ ");
                stringValue = stringValue.Replace("]", " ] ");
                stringValue = stringValue.Replace(",", " , ");
                stringValue = stringValue.Replace("!==", " !== ");
                stringValue = stringValue.Replace("!=", " != ");
                stringValue = stringValue.Replace("===", " === ");
                stringValue = stringValue.Replace("==", " == ");
                stringValue = stringValue.Replace("=", " = ");
                stringValue = stringValue.Replace(":", " : ");
                stringValue = stringValue.Replace("&&", " && ");
                stringValue = stringValue.Replace("||", " || ");
                stringValue = stringValue.Replace("'", " ' ");
                stringValue = stringValue.Replace("\"", " \" ");
                stringValue = stringValue.Replace("\t", " ");
                stringValue = stringValue.Replace(";", " ; ");
                stringValue = stringValue.Replace("/*", " /* ");
                stringValue = stringValue.Replace("*/", " */ ");
                stringValue = stringValue.Replace("//", " //");
                //stringValue = stringValue.Replace(".", " . ");

                stringValue = stringValue.Replace("  ", " ");
                stringValue = stringValue.Replace("  ", " ");
                stringValue = stringValue.Replace("  ", " ");
                stringValue = stringValue.Replace("  ", " ");
                stringValue = stringValue.Replace("  ", " ");
                stringValue = stringValue.Replace("  ", " ");
                stringValue = stringValue.Replace("  ", " ");
                stringValue = stringValue.Replace("  ", " ");
                stringValue = stringValue.Replace("  ", " ");
                stringValue = stringValue.Replace("  ", " ");

                rLineSplit = stringValue.Split(' ');
                splitStringList = new List<string>();

                Int32 arrayPos = 0;

                // Allows for filtering if the string array value is part of a larger string between " and ' and will concatenate the 
                // individual string values from the array into one string value
                for (Int32 i = 0; i < rLineSplit.Length; i++)
                {
                    if (arrayPos < i) arrayPos = i;

                    if (arrayPos == i)
                    {
                        if (rLineSplit.Length > i + 2 && isStringValue == false && rLineSplit[i] == "=" && rLineSplit[i + 1] == "=" && rLineSplit[i + 2] == "=")
                        {
                            splitStringList.Add("===");
                            arrayPos = i + 3;
                        }
                        else if (rLineSplit.Length > i + 2 && isStringValue == false && rLineSplit[i] == "!" && rLineSplit[i + 1] == "=" && rLineSplit[i + 2] == "=")
                        {
                            splitStringList.Add("!==");
                            arrayPos = i + 3;
                        }
                        else if (rLineSplit.Length > i + 1 && isStringValue == false && rLineSplit[i] == "!" && rLineSplit[i + 1] == "=")
                        {
                            splitStringList.Add("!=");
                            arrayPos = i + 2;
                        }
                        else if (rLineSplit.Length > i + 1 && isStringValue == false && rLineSplit[i] == "=" && rLineSplit[i + 1] == "=")
                        {
                            splitStringList.Add("==");
                            arrayPos = i + 2;
                        }
                        else if (rLineSplit.Length > i + 1 && isStringValue == false && rLineSplit[i] == "=" && rLineSplit[i + 1] == ">")
                        {
                            splitStringList.Add("=>");
                            arrayPos = i + 2;
                        }
                        else if (rLineSplit.Length > i + 1 && isStringValue == false && rLineSplit[i] == "|" && rLineSplit[i + 1] == "|")
                        {
                            splitStringList.Add("||");
                            arrayPos = i + 2;
                        }
                        else
                        {
                            splitStringList.Add(rLineSplit[i]);
                        }
                    }
                }

                return splitStringList.ToArray();
            }
            else
            {
                String[] readLineArray = new string[1];
                readLineArray[0] = "";

                return readLineArray;
            }
        }

        private List<String> getSubdirectories(String folderLocation)
        {
            // Check for additional subdirectories in the current subdirectory list and add them to the list
            List<String> subDirectoryList = new List<String>();
            String[] subDirectories = new String[0];
            try
            {
                subDirectories = Directory.GetDirectories(folderLocation);
                foreach (String sub in subDirectories)
                {
                    subDirectoryList.Add(sub);
                }
            }
            catch (Exception e)
            {

            }

            return subDirectoryList;
        }

        private Int32 parseConstant(String folderName, String fileName, List<String> stringArray, Int32 characterPos, StreamWriter swLog)
        {
            swLog.Write(folderName + "." + fileName + '\t' + "Constant\t");

            JSConstant constant = new JSConstant();
            constant.folderName = folderName;
            constant.fileName = fileName;

            Int32 braceCount = 0;
            Int32 bracketCount = 0;

            Int32 newPos = characterPos;
            String constVal = "";
            for (Int32 i = characterPos; i < stringArray.Count; i++)
            {
                swLog.Write(stringArray[i] + " ");

                if (stringArray[i] == ";"
                    && braceCount == 0
                    && bracketCount == 0)
                {
                    if (constVal != "")
                    {
                        constant.constantValue = constVal;
                        constVal = "";
                    }

                    newPos = i + 1;
                    break;
                }
                else if (stringArray[i] == "{")
                {
                    braceCount++;
                }
                else if (stringArray[i] == "}")
                {
                    braceCount--;
                }
                else if (stringArray[i] == "[")
                {
                    bracketCount++;
                }
                else if (stringArray[i] == "]")
                {
                    bracketCount--;
                }
                else if (stringArray[i].ToLower() == "const")
                {
                    constant.constantName = stringArray[i + 1];
                }
                else if (stringArray[i] == "]")
                {
                    if (constVal != "")
                    {
                        constant.constantValue = constVal + stringArray[i];
                        constVal = "";
                    }

                    if (stringArray[i + 1] == ";")
                    {
                        newPos = i + 2;
                    }
                    else
                    {
                        newPos = i + 1;
                    }

                    break;
                }
                else if (stringArray[i].ToLower() == "=")
                {
                    constVal = "";
                }
                else
                {
                    constVal = constVal + stringArray[i];
                }
            }

            if (this.jsFileHierarchyDict.ContainsKey(folderName + "|" + fileName))
            {
                this.jsFileHierarchyDict[folderName + "|" + fileName].constants.Add(constant);
            }
            else
            {
                JSFileHierarchy fileHier = new JSFileHierarchy();
                fileHier.folderName = folderName;
                fileHier.fileName = fileName;
                fileHier.constants.Add(constant);

                this.jsFileHierarchyDict.Add(folderName + "|" + fileName, fileHier);
            }


            // Add the constant value to constToComponent if the value starts with a c-
            // Example: const TIERING_MARGIN_ACTIVE_TIER_CALCS = 'c-tiering-margin-active-tier-calcs';
            if (constant.constantValue != "")
            {
                String[] constValueSplit = constant.constantValue.Split(new Char[] { '\'', '-' }, StringSplitOptions.RemoveEmptyEntries);

                if (constValueSplit[0] == "c")
                {
                    String componentName = "";
                    for (Int32 j = 1; j < constValueSplit.Length; j++)
                    {
                        componentName = componentName + constValueSplit[j];
                    }

                    if (!this.constToComponent.ContainsKey(constant.folderName + "|" + constant.fileName + "|" + constant.constantName))
                    {
                        this.constToComponent.Add(constant.folderName + "|" + constant.fileName + "|" + constant.constantName, componentName);
                    }
                }
            }

            swLog.Write(Environment.NewLine);

            return newPos;
        }

        private Int32 parseExport(String folderName, String fileName, List<String> stringArray, Int32 characterPos, StreamWriter swLog)
        {
            swLog.Write(folderName + "." + fileName + '\t' + "Export\t");

            Int32 braceCount = 0;

            Int32 newPos = characterPos;
            String exports = "";

            for (Int32 i = characterPos; i < stringArray.Count; i++)
            {
                swLog.Write(stringArray[i] + " ");

                if (stringArray[i] == "{")
                {
                    braceCount++;
                }
                else if (stringArray[i] == "}")
                {
                    braceCount--;

                    JSFileHierarchy fileHier = new JSFileHierarchy();
                    fileHier.folderName = folderName;
                    fileHier.fileName = fileName;
                    if (this.jsFileHierarchyDict.ContainsKey(folderName + "|" + fileName))
                    {
                        fileHier = this.jsFileHierarchyDict[folderName + "|" + fileName];
                    }
                    else
                    {
                        this.jsFileHierarchyDict.Add(folderName + "|" + fileName, fileHier);
                    }

                    if (exports != "")
                    {
                        String[] exportArray = exports.Split(',');
                        foreach (String exp in exportArray)
                        {
                            fileHier.exports.Add(exp);
                        }

                        exports = "";
                    }

                    newPos = i + 1;
                }
                else if (braceCount == 1)
                {
                    exports = exports + stringArray[i];
                }
            }

            swLog.Write(Environment.NewLine);

            return newPos;
        }

        private Int32 parseExportDefault(List<String> stringArray, Int32 characterPos, StreamWriter swLog)
        {
            Int32 newPos = characterPos;

            for (Int32 i = characterPos; i < stringArray.Count; i++)
            {
                if (stringArray[i] == "{")
                {
                    newPos = i + 1;
                    break;
                }
            }

            return newPos;
        }

        private Int32 parseFunction(String folderName, String fileName, List<String> stringArray, Int32 characterPos, Boolean isExported, StreamWriter swLog)
        {
            swLog.Write(folderName + "." + fileName + '\t' + "Function\t");

            JSFunction function = new JSFunction();
            function.folderName = folderName;
            function.fileName = fileName;
            function.isExported = isExported;

            Int32 leftBraceCount = 0;
            Int32 braceCount = 0;
            Int32 parenthCount = 0;

            Int32 newPos = characterPos;
            String functionParameters = "";
            Boolean skipToBrace = false;
            Boolean skipToSemiColon = false;
            Boolean setParameters = false;

            // Get the last brace location from the array. We cannot depend on the brace count to determine when the end of the function occurs
            // We need to make sure the function parameters are accounted for as the first brace won't occur until after the ) designation
            Boolean beginFuncParameterReached = false;
            Boolean endFuncParameterReached = false;
            Int32 lastBracePos = 0;
            for (Int32 i = characterPos; i < stringArray.Count; i++)
            {
                swLog.Write(stringArray[i] + " ");
                Debug.WriteLine(stringArray[i]);

                if (stringArray[i] == "{")
                {
                    braceCount++;
                    leftBraceCount++;
                    Debug.WriteLine("braceCount++" + i.ToString() + " " + braceCount.ToString());
                    Debug.WriteLine("braceCount++" + i.ToString() + " " + leftBraceCount.ToString());
                }
                else if (stringArray[i] == "}")
                {
                    braceCount--;
                    //Debug.WriteLine("braceCount--" + i.ToString() + " " + braceCount.ToString());

                    if (braceCount == 0
                        && beginFuncParameterReached == true
                        && endFuncParameterReached == true)
                    {
                        lastBracePos = i;
                        break;
                    }
                }
                else if (stringArray[i] == "("
                    && braceCount == 0)
                {
                    beginFuncParameterReached = true;
                }
                else if (stringArray[i] == ")"
                    && beginFuncParameterReached == true)
                {
                    endFuncParameterReached = true;
                }
            }

            // Parse the function
            for (Int32 i = characterPos; i < stringArray.Count; i++)
            {
                if (newPos < i) newPos = i;

                if (i == lastBracePos)
                {
                    newPos = i + 1;
                    break;
                }

                if (newPos == i)
                {
                    if (stringArray[i].ToLower() == "@api")
                    {
                        function.functionAnnotation = "@api";
                    }
                    else if (stringArray[i].ToLower() == "get")
                    {
                        function.isGetter = true;
                    }
                    else if (stringArray[i].ToLower() == "set")
                    {
                        function.isSetter = true;
                    }
                    else if (stringArray[i].ToLower() == "static")
                    {
                        function.isStaticFunction = true;
                    }
                    else if (stringArray[i].ToLower() == "function")
                    {
                        //Debug.WriteLine("");
                    }
                    else if (skipToBrace == false
                    && stringArray[i].ToLower() == "const")
                    {
                        skipToSemiColon = true;
                    }
                    else if (skipToBrace == false
                        && stringArray[i].ToLower() == "let")
                    {
                        skipToSemiColon = true;
                    }
                    else if (skipToBrace == false
                        && stringArray[i].ToLower() == "for")
                    {
                        skipToBrace = true;
                    }
                    else if (stringArray[i].ToLower() == "if")
                    {
                        skipToBrace = true;
                    }
                    else if (stringArray[i].ToLower() == "else")
                    {
                        skipToBrace = true;
                    }
                    //else if (stringArray[i].ToLower() == "return")
                    //{
                    //    Int32 j = 0;
                    //    for (j = i + 1; j < stringArray.Count; j++)
                    //    {
                    //        if (stringArray[j] == ";")
                    //        {
                    //            newPos = j + 1;
                    //            break;
                    //        }
                    //        else
                    //        {
                    //            function.returnValue = function.returnValue + stringArray[j];
                    //        }
                    //    }
                    //}
                    else if (stringArray[i].ToLower().EndsWith(".foreach"))
                    {
                        skipToBrace = true;

                        // Add + 1 since we are not going to count anything after the .foreach, but need to 
                        // consider the closing ) at the end of the foreach block
                        parenthCount++;

                        // TODO: Parse out the variable being looped through

                    }
                    else if (stringArray[i] == ";")
                    {
                        skipToSemiColon = false;
                    }
                    else if (skipToBrace == false
                             && stringArray[i] == "("
                             && braceCount == 0)
                    {
                        parenthCount++;
                        setParameters = true;
                    }
                    else if (skipToBrace == false
                             && stringArray[i] == "(")
                    {
                        parenthCount++;
                    }
                    else if (skipToBrace == false
                             && stringArray[i] == ")")
                    {
                        parenthCount--;
                        setParameters = false;
                    }
                    else if (stringArray[i] == "{")
                    {
                        skipToBrace = false;
                        braceCount++;
                    }
                    else if (stringArray[i] == "}")
                    {
                        braceCount--;
                        setParameters = false;
                    }
                    else if (parenthCount == 0
                        && braceCount == 0)
                    {
                        function.functionName = stringArray[i];

                        if (functionParameters != "")
                        {
                            String[] parameterArray = functionParameters.Split(',');

                            foreach (String param in parameterArray)
                            {
                                function.parameters.Add(param);
                            }

                            functionParameters = "";
                        }
                    }
                    else if (setParameters == true)
                    {
                        functionParameters = functionParameters + stringArray[i];
                    }
                    // Child Property / Function logic
                    else if (skipToBrace == false
                        && stringArray[i].ToLower().StartsWith("this."))
                    {
                        // Potentially, this can be a push
                        String[] splitPropertyOrFunction = stringArray[i].Split('.');

                        // Is Property or Function?
                        if (stringArray[i + 1] == "=")
                        {
                            JSProperty prop = new JSProperty();
                            prop.folderName = folderName;
                            prop.fileName = fileName;
                            prop.propertyName = splitPropertyOrFunction[1];
                            prop.whereSet = function.functionName;

                            newPos = i;
                            String setToValue = "";
                            Boolean setValue = false;
                            for (Int32 j = newPos; j < stringArray.Count; j++)
                            {
                                if (stringArray[j] == "=")
                                {
                                    setValue = true;
                                }
                                else if (stringArray[j] == ";")
                                {
                                    setValue = false;
                                    newPos = j + 1;
                                    break;
                                }
                                else if (setValue == true)
                                {
                                    setToValue = setToValue + stringArray[j];
                                }
                            }

                            prop.propertyValue = setToValue;
                            function.propertiesSet.Add(prop);
                        }
                        else if (splitPropertyOrFunction.Length == 3
                            && splitPropertyOrFunction[2] == "push")
                        {
                            JSProperty prop = new JSProperty();
                            prop.folderName = folderName;
                            prop.fileName = fileName;
                            prop.propertyName = splitPropertyOrFunction[1];
                            prop.whereSet = function.functionName;

                            newPos = i + 1;
                            String setToValue = "";
                            for (Int32 j = newPos; j < stringArray.Count; j++)
                            {
                                if (stringArray[j] == ";")
                                {
                                    newPos = j + 1;
                                    break;
                                }
                                else
                                {
                                    setToValue = setToValue + stringArray[j];
                                }
                            }

                            prop.propertyValue = setToValue;
                            function.propertiesSet.Add(prop);
                        }
                        else if (splitPropertyOrFunction.Length == 3
                            && splitPropertyOrFunction[2] == "querySelector")
                        {
                            JSFunction cf = new JSFunction();

                            String parameters = "";
                            setParameters = false;
                            for (Int32 j = i; j < stringArray.Count; j++)
                            {
                                if (stringArray[j] == ";")
                                {
                                    newPos = j + 1;
                                    break;
                                }
                                else if (stringArray[j] == "(")
                                {
                                    setParameters = true;
                                }
                                else if (stringArray[j] == ")")
                                {
                                    setParameters = false;

                                    newPos = j + 1;
                                    break;
                                }
                                else if (setParameters == true)
                                {
                                    String[] parameterSplit = stringArray[j].Split('.');

                                    if (parameterSplit.Length == 1)
                                    {
                                        parameters = parameters + parameterSplit[0];
                                    }
                                    else
                                    {
                                        parameters = parameters + parameterSplit[1];
                                    }
                                }
                            }

                            cf.functionName = "querySelector";

                            String[] parametersArray = parameters.Split(',');
                            foreach (String param in parametersArray)
                            {
                                cf.parameters.Add(param);
                            }

                            function.childFunctions.Add(cf);
                        }
                        else if (stringArray[i + 1] == "(")
                        {
                            JSFunction cf = new JSFunction();
                            cf.folderName = folderName;
                            cf.fileName = fileName;
                            cf.isLocal = true;

                            //String[] functionName = stringArray[i].Split('.');

                            // TODO: if the functionName length == 3, then go through the constants to determine 
                            // what the component reference is in relation to
                            // then link the component to the function name
                            if (splitPropertyOrFunction.Length == 3)
                            {
                                cf.componentReferenceVar = splitPropertyOrFunction[1];
                                cf.functionName = splitPropertyOrFunction[2];
                            }
                            else
                            {
                                cf.functionName = splitPropertyOrFunction[1];
                            }

                            String parameters = "";
                            setParameters = false;
                            for (Int32 j = i; j < stringArray.Count; j++)
                            {
                                if (stringArray[j] == ";")
                                {
                                    newPos = j + 1;
                                    break;
                                }
                                else if (stringArray[j] == "(")
                                {
                                    setParameters = true;
                                }
                                else if (stringArray[j] == ")")
                                {
                                    setParameters = false;

                                    newPos = j + 1;
                                    break;
                                }
                                else if (setParameters == true)
                                {
                                    String[] parameterSplit = stringArray[j].Split('.');

                                    if (parameterSplit.Length == 1)
                                    {
                                        parameters = parameters + parameterSplit[0];
                                    }
                                    else
                                    {
                                        parameters = parameters + parameterSplit[1];
                                    }
                                }
                            }

                            // Finish setting the rest of the child function properties
                            String[] parametersArray = parameters.Split(',');
                            foreach (String param in parametersArray)
                            {
                                cf.parameters.Add(param);
                            }

                            function.childFunctions.Add(cf);
                        }
                        else
                        {
                            // Do nothing.

                            // It must ABSOLUTELY be a property or something else at this point after all of the checks above. Right????  I'm sure of it! I'm sure of it! :D
                            // JSProperty prop = new JSProperty();
                            // prop.folderName = folderName;
                            // prop.fileName = fileName;
                            // prop.propertyName = splitPropertyOrFunction[1];
                            // prop.whereSet = function.functionName;

                            // function.propertiesSet.Add(prop);
                        }
                    }
                    else if (skipToBrace == false
                        && stringArray[i].ToLower().StartsWith(".then"))
                    {
                        for (Int32 j = i + 1; j < stringArray.Count; j++)
                        {
                            if (stringArray[j] == "(")
                            {
                                parenthCount++;
                            }
                            else if (stringArray[j] == ")")
                            {
                                parenthCount--;
                            }
                            else if (stringArray[j] == "{")
                            {
                                braceCount++;
                                break;
                            }
                        }
                    }
                    else if (skipToBrace == false
                        && stringArray[i].ToLower().StartsWith(".catch"))
                    {
                        for (Int32 j = i + 1; j < stringArray.Count; j++)
                        {
                            if (stringArray[j] == "(")
                            {
                                parenthCount++;
                            }
                            else if (stringArray[j] == ")")
                            {
                                parenthCount--;
                            }
                            else if (stringArray[j] == "{")
                            {
                                braceCount++;
                                break;
                            }
                        }
                    }
                    else if (skipToBrace == false
                        && stringArray.Count > i + 1
                        && stringArray[i + 1] == "(")
                    {
                        JSFunction cf = new JSFunction();
                        cf.folderName = folderName;
                        cf.fileName = fileName;
                        cf.functionName = stringArray[i];

                        if (this.importDictionary.ContainsKey(stringArray[i]))
                        {
                            cf.importFrom = this.importDictionary[stringArray[i]];
                        }

                        // Get the import-from since this is a function w without a this. designation
                        String parameters = "";
                        setParameters = false;
                        for (Int32 j = i; j < stringArray.Count; j++)
                        {
                            if (stringArray[j] == ";")
                            {
                                newPos = j + 1;
                                break;
                            }
                            else if (stringArray[j] == "(")
                            {
                                setParameters = true;
                            }
                            else if (stringArray[j] == ")")
                            {
                                setParameters = false;

                                newPos = j + 1;
                                break;
                            }
                            else if (setParameters == true)
                            {
                                String[] parameterSplit = stringArray[j].Split('.');

                                if (parameterSplit.Length == 1)
                                {
                                    parameters = parameters + parameterSplit[0];
                                }
                                else
                                {
                                    parameters = parameters + parameterSplit[1];
                                }
                            }
                        }

                        // Finish setting the rest of the child function properties
                        String[] parametersArray = parameters.Split(',');
                        foreach (String param in parametersArray)
                        {
                            cf.parameters.Add(param);
                        }

                        function.childFunctions.Add(cf);
                    }
                    else
                    {

                    }
                }
            }

            if (functionParameters != "")
            {
                String[] parametersArray = functionParameters.Split(',');
                foreach (String param in parametersArray)
                {
                    function.parameters.Add(param);
                }
            }

            // Loop through the function's properties and return value to determine if there are child function calls
            // Set the properties to the constants if there is a value assigned to a property
            foreach(JSProperty prop in function.propertiesSet)
            {
                String[] propertyValueSplit = prop.propertyValue.Split(new Char[] { '.', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);

                // If the propertyValueSplit appears to be a function, add it to the child function calls

                // Determine if the value being passed in is a Constant, and then set the relationship between the variable and the constant for easier assessment later in the code.

                if (propertyValueSplit.Length == 3)
                {
                    if (propertyValueSplit[0] == "this")
                    {
                        JSFunction cf = new JSFunction();
                        cf.folderName = folderName;
                        cf.fileName = fileName;
                        cf.functionName = propertyValueSplit[1];
                        cf.parentFunction = function.functionName;
                        cf.isLocal = true;

                        cf.valueSet = prop.propertyName;

                        String[] parameterSplit = propertyValueSplit[2].Split(',');

                        foreach (String param in parameterSplit)
                        {
                            cf.parameters.Add(param);
                        }

                        function.childFunctions.Add(cf);
                    }
                    else
                    {
                        JSFunction cf = new JSFunction();
                        cf.functionName = propertyValueSplit[0];
                        cf.parentFunction = function.functionName;

                        // find the reference component from the imports
                        String compRef = "";

                        if (importDictionary.ContainsKey(propertyValueSplit[0]))
                        {
                            compRef = importDictionary[propertyValueSplit[0]];

                            String[] compRefSplit = compRef.Split('/');

                            // Another Component
                            if (compRefSplit[0] == "c")
                            {
                                cf.folderName = compRefSplit[1];
                                cf.fileName = compRefSplit[1];
                            }
                            // JS file in same component
                            else if (compRefSplit[0] == ".")
                            {
                                cf.folderName = folderName;
                                cf.fileName = compRefSplit[1];
                            }

                            cf.valueSet = prop.propertyName;

                            String[] parameterSplit = propertyValueSplit[2].Split(',');

                            foreach (String param in parameterSplit)
                            {
                                cf.parameters.Add(param);
                            }

                            function.childFunctions.Add(cf);
                        }
                    }
                }
                else if (propertyValueSplit.Length == 2
                    && prop.propertyValue.Contains("(")
                    && prop.propertyValue.Contains(")"))
                {
                    if (propertyValueSplit[0] == "this")
                    {
                        JSFunction cf = new JSFunction();
                        cf.functionName = propertyValueSplit[1];
                        cf.parentFunction = function.functionName;
                        cf.folderName = folderName;
                        cf.fileName = fileName;
                        cf.isLocal = true;

                        cf.valueSet = prop.propertyName;

                        function.childFunctions.Add(cf);
                    }
                    else
                    {
                        JSFunction cf = new JSFunction();
                        cf.functionName = propertyValueSplit[0];
                        cf.parentFunction = function.functionName;

                        // find the reference component from the imports
                        String compRef = "";

                        if (importDictionary.ContainsKey(propertyValueSplit[0]))
                        {
                            compRef = importDictionary[propertyValueSplit[0]];

                            String[] compRefSplit = compRef.Split('/');

                            // Another Component
                            if (compRefSplit[0] == "c")
                            {
                                cf.folderName = compRefSplit[1];
                                cf.fileName = compRefSplit[1];
                            }
                            // JS file in same component
                            else if (compRefSplit[0] == ".")
                            {
                                cf.folderName = folderName;
                                cf.fileName = compRefSplit[1];
                            }

                            cf.valueSet = prop.propertyName;

                            function.childFunctions.Add(cf);
                        }
                    }
                }
            }


            if (this.jsFileHierarchyDict.ContainsKey(folderName + "|" + fileName))
            {
                this.jsFileHierarchyDict[folderName + "|" + fileName].functions.Add(function);
            }
            else
            {
                JSFileHierarchy fileHier = new JSFileHierarchy();
                fileHier.folderName = folderName;
                fileHier.fileName = fileName;
                fileHier.functions.Add(function);

                this.jsFileHierarchyDict.Add(folderName + "|" + fileName, fileHier);
            }

            swLog.Write(Environment.NewLine);

            return lastBracePos + 1;
        }

        private Int32 parseImport(String folderName, String fileName, List<String> stringArray, Int32 characterPos, StreamWriter swLog)
        {
            swLog.Write(folderName + "." + fileName + '\t' + "Import\t");

            JSImport import = new JSImport();
            import.folderName = folderName;
            import.fileName = fileName;

            Int32 braceCount = 0;

            Int32 newPos = characterPos;
            String imports = "";

            for (Int32 i = characterPos; i < stringArray.Count; i++)
            {
                if (stringArray[i] == ";")
                {
                    swLog.Write(stringArray[i] + " ");
                    break;
                }
                else
                {
                    swLog.Write(stringArray[i] + " ");
                }
            }

            if (stringArray[characterPos + 1] == "{")
            {
                for (Int32 i = characterPos; i < stringArray.Count; i++)
                {
                    if (stringArray[i] == ";")
                    {
                        newPos = i + 1;
                        break;
                    }
                    else if (stringArray[i].ToLower() == "import"
                        && stringArray[i + 1] != "{")
                    {
                        import.importItems.Add(stringArray[i + 1]);
                    }
                    else if (stringArray[i].ToLower() == "{")
                    {
                        braceCount++;
                    }
                    else if (stringArray[i].ToLower() == "}")
                    {
                        braceCount--;

                        if (imports != "")
                        {
                            String[] importArray = imports.Split(',');
                            foreach (String imp in importArray)
                            {
                                import.importItems.Add(imp);
                            }

                            imports = "";
                        }
                    }
                    else if (braceCount == 1)
                    {
                        imports = imports + stringArray[i];
                    }
                    else if (stringArray[i].ToLower() == "from")
                    {
                        if (imports != "")
                        {
                            String[] importArray = imports.Split(',');
                            foreach (String imp in importArray)
                            {
                                import.importItems.Add(imp);
                            }

                            imports = "";
                        }

                        import.importFrom = stringArray[i + 1];
                    }
                }
            }
            else
            {
                import.folderName = folderName;
                import.fileName = fileName;
                import.importItems.Add(stringArray[characterPos + 1]);
                import.importFrom = stringArray[characterPos + 3];

                if (stringArray[characterPos + 4] == ";")
                {
                    newPos = characterPos + 5;
                }
                else
                {
                    newPos = characterPos + 4;
                }
            }

            if (this.jsFileHierarchyDict.ContainsKey(folderName + "|" + fileName))
            {
                this.jsFileHierarchyDict[folderName + "|" + fileName].imports.Add(import);
            }
            else
            {
                JSFileHierarchy fileHier = new JSFileHierarchy();
                fileHier.folderName = folderName;
                fileHier.fileName = fileName;
                fileHier.imports.Add(import);

                this.jsFileHierarchyDict.Add(folderName + "|" + fileName, fileHier);
            }

            // Add the imports to the importDictionary
            foreach (JSFileHierarchy fileHier in this.jsFileHierarchyDict.Values)
            {
                foreach (JSImport imp in fileHier.imports)
                {
                    foreach (String impItem in imp.importItems)
                    {
                        if (!this.importDictionary.ContainsKey(impItem))
                        {
                            this.importDictionary.Add(impItem, imp.importFrom);
                        }
                    }
                }
            }

            swLog.Write(Environment.NewLine);

            return newPos;
        }

        private Int32 parseProperty(String folderName, String fileName, List<String> stringArray, Int32 characterPos, Boolean isExported, StreamWriter swLog)
        {
            swLog.Write(folderName + "." + fileName + '\t' + "Property\t");

            JSProperty property = new JSProperty();
            property.folderName = folderName;
            property.fileName = fileName;
            property.isExported = isExported;

            Int32 newPos = characterPos;
            Int32 braceCount = 0;
            Int32 bracketCount = 0;

            Boolean setPropertyValue = false;
            String propertyName = "";
            String propertyValue = "";

            for (Int32 i = characterPos; i < stringArray.Count; i++)
            {
                swLog.Write(stringArray[i] + " ");

                if (newPos < i) newPos = i;

                if (newPos == i)
                {
                    if (stringArray[i] == ";"
                        && braceCount == 0
                        && bracketCount == 0)
                    {
                        property.propertyName = propertyName;
                        property.propertyValue = propertyValue;

                        setPropertyValue = false;

                        propertyName = "";
                        propertyValue = "";

                        newPos = i + 1;
                        break;
                    }
                    else if (stringArray[i] == "{")
                    {
                        braceCount++;
                        propertyValue = propertyValue + stringArray[i] + " ";
                    }
                    else if (stringArray[i] == "}")
                    {
                        braceCount--;
                        propertyValue = propertyValue + stringArray[i] + " ";

                        if (braceCount == 0
                            && bracketCount == 0)
                        {
                            property.propertyName = propertyName;
                            property.propertyValue = propertyValue;

                            setPropertyValue = false;

                            propertyName = "";
                            propertyValue = "";

                            newPos = i + 1;
                            break;
                        }

                    }
                    else if (stringArray[i] == "[")
                    {
                        bracketCount++;
                        propertyValue = propertyValue + stringArray[i] + " ";
                    }
                    else if (stringArray[i] == "]")
                    {
                        bracketCount--;
                        propertyValue = propertyValue + stringArray[i] + " ";
                    }
                    else if (stringArray[i].ToLower() == "@api")
                    {
                        property.propertyAnnotation = "@api";
                    }
                    else if (stringArray[i].ToLower() == "@track")
                    {
                        property.propertyAnnotation = "@track";
                    }
                    else if (stringArray[i] == "="
                        || stringArray[i] == "=="
                        || stringArray[i] == "===")
                    {
                        if (setPropertyValue == false)
                        {
                            setPropertyValue = true;
                        }
                        else
                        {
                            propertyValue = propertyValue + stringArray[i] + " ";
                        }
                    }
                    else if (stringArray[i] == "!="
                        || stringArray[i] == "!==")
                    {
                        if (setPropertyValue == false)
                        {
                            setPropertyValue = true;
                        }
                        else
                        {
                            propertyValue = propertyValue + stringArray[i] + " ";
                        }
                    }
                    else if (setPropertyValue)
                    {
                        propertyValue = propertyValue + stringArray[i] + " ";
                    }
                    else
                    {
                        propertyName = stringArray[i];
                    }
                }
            }

            if (this.jsFileHierarchyDict.ContainsKey(folderName + "|" + fileName))
            {
                this.jsFileHierarchyDict[folderName + "|" + fileName].properties.Add(property);
            }
            else
            {
                JSFileHierarchy fileHier = new JSFileHierarchy();
                fileHier.folderName = folderName;
                fileHier.fileName = fileName;
                fileHier.properties.Add(property);

                this.jsFileHierarchyDict.Add(folderName + "|" + fileName, fileHier);
            }

            swLog.Write(Environment.NewLine);

            return newPos;
        }

        private Int32 parseWireFunction(String folderName, String fileName, List<String> stringArray, Int32 characterPos, Boolean isExported, StreamWriter swLog)
        {
            swLog.Write(folderName + "." + fileName + '\t' + "Wire\t");

            JSFunction function = new JSFunction();
            function.folderName = folderName;
            function.fileName = fileName;
            function.isWireFunction = true;
            function.isExported = isExported;

            Int32 braceCount = 0;
            Int32 parenthCount = 0;

            Int32 newPos = characterPos;
            String functionParameter = "";
            for (Int32 i = characterPos; i < stringArray.Count; i++)
            {
                swLog.Write(stringArray[i] + " ");

                if (stringArray[i] == "{")
                {
                    braceCount++;
                }
                else if (stringArray[i] == "}")
                {
                    braceCount--;
                    if (parenthCount == 1
                        && braceCount == 0
                        && functionParameter != "")
                    {
                        function.parameters.Add(functionParameter);
                        functionParameter = "";
                    }
                }
                else if (stringArray[i] == "(")
                {
                    parenthCount++;
                }
                else if (stringArray[i] == ")")
                {
                    parenthCount--;

                    if (parenthCount == 0)
                    {
                        function.functionWithWireAnnotated = stringArray[i + 1];

                        newPos = i + 1;
                        break;
                    }
                }
                // @wire Function Name
                else if (parenthCount == 1
                    && braceCount == 0
                    && stringArray[i] != ",")
                {
                    function.functionName = stringArray[i];
                }
                else if (parenthCount == 1
                    && braceCount == 1
                    && stringArray[i] != ",")
                {
                    functionParameter = functionParameter + stringArray[i];
                }
                else if (parenthCount == 1
                    && braceCount == 1
                    && stringArray[i] == ",")
                {
                    function.parameters.Add(functionParameter);
                    functionParameter = "";
                }
            }

            if (this.jsFileHierarchyDict.ContainsKey(folderName + "|" + fileName))
            {
                this.jsFileHierarchyDict[folderName + "|" + fileName].functions.Add(function);
            }
            else
            {
                JSFileHierarchy fileHier = new JSFileHierarchy();
                fileHier.folderName = folderName;
                fileHier.fileName = fileName;
                fileHier.functions.Add(function);

                this.jsFileHierarchyDict.Add(folderName + "|" + fileName, fileHier);
            }

            swLog.Write(Environment.NewLine);

            return newPos;
        }

        private void writeFunctionsToFile()
        {
            StreamWriter sw = new StreamWriter(this.tbSaveResultsTo.Text + "\\LWCFunctionHierarchy.txt");

            // Key = JS function name => Value = wire call
            Dictionary<String, String> jsFunctionToWireFunction = new Dictionary<String, String>();
            Dictionary<String, JSFunction> jsLocalFunctionDictionary = new Dictionary<String, JSFunction>();
            //Dictionary<String, JSFunction> jsExternalFunctionDictionary = new Dictionary<String, JSFunction>();

            foreach (String compFile in this.jsFileHierarchyDict.Keys)
            {
                foreach (JSFunction func in this.jsFileHierarchyDict[compFile].functions)
                {
                    if (func.isWireFunction == true
                        && !jsFunctionToWireFunction.ContainsKey(func.folderName + "." + func.fileName + "." + func.functionWithWireAnnotated))
                    {
                        jsFunctionToWireFunction.Add(func.folderName + "." + func.fileName + "." + func.functionWithWireAnnotated, func.functionName);
                    }
                    else if (func.isLocal == false
                        && func.isGetter == false
                        && func.isSetter == false
                        && !jsLocalFunctionDictionary.ContainsKey(func.folderName + "." + func.fileName + "." + func.functionName))
                    {
                        jsLocalFunctionDictionary.Add(func.folderName + "." + func.fileName + "." + func.functionName, func);
                    }
                    //else if (func.isLocal == false
                    //    && func.isGetter == false
                    //    && func.isSetter == false
                    //    && !jsExternalFunctionDictionary.ContainsKey(func.folderName + "." + func.fileName + "." + func.functionName))
                    //{
                    //    jsExternalFunctionDictionary.Add(func.folderName + "." + func.fileName + "." + func.functionName, func);
                    //}
                }
            }

            foreach(String compFile in this.jsFileHierarchyDict.Keys)
            {
                // Now write the function hierarchy to the file
                foreach (JSFunction func in this.jsFileHierarchyDict[compFile].functions)
                {
                    sw.WriteLine("");
                    sw.WriteLine("");

                    // Write the function in this process as the parent function. 
                    // If there are related function calls, then write those related functions incrementing the tab
                    writeSubFunctions(jsFunctionToWireFunction, jsLocalFunctionDictionary, func, 1, sw);
                }
            }

            sw.Close();

        }

        private void writeSubFunctions(Dictionary<String, String> jsFunctionToWireFunction,
                                       Dictionary<String, JSFunction> jsLocalFunctionDictionary,
                                       JSFunction func,
                                       Int32 tabCount,
                                       StreamWriter sw)
        {
            sw.Write(tabCount.ToString());

            for (Int32 t = 0; t < tabCount; t++)
            {
                sw.Write('\t');
            }

            if (func.functionAnnotation == "")
            {
                sw.Write(func.folderName + "." + func.fileName + "." + func.functionName + "(");
                String functParams = "";

                if (func.parameters.Count > 0)
                {
                    foreach (String param in func.parameters)
                    {
                        functParams = functParams + param + ", ";
                    }

                    functParams = functParams.Substring(0, functParams.Length - 2);
                }

                sw.Write(functParams + ")");

                if (func.isExported) sw.Write('\t' + "--IsExported");
                if (func.isStaticFunction) sw.Write('\t' + "--IsStatic");
                if (func.isGetter) sw.Write('\t' + "--IsGetter");
                if (func.isSetter) sw.Write('\t' + "--IsSetter");
                if (func.isLocal)
                {
                    sw.Write('\t' + "--IsLocalFunction");
                }
                else
                {
                    sw.Write('\t' + "--IsImportedOrAPIFunction");
                }

                sw.Write(Environment.NewLine);
            }
            else
            {
                sw.Write(func.folderName + "." + func.fileName + "." + func.functionName + "(");

                String functParams = "";
                if (func.parameters.Count > 0)
                {
                    foreach (String param in func.parameters)
                    {
                        functParams = functParams + param + ", ";
                    }

                    functParams = functParams.Substring(0, functParams.Length - 2);
                }

                sw.Write(functParams + ")");

                sw.Write('\t' + "--" + func.functionAnnotation);

                if (func.isExported) sw.Write('\t' + "--IsExported");
                if (func.isStaticFunction) sw.Write('\t' + "--IsStatic");
                if (func.isGetter) sw.Write('\t' + "--IsGetter");
                if (func.isSetter) sw.Write('\t' + "--IsSetter");
                if (func.isLocal)
                {
                    sw.Write('\t' + "--IsLocal");
                }
                else
                {
                    sw.Write('\t' + "--IsImportedOrAPI");
                }

                sw.Write(Environment.NewLine);
            }

            // If a wire function exists, then get the wire import and import from
            //if (jsFunctionToWireFunction.ContainsKey(func.functionName))
            //{

            //}

            if (func.childFunctions.Count > 0)
            {
                foreach (JSFunction cf in func.childFunctions)
                {
                    // Get the local function and pass in the function from main function

                    if (cf.importFrom != "")
                    {
                        String[] importFromSplit = cf.importFrom.Split('/');
                        if (importFromSplit[0] == "@salesforce"
                            && importFromSplit.Length == 3)
                        {
                            sw.Write((tabCount + 1).ToString());

                            for (Int32 t = 0; t < tabCount + 1; t++)
                            {
                                sw.Write('\t');
                            }

                            sw.WriteLine(importFromSplit[2] + "\t--" + importFromSplit[1]);
                        }
                        else if (importFromSplit[0] == "@salesforce"
                            && importFromSplit.Length == 2)
                        {
                            sw.Write((tabCount + 1).ToString());

                            for (Int32 t = 0; t < tabCount + 1; t++)
                            {
                                sw.Write('\t');
                            }

                            sw.WriteLine(cf.functionName + "\t--" + importFromSplit[1]);
                        }
                        else if (importFromSplit[0] == "lightning")
                        {
                            sw.Write((tabCount + 1).ToString());

                            for (Int32 t = 0; t < tabCount + 1; t++)
                            {
                                sw.Write('\t');
                            }

                            sw.WriteLine(cf.folderName + "." + cf.fileName + "." + cf.functionName);
                        }
                        else if (importFromSplit[0] == ".")
                        {
                            String importKey = (importFromSplit[1] + "|" + importFromSplit[1]).ToLower();

                            foreach (String fileHierKey in this.jsFileHierarchyDict.Keys)
                            {
                                if (fileHierKey == importKey)
                                {
                                    foreach (JSFunction cfunc in jsFileHierarchyDict[fileHierKey].functions)
                                    {
                                        if (cfunc.functionName == cf.functionName)
                                        {
                                            writeSubFunctions(jsFunctionToWireFunction,
                                                              jsLocalFunctionDictionary,
                                                              cfunc,
                                                              tabCount + 1,
                                                              sw);
                                            break;
                                        }
                                    }

                                    break;
                                }
                            }
                        }
                        else if (importFromSplit[0] == "c")
                        {
                            String importKey = (importFromSplit[1] + "|" + importFromSplit[1]).ToLower();

                            foreach (String fileHierKey in this.jsFileHierarchyDict.Keys)
                            {
                                if (fileHierKey == importKey)
                                {
                                    foreach (JSFunction cfunc in jsFileHierarchyDict[fileHierKey].functions)
                                    {
                                        if (cfunc.functionName == cf.functionName)
                                        {
                                            writeSubFunctions(jsFunctionToWireFunction,
                                                              jsLocalFunctionDictionary,
                                                              cfunc,
                                                              tabCount + 1,
                                                              sw);
                                            break;
                                        }
                                    }

                                    break;
                                }
                            }
                        }
                    }
                    else if (cf.componentReferenceVar != "")
                    {
                        sw.Write((tabCount + 1).ToString() + '\t');
                        sw.WriteLine(cf.folderName + "." + cf.fileName + "." + cf.functionName + " - " + cf.componentReferenceVar);
                    }
                    else if (cf.isLocal
                             && cf.isGetter == false
                             && cf.isSetter == false
                             && jsLocalFunctionDictionary.ContainsKey(cf.folderName + "." + cf.fileName + "." + cf.functionName))
                    {
                        JSFunction childFunction = jsLocalFunctionDictionary[cf.folderName + "." + cf.fileName + "." + cf.functionName];

                        if (childFunction.functionName != func.functionName)
                        {
                            writeSubFunctions(jsFunctionToWireFunction,
                                              jsLocalFunctionDictionary,
                                              childFunction,
                                              tabCount + 1,
                                              sw);
                        }
                        else
                        {
                            sw.Write((tabCount + 1).ToString());
                            writeFunctionToFile(sw, tabCount + 1, cf);
                        }
                    }
                    else if (cf.isLocal == false)
                    {
                        if (jsLocalFunctionDictionary.ContainsKey(cf.folderName + "." + cf.fileName + "." + cf.functionName))
                        {
                            JSFunction childFunction = jsLocalFunctionDictionary[cf.folderName + "." + cf.fileName + "." + cf.functionName];

                            if (childFunction.functionName != func.functionName)
                            {

                                writeSubFunctions(jsFunctionToWireFunction,
                                                  jsLocalFunctionDictionary,
                                                  childFunction,
                                                  tabCount + 1,
                                                  sw);
                            }
                            else
                            {
                                sw.Write((tabCount + 1).ToString());
                                writeFunctionToFile(sw, tabCount + 1, cf);
                            }
                        }
                        else if (jsFunctionToWireFunction.ContainsKey(cf.folderName + "." + cf.fileName + "." + cf.functionName))
                        {
                            Debug.WriteLine("");
                        }
                        else
                        {
                            sw.Write((tabCount + 1).ToString());
                            writeFunctionToFile(sw, tabCount + 1, cf);
                        }
                    }
                    else
                    {
                        sw.Write((tabCount + 1).ToString());
                        writeFunctionToFile(sw, tabCount + 1, cf);
                    }
                }
            }
        }


        private void writeFunctionToFile(StreamWriter sw, Int32 tabCount, JSFunction function)
        {
            //sw.Write(tabCount.ToString());

            for (Int32 t = 0; t < tabCount; t++)
            {
                sw.Write('\t');
            }

            sw.Write(function.folderName + "." + function.fileName + "." + function.functionName + "(");
            String functParams = "";
            if (function.parameters.Count > 0)
            {
                foreach (String param in function.parameters)
                {
                    functParams = functParams + param + ", ";
                }

                functParams = functParams.Substring(0, functParams.Length - 2);
            }

            sw.Write(functParams + ")");
            sw.Write(Environment.NewLine);
        }


        private class JSFileHierarchy
        {
            public String folderName;
            public String fileName;
            public List<JSImport> imports;
            public List<JSConstant> constants;
            public List<String> exports;
            public List<JSFunction> functions;
            public List<JSProperty> properties;

            public JSFileHierarchy()
            {
                folderName = "";
                fileName = "";
                imports = new List<JSImport>();
                constants = new List<JSConstant>();
                exports = new List<String>();
                functions = new List<JSFunction>();
                properties = new List<JSProperty>();
            }
        }

        private class JSImport
        {
            public String folderName;
            public String fileName;
            public List<String> importItems;
            public String importFrom;

            public JSImport()
            {
                folderName = "";
                fileName = "";
                importItems = new List<string>();
                importFrom = "";
            }
        }

        private class JSConstant
        {
            public String folderName;
            public String fileName;
            public String constantName;
            public String constantValue;
            public String componentName;
            public String variableName;
            public JSConstant()
            {
                folderName = "";
                fileName = "";
                constantName = "";
                constantValue = "";
                componentName = "";
                variableName = "";
            }
        }

        private class JSProperty
        {
            public String folderName;
            public String fileName;
            public String propertyAnnotation;
            public String propertyName;
            public String propertyValue;
            public Boolean isExported;
            public String whereSet;
            public String componentReference;

            public JSProperty()
            {
                folderName = "";
                fileName = "";
                propertyAnnotation = "";
                propertyName = "";
                propertyValue = "";
                isExported = false;
                whereSet = "";
                componentReference = "";
            }
        }

        private class JSFunction
        {
            public String folderName;
            public String fileName;
            public String functionAnnotation;
            public String functionDesignation;
            public String functionName;
            public String parentFunction;
            // The component where the function is called from
            // This can be set in a CONST, as a parameter, as a local variable
            // If it is local, then the 
            public String componentReferenceVar;
            public String parameterSet;
            public String valueSet;
            public String functionWithWireAnnotated;
            public String importFrom;
            public String returnValue;
            public Boolean isWireFunction;
            public Boolean isStaticFunction;
            public Boolean isExported;
            public Boolean isGetter;
            public Boolean isSetter;
            public Boolean isLocal;
            public List<String> parameters;
            public List<JSFunction> childFunctions;
            public List<JSProperty> propertiesSet;

            public JSFunction()
            {
                folderName = "";
                fileName = "";
                functionAnnotation = "";
                functionDesignation = "";
                functionName = "";
                parentFunction = "";
                componentReferenceVar = "";
                parameterSet = "";
                valueSet = "";
                functionWithWireAnnotated = "";
                importFrom = "";
                returnValue = "";
                isWireFunction = false;
                isStaticFunction = false;
                isExported = false;
                isGetter = false;
                isSetter = false;
                isLocal = false;
                parameters = new List<string>();
                childFunctions = new List<JSFunction>();
                propertiesSet = new List<JSProperty>();
            }
        }

        private void btnConsolidateAll_Click(object sender, EventArgs e)
        {
        }
    }

}
