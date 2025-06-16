using Smartnest.Model;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace Smartnest.AppData
{
    public class AuthorizationHelper
    {
        public static User _currentUser;

        public static User currentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                // Обновляем контекст, чтобы избежать проблем с отслеживанием сущностей
                if (value != null)
                {
                    App.context.Entry(value).State = EntityState.Detached;
                    App.context = new SmartnestEntities();
                }
            }
        }

        // Регистрация нового пользователя
        public static void RegisterUser(string surname, string name, string patronymic, string phone, string password)
        {
            // Проверка существования пользователя
            if (App.context.User.Any(u => u.Phone == phone))
            {
                throw new Exception("Пользователь с таким номером телефона уже существует");
            }

            var newUser = new User
            {
                Surname = surname,
                Name = name,
                Patronymic = patronymic,
                Phone = phone,
                RoleID = 1, // Клиент по умолчанию
                Password = password
            };
            _currentUser = newUser;
            App.context.User.Add(newUser);
            App.context.SaveChanges();
        }

        // Авторизация пользователя
        public static User Login(string phone, string password)
        {
            var user = (from u in App.context.User
                        where u.Phone == phone && u.Password == password
                        select u).FirstOrDefault();

            if (user == null)
            {
                throw new Exception("Неверный номер телефона или пароль");
            }
            _currentUser = user;

            return user;
        }

    }
}
