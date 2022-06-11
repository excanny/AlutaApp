using Aluta.Constants;
using AlutaApp.Constants;
using AlutaApp.Models;
using AlutaApp.Permissions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AlutaApp
{
    public static class IdentityMigrationManager
    {
        public static async Task SeedDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            try
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                await SeedDefaultUserRolesAsync(userManager, roleManager, PermissionHelper.GetAllPermissions());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ". " + ex.Source);
                throw;
            }
        }

        public static async Task SeedDefaultUserRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, List<Claim> permissions)
        {
            var defaultRoles = DefaultApplicationRoles.GetDefaultRoles();
            if (!await roleManager.Roles.AnyAsync())
            {
                foreach (var defaultRole in defaultRoles)
                {
                    await roleManager.CreateAsync(defaultRole);
                }
            }
            if (!await roleManager.RoleExistsAsync(DefaultApplicationRoles.SuperAdmin))
            {
                await roleManager.CreateAsync(new ApplicationRole(DefaultApplicationRoles.SuperAdmin));
            }
            var defaultUser = DefaultApplicationUsers.GetSuperUser();
            var userByName = await userManager.FindByNameAsync(defaultUser.UserName);
            var userByEmail = await userManager.FindByEmailAsync(defaultUser.Email);
            if (userByName == null && userByEmail == null)
            {
                await userManager.CreateAsync(defaultUser, "p,W:xcCjF^cR8JRK");
                foreach (var defaultRole in defaultRoles)
                {
                    await userManager.AddToRoleAsync(defaultUser, defaultRole.Name);
                }
            }

            var role = await roleManager.FindByNameAsync(DefaultApplicationRoles.SuperAdmin);
            var rolePermissions = await roleManager.GetClaimsAsync(role);
            var allPermissions = permissions;
            foreach (var permission in allPermissions)
            {
                if (rolePermissions.Any(x => x.Value == permission.Value && x.Type == permission.Type) == false)
                {
                    await roleManager.AddClaimAsync(role, permission);
                }
            }
        }
    }
}
