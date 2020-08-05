using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Notebook_L.FileSystem;
using System.IO;

namespace Notebook_L.Settings
{
    public sealed partial class NotebooksPage : Page
    {
        public NotebooksPage()
        {
            this.InitializeComponent();
        }

        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            ListView listView = sender as ListView;
            ObservableCollection<Notebook> notebooks = Settings.Notebooks;

            for (Int32 i = 0; i < 100; i++)
            {
                notebooks.Add(new FileSystem.Notebook
                {
                    Name = "Item " + i.ToString(),
                    Source = FileSystem.Notebook.SourceType.Local,
                    Path = "/Path/To/Notebook"
                });
            }

            listView.ItemsSource = notebooks;
        }

        private void ListView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
