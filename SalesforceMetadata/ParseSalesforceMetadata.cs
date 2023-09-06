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
            String folderPath = "C:\\Users\\marcu\\Documents\\Projects\\SO Asher\\lionheartsystemsinc__elmsdev\\unpackaged\\objects";

            SalesforceMetadataStep2 sfm = new SalesforceMetadataStep2();
            sfm.addVSCodeFileExtension(folderPath);
        }
    }
}
