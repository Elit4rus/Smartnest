﻿using System.Windows;
using System.Windows.Controls;

namespace Smartnest.View.Pages.Client
{
    /// <summary>
    /// Логика взаимодействия для WaterPage.xaml
    /// </summary>
    public partial class WaterPage : Page
    {
        public WaterPage()
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
