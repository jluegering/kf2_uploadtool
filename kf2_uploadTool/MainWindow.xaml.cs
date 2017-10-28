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

namespace kf2_uploadTool
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Helper.config configuration;

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

        private void uploadToRedirect()
        {
            string filePath = configuration.redirect.path + "test.kfm"; //for testing

            FtpClient ftpClient = new FtpClient(configuration.redirect.address, configuration.redirect.port, configuration.redirect.username, configuration.redirect.password);
            ftpClient.Connect();
            ftpClient.UploadFile(tb_MapPath.Text, filePath, FtpExists.Overwrite);
        }
    }
}
