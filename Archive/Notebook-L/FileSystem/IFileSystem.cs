using System;
using System.Threading.Tasks;

namespace Notebook_L.FileSystem
{
    interface IFileSystem
    {       
        String Name { get; }

        Task<IDirectory> GetDirectory(String path);
        Task<IFile> GetFile(String path);
    }
}
