using System;

namespace Notebook_L.Serializable
{
    [Serializable]
    struct Account
    {
        public enum SourceType
        {
            OneDrive,
            GoogleDrive,
            iCloudDrive,
            DropBox
        }

        public String Name;
        public String Credential;
        public SourceType Source;
    }
}
