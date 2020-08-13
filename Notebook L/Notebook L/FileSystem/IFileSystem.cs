using Notebook_L.Serializable;
using System;
using System.Threading.Tasks;

namespace Notebook_L.FileSystem
{
    interface IFileSystem
    {
        Nullable<Account> Data { get; }
        String Name { get; }

        Task<IFolder> GetRootFolderAsync();
    }
}
