using Smartnest.AppData;
using Smartnest.View.Windows.Client;
using System;
using System.Windows;
using System.Xml.Linq;

namespace Smartnest.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void LoginHp_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow authorizationWindow = new AuthorizationWindow();
            Close();
            authorizationWindow.Show();
        }

        private void RegBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string surname = SurnameTb.Text;
                string name = NameTb.Text;
                string patronymic = PatronymicTb.Text;
                string phone = PhoneTb.Text;
                string password = PasswordTb.Text;

                // Проверка обязательных полей
                if (string.IsNullOrWhiteSpace(surname) || string.IsNullOrWhiteSpace(name) ||
                    string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Заполните все обязательные поля!");
                    return;
                }

                AuthorizationHelper.RegisterUser(surname, name, patronymic, phone, password);
                MessageBox.Show("Регистрация успешна!");
                MenuWindow menuWindow = new MenuWindow();
                menuWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}
