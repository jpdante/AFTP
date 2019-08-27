using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Shapes;
using AFTP.Client.Model;
using AFTP.Client.Model.Server;
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
        private AftpClientConfig _config;

        public MainWindow() {
            InitializeComponent();
            var folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"AftpClient\");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            var configPath = System.IO.Path.Combine(folder, "config.json");
            _configLoader = new ConfigLoader(configPath);
            _config = _configLoader.LoadConfig();
            var hsl = new HorizontalServerOnly();
            Frame.Content = hsl;
        }

        private void ManageConnections_Click(object sender, RoutedEventArgs e) {
            var serverManager = new ServerManager(_config.Servers);
            serverManager.ShowDialog();
            _config.Servers = serverManager.ServerSettings;
            _configLoader.SaveConfig(_config);
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
