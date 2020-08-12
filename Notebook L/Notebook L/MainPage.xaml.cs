using MetroLog;
using System;
using System.Collections.Generic;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Notebook_L
{
    public sealed partial class MainPage : Page
    {
        // The logger for the class
        private static readonly ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger<MainPage>();
        private static readonly ISet<MainPage> Instances = new HashSet<MainPage>();

        public MainPage()
        {
            Log.Info("Create object MainPage");

            Log.Info("Add this into Instances");
            Instances.Add(this);

            this.InitializeComponent();

            ApplicationView.GetForCurrentView().Consolidated += (ApplicationView appView, ApplicationViewConsolidatedEventArgs args) =>
            {
                Log.Info("Remove this from Instances");
                Instances.Remove(this);
            };
        }

        private void TabView_Main_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void TabView_Main_TabItemsChanged(Microsoft.UI.Xaml.Controls.TabView sender, IVectorChangedEventArgs args)
        {

        }

        private void TabView_Main_AddTabButtonClick(Microsoft.UI.Xaml.Controls.TabView sender, object args)
        {

        }

        private void TabView_Main_TabCloseRequested(Microsoft.UI.Xaml.Controls.TabView sender, Microsoft.UI.Xaml.Controls.TabViewTabCloseRequestedEventArgs args)
        {

        }

        private void TabView_Main_TabDroppedOutside(Microsoft.UI.Xaml.Controls.TabView sender, Microsoft.UI.Xaml.Controls.TabViewTabDroppedOutsideEventArgs args)
        {

        }

        private void TabView_Main_TabStripDragOver(object sender, DragEventArgs e)
        {

        }

        private void TabView_Main_TabStripDrop(object sender, DragEventArgs e)
        {

        }

        private void TabView_Main_TabDragStarting(Microsoft.UI.Xaml.Controls.TabView sender, Microsoft.UI.Xaml.Controls.TabViewTabDragStartingEventArgs args)
        {

        }

        private void Button_Setting_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
