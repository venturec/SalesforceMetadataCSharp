using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml;
using static SalesforceMetadata.XmlNodeParser;

namespace SalesforceMetadata
{
    public class XmlNodeParser
    {
        // The whole purpose of this class is mostly to handle making sure the XML text values have the proper escape values
        // When you use the XmlDocument.loadXml or load, it converts all escaped text back to the original format and can't find 
        // a flag available to prevent this from happening, so built this structure to handle adding back the escape sequences.

        // Directory Name -> File Name -> nd1.Name -> nd2.Name -> nameValue + "|" + nd3.Name + "|" + nd4.Name + "|" + nd5.Name + "|" + nd6.Name + "|" + nd7.Name + "|" + nd8.Name -> List of node values
        public void parseXmlDocument(String directoryName, String fileName, XmlDocument xmlDoc,
                                      HashSet<String> comparisonHashSet)
        {
            // First find the #text node values to determine if there are changes to those vlaues.
            // Use the parent node names up the chain to be the keys with the #text value being the value

            XmlNodeParser ndParser = new XmlNodeParser();

            String nodeBlockNameValue = "";
            foreach (XmlNode nd3 in xmlDoc.ChildNodes)
            {
                if (nd3.OuterXml == "<?xml version=\"1.0\" encoding=\"UTF-8\"?>")
                {
                    continue;
                }

                if (nd3.HasChildNodes)
                {
                    String startingNodeValue = directoryName + "\\" + fileName + "\\" + nd3.Name;

                    foreach (XmlNode nd4 in nd3.ChildNodes)
                    {
                        /****************************************************************/
                        nodeBlockNameValue = MetadataDifferenceProcessing.getNameField(nd3.Name, nd4.Name, nd4.OuterXml);
                        /****************************************************************/

                        if (nd4.HasChildNodes)
                        {
                            if (nodeBlockNameValue == "")
                            {
                                nodeBlockNameValue = nd4.Name;
                            }
                            else
                            {
                                nodeBlockNameValue = nd4.Name + " | " + nodeBlockNameValue;
                            }

                            List<XmlNodeParser.XmlNodeValue> ndPathAndValues = ndParser.parseXmlChildNodes1(nd4);
                            HashSet<String> tempHashSet = ndParser.flattenXmlNodeValue(ndPathAndValues);

                            foreach (String val in tempHashSet)
                            {
                                comparisonHashSet.Add(startingNodeValue + "\\" + nodeBlockNameValue + "\\" + val);
                            }
                        }
                        else
                        {
                            comparisonHashSet.Add(startingNodeValue + "\\" + ndParser.replaceSpecialCharacters(nd4.OuterXml));
                        }
                    }
                }
                //else
                //{
                //}
            }
        }

