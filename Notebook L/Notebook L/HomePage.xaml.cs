using MetroLog;
using Notebook_L.FileSystem;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Notebook_L
{
    public sealed partial class HomePage : Page
    {
        private static readonly ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger<HomePage>();

        public Visibility BackVisibility => Folder == null ? Visibility.Collapsed : Visibility.Visible;
        public String FolderName => Folder == null ? "Notebooks" : Folder.Name;
        
        private IFolder Folder = null;

        public HomePage()
        {
            Log.Info(String.Format("Create object HomePage@{0:X8}", this.GetHashCode()));

            this.InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs args)
        {
            Log.Info("OnNavigatedFrom");
        }

        protected override void OnNavigatedTo(NavigationEventArgs args)
        {
            Log.Info("OnNavigatedTo");
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs args)
        {
            Log.Info("OnNavigatingFrom");
        }

        #region Sorting
        enum SortType
        {
            Name, Type, Date
        }

        public String SortName => SortMode.ToString("G");
        private SortType SortMode = SortType.Name;

        private void MenuFlyoutItem_Name_Click(object sender, RoutedEventArgs e)
        {
            SortMode = SortType.Name;
            SortContent();
        }

        private void MenuFlyoutItem_Type_Click(object sender, RoutedEventArgs e)
        {
            SortMode = SortType.Type;
            SortContent();
        }

        private void MenuFlyoutItem_Date_Click(object sender, RoutedEventArgs e)
        {
            SortMode = SortType.Date;
            SortContent();
        }

        private void SortContent()
        {
            DropDownButton_Sort.Content = SortName;
        }
        #endregion
    }
}
