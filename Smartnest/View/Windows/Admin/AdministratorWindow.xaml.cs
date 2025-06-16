using ClosedXML.Excel;
using Microsoft.Win32;
using Smartnest.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Smartnest.View.Windows.Admin
{
    /// <summary>
    /// Логика взаимодействия для AdministratorWindow.xaml
    /// </summary>
    public partial class AdministratorWindow : System.Windows.Window
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
            try
            {
                // Создаем диалог сохранения файла
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    FileName = $"Отчет_по_заявкам_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                    Title = "Сохранить отчет"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    // Создаем новую книгу Excel
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Заявки");

                        // Заголовки столбцов
                        worksheet.Cell(1, 1).Value = "ФИО";
                        worksheet.Cell(1, 2).Value = "Телефон";
                        worksheet.Cell(1, 3).Value = "Области";
                        worksheet.Cell(1, 4).Value = "Оборудование";
                        worksheet.Cell(1, 5).Value = "Площадь (кв.м)";
                        worksheet.Cell(1, 6).Value = "Комментарий";
                        worksheet.Cell(1, 7).Value = "Дата создания";

                        // Форматирование заголовков
                        var headerRange = worksheet.Range("A1:G1");
                        headerRange.Style.Font.Bold = true;
                        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

                        // Заполняем данные
                        int row = 2;
                        foreach (ApplicationView app in applications)
                        {
                            worksheet.Cell(row, 1).Value = app.FullName;
                            worksheet.Cell(row, 2).Value = app.Phone;
                            worksheet.Cell(row, 3).Value = app.Areas;
                            worksheet.Cell(row, 4).Value = app.Equipment;
                            worksheet.Cell(row, 5).Value = app.ApartmentArea;
                            worksheet.Cell(row, 6).Value = app.Comment;
                            worksheet.Cell(row, 7).Value = DateTime.Now.ToString("g");
                            row++;
                        }

                        // Автоподбор ширины столбцов
                        worksheet.Columns().AdjustToContents();

                        // Сохраняем файл
                        workbook.SaveAs(saveFileDialog.FileName);
                    }

                    MessageBox.Show($"Отчет успешно сохранен!\nПуть: {saveFileDialog.FileName}",
                                    "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании отчета: {ex.Message}",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChangeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicationLv.SelectedItem == null)
            {
                MessageBox.Show("Выберите заявку для редактирования!");
                return;
            }

            var selectedApp = (ApplicationView)ApplicationLv.SelectedItem;
            ChangeWindow changeWindow = new ChangeWindow(selectedApp.ID);
            changeWindow.Closed += (s, args) => LoadApplications(); // Обновляем при закрытии
            changeWindow.ShowDialog();
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
