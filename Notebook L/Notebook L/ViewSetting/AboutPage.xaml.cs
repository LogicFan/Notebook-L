﻿using MetroLog;
using System;
using System.Reflection;
using Windows.UI.Xaml.Controls;

namespace Notebook_L.ViewSetting
{
    public sealed partial class AboutPage : Page
    {
        private static readonly ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger<AboutPage>();

        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;

        public String SoftwareName => "Notebook L";
        public String Platform => "Universal Windows Platform";
        public String BuildNumber => Version.ToString(3);

        public AboutPage()
        {
            Log.Trace("Create object AboutPage@{0:X8}", GetHashCode());
            InitializeComponent();
        }

        private void Button_Diagnostic_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Log.Trace("AboutPage@{0:X8}: Button_Diagnostic_Click", GetHashCode());
            throw new NotImplementedException();
        }
    }
}
