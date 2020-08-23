using System;
using System.ComponentModel;
using Windows.UI.Xaml.Media;
using Notebook_L.FileSystem;
using Windows.UI.Xaml.Media.Imaging;
using System.Runtime.Serialization;

namespace Notebook_L.FileItem
{
    class Library : IFileItem
    {
        private readonly IFileSystem m_fileSystem;

        public string Name => m_fileSystem.Data.Name;
        public string Path => m_fileSystem.RootPath;
        public FileItemType ItemType => FileItemType.Library;
        public ImageSource Icon { get; private set; }
        public DateTime ModifiedTime { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public Library(IFileSystem fileSystem)
        {
            m_fileSystem = fileSystem;
            Icon = new SvgImageSource(new Uri("ms-appx:///Assets/" + fileSystem.Data.Source.ToString("G")));
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    //[Serializable]
    //class LibraryMetadata : ISerializable
    //{
        
    //}
}
