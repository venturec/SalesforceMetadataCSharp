using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SalesforceMetadata
{
    public class ZipFileWithPackageXML
    {
        // Return zip file path
        public static String buildZipFileWithPackageXml(TreeNodeCollection tndNodes, 
                                                 String baseFolderPath, 
                                                 String deployFrom,
                                                 String projectFolder)
        {
            Dictionary<String, HashSet<String>> packageXml = new Dictionary<String, HashSet<String>>();
            HashSet<String> directoryFilesDeleted = new HashSet<String>();

            DateTime dtt = DateTime.Now;
            String directoryName = dtt.Year + "_" + dtt.Month + "_" + dtt.Day + "_" + dtt.Hour + "_" + dtt.Minute + "_" + dtt.Second + "_" + dtt.Millisecond;
            String folderPath = deployFrom + "\\" + directoryName;

            DirectoryInfo cdDi = Directory.CreateDirectory(folderPath);

            // We want to track the directory and files which will be deployed so we can build the package.xml properly
            List<String> filesDeployed = new List<string>();
            foreach (TreeNode tnd1 in tndNodes)
            {
                String metadataType = MetadataDifferenceProcessing.folderToType(tnd1.Text, "");

                if (tnd1.Nodes.Count > 0)
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (tnd2.Checked == true)
                        {
                            String[] tnd2NodeFullPath = tnd2.FullPath.Split('\\');
                            filesDeployed.Add(tnd1.Text + "\\" + tnd2.Text);

                            DirectoryInfo di;
                            if (!Directory.Exists(folderPath + "\\" + tnd1.Text))
                            {
                                di = Directory.CreateDirectory(folderPath + "\\" + tnd1.Text);
                            }
                            else
                            {
                                di = new DirectoryInfo(folderPath + "\\" + tnd1.Text);
                            }

                            // Copy the directory
                            if (metadataType == "AuraDefinitionBundle" || metadataType == "LightningComponentBundle")
                            {
                                UtilityClass.copyDirectory(projectFolder + "\\" + tnd1.Text + "\\" + tnd2NodeFullPath[1],
                                                           folderPath + "\\" + tnd1.Text + "\\" + tnd2NodeFullPath[1],
                                                           true);

                                if (packageXml.ContainsKey(metadataType))
                                {
                                    packageXml[metadataType].Add(tnd2NodeFullPath[1]);
                                }
                                else
                                {
                                    packageXml.Add(metadataType, new HashSet<String> { tnd2NodeFullPath[1] });
                                }
                            }
                            else if (metadataType == "CustomMetadata")
                            {
                                // Loop through the child nodes, get the CMT names and then add an .md before copying to the deployment folder
                                if (tnd2.Checked == true)
                                {
                                    foreach (TreeNode tnd3 in tnd2.Nodes)
                                    {
                                        if (tnd3.Checked == true)
                                        {
                                            String[] cmtRecordSplit = tnd3.Text.Split('.');

                                            File.Copy(projectFolder + "\\" + tnd1.Text + "\\" + tnd2.Text + "." + tnd3.Text,
                                                folderPath + "\\" + tnd1.Text + "\\" + tnd2.Text + "." + tnd3.Text);

                                            if (packageXml.ContainsKey(metadataType))
                                            {
                                                packageXml[metadataType].Add(tnd2.Text + '.' + cmtRecordSplit[0]);
                                            }
                                            else
                                            {
                                                packageXml.Add(metadataType, new HashSet<string> { tnd2.Text + '.' + cmtRecordSplit[0] });
                                            }
                                        }
                                    }
                                }
                            }
                            else if (metadataType == "CustomObject" || metadataType == "CustomObjectTranslation")
                            {
                                // Create the file and write the selected values to the file

                                StreamWriter objSw = new StreamWriter(di.FullName + "\\" + tnd2NodeFullPath[1]);

                                objSw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                                objSw.WriteLine("<CustomObject xmlns=\"http://soap.sforce.com/2006/04/metadata\">");

                                foreach (TreeNode tnd3 in tnd2.Nodes)
                                {
                                    if (tnd3.Checked == true)
                                    {
                                        objSw.WriteLine(tnd3.Text);

                                        String[] tnd3NodeFullPath = tnd3.FullPath.Split('\\');
                                        directoryName = tnd3NodeFullPath[0];

                                        String[] objectNameSplit = tnd3NodeFullPath[1].Split('.');

                                        //String parentNode = MetadataDifferenceProcessing.folderToType(tnd3NodeFullPath[0], "");

                                        // Add the custom field to the dictionary
                                        if (tnd3NodeFullPath.Length == 3)
                                        {
                                            if (tnd3NodeFullPath[0] == "objects"
                                                && tnd3NodeFullPath[2].StartsWith("<fields"))
                                            {
                                                String xmlString = "<document>" + tnd3NodeFullPath[2] + "</document>";
                                                XmlDocument xd = new XmlDocument();
                                                xd.LoadXml(xmlString);

                                                String objectFieldCombo = objectNameSplit[0] + "." + xd.ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;

                                                // Add the custom field to the packagexml dictionary
                                                if (packageXml.ContainsKey("CustomField"))
                                                {
                                                    packageXml["CustomField"].Add(objectFieldCombo);
                                                }
                                                else
                                                {
                                                    packageXml.Add("CustomField", new HashSet<string> { objectFieldCombo });
                                                }

                                                // Add the custom object to the packagexml dictionary
                                                if (packageXml.ContainsKey(metadataType))
                                                {
                                                    packageXml[metadataType].Add(objectNameSplit[0]);
                                                }
                                                else
                                                {
                                                    packageXml.Add(metadataType, new HashSet<string> { objectNameSplit[0] });
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (packageXml.ContainsKey(metadataType))
                                            {
                                                packageXml[metadataType].Add(objectNameSplit[0]);
                                            }
                                            else
                                            {
                                                packageXml.Add(metadataType, new HashSet<string> { objectNameSplit[0] });
                                            }
                                        }
                                    }
                                }

                                objSw.WriteLine("</CustomObject>");
                                objSw.Close();
                            }
                            else if (metadataType == "Profile")
                            {
                                //Debug.Write("tnd1.Text == \"profiles\"");
                            }
                            else if (metadataType == "PermissionSet")
                            {
                                //Debug.Write("tnd1.Text == \"permissionsets\"");
                            }
                            else if (metadataType == "Report")
                            {
                                //Debug.Write("tnd1.Text == \"reports\"");
                            }
                            else
                            {
                                File.Copy(projectFolder + "\\" + tnd1.Text + "\\" + tnd2NodeFullPath[1],
                                          folderPath + "\\" + tnd1.Text + "\\" + tnd2NodeFullPath[1]);


                                String[] objectNameSplit = tnd2NodeFullPath[1].Split('.');

                                if (metadataType == "ApprovalProcess")
                                {
                                    if (packageXml.ContainsKey(metadataType))
                                    {
                                        packageXml[metadataType].Add(objectNameSplit[0] + "." + objectNameSplit[1]);
                                    }
                                    else
                                    {
                                        packageXml.Add(metadataType, new HashSet<string> { objectNameSplit[0] + "." + objectNameSplit[1] });
                                    }
                                }
                                else if (metadataType == "ApexClass")
                                {
                                    if (packageXml.ContainsKey(metadataType)
                                        && !tnd2.Text.EndsWith("-meta.xml"))
                                    {
                                        packageXml[metadataType].Add(objectNameSplit[0]);
                                    }
                                    else if (!tnd2.Text.EndsWith("-meta.xml"))
                                    {
                                        packageXml.Add(metadataType, new HashSet<string> { objectNameSplit[0] });
                                    }
                                }
                                else if (metadataType == "ApexTrigger")
                                {
                                    if (packageXml.ContainsKey(metadataType)
                                        && !tnd2.Text.EndsWith("-meta.xml"))
                                    {
                                        packageXml[metadataType].Add(objectNameSplit[0]);
                                    }
                                    else if (!tnd2.Text.EndsWith("-meta.xml"))
                                    {
                                        packageXml.Add(metadataType, new HashSet<string> { objectNameSplit[0] });
                                    }
                                }
                                else if (metadataType == "QuickAction")
                                {
                                    if (objectNameSplit.Length == 2)
                                    {
                                        if (packageXml.ContainsKey(metadataType))
                                        {
                                            packageXml[metadataType].Add(objectNameSplit[0]);
                                        }
                                        else
                                        {
                                            packageXml.Add(metadataType, new HashSet<string> { objectNameSplit[0] });
                                        }
                                    }
                                    else if (objectNameSplit.Length == 3)
                                    {
                                        if (packageXml.ContainsKey(metadataType))
                                        {
                                            packageXml[metadataType].Add(objectNameSplit[0] + "." + objectNameSplit[1]);
                                        }
                                        else
                                        {
                                            packageXml.Add(metadataType, new HashSet<string> { objectNameSplit[0] + "." + objectNameSplit[1] });
                                        }
                                    }
                                }
                                else
                                {
                                    if (packageXml.ContainsKey(metadataType))
                                    {
                                        packageXml[metadataType].Add(objectNameSplit[0]);
                                    }
                                    else
                                    {
                                        packageXml.Add(metadataType, new HashSet<string> { objectNameSplit[0] });
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Write out the package.xml file and then build the zip file
            buildPackageXmlFile(packageXml, folderPath);

            // Zip up the contents of the folders and package.xml file
            String zipPathAndName = zipUpContents(folderPath, deployFrom);

            if (baseFolderPath != "")
            {
                String codeArchiveRootPath = baseFolderPath + "\\Code Archive";
                String logFile = baseFolderPath + "\\Code Archive\\LogFile.txt";

                if (!Directory.Exists(codeArchiveRootPath))
                {
                    Directory.CreateDirectory(codeArchiveRootPath);
                }

                StreamWriter sw = new StreamWriter(logFile, true);
                foreach (String objName in filesDeployed)
                {
                    sw.Write(objName + "\t" +
                             dtt.Year.ToString() + "\t" +
                             dtt.Month.ToString() + "\t" +
                             dtt.Day.ToString() + "\t" +
                             dtt.Hour.ToString() + "\t" +
                             dtt.Minute.ToString() + "\t" +
                             dtt.Second.ToString() + "\t" +
                             "Deployed" + Environment.NewLine);
                }
                sw.Close();
            }

            return zipPathAndName;
        }

        private static void buildPackageXmlFile(Dictionary<String, HashSet<String>> packageXml, String folderPath)
        {
            StreamWriter sw = new StreamWriter(folderPath + "\\package.xml", false);

            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sw.WriteLine("<Package xmlns=\"http://soap.sforce.com/2006/04/metadata\">");

            foreach (String typeName in packageXml.Keys)
            {
                sw.WriteLine("<types>");

                foreach (String memberName in packageXml[typeName])
                {
                    sw.WriteLine("<members>" + memberName + "</members>");
                }

                sw.WriteLine("<name>" + typeName + "</name>");
                sw.WriteLine("</types>");
            }

            sw.WriteLine("<version>" + Properties.Settings.Default.DefaultAPI + "</version>");

            //if (this.tbOutboundChangeSetName.Text != "")
            //{
            //    sw.WriteLine("<fullName>" + this.tbOutboundChangeSetName.Text + "</fullName>");
            //}

            sw.WriteLine("</Package>");

            sw.Close();
        }

        private static String zipUpContents(String folderPath, String deployFrom)
        {
            String[] folderPathSplit = folderPath.Split('\\');

            String zipFileName = folderPathSplit[folderPathSplit.Length - 1] + ".zip";
            String zipPathAndName = deployFrom + "\\" + zipFileName;

            ZipFile.CreateFromDirectory(folderPath, zipPathAndName, CompressionLevel.Fastest, false);

            return zipPathAndName;
        }

        public static void copySelectedToRepository(TreeNodeCollection tndNodes,
                                              String projectFolder,
                                              String repositoryPath)
        {
            // Check if the tbRepositoryPath is populated and valid first

            if (repositoryPath == "") { return; }

            String repFolderPath = repositoryPath;

            foreach (TreeNode tnd1 in tndNodes)
            {
                String metadataType = MetadataDifferenceProcessing.folderToType(tnd1.Text, "");

                if (tnd1.Nodes.Count > 0)
                {
                    foreach (TreeNode tnd2 in tnd1.Nodes)
                    {
                        if (tnd2.Checked == true)
                        {
                            String[] tnd2NodeFullPath = tnd2.FullPath.Split('\\');

                            DirectoryInfo di;
                            if (!Directory.Exists(repFolderPath + "\\" + tnd1.Text))
                            {
                                di = Directory.CreateDirectory(repFolderPath + "\\" + tnd1.Text);
                            }
                            else
                            {
                                di = new DirectoryInfo(repFolderPath + "\\" + tnd1.Text);
                            }

                            // tnd2.FullPath will be something like this: "classes\\AccOppTerritoryBatch.cls"
                            // Copy the file into the folder
                            // If it is an aura / lwc folder, then follow the same processes as you have for the buildMetadataPackage.
                            if (metadataType == "AuraDefinitionBundle" || metadataType == "LightningComponentBundle")
                            {
                                UtilityClass.copyDirectory(projectFolder + "\\" + tnd1.Text + "\\" + tnd2NodeFullPath[1],
                                                           repFolderPath + "\\" + tnd1.Text + "\\" + tnd2NodeFullPath[1],
                                                           true);
                            }
                            else
                            {
                                File.Copy(projectFolder + "\\" + tnd1.Text + "\\" + tnd2NodeFullPath[1],
                                          repFolderPath + "\\" + tnd1.Text + "\\" + tnd2NodeFullPath[1],
                                          true);
                            }
                        }
                    }
                }
            }
        }
    }
}
