using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Notebook_L.FileItem
{
    public interface IFileItem : IDisposable, INotifyPropertyChanged
    {
        FileItemType ItemType { get; }
        String Name { get; }
        String Path { get; }
        ImageSource Icon { get; }
        DateTime ModifiedTime { get; }
        Boolean IsFlagged { get; }

        ObservableCollection<IFileItem> Items { get; }

        Boolean IsCopyable { get; }
        Boolean IsMoveable { get; }
        Boolean IsRemoveable { get; }
        Boolean IsExportable { get; }
        Boolean IsFlagable { get; }

        event EventHandler<FileItemEventArgs> RaiseOpenEvent;
        event EventHandler<FileItemEventArgs> RaiseCopyEvent;
        event EventHandler<FileItemEventArgs> RaiseMoveEvent;
        event EventHandler<FileItemEventArgs> RaiseRemoveEvent;
        event EventHandler<FileItemEventArgs> RaiseExportEvent;
        event EventHandler<FileItemEventArgs> RaiseFlagEvent;

        Task OpenAsync(object args);
        Task CopyAsync(object args);
        Task MoveAsync(object args);
        Task RemoveAsync(object args);
        Task ExportAsync(object args);
        Task FlagAsync(object args);
    }

    public enum FileItemType
    {
        Library, Folder, Document
    }

    public class FileItemEventArgs : EventArgs
    {

    }
}
