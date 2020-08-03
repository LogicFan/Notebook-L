using MetroLog;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using muxc = Microsoft.UI.Xaml.Controls;

namespace Notebook_L.Settings
{
    public sealed partial class SettingsPage : Page
    {
        private readonly ILogger log = LogManagerFactory.DefaultLogManager.GetLogger<SettingsPage>();

        public SettingsPage()
        {
            this.InitializeComponent();
        }

        private void NavigationView_BackRequested(muxc.NavigationView sender, muxc.NavigationViewBackRequestedEventArgs args)
        {
            
        }

        private void NavigationView_SelectionChanged(muxc.NavigationView sender, muxc.NavigationViewSelectionChangedEventArgs args)
        {
            muxc.NavigationViewItem selectedItem = args.SelectedItem as muxc.NavigationViewItem;
            if (selectedItem != null)
            {
                string itemTag = selectedItem.Tag as string;
                sender.Header = itemTag;
                Type itemPage = Type.GetType("Notebook_L.Settings." + itemTag);
                Frame_Settings.Navigate(itemPage);
            }
        }

        private void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            muxc.NavigationView navigationView = sender as muxc.NavigationView;
            navigationView.SelectedItem = navigationView.MenuItems[0];
        }
    }
}
