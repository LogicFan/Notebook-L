using System;
using System.ComponentModel;
using Windows.UI.Xaml.Media;
using Notebook_L.FileSystem;
using Windows.UI.Xaml.Media.Imaging;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Notebook_L.FileItem
{
    public class Library : IFileItem
    {
        private IFileSystem m_fileSystem;
        private ImageSource m_icon;
        private DateTime m_modifiedTime;
        private ObservableCollection<FileItemMetadata> m_metadata;
        private ObservableCollection<IFileItem> m_items;

        public IFileSystem UnderlyingFileSystem => m_fileSystem;

        public string Name => m_fileSystem.Data.Name;
        public string Path => m_fileSystem.RootPath;
        public FileItemType ItemType => FileItemType.Library;
        public ImageSource Icon => m_icon;
        public DateTime ModifiedTime => m_modifiedTime;
        public bool IsFlagged => false;

        public ObservableCollection<FileItemMetadata> Metadata => m_metadata;
        public ObservableCollection<IFileItem> Items => m_items;

        public bool IsCopyable => false;
        public bool IsMoveable => false;
        public bool IsRemoveable => false;
        public bool IsExportable => true;
        public bool IsFlagable => false;

        public event EventHandler<FileItemEventArgs> RaiseOpenEvent;
        public event EventHandler<FileItemEventArgs> RaiseCopyEvent;
        public event EventHandler<FileItemEventArgs> RaiseMoveEvent;
        public event EventHandler<FileItemEventArgs> RaiseRemoveEvent;
        public event EventHandler<FileItemEventArgs> RaiseExportEvent;
        public event EventHandler<FileItemEventArgs> RaiseFlagEvent;
        public event PropertyChangedEventHandler PropertyChanged;

        private Library() { }

        private static async Task<ObservableCollection<FileItemMetadata>> LoadMetadata(IFileSystem fileSystem)
        {
            return new ObservableCollection<FileItemMetadata>();
        }

        public static async Task<IFileItem> CreateFileItemAsync(IFileSystem fileSystem)
        {
            IFileItem item = new Library()
            {
                m_fileSystem = fileSystem,
                m_icon = new SvgImageSource(new Uri("ms-appx:///Assets/" + fileSystem.Data.Source.ToString("G"))),
                m_modifiedTime = new DateTime(),
                m_metadata = await LoadMetadata(fileSystem),
                m_items = new ObservableCollection<IFileItem>()
            };
            return item;
        }

        public Task OpenAsync(object args)
        {
            throw new NotImplementedException();
        }
        public Task ExportAsync(object args)
        {
            throw new NotImplementedException();
        }

        public Task CopyAsync(object args) { throw new InvalidOperationException(); }
        public Task MoveAsync(object args) { throw new InvalidOperationException(); }
        public Task RemoveAsync(object args) { throw new InvalidOperationException(); }
        public Task FlagAsync(object args) { throw new InvalidOperationException(); }

        public void Dispose() { }
    }
}
