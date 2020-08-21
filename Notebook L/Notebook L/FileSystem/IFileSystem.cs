using System;

namespace Notebook_L.FileSystem
{
    public interface IFileSystem
    {
        FileSystemData Data { get; }
        String RootPath { get; }

        IFolder GetRootFolder();
    }
}
