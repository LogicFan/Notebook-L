using MetroLog;
using Microsoft.UI.Xaml.Controls;
using Notebook_L.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using muxc = Microsoft.UI.Xaml.Controls;

namespace Notebook_L
{
    public sealed partial class MainPage : Page
    {
        // The logger for the class
        private static readonly ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger<MainPage>();
        // The set of instances of this class
        private static readonly ISet<MainPage> Instances = new HashSet<MainPage>();
        private readonly AppWindow Window;

        public Boolean InitialTab { get; set; }
        public Action TryShow { get; }

        #region Constructor
        public MainPage() : this(null, true) { }

        public MainPage(AppWindow window, Boolean initialTab)
        {
            Log.Info(String.Format("Create object MainPage@{0:X8}, window = AppWindow@{1:X8}, initialTab = {2}",
                this.GetHashCode(), window == null ? 0 : window.GetHashCode(), initialTab));

            Instances.Add(this);
            Log.Info(String.Format("Add MainPage@{0:X8} into Instances", this.GetHashCode()));

            Window = window;
            InitialTab = initialTab;
            this.InitializeComponent();

            void action()
            {
                Instances.Remove(this);
                Log.Info(String.Format("Remove MainPage@{0:X8} from Instances", this.GetHashCode()));

                Log.Info(String.Format("Trigger OnNavigatedFrom for each tab in MainPage@{0:X8}", this.GetHashCode()));
                foreach (muxc.TabViewItem tabViewItem in TabView_Main.TabItems)
                {
                    (tabViewItem.Content as Frame).GetNavigationState();
                }
            }

            if (window == null)
            {
                ApplicationView.GetForCurrentView().Consolidated += (ApplicationView appView, ApplicationViewConsolidatedEventArgs args) =>
                {
                    action();
                };

                TryShow = async () =>
                {
                    Int32 id = ApplicationView.GetForCurrentView().Id;
                    await ApplicationViewSwitcher.TryShowAsStandaloneAsync(id);
                    Log.Info(String.Format("Show MainPage@{0:X8}", this.GetHashCode()));
                };
            }
            else
            {
                window.Closed += (AppWindow win, AppWindowClosedEventArgs args) =>
                {
                    action();
                };

                TryShow = async () =>
                {
                    await window.TryShowAsync();
                    Log.Info(String.Format("Show MainPage@{0:X8}", this.GetHashCode()));
                };
            }
        }
        #endregion

        #region TabViewItem
        public static IEnumerable<TabViewItem> TabItems => Instances.SelectMany(e => e.TabView_Main.TabItems).Select(e => e as TabViewItem);

        public static Tuple<MainPage, TabViewItem> SearchTab(String tabId)
        {
            foreach (MainPage page in Instances)
            {
                foreach (TabViewItem item in page.TabView_Main.TabItems)
                {
                    if (item.Tag as String == tabId)
                    {
                        Log.Info(String.Format("Found TabViewItem@{0:X8} in MainPage@{1:X8} with Tag = {2}", 
                            page.GetHashCode(), item.GetHashCode(), tabId));
                        return new Tuple<MainPage, TabViewItem>(page, item);
                    }
                }
            }
            Log.Info(String.Format("No TabViewItem with Tag = {0} Found", tabId));
            return null;
        }

