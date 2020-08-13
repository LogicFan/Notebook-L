using System;
using MetroLog;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Notebook_L.Setting
{
    public sealed partial class AboutPage : Page
    {
        private static readonly ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger<AboutPage>();

        public AboutPage()
        {
            Log.Info(String.Format("Create object AboutPage@{0:X8}", this.GetHashCode()));
            this.InitializeComponent();
        }

        private async void Button_Diagnostic_Click(object sender, RoutedEventArgs args)
        {
            Log.Info(String.Format("@{0:X8}: Button_Diagnostic_Click", this.GetHashCode()));

            FileSavePicker fileSavePicker = new FileSavePicker();
            fileSavePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            fileSavePicker.FileTypeChoices.Add("Compressed Folder", new List<string>() { ".zip" });
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
