using MetroLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
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

        private async void Button_Diagnostic_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Log.Trace("AboutPage@{0:X8}: Button_Diagnostic_Click", GetHashCode());
            FileSavePicker fileSavePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            fileSavePicker.FileTypeChoices.Add("Compressed Folder", new List<String>() { ".zip" });
            fileSavePicker.SuggestedFileName = "Diagnostic Log";

            StorageFile logFile = await fileSavePicker.PickSaveFileAsync();
            if (logFile != null)
            {
                CachedFileManager.DeferUpdates(logFile);

                using (Stream logFileStream = (await logFile.OpenAsync(FileAccessMode.ReadWrite)).AsStream())
                using (Stream logDataStream = await LogManagerFactory.DefaultLogManager.GetCompressedLogs())
                {
                    await logDataStream.CopyToAsync(logFileStream);
                }

                await CachedFileManager.CompleteUpdatesAsync(logFile);
            }
        }
    }
}
