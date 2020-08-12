using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
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

            // Logging some global information
            Log.Info(message: String.Format("The LocalFolder is {0}", ApplicationData.Current.LocalFolder.Path));

            Log.Info("Create object App");

            this.InitializeComponent();

            this.EnteredBackground += OnEnteredBackground;
            this.Suspending += OnSuspending;
            this.Resuming += OnResuming;
            this.LeavingBackground += OnLeavingBackground;
        }

        // Invoked when Navigation to a certain page fails
        void NavigationFailed(object sender, NavigationFailedEventArgs args)
        {
            throw new Exception("Failed to load Page " + args.SourcePageType.FullName);
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Log.Info(message: String.Format("OnLaunched"));
            Log.Info(message: String.Format("App previous execution state is {0}", args.PreviousExecutionState.ToString("G")));
            Log.Info(message: String.Format("App pre-launch activated is {0}", args.PrelaunchActivated));

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (!(Window.Current.Content is Frame rootFrame))
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += NavigationFailed;

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (args.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), args.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        private void OnEnteredBackground(object sender, EnteredBackgroundEventArgs args)
        {
            Log.Info(message: String.Format("OnEnteredBackground"));
            // TODO: Save all tabs
        }

        private void OnSuspending(object sender, SuspendingEventArgs args)
        {
            Log.Info(message: String.Format("OnSuspending"));
        }

        private void OnResuming(object sender, object args)
        {
            Log.Info(message: String.Format("OnResuming"));
        }

        private void OnLeavingBackground(object sender, LeavingBackgroundEventArgs args)
        {
            Log.Info(message: String.Format("OnLeavingBackground"));
            // TODO: Retrive all tabs and clear the cache
        }
    }
}
