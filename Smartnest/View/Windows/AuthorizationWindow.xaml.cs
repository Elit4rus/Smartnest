using System.Windows;

namespace Smartnest.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        private void RegistrationHp_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            Close();
            registrationWindow.Show();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
