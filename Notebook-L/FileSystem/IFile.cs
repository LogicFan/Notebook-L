using System;
using System.IO;

namespace Notebook_L.FileSystem
{
    interface IFile
    {
        String Name { get; }

        Stream Content { get; }
    }
}
