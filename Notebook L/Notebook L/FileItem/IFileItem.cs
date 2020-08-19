using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Notebook_L.FileItem
{
    enum FileItemType : Int32
    {
        Notebook = 0, 
        Folder = 1, 
        Document = 2
    }

    interface IFileItem : INotifyPropertyChanged
    {
        String Name { get; }
        String Path { get; }

        FileItemType ItemType { get; }
        DateTime LastModifiedDate { get; }

        ImageSource Icon { get; }
    }
}
