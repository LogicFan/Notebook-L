using MetroLog;
using Notebook_L.Serializable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notebook_L.FileSystem
{
    class Notebook
    {
        private static readonly ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger<Notebook>();

        public Nullable<Account> Data => RootFolder.FileSystem.Data;

        private String Name => RootFolder.FileSystem.Name;
        private Boolean Primary { get; set; }
        public String UIName => Name + (Primary ? " (Primary Notebook)" : "");

        private String Source => Data == null ? "" : Data.Value.Source.ToString("G") + " - ";
        private String Path => RootFolder.Path;
        public String UIPath => Source + Path;

        public IFolder RootFolder { get; }

        public Notebook(IFolder folder)
        {
            Log.Info(String.Format("Create object Notebook@{0:X8}, folder = IFolder@{1:X8}", 
                this.GetHashCode(), folder.GetHashCode()));
            
            RootFolder = folder;
        }
    }
}
