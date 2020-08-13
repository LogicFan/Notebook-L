﻿using System;
using MetroLog;
using Notebook_L.Serializable;

namespace Notebook_L.FileSystem
{
    static class FileSystemFactory
    {
        private static readonly ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger(typeof(FileSystemFactory).Name);

        public static IFileSystem CreateFileSystem(Account account)
        {
            throw new NotImplementedException();
        }
    }
}