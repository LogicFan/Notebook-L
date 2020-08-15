using System;
using MetroLog;
using Notebook_L.Serializable;

namespace Notebook_L.FileSystem
{
    static class FileSystemFactory
    {
        private static readonly ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger(typeof(FileSystemFactory).Name);

        public static IFileSystem CreateFileSystem(Location location)
        {
            Log.Info("CreateFileSystem");

            switch (location.Source)
            {
                case Location.SourceType.Local:
                    return new LocalFileSystem(location);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
