using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesforceMetadata
{
    public class AddObjectEvent : EventArgs
    {
        public AddObjectEvent(Boolean refreshForm, String nt, String[] fc) 
        {
            refreshParentForm = refreshForm;
            nodeType = nt;
            filesCreated = fc;
        }
        public Boolean refreshParentForm { get; private set; }
        public String nodeType { get; private set; }
        public String[] filesCreated { get; private set; }
    }
}
