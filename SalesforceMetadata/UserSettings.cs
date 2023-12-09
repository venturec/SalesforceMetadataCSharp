using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesforceMetadata
{
    public partial class UserSettings : System.Windows.Forms.Form
    {

        public UserSettings()
        {
            InitializeComponent();
            getCurrentUserFileLocation();
            loadDefaultApis();
            loadDefaultTextEditorPath();
            loadOtherDefaults();
        }


        private void tbXmlFileLocation_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "xml files (*.xml)|*.xml|All Files (*.*)|*.*";
            ofd.Title = "Please select the credentials file";
            ofd.ShowDialog();

            this.tbXmlFileLocation.Text = ofd.FileName;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // If there are no errors returned, dispose of the User Settings form
            if (saveValuesToProperties() == false)
            {
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void getCurrentUserFileLocation()
        {
            this.tbXmlFileLocation.Text = Properties.Settings.Default.UserAndAPIFileLocation;
            this.tbSharedSecret.Text = Properties.Settings.Default.SharedSecretLocation;
            this.tbSalt.Text = Properties.Settings.Default.Salt;
        }

        private void tbSharedSecret_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "All Files (*.*)|*.*";
            ofd.Title = "Please select the Shared Secret file";
            ofd.ShowDialog();

            this.tbSharedSecret.Text = ofd.FileName;
        }

        private void encryptDecrypt_Click(object sender, EventArgs e)
        {
            if (saveValuesToProperties() == false)
            {
                DialogResult mbResult = MessageBox.Show("This form will show unencrypted values including passwords. Do you want to continue?", "Confirmation Message", 
                                                         MessageBoxButtons.OKCancel, 
                                                         MessageBoxIcon.Exclamation);
                if (mbResult == DialogResult.OK)
                {
                    EncryptDecryptText edt = new EncryptDecryptText();
                    edt.Show();
                }
            }
        }


        private Boolean saveValuesToProperties()
        {
            Boolean error = false;
            if (this.tbXmlFileLocation.Text == "")
            {
                error = true;
            }

            if (this.tbAsynchronousThreads.Text == "")
            {
                Properties.Settings.Default.MetadataAynchrounsThreads = 1;
            }
            else
            {
                Int32 txtToInt;
                if (Int32.TryParse(this.tbAsynchronousThreads.Text, out txtToInt) == true)
                {
                    Properties.Settings.Default.MetadataAynchrounsThreads = txtToInt;
                }
                else
                {
                    error = true;
                }
            }

            if (this.tbSharedSecret.Text == "")
            {
                error = true;
            }

            if (this.tbSalt.Text == "")
            {
                error = true;
            }

            if (error == false)
            {
                // Save the file location to the Default Settings
                Properties.Settings.Default.UserAndAPIFileLocation = this.tbXmlFileLocation.Text;
                Properties.Settings.Default.SharedSecretLocation = this.tbSharedSecret.Text;
                Properties.Settings.Default.Salt = this.tbSalt.Text;
                Properties.Settings.Default.DefaultAPI = this.cmbDefaultAPI.Text;
                Properties.Settings.Default.DefaultTextEditorPath = this.tbDefaultTextEditor.Text;
                Properties.Settings.Default.Save();

                SalesforceCredentials.populateUsernameMaps();
            }
            else
            {
                MessageBox.Show("Please populate the Credentials file location, Shared Secret Location and Salt OR correct the value in Metadata Retrieval Asynchronous Threads");
            }

            return error;
        }

        public void loadDefaultApis()
        {
            foreach (String api in UtilityClass.generateAPIArray())
            {
                this.cmbDefaultAPI.Items.Add(api);
            }

            this.cmbDefaultAPI.Text = Properties.Settings.Default.DefaultAPI;
        }

        public void loadDefaultTextEditorPath()
        {
            this.tbDefaultTextEditor.Text = Properties.Settings.Default.DefaultTextEditorPath;
        }

        public void loadOtherDefaults()
        {
            this.tbAsynchronousThreads.Text = Properties.Settings.Default.MetadataAynchrounsThreads.ToString();
        }

        private void tbDefaultTextEditor_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "exe files (*.exe)|*.exe|All Files (*.*)|*.*";
            ofd.Title = "Please select an executable file";
            ofd.ShowDialog();

            this.tbDefaultTextEditor.Text = ofd.FileName;

            Properties.Settings.Default.DefaultTextEditorPath = ofd.FileName;
            Properties.Settings.Default.Save();
        }
    }
}
