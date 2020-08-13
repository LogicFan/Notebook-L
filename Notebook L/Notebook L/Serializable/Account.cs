using System;

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
