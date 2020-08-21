using MetroLog;
using Notebook_L.FileSystem.Detail;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Notebook_L.FileSystem
{
    static public class FileSystemFactory
    {
        private static readonly ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger(typeof(FileSystemFactory).Name);

        public static StorageFolder LocalFolder => ApplicationData.Current.LocalFolder;
        public static StorageFolder TemporaryFolder => ApplicationData.Current.TemporaryFolder;
        public static StorageFolder RoamingFolder => ApplicationData.Current.RoamingFolder;

        public async static Task<IFileSystem> CreateFileSystemAsync(FileSystemData data)
        {
            Log.Trace("CreateFileSystemAsync");

            switch (data.Source)
            {
                case FileSystemData.SourceType.Local:
                    return await LocalFileSystem.CreateFileSystemAsync(data);
                default:
                    throw new NotImplementedException();
            }
        }

        public async static Task<StorageFolder> CreateTemporaryFolderAsync()
        {
            Log.Trace("CreateTemporaryFolderAsync");

            while (true)
            {
                Guid guid = Guid.NewGuid();
                try
                {
                    return await TemporaryFolder.CreateFolderAsync(guid.ToString(), CreationCollisionOption.FailIfExists);
                }
                catch
                {
                    Log.Info("Nameing Collision on guid = {0}", guid.ToString());
                }
            }
        }
    }

    public class NameCollisionException : Exception
    {
        public NameCollisionException(String msg) : base(msg) { }
    }
}
