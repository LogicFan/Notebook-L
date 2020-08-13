using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notebook_L.FileSystem
{
    interface IFolder
    {
        String Name { get; }
        IFileSystem FileSystem { get; }
        String Path { get; }

        Task<IFolder> GetParentAsync();
        Task<IEnumerable<IFolder>> GetFoldersAsync();
        Task<IEnumerable<IFile>> GetFilesAsync();

        Task<IFolder> CreateFolderAsync(String name);
        Task<IFile> CreateFileAsync(String name);

        void DeleteAsync();
    }
}
