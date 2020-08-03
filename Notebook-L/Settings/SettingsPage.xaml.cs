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
            log.Info(String.Format("Padding = {0}, Boderding = {1}", this.ListBox_Menu.Padding, this.ListBox_Menu.BorderThickness));
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String itemTag = (e.AddedItems[0] as SymbolIcon).Tag as String;

            Type itemPage = Type.GetType("Notebook_L.Settings." + itemTag);
            Frame_Settings.Navigate(itemPage);
        }
    }
}
