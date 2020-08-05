using System;
using System.Collections.Generic;

namespace Notebook_L.FileSystem
{
    interface IDirectory
    {
        String Name { get; }
        
        IDirectory Parent { get; }
        IEnumerable<IDirectory> Directories { get; }
        IEnumerable<IFile> Files { get; }
    }
}
