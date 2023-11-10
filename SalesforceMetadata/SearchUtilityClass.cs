using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SalesforceMetadata
{
    class SearchUtilityClass
    {
        public static List<String> searchForObjectName(String mainDirectoryName, String objectDirectoryName, String objectFileName)
        {
            List<String> rtnResults = new List<String>();

            // Search in contents of ALL files
            List<String> subdirectorySearchCompleted = new List<String>();

            // Escape any characters in the search String first
            // Get each folder and subfolder
            List<String> subDirectoryList = new List<String>();
            subDirectoryList.AddRange(getSubdirectories(mainDirectoryName));

            //Boolean subdirectoriesExist = false;
            //if (subDirectoryList.Count > 0)
            //{
            //    subdirectoriesExist = true;
            //}

            // Get the Aura and LWC subdirectories populated so that the files are populated
            Int32 subdirectoryCount = subDirectoryList.Count;
            for (Int32 m = 0; m < subdirectoryCount; m++)
            {
                String[] directoryNameSplit = subDirectoryList[m].Split('\\');
                String directoryName = directoryNameSplit[directoryNameSplit.Length - 1];

                // Get all files in the current directory
                if (directoryName == "aura"
                    || directoryName == "lwc")
                {
                    // Get the subdirectories
                    subDirectoryList.AddRange(getSubdirectories(subDirectoryList[m]));
                }
            }

            for (Int32 i = 0; i < subDirectoryList.Count; i++)
            {
                try
                {
                    String[] files = Directory.GetFiles(subDirectoryList[i]);

                    if (files.Length > 0)
                    {
                        for (Int32 j = 0; j < files.Length; j++)
                        {
                            String[] parsedFileName = files[j].Split('\\');
                            if (parsedFileName[parsedFileName.Length - 1] == objectFileName) continue;

                            //Debug.WriteLine("SearchUtilityClass: " + className);
                            //Debug.WriteLine("SearchUtilityClass: " + parsedFileName[parsedFileName.Length - 1]);

                            String[] objName = objectFileName.Split('.');

                            // Open each file
                            // Read each line 
                            // Determine if the search String is in the line
                            // if so, write that file name to the Rich Text Box
                            StreamReader sr = File.OpenText(files[j]);

                            String objectTypeAndName = parsedFileName[parsedFileName.Length - 2] + " - " + parsedFileName[parsedFileName.Length - 1];

                            while (sr.EndOfStream == false)
                            {
                                String srLine = sr.ReadLine();
                                if (srLine.ToLower().Contains(objName[0].ToLower())
                                    && !rtnResults.Contains(objectTypeAndName))
                                {
                                    rtnResults.Add(objectTypeAndName);
                                }
                            }

                            sr.Close();
                            sr.Dispose();
                        }
                    }
                }
                catch (Exception exc)
                {
                        
                }

                subdirectorySearchCompleted.Add(subDirectoryList[i]);
            }

            return rtnResults;
        }


        private static List<String> getSubdirectories(String folderLocation)
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

    }
}
