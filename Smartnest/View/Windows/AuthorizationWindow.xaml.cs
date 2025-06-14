using Smartnest.AppData;
using Smartnest.Model;
using Smartnest.View.Windows.Admin;
using Smartnest.View.Windows.Client;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            try
            {
                string phone = PhoneTb.Text;
                string password = PasswordTb.Text;

                User user = AuthorizationHelper.Login(phone, password);

                // Проверка роли
                if (user.RoleID == 1) // Клиент
                {
                    // Показываем клиентское окно
                    MenuWindow menuWindow = new MenuWindow();
                    this.Close();
                    menuWindow.Show();
                }
                else if (user.RoleID == 2) // Админ
                {
                    // Показываем админское окно
                    AdministratorWindow administratorWindow = new AdministratorWindow();
                    this.Close();
                    administratorWindow.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
