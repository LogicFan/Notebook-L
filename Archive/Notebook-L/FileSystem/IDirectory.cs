using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notebook_L.FileSystem
{
    interface IDirectory
    {
        // The name of directory
        String Name { get; }

        // Underlying file system and the path
        IFileSystem FileSystem { get; }
        String Path { get; }

        // Access parent directory, sub-directories and files inside the directory
        Task<IDirectory> GetParent();
        Task<IEnumerable<IDirectory>> GetDirectories();
        Task<IEnumerable<IFile>> GetFiles();

        // Create the file or sub-directory
        Task<IDirectory> CreateDirectory(String name);
        Task<IFile> CreateFile(String name);
        
        // Delete this directory, after this call, the object become invalid.
        void Delete();
    }
}