        // TODO:
        // Bypass the description field when running the replaceSpecialCharacters
        public List<XmlNodeValue> parseXmlChildNodes1(XmlNode nd1)
        {
            List<XmlNodeValue> xmlNodeAndValuesList = new List<XmlNodeValue>();
            if (nd1.HasChildNodes)
            {
                foreach (XmlNode nd in nd1.ChildNodes)
                {
                    if (nd.NodeType == XmlNodeType.Text)
                    {
                        XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                        xmlNodeAndValues.nodeValue = replaceSpecialCharacters(nd.OuterXml);
                        xmlNodeAndValuesList.Add(xmlNodeAndValues);
                    }
                    else
                    {
                        XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                        xmlNodeAndValues.nodeName = nd.Name;
                        parseXmlChildNodes2(nd, xmlNodeAndValues);
                        xmlNodeAndValuesList.Add(xmlNodeAndValues);
                    }
                }
            }
            else
            {
                XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                xmlNodeAndValues.nodeName = nd1.Name;
                xmlNodeAndValues.nodeValue = replaceSpecialCharacters(nd1.OuterXml);
                xmlNodeAndValuesList.Add(xmlNodeAndValues);
            }

            return xmlNodeAndValuesList;
        }
        private void parseXmlChildNodes2(XmlNode nd2, XmlNodeValue XmlNodeValues2)
        {
            if (nd2.HasChildNodes)
            {
                foreach (XmlNode nd in nd2.ChildNodes)
                {
                    if (nd.NodeType == XmlNodeType.Text)
                    {
                        XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                        xmlNodeAndValues.nodeName = nd.Name;
                        xmlNodeAndValues.nodeValue = replaceSpecialCharacters(nd.OuterXml);
                        XmlNodeValues2.relatedNodeValues.Add(xmlNodeAndValues);
                    }
                    else
                    {
                        XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                        xmlNodeAndValues.nodeName = nd.Name;
                        parseXmlChildNodes3(nd, xmlNodeAndValues);
                        XmlNodeValues2.relatedNodeValues.Add(xmlNodeAndValues);
                    }
                }
            }
            else
            {
                XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                xmlNodeAndValues.nodeName = nd2.Name;
                xmlNodeAndValues.nodeValue = replaceSpecialCharacters(nd2.OuterXml);
                XmlNodeValues2.relatedNodeValues.Add(xmlNodeAndValues);
            }
        }
        private void parseXmlChildNodes3(XmlNode nd3, XmlNodeValue XmlNodeValues3)
        {
            if (nd3.HasChildNodes)
            {
                foreach (XmlNode nd in nd3.ChildNodes)
                {
                    if (nd.NodeType == XmlNodeType.Text)
                    {
                        XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                        xmlNodeAndValues.nodeName = nd.Name;
                        xmlNodeAndValues.nodeValue = replaceSpecialCharacters(nd.OuterXml);
                        XmlNodeValues3.relatedNodeValues.Add(xmlNodeAndValues);
                    }
                    else
                    {
                        XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                        xmlNodeAndValues.nodeName = nd.Name;
                        parseXmlChildNodes4(nd, xmlNodeAndValues);
                        XmlNodeValues3.relatedNodeValues.Add(xmlNodeAndValues);
                    }
                }
            }
            else
            {
                XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                xmlNodeAndValues.nodeName = nd3.Name;
                xmlNodeAndValues.nodeValue = replaceSpecialCharacters(nd3.OuterXml);
                XmlNodeValues3.relatedNodeValues.Add(xmlNodeAndValues);
            }
        }
        private void parseXmlChildNodes4(XmlNode nd4, XmlNodeValue XmlNodeValues4)
        {
            if (nd4.HasChildNodes)
            {
                foreach (XmlNode nd in nd4.ChildNodes)
                {
                    if (nd.NodeType == XmlNodeType.Text)
                    {
                        XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                        xmlNodeAndValues.nodeName = nd.Name;
                        xmlNodeAndValues.nodeValue = replaceSpecialCharacters(nd.OuterXml);
                        XmlNodeValues4.relatedNodeValues.Add(xmlNodeAndValues);
                    }
                    else
                    {
                        XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                        xmlNodeAndValues.nodeName = nd.Name;
                        parseXmlChildNodes5(nd, xmlNodeAndValues);
                        XmlNodeValues4.relatedNodeValues.Add(xmlNodeAndValues);
                    }
                }
            }
            else
            {
                XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                xmlNodeAndValues.nodeName = nd4.Name;
                xmlNodeAndValues.nodeValue = replaceSpecialCharacters(nd4.OuterXml);
                XmlNodeValues4.relatedNodeValues.Add(xmlNodeAndValues);
            }
        }
        private void parseXmlChildNodes5(XmlNode nd5, XmlNodeValue XmlNodeValues5)
        {
            if (nd5.HasChildNodes)
            {
                foreach (XmlNode nd in nd5.ChildNodes)
                {
                    if (nd.NodeType == XmlNodeType.Text)
                    {
                        XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                        xmlNodeAndValues.nodeName = nd.Name;
                        xmlNodeAndValues.nodeValue = replaceSpecialCharacters(nd.OuterXml);
                        XmlNodeValues5.relatedNodeValues.Add(xmlNodeAndValues);
                    }
                    else
                    {
                        XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                        xmlNodeAndValues.nodeName = nd.Name;
                        parseXmlChildNodes6(nd, xmlNodeAndValues);
                        XmlNodeValues5.relatedNodeValues.Add(xmlNodeAndValues);
                    }
                }
            }
            else
            {
                XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                xmlNodeAndValues.nodeName = nd5.Name;
                xmlNodeAndValues.nodeValue = replaceSpecialCharacters(nd5.OuterXml);
                XmlNodeValues5.relatedNodeValues.Add(xmlNodeAndValues);
            }
        }
        private void parseXmlChildNodes6(XmlNode nd6, XmlNodeValue XmlNodeValues6)
        {
            if (nd6.HasChildNodes)
            {
                foreach (XmlNode nd in nd6.ChildNodes)
                {
                    if (nd.NodeType == XmlNodeType.Text)
                    {
                        XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                        xmlNodeAndValues.nodeName = nd.Name;
                        xmlNodeAndValues.nodeValue = replaceSpecialCharacters(nd.OuterXml);
                        XmlNodeValues6.relatedNodeValues.Add(xmlNodeAndValues);
                    }
                    else
                    {
                        XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                        xmlNodeAndValues.nodeName = nd.Name;
                        parseXmlChildNodes7(nd, xmlNodeAndValues);
                        XmlNodeValues6.relatedNodeValues.Add(xmlNodeAndValues);
                    }
                }
            }
            else
            {
                XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                xmlNodeAndValues.nodeName = nd6.Name;
                xmlNodeAndValues.nodeValue = replaceSpecialCharacters(nd6.OuterXml);
                XmlNodeValues6.relatedNodeValues.Add(xmlNodeAndValues);
            }
        }
        private void parseXmlChildNodes7(XmlNode nd7, XmlNodeValue XmlNodeValues7)
        {
            if (nd7.HasChildNodes)
            {
                foreach (XmlNode nd in nd7.ChildNodes)
                {
                    if (nd.NodeType == XmlNodeType.Text)
                    {
                        XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                        xmlNodeAndValues.nodeName = nd.Name;
                        xmlNodeAndValues.nodeValue = replaceSpecialCharacters(nd.OuterXml);
                        XmlNodeValues7.relatedNodeValues.Add(xmlNodeAndValues);
                    }
                    else
                    {
                        XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                        xmlNodeAndValues.nodeName = nd.Name;
                        parseXmlChildNodes8(nd, xmlNodeAndValues);
                        XmlNodeValues7.relatedNodeValues.Add(xmlNodeAndValues);
                    }
                }
            }
            else
            {
                XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                xmlNodeAndValues.nodeName = nd7.Name;
                xmlNodeAndValues.nodeValue = replaceSpecialCharacters(nd7.OuterXml);
                XmlNodeValues7.relatedNodeValues.Add(xmlNodeAndValues);
            }
        }
        private void parseXmlChildNodes8(XmlNode nd8, XmlNodeValue XmlNodeValues8)
        {
            if (nd8.HasChildNodes)
            {
                foreach (XmlNode nd in nd8.ChildNodes)
                {
                    if (nd.NodeType == XmlNodeType.Text)
                    {
                        XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                        xmlNodeAndValues.nodeName = nd.Name;
                        xmlNodeAndValues.nodeValue = replaceSpecialCharacters(nd.OuterXml);
                        XmlNodeValues8.relatedNodeValues.Add(xmlNodeAndValues);
                    }
                    else
                    {
                        XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                        xmlNodeAndValues.nodeName = nd.Name;
                        parseXmlChildNodes9(nd, xmlNodeAndValues);
                        XmlNodeValues8.relatedNodeValues.Add(xmlNodeAndValues);
                    }
                }
            }
            else
            {
                XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                xmlNodeAndValues.nodeName = nd8.Name;
                xmlNodeAndValues.nodeValue = replaceSpecialCharacters(nd8.OuterXml);
                XmlNodeValues8.relatedNodeValues.Add(xmlNodeAndValues);
            }
        }
        private void parseXmlChildNodes9(XmlNode nd9, XmlNodeValue XmlNodeValues9)
        {
            if (nd9.HasChildNodes)
            {
                foreach (XmlNode nd in nd9.ChildNodes)
                {
                    if (nd.NodeType == XmlNodeType.Text)
                    {
                        XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                        xmlNodeAndValues.nodeName = nd.Name;
                        xmlNodeAndValues.nodeValue = replaceSpecialCharacters(nd.OuterXml);
                        XmlNodeValues9.relatedNodeValues.Add(xmlNodeAndValues);
                    }
                    else
                    {
                        XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                        xmlNodeAndValues.nodeName = nd.Name;
                        parseXmlChildNodes10(nd, xmlNodeAndValues);
                        XmlNodeValues9.relatedNodeValues.Add(xmlNodeAndValues);
                    }
                }
            }
            else
            {
                XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                xmlNodeAndValues.nodeName = nd9.Name;
                xmlNodeAndValues.nodeValue = replaceSpecialCharacters(nd9.OuterXml);
                XmlNodeValues9.relatedNodeValues.Add(xmlNodeAndValues);
            }
        }
        private void parseXmlChildNodes10(XmlNode nd10, XmlNodeValue XmlNodeValues10)
        {
            if (nd10.HasChildNodes)
            {
                foreach (XmlNode nd in nd10.ChildNodes)
                {
                    if (nd.NodeType == XmlNodeType.Text)
                    {
                        XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                        xmlNodeAndValues.nodeName = nd.Name;
                        xmlNodeAndValues.nodeValue = replaceSpecialCharacters(nd.OuterXml);
                        XmlNodeValues10.relatedNodeValues.Add(xmlNodeAndValues);
                    }
                    else
                    {
                        XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                        xmlNodeAndValues.nodeName = nd.Name;
                        parseXmlChildNodes11(nd, xmlNodeAndValues);
                        XmlNodeValues10.relatedNodeValues.Add(xmlNodeAndValues);
                    }
                }
            }
            else
            {
                XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                xmlNodeAndValues.nodeName = nd10.Name;
                xmlNodeAndValues.nodeValue = replaceSpecialCharacters(nd10.OuterXml);
                XmlNodeValues10.relatedNodeValues.Add(xmlNodeAndValues);
            }
        }
        private void parseXmlChildNodes11(XmlNode nd11, XmlNodeValue XmlNodeValues11)
        {
            if (nd11.HasChildNodes)
            {
                foreach (XmlNode nd in nd11.ChildNodes)
                {
                    if (nd.NodeType == XmlNodeType.Text)
                    {
                        XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                        xmlNodeAndValues.nodeName = nd.Name;
                        xmlNodeAndValues.nodeValue = replaceSpecialCharacters(nd.OuterXml);
                        XmlNodeValues11.relatedNodeValues.Add(xmlNodeAndValues);
                    }
                    else
                    {
                        XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                        xmlNodeAndValues.nodeName = nd.Name;
                        parseXmlChildNodes12(nd, xmlNodeAndValues);
                        XmlNodeValues11.relatedNodeValues.Add(xmlNodeAndValues);
                    }
                }
            }
            else
            {
                XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                xmlNodeAndValues.nodeName = nd11.Name;
                xmlNodeAndValues.nodeValue = replaceSpecialCharacters(nd11.OuterXml);
                XmlNodeValues11.relatedNodeValues.Add(xmlNodeAndValues);
            }
        }
        private void parseXmlChildNodes12(XmlNode nd12, XmlNodeValue XmlNodeValues12)
        {
            if (nd12.HasChildNodes)
            {
                foreach (XmlNode nd in nd12.ChildNodes)
                {
                    if (nd.NodeType == XmlNodeType.Text)
                    {
                        XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                        xmlNodeAndValues.nodeName = nd.Name;
                        xmlNodeAndValues.nodeValue = replaceSpecialCharacters(nd.OuterXml);
                        XmlNodeValues12.relatedNodeValues.Add(xmlNodeAndValues);
                    }
                    else
                    {
                        //XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                        //xmlNodeAndValues.nodeName = nd.Name;
                        //parseXmlChildNodes3(nd, xmlNodeAndValues);
                        //XmlNodeValues12.relatedNodeValues.Add(xmlNodeAndValues);
                    }
                }
            }
            else
            {
                XmlNodeValue xmlNodeAndValues = new XmlNodeValue();
                xmlNodeAndValues.nodeName = nd12.Name;
                xmlNodeAndValues.nodeValue = replaceSpecialCharacters(nd12.OuterXml);
                XmlNodeValues12.relatedNodeValues.Add(xmlNodeAndValues);
            }
        }

