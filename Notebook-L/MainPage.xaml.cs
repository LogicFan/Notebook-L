using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.DataTransfer;
using muxc = Microsoft.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using Microsoft.UI.Xaml.Controls;
using Notebook_L.Settings;
using System.Collections.Generic;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml.Hosting;
using Windows.Foundation.Collections;
using MetroLog;

namespace Notebook_L
{
    public sealed partial class MainPage : Page
    {
        private readonly ILogger log = LogManagerFactory.DefaultLogManager.GetLogger<MainPage>();

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

        // Used by: TabView_AddTabButtonClick
        //          TabView_Loaded
        private muxc.TabViewItem CreateDefaultTabViewItem()
        {
            muxc.TabViewItem tabViewItem = new muxc.TabViewItem
            {
                Header = "Home",
                IconSource = new muxc.SymbolIconSource()
                {
                    Symbol = Symbol.Home
                }
            };

            tabViewItem.Content = new Frame();
            (tabViewItem.Content as Frame).Navigate(typeof(HomePage));

            return tabViewItem;
        }

        private void TabView_Loaded(object sender, RoutedEventArgs e)
        {
            log.Info("TabView_Loaded");

            muxc.TabView tabView = sender as muxc.TabView;
            if (tabView.TabItems.Count == 0)
            {
                tabView.TabItems.Add(CreateDefaultTabViewItem());
            }
        }

        private async void TabView_TabItemsChanged(muxc.TabView sender, IVectorChangedEventArgs args)
        {
            log.Info("TabView_TabItemsChanged");

            if (sender.TabItems.Count == 0)
            {
                if (appWindow != null)
                {
                    log.Info("Close the AppWindow");
                    await appWindow.CloseAsync();
                }
                else
                {
                    log.Info("Close the main ApplicationView");
                    await ApplicationView.GetForCurrentView().TryConsolidateAsync();
                }
            }
        }

        private void TabView_AddTabButtonClick(muxc.TabView sender, object args)
        {
            log.Info("TabView_AddTabButtonClick");

            muxc.TabViewItem tabViewItem = CreateDefaultTabViewItem();
            sender.TabItems.Add(tabViewItem);
        }

        private void TabView_TabCloseRequested(muxc.TabView sender, muxc.TabViewTabCloseRequestedEventArgs args)
        {
            log.Info("TabView_TabCloseRequested");

            sender.TabItems.Remove(args.Tab);
        }

        private async void TabView_TabDroppedOutside(muxc.TabView sender, muxc.TabViewTabDroppedOutsideEventArgs args)
        {
            log.Info("TabView_TabDroppedOutside");

            // Removing the last TabViewItem in TabView will result lose that TabViewItem
            if (this.TabView_Document.TabItems.Count > 1)
            {
                log.Info("Create a new AppWindow with content MainPage");

                AppWindow newWindow = await AppWindow.TryCreateAsync();
                var newPage = new MainPage(newWindow);
                ElementCompositionPreview.SetAppWindowContent(newWindow, newPage);

                log.Info("Move TabViewItem from old window to new AppWindow");
                this.TabView_Document.TabItems.Remove(args.Tab);
                newPage.TabView_Document.TabItems.Add(args.Tab);

                log.Info("Show the new AppWindow");
                await newWindow.TryShowAsync();
            }
        }

        private void TabView_TabStripDragOver(object sender, DragEventArgs args)
        {
            log.Info("TabView_TabStripDragOver");

            // Only allow drop tabs
            if (args.DataView.Properties.ContainsKey(DataIdentifier))
            {
                args.AcceptedOperation = DataPackageOperation.Move;
            }
        }

        private void TabView_TabStripDrop(object sender, DragEventArgs args)
        {
            log.Info("TabView_TabStripDrop");

            if (args.DataView.Properties.TryGetValue(DataIdentifier, out object obj))
            {
                log.Info("Get the list of TabViewItems from destination window");
                muxc.TabViewItem srcTabViewItem = obj as muxc.TabViewItem;
                muxc.TabView destTabView = sender as muxc.TabView;
                IList<object> destTabItems = destTabView.TabItems;

                log.Info("Find the drop location of new TabViewItem");
                Int32 index = -1;
                for (Int32 i = 0; i < destTabItems.Count; i++)
                {
                    TabViewItem tabViewItem = destTabItems[i] as TabViewItem;
                    if (args.GetPosition(tabViewItem).X - tabViewItem.ActualWidth < 0)
                    {
                        index = i;
                        break;
                    }
                }

                log.Info("Remove TabViewItem from source window");
                (srcTabViewItem.Parent as muxc.Primitives.TabViewListView).Items.Remove(obj);

                if (index < 0)
                {
                    log.Info("Insert to the end.");
                    destTabItems.Add(srcTabViewItem);
                }
                else
                {
                    log.Info(String.Format("Insert to location {0} ({1} TabViewItems in the list)", index, destTabItems.Count));
                    destTabItems.Insert(index, srcTabViewItem);
                }

                destTabView.SelectedItem = srcTabViewItem;
            }
        }

        private void TabView_TabDragStarting(object sender, muxc.TabViewTabDragStartingEventArgs args)
        {
            log.Info("TabView_TabDragStarging");

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
