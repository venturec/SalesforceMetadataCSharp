using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SalesforceMetadata.PartnerWSDL;


namespace SalesforceMetadata
{
    public class UtilityClass
    {
        public static HashSet<String> hrefsExtracted;
        public static HashSet<String> retrievedUrls;
        public static String partialUrl = "";

        public enum REQUESTINGORG{ FROMORG = 1,
                                   TOORG = 2 };

        // Populate the Record Type checked list box
        // Loop through the inner text of the xml returned
        // System.Xml.XmlElement[]
        // recordTypeQueryResults.any[0] = DeveloperName
        // recordTypeQueryResults.any[1] = Name
        // recordTypeQueryResults.any[2] = SobjectType
        //
        // LocalName
        // InnerText


        public static String[] generateAPIArray()
        {
            String[] apiArray = new String[] {
                "50.0",
                "51.0",
                "52.0",
                "53.0",
                "54.0",
                "55.0",
                "56.0",
                "57.0",
                "58.0",
                "59.0",
                "60.0",
                "61.0",
                "62.0",
                "63.0",
                "64.0",
                "65.0",
                "66.0",
                "67.0",
                "68.0",
                "69.0",
                "70.0"
            };

            return apiArray;
        }


        public static String folderBrowserSelectPath(String descr, Boolean showNewFolderBtn, FolderEnum fe, String lastSelectedPath)
        {
            String selectedFolderPath = "";

            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (descr != "") fbd.Description = descr;

            if (fe == FolderEnum.ReadFrom && lastSelectedPath != "")
            {
                fbd.SelectedPath = lastSelectedPath;
            }
            else if (fe == FolderEnum.SaveTo && lastSelectedPath != "")
            {
                fbd.SelectedPath = lastSelectedPath;
            }

            fbd.ShowNewFolderButton = showNewFolderBtn;

            if (fbd.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                if (fbd.SelectedPath != null && fbd.SelectedPath != "")
                {
                    selectedFolderPath = fbd.SelectedPath;
                }
            }

            return selectedFolderPath;
        }

        public static Boolean microsoftExcelInstalledCheck()
        {
            Boolean msExcelInstalled = false;

            Type excelType = Type.GetTypeFromProgID("Excel.Application");

            if (excelType == null)
            {
            }
            else
            {
                msExcelInstalled = true;
            }

            return msExcelInstalled;
        }

        public static void copyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(sourceDir);

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dirInfo.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    copyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }

        public static void deleteAllFoldersAndFiles(string sourceDir, bool recursive)
        {
            DirectoryInfo di = new DirectoryInfo(sourceDir);
            foreach (FileInfo file in di.GetFiles()) 
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(recursive);
            }
        }

        public static Int32 getPhysicalProcessors()
        {
            return Environment.ProcessorCount;
        }

        public static Int32 getCPUCoreCount()
        {
            int coreCount = 0;
            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                coreCount += int.Parse(item["NumberOfCores"].ToString());
            }

            return coreCount;
        }

        public static Int32 getLogicalProcessors()
        {
            Int32 logicalProcessors = 0;
            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem").Get())
            {
                //logicalProcessors = (Int32)item["NumberOfLogicalProcessors"];
                UInt32 lp = (UInt32)item["NumberOfLogicalProcessors"];
                logicalProcessors = Convert.ToInt32(lp);
            }

            return logicalProcessors;
        }

    }
}
