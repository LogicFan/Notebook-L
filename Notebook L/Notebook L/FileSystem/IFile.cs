using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Notebook_L.FileSystem
{
    interface IFile
    {
        String Name { get; }
        IFileSystem FileSystem { get; }
        String Path { get; }

        Task<IFolder> GetParentAsync();

        Task<StorageFile> GetFileAsync();
        void SetFileAsync(Task<StorageFile> file);

        void DeleteAsync();
    }
}