        public HashSet<String> buildTreeNodeDiffs1(TreeNodeCollection tndCollection)
        {
            HashSet<String> treeNodeDiffs = new HashSet<string>();

            foreach (TreeNode tndDiff1 in tndCollection)
            {
                if (tndDiff1.Checked == true && tndDiff1.Nodes.Count > 0)
                {
                    buildTreeNodeDiffs2(tndDiff1, treeNodeDiffs);
                }
                else if (tndDiff1.Checked == true)
                {
                    treeNodeDiffs.Add(tndDiff1.Text);
                }
            }

            return treeNodeDiffs;
        }

        private void buildTreeNodeDiffs2(TreeNode parentNode, HashSet<String> treeNodeDiffs)
        {
            foreach (TreeNode tndDiff2 in parentNode.Nodes)
            {
                if (tndDiff2.Checked == true && parentNode.Text == "staticresources")
                {
                    String modifiedTndDiff = tndDiff2.FullPath.Replace("[New] ", "");
                    String[] parsedFullPath = modifiedTndDiff.Split('\\');
                    treeNodeDiffs.Add(buildParsedFullPath(parsedFullPath));
                }
                else if (tndDiff2.Checked == true && tndDiff2.Nodes.Count > 0)
                {
                    buildTreeNodeDiffs3(tndDiff2, treeNodeDiffs);
                }
                else if (tndDiff2.Checked == true)
                {
                    if (tndDiff2.Checked == true && tndDiff2.FullPath.Contains("[New]"))
                    {
                        String modifiedTndDiff = tndDiff2.FullPath.Replace("[New] ", "");
                        String[] parsedFullPath = modifiedTndDiff.Split('\\');
                        treeNodeDiffs.Add(buildParsedFullPath(parsedFullPath));
                    }
                    else if (tndDiff2.Checked == true && tndDiff2.FullPath.Contains("[Updated]"))
                    {
                        String modifiedTndDiff = tndDiff2.FullPath.Replace("[Updated] ", "");
                        String[] parsedFullPath = modifiedTndDiff.Split('\\');
                        treeNodeDiffs.Add(buildParsedFullPath(parsedFullPath));
                    }
                    else if (tndDiff2.Checked == true)
                    {
                        String[] parsedFullPath = tndDiff2.FullPath.Split('\\');
                        treeNodeDiffs.Add(buildParsedFullPath(parsedFullPath));
                    }
                }
            }
        }

