using Smartnest.AppData;
using Smartnest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Smartnest.View.Windows.Admin
{
    /// <summary>
    /// Логика взаимодействия для AdministratorWindow.xaml
    /// </summary>
    public partial class AdministratorWindow : Window
    {
        public AdministratorWindow()
        {
            InitializeComponent();
            FrameHelper.adminFrame = AdminFrame;

            CurrentFullnameTbl.Text = AuthorizationHelper.currentUser.Surname + " " + AuthorizationHelper.currentUser.Name;

            LoadApplications();
        }

        private void LoadApplications()
        {
            try
            {
                using (var db = new SmartnestEntities())
                {
                    var applications = new List<ApplicationViewModel>();

                    // Получаем все заявки
                    var allApplications = db.Application.ToList();

                    foreach (var application in allApplications)
                    {
                        // Находим пользователя для этой заявки
                        var userApp = db.UserApplication.FirstOrDefault(ua => ua.ApplicationID == application.ID);
                        if (userApp == null) continue;

                        var user = db.User.Find(userApp.UserID);
                        if (user == null) continue;

                        // Получаем названия областей
                        var areaNames = db.AreaApplication
                            .Where(aa => aa.ApplicationID == application.ID)
                            .Join(db.Area,
                                aa => aa.AreaID,
                                a => a.ID,
                                (aa, a) => a.Title)
                            .ToList();

                        // Получаем названия оборудования
                        var equipmentNames = db.EquipmentApplication
                            .Where(ea => ea.ApplicationID == application.ID)
                            .Join(db.Equipment,
                                ea => ea.EquipmentID,
                                e => e.ID,
                                (ea, e) => e.Title)
                            .ToList();

                        applications.Add(new ApplicationViewModel
                        {
                            ApplicationId = application.ID,
                            FullName = $"{user.Surname} {user.Name} {user.Patronymic}".Trim(),
                            Phone = user.Phone,
                            Areas = string.Join(", ", areaNames),
                            Equipment = string.Join(", ", equipmentNames),
                            ApartmentArea = application.ApartmentArea,
                            Comment = application.Comment
                        });
                    }

                    ApplicationLv.ItemsSource = applications;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заявок: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Модель для отображения заявки в ListView
        public class ApplicationViewModel
        {
            public int ApplicationId { get; set; }
            public string FullName { get; set; }
            public string Phone { get; set; }
            public string Areas { get; set; }
            public string Equipment { get; set; }
            public string ApartmentArea { get; set; }
            public string Comment { get; set; }
        }

        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Для редактирования необходимо выбрать заявку из списка!");
            //ChangeWindow changeWindow = new ChangeWindow();
            //changeWindow.ShowDialog();
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
    }
}
