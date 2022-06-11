using AlutaApp.Models;

namespace AlutaApp.Constants
{
    public class DefaultApplicationUsers
    {
        public static ApplicationUser GetSuperUser()
        {
            var defaultUser = new ApplicationUser
            {
                Id = "39c64ad6-39ae-4de0-a0b2-9298a46b4b4c",
                UserName = "SuperAdmin",
                Email = "alutasuperadmin@aluta.com",
                FirstName = "Super",
                LastName = "Admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            return defaultUser;
        }
    }
}
