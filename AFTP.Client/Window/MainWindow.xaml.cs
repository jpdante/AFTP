using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Threading;
using AFTP.Client.Enum;
using AFTP.Client.Model;
using AFTP.Client.Model.Client;
using AFTP.Client.Model.Config;
using AFTP.Client.Model.Utils;
using AFTP.Client.Util;
using AFTP.Client.View;
using AFTP.Client.View.Explorer;
using MahApps.Metro.Controls;

namespace AFTP.Client.Window {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow {
        private readonly ConfigLoader _configLoader;
        private readonly AftpClientConfig _config;
        private BaseClient _currentClient;

        public MainWindow() {
            InitializeComponent();
            var folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"AftpClient\");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            var configPath = System.IO.Path.Combine(folder, "config.json");
            _configLoader = new ConfigLoader(configPath);
            _config = _configLoader.LoadConfig();
            var hsl = new HorizontalServerOnly();
            Frame.Content = hsl;
            LogTraceListener.OnWrite += this.LogTraceListener_OnWrite;
            LogTraceListener.OnWriteLine += this.LogTraceListener_OnWriteLine;
        }

        private void LogTraceListener_OnWriteLine(object sender, string data) {
            this.LogBox.Dispatcher?.Invoke(DispatcherPriority.Normal, new Action(() => {
                LogBox.AppendText(data + Environment.NewLine);
                LogBox.ScrollToEnd();
            }));
        }

        private void LogTraceListener_OnWrite(object sender, string data) {
            this.LogBox.Dispatcher?.Invoke(DispatcherPriority.Normal, new Action(() => {
                LogBox.AppendText(data);
                LogBox.ScrollToEnd();
            }));
        }

        private void ManageConnections_Click(object sender, RoutedEventArgs e) {
            var serverManager = new ServerManager(_config.Servers);
            serverManager.ShowDialog();
            _config.Servers = serverManager.ServerSettings;
            _configLoader.SaveConfig(_config);
            if (serverManager.Connect) {
                var server = _config.Servers[serverManager.CurrentEdit];
                switch (server.Type) {
                    case ServerType.Aftp:
                        break;
                    case ServerType.Ftp:
                        if (server.Settings.TryGetValue(ServerSettingsType.LogonType, out var logonType)) {
                            switch (logonType) {
                                case "ask-password":
                                    // TODO: Ask for password
                                    break;
                                case "key-file":
                                    // TODO: Load from certificate
                                    break;
                                default:
                                    if (server.Settings.TryGetValue(ServerSettingsType.Username, out var username) &&
                                        server.Settings.TryGetValue(ServerSettingsType.Password, out var password)) {
                                        _currentClient = new BaseFtpClient(server.Host, server.Port, username, password);
                                    }
                                    break;
                            }
                        }
                        break;
                    case ServerType.Sftp:
                        break;
                }
                _currentClient?.Connect();
            }
        }

        private void NewTab_Click(object sender, RoutedEventArgs e) {

        }

        private void CloseTab_Click(object sender, RoutedEventArgs e) {

        }

        private void Exit_Click(object sender, RoutedEventArgs e) {
            Environment.Exit(0);
        }

        private void Settings_Click(object sender, RoutedEventArgs e) {

        }

        private void SetupWizard_Click(object sender, RoutedEventArgs e) {

        }

        private void Refresh_Click(object sender, RoutedEventArgs e) {

        }

        private void ProcessQueue_Click(object sender, RoutedEventArgs e) {

        }

        private void CancelOperation_Click(object sender, RoutedEventArgs e) {

        }

        private void Reconnect_Click(object sender, RoutedEventArgs e) {

        }

        private void Disconnect_Click(object sender, RoutedEventArgs e) {

        }

        private void SearchRemoteFiles_Click(object sender, RoutedEventArgs e) {

        }

        private void ShowHidenFiles_Click(object sender, RoutedEventArgs e) {

        }

        private void SearchUpdates_Click(object sender, RoutedEventArgs e) {

        }

        private void Help_Click(object sender, RoutedEventArgs e) {

        }

        private void Report_Click(object sender, RoutedEventArgs e) {

        }

        private void About_Click(object sender, RoutedEventArgs e) {

        }
    }
}
