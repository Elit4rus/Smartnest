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

            // Получаем выбранное оборудование для заявки
            var equipmentIds = App.context.EquipmentApplication
                .Where(ea => ea.ApplicationID == applicationId)
                .Select(ea => ea.EquipmentID)
                .ToList();

            // Устанавливаем чекбоксы областей
            SetCheckBoxes(
                areaIds,
                new List<CheckBox> {
            KitchenCb, HallwayCb, LivingRoomCb, AtelierCb,
            BathroomCb, ToiletCb, BedroomCb, BasementCb,
            BalconyCb, DressingRoomCb, YardCb, RoofCb
                },
                App.context.Area.ToList()
            );

            // Устанавливаем чекбоксы оборудования
            SetCheckBoxes(
                equipmentIds,
                new List<CheckBox> {
            CameraCb, SecurityCb, WaterCb, CurtainCb, LightCb, HeatingCb
                },
                App.context.Equipment.ToList()
            );
        }

        private void SetCheckBoxes(List<int> selectedIds, List<CheckBox> checkBoxes, List<Equipment> equipments)
        {
            for (int i = 0; i < checkBoxes.Count; i++)
            {
                if (i < equipments.Count)
                {
                    checkBoxes[i].Tag = equipments[i].ID; // Сохраняем ID в Tag
                    checkBoxes[i].IsChecked = selectedIds.Contains(equipments[i].ID);
                }
            }
        }

        private void SetCheckBoxes(List<int> selectedIds, List<CheckBox> checkBoxes, List<Area> areas)
        {
            for (int i = 0; i < checkBoxes.Count; i++)
            {
                if (i < areas.Count)
                {
                    checkBoxes[i].Tag = areas[i].ID; // Сохраняем ID в Tag
                    checkBoxes[i].IsChecked = selectedIds.Contains(areas[i].ID);
                }
            }
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
                    var nameParts = FullnameTb.Text.Split(new[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
                    currentUser.Surname = nameParts.Length > 0 ? nameParts[0] : "";
                    currentUser.Name = nameParts.Length > 1 ? nameParts[1] : "";
                    currentUser.Patronymic = nameParts.Length > 2 ? nameParts[2] : null;
                    currentUser.Phone = PhoneTb.Text;
                }

                // Обновляем данные заявки
                currentApplication.ApartmentArea = AreaTb.Text;
                currentApplication.Comment = CommentTb.Text;

                // Обновляем связи
                UpdateAreaApplications();
                UpdateEquipmentApplications();

                // Сохраняем изменения
                App.context.SaveChanges();
                MessageBox.Show("Изменения сохранены успешно!");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}\n\n{ex.InnerException?.Message}");
            }
        }

        private void UpdateAreaApplications()
        {
            // Получаем все существующие связи
            var existingAreas = App.context.AreaApplication
                .Where(aa => aa.ApplicationID == applicationId)
                .ToList();

            // Удаляем каждую связь по отдельности
            foreach (var area in existingAreas)
            {
                App.context.AreaApplication.Remove(area);
            }

            // Список всех чекбоксов областей
            var areaCheckBoxes = new List<CheckBox>
    {
        KitchenCb, HallwayCb, LivingRoomCb, AtelierCb,
        BathroomCb, ToiletCb, BedroomCb, BasementCb,
        BalconyCb, DressingRoomCb, YardCb, RoofCb
    };

            // Добавляем новые выбранные связи
            foreach (var cb in areaCheckBoxes)
            {
                if (cb.IsChecked == true && cb.Tag != null)
                {
                    int areaId = (int)cb.Tag;
                    App.context.AreaApplication.Add(new AreaApplication
                    {
                        ApplicationID = applicationId,
                        AreaID = areaId
                    });
                }
            }
        }

        private void UpdateEquipmentApplications()
        {
            // Получаем все существующие связи
            var existingEquipment = App.context.EquipmentApplication
                .Where(ea => ea.ApplicationID == applicationId)
                .ToList();

            // Удаляем каждую связь по отдельности
            foreach (var equipment in existingEquipment)
            {
                App.context.EquipmentApplication.Remove(equipment);
            }

            // Список всех чекбоксов оборудования
            var equipmentCheckBoxes = new List<CheckBox>
    {
        CameraCb, SecurityCb, WaterCb, CurtainCb, LightCb, HeatingCb
    };

            // Добавляем новые выбранные связи
            foreach (var cb in equipmentCheckBoxes)
            {
                if (cb.IsChecked == true && cb.Tag != null)
                {
                    int equipmentId = (int)cb.Tag;
                    App.context.EquipmentApplication.Add(new EquipmentApplication
                    {
                        ApplicationID = applicationId,
                        EquipmentID = equipmentId
                    });
                }
            }
        }
    }
}
