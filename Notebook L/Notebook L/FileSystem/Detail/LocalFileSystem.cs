using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace Notebook_L.FileSystem.Detail
{
    class LocalFileSystem : IFileSystem
    {
        private StorageFolder m_folder;

        public LocationData Data { get; private set; }
        public String RootPath => m_folder.Path;

        private LocalFileSystem() { }

        public async static Task<IFileSystem> CreateFileSystemAsync(LocationData data)
        {
            IFileSystem fileSystem = new LocalFileSystem
            {
                m_folder = await FileSystemFactory.LocalFolder.CreateFolderAsync("Notebook_L", CreationCollisionOption.OpenIfExists),
                Data = data
            };

            return fileSystem;
        }

        public IFolder GetRootFolder()
        {
            return new LocalFolder(m_folder, this);
        }
    }

    class LocalFolder : IFolder
    {
        private readonly StorageFolder m_folder;

        public String Name => m_folder.Name;
        public String Path => m_folder.Path;
        public IFileSystem UnderlyingFileSystem { get; private set; }

        public LocalFolder(StorageFolder folder, IFileSystem fileSystem)
        {
            m_folder = folder;
            UnderlyingFileSystem = fileSystem;
        }

        public async Task<IFolder> GetParentAsync()
        {
            if (m_folder.Path == UnderlyingFileSystem.RootPath)
            {
                return null;
            }

            return new LocalFolder(await m_folder.GetParentAsync(), UnderlyingFileSystem);
        }

        public async Task RenameAsync(String name)
        {
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
            await m_folder.DeleteAsync();
        }

        public async Task<IEnumerable<IFolder>> GetFoldersAsync()
        {
            IEnumerable<StorageFolder> folders = await m_folder.GetFoldersAsync();
            return folders.Select(e => new LocalFolder(e, UnderlyingFileSystem));
        }

        public async Task<IEnumerable<IFile>> GetFilesAsync()
        {
            IEnumerable<StorageFile> files = await m_folder.GetFilesAsync();
            return files.Select(e => new LocalFile(e, UnderlyingFileSystem));
        }

        public async Task<IFolder> CreateFolderAsync(String name)
        {
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
        private readonly StorageFile m_file;

        public String Name => m_file.Name;
        public String Path => m_file.Path;
        public IFileSystem UnderlyingFileSystem { get; private set; }

        public LocalFile(StorageFile file, IFileSystem fileSystem)
        {
            m_file = file;
            UnderlyingFileSystem = fileSystem;
        }

        public async Task<IFolder> GetParentAsync()
        {
            return new LocalFolder(await m_file.GetParentAsync(), UnderlyingFileSystem);
        }

        public async Task RenameAsync(String name)
        {
            try { await m_file.RenameAsync(name, NameCollisionOption.FailIfExists); }
            catch
            {
                throw new NameCollisionException(
                    String.Format("Name collision for name {0} at path {1}", name, Path));
            }
        }

        public async Task DeleteAsync()
        {
            await m_file.DeleteAsync();
        }

        public async Task<StorageFile> GetFileAsync(StorageFolder target)
        {
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
            await source.CopyAndReplaceAsync(m_file);
        }
    }
}
