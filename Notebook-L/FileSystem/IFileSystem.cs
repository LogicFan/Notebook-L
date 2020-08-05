using System;

namespace Notebook_L.FileSystem
{
    interface IFileSystem
    {       
        String Name { get; }

        IDirectory GetDirectory(String path);
        IFile GetFile(String path);
    }
}
