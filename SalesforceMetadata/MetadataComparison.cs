using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
        private Boolean runTreeNodeSelector = true;

        private HashSet<String> mstrFileComparison;
        private HashSet<String> compFileComparison;

        private HashSet<String> comparisonResults;

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

            XmlNodeParser xmlNdParser = new XmlNodeParser();

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

            mstrFileComparison = new HashSet<string>();
            compFileComparison = new HashSet<string>();

            comparisonResults = new HashSet<string>();

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

                                    // Check if the file is in XML format and can be loaded as an XmlDocument
                                    Boolean isXmlDocument = true;

                                    // Add MASTER document to mstrFileComparison
                                    XmlDocument mDoc = new XmlDocument();
                                    if (mDir == "aura" || mDir == "lwc")
                                    {
                                        comparisonResults.Add(mDir + "\\" + mObjFile + "\\[Updated] " + mFile);
                                    }
                                    else if (mDir == "classes" || mDir == "components" || mDir == "pages" || mDir == "triggers")
                                    {
                                        comparisonResults.Add(mDir + "\\[Updated] " + mFile);
                                    }
                                    else
                                    {
                                        // Using this structure because for some reason when I load an XML file where the file name contains %2E, it is replacing it with a . and then can't find the file
                                        String mstrFileContents = File.ReadAllText(masterFile);

                                        try
                                        {
                                            mDoc.LoadXml(mstrFileContents);
                                        }
                                        catch (Exception exc)
                                        {
                                            isXmlDocument = false;
                                        }

                                        if (isXmlDocument == true)
                                        {
                                            xmlNdParser.parseXmlDocument(mDir, mFile, mDoc, mstrFileComparison);
                                        }
                                    }

                                    // Add COMPARISON document to compFileComparison
                                    isXmlDocument = true;
                                    XmlDocument cDoc = new XmlDocument();
                                    if (mDir == "aura" || mDir == "classes" || mDir == "lwc" || mDir == "triggers")
                                    {
                                        // Do nothing. We've already confirmed the files are different and added them previously.
                                    }
                                    else
                                    {
                                        String compFileContents = File.ReadAllText(comparisonFile);

                                        try
                                        {
                                            cDoc.LoadXml(compFileContents);
                                        }
                                        catch (Exception exc)
                                        {
                                            //Console.WriteLine(exc.Message + '\n' + exc.StackTrace);
                                            isXmlDocument = false;
                                        }

                                        if (isXmlDocument == true)
                                        {
                                            xmlNdParser.parseXmlDocument(mDir, mFile, cDoc, compFileComparison);
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
                                            comparisonResults.Add(mDir + "\\" + mObjFile + "\\[Updated] " + mFile);
                                        }
                                    }
                                }
                            }
                            // Comparison Directory Exists in both the Master and Compare To paths, but the File does not exist in the Compare To path
                            else
                            {
                                // TODO: Move the XML Parsing to a separate method and parse out the entire XML file adding it to the tree view instead of just adding the name of the file
                                // Add the file since it does not exist
                                Boolean isXmlDocument = true;

                                XmlDocument mDoc = new XmlDocument();
                                if (mDir == "aura" || mDir == "lwc")
                                {
                                    comparisonResults.Add(mDir + "\\" + mObjFile + "\\[New] " + mFile);
                                }
                                else if (mDir == "classes" || mDir == "components" || mDir == "pages" || mDir == "triggers")
                                {
                                    comparisonResults.Add(mDir + "\\[New] " + mFile);
                                }
                                else
                                {
                                    try
                                    {
                                        if (appendDirectory)
                                        {
                                            String mstrFileContents = File.ReadAllText(this.tbFromFolder.Text + '\\' + mDir + '\\' + mFile);
                                            mDoc.LoadXml(mstrFileContents);
                                        }
                                        else
                                        {
                                            String mstrFileContents = File.ReadAllText(this.tbFromFolder.Text + '\\' + mFile);
                                            mDoc.LoadXml(mstrFileContents);
                                        }
                                    }
                                    catch (Exception exc)
                                    {
                                        //Console.WriteLine(exc.Message + '\n' + exc.StackTrace);
                                        isXmlDocument = false;
                                    }

                                    if (isXmlDocument == true)
                                    {
                                        xmlNdParser.parseXmlDocument(mDir, mFile, mDoc, mstrFileComparison);
                                    }
                                }

                                if (isXmlDocument == false)
                                {
                                    comparisonResults.Add(mDir + "\\" + mObjFile + "\\[New] " + mFile);
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
                            if (mDir == "aura" || mDir == "lwc")
                            {
                                comparisonResults.Add(mDir + "\\" + mObjFile + "\\[New] " + mFile);
                            }
                            else if (mDir == "classes" || mDir == "components" || mDir == "pages" || mDir == "triggers")
                            {
                                comparisonResults.Add(mDir + "\\[New] " + mFile);
                            }
                            else
                            {
                                try
                                {
                                    if (appendDirectory)
                                    {
                                        String mstrFileContents = File.ReadAllText(this.tbFromFolder.Text + '\\' + mDir + '\\' + mFile);
                                        mDoc.LoadXml(mstrFileContents);
                                    }
                                    else
                                    {
                                        String mstrFileContents = File.ReadAllText(this.tbFromFolder.Text + '\\' + mFile);
                                        mDoc.LoadXml(mstrFileContents);
                                    }
                                }
                                catch (Exception exc)
                                {
                                    //Console.WriteLine(exc.Message + '\n' + exc.StackTrace);
                                    isXmlDocument = false;
                                }

                                if (isXmlDocument == true)
                                {
                                    xmlNdParser.parseXmlDocument(mDir, mFile, mDoc, mstrFileComparison);
                                }
                            }

                            if (isXmlDocument == false)
                            {
                                comparisonResults.Add(mDir + "\\" + mObjFile + "\\[New] " + mFile);
                            }
                        }
                    }
                }
            }

            foreach (String mstrValue in mstrFileComparison)
            {
                if (!compFileComparison.Contains(mstrValue))
                {
                    comparisonResults.Add(mstrValue);
                }
            }

            // Now loop through the HashSet and add the values to the TreeNode
            // This adds everything which is XML based
            if (this.comparisonResults.Count > 0)
            {
                populateTreeView();
            }
        }

        /**********************************************************************************************************************/
        // Helper methods
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

        // I'm not real happy with this method, but it works for now and needs to be refactored later
        private void populateTreeView()
        {
            TreeNode tnd0 = new TreeNode();
            TreeNode tnd1 = new TreeNode();
            TreeNode tnd2 = new TreeNode();
            TreeNode tnd3 = new TreeNode();
            TreeNode tnd4 = new TreeNode();
            TreeNode tnd5 = new TreeNode();
            //TreeNode tnd6 = new TreeNode();
            //TreeNode tnd7 = new TreeNode();
            //TreeNode tnd8 = new TreeNode();
            //TreeNode tnd9 = new TreeNode();
            //TreeNode tnd10 = new TreeNode();
            //TreeNode tnd11 = new TreeNode();
            //TreeNode tnd12 = new TreeNode();
            //TreeNode tnd13 = new TreeNode();
            //TreeNode tnd14 = new TreeNode();
            //TreeNode tnd15 = new TreeNode();
            //TreeNode tnd16 = new TreeNode();
            //TreeNode tnd17 = new TreeNode();
            //TreeNode tnd18 = new TreeNode();
            //TreeNode tnd19 = new TreeNode();
            //TreeNode tnd20 = new TreeNode();
            //TreeNode tnd21 = new TreeNode();

            foreach (String stringValue in this.comparisonResults)
            {
                String[] stringValueSplit = stringValue.Split('\\');

                // This one needs to remain slightly different since the logic is confirming there is a change in the folder name
                if (tnd0.Text != stringValueSplit[0])
                {
                    // Logical problem here on the initial/first loop since 
                    if (tnd0.Text != "")
                    {
                        treeViewDifferences.Nodes.Add(tnd0);
                    }

                    // Anticipate a new tree node block and instantiate the variables as new
                    tnd0 = new TreeNode(stringValueSplit[0]);
                    tnd1 = new TreeNode();
                    tnd2 = new TreeNode();
                    tnd3 = new TreeNode();
                    tnd4 = new TreeNode();
                    tnd5 = new TreeNode();
                    //tnd6 = new TreeNode();
                    //tnd7 = new TreeNode();
                    //tnd8 = new TreeNode();
                    //tnd9 = new TreeNode();
                    //tnd10 = new TreeNode();
                    //tnd11 = new TreeNode();
                    //tnd12 = new TreeNode();
                    //tnd13 = new TreeNode();
                    //tnd14 = new TreeNode();
                    //tnd15 = new TreeNode();
                    //tnd16 = new TreeNode();
                    //tnd17 = new TreeNode();
                    //tnd18 = new TreeNode();
                    //tnd19 = new TreeNode();
                    //tnd20 = new TreeNode();
                }

                if (tnd1.Text != stringValueSplit[1])
                {
                    if (stringValueSplit[1] != "")
                    {
                        tnd1 = new TreeNode(stringValueSplit[1]);
                        tnd0.Nodes.Add(tnd1);
                    }

                    // Anticipate a new tree node block and instantiate the variables as new
                    tnd2 = new TreeNode();
                    tnd3 = new TreeNode();
                    tnd4 = new TreeNode();
                    tnd5 = new TreeNode();
                    //tnd6 = new TreeNode();
                    //tnd7 = new TreeNode();
                    //tnd8 = new TreeNode();
                    //tnd9 = new TreeNode();
                    //tnd10 = new TreeNode();
                    //tnd11 = new TreeNode();
                    //tnd12 = new TreeNode();
                    //tnd13 = new TreeNode();
                    //tnd14 = new TreeNode();
                    //tnd15 = new TreeNode();
                    //tnd16 = new TreeNode();
                    //tnd17 = new TreeNode();
                    //tnd18 = new TreeNode();
                    //tnd19 = new TreeNode();
                    //tnd20 = new TreeNode();

                    if (stringValueSplit[1].StartsWith("[New] "))
                    {
                        tnd1.BackColor = Color.LightBlue;
                    }
                }

                if (stringValueSplit.Length > 2
                    && tnd2.Text != stringValueSplit[2])
                {
                    if (stringValueSplit[2] != "")
                    {
                        tnd2 = new TreeNode(stringValueSplit[2]);
                        tnd1.Nodes.Add(tnd2);
                    }

                    // Anticipate a new tree node block and instantiate the variables as new
                    tnd3 = new TreeNode();
                    tnd4 = new TreeNode();
                    tnd5 = new TreeNode();
                    //tnd6 = new TreeNode();
                    //tnd7 = new TreeNode();
                    //tnd8 = new TreeNode();
                    //tnd9 = new TreeNode();
                    //tnd10 = new TreeNode();
                    //tnd11 = new TreeNode();
                    //tnd12 = new TreeNode();
                    //tnd13 = new TreeNode();
                    //tnd14 = new TreeNode();
                    //tnd15 = new TreeNode();
                    //tnd16 = new TreeNode();
                    //tnd17 = new TreeNode();
                    //tnd18 = new TreeNode();
                    //tnd19 = new TreeNode();
                    //tnd20 = new TreeNode();

                    if (stringValueSplit[2].StartsWith("[New] "))
                    {
                        tnd2.BackColor = Color.LightBlue;
                    }
                }

                if (stringValueSplit.Length > 3
                    && tnd3.Text != stringValueSplit[3])
                {
                    if (stringValueSplit[3] != "")
                    {
                        tnd3 = new TreeNode(stringValueSplit[3]);
                        tnd2.Nodes.Add(tnd3);
                    }

                    // Anticipate a new tree node block and instantiate the variables as new
                    tnd4 = new TreeNode();
                    tnd5 = new TreeNode();
                    //tnd6 = new TreeNode();
                    //tnd7 = new TreeNode();
                    //tnd8 = new TreeNode();
                    //tnd9 = new TreeNode();
                    //tnd10 = new TreeNode();
                    //tnd11 = new TreeNode();
                    //tnd12 = new TreeNode();
                    //tnd13 = new TreeNode();
                    //tnd14 = new TreeNode();
                    //tnd15 = new TreeNode();
                    //tnd16 = new TreeNode();
                    //tnd17 = new TreeNode();
                    //tnd18 = new TreeNode();
                    //tnd19 = new TreeNode();
                    //tnd20 = new TreeNode();

                    if (stringValueSplit[3].StartsWith("[New] "))
                    {
                        tnd3.BackColor = Color.LightBlue;
                    }
                }

                if (stringValueSplit.Length > 4
                    && tnd4.Text != stringValueSplit[4])
                {
                    if (stringValueSplit[4] != "")
                    {
                        tnd4 = new TreeNode(stringValueSplit[4]);
                        tnd3.Nodes.Add(tnd4);
                    }

                    // Anticipate a new tree node block and instantiate the variables as new
                    tnd5 = new TreeNode();
                    //tnd6 = new TreeNode();
                    //tnd7 = new TreeNode();
                    //tnd8 = new TreeNode();
                    //tnd9 = new TreeNode();
                    //tnd10 = new TreeNode();
                    //tnd11 = new TreeNode();
                    //tnd12 = new TreeNode();
                    //tnd13 = new TreeNode();
                    //tnd14 = new TreeNode();
                    //tnd15 = new TreeNode();
                    //tnd16 = new TreeNode();
                    //tnd17 = new TreeNode();
                    //tnd18 = new TreeNode();
                    //tnd19 = new TreeNode();
                    //tnd20 = new TreeNode();

                    if (stringValueSplit[4].StartsWith("[New] "))
                    {
                        tnd4.BackColor = Color.LightBlue;
                    }
                    else if (stringValueSplit[1].StartsWith("[Updated] "))
                    {
                        tnd1.BackColor = Color.AliceBlue;
                    }
                }

                if (stringValueSplit.Length > 5
                    && tnd5.Text != stringValueSplit[5])
                {
                    if (stringValueSplit[5] != "")
                    {
                        tnd5 = new TreeNode();

                        // Check if there are additional layers to add
                        String tnd5Text = stringValueSplit[5];
                        if (stringValueSplit.Length > 6)
                        {
                            for (Int32 i = 6; i < stringValueSplit.Length; i++)
                            {
                                tnd5Text = tnd5Text + "\\" + stringValueSplit[i];
                            }
                        }

                        tnd5.Text = tnd5Text;
                        tnd4.Nodes.Add(tnd5);
                    }

                    //tnd6 = new TreeNode();
                    //tnd7 = new TreeNode();
                    //tnd8 = new TreeNode();
                    //tnd9 = new TreeNode();
                    //tnd10 = new TreeNode();
                    //tnd11 = new TreeNode();
                    //tnd12 = new TreeNode();
                    //tnd13 = new TreeNode();
                    //tnd14 = new TreeNode();
                    //tnd15 = new TreeNode();
                    //tnd16 = new TreeNode();
                    //tnd17 = new TreeNode();
                    //tnd18 = new TreeNode();
                    //tnd19 = new TreeNode();
                    //tnd20 = new TreeNode();

                    if (stringValueSplit[5].StartsWith("[New] "))
                    {
                        tnd5.BackColor = Color.LightBlue;
                    }
                }

                //if (stringValueSplit.Length > 6
                //    && tnd6.Text != stringValueSplit[6])
                //{
                //    if (stringValueSplit[6] != "")
                //    {
                //        tnd6 = new TreeNode(stringValueSplit[6]);
                //        tnd5.Nodes.Add(tnd6);
                //    }

                //    tnd7 = new TreeNode();
                //    tnd8 = new TreeNode();
                //    tnd9 = new TreeNode();
                //    tnd10 = new TreeNode();
                //    tnd11 = new TreeNode();
                //    tnd12 = new TreeNode();
                //    tnd13 = new TreeNode();
                //    tnd14 = new TreeNode();
                //    tnd15 = new TreeNode();
                //    tnd16 = new TreeNode();
                //    tnd17 = new TreeNode();
                //    tnd18 = new TreeNode();
                //    tnd19 = new TreeNode();
                //    tnd20 = new TreeNode();

                //    if (stringValueSplit[6].StartsWith("[New] "))
                //    {
                //        tnd6.BackColor = Color.LightBlue;
                //    }
                //}

                //if (stringValueSplit.Length > 7
                //    && tnd7.Text != stringValueSplit[7])
                //{
                //    if (stringValueSplit[7] != "")
                //    {
                //        tnd7 = new TreeNode(stringValueSplit[7]);
                //        tnd6.Nodes.Add(tnd7);
                //    }

                //    tnd8 = new TreeNode();
                //    tnd9 = new TreeNode();
                //    tnd10 = new TreeNode();
                //    tnd11 = new TreeNode();
                //    tnd12 = new TreeNode();
                //    tnd13 = new TreeNode();
                //    tnd14 = new TreeNode();
                //    tnd15 = new TreeNode();
                //    tnd16 = new TreeNode();
                //    tnd17 = new TreeNode();
                //    tnd18 = new TreeNode();
                //    tnd19 = new TreeNode();
                //    tnd20 = new TreeNode();

                //    if (stringValueSplit[7].StartsWith("[New] "))
                //    {
                //        tnd7.BackColor = Color.LightBlue;
                //    }
                //}

                //if (stringValueSplit.Length > 8
                //    && tnd8.Text != stringValueSplit[8])
                //{
                //    if (stringValueSplit[8] != "")
                //    {
                //        tnd8 = new TreeNode(stringValueSplit[8]);
                //        tnd7.Nodes.Add(tnd8);
                //    }

                //    tnd9 = new TreeNode();
                //    tnd10 = new TreeNode();
                //    tnd11 = new TreeNode();
                //    tnd12 = new TreeNode();
                //    tnd13 = new TreeNode();
                //    tnd14 = new TreeNode();
                //    tnd15 = new TreeNode();
                //    tnd16 = new TreeNode();
                //    tnd17 = new TreeNode();
                //    tnd18 = new TreeNode();
                //    tnd19 = new TreeNode();
                //    tnd20 = new TreeNode();

                //    if (stringValueSplit[8].StartsWith("[New] "))
                //    {
                //        tnd8.BackColor = Color.LightBlue;
                //    }
                //}

                //if (stringValueSplit.Length > 9
                //    && tnd9.Text != stringValueSplit[9])
                //{
                //    if (stringValueSplit[9] != "")
                //    {
                //        tnd9 = new TreeNode(stringValueSplit[9]);
                //        tnd8.Nodes.Add(tnd9);
                //    }

                //    tnd10 = new TreeNode();
                //    tnd11 = new TreeNode();
                //    tnd12 = new TreeNode();
                //    tnd13 = new TreeNode();
                //    tnd14 = new TreeNode();
                //    tnd15 = new TreeNode();
                //    tnd16 = new TreeNode();
                //    tnd17 = new TreeNode();
                //    tnd18 = new TreeNode();
                //    tnd19 = new TreeNode();
                //    tnd20 = new TreeNode();

                //    if (stringValueSplit[9].StartsWith("[New] "))
                //    {
                //        tnd9.BackColor = Color.LightBlue;
                //    }
                //}

                //if (stringValueSplit.Length > 10
                //    && tnd10.Text != stringValueSplit[10])
                //{
                //    if (stringValueSplit[10] != "")
                //    {
                //        tnd10 = new TreeNode(stringValueSplit[10]);
                //        tnd9.Nodes.Add(tnd10);
                //    }

                //    tnd11 = new TreeNode();
                //    tnd12 = new TreeNode();
                //    tnd13 = new TreeNode();
                //    tnd14 = new TreeNode();
                //    tnd15 = new TreeNode();
                //    tnd16 = new TreeNode();
                //    tnd17 = new TreeNode();
                //    tnd18 = new TreeNode();
                //    tnd19 = new TreeNode();
                //    tnd20 = new TreeNode();

                //    if (stringValueSplit[10].StartsWith("[New] "))
                //    {
                //        tnd10.BackColor = Color.LightBlue;
                //    }
                //}

                //if (stringValueSplit.Length > 11
                //    && tnd11.Text != stringValueSplit[11])
                //{
                //    if (stringValueSplit[11] != "")
                //    {
                //        tnd11 = new TreeNode(stringValueSplit[11]);
                //        tnd10.Nodes.Add(tnd11);
                //    }

                //    tnd12 = new TreeNode();
                //    tnd13 = new TreeNode();
                //    tnd14 = new TreeNode();
                //    tnd15 = new TreeNode();
                //    tnd16 = new TreeNode();
                //    tnd17 = new TreeNode();
                //    tnd18 = new TreeNode();
                //    tnd19 = new TreeNode();
                //    tnd20 = new TreeNode();

                //    if (stringValueSplit[11].StartsWith("[New] "))
                //    {
                //        tnd11.BackColor = Color.LightBlue;
                //    }
                //}

                //if (stringValueSplit.Length > 12
                //    && tnd12.Text != stringValueSplit[12])
                //{
                //    if (stringValueSplit[12] != "")
                //    {
                //        tnd12 = new TreeNode(stringValueSplit[12]);
                //        tnd11.Nodes.Add(tnd12);
                //    }

                //    tnd13 = new TreeNode();
                //    tnd14 = new TreeNode();
                //    tnd15 = new TreeNode();
                //    tnd16 = new TreeNode();
                //    tnd17 = new TreeNode();
                //    tnd18 = new TreeNode();
                //    tnd19 = new TreeNode();
                //    tnd20 = new TreeNode();

                //    if (stringValueSplit[12].StartsWith("[New] "))
                //    {
                //        tnd12.BackColor = Color.LightBlue;
                //    }
                //}

                //if (stringValueSplit.Length > 13
                //    && tnd13.Text != stringValueSplit[13])
                //{
                //    if (stringValueSplit[13] != "")
                //    {
                //        tnd13 = new TreeNode(stringValueSplit[13]);
                //        tnd12.Nodes.Add(tnd13);
                //    }

                //    tnd14 = new TreeNode();
                //    tnd15 = new TreeNode();
                //    tnd16 = new TreeNode();
                //    tnd17 = new TreeNode();
                //    tnd18 = new TreeNode();
                //    tnd19 = new TreeNode();
                //    tnd20 = new TreeNode();

                //    if (stringValueSplit[13].StartsWith("[New] "))
                //    {
                //        tnd13.BackColor = Color.LightBlue;
                //    }
                //}

                //if (stringValueSplit.Length > 14
                //    && tnd14.Text != stringValueSplit[14])
                //{
                //    if (stringValueSplit[14] != "")
                //    {
                //        tnd14 = new TreeNode(stringValueSplit[14]);
                //        tnd13.Nodes.Add(tnd14);
                //    }

                //    tnd15 = new TreeNode();
                //    tnd16 = new TreeNode();
                //    tnd17 = new TreeNode();
                //    tnd18 = new TreeNode();
                //    tnd19 = new TreeNode();
                //    tnd20 = new TreeNode();

                //    if (stringValueSplit[14].StartsWith("[New] "))
                //    {
                //        tnd14.BackColor = Color.LightBlue;
                //    }
                //}

                //if (stringValueSplit.Length > 15
                //    && tnd15.Text != stringValueSplit[15])
                //{
                //    if (stringValueSplit[15] != "")
                //    {
                //        tnd15 = new TreeNode(stringValueSplit[15]);
                //        tnd14.Nodes.Add(tnd15);
                //    }

                //    tnd16 = new TreeNode();
                //    tnd17 = new TreeNode();
                //    tnd18 = new TreeNode();
                //    tnd19 = new TreeNode();
                //    tnd20 = new TreeNode();

                //    if (stringValueSplit[15].StartsWith("[New] "))
                //    {
                //        tnd15.BackColor = Color.LightBlue;
                //    }
                //}

                //if (stringValueSplit.Length > 16
                //    && tnd16.Text != stringValueSplit[16])
                //{
                //    if (stringValueSplit[16] != "")
                //    {
                //        tnd16 = new TreeNode(stringValueSplit[16]);
                //        tnd15.Nodes.Add(tnd16);
                //    }

                //    tnd17 = new TreeNode();
                //    tnd18 = new TreeNode();
                //    tnd19 = new TreeNode();
                //    tnd20 = new TreeNode();

                //    if (stringValueSplit[16].StartsWith("[New] "))
                //    {
                //        tnd16.BackColor = Color.LightBlue;
                //    }
                //}

                //if (stringValueSplit.Length > 17
                //    && tnd17.Text != stringValueSplit[17])
                //{
                //    if (stringValueSplit[17] != "")
                //    {
                //        tnd17 = new TreeNode(stringValueSplit[17]);
                //        tnd16.Nodes.Add(tnd17);
                //    }

                //    tnd18 = new TreeNode();
                //    tnd19 = new TreeNode();
                //    tnd20 = new TreeNode();

                //    if (stringValueSplit[17].StartsWith("[New] "))
                //    {
                //        tnd17.BackColor = Color.LightBlue;
                //    }
                //}

                //if (stringValueSplit.Length > 18
                //    && tnd18.Text != stringValueSplit[18])
                //{
                //    if (stringValueSplit[18] != "")
                //    {
                //        tnd18 = new TreeNode(stringValueSplit[18]);
                //        tnd17.Nodes.Add(tnd18);
                //    }

                //    tnd19 = new TreeNode();
                //    tnd20 = new TreeNode();

                //    if (stringValueSplit[18].StartsWith("[New] "))
                //    {
                //        tnd18.BackColor = Color.LightBlue;
                //    }
                //}

                //if (stringValueSplit.Length > 19
                //    && tnd19.Text != stringValueSplit[19])
                //{
                //    if (stringValueSplit[19] != "")
                //    {
                //        tnd19 = new TreeNode(stringValueSplit[19]);
                //        tnd18.Nodes.Add(tnd19);
                //    }

                //    tnd20 = new TreeNode();

                //    if (stringValueSplit[19].StartsWith("[New] "))
                //    {
                //        tnd19.BackColor = Color.LightBlue;
                //    }
                //}

                //if (stringValueSplit.Length > 20
                //    && tnd20.Text != stringValueSplit[20])
                //{
                //    if (stringValueSplit[20] != "")
                //    {
                //        tnd20 = new TreeNode(stringValueSplit[20]);
                //        tnd19.Nodes.Add(tnd20);
                //    }

                //    if (stringValueSplit[20].StartsWith("[New] "))
                //    {
                //        tnd20.BackColor = Color.LightBlue;
                //    }
                //}
            }

            treeViewDifferences.Nodes.Add(tnd1);

            mstrFileComparison.Clear();
            compFileComparison.Clear();
            comparisonResults.Clear();

            this.btnExport.Enabled = true;
            this.cbExportXML.Enabled = true;
        }

        /**********************************************************************************************************************/
        private void treeViewDifference_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (runTreeNodeSelector == true)
            {
                runTreeNodeSelector = false;
            }
            else
            {
                return;
            }

            TreeNode tn = e.Node;

            if (tn.Checked == true)
            {
                if (tn.Nodes.Count > 0)
                {
                    foreach (TreeNode cNode in tn.Nodes)
                    {
                        cNode.Checked = true;
                    }
                }

                // Now go up the chain to the parent
                if (tn.Parent != null)
                {
                    tn.Parent.Checked = true;

                    if (tn.Parent.Parent != null)
                    {
                        tn.Parent.Parent.Checked = true;

                        if (tn.Parent.Parent.Parent != null)
                        {
                            tn.Parent.Parent.Parent.Checked = true;

                            if (tn.Parent.Parent.Parent.Parent != null)
                            {
                                tn.Parent.Parent.Parent.Parent.Checked = true;

                                if (tn.Parent.Parent.Parent.Parent.Parent != null)
                                {
                                    tn.Parent.Parent.Parent.Parent.Parent.Checked = true;
                                }
                            }
                        }
                    }
                }

                if (tn.Nodes.Count > 0)
                {
                    foreach (TreeNode cnd1 in tn.Nodes)
                    {
                        cnd1.Checked = true;

                        if (cnd1.Nodes.Count > 0)
                        {
                            foreach (TreeNode cnd2 in cnd1.Nodes)
                            {
                                cnd2.Checked = true;

                                if (cnd2.Nodes.Count > 0)
                                {
                                    foreach (TreeNode cnd3 in cnd2.Nodes)
                                    {
                                        cnd3.Checked = true;

                                        if (cnd3.Nodes.Count > 0)
                                        {
                                            foreach (TreeNode cnd4 in cnd3.Nodes)
                                            {
                                                cnd4.Checked = true;

                                                if (cnd4.Nodes.Count > 0)
                                                {
                                                    foreach (TreeNode cnd5 in cnd4.Nodes)
                                                    {
                                                        cnd5.Checked = true;
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
            else if (tn.Checked == false && tn.Nodes.Count > 0)
            {
                foreach (TreeNode cnd1 in tn.Nodes)
                {
                    cnd1.Checked = false;

                    if (cnd1.Nodes.Count > 0)
                    {
                        foreach (TreeNode cnd2 in cnd1.Nodes)
                        {
                            cnd2.Checked = false;

                            if (cnd2.Nodes.Count > 0)
                            {
                                foreach (TreeNode cnd3 in cnd2.Nodes)
                                {
                                    cnd3.Checked = false;

                                    if (cnd3.Nodes.Count > 0)
                                    {
                                        foreach (TreeNode cnd4 in cnd3.Nodes)
                                        {
                                            cnd4.Checked = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            runTreeNodeSelector = true;
        }

        private void tbFromFolder_DoubleClick(object sender, EventArgs e)
        {
            String selectedPath = UtilityClass.folderBrowserSelectPath("Select the Compare From Folder",
                                                                          false,
                                                                          FolderEnum.ReadFrom,
                                                                          Properties.Settings.Default.MetadataComparisonLastCompareFrom);

            if (selectedPath != "")
            {
                this.tbFromFolder.Text = selectedPath;
                Properties.Settings.Default.MetadataComparisonLastCompareFrom = selectedPath;
                Properties.Settings.Default.Save();
            }
        }

        private void tbToFolder_DoubleClick(object sender, EventArgs e)
        {
            String selectedPath = UtilityClass.folderBrowserSelectPath("Select the Compare To Folder",
                                                                        false,
                                                                        FolderEnum.ReadFrom,
                                                                        Properties.Settings.Default.MetadataComparisonLastCompareTo);

            if (selectedPath != "")
            {
                this.tbToFolder.Text = selectedPath;
                Properties.Settings.Default.MetadataComparisonLastCompareTo = selectedPath;
                Properties.Settings.Default.Save();
            }
        }

        /*******************************************************************************************************************************/

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (this.tbFromFolder.Text == "") return;

            if (this.cmbExportType.Text == "Export All to HTML")
            {
                exportFolderAndTypesToHTML();
            }
            else if (this.cmbExportType.Text == "Export All to CSV")
            {
                exportToCSV(false);
            }
            else if (this.cmbExportType.Text == "Export All to Excel")
            {
                DialogResult mbOkOrCancel = MessageBox.Show("Exporting to Excel can be a very long process. Would you like to continue?", "Continue", MessageBoxButtons.OKCancel);

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
        }

        private void exportFolderAndTypesToExcel()
        {
            // TODO: Come back to this
            //Key               //Folder           //Metadata Type
            //Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, Dictionary<String, List<String>>>>>>>>>>>
            //    diffDictionary = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>>>>>>>>();

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
            xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 3].Value = "Metadata Type";           // tnd3.Text
            xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 4].Value = "Object New Or Updated";   // tnd1.Text Split
            xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 5].Value = "Object Name";             // tnd2.Text
            xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 6].Value = "Metadata Field";          // tnd4.Text
            xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 7].Value = "Object Variable New Or Updated";  // tn5.Text Split
            xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 8].Value = "Object Variable";         // tn5.Text
            xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 9].Value = "XML Value";               // tn5.Text

            generalRowStart++;

            foreach (TreeNode tnd1 in this.treeViewDifferences.Nodes)
            {
                foreach (TreeNode tnd2 in tnd1.Nodes)
                {
                    //Microsoft.Office.Interop.Excel.Range rng;
                    //rng = xlWorksheetGeneral.Range[xlWorksheetGeneral.Cells[generalRowStart, 1], xlWorksheetGeneral.Cells[generalRowStart, 10]];
                    //rng.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbAliceBlue;
                    //generalRowStart++;

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
                                                xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 3].Value = tnd3.Text;               // tnd3: Metadata Type
                                                xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 4].Value = tnd2ObjectNewOrUpdated;  // File Name [New] / [Updated]
                                                xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 5].Value = tnd2ObjectName;          // File Name
                                                xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 6].Value = tnd4.Text;               // tnd4: Metadata sub-type (Metadata Field)
                                                xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 7].Value = tnd5ObjectVarNewOrUpdated;
                                                xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 8].Value = tnd5ObjectVar;

                                                if (this.cbExportXML.Checked == true)
                                                {
                                                    foreach (TreeNode tnd6 in tnd5.Nodes)
                                                    {
                                                        xlWorksheetGeneral.Cells[generalRowStart, generalColStart].Value = generalRowStart;
                                                        xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 1].Value = key;
                                                        xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 2].Value = tnd1.Text;               // tnd1.Text - Folder Name
                                                        xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 3].Value = tnd3.Text;               // tnd3: Metadata Type
                                                        xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 4].Value = tnd2ObjectNewOrUpdated;  // File Name [New] / [Updated]
                                                        xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 5].Value = tnd2ObjectName;          // File Name
                                                        xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 6].Value = tnd4.Text;               // tnd4: Metadata sub-type (Metadata Field)
                                                        xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 7].Value = tnd5ObjectVarNewOrUpdated;
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
                                    xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 3].Value = MetadataDifferenceProcessing.folderToType(tnd1.Text, "");
                                    xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 4].Value = tnd2ObjectNewOrUpdated;
                                    xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 5].Value = tnd2ObjectName;
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
                            xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 3].Value = MetadataDifferenceProcessing.folderToType(tnd1.Text, "");
                            xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 4].Value = tnd2ObjectNewOrUpdated;
                            xlWorksheetGeneral.Cells[generalRowStart, generalColStart + 5].Value = tnd2ObjectName;
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

        private void exportFolderAndTypesToHTML()
        {
            
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

        /*******************************************************************************************************************************/
        private void GenerateDeploymentPackage_Click(object sender, EventArgs e)
        {
            if (this.tbFromFolder.Text == "")
            {
                MessageBox.Show("Please select a Read-From folder", "Missing Read-From Folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            HashSet<String> treeNodeDiffs = new HashSet<string>();
            XmlNodeParser xmlNodeParser= new XmlNodeParser();
            treeNodeDiffs = xmlNodeParser.buildTreeNodeDiffs1(this.treeViewDifferences.Nodes);

            GenerateDeploymentPackage gdp = new GenerateDeploymentPackage();

            gdp.tbProjectFolder.Text = this.tbFromFolder.Text;
            gdp.populateMetadataTreeViewFromDiff(treeNodeDiffs);
            gdp.Show();
        }

        private void treeViewDifferences_DoubleClick(object sender, EventArgs e)
        {
            System.Windows.Forms.MouseEventArgs evtArgs = (System.Windows.Forms.MouseEventArgs)e;

            if (evtArgs.Button == MouseButtons.Left)
            {
                TreeView tv = (TreeView)sender;

                if (tv.SelectedNode != null)
                {
                    String originFile = tv.SelectedNode.FullPath;
                    if (originFile.Contains("[New]"))
                    {
                        originFile = originFile.Replace("[New] ", "");
                    }
                    else if (originFile.Contains("[Updated]"))
                    {
                        originFile = originFile.Replace("[Updated] ", "");
                    }

                    String pathToOriginFile = "\"" + this.tbFromFolder.Text + "\\" + originFile;
                    String pathToCompareFile = "\"" + this.tbToFolder.Text + "\\" + originFile;

                    if (Properties.Settings.Default.DefaultTextEditorPath == "")
                    {
                        Process procOrigin = Process.Start(@"notepad.exe", pathToOriginFile);
                        Process procCompare = Process.Start(@"notepad.exe", pathToCompareFile);
                    }
                    else
                    {
                        Process procOrigin = Process.Start(@Properties.Settings.Default.DefaultTextEditorPath, pathToOriginFile);
                        Process procCompare = Process.Start(@Properties.Settings.Default.DefaultTextEditorPath, pathToCompareFile);
                    }
                }
            }
        }
    }
}
