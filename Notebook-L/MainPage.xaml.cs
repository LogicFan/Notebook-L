using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.DataTransfer;
using muxc = Microsoft.UI.Xaml.Controls;

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

            Frame frame = new Frame();
            frame.Navigate(typeof(HomePage));
            newItem.Content = frame;

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

        private void TabView_AddTabButtonClick(muxc.TabView sender, object args)
        {
            sender.TabItems.Add(CreateDefaultTabViewItem());
        }

        private void TabView_TabCloseRequested(muxc.TabView sender, muxc.TabViewTabCloseRequestedEventArgs args)
        {
            sender.TabItems.Remove(args.Tab);

            if (sender.TabItems.Count == 0)
            {
                Application.Current.Exit();
            }
        }

        private void TabView_TabDroppedOutside(muxc.TabView sender, muxc.TabViewTabDroppedOutsideEventArgs args)
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
