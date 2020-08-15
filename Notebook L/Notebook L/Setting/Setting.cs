using Newtonsoft.Json;
using Notebook_L.FileSystem;
using Notebook_L.Serializable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Notebook_L.Setting
{
    static class Setting
    {
        #region NotebookPage
        public static IEnumerable<Location> GetLocations()
        {
            IEnumerable<Location> DefaultLocations()
            {
                return new Location[] { new Location
                {
                    Name = "Local Notebook",
                    Source = Location.SourceType.Local
                } };
            }

            String str = Constant.LocalSettings.Values[Constant.LocationSettingId] as String;
            if (String.IsNullOrEmpty(str))
            {
                return DefaultLocations();
            }
            else
            {
                return JsonConvert.DeserializeObject<Location[]>(str);
            }
        }

        public static void SetLocations(IEnumerable<Location> locations)
        {
            String str = JsonConvert.SerializeObject(locations.ToArray());
            Constant.LocalSettings.Values[Constant.LocationSettingId] = str;
        }

        public static IEnumerable<IFileSystem> GetFileSystems()
        {
            return GetLocations().Select(e => FileSystemFactory.CreateFileSystem(e));
        }

        public static async Task<IEnumerable<Notebook>> GetNotebooksAsync()
        {
            return await Task.WhenAll(
                GetFileSystems().Select(async e => new Notebook(await e.GetRootFolderAsync())));
        }
        #endregion

        #region AboutPage
        public static String Version => "Alpha v0.1.1";
        #endregion
    }
}
