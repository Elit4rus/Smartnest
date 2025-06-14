using Smartnest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Smartnest.AppData
{
    public class AuthorizationHelper
    {
        public static User currentUser;

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
            currentUser = newUser;
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
            currentUser = user;

            return user;
        }
    }
}
