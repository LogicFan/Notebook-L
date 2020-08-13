using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notebook_L.Serializable
{
    struct Account
    {
        public enum SourceType
        {
            OneDrive,
            GoogleDrive,
            iCloudDrive,
            Dropbox
        }

        public String Name;
        public String Credential;
        public SourceType Source;
    }
}
