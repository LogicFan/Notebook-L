using MetroLog;
using Microsoft.UI.Xaml.Controls;
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
    public sealed partial class MainPage : Page, IDisposable
    {
        private static readonly ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger<MainPage>();

        private static readonly ISet<MainPage> m_instances = new HashSet<MainPage>();
        private readonly AppWindow m_window = null;

        public static IEnumerable<MainPage> Instances => m_instances;

        public MainPage() : this(null) { }

        public MainPage(AppWindow appWindow)
        {
            Log.Trace("Create object MainPage@{0:X8}", GetHashCode());
            Log.Info("appWindow = AppWindow@{0:X8}", 
                appWindow == null ? 0 : appWindow.GetHashCode());

            m_window = appWindow;
            m_instances.Add(this);
            Log.Info("Add MainPage@{0:X8} into m_instances", GetHashCode());

            InitializeComponent();

            if (m_window == null)
            {
                ApplicationView.GetForCurrentView().Consolidated += (ApplicationView appView, ApplicationViewConsolidatedEventArgs args) =>
                {
                    Dispose();
                };
            }
            else
            {
                m_window.Closed += (AppWindow win, AppWindowClosedEventArgs args) =>
                {
                    Dispose();
                };
            }
        }

        public void Dispose()
        {
            Log.Trace("MainPage@{0:X8}: Dispose", GetHashCode());

            m_instances.Remove(this);
            Log.Info("Remove MainPage@{0:X8} from m_instances", GetHashCode());

            foreach (TabViewItem tabViewItem in TabView_Main.TabItems)
            {
                (tabViewItem.Content as Frame).GetNavigationState();
            }
            Log.Info("Trigger OnNavigatedFrom for each Page in TabView_Main.TabItems");
        }

        public async void CloseAsync()
        {
            Log.Trace("MainPage@{0:X8}: CloseAsync", GetHashCode());

            if (m_window != null)
            {
                await m_window.CloseAsync();
            }
            else
            {
                await ApplicationView.GetForCurrentView().TryConsolidateAsync();
            }
        }

        public async void TryShowAsync()
        {
            Log.Trace("MainPage@{0:X8}: TryShowAsync", GetHashCode());

            if (m_window == null)
            {
                Int32 id = ApplicationView.GetForCurrentView().Id;
                await ApplicationViewSwitcher.TryShowAsStandaloneAsync(id);
            }
            else
            {
                await m_window.TryShowAsync();
            }
        }

        #region TabViewItem
        private const String TabDataIdentifier = "iLoj4PTo";

        public const String HomePageTabId = "HomePage-BvlzpOeJ";
        public const String SettingPageTabId = "SettingPage-fkXRAjq1";
        public static String EditorPageTabId(String path = "")
        {
            return "EditorPage-" + path;
        }

        public static IEnumerable<TabViewItem> TabItems => 
            Instances.SelectMany(e => e.TabView_Main.TabItems).Select(e => e as TabViewItem);

        public static Tuple<MainPage, TabViewItem> GetTabById(String id)
        {
            foreach (MainPage page in Instances)
            {
                foreach (TabViewItem item in page.TabView_Main.TabItems)
                {
                    if (item.Tag as String == id)
                    {
                        Log.Info("Found TabViewItem@{0:X8} in MainPage@{1:X8} with Tag = {2}",
                            page.GetHashCode(), item.GetHashCode(), id);
                        return new Tuple<MainPage, TabViewItem>(page, item);
                    }
                }
            }

            Log.Info("No TabViewItem with Tag = {0} Found", id);
            return null;
        }

        public static TabViewItem CreateTabById(String id)
        {
            Frame frame = new Frame();
            TabViewItem tabViewItem = new TabViewItem
            {
                Tag = id
            };

            switch (id)
            {
                case HomePageTabId:
                    frame.Navigate(typeof(Playground));
                    tabViewItem.Header = "Home";
                    tabViewItem.IconSource = new muxc.SymbolIconSource() { Symbol = Symbol.Home };
                    tabViewItem.Content = frame;
                    break;
                case SettingPageTabId:
                    frame.Navigate(typeof(BlankPage));
                    tabViewItem.Header = "Settings";
                    tabViewItem.IconSource = new muxc.SymbolIconSource() { Symbol = Symbol.Setting };
                    tabViewItem.Content = frame;
                    break;
                default:
                    throw new NotImplementedException();
            }

            Log.Info("Create object TabViewItem@{0:X8} with tag = ", 
                tabViewItem.GetHashCode(), id);

            return tabViewItem;
        }
        #endregion

        #region TabView_Main
        private void TabView_Main_Loaded(object sender, RoutedEventArgs args)
        {
            Log.Trace("MainPage@{0:X8}: TabView_Main_Loaded", GetHashCode());

            TabView tabView = sender as muxc.TabView;
            if (tabView.TabItems.Count == 0)
            {
                tabView.TabItems.Add(CreateTabById(HomePageTabId));
            }
        }

        private void TabView_Main_TabItemsChanged(muxc.TabView sender, IVectorChangedEventArgs args)
        {
            Log.Trace("MainPage@{0:X8}: TabView_Main_TabItemsChanged", GetHashCode());

            if (sender.TabItems.Count == 0)
            {
                CloseAsync();
            }
        }

        private void TabView_Main_AddTabButtonClick(muxc.TabView sender, object args)
        {
            Log.Trace("MainPage@{0:X8}: TabView_Main_AddTabButtonClick", GetHashCode());

            TabViewItem tabViewItem = CreateTabById(HomePageTabId);
            sender.TabItems.Add(tabViewItem);
            sender.SelectedItem = tabViewItem;

            Log.Info("Add TabViewItem@{0:X8} into MainPage@{1:X8}",
                tabViewItem.GetHashCode(), GetHashCode());
        }

        private void TabView_Main_TabCloseRequested(muxc.TabView sender, muxc.TabViewTabCloseRequestedEventArgs args)
        {
            Log.Trace("MainPage@{0:X8}: TabView_Main_TabCloseRequested", GetHashCode());

            (args.Tab.Content as Frame).GetNavigationState();   // Trigger OnNavigatedFrom
            sender.TabItems.Remove(args.Tab);

            Log.Info("Remove TabViewItem@{0:X8} from MainPage@{1:X8}",
                args.Tab.GetHashCode(), GetHashCode());
        }

        private async void TabView_Main_TabDroppedOutside(TabView sender, TabViewTabDroppedOutsideEventArgs args)
        {
            Log.Trace("MainPage@{0:X8}: TabView_Main_TabDroppedOutside", GetHashCode());

            // Cannot remove the last TabViewItem in a window, otherwise it will result
            // the lose of TabViewItem
            if (TabView_Main.TabItems.Count > 1)
            {
                AppWindow appWindow = await AppWindow.TryCreateAsync();
                MainPage mainPage = new MainPage(appWindow);

                TabView_Main.TabItems.Remove(args.Tab);
                mainPage.TabView_Main.TabItems.Add(args.Tab);
                Log.Info("Move TabViewItem@{0:X8} from MainPage@{1:X8} to MainPage@{2:X8}",
                    args.Tab.GetHashCode(), GetHashCode(), mainPage.GetHashCode());

                ElementCompositionPreview.SetAppWindowContent(appWindow, mainPage);
                Log.Info("Compose AppWindow@{0:X8} and MainPage@{1:X8}",
                    appWindow.GetHashCode(), mainPage.GetHashCode());

                mainPage.TryShowAsync();
            }
        }

        private void TabView_Main_TabStripDragOver(object sender, DragEventArgs args)
        {
            Log.Trace("MainPage@{0:X8}: TabView_Main_TabStripDragOver", GetHashCode());

            // Only TabViewItem is allowed to drop
            if (args.DataView.Properties.ContainsKey(TabDataIdentifier))
            {
                args.AcceptedOperation = DataPackageOperation.Move;
            }
        }

        private void TabView_Main_TabStripDrop(object sender, DragEventArgs args)
        {
            Log.Trace("MainPage@{0:X8}: TabView_Main_TabStripDrop", GetHashCode());

            if (args.DataView.Properties.TryGetValue(TabDataIdentifier, out object obj))
            {
                TabView destTabView = sender as TabView;
                IList<object> destTabItems = destTabView.TabItems;

                Int32 index = destTabItems.Count;
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
                Log.Info("Found the drop location, index = {0}", index);

                TabViewItem srcTabViewItem = obj as TabViewItem;
                (srcTabViewItem.Parent as muxc.Primitives.TabViewListView).Items.Remove(obj);
                Log.Info("Remove TabViewItem@{0:X8} from source page", srcTabViewItem.GetHashCode());

                destTabItems.Insert(index, srcTabViewItem);
                Log.Info("Insert to destTabItems at location {0}/{1}", index, destTabItems.Count);

                destTabView.SelectedItem = srcTabViewItem;
            }
        }

        private void TabView_Main_TabDragStarting(TabView sender, TabViewTabDragStartingEventArgs args)
        {
            Log.Trace("MainPage@{0:X8}: TabView_Main_TabDragStarting", GetHashCode());

            args.Data.Properties.Add(TabDataIdentifier, args.Tab);
            args.Data.RequestedOperation = DataPackageOperation.Move;
        }
        #endregion

        #region Button_Setting
        private void Button_Setting_Click(object sender, RoutedEventArgs args)
        {
            Log.Trace("MainPage@{0:X8}: Button_Setting_Click", GetHashCode());
            
            Tuple<MainPage, TabViewItem> tuple = GetTabById(SettingPageTabId);

            if (tuple != null)
            {
                tuple.Item1.TryShowAsync();
                tuple.Item1.TabView_Main.SelectedItem = tuple.Item2;
            }
            else
            {
                TabViewItem tabViewItem = CreateTabById(SettingPageTabId);
                TabView_Main.TabItems.Add(tabViewItem);
                TabView_Main.SelectedItem = tabViewItem;
                Log.Info("Add TabViewItem@{0:X8} into MainPage@{1:X8}",
                    tabViewItem.GetHashCode(), GetHashCode());
            }
        }
        #endregion
    }
}
