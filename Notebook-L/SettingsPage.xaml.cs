using Windows.UI.Xaml.Controls;
using muxc = Microsoft.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Notebook_L
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
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
    }
}
