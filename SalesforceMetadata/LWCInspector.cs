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
    public partial class LWCInspector : Form
    {
        private Dictionary<String, JSFunction> jsFunctionsDict;

        // Key = What file name the property or function is referenced in + the reference
        // Value = the file the above translates too so you can get the actual file the function is housed in
        // Ex: import { helper } from './jsfile'
        // Current jsFile name = otherJSFile
        // Key = otherJSFile|helper
        // Value = jsFile
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
            Dictionary<String, JSFileHierarchy> jsFileHieararchyDict = new Dictionary<string, JSFileHierarchy>();
            if (jsFiles.Count > 0)
            {
                foreach (String fileName in jsFiles)
                {
                    String[] filePathSplit = fileName.Split('\\');
                    String[] fileNameSplit = filePathSplit[filePathSplit.Length - 1].Split('.');

                    Dictionary<String, String> importReference = new Dictionary<String, String>();

                    JSFileHierarchy jsParsedFiles = new JSFileHierarchy();
                    jsParsedFiles.folderName = filePathSplit[filePathSplit.Length - 2];
                    jsParsedFiles.fileName = fileNameSplit[0];

                    StreamReader sr = new StreamReader(fileName);

                    Boolean isComment = false;
                    Boolean isQuoteValue = false;
                    while (!sr.EndOfStream)
                    {
                        String[] parsedLine = readLineSplit(sr.ReadLine());

                        // Parse the readLine because some developers think it's cool to add multi-line comments inline next to active variables
                        // Example: /*value*/ 'Opportunity_Competitor__c',
                        // The above is stupid and I sometimes wonder about the lack of logic on other developer's parts
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
                                break;
                            }
                            else if (isComment == false
                                && parsedLine[i] == "const")
                            {
                                JSConstant cons = new JSConstant();
                                cons.folderName = jsParsedFiles.folderName;
                                cons.fileName = jsParsedFiles.fileName;

                                Boolean endOfStatement = false;
                                while (endOfStatement == false)
                                {
                                    Boolean constantName = false;
                                    Boolean constantValue = false;
                                    for (Int32 j = 0; j < parsedLine.Length; j++)
                                    {
                                        if (parsedLine[j].ToLower() == "const")
                                        {
                                            constantName = true;
                                        }
                                        else if (parsedLine[j] == ";")
                                        {
                                            endOfStatement = true;
                                            constantValue = false;
                                        }
                                        else if (parsedLine[j] == "=")
                                        {
                                            constantValue = true;
                                        }
                                        else if (constantName == true)
                                        {
                                            cons.constantName = parsedLine[j];
                                            constantName = false;
                                        }
                                        else if (constantValue == true)
                                        {
                                            cons.constantValue = parsedLine[j];
                                        }
                                    }

                                    if (endOfStatement == false)
                                    {
                                        parsedLine = readLineSplit(sr.ReadLine());
                                    }
                                }

                                jsParsedFiles.constants.Add(cons);
                                break;
                            }
                            else if (isComment == false
                                && parsedLine[i] == "import")
                            {
                                JSImport imprt = new JSImport();
                                imprt.folderName = jsParsedFiles.folderName;
                                imprt.fileName = jsParsedFiles.fileName;

                                Boolean endOfStatement = false;
                                while (endOfStatement == false)
                                {
                                    Boolean insideBrace = false;
                                    Boolean fromReference = false;
                                    for (Int32 j = 0; j < parsedLine.Length; j++)
                                    {
                                        if (parsedLine[j] == "import")
                                        {
                                            // Don't do anything
                                        }
                                        else if (parsedLine[j] == "{")
                                        {
                                            insideBrace = true;
                                        }
                                        else if (parsedLine[j] == "}")
                                        {
                                            insideBrace = false;
                                        }
                                        else if (parsedLine[j].ToLower() == "from")
                                        {
                                            fromReference = true;
                                        }
                                        else if (parsedLine[j] == ",")
                                        {
                                            // Don't do anything
                                        }
                                        else if (parsedLine[j] == ";")
                                        {
                                            endOfStatement = true;
                                            fromReference = false;
                                        }
                                        else if (parsedLine[j] != ""
                                            && insideBrace)
                                        {
                                            imprt.importItems.Add(parsedLine[j].Trim());
                                        }
                                        else if (parsedLine[j] != ""
                                            && fromReference)
                                        {
                                            imprt.importFrom = parsedLine[j];
                                            imprt.importFrom = imprt.importFrom.Replace("\"", "");
                                            imprt.importFrom = imprt.importFrom.Replace("\'", "");

                                            if (imprt.importFrom.StartsWith("./")
                                                || imprt.importFrom.StartsWith("c/"))
                                            {
                                                imprt.importFrom = imprt.importFrom.Substring(2, imprt.importFrom.Length - 2);
                                            }

                                            if (imprt.importFrom.EndsWith(".js"))
                                            {
                                                imprt.importFrom = imprt.importFrom.Substring(0, imprt.importFrom.Length - 3);
                                            }

                                            imprt.importFrom = imprt.importFrom.Trim();

                                            foreach (String impItem in imprt.importItems)
                                            {
                                                importReference.Add(impItem, imprt.importFrom);
                                            }
                                        }
                                        else if (parsedLine[j] != ""
                                            && insideBrace == false
                                            && fromReference == false)
                                        {
                                            imprt.importItems.Add(parsedLine[j].Trim());
                                        }
                                    }

                                    if (endOfStatement == false)
                                    {
                                        parsedLine = readLineSplit(sr.ReadLine());
                                    }
                                }

                                jsParsedFiles.imports.Add(imprt);
                                break;
                            }
                            else if (isComment == false
                                && parsedLine[i] == "@wire")
                            {
                                JSFunction function = new JSFunction();
                                function.folderName = jsParsedFiles.folderName;
                                function.fileName = jsParsedFiles.fileName;
                                function.functionAnnotation = "@wire";
                                function.wireFunction = true;

                                Boolean firstBraceReached = false;
                                Boolean firstParenthReached = false;

                                String parameter = "";

                                Int32 parenthLeftCount = 1;
                                while (parenthLeftCount > 0)
                                {
                                    for (Int32 j = 0; j < parsedLine.Length; j++)
                                    {
                                        if (parsedLine[j] == "(")
                                        {
                                            firstParenthReached = true;
                                        }
                                        else if (parsedLine[j] == ")")
                                        {
                                            parenthLeftCount--;
                                        }
                                        else if (parsedLine[j] == "{")
                                        {
                                            firstBraceReached = true;
                                        }
                                        else if (parsedLine[j] == "}")
                                        {
                                            firstBraceReached = false;
                                            function.parameters.Add(parameter);
                                            parameter = "";
                                        }
                                        else if (parsedLine[j] != ""
                                            && firstBraceReached == true
                                            && parsedLine[j] == ",")
                                        {
                                            function.parameters.Add(parameter);
                                            parameter = "";
                                        }
                                        else if (parsedLine[j] != ""
                                            && firstParenthReached == true)
                                        {
                                            function.functionName = parsedLine[j];
                                            firstParenthReached = false;
                                        }
                                        else if (parsedLine[j] != ""
                                            && firstBraceReached == true)
                                        {
                                            parameter = parameter + parsedLine[j];
                                        }
                                    }

                                    if (parenthLeftCount > 0)
                                    {
                                        parsedLine = readLineSplit(sr.ReadLine());
                                    }
                                }

                                if (importReference.ContainsKey(function.functionName))
                                {
                                    function.importReference = importReference[function.functionName];
                                }

                                jsParsedFiles.functions.Add(function);
                                break;
                            }
                            else if(isComment == false)
                            {
                                // Let's figure out what this actually is and how we should handle those values
                                List<String> functionOrProperty = new List<String>();

                                Boolean isProperty = false;
                                Boolean isFunction = false;

                                Int32 braceLeftCount = 0;

                                if (parsedLine[i] != "")
                                {
                                    // Build the functionOrProperty list values excluding any blank spaces until you come across a 
                                    // ; or {
                                    // The exception to this rule are calls made back to salesforce within the imports
                                    Boolean findFunctionOrProperty = true;
                                    isComment = false;
                                    while (findFunctionOrProperty == true)
                                    {
                                        for (Int32 j = 0; j < parsedLine.Length; j++)
                                        {
                                            if (parsedLine[j] != "")
                                            {
                                                if (isComment == false
                                                    && parsedLine[j].StartsWith("//"))
                                                {
                                                    break;
                                                }
                                                else if (parsedLine[j].StartsWith("/*"))
                                                {
                                                    isComment = true;
                                                }
                                                else if (parsedLine[j].EndsWith("*/"))
                                                {
                                                    isComment = false;
                                                }
                                                else if (parsedLine[j].StartsWith("console.log"))
                                                {
                                                    break;
                                                }

                                                if (isComment == false
                                                    && parsedLine[j] == ";")
                                                {
                                                    isProperty = true;
                                                    findFunctionOrProperty = false;
                                                }

                                                if (isComment == false
                                                    && parsedLine[j] == "{")
                                                {
                                                    braceLeftCount = braceLeftCount + 1;
                                                    isFunction = true;
                                                    findFunctionOrProperty = false;
                                                }

                                                if (isComment == false
                                                    && !parsedLine[j].EndsWith("*/"))
                                                {
                                                    functionOrProperty.Add(parsedLine[j]);
                                                }
                                            }
                                        }

                                        //foreach (String cl in parsedLine)
                                        //{
                                        //    Console.Write(cl);
                                        //}
                                        //Console.WriteLine();

                                        if (findFunctionOrProperty == true)
                                        {
                                            parsedLine = readLineSplit(sr.ReadLine());
                                            if (parsedLine[0] == "")
                                            {
                                                findFunctionOrProperty = false;
                                            }
                                        }
                                    }

                                    // The following is mostly geared towards functions since the functions have a closing tag
                                    // We need to continue building the functionOrPropertyList until we reach the closing tag
                                    // For properties, since we have reached the ;, then we can add those to the inner class property list.
                                    if (isProperty == true)
                                    {
                                        Boolean propertyNameSet = false;
                                        JSProperty property = new JSProperty();
                                        property.folderName = jsParsedFiles.folderName;
                                        property.fileName = jsParsedFiles.fileName;

                                        for (int k = 0; k < functionOrProperty.Count; k++)
                                        {
                                            if (functionOrProperty[k] == "@api"
                                                || functionOrProperty[k] == "@track")
                                            {
                                                property.propertyAnnotation = functionOrProperty[k];
                                            }
                                            else if (functionOrProperty[k] == "="
                                                && propertyNameSet == false)
                                            {
                                                property.propertyName = functionOrProperty[k - 1];

                                                for (Int32 m = k + 1; m < functionOrProperty.Count - 1; m++)
                                                {
                                                    property.propertyValue = property.propertyValue + functionOrProperty[m] + " ";
                                                }

                                                property.propertyValue = property.propertyValue.Trim();

                                                propertyNameSet = true;
                                            }
                                            else if (functionOrProperty[k] == ";"
                                                && propertyNameSet == false)
                                            {
                                                property.propertyName = functionOrProperty[k - 1];
                                                propertyNameSet = true;
                                            }
                                        }

                                        functionOrProperty.Clear();

                                        //foreach(String cl in parsedLine)
                                        //{
                                        //    Console.Write(cl);
                                        //}
                                        //Console.WriteLine();

                                        if (propertyNameSet == true)
                                        {
                                            jsParsedFiles.properties.Add(property);
                                        }

                                        break;
                                    }
                                    else if (isFunction == true)
                                    {
                                        JSFunction function = new JSFunction();
                                        function.folderName = jsParsedFiles.folderName;
                                        function.fileName = jsParsedFiles.fileName;

                                        if (functionOrProperty[0].ToLower() == "export"
                                            && functionOrProperty[1].ToLower() == "default")
                                        {
                                            // Disregard the export default class stuff as this usually includes all related properties and functions.
                                            // but add it to the function inner class as a placeholder.
                                        }
                                        else if (functionOrProperty[0].ToLower() == "export"
                                            && functionOrProperty[1].ToLower() == "class")
                                        {
                                            // Disregard
                                        }
                                        else
                                        {
                                            // Get the entire function into the array before parsing it out
                                            isComment = false;
                                            isQuoteValue = false;
                                            while (braceLeftCount > 0)
                                            {
                                                parsedLine = readLineSplit(sr.ReadLine());

                                                for (Int32 j = 0; j < parsedLine.Length; j++)
                                                {
                                                    if (isComment == false
                                                        && parsedLine[j].StartsWith("//"))
                                                    {
                                                        break;
                                                    }
                                                    else if (parsedLine[j].StartsWith("/*"))
                                                    {
                                                        isComment = true;
                                                    }
                                                    else if (parsedLine[j].EndsWith("*/"))
                                                    {
                                                        isComment = false;
                                                    }
                                                    else if (parsedLine[j].StartsWith("console.log"))
                                                    {
                                                        break;
                                                    }


                                                    if (isComment == false
                                                        && parsedLine[j] == "{")
                                                    {
                                                        braceLeftCount++;
                                                    }
                                                    else if (isComment == false
                                                        && parsedLine[j] == "}")
                                                    {
                                                        braceLeftCount--;
                                                    }

                                                    if (isComment == false
                                                        && parsedLine[j] != "")
                                                    {
                                                        functionOrProperty.Add(parsedLine[j]);
                                                    }
                                                }
                                            }
                                        }

                                        // Now parse through the function and add all critical information.
                                        // Function name
                                        // Parameters
                                        // Any other function calls whether this.functionName with params or just ();
                                        if (functionOrProperty[0].ToLower() == "export"
                                            && functionOrProperty[1].ToLower() == "default")
                                        {
                                            String exportDefault = "";

                                            foreach (String fnct in functionOrProperty)
                                            {
                                                exportDefault = exportDefault + fnct + " ";
                                            }

                                            function.functionName = exportDefault;

                                            jsParsedFiles.functions.Add(function);

                                            break;
                                        }
                                        else if (functionOrProperty[0].ToLower() == "export"
                                            && functionOrProperty[1].ToLower() == "class")
                                        {
                                            String exportDefault = "";

                                            foreach (String fnct in functionOrProperty)
                                            {
                                                exportDefault = exportDefault + fnct + " ";
                                            }

                                            function.functionName = exportDefault;

                                            jsParsedFiles.functions.Add(function);

                                            break;
                                        }
                                        // Parse the rest of the function to filter out the calls to other functions and populate any variables
                                        // which are set for that function
                                        else
                                        {
                                            braceLeftCount = 0;
                                            Int32 parenthLeftCount = 0;

                                            Boolean inIfForLoop = false;
                                            Boolean addParameters = false;
                                            Boolean apexFunctionCall = false;

                                            String otherFunctionCalls = "";

                                            for (Int32 j = 0; j < functionOrProperty.Count; j++)
                                            {
                                                //Console.WriteLine("In function parsing");
                                                if (functionOrProperty[j] == "@api"
                                                    || functionOrProperty[j] == "@track")
                                                {
                                                    function.functionAnnotation = functionOrProperty[j];
                                                }
                                                else if (addParameters == false
                                                    && functionOrProperty[j].ToLower() == "static")
                                                {
                                                    function.staticFunction = true;
                                                }
                                                else if (addParameters == false
                                                    && functionOrProperty[j].ToLower() == "function")
                                                {
                                                    function.functionDesignation = "function";
                                                }
                                                else if (functionOrProperty[j] == "("
                                                    && functionOrProperty[j + 1] == "{")
                                                {
                                                    apexFunctionCall = true;
                                                    ChildFunction cf = new ChildFunction();
                                                    cf.folderName = function.folderName;
                                                    cf.fileName = function.fileName;
                                                    cf.functionName = otherFunctionCallsCleanup(otherFunctionCalls, true);

                                                    if (importReference.ContainsKey(cf.functionName))
                                                    {
                                                        cf.importFrom = importReference[cf.functionName];
                                                    }

                                                    function.childFunctions.Add(cf);
                                                }
                                                else if (apexFunctionCall == false
                                                    && functionOrProperty[j] == "(")
                                                {
                                                    parenthLeftCount++;

                                                    // Get the function name populated
                                                    if (parenthLeftCount == 1
                                                        && braceLeftCount == 0)
                                                    {
                                                        function.functionName = otherFunctionCallsCleanup(otherFunctionCalls, true);
                                                        otherFunctionCalls = "";
                                                        addParameters = true;
                                                    }
                                                    else if (inIfForLoop == false)
                                                    {
                                                        otherFunctionCalls = otherFunctionCalls + functionOrProperty[j] + " ";
                                                    }
                                                }
                                                else if (functionOrProperty[j] == ")"
                                                    && functionOrProperty[j - 1] == "}"
                                                    && apexFunctionCall == true)
                                                {
                                                    apexFunctionCall = false;
                                                    otherFunctionCalls = "";
                                                }
                                                else if (apexFunctionCall == false
                                                    && functionOrProperty[j] == ")")
                                                {
                                                    parenthLeftCount--;
                                                    addParameters = false;

                                                    if (inIfForLoop == false)
                                                    {
                                                        otherFunctionCalls = otherFunctionCalls + functionOrProperty[j] + " ";
                                                    }
                                                }
                                                else if (apexFunctionCall == false
                                                    && functionOrProperty[j] == "{")
                                                {
                                                    braceLeftCount++;

                                                    if (inIfForLoop == true)
                                                    {
                                                        inIfForLoop = false;
                                                    }
                                                    else if (inIfForLoop == false)
                                                    {
                                                        otherFunctionCalls = otherFunctionCalls + functionOrProperty[j] + " ";
                                                    }
                                                }
                                                else if (apexFunctionCall == false
                                                    && functionOrProperty[j] == "}")
                                                {
                                                    braceLeftCount--;

                                                    if (inIfForLoop == false)
                                                    {
                                                        otherFunctionCalls = otherFunctionCalls + functionOrProperty[j] + " ";
                                                    }
                                                }
                                                else if (functionOrProperty[j].ToLower() == "if")
                                                {
                                                    inIfForLoop = true;
                                                    otherFunctionCalls = "";
                                                }
                                                else if (functionOrProperty[j].ToLower() == "else")
                                                {
                                                    inIfForLoop = true;
                                                    otherFunctionCalls = "";
                                                }
                                                else if (functionOrProperty[j].ToLower() == "for")
                                                {
                                                    inIfForLoop = true;
                                                    otherFunctionCalls = "";
                                                }
                                                else if (parenthLeftCount == 1 && addParameters == true)
                                                {
                                                    if (functionOrProperty[j] != ",")
                                                    {
                                                        function.parameters.Add(functionOrProperty[j]);
                                                    }
                                                }
                                                else if (functionOrProperty[j] == ";")
                                                {
                                                    if (otherFunctionCalls != "")
                                                    {
                                                        // Clean up the otherFunctionCalls before adding it to the inner class
                                                        otherFunctionCalls = otherFunctionCallsCleanup(otherFunctionCalls, false);

                                                        if (otherFunctionCalls.Contains("("))
                                                        {
                                                            if (otherFunctionCalls.StartsWith("return "))
                                                            {
                                                                otherFunctionCalls = otherFunctionCalls.Substring(6, otherFunctionCalls.Length - 6);
                                                                otherFunctionCalls = otherFunctionCalls.Trim();
                                                                otherFunctionCalls = otherFunctionCalls.Trim();
                                                                otherFunctionCalls = otherFunctionCalls.Trim();
                                                            }

                                                            // Get the value or property which is set when this function runs.
                                                            //Console.WriteLine(otherFunctionCalls);

                                                            Int32 equalIdx = otherFunctionCalls.IndexOf('=');
                                                            Int32 leftParenIdx = otherFunctionCalls.IndexOf('(');

                                                            String valueSet = "";
                                                            String otherFunctionCallsTeamp = "";
                                                            if (equalIdx != -1 && equalIdx < leftParenIdx)
                                                            {
                                                                Char[] otherFunctionCharsForParam = otherFunctionCalls.ToCharArray();
                                                                Boolean firstEqualReached = false;
                                                                for (Int32 m = 0; m < otherFunctionCharsForParam.Length; m++)
                                                                {
                                                                    if (firstEqualReached == false
                                                                        && otherFunctionCharsForParam[m].ToString() == "=")
                                                                    {
                                                                        firstEqualReached = true;
                                                                    }
                                                                    else if (firstEqualReached == false)
                                                                    {
                                                                        valueSet += otherFunctionCharsForParam[m].ToString();
                                                                    }
                                                                    else
                                                                    {
                                                                        otherFunctionCallsTeamp += otherFunctionCharsForParam[m].ToString();
                                                                    }
                                                                }

                                                                otherFunctionCalls = otherFunctionCallsTeamp.Trim();
                                                            }

                                                            ChildFunction cf = new ChildFunction();
                                                            cf.folderName = function.folderName;
                                                            cf.fileName = function.fileName;
                                                            cf.valueSet = valueSet;

                                                            // The idea here is to check if the index of the '.' is before the first (, if there is one.
                                                            // That way, we don't accidentally assume there will always be a decimal and if there is a parameter with
                                                            // a decimal in it, it will nullify the results
                                                            // EX: helper.setValuesForOtherCalcInputsWrapper(thisPrdsCountryToOnrInfo, dealTotals, GLOBAL_COUNTRY_DEFAULT, false, family, sku, null, null, null);
                                                            //     setValuesForOtherCalcInputsWrapper(thisPrdsCountryToOnrInfo.Id, dealTotals, GLOBAL_COUNTRY_DEFAULT, false, family, sku, null, null, null);
                                                            // Assuming a decimal in the second function would be incorrect

                                                            Char[] otherFunctionChars = otherFunctionCalls.ToCharArray();

                                                            Int32 dotIdx = otherFunctionCalls.IndexOf('.');
                                                            leftParenIdx = otherFunctionCalls.IndexOf('(');

                                                            if (dotIdx != -1 && dotIdx < leftParenIdx)
                                                            {
                                                                // Populate the ChildFunction designation first
                                                                // Get the importFrom value based on the designation
                                                                for (Int32 m = 0; m < dotIdx; m++)
                                                                {
                                                                    cf.functionDesignation += otherFunctionChars[m].ToString();
                                                                    cf.functionDesignation = cf.functionDesignation.Trim();
                                                                }

                                                                if (cf.functionDesignation.StartsWith("="))
                                                                {
                                                                    cf.functionDesignation = cf.functionDesignation.Substring(1, cf.functionDesignation.Length - 1);
                                                                    cf.functionDesignation = cf.functionDesignation.Trim();
                                                                }

                                                                if (importReference.ContainsKey(cf.functionDesignation))
                                                                {
                                                                    cf.importFrom = importReference[cf.functionDesignation];
                                                                }

                                                                for (Int32 m = dotIdx + 1; m < leftParenIdx; m++)
                                                                {
                                                                    cf.functionName += otherFunctionChars[m].ToString();
                                                                    cf.functionName = cf.functionName.Trim();
                                                                    if (cf.functionName.StartsWith("!"))
                                                                    {
                                                                        cf.functionName = cf.functionName.Substring(1);
                                                                    }
                                                                }

                                                                for (Int32 m = leftParenIdx; m < otherFunctionChars.Length; m++)
                                                                {
                                                                    cf.functionParameters += otherFunctionChars[m].ToString();
                                                                    cf.functionParameters = cf.functionParameters.Trim();
                                                                }

                                                                if (cf.functionName.ToLower() != "foreach"
                                                                    && !cf.functionName.EndsWith(".forEach"))
                                                                {
                                                                    function.childFunctions.Add(cf);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                for (Int32 m = 0; m < leftParenIdx; m++)
                                                                {
                                                                    cf.functionName += otherFunctionChars[m].ToString();
                                                                    cf.functionName = cf.functionName.Trim();

                                                                    if (cf.functionName.StartsWith("!"))
                                                                    {
                                                                        cf.functionName = cf.functionName.Substring(1);
                                                                    }
                                                                }

                                                                if (cf.functionName.StartsWith("="))
                                                                {
                                                                    cf.functionName = cf.functionName.Substring(1, cf.functionName.Length - 1);
                                                                    cf.functionName = cf.functionName.Trim();
                                                                }

                                                                for (Int32 m = leftParenIdx; m < otherFunctionChars.Length; m++)
                                                                {
                                                                    cf.functionParameters += otherFunctionChars[m].ToString();
                                                                    cf.functionParameters = cf.functionParameters.Trim();
                                                                }

                                                                if (cf.functionName.ToLower() != "foreach"
                                                                    && !cf.functionName.EndsWith(".forEach"))
                                                                {
                                                                    function.childFunctions.Add(cf);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            function.propertiesSet.Add(otherFunctionCalls);
                                                        }

                                                        otherFunctionCalls = "";
                                                    }
                                                }
                                                else if (inIfForLoop == false)
                                                {
                                                    otherFunctionCalls = otherFunctionCalls + functionOrProperty[j] + " ";
                                                }
                                            }
                                        }

                                        jsParsedFiles.functions.Add(function);

                                        isFunction = false;
                                        functionOrProperty.Clear();
                                        
                                        break;
                                    }
                                    else
                                    {
                                        functionOrProperty.Clear();
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    sr.Close();

                    jsFileHieararchyDict.Add(jsParsedFiles.folderName + "." + jsParsedFiles.fileName, jsParsedFiles);

                }
            }


            // After the above is done, then start writing the values to the file
            if (jsFileHieararchyDict.Count > 0)
            {
                if (!this.tbSaveResultsTo.Text.EndsWith("\\"))
                {
                    this.tbSaveResultsTo.Text = this.tbSaveResultsTo.Text + "\\";
                }

                StreamWriter sw = new StreamWriter(this.tbSaveResultsTo.Text + "LWCFunctionHierarchy.txt");

                // Key = fileName + function name => function
                this.jsFunctionsDict = new Dictionary<String, JSFunction>();
                this.importDictionary = new Dictionary<String, String>();
                foreach (JSFileHierarchy jsFileHier in jsFileHieararchyDict.Values)
                {
                    foreach (JSFunction fnc in jsFileHier.functions)
                    {
                        if (!fnc.functionName.StartsWith("export"))
                        {
                            String key = fnc.fileName + "|" + fnc.functionName;
                            if (!jsFunctionsDict.ContainsKey(key))
                            {
                                this.jsFunctionsDict.Add(key, fnc);
                            }
                        }
                    }

                    foreach (JSImport imp in jsFileHier.imports)
                    {
                        foreach (String impItem in imp.importItems)
                        {
                            if (!this.importDictionary.ContainsKey(imp.fileName + "|" + impItem))
                            {
                                this.importDictionary.Add(imp.fileName + "|" + impItem, imp.importFrom);
                            }
                        }
                    }
                }

                // Write the values contained in jsFileHierarchyDict to a file
                foreach (JSFileHierarchy jsFileHier in jsFileHieararchyDict.Values)
                {
                    sw.WriteLine("/***************************************************************************************/");
                    sw.WriteLine(jsFileHier.folderName + "." + jsFileHier.fileName);

                    if (jsFileHier.functions.Count > 0)
                    {
                        foreach (JSFunction fnc in jsFileHier.functions)
                        {
                            Console.WriteLine("[NEW INCOMING FUNCTION] - " + fnc.fileName + "." + fnc.functionName);

                            sw.WriteLine("        /***************************************************************************************/");
                            
                            writeFunctionsToFile(sw, fnc, 2);
                            sw.WriteLine();
                            sw.WriteLine();

                        }
                    }
                }

                sw.Close();

                MessageBox.Show("LWC Parsing Complete");
            }

            if (htmlFiles.Count > 0)
            {
                foreach (String fileName in htmlFiles)
                {
                    StreamReader sr = new StreamReader(fileName);

                }
            }

            // Now loop through the jsFileHieararchyDict and associate the calling fumctions to the function itself with a hierarchical value;

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
                readLine = readLine.Replace("=", " = ");
                readLine = readLine.Replace(";", " ;");
                readLine = readLine.Replace(":", " : ");
                readLine = readLine.Replace("&&", " && ");
                readLine = readLine.Replace("||", " || ");
                readLine = readLine.Replace("!=", " != ");
                readLine = readLine.Replace("!==", " !== ");
                readLine = readLine.Replace("==", " == ");
                readLine = readLine.Replace("===", " === ");
                readLine = readLine.Replace("'", " ");
                //readLine = readLine.Replace(".", " . ");

                return readLine.Split(' ');
            }
            else
            {
                String[] readLineArray = new string[1];
                readLineArray[0] = "";

                return readLineArray;
            }
        }


        private String otherFunctionCallsCleanup(String functionOrPropertyString, Boolean isFunction)
        {
            while (functionOrPropertyString.StartsWith(" ")
                || functionOrPropertyString.StartsWith("(")
                || functionOrPropertyString.StartsWith(")")
                || functionOrPropertyString.StartsWith("{")
                || functionOrPropertyString.StartsWith("}")
                || functionOrPropertyString.StartsWith("!"))
            {
                if (functionOrPropertyString.StartsWith(" "))
                {
                    functionOrPropertyString = functionOrPropertyString.Trim();
                }
                else if (functionOrPropertyString.StartsWith("("))
                {
                    functionOrPropertyString = functionOrPropertyString.Substring(1);
                }
                else if (functionOrPropertyString.StartsWith(")"))
                {
                    functionOrPropertyString = functionOrPropertyString.Substring(1);
                }
                else if (functionOrPropertyString.StartsWith("{"))
                {
                    functionOrPropertyString = functionOrPropertyString.Substring(1);
                }
                else if (functionOrPropertyString.StartsWith("}"))
                {
                    functionOrPropertyString = functionOrPropertyString.Substring(1);
                }
                else if (functionOrPropertyString.StartsWith("!"))
                {
                    functionOrPropertyString = functionOrPropertyString.Substring(1);
                }
            }

            functionOrPropertyString = functionOrPropertyString.Trim();

            return functionOrPropertyString;
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


        List<String> layersDeep = new List<string>();

        private void writeFunctionsToFile(StreamWriter sw, 
            JSFunction parentFunction,
            Int32 tabCount)
        {
            //try
            //{

            Console.WriteLine("Loop Call - " + tabCount + " - " + parentFunction.fileName + "." + parentFunction.functionName);

            Boolean loopThroughChildReferences = true;

            for (Int32 i = 0; i < tabCount; i++)
            {
                sw.Write("\t");
            }

            String functionParameters = "";
            if (parentFunction.parameters.Count > 0)
            {
                functionParameters = "(";

                foreach (String param in parentFunction.parameters)
                {
                    functionParameters = functionParameters + param + ",";
                }

                functionParameters = functionParameters.Substring(0, functionParameters.Length - 1);
                functionParameters = functionParameters + ")";
            }
            else
            {
                functionParameters = "()";
            }

            String valueSet = "";
            if (parentFunction.valueSet != "")
            {
                valueSet = parentFunction.valueSet + " = ";
            }

            if (parentFunction.importReference.StartsWith("@salesforce"))
            {
                sw.WriteLine(parentFunction.fileName + "." + parentFunction.functionName + "\t\timport from: " + parentFunction.importReference);
            }
            else if (parentFunction.functionAnnotation != ""
                    && parentFunction.importReference != "")
            {
                sw.WriteLine(valueSet + parentFunction.functionAnnotation + " " + parentFunction.fileName + "." + parentFunction.functionName + "\t\timport from: " + parentFunction.importReference);
            }
            else if (parentFunction.functionAnnotation == ""
                && parentFunction.functionAnnotation != "")
            {
                sw.WriteLine(valueSet + parentFunction.functionAnnotation + " " + parentFunction.fileName + "." + parentFunction.functionName + functionParameters);
            }
            else
            {
                sw.WriteLine(valueSet + parentFunction.fileName + "." + parentFunction.functionName + functionParameters);
            }

            // Reference to function within the same JS file
            // We need to get the child functions from the child function in context, and call this same method again for each child function
            if (loopThroughChildReferences == true
                && parentFunction.childFunctions.Count > 0)
            {
                foreach (ChildFunction childFunction in parentFunction.childFunctions)
                {
                    if (childFunction.importFrom.StartsWith("@salesforce"))
                    {
                            // Last resort. Review to clean up later?
                            for (Int32 i = 0; i < tabCount; i++)
                            {
                                sw.Write("\t");
                            }

                            sw.WriteLine(childFunction.fileName + "." + childFunction.functionName + "\t\timport from: " + childFunction.importFrom);
                    }
                    // THIS - references a function in the same file
                    else if (childFunction.functionDesignation == "this")
                    {
                        //Console.WriteLine("THIS - " + tabCount + " - " + childFunction.fileName + "  " + childFunction.functionName);

                        String key = childFunction.fileName + "|" + childFunction.functionName;

                        if (this.jsFunctionsDict.ContainsKey(key))
                        {
                            JSFunction jsFunction = this.jsFunctionsDict[key];
                            jsFunction.valueSet = childFunction.valueSet;

                            Int32 oldTabCount = tabCount;
                            if (jsFunction.childFunctions.Count > 0)
                            {
                                oldTabCount += 2;
                            }
                            
                            sw.WriteLine();
                            writeFunctionsToFile(sw,
                                                 jsFunction,
                                                 oldTabCount);
                        }
                        else
                        {
                            JSFunction jsFunction = new JSFunction();
                            jsFunction.fileName = childFunction.fileName;
                            jsFunction.folderName = childFunction.folderName;
                            jsFunction.functionDesignation = childFunction.functionDesignation;
                            jsFunction.functionName = childFunction.functionName;
                            jsFunction.importReference = childFunction.importFrom;
                            jsFunction.valueSet = childFunction.valueSet;

                            writeFunctionsToFile(sw,
                                                 jsFunction,
                                                 tabCount + 2);
                        }
                    }
                    // If the function does not have a designation (ie an import reference), and there is no THIS before it
                    else if (childFunction.functionDesignation == ""
                        && this.jsFunctionsDict.ContainsKey(parentFunction.fileName + "|" + childFunction.functionName))
                    {
                        //Console.WriteLine("Second ELSE IF - " + tabCount + " - " + parentFunction.fileName + "  " + childFunction.functionName);

                        JSFunction jsFunction = this.jsFunctionsDict[parentFunction.fileName + "|" + childFunction.functionName];
                        jsFunction.valueSet = childFunction.valueSet;

                        Int32 oldTabCount = tabCount;
                        if (jsFunction.childFunctions.Count > 0)
                        {
                            oldTabCount += 2;
                        }

                        sw.WriteLine();
                        writeFunctionsToFile(sw,
                                             jsFunction,
                                             oldTabCount);
                    }
                    else if (childFunction.functionDesignation == ""
                        && !this.jsFunctionsDict.ContainsKey(parentFunction.fileName + "|" + childFunction.functionName))
                    {
                        //Console.WriteLine("THIRD ELSE IF - " + tabCount + " - " + parentFunction.fileName + "  " + childFunction.functionName);

                        if (this.importDictionary.ContainsKey(childFunction.fileName + "|" + childFunction.functionName))
                        {
                            String key = this.importDictionary[childFunction.fileName + "|" + childFunction.functionName] + "|" + childFunction.functionName;
                            if (this.jsFunctionsDict.ContainsKey(key))
                            {
                                JSFunction jsFunction = this.jsFunctionsDict[key];
                                jsFunction.valueSet = childFunction.valueSet;

                                Int32 oldTabCount = tabCount;
                                if (jsFunction.childFunctions.Count > 0)
                                {
                                    oldTabCount += 2;
                                }

                                sw.WriteLine();
                                writeFunctionsToFile(sw,
                                                     jsFunction,
                                                     oldTabCount);
                            }
                            else
                            {
                                JSFunction jsFunction = new JSFunction();
                                jsFunction.fileName = childFunction.fileName;
                                jsFunction.folderName = childFunction.folderName;
                                jsFunction.functionDesignation = childFunction.functionDesignation;
                                jsFunction.functionName = childFunction.functionName;
                                jsFunction.importReference = childFunction.importFrom;
                                jsFunction.valueSet = childFunction.valueSet;

                                writeFunctionsToFile(sw,
                                                     jsFunction,
                                                     tabCount + 2);
                            }
                        }
                        else
                        {
                            JSFunction jsFunction = new JSFunction();
                            jsFunction.fileName = childFunction.fileName;
                            jsFunction.folderName = childFunction.folderName;
                            jsFunction.functionDesignation = childFunction.functionDesignation;
                            jsFunction.functionName = childFunction.functionName;
                            jsFunction.importReference = childFunction.importFrom;
                            jsFunction.valueSet = childFunction.valueSet;

                            writeFunctionsToFile(sw,
                                                 jsFunction,
                                                 tabCount + 2);
                        }
                    }
                    else if (this.importDictionary.ContainsKey(childFunction.fileName + "|" + childFunction.functionDesignation))
                    {
                        //Console.WriteLine("FOURTH ELSE IF - " + tabCount + " - " + childFunction.fileName + "  " + childFunction.functionName);

                        String componentName = this.importDictionary[childFunction.fileName + "|" + childFunction.functionDesignation];
                        String key = componentName + "|" + childFunction.functionName;
                        JSFunction jsFunction = this.jsFunctionsDict[key];
                        jsFunction.valueSet = childFunction.valueSet;

                        Int32 oldTabCount = tabCount;
                        if (jsFunction.childFunctions.Count > 0)
                        {
                            oldTabCount += 2;
                        }

                        sw.WriteLine();
                        writeFunctionsToFile(sw,
                                             jsFunction,
                                             oldTabCount);
                    }
                    else
                    {
                        //Console.WriteLine("LAST ELSE - " + tabCount + " - " + childFunction.fileName + "  " + childFunction.functionName);

                        // Last resort. Review to clean up later?
                        for (Int32 i = 0; i < tabCount + 2; i++)
                        {
                            sw.Write("\t");
                        }

                        valueSet = "";
                        if (childFunction.valueSet != "")
                        {
                            valueSet = childFunction.valueSet + " = ";
                        }

                        if (childFunction.functionDesignation != "")
                        {
                            sw.WriteLine(valueSet + childFunction.fileName + "." + childFunction.functionDesignation + "." + childFunction.functionName + childFunction.functionParameters);
                        }
                        else
                        {
                            sw.WriteLine(valueSet + childFunction.fileName + "." + childFunction.functionName + childFunction.functionParameters);
                        }
                    }
                }
                }
            //}
            //catch (Exception e)
            //{
            //    sw.Close();
            //    Console.WriteLine(tabCount + "  " + parentFunction.fileName + " " + parentFunction.functionDesignation + "  " + parentFunction.functionName + " " + parentFunction.importReference);
            //}
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

            public JSProperty()
            {
                folderName = "";
                fileName = "";
                propertyAnnotation = "";
                propertyName = "";
                propertyValue = "";
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
            public Boolean wireFunction;
            public String importReference;
            public Boolean staticFunction;
            public List<String> parameters;
            public List<ChildFunction> childFunctions;
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
                wireFunction = false;
                importReference = "";
                staticFunction = false;
                parameters = new List<string>();
                childFunctions = new List<ChildFunction>();
                propertiesSet = new List<string>();
            }
        }


        private class ChildFunction
        {
            public String folderName;
            public String fileName;
            public String functionDesignation;
            public String functionName;
            public String functionParameters;
            public String valueSet;
            public String importFrom;

            public ChildFunction()
            {
                folderName = "";
                fileName = "";
                functionDesignation = "";
                functionName = "";
                functionParameters = "";
                valueSet = "";
                importFrom = "";
            }
        }


        //public class JSFunctionWithChildFunction
        //{
        //    public String folderName;
        //    public String fileName;
        //    public String importFrom;
        //    public List<String> childFunctions;

        //    public JSFunctionWithChildFunction()
        //    {
        //        folderName = "";
        //        fileName = "";
        //        importFrom = "";
        //        childFunctions = new List<string>();
        //    }
        //}
    }

}
