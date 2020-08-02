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
using Microsoft.UI.Xaml.Controls;
using AppUIBasics.SamplePages;

namespace Notebook_L
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        #region TabView
        private void TabView_Initialization(object sender, RoutedEventArgs e)
        {
            TabView tabView = sender as TabView;

            if (tabView.TabItems.Count == 0)
            {
                tabView.TabItems.Add(TabView_Create());
            }
        }

        private TabViewItem TabView_Create()
        {
            TabViewItem newItem = new TabViewItem
            {
                Header = "Home",
                IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource()
                {
                    Symbol = Symbol.Home
                }
            };

            Frame frame = new Frame();
            frame.Navigate(typeof(HomePage));
            newItem.Content = frame;

            return newItem;
        }

        private void TabView_ButtonClick_Add(TabView sender, object args)
        {
            sender.TabItems.Add(TabView_Create());
        }

        private void TabView_ButtonClick_Close(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            sender.TabItems.Remove(args.Tab);

            if (sender.TabItems.Count == 0)
            {
                Application.Current.Exit();
            }
        }
        #endregion

        #region Settings
        private void Button_Click_Settings(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsPage), new SettingsPage(this));
        }
        #endregion
    }
}
