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
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.WindowManagement;

namespace Notebook_L
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        #region TabView
        // Used by:
        private const string DataIdentifier = "MainPage_TabView_TabViewItem";

        // Used by: TabView_AddTabButtonClick
        //          TabView_Loaded
        private TabViewItem CreateDefaultTabViewItem()
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

        private void TabView_Loaded(object sender, RoutedEventArgs e)
        {
            TabView tabView = sender as TabView;

            if (tabView.TabItems.Count == 0)
            {
                tabView.TabItems.Add(CreateDefaultTabViewItem());
            }
        }

        private void TabView_AddTabButtonClick(TabView sender, object args)
        {
            sender.TabItems.Add(CreateDefaultTabViewItem());
        }

        private void TabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            sender.TabItems.Remove(args.Tab);

            if (sender.TabItems.Count == 0)
            {
                Application.Current.Exit();
            }
        }

        private void TabView_TabDroppedOutside(TabView sender, TabViewTabDroppedOutsideEventArgs args)
        {
            throw new NotImplementedException();
        }

        private void TabView_TabStripDragOver(object sender, DragEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TabView_TabStripDrop(object sender, DragEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TabView_TabDragStarting(object sender, TabViewTabDragStartingEventArgs args)
        {
            args.Data.Properties.Add(DataIdentifier, args.Tab);
            args.Data.RequestedOperation = DataPackageOperation.Move;
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
