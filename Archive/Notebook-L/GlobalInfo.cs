using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Notebook_L
{
    static class GlobalInfo
    {
        public static readonly ApplicationDataContainer SettingContainer = ApplicationData.Current.LocalSettings.CreateContainer("Setting", ApplicationDataCreateDisposition.Always);
        public static readonly StorageFolder BaseStorageFolder = ApplicationData.Current.LocalFolder;
        public const String LocalFileSystemName = "LocalFileSystem-EkbBvmFCn9INFSYO";
    }
}
