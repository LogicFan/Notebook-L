using Newtonsoft.Json;
using Notebook_L.FileSystem;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage;
using System.Collections.Specialized;

namespace Notebook_L.Setting
{
    static class Setting
    {
        #region Constant
        private const String ContainerId = "Setting.Setting";
        private const String LocationsId = "Locations";
        private const String ValueNotebooksId = "Notebooks";
        #endregion

        #region Locations
        public static void ObservableCollection_Locations_CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            ObservableCollection<Location.Location> locations = sender as ObservableCollection<Location.Location>;
            Serializable.Location[] array = locations.Select(e => e.Data).ToArray();
            GlobalInfo.SettingContainer.Values[LocationsId] = JsonConvert.SerializeObject(array);
        }

        public static ObservableCollection<Location.Location> Locations
        {
            get
            {
                String str = GlobalInfo.SettingContainer.Values[LocationsId] as String;
                Serializable.Location[] array = String.IsNullOrEmpty(str) ? new Serializable.Location[] { } : JsonConvert.DeserializeObject<Serializable.Location[]>(str);
                ObservableCollection<Location.Location> locations = new ObservableCollection<Location.Location>(array.Select(e => new Location.Location(e)));
                locations.CollectionChanged += ObservableCollection_Locations_CollectionChanged;
                return locations;
            }
        }
        #endregion Locations

        public static ObservableCollection<Notebook> Notebooks
        {
            get
            {
                String str = GlobalInfo.SettingContainer.Values[ValueNotebooksId] as String;
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
                GlobalInfo.SettingContainer.Values[ValueNotebooksId] = str;
            }
        }
    }
}
