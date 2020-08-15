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

        public Location Data { get; }
        public String Name => Constant.LocalFileSystemName;

        public LocalFileSystem(Location location)
        {
            Log.Info(String.Format("Create object LocalFileSystem@{0:X8}", this.GetHashCode()));
            Data = location;
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
        private static readonly ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger<LocalFolder>();

        private readonly StorageFolder Folder;
        
        public String Name => Folder.Name;
        public IFileSystem FileSystem { get; }
        public String Path => Folder.Path;

        public LocalFolder(StorageFolder folder, IFileSystem fileSystem)
        {
            Log.Info(String.Format("Create object LocalFolder@{0:X8}, fileSystem = LocalFileSystem@{1:X8}", 
                this.GetHashCode(), fileSystem.GetHashCode()));

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
            Log.Info(String.Format("@{0:X8}: DeleteAsync", this.GetHashCode()));
            await Folder.DeleteAsync();
        }
    }

    class LocalFile : IFile
    {
        private static readonly ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger<LocalFile>();

        private readonly StorageFile File;

        public String Name => File.Name;
        public IFileSystem FileSystem { get; }
        public String Path => File.Path;

        public LocalFile(StorageFile file, IFileSystem fileSystem)
        {
            Log.Info(String.Format("Create object LocalFile@{0:X8}, fileSystem = LocalFileSystem@{1:X8}",
                this.GetHashCode(), fileSystem.GetHashCode()));

            File = file;
            FileSystem = FileSystem;
        }

        public async Task<IFolder> GetParentAsync()
        {
            return new LocalFolder(await File.GetParentAsync(), FileSystem);
        }

        public async Task<StorageFile> GetFileAsync(StorageFolder target)
        {
            Log.Info(String.Format("@{0:X8}: GetFileAsync, target = {1}", this.GetHashCode(), target.Path));
            return await File.CopyAsync(target);
        }
        public async void SetFileAsync(StorageFile source)
        {
            Log.Info(String.Format("@{0:X8}: SetFileAsync, source = {1}", this.GetHashCode(), source.Path));
            await source.CopyAndReplaceAsync(File);
        }

        public async void DeleteAsync()
        {
            Log.Info(String.Format("@{0:X8}: DeleteAsync", this.GetHashCode()));
            await File.DeleteAsync();
        }
    }
    
}
