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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Notebook_L
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        #region TabView
        private void TabView_Initialization(object sender, RoutedEventArgs e)
        {
            TabView tabView = sender as TabView;

            // Create a default TabItem
            tabView.TabItems.Add(TabView_Create());
        }

        private TabViewItem TabView_Create()
        {
            TabViewItem newItem = new TabViewItem
            {
                Header = "New Tab",
                IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource()
                {
                    Symbol = Symbol.Document
                }
            };

            Frame frame = new Frame();

            frame.Navigate(typeof(SamplePage1));

            newItem.Content = frame;
            return newItem;
        }

        private void TabView_ButtonClick_Add(TabView sender, object args)
        {
            sender.TabItems.Add(TabView_Create());
        }

        private void TabView_ButtonClick_Close(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            sender.TabItems.Remove(args.Tab);
        }
        #endregion
    }
}
