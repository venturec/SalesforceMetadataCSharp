using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using Microsoft.VisualBasic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;

namespace SalesforceMetadata
{
    // When the SandboxSeedingStep2 form opens, I want to have the XML file load, parsed, and auto-select the objects in the XML file.
    // If there is a change to the list, then I will re-save the XML file.
    // TODO: Establish a default folder / filename and location. The user can select a different folder, but for the initial purposes, 
    // the XML file with the objects to bring over from production should be available in a default file folder.

    // TODO:
    // lbSalesforceSobjects: Find a way to change the height for each entry
    // Using a ListView box, I can add an additional column for the number of records to retrieve
    // "All", 150, 200, 300
    // These will be stored in the XML file.

    public partial class ConfigurationWorkbook : System.Windows.Forms.Form
    {
        public ConfigurationWorkbook()
        {
            InitializeComponent();
        }

        private void tbSelectedFolder_DoubleClick(object sender, EventArgs e)
        {
            String selectedPath = UtilityClass.folderBrowserSelectPath("Select Project Folder to Parse", 
                                                                       false, 
                                                                       FolderEnum.ReadFrom,
                                                                       Properties.Settings.Default.ConfigurationWorkbookLastReadLocation);

            if (selectedPath != "")
            {
                this.tbSelectedFolder.Text = selectedPath;
                Properties.Settings.Default.ConfigurationWorkbookLastReadLocation = selectedPath;
                Properties.Settings.Default.Save();
            }
        }

        private void tbSaveResultsTo_DoubleClick(object sender, EventArgs e)
        {
            String selectedPath = UtilityClass.folderBrowserSelectPath("Save Results to...", 
                                                                       true, 
                                                                       FolderEnum.SaveTo,
                                                                       Properties.Settings.Default.ConfigurationWorkbookLastSaveLocation);

            if (selectedPath != "")
            {
                this.tbSaveResultsTo.Text = selectedPath;
                Properties.Settings.Default.ConfigurationWorkbookLastSaveLocation = selectedPath;
                Properties.Settings.Default.Save();
            }
        }

        private void btnGenerateConfigReportAsHTML_Click(object sender, EventArgs e)
        {
            if (tbSelectedFolder.Text == null || tbSelectedFolder.Text == "")
            {
                MessageBox.Show("Please select a folder with the metadata to parse");
                return;
            }

            if (tbSaveResultsTo.Text == null || tbSaveResultsTo.Text == "")
            {
                MessageBox.Show("Please select a file to save the results to");
                return;
            }

            Directory.CreateDirectory(tbSaveResultsTo.Text + "\\styles");

            String[] folderSplit = this.tbSelectedFolder.Text.Split('\\');
            String companyName = folderSplit[folderSplit.Length - 2];

            // Build the stylesheet
            buildStylesheet(tbSaveResultsTo.Text + "\\styles");

            // Build the core summary HTML page
            StreamWriter swSummary = new StreamWriter(this.tbSaveResultsTo.Text + "\\Summary.html");
            swSummary.WriteLine("<!DOCTYPE html>");
            swSummary.WriteLine("<html>");
            swSummary.WriteLine("<head>");
            swSummary.WriteLine("<title>" + companyName.ToUpper() + " Configuration Report</title>");
            swSummary.WriteLine("<link rel='stylesheet' href='./styles/configworkbook.css'>");
            swSummary.WriteLine("</head>");
            swSummary.WriteLine("<body>");
            swSummary.WriteLine("<h1 class=\"pageheader\">" + companyName.ToUpper() + " Configuration Report</h1>");
            swSummary.WriteLine("<div>&nbsp;</div>");
            swSummary.WriteLine("<div>" + Environment.NewLine +
                                "<table>" + Environment.NewLine +
                                "<tbody>" + Environment.NewLine +
                                "<tr>" + Environment.NewLine +
                                "<td class=\"tdcolumnheader columnWidth125\">Created Date</td>" + Environment.NewLine +
                                "<td class=\"tdvalue columnWidth256\">" + DateTime.Now + "</td>" + Environment.NewLine +
                                "</tr>" + Environment.NewLine +
                                "</tbody>" + Environment.NewLine +
                                "</table>" + Environment.NewLine +
                                "</div>" + Environment.NewLine +
                                "<br />" + Environment.NewLine);

            swSummary.WriteLine("<table>" + Environment.NewLine +
                                "<colgroup>" + Environment.NewLine + 
                                "<col width=\"64\" style=\"width:48pt\">" + Environment.NewLine +
                                "<col width=\"251\" style=\"width:188pt\">" + Environment.NewLine +
                                "<col width=\"64\" span=\"9\" style=\"width:48pt\">" + Environment.NewLine +
                                "</colgroup>" + Environment.NewLine +
                                "<tbody>" + Environment.NewLine);

            // Bypass Aura and LWC folders
            String[] folders = Directory.GetDirectories(tbSelectedFolder.Text);

            foreach (String fldr in folders)
            {
                String[] splitFldrName = fldr.Split('\\');

                if (splitFldrName[splitFldrName.Length - 1] == "profiles" || splitFldrName[splitFldrName.Length - 1] == "permissionsets")
                {
                    // Build the dictionary for the related items to Profiles and Permission Sets
                    // We will get all of the metadata parsed and then pass this into the 
                    // We'll do this once since it can be a long process
                    // If the Profile/Permission Set contains a key in the Dictionary, then will use the permissions provided in the Profile/Perm Set XML
                    // If not, then will add 

                    //         MetadataType
                    Dictionary<String, Dictionary<String, MetadataObjectInfo>> typesToName = new Dictionary<String, Dictionary<String, MetadataObjectInfo>>();
                    typesToName = getMetadataTypesToName(tbSelectedFolder.Text);

                    Directory.CreateDirectory(tbSaveResultsTo.Text + "\\" + splitFldrName[splitFldrName.Length - 1]);
                    buildProfilePermSetHTMLReport(fldr, swSummary, typesToName);
                }
            }
            
            swSummary.WriteLine("</tbody>" + Environment.NewLine);
            swSummary.WriteLine("</table>" + Environment.NewLine);

            swSummary.WriteLine("</body>");
            swSummary.WriteLine("</html>");

            swSummary.Close();

            MessageBox.Show("Configuration Report as HTML Complete");
        }

        private void buildStylesheet(String styleDirectory)
        {
            StreamWriter styleFile = new StreamWriter(styleDirectory + "\\configworkbook.css");

            styleFile.WriteLine("div { display:block; height:28.656px }");
            styleFile.WriteLine("a, a:hover, a:active, a:visited { color: white; }");

            styleFile.WriteLine(".accordionGreen {" +
                                " background-color: #008B8B;" +
                                " cursor: pointer;" +
                                " width:100%;" +
                                " border: none;" +
                                " text-align:left;" +
                                " color:#ECF0F1;" +
                                " outline:none;" +
                                " font-size: 14pt;" +
                                " transition: 0.4s;" +
                                "}");

            styleFile.WriteLine(".accordionBlue {" +
                                " background-color: #3F62AE;" +
                                " cursor: pointer;" +
                                " width:100%;" +
                                " border: none;" +
                                " text-align:left;" +
                                " color:#ECF0F1;" +
                                " outline:none;" +
                                " font-size: 14pt;" +
                                " transition: 0.4s;" +
                                "}");

            styleFile.WriteLine(".accordionDrkGray {" +
                    " background-color: #A9A9A9;" +
                    " cursor: pointer;" +
                    " width:100%;" +
                    " border: none;" +
                    " text-align:left;" +
                    " color:#ECF0F1;" +
                    " outline:none;" +
                    " font-size: 14pt;" +
                    " transition: 0.4s;" +
                    "}");

            styleFile.WriteLine(".active, .accordionGreen:hover {" +
                                " background-color: #3CB371;" +
                                "}");

            styleFile.WriteLine(".active, .accordionBlue:hover {" +
                                " background-color: #5F9EA0;" +
                                "}");

            styleFile.WriteLine(".active, .accordionDrkGray:hover {" +
                                " background-color: #D3D3D3;" +
                                "}");

            styleFile.WriteLine(".panel {" +
                                " padding: 0 18px;" +
                                " display: none;" +
                                " background-color: white;" +
                                " overflow: hidden;" +
                                "}");

            styleFile.WriteLine(".fontNormal{ font-style:normal; }");
            styleFile.WriteLine(".fontItalic{ font-style:italic; }");
            styleFile.WriteLine(".fontOblique{ font-style:oblique; }");

            styleFile.WriteLine(".textAlignLeft{ text-align:left; }");
            styleFile.WriteLine(".textAlignCenter{ text-align:center; }");
            styleFile.WriteLine(".textAlignRight{ text-align:right; }");

            styleFile.WriteLine(".fieldBackgroundGray{ background:#CACFD2; }");
            styleFile.WriteLine(".fieldBackgroundGunmetalBlue{ background:#EAF2F8; }");
            styleFile.WriteLine(".fieldBackgroundLightBlue{ background:#DCE6F1; }");
            styleFile.WriteLine(".fieldBackgroundDarkBlue{ background:#3F62AE; }");
            styleFile.WriteLine(".fieldBackgroundOrange{ background:#FABF8F; }");
            styleFile.WriteLine(".fieldBackgroundGreen{ background:#00B050; }");
            styleFile.WriteLine(".fieldBackgroundDarkCyan{ background:#008B8B; }");
            styleFile.WriteLine(".fieldBackgroundDarkSeaGreen{ background:#8FBC8F; }");
            styleFile.WriteLine(".fieldBackgroundLightSeaGreen{ background:#20B2AA; }");
            styleFile.WriteLine(".fieldBackgroundMediumSeaGreen{ background:#3CB371; }");
            styleFile.WriteLine(".fieldBackgroundSalmon{ background:#FA8072; }");
            styleFile.WriteLine(".fieldBackgroundIndianRed{ background:#CD5C5C; }");

            styleFile.WriteLine(".displayInlineBlock{ display:inline-block; }");

            styleFile.WriteLine(".columnWidth120{ width:120px; }");
            styleFile.WriteLine(".columnWidth125{ width:125px; }");
            styleFile.WriteLine(".columnWidth160{ width:160px; }");
            styleFile.WriteLine(".columnWidth180{ width:180px; }");
            styleFile.WriteLine(".columnWidth256{ width:256px; }");
            styleFile.WriteLine(".columnWidth300{ width:300px; }");
            styleFile.WriteLine(".columnWidth400{ width:400px; }");
            styleFile.WriteLine(".columnWidth630{ width:630px; }");

            styleFile.WriteLine(".pageheader{" +
                                " color:#4472C4;" +
                                " font-size:14.0pt;" +
                                " font-family: Arial, sans-serif;" +
                                " }");

            styleFile.WriteLine(".tdcolumnheader{" +
                                " font-weight:700;" +
                                " font-family:Arial, sans-serif;" +
                                " text-align:left;" +
                                " font-size:10.0pt;" +
                                " vertical-align: bottom;" +
                                " white-space:nowrap;" +
                                " }");

            styleFile.WriteLine(".tdvalue{" +
                                " font-weight:400;" +
                                " font-family:Arial, sans-serif;" +
                                " text-align:left;" +
                                " font-size:10.0pt;" +
                                " vertical-align: top;" +
                                " white-space:nowrap;" +
                                " }");

            styleFile.WriteLine(".trsummaryfolder{" +
                                " background-color:#008B8B;" +
                                " border:1px solid silver}");

            styleFile.WriteLine(".tdsummaryfolder{" +
                                " color:#ECF0F1;" +
                                " width:570px;" +
                                " text-align:left;" +
                                " }");

            styleFile.WriteLine(".tdsummaryblank{" +
                                " background-color:#ffffff;" +
                                " width:65px;" +
                                " text-align:left;" +
                                " }");

            styleFile.WriteLine(".tdsummaryfile{" +
                                " color:#ECF0F1;" +
                                " height:22px;" +
                                " width:505px;" +
                                " text-align:left;" +
                                " }");

            styleFile.WriteLine(".objectTitles{" +
                                " color:#ECF0F1;" +
                                " text-align:left;" +
                                " font-size:14pt;" +
                                " font-weight:700;" +
                                " font-family:Calibri, sans-serif;" +
                                " border-top:1pt solid black;" +
                                " border-right:1pt solid black;" +
                                " border-bottom:1pt solid black;" +
                                " border-left:1pt solid black;" +
                                " display:inline-block;" +
                                " height:23px;" +
                                " vertical-align:bottom;" + 
                                " }");

            styleFile.WriteLine(".objectValues{" +
                                " color:#34495E;" +
                                " font-size:12pt;" +
                                " font-weight:525;" +
                                " font-family:Calibri, sans-serif;" +
                                " border-top:1pt solid black;" +
                                " border-right:1pt solid black;" +
                                " border-bottom:1pt solid black;" +
                                " border-left:1pt solid black;" +
                                " display:inline-block;" +
                                " height:23px;" +
                                " vertical-align:bottom;" +
                                " }");

            styleFile.Close();
        }

