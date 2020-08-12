using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notebook_L.FileSystem
{
    [Serializable()]
    class Notebook
    {
        public enum SourceType
        {
            Local,
            OneDrive,
            GoogleDrive,
            iCloudDrive,
            DropBox
        }

        public String Name { get; set; }
        public SourceType Source { get; set; }
        public String Path { get; set; }

        public String UIPath => Source.ToString("G") + " - " + Path;
    }
}
