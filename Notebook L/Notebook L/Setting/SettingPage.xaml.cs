using MetroLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Notebook_L.Setting
{
    public sealed partial class SettingPage : Page
    {
        private static readonly ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger<SettingPage>();

        public SettingPage()
        {
            Log.Info(String.Format("Create object SettingPage@{0:X8}", this.GetHashCode()));

            this.InitializeComponent();
        }

        private void ListBox_Menu_Loaded(object sender, RoutedEventArgs e)
        {
            Log.Info(String.Format("@{0:X8}: ListBox_Menu_Loaded", this.GetHashCode()));

            ListBox listBox = sender as ListBox;
            if (listBox.SelectedItem == null)
            {
                listBox.SelectedItem = listBox.Items[0];
            }
        }

        private void ListBox_Menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Log.Info(String.Format("@{0:X8}: ListBox_Menu_SelectionChanged", this.GetHashCode()));

            String itemTag = (e.AddedItems[0] as SymbolIcon).Tag as String;

            Log.Info(String.Format("Select item {0}", itemTag));

            Type itemPage = Type.GetType("Notebook_L.Setting." + itemTag + "Page");
            Frame_Settings.Navigate(itemPage);
        }
    }
}
