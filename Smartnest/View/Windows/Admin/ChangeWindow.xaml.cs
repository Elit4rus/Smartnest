using Smartnest.AppData;
using Smartnest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Application = Smartnest.Model.Application;

namespace Smartnest.View.Windows.Admin
{
    /// <summary>
    /// Логика взаимодействия для ChangeWindow.xaml
    /// </summary>
    public partial class ChangeWindow : Window
    {
        private int applicationId;
        private Application currentApplication;
        private User currentUser;
        public ChangeWindow(int appId)
        {
            InitializeComponent();

            applicationId = appId;
            LoadApplicationData(); // Загружаем данные заявки
        }

        // Метод для загрузки данных заявки
        private void LoadApplicationData()
        {
            // Получаем заявку из базы данных
            currentApplication = App.context.Application.FirstOrDefault(a => a.ID == applicationId);

            if (currentApplication == null)
            {
                MessageBox.Show("Заявка не найдена!");
                Close();
                return;
            }

            // Получаем пользователя, связанного с заявкой
            currentUser = App.context.User.FirstOrDefault(u => u.ID == currentApplication.UserID);

            if (currentUser != null)
            {
                FullnameTb.Text = currentUser.GetFullName();
                PhoneTb.Text = currentUser.Phone;
            }

            // Заполняем данные о площади и комментарии
            AreaTb.Text = currentApplication.ApartmentArea;
            CommentTb.Text = currentApplication.Comment;

            // Получаем выбранные области для заявки
            var areaIds = App.context.AreaApplication
                .Where(aa => aa.ApplicationID == applicationId)
                .Select(aa => aa.AreaID)
                .ToList();

            // Устанавливаем чекбоксы областей
            SetCheckBoxes(areaIds, new List<CheckBox>
            {
                KitchenCb, HallwayCb, LivingRoomCb, AtelierCb,
                BathroomCb, ToiletCb, BedroomCb, BasementCb,
                BalconyCb, DressingRoomCb, YardCb, RoofCb
            }, App.context.Area.ToList());

            // Получаем выбранное оборудование для заявки
            var equipmentIds = App.context.EquipmentApplication
                .Where(ea => ea.ApplicationID == applicationId)
                .Select(ea => ea.EquipmentID)
                .ToList();

            // Устанавливаем чекбоксы оборудования
            SetCheckBoxes(equipmentIds, new List<CheckBox>
            {
                CameraCb, SecurityCb, WaterCb, CurtainCb, LightCb, HeatingCb
            }, App.context.Equipment.ToList());
        }

        private void SetCheckBoxes(List<int> equipmentIds, List<CheckBox> checkBoxes, List<Equipment> equipment)
        {
            throw new NotImplementedException();
        }

        private void SetCheckBoxes(List<int> areaIds, List<CheckBox> checkBoxes, List<Area> areas)
        {
            throw new NotImplementedException();
        }

        // Метод для установки состояния чекбоксов
        private void SetCheckBoxes(List<int> selectedIds, List<CheckBox> checkBoxes, List<dynamic> items)
        {
            for (int i = 0; i < checkBoxes.Count; i++)
            {
                if (i < items.Count)
                {
                    checkBoxes[i].Tag = items[i].ID; // Сохраняем ID в Tag чекбокса
                    checkBoxes[i].IsChecked = selectedIds.Contains(items[i].ID);
                }
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Обновляем данные пользователя
                if (currentUser != null)
                {
                    var nameParts = FullnameTb.Text.Split(new[] { ' ' }, 3);
                    currentUser.Surname = nameParts.Length > 0 ? nameParts[0] : "";
                    currentUser.Name = nameParts.Length > 1 ? nameParts[1] : "";
                    currentUser.Patronymic = nameParts.Length > 2 ? nameParts[2] : null;
                    currentUser.Phone = PhoneTb.Text;
                }

                // Обновляем данные заявки
                currentApplication.ApartmentArea = AreaTb.Text;
                currentApplication.Comment = CommentTb.Text;

                // Обновляем выбранные области
                UpdateSelectedItems(
                    App.context.AreaApplication.Where(aa => aa.ApplicationID == applicationId).ToList(),
                    new List<CheckBox> { KitchenCb, HallwayCb, LivingRoomCb, AtelierCb, BathroomCb,
                                       ToiletCb, BedroomCb, BasementCb, BalconyCb, DressingRoomCb,
                                       YardCb, RoofCb },
                    (areaId) => new AreaApplication { ApplicationID = applicationId, AreaID = areaId });

                // Обновляем выбранное оборудование
                UpdateSelectedItems(
                    App.context.EquipmentApplication.Where(ea => ea.ApplicationID == applicationId).ToList(),
                    new List<CheckBox> { CameraCb, SecurityCb, WaterCb, CurtainCb, LightCb, HeatingCb },
                    (equipmentId) => new EquipmentApplication { ApplicationID = applicationId, EquipmentID = equipmentId });

                // Сохраняем изменения
                App.context.SaveChanges();
                MessageBox.Show("Изменения сохранены успешно!");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }

        // Метод для обновления выбранных элементов(областей или оборудования)
        private void UpdateSelectedItems<T>(List<T> existingItems, List<CheckBox> checkBoxes, Func<int, T> createNewItem) where T : class
        {
            // Удаляем невыбранные элементы
            for (int i = existingItems.Count - 1; i >= 0; i--)
            {
                dynamic item = existingItems[i];
                int id = item.AreaID ?? item.EquipmentID; // Теперь это будет работать

                var correspondingCheckBox = checkBoxes.FirstOrDefault(cb => (int)cb.Tag == id);
                if (correspondingCheckBox == null || !correspondingCheckBox.IsChecked.Value)
                {
                    App.context.Set<T>().Remove(existingItems[i]);
                    existingItems.RemoveAt(i);
                }
            }

            // Добавляем новые выбранные элементы
            foreach (var checkBox in checkBoxes.Where(cb => cb.IsChecked.Value))
            {
                int id = (int)checkBox.Tag;
                if (!existingItems.Any(item =>
                {
                    dynamic dynItem = item;
                    return (dynItem.AreaID == id) || (dynItem.EquipmentID == id);
                }))
                {
                    var newItem = createNewItem(id);
                    App.context.Set<T>().Add(newItem);
                }
            }
        }

    }
}
