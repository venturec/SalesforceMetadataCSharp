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
                        Console.WriteLine(subDirectoryList[i]);

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
            jsFileHierarchyDict = new Dictionary<string, JSFileHierarchy>();

            // get the text values into a List<string> and then add them to the 
            Dictionary<String, List<String>> fileToParsedContent = new Dictionary<String, List<String>>();
            if (jsFiles.Count > 0)
            {
                foreach (String fileName in jsFiles)
                {
                    importDictionary = new Dictionary<String, String>();

                    String[] filePathSplit = fileName.Split('\\');
                    String[] fileNameSplit = filePathSplit[filePathSplit.Length - 1].Split('.');

                    String folderName = filePathSplit[filePathSplit.Length - 2];

                    StreamReader sr = new StreamReader(fileName);


                    // Read the line and add to the stringArrayList to loop through as individual characters
                    List<String> stringArray = new List<string>();
                    Boolean isComment = false;
                    Boolean isConsoleLog = false;
                    while (!sr.EndOfStream)
                    {
                        String[] parsedLine = readLineSplit(sr.ReadLine());
                        for (Int32 i = 0; i < parsedLine.Length; i++)
                        {
                            if (parsedLine[i].StartsWith("/*"))
                            {
                                isComment = true;
                            }
                            else if (parsedLine[i].EndsWith("*/"))
                            {
                                isComment = false;
                            }
                            else if (isComment == false
                                && (parsedLine[i] == "//"
                                || parsedLine[i].StartsWith("//")))
                            {
                                break;
                            }
                            else if (parsedLine[i].StartsWith("console.log"))
                            {
                                isConsoleLog = true;
                            }
                            else if (isConsoleLog == true
                                && parsedLine[i] == ";")
                            {
                                isConsoleLog = false;
                            }
                            else if (isComment == false
                                && isConsoleLog == false)
                            {
                                if (parsedLine[i] != "")
                                {
                                    stringArray.Add(parsedLine[i]);
                                }
                            }
                        }
                    }

                    sr.Close();

                    // Now loop through the text array and determine the types, constructing them using the inner classes

                    // This is more of a catch all in case the parser misses an end of block character or, in the case of much of LWC components, the develoepr
                    // forgets to end their statement with a ";".
                    // There's really no other reason to have this variable.

                    Boolean isExported = false;

                    Int32 arrayPos = 0;
                    for (Int32 i = 0; i < stringArray.Count; i++)
                    {
                        if (arrayPos < i) arrayPos = i;

                        if (arrayPos == i)
                        {
                            if (stringArray[i].ToLower() == "@api")
                            {
                                // Determine if the item is a variable or function
                                if (stringArray.Count > i + 2 && stringArray[i + 2] == ";")
                                {
                                    arrayPos = parseProperty(folderName, fileNameSplit[0], stringArray, i, isExported);
                                }
                                else if (stringArray.Count > i + 2 && stringArray[i + 2] == "=")
                                {
                                    arrayPos = parseProperty(folderName, fileNameSplit[0], stringArray, i, isExported);
                                }
                                else if (stringArray.Count > i + 4 && stringArray[i + 4] == ";")
                                {
                                    arrayPos = parseProperty(folderName, fileNameSplit[0], stringArray, i, isExported);
                                }
                                else if (stringArray.Count > i + 7 && stringArray[i + 7] == ";")
                                {
                                    arrayPos = parseProperty(folderName, fileNameSplit[0], stringArray, i, isExported);
                                }
                                else
                                {
                                    arrayPos = parseFunction(folderName, fileNameSplit[0], stringArray, i, isExported);
                                }
                            }
                            else if (stringArray[i].ToLower() == "@track")
                            {
                                // I believe this is a variable / property
                                arrayPos = parseProperty(folderName, fileNameSplit[0], stringArray, i, isExported);
                            }
                            else if (stringArray[i].ToLower() == "@wire")
                            {
                                arrayPos = parseWireFunction(folderName, fileNameSplit[0], stringArray, i, isExported);
                            }
                            else if (stringArray[i].ToLower() == "const")
                            {
                                arrayPos = parseConstant(folderName, fileNameSplit[0], stringArray, i);
                            }
                            else if (stringArray[i].ToLower() == "export")
                            {
                                if (stringArray[i].ToLower() == "export"
                                    && stringArray[i + 1].ToLower() == "default")
                                {
                                    isExported = true;
                                    arrayPos = parseExportDefault(stringArray, i);
                                }
                                else if (stringArray[i].ToLower() == "export"
                                    && stringArray[i + 1].ToLower() == "class")
                                {
                                    arrayPos = parseExportDefault(stringArray, i);
                                }
                                else
                                {
                                    // Call export function parser
                                    arrayPos = parseExport(folderName, fileNameSplit[0], stringArray, i);
                                }
                            }
                            else if (stringArray[i].ToLower() == "function")
                            {
                                arrayPos = parseFunction(folderName, fileNameSplit[0], stringArray, i, isExported);
                            }
                            else if (stringArray[i].ToLower() == "let")
                            {
                                arrayPos = parseProperty(folderName, fileNameSplit[0], stringArray, i, isExported);
                            }
                            else if (stringArray[i].ToLower() == "import")
                            {
                                arrayPos = parseImport(folderName, fileNameSplit[0], stringArray, i);
                            }
                            else if (stringArray[i].ToLower() == "static")
                            {
                                if (stringArray.Count > i + 1 && stringArray[i + 1] == ";")
                                {
                                    arrayPos = parseProperty(folderName, fileNameSplit[0], stringArray, i, isExported);
                                }
                                else if (stringArray.Count > i + 1 && stringArray[i + 1] == "=")
                                {
                                    arrayPos = parseProperty(folderName, fileNameSplit[0], stringArray, i, isExported);
                                }
                                else if (stringArray.Count > i + 3 && stringArray[i + 3] == ";")
                                {
                                    arrayPos = parseProperty(folderName, fileNameSplit[0], stringArray, i, isExported);
                                }
                                else if (stringArray.Count > i + 6 && stringArray[i + 6] == ";")
                                {
                                    arrayPos = parseProperty(folderName, fileNameSplit[0], stringArray, i, isExported);
                                }
                                else
                                {
                                    arrayPos = parseFunction(folderName, fileNameSplit[0], stringArray, i, isExported);
                                }
                            }
                            else
                            {
                                if (stringArray.Count > i + 1 && stringArray[i + 1] == ";")
                                {
                                    arrayPos = parseProperty(folderName, fileNameSplit[0], stringArray, i, isExported);
                                }
                                else if (stringArray.Count > i + 1 && stringArray[i + 1] == "=")
                                {
                                    arrayPos = parseProperty(folderName, fileNameSplit[0], stringArray, i, isExported);
                                }
                                else if (stringArray.Count > i + 3 && stringArray[i + 3] == ";")
                                {
                                    arrayPos = parseProperty(folderName, fileNameSplit[0], stringArray, i, isExported);
                                }
                                else if (stringArray.Count > i + 6 && stringArray[i + 6] == ";")
                                {
                                    arrayPos = parseProperty(folderName, fileNameSplit[0], stringArray, i, isExported);
                                }
                                else if (stringArray[i] == "}" && isExported == true)
                                {
                                    isExported = false;
                                }
                                else
                                {
                                    arrayPos = parseFunction(folderName, fileNameSplit[0], stringArray, i, isExported);
                                }
                            }
                        }
                    }
                }
            }


            // Now loop through the jsFileHieararchyDict and associate the calling fumctions to the function itself with a hierarchical value;
            //Boolean loopJSFileHieararchyDict = true;

            // Write the values for both functions and properties to the file
            Boolean writeToFile = true;
            writeFunctionsToFile();

        }

        private String[] readLineSplit(String line)
        {
            if (line != null)
            {
                String readLine = line.Trim();
                readLine = readLine.Replace("(", " ( ");
                readLine = readLine.Replace(")", " ) ");
                readLine = readLine.Replace("{", " { ");
                readLine = readLine.Replace("}", " } ");
                readLine = readLine.Replace("[", " [ ");
                readLine = readLine.Replace("]", " ] ");
                readLine = readLine.Replace(",", " , ");
                readLine = readLine.Replace("!==", " !== ");
                readLine = readLine.Replace("!=", " != ");
                readLine = readLine.Replace("===", " === ");
                readLine = readLine.Replace("==", " == ");
                readLine = readLine.Replace("=", " = ");
                readLine = readLine.Replace(";", " ;");
                readLine = readLine.Replace(":", " : ");
                readLine = readLine.Replace("&&", " && ");
                readLine = readLine.Replace("||", " || ");
                readLine = readLine.Replace("'", " ' ");
                readLine = readLine.Replace("\"", " \" ");
                readLine = readLine.Replace("\t", " ");
                //readLine = readLine.Replace(".", " . ");

                String[] rLineSplit = readLine.Split(' ');
                List<String> splitStringList = new List<string>();

                Int32 arrayPos = 0;
                Boolean isStringValue = false;
                String stringValue = "";

                for (Int32 i = 0; i < rLineSplit.Length; i++)
                {
                    if (rLineSplit[i] != "")
                    {
                        splitStringList.Add(rLineSplit[i]);
                    }
                }

                rLineSplit = splitStringList.ToArray();
                splitStringList.Clear();

                for (Int32 i = 0; i < rLineSplit.Length; i++)
                {
                    if (arrayPos < i) arrayPos = i;

                    if (arrayPos == i)
                    {
                        if (rLineSplit.Length > i + 2 &&  isStringValue == false && rLineSplit[i] == "=" && rLineSplit[i + 1] == "=" && rLineSplit[i + 2] == "=")
                        {
                            splitStringList.Add("===");
                            arrayPos = i + 3;
                        }
                        else if (rLineSplit.Length > i + 2 && isStringValue == false && rLineSplit[i] == "!" && rLineSplit[i + 1] == "=" && rLineSplit[i + 2] == "=")
                        {
                            splitStringList.Add("!==");
                            arrayPos = i + 3;
                        }
                        else if (rLineSplit.Length > i + 1 &&  isStringValue == false && rLineSplit[i] == "!" && rLineSplit[i + 1] == "=")
                        {
                            splitStringList.Add("!=");
                            arrayPos = i + 2;
                        }
                        else if (rLineSplit.Length > i + 1 &&  isStringValue == false && rLineSplit[i] == "=" && rLineSplit[i + 1] == "=")
                        {
                            splitStringList.Add("==");
                            arrayPos = i + 2;
                        }
                        else if (isStringValue == false &&
                            (rLineSplit[i] == "'" || rLineSplit[i] == "\""))
                        {
                            isStringValue = true;
                        }
                        else if (isStringValue == true &&
                            (rLineSplit[i] == "'" || rLineSplit[i] == "\""))
                        {
                            isStringValue = false;
                            splitStringList.Add(stringValue.Trim());

                            stringValue = "";
                        }
                        else if (isStringValue == false)
                        {
                            splitStringList.Add(rLineSplit[i]);
                        }
                        else if (isStringValue == true)
                        {
                            stringValue = stringValue + rLineSplit[i] + " ";
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

        private Int32 parseConstant(String folderName, String fileName, List<String> stringArray, Int32 characterPos)
        {
            JSConstant constant = new JSConstant();
            constant.folderName = folderName;
            constant.fileName = fileName;

            Int32 newPos = characterPos;
            String constVal = "";
            for (Int32 i = characterPos; i < stringArray.Count; i++)
            {
                if (stringArray[i] == ";")
                {
                    if (constVal != "")
                    {
                        constant.constantValue = constVal;
                        constVal = "";
                    }

                    newPos = i + 1;
                    break;
                }
                else if (stringArray[i].ToLower() == "const")
                {
                    constant.constantName = stringArray[i + 1];
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


            return newPos;
        }

        private Int32 parseExport(String folderName, String fileName, List<String> stringArray, Int32 characterPos)
        {
            Int32 braceCount = 0;

            Int32 newPos = characterPos;
            String exports = "";

            for (Int32 i = characterPos; i < stringArray.Count; i++)
            {
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
                }
                else if (braceCount == 1)
                {
                    exports = exports + stringArray[i];
                }
            }

            return newPos;
        }

        private Int32 parseExportDefault(List<String> stringArray, Int32 characterPos)
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

        private Int32 parseFunction(String folderName, String fileName, List<String> stringArray, Int32 characterPos, Boolean isExported)
        {
            JSFunction function = new JSFunction();
            function.folderName = folderName;
            function.fileName = fileName;
            function.isExported = isExported;

            Int32 braceCount = 0;
            Int32 parenthCount = 0;

            Int32 newPos = characterPos;
            String functionParameters = "";
            Boolean skipToBrace = false;
            Boolean setParameters = false;

            for (Int32 i = characterPos; i < stringArray.Count; i++)
            {
                //Debug.WriteLine(i.ToString() + " " + stringArray[i]);

                if (newPos < i) newPos = i;

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
                    else if (stringArray[i].ToLower() == "return")
                    {
                        Int32 j = 0;
                        for (j = i + 1; j < stringArray.Count; j++)
                        {
                            if (stringArray[j] == ";")
                            {
                                newPos = j + 1;
                                break;
                            }
                            else
                            {
                                function.returnValue = function.returnValue + stringArray[j];
                            }
                        }
                    }
                    else if (stringArray[i].ToLower() == "("
                             && braceCount == 0)
                    {
                        parenthCount++;
                        setParameters = true;
                    }
                    else if (stringArray[i].ToLower() == ")")
                    {
                        parenthCount--;
                        setParameters = false;
                    }
                    else if (stringArray[i].ToLower() == "{")
                    {
                        skipToBrace = false;
                        braceCount++;
                    }
                    else if (stringArray[i].ToLower() == "}")
                    {
                        braceCount--;
                        setParameters = false;

                        if (stringArray.Count > i + 2
                            && stringArray[i + 1] != ")"
                            && stringArray[i + 2] != "{"
                            && braceCount == 0)
                        {
                            newPos = i + 1;
                            break;
                        }
                        else if (stringArray.Count > i + 1
                            && stringArray[i + 1] != ")"
                            && braceCount == 0)
                        {
                            newPos = i + 1;
                            break;
                        }
                    }
                    else if (stringArray[i].ToLower() == "if")
                    {
                        skipToBrace = true;
                    }
                    else if (stringArray[i].ToLower() == "else")
                    {
                        skipToBrace = true;
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
                        String[] splitPropertyOrFunction = stringArray[i].Split('.');

                        // Is Property or Function?
                        if (stringArray[i + 1] == "=")
                        {
                            function.propertiesSet.Add(splitPropertyOrFunction[1]);
                            newPos = i;
                            for (Int32 j = newPos; j < stringArray.Count; j++)
                            {
                                if (stringArray[j] == ";")
                                {
                                    newPos = j + 1;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            JSFunction cf = new JSFunction();
                            cf.folderName = folderName;
                            cf.fileName = fileName;
                            cf.isLocalFunction = true;

                            String[] functionName = stringArray[i].Split('.');
                            cf.functionName = functionName[1];

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
                    }
                    else if (skipToBrace == false
                        && stringArray[i].ToLower().StartsWith(".then"))
                    {
                        Int32 thenParenthCount = 0;
                        for (Int32 j = i + 1; j < stringArray.Count; j++)
                        {
                            if (stringArray[j] == "(")
                            {
                                thenParenthCount++;
                            }
                            else if (stringArray[j] == ")")
                            {
                                thenParenthCount--;
                            }

                            if (thenParenthCount == 0
                                && stringArray[j] == ";")
                            {
                                newPos = j + 1;
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

                        if (importDictionary.ContainsKey(stringArray[i]))
                        {
                            cf.importFrom = importDictionary[stringArray[i]];
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
                        Debug.WriteLine(folderName + "." + fileName + " : " + stringArray[i]);
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

            return newPos;
        }

        private Int32 parseImport(String folderName, String fileName, List<String> stringArray, Int32 characterPos)
        {
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
                            this.importDictionary.Add(imp, stringArray[i + 1]);
                        }

                        imports = "";
                    }

                    import.importFrom = stringArray[i + 1];
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

            return newPos;
        }

        private Int32 parseProperty(String folderName, String fileName, List<String> stringArray, Int32 characterPos, Boolean isExported)
        {
            JSProperty property = new JSProperty();
            property.folderName = folderName;
            property.fileName = fileName;
            property.isExported = isExported;

            Int32 newPos = characterPos;

            Boolean setPropertyValue = false;
            String propertyName = "";
            String propertyValue = "";

            for (Int32 i = characterPos; i < stringArray.Count; i++)
            {
                if (newPos < i) newPos = i;

                if (newPos == i)
                {
                    if (stringArray[i].ToLower() == "@api")
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
                        propertyName = stringArray[i - 1];
                        propertyValue = stringArray[i + 1];

                        setPropertyValue = true;

                        newPos = i + 2;
                    }
                    else if (stringArray[i] == "!="
                        || stringArray[i] == "!==")
                    {
                        propertyName = stringArray[i - 1];
                        propertyValue = stringArray[i + 1];

                        setPropertyValue = true;

                        newPos = i + 2;
                    }
                    else if (stringArray[i] == "}")
                    {
                        propertyValue = propertyValue + stringArray[i];

                        property.propertyName = propertyName;
                        property.propertyValue = propertyValue;

                        setPropertyValue = false;

                        propertyName = "";
                        propertyValue = "";

                        newPos = i + 1;
                        break;
                    }
                    else if (stringArray[i] == ";")
                    {
                        property.propertyName = propertyName;
                        property.propertyValue = propertyValue;
                        propertyName = "";
                        propertyValue = "";

                        setPropertyValue = false;

                        newPos = i + 1;
                        break;
                    }
                    else if (setPropertyValue)
                    {
                        propertyValue = propertyValue + stringArray[i];
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

            return newPos;
        }

        private Int32 parseWireFunction(String folderName, String fileName, List<String> stringArray, Int32 characterPos, Boolean isExported)
        {
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

            return newPos;
        }

        private void writeFunctionsToFile()
        {
            StreamWriter sw = new StreamWriter(this.tbSaveResultsTo.Text + "\\LWCFunctionHierarchy.txt");

            foreach (String compFile in this.jsFileHierarchyDict.Keys)
            {
                // Key = import | Value = import from
                //Dictionary<String, String> importsDictionary = new Dictionary<String, String>();

                //foreach (JSImport imp in this.jsFileHierarchyDict[compFile].imports)
                //{
                //    foreach (String importItem in imp.importItems)
                //    {
                //        importsDictionary.Add(importItem, imp.importFrom);
                //    }
                //}

                // Key = JS function name => Value = wire call
                //Dictionary<String, String> jsFunctionToWireFunction = new Dictionary<String, String>();

                //foreach (JSFunction func in this.jsFileHierarchyDict[compFile].functions)
                //{
                //    if (func.isWireFunction == true)
                //    {
                //        jsFunctionToWireFunction.Add(func.functionWithWireAnnotated, func.functionName);
                //    }
                //}

                //// Now write the function hierarchy to the file
                //foreach (JSFunction func in this.jsFileHierarchyDict[compFile].functions)
                //{
                //    // Write the function in this process as the parent function. 
                //    // If there are related function calls, then write those related functions incrementing the tab
                //    writeSubFunctions(importsDictionary, jsFunctionToWireFunction, func, 1, sw);
                //}

                foreach (JSFunction func in this.jsFileHierarchyDict[compFile].functions)
                {
                    sw.WriteLine(func.folderName + '\t' + func.fileName + '\t' + func.functionName);
                }
            }

            sw.Close();

        }

        private void writeSubFunctions(Dictionary<String, String> importsDictionary, 
                                       Dictionary<String, String> jsFunctionToWireFunction,
                                       JSFunction func,
                                       Int32 tabCount,
                                       StreamWriter sw)
        {
            for (Int32 t = 0; t < tabCount; t++)
            {
                sw.Write('\t');
            }

            sw.WriteLine(func.functionAnnotation + " - " + func.folderName + "." + func.fileName + "." + func.functionName);
            
            // If a wire function exists, then get the wire import and import from
            if (jsFunctionToWireFunction.ContainsKey(func.functionName))
            {

            }

            if (func.childFunctions.Count > 0)
            {
                foreach (JSFunction cf in func.childFunctions)
                {
                    writeSubFunctions(importsDictionary, jsFunctionToWireFunction, cf, tabCount + 1, sw);
                }
            }
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
            public JSConstant()
            {
                folderName = "";
                fileName = "";
                constantName = "";
                constantValue = "";
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

            public JSProperty()
            {
                folderName = "";
                fileName = "";
                propertyAnnotation = "";
                propertyName = "";
                propertyValue = "";
                isExported = false;
            }
        }

        private class JSFunction
        {
            public String folderName;
            public String fileName;
            public String functionAnnotation;
            public String functionDesignation;
            public String functionName;
            public String parameterSet;
            public String valueSet;
            public String functionWithWireAnnotated;
            public String importFrom;
            public String returnValue;
            public Boolean isWireFunction;
            public Boolean isStaticFunction;
            public Boolean isExported;
            public Boolean isGetter;
            public Boolean isLocalFunction;
            public List<String> parameters;
            public List<JSFunction> childFunctions;
            public List<String> propertiesSet;

            public JSFunction()
            {
                folderName = "";
                fileName = "";
                functionAnnotation = "";
                functionDesignation = "";
                functionName = "";
                parameterSet = "";
                valueSet = "";
                functionWithWireAnnotated = "";
                importFrom = "";
                returnValue = "";
                isWireFunction = false;
                isStaticFunction = false;
                isExported = false;
                isGetter = false;
                isLocalFunction = false;
                parameters = new List<string>();
                childFunctions = new List<JSFunction>();
                propertiesSet = new List<string>();
            }
        }

        //private class ChildFunction
        //{
        //    public String folderName;
        //    public String fileName;
        //    public String functionDesignation;
        //    public String functionName;
        //    public List<String> functionParameters;
        //    public String valueSet;
        //    public String importFrom;
        //    public Boolean localFunction;

        //    public ChildFunction()
        //    {
        //        folderName = "";
        //        fileName = "";
        //        functionDesignation = "";
        //        functionName = "";
        //        functionParameters = new List<string>();
        //        valueSet = "";
        //        importFrom = "";
        //        localFunction = false;
        //    }
        //}
    }

}
