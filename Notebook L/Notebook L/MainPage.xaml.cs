﻿using MetroLog;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
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

        #region Constructor
        public MainPage()
        {
            Log.Info("Create object MainPage");

            Log.Info("Add this into Instances");
            Instances.Add(this);

            Window = null;
            this.InitializeComponent();

            ApplicationView.GetForCurrentView().Consolidated += (ApplicationView appView, ApplicationViewConsolidatedEventArgs args) =>
            {
                Log.Info("Remove this from Instances");
                Instances.Remove(this);

                Log.Info("Trigger OnNavigatingFrom for each page.");
                foreach(muxc.TabViewItem tabViewItem in TabView_Main.TabItems)
                {
                    (tabViewItem.Content as Frame).Navigate(typeof(BlankPage));
                }
            };
        }

        public MainPage(AppWindow window)
        {
            Log.Info("Create object MainPage with AppWindow");

            Log.Info("Add this into Instances");
            Instances.Add(this);

            Window = window;
            this.InitializeComponent();

            Window.Closed += (AppWindow win, AppWindowClosedEventArgs args) =>
            {
                Log.Info("Remove this from Instances");
                Instances.Remove(this);

                Log.Info("Trigger OnNavigatingFrom for each page.");
                foreach (muxc.TabViewItem tabViewItem in TabView_Main.TabItems)
                {
                    (tabViewItem.Content as Frame).Navigate(typeof(BlankPage));
                }
            };
        }
        #endregion

        #region Tab ID
        public const String HomePageTabId = "HomePage-BvlzpOeJ";
        public const String SettingPageTabId = "SettingPage-fkXRAjq1";
        public static String DocumentPageTabId(String id)
        {
            return "DocumentPage-" + id;
        }

        public const String TabDataIdentifier = "iLoj4PTo";
        #endregion

        #region TabViewItem
        private muxc.TabViewItem CreateHomePageTab()
        {
            muxc.TabViewItem tabViewItem = new muxc.TabViewItem
            {
                Tag = HomePageTabId,
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
                Tag = SettingPageTabId,
                Header = "Settings",
                IconSource = new muxc.SymbolIconSource()
                {
                    Symbol = Symbol.Setting
                }
            };

            tabViewItem.Content = new Frame();
            (tabViewItem.Content as Frame).Navigate(typeof(BlankPage));

            return tabViewItem;
        }
        #endregion

        #region EventHandler TabView_Main
        private void TabView_Main_Loaded(object sender, RoutedEventArgs args)
        {
            Log.Info("TabView_Main_Loaded");

            muxc.TabView tabView = sender as muxc.TabView;
            if (tabView.TabItems.Count == 0)
            {
                tabView.TabItems.Add(CreateHomePageTab());
            }
        }

        private async void TabView_Main_TabItemsChanged(muxc.TabView sender, IVectorChangedEventArgs args)
        {
            Log.Info("TabView_Main_TabItemsChanged");

            if (sender.TabItems.Count == 0)
            {
                if (Window != null)
                {
                    Log.Info("Close the AppWindow");
                    await Window.CloseAsync();
                }
                else
                {
                    Log.Info("Close the main ApplicationView");
                    await ApplicationView.GetForCurrentView().TryConsolidateAsync();
                }
            }
        }

        private void TabView_Main_AddTabButtonClick(muxc.TabView sender, object args)
        {
            Log.Info("TabView_Main_AddTabButtonClick");

            muxc.TabViewItem tabViewItem = CreateHomePageTab();
            sender.TabItems.Add(tabViewItem);
        }

        private void TabView_Main_TabCloseRequested(muxc.TabView sender, muxc.TabViewTabCloseRequestedEventArgs args)
        {
            Log.Info("TabView_Main_TabCloseRequested");

            (args.Tab.Content as Frame).Navigate(typeof(BlankPage));
            sender.TabItems.Remove(args.Tab);
        }

        private async void TabView_Main_TabDroppedOutside(muxc.TabView sender, muxc.TabViewTabDroppedOutsideEventArgs args)
        {
            Log.Info("TabView_Main_TabDroppedOutside");

            // Cannot remove the last TabViewItem in a window, otherwise it will result
            // the lose of TabViewItem
            if (this.TabView_Main.TabItems.Count > 1)
            {
                Log.Info("Create a new AppWindow");

                AppWindow newWindow = await AppWindow.TryCreateAsync();
                MainPage newPage = new MainPage(newWindow);
                ElementCompositionPreview.SetAppWindowContent(newWindow, newPage);

                Log.Info("Move the TabViewItem from old window to new window");
                this.TabView_Main.TabItems.Remove(args.Tab);
                newPage.TabView_Main.TabItems.Add(args.Tab);

                Log.Info("Show the new AppWindow");
                await newWindow.TryShowAsync();
            }
        }

        private void TabView_Main_TabStripDragOver(object sender, DragEventArgs args)
        {
            Log.Info("TabView_Main_TabStripDragOver");

            // Only TabViewItem is allowed to drop
            if (args.DataView.Properties.ContainsKey(TabDataIdentifier))
            {
                args.AcceptedOperation = DataPackageOperation.Move;
            }
        }

        private void TabView_Main_TabStripDrop(object sender, DragEventArgs args)
        {
            Log.Info("TabView_Main_TabStripDrop");

            if (args.DataView.Properties.TryGetValue(TabDataIdentifier, out object obj))
            {
                Log.Info("Get the list of TabViewItems from destination window");
                muxc.TabView destTabView = sender as muxc.TabView;
                IList<object> destTabItems = destTabView.TabItems;

                Log.Info("Find the drop location of new TabViewItem");
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

                Log.Info("Remove TabViewItem from source window");
                muxc.TabViewItem srcTabViewItem = obj as muxc.TabViewItem;
                (srcTabViewItem.Parent as muxc.Primitives.TabViewListView).Items.Remove(obj);

                if (index < 0)
                {
                    Log.Info("Insert to the end ({0} TabViewItems in the list)", destTabItems.Count);
                    destTabItems.Add(srcTabViewItem);
                }
                else
                {
                    Log.Info(String.Format("Insert to location {0} ({1} TabViewItems in the list)", index, destTabItems.Count));
                    destTabItems.Insert(index, srcTabViewItem);
                }

                destTabView.SelectedItem = srcTabViewItem;
            }
        }

        private void TabView_Main_TabDragStarting(muxc.TabView sender, muxc.TabViewTabDragStartingEventArgs args)
        {
            Log.Info("TabView_Main_TabDragStarting");

            args.Data.Properties.Add(TabDataIdentifier, args.Tab);
            args.Data.RequestedOperation = DataPackageOperation.Move;
        }
        #endregion

        private void Button_Setting_Click(object sender, RoutedEventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
