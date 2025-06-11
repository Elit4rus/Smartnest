using Smartnest.AppData;
using System.Windows;
using System.Windows.Controls;

namespace Smartnest.View.Pages.Client
{
    /// <summary>
    /// Логика взаимодействия для CameraPage.xaml
    /// </summary>
    public partial class CameraPage : Page
    {
        public CameraPage()
        {
            InitializeComponent();
            FrameHelper.applicationFrame = ApplicationFrame;
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