        private muxc.TabViewItem CreateHomePageTab()
        {
            muxc.TabViewItem tabViewItem = new muxc.TabViewItem
            {
                Tag = Constant.HomePageTabId,
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

        private muxc.TabViewItem CreateSettingPageTab()
        {
            muxc.TabViewItem tabViewItem = new muxc.TabViewItem
            {
                Tag = Constant.SettingPageTabId,
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
        #endregion

        #region EventHandler TabView_Main
        private void TabView_Main_Loaded(object sender, RoutedEventArgs args)
        {
            Log.Info(String.Format("@{0:X8}: TabView_Main_Loaded", this.GetHashCode()));

            if (InitialTab)
            {
                muxc.TabView tabView = sender as muxc.TabView;
                tabView.TabItems.Add(CreateHomePageTab());
            }
        }

        private async void TabView_Main_TabItemsChanged(muxc.TabView sender, IVectorChangedEventArgs args)
        {
            Log.Info(String.Format("@{0:X8}: TabView_Main_TabItemsChanged", this.GetHashCode()));

            if (sender.TabItems.Count == 0)
            {
                if (Window != null)
                {
                    await Window.CloseAsync();
                }
                else
                {
                    await ApplicationView.GetForCurrentView().TryConsolidateAsync();
                }

                Log.Info(String.Format("Close AppWindow@{0:X8} and corresponding MainPage@{1:X8}",
                    Window == null ? 0 : Window.GetHashCode(), this.GetHashCode()));
            }
        }

        private void TabView_Main_AddTabButtonClick(muxc.TabView sender, object args)
        {
            Log.Info(String.Format("@{0:X8}: TabView_Main_AddTabButtonClick", this.GetHashCode()));

            muxc.TabViewItem tabViewItem = CreateHomePageTab();
            sender.TabItems.Add(tabViewItem);
            sender.SelectedItem = tabViewItem;

            Log.Info(String.Format("Add TabViewItem@{0:X8} into MainPage@{1:X8}",
                tabViewItem.GetHashCode(), this.GetHashCode()));
        }

        private void TabView_Main_TabCloseRequested(muxc.TabView sender, muxc.TabViewTabCloseRequestedEventArgs args)
        {
            Log.Info(String.Format("@{0:X8}: TabView_Main_TabCloseRequested", this.GetHashCode()));

            (args.Tab.Content as Frame).GetNavigationState();   // Trigger OnNavigatedFrom
            sender.TabItems.Remove(args.Tab);

            Log.Info(String.Format("Remove TabViewItem@{0:X8} from MainPage@{1:X8}",
                args.Tab.GetHashCode(), this.GetHashCode()));
        }

        private async void TabView_Main_TabDroppedOutside(muxc.TabView sender, muxc.TabViewTabDroppedOutsideEventArgs args)
        {
            Log.Info(String.Format("@{0:X8}: TabView_Main_TabDroppedOutside", this.GetHashCode()));

            // Cannot remove the last TabViewItem in a window, otherwise it will result
            // the lose of TabViewItem
            if (this.TabView_Main.TabItems.Count > 1)
            {
                AppWindow newWindow = await AppWindow.TryCreateAsync();
                MainPage newPage = new MainPage(newWindow, false);
                ElementCompositionPreview.SetAppWindowContent(newWindow, newPage);
                
                Log.Info(String.Format("Compose AppWindow@{0:X8} and MainPage@{1:X8}",
                    newWindow.GetHashCode(), newPage.GetHashCode()));

                this.TabView_Main.TabItems.Remove(args.Tab);
                newPage.TabView_Main.TabItems.Add(args.Tab);

                Log.Info(String.Format("Move TabViewItem@{0:X8} from MainPage@{1:X8} to MainPage@{2:X8}",
                    args.Tab.GetHashCode(), this.GetHashCode(), newPage.GetHashCode()));

                newPage.TryShow();
            }
        }

        private void TabView_Main_TabStripDragOver(object sender, DragEventArgs args)
        {
            Log.Info(String.Format("@{0:X8}: TabView_Main_TabStripDragOver", this.GetHashCode()));

            // Only TabViewItem is allowed to drop
            if (args.DataView.Properties.ContainsKey(Constant.TabDataIdentifier))
            {
                args.AcceptedOperation = DataPackageOperation.Move;
            }
        }

        private void TabView_Main_TabStripDrop(object sender, DragEventArgs args)
        {
            Log.Info(String.Format("@{0:X8}: TabView_Main_TabStripDrop", this.GetHashCode()));

            if (args.DataView.Properties.TryGetValue(Constant.TabDataIdentifier, out object obj))
            {
                muxc.TabView destTabView = sender as muxc.TabView;
                IList<object> destTabItems = destTabView.TabItems;

                Int32 index = -1;
                for (Int32 i = 0; i < destTabItems.Count; i++)
                {
                    muxc.TabViewItem tabViewItem = destTabItems[i] as muxc.TabViewItem;
                    Double offset = args.GetPosition(tabViewItem).X - 0.5 * tabViewItem.ActualWidth;
                    if (offset < 0)
                    {
                        index = i;
                        break;
                    }
                }
                Log.Info(String.Format("Found the drop location, index = {0}", index));

                muxc.TabViewItem srcTabViewItem = obj as muxc.TabViewItem;
                (srcTabViewItem.Parent as muxc.Primitives.TabViewListView).Items.Remove(obj);
                Log.Info(String.Format("Remove TabViewItem@{0:X8} from source page", srcTabViewItem.GetHashCode()));

                if (index < 0)
                {
                    Log.Info("Insert to the end ({0} in the list)", destTabItems.Count);
                    destTabItems.Add(srcTabViewItem);
                }
                else
                {
                    Log.Info(String.Format("Insert to location {0} ({1} in the list)", index, destTabItems.Count));
                    destTabItems.Insert(index, srcTabViewItem);
                }

                destTabView.SelectedItem = srcTabViewItem;
            }
        }

        private void TabView_Main_TabDragStarting(muxc.TabView sender, muxc.TabViewTabDragStartingEventArgs args)
        {
            Log.Info(String.Format("@{0:X8}: TabView_Main_TabDragStarting", this.GetHashCode()));

            args.Data.Properties.Add(Constant.TabDataIdentifier, args.Tab);
            args.Data.RequestedOperation = DataPackageOperation.Move;
        }
        #endregion

        #region EventHandler Button_Setting
        private void Button_Setting_Click(object sender, RoutedEventArgs args)
        {
            Log.Info(String.Format("@{0:X8}: Button_Setting_Click", this.GetHashCode()));

            Tuple<MainPage, TabViewItem> tuple = SearchTab(Constant.SettingPageTabId);

            if (tuple != null)
            {
                tuple.Item1.TryShow();
                tuple.Item1.TabView_Main.SelectedItem = tuple.Item2;
            }
            else
            {
                muxc.TabViewItem tabViewItem = CreateSettingPageTab();
                TabView_Main.TabItems.Add(tabViewItem);
                TabView_Main.SelectedItem = tabViewItem;
                Log.Info(String.Format("Add TabViewItem@{0:X8} into MainPage@{1:X8}",
                    tabViewItem.GetHashCode(), this.GetHashCode()));
            }
        }
        #endregion
    }
}
