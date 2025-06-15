using Smartnest.AppData;
using Smartnest.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Application = Smartnest.Model.Application;

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
            try
            {
                // Проверка введенных данных
                if (string.IsNullOrWhiteSpace(AreaTb.Text))
                {
                    MessageBox.Show("Укажите площадь помещения!");
                    return;
                }

                // Проверка, что выбрана хотя бы одна область
                if (!IsAnyAreaSelected())
                {
                    MessageBox.Show("Выберите хотя бы одну область!");
                    return;
                }

                // Проверка, что выбрано хотя бы одно оборудование
                if (!IsAnyEquipmentSelected())
                {
                    MessageBox.Show("Выберите хотя бы одно оборудование!");
                    return;
                }

                // Создаем новую заявку
                var newApplication = new Application
                {
                    ApartmentArea = AreaTb.Text,
                    Comment = CommentTb.Text,
                    UserID = AuthorizationHelper.currentUser?.ID // Связываем с текущим пользователем
                };

                // Добавляем заявку в базу данных
                App.context.Application.Add(newApplication);
                App.context.SaveChanges();

                // Получаем ID только что созданной заявки
                int applicationId = newApplication.ID;

                // Добавляем выбранные области
                AddSelectedAreas(applicationId);

                // Добавляем выбранное оборудование
                AddSelectedEquipment(applicationId);

                // Сообщаем пользователю об успехе
                MessageBox.Show("Заявка успешно создана! В ближайшее время с вами свяжется оператор для уточнения деталей");

                // Возвращаемся назад
                this.NavigationService.Content = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании заявки: {ex.Message}");
            }
        }
        // Проверка, выбрана ли хотя бы одна область
        private bool IsAnyAreaSelected()
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

        // Проверка, выбрано ли хотя бы одно оборудование
        private bool IsAnyEquipmentSelected()
        {
            return CameraCb.IsChecked == true ||
                   SecurityCb.IsChecked == true ||
                   WaterCb.IsChecked == true ||
                   CurtainCb.IsChecked == true ||
                   LightCb.IsChecked == true ||
                   HeatingCb.IsChecked == true;
        }

        // Добавление выбранных областей в базу данных
        private void AddSelectedAreas(int applicationId)
        {
            var areaCheckBoxes = new List<CheckBox>
            {
                KitchenCb, HallwayCb, LivingRoomCb, AtelierCb,
                BathroomCb, ToiletCb, BedroomCb, BasementCb,
                BalconyCb, DressingRoomCb, YardCb, RoofCb
            };

            // Сопоставляем чекбоксы с ID областей (по порядку как в базе)
            for (int i = 0; i < areaCheckBoxes.Count; i++)
            {
                if (areaCheckBoxes[i].IsChecked == true)
                {
                    App.context.AreaApplication.Add(new AreaApplication
                    {
                        ApplicationID = applicationId,
                        AreaID = i + 1 // ID областей начинаются с 1
                    });
                }
            }

            App.context.SaveChanges();
        }

        // Добавление выбранного оборудования в базу данных
        private void AddSelectedEquipment(int applicationId)
        {
            var equipmentCheckBoxes = new List<CheckBox>
            {
                CameraCb, SecurityCb, WaterCb, CurtainCb, LightCb, HeatingCb
            };

            // Сопоставляем чекбоксы с ID оборудования (по порядку как в базе)
            for (int i = 0; i < equipmentCheckBoxes.Count; i++)
            {
                if (equipmentCheckBoxes[i].IsChecked == true)
                {
                    App.context.EquipmentApplication.Add(new EquipmentApplication
                    {
                        ApplicationID = applicationId,
                        EquipmentID = i + 1 // ID оборудования начинаются с 1
                    });
                }
            }

            App.context.SaveChanges();
        }
    }
}
