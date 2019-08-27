using System;
using System.Collections.Generic;
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

namespace AFTP.Client.View.ProtocolConfig {
    /// <summary>
    /// Interaction logic for FtpConfig.xaml
    /// </summary>
    public partial class FtpConfig : Page {
        public FtpConfig() {
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
    }
}