        private void buildTreeNodeDiffs3(TreeNode parentNode, HashSet<String> treeNodeDiffs)
        {
            foreach (TreeNode tndDiff3 in parentNode.Nodes)
            {
                if (tndDiff3.Checked == true && tndDiff3.Nodes.Count > 0)
                {
                    buildTreeNodeDiffs4(tndDiff3, treeNodeDiffs);
                }
                else if(tndDiff3.Checked == true)
                {
                    if (tndDiff3.Checked == true && tndDiff3.FullPath.Contains("[New]"))
                    {
                        String modifiedTndDiff = tndDiff3.FullPath.Replace("[New] ", "");
                        String[] parsedFullPath = modifiedTndDiff.Split('\\');
                        treeNodeDiffs.Add(buildParsedFullPath(parsedFullPath));
                    }
                    else if (tndDiff3.Checked == true && tndDiff3.FullPath.Contains("[Updated]"))
                    {
                        String modifiedTndDiff = tndDiff3.FullPath.Replace("[Updated] ", "");
                        String[] parsedFullPath = modifiedTndDiff.Split('\\');
                        treeNodeDiffs.Add(buildParsedFullPath(parsedFullPath));
                    }
                    else if (tndDiff3.Checked == true)
                    {
                        String[] parsedFullPath = tndDiff3.FullPath.Split('\\');
                        treeNodeDiffs.Add(buildParsedFullPath(parsedFullPath));
                    }
                }
            }
        }

        private void buildTreeNodeDiffs4(TreeNode parentNode, HashSet<String> treeNodeDiffs)
        {
            foreach (TreeNode tndDiff4 in parentNode.Nodes)
            {
                if (tndDiff4.Checked == true && tndDiff4.FullPath.Contains("[New]"))
                {
                    String modifiedTndDiff = tndDiff4.FullPath.Replace("[New] ", "");
                    String[] parsedFullPath = modifiedTndDiff.Split('\\');
                    treeNodeDiffs.Add(buildParsedFullPath(parsedFullPath));
                }
                else if (tndDiff4.Checked == true && tndDiff4.FullPath.Contains("[Updated]"))
                {
                    String modifiedTndDiff = tndDiff4.FullPath.Replace("[Updated] ", "");
                    String[] parsedFullPath = modifiedTndDiff.Split('\\');
                    treeNodeDiffs.Add(buildParsedFullPath(parsedFullPath));
                }
                else if (tndDiff4.Checked == true)
                {
                    String[] parsedFullPath = tndDiff4.FullPath.Split('\\');
                    treeNodeDiffs.Add(buildParsedFullPath(parsedFullPath));
                }
            }
        }

