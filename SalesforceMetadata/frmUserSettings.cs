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
    public partial class frmUserSettings : System.Windows.Forms.Form
    {

        public frmUserSettings()
        {
            InitializeComponent();
            getCurrentUserFileLocation();
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
                Properties.Settings.Default.Save();
            }
            else
            {
                MessageBox.Show("Please populate the Credentials file location, Shared Secret Location and Salt first before continuing");
            }


            return error;
        }
    }
}
