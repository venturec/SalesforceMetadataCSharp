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
    }
}
