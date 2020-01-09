using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AFTP.Client.Enum;
using System.Windows.Forms;
using AFTP.Client.Model.Config;
using MessageBox = System.Windows.MessageBox;

namespace AFTP.Client.View.ProtocolConfig {
    /// <summary>
    /// Interaction logic for SftpConfig.xaml
    /// </summary>
    public partial class FtpConfig : Page {
        private readonly ServerConfig _serverConfig;
        private static readonly Regex PortRegex = new Regex("[^0-9]+");

        public FtpConfig(ServerConfig server) {
            InitializeComponent();
            this._serverConfig = server;
        }

        private void Username_TextChanged(object sender, TextChangedEventArgs e) {
            if (_serverConfig == null) return;
            _serverConfig.Settings[ServerSettingsType.Username] = Username.Text;
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e) {
            if (_serverConfig == null) return;
            _serverConfig.Settings[ServerSettingsType.Password] = Password.Password;
        }

        private void LogonTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (_serverConfig == null) return;
            switch (((ComboBoxItem)LogonTypeComboBox.SelectedItem).Tag.ToString()) {
                case "anonymous":
                    Username.IsEnabled = false;
                    Password.IsEnabled = false;
                    Username.Text = "anonymous";
                    Password.Password = "anonymous";
                    Password.Visibility = Visibility.Visible;
                    PasswordLabel.Visibility = Visibility.Visible;
                    KeyFile.Visibility = Visibility.Hidden;
                    KeyFileButton.Visibility = Visibility.Hidden;
                    KeyFileLabel.Visibility = Visibility.Hidden;
                    break;
                case "normal":
                    Username.IsEnabled = true;
                    Password.IsEnabled = true;
                    if (_serverConfig.Settings.TryGetValue(ServerSettingsType.Username, out var username)) Username.Text = username;
                    if (_serverConfig.Settings.TryGetValue(ServerSettingsType.Password, out var password)) Password.Password = password;
                    Password.Visibility = Visibility.Visible;
                    PasswordLabel.Visibility = Visibility.Visible;
                    KeyFile.Visibility = Visibility.Hidden;
                    KeyFileButton.Visibility = Visibility.Hidden;
                    KeyFileLabel.Visibility = Visibility.Hidden;
                    break;
                case "ask-password":
                    Username.IsEnabled = true;
                    Password.IsEnabled = false;
                    if (_serverConfig.Settings.TryGetValue(ServerSettingsType.Username, out var username2)) Username.Text = username2;
                    Password.Password = "";
                    Password.Visibility = Visibility.Visible;
                    PasswordLabel.Visibility = Visibility.Visible;
                    KeyFile.Visibility = Visibility.Hidden;
                    KeyFileButton.Visibility = Visibility.Hidden;
                    KeyFileLabel.Visibility = Visibility.Hidden;
                    break;
                case "key-file":
                    Username.IsEnabled = true;
                    Password.IsEnabled = false;
                    if (_serverConfig.Settings.TryGetValue(ServerSettingsType.Username, out var username3)) Username.Text = username3;
                    Password.Password = "";
                    Password.Visibility = Visibility.Hidden;
                    PasswordLabel.Visibility = Visibility.Hidden;
                    KeyFile.Visibility = Visibility.Visible;
                    KeyFileButton.Visibility = Visibility.Visible;
                    KeyFileLabel.Visibility = Visibility.Visible;
                    break;
                default:
                    Username.IsEnabled = false;
                    Password.IsEnabled = false;
                    Username.Text = "anonymous";
                    Password.Password = "anonymous";
                    Password.Visibility = Visibility.Visible;
                    PasswordLabel.Visibility = Visibility.Visible;
                    KeyFile.Visibility = Visibility.Hidden;
                    KeyFileButton.Visibility = Visibility.Hidden;
                    KeyFileLabel.Visibility = Visibility.Hidden;
                    break;
            }
            _serverConfig.Settings[ServerSettingsType.LogonType] = ((ComboBoxItem)LogonTypeComboBox.SelectedItem).Tag.ToString();
        }

