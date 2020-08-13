using System;
using Windows.Storage;

namespace Notebook_L
{
    static class Constant
    {
        #region TabView_Main
        public const String HomePageTabId = "HomePage-BvlzpOeJ";
        public const String SettingPageTabId = "SettingPage-fkXRAjq1";

        public const String TabDataIdentifier = "iLoj4PTo";
        #endregion

        #region FileSystem
        public const String LocalFileSystemName = "Local";

        public static StorageFolder DocumentsLibrary => KnownFolders.DocumentsLibrary;
        public static StorageFolder LocalFolder => ApplicationData.Current.LocalFolder;
        public static StorageFolder TemporaryFolder => ApplicationData.Current.TemporaryFolder;
        public static StorageFolder RoamingFolder => ApplicationData.Current.RoamingFolder;
        #endregion
    }
}
