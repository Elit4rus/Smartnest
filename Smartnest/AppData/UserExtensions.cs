using Smartnest.Model;

namespace Smartnest.AppData
{
    public static class UserExtensions
    {
        public static string GetFullName(this User user)
        {
            return $"{user.Surname} {user.Name} {user.Patronymic}".Trim();
        }
    }
}
