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

            // Property / JavaScript file
            Dictionary<String, String> properties = new Dictionary<String, String>();

            // Function Name / / JavaScript file
            Dictionary<String, String> functions = new Dictionary<String, String>();

            // get the text values into a List<string> and then add them to the 
            Dictionary<String, List<String>> fileToParsedContent = new Dictionary<String, List<String>>();
            if (jsFiles.Count > 0)
            {
                foreach (String fileName in jsFiles)
                {
                    String[] filePathSplit = fileName.Split('\\');
                    String[] fileNameSplit = filePathSplit[filePathSplit.Length - 1].Split('.');

                    //Dictionary<String, String> importReference = new Dictionary<String, String>();
                    
                    //JSFileHierarchy jsParsedFiles = new JSFileHierarchy();
                    //jsParsedFiles.folderName = filePathSplit[filePathSplit.Length - 2];
                    //jsParsedFiles.fileName = fileNameSplit[0];

                    StreamReader sr = new StreamReader(fileName);
                    
                    List<String> tempContent = new List<string>();
                    Boolean isComment = false;
                    Boolean isQuoteValue = false;
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
                                break;
                            }
                            else if(isComment == false)
                            {
                                if (parsedLine[i] != "")
                                {
                                    tempContent.Add(parsedLine[i]);
                                }
                            }
                        }
                    }

                    sr.Close();

                    // Now loop through the text array and determine the types, constructing them using the inner classes
                    Boolean isApiEnabled = false;
                    Boolean isExport = false;
                    Boolean isImport = false;
                    Boolean isWire = false;
                    Boolean isTrack = false;
                    Boolean isStatic = false;
                    Boolean isFunction = false;
                    Boolean isVariable = false;

                    for (Int32 i = 0; i < tempContent.Count; i++)
                    {
                        if (tempContent[i].ToLower() == "export")
                        {

                        }
                        else if (tempContent[i].ToLower() == "import")
                        {

                        }
                        else if (tempContent[i].ToLower() == "const")
                        {

                        }
                        else if (tempContent[i].ToLower() == "static")
                        {

                        }
                        else if (tempContent[i].ToLower() == "@api")
                        {

                        }
                        else if (tempContent[i].ToLower() == "@wire")
                        {

                        }
                        else if (tempContent[i].ToLower() == "@track")
                        {

                        }
                        else if (tempContent[i].ToLower() == "function")
                        {

                        }
                        else if (tempContent[i].ToLower().StartsWith("this."))
                        {
                            // Can be a function or a property

                        }
                        else
                        {

                        }
                    }

                    // jsFileHieararchyDict.Add(jsParsedFiles.folderName + "." + jsParsedFiles.fileName, jsParsedFiles);

                    // Now determine how the properties are set, the functions, the function parameters and the return value if there is one

                    // Deteremine the Function call hieararchy

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
