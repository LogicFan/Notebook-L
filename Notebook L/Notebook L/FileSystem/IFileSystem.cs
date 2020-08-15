using Notebook_L.Serializable;
using System;
using System.Threading.Tasks;

namespace Notebook_L.FileSystem
{
    interface IFileSystem
    {
        Location Data { get; }
        String Name { get; }

        Task<IFolder> GetRootFolderAsync();
    }
}
