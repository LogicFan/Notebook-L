using Notebook_L.Serializable;
using Notebook_L;
using System;
using Windows.Storage;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using MetroLog;

namespace Notebook_L.FileSystem
{
    class LocalFileSystem : IFileSystem
    {
        private static readonly ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger<LocalFileSystem>();

        public Nullable<Account> Data => null;
        public String Name => Constant.LocalFileSystemName;

        public LocalFileSystem()
        {
            Log.Info(String.Format("Create object LocalFileSystem@{0:X8}", this.GetHashCode()));
        }

        public async Task<IFolder> GetRootFolderAsync()
        {
            Log.Info(String.Format("{0:X8}: GetRootFolderAsync", this.GetHashCode()));

            StorageFolder root = await Constant.LocalFolder.CreateFolderAsync("Notebook-L", CreationCollisionOption.OpenIfExists);
            return new LocalFolder(root, this);
        }
    }

    class LocalFolder : IFolder
    {
        private readonly StorageFolder Folder;
        
        public String Name => Folder.Name;
        public IFileSystem FileSystem { get; }
        public String Path => Folder.Path;

        public LocalFolder(StorageFolder folder, IFileSystem fileSystem)
        {
            Folder = folder;
            FileSystem = fileSystem;
        }

        public async Task<IFolder> GetParentAsync()
        {
            return new LocalFolder(await Folder.GetParentAsync(), FileSystem);
        }
        public async Task<IEnumerable<IFolder>> GetFoldersAsync()
        {
            return (await Folder.GetFoldersAsync()).Select(e => new LocalFolder(e, FileSystem));
        }
        public async Task<IEnumerable<IFile>> GetFilesAsync()
        {
            return (await Folder.GetFilesAsync()).Select(e => new LocalFile(e, FileSystem));
        }

        public async Task<IFolder> CreateFolderAsync(String name)
        {
            return new LocalFolder(await Folder.CreateFolderAsync(name), FileSystem);
        }
        public async Task<IFile> CreateFileAsync(String name)
        {
            return new LocalFile(await Folder.CreateFileAsync(name), FileSystem);
        }

        public async void DeleteAsync()
        {
            await Folder.DeleteAsync();
        }
    }

    class LocalFile : IFile
    {
        private readonly StorageFile File;

        public String Name => File.Name;
        public IFileSystem FileSystem { get; }
        public String Path => File.Path;

        public LocalFile(StorageFile file, IFileSystem fileSystem)
        {
            File = file;
            FileSystem = FileSystem;
        }

        public async Task<IFolder> GetParentAsync()
        {
            return new LocalFolder(await File.GetParentAsync(), FileSystem);
        }

        public async Task<StorageFile> GetFileAsync(StorageFolder target)
        {
            return await File.CopyAsync(target);
        }
        public async void SetFileAsync(StorageFile source)
        {
            await source.CopyAndReplaceAsync(File);
        }

        public async void DeleteAsync()
        {
            await File.DeleteAsync();
        }
    }
    
}
