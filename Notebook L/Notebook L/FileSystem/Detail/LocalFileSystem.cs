using MetroLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace Notebook_L.FileSystem.Detail
{
    class LocalFileSystem : IFileSystem
    {
        private static readonly ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger<LocalFileSystem>();

        private StorageFolder m_folder;

        public FileSystemData Data { get; private set; }
        public String RootPath => m_folder.Path;

        private LocalFileSystem()
        {
            Log.Trace("Create object LocalFileSystem@{0:X8}", GetHashCode());
        }

        public async static Task<IFileSystem> CreateFileSystemAsync(FileSystemData data)
        {
            Log.Trace("CreateFileSystemAsync");
            Log.Info("data = LocationData@{0:X8}", data.GetHashCode());

            IFileSystem fileSystem = new LocalFileSystem
            {
                m_folder = await FileSystemFactory.LocalFolder.CreateFolderAsync("Notebook_L", CreationCollisionOption.OpenIfExists),
                Data = data
            };

            return fileSystem;
        }

        public IFolder GetRootFolder()
        {
            Log.Trace("LocalFileSystem@{0:X8}: GetRootFolder", GetHashCode());
            return new LocalFolder(m_folder, this);
        }
    }

    class LocalFolder : IFolder
    {
        private static readonly ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger<LocalFolder>();

        private readonly StorageFolder m_folder;

        public String Name => m_folder.Name;
        public String Path => m_folder.Path;
        public IFileSystem UnderlyingFileSystem { get; private set; }

        public LocalFolder(StorageFolder folder, IFileSystem fileSystem)
        {
            Log.Trace("Create object LocalFolder@{0:X8}", GetHashCode());
            Log.Info("folder = {0}, fileSystem = IFileSystem@{1:X8}",
                folder.Path, fileSystem.GetHashCode());

            m_folder = folder;
            UnderlyingFileSystem = fileSystem;
        }

        public async Task<IFolder> GetParentAsync()
        {
            Log.Trace("LocalFolder@{0:X8}: GetParentAsync", GetHashCode());

            if (m_folder.Path == UnderlyingFileSystem.RootPath)
            {
                Log.Info("this is root folder, there is no parent");
                return null;
            }

            return new LocalFolder(await m_folder.GetParentAsync(), UnderlyingFileSystem);
        }

        public async Task RenameAsync(String name)
        {
            Log.Trace("LocalFolder@{0:X8}: RenameAsync", GetHashCode());
            Log.Info("name = {0}", name);

            try
            {
                await m_folder.RenameAsync(name, NameCollisionOption.FailIfExists);
            }
            catch
            {
                throw new NameCollisionException(
                    String.Format("Name collision for name {0} at path {1}", name, Path));
            }
        }

        public async Task DeleteAsync()
        {
            Log.Trace("LocalFolder@{0:X8}: DeleteAsync", GetHashCode());

            await m_folder.DeleteAsync();
        }

        public async Task<IEnumerable<IFolder>> GetFoldersAsync()
        {
            Log.Trace("LocalFolder@{0:X8}: GetFoldersAsync", GetHashCode());

            IEnumerable<StorageFolder> folders = await m_folder.GetFoldersAsync();
            return folders.Select(e => new LocalFolder(e, UnderlyingFileSystem));
        }

        public async Task<IEnumerable<IFile>> GetFilesAsync()
        {
            Log.Trace("LocalFolder@{0:X8}: GetFilesAsync", GetHashCode());

            IEnumerable<StorageFile> files = await m_folder.GetFilesAsync();
            return files.Select(e => new LocalFile(e, UnderlyingFileSystem));
        }

        public async Task<IFolder> CreateFolderAsync(String name)
        {
            Log.Trace("LocalFolder@{0:X8}: CreateFolderAsync", GetHashCode());
            Log.Info("name = {0}", name);

            try
            {
                return new LocalFolder(
                    await m_folder.CreateFolderAsync(name, CreationCollisionOption.FailIfExists),
                    UnderlyingFileSystem);
            }
            catch
            {
                throw new NameCollisionException(
                    String.Format("Name collision for name {0} at path {1}", name, Path));
            }
        }

        public async Task<IFile> CreateFileAsync(String name)
        {
            Log.Trace("LocalFolder@{0:X8}: CreateFileAsync", GetHashCode());
            Log.Info("name = {0}", name);

            try
            {
                return new LocalFile(
                    await m_folder.CreateFileAsync(name, CreationCollisionOption.FailIfExists),
                    UnderlyingFileSystem);
            }
            catch
            {
                throw new NameCollisionException(
                    String.Format("Name collision for name {0} at path {1}", name, Path));
            }
        }
    }

    class LocalFile : IFile
    {
        private static readonly ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger<LocalFile>();

        private readonly StorageFile m_file;

        public String Name => m_file.Name;
        public String Path => m_file.Path;
        public IFileSystem UnderlyingFileSystem { get; private set; }

        public LocalFile(StorageFile file, IFileSystem fileSystem)
        {
            Log.Trace("Create object LocalFile@{0:X8}", GetHashCode());
            Log.Info("file = {0}, fileSystem = IFileSystem@{1:X8}",
                file.Path, fileSystem.GetHashCode());

            m_file = file;
            UnderlyingFileSystem = fileSystem;
        }

        public async Task<IFolder> GetParentAsync()
        {
            Log.Trace("LocalFile@{0:X8}: GetParentAsync", GetHashCode());

            return new LocalFolder(await m_file.GetParentAsync(), UnderlyingFileSystem);
        }

        public async Task RenameAsync(String name)
        {
            Log.Trace("LocalFile@{0:X8}: RenameAsync", GetHashCode());
            Log.Info("name = {0}", name);

            try
            {
                await m_file.RenameAsync(name, NameCollisionOption.FailIfExists);
            }
            catch
            {
                throw new NameCollisionException(
                    String.Format("Name collision for name {0} at path {1}", name, Path));
            }
        }

        public async Task DeleteAsync()
        {
            Log.Trace("LocalFile@{0:X8}: DeleteAsync", GetHashCode());

            await m_file.DeleteAsync();
        }

        public async Task<StorageFile> GetFileAsync(StorageFolder target)
        {
            Log.Trace("LocalFile@{0:X8}: GetFileAsync", GetHashCode());
            Log.Info("target = {0}", target.Path);

            try
            {
                return await m_file.CopyAsync(target, m_file.Name, NameCollisionOption.FailIfExists);
            }
            catch
            {
                throw new NameCollisionException(
                    String.Format("Name collision for name {0} at path {1}", m_file.Name, Path));
            }
        }

        public async Task SetFileAsync(StorageFile source)
        {
            Log.Trace("LocalFile@{0:X8}: SetFileAsync", GetHashCode());
            Log.Info("source = {0}", source.Path);

            await source.CopyAndReplaceAsync(m_file);
        }
    }
}
