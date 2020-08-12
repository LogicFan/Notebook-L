using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.DataTransfer;
using muxc = Microsoft.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using Microsoft.UI.Xaml.Controls;
using Notebook_L.Setting;
using System.Collections.Generic;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml.Hosting;
using Windows.Foundation.Collections;
using MetroLog;
using System.Threading;
using Windows.UI.Xaml.Navigation;

namespace Notebook_L
{
    public sealed partial class MainPage : Page
    {
        private readonly ILogger log = LogManagerFactory.DefaultLogManager.GetLogger<MainPage>();

        public static ISet<MainPage> Instances { get; } = new HashSet<MainPage>();

        public MainPage()
        {
            log.Info("Add this to Instances");
            MainPage.Instances.Add(this);
            
            this.InitializeComponent();

            ApplicationView.GetForCurrentView().Consolidated += (ApplicationView appView, ApplicationViewConsolidatedEventArgs args) =>
            {
                log.Info("Remove this from Instances");
                MainPage.Instances.Remove(this);
            };
        }

        public MainPage(AppWindow appWindow)
        {
            log.Info("Add this to Instances");
            MainPage.Instances.Add(this);

            this.appWindow = appWindow;
            this.InitializeComponent();

            this.appWindow.Closed += (AppWindow window, AppWindowClosedEventArgs args) => {
                log.Info("Remove this from Instances");
                MainPage.Instances.Remove(this);
            };
        }

        #region TabView
        private const String DataIdentifier = "MainPage_TabView_TabViewItem";
        private const String HomeTabId = "Home";
        private AppWindow appWindow = null;

        private muxc.TabViewItem CreateDefaultTabViewItem()
        {
            muxc.TabViewItem tabViewItem = new muxc.TabViewItem
            {
                Name = HomeTabId,
                Header = HomeTabId,
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
                MainPage newPage = new MainPage(newWindow);
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
                    Double offset = args.GetPosition(tabViewItem).X - 0.5 * tabViewItem.ActualWidth;
                    if (offset < 0)
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
        private const String SettingsTabId = "Settings";

        private muxc.TabViewItem CreateSettingsTabViewItem()
        {
            muxc.TabViewItem tabViewItem = new muxc.TabViewItem
            {
                Name = "Settings",
                Header = "Settings",
                IconSource = new muxc.SymbolIconSource()
                {
                    Symbol = Symbol.Setting
                }
            };

            tabViewItem.Content = new Frame();
            (tabViewItem.Content as Frame).Navigate(typeof(SettingPage));

            return tabViewItem;
        }

        private async void Button_Settings_Click(object sender, RoutedEventArgs e)
        {
            log.Info("Button_Settings_Click");

            foreach (MainPage page in MainPage.Instances)
            {
                foreach (TabViewItem item in page.TabView_Document.TabItems)
                {
                    if (item.Name == SettingsTabId)
                    {
                        log.Info(String.Format("Find TabViewItem with Name = {0}", SettingsTabId));
                        if (page.appWindow == null)
                        {
                            Int32 id = ApplicationView.GetForCurrentView().Id;
                            await ApplicationViewSwitcher.TryShowAsStandaloneAsync(id);
                        }
                        else
                        {
                            await page.appWindow.TryShowAsync();
                        }
                        page.TabView_Document.SelectedItem = item;
                        return;
                    }
                }
            }

            log.Info("Create a new TabViewItem");
            TabViewItem tabViewItem = CreateSettingsTabViewItem();
            this.TabView_Document.TabItems.Add(tabViewItem);
            this.TabView_Document.SelectedItem = tabViewItem;
        }
        #endregion
    }
}