        private String buildParsedFullPath(String[] parsedFullPath) 
        {
            String fullPath = "";

            for (Int32 i = 0; i < parsedFullPath.Length; i++) 
            {
                if (i == 0)
                {
                    fullPath = parsedFullPath[i];
                }
                // Skip the top node
                else if (i == 2)
                {
                    //fullPath = fullPath + " \\" + parsedFullPath[i];
                }
                else
                {
                    fullPath = fullPath + "\\" + parsedFullPath[i];
                }
            }

            return fullPath;
        }

        //private void buildTreeNodeDiffs5(TreeNode parentNode, HashSet<String> treeNodeDiffs)
        //{
        //    foreach (TreeNode tndDiff5 in parentNode.Nodes)
        //    {
        //        if (tndDiff5.Checked == true && tndDiff5.Nodes.Count > 0)
        //        {
        //            buildTreeNodeDiffs6(tndDiff5, treeNodeDiffs);
        //        }
        //        else if (tndDiff5.Checked == true)
        //        {
        //            Debug.WriteLine("");
        //        }
        //    }
        //}

        //private void buildTreeNodeDiffs6(TreeNode parentNode, HashSet<String> treeNodeDiffs)
        //{
        //    foreach (TreeNode tndDiff6 in parentNode.Nodes)
        //    {
        //        if (tndDiff6.Checked == true && tndDiff6.Nodes.Count > 0)
        //        {
        //            Debug.WriteLine("");
        //        }
        //        else if (tndDiff6.Checked == true)
        //        {
        //            Debug.WriteLine("");
        //        }
        //    }
        //}

        public String[] parseNodeNameAndValue(String nodeNameWithValue)
        {
            String[] parsedNodeNameWithValue = nodeNameWithValue.Split('|');
            return parsedNodeNameWithValue;
        }
        public String replaceSpecialCharacters(String xmlText)
        {
            xmlText = xmlText.Replace("\'", "&apos;");
            xmlText = xmlText.Replace("\"", "&quot;");
            //xmlText = xmlText.Replace("&", "&amp;");
            xmlText = xmlText.Replace(">", "&gt;");
            xmlText = xmlText.Replace("<", "&lt;");

            return xmlText;
        }

