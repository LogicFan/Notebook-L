using System;

namespace Notebook_L.Serializable
{
    [Serializable]
    struct Location
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
