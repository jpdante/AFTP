using System;
using System.Windows;
using AFTP.Client.View;
using MahApps.Metro.Controls;

namespace AFTP.Client.Window {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow {


        public MainWindow() {
            InitializeComponent();
            var hsl = new HorizontalServerOnly();
            Frame.Content = hsl;
        }

        private void ManageConnections_Click(object sender, RoutedEventArgs e) {
            var serverManager = new ServerManager();
            serverManager.ShowDialog();
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
