using System.Threading.Tasks;
using Windows.Storage;

namespace Notebook_L.FileSystem
{
    public interface IFile : IFileBase
    {
        Task<StorageFile> GetFileAsync(StorageFolder target);
        void SetFileAsync(StorageFile source);
    }
}
