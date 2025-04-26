using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;
using System.Xml;
using static SalesforceMetadata.XmlNodeParser;

namespace SalesforceMetadata
{
    public class XmlNodeParser
    {
        // The whole purpose of this class is mostly to handle making sure the XML text values have the proper escape values
        // When you use the XmlDocument.loadXml or load, it converts all escaped text back to the original format and can't find 
        // a flag available to prevent this from happening, so built this structure to handle adding back the escape sequences.

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
        public String[] parseNodeNameAndValue(String nodeNameWithValue)
        {
            String[] parsedNodeNameWithValue = nodeNameWithValue.Split('|');
            return parsedNodeNameWithValue;
        }
        public String replaceSpecialCharacters(String xmlText)
        {
            // Prep the incoming XML text 
            xmlText = xmlText.Replace("'", " ' ");
            xmlText = xmlText.Replace("\"", " \" ");
            xmlText = xmlText.Replace("&&", " && ");
            xmlText = xmlText.Replace("<>", " <> ");
            xmlText = xmlText.Replace(">=", " >= ");
            xmlText = xmlText.Replace("<=", " <= ");
            xmlText = xmlText.Replace("||", " || ");
            xmlText = xmlText.Replace("==", " == ");
            xmlText = xmlText.Replace(">", " > ");
            xmlText = xmlText.Replace("<", " < ");

            // Reduce whitespace
            xmlText = xmlText.Replace("  ", " ");
            xmlText = xmlText.Replace("  ", " ");
            xmlText = xmlText.Replace("  ", " ");
            xmlText = xmlText.Replace("  ", " ");
            xmlText = xmlText.Replace("  ", " ");
            xmlText = xmlText.Replace("  ", " ");
            xmlText = xmlText.Replace("  ", " ");
            xmlText = xmlText.Replace("  ", " ");
            xmlText = xmlText.Replace("  ", " ");
            xmlText = xmlText.Replace("  ", " ");

            // Bring the appropriate characters back together
            xmlText = xmlText.Replace(" & & ", " && ");
            xmlText = xmlText.Replace(" < > ", " <> ");
            xmlText = xmlText.Replace(" > = ", " >= ");
            xmlText = xmlText.Replace(" < = ", " <= ");

            Char[] charCheck = new char[5];
            charCheck[0] = ' ';

            String[] xmlSplit = xmlText.Split(charCheck);

            for (Int32 i = 0; i < xmlSplit.Length; i++)
            {
                if (xmlSplit[i] == "\'")
                {
                    xmlSplit[i] = "&apos;";
                }
                else if (xmlSplit[i] == "\"")
                {
                    xmlSplit[i] = "&quot;";
                }
                else if (xmlSplit[i] == "&&")
                {
                    xmlSplit[i] = "&amp;&amp;";
                }
                else if (xmlSplit[i] == "<>")
                {
                    xmlSplit[i] = "&lt;&gt;";
                }
                else if (xmlSplit[i] == ">=")
                {
                    xmlSplit[i] = "&gt;=";
                }
                else if (xmlSplit[i] == "<=")
                {
                    xmlSplit[i] = "&lt;=";
                }
                else if (xmlSplit[i] == ">")
                {
                    xmlSplit[i] = "&gt;";
                }
                else if (xmlSplit[i] == "<")
                {
                    xmlSplit[i] = "&lt;";
                }
                else if (xmlSplit[i] == "&")
                {
                    xmlSplit[i] = "&amp;";
                }
            }

            String xmlRtnVal = "";

            // will be either 0 or 1.
            // If 0 add a space before but not after. If 1, add a space after, but not before, then set the value back to 0
            Int32 quoteCount = 0;

            foreach (String val in xmlSplit)
            {
                if (val == "&apos;")
                {
                    if (quoteCount == 0)
                    {
                        xmlRtnVal = xmlRtnVal + " " + val;
                        quoteCount++;
                    }
                    else
                    {
                        xmlRtnVal = xmlRtnVal + val + " ";
                        quoteCount = 0;
                    }
                }
                else if (val == "&quot;")
                {
                    if (quoteCount == 0)
                    {
                        xmlRtnVal = xmlRtnVal + " " + val;
                        quoteCount++;
                    }
                    else
                    {
                        xmlRtnVal = xmlRtnVal + val + " ";
                        quoteCount = 0;
                    }
                }
                else
                {
                    if (quoteCount == 1)
                    {
                        xmlRtnVal = xmlRtnVal + val;
                    }
                    else
                    {
                        xmlRtnVal = xmlRtnVal + val + " ";
                    }
                }
            }

            // Reduce whitespace
            xmlText = xmlText.Replace("  ", " ");
            xmlText = xmlText.Replace("  ", " ");
            xmlText = xmlText.Replace("  ", " ");
            xmlText = xmlText.Replace("  ", " ");
            xmlText = xmlText.Replace("  ", " ");
            xmlText = xmlText.Replace("  ", " ");
            xmlText = xmlText.Replace("  ", " ");
            xmlText = xmlText.Replace("  ", " ");
            xmlText = xmlText.Replace("  ", " ");
            xmlText = xmlText.Replace("  ", " ");

            // Trim end
            xmlRtnVal = xmlRtnVal.TrimEnd(' ');

            return xmlRtnVal;
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

        public TreeNode buildTreeNodeWithValuesMini(TreeNode parentNode, List<XmlNodeValue> ndPathAndValues)
        {
            // This accommodates the GenerateDeploymentPackage and the DevelopmentEnvironment objects
            // The incoming parentNode should be tnd4 with the structure:
            // folder name \\ file name \\ top xml node \\ parent node | node name (if it exits)
            // tnd5 will contain element names under tnd4
            // and tnd6 will contain all other element names and values
            // We are only going to go to two TreeNode layers
            // tnd1 will translate to tnd5 - parentNode (tnd4) + tnd5
            // tnd2 will translate to tnd6 - parentNode (tnd4) + tnd5 + tn6
            // We don't want to create any more than this as we will run into memor issues by having a bunch of layered embedded TreeNode elements

            foreach (XmlNodeValue ndVal1 in ndPathAndValues)
            {
                if (ndVal1.relatedNodeValues.Count > 0)
                {
                    TreeNode tnd1 = new TreeNode(ndVal1.nodeName);

                    foreach (XmlNodeValue ndVal2 in ndVal1.relatedNodeValues)
                    {
                        if (ndVal2.relatedNodeValues.Count > 0)
                        {
                            TreeNode tnd2 = new TreeNode(ndVal2.nodeName);

                            foreach (XmlNodeValue ndVal3 in ndVal2.relatedNodeValues)
                            {
                                tnd2.Text = tnd2.Text + "\\" + ndVal3.nodeName;

                                if (ndVal3.relatedNodeValues.Count > 0)
                                {
                                    foreach (XmlNodeValue ndVal4 in ndVal3.relatedNodeValues)
                                    {
                                        if (ndVal4.relatedNodeValues.Count > 0)
                                        {
                                            tnd2.Text = tnd2.Text + "\\" + ndVal4.nodeName;

                                            foreach (XmlNodeValue ndVal5 in ndVal4.relatedNodeValues)
                                            {
                                                if (ndVal5.relatedNodeValues.Count > 0)
                                                {
                                                    tnd2.Text = tnd2.Text + "\\" + ndVal5.nodeName;

                                                    foreach (XmlNodeValue ndVal6 in ndVal5.relatedNodeValues)
                                                    {
                                                        if (ndVal6.relatedNodeValues.Count > 0)
                                                        {
                                                            tnd2.Text = tnd2.Text + "\\" + ndVal6.nodeName;

                                                            foreach (XmlNodeValue ndVal7 in ndVal6.relatedNodeValues)
                                                            {
                                                                if (ndVal7.relatedNodeValues.Count > 0)
                                                                {
                                                                    tnd2.Text = tnd2.Text + "\\" + ndVal7.nodeName;

                                                                    foreach (XmlNodeValue ndVal8 in ndVal7.relatedNodeValues)
                                                                    {
                                                                        if (ndVal8.relatedNodeValues.Count > 0)
                                                                        {
                                                                            tnd2.Text = tnd2.Text + "\\" + ndVal8.nodeName;

                                                                            foreach (XmlNodeValue ndVal9 in ndVal8.relatedNodeValues)
                                                                            {
                                                                                if (ndVal9.relatedNodeValues.Count > 0)
                                                                                {
                                                                                    tnd2.Text = tnd2.Text + "\\" + ndVal9.nodeName;

                                                                                    foreach (XmlNodeValue ndVal10 in ndVal9.relatedNodeValues)
                                                                                    {
                                                                                        if (ndVal10.relatedNodeValues.Count > 0)
                                                                                        {
                                                                                            tnd2.Text = tnd2.Text + "\\" + ndVal10.nodeName;

                                                                                            foreach (XmlNodeValue ndVal11 in ndVal10.relatedNodeValues)
                                                                                            {
                                                                                                if (ndVal11.relatedNodeValues.Count > 0)
                                                                                                {
                                                                                                    tnd2.Text = tnd2.Text + "\\" + ndVal11.nodeName;

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
                                                                                                            tnd2.Text = tnd2.Text + "\\" + ndVal12.nodeValue;
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    tnd2.Text = tnd2.Text + "\\" + ndVal11.nodeValue;
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            tnd2.Text = tnd2.Text + "\\" + ndVal10.nodeValue;
                                                                                        }
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    tnd2.Text = tnd2.Text + "\\" + ndVal9.nodeValue;
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            tnd2.Text = tnd2.Text + "\\" + ndVal8.nodeValue;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    tnd2.Text = tnd2.Text + "\\" + ndVal7.nodeValue;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            tnd2.Text = tnd2.Text + "\\" + ndVal6.nodeValue;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    tnd2.Text = tnd2.Text + "\\" + ndVal5.nodeValue;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            tnd2.Text = tnd2.Text + "\\" + ndVal4.nodeValue;
                                        }
                                    }
                                }
                                else
                                {
                                    tnd2.Text = tnd2.Text + "\\" + ndVal3.nodeValue;
                                }
                            }

                            tnd1.Nodes.Add(tnd2);
                        }
                        else
                        {
                            TreeNode tnd2 = new TreeNode(ndVal2.nodeValue);
                            tnd1.Nodes.Add(tnd2);
                        }
                    }

                    parentNode.Nodes.Add(tnd1);
                }
                else
                {
                    TreeNode tnd1 = new TreeNode(ndVal1.nodeValue);
                    parentNode.Nodes.Add(tnd1);
                }
            }

            return parentNode;
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
