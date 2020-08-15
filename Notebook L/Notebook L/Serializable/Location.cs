using System;

namespace Notebook_L.Serializable
{
    struct Location
    {
        public enum SourceType
        {
            Local,
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
