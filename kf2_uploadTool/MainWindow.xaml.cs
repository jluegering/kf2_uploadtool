using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FluentFTP;
using System.Collections;
using System.Threading;

namespace kf2_uploadTool
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Helper.config configuration;
        private List<Helper.maps> files;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            settings settingsWindow = new settings(this, configuration);
            settingsWindow.Show();
            this.IsEnabled = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            configuration = null;

            if (File.Exists("config.json"))
            {
                using (StreamReader file = File.OpenText("config.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    configuration = (Helper.config)serializer.Deserialize(file, typeof(Helper.config));
                }
            }
        }

        private void uploadMaps(IProgress<double> progress)
        {
            string remoteServerPath = String.Empty;
            string remoteRedirectPath = String.Empty;

            FtpClient ftpClientRedirect = new FtpClient(configuration.redirect.address, configuration.redirect.port, configuration.redirect.username, configuration.redirect.password);
            ftpClientRedirect.Connect();

            int fileCount = files.Count;

            for (int i = 0; i < fileCount; i++)
            {
                Dispatcher.Invoke(new Action(() => lbl_fileProgress.Content = "Redirect map " + (i + 1) + " of " + fileCount + ": " + files[i].name));
                remoteRedirectPath = configuration.redirect.path + files[i].name;
                ftpClientRedirect.UploadFile(files[i].path, remoteRedirectPath, FtpExists.Overwrite, false, FtpVerify.None, progress);
                Dispatcher.Invoke(new Action(() => pb_complete.Value++));
            }

            ftpClientRedirect.Disconnect();
            
            FtpClient ftpClientServer = new FtpClient(configuration.server.address, configuration.server.port, configuration.server.username, configuration.server.password);
            ftpClientServer.Connect();

            for (int i = 0; i < fileCount; i++)
            {
                Dispatcher.Invoke(new Action(() => lbl_fileProgress.Content = "Server map " + (i + 1) + " of " + fileCount + ": " + files[i].name));
                remoteServerPath = configuration.server.path + "BrewedPC/Maps/" + files[i].name;
                ftpClientServer.UploadFile(files[i].path, remoteServerPath, FtpExists.Overwrite, false, FtpVerify.None, progress);
                Dispatcher.Invoke(new Action(() => pb_complete.Value++));
            }

            ftpClientServer.Disconnect();
        }

        private void btn_browseMap_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog browseDialog = new Microsoft.Win32.OpenFileDialog();

            browseDialog.DefaultExt = ".kfm";
            browseDialog.Filter = "KF2 Maps|*.kfm";
            browseDialog.Multiselect = true;
            
            Nullable<bool> result = browseDialog.ShowDialog();
            
            if (result == true)
            {
                files = new List<Helper.maps>();

                for (int i = 0; i < browseDialog.FileNames.Length; i++)
                {
                    files.Add(new Helper.maps(browseDialog.FileNames[i], browseDialog.SafeFileNames[i]));
                    lb_files.Items.Clear();
                    lb_files.Items.Add(browseDialog.FileNames[i]);
                }
            }
        }

        private async void btn_start_Click(object sender, RoutedEventArgs e)
        {
            pb_file.Maximum = 100;
            pb_file.Minimum = 0;

            pb_complete.Maximum = (files.Count * 2); //TODO: Adding one after INI editing works
            pb_complete.Minimum = 0;

            var progressFile = new Progress<double>(percent =>
            {
                pb_file.Value = percent;
            });

            btn_start.IsEnabled = false;
            btn_browseMap.IsEnabled = false;
            menu_topBar.IsEnabled = false;

            // DoProcessing is run on the thread pool.
            await Task.Run(() => uploadMaps(progressFile));

            btn_start.IsEnabled = true;
            btn_browseMap.IsEnabled = true;
            menu_topBar.IsEnabled = true;
        }
    }
}
