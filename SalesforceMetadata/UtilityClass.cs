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
            String[] apiArray = new String[] {"35.0",
                                              "36.0",
                                              "37.0",
                                              "38.0",
                                              "39.0",
                                              "40.0",
                                              "41.0",
                                              "42.0",
                                              "43.0",
                                              "44.0",
                                              "45.0",
                                              "46.0",
                                              "47.0",
                                              "48.0",
                                              "49.0",
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
                                              "60.0"};

            return apiArray;
        }


        public static String folderBrowserSelectPath(String descr, Boolean showNewFolderBtn, FolderEnum fe)
        {
            String selectedFolderPath = "";

            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (descr != "") fbd.Description = descr;

            if (fe == FolderEnum.ReadFrom && Properties.Settings.Default.LastFolderReadLocation != "")
            {
                fbd.SelectedPath = Properties.Settings.Default.LastFolderReadLocation;
            }
            else if (fe == FolderEnum.SaveTo && Properties.Settings.Default.LastFolderSaveLocation != "")
            {
                fbd.SelectedPath = Properties.Settings.Default.LastFolderSaveLocation;
            }

            fbd.ShowNewFolderButton = showNewFolderBtn;

            fbd.ShowDialog();

            if (fbd.SelectedPath != null && fbd.SelectedPath != "")
            {
                selectedFolderPath = fbd.SelectedPath;
            }

            if (fe == FolderEnum.ReadFrom)
            {
                Properties.Settings.Default.LastFolderReadLocation = selectedFolderPath;
            }
            else if (fe == FolderEnum.SaveTo)
            {
                Properties.Settings.Default.LastFolderSaveLocation = selectedFolderPath;
            }

            Properties.Settings.Default.Save();

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
