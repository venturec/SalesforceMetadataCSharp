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
using System.Xml;

using SalesforceMetadata.PartnerWSDL;
using SalesforceMetadata.ToolingWSDL;

namespace SalesforceMetadata
{
    public partial class MetadataComparison : Form
    {
        // List if inner classes which hold all of the values needed for each item in the XML file
        // Then loop through the inner class list to determine what section these belong to within the Tree View


        // Directory Name ->  SubfolderName (if applicable) OR File Name -> FileName -> Files which are different or do not exist
        private Dictionary<String, Dictionary<String, List<String>>> comparedValuesWithFolderAndNameOnly;

        //                 Directory Name ->  File Name ->       nd1.Name ->        nd2.Name ->        Name Value (may be noName) -> Tag Name -> node values which are different or do not exist
        // Example:        objects ->         Account.object ->  CustomObject       fields ->          Account_Status__c -> tag name -> values
        private Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, List<String>>>>>> comparedValuesWithNameValue;


        //                 Directory Name ->  File Name ->       nd1.Name ->        nd2.Name ->        nameValue + "|" + nd3.Name + "|" + nd4.Name + "|" + nd5.Name + "|" + nd6.Name + "|" + nd7.Name + "|" + nd8.Name -> List of node values
        private Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, List<String>>>>>> mstrFileComparison;
        private Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, List<String>>>>>> compFileComparison;

        public MetadataComparison()
        {
            InitializeComponent();
        }

