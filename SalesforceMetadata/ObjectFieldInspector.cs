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

using SalesforceMetadata.PartnerWSDL;
using SalesforceMetadata.MetadataWSDL;

namespace SalesforceMetadata
{
    public partial class ObjectFieldInspector : System.Windows.Forms.Form
    {
        public UtilityClass.REQUESTINGORG reqOrg;
        //private Dictionary<String, String> metadataXmlNameToFolder;
        private Dictionary<String, String> usernameToSecurityToken;

        private List<String> sObjectsList;
        private ListViewColumnSorter lvwColumnSorter;


        public ObjectFieldInspector()
        {
            InitializeComponent();
            lvwColumnSorter = new ListViewColumnSorter();
            this.listViewSobjectFields.ListViewItemSorter = lvwColumnSorter;
            populateCredentialsFile();
        }

        private void btnGetSobjectFields_Click(object sender, EventArgs e)
        {
            getSobjects();
        }

        private void cbSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            if (cb.Checked == true)
            {
                this.cbSelectNone.Checked = false;
                sobjectListBox.Items.Clear();
                foreach (String s in sObjectsList)
                {
                    sobjectListBox.Items.Add(s, true);
                }
            }
        }

        private void cbSelectNone_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if (cb.Checked == true)
            {
                this.cbSelectAll.Checked = false;
                sobjectListBox.Items.Clear();
                foreach (String s in sObjectsList)
                {
                    sobjectListBox.Items.Add(s, false);
                }
            }
        }

        private void getSobjects()
        {
            Boolean sfLoginSuccess = false;
            this.listViewSobjectFields.Items.Clear();
            sObjectsList = new List<string>();

            if (this.cmbUserName.Text == "" || this.tbPassword.Text == "")
            {
                MessageBox.Show("Please enter your credentials before continuing");
                return;
            }
            else
            {
                SalesforceCredentials.fromOrgUsername = this.cmbUserName.Text;
                SalesforceCredentials.fromOrgPassword = this.tbPassword.Text;
                SalesforceCredentials.fromOrgSecurityToken = this.tbSecurityToken.Text;
                sfLoginSuccess = SalesforceCredentials.salesforceLogin(UtilityClass.REQUESTINGORG.FROMORG);
            }

            if (sfLoginSuccess == false) return;

            DescribeGlobalResult dgr = SalesforceCredentials.fromOrgSS.describeGlobal();
            DescribeGlobalSObjectResult[] sObjGlobalResult = dgr.sobjects;

            foreach (DescribeGlobalSObjectResult dgsr in sObjGlobalResult)
            {
                this.cmbSalesforceSObjects.Items.Add(dgsr.name);
                this.sobjectListBox.Items.Add(dgsr.name);
                sObjectsList.Add(dgsr.name);
            }

            this.cmbSalesforceSObjects.Sorted = true;

            this.btnGetSobjects.Enabled = false;

        }

        private void populateListView(String sobjectName)
        {
            this.listViewSobjectFields.Clear();
            Boolean loginSuccess = SalesforceCredentials.salesforceLogin(UtilityClass.REQUESTINGORG.FROMORG);
            if (loginSuccess == false)
            {
                MessageBox.Show("Please check username, password and/or security token");
                return;
            }

            DescribeSObjectResult dsr = new DescribeSObjectResult();

            try
            {
                dsr = SalesforceCredentials.fromOrgSS.describeSObject(sobjectName);
            }
            catch (Exception e)
            {

            }

            // Now we need to retrieve the fields associated with those objects
            if (dsr.fields.Length > 0)
            {
                this.listViewSobjectFields.Columns.Add(new ColumnHeader());
                this.listViewSobjectFields.Columns[0].Text = "API Name";

                this.listViewSobjectFields.Columns.Add(new ColumnHeader());
                this.listViewSobjectFields.Columns[1].Text = "Label";

                this.listViewSobjectFields.Columns.Add(new ColumnHeader());
                this.listViewSobjectFields.Columns[2].Text = "Data Type";

                this.listViewSobjectFields.Columns.Add(new ColumnHeader());
                this.listViewSobjectFields.Columns[3].Text = "Length";

                this.listViewSobjectFields.Columns.Add(new ColumnHeader());
                this.listViewSobjectFields.Columns[4].Text = "Precision";

                this.listViewSobjectFields.Columns.Add(new ColumnHeader());
                this.listViewSobjectFields.Columns[5].Text = "Scale";

                this.listViewSobjectFields.Columns.Add(new ColumnHeader());
                this.listViewSobjectFields.Columns[6].Text = "Custom";

                this.listViewSobjectFields.Columns.Add(new ColumnHeader());
                this.listViewSobjectFields.Columns[7].Text = "Unique";

                this.listViewSobjectFields.Columns.Add(new ColumnHeader());
                this.listViewSobjectFields.Columns[8].Text = "Is Formula";

                this.listViewSobjectFields.Columns.Add(new ColumnHeader());
                this.listViewSobjectFields.Columns[9].Text = "Reference To";

                this.listViewSobjectFields.Columns.Add(new ColumnHeader());
                this.listViewSobjectFields.Columns[10].Text = "Reference Target Field";

                this.listViewSobjectFields.Columns.Add(new ColumnHeader());
                this.listViewSobjectFields.Columns[11].Text = "Relationship Name";

                this.listViewSobjectFields.Columns.Add(new ColumnHeader());
                this.listViewSobjectFields.Columns[12].Text = "External ID";

                this.listViewSobjectFields.Columns.Add(new ColumnHeader());
                this.listViewSobjectFields.Columns[13].Text = "Encrypted";

                this.listViewSobjectFields.Columns.Add(new ColumnHeader());
                this.listViewSobjectFields.Columns[14].Text = "Picklist Values";

                this.listViewSobjectFields.Columns.Add(new ColumnHeader());
                this.listViewSobjectFields.Columns[15].Text = "Multi-Select Picklist Values";

                // Example
                //ListViewItem listViewItem1 = new ListViewItem(new String[] { "Banana", "a", "b", "c" }, -1, Color.Empty, Color.Yellow, null);
                //ListViewItem listViewItem2 = new ListViewItem(new String[] { "Cherry", "v", "g", "t" }, -1, Color.Empty, Color.Red, new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((System.Byte)(0))));
                //ListViewItem listViewItem3 = new ListViewItem(new String[] { "Apple", "h", "j", "n" }, -1, Color.Empty, Color.Lime, null);
                //ListViewItem listViewItem4 = new ListViewItem(new String[] { "Pear", "y", "u", "i" }, -1, Color.Empty, Color.FromArgb(((System.Byte)(192)), ((System.Byte)(128)), ((System.Byte)(156))), null);

                Int32 i = 0;
                foreach (Field cf in dsr.fields)
                {
                    String referenceToObjects = "";
                    if (cf.referenceTo != null)
                    {
                        referenceToObjects = getReferenceToObjects(cf.referenceTo.ToList());
                    }

                    String fieldDataType = getFieldDataTypes(cf, cf.type);
                    String picklistValues = "";
                    String multiselectValues = "";


                    if (fieldDataType == "Picklist" && cf.picklistValues != null)
                    {
                        picklistValues = getPicklistValues(cf);
                    }

                    if (fieldDataType == "Multi-Select Picklist" && cf.picklistValues != null)
                    {
                        multiselectValues = getPicklistValues(cf);
                    }

                    if (i == 0)
                    {
                        ListViewItem lvi = new ListViewItem(new String[] { cf.name,
                                                                           cf.label,
                                                                           fieldDataType,
                                                                           cf.length.ToString(),
                                                                           cf.precision.ToString(),
                                                                           cf.scale.ToString(),
                                                                           cf.custom.ToString(),
                                                                           cf.unique.ToString(),
                                                                           cf.calculated.ToString(),
                                                                           referenceToObjects,
                                                                           cf.referenceTargetField,
                                                                           cf.relationshipName,
                                                                           cf.externalId.ToString(),
                                                                           cf.encrypted.ToString(),
                                                                           picklistValues,
                                                                           multiselectValues
                                                                          }, -1, Color.Empty, Color.AliceBlue, null);
                        this.listViewSobjectFields.Items.Add(lvi);
                        i++;
                    }
                    else if (i == 1)
                    {
                        ListViewItem lvi = new ListViewItem(new String[] {cf.name,
                                                                            cf.label,
                                                                            fieldDataType,
                                                                            cf.length.ToString(),
                                                                            cf.precision.ToString(),
                                                                            cf.scale.ToString(),
                                                                            cf.custom.ToString(),
                                                                            cf.unique.ToString(),
                                                                            cf.calculated.ToString(),
                                                                            referenceToObjects,
                                                                            cf.referenceTargetField,
                                                                            cf.relationshipName,
                                                                            cf.externalId.ToString(),
                                                                            cf.encrypted.ToString(),
                                                                            picklistValues,
                                                                            multiselectValues
                                                                           }, -1, Color.Empty, Color.AntiqueWhite, null);

                        this.listViewSobjectFields.Items.Add(lvi);
                        i = 0;
                    }
                }

                foreach (ColumnHeader ch in this.listViewSobjectFields.Columns)
                {
                    ch.Width = -2;
                }
            }
        }


        private Metadata[] readMetadata(String metadataFolderName, String[] sobjectArray)
        {
            Metadata[] sobjMetadata = SalesforceCredentials.fromOrgMS.readMetadata(metadataFolderName, sobjectArray);

            return sobjMetadata;
        }

        private void getSobjectFields_Click(object sender, EventArgs e)
        {
            populateListView(this.cmbSalesforceSObjects.SelectedItem.ToString());
        }

        private String getReferenceToObjects(List<String> objectList)
        {
            String referenceObjects = "";

            if (objectList.Count == 1)
            {
                referenceObjects = objectList[0];
            }
            else
            {
                foreach (String obj in objectList)
                {
                    referenceObjects = referenceObjects + obj + ",";
                }
            }

            return referenceObjects;
        }

        private String getFieldDataTypes(Field fld, fieldType ft)
        {
            String dataType = "";

            if (ft == fieldType.address)
            {
                dataType = "Address";
            }
            else if (ft == fieldType.anyType)
            {
                dataType = "Any Type";
            }
            else if (ft == fieldType.base64)
            {
                dataType = "Base 64";
            }
            else if (ft == fieldType.boolean)
            {
                dataType = "Boolean";
            }
            else if (ft == fieldType.combobox)
            {
                dataType = "Combobox";
            }
            else if (ft == fieldType.complexvalue)
            {
                dataType = "Complex Value";
            }
            else if (ft == fieldType.currency)
            {
                dataType = "Currency";
            }
            else if (ft == fieldType.datacategorygroupreference)
            {
                dataType = "Data Category Group Reference";
            }
            else if (ft == fieldType.date)
            {
                dataType = "Date";
            }
            else if (ft == fieldType.datetime)
            {
                dataType = "Date/Time";
            }
            else if (ft == fieldType.@double)
            {
                dataType = "Double";
            }
            else if (ft == fieldType.email)
            {
                dataType = "Email";
            }
            else if (ft == fieldType.encryptedstring)
            {
                dataType = "Encrypted String";
            }
            else if (ft == fieldType.id)
            {
                dataType = "Id";
            }
            else if (ft == fieldType.@int)
            {
                dataType = "Integer";
            }
            else if (ft == fieldType.location)
            {
                dataType = "Location";
            }
            else if (ft == fieldType.@long)
            {
                dataType = "Long";
            }
            else if (ft == fieldType.multipicklist)
            {
                dataType = "Multi-Select Picklist";
            }
            else if (ft == fieldType.percent)
            {
                dataType = "Percent";
            }
            else if (ft == fieldType.phone)
            {
                dataType = "Phone";
            }
            else if (ft == fieldType.picklist)
            {
                dataType = "Picklist";
            }
            else if (ft == fieldType.reference && fld.cascadeDelete == true)
            {
                dataType = "Master-Detail";
            }
            else if (ft == fieldType.reference && fld.cascadeDelete == false)
            {
                dataType = "Lookup";
            }
            else if (ft == fieldType.@string)
            {
                dataType = "String";
            }
            else if (ft == fieldType.textarea)
            {
                dataType = "Text-Area";
            }
            else if (ft == fieldType.time)
            {
                dataType = "Time";
            }
            else if (ft == fieldType.url)
            {
                dataType = "URL";
            }

            return dataType;
        }


        private String getPicklistValues(Field fld)
        {
            String picklistValues = "";

            foreach (SalesforceMetadata.PartnerWSDL.PicklistEntry pe in fld.picklistValues)
            {
                picklistValues += pe.label;
                picklistValues += ", ";
            }

            picklistValues = picklistValues.Substring(0, picklistValues.Length - 2);

            return picklistValues;
        }

        private void btnSaveFieldsToFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV|*.csv";
            sfd.FileName = this.cmbSalesforceSObjects.SelectedItem.ToString() + ".csv";
            sfd.Title = "Save Object Fields to File";
            sfd.ShowDialog();

            if (sfd.FileName != "")
            {
                StringBuilder sb = new StringBuilder();

                foreach (ColumnHeader ch in this.listViewSobjectFields.Columns)
                {
                    sb.Append(ch.Text + ",");
                }

                sb.Remove(sb.Length - 1, 1);
                sb.Append(Environment.NewLine);

                foreach (ListViewItem val in listViewSobjectFields.Items)
                {
                    //sb.Append(val.Text + ",");
                    foreach (ListViewItem.ListViewSubItem sub in val.SubItems)
                    {
                        sb.Append(sub.Text + ",");
                    }

                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(Environment.NewLine);
                }

                using (StreamWriter sw = new StreamWriter(sfd.FileName, false))
                {
                    sw.Write(sb.ToString());
                }
            }
        }


        private void populateCredentialsFile()
        {
            Boolean encryptionFileSettingsPopulated = true;
            if (Properties.Settings.Default.UserAndAPIFileLocation == ""
            || Properties.Settings.Default.SharedSecretLocation == "")
            {
                encryptionFileSettingsPopulated = false;
            }

            if (encryptionFileSettingsPopulated == false)
            {
                MessageBox.Show("Please populate the fields in the Settings from the Landing Page first, then use this form to download the Metadata.");
                return;
            }

            SalesforceCredentials.usernamePartnerUrl = new Dictionary<String, String>();
            SalesforceCredentials.usernameMetadataUrl = new Dictionary<String, String>();
            SalesforceCredentials.usernameToolingWsdlUrl = new Dictionary<String, String>();
            SalesforceCredentials.isProduction = new Dictionary<String, Boolean>();
            SalesforceCredentials.defaultWsdlObjects = new Dictionary<String, List<String>>();

            // Decrypt the contents of the file and place in an XML Document format
            StreamReader encryptedContents = new StreamReader(Properties.Settings.Default.UserAndAPIFileLocation);
            StreamReader sharedSecret = new StreamReader(Properties.Settings.Default.SharedSecretLocation);
            String decryptedContents = Crypto.DecryptString(encryptedContents.ReadToEnd(),
                                                            sharedSecret.ReadToEnd(),
                                                            Properties.Settings.Default.Salt);

            encryptedContents.Close();
            sharedSecret.Close();

            XmlDocument sfUser = new XmlDocument();
            sfUser.LoadXml(decryptedContents);

            XmlNodeList documentNodes = sfUser.GetElementsByTagName("usersetting");

            this.usernameToSecurityToken = new Dictionary<string, string>();

            for (int i = 0; i < documentNodes.Count; i++)
            {
                String username = "";
                //String enterpriseWsdlUrl = "";
                String partnerWsdlUrl = "";
                String metadataWdldUrl = "";
                String toolingWsdlUrl = "";
                Boolean isProd = false;
                List<String> defaultWsdlObjectList = new List<String>();
                foreach (XmlNode childNode in documentNodes[i].ChildNodes)
                {
                    if (childNode.Name == "username")
                    {
                        username = childNode.InnerText;
                    }

                    if (childNode.Name == "securitytoken")
                    {
                        usernameToSecurityToken.Add(username, childNode.InnerText);
                    }

                    if (childNode.Name == "isproduction")
                    {
                        isProd = Convert.ToBoolean(childNode.InnerText);
                    }

                    if (childNode.Name == "partnerwsdlurl")
                    {
                        partnerWsdlUrl = childNode.InnerText;
                    }

                    if (childNode.Name == "metadatawsdlurl")
                    {
                        metadataWdldUrl = childNode.InnerText;
                    }

                    if (childNode.Name == "toolingwsdlurl")
                    {
                        toolingWsdlUrl = childNode.InnerText;
                    }

                    if (childNode.Name == "defaultpackages" && childNode.HasChildNodes)
                    {
                        XmlNodeList defObjects = childNode.ChildNodes;
                        foreach (XmlNode obj in defObjects)
                        {
                            defaultWsdlObjectList.Add(obj.InnerText);
                        }
                    }
                }

                SalesforceCredentials.usernamePartnerUrl.Add(username, partnerWsdlUrl);
                SalesforceCredentials.usernameMetadataUrl.Add(username, metadataWdldUrl);
                SalesforceCredentials.isProduction.Add(username, isProd);

                if (defaultWsdlObjectList.Count > 0)
                {
                    SalesforceCredentials.defaultWsdlObjects.Add(username, defaultWsdlObjectList);
                }

                if (toolingWsdlUrl != "")
                {
                    SalesforceCredentials.usernameToolingWsdlUrl.Add(username, toolingWsdlUrl);
                }
            }

            populateUserNames();
        }

        private void populateUserNames()
        {
            foreach (String un in SalesforceCredentials.usernamePartnerUrl.Keys)
            {
                this.cmbUserName.Items.Add(un);
            }
        }

        private void btnSaveObjectsToFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV|*.csv";
            sfd.FileName =  "SobjectList.csv";
            sfd.Title = "Save sObjects to File";
            sfd.ShowDialog();

            if (sfd.FileName != "")
            {
                StringBuilder sb = new StringBuilder();

                foreach (String sObj in cmbSalesforceSObjects.Items)
                {
                    sb.Append(sObj + Environment.NewLine);
                }

                using (StreamWriter sw = new StreamWriter(sfd.FileName, false))
                {
                    sw.Write(sb.ToString());
                }
            }
        }

        private void btnSaveSelectedToExcel_Click(object sender, EventArgs evt)
        {
            if (sobjectListBox.CheckedItems.Count > 0)
            {
                Boolean loginSuccess = SalesforceCredentials.salesforceLogin(UtilityClass.REQUESTINGORG.FROMORG);

                if (loginSuccess == false)
                {
                    MessageBox.Show("Please check username, password and/or security token");
                    return;
                }

                Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
                xlapp.Visible = false;

                Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlapp.Workbooks.Add();

                foreach (String sobj in sobjectListBox.CheckedItems)
                {
                    DescribeSObjectResult dsr = new DescribeSObjectResult();

                    try
                    {
                        dsr = SalesforceCredentials.fromOrgSS.describeSObject(sobj);

                        Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                                    (System.Reflection.Missing.Value,
                                                                                     xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                                     System.Reflection.Missing.Value,
                                                                                     System.Reflection.Missing.Value);
                        xlWorksheet.Name = tabNameLengthCheck(sobj);

                        xlWorksheet.Cells[1, 1].Value = "API Name";
                        xlWorksheet.Cells[1, 2].Value = "Label";
                        xlWorksheet.Cells[1, 3].Value = "Data Type";
                        xlWorksheet.Cells[1, 4].Value = "Length";
                        xlWorksheet.Cells[1, 5].Value = "Precision";
                        xlWorksheet.Cells[1, 6].Value = "Scale";
                        xlWorksheet.Cells[1, 7].Value = "Custom";
                        xlWorksheet.Cells[1, 8].Value = "Unique";
                        xlWorksheet.Cells[1, 9].Value = "Default Value";
                        xlWorksheet.Cells[1, 10].Value = "Default Value Formula";
                        xlWorksheet.Cells[1, 11].Value = "Cascade Delete";
                        xlWorksheet.Cells[1, 12].Value = "Cascade Delete Specified";
                        xlWorksheet.Cells[1, 13].Value = "Is Formula";
                        xlWorksheet.Cells[1, 14].Value = "Calculated Formula";
                        xlWorksheet.Cells[1, 15].Value = "Treat Null Number As Zero";
                        xlWorksheet.Cells[1, 16].Value = "Treat Null Number As Zero Specified";
                        xlWorksheet.Cells[1, 17].Value = "Reference To";
                        xlWorksheet.Cells[1, 18].Value = "Reference Target Field";
                        xlWorksheet.Cells[1, 19].Value = "Relationship Name";
                        xlWorksheet.Cells[1, 20].Value = "External ID";
                        xlWorksheet.Cells[1, 21].Value = "Encrypted";
                        xlWorksheet.Cells[1, 22].Value = "Picklist Values";
                        xlWorksheet.Cells[1, 23].Value = "Dependent Picklist";
                        xlWorksheet.Cells[1, 24].Value = "Dependent Picklist Specified";
                        xlWorksheet.Cells[1, 25].Value = "Multi-Select Picklist Values";

                        Microsoft.Office.Interop.Excel.Range rng;
                        rng = xlWorksheet.Range[xlWorksheet.Cells[1, 1], xlWorksheet.Cells[1, 25]];
                        rng.Font.Bold = true;
                        rng.Font.Size = 14;
                        rng.Font.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbFloralWhite;
                        rng.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbRoyalBlue;

                        Int32 i = 0;
                        Int32 rowNumber = 2;
                        foreach (Field cf in dsr.fields)
                        {
                            String referenceToObjects = "";
                            if (cf.referenceTo != null)
                            {
                                referenceToObjects = getReferenceToObjects(cf.referenceTo.ToList());
                            }

                            String fieldDataType = getFieldDataTypes(cf, cf.type);
                            String picklistValues = "";
                            String multiselectValues = "";

                            if (fieldDataType == "Picklist" && cf.picklistValues != null)
                            {
                                picklistValues = getPicklistValues(cf);
                            }

                            if (fieldDataType == "Multi-Select Picklist" && cf.picklistValues != null)
                            {
                                multiselectValues = getPicklistValues(cf);
                            }

                            if (i == 0)
                            {
                                xlWorksheet.Cells[rowNumber, 1].Value = cf.name;
                                xlWorksheet.Cells[rowNumber, 2].Value = cf.label;
                                xlWorksheet.Cells[rowNumber, 3].Value = fieldDataType;
                                xlWorksheet.Cells[rowNumber, 4].Value = cf.length.ToString();
                                xlWorksheet.Cells[rowNumber, 5].Value = cf.precision.ToString();
                                xlWorksheet.Cells[rowNumber, 6].Value = cf.scale.ToString();
                                xlWorksheet.Cells[rowNumber, 7].Value = cf.custom.ToString();
                                xlWorksheet.Cells[rowNumber, 8].Value = cf.unique.ToString();
                                xlWorksheet.Cells[rowNumber, 9].Value = cf.defaultValue;
                                xlWorksheet.Cells[rowNumber, 10].Value = cf.defaultValueFormula;
                                xlWorksheet.Cells[rowNumber, 11].Value = cf.cascadeDelete;
                                xlWorksheet.Cells[rowNumber, 12].Value = cf.cascadeDeleteSpecified;
                                xlWorksheet.Cells[rowNumber, 13].Value = cf.calculated.ToString();
                                xlWorksheet.Cells[rowNumber, 14].Value = cf.calculatedFormula;
                                xlWorksheet.Cells[rowNumber, 15].Value = cf.formulaTreatNullNumberAsZero;
                                xlWorksheet.Cells[rowNumber, 16].Value = cf.formulaTreatNullNumberAsZeroSpecified;
                                xlWorksheet.Cells[rowNumber, 17].Value = referenceToObjects;
                                xlWorksheet.Cells[rowNumber, 18].Value = cf.referenceTargetField;
                                xlWorksheet.Cells[rowNumber, 19].Value = cf.relationshipName;
                                xlWorksheet.Cells[rowNumber, 20].Value = cf.externalId.ToString();
                                xlWorksheet.Cells[rowNumber, 21].Value = cf.encrypted.ToString();
                                xlWorksheet.Cells[rowNumber, 22].Value = picklistValues;
                                xlWorksheet.Cells[rowNumber, 23].Value = cf.dependentPicklist;
                                xlWorksheet.Cells[rowNumber, 24].Value = cf.dependentPicklistSpecified;
                                xlWorksheet.Cells[rowNumber, 25].Value = multiselectValues;

                                rng = xlWorksheet.Range[xlWorksheet.Cells[rowNumber, 1], xlWorksheet.Cells[rowNumber, 25]];
                                rng.Font.Size = 11;

                                Microsoft.Office.Interop.Excel.Borders border = rng.Borders;
                                border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                                border.Weight = 1d;
                                border.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbLightGray;

                                i++;
                                rowNumber++;

                            }
                            else if (i == 1)
                            {
                                xlWorksheet.Cells[rowNumber, 1].Value = cf.name;
                                xlWorksheet.Cells[rowNumber, 2].Value = cf.label;
                                xlWorksheet.Cells[rowNumber, 3].Value = fieldDataType;
                                xlWorksheet.Cells[rowNumber, 4].Value = cf.length.ToString();
                                xlWorksheet.Cells[rowNumber, 5].Value = cf.precision.ToString();
                                xlWorksheet.Cells[rowNumber, 6].Value = cf.scale.ToString();
                                xlWorksheet.Cells[rowNumber, 7].Value = cf.custom.ToString();
                                xlWorksheet.Cells[rowNumber, 8].Value = cf.unique.ToString();
                                xlWorksheet.Cells[rowNumber, 9].Value = cf.defaultValue;
                                xlWorksheet.Cells[rowNumber, 10].Value = cf.defaultValueFormula;
                                xlWorksheet.Cells[rowNumber, 11].Value = cf.cascadeDelete;
                                xlWorksheet.Cells[rowNumber, 12].Value = cf.cascadeDeleteSpecified;
                                xlWorksheet.Cells[rowNumber, 13].Value = cf.calculated.ToString();
                                xlWorksheet.Cells[rowNumber, 14].Value = cf.calculatedFormula;
                                xlWorksheet.Cells[rowNumber, 15].Value = cf.formulaTreatNullNumberAsZero;
                                xlWorksheet.Cells[rowNumber, 16].Value = cf.formulaTreatNullNumberAsZeroSpecified;
                                xlWorksheet.Cells[rowNumber, 17].Value = referenceToObjects;
                                xlWorksheet.Cells[rowNumber, 18].Value = cf.referenceTargetField;
                                xlWorksheet.Cells[rowNumber, 19].Value = cf.relationshipName;
                                xlWorksheet.Cells[rowNumber, 20].Value = cf.externalId.ToString();
                                xlWorksheet.Cells[rowNumber, 21].Value = cf.encrypted.ToString();
                                xlWorksheet.Cells[rowNumber, 22].Value = picklistValues;
                                xlWorksheet.Cells[rowNumber, 23].Value = cf.dependentPicklist;
                                xlWorksheet.Cells[rowNumber, 24].Value = cf.dependentPicklistSpecified;
                                xlWorksheet.Cells[rowNumber, 25].Value = multiselectValues;

                                rng = xlWorksheet.Range[xlWorksheet.Cells[rowNumber, 1], xlWorksheet.Cells[rowNumber, 25]];
                                rng.Font.Size = 11;

                                Microsoft.Office.Interop.Excel.Borders border = rng.Borders;
                                border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                                border.Weight = 1d;
                                border.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbLightGray;

                                i = 0;
                                rowNumber++;
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc.Message);
                    }

                }

                xlapp.Visible = true;
            }
        }

        private void cmbUserName_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.tbSecurityToken.Text = "";
            if (this.usernameToSecurityToken.ContainsKey(this.cmbUserName.Text))
            {
                this.tbSecurityToken.Text = this.usernameToSecurityToken[cmbUserName.Text];
            }

            this.btnGetSobjects.Enabled = true;
            this.cmbSalesforceSObjects.Items.Clear();
            this.sobjectListBox.Items.Clear();
        }

        private void btnGetReferenceFields_Click(object sender, EventArgs e)
        {
            if (sobjectListBox.CheckedItems.Count > 0)
            {
                Boolean loginSuccess = SalesforceCredentials.salesforceLogin(UtilityClass.REQUESTINGORG.FROMORG);

                if (loginSuccess == false)
                {
                    MessageBox.Show("Please check username, password and/or security token");
                    return;
                }

                Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
                xlapp.Visible = true;

                Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlapp.Workbooks.Add();


                Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                            (System.Reflection.Missing.Value,
                                                                             xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                             System.Reflection.Missing.Value,
                                                                             System.Reflection.Missing.Value);

                xlWorksheet.Name = "Sobjects With Reference Fields";

                xlWorksheet.Cells[1, 1].Value = "Sobject Name";
                xlWorksheet.Cells[1, 2].Value = "API Name";
                xlWorksheet.Cells[1, 3].Value = "Label";
                xlWorksheet.Cells[1, 4].Value = "Data Type";
                xlWorksheet.Cells[1, 5].Value = "Reference To";
                xlWorksheet.Cells[1, 6].Value = "Reference Target Field";
                xlWorksheet.Cells[1, 7].Value = "Relationship Name";

                Microsoft.Office.Interop.Excel.Range rng;
                rng = xlWorksheet.Range[xlWorksheet.Cells[1, 1], xlWorksheet.Cells[1, 7]];
                rng.Font.Bold = true;
                rng.Font.Size = 14;
                rng.Font.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbFloralWhite;
                rng.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbRoyalBlue;


                Int32 i = 0;
                Int32 rowNumber = 2;

                foreach (String sobj in sobjectListBox.CheckedItems)
                {
                    DescribeSObjectResult dsr = new DescribeSObjectResult();

                    try
                    {
                        dsr = SalesforceCredentials.fromOrgSS.describeSObject(sobj);

                        foreach (Field cf in dsr.fields)
                        {
                            String referenceToObjects = "";
                            if (cf.referenceTo != null)
                            {
                                referenceToObjects = getReferenceToObjects(cf.referenceTo.ToList());
                            }

                            String fieldDataType = getFieldDataTypes(cf, cf.type);

                            if (cf.type != fieldType.reference) continue;

                            if (i == 0)
                            {
                                xlWorksheet.Cells[rowNumber, 1].Value = sobj;
                                xlWorksheet.Cells[rowNumber, 2].Value = cf.name;
                                xlWorksheet.Cells[rowNumber, 3].Value = cf.label;
                                xlWorksheet.Cells[rowNumber, 4].Value = fieldDataType;
                                xlWorksheet.Cells[rowNumber, 5].Value = referenceToObjects;
                                xlWorksheet.Cells[rowNumber, 6].Value = cf.referenceTargetField;
                                xlWorksheet.Cells[rowNumber, 7].Value = cf.relationshipName;

                                rng = xlWorksheet.Range[xlWorksheet.Cells[rowNumber, 1], xlWorksheet.Cells[rowNumber, 7]];
                                rng.Font.Size = 11;
                                //rng.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbAliceBlue;

                                Microsoft.Office.Interop.Excel.Borders border = rng.Borders;
                                border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                                border.Weight = 1d;
                                border.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbLightGray;

                                i++;
                                rowNumber++;

                            }
                            else if (i == 1)
                            {
                                xlWorksheet.Cells[rowNumber, 1].Value = sobj;
                                xlWorksheet.Cells[rowNumber, 2].Value = cf.name;
                                xlWorksheet.Cells[rowNumber, 3].Value = cf.label;
                                xlWorksheet.Cells[rowNumber, 4].Value = fieldDataType;
                                xlWorksheet.Cells[rowNumber, 5].Value = referenceToObjects;
                                xlWorksheet.Cells[rowNumber, 6].Value = cf.referenceTargetField;
                                xlWorksheet.Cells[rowNumber, 7].Value = cf.relationshipName;


                                rng = xlWorksheet.Range[xlWorksheet.Cells[rowNumber, 1], xlWorksheet.Cells[rowNumber, 7]];
                                rng.Font.Size = 11;
                                //rng.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbPaleTurquoise;

                                Microsoft.Office.Interop.Excel.Borders border = rng.Borders;
                                border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                                border.Weight = 1d;
                                border.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbLightGray;

                                i = 0;
                                rowNumber++;
                            }
                        }
                    }
                    catch (Exception exc)
                    {

                    }
                }

                xlapp.Visible = true;

                MessageBox.Show("Field extract complete");

            }
        }

        private void listViewSobjectFields_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == System.Windows.Forms.SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = System.Windows.Forms.SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = System.Windows.Forms.SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = System.Windows.Forms.SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.listViewSobjectFields.Sort();

            Int32 i = 0;
            foreach (ListViewItem lvi in this.listViewSobjectFields.Items)
            {
                if (i == 0)
                {
                    lvi.BackColor = Color.AliceBlue;
                    i++;
                }
                else if (i == 1)
                {
                    lvi.BackColor = Color.AntiqueWhite;
                    i = 0;
                }
            }

        }

        private void cbCustomObjectsOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbCustomObjectsOnly.Checked == true)
            {
                this.cmbSalesforceSObjects.Items.Clear();
                this.sobjectListBox.Items.Clear();

                foreach (String sobjName in this.sObjectsList)
                {
                    // Example:
                    // Ad_Campaign__c - the split should be 2
                    // rh2__Job__c - the split shoudl be 3 since this is a managed package and has a namespace
                    String[] sobjNameSplit = sobjName.Split(new String[] { "__" }, StringSplitOptions.None);

                    if (sobjNameSplit.Length == 2 
                        && sobjName.EndsWith("__c"))
                    {
                        this.cmbSalesforceSObjects.Items.Add(sobjName);
                        this.sobjectListBox.Items.Add(sobjName);
                    }
                }
            }
            else if (this.cbCustomObjectsOnly.Checked == false)
            {
                this.cmbSalesforceSObjects.Items.Clear();
                this.sobjectListBox.Items.Clear();

                foreach (String sobjName in this.sObjectsList)
                {
                    this.cmbSalesforceSObjects.Items.Add(sobjName);
                    this.sobjectListBox.Items.Add(sobjName);
                }
            }
        }

        private void cbFilterManagedPkg_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbFilterManagedPkg.Checked == true)
            {
                this.cmbSalesforceSObjects.Items.Clear();
                this.sobjectListBox.Items.Clear();

                foreach (String sobjName in this.sObjectsList)
                {
                    // Example:
                    // Ad_Campaign__c - the split should be 2
                    // rh2__Job__c - the split shoudl be 3 since this is a managed package and has a namespace
                    String[] sobjNameSplit = sobjName.Split(new String[] { "__" }, StringSplitOptions.None);

                    if (sobjNameSplit.Length == 3)
                    {
                        this.cmbSalesforceSObjects.Items.Add(sobjName);
                        this.sobjectListBox.Items.Add(sobjName);
                    }
                }
            }
            else if (this.cbFilterManagedPkg.Checked == false)
            {
                this.cmbSalesforceSObjects.Items.Clear();
                this.sobjectListBox.Items.Clear();

                foreach (String sobjName in this.sObjectsList)
                {
                    this.cmbSalesforceSObjects.Items.Add(sobjName);
                    this.sobjectListBox.Items.Add(sobjName);
                }
            }
        }

        private String tabNameLengthCheck(String potentialName)
        {
            String reducedName = potentialName;

            if (reducedName.Length > 30)
            {
                reducedName = reducedName.Substring(0, 28);
                reducedName = reducedName + "_1";
            }

            return reducedName;
        }

        private void cmbSalesforceSObjects_SelectedValueChanged(object sender, EventArgs e)
        {
            populateListView(this.cmbSalesforceSObjects.SelectedItem.ToString());
        }
    }
}
