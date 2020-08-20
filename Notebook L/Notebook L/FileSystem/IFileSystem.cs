﻿using System;

namespace Notebook_L.FileSystem
{
    public interface IFileSystem
    {
        LocationData Data { get; }
        String RootPath { get; }

        IFolder GetRootFolder();
    }
}