        private String directoryNameExtract(String folderNameWithPath)
        {
            String directoryName = "";
            String[] folderParse = folderNameWithPath.Split('\\');
            directoryName = folderParse[folderParse.Length - 1];

            return directoryName;
        }

        private String fileExtension(String fileNameWithPath)
        {
            String fileExt = "";

            String[] fileParse = fileNameWithPath.Split('.');
            fileExt = fileParse[fileParse.Length - 1];

            return fileExt;
        }

        private Dictionary<String, Dictionary<String, MetadataObjectInfo>> getMetadataTypesToName(String metadataFolder)
        {
            Dictionary<String, Dictionary<String, MetadataObjectInfo>> typeToName = new Dictionary<String, Dictionary<String, MetadataObjectInfo>>();

            String[] folders = Directory.GetDirectories(tbSelectedFolder.Text);

            foreach (String folderName in folders)
            {
                String directoryName = directoryNameExtract(folderName);

                if (directoryName == "applications"
                    || directoryName == "classes"
                    || directoryName == "components"
                    || directoryName == "connectedApps"
                    || directoryName == "flowDefinitions"
                    || directoryName == "flows"
                    || directoryName == "layouts"
                    || directoryName == "pages"
                    || directoryName == "tabs"
                    || directoryName == "triggers")
                {
                    String[] files = Directory.GetFiles(folderName);

                    foreach (String file in files)
                    {
                        String[] splitFileName = file.Split('\\');
                        String[] fileName = splitFileName[splitFileName.Length - 1].Split('.');
                        String dirNameFileName = directoryName + "." + fileName[0];

                        MetadataObjectInfo moi = new MetadataObjectInfo();
                        moi.metadataObject = directoryName;
                        moi.objectName = fileName[0];

                        // We don't want to add duplicates or overwrite pre-existing date. The Classes, Triggers, VF pages, etc. have meta-xml extensions with the same file name. So we will skip over these.
                        if (typeToName.ContainsKey(directoryName))
                        {
                            if (!typeToName[directoryName].ContainsKey(fileName[0]))
                            {
                                typeToName[directoryName].Add(fileName[0], moi);
                            }
                        }
                        else
                        {
                            typeToName.Add(directoryName, new Dictionary<String, MetadataObjectInfo>());
                            typeToName[directoryName].Add(fileName[0], moi);
                        }
                    }
                }
                else if (directoryName == "objects")
                {
                    // We'll actually need to open the Xml file up to get the following:
                    // Fields
                    // Record Types

                    String[] files = Directory.GetFiles(folderName);

                    foreach (String file in files)
                    {
                        String[] splitFileName = file.Split('\\');
                        String[] fileName = splitFileName[splitFileName.Length - 1].Split('.');
                        String dirNameFileName = directoryName + "." + fileName[0];

                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(file);

                        XmlNodeList customSetting = xmlDoc.GetElementsByTagName("customSettingsType");
                        XmlNodeList objVisibility = xmlDoc.GetElementsByTagName("visibility");
                        XmlNodeList objSharingModel = xmlDoc.GetElementsByTagName("sharingModel");
                        XmlNodeList fieldNodeList = xmlDoc.GetElementsByTagName("fields");
                        XmlNodeList rtNodeList = xmlDoc.GetElementsByTagName("recordTypes");

                        MetadataObjectInfo moi = new MetadataObjectInfo();
                        moi.metadataObject = directoryName;
                        moi.objectName = fileName[0];

                        if (objVisibility != null)
                        {
                            foreach (XmlNode nd in objVisibility)
                            {
                                moi.objectVisibility = nd.InnerText;
                            }
                        }

                        if (objSharingModel != null)
                        {
                            foreach (XmlNode nd in objSharingModel)
                            {
                                moi.objectSharingModel = nd.InnerText;
                            }
                        }

                        if (customSetting != null)
                        {
                            if (fileName[0].EndsWith("__mdt"))
                            {
                                moi.isCustomMetadataType = true;
                            }
                            else
                            {
                                moi.isCustomSetting = true;
                            }
                        }

                        //if (typeToName.ContainsKey(dirNameFileName))
                        //{
                        //    typeToName[dirNameFileName].Add(moi);
                        //}

                        // Get the field references in the object
                        foreach (XmlNode fldNode in fieldNodeList)
                        {
                            if (fldNode.ParentNode.Name == "CustomObject")
                            {
                                ObjectFieldInfo ofi = new ObjectFieldInfo();
                                foreach (XmlNode fld in fldNode.ChildNodes)
                                {
                                    if (fld.Name == "fullName")
                                    {
                                        ofi.fieldName = fld.InnerText;
                                    }
                                    else if (fld.Name == "required")
                                    {
                                        ofi.fieldIsRequired = Convert.ToBoolean(fld.InnerText);
                                    }
                                }

                                String objFieldName = fileName[0] + "." + ofi.fieldName;
                                moi.objFieldInfo.Add(objFieldName, ofi);
                            }
                        }

                        // Get the record type info
                        foreach (XmlNode rtNode in rtNodeList)
                        {
                            if (rtNode.ParentNode.Name == "CustomObject")
                            {
                                ObjectRecordTypInfo orti = new ObjectRecordTypInfo();
                                foreach (XmlNode rt in rtNode.ChildNodes)
                                {
                                    if (rt.Name == "fullName")
                                    {
                                        orti.recordType = rt.InnerText;
                                    }
                                }

                                String objRtName = fileName[0] + "." + orti.recordType;
                                moi.objRecordTypeInfo.Add(objRtName, orti);
                            }
                        }

                        if (typeToName.ContainsKey(directoryName))
                        {
                            typeToName[directoryName].Add(fileName[0], moi);
                        }
                        else
                        {
                            typeToName.Add(directoryName, new Dictionary<String, MetadataObjectInfo>());
                            typeToName[directoryName].Add(fileName[0], moi);
                        }
                    }
                }
            }

            return typeToName;
        }

        public String stringColumnPosition(Int32 colNumber, String value)
        {
            String stringValue = "";

            for (Int32 i = 0; i < colNumber - 1; i++)
            {
                stringValue = stringValue + ",";
            }

            stringValue = stringValue + "\"" + value + "\"";

            return stringValue;
        }

        public void buildProfilePermSetHTMLReport(String folderName, StreamWriter swSummary, Dictionary<String, Dictionary<String, MetadataObjectInfo>> metadataObjInfo)
        {
            String[] files = Directory.GetFiles(folderName);
            String directoryName = directoryNameExtract(folderName);

            // Update the swSummary page
            swSummary.WriteLine("<tr class=\"trsummaryfolder\"><td class=\"tdsummaryfolder\" colspan=\"6\">" + directoryName + "</td></tr>");

            // Write the files to the summary page with the href links
            foreach (String fn in files) 
            {
                String[] splitFileName = fn.Split('\\');
                String[] fileName = splitFileName[splitFileName.Length - 1].Split('.');
                swSummary.WriteLine("<tr class=\"trsummaryfolder\">" +
                                    "<td class=\"tdsummaryblank\"></td>" + 
                                    "<td class=\"tdsummaryfile\" colspan=\"5\">" + 
                                    "<a href='.//" + directoryName + "//" + fileName[0] + ".html' target=\"_blank\">" + 
                                    fileName[0] +
                                    "</a></td></tr>");

                // Build the configuration files themselves
                writeFieldsAndValuesToHTML(fn, directoryName, fileName[0], metadataObjInfo);
            }
        }

        private void writeFieldsAndValuesToHTML(String fileName, String directoryName, String fullObjectName, Dictionary<String, Dictionary<String, MetadataObjectInfo>> metadataObjInfo)
        {
            XmlDocument xd = new XmlDocument();
            Boolean isXmlDocument = true;
            try
            {
                xd.Load(fileName);
            }
            catch (Exception e)
            {
                isXmlDocument = false;
            }

            if (isXmlDocument == false) return;

            StreamWriter sw = new StreamWriter(this.tbSaveResultsTo.Text + "\\" + directoryName + "\\" + fullObjectName + ".html");
            sw.WriteLine("<!DOCTYPE html>");
            sw.WriteLine("<html>");
            sw.WriteLine("<head>");
            sw.WriteLine("<title>" + fullObjectName + "</title>");
            sw.WriteLine("<link rel='stylesheet' href='../../styles/configworkbook.css'>");
            sw.WriteLine("</head>");
            sw.WriteLine("<body>");

            foreach (XmlNode nd1 in xd.ChildNodes)
            {
                if (nd1.LocalName == "xml")
                {
                    continue;
                }
                else if (nd1.NodeType == XmlNodeType.Element)
                {
                    writeObjectTypeAndName(nd1.Name, fullObjectName, sw);
                }

                if (nd1.Name == "PermissionSet")
                {
                    writeProfilesToHtml(xd, sw, metadataObjInfo);
                }
                else if (nd1.Name == "Profile")
                {
                    writeProfilesToHtml(xd, sw, metadataObjInfo);
                }
            }

            sw.WriteLine("<script>\r\n");
            sw.WriteLine("var acc = document.getElementsByClassName(\"accordionGreen\"); \r\n var i; \r\n for (i = 0; i < acc.length; i++) { \r\n acc[i].addEventListener(\"click\", function() { \r\n this.classList.toggle(\"active\"); \r\n var panel = this.nextElementSibling; \r\n if (panel.style.display === \"contents\") { \r\n panel.style.display=\"none\"; \r\n } else { \r\n panel.style.display=\"contents\"; \r\n } \r\n }); \r\n}");
            sw.WriteLine("var acc = document.getElementsByClassName(\"accordionBlue\"); \r\n var i; \r\n for (i = 0; i < acc.length; i++) { \r\n acc[i].addEventListener(\"click\", function() { \r\n this.classList.toggle(\"active\"); \r\n var panel = this.nextElementSibling; \r\n if (panel.style.display === \"contents\") { \r\n panel.style.display=\"none\"; \r\n } else { \r\n panel.style.display=\"contents\"; \r\n } \r\n }); \r\n}");
            sw.WriteLine("var acc = document.getElementsByClassName(\"accordionDrkGray\"); \r\n var i; \r\n for (i = 0; i < acc.length; i++) { \r\n acc[i].addEventListener(\"click\", function() { \r\n this.classList.toggle(\"active\"); \r\n var panel = this.nextElementSibling; \r\n if (panel.style.display === \"contents\") { \r\n panel.style.display=\"none\"; \r\n } else { \r\n panel.style.display=\"contents\"; \r\n } \r\n }); \r\n}");
            sw.WriteLine("</script>");

            sw.WriteLine("</body>");
            sw.WriteLine("</html>");
            sw.Close();
        }

