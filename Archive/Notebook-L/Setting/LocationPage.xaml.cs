using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace Notebook_L.Setting
{
    public sealed partial class LocationPage : Page
    {
        public LocationPage()
        {
            this.InitializeComponent();
        }

        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            ListView listView = sender as ListView;
            ObservableCollection<Location.Location> locations = Setting.Locations;

            listView.ItemsSource = locations;
        }

        private void ListView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            #region TestCode
            Random random = new Random();
            ObservableCollection<Location.Location> locations = this.ListView.ItemsSource as ObservableCollection<Location.Location>;

            locations.Add(new Location.Location(new Serializable.Location
            {
                Name = "Item " + random.Next().ToString(),
                Credential = "p9eflkdsjaoij43p1foij423jf",
                Source = Serializable.Location.SourceType.OneDrive
            }));
            #endregion
        }
    }
}
