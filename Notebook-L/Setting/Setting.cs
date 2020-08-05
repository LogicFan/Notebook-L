using Newtonsoft.Json;
using Notebook_L.FileSystem;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Notebook_L;
using Windows.Storage;

namespace Notebook_L.Setting
{
    static class Setting
    {
        #region Constant
        private const String ContainerId = "Setting.Setting";
        private const String LocationsId = "Locations";
        private const String ValueNotebooksId = "Notebooks";
        #endregion

        #region BaseContainer
        private static ApplicationDataContainer data = ApplicationData.Current.LocalSettings
            .CreateContainer(ContainerId, ApplicationDataCreateDisposition.Always);
        #endregion

        #region Locations
        private static Serializable.Location[] SerializableLocations
        {
            get
            {
                String str = data.Values[LocationsId] as String;
                return String.IsNullOrEmpty(str) ? new Serializable.Location[] { } : JsonConvert.DeserializeObject<Serializable.Location[]>(str);
            }
        }

        public static ObservableCollection<Location.Location> Locations
        {
            get => new ObservableCollection<Location.Location>(SerializableLocations.Select(e => new Location.Location(e)));
        }

        public static void AddLocation(Serializable.Location location)
        {
            Serializable.Location[] locations = SerializableLocations;
            String str = JsonConvert.SerializeObject(locations.Append(location).ToArray());
            data.Values[LocationsId] = str;
        }

        public static void RemoveLocation(Serializable.Location location)
        {
            Serializable.Location[] locations = SerializableLocations;
            String str = JsonConvert.SerializeObject(locations.Where(e => e.Name != location.Name).ToArray());
            data.Values[LocationsId] = str;
        }
        #endregion Locations

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
