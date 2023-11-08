using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SalesforceMetadata
{
    public partial class ExtractFieldsFromMetadata : Form
    {
        public ExtractFieldsFromMetadata()
        {
            InitializeComponent();
        }

        private void tbSelectedFolder_DoubleClick(object sender, EventArgs e)
        {
            this.tbSelectedFolder.Text = UtilityClass.folderBrowserSelectPath("Select folder to read the Object items from", true, FolderEnum.SaveTo);
        }

        private void btnExtractFields_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
            xlapp.Visible = true;

            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlapp.Workbooks.Add();

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                    (System.Reflection.Missing.Value,
                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                        System.Reflection.Missing.Value,
                                                                        System.Reflection.Missing.Value);

            xlWorksheet.Name = "Object Fields";

            Int32 rowStart = 1;

            xlWorksheet.Cells[rowStart, 1].Value = "ObjectName";
            xlWorksheet.Cells[rowStart, 2].Value = "FieldApiName";
            xlWorksheet.Cells[rowStart, 3].Value = "FieldLabel";
            xlWorksheet.Cells[rowStart, 4].Value = "Type";
            xlWorksheet.Cells[rowStart, 5].Value = "Length";
            xlWorksheet.Cells[rowStart, 6].Value = "Precision";
            xlWorksheet.Cells[rowStart, 7].Value = "Scale";
            xlWorksheet.Cells[rowStart, 8].Value = "ReferenceTo";
            xlWorksheet.Cells[rowStart, 9].Value = "RelationshipName";
            xlWorksheet.Cells[rowStart, 10].Value = "RelationshipLabel";
            xlWorksheet.Cells[rowStart, 11].Value = "DeleteConstraint";
            xlWorksheet.Cells[rowStart, 12].Value = "Required";
            xlWorksheet.Cells[rowStart, 13].Value = "Unique";
            xlWorksheet.Cells[rowStart, 14].Value = "ExternalId";

            rowStart++;

            String[] files = Directory.GetFiles(this.tbSelectedFolder.Text);

            foreach (String file in files)
            {
                XmlDocument xd = new XmlDocument();
                xd.Load(file);

                XmlNodeList fieldNodeList = xd.GetElementsByTagName("fields");

                String[] filePathSplit = file.Split('\\');
                String[] fileNameSplit = filePathSplit[filePathSplit.Length - 1].Split('.');

                foreach (XmlNode nd1 in fieldNodeList)
                {
                    xlWorksheet.Cells[rowStart, 1].Value = fileNameSplit[0];

                    if (nd1.ParentNode.Name == "CustomObject")
                    {
                        foreach (XmlNode nd2 in nd1.ChildNodes)
                        {
                            if (nd2.Name == "fullName")
                            {
                                xlWorksheet.Cells[rowStart, 2].Value = nd2.InnerText;
                            }
                            else if (nd2.Name == "label")
                            {
                                xlWorksheet.Cells[rowStart, 3].Value = nd2.InnerText;
                            }
                            else if (nd2.Name == "type")
                            {
                                xlWorksheet.Cells[rowStart, 4].Value = nd2.InnerText;
                            }
                            else if (nd2.Name == "length")
                            {
                                xlWorksheet.Cells[rowStart, 5].Value = nd2.InnerText;
                            }
                            else if (nd2.Name == "precision")
                            {
                                xlWorksheet.Cells[rowStart, 6].Value = nd2.InnerText;
                            }
                            else if (nd2.Name == "scale")
                            {
                                xlWorksheet.Cells[rowStart, 7].Value = nd2.InnerText;
                            }
                            else if (nd2.Name == "referenceTo")
                            {
                                xlWorksheet.Cells[rowStart, 8].Value = nd2.InnerText;
                            }
                            else if (nd2.Name == "relationshipName")
                            {
                                xlWorksheet.Cells[rowStart, 9].Value = nd2.InnerText;
                            }
                            else if (nd2.Name == "relationshipLabel")
                            {
                                xlWorksheet.Cells[rowStart, 10].Value = nd2.InnerText;
                            }
                            else if (nd2.Name == "deleteConstraint")
                            {
                                xlWorksheet.Cells[rowStart, 11].Value = nd2.InnerText;
                            }
                            else if (nd2.Name == "required")
                            {
                                xlWorksheet.Cells[rowStart, 12].Value = nd2.InnerText;
                            }
                            else if (nd2.Name == "unique")
                            {
                                xlWorksheet.Cells[rowStart, 13].Value = nd2.InnerText;
                            }
                            else if (nd2.Name == "externalId")
                            {
                                xlWorksheet.Cells[rowStart, 14].Value = nd2.InnerText;
                            }
                        }

                        rowStart++;
                    }
                }
            }

            xlapp.Visible = true;
        }
    }
}
