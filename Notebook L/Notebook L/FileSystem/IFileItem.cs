using System;

namespace Notebook_L.FileSystem
{
    interface IFileItem
    {
        String Name { get; }
        String Path { get; }
    }
}
