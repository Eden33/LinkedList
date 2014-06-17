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
using System.Windows.Shapes;
using Client.Resource;

namespace Client
{
    /// <summary>
    /// Interaction logic for LoginDialog.xaml
    /// </summary>
    public partial class LoginDialog : Window
    {
        public LoginDialog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Instance.LoginName = loginTB.Text;
            bool loginSuccess = ResourceManager.Instance.Login();
            if(loginSuccess == false && ResourceManager.Instance.IsConnected == false)
            {
                MessageBox.Show("Service connection couldn't be established.\nCheck service is running and try again.", 
                    "Service not running", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (loginSuccess == false && ResourceManager.Instance.IsConnected == true)
            {
                MessageBox.Show("Login failed please try again.", "Service login failed", MessageBoxButton.OK, MessageBoxImage.Information);                
            }
            else
            {
                MainWindow mainWin = new MainWindow();
                mainWin.Show();
                mainWin.loginNameLbl.Content = "Logged in as: "+ResourceManager.Instance.LoginName;
                Close();
            }
        }

        private void loginTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(String.IsNullOrEmpty(loginTB.Text) == false)
            {
                loginBtn.IsEnabled = true;
            }
            else
            {
                loginBtn.IsEnabled = false;
            }
        }
    }
}
