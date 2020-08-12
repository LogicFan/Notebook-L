using MetroLog;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Notebook_L
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        // The logger for the class
        private static readonly ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger<HomePage>();
        private static Int64 Counter = 0;

        public HomePage()
        {
            Log.Info(String.Format("Create object BlankPage with Counter = {0}", Counter));

            this.InitializeComponent();
            
            TextBlock.Text = "HomePage" + Counter.ToString();
            Counter += 1;
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
    }
}
