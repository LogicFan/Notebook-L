using MetroLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace Notebook_L.FileSystem
{
    class LocalFileSystem : IFileSystem
    {
        private readonly ILogger log = LogManagerFactory.DefaultLogManager.GetLogger<LocalFileSystem>();

        public String Name { get; }

        public LocalFileSystem(String name)
        {
            log.Info(String.Format("Create object, Name = {0}", name));
            this.Name = name;
        }

        public async Task<IDirectory> GetDirectory(String path)
        {
            log.Info(String.Format("GetDirectory, Path = {0}", path));
            return new Directory(this, await StorageFolder.GetFolderFromPathAsync(path));
        }

        public async Task<IFile> GetFile(String path)
        {
            log.Info(String.Format("GetFile, Path = {0}", path));
            return new File(this, await StorageFile.GetFileFromPathAsync(path));
        }

        public override String ToString()
        {
            return String.Format("LocalFileSystem(Name = {0})", Name);
        }

        class Directory : IDirectory
        {
            private readonly ILogger log = LogManagerFactory.DefaultLogManager.GetLogger<Directory>();

            private StorageFolder storageFolder;

            public String Name => storageFolder.Name;
            public IFileSystem FileSystem { get; }
            public String Path => storageFolder.Path;

            public Directory(IFileSystem fileSystem, StorageFolder storageFolder)
            {
                log.Info("Create object, FileSystem = {0}, Path = {1}", fileSystem.Name, storageFolder.Path);
                this.FileSystem = fileSystem;
                this.storageFolder = storageFolder;
            }

            public async Task<IDirectory> GetParent()
            {
                log.Info("GetParent, current Path = {0}", storageFolder.Path);
                return new Directory(FileSystem, await storageFolder.GetParentAsync());
            }

            public async Task<IEnumerable<IDirectory>> GetDirectories()
            {
                log.Info("GetDirectories, current Path = {0}", storageFolder.Path);
                List<IDirectory> directories = new List<IDirectory>();
                foreach (StorageFolder subfolder in await storageFolder.GetFoldersAsync())
                {
                    directories.Add(new Directory(FileSystem, subfolder));
                }
                return directories;
            }

            public Task<IEnumerable<IFile>> GetFiles()
            {
                throw new NotImplementedException();
            }

            public async Task<IDirectory> CreateDirectory(String name)
            {
                return new Directory(FileSystem, await storageFolder.CreateFolderAsync(name));
            }

            public Task<IFile> CreateFile(String name)
            {
                throw new NotImplementedException();
            }
            
            public async void Delete()
            {
                await storageFolder.DeleteAsync();
            }
        }

        class File : IFile
        {
            private StorageFile storageFile;
            private StorageFile localCopy;

            public String Name => storageFile.Name;

            public IFileSystem FileSystem { get; }
            public String Path => storageFile.Path;

            public File(IFileSystem fileSystem, StorageFile storageFile)
            {
                this.FileSystem = fileSystem;
                this.storageFile = storageFile;
                this.localCopy = null;
            }

            public async Task<IDirectory> GetParent()
            {
                return new Directory(FileSystem, await storageFile.GetParentAsync());
            }

            public async Task<StorageFile> Download()
            {
                StorageFolder tempFolder = await Util.CreateTempDirectory();
                return await storageFile.CopyAsync(tempFolder);
            }
            
            public async void Upload()
            {
                await localCopy.CopyAndReplaceAsync(storageFile);
                await localCopy.GetParentAsync().GetResults().DeleteAsync();
                localCopy = null;
            }

            public async void Delete()
            {
                await storageFile.DeleteAsync();
                if (localCopy != null)
                {
                    await localCopy.GetParentAsync().GetResults().DeleteAsync();
                }
            }
        }
    }
}