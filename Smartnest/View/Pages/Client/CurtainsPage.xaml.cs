using System.Windows;
using System.Windows.Controls;

namespace Smartnest.View.Pages.Client
{
    /// <summary>
    /// Логика взаимодействия для CurtainsPage.xaml
    /// </summary>
    public partial class CurtainsPage : Page
    {
        public CurtainsPage()
        {
            InitializeComponent();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Content = null;
        }

        private void SendAppBtn_Click(object sender, RoutedEventArgs e)
        {
            ApplicationFrame.Navigate(new View.Pages.Client.ApplicationPage());
        }
    }
}
