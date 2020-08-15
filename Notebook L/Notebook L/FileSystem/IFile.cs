using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Notebook_L.FileSystem
{
    interface IFile : IFileItem
    {
        IFileSystem FileSystem { get; }
        
        Task<IFolder> GetParentAsync();

        Task<StorageFile> GetFileAsync(StorageFolder target);
        void SetFileAsync(StorageFile source);

        void DeleteAsync();
    }
}
