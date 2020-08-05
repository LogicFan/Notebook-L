using System;

namespace Notebook_L.Serializable
{
    [Serializable]
    struct Notebook
    {
        public String Name;
        public String Path;
        public String SourceName;

        public Notebook(String name, String path, String sourceName)
        {
            this.Name = name;
            this.Path = path;
            this.SourceName = sourceName;
        }
    }
}