        // Verify if there is a file in the comparison folder if not, the whole thing is a difference
        // If the files exist, validate whether or not they are the same. If not, there is a difference. 
        // These objects will need to have further comparisons done.
        private void RunComparison_Click(object sender, EventArgs e)
        {
            if (this.tbFromFolder.Text == ""
                || this.tbToFolder.Text == "")
            {
                MessageBox.Show("Please select a Read-From folder as well as a Compare-To Folder", "Missing Read-From / Compare-To Folder(s)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.treeViewDifferences.Nodes.Clear();

            // Add all files from the different directory folders to the dictionary
            String[] mstrDir = Directory.GetDirectories(this.tbFromFolder.Text);
            String[] compDir = Directory.GetDirectories(this.tbToFolder.Text);

            Dictionary<String, Dictionary<String, List<String>>> mDirAndFiles = new Dictionary<String, Dictionary<String, List<String>>>();
            Dictionary<String, Dictionary<String, List<String>>> cDirAndFiles = new Dictionary<String, Dictionary<String, List<String>>>();

            // Master Folder / Files Dictionary population

            // If there are no additional folders in the directory, we will add the folder name at minimum to the mstrDir and compDir and set the appendDirectory to false so that we 
            // are not appending the sub-folder name to the path.
            // Example: c:\Users\username\Documents\Projects\Project Name\unpackaged\objects
            // We do not want to appending the \objects\ folder to the already existing path since that would cause an error.

            Boolean appendDirectory = true;
            if (mstrDir.Length == 0)
            {
                mstrDir = new string[1];
                mstrDir[0] = this.tbFromFolder.Text;
                appendDirectory = false;
            }

            if (compDir.Length == 0)
            {
                compDir = new string[1];
                compDir[0] = this.tbToFolder.Text;
            }

            // Populate the mDirAndFiles Dictionary variable
            foreach (String mDir in mstrDir)
            {
                String[] dirSplit = mDir.Split('\\');
                String mFolderName = dirSplit[dirSplit.Length - 1];

                if (mFolderName == "aura")
                {
                    String[] mstrAuraDir = Directory.GetDirectories(mDir);
                    mDirAndFiles.Add("aura", new Dictionary<String, List<String>>());

                    foreach (String mAuraDir in mstrAuraDir)
                    {
                        String[] folderPathSplit = mAuraDir.Split('\\');
                        String auraBundleName = folderPathSplit[folderPathSplit.Length - 1];
                        mDirAndFiles["aura"].Add(auraBundleName, new List<String>());

                        Int32 i = 0;
                        foreach (String mFile in Directory.GetFiles(mAuraDir))
                        {
                            String[] filePathSplit = mFile.Split('\\');
                            mDirAndFiles["aura"][auraBundleName].Add(filePathSplit[filePathSplit.Length - 1]);

                            i++;
                        }
                    }
                }
                else if (mFolderName == "lwc")
                {
                    String[] mstrLwcDir = Directory.GetDirectories(mDir);
                    mDirAndFiles.Add("lwc", new Dictionary<String, List<String>>());

                    foreach (String mLwcDir in mstrLwcDir)
                    {
                        String[] folderPathSplit = mLwcDir.Split('\\');
                        String lwcBundleName = folderPathSplit[folderPathSplit.Length - 1];
                        mDirAndFiles["lwc"].Add(lwcBundleName, new List<string>());

                        Int32 i = 0;
                        foreach (String mFile in Directory.GetFiles(mLwcDir))
                        {
                            String[] filePathSplit = mFile.Split('\\');
                            mDirAndFiles["lwc"][lwcBundleName].Add(filePathSplit[filePathSplit.Length - 1]);

                            i++;
                        }
                    }
                }
                else
                {
                    mDirAndFiles.Add(mFolderName, new Dictionary<String, List<String>>());

                    Int32 i = 0;
                    foreach (String mFile in Directory.GetFiles(mDir))
                    {
                        String[] filePathSplit = mFile.Split('\\');
                        String fileName = filePathSplit[filePathSplit.Length - 1];
                        mDirAndFiles[mFolderName].Add(filePathSplit[filePathSplit.Length - 1], new List<string>());
                        mDirAndFiles[mFolderName][fileName].Add(filePathSplit[filePathSplit.Length - 1]);

                        i++;
                    }
                }
            }

            // Populate the cDirAndFiles Dictionary variable
            foreach (String cDir in compDir)
            {
                String[] dirSplit = cDir.Split('\\');
                String cFolderName = dirSplit[dirSplit.Length - 1];

                if (cFolderName == "aura")
                {
                    String[] compAuraDir = Directory.GetDirectories(cDir);
                    cDirAndFiles.Add("aura", new Dictionary<String, List<String>>());

                    foreach (String cAuraDir in compAuraDir)
                    {
                        String[] folderPathSplit = cAuraDir.Split('\\');
                        String auraBundleName = folderPathSplit[folderPathSplit.Length - 1];
                        cDirAndFiles["aura"].Add(auraBundleName, new List<String>());

                        Int32 i = 0;
                        foreach (String cFile in Directory.GetFiles(cAuraDir))
                        {
                            String[] filePathSplit = cFile.Split('\\');
                            String fileName = filePathSplit[filePathSplit.Length - 1];
                            cDirAndFiles["aura"][auraBundleName].Add(filePathSplit[filePathSplit.Length - 1]);

                            i++;
                        }
                    }
                }
                else if (cFolderName == "lwc")
                {
                    String[] compLwcDir = Directory.GetDirectories(cDir);
                    cDirAndFiles.Add("lwc", new Dictionary<String, List<String>>());

                    foreach (String cLwcDir in compLwcDir)
                    {
                        String[] folderPathSplit = cLwcDir.Split('\\');
                        String lwcBundleName = folderPathSplit[folderPathSplit.Length - 1];
                        cDirAndFiles["lwc"].Add(lwcBundleName, new List<String>());

                        Int32 i = 0;
                        foreach (String cFile in Directory.GetFiles(cLwcDir))
                        {
                            String[] filePathSplit = cFile.Split('\\');
                            cDirAndFiles["lwc"][lwcBundleName].Add(filePathSplit[filePathSplit.Length - 1]);

                            i++;
                        }
                    }
                }
                else
                {
                    cDirAndFiles.Add(cFolderName, new Dictionary<String, List<String>>());

                    Int32 i = 0;
                    foreach (String cFile in Directory.GetFiles(cDir))
                    {
                        String[] filePathSplit = cFile.Split('\\');
                        String fileName = filePathSplit[filePathSplit.Length - 1];
                        cDirAndFiles[cFolderName].Add(fileName, new List<string>());
                        cDirAndFiles[cFolderName][fileName].Add(filePathSplit[filePathSplit.Length - 1]);

                        i++;
                    }
                }
            }


            // Now go through the mDirAndFiles, find a related file in cDirAndFiles. 
            // If one does not exist, it is new and does not exist in Production
            // Add this to the List View - Differences
            // If the file exists, but is different, then add it to the List View - Differences

            comparedValuesWithFolderAndNameOnly = new Dictionary<String, Dictionary<String, List<String>>>();
            comparedValuesWithNameValue = new Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, List<String>>>>>>();

            foreach (String mDir in mDirAndFiles.Keys)
            {
                if (directoryExists(mDir, appendDirectory))
                {
                    foreach (String mObjFile in mDirAndFiles[mDir].Keys)
                    {
                        foreach (String mFile in mDirAndFiles[mDir][mObjFile])
                        {
                            // Check if the files between the two orgs are different and if so,
                            // Check if it is an XML file. 
                            // Ex: Classes, Triggers, Visualforce Pages, Components are not in XML format and will always throw an error
                            // However, if there is a difference between the two files within the two orgs, add it to the list of differences to display.

                            if (fileExists(mDir, mObjFile, mFile, appendDirectory))
                            {
                                if (areFilesDifferent(mDir, mObjFile, mFile, appendDirectory))
                                {
                                    String masterFile = "";
                                    String comparisonFile = "";
                                    if (appendDirectory == true)
                                    {
                                        masterFile = this.tbFromFolder.Text + "\\" + mDir + "\\" + mObjFile;
                                        comparisonFile = this.tbToFolder.Text + "\\" + mDir + "\\" + mObjFile;
                                    }
                                    else
                                    {
                                        masterFile = this.tbFromFolder.Text + "\\" + mObjFile;
                                        comparisonFile = this.tbToFolder.Text + "\\" + mObjFile;
                                    }

                                    // Parse the XML files and put into the mstrFileComparison / compFileComparison Dictionaries
                                    // Directory Name -> File Name -> nd1.Name -> nd2.Name -> 
                                    // nameValue + "|" + nd3.Name + "|" + nd4.Name + "|" + nd5.Name + "|" + nd6.Name + "|" + nd7.Name + "|" + nd8.Name + "|" + <etc> -> List of node values
                                    //
                                    // The nameValue can be the unique name provided such as an Object API field name or it can be "noName"
                                    // If the nameValue key is noName, then check this first before parsing the differences betweeen the two files
                                    // If key startsWith("noName") then...
                                    mstrFileComparison = new Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, List<String>>>>>>();
                                    compFileComparison = new Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, List<String>>>>>>();

                                    // Check if the file is in XML format and can be loaded as an XmlDocument
                                    Boolean isXmlDocument = true;

                                    XmlDocument mDoc = new XmlDocument();
                                    if (mDir == "aura")
                                    {
                                        checkAndAddFileNamesToDictionary(mDir, mObjFile, mFile, false);
                                    }
                                    else if (mDir == "lwc")
                                    {
                                        checkAndAddFileNamesToDictionary(mDir, mObjFile, mFile, false);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            mDoc.Load(masterFile);
                                        }
                                        catch (Exception exc)
                                        {
                                            isXmlDocument = false;
                                        }

                                        if(isXmlDocument == true) parseXmlDocument(mDir, mFile, mDoc, mstrFileComparison);
                                    }


                                    XmlDocument cDoc = new XmlDocument();
                                    if (mDir == "aura")
                                    {
                                        // Do nothing. We've already confirmed the files are different and added them previously.
                                    }
                                    else if (mDir == "lwc")
                                    {
                                        // Do nothing. We've already confirmed the files are different and added them previously.
                                    }
                                    else
                                    {
                                        try
                                        {
                                            cDoc.Load(comparisonFile);
                                        }
                                        catch (Exception exc)
                                        {
                                            Console.WriteLine(exc.Message + '\n' + exc.StackTrace);
                                            isXmlDocument = false;
                                        }

                                        if (isXmlDocument == true) parseXmlDocument(mDir, mFile, cDoc, compFileComparison);
                                    }


                                    // Loop through mstrFileComparison and compFileComparison to determine the differences and add to the comparedValuesWithNameValue
                                    // if the mstrFileComparison and compFileComparison contain values
                                    if (isXmlDocument == true && mstrFileComparison.Count > 0)
                                    {
                                        try
                                        {
                                            checkAndAddDifferencesToDictionary(mstrFileComparison, compFileComparison, false);
                                        }
                                        catch (Exception exc)
                                        {
                                            //MessageBox.Show(exc.Message + '\n' + exc.StackTrace);
                                            Console.WriteLine(exc.Message + '\n' + exc.StackTrace);
                                        }
                                    }

                                    // If it cannot be loaded as an XML document, then just add the name of the file to the TreeView
                                    if (isXmlDocument == false)
                                    {
                                        // Remove Whitespace
                                        StreamReader masterSR = new StreamReader(masterFile);
                                        StreamReader comparisonSR = new StreamReader(comparisonFile);

                                        String masterContents = masterSR.ReadToEnd();
                                        String comparisonContents = comparisonSR.ReadToEnd();

                                        masterContents = masterContents.Replace("\t", String.Empty)
                                                                       .Replace("\r", String.Empty)
                                                                       .Replace("\n", String.Empty)
                                                                       .Replace(" ", String.Empty);

                                        comparisonContents = comparisonContents.Replace("\t", String.Empty)
                                                                               .Replace("\r", String.Empty)
                                                                               .Replace("\n", String.Empty)
                                                                               .Replace(" ", String.Empty);

                                        masterSR.Close();
                                        comparisonSR.Close();

                                        if (masterContents != comparisonContents)
                                        {
                                            checkAndAddFileNamesToDictionary(mDir, "", mFile, false);
                                        }
                                    }
                                }
                            }
                            // Comparison Directory Exists in both the Master and Compare To paths, but the File does not exist in the Compare To path
                            else
                            {
                                mstrFileComparison = new Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, List<String>>>>>>();
                                compFileComparison = new Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, List<String>>>>>>();

                                // TODO: Move the XML Parsing to a separate method and parse out the entire XML file adding it to the tree view instead of just adding the name of the file
                                // Add the file since it does not exist
                                Boolean isXmlDocument = true;

                                XmlDocument mDoc = new XmlDocument();
                                if (mDir == "aura")
                                {
                                    checkAndAddFileNamesToDictionary(mDir, mObjFile, mFile, true);
                                }
                                else if (mDir == "lwc")
                                {
                                    checkAndAddFileNamesToDictionary(mDir, mObjFile, mFile, true);
                                }
                                else
                                {
                                    try
                                    {
                                        if (appendDirectory)
                                        {
                                            mDoc.Load(this.tbFromFolder.Text + '\\' + mDir + '\\' + mFile);
                                        }
                                        else 
                                        {
                                            mDoc.Load(this.tbFromFolder.Text + '\\' + mFile);
                                        }
                                    }
                                    catch (Exception exc)
                                    {
                                        Console.WriteLine(exc.Message + '\n' + exc.StackTrace);
                                        isXmlDocument = false;
                                    }

                                    if (isXmlDocument == true)  parseXmlDocument(mDir, mFile, mDoc, mstrFileComparison);
                                }

                                // Loop through mstrFileComparison and compFileComparison to determine the differences
                                if (isXmlDocument == true && mstrFileComparison.Count > 0)
                                {
                                    checkAndAddDifferencesToDictionary(mstrFileComparison, compFileComparison, true);
                                }

                                if (isXmlDocument == false)
                                {
                                    checkAndAddFileNamesToDictionary(mDir, "", mFile, true);
                                }
                            }
                        }
                    }
                }
                // The Directory exists in the Master folder, but the Compare To folder does not contain the directory
                // Therefore, add all files in the directory
                else
                {
                    // add directory from the master and all related files
                    // Add the entire block to the Difference Tree View: folder name + all files in that folder as subnodes of the parent
                    foreach (String mObjFile in mDirAndFiles[mDir].Keys)
                    {
                        foreach (String mFile in mDirAndFiles[mDir][mObjFile])
                        {
                            Boolean isXmlDocument = true;

                            XmlDocument mDoc = new XmlDocument();
                            if (mDir == "aura")
                            {
                                checkAndAddFileNamesToDictionary(mDir, mObjFile, mFile, true);
                            }
                            else if (mDir == "lwc")
                            {
                                checkAndAddFileNamesToDictionary(mDir, mObjFile, mFile, true);
                            }
                            else
                            {
                                try
                                {
                                    if (appendDirectory)
                                    {
                                        mDoc.Load(this.tbFromFolder.Text + '\\' + mDir + '\\' + mFile);
                                    }
                                    else
                                    {
                                        mDoc.Load(this.tbFromFolder.Text + '\\' + mFile);
                                    }
                                }
                                catch (Exception exc)
                                {
                                    Console.WriteLine(exc.Message + '\n' + exc.StackTrace);
                                    isXmlDocument = false;
                                }

                                if(isXmlDocument == true) parseXmlDocument(mDir, mFile, mDoc, mstrFileComparison);
                            }

                            // Loop through mstrFileComparison and compFileComparison to determine the differences
                            if (isXmlDocument == true && mstrFileComparison.Count > 0)
                            {
                                checkAndAddDifferencesToDictionary(mstrFileComparison, compFileComparison, true);
                            }

                            if (isXmlDocument == false)
                            {
                                checkAndAddFileNamesToDictionary(mDir, "", mFile, true);
                            }
                        }
                    }
                }
            }


            // Now loop through the Maps and add the values to the TreeNode
            //
            // This adds the Apex Classes, Triggers, LWCs and Aura components which are not XML based, but came up different between the 
            // two orgs
            if (comparedValuesWithFolderAndNameOnly.Count > 0)
            {
                foreach (String folderName in this.comparedValuesWithFolderAndNameOnly.Keys)
                {
                    TreeNode tnd1 = new TreeNode();
                    tnd1.Text = folderName;

                    foreach (String subFolderName in this.comparedValuesWithFolderAndNameOnly[folderName].Keys)
                    {
                        if (subFolderName == "")
                        {
                            foreach (String listValue in this.comparedValuesWithFolderAndNameOnly[folderName][subFolderName])
                            {
                                TreeNode tnd2 = new TreeNode();
                                tnd2.Text = listValue;
                                tnd1.Nodes.Add(tnd2);
                            }
                        }
                        else
                        {
                            TreeNode tnd2 = new TreeNode();
                            tnd2.Text = subFolderName;

                            foreach (String listValue in this.comparedValuesWithFolderAndNameOnly[folderName][subFolderName])
                            {
                                TreeNode tnd3 = new TreeNode();
                                tnd3.Text = listValue;
                                tnd2.Nodes.Add(tnd3);
                            }

                            tnd1.Nodes.Add(tnd2);
                        }
                    }

                    treeViewDifferences.Nodes.Add(tnd1);
                }

                comparedValuesWithFolderAndNameOnly.Clear();
            }


            // Directory Name -> File Name -> Nd1Name -> Nd2Name -> Name Value (may be noName) -> node values which are different or do not exist
            // This adds everything which is XML based
            if (comparedValuesWithNameValue.Count > 0)
            {
                foreach (String directoryName in this.comparedValuesWithNameValue.Keys)
                {
                    TreeNode tnd1 = new TreeNode();
                    tnd1.Text = directoryName;

                    foreach (String fileName in this.comparedValuesWithNameValue[directoryName].Keys)
                    {
                        TreeNode tnd2 = new TreeNode();
                        tnd2.Text = fileName;

                        if (fileName.StartsWith("[New] "))
                        {
                            tnd2.BackColor = Color.LightBlue;
                        }

                        foreach (String nd1Name in this.comparedValuesWithNameValue[directoryName][fileName].Keys)
                        {
                            TreeNode tnd3 = new TreeNode();
                            tnd3.Text = nd1Name;

                            foreach (String nd2Name in this.comparedValuesWithNameValue[directoryName][fileName][nd1Name].Keys)
                            {
                                TreeNode tnd4 = new TreeNode();
                                tnd4.Text = nd2Name;

                                foreach (String nameValue in this.comparedValuesWithNameValue[directoryName][fileName][nd1Name][nd2Name].Keys)
                                {
                                    TreeNode tnd5 = new TreeNode();
                                    tnd5.Text = nameValue;

                                    if (nameValue.StartsWith("[New] "))
                                    {
                                        tnd5.BackColor = Color.LightBlue;
                                    }
                                    else
                                    {
                                        tnd5.BackColor = Color.LightGoldenrodYellow;
                                    }

                                    foreach (String listValue in this.comparedValuesWithNameValue[directoryName][fileName][nd1Name][nd2Name][nameValue])
                                    {
                                        TreeNode tnd6 = new TreeNode();
                                        tnd6.Text = listValue;

                                        tnd5.Nodes.Add(tnd6);
                                    }
                                    
                                    tnd4.Nodes.Add(tnd5);
                                }

                                tnd3.Nodes.Add(tnd4);
                            }

                            tnd2.Nodes.Add(tnd3);
                        }

                        tnd1.Nodes.Add(tnd2);
                    }

                    treeViewDifferences.Nodes.Add(tnd1);
                }

                comparedValuesWithNameValue.Clear();
            }


            this.btnExport.Enabled = true;
            this.cbExportXML.Enabled = true;

        }


        /**********************************************************************************************************************/
        // Utility Nodes for the handling the Dictionaries

        // Directory Name -> File Name -> nd1.Name -> nd2.Name -> nameValue + "|" + nd3.Name + "|" + nd4.Name + "|" + nd5.Name + "|" + nd6.Name + "|" + nd7.Name + "|" + nd8.Name -> List of node values
        private void parseXmlDocument(String directoryName, String fileName, XmlDocument xmlDoc,
                                      Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, List<String>>>>>> comparisonDictionary)
        {
            //Console.WriteLine("parseXmlDocument" + Environment.NewLine);

            // First find the #text node values to determine if there are changes to those vlaues.
            // Use the parent node names up the chain to be the keys with the #text value being the value
            String nodeBlockNameValue = "";
            foreach (XmlNode nd1 in xmlDoc.ChildNodes)
            {
                if (nd1.HasChildNodes)
                {
                    foreach (XmlNode nd2 in nd1.ChildNodes)
                    {
                        /****************************************************************/

                        nodeBlockNameValue = MetadataDifferenceProcessing.getNameField(nd1.Name, nd2.Name, nd2.OuterXml);

                        /****************************************************************/
                        if (nodeBlockNameValue == "")
                        {
                            // Just add the entire block to the dictionary
                            //nodeBlockNameValue = "--noName--";
                            nodeBlockNameValue = nd2.Name;

                            String nd2OuterXml = nd2.OuterXml.Replace(" xmlns=\"http://soap.sforce.com/2006/04/metadata\"", "");

                            if (comparisonDictionary.ContainsKey(directoryName))
                            {
                                if (comparisonDictionary[directoryName].ContainsKey(fileName))
                                {
                                    if (comparisonDictionary[directoryName][fileName].ContainsKey(nd1.Name))
                                    {
                                        if (comparisonDictionary[directoryName][fileName][nd1.Name].ContainsKey(nd2.Name))
                                        {
                                            if (comparisonDictionary[directoryName][fileName][nd1.Name][nd2.Name].ContainsKey(nodeBlockNameValue))
                                            {
                                                comparisonDictionary[directoryName][fileName][nd1.Name][nd2.Name][nodeBlockNameValue].Add(nd2OuterXml);
                                            }
                                            else
                                            {
                                                comparisonDictionary[directoryName][fileName][nd1.Name][nd2.Name].Add(nodeBlockNameValue, new List<string> { nd2OuterXml });
                                            }
                                        }
                                        else
                                        {
                                            comparisonDictionary[directoryName][fileName][nd1.Name].Add(nd2.Name, new Dictionary<string, List<string>>());
                                            comparisonDictionary[directoryName][fileName][nd1.Name][nd2.Name].Add(nodeBlockNameValue, new List<string> { nd2OuterXml });
                                        }
                                    }
                                    else
                                    {
                                        comparisonDictionary[directoryName][fileName].Add(nd1.Name, new Dictionary<string, Dictionary<string, List<string>>>());
                                        comparisonDictionary[directoryName][fileName][nd1.Name].Add(nd2.Name, new Dictionary<string, List<string>>());
                                        comparisonDictionary[directoryName][fileName][nd1.Name][nd2.Name].Add(nodeBlockNameValue, new List<string> { nd2OuterXml });
                                    }
                                }
                                else
                                {
                                    comparisonDictionary[directoryName].Add(fileName, new Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>());
                                    comparisonDictionary[directoryName][fileName].Add(nd1.Name, new Dictionary<string, Dictionary<string, List<string>>>());
                                    comparisonDictionary[directoryName][fileName][nd1.Name].Add(nd2.Name, new Dictionary<string, List<string>>());
                                    comparisonDictionary[directoryName][fileName][nd1.Name][nd2.Name].Add(nodeBlockNameValue, new List<string> { nd2OuterXml });
                                }
                            }
                            else
                            {
                                comparisonDictionary.Add(directoryName, new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>>());
                                comparisonDictionary[directoryName].Add(fileName, new Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>());
                                comparisonDictionary[directoryName][fileName].Add(nd1.Name, new Dictionary<string, Dictionary<string, List<string>>>());
                                comparisonDictionary[directoryName][fileName][nd1.Name].Add(nd2.Name, new Dictionary<string, List<string>>());
                                comparisonDictionary[directoryName][fileName][nd1.Name][nd2.Name].Add(nodeBlockNameValue, new List<string> { nd2OuterXml });
                            }
                        }
                        else if (nd2.HasChildNodes)
                        {
                            foreach (XmlNode nd3 in nd2.ChildNodes)
                            {
                                if (nd3.HasChildNodes)
                                {
                                    foreach (XmlNode nd4 in nd3.ChildNodes)
                                    {
                                        if (nd4.HasChildNodes)
                                        {
                                            foreach (XmlNode nd5 in nd4.ChildNodes)
                                            {
                                                if (nd5.HasChildNodes)
                                                {
                                                    foreach (XmlNode nd6 in nd5.ChildNodes)
                                                    {
                                                        if (nd6.HasChildNodes)
                                                        {
                                                            foreach (XmlNode nd7 in nd6.ChildNodes)
                                                            {
                                                                if (nd7.HasChildNodes)
                                                                {
                                                                    foreach (XmlNode nd8 in nd7.ChildNodes)
                                                                    {
                                                                        if (nd8.HasChildNodes)
                                                                        {
                                                                            foreach (XmlNode nd9 in nd8.ChildNodes)
                                                                            {
                                                                                if (nd9.HasChildNodes)
                                                                                {
                                                                                    foreach (XmlNode nd10 in nd9.ChildNodes)
                                                                                    {
                                                                                        if (nd10.HasChildNodes)
                                                                                        {
                                                                                            foreach (XmlNode nd11 in nd10.ChildNodes)
                                                                                            {
                                                                                                if (nd11.HasChildNodes)
                                                                                                {

                                                                                                }
                                                                                                else if (nd11.Name == "#text")
                                                                                                {
                                                                                                    String nodeBlockKeys = nodeBlockNameValue + "|" + nd3.Name + "|" + nd4.Name + "|" + nd5.Name + "|" + nd6.Name + "|" + nd7.Name + "|" + nd8.Name + "|" + nd9.Name + "|" + nd10.Name;
                                                                                                    addValuesToDictionary(directoryName, fileName, nd1.Name, nd2.Name, nodeBlockKeys, nd10.OuterXml, comparisonDictionary);
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        else if (nd10.Name == "#text")
                                                                                        {
                                                                                            String nodeBlockKeys = nodeBlockNameValue + "|" + nd3.Name + "|" + nd4.Name + "|" + nd5.Name + "|" + nd6.Name + "|" + nd7.Name + "|" + nd8.Name + "|" + nd9.Name;
                                                                                            addValuesToDictionary(directoryName, fileName, nd1.Name, nd2.Name, nodeBlockKeys, nd9.OuterXml, comparisonDictionary);
                                                                                        }
                                                                                    }
                                                                                }
                                                                                else if (nd9.Name == "#text")
                                                                                {
                                                                                    String nodeBlockKeys = nodeBlockNameValue + "|" + nd3.Name + "|" + nd4.Name + "|" + nd5.Name + "|" + nd6.Name + "|" + nd7.Name + "|" + nd8.Name;
                                                                                    addValuesToDictionary(directoryName, fileName, nd1.Name, nd2.Name, nodeBlockKeys, nd8.OuterXml, comparisonDictionary);
                                                                                }
                                                                            }
                                                                        }
                                                                        else if (nd8.Name == "#text")
                                                                        {
                                                                            String nodeBlockKeys = nodeBlockNameValue + "|" + nd3.Name + "|" + nd4.Name + "|" + nd5.Name + "|" + nd6.Name + "|" + nd7.Name;
                                                                            addValuesToDictionary(directoryName, fileName, nd1.Name, nd2.Name, nodeBlockKeys, nd7.OuterXml, comparisonDictionary);
                                                                        }
                                                                    }
                                                                }
                                                                else if (nd7.Name == "#text")
                                                                {
                                                                    String nodeBlockKeys = nodeBlockNameValue + "|" + nd3.Name + "|" + nd4.Name + "|" + nd5.Name + "|" + nd6.Name;
                                                                    addValuesToDictionary(directoryName, fileName, nd1.Name, nd2.Name, nodeBlockKeys, nd6.OuterXml, comparisonDictionary);
                                                                }
                                                            }
                                                        }
                                                        else if (nd6.Name == "#text")
                                                        {
                                                            String nodeBlockKeys = nodeBlockNameValue + "|" + nd3.Name + "|" + nd4.Name + "|" + nd5.Name;
                                                            addValuesToDictionary(directoryName, fileName, nd1.Name, nd2.Name, nodeBlockKeys, nd5.OuterXml, comparisonDictionary);
                                                        }
                                                    }
                                                }
                                                else if (nd5.Name == "#text")
                                                {
                                                    String nodeBlockKeys = nodeBlockNameValue + "|" + nd3.Name + "|" + nd4.Name;
                                                    addValuesToDictionary(directoryName, fileName, nd1.Name, nd2.Name, nodeBlockKeys, nd4.OuterXml, comparisonDictionary);
                                                }
                                            }
                                        }
                                        else if (nd4.Name == "#text")
                                        {
                                            String nodeBlockKeys = nodeBlockNameValue + "|" + nd3.Name;
                                            addValuesToDictionary(directoryName, fileName, nd1.Name, nd2.Name, nodeBlockKeys, nd3.OuterXml, comparisonDictionary);
                                        }
                                    }
                                }
                                else if (nd3.Name == "#text")
                                {
                                    String nodeBlockKeys = nodeBlockNameValue;
                                    addValuesToDictionary(directoryName, fileName, nd1.Name, nd2.Name, nodeBlockKeys, nd2.OuterXml, comparisonDictionary);
                                }
                            }
                        }
                    }
                }
            }
        }

        // Directory/Folder Name -> File Name -> nd1.Name -> nd2.Name -> nameValue + "|" + nd3.Name + "|" + nd4.Name + "|" + nd5.Name + "|" + nd6.Name + "|" + nd7.Name + "|" + nd8.Name -> List of node values
        private void addValuesToDictionary(String directoryName, String fileName, String node1Name, String node2Name, String nodeBlockKeys, String value,
                                           Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, List<String>>>>>> comparisonDictionary)
        {
            value = value.Replace(" xmlns=\"http://soap.sforce.com/2006/04/metadata\"", "");

            if (comparisonDictionary.ContainsKey(directoryName))
            {
                if (comparisonDictionary[directoryName].ContainsKey(fileName))
                {
                    if (comparisonDictionary[directoryName][fileName].ContainsKey(node1Name))
                    {
                        if (comparisonDictionary[directoryName][fileName][node1Name].ContainsKey(node2Name))
                        {
                            if (comparisonDictionary[directoryName][fileName][node1Name][node2Name].ContainsKey(nodeBlockKeys))
                            {
                                comparisonDictionary[directoryName][fileName][node1Name][node2Name][nodeBlockKeys].Add(value);
                            }
                            else
                            {
                                List<String> tempList = new List<string> { value };
                                comparisonDictionary[directoryName][fileName][node1Name][node2Name].Add(nodeBlockKeys, tempList);
                            }
                        }
                        else
                        {
                            comparisonDictionary[directoryName][fileName][node1Name].Add(node2Name, new Dictionary<string, List<string>>());
                            comparisonDictionary[directoryName][fileName][node1Name][node2Name].Add(nodeBlockKeys, new List<string>());
                            comparisonDictionary[directoryName][fileName][node1Name][node2Name][nodeBlockKeys].Add(value);
                        }
                    }
                    else
                    {
                        comparisonDictionary[directoryName][fileName].Add(node1Name, new Dictionary<string, Dictionary<string, List<string>>>());
                        comparisonDictionary[directoryName][fileName][node1Name].Add(node2Name, new Dictionary<string, List<string>>());
                        comparisonDictionary[directoryName][fileName][node1Name][node2Name].Add(nodeBlockKeys, new List<string>());
                        comparisonDictionary[directoryName][fileName][node1Name][node2Name][nodeBlockKeys].Add(value);
                    }
                }
                else
                {
                    comparisonDictionary[directoryName].Add(fileName, new Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>());
                    comparisonDictionary[directoryName][fileName].Add(node1Name, new Dictionary<string, Dictionary<string, List<string>>>());
                    comparisonDictionary[directoryName][fileName][node1Name].Add(node2Name, new Dictionary<string, List<string>>());
                    comparisonDictionary[directoryName][fileName][node1Name][node2Name].Add(nodeBlockKeys, new List<string>());
                    comparisonDictionary[directoryName][fileName][node1Name][node2Name][nodeBlockKeys].Add(value);
                }
            }
            else
            {
                comparisonDictionary.Add(directoryName, new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>>());
                comparisonDictionary[directoryName].Add(fileName, new Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>());
                comparisonDictionary[directoryName][fileName].Add(node1Name, new Dictionary<string, Dictionary<string, List<string>>>());
                comparisonDictionary[directoryName][fileName][node1Name].Add(node2Name, new Dictionary<string, List<string>>());
                comparisonDictionary[directoryName][fileName][node1Name][node2Name].Add(nodeBlockKeys, new List<string>());
                comparisonDictionary[directoryName][fileName][node1Name][node2Name][nodeBlockKeys].Add(value);
            }
        }

        private void checkAndAddFileNamesToDictionary(String directoryName, String subDirectoryName, String fileName, Boolean fileIsNew)
        {
            if (fileIsNew == true)
            {
                fileName = "[New] " + fileName;
            }
            else
            {
                fileName = "[Updated] " + fileName;
            }

            if (comparedValuesWithFolderAndNameOnly.ContainsKey(directoryName))
            {
                if (comparedValuesWithFolderAndNameOnly[directoryName].ContainsKey(subDirectoryName))
                {
                    comparedValuesWithFolderAndNameOnly[directoryName][subDirectoryName].Add(fileName);
                }
                else
                {
                    comparedValuesWithFolderAndNameOnly[directoryName].Add(subDirectoryName, new List<string> { fileName });
                }
            }
            else
            {
                comparedValuesWithFolderAndNameOnly.Add(directoryName, new Dictionary<string, List<string>>());
                comparedValuesWithFolderAndNameOnly[directoryName].Add(subDirectoryName, new List<string> { fileName });
            }
        }


        // Directory Name -> File Name -> nd1.Name -> nd2.Name -> nameValue + "|" + nd3.Name + "|" + nd4.Name + "|" + nd5.Name + "|" + nd6.Name + "|" + nd7.Name + "|" + nd8.Name -> List of node values
        private void checkAndAddDifferencesToDictionary(Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, List<String>>>>>> mstrFileComparison,
                                                        Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, List<String>>>>>> compFileComparison,
                                                        Boolean fileIsNew)
        {
            
            String fileNamePrependedVlaue = "";

            if (fileIsNew == true)
            {
                fileNamePrependedVlaue = "[New] ";
            }

            // We only want to add the element values which have changed
            foreach (String directoryName in mstrFileComparison.Keys)
            {
                if (compFileComparison.ContainsKey(directoryName))
                {
                    foreach (String fileName in mstrFileComparison[directoryName].Keys)
                    {
                        // Skip over managed packages and continue on with the next iteration
                        //String[] splitFileName = fileName.Split(new String[] { "__" }, StringSplitOptions.None);
                        //if (splitFileName.Length > 2) continue;

                        if (compFileComparison[directoryName].ContainsKey(fileName))
                        {
                            foreach (String nd1Name in mstrFileComparison[directoryName][fileName].Keys)
                            {
                                if (compFileComparison[directoryName][fileName].ContainsKey(nd1Name))
                                {
                                    foreach (String nd2Name in mstrFileComparison[directoryName][fileName][nd1Name].Keys)
                                    {
                                        if (compFileComparison[directoryName][fileName][nd1Name].ContainsKey(nd2Name))
                                        {
                                            List<String> mstrKeyList = new List<string>(mstrFileComparison[directoryName][fileName][nd1Name][nd2Name].Keys);
                                            List<String> compKeyList = new List<string>(compFileComparison[directoryName][fileName][nd1Name][nd2Name].Keys);

                                            HashSet<String> mstrApiNames = new HashSet<string>();
                                            HashSet<String> compApiNames = new HashSet<string>();

                                            foreach (String nameKey in mstrKeyList)
                                            {
                                                String[] mstrNameWithValue = parseNodeNameAndValue(nameKey);
                                                mstrApiNames.Add(mstrNameWithValue[0]);
                                            }

                                            foreach (String nameKey in compKeyList)
                                            {
                                                String[] compNameWithValue = parseNodeNameAndValue(nameKey);
                                                compApiNames.Add(compNameWithValue[0]);
                                            }

                                            foreach (String nameKey in mstrFileComparison[directoryName][fileName][nd1Name][nd2Name].Keys)
                                            {
                                                String[] mstrNameWithValue = parseNodeNameAndValue(nameKey);

                                                // Now determine if the compFileComparison contains the name or not and then prepend the Updated or New
                                                //if (compFileComparison[directoryName][fileName][nd1Name][nd2Name].ContainsKey(nameKey))
                                                if(compApiNames.Contains(mstrNameWithValue[0]))
                                                {
                                                    if (compFileComparison[directoryName][fileName][nd1Name][nd2Name].ContainsKey(nameKey))
                                                    {
                                                        foreach (String elemValue in mstrFileComparison[directoryName][fileName][nd1Name][nd2Name][nameKey])
                                                        {
                                                            try
                                                            {
                                                                if (!compFileComparison[directoryName][fileName][nd1Name][nd2Name][nameKey].Contains(elemValue))
                                                                {
                                                                    // Add to the comparedValuesWithNameValue
                                                                    addToComparedValuesWithNameValueDictionary(directoryName,
                                                                                                               fileNamePrependedVlaue + fileName,
                                                                                                               nd1Name,
                                                                                                               nd2Name,
                                                                                                               "[Updated] " + mstrNameWithValue[0],
                                                                                                               elemValue);
                                                                }
                                                            }
                                                            catch (Exception exc)
                                                            {
                                                                Console.WriteLine(exc);
                                                            }
                                                        }
                                                    }
                                                    else 
                                                    {
                                                        foreach (String elemValue in mstrFileComparison[directoryName][fileName][nd1Name][nd2Name][nameKey])
                                                        {
                                                            try
                                                            {
                                                                if (compFileComparison[directoryName][fileName][nd1Name][nd2Name].ContainsKey(nameKey)
                                                                    && !compFileComparison[directoryName][fileName][nd1Name][nd2Name][nameKey].Contains(elemValue))
                                                                {
                                                                    // Add to the comparedValuesWithNameValue
                                                                    addToComparedValuesWithNameValueDictionary(directoryName,
                                                                                                               fileNamePrependedVlaue + fileName,
                                                                                                               nd1Name,
                                                                                                               nd2Name,
                                                                                                               "[Updated] " + mstrNameWithValue[0],
                                                                                                               elemValue);
                                                                }
                                                                else if (!compFileComparison[directoryName][fileName][nd1Name][nd2Name].ContainsKey(nameKey))
                                                                {
                                                                    addToComparedValuesWithNameValueDictionary(directoryName,
                                                                                                               fileNamePrependedVlaue + fileName,
                                                                                                               nd1Name,
                                                                                                               nd2Name,
                                                                                                               "[Updated] " + mstrNameWithValue[0],
                                                                                                               elemValue);
                                                                }
                                                            }
                                                            catch (Exception exc)
                                                            {
                                                                Console.WriteLine(exc);
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    foreach (String elemValue in mstrFileComparison[directoryName][fileName][nd1Name][nd2Name][nameKey])
                                                    {
                                                        addToComparedValuesWithNameValueDictionary(directoryName, 
                                                                                                   fileNamePrependedVlaue + fileName, 
                                                                                                   nd1Name, 
                                                                                                   nd2Name, 
                                                                                                   "[New] " + mstrNameWithValue[0], 
                                                                                                   elemValue);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            foreach (String nameKey in mstrFileComparison[directoryName][fileName][nd1Name][nd2Name].Keys)
                                            {
                                                foreach (String elemValue in mstrFileComparison[directoryName][fileName][nd1Name][nd2Name][nameKey])
                                                {
                                                    addToComparedValuesWithNameValueDictionary(directoryName, 
                                                                                               fileNamePrependedVlaue + fileName, 
                                                                                               nd1Name, 
                                                                                               nd2Name, 
                                                                                               nameKey, 
                                                                                               elemValue);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (String nd2Name in mstrFileComparison[directoryName][fileName][nd1Name].Keys)
                                    {
                                        foreach (String nameKey in mstrFileComparison[directoryName][fileName][nd1Name][nd2Name].Keys)
                                        {
                                            foreach (String elemValue in mstrFileComparison[directoryName][fileName][nd1Name][nd2Name][nameKey])
                                            {
                                                addToComparedValuesWithNameValueDictionary(directoryName, 
                                                                                           fileNamePrependedVlaue + fileName, 
                                                                                           nd1Name, 
                                                                                           nd2Name, 
                                                                                           nameKey, 
                                                                                           elemValue);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        // The Source of Truth contains a file, but the destination does not. 
                        // This is where we add the entire document to the Tree View, even if the directories exist in both orgs
                        else
                        {
                            foreach (String nd1Name in mstrFileComparison[directoryName][fileName].Keys)
                            {
                                foreach (String nd2Name in mstrFileComparison[directoryName][fileName][nd1Name].Keys)
                                {
                                    foreach (String nameKey in mstrFileComparison[directoryName][fileName][nd1Name][nd2Name].Keys)
                                    {
                                        String[] mstrNameWithValue = parseNodeNameAndValue(nameKey);

                                        foreach (String elemValue in mstrFileComparison[directoryName][fileName][nd1Name][nd2Name][nameKey])
                                        {
                                                addToComparedValuesWithNameValueDictionary(directoryName, 
                                                                                       fileNamePrependedVlaue + fileName, 
                                                                                       nd1Name, 
                                                                                       nd2Name,
                                                                                       mstrNameWithValue[0], 
                                                                                       elemValue);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                // The Source of Truth contains a Directory or folder, but the destination does not contain the directory. 
                // Therefore, add the directory name and all files Treeview marking them as [NEW]
                else
                {
                    foreach (String fileName in mstrFileComparison[directoryName].Keys)
                    {
                        foreach (String nd1Name in mstrFileComparison[directoryName][fileName].Keys)
                        {
                            foreach (String nd2Name in mstrFileComparison[directoryName][fileName][nd1Name].Keys)
                            {
                                foreach (String nameKey in mstrFileComparison[directoryName][fileName][nd1Name][nd2Name].Keys)
                                {
                                    String[] mstrNameWithValue = parseNodeNameAndValue(nameKey);

                                    foreach (String elemValue in mstrFileComparison[directoryName][fileName][nd1Name][nd2Name][nameKey])
                                    {
                                        addToComparedValuesWithNameValueDictionary(directoryName, 
                                                                                   fileNamePrependedVlaue + fileName, 
                                                                                   nd1Name, 
                                                                                   nd2Name,
                                                                                   mstrNameWithValue[0], 
                                                                                   elemValue);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        private String[] parseNodeNameAndValue(String nodeNameWithValue)
        {
            String[] parsedNodeNameWithValue = nodeNameWithValue.Split('|');
            return parsedNodeNameWithValue;
        }


        // Directory Name -> File Name -> Nd1Name -> Nd2Name -> Name Value (may be noName) -> tagName -> node values which are different or do not exist
        private void addToComparedValuesWithNameValueDictionary(String directoryName, 
                                                                String fileName, 
                                                                String nd1Name, 
                                                                String nd2Name, 
                                                                String nameKey, 
                                                                String elemValue)
        {
            if (comparedValuesWithNameValue.ContainsKey(directoryName))
            {
                if (comparedValuesWithNameValue[directoryName].ContainsKey(fileName))
                {
                    if (comparedValuesWithNameValue[directoryName][fileName].ContainsKey(nd1Name))
                    {
                        if (comparedValuesWithNameValue[directoryName][fileName][nd1Name].ContainsKey(nd2Name))
                        {
                            if (comparedValuesWithNameValue[directoryName][fileName][nd1Name][nd2Name].ContainsKey(nameKey))
                            {
                                comparedValuesWithNameValue[directoryName][fileName][nd1Name][nd2Name][nameKey].Add(elemValue);
                            }
                            else
                            {
                                comparedValuesWithNameValue[directoryName][fileName][nd1Name][nd2Name].Add(nameKey, new List<string> { elemValue });
                            }
                        }
                        else
                        {
                            comparedValuesWithNameValue[directoryName][fileName][nd1Name].Add(nd2Name, new Dictionary<string, List<string>>());
                            comparedValuesWithNameValue[directoryName][fileName][nd1Name][nd2Name].Add(nameKey, new List<string> { elemValue });
                        }
                    }
                    else
                    {
                        comparedValuesWithNameValue[directoryName][fileName].Add(nd1Name, new Dictionary<string, Dictionary<string, List<string>>>());
                        comparedValuesWithNameValue[directoryName][fileName][nd1Name].Add(nd2Name, new Dictionary<string, List<string>>());
                        comparedValuesWithNameValue[directoryName][fileName][nd1Name][nd2Name].Add(nameKey, new List<string> { elemValue });
                    }
                }
                else
                {
                    comparedValuesWithNameValue[directoryName].Add(fileName, new Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>());
                    comparedValuesWithNameValue[directoryName][fileName].Add(nd1Name, new Dictionary<string, Dictionary<string, List<string>>>());
                    comparedValuesWithNameValue[directoryName][fileName][nd1Name].Add(nd2Name, new Dictionary<string, List<string>>());
                    comparedValuesWithNameValue[directoryName][fileName][nd1Name][nd2Name].Add(nameKey, new List<string> { elemValue });
                }
            }
            else
            {
                comparedValuesWithNameValue.Add(directoryName, new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>>());
                comparedValuesWithNameValue[directoryName].Add(fileName, new Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>());
                comparedValuesWithNameValue[directoryName][fileName].Add(nd1Name, new Dictionary<string, Dictionary<string, List<string>>>());
                comparedValuesWithNameValue[directoryName][fileName][nd1Name].Add(nd2Name, new Dictionary<string, List<string>>());
                comparedValuesWithNameValue[directoryName][fileName][nd1Name][nd2Name].Add(nameKey, new List<string> { elemValue });
            }
        }


        /**********************************************************************************************************************/

        private Boolean directoryExists(String directoryName, Boolean appendDirectoryName)
        {
            Boolean directoryExists = false;

            if (appendDirectoryName == true)
            {
                directoryExists = Directory.Exists(this.tbToFolder.Text + '\\' + directoryName);
            }
            else
            {
                directoryExists = Directory.Exists(this.tbToFolder.Text);
            }

            return directoryExists;
        }

        private Boolean fileExists(String directoryName, String subDirectoryName, String fileName, Boolean appendDirectoryName)
        {
            Boolean fileExists = false;

            if (subDirectoryName == fileName
                && appendDirectoryName == true)
            {
                if (Directory.Exists(this.tbToFolder.Text + '\\' + directoryName))
                {
                    fileExists = File.Exists(this.tbToFolder.Text + '\\' + directoryName + '\\' + fileName);
                }
            }
            else if (subDirectoryName == fileName
                && appendDirectoryName == false)
            {
                if (Directory.Exists(this.tbToFolder.Text))
                {
                    fileExists = File.Exists(this.tbToFolder.Text + '\\' + fileName);
                }
            }
            else if (subDirectoryName != fileName
                    && appendDirectoryName == true)
            {
                if (Directory.Exists(this.tbToFolder.Text + '\\' + directoryName + '\\' + subDirectoryName))
                {
                    fileExists = File.Exists(this.tbToFolder.Text + '\\' + directoryName + '\\' + subDirectoryName + '\\' + fileName);
                }
            }
            else if(subDirectoryName != fileName
                    && appendDirectoryName == false)
            {
                if (Directory.Exists(this.tbToFolder.Text + '\\' + subDirectoryName))
                {
                    fileExists = File.Exists(this.tbToFolder.Text + '\\' + subDirectoryName + '\\' + fileName);
                }
            }

            return fileExists;
        }

        private Boolean areFilesDifferent(String directoryName, String subDirectoryName, String fileName, Boolean appendDirectoryName)
        {
            Boolean filesAreDifferent = false;

            if (subDirectoryName == fileName
                && appendDirectoryName == true)
            {
                StreamReader mFileSR = new StreamReader(this.tbFromFolder.Text + '\\' + directoryName + '\\' + fileName);
                StreamReader cFileSR = new StreamReader(this.tbToFolder.Text + '\\' + directoryName + '\\' + fileName);

                if (mFileSR.ReadToEnd() != cFileSR.ReadToEnd()) filesAreDifferent = true;

                mFileSR.Close();
                cFileSR.Close();
            }
            else if (subDirectoryName == fileName
                    && appendDirectoryName == false)
            {
                StreamReader mFileSR = new StreamReader(this.tbFromFolder.Text + '\\' + fileName);
                StreamReader cFileSR = new StreamReader(this.tbToFolder.Text + '\\' + fileName);

                if (mFileSR.ReadToEnd() != cFileSR.ReadToEnd()) filesAreDifferent = true;

                mFileSR.Close();
                cFileSR.Close();
            }
            else if (subDirectoryName != fileName
                && appendDirectoryName == true)
            {
                StreamReader mFileSR = new StreamReader(this.tbFromFolder.Text + '\\' + directoryName + '\\' + subDirectoryName + '\\' + fileName);
                StreamReader cFileSR = new StreamReader(this.tbToFolder.Text + '\\' + directoryName + '\\' + subDirectoryName + '\\' + fileName);

                if (mFileSR.ReadToEnd() != cFileSR.ReadToEnd()) filesAreDifferent = true;

                mFileSR.Close();
                cFileSR.Close();
            }
            else if (subDirectoryName != fileName
                && appendDirectoryName == false)
            {
                StreamReader mFileSR = new StreamReader(this.tbFromFolder.Text + '\\' + subDirectoryName + '\\' + fileName);
                StreamReader cFileSR = new StreamReader(this.tbToFolder.Text + '\\' + subDirectoryName + '\\' + fileName);

                if (mFileSR.ReadToEnd() != cFileSR.ReadToEnd()) filesAreDifferent = true;

                mFileSR.Close();
                cFileSR.Close();
            }

            return filesAreDifferent;
        }

        private void treeViewDifference_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeNode tn = e.Node;

            if (tn.Checked == true && tn.Nodes.Count > 0)
            {
                foreach (TreeNode cNode in tn.Nodes)
                {
                    cNode.Checked = true;
                }
            }
            else if (tn.Checked == false && tn.Nodes.Count > 0)
            {
                foreach (TreeNode cNode in tn.Nodes)
                {
                    cNode.Checked = false;
                }
            }
        }

        private void tbFromFolder_DoubleClick(object sender, EventArgs e)
        {
            this.tbFromFolder.Text = UtilityClass.folderBrowserSelectPath("Select the Compare From Folder", false, FolderEnum.ReadFrom);
        }

        private void tbToFolder_DoubleClick(object sender, EventArgs e)
        {
            this.tbToFolder.Text = UtilityClass.folderBrowserSelectPath("Select the Compare To Folder", false, FolderEnum.ReadFrom);
        }


        /*******************************************************************************************************************************/
        private void exportFolderAndTypesToExcel()
        {
            HashSet<String> alreadyAdded = new HashSet<string>();

            Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
            xlapp.Visible = true;

            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlapp.Workbooks.Add();

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheetGeneral = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                        (System.Reflection.Missing.Value,
                                                                         xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                         System.Reflection.Missing.Value,
                                                                         System.Reflection.Missing.Value);

            Int32 generalRowStart = 1;
            Int32 generalColStart = 1;

            xlWorksheetGeneral.Name = "General Differences";
            xlWorksheetGeneral.Cells[generalRowStart, generalColStart].Value = "ID";
            xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 1].Value = "KeyId";
            xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 2].Value = "Folder Name";             // tnd1.Text
            xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 3].Value = "Object New Or Updated";   // tnd1.Text Split
            xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 4].Value = "Object Name";             // tnd2.Text
            xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 5].Value = "Metadata Type";           // tnd3.Text
            xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 6].Value = "Metadata Field";          // tnd4.Text
            xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 7].Value = "Object Variable New Or Updated";  // tn5.Text Split
            xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 8].Value = "Object Variable";         // tn5.Text
            xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 9].Value = "XML Value";               // tn5.Text

            generalRowStart++;

            foreach (TreeNode tnd1 in this.treeViewDifferences.Nodes)
            {
                foreach (TreeNode tnd2 in tnd1.Nodes)
                {
                    // Use the object variable as the Key
                    // Exceptions will be Aura and LWC Components
                    String tnd2ObjectNewOrUpdated = "";
                    String tnd2ObjectName = "";
                    String tnd2FileName = "";

                    if (tnd2.Text.StartsWith("[New]"))
                    {
                        tnd2ObjectNewOrUpdated = "New";
                        tnd2ObjectName = tnd2.Text.Substring(6, tnd2.Text.Length - 6);
                        tnd2FileName = tnd2.Text.Substring(6, tnd2.Text.Length - 6);
                    }
                    else if (tnd2.Text.StartsWith("[Updated]"))
                    {
                        tnd2ObjectNewOrUpdated = "Updated";
                        tnd2ObjectName = tnd2.Text.Substring(10, tnd2.Text.Length - 10);
                        tnd2FileName = tnd2.Text.Substring(10, tnd2.Text.Length - 10);
                    }
                    else
                    {
                        tnd2ObjectName = tnd2.Text;
                    }

                    String[] objectNameSplit = tnd2ObjectName.Split('.');
                    if (objectNameSplit.Length > 1)
                    {
                        tnd2ObjectName = "";

                        // We need to loop through the objectNameSplit as there are specific file names with multiple '.'
                        for (Int32 i = 0; i < objectNameSplit.Length - 1; i++)
                        {
                            tnd2ObjectName = tnd2ObjectName + objectNameSplit[i] + ".";
                        }

                        // Knock off the last '.' at the end
                        tnd2ObjectName = tnd2ObjectName.Substring(0, tnd2ObjectName.Length - 1);
                    }

                    if (tnd2.Nodes.Count > 0)
                    {
                        foreach (TreeNode tnd3 in tnd2.Nodes)
                        {
                            String tnd3ObjectNewOrUpdated = "";
                            String tnd3FileName = "";

                            if (tnd1.Text == "aura" || tnd1.Text == "lwc")
                            {
                                if (tnd3.Text.StartsWith("[New]"))
                                {
                                    tnd3ObjectNewOrUpdated = "New";
                                    tnd3FileName = tnd3.Text.Substring(6, tnd3.Text.Length - 6);
                                }
                                else if (tnd2.Text.StartsWith("[Updated]"))
                                {
                                    tnd3ObjectNewOrUpdated = "Updated";
                                    tnd3FileName = tnd3.Text.Substring(10, tnd3.Text.Length - 10);
                                }
                            }

                            if (tnd3.Nodes.Count > 0)
                            {
                                foreach (TreeNode tnd4 in tnd3.Nodes)
                                {
                                    if (tnd4.Nodes.Count > 0)
                                    {
                                        foreach (TreeNode tnd5 in tnd4.Nodes)
                                        {
                                            String tnd5ObjectVarNewOrUpdated = "";
                                            String tnd5ObjectVar = "";
                                            if (tnd5.Text.StartsWith("[New]"))
                                            {
                                                tnd5ObjectVarNewOrUpdated = "New";
                                                tnd5ObjectVar = tnd5.Text.Substring(6, tnd5.Text.Length - 6);
                                            }
                                            else if (tnd5.Text.StartsWith("[Updated]"))
                                            {
                                                tnd5ObjectVarNewOrUpdated = "Updated";
                                                tnd5ObjectVar = tnd5.Text.Substring(10, tnd5.Text.Length - 10);
                                            }

                                            String key = "";
                                            if (tnd1.Text == "objects")
                                            {
                                                key = tnd1.Text + "_" + tnd3.Text + "_" + tnd2ObjectName + "_" + tnd4.Text + "_" + tnd5ObjectVar;
                                            }
                                            else
                                            {
                                                key = tnd1.Text + "_" + tnd3.Text + "_" + tnd2ObjectName + "_" + tnd4.Text + "_" + tnd5ObjectVar;
                                            }

                                            if (!alreadyAdded.Contains(key))
                                            {
                                                xlWorksheetGeneral.Cells[generalRowStart, generalColStart].Value = generalRowStart;
                                                xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 1].Value = key;
                                                xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 2].Value = tnd1.Text;               // tnd1.Text - Folder Name
                                                xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 3].Value = tnd2ObjectNewOrUpdated;  // File Name [New] / [Updated]
                                                xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 4].Value = tnd2ObjectName;          // File Name
                                                xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 5].Value = tnd3.Text;               // tnd3: Metadata Type
                                                xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 6].Value = tnd4.Text;               // tnd4: Metadata sub-type
                                                xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 7].Value = tnd5ObjectVarNewOrUpdated; 
                                                xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 8].Value = tnd5ObjectVar;

                                                if (this.cbExportXML.Checked == true)
                                                {
                                                    foreach (TreeNode tnd6 in tnd5.Nodes)
                                                    {
                                                        xlWorksheetGeneral.Cells[generalRowStart, generalColStart].Value = generalRowStart;
                                                        xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 8].Value = tnd5ObjectVar;
                                                        xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 9].Value = tnd6.Text;
                                                        generalRowStart++;
                                                    }
                                                }
                                                else
                                                {
                                                    generalRowStart++;
                                                }

                                                alreadyAdded.Add(key);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        String key = tnd1.Text + "_" + tnd3.Text + "_" + tnd2ObjectName;

                                        //if (!alreadyAdded.Contains(key))
                                        //{
                                        //    xlWorksheetGeneral.Cells[generalRowStart, generalColStart].Value = key;
                                        //    xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 1].Value = tnd1.Text;   // Folder Name tnd1.Text
                                        //    xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 2].Value = tnd2ObjectNewOrUpdated;
                                        //    xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 3].Value = tnd2ObjectName;
                                        //    //xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 2].Value = fileName;    // 
                                        //    //xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 3].Value = tnd3.Text;   // tnd4.Text
                                        //    //xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 4].Value = tnd4.Text;   // tnd4.Text

                                        //    alreadyAdded.Add(key);
                                        //    generalRowStart++;
                                        //}
                                    }
                                }
                            }
                            else
                            {
                                String key = tnd1.Text + "_" + tnd2ObjectName + "_" + tnd3FileName;

                                if (!alreadyAdded.Contains(key))
                                {
                                    xlWorksheetGeneral.Cells[generalRowStart, generalColStart].Value = generalRowStart;
                                    xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 1].Value = key;
                                    xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 2].Value = tnd1.Text;
                                    xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 3].Value = tnd2ObjectNewOrUpdated;
                                    xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 4].Value = tnd2ObjectName;
                                    xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 7].Value = tnd3ObjectNewOrUpdated;
                                    xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 8].Value = tnd3FileName;

                                    alreadyAdded.Add(key);
                                    generalRowStart++;
                                }
                            }
                        }
                    }
                    else
                    {
                        String key = tnd1.Text + "_" + tnd2ObjectName + "_"  + tnd2FileName;

                        if (!alreadyAdded.Contains(key))
                        {
                            xlWorksheetGeneral.Cells[generalRowStart, generalColStart].Value = generalRowStart;
                            xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 1].Value = key;
                            xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 2].Value = tnd1.Text;
                            xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 3].Value = tnd2ObjectNewOrUpdated;
                            xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 4].Value = tnd2ObjectName;
                            xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 7].Value = tnd2ObjectNewOrUpdated;
                            xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 8].Value = tnd2FileName;

                            alreadyAdded.Add(key);
                            generalRowStart++;
                        }
                    }
                }
            }

            xlapp.Visible = true;

            MessageBox.Show("Export of Folders and Types to Excel Complete");
        }


        public void formatExcelRange(Microsoft.Office.Interop.Excel.Worksheet xlWorksheet, 
                                     Int32 startRowNumber, 
                                     Int32 startColNumber, 
                                     Int32 endRowNumber, 
                                     Int32 endColNumber,
                                     Boolean boldText,
                                     Boolean italicText,
                                     Int32 fontSize,
                                     Int32 interiorColorRed,
                                     Int32 interiorColorGreen,
                                     Int32 interiorColorBlue)
        {
            Microsoft.Office.Interop.Excel.Range rng;
            rng = xlWorksheet.Range[xlWorksheet.Cells[startRowNumber, startColNumber], xlWorksheet.Cells[endRowNumber, endColNumber]];
            rng.Font.Bold = boldText;
            rng.Font.Italic = italicText;
            rng.Font.Size = fontSize;
            //rng.Font.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbFloralWhite;
            //rng.Font.Color = System.Drawing.Color.FromArgb(erf.fontColorRed, erf.fontColorGreen, erf.fontColorBlue);

            rng.Interior.Color = System.Drawing.Color.FromArgb(interiorColorRed, interiorColorGreen, interiorColorBlue);

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


        /*******************************************************************************************************************************/

        // Use Tooling API
        private void btnFindUnusedApexItems_Click(object sender, EventArgs e)
        {
            Dictionary<String, SalesforceMetadata.ToolingWSDL.ApexTrigger1> apexTriggers = new Dictionary<String, SalesforceMetadata.ToolingWSDL.ApexTrigger1>();
            Dictionary<String, SalesforceMetadata.ToolingWSDL.ApexClass1> apexClasses = new Dictionary<String, SalesforceMetadata.ToolingWSDL.ApexClass1>();
            Dictionary<String, SalesforceMetadata.ToolingWSDL.Flow1> flows = new Dictionary<String, SalesforceMetadata.ToolingWSDL.Flow1>();
            Dictionary<String, SalesforceMetadata.ToolingWSDL.ApexPage1> apexPage = new Dictionary<String, SalesforceMetadata.ToolingWSDL.ApexPage1>();
            Dictionary<String, SalesforceMetadata.ToolingWSDL.AuraDefinition1> auraDefinition = new Dictionary<String, SalesforceMetadata.ToolingWSDL.AuraDefinition1>();

            Dictionary<String, SalesforceMetadata.ToolingWSDL.SymbolTable> symbolDefinition = new Dictionary<String, SalesforceMetadata.ToolingWSDL.SymbolTable>();

            // Items to consider
            // Triggers
            // Flows with Apex actions
            // Classes
            // CLass Properties
            // Class Methods
            // Visualforce Pages
            // Lightning Web Components
            // Aura Components

            //    Find out if the trigger call is commented out

            // Next sift through the SOQL statements to determine if any can be consolidated into the SobjectQueries class


            // Log in with the Partner WSDL to retrieve all of the Triggers, Classes, VF Pages, Flows etc.
        }

        private void GenerateDeploymentPackage_Click(object sender, EventArgs e)
        {
            if (this.tbFromFolder.Text == "")
            {
                MessageBox.Show("Please select a Read-From folder", "Missing Read-From Folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            GenerateDeploymentPackage gdp = new GenerateDeploymentPackage();

            gdp.tbMetadataFolderToReadFrom.Text = this.tbFromFolder.Text;
            gdp.treeNodeCollFromDiff = this.treeViewDifferences.Nodes;
            
            gdp.populateMetadataTreeViewFromDiff();
            gdp.Show();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (this.tbFromFolder.Text == "") return;

            if (this.cmbExportType.Text == "Export All to CSV")
            {
                exportToCSV(false);
            }
            else if (this.cmbExportType.Text == "Export All to Excel")
            {
                DialogResult mbOkOrCancel = MessageBox.Show("Exporting to Excel can be a very long process. Would you like to continue?", "Continue", MessageBoxButtons.OKCancel );

                if (mbOkOrCancel == DialogResult.OK)
                {
                    exportFolderAndTypesToExcel();
                }
            }
            else if (this.cmbExportType.Text == "Export Selected to CSV")
            {
                exportToCSV(true);
            }
            else if (this.cmbExportType.Text == "Export Selected to Excel")
            {
                
            }

            MessageBox.Show("Export of Folders and Types to CSV Complete");
        }

        private void exportToCSV(Boolean exportSelected)
        {
            String[] folderPathName = this.tbFromFolder.Text.Split('\\');

            String folderSaveLocation = "";
            for (Int32 i = 0; i < folderPathName.Length - 2; i++)
            {
                if (i == 0)
                {
                    folderSaveLocation = folderSaveLocation + folderPathName[i];
                }
                else
                {
                    folderSaveLocation = folderSaveLocation + "\\" + folderPathName[i];
                }
            }

            folderSaveLocation = folderSaveLocation + "\\Reports";

            if (!Directory.Exists(folderSaveLocation))
            {
                Directory.CreateDirectory(folderSaveLocation);
            }

            String fileSaveName = folderSaveLocation + "\\DifferenceReport.csv";

            StreamWriter sw = new StreamWriter(fileSaveName);

            // Start writing values to the CSV file
            HashSet<String> alreadyAdded = new HashSet<string>();

            Int32 generalRowStart = 1;


            sw.Write("RowId" + '\t');
            sw.Write("KeyId" + '\t');
            sw.Write("Folder Name" + '\t');
            sw.Write("Object New Or Updated" + '\t');
            sw.Write("Object Name" + '\t');
            sw.Write("Metadata Type" + '\t');
            sw.Write("Metadata Field" + '\t');
            sw.Write("Object Variable New Or Updated" + '\t');
            sw.Write("Object Variable" + '\t');
            sw.Write("XML Value");
            sw.Write(Environment.NewLine);

            Boolean continueLoop = true;

            foreach (TreeNode tnd1 in this.treeViewDifferences.Nodes)
            {
                foreach (TreeNode tnd2 in tnd1.Nodes)
                {
                    if (exportSelected && tnd2.Checked == true)
                    {
                        continueLoop = true;
                    }
                    else if (exportSelected && tnd2.Checked == false)
                    {
                        continueLoop = false;
                    }

                    if (continueLoop == false) continue;

                    // Use the object variable as the Key
                    // Exceptions will be Aura and LWC Components
                    String tnd2ObjectNewOrUpdated = "";
                    String tnd2ObjectName = "";
                    String tnd2FileName = "";

                    if (tnd2.Text.StartsWith("[New]"))
                    {
                        tnd2ObjectNewOrUpdated = "New";
                        tnd2ObjectName = tnd2.Text.Substring(6, tnd2.Text.Length - 6);
                        tnd2FileName = tnd2.Text.Substring(6, tnd2.Text.Length - 6);
                    }
                    else if (tnd2.Text.StartsWith("[Updated]"))
                    {
                        tnd2ObjectNewOrUpdated = "Updated";
                        tnd2ObjectName = tnd2.Text.Substring(10, tnd2.Text.Length - 10);
                        tnd2FileName = tnd2.Text.Substring(10, tnd2.Text.Length - 10);
                    }
                    else
                    {
                        tnd2ObjectName = tnd2.Text;
                    }

                    String[] objectNameSplit = tnd2ObjectName.Split('.');
                    if (objectNameSplit.Length > 1)
                    {
                        tnd2ObjectName = "";

                        // We need to loop through the objectNameSplit as there are specific file names with multiple '.'
                        for (Int32 i = 0; i < objectNameSplit.Length - 1; i++)
                        {
                            tnd2ObjectName = tnd2ObjectName + objectNameSplit[i] + ".";
                        }

                        // Knock off the last '.' at the end
                        tnd2ObjectName = tnd2ObjectName.Substring(0, tnd2ObjectName.Length - 1);
                    }

                    if (tnd2.Nodes.Count > 0)
                    {
                        foreach (TreeNode tnd3 in tnd2.Nodes)
                        {
                            String tnd3ObjectNewOrUpdated = "";
                            String tnd3FileName = "";

                            if (tnd1.Text == "aura" || tnd1.Text == "lwc")
                            {
                                if (tnd3.Text.StartsWith("[New]"))
                                {
                                    tnd3ObjectNewOrUpdated = "New";
                                    tnd3FileName = tnd3.Text.Substring(6, tnd3.Text.Length - 6);
                                }
                                else if (tnd2.Text.StartsWith("[Updated]"))
                                {
                                    tnd3ObjectNewOrUpdated = "Updated";
                                    tnd3FileName = tnd3.Text.Substring(10, tnd3.Text.Length - 10);
                                }
                            }

                            if (tnd3.Nodes.Count > 0)
                            {
                                foreach (TreeNode tnd4 in tnd3.Nodes)
                                {
                                    if (tnd4.Nodes.Count > 0)
                                    {
                                        foreach (TreeNode tnd5 in tnd4.Nodes)
                                        {
                                            String tnd5ObjectVarNewOrUpdated = "";
                                            String tnd5ObjectVar = "";
                                            if (tnd5.Text.StartsWith("[New]"))
                                            {
                                                tnd5ObjectVarNewOrUpdated = "New";
                                                tnd5ObjectVar = tnd5.Text.Substring(6, tnd5.Text.Length - 6);
                                            }
                                            else if (tnd5.Text.StartsWith("[Updated]"))
                                            {
                                                tnd5ObjectVarNewOrUpdated = "Updated";
                                                tnd5ObjectVar = tnd5.Text.Substring(10, tnd5.Text.Length - 10);
                                            }

                                            String key = "";
                                            if (tnd1.Text == "objects")
                                            {
                                                key = tnd1.Text + "_" + tnd3.Text + "_" + tnd2ObjectName + "_" + tnd4.Text + "_" + tnd5ObjectVar;
                                            }
                                            else
                                            {
                                                key = tnd1.Text + "_" + tnd3.Text + "_" + tnd2ObjectName + "_" + tnd4.Text + "_" + tnd5ObjectVar;
                                            }

                                            if (!alreadyAdded.Contains(key))
                                            {
                                                sw.Write(generalRowStart.ToString() + '\t');
                                                sw.Write(key + '\t');
                                                sw.Write(tnd1.Text + '\t');               // tnd1.Text - Folder Name
                                                sw.Write(tnd2ObjectNewOrUpdated + '\t');  // File Name [New] / [Updated]
                                                sw.Write(tnd2ObjectName + '\t');          // File Name
                                                sw.Write(tnd3.Text + '\t');               // tnd3: Metadata Type
                                                sw.Write(tnd4.Text + '\t');               // tnd4: Metadata sub-type
                                                sw.Write(tnd5ObjectVarNewOrUpdated + '\t');
                                                sw.Write(tnd5ObjectVar);

                                                if (this.cbExportXML.Checked == true)
                                                {
                                                    foreach (TreeNode tnd6 in tnd5.Nodes)
                                                    {
                                                        sw.Write(generalRowStart.ToString() + '\t');
                                                        sw.Write('\t');
                                                        sw.Write('\t');
                                                        sw.Write('\t');
                                                        sw.Write('\t');
                                                        sw.Write('\t');
                                                        sw.Write('\t');
                                                        sw.Write(tnd5ObjectVar + '\t');
                                                        sw.Write(tnd6.Text);

                                                        generalRowStart++;
                                                    }
                                                }
                                                else
                                                {
                                                    generalRowStart++;
                                                }

                                                alreadyAdded.Add(key);
                                                sw.Write(Environment.NewLine);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        String key = tnd1.Text + "_" + tnd3.Text + "_" + tnd2ObjectName;

                                        //if (!alreadyAdded.Contains(key))
                                        //{
                                        //    xlWorksheetGeneral.Cells[generalRowStart, generalColStart].Value = key;
                                        //    xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 1].Value = tnd1.Text;   // Folder Name tnd1.Text
                                        //    xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 2].Value = tnd2ObjectNewOrUpdated;
                                        //    xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 3].Value = tnd2ObjectName;
                                        //    //xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 2].Value = fileName;    // 
                                        //    //xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 3].Value = tnd3.Text;   // tnd4.Text
                                        //    //xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 4].Value = tnd4.Text;   // tnd4.Text

                                        //    alreadyAdded.Add(key);
                                        //    generalRowStart++;
                                        //}
                                    }
                                }
                            }
                            else
                            {
                                String key = tnd1.Text + "_" + tnd2ObjectName + "_" + tnd3FileName;

                                if (!alreadyAdded.Contains(key))
                                {
                                    sw.Write(generalRowStart.ToString() + '\t');
                                    sw.Write(key + '\t');
                                    sw.Write(tnd1.Text + '\t');               // tnd1.Text - Folder Name
                                    sw.Write(tnd2ObjectNewOrUpdated + '\t');  // File Name [New] / [Updated]
                                    sw.Write(tnd2ObjectName + '\t');          // File Name
                                    sw.Write('\t');
                                    sw.Write('\t');
                                    sw.Write(tnd3ObjectNewOrUpdated + '\t');
                                    sw.Write(tnd3FileName);

                                    alreadyAdded.Add(key);
                                    sw.Write(Environment.NewLine);

                                    generalRowStart++;
                                }
                            }
                        }
                    }
                    else
                    {
                        String key = tnd1.Text + "_" + tnd2ObjectName + "_" + tnd2FileName;

                        if (!alreadyAdded.Contains(key))
                        {
                            sw.Write(generalRowStart.ToString() + '\t');
                            sw.Write(key + '\t');
                            sw.Write(tnd1.Text + '\t');               // tnd1.Text - Folder Name
                            sw.Write(tnd2ObjectNewOrUpdated + '\t');  // File Name [New] / [Updated]
                            sw.Write(tnd2ObjectName + '\t');          // File Name
                            sw.Write('\t');
                            sw.Write('\t');
                            sw.Write(tnd2ObjectNewOrUpdated + '\t');
                            sw.Write(tnd2FileName + '\t');

                            alreadyAdded.Add(key);
                            sw.Write(Environment.NewLine);

                            generalRowStart++;
                        }
                    }
                }
            }

            sw.Close();
        }

    }
}
