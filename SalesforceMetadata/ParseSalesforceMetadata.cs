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
    public partial class ParseSalesforceMetadata : Form
    {
        public ParseSalesforceMetadata()
        {
            InitializeComponent();
        }

        private void btnParseMetadata_Click(object sender, EventArgs e)
        {
            if (this.tbMetadataFolderPath.Text == "")
            {
                MessageBox.Show("Please select a path to the metadata folder");
                return;
            }

            //String folderPath = this.tbMetadataFolderPath.Text;

            SalesforceMetadataStep2 sfm = new SalesforceMetadataStep2();
            sfm.addVSCodeFileExtension(this.tbMetadataFolderPath.Text);
        }

        private void tbMetadataFolderPath_DoubleClick(object sender, EventArgs e)
        {
            this.tbMetadataFolderPath.Text = UtilityClass.folderBrowserSelectPath("Select the Metadata Directory to Parse",
                                                                                  false,
                                                                                  FolderEnum.SaveTo,
                                                                                  Properties.Settings.Default.ParseMetadataFolderaPath);
        }
    }
}
