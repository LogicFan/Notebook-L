using MetroLog;
using System;
using System.Runtime.Serialization;

namespace Notebook_L.FileItem
{
    [Serializable]
    public sealed class FileItemMetadata : ISerializable
    {
        private static readonly ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger<FileItemMetadata>();

        public String Path { get; set; }
        public FileItemType ItemType { get; set; }
        public DateTime ModifiedTime { get; set; }      // CreationTime for Folder, LastModifiedTime for Document
        public Boolean IsFlagged { get; set; }

        public FileItemMetadata()
        {
            Log.Trace("Create object FileItemMetadata@{0:X8}", GetHashCode());
        }

        public FileItemMetadata(SerializationInfo info, StreamingContext context)
        {
            Log.Trace("Create object FileItemMetadata@{0:X8}", GetHashCode());

            Path = (String)info.GetValue("Path", typeof(String));
            ItemType = (FileItemType)info.GetValue("ItemType", typeof(FileItemType));
            ModifiedTime = (DateTime)info.GetValue("ModifiedTime", typeof(DateTime));
            IsFlagged = (Boolean)info.GetValue("IsFlagged", typeof(Boolean));

            Log.Info("Path = {0}, ItemType = {1}, ModifiedTime = {2}, IsFlagged = {3}",
                Path, ItemType.ToString("G"), ModifiedTime, IsFlagged);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Log.Trace("LocationData@{0:X8}: GetObjectData", GetHashCode());
            Log.Info("Path = {0}, ItemType = {1}, ModifiedTime = {2}, IsFlagged = {3}",
                Path, ItemType.ToString("G"), ModifiedTime, IsFlagged);

            info.AddValue("Path", Path, typeof(String));
            info.AddValue("ItemType", ItemType, typeof(FileItemType));
            info.AddValue("ModifiedTime", ModifiedTime, typeof(DateTime));
            info.AddValue("IsFlagged", IsFlagged, typeof(Boolean));
        }
    }
}
