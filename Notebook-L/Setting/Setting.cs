using Newtonsoft.Json;
using Notebook_L.FileSystem;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Notebook_L;
using Windows.Storage;

namespace Notebook_L.Setting
{
    static class Settings
    {
        #region Constant
        private const String ContainerId = "Settings.Settings";
        private const String LocationsId = "Locations";
        private const String ValueNotebooksId = "Notebooks";
        #endregion

        #region BaseContainer
        private static ApplicationDataContainer data = ApplicationData.Current.LocalSettings
            .CreateContainer(ContainerId, ApplicationDataCreateDisposition.Always);
        #endregion

        public static ObservableCollection<Location.Location> Locations
        {
            get
            {
                String str = data.Values[LocationsId] as String;
                if (String.IsNullOrEmpty(str))
                {
                    // Initialize with default value
                    str = JsonConvert.SerializeObject(new Serializable.Location[] { });
                    data.Values[LocationsId] = str;
                }
                Serializable.Location[] array = JsonConvert.DeserializeObject<Serializable.Location[]>(str);

                return new ObservableCollection<Location.Location>(array.Select(e => new Location.Location(e)));
            }

            set
            {

            }
        }


        public static ObservableCollection<Notebook> Notebooks
        {
            get
            {
                String str = data.Values[ValueNotebooksId] as String;
                if (String.IsNullOrEmpty(str))
                {
                    return new ObservableCollection<Notebook>();
                }
                Notebook[] array = JsonConvert.DeserializeObject<Notebook[]>(str);
                return new ObservableCollection<Notebook>(array);
            }
            set
            {
                Notebook[] array = value.ToArray();
                String str = JsonConvert.SerializeObject(array);
                data.Values[ValueNotebooksId] = str;
            }
        }
    }
}
