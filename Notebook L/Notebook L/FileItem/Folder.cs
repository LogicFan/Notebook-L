using Notebook_L.FileSystem;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Notebook_L.FileItem
{
    class Folder : IFileItem
    {
        public IFolder Data { get; }

        public Folder(IFolder folder)
        {
            Data = folder;

            LastModifiedDate = new DateTime();
            GetLastModifiedDate().ContinueWith(async e =>
            {
                LastModifiedDate = await e;
                NotifyPropertyChanged("LastModifiedDate");
            });
        }

        #region IFileItem
        public String Name => Data.Name;
        public String Path => Data.Path;

        public FileItemType ItemType => FileItemType.Folder;
        public DateTime LastModifiedDate { get; private set; }
        
        public ImageSource Icon => new SvgImageSource(new Uri("ms-appx:///Assets/Symbol/Folder.svg"));

        public event PropertyChangedEventHandler PropertyChanged;

        
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task<DateTime> GetLastModifiedDate()
        {
            return new DateTime();
        }
        #endregion


    }
}
