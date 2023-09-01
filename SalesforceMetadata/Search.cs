using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesforceMetadata
{
    public partial class frmSearch : System.Windows.Forms.Form
    {
        private List<String> filePathsInDirectory;

        public frmSearch()
        {
            InitializeComponent();
            populateLastSearchLocation();
        }

        private void populateLastSearchLocation()
        {
            this.tbSearchLocation.Text = Properties.Settings.Default.LastSearchLocation;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.rtbResults.Text = "";

            this.searchProgressBar.Visible = true;
            this.searchProgressBar.Minimum = 1;
            this.searchProgressBar.Value = 1;
            this.searchProgressBar.Step = 1;

            filePathsInDirectory = new List<string>();

            HashSet<String> folderSkips = new HashSet<string>();
            folderSkips.Add("objectTranslations");
            folderSkips.Add("profiles");
            folderSkips.Add("permissionsets");
            folderSkips.Add("reports");
            folderSkips.Add("reportTypes");

            if (this.tbSearchLocation.Text == "")
            {
                MessageBox.Show("Please select a location to search in the location box");
                return;
            }

            // Search for files with a specific extension
            if (this.tbSearchString.Text == ""
                && this.tbFileExtension.Text != "")
            {
                List<String> subdirectorySearchCompleted = new List<String>();

                // Escape any characters in the search String first
                // Get each folder and subfolder
                List<String> subDirectoryList = new List<String>();
                subDirectoryList.Add(this.tbSearchLocation.Text);
                subDirectoryList.AddRange(getSubdirectories(this.tbSearchLocation.Text));

                Boolean resultsFound = false;
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
                                    if (cbIncludeFileName.Checked == true) this.rtbResults.Text = this.rtbResults.Text + "Searching: " + files[j] + Environment.NewLine;

                                    FileInfo fi = new FileInfo(files[j]);
                                    if (fi.Extension == this.tbFileExtension.Text)
                                    {
                                        if (resultsFound == false) resultsFound = true;
                                        this.rtbResults.Text = this.rtbResults.Text + fi.Name + " " + files[j] + Environment.NewLine;
                                    }
                                }
                            }
                        }
                        catch (Exception exc)
                        {
                            this.rtbResults.Text = this.rtbResults.Text + exc.Message + Environment.NewLine;
                        }

                        subdirectorySearchCompleted.Add(subDirectoryList[i]);
                    }

                    // Check if there are any additional sub directories in the current directory and add them to the list
                    List<String> subDirectories = new List<String>();
                    for (Int32 i = 0; i < subDirectoryList.Count; i++)
                    {
                        if (subDirectoryList[i] != this.tbSearchLocation.Text)
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

                if (resultsFound == false)
                {
                    this.rtbResults.Text = this.rtbResults.Text + "Search complete. No results found in the files searched.";
                }
            }
            // Search in contents of ALL files
            else if (this.tbSearchString.Text != ""
                    && this.tbFileExtension.Text == "")
            {
                List<String> subdirectorySearchCompleted = new List<String>();

                // Escape any characters in the search String first
                // Get each folder and subfolder
                List<String> subDirectoryList = new List<String>();
                List<String> fileList = new List<string>();
                subDirectoryList.Add(this.tbSearchLocation.Text);
                subDirectoryList.AddRange(getSubdirectories(this.tbSearchLocation.Text));


                Boolean resultsFound = false;
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
                        //Debug.WriteLine(subDirectoryList[0]);

                        try
                        {
                            // Get all files in the current directory
                            String[] files = Directory.GetFiles(subDirectoryList[i]);
                            if (files.Length > 0)
                            {
                                fileList.AddRange(files);
                            }
                        }
                        catch (Exception exc)
                        {
                            this.rtbResults.Text = this.rtbResults.Text + exc.Message + Environment.NewLine;
                        }

                        subdirectorySearchCompleted.Add(subDirectoryList[i]);
                    }

                    // Check if there are any additional sub directories in the current directory and add them to the list
                    List<String> subDirectories = new List<String>();
                    for (Int32 i = 0; i < subDirectoryList.Count; i++)
                    {
                        if (subDirectoryList[i] != this.tbSearchLocation.Text)
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


                // Loop through the Files list and search for the text entered
                this.searchProgressBar.Maximum = fileList.Count;

                List<String> filePathAdded = new List<string>();

                foreach (String fl in fileList)
                {
                    Boolean blContinue = false;

                    String[] fileSplit = fl.Split('\\');
                    for (Int32 i = 0; i < fileSplit.Length; i++)
                    {
                        if (fileSplit[i] == "unpackaged" 
                            && this.cbSearchAll.Checked == false
                            && folderSkips.Contains(fileSplit[i+1]))
                        {
                            blContinue = true;
                        }
                    }

                    if (blContinue) continue;

                    if (cbIncludeFileName.Checked == true) this.rtbResults.Text = this.rtbResults.Text + "Searching: " + fl + Environment.NewLine;

                    // Open each file
                    // Read each line 
                    // Determine if the search String is in the line
                    // if so, write that file name to the Rich Text Box
                    StreamReader sr = File.OpenText(fl);
                    Int32 m = 0;
                    while (sr.EndOfStream == false)
                    {
                        m++;
                        String srLine = sr.ReadLine();
                        if (srLine.ToLower().Contains(this.tbSearchString.Text.ToLower()))
                        {
                            if (resultsFound == false) resultsFound = true;

                            if (!filePathAdded.Contains(fl))
                            {
                                this.rtbResults.Text = this.rtbResults.Text + fl + Environment.NewLine;
                                filePathAdded.Add(fl);
                            }
                            
                            this.rtbResults.Text = this.rtbResults.Text + "    Line: " + m.ToString() + "  " + srLine + Environment.NewLine + Environment.NewLine;
                        }
                    }

                    sr.Close();
                    sr.Dispose();

                    this.searchProgressBar.PerformStep();
                }


                if (resultsFound == false)
                {
                    this.rtbResults.Text = this.rtbResults.Text + "Search complete. No results found in the files searched.";
                }

            }
            // Search for text in only specific file extensions
            else if (this.tbSearchString.Text != ""
                    && this.tbFileExtension.Text != "")
            {

            }

            MessageBox.Show("Search Complete");
        }

        private void tbLocation_DoubleClick(object sender, EventArgs e)
        {
            String selectedPath = UtilityClass.folderBrowserSelectPath("Select the Directory to search",
                                                                              false,
                                                                              FolderEnum.ReadFrom,
                                                                              Properties.Settings.Default.LastSearchLocation);
            if (selectedPath != "")
            {
                this.tbSearchLocation.Text = selectedPath;
                Properties.Settings.Default.LastSearchLocation = selectedPath;
                Properties.Settings.Default.Save();
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


        private List<String> getApexNamesInSubdirectories(String searchLocation)
        {
            // Escape any characters in the search String first
            // Get each folder and subfolder
            List<String> subDirectoryList = new List<String>();
            subDirectoryList.Add(searchLocation);
            subDirectoryList.AddRange(getSubdirectories(searchLocation));

            Boolean subdirectoriesExist = false;
            if (subDirectoryList.Count > 0)
            {
                subdirectoriesExist = true;
            }

            // Get the names of the Classes Visualforce Pages and Components
            // We are not going to worry about triggers at this point since the triggers cannot be instantiated
            List<String> apexObjectNames = new List<String>();
            while (subdirectoriesExist == true)
            {
                if (subDirectoryList.Count == 0) subdirectoriesExist = false;

                for (Int32 i = 0; i < subDirectoryList.Count; i++)
                {
                    // Get all files in the current directory
                    String[] files = Directory.GetFiles(subDirectoryList[i]);
                    if (files.Length > 0)
                    {
                        for (Int32 j = 0; j < files.Length; j++)
                        {
                            filePathsInDirectory.Add(files[j]);

                            if (files[j].EndsWith("cls"))
                            {
                                String[] fileStructure = files[j].Split('\\');
                                apexObjectNames.Add(fileStructure[fileStructure.Length - 1]);
                            }
                            else if (files[j].EndsWith("component"))
                            {
                                String[] fileStructure = files[j].Split('\\');
                                apexObjectNames.Add(fileStructure[fileStructure.Length - 1]);
                            }
                            else if (files[j].EndsWith("page"))
                            {
                                String[] fileStructure = files[j].Split('\\');
                                apexObjectNames.Add(fileStructure[fileStructure.Length - 1]);
                            }
                        }
                    }
                }

                // Check if there are any additional sub directories in the current directory and add them to the list
                List<String> subDirectories = new List<String>();
                for (Int32 i = 0; i < subDirectoryList.Count; i++)
                {
                    List<String> sds = getSubdirectories(subDirectoryList[i]);
                    if (sds.Count > 0)
                    {
                        foreach (String s in sds)
                        {
                            subDirectories.Add(s);
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

            return apexObjectNames;

        }


        private StringBuilder getReferenceToFromOtherApexObjects(List<String> apexObjects)
        {
            // Key will be the file name
            StringBuilder results = new StringBuilder();

            foreach (String objName in apexObjects)
            {
                String objNameNoExt = "";
                if (objName.EndsWith("cls"))
                {
                    objNameNoExt = objName.Remove(objName.Length - 4, 4);
                }
                else if (objName.EndsWith("component"))
                {
                    objNameNoExt = objName.Remove(objName.Length - 10, 10);
                }
                else if (objName.EndsWith("page"))
                {
                    objNameNoExt = objName.Remove(objName.Length - 5, 5);
                }

                foreach (String fullFileName in this.filePathsInDirectory)
                {
                    // Open the file and search line by line to determine if the Apex Object Name is referenced

                    StreamReader sr = File.OpenText(fullFileName);
                    Int32 m = 0;
                    while (sr.EndOfStream == false)
                    {
                        m++;
                        if (sr.ReadLine().Contains(objNameNoExt))
                        {
                            String[] fileStructure = fullFileName.Split('\\');
                            results.Append(objName + " - Found In: " + fileStructure[fileStructure.Length - 1] + " At Line: " + m.ToString() + Environment.NewLine);
                        }
                    }

                    sr.Close();
                }

                results.Append(Environment.NewLine);
            }

            return results;
        }


        private void btnSearchFileExtension_Click(object sender, EventArgs e)
        {
            this.rtbResults.Text = "";

            if (this.tbSearchLocation.Text == "")
            {
                MessageBox.Show("Please select a location to search in the location box");
                return;
            }

            List<String> subdirectorySearchCompleted = new List<String>();

            // Escape any characters in the search String first
            // Get each folder and subfolder
            List<String> subDirectoryList = new List<String>();
            subDirectoryList.Add(this.tbSearchLocation.Text);
            subDirectoryList.AddRange(getSubdirectories(this.tbSearchLocation.Text));

            Boolean resultsFound = false;
            Boolean subdirectoriesExist = false;
            if (subDirectoryList.Count > 0)
            {
                subdirectoriesExist = true;
            }

            String startsWithValue = "";
            Int32 intStartsWithLen = 0;
            if (this.cmbSearchFilter.Text == "Starts With")
            {
                startsWithValue = this.tbFileExtension.Text;
                intStartsWithLen = startsWithValue.Length;
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
                                if (cbIncludeFileName.Checked == true) this.rtbResults.Text = this.rtbResults.Text + "Searching: " + files[j] + Environment.NewLine;

                                // TODO: Move this into a separate private method and return the results
                                if (this.cmbSearchFilter.Text == "By File Extension")
                                {
                                    String[] fileNameSplit = files[j].Split('.');

                                    String fileExt = fileNameSplit[fileNameSplit.Length - 1];

                                    if (fileExt == this.tbFileExtension.Text)
                                    {
                                        resultsFound = true;
                                        this.rtbResults.Text = this.rtbResults.Text + files[j] + Environment.NewLine;
                                    }
                                }
                                else if (this.cmbSearchFilter.Text == "Starts With")
                                {
                                    String[] fileNameSplit = files[j].Split('\\');

                                    String fileName = fileNameSplit[fileNameSplit.Length - 1];

                                    if (fileName.StartsWith(startsWithValue))
                                    {
                                        this.rtbResults.Text = this.rtbResults.Text + fileNameSplit[7] + '\t' + fileNameSplit[8] + Environment.NewLine;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        this.rtbResults.Text = this.rtbResults.Text + exc.Message + Environment.NewLine;
                    }

                    subdirectorySearchCompleted.Add(subDirectoryList[i]);
                }

                // Check if there are any additional sub directories in the current directory and add them to the list
                List<String> subDirectories = new List<String>();
                for (Int32 i = 0; i < subDirectoryList.Count; i++)
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

            if (resultsFound == false)
            {
                this.rtbResults.Text = this.rtbResults.Text + "Search complete. No results found in the files searched.";
            }
        }
    }
}
