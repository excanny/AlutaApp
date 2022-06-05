using Microsoft.AspNetCore.Identity;
using AlutaApp.Constants;
using AlutaApp.Models;

namespace AlutaApp.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<ApplicationRole> roleManager)
        {
            await roleManager.CreateAsync(new ApplicationRole(Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new ApplicationRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new ApplicationRole(Roles.Basic.ToString()));
        }
    }
}
