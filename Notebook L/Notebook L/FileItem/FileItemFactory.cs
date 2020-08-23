using Notebook_L.FileSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notebook_L.FileItem
{
    public static class FileItemFactory
    {
        private static ObservableCollection<Library> m_locations;
        private static ObservableCollection<IFileItem> m_recentItems;
        private static ObservableCollection<IFileItem> m_laggedItems;

        public static async Task<ObservableCollection<Library>> GetLocations()
        {
            if (m_locations != null) { return m_locations; }

            throw new NotImplementedException();
        }

        public static async Task<IFileItem> CreateFileItemAsync(IFileSystem fileSystem)
        {
            return await Library.CreateFileItemAsync(fileSystem);
        }
    }
}
