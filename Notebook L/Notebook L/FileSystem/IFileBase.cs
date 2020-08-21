using System;
using System.Threading.Tasks;

namespace Notebook_L.FileSystem
{
    public interface IFileBase
    {
        String Name { get; }
        String Path { get; }
        IFileSystem UnderlyingFileSystem { get; }

        Task<IFolder> GetParentAsync();

        Task RenameAsync(String name);
        Task DeleteAsync();
    }
}
