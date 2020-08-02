using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.DataTransfer;
using muxc = Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;

namespace Notebook_L
{
    public sealed partial class MainPage : Page
    {
        private const String DataIdentifier = "MainPage_TabView_TabViewItem";

        public MainPage()
        {
            this.InitializeComponent();
        }

        #region TabView
        // Used by: TabView_AddTabButtonClick
        //          TabView_Loaded
        private muxc.TabViewItem CreateDefaultTabViewItem()
        {
            muxc.TabViewItem newItem = new muxc.TabViewItem
            {
                Header = "Home",
                IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource()
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

        private async void TabView_TabItemsChanged(muxc.TabView sender, Windows.Foundation.Collections.IVectorChangedEventArgs args)
        {
            if (sender.TabItems.Count == 0)
            {
                await ApplicationView.GetForCurrentView().TryConsolidateAsync();
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
            CoreApplicationView newView = CoreApplication.CreateNewView();
            Int32 Id = 0;

            this.TabView_Document.TabItems.Remove(args.Tab);

            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                MainPage newPage = new MainPage();
                Frame frame = new Frame();

                newPage.TabView_Document.TabItems.Add(args.Tab);

                frame.Navigate(typeof(MainPage), newPage);
                Window.Current.Content = frame;
                Window.Current.Activate();
                Id = ApplicationView.GetForCurrentView().Id;
            });

            await ApplicationViewSwitcher.TryShowAsStandaloneAsync(Id);
        }

        private void TabView_TabStripDragOver(object sender, DragEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TabView_TabStripDrop(object sender, DragEventArgs e)
        {
            throw new NotImplementedException();
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