        private void writeProfilesToHtml(XmlDocument xd, StreamWriter sw, Dictionary<String, Dictionary<String, MetadataObjectInfo>> metadataObjInfo)
        {
            writeProfileHeader(xd, sw);
            writeProfileUserPerms(xd, sw);
            writeProfileObjectPerms(xd, sw, metadataObjInfo);
            writeProfileObjectFieldPerms(xd, sw, metadataObjInfo);
            writeProfileRecordTypePerms(xd, sw, metadataObjInfo);
            writeProfileApplicationPerms(xd, sw, metadataObjInfo);
            writeProfileTabPerms(xd, sw, metadataObjInfo);
            writeProfileLayoutPerms(xd, sw, metadataObjInfo);
            writeProfileFlowPerms(xd, sw, metadataObjInfo);
            writeProfileClassPerms(xd, sw, metadataObjInfo);
            writeProfileVFPagePerms(xd, sw, metadataObjInfo);
            writeProfileCustomSettingsPerms(xd, sw, metadataObjInfo);
            writeProfileCustomMetadataTypePerms(xd, sw, metadataObjInfo);
            writeProfileCustomPermissionsPerms(xd, sw, metadataObjInfo);
            writeProfileCategoryGrpPerms(xd, sw);
        }

        private void writeProfileHeader(XmlDocument xd, StreamWriter sw)
        {
            XmlNodeList description = xd.GetElementsByTagName("description");
            if (description.Count > 0)
            {
                sw.WriteLine("<div>");
                sw.WriteLine("<span class=\"objectTitles fieldBackgroundDarkCyan columnWidth300\">");
                sw.WriteLine("Description");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth630\">");
                sw.WriteLine(description[0].InnerText);
                sw.WriteLine("</span>");
                sw.WriteLine("</div>");
            }

            XmlNodeList custom = xd.GetElementsByTagName("custom");
            if (custom.Count > 0)
            {
                sw.WriteLine("<div>");
                sw.WriteLine("<span class=\"objectTitles fieldBackgroundDarkCyan columnWidth300\">");
                sw.WriteLine("Custom");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth630 fontItalic\">");
                sw.WriteLine(custom[0].InnerText);
                sw.WriteLine("</span>");
                sw.WriteLine("</div>");
            }

