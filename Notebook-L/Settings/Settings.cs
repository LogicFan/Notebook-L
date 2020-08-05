using Newtonsoft.Json;
using Notebook_L.FileSystem;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage;

namespace Notebook_L.Settings
{
    static class Settings
    {
        private const String ContainerId = "Settings.Settings";
        private const String ValueNotebooksId = "Notebooks";

        private static ApplicationDataContainer data = ApplicationData.Current.LocalSettings
            .CreateContainer(ContainerId, ApplicationDataCreateDisposition.Always);

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
