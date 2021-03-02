using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Threading;
using AFTP.Client.Enum;
using AFTP.Client.Model;
using AFTP.Client.Model.Client;
using AFTP.Client.Model.Config;
using AFTP.Client.Model.Util;
using AFTP.Client.Model.View;
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
        private FileExplorer _currentFileExplorer;
        private BaseClient _currentClient;
        private ServerConfig _currentServerConfig;

        public MainWindow() {
            InitializeComponent();
            var folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"AftpClient\");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            var configPath = System.IO.Path.Combine(folder, "config.json");
            _configLoader = new ConfigLoader(configPath);
            _config = _configLoader.LoadConfig() ?? new AftpClientConfig();
            _currentFileExplorer = new HorizontalServerOnly();
            Frame.Content = _currentFileExplorer;
            LogTraceListener.OnWrite += this.LogTraceListener_OnWrite;
            LogTraceListener.OnWriteLine += this.LogTraceListener_OnWriteLine;
            _currentFileExplorer.OnGoToLocalPath += this._currentFileExplorer_OnGoToLocalPath;
            _currentFileExplorer.OnGoToRemotePath += CurrentFileExplorerOnOnGoToRemotePath;
        }

        private async void CurrentFileExplorerOnOnGoToRemotePath(object sender, string path) {
            if (_currentClient == null || !_currentClient.IsConnected) return;
            _currentFileExplorer.UpdateRemotePath(path);
            var remoteEntries = await _currentClient.ListDirectory(path, CancellationToken.None);
            _currentFileExplorer.UpdateRemoteEntries(remoteEntries);
        }

        private void _currentFileExplorer_OnGoToLocalPath(object sender, string path) {
            
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
            if (!serverManager.Connect) return;
            var server = _config.Servers[serverManager.CurrentEdit];
            switch (server.Type) {
                case ServerType.Aftp:
                    SetupAFtp(server);
                    break;
                case ServerType.Ftp:
                    SetupFtp(server);
                    break;
                case ServerType.Sftp:
                    SetupSFtp(server);
                    break;
            }
            _currentServerConfig = server;
            BindEvents();
            _currentClient?.Configure(server.Settings);
            _currentClient?.Connect();
        }

        
        private void SetupAFtp(ServerConfig serverConfig) {
        }

        private void SetupFtp(ServerConfig serverConfig) {
            if (serverConfig.Settings.TryGetValue(ServerSettingsType.LogonType, out var logonType)) {
                switch (logonType) {
                    case "ask-password":
                        // TODO: Ask for password
                        break;
                    case "key-file":
                        // TODO: Load from certificate
                        break;
                    default:
                        if (serverConfig.Settings.TryGetValue(ServerSettingsType.Username, out var username) &&
                            serverConfig.Settings.TryGetValue(ServerSettingsType.Password, out var password)) {
                            _currentClient = new BaseFtpClient(serverConfig.Host, serverConfig.Port, username, password);
                        }
                        break;
                }
            }
        }

        private void SetupSFtp(ServerConfig serverConfig) {
            if (!serverConfig.Settings.TryGetValue(ServerSettingsType.LogonType, out string logonType)) return;
            string username = null;
            string password = null;
            switch (logonType) {
                case "ask-password":
                    // TODO: Ask for password
                    break;
                case "key-file":
                    if (serverConfig.Settings.TryGetValue(ServerSettingsType.Username, out username) &&
                        serverConfig.Settings.TryGetValue(ServerSettingsType.Password, out password) &&
                        serverConfig.Settings.TryGetValue(ServerSettingsType.KeyFile, out var keyFile)) {
                        _currentClient = new BaseSFtpClient(serverConfig.Host, serverConfig.Port, username, keyFile, password);
                    }
                    break;
                default:
                    if (serverConfig.Settings.TryGetValue(ServerSettingsType.Username, out username) &&
                        serverConfig.Settings.TryGetValue(ServerSettingsType.Password, out password)) {
                        _currentClient = new BaseSFtpClient(serverConfig.Host, serverConfig.Port, username, password);
                    }
                    break;
            }
        }

        private void BindEvents() {
            _currentClient.OnConnected += CurrentClientOnOnConnected;
            _currentClient.OnDisconnected += CurrentClientOnOnDisconnected;
        }

        private void CurrentClientOnOnDisconnected(object sender) {
            RefreshMenuButton.IsEnabled = false;
        }

        private async void CurrentClientOnOnConnected(object sender) {
            RefreshMenuButton.IsEnabled = true;
            _currentServerConfig.Settings.TryGetValue(ServerSettingsType.DefaultRemoteDirectory, out var path);
            if (string.IsNullOrEmpty(path)) {
                path = "/";
            }
            _currentFileExplorer.UpdateRemotePath(path);
            var remoteEntries = await _currentClient.ListDirectory(path, CancellationToken.None);
            _currentFileExplorer.UpdateRemoteEntries(remoteEntries);
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
