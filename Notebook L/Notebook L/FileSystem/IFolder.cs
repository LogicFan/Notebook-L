using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notebook_L.FileSystem
{
    public interface IFolder : IFileBase
    {
        Task<IEnumerable<IFolder>> GetFoldersAsync();
        Task<IEnumerable<IFile>> GetFilesAsync();

        Task<IFolder> CreateFolderAsync(String name);
        Task<IFile> CreateFileAsync(String name);
    }
}
