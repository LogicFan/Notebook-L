using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MetroLog;
using MetroLog.Targets;
using Windows.Storage;

namespace Notebook_L
{
    sealed partial class App : Application
    {
        // The logger for the class
        private static readonly ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger<App>();

        public App()
        {
            // Setting up the logger
            LogManagerFactory.DefaultConfiguration.AddTarget(LogLevel.Trace, LogLevel.Fatal, new StreamingFileTarget());
            GlobalCrashHandler.Configure();
#if DEBUG
            Log.Info("The LocalFolder is {0}", ApplicationData.Current.LocalFolder.Path);
#endif

            Log.Trace("Create object App@{0:X8}", GetHashCode());

            InitializeComponent();

            EnteredBackground += OnEnteredBackground;
            Suspending += OnSuspending;
            Resuming += OnResuming;
            LeavingBackground += OnLeavingBackground;
        }

        // Invoked when Navigation to a certain page fails
        private void NavigationFailed(object sender, NavigationFailedEventArgs args)
        {
            throw new Exception("Failed to load Page " + args.SourcePageType.FullName);
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Log.Trace("App@{0:X8}: OnLaunched", GetHashCode());
            
            Log.Info("PreviousExecutionState = {0}, PrelaunchActivated = {1}",
                args.PreviousExecutionState.ToString("G"),
                args.PrelaunchActivated);

            if (!(Window.Current.Content is Frame rootFrame))
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += NavigationFailed;

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load all saved tabs
                }

                Window.Current.Content = rootFrame;
            }

            if (args.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(MainPage), args.Arguments);
                }
                Window.Current.Activate();
            }
        }

        private void OnEnteredBackground(object sender, EnteredBackgroundEventArgs args)
        {
            Log.Trace("App@{0:X8}: OnEnteredBackground", GetHashCode());
            // TODO: Save all tabs
        }

        private void OnSuspending(object sender, SuspendingEventArgs args)
        {
            Log.Trace("App@{0:X8}: OnSuspending", GetHashCode());
            // TODO: Save all tabs
        }

        private void OnResuming(object sender, object args)
        {
            Log.Trace("App@{0:X8}: OnResuming", GetHashCode());
            // TODO: Load all tabs
        }

        private void OnLeavingBackground(object sender, LeavingBackgroundEventArgs args)
        {
            Log.Info("App@{0:X8}: OnLeavingBackground", GetHashCode());
            // TODO: Load all tabs
        }
    }
}
