using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.DataTransfer;
using muxc = Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Microsoft.UI.Xaml.Controls;
using AppUIBasics.SamplePages;
using Notebook_L.Settings;
using Windows.UI.Xaml.Navigation;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml.Hosting;
using Windows.Foundation.Collections;

namespace Notebook_L
{
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();
        }

        public MainPage(AppWindow appWindow)
        {
            this.appWindow = appWindow;
            this.InitializeComponent();
        }

        #region TabView
        private const String DataIdentifier = "MainPage_TabView_TabViewItem";
        private AppWindow appWindow = null;
        private static Int64 windowCounter = 0;

        // Used by: TabView_AddTabButtonClick
        //          TabView_Loaded
        private muxc.TabViewItem CreateDefaultTabViewItem()
        {
            muxc.TabViewItem newItem = new muxc.TabViewItem
            {
                Header = "Home",
                IconSource = new muxc.SymbolIconSource()
                {
                    Symbol = Symbol.Home
                }
            };

            newItem.Content = new Frame();
            (newItem.Content as Frame).Navigate(typeof(HomePage));

            return newItem;
        }

        private void TabView_Loaded(object sender, RoutedEventArgs e)
        {
            muxc.TabView tabView = sender as muxc.TabView;

            if (tabView.TabItems.Count == 0)
            {
                tabView.TabItems.Add(CreateDefaultTabViewItem());
            }
        }

        private async void TabView_TabItemsChanged(muxc.TabView sender, IVectorChangedEventArgs args)
        {
            if (sender.TabItems.Count == 0)
            {
                if (appWindow != null)
                {
                    await appWindow.CloseAsync();
                }
                else
                {
                    await ApplicationView.GetForCurrentView().TryConsolidateAsync();
                }

            }
        }

        private void TabView_AddTabButtonClick(muxc.TabView sender, object args)
        {
            sender.TabItems.Add(CreateDefaultTabViewItem());
        }

        private void TabView_TabCloseRequested(muxc.TabView sender, muxc.TabViewTabCloseRequestedEventArgs args)
        {
            sender.TabItems.Remove(args.Tab);
        }

        private async void TabView_TabDroppedOutside(muxc.TabView sender, muxc.TabViewTabDroppedOutsideEventArgs args)
        {
            // Removing the last TabViewItem in TabView will result lose that TabViewItem
            if (this.TabView_Document.TabItems.Count > 1)
            {
                AppWindow newWindow = await AppWindow.TryCreateAsync();

                var newPage = new MainPage(newWindow);

                ElementCompositionPreview.SetAppWindowContent(newWindow, newPage);

                // Transfer tabs from one window to another 
                this.TabView_Document.TabItems.Remove(args.Tab);
                newPage.TabView_Document.TabItems.Add(args.Tab);

                await newWindow.TryShowAsync();
            }
        }

        private void TabView_TabStripDragOver(object sender, DragEventArgs args)
        {
            // Only allow drop tabs
            if (args.DataView.Properties.ContainsKey(DataIdentifier))
            {
                args.AcceptedOperation = DataPackageOperation.Move;
            }
        }

        private void TabView_TabStripDrop(object sender, DragEventArgs args)
        {
            if (args.DataView.Properties.TryGetValue(DataIdentifier, out object obj))
            {
                IList<object> destinationTabs = (sender as TabView).TabItems;

                // Find location in destination window
                Int32 index = -1;
                for (Int32 i = 0; i < destinationTabs.Count; i++)
                {
                    TabViewItem tab = destinationTabs[i] as TabViewItem;
                    if (args.GetPosition(tab).X - tab.ActualWidth < 0)
                    {
                        index = i;
                        break;
                    }
                }

                // Remove tab from old window
                ((obj as TabViewItem).Parent as muxc.Primitives.TabViewListView).Items.Remove(obj);

                if (index < 0)
                {
                    // Add to the end of the list
                    destinationTabs.Add(obj);
                }
                else
                {
                    // Otherwise, insert to given location
                    destinationTabs.Insert(index, obj);
                }

                (sender as TabView).SelectedItem = obj;
            }
        }

        private void TabView_TabDragStarting(object sender, muxc.TabViewTabDragStartingEventArgs args)
        {
            args.Data.Properties.Add(DataIdentifier, args.Tab);
            args.Data.RequestedOperation = DataPackageOperation.Move;
        }
        #endregion

        #region Button_Settings
        private void Button_Settings_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsPage), new SettingsPage(this));
        }
        #endregion
    }
}
