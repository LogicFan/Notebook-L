﻿using MetroLog;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using muxc = Microsoft.UI.Xaml.Controls;

namespace Notebook_L.Setting
{
    public sealed partial class SettingPage : Page
    {
        private readonly ILogger log = LogManagerFactory.DefaultLogManager.GetLogger<SettingPage>();

        public SettingPage()
        {
            this.InitializeComponent();
        }

        private void ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            log.Info("ListBox_Loaded");

            ListBox listBox = sender as ListBox;
            if (listBox.SelectedItem == null)
            {
                listBox.SelectedItem = listBox.Items[0];
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            log.Info("ListBox_SelectionChanged");

            String itemTag = (e.AddedItems[0] as SymbolIcon).Tag as String;

            log.Info(String.Format("Select item {0}", itemTag));

            Type itemPage = Type.GetType("Notebook_L.Setting." + itemTag + "Page");
            Frame_Settings.Navigate(itemPage);
        }
    }
}
