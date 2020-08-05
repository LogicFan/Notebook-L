using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Notebook_L.Setting
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
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

            Setting.AddLocation(new Serializable.Location
            {
                Name = "Item " + random.Next().ToString(),
                Source = Serializable.Location.SourceType.OneDrive
            });
            #endregion

            ObservableCollection<Location.Location> locations = Setting.Locations;
            this.ListView.ItemsSource = locations;
        }
    }
}
