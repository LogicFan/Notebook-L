using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace Notebook_L.FileSystem
{
    static class Util
    {
        public static async Task<StorageFolder> CreateTempDirectory()
        {
            String id = Guid.NewGuid().ToString();
            return await GlobalInfo.BaseStorageFolder.CreateFolderAsync("Temporary-" + id);
        }
    }
}