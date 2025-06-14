using Smartnest.Model;
using Smartnest.View.Windows.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Smartnest.View.Pages.Client
{
    /// <summary>
    /// Логика взаимодействия для ApplicationPage.xaml
    /// </summary>
    public partial class ApplicationPage : Page
    {
        public ApplicationPage()
        {
            InitializeComponent();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Content = null;
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            // 1. Проверка заполнения обязательных полей
            if (string.IsNullOrWhiteSpace(AreaTb.Text))
            {
                MessageBox.Show("Укажите площадь помещения");
                return;
            }

            // 2. Проверка, что выбрана хотя бы одна область
            if (!IsAnyAreaChecked())
            {
                MessageBox.Show("Выберите хотя бы одну область");
                return;
            }

            // 3. Создание новой заявки
            var newApplication = new Model.Application
            {
                ApartmentArea = AreaTb.Text,
                Comment = CommentTb.Text
            };

            // 4. Добавление заявки в базу данных
            App.context.Application.Add(newApplication);
            App.context.SaveChanges(); // Сохраняем, чтобы получить ID заявки

            // 5. Добавление выбранных областей
            AddSelectedAreas(newApplication.ID);

            // 6. Добавление выбранного оборудования
            AddSelectedEquipment(newApplication.ID);

            // 7. Связывание заявки с текущим пользователем
            LinkApplicationToCurrentUser(newApplication.ID);

            // 8. Сообщение об успехе и закрытие страницы
            MessageBox.Show("Заявка успешно создана!");
            this.NavigationService.Content = null;
        }
        // Проверка, выбрана ли хотя бы одна область
        private bool IsAnyAreaChecked()
        {
            return KitchenCb.IsChecked == true ||
                   HallwayCb.IsChecked == true ||
                   LivingRoomCb.IsChecked == true ||
                   AtelierCb.IsChecked == true ||
                   BathroomCb.IsChecked == true ||
                   ToiletCb.IsChecked == true ||
                   BedroomCb.IsChecked == true ||
                   BasementCb.IsChecked == true ||
                   BalconyCb.IsChecked == true ||
                   DressingRoomCb.IsChecked == true ||
                   YardCb.IsChecked == true ||
                   RoofCb.IsChecked == true;
        }

        // Добавление выбранных областей в базу данных
        private void AddSelectedAreas(int applicationId)
        {
            // Словарь для связи чекбоксов с ID областей
            var areaCheckboxes = new[]
            {
                (CheckBox: KitchenCb, AreaId: 1),    // Кухня
                (CheckBox: HallwayCb, AreaId: 2),    // Коридор
                (CheckBox: LivingRoomCb, AreaId: 3), // Гостиная
                (CheckBox: AtelierCb, AreaId: 4),    // Студия
                (CheckBox: BathroomCb, AreaId: 5),   // Ванная
                (CheckBox: ToiletCb, AreaId: 6),     // Туалет
                (CheckBox: BedroomCb, AreaId: 12),   // Спальня
                (CheckBox: BasementCb, AreaId: 11),  // Подвал
                (CheckBox: BalconyCb, AreaId: 9),    // Балкон
                (CheckBox: DressingRoomCb, AreaId: 7), // Гардеробная
                (CheckBox: YardCb, AreaId: 8),       // Двор
                (CheckBox: RoofCb, AreaId: 10)       // Крыша
            };

            foreach (var (CheckBox, AreaId) in areaCheckboxes)
            {
                if (CheckBox.IsChecked == true)
                {
                    App.context.AreaApplication.Add(new AreaApplication
                    {
                        AreaID = AreaId,
                        ApplicationID = applicationId
                    });
                }
            }
            App.context.SaveChanges();
        }

        // Добавление выбранного оборудования в базу данных
        private void AddSelectedEquipment(int applicationId)
        {
            // Словарь для связи чекбоксов с ID оборудования
            var equipmentCheckboxes = new[]
            {
                (CheckBox: CameraCb, EquipmentId: 1),    // Видеонаблюдение
                (CheckBox: SecurityCb, EquipmentId: 2),   // Система безопасности
                (CheckBox: WaterCb, EquipmentId: 3),      // Контроль протечки воды
                (CheckBox: CurtainCb, EquipmentId: 4),    // Управление шторами
                (CheckBox: LightCb, EquipmentId: 5),      // Управление освещением
                (CheckBox: HeatingCb, EquipmentId: 6)     // Управление отоплением
            };

            foreach (var (CheckBox, EquipmentId) in equipmentCheckboxes)
            {
                if (CheckBox.IsChecked == true)
                {
                    App.context.EquipmentApplication.Add(new EquipmentApplication
                    {
                        EquipmentID = EquipmentId,
                        ApplicationID = applicationId
                    });
                }
            }
            App.context.SaveChanges();
        }

        // Связывание заявки с текущим пользователем
        private void LinkApplicationToCurrentUser(int applicationId)
        {
            // Получаем ID текущего пользователя (здесь нужно реализовать логику получения текущего пользователя)
            // В данном примере предполагаем, что ID пользователя хранится в статическом классе или свойствах приложения
            int currentUserId = 1; // Замените на реальный способ получения ID текущего пользователя

            // Находим максимальный ID в таблице UserApplication
            int maxId = App.context.UserApplication.Any() ? App.context.UserApplication.Max(ua => ua.ID) : 0;

            App.context.UserApplication.Add(new UserApplication
            {
                ID = maxId + 1,
                UserID = currentUserId,
                ApplicationID = applicationId
            });
            App.context.SaveChanges();
        }
    }
}