        // This is used for the Metadata Comparison logic and is a flattened version of all elements with their values
        // This discards the group sub-elements under the parent
        // It only returns a flattened Set (unique)
        public HashSet<String> flattenXmlNodeValue(List<XmlNodeValue> ndPathAndValues)
        {
            HashSet<String> flattenedNodeValues = new HashSet<String>();

            foreach (XmlNodeValue ndVal1 in ndPathAndValues)
            {
                if (ndVal1.relatedNodeValues.Count > 0)
                {
                    foreach (XmlNodeValue ndVal2 in ndVal1.relatedNodeValues)
                    {
                        if (ndVal2.relatedNodeValues.Count > 0)
                        {
                            foreach (XmlNodeValue ndVal3 in ndVal2.relatedNodeValues)
                            {
                                if (ndVal3.relatedNodeValues.Count > 0)
                                {
                                    foreach (XmlNodeValue ndVal4 in ndVal3.relatedNodeValues)
                                    {
                                        if (ndVal4.relatedNodeValues.Count > 0)
                                        {
                                            foreach (XmlNodeValue ndVal5 in ndVal4.relatedNodeValues)
                                            {
                                                if (ndVal5.relatedNodeValues.Count > 0)
                                                {
                                                    foreach (XmlNodeValue ndVal6 in ndVal5.relatedNodeValues)
                                                    {
                                                        if (ndVal6.relatedNodeValues.Count > 0)
                                                        {
                                                            foreach (XmlNodeValue ndVal7 in ndVal6.relatedNodeValues)
                                                            {
                                                                if (ndVal7.relatedNodeValues.Count > 0)
                                                                {
                                                                    foreach (XmlNodeValue ndVal8 in ndVal7.relatedNodeValues)
                                                                    {
                                                                        if (ndVal8.relatedNodeValues.Count > 0)
                                                                        {
                                                                            foreach (XmlNodeValue ndVal9 in ndVal8.relatedNodeValues)
                                                                            {
                                                                                if (ndVal9.relatedNodeValues.Count > 0)
                                                                                {
                                                                                    foreach (XmlNodeValue ndVal10 in ndVal9.relatedNodeValues)
                                                                                    {
                                                                                        if (ndVal10.relatedNodeValues.Count > 0)
                                                                                        {
                                                                                            foreach (XmlNodeValue ndVal11 in ndVal10.relatedNodeValues)
                                                                                            {
                                                                                                if (ndVal11.relatedNodeValues.Count > 0)
                                                                                                {
                                                                                                    foreach (XmlNodeValue ndVal12 in ndVal11.relatedNodeValues)
                                                                                                    {
                                                                                                        if (ndVal12.relatedNodeValues.Count > 0)
                                                                                                        {
                                                                                                            // I think we are safe to assume the layers don't go this deep, but will monitor 
                                                                                                            // and possibly throw an error if it gets here to let me know I should add more logical 
                                                                                                            // layers
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            String ndNamesAndValues = ndVal1.nodeName + "\\"
                                                                                                                                    + ndVal2.nodeName + "\\"
                                                                                                                                    + ndVal3.nodeName + "\\"
                                                                                                                                    + ndVal4.nodeName + "\\"
                                                                                                                                    + ndVal5.nodeName + "\\"
                                                                                                                                    + ndVal6.nodeName + "\\"
                                                                                                                                    + ndVal7.nodeName + "\\"
                                                                                                                                    + ndVal8.nodeName + "\\"
                                                                                                                                    + ndVal9.nodeName + "\\"
                                                                                                                                    + ndVal10.nodeName + "\\"
                                                                                                                                    + ndVal11.nodeName + "\\"
                                                                                                                                    + ndVal12.nodeValue;

                                                                                                            flattenedNodeValues.Add(ndNamesAndValues);
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    String ndNamesAndValues = ndVal1.nodeName + "\\"
                                                                                                                            + ndVal2.nodeName + "\\"
                                                                                                                            + ndVal3.nodeName + "\\"
                                                                                                                            + ndVal4.nodeName + "\\"
                                                                                                                            + ndVal5.nodeName + "\\"
                                                                                                                            + ndVal6.nodeName + "\\"
                                                                                                                            + ndVal7.nodeName + "\\"
                                                                                                                            + ndVal8.nodeName + "\\"
                                                                                                                            + ndVal9.nodeName + "\\"
                                                                                                                            + ndVal10.nodeName + "\\"
                                                                                                                            + ndVal11.nodeValue;

                                                                                                    flattenedNodeValues.Add(ndNamesAndValues);
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            String ndNamesAndValues = ndVal1.nodeName + "\\"
                                                                                                                    + ndVal2.nodeName + "\\"
                                                                                                                    + ndVal3.nodeName + "\\"
                                                                                                                    + ndVal4.nodeName + "\\"
                                                                                                                    + ndVal5.nodeName + "\\"
                                                                                                                    + ndVal6.nodeName + "\\"
                                                                                                                    + ndVal7.nodeName + "\\"
                                                                                                                    + ndVal8.nodeName + "\\"
                                                                                                                    + ndVal9.nodeName + "\\"
                                                                                                                    + ndVal10.nodeValue;

                                                                                            flattenedNodeValues.Add(ndNamesAndValues);
                                                                                        }
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    String ndNamesAndValues = ndVal1.nodeName + "\\"
                                                                                                            + ndVal2.nodeName + "\\"
                                                                                                            + ndVal3.nodeName + "\\"
                                                                                                            + ndVal4.nodeName + "\\"
                                                                                                            + ndVal5.nodeName + "\\"
                                                                                                            + ndVal6.nodeName + "\\"
                                                                                                            + ndVal7.nodeName + "\\"
                                                                                                            + ndVal8.nodeName + "\\"
                                                                                                            + ndVal9.nodeValue;

                                                                                    flattenedNodeValues.Add(ndNamesAndValues);
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            String ndNamesAndValues = ndVal1.nodeName + "\\"
                                                                                                    + ndVal2.nodeName + "\\"
                                                                                                    + ndVal3.nodeName + "\\"
                                                                                                    + ndVal4.nodeName + "\\"
                                                                                                    + ndVal5.nodeName + "\\"
                                                                                                    + ndVal6.nodeName + "\\"
                                                                                                    + ndVal7.nodeName + "\\"
                                                                                                    + ndVal8.nodeValue;

                                                                            flattenedNodeValues.Add(ndNamesAndValues);
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    String ndNamesAndValues = ndVal1.nodeName + "\\"
                                                                                            + ndVal2.nodeName + "\\"
                                                                                            + ndVal3.nodeName + "\\"
                                                                                            + ndVal4.nodeName + "\\"
                                                                                            + ndVal5.nodeName + "\\"
                                                                                            + ndVal6.nodeName + "\\"
                                                                                            + ndVal7.nodeValue;

                                                                    flattenedNodeValues.Add(ndNamesAndValues);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            String ndNamesAndValues = ndVal1.nodeName + "\\"
                                                                                    + ndVal2.nodeName + "\\"
                                                                                    + ndVal3.nodeName + "\\"
                                                                                    + ndVal4.nodeName + "\\"
                                                                                    + ndVal5.nodeName + "\\"
                                                                                    + ndVal6.nodeValue;

                                                            flattenedNodeValues.Add(ndNamesAndValues);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    String ndNamesAndValues = ndVal1.nodeName + "\\"
                                                                            + ndVal2.nodeName + "\\"
                                                                            + ndVal3.nodeName + "\\"
                                                                            + ndVal4.nodeName + "\\"
                                                                            + ndVal5.nodeValue;

                                                    flattenedNodeValues.Add(ndNamesAndValues);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            String ndNamesAndValues = ndVal1.nodeName + "\\"
                                                                    + ndVal2.nodeName + "\\"
                                                                    + ndVal3.nodeName + "\\"
                                                                    + ndVal4.nodeValue;

                                            flattenedNodeValues.Add(ndNamesAndValues);
                                        }
                                    }
                                }
                                else
                                {
                                    String ndNamesAndValues = ndVal1.nodeName + "\\"
                                                            + ndVal2.nodeName + "\\"
                                                            + ndVal3.nodeValue;

                                    flattenedNodeValues.Add(ndNamesAndValues);
                                }
                            }
                        }
                        else
                        {
                            String ndNamesAndValues = ndVal1.nodeName + "\\"
                                                    + ndVal2.nodeValue;

                            flattenedNodeValues.Add(ndNamesAndValues);
                        }
                    }
                }
                else
                {
                    String ndNamesAndValues = ndVal1.nodeValue;
                    flattenedNodeValues.Add(ndNamesAndValues);
                }
            }

            return flattenedNodeValues;
        }

        // This accommodates the GenerateDeploymentPackage and the DevelopmentEnvironment objects
        // The incoming parentNode should be tnd4 with the structure:
        // folder name \\ file name \\ top xml node \\ parent node | node name (if it exits)
        // tnd5 will contain element names under tnd4
        // and tnd6 will contain all other element names and values
        // We are only going to go to two TreeNode layers
        // tnd1 will translate to tnd5 - parentNode (tnd4) + tnd5
        // tnd2 will translate to tnd6 - parentNode (tnd4) + tnd5 + tn6
        // tnd2 will be the concatenation of all xml elements and any values
        // We don't want to create any more than this as we will run into memor issues by having a bunch of layered embedded TreeNode elements
        
        // tnd5
        public TreeNode buildTreeNodeWithValues1(TreeNode parentNode, List<XmlNodeValue> ndPathAndValues)
        {
            foreach (XmlNodeValue ndVal in ndPathAndValues)
            {
                TreeNode tnd1 = new TreeNode();

                // #text node
                if (ndVal.relatedNodeValues.Count == 0)
                {
                    tnd1.Text = ndVal.nodeValue;
                    parentNode.Nodes.Add(tnd1);
                    continue;
                }
                else
                {
                    tnd1.Text = ndVal.nodeName;
                    buildTreeNodeWithValues2(tnd1, ndVal.relatedNodeValues);
                    parentNode.Nodes.Add(tnd1);
                }
            }

            return parentNode;
        }

        // tnd6
        private void buildTreeNodeWithValues2(TreeNode parentNode, List<XmlNodeValue> ndPathAndValues)
        {
            foreach (XmlNodeValue ndVal in ndPathAndValues)
            {
                TreeNode tnd2 = new TreeNode();

                // #text node
                if (ndVal.relatedNodeValues.Count == 0)
                {
                    tnd2.Text = ndVal.nodeValue;
                    parentNode.Nodes.Add(tnd2);
                    continue;
                }
                else
                {
                    tnd2.Text = "<" + ndVal.nodeName + ">";
                    buildTreeNodeWithValues3(tnd2, ndVal.nodeName, ndVal.relatedNodeValues);
                    tnd2.Text = tnd2.Text + "</" + ndVal.nodeName + ">";
                    parentNode.Nodes.Add(tnd2);
                }
            }
        }

        // Shift in logic from the above two
        // We are concatenating the node element names and values
        private void buildTreeNodeWithValues3(TreeNode parentNode, String nodeName, List<XmlNodeValue> ndPathAndValues)
        {
            foreach (XmlNodeValue ndVal in ndPathAndValues)
            {
                // #text node
                if (ndVal.relatedNodeValues.Count == 0)
                {
                    parentNode.Text = parentNode.Text + ndVal.nodeValue;
                    continue;
                }
                else
                {
                    parentNode.Text = parentNode.Text + "<" + ndVal.nodeName + ">";
                    buildTreeNodeWithValues4(parentNode, ndVal.relatedNodeValues);
                    parentNode.Text = parentNode.Text + "</" + ndVal.nodeName + ">";
                }
            }
        }
        private void buildTreeNodeWithValues4(TreeNode parentNode, List<XmlNodeValue> ndPathAndValues)
        {
            foreach (XmlNodeValue ndVal in ndPathAndValues)
            {
                // #text node
                if (ndVal.relatedNodeValues.Count == 0)
                {
                    parentNode.Text = parentNode.Text + ndVal.nodeValue;
                    continue;
                }
                else
                {
                    parentNode.Text = parentNode.Text + "<" + ndVal.nodeName + ">";
                    buildTreeNodeWithValues5(parentNode, ndVal.relatedNodeValues);
                    parentNode.Text = parentNode.Text + "</" + ndVal.nodeName + ">";
                }
            }
        }
        private void buildTreeNodeWithValues5(TreeNode parentNode, List<XmlNodeValue> ndPathAndValues)
        {
            foreach (XmlNodeValue ndVal in ndPathAndValues)
            {
                // #text node
                if (ndVal.relatedNodeValues.Count == 0)
                {
                    parentNode.Text = parentNode.Text + ndVal.nodeValue;
                    continue;
                }
                else
                {
                    parentNode.Text = parentNode.Text + "<" + ndVal.nodeName + ">";
                    buildTreeNodeWithValues6(parentNode, ndVal.relatedNodeValues);
                    parentNode.Text = parentNode.Text + "</" + ndVal.nodeName + ">";
                }
            }
        }
        private void buildTreeNodeWithValues6(TreeNode parentNode, List<XmlNodeValue> ndPathAndValues)
        {
            foreach (XmlNodeValue ndVal in ndPathAndValues)
            {
                // #text node
                if (ndVal.relatedNodeValues.Count == 0)
                {
                    parentNode.Text = parentNode.Text + ndVal.nodeValue;
                    continue;
                }
                else
                {
                    parentNode.Text = parentNode.Text + "<" + ndVal.nodeName + ">";
                    buildTreeNodeWithValues7(parentNode, ndVal.relatedNodeValues);
                    parentNode.Text = parentNode.Text + "</" + ndVal.nodeName + ">";
                }
            }
        }
        private void buildTreeNodeWithValues7(TreeNode parentNode, List<XmlNodeValue> ndPathAndValues)
        {
            foreach (XmlNodeValue ndVal in ndPathAndValues)
            {
                // #text node
                if (ndVal.relatedNodeValues.Count == 0)
                {
                    parentNode.Text = parentNode.Text + ndVal.nodeValue;
                    continue;
                }
                else
                {
                    parentNode.Text = parentNode.Text + "<" + ndVal.nodeName + ">";
                    buildTreeNodeWithValues8(parentNode, ndVal.relatedNodeValues);
                    parentNode.Text = parentNode.Text + "</" + ndVal.nodeName + ">";
                }
            }
        }
        private void buildTreeNodeWithValues8(TreeNode parentNode, List<XmlNodeValue> ndPathAndValues)
        {
            foreach (XmlNodeValue ndVal in ndPathAndValues)
            {
                // #text node
                if (ndVal.relatedNodeValues.Count == 0)
                {
                    parentNode.Text = parentNode.Text + ndVal.nodeValue;
                    continue;
                }
                else
                {
                    parentNode.Text = parentNode.Text + "<" + ndVal.nodeName + ">";
                    buildTreeNodeWithValues9(parentNode, ndVal.relatedNodeValues);
                    parentNode.Text = parentNode.Text + "</" + ndVal.nodeName + ">";
                }
            }
        }
        private void buildTreeNodeWithValues9(TreeNode parentNode, List<XmlNodeValue> ndPathAndValues)
        {
            foreach (XmlNodeValue ndVal in ndPathAndValues)
            {
                // #text node
                if (ndVal.relatedNodeValues.Count == 0)
                {
                    parentNode.Text = parentNode.Text + ndVal.nodeValue;
                    continue;
                }
                else
                {
                    parentNode.Text = parentNode.Text + "<" + ndVal.nodeName + ">";
                    buildTreeNodeWithValues10(parentNode, ndVal.relatedNodeValues);
                    parentNode.Text = parentNode.Text + "</" + ndVal.nodeName + ">";
                }
            }
        }
        private void buildTreeNodeWithValues10(TreeNode parentNode, List<XmlNodeValue> ndPathAndValues)
        {
            foreach (XmlNodeValue ndVal in ndPathAndValues)
            {
                // #text node
                if (ndVal.relatedNodeValues.Count == 0)
                {
                    parentNode.Text = parentNode.Text + ndVal.nodeValue;
                    continue;
                }
                else
                {
                    parentNode.Text = parentNode.Text + "<" + ndVal.nodeName + ">";
                    buildTreeNodeWithValues11(parentNode, ndVal.relatedNodeValues);
                    parentNode.Text = parentNode.Text + "</" + ndVal.nodeName + ">";
                }
            }
        }
        private void buildTreeNodeWithValues11(TreeNode parentNode, List<XmlNodeValue> ndPathAndValues)
        {
            foreach (XmlNodeValue ndVal in ndPathAndValues)
            {
                // #text node
                if (ndVal.relatedNodeValues.Count == 0)
                {
                    parentNode.Text = parentNode.Text + ndVal.nodeValue;
                    continue;
                }
                else
                {
                    parentNode.Text = parentNode.Text + "<" + ndVal.nodeName + ">";
                    buildTreeNodeWithValues12(parentNode, ndVal.relatedNodeValues);
                    parentNode.Text = parentNode.Text + "</" + ndVal.nodeName + ">";
                }
            }
        }
        private void buildTreeNodeWithValues12(TreeNode parentNode, List<XmlNodeValue> ndPathAndValues)
        {
            foreach (XmlNodeValue ndVal in ndPathAndValues)
            {
                // #text node
                if (ndVal.relatedNodeValues.Count == 0)
                {
                    parentNode.Text = parentNode.Text + ndVal.nodeValue;
                    continue;
                }
                else
                {
                    // I think we are safe to assume the layers don't go this deep, but will monitor 
                    // and possibly throw an error if it gets here to let me know I should add more logical layers

                    //parentNode.Text = parentNode.Text + "<" + ndVal.nodeName + ">";
                    //buildTreeNodeWithValues6(parentNode, ndVal.relatedNodeValues);
                    //parentNode.Text = parentNode.Text + "</" + ndVal.nodeName + ">";
                }
            }
        }

        public class XmlNodeValue
        {
            public String nodeName;
            public String nodeValue;
            public List<XmlNodeValue> relatedNodeValues;

            public XmlNodeValue()
            {
                nodeName = "";
                nodeValue = "";
                relatedNodeValues = new List<XmlNodeValue>();
            }
        }
    }
}
