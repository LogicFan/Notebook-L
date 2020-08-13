using Notebook_L.Serializable;
using System;
using System.Threading.Tasks;

namespace Notebook_L.FileSystem
{
    interface IFileSystem
    {
        Account Data { get; }
        String Path { get; }

        Task<IFolder> GetRootFolderAsync();
    }
}
