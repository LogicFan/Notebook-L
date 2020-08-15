using MetroLog;
using Notebook_L.Serializable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Notebook_L.FileSystem
{
    class Notebook : IFileItem
    {
        private static readonly ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger<Notebook>();

        private readonly IFolder Folder;

        public String Name => Data.Name;
        public String Path => Folder.Path;

        public Location Data => Folder.FileSystem.Data;
        public Boolean IsPrimary { get; set; } = false;
        public String UIName => Name + (IsPrimary ? " (Primary)" : "");
        public String UIPath => Data.Source.ToString("G") + " - " + Path;
        public ImageSource Icon { get; }

        public Notebook(IFolder folder)
        {
            Log.Info(String.Format("Create object Notebook@{0:X8}, folder = IFolder@{1:X8}", 
                this.GetHashCode(), folder.GetHashCode()));

            Folder = folder;

            Icon = new SvgImageSource(new Uri("ms-appx:///Assets/Symbol/" + folder.FileSystem.Data.Source.ToString("G") + ".svg"));
        }

        public IFolder GetRootFolder()
        {
            return Folder;
        }

        public static Notebook CreateNotebookFromFileSystem(IFileSystem fileSystem)
        {
            return null;
        }

        public static Notebook CreateNotebookFromLocation(Location location)
        {
            return null;
        }
    }
}
