using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Notebook_L.FileItem
{
    public interface IFileItem : IDisposable, INotifyPropertyChanged
    {
        String Name { get; }
        String Path { get; }
        FileItemType ItemType { get; }

        ImageSource Icon { get; }
        //Boolean IsFlagged { get; }
        
        DateTime ModifiedTime { get; }

        // TODO Add events
        //Boolean IsOpenable { get; }
        //Boolean IsCopyable { get; }
        //Boolean IsMoveable { get; }
        //Boolean IsRemoveable { get; }
        //Boolean IsExportable { get; }
        //Boolean IsFlagable { get; }

        //EventHandler<FileItemEventArgs> RaiseOpenEvent { get; }
        //EventHandler<FileItemEventArgs> RaiseCopyEvent { get; }
        //EventHandler<FileItemEventArgs> RaiseMoveEvent { get; }
        //EventHandler<FileItemEventArgs> RaiseRemoveEvent { get; }
        //EventHandler<FileItemEventArgs> RaiseAddItemEvent { get; }
        //EventHandler<FileItemEventArgs> RaiseExportEvent { get; }
        //EventHandler<FileItemEventArgs> RaiseFlagEvent { get; }

        //Task OpenAsync();
        //Task CopyAsync();
        //Task MoveAsync();
        //Task RemoveAsync();
        //Task AddItemAsnyc();
        //Task ExportAsync();
        //Task FlagAsync();
    }

    public enum FileItemType
    {
        Library, Folder, Document
    }

    public class FileItemEventArgs : EventArgs
    {

    }
}
