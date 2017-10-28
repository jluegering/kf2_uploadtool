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
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace kf2_uploadTool
{
    /// <summary>
    /// Interaktionslogik für settings.xaml
    /// </summary>
    public partial class settings : Window
    {
        private MainWindow mainWin;
        private Helper.config configuration;

        public settings(MainWindow mainWin, Helper.config configuration)
        {
            InitializeComponent();
            this.mainWin = mainWin;
            this.configuration = configuration;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (configuration != null)
            {
                tb_serverAddress.Text = configuration.server.address;
                tb_serverPort.Text = configuration.server.port.ToString();
                tb_serverUsername.Text = configuration.server.username;
                tb_serverPassword.Text = configuration.server.password;
                tb_serverPath.Text = configuration.server.path;

                tb_redirectAddress.Text = configuration.redirect.address;
                tb_redirectPort.Text = configuration.redirect.port.ToString();
                tb_redirectUsername.Text = configuration.redirect.username;
                tb_redirectPassword.Text = configuration.redirect.password;
                tb_redirectPath.Text = configuration.redirect.path;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            mainWin.IsEnabled = true;
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            Helper.config configuration = new Helper.config();
            Helper.Server configServer = new Helper.Server();
            Helper.Redirect configRedirect = new Helper.Redirect();

            configServer.address = tb_serverAddress.Text;
            configServer.port = Int32.Parse(tb_serverPort.Text);
            configServer.username = tb_serverUsername.Text;
            configServer.password = tb_serverPassword.Text;
            configServer.path = tb_serverPath.Text;

            configRedirect.address = tb_redirectAddress.Text;
            configRedirect.port = Int32.Parse(tb_redirectPort.Text);
            configRedirect.username = tb_redirectUsername.Text;
            configRedirect.password = tb_redirectPassword.Text;
            configRedirect.path = tb_redirectPath.Text;

            configuration.server = configServer;
            configuration.redirect = configRedirect;

            using (StreamWriter file = File.CreateText("config.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, configuration);
            }

            this.Close();
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
