using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using AFTP.Client.Enum;
using AFTP.Client.Model;
using AFTP.Client.Model.Server;

namespace AFTP.Client.View.ProtocolConfig {
    /// <summary>
    /// Interaction logic for AftpConfig.xaml
    /// </summary>
    public partial class AftpConfig : Page {
        private readonly ServerConfig _serverConfig;
        private static readonly Regex PortRegex = new Regex("[^0-9]+");

        public AftpConfig(ServerConfig serverConfig) {
            _serverConfig = serverConfig;
            InitializeComponent();
        }

        private void AnonymousLogin_Unchecked(object sender, RoutedEventArgs e) {
            Username.IsEnabled = true;
            Password.IsEnabled = true;
            Username.Text = "";
            Password.Password = "";
        }

        private void AnonymousLogin_Checked(object sender, RoutedEventArgs e) {
            Username.IsEnabled = false;
            Password.IsEnabled = false;
            Username.Text = "anonymous";
            Password.Password = "anonymous";
        }

        private void Username_TextChanged(object sender, TextChangedEventArgs e) {
            _serverConfig.Settings[ServerSettingsType.Username] = Username.Text;
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e) {
            _serverConfig.Settings[ServerSettingsType.Password] = Password.Password;
        }

        private void Host_TextChanged(object sender, TextChangedEventArgs e) {
            _serverConfig.Host = Host.Text;
        }

        private void Port_TextChanged(object sender, TextChangedEventArgs e) {
            if (Port.Text.Length == 0) return;
            var port = int.Parse(Port.Text);
            if (port < 0) Port.Text = "0";
            if (port > 65535) Port.Text = "65535";
            _serverConfig.Port = port;
        }

        private void Port_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            e.Handled = PortRegex.IsMatch(e.Text);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            if (_serverConfig.Settings.TryGetValue(ServerSettingsType.Username, out var username) && _serverConfig.Settings.TryGetValue(ServerSettingsType.Password, out var password)) {
                if (username.Equals("anonymous") && password.Equals("anonymous")) {
                    Username.IsEnabled = false;
                    Password.IsEnabled = false;
                    Username.Text = "anonymous";
                    Password.Password = "anonymous";
                    AnonymousLogin.IsChecked = true;
                } else {
                    Username.Text = username;
                    Password.Password = password;
                }
            } else {
                Username.Text = "";
                Password.Password = "";
            }
            if(_serverConfig.Host != null) Host.Text = _serverConfig.Host;
            Port.Text = _serverConfig.Port.ToString();
        }
    }
}
