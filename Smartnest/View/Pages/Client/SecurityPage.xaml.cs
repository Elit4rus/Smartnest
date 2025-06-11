using Smartnest.AppData;
using System.Windows;
using System.Windows.Controls;

namespace Smartnest.View.Pages.Client
{
    /// <summary>
    /// Логика взаимодействия для SecurityPage.xaml
    /// </summary>
    public partial class SecurityPage : Page
    {
        public SecurityPage()
        {
            InitializeComponent();
            FrameHelper.applicationFrame = ApplicationFrame;
        }

        private void SendAppBtn_Click(object sender, RoutedEventArgs e)
        {
            ApplicationFrame.Navigate(new View.Pages.Client.ApplicationPage());
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Content = null;
        }
    }
}
