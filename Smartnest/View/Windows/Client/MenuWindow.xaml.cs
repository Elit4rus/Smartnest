using Smartnest.AppData;
using System.Windows;

namespace Smartnest.View.Windows.Client
{
    /// <summary>
    /// Логика взаимодействия для MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
            FrameHelper.clientFrame = ClientFrame;
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти из аккаунта?", "Предупреждение", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                AuthorizationWindow authorizationWindow = new AuthorizationWindow();
                Close();
                authorizationWindow.Show();
            }
        }

        private void SecuritySystemWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            ClientFrame.Navigate(new View.Pages.Client.SecurityPage());
        }

        private void VideoSurveillanceWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            ClientFrame.Navigate(new View.Pages.Client.CameraPage());
        }

        private void WaterLeakageControlWindowBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CurtainControlWindowBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ControlLightingWindowBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ManageHeatingWindowBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
