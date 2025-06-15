using Smartnest.AppData;
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
        private List<ApplicationView> applications = new List<ApplicationView>();
        public AdministratorWindow()
        {
            InitializeComponent();
            FrameHelper.adminFrame = AdminFrame;

            CurrentFullnameTbl.Text = AuthorizationHelper.currentUser.Surname + " " + AuthorizationHelper.currentUser.Name;

            LoadApplications(); // Загружаем заявки при открытии окна
        }

        private void LoadApplications()
        {
            applications.Clear(); // Очищаем текущий список

            // Получаем все заявки из базы данных
            var dbApplications = App.context.Application.ToList();

            foreach (var app in dbApplications)
            {
                // Получаем пользователя, связанного с заявкой
                var user = App.context.User.FirstOrDefault(u => u.ID == app.UserID);

                // Получаем список областей для заявки
                var areaIds = App.context.AreaApplication
                    .Where(aa => aa.ApplicationID == app.ID)
                    .Select(aa => aa.AreaID)
                    .ToList();

                var areas = new List<string>();
                foreach (var id in areaIds)
                {
                    var area = App.context.Area.FirstOrDefault(a => a.ID == id);
                    if (area != null) areas.Add(area.Title);
                }

                // Получаем список оборудования для заявки
                var equipmentIds = App.context.EquipmentApplication
                    .Where(ea => ea.ApplicationID == app.ID)
                    .Select(ea => ea.EquipmentID)
                    .ToList();

                var equipments = new List<string>();
                foreach (var id in equipmentIds)
                {
                    var equipment = App.context.Equipment.FirstOrDefault(e => e.ID == id);
                    if (equipment != null) equipments.Add(equipment.Title);
                }

                // Создаем объект для отображения
                applications.Add(new ApplicationView
                {
                    ID = app.ID,
                    FullName = user != null ? user.GetFullName() : "Не указано",
                    Phone = user?.Phone ?? "Не указано",
                    Areas = string.Join(", ", areas),
                    Equipment = string.Join(", ", equipments),
                    ApartmentArea = app.ApartmentArea,
                    Comment = app.Comment ?? "Без комментария"
                });
            }

            // Устанавливаем источник данных для ListView
            ApplicationLv.ItemsSource = applications;
            ApplicationLv.Items.Refresh();
        }

        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicationLv.SelectedItem == null)
            {
                MessageBox.Show("Выберите заявку для редактирования!");
                return;
            }

            var selectedApp = (ApplicationView)ApplicationLv.SelectedItem;

            // Открываем окно редактирования с передачей выбранной заявки
            ChangeWindow changeWindow = new ChangeWindow(selectedApp.ID);
            changeWindow.ShowDialog();

            // Обновляем список после закрытия окна редактирования
            LoadApplications();
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
