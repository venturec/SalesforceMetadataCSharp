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

namespace SalesforceMetadata
{
    public partial class EncryptDecryptText : Form
    {

        public Boolean textChanged = false;

        public EncryptDecryptText()
        {
            InitializeComponent();
            populateValues();
        }


        private void populateValues()
        {
            this.tbSharedSecret.Text = System.IO.File.ReadAllText(Properties.Settings.Default.SharedSecretLocation);
            this.tbSalt.Text = Properties.Settings.Default.Salt;
            this.tbEncryptedText.Text = System.IO.File.ReadAllText(Properties.Settings.Default.UserAndAPIFileLocation);
        }

        private void btnSaveResults_Click(object sender, EventArgs e)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Properties.Settings.Default.UserAndAPIFileLocation, false))
            {
                file.Write(this.tbEncryptedText.Text);
                this.textChanged = false;
            }
        }

        private void btnDecryptText_Click(object sender, EventArgs e)
        {
            if (this.tbEncryptedText.Text == "" || this.tbSharedSecret.Text == "" || this.tbSalt.Text == "")
            {
                MessageBox.Show("Please make sure the Shared Secret, Salt and " + this.lblEncryptedText.Text + " fields are populated before clicking the " + this.btnEncryptDecrypt.Text + " button.");
            }
            else
            {
                this.tbDecryptedText.Text = Crypto.DecryptString(this.tbEncryptedText.Text, this.tbSharedSecret.Text, this.tbSalt.Text);
            }
        }

        private void btnEncryptClick(object sender, EventArgs e)
        {
            if (this.tbDecryptedText.Text == "" || this.tbSharedSecret.Text == "" || this.tbSalt.Text == "")
            {
                MessageBox.Show("Please make sure the Shared Secret, Salt and " + this.lblEncryptedText.Text + " fields are populated before clicking the " + this.btnEncryptDecrypt.Text + " button.");
            }
            else
            {
                this.tbEncryptedText.Text = "";
                this.tbEncryptedText.Text = Crypto.EncryptString(this.tbDecryptedText.Text, this.tbSharedSecret.Text, this.tbSalt.Text);
            }
        }

        private void tbDecryptedText_TextChanged(object sender, EventArgs e)
        {
            this.textChanged = true;
        }

        private void EncryptDecryptText_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (textChanged == true)
            {
                DialogResult dr = MessageBox.Show("The decrypted text has changed. Would you like to save your changes?", "Save Changes Before Closing", MessageBoxButtons.YesNo);

                if (dr == DialogResult.OK || dr == DialogResult.Yes)
                {
                    this.tbEncryptedText.Text = "";
                    this.tbEncryptedText.Text = Crypto.EncryptString(this.tbDecryptedText.Text, this.tbSharedSecret.Text, this.tbSalt.Text);

                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(Properties.Settings.Default.UserAndAPIFileLocation, false))
                    {
                        file.Write(this.tbEncryptedText.Text);
                    }
                }
            }
        }
    }
}
