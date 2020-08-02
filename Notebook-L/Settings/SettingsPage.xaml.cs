using AppUIBasics.SamplePages;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using muxc = Microsoft.UI.Xaml.Controls;

namespace Notebook_L
{
    namespace Settings
    {
        public sealed partial class SettingsPage : Page
        {
            private MainPage m_mainPage;

            public SettingsPage()
            {
                this.InitializeComponent();
            }

            public SettingsPage(MainPage mainPage)
            {
                this.m_mainPage = mainPage;
                this.InitializeComponent();
            }

            private void NavigationView_BackRequested(muxc.NavigationView sender, muxc.NavigationViewBackRequestedEventArgs args)
            {
                this.Frame.Navigate(typeof(MainPage), m_mainPage);
            }

            private void NavigationView_SelectionChanged(muxc.NavigationView sender, muxc.NavigationViewSelectionChangedEventArgs args)
            {
                muxc.NavigationViewItem selectedItem = args.SelectedItem as muxc.NavigationViewItem;
                if (selectedItem != null)
                {
                    string itemTag = selectedItem.Tag as string;
                    sender.Header = itemTag;
                    Type itemPage = Type.GetType("AppUIBasics.SamplePages.SamplePage1");
                    //string pageName = "AppUIBasics.SamplePages." + selectedItemTag;
                    //Type pageType = Type.GetType(pageName);
                    Frame_Settings.Navigate(itemPage);
                }
            }

            private void NavigationView_Loaded(object sender, RoutedEventArgs e)
            {
                muxc.NavigationView navigationView = sender as muxc.NavigationView;
                navigationView.SelectedItem = navigationView.MenuItems[0];
            }
        }
    }
}
