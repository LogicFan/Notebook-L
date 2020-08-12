using MetroLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Notebook_L.FileSystem
{
    static class FileSystemFactory
    {
        public static IEnumerable<IFileSystem> FileSystems
        {
            get => Setting.Setting.Locations.Select(e => CreateFileSystem(e)).Append(CreateFileSystem(null));
        }

        public static IFileSystem CreateFileSystem(Location.Location location)
        {
            if (location == null)
            {
                return new LocalFileSystem(GlobalInfo.LocalFileSystemName);
            }

            switch (location.Data.Source)
            {
                case Serializable.Location.SourceType.OneDrive:
                    throw new NotImplementedException();
                case Serializable.Location.SourceType.GoogleDrive:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
