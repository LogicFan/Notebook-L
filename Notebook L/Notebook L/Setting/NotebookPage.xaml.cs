using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Notebook_L.Setting
{
    public sealed partial class NotebookPage : Page
    {
        public NotebookPage()
        {
            this.InitializeComponent();
        }

        private async void ListView_Notebook_Loaded(object sender, RoutedEventArgs args)
        {
            ListView listView = sender as ListView;
            listView.ItemsSource = await Setting.GetNotebooksAsync();
        }
    }
}
