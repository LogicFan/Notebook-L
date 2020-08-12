using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Notebook_L.FileSystem
{
    interface IFile
    {
        // The name of file
        String Name { get; }

        // Underlying file system and the path
        IFileSystem FileSystem { get; }
        String Path { get; }

        // Access the parent directory
        Task<IDirectory> GetParent();

        // Download to local waiting for modification, acquire the lock
        Task<StorageFile> Download();

        // Upload to underlying file system, release the lock
        void Upload();

        // Delete this file, after this call, the object become invalid.
        void Delete();
    }
}