        private void Port_TextChanged(object sender, TextChangedEventArgs e) {
            if (Port.Text.Length == 0) return;
            var port = int.Parse(Port.Text);
            if (port <= 0) Port.Text = "1";
            if (port > 65535) Port.Text = "65535";
            if (_serverConfig == null) return;
            _serverConfig.Port = port;
        }

        private void Port_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            e.Handled = PortRegex.IsMatch(e.Text);
        }

        private void Host_TextChanged(object sender, TextChangedEventArgs e) {
            if (_serverConfig == null) return;
            _serverConfig.Host = Host.Text;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            if (_serverConfig == null) return;
            if (_serverConfig.Settings.TryGetValue(ServerSettingsType.LogonType, out var logonTypeRaw) &&
                _serverConfig.Settings.TryGetValue(ServerSettingsType.Username, out var username) &&
                _serverConfig.Settings.TryGetValue(ServerSettingsType.Password, out var password)) {
                switch (logonTypeRaw) {
                    case "anonymous":
                        Username.IsEnabled = false;
                        Password.IsEnabled = false;
                        Username.Text = "anonymous";
                        Password.Password = "anonymous";
                        Password.Visibility = Visibility.Visible;
                        PasswordLabel.Visibility = Visibility.Visible;
                        KeyFile.Visibility = Visibility.Hidden;
                        KeyFileButton.Visibility = Visibility.Hidden;
                        KeyFileLabel.Visibility = Visibility.Hidden;
                        LogonTypeComboBox.SelectedItem = LogonTypeComboBox.Items[0];
                        break;
                    case "normal":
                        Username.IsEnabled = true;
                        Password.IsEnabled = true;
                        Username.Text = username;
                        Password.Password = password;
                        Password.Visibility = Visibility.Visible;
                        PasswordLabel.Visibility = Visibility.Visible;
                        KeyFile.Visibility = Visibility.Hidden;
                        KeyFileButton.Visibility = Visibility.Hidden;
                        KeyFileLabel.Visibility = Visibility.Hidden;
                        LogonTypeComboBox.SelectedItem = LogonTypeComboBox.Items[1];
                        break;
                    case "ask-password":
                        Username.IsEnabled = true;
                        Password.IsEnabled = false;
                        Username.Text = username;
                        Password.Password = "";
                        Password.Visibility = Visibility.Visible;
                        PasswordLabel.Visibility = Visibility.Visible;
                        KeyFile.Visibility = Visibility.Hidden;
                        KeyFileButton.Visibility = Visibility.Hidden;
                        KeyFileLabel.Visibility = Visibility.Hidden;
                        LogonTypeComboBox.SelectedItem = LogonTypeComboBox.Items[2];
                        break;
                    case "key-file":
                        Username.IsEnabled = true;
                        Password.IsEnabled = false;
                        Username.Text = username;
                        Password.Password = "";
                        if (_serverConfig.Settings.TryGetValue(ServerSettingsType.KeyFile, out var keyFile)) KeyFile.Text = keyFile;
                        Password.Visibility = Visibility.Hidden;
                        PasswordLabel.Visibility = Visibility.Hidden;
                        KeyFile.Visibility = Visibility.Visible;
                        KeyFileButton.Visibility = Visibility.Visible;
                        KeyFileLabel.Visibility = Visibility.Visible;
                        LogonTypeComboBox.SelectedItem = LogonTypeComboBox.Items[3];
                        break;
                    default:
                        Username.IsEnabled = false;
                        Password.IsEnabled = false;
                        Username.Text = "anonymous";
                        Password.Password = "anonymous";
                        Password.Visibility = Visibility.Visible;
                        PasswordLabel.Visibility = Visibility.Visible;
                        KeyFile.Visibility = Visibility.Hidden;
                        KeyFileButton.Visibility = Visibility.Hidden;
                        KeyFileLabel.Visibility = Visibility.Hidden;
                        LogonTypeComboBox.SelectedItem = LogonTypeComboBox.Items[0];
                        break;
                }
            } else {
                Username.Text = "";
                Password.Password = "";
                LogonTypeComboBox.SelectedIndex = 0;
                _serverConfig.Settings[ServerSettingsType.LogonType] = ((ComboBoxItem)LogonTypeComboBox.SelectedItem).Tag.ToString();
            }
            if (_serverConfig.Host != null) Host.Text = _serverConfig.Host;
            if (_serverConfig.Port <= 0) _serverConfig.Port = 21;
            if (_serverConfig.Port > 65535) _serverConfig.Port = 65535;
            Port.Text = _serverConfig.Port.ToString();

            if (_serverConfig.Settings.TryGetValue(ServerSettingsType.ServerType, out var serverTypeRaw)) {
                switch (serverTypeRaw) {
                    case "default":
                        ServerTypeComboBox.SelectedItem = ServerTypeComboBox.Items[0];
                        break;
                    case "unix":
                        ServerTypeComboBox.SelectedItem = ServerTypeComboBox.Items[1];
                        break;
                    case "windows":
                        ServerTypeComboBox.SelectedItem = ServerTypeComboBox.Items[2];
                        break;
                    default:
                        ServerTypeComboBox.SelectedItem = ServerTypeComboBox.Items[0];
                        break;
                }
            }
            if (_serverConfig.Settings.TryGetValue(ServerSettingsType.DefaultLocalDirectory, out var defaultLocalDirectory)) {
                DefaultLocalDirectory.Text = defaultLocalDirectory;
            }
            if (_serverConfig.Settings.TryGetValue(ServerSettingsType.DefaultRemoteDirectory, out var defaultRemoteDirectory)) {
                DefaultRemoteDirectory.Text = defaultRemoteDirectory;
            }
            if (_serverConfig.Settings.TryGetValue(ServerSettingsType.LimitSimultaneousConnections, out var limitSimultaneousConnections)) {
                LimitSimultaneousConnectionsCb.IsChecked = limitSimultaneousConnections.Equals("true");
                if (_serverConfig.Settings.TryGetValue(ServerSettingsType.MaxConnections, out var maxConnections)) {
                    MaxNumberOfConnections.Text = maxConnections;
                }
            }
            if (_serverConfig.Settings.TryGetValue(ServerSettingsType.TransferMode, out var transferModeRaw)) {
                switch (transferModeRaw) {
                    case "default":
                        ActiveModeRb.IsChecked = false;
                        PassiveModeRb.IsChecked = false;
                        DefaultModeRb.IsChecked = true;
                        break;
                    case "active":
                        PassiveModeRb.IsChecked = false;
                        DefaultModeRb.IsChecked = false;
                        ActiveModeRb.IsChecked = true;
                        break;
                    case "passive":
                        ActiveModeRb.IsChecked = false;
                        DefaultModeRb.IsChecked = false;
                        PassiveModeRb.IsChecked = true;
                        break;
                    default:
                        ActiveModeRb.IsChecked = false;
                        PassiveModeRb.IsChecked = false;
                        DefaultModeRb.IsChecked = true;
                        break;
                }
            } else {
                _serverConfig.Settings[ServerSettingsType.TransferMode] = "default";
                ActiveModeRb.IsChecked = false;
                PassiveModeRb.IsChecked = false;
                DefaultModeRb.IsChecked = true;
            }
            if (_serverConfig.Settings.TryGetValue(ServerSettingsType.CharsetEncoding, out var charsetEncoding)) {
                switch (charsetEncoding) {
                    case "auto-detect":
                        AsciiRb.IsChecked = false;
                        Utf8Rb.IsChecked = false;
                        Utf16Rb.IsChecked = false;
                        CustomEncodingRb.IsChecked = false;
                        AutoDetectRb.IsChecked = true;
                        CustomEncodingTb.Text = "";
                        break;
                    case "ASCII":
                        Utf8Rb.IsChecked = false;
                        Utf16Rb.IsChecked = false;
                        CustomEncodingRb.IsChecked = false;
                        AutoDetectRb.IsChecked = false;
                        AsciiRb.IsChecked = true;
                        CustomEncodingTb.Text = "";
                        break;
                    case "UTF-8":
                        AsciiRb.IsChecked = false;
                        AutoDetectRb.IsChecked = false;
                        Utf16Rb.IsChecked = false;
                        CustomEncodingRb.IsChecked = false;
                        Utf8Rb.IsChecked = true;
                        CustomEncodingTb.Text = "";
                        break;
                    case "UTF-16":
                        AsciiRb.IsChecked = false;
                        Utf8Rb.IsChecked = false;
                        CustomEncodingRb.IsChecked = false;
                        AutoDetectRb.IsChecked = false;
                        Utf16Rb.IsChecked = true;
                        CustomEncodingTb.Text = "";
                        break;
                    default:
                        AsciiRb.IsChecked = false;
                        Utf8Rb.IsChecked = false;
                        Utf16Rb.IsChecked = false;
                        AutoDetectRb.IsChecked = false;
                        CustomEncodingRb.IsChecked = true;
                        CustomEncodingTb.Text = charsetEncoding;
                        break;
                }
            } else {
                _serverConfig.Settings[ServerSettingsType.CharsetEncoding] = "auto-detect";
                AsciiRb.IsChecked = false;
                Utf8Rb.IsChecked = false;
                Utf16Rb.IsChecked = false;
                CustomEncodingRb.IsChecked = false;
                AutoDetectRb.IsChecked = true;
                CustomEncodingTb.Text = "";
            }
            if (_serverConfig.Settings.TryGetValue(ServerSettingsType.Encryption, out var encryption)) {
                switch (encryption) {
                    case "try-explicit-tls":
                        ServerTypeComboBox.SelectedItem = ServerTypeComboBox.Items[0];
                        break;
                    case "require-explicit-tls":
                        ServerTypeComboBox.SelectedItem = ServerTypeComboBox.Items[1];
                        break;
                    case "require-implicit-tls":
                        ServerTypeComboBox.SelectedItem = ServerTypeComboBox.Items[2];
                        break;
                    case "plain":
                        ServerTypeComboBox.SelectedItem = ServerTypeComboBox.Items[3];
                        break;
                    default:
                        ServerTypeComboBox.SelectedItem = ServerTypeComboBox.Items[0];
                        break;
                }
            } else {
                ServerTypeComboBox.SelectedItem = ServerTypeComboBox.Items[0];
                _serverConfig.Settings[ServerSettingsType.Encryption] = ((ComboBoxItem)EncryptionComboBox.SelectedItem).Tag.ToString();
            }
        }

        private void KeyFile_Changed(object sender, TextChangedEventArgs e) {
            if (_serverConfig == null) return;
            _serverConfig.Settings[ServerSettingsType.KeyFile] = KeyFile.Text;
        }

        private void KeyFileButton_Click(object sender, RoutedEventArgs e) {
            var fileDialog = new OpenFileDialog {
                Filter = @"PEM file|*.pem|All files|*.*"
            };
            if (fileDialog.ShowDialog() == DialogResult.OK) {
                KeyFile.Text = fileDialog.FileName;
            }
        }

        private void MaxNumberOfConnections_TextChanged(object sender, TextChangedEventArgs e) {
            if (MaxNumberOfConnections.Text.Length == 0) return;
            var maxConnections = int.Parse(MaxNumberOfConnections.Text);
            if (maxConnections <= 0) MaxNumberOfConnections.Text = "1";
            if (maxConnections > 30) MaxNumberOfConnections.Text = "30";
            if (_serverConfig == null) return;
            _serverConfig.Settings[ServerSettingsType.MaxConnections] = maxConnections.ToString();
            MaxNumberOfConnectionsSlider.Value = maxConnections;
        }

        private void ServerTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (_serverConfig == null) return;
            _serverConfig.Settings[ServerSettingsType.ServerType] = ((ComboBoxItem)ServerTypeComboBox.SelectedItem).Tag.ToString();
        }

        private void DefaultLocalDirectory_Changed(object sender, TextChangedEventArgs e) {
            if (_serverConfig == null) return;
            _serverConfig.Settings[ServerSettingsType.DefaultLocalDirectory] = DefaultLocalDirectory.Text;
        }

        private void DefaultLocalDirectoryButton_Click(object sender, RoutedEventArgs e) {
            var folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == DialogResult.OK) {
                DefaultLocalDirectory.Text = folderDialog.SelectedPath;
            }
        }

        private void DefaultRemoteDirectory_Changed(object sender, TextChangedEventArgs e) {
            if (_serverConfig == null) return;
            _serverConfig.Settings[ServerSettingsType.DefaultRemoteDirectory] = DefaultRemoteDirectory.Text;
        }

        private void DefaultModeRb_Checked(object sender, RoutedEventArgs e) {
            if (_serverConfig == null) return;
            _serverConfig.Settings[ServerSettingsType.TransferMode] = "default";
        }

        private void ActiveModeRb_Checked(object sender, RoutedEventArgs e) {
            if (_serverConfig == null) return;
            _serverConfig.Settings[ServerSettingsType.TransferMode] = "active";
        }

        private void PassiveModeRb_Checked(object sender, RoutedEventArgs e) {
            if (_serverConfig == null) return;
            _serverConfig.Settings[ServerSettingsType.TransferMode] = "passive";
        }

        private void LimitSimultaneousConnectionsCb_Unchecked(object sender, RoutedEventArgs e) {
            if (_serverConfig == null) return;
            _serverConfig.Settings[ServerSettingsType.LimitSimultaneousConnections] = "false";
            MaxNumberOfConnections.IsEnabled = false;
            MaxNumberOfConnectionsSlider.IsEnabled = false;
            MaxNumberOfConnections.Text = "1";
            MaxNumberOfConnectionsSlider.Value = 1;
        }

        private void LimitSimultaneousConnectionsCb_Checked(object sender, RoutedEventArgs e) {
            if (_serverConfig == null) return;
            _serverConfig.Settings[ServerSettingsType.LimitSimultaneousConnections] = "true";
            MaxNumberOfConnections.IsEnabled = true;
            MaxNumberOfConnectionsSlider.IsEnabled = true;
        }

        private void MaxNumberOfConnectionsSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (_serverConfig == null) return;
            MaxNumberOfConnections.Text = ((int)e.NewValue).ToString();
        }

        private void AutoDetectRb_Checked(object sender, RoutedEventArgs e) {
            if (_serverConfig == null) return;
            CustomEncodingLabel.IsEnabled = false;
            CustomEncodingTb.IsEnabled = false;
            _serverConfig.Settings[ServerSettingsType.CharsetEncoding] = "auto-detect";
        }

        private void AsciiRb_Checked(object sender, RoutedEventArgs e) {
            if (_serverConfig == null) return;
            CustomEncodingLabel.IsEnabled = false;
            CustomEncodingTb.IsEnabled = false;
            _serverConfig.Settings[ServerSettingsType.CharsetEncoding] = "ASCII";
        }

        private void Utf8Rb_Checked(object sender, RoutedEventArgs e) {
            if (_serverConfig == null) return;
            CustomEncodingLabel.IsEnabled = false;
            CustomEncodingTb.IsEnabled = false;
            _serverConfig.Settings[ServerSettingsType.CharsetEncoding] = "UTF-8";
        }

        private void Utf16Rb_Checked(object sender, RoutedEventArgs e) {
            if (_serverConfig == null) return;
            CustomEncodingLabel.IsEnabled = false;
            CustomEncodingTb.IsEnabled = false;
            _serverConfig.Settings[ServerSettingsType.CharsetEncoding] = "UTF-16";
        }

        private void CustomEncodingRb_Checked(object sender, RoutedEventArgs e) {
            if (_serverConfig == null) return;
            CustomEncodingLabel.IsEnabled = true;
            CustomEncodingTb.IsEnabled = true;
            _serverConfig.Settings[ServerSettingsType.CharsetEncoding] = "";
        }

        private void CustomEncodingTb_TextChanged(object sender, TextChangedEventArgs e) {
            if (_serverConfig == null) return;
            _serverConfig.Settings[ServerSettingsType.CharsetEncoding] = CustomEncodingTb.Text;
        }

        private void EncryptionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (_serverConfig == null) return;
            _serverConfig.Settings[ServerSettingsType.Encryption] = ((ComboBoxItem)EncryptionComboBox.SelectedItem).Tag.ToString();
        }
    }
}