            XmlNodeList userLicense = xd.GetElementsByTagName("userLicense");
            if (userLicense.Count > 0)
            {
                sw.WriteLine("<div>");
                sw.WriteLine("<span class=\"objectTitles fieldBackgroundDarkCyan columnWidth300\">");
                sw.WriteLine("User License");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth630 fontItalic\">");
                sw.WriteLine(userLicense[0].InnerText);
                sw.WriteLine("</span>");
                sw.WriteLine("</div>");
            }

            XmlNodeList fullName = xd.GetElementsByTagName("fullName");
            if (fullName.Count > 0)
            {
                sw.WriteLine("<div>");
                sw.WriteLine("<span class=\"objectTitles fieldBackgroundDarkCyan columnWidth300\">");
                sw.WriteLine("Full Name");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth630 fontItalic\">");
                sw.WriteLine(fullName[0].InnerText);
                sw.WriteLine("</span>");
                sw.WriteLine("</div>");
            }

            XmlNodeList label = xd.GetElementsByTagName("label");
            if (label.Count > 0)
            {
                sw.WriteLine("<div>");
                sw.WriteLine("<span class=\"objectTitles fieldBackgroundDarkCyan columnWidth300\">");
                sw.WriteLine("Label");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth630 fontItalic\">");
                sw.WriteLine(label[0].InnerText);
                sw.WriteLine("</span>");
                sw.WriteLine("</div>");
            }

            XmlNodeList license = xd.GetElementsByTagName("license");
            if (license.Count > 0)
            {
                sw.WriteLine("<div>");
                sw.WriteLine("<span class=\"objectTitles fieldBackgroundDarkCyan columnWidth300\">");
                sw.WriteLine("license");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth630 fontItalic\">");
                sw.WriteLine(license[0].InnerText);
                sw.WriteLine("</span>");
                sw.WriteLine("</div>");
            }

            XmlNodeList hasActivationRequired = xd.GetElementsByTagName("hasActivationRequired");
            if (hasActivationRequired.Count > 0)
            {
                sw.WriteLine("<div>");
                sw.WriteLine("<span class=\"objectTitles fieldBackgroundDarkCyan columnWidth300\">");
                sw.WriteLine("Has Activation Required");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth630 fontItalic\">");
                sw.WriteLine(hasActivationRequired[0].InnerText);
                sw.WriteLine("</span>");
                sw.WriteLine("</div>");
            }
        }

        private void writeProfileUserPerms(XmlDocument xd, StreamWriter sw)
        {
            XmlNodeList userPermissions = xd.GetElementsByTagName("userPermissions");
            if (userPermissions.Count > 0)
            {
                sw.WriteLine("<br />");
                sw.WriteLine("<br />");

                sw.WriteLine("<button class=\"accordionGreen\">User Permission</button>");
                sw.WriteLine("<div class=\"panel\">");

                sw.WriteLine("<div>");
                sw.WriteLine("<span class=\"objectTitles fieldBackgroundDarkBlue columnWidth400\">");
                sw.WriteLine("User Permission");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth120 fontItalic textAlignCenter\">");
                sw.WriteLine("Enabled");
                sw.WriteLine("</span>");
                sw.WriteLine("</div>");

                foreach (XmlNode nd1 in userPermissions)
                {
                    sw.WriteLine("<div>");
                    sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth400 textAlignLeft\">");
                    sw.WriteLine(nd1.ChildNodes[1].InnerText);
                    sw.WriteLine("</span>");
                    sw.WriteLine(checkTrueFalseValueForCSS(nd1.ChildNodes[0].InnerText));
                    sw.WriteLine(nd1.ChildNodes[0].InnerText);
                    sw.WriteLine("</span>");
                    sw.WriteLine("</div>");
                }

                sw.WriteLine("</div>");
            }
        }

        private void writeProfileObjectPerms(XmlDocument xd, StreamWriter sw, Dictionary<String, Dictionary<String, MetadataObjectInfo>> metadataObjInfo)
        {
            // Object permissions
            HashSet<String> objPermissionsConfigured = new HashSet<string>();
            XmlNodeList objectPermissions = xd.GetElementsByTagName("objectPermissions");
            if (objectPermissions.Count > 0)
            {
                sw.WriteLine("<br />");
                sw.WriteLine("<br />");

                sw.WriteLine("<button class=\"accordionGreen\">Object Permissions - Configured</button>");
                sw.WriteLine("<div class=\"panel\">");

                sw.WriteLine("<div>");
                sw.WriteLine("<span class=\"objectTitles fieldBackgroundDarkBlue columnWidth400\">");
                sw.WriteLine("Object Permissions");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth120 fontItalic textAlignCenter\">");
                sw.WriteLine("Create");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth120 fontItalic textAlignCenter\">");
                sw.WriteLine("Read");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth120 fontItalic textAlignCenter\">");
                sw.WriteLine("Edit");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth120 fontItalic textAlignCenter\">");
                sw.WriteLine("Delete");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth120 fontItalic textAlignCenter\">");
                sw.WriteLine("View All");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth120 fontItalic textAlignCenter\">");
                sw.WriteLine("Modify All");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth120 fontItalic textAlignCenter\">");
                sw.WriteLine("Visibility");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth180 fontItalic textAlignCenter\">");
                sw.WriteLine("Sharing Model");
                sw.WriteLine("</span>");
                sw.WriteLine("</div>");

                foreach (XmlNode nd1 in objectPermissions)
                {
                    String dirNameFileName = "objects" + "." + nd1.ChildNodes[5].InnerText;
                    objPermissionsConfigured.Add(dirNameFileName);

                    sw.WriteLine("<div>");
                    sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth400\">");
                    sw.WriteLine(nd1.ChildNodes[5].InnerText);
                    sw.WriteLine("</span>");

                    // Create
                    sw.WriteLine(checkTrueFalseValueForCSS(nd1.ChildNodes[0].InnerText));
                    sw.WriteLine(nd1.ChildNodes[0].InnerText);
                    sw.WriteLine("</span>");

                    // Read
                    sw.WriteLine(checkTrueFalseValueForCSS(nd1.ChildNodes[3].InnerText));
                    sw.WriteLine(nd1.ChildNodes[3].InnerText);
                    sw.WriteLine("</span>");

                    // Edit
                    sw.WriteLine(checkTrueFalseValueForCSS(nd1.ChildNodes[2].InnerText));
                    sw.WriteLine(nd1.ChildNodes[2].InnerText);
                    sw.WriteLine("</span>");

                    // Delete
                    sw.WriteLine(checkTrueFalseValueForCSS(nd1.ChildNodes[1].InnerText));
                    sw.WriteLine(nd1.ChildNodes[1].InnerText);
                    sw.WriteLine("</span>");

                    // View All
                    sw.WriteLine(checkTrueFalseValueForCSS(nd1.ChildNodes[6].InnerText));
                    sw.WriteLine(nd1.ChildNodes[6].InnerText);
                    sw.WriteLine("</span>");

                    // Modify All
                    sw.WriteLine(checkTrueFalseValueForCSS(nd1.ChildNodes[4].InnerText));
                    sw.WriteLine(nd1.ChildNodes[4].InnerText);
                    sw.WriteLine("</span>");

                    // Org Visibility
                    sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth120 textAlignCenter\">");
                    if (metadataObjInfo.ContainsKey("objects"))
                    {
                        if (metadataObjInfo["objects"].ContainsKey(nd1.ChildNodes[5].InnerText))
                        {
                            sw.WriteLine(metadataObjInfo["objects"][nd1.ChildNodes[5].InnerText].objectVisibility);
                        }
                        else
                        {
                            sw.WriteLine("");
                        }
                    }
                    else
                    {
                        sw.WriteLine("");
                    }

                    sw.WriteLine("</span>");

                    // Sharing Model
                    sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth180 textAlignCenter\">");
                    if (metadataObjInfo.ContainsKey("objects"))
                    {
                        if (metadataObjInfo["objects"].ContainsKey(nd1.ChildNodes[5].InnerText))
                        {
                            sw.WriteLine(metadataObjInfo["objects"][nd1.ChildNodes[5].InnerText].objectSharingModel);
                        }
                        else
                        {
                            sw.WriteLine("");
                        }
                    }
                    else
                    {
                        sw.WriteLine("");
                    }
                    sw.WriteLine("</span>");

                    sw.WriteLine("</div>");
                }

                sw.WriteLine("</div>");
            }

            // Now loop through the objects which are not included in this profile/permission set
            sw.WriteLine("<br />");
            sw.WriteLine("<br />");

            sw.WriteLine("<button class=\"accordionGreen\">Object Permissions - Not Configured</button>");
            sw.WriteLine("<div class=\"panel\">");

            sw.WriteLine("<div>");
            sw.WriteLine("<span class=\"objectTitles fieldBackgroundDarkBlue columnWidth400\">");
            sw.WriteLine("Object Permissions");
            sw.WriteLine("</span>");
            sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth120 fontItalic textAlignCenter\">");
            sw.WriteLine("Create");
            sw.WriteLine("</span>");
            sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth120 fontItalic textAlignCenter\">");
            sw.WriteLine("Read");
            sw.WriteLine("</span>");
            sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth120 fontItalic textAlignCenter\">");
            sw.WriteLine("Edit");
            sw.WriteLine("</span>");
            sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth120 fontItalic textAlignCenter\">");
            sw.WriteLine("Delete");
            sw.WriteLine("</span>");
            sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth120 fontItalic textAlignCenter\">");
            sw.WriteLine("View All");
            sw.WriteLine("</span>");
            sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth120 fontItalic textAlignCenter\">");
            sw.WriteLine("Modify All");
            sw.WriteLine("</span>");
            sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth120 fontItalic textAlignCenter\">");
            sw.WriteLine("Visibility");
            sw.WriteLine("</span>");
            sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth180 fontItalic textAlignCenter\">");
            sw.WriteLine("Sharing Model");
            sw.WriteLine("</span>");
            sw.WriteLine("</div>");

            foreach (String nd1 in metadataObjInfo["objects"].Keys)
            {
                String dirNameFileName = "objects" + "." + nd1;

                if (!objPermissionsConfigured.Contains(dirNameFileName))
                {
                    sw.WriteLine("<div>");
                    sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth400\">");
                    sw.WriteLine(nd1);
                    sw.WriteLine("</span>");

                    // Create
                    sw.WriteLine(checkTrueFalseValueForCSS("false"));
                    sw.WriteLine("false");
                    sw.WriteLine("</span>");

                    // Read
                    sw.WriteLine(checkTrueFalseValueForCSS("false"));
                    sw.WriteLine("false");
                    sw.WriteLine("</span>");

                    // Edit
                    sw.WriteLine(checkTrueFalseValueForCSS("false"));
                    sw.WriteLine("false");
                    sw.WriteLine("</span>");

                    // Delete
                    sw.WriteLine(checkTrueFalseValueForCSS("false"));
                    sw.WriteLine("false");
                    sw.WriteLine("</span>");

                    // View All
                    sw.WriteLine(checkTrueFalseValueForCSS("false"));
                    sw.WriteLine("false");
                    sw.WriteLine("</span>");

                    // Modify All
                    sw.WriteLine(checkTrueFalseValueForCSS("false"));
                    sw.WriteLine("false");
                    sw.WriteLine("</span>");

                    // Org Visibility
                    sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth120 textAlignCenter\">");
                    if (metadataObjInfo.ContainsKey("objects"))
                    {
                        if (metadataObjInfo["objects"].ContainsKey(nd1))
                        {
                            sw.WriteLine(metadataObjInfo["objects"][nd1].objectVisibility);
                        }
                        else
                        {
                            sw.WriteLine("");
                        }
                    }
                    else
                    {
                        sw.WriteLine("");
                    }
                    sw.WriteLine("</span>");

                    // Sharing Model
                    sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth180 textAlignCenter\">");
                    if (metadataObjInfo.ContainsKey("objects"))
                    {
                        if (metadataObjInfo["objects"].ContainsKey(nd1))
                        {
                            sw.WriteLine(metadataObjInfo["objects"][nd1].objectSharingModel);
                        }
                        else
                        {
                            sw.WriteLine("");
                        }
                    }
                    else
                    {
                        sw.WriteLine("");
                    }
                    sw.WriteLine("</span>");

                    sw.WriteLine("</div>");
                }
            }

            sw.WriteLine("</div>");

            // Free up memory
            objPermissionsConfigured.Clear();
        }

        private void writeProfileObjectFieldPerms(XmlDocument xd, StreamWriter sw, Dictionary<String, Dictionary<String, MetadataObjectInfo>> metadataObjInfo)
        {
            // Field Permissions
            List<String> objectTitleWritten = new List<string>();
            HashSet<String> objFieldPermissionsConfigured = new HashSet<string>();

            XmlNodeList fieldPermissions = xd.GetElementsByTagName("fieldPermissions");
            Int32 i = 0;
            if (fieldPermissions.Count > 0)
            {
                sw.WriteLine("<br />");
                sw.WriteLine("<br />");

                sw.WriteLine("<button class=\"accordionGreen\">Field Permissions - Configured</button>");
                sw.WriteLine("<div class=\"panel\">");

                foreach (XmlNode nd1 in fieldPermissions)
                {
                    String[] objectFieldSplit = nd1.ChildNodes[1].InnerText.Split('.');

                    // Write header line for the fields with an exapandable accordion
                    if (!objectTitleWritten.Contains(objectFieldSplit[0]))
                    {
                        // This sets the div for the last panel block so the accordion can work properly.
                        // Skip the first one in the list though since there would be an extra div
                        if (i > 0)
                        {
                            sw.WriteLine("</div>");
                            sw.WriteLine("<br />");
                        }

                        sw.WriteLine("<br />");
                        sw.WriteLine("<button class=\"accordionBlue\">" + objectFieldSplit[0] + "</button>");
                        sw.WriteLine("<div class=\"panel\">");
                        sw.WriteLine("<div>");
                        sw.WriteLine("<span class=\"objectTitles fieldBackgroundGreen columnWidth400\">");
                        sw.WriteLine("Field Name");
                        sw.WriteLine("</span>");
                        sw.WriteLine("<span class=\"columnWidth400 displayInlineBlock\">&nbsp;</span>");
                        sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth120 fontItalic textAlignCenter\">");
                        sw.WriteLine("Readable");
                        sw.WriteLine("</span>");
                        sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth120 fontItalic textAlignCenter\">");
                        sw.WriteLine("Editable");
                        sw.WriteLine("</span>");
                        sw.WriteLine("</div>");

                        objectTitleWritten.Add(objectFieldSplit[0]);
                        i++;
                    }

                    sw.WriteLine("<div>");
                    sw.WriteLine("<span class=\"columnWidth400 displayInlineBlock\">");
                    sw.WriteLine("&nbsp;");
                    sw.WriteLine("</span>");
                    sw.WriteLine("<span class=\"objectValues fieldBackgroundLightBlue columnWidth400\">");
                    sw.WriteLine(objectFieldSplit[1]);
                    sw.WriteLine("</span>");

                    sw.WriteLine(checkTrueFalseValueForCSS(nd1.ChildNodes[2].InnerText));
                    sw.WriteLine(nd1.ChildNodes[2].InnerText);
                    sw.WriteLine("</span>");
                    sw.WriteLine(checkTrueFalseValueForCSS(nd1.ChildNodes[0].InnerText));
                    sw.WriteLine(nd1.ChildNodes[0].InnerText);
                    sw.WriteLine("</span>");
                    sw.WriteLine("</div>");

                    objFieldPermissionsConfigured.Add(nd1.ChildNodes[1].InnerText);
                }

                sw.WriteLine("</div>"); // - closes the last ObjectFieldSplit DIV
                sw.WriteLine("</div>"); // - closes the accordion panel DIV
            }

            objectTitleWritten.Clear();

            // Now loop through the object fields which are not configured
            sw.WriteLine("<br />");
            sw.WriteLine("<br />");

            sw.WriteLine("<button class=\"accordionGreen\">Field Permissions - Not Configured</button>");
            sw.WriteLine("<div class=\"panel\">");

            i = 0;
            foreach (String nd1 in metadataObjInfo["objects"].Keys)
            {
                foreach (String nd2 in metadataObjInfo["objects"][nd1].objFieldInfo.Keys)
                {
                    String objectFieldName = nd1 + "." + nd2;

                    if (!objFieldPermissionsConfigured.Contains(objectFieldName))
                    {
                        if (!objectTitleWritten.Contains(nd1))
                        {
                            if (i > 0)
                            {
                                sw.WriteLine("</div>");
                                sw.WriteLine("<br />");
                            }

                            sw.WriteLine("<br />");
                            sw.WriteLine("<button class=\"accordionBlue\">" + nd1 + "</button>");
                            sw.WriteLine("<div class=\"panel\">");
                            sw.WriteLine("<div>");
                            sw.WriteLine("<span class=\"objectTitles fieldBackgroundGreen columnWidth400\">");
                            sw.WriteLine("Field Name");
                            sw.WriteLine("</span>");
                            sw.WriteLine("<span class=\"columnWidth400 displayInlineBlock\">&nbsp;</span>");
                            sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth120 fontItalic textAlignCenter\">");
                            sw.WriteLine("Readable");
                            sw.WriteLine("</span>");
                            sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth120 fontItalic textAlignCenter\">");
                            sw.WriteLine("Editable");
                            sw.WriteLine("</span>");
                            sw.WriteLine("</div>");

                            objectTitleWritten.Add(nd1);
                            i++;
                        }

                        sw.WriteLine("<div>");
                        sw.WriteLine("<span class=\"columnWidth400 displayInlineBlock\">");
                        sw.WriteLine("&nbsp;");
                        sw.WriteLine("</span>");
                        sw.WriteLine("<span class=\"objectValues fieldBackgroundLightBlue columnWidth400\">");
                        sw.WriteLine(nd2);
                        sw.WriteLine("</span>");

                        sw.WriteLine(checkTrueFalseValueForCSS("false"));
                        sw.WriteLine("false");
                        sw.WriteLine("</span>");
                        sw.WriteLine(checkTrueFalseValueForCSS("false"));
                        sw.WriteLine("false");
                        sw.WriteLine("</span>");
                        sw.WriteLine("</div>");

                    }
                }
            }

            sw.WriteLine("</div>");
            sw.WriteLine("</div>");


            // TODO if needed: Now loop through the object fields to find the ones which are required by default and cannot be configured (not accommodating page layout field requirements here)


            // Free up some memory
            objectTitleWritten.Clear();
            objFieldPermissionsConfigured.Clear();
        }

        private void writeProfileRecordTypePerms(XmlDocument xd, StreamWriter sw, Dictionary<String, Dictionary<String, MetadataObjectInfo>> metadataObjInfo)
        {
            HashSet<String> objPermissionsConfigured = new HashSet<string>();

            XmlNodeList recordTypeVisibilities = xd.GetElementsByTagName("recordTypeVisibilities");
            if (recordTypeVisibilities.Count > 0)
            {
                sw.WriteLine("<br />");
                sw.WriteLine("<br />");

                sw.WriteLine("<button class=\"accordionGreen\">Record Type Visibilities - Configured</button>");
                sw.WriteLine("<div class=\"panel\">");

                sw.WriteLine("<div>");
                sw.WriteLine("<span class=\"objectTitles fieldBackgroundDarkBlue columnWidth400\">");
                sw.WriteLine("Object Name");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectTitles fieldBackgroundGreen columnWidth400\">");
                sw.WriteLine("RecordType Name");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth120 fontItalic textAlignCenter\">");
                sw.WriteLine("Visible");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth120 fontItalic textAlignCenter\">");
                sw.WriteLine("Default");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth160 fontItalic textAlignCenter\">");
                sw.WriteLine("PersonAccountDefault");
                sw.WriteLine("</span>");
                sw.WriteLine("</div>");

                foreach (XmlNode nd1 in recordTypeVisibilities)
                {
                    if (nd1.ChildNodes.Count == 2)
                    {
                        String[] objectFieldSplit = nd1.ChildNodes[0].InnerText.Split('.');
                        objPermissionsConfigured.Add(nd1.ChildNodes[0].InnerText);

                        sw.WriteLine("<div>");
                        sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth400\">");
                        sw.WriteLine(objectFieldSplit[0]);
                        sw.WriteLine("</span>");
                        sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth400\">");
                        sw.WriteLine(objectFieldSplit[1]);
                        sw.WriteLine("</span>");

                        sw.WriteLine(checkTrueFalseValueForCSS(nd1.ChildNodes[1].InnerText));
                        sw.WriteLine(nd1.ChildNodes[1].InnerText);
                        sw.WriteLine("</span>");
                    }
                    else if (nd1.ChildNodes.Count == 3)
                    {
                        String[] objectFieldSplit = nd1.ChildNodes[1].InnerText.Split('.');
                        objPermissionsConfigured.Add(nd1.ChildNodes[1].InnerText);

                        sw.WriteLine("<div>");
                        sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth400\">");
                        sw.WriteLine(objectFieldSplit[0]);
                        sw.WriteLine("</span>");
                        sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth400\">");
                        sw.WriteLine(objectFieldSplit[1]);
                        sw.WriteLine("</span>");

                        sw.WriteLine(checkTrueFalseValueForCSS(nd1.ChildNodes[2].InnerText));
                        sw.WriteLine(nd1.ChildNodes[2].InnerText);
                        sw.WriteLine("</span>");

                        sw.WriteLine(checkTrueFalseValueForCSS(nd1.ChildNodes[0].InnerText));
                        sw.WriteLine(nd1.ChildNodes[0].InnerText);
                        sw.WriteLine("</span>");
                    }
                    else if (nd1.ChildNodes.Count == 4)
                    {
                        String[] objectFieldSplit = nd1.ChildNodes[2].InnerText.Split('.');
                        objPermissionsConfigured.Add(nd1.ChildNodes[2].InnerText);

                        sw.WriteLine("<div>");
                        sw.WriteLine("<span class=\"objectValues fieldBackgroundGray columnWidth400\">");
                        sw.WriteLine(objectFieldSplit[0]);
                        sw.WriteLine("</span>");
                        sw.WriteLine("<span class=\"objectValues fieldBackgroundGray columnWidth400\">");
                        sw.WriteLine(objectFieldSplit[1]);
                        sw.WriteLine("</span>");

                        sw.WriteLine(checkTrueFalseValueForCSS(nd1.ChildNodes[3].InnerText));
                        sw.WriteLine(nd1.ChildNodes[3].InnerText);
                        sw.WriteLine("</span>");

                        sw.WriteLine(checkTrueFalseValueForCSS(nd1.ChildNodes[0].InnerText));
                        sw.WriteLine(nd1.ChildNodes[0].InnerText);
                        sw.WriteLine("</span>");

                        String cssForPersonAccount = checkTrueFalseValueForCSS(nd1.ChildNodes[1].InnerText);
                        cssForPersonAccount = cssForPersonAccount.Replace("columnWidth120", "columnWidth160");
                        sw.WriteLine(cssForPersonAccount);
                        sw.WriteLine(nd1.ChildNodes[1].InnerText);
                        sw.WriteLine("</span>");
                    }

                    sw.WriteLine("</div>");
                }

                sw.WriteLine("</div>");
            }

            // Now add the unconfigured record types to the report
            sw.WriteLine("<br />");
            sw.WriteLine("<br />");

            sw.WriteLine("<button class=\"accordionGreen\">Record Type Visibilities - Not Configured</button>");
            sw.WriteLine("<div class=\"panel\">");

            sw.WriteLine("<div>");
            sw.WriteLine("<span class=\"objectTitles fieldBackgroundDarkBlue columnWidth400\">");
            sw.WriteLine("Object Name");
            sw.WriteLine("</span>");
            sw.WriteLine("<span class=\"objectTitles fieldBackgroundGreen columnWidth400\">");
            sw.WriteLine("RecordType Name");
            sw.WriteLine("</span>");
            sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth120 fontItalic textAlignCenter\">");
            sw.WriteLine("Visible");
            sw.WriteLine("</span>");
            sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth120 fontItalic textAlignCenter\">");
            sw.WriteLine("Default");
            sw.WriteLine("</span>");
            sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth160 fontItalic textAlignCenter\">");
            sw.WriteLine("PersonAccountDefault");
            sw.WriteLine("</span>");
            sw.WriteLine("</div>");

            foreach (String nd1 in metadataObjInfo["objects"].Keys)
            {
                foreach (String nd2 in metadataObjInfo["objects"][nd1].objRecordTypeInfo.Keys)
                {
                    String objRecordtype = nd1 + "." + nd2;

                    if (!objPermissionsConfigured.Contains(objRecordtype))
                    {
                        sw.WriteLine("<div>");
                        sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth400\">");
                        sw.WriteLine(nd1);
                        sw.WriteLine("</span>");
                        sw.WriteLine("<span class=\"objectValues fieldBackgroundGunmetalBlue columnWidth400\">");
                        sw.WriteLine(nd2);
                        sw.WriteLine("</span>");

                        sw.WriteLine(checkTrueFalseValueForCSS("false"));
                        sw.WriteLine("false");
                        sw.WriteLine("</span>");
                        sw.WriteLine("</div>");
                    }
                }
            }

            sw.WriteLine("</div>");  // Closing DIV tag for the accordian panel
        }

        private void writeProfileApplicationPerms(XmlDocument xd, StreamWriter sw, Dictionary<String, Dictionary<String, MetadataObjectInfo>> metadataObjInfo)
        {
            XmlNodeList applicationVisibilities = xd.GetElementsByTagName("applicationVisibilities");
            if (applicationVisibilities.Count > 0)
            {
                sw.WriteLine("<br />");
                sw.WriteLine("<br />");

                sw.WriteLine("<button class=\"accordionGreen\">Application Visibilities</button>");
                sw.WriteLine("<div class=\"panel\">");

                sw.WriteLine("<div>");
                sw.WriteLine("<span class=\"objectTitles fieldBackgroundDarkBlue columnWidth400\">");
                sw.WriteLine("Application");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectValues fieldBackgroundGray columnWidth120 fontItalic textAlignCenter\">");
                sw.WriteLine("Visible");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectValues fieldBackgroundGray columnWidth120 fontItalic textAlignCenter\">");
                sw.WriteLine("default");
                sw.WriteLine("</span>");
                sw.WriteLine("</div>");

                foreach (XmlNode nd1 in applicationVisibilities)
                {
                    sw.WriteLine("<div>");
                    sw.WriteLine("<span class=\"objectValues fieldBackgroundGray columnWidth400 fontItalic textAlignLeft\">");
                    sw.WriteLine(nd1.ChildNodes[0].InnerText);
                    sw.WriteLine("</span>");

                    if (nd1.ChildNodes.Count == 2)
                    {
                        sw.WriteLine(checkTrueFalseValueForCSS(nd1.ChildNodes[1].InnerText));
                        sw.WriteLine(nd1.ChildNodes[1].InnerText);
                        sw.WriteLine("</span>");
                    }
                    else if (nd1.ChildNodes.Count == 3)
                    {
                        sw.WriteLine(checkTrueFalseValueForCSS(nd1.ChildNodes[2].InnerText));
                        sw.WriteLine(nd1.ChildNodes[2].InnerText);
                        sw.WriteLine("</span>");
                        sw.WriteLine(checkTrueFalseValueForCSS(nd1.ChildNodes[1].InnerText));
                        sw.WriteLine(nd1.ChildNodes[1].InnerText);
                        sw.WriteLine("</span>");
                    }

                    sw.WriteLine("</div>");
                }

                sw.WriteLine("</div>");
            }
        }

        private void writeProfileTabPerms(XmlDocument xd, StreamWriter sw, Dictionary<String, Dictionary<String, MetadataObjectInfo>> metadataObjInfo)
        {
            XmlNodeList tabVisibilities = xd.GetElementsByTagName("tabVisibilities");
            if (tabVisibilities.Count > 0)
            {
                sw.WriteLine("<br />");
                sw.WriteLine("<br />");

                sw.WriteLine("<button class=\"accordionGreen\">Tab Visibilities - Configured</button>");
                sw.WriteLine("<div class=\"panel\">");

                sw.WriteLine("<div>");
                sw.WriteLine("<span class=\"objectTitles fieldBackgroundDarkBlue columnWidth400\">");
                sw.WriteLine("Tab Name");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectValues fieldBackgroundGray columnWidth120 fontItalic textAlignCenter\">");
                sw.WriteLine("Visibility");
                sw.WriteLine("</span>");
                sw.WriteLine("</div>");

                foreach (XmlNode nd1 in tabVisibilities)
                {
                    sw.WriteLine("<div>");
                    sw.WriteLine("<span class=\"objectValues fieldBackgroundGray columnWidth400 fontItalic textAlignLeft\">");
                    sw.WriteLine(nd1.ChildNodes[0].InnerText);
                    sw.WriteLine("</span>");
                    sw.WriteLine(checkTrueFalseValueForCSS(nd1.ChildNodes[1].InnerText));
                    sw.WriteLine(nd1.ChildNodes[1].InnerText);
                    sw.WriteLine("</span>");
                    sw.WriteLine("</div>");
                }

                sw.WriteLine("</div>");
            }
        }

        private void writeProfileLayoutPerms(XmlDocument xd, StreamWriter sw, Dictionary<String, Dictionary<String, MetadataObjectInfo>> metadataObjInfo) 
        {
            XmlNodeList layoutAssignments = xd.GetElementsByTagName("layoutAssignments");
            if (layoutAssignments.Count > 0)
            {
                sw.WriteLine("<br />");
                sw.WriteLine("<br />");

                sw.WriteLine("<button class=\"accordionGreen\">Layout Assignments</button>");
                sw.WriteLine("<div class=\"panel\">");

                sw.WriteLine("<div>");
                sw.WriteLine("<span class=\"objectTitles fieldBackgroundDarkBlue columnWidth400\">");
                sw.WriteLine("Layout Name");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectTitles fieldBackgroundDarkBlue columnWidth400\">");
                sw.WriteLine("Object Name");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectTitles fieldBackgroundDarkBlue columnWidth400\">");
                sw.WriteLine("Record Type Name");
                sw.WriteLine("</span>");
                sw.WriteLine("</div>");

                foreach (XmlNode nd1 in layoutAssignments)
                {
                    sw.WriteLine("<div>");

                    if (nd1.ChildNodes.Count == 1)
                    {
                        sw.WriteLine("<span class=\"objectValues fieldBackgroundGray columnWidth400 fontItalic textAlignLeft\">");
                        sw.WriteLine(nd1.ChildNodes[0].InnerText);
                        sw.WriteLine("</span>");
                    }
                    else if (nd1.ChildNodes.Count == 2)
                    {
                        String[] objectFieldSplit = nd1.ChildNodes[1].InnerText.Split('.');
                        sw.WriteLine("<span class=\"objectValues fieldBackgroundGray columnWidth400 fontItalic textAlignLeft\">");
                        sw.WriteLine(nd1.ChildNodes[0].InnerText);
                        sw.WriteLine("</span>");
                        sw.WriteLine("<span class=\"objectValues fieldBackgroundGray columnWidth400 fontItalic textAlignLeft\">");
                        sw.WriteLine(objectFieldSplit[0]);
                        sw.WriteLine("</span>");
                        sw.WriteLine("<span class=\"objectValues fieldBackgroundGray columnWidth400 fontItalic textAlignLeft\">");
                        sw.WriteLine(objectFieldSplit[1]);
                        sw.WriteLine("</span>");
                    }

                    sw.WriteLine("</div>");
                }

                sw.WriteLine("</div>");
            }
        }

        private void writeProfileFlowPerms(XmlDocument xd, StreamWriter sw, Dictionary<String, Dictionary<String, MetadataObjectInfo>> metadataObjInfo) 
        {
            XmlNodeList flowAccesses = xd.GetElementsByTagName("flowAccesses");
            if (flowAccesses.Count > 0)
            {
                sw.WriteLine("<br />");
                sw.WriteLine("<br />");

                sw.WriteLine("<button class=\"accordionGreen\">Flow Access</button>");
                sw.WriteLine("<div class=\"panel\">");

                sw.WriteLine("<div>");
                sw.WriteLine("<span class=\"objectTitles fieldBackgroundDarkBlue columnWidth400\">");
                sw.WriteLine("Flow Name");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectValues fieldBackgroundGray columnWidth120 textAlignCenter\">");
                sw.WriteLine("Enabled");
                sw.WriteLine("</span>");
                sw.WriteLine("</div>");

                foreach (XmlNode nd1 in flowAccesses)
                {
                    sw.WriteLine("<div>");

                    sw.WriteLine("<span class=\"objectValues fieldBackgroundGray columnWidth400 fontItalic textAlignLeft\">");
                    sw.WriteLine(nd1.ChildNodes[1].InnerText);
                    sw.WriteLine("</span>");

                    sw.WriteLine(checkTrueFalseValueForCSS(nd1.ChildNodes[0].InnerText));
                    sw.WriteLine(nd1.ChildNodes[0].InnerText);
                    sw.WriteLine("</span>");

                    sw.WriteLine("</div>");
                }

                sw.WriteLine("</div>");
            }
        }

        private void writeProfileClassPerms(XmlDocument xd, StreamWriter sw, Dictionary<String, Dictionary<String, MetadataObjectInfo>> metadataObjInfo) 
        {
            XmlNodeList classAccesses = xd.GetElementsByTagName("classAccesses");
            if (classAccesses.Count > 0)
            {
                sw.WriteLine("<br />");
                sw.WriteLine("<br />");

                sw.WriteLine("<button class=\"accordionGreen\">Apex Class Access</button>");
                sw.WriteLine("<div class=\"panel\">");

                sw.WriteLine("<div>");
                sw.WriteLine("<span class=\"objectTitles fieldBackgroundDarkBlue columnWidth400\">");
                sw.WriteLine("Apex Class Name");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectValues fieldBackgroundGray columnWidth120 textAlignCenter\">");
                sw.WriteLine("Enabled");
                sw.WriteLine("</span>");
                sw.WriteLine("</div>");

                foreach (XmlNode nd1 in classAccesses)
                {
                    sw.WriteLine("<div>");

                    sw.WriteLine("<span class=\"objectValues fieldBackgroundGray columnWidth400 fontItalic textAlignLeft\">");
                    sw.WriteLine(nd1.ChildNodes[0].InnerText);
                    sw.WriteLine("</span>");

                    sw.WriteLine(checkTrueFalseValueForCSS(nd1.ChildNodes[1].InnerText));
                    sw.WriteLine(nd1.ChildNodes[1].InnerText);
                    sw.WriteLine("</span>");

                    sw.WriteLine("</div>");
                }

                sw.WriteLine("</div>");
            }
        }

        private void writeProfileVFPagePerms(XmlDocument xd, StreamWriter sw, Dictionary<String, Dictionary<String, MetadataObjectInfo>> metadataObjInfo) 
        {
            XmlNodeList pageAccesses = xd.GetElementsByTagName("pageAccesses");
            if (pageAccesses.Count > 0)
            {
                sw.WriteLine("<br />");
                sw.WriteLine("<br />");

                sw.WriteLine("<button class=\"accordionGreen\">Apex Page Access</button>");
                sw.WriteLine("<div class=\"panel\">");

                sw.WriteLine("<div>");
                sw.WriteLine("<span class=\"objectTitles fieldBackgroundDarkBlue columnWidth400\">");
                sw.WriteLine("Apex Page Name");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectValues fieldBackgroundGray columnWidth120 textAlignCenter\">");
                sw.WriteLine("Enabled");
                sw.WriteLine("</span>");
                sw.WriteLine("</div>");

                foreach (XmlNode nd1 in pageAccesses)
                {
                    sw.WriteLine("<div>");

                    sw.WriteLine("<span class=\"objectValues fieldBackgroundGray columnWidth400 fontItalic textAlignLeft\">");
                    sw.WriteLine(nd1.ChildNodes[0].InnerText);
                    sw.WriteLine("</span>");

                    sw.WriteLine(checkTrueFalseValueForCSS(nd1.ChildNodes[1].InnerText));
                    sw.WriteLine(nd1.ChildNodes[1].InnerText);
                    sw.WriteLine("</span>");

                    sw.WriteLine("</div>");
                }

                sw.WriteLine("</div>");
            }
        }

        private void writeProfileCustomSettingsPerms(XmlDocument xd, StreamWriter sw, Dictionary<String, Dictionary<String, MetadataObjectInfo>> metadataObjInfo) 
        {
            XmlNodeList customSettingAccesses = xd.GetElementsByTagName("customSettingAccesses");
            if (customSettingAccesses.Count > 0)
            {
                sw.WriteLine("<br />");
                sw.WriteLine("<br />");

                sw.WriteLine("<button class=\"accordionGreen\">Custom Setting Access</button>");
                sw.WriteLine("<div class=\"panel\">");

                sw.WriteLine("<div>");
                sw.WriteLine("<span class=\"objectTitles fieldBackgroundDarkBlue columnWidth400\">");
                sw.WriteLine("Custom Setting Name");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectValues fieldBackgroundGray columnWidth120 textAlignCenter\">");
                sw.WriteLine("Enabled");
                sw.WriteLine("</span>");
                sw.WriteLine("</div>");

                foreach (XmlNode nd1 in customSettingAccesses)
                {
                    sw.WriteLine("<div>");

                    sw.WriteLine("<span class=\"objectValues fieldBackgroundGray columnWidth400 fontItalic textAlignLeft\">");
                    sw.WriteLine(nd1.ChildNodes[1].InnerText);
                    sw.WriteLine("</span>");

                    sw.WriteLine(checkTrueFalseValueForCSS(nd1.ChildNodes[0].InnerText));
                    sw.WriteLine(nd1.ChildNodes[0].InnerText);
                    sw.WriteLine("</span>");

                    sw.WriteLine("</div>");
                }

                sw.WriteLine("</div>");
            }
        }

        private void writeProfileCustomMetadataTypePerms(XmlDocument xd, StreamWriter sw, Dictionary<String, Dictionary<String, MetadataObjectInfo>> metadataObjInfo) 
        {
            XmlNodeList customMetadataTypeAccesses = xd.GetElementsByTagName("customMetadataTypeAccesses");
            if (customMetadataTypeAccesses.Count > 0)
            {
                sw.WriteLine("<br />");
                sw.WriteLine("<br />");

                sw.WriteLine("<button class=\"accordionGreen\">Custom Metadata Type Name</button>");
                sw.WriteLine("<div class=\"panel\">");

                sw.WriteLine("<div>");
                sw.WriteLine("<span class=\"objectTitles fieldBackgroundDarkBlue columnWidth400\">");
                sw.WriteLine("Custom Metadata Type");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectValues fieldBackgroundGray columnWidth120 textAlignCenter\">");
                sw.WriteLine("Enabled");
                sw.WriteLine("</span>");
                sw.WriteLine("</div>");

                foreach (XmlNode nd1 in customMetadataTypeAccesses)
                {
                    sw.WriteLine("<div>");

                    sw.WriteLine("<span class=\"objectValues fieldBackgroundGray columnWidth400 fontItalic textAlignLeft\">");
                    sw.WriteLine(nd1.ChildNodes[1].InnerText);
                    sw.WriteLine("</span>");

                    sw.WriteLine(checkTrueFalseValueForCSS(nd1.ChildNodes[0].InnerText));
                    sw.WriteLine(nd1.ChildNodes[0].InnerText);
                    sw.WriteLine("</span>");

                    sw.WriteLine("</div>");
                }

                sw.WriteLine("</div>");
            }
        }

        private void writeProfileCustomPermissionsPerms(XmlDocument xd, StreamWriter sw, Dictionary<String, Dictionary<String, MetadataObjectInfo>> metadataObjInfo) 
        {
            XmlNodeList customPermissions = xd.GetElementsByTagName("customPermissions");
            if (customPermissions.Count > 0)
            {
                sw.WriteLine("<br />");
                sw.WriteLine("<br />");

                sw.WriteLine("<button class=\"accordionGreen\">Custom Permissions</button>");
                sw.WriteLine("<div class=\"panel\">");

                sw.WriteLine("<div>");
                sw.WriteLine("<span class=\"objectTitles fieldBackgroundDarkBlue columnWidth400\">");
                sw.WriteLine("Custom Permissions Name");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectValues fieldBackgroundGray columnWidth120 textAlignCenter\">");
                sw.WriteLine("Enabled");
                sw.WriteLine("</span>");
                sw.WriteLine("</div>");

                foreach (XmlNode nd1 in customPermissions)
                {
                    sw.WriteLine("<div>");

                    sw.WriteLine("<span class=\"objectValues fieldBackgroundGray columnWidth400 fontItalic textAlignLeft\">");
                    sw.WriteLine(nd1.ChildNodes[1].InnerText);
                    sw.WriteLine("</span>");

                    sw.WriteLine(checkTrueFalseValueForCSS(nd1.ChildNodes[0].InnerText));
                    sw.WriteLine(nd1.ChildNodes[0].InnerText);
                    sw.WriteLine("</span>");

                    sw.WriteLine("</div>");
                }

                sw.WriteLine("</div>");
            }
        }

        private void writeProfileCategoryGrpPerms(XmlDocument xd, StreamWriter sw) 
        {
            XmlNodeList categoryGroupVisibilities = xd.GetElementsByTagName("categoryGroupVisibilities");
            if (categoryGroupVisibilities.Count > 0)
            {
                sw.WriteLine("<br />");
                sw.WriteLine("<br />");

                sw.WriteLine("<button class=\"accordionGreen\">Data Category Group Visibilities</button>");
                sw.WriteLine("<div class=\"panel\">");

                sw.WriteLine("<div>");
                sw.WriteLine("<span class=\"objectTitles fieldBackgroundDarkBlue columnWidth400\">");
                sw.WriteLine("Data Category Group");
                sw.WriteLine("</span>");
                sw.WriteLine("<span class=\"objectValues fieldBackgroundGray columnWidth120 textAlignCenter\">");
                sw.WriteLine("Visibility");
                sw.WriteLine("</span>");
                sw.WriteLine("</div>");

                foreach (XmlNode nd1 in categoryGroupVisibilities)
                {
                    sw.WriteLine("<div>");

                    sw.WriteLine("<span class=\"objectValues fieldBackgroundGray columnWidth400 fontItalic textAlignLeft\">");
                    sw.WriteLine(nd1.ChildNodes[0].InnerText);
                    sw.WriteLine("</span>");

                    sw.WriteLine("<span class=\"objectValues fieldBackgroundGray columnWidth120 fontItalic textAlignCenter\">");
                    sw.WriteLine(nd1.ChildNodes[1].InnerText);
                    sw.WriteLine("</span>");

                    sw.WriteLine("</div>");
                }

                sw.WriteLine("</div>");
            }
        }

        private String checkTrueFalseValueForCSS(String nodeValue)
        {
            String returnValue = "";

            if (nodeValue.ToLower() == "true")
            {
                returnValue = "<span class=\"objectValues fieldBackgroundLightBlue columnWidth120 fontItalic textAlignCenter\">";
            }
            else if (nodeValue.ToLower() == "false")
            {
                returnValue = "<span class=\"objectValues fieldBackgroundOrange columnWidth120 fontItalic textAlignCenter\">";
            }
            else if (nodeValue.ToLower() == "enable")
            {
                returnValue = "<span class=\"objectValues fieldBackgroundLightBlue columnWidth120 fontItalic textAlignCenter\">";
            }
            else if (nodeValue.ToLower() == "defaulton")
            {
                returnValue = "<span class=\"objectValues fieldBackgroundLightBlue columnWidth120 fontItalic textAlignCenter\">";
            }
            else if (nodeValue.ToLower() == "defaultoff")
            {
                returnValue = "<span class=\"objectValues fieldBackgroundOrange columnWidth120 fontItalic textAlignCenter\">";
            }
            else if (nodeValue.ToLower() == "hidden")
            {
                returnValue = "<span class=\"objectValues fieldBackgroundOrange columnWidth120 fontItalic textAlignCenter\">";
            }
            else if (nodeValue.ToLower() == "private")
            {
            }
            else if (nodeValue.ToLower() == "public")
            {
            }
            else if (nodeValue.ToLower() == "hidden")
            {
                
            }

            return returnValue;
        }

        private List<String> getObjectTopFieldNames(String directoryName)
        {
            List<String> xmlTopNodeNames = new List<String>();

            if (directoryName == "accountRelationshipShareRules")
            {
                List<MetadataDifferenceProcessing.MetadataFieldTypes> mftList = MetadataDifferenceProcessing.accountRelationshipShareRuleFieldNames();

                foreach(MetadataDifferenceProcessing.MetadataFieldTypes mft in mftList)
                {
                    xmlTopNodeNames.Add(mft.fieldName);
                }

                //xmlTopNodeNames = MetadataDifferenceProcessing.accountRelationshipShareRuleFieldNames();
            }
            else if (directoryName == "actionLinkGroupTemplates")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.actionLinkGroupTemplateFieldNames();
            }
            else if (directoryName == "actionPlanTemplates")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.actionPlanTemplateFieldNames();
            }
            else if (directoryName == "analyticSnapshots")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.analyticSnapshotFieldNames();
            }
            else if (directoryName == "animationRules")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.animationRuleFieldNames();
            }
            else if (directoryName == "classes")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.apexClassFieldNames();
            }
            else if (directoryName == "applications")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.customApplicationFieldNames();
            }
            else if (directoryName == "appMenus")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.appMenuFieldNames();
            }
            else if (directoryName == "appointmentSchedulingPolicies")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.appointmentSchedulingPolicyFieldNames();
            }
            else if (directoryName == "approvalProcesses")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.approvalProcessFieldNames();
            }
            else if (directoryName == "assignmentRules")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.assignmentRulesFieldNames();
            }
            else if (directoryName == "audience")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.audienceFieldNames();
            }
            else if (directoryName == "aura")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.auraDefinitionBundleFieldNames();
            }
            else if (directoryName == "authproviders")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.authProviderFieldNames();
            }
            else if (directoryName == "autoResponseRules")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.autoResponseRulesFieldNames();
            }
            else if (directoryName == "bot")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.botFieldNames();
            }
            else if (directoryName == "brandingSets")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.brandingSetFieldNames();
            }
            else if (directoryName == "cachePartitions")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.platformCachePartitionFieldNames();
            }
            else if (directoryName == "callCenters")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.callCenterFieldNames();
            }
            else if (directoryName == "campaignInfluenceModels")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.campaignInfluenceModelFieldNames();
            }
            else if (directoryName == "CaseSubjectParticles")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.caseSubjectParticleFieldNames();
            }
            else if (directoryName == "certs")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.certificateFieldNames();
            }
            else if (directoryName == "cleanDataServices")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.cleanDataServiceFieldNames();
            }
            else if (directoryName == "cmsConnectSource")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.CMSConnectSourceFieldNames();
            }
            else if (directoryName == "communities")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.communityFieldNames();
            }
            else if (directoryName == "communityTemplateDefinitions")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.communityTemplateDefinitionFieldNames();
            }
            else if (directoryName == "communityThemeDefinitions")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.communityThemeDefinitionFieldNames();
            }
            else if (directoryName == "components")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.apexComponentFieldNames();
            }
            else if (directoryName == "connectedApps")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.connectedAppFieldNames();
            }
            else if (directoryName == "contentassets")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.contentAssetFieldNames();
            }
            else if (directoryName == "corsWhitelistOrigins")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.corsWhitelistOriginFieldNames();
            }
            else if (directoryName == "cspTrustedSites")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.cspTrustedSiteFieldNames();
            }
            else if (directoryName == "customApplicationComponents")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.customApplicationFieldNames();
            }
            else if (directoryName == "customHelpMenuSections")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.customHelpMenuSectionFieldNames();
            }
            else if (directoryName == "customMetadata")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.customMetadataFieldNames();
            }
            else if (directoryName == "customPermissions")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.customPermissionFieldNames();
            }
            else if (directoryName == "dashboards")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.dashboardFieldNames();
            }
            else if (directoryName == "datacategorygroups")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.dataCategoryGroupFieldNames();
            }
            else if (directoryName == "dataSources")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.externalDataSourceFieldNames();
            }
            else if (directoryName == "delegateGroups")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.delegateGroupFieldNames();
            }
            else if (directoryName == "document")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.documentFieldNames();
            }
            else if (directoryName == "duplicateRules")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.duplicateRuleFieldNames();
            }
            else if (directoryName == "eclair")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.eclairGeoDataFieldNames();
            }
            else if (directoryName == "email")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.emailTemplateFieldNames();
            }
            else if (directoryName == "emailservices")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.emailServicesFunctionFieldNames();
            }
            else if (directoryName == "EmbeddedServiceBranding")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.embeddedServiceBrandingFieldNames();
            }
            else if (directoryName == "EmbeddedServiceConfig")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.embeddedServiceConfigFieldNames();
            }
            else if (directoryName == "EmbeddedServiceFieldService")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.embeddedServiceFieldServiceFieldNames();
            }
            else if (directoryName == "EmbeddedServiceFlowConfig")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.embeddedServiceFlowConfigFieldNames();
            }
            else if (directoryName == "EmbeddedServiceLiveAgent")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.embeddedServiceLiveAgentFieldNames();
            }
            else if (directoryName == "EmbeddedServiceMenuSettings")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.embeddedServiceMenuSettingsFieldNames();
            }
            else if (directoryName == "entitlementProcesses")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.entitlementProcessFieldNames();
            }
            else if (directoryName == "entitlementTemplates")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.entitlementTemplateFieldNames();
            }
            else if (directoryName == "escalationRules")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.escalationRulesFieldNames();
            }
            else if (directoryName == "eventDeliveries")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.eventDeliveryFieldNames();
            }
            else if (directoryName == "eventSubscriptions")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.eventSubscriptionFieldNames();
            }
            else if (directoryName == "experiences")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.experienceBundleFieldNames();
            }
            else if (directoryName == "externalServiceRegistrations")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.externalServiceRegistrationFieldNames();
            }
            else if (directoryName == "featureParameters")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.featureParameterFieldNames();
            }
            else if (directoryName == "feedFilters")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.customFeedFilterFieldNames();
            }
            else if (directoryName == "flexipages")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.flexiPageFieldNames();
            }
            else if (directoryName == "flows")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.flowFieldNames();
            }
            else if (directoryName == "flowCategories")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.flowCategoryFieldNames();
            }
            else if (directoryName == "flowDefinitions")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.flowDefinitionFieldNames();
            }
            else if (directoryName == "globalPicklist")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.globalPicklistFieldNames();
            }
            else if (directoryName == "globalValueSets")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.globalValueSetFieldNames();
            }
            else if (directoryName == "globalValueSetTranslations")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.globalValueSetTranslationFieldNames();
            }
            else if (directoryName == "groups")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.groupFieldNames();
            }
            else if (directoryName == "homepagecomponents")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.homePageComponentFieldNames();
            }
            else if (directoryName == "homePageLayouts")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.homePageLayoutFieldNames();
            }
            else if (directoryName == "installedPackages")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.installedPackageFieldNames();
            }
            else if (directoryName == "layouts")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.layoutFieldNames();
            }
            else if (directoryName == "letterhead")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.letterheadFieldNames();
            }
            else if (directoryName == "lightningBolts")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.lightningBoltFieldNames();
            }
            else if (directoryName == "lightningExperienceThemes")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.lightningExperienceThemeFieldNames();
            }
            else if (directoryName == "liveChatAgentConfigs")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.liveChatAgentConfigFieldNames();
            }
            else if (directoryName == "liveChatButtons")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.liveChatButtonFieldNames();
            }
            else if (directoryName == "liveChatDeployments")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.liveChatDeploymentFieldNames();
            }
            else if (directoryName == "liveChatSensitiveDataRule")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.liveChatSensitiveDataRuleFieldNames();
            }
            else if (directoryName == "managedContentTypes")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.managedContentTypeFieldNames();
            }
            else if (directoryName == "managedTopics")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.managedTopicsFieldNames();
            }
            else if (directoryName == "matchingRules")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.matchingRuleFieldNames();
            }
            else if (directoryName == "messageChannels")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.lightningMessageChannelFieldNames();
            }
            else if (directoryName == "milestoneTypes")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.milestoneTypeFieldNames();
            }
            else if (directoryName == "mlDomains")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.mlDomainFieldNames();
            }
            else if (directoryName == "MobileApplicationDetails")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.mobileApplicationDetailFieldNames();
            }
            else if (directoryName == "moderation")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.moderationRuleFieldNames();
            }
            else if (directoryName == "myDomainDiscoverableLogins")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.myDomainDiscoverableLoginFieldNames();
            }
            else if (directoryName == "namedCredentials")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.namedCredentialFieldNames();
            }
            else if (directoryName == "navigationMenus")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.navigationMenuFieldNames();
            }
            else if (directoryName == "networkBranding")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.networkBrandingFieldNames();
            }
            else if (directoryName == "networks")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.networkFieldNames();
            }
            else if (directoryName == "notificationTypeConfig")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.notificationTypeConfigFieldNames();
            }
            else if (directoryName == "notificationtypes")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.customNotificationTypeFieldNames();
            }
            else if (directoryName == "oauthcustomscopes")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.oauthCustomScopeFieldNames();
            }
            else if (directoryName == "objects")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.customObjectFieldNames();
            }
            else if (directoryName == "objectTranslations")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.customObjectTranslationFieldNames();
            }
            else if (directoryName == "pages")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.apexPageFieldNames();
            }
            else if (directoryName == "pathAssistants")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.pathAssistantFieldNames();
            }
            else if (directoryName == "paymentGatewayProviders")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.paymentGatewayProviderFieldNames();
            }
            else if (directoryName == "permissionsetgroups")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.permissionSetGroupFieldNames();
            }
            else if (directoryName == "permissionsets")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.profilePermissionSetFieldNames();
            }
            else if (directoryName == "platformEventChannelMembers")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.platformEventChannelMemberFieldNames();
            }
            else if (directoryName == "platformEventChannels")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.platformEventChannelFieldNames();
            }
            else if (directoryName == "portals")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.portalFieldNames();
            }
            else if (directoryName == "postTemplates")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.postTemplateFieldNames();
            }
            else if (directoryName == "presenceDeclineReasons")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.presenceDeclineReasonFieldNames();
            }
            else if (directoryName == "presenceUserConfigs")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.presenceUserConfigFieldNames();
            }
            else if (directoryName == "profilePasswordPolicies")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.profilePasswordPolicyFieldNames();
            }
            else if (directoryName == "profiles")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.profilePermissionSetFieldNames();
            }
            else if (directoryName == "profileSessionSettings")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.profileSessionSettingFieldNames();
            }
            else if (directoryName == "prompts")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.promptFieldNames();
            }
            else if (directoryName == "queueRoutingConfigs")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.queueRoutingConfigFieldNames();
            }
            else if (directoryName == "queues")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.queueFieldNames();
            }
            else if (directoryName == "quickActions")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.quickActionFieldNames();
            }
            else if (directoryName == "recommendationStrategies")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.recommendationStrategyFieldNames();
            }
            else if (directoryName == "recordActionDeployments")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.recordActionDeploymentFieldNames();
            }
            else if (directoryName == "redirectWhitelistUrls")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.redirectWhitelistUrlFieldNames();
            }
            else if (directoryName == "remoteSiteSettings")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.remoteSiteSettingFieldNames();
            }
            else if (directoryName == "reports")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.reportFieldNames();
            }
            else if (directoryName == "reportTypes")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.reportTypeFieldNames();
            }
            else if (directoryName == "roles")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.roleFieldNames();
            }
            else if (directoryName == "samlssoconfigs")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.samlSsoConfigFieldNames();
            }
            else if (directoryName == "scontrols")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.scontrolFieldNames();
            }
            else if (directoryName == "serviceChannels")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.serviceChannelFieldNames();
            }
            else if (directoryName == "servicePresenceStatuses")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.servicePresenceStatusFieldNames();
            }
            //else if (directoryName == "settings")
            //{
            //    xmlTopNodeNames = MetadataDifferenceProcessing.settingsFieldNames();
            //}
            else if (directoryName == "sharingRules")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.sharingRulesFieldNames();
            }
            else if (directoryName == "sharingSets")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.sharingSetFieldNames();
            }
            else if (directoryName == "siteDotComSites")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.siteDotComFieldNames();
            }
            else if (directoryName == "sites")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.customSiteFieldNames();
            }
            else if (directoryName == "skills")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.skillFieldNames();
            }
            else if (directoryName == "standardValueSets")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.standardValueSetFieldNames();
            }
            else if (directoryName == "standardValueSetTranslations")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.standardValueSetTranslationFieldNames();
            }
            else if (directoryName == "staticresources")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.staticResourceFieldNames();
            }
            else if (directoryName == "synonymDictionaries")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.synonymDictionaryFieldNames();
            }
            else if (directoryName == "tabs")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.customTabFieldNames();
            }
            else if (directoryName == "territories")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.territoryFieldNames();
            }
            else if (directoryName == "territory2Models")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.territory2ModelFieldNames();
            }
            else if (directoryName == "territory2Types")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.territory2TypeFieldNames();
            }
            //else if (directoryName == "testSuites")
            //{

            //}
            else if (directoryName == "timeSheetTemplates")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.timeSheetTemplateFieldNames();
            }
            else if (directoryName == "topicsForObjects")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.topicsForObjectsFieldNames();
            }
            else if (directoryName == "transactionSecurityPolicies")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.transactionSecurityPolicyFieldNames();
            }
            else if (directoryName == "translations")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.translationsFieldNames();
            }
            else if (directoryName == "triggers")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.apexTriggerFieldNames();
            }
            else if (directoryName == "UserCriteria")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.userCriteriaFieldNames();
            }
            else if (directoryName == "wave")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.waveApplicationFieldNames();
            }
            else if (directoryName == "waveTemplates")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.waveTemplateBundleFieldNames();
            }
            else if (directoryName == "weblinks")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.customPageWebLinkFieldNames();
            }
            else if (directoryName == "workflows")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.workflowFieldNames();
            }
            else if (directoryName == "workSkillRoutings")
            {
                xmlTopNodeNames = MetadataDifferenceProcessing.workSkillRoutingFieldNames();
            }

            return xmlTopNodeNames;
        }

        private List<String> getObjectTopFieldNames(String directoryName, String fileName)
        {
            Dictionary <String, String> xmlTopNodeNames = new Dictionary<String, String>();

            XmlDocument xd = new XmlDocument();
            xd.Load(fileName);

            foreach (XmlNode nd in xd.ChildNodes)
            {
                foreach (XmlNode nd2 in nd.ChildNodes)
                {
                    if (!xmlTopNodeNames.ContainsKey(nd2.Name))
                    {
                        xmlTopNodeNames.Add(nd2.Name, nd2.Name);
                    }
                }
            }


            return xmlTopNodeNames.Keys.ToList<String>();
        }

        public Int32 getColumnEndValue(Int32 currColEndValue, Int32 c1, Int32 c2, Int32 c3,
                                       Dictionary<String, List<String>> columnNames4,
                                       Dictionary<String, List<String>> columnNames5,
                                       Dictionary<String, List<String>> columnNames6,
                                       Dictionary<String, List<String>> columnNames7,
                                       Dictionary<String, List<String>> columnNames8)
        {
            Int32 newColumnEndValue = currColEndValue;

            if (c1 > 0) newColumnEndValue = newColumnEndValue + c1;
            if (c2 > 0) newColumnEndValue = newColumnEndValue + c2;
            if (c3 > 0) newColumnEndValue = newColumnEndValue = c3 + 1;

            foreach (String key4 in columnNames4.Keys)
            {
                List<String> colNames = columnNames4[key4];
                newColumnEndValue = newColumnEndValue + colNames.Count;
            }

            foreach (String key5 in columnNames5.Keys)
            {
                List<String> colNames = columnNames5[key5];
                newColumnEndValue = newColumnEndValue + colNames.Count;
            }

            foreach (String key6 in columnNames6.Keys)
            {
                List<String> colNames = columnNames6[key6];
                newColumnEndValue = newColumnEndValue + colNames.Count;
            }

            foreach (String key7 in columnNames7.Keys)
            {
                List<String> colNames = columnNames7[key7];
                newColumnEndValue = newColumnEndValue + colNames.Count;
            }

            foreach (String key8 in columnNames8.Keys)
            {
                List<String> colNames = columnNames8[key8];
                newColumnEndValue = newColumnEndValue + colNames.Count;
            }

            return newColumnEndValue;
        }

        private void writeObjectTypeAndName(String objectType, String objectName, StreamWriter sw)
        {
            sw.WriteLine("<div>");
            sw.WriteLine("<span class=\"objectTitles fieldBackgroundDarkCyan columnWidth300\">");
            sw.WriteLine(objectType);
            sw.WriteLine("</span>");
            sw.WriteLine("<span class=\"objectTitles fieldBackgroundDarkCyan columnWidth630\">");
            sw.WriteLine(objectName);
            sw.WriteLine("</span>");
            sw.WriteLine("</div>");
        }


        public class MetadataObjectInfo
        {
            public String metadataObject = "";
            public String objectName = "";
            public String objectVisibility = "";
            public String objectSharingModel = "";
            public Boolean isCustomSetting = false;
            public Boolean isCustomMetadataType = false;

            // Key = object name + '.' + field name
            public Dictionary<String, ObjectFieldInfo> objFieldInfo;

            // Key = object name + '.' + record type name
            public Dictionary<String, ObjectRecordTypInfo> objRecordTypeInfo;

            public MetadataObjectInfo()
            {
                objFieldInfo = new Dictionary<string, ObjectFieldInfo>();
                objRecordTypeInfo = new Dictionary<string, ObjectRecordTypInfo>();
            }
        }

        public class ObjectFieldInfo
        {
            public String fieldName = "";
            public Boolean fieldIsRequired = false;
        }

        public class ObjectRecordTypInfo
        {
            public String recordType = "";
        }
    }
}
