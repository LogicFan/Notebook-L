using System;

namespace Notebook_L.FileSystem
{
    class LocalFileSystem : IFileSystem
    {
        private String name;

        public String Name => name;

        public LocalFileSystem(String name)
        {
            this.name = name;
        }

        public IDirectory GetDirectory(String path)
        {
            throw new NotImplementedException();
        }
        public IFile GetFile(String path)
        {
            throw new NotImplementedException();
        }
    }
}