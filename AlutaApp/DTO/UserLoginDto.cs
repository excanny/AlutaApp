using AlutaApp.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Claims;

namespace AlutaApp.DTO
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }

    public class ManageUserRolesViewModel
    {
        public string UserId { get; set; }
        public IList<UserRolesViewModel> UserRoles { get; set; }
    }

    public class UpdateUserRolesViewModel
    {
        public string UserId { get; set; }
        public IList<UserRolesViewModel> UserRoles { get; set; }
    }
    public class UserRolesViewModel
    {
        public string RoleName { get; set; }
        public bool Selected { get; set; }
    }
    public class UserRegistrationDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "The Password and Confirm Password do not match.")]
        public string ConfirmPassword { get; set; }

        public string PhoneNumber { get; set; }
        public string Role { get; set; }
    }








    //public static class DefaultRoles
    //{
    //    public static async Task SeedAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    //    {
    //        await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
    //        await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
    //        await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
    //    }
    //}

    //public enum Roles
    //{
    //    SuperAdmin,
    //    Admin,
    //    Basic
    //}

    public static class Permissions
    {
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
        {
            $"Permissions.{module}.Create",
            $"Permissions.{module}.View",
            $"Permissions.{module}.Edit",
            $"Permissions.{module}.Delete",
        };
        }
        public static class Products
        {
            public const string View = "Permissions.Products.View";
            public const string Create = "Permissions.Products.Create";
            public const string Edit = "Permissions.Products.Edit";
            public const string Delete = "Permissions.Products.Delete";
        }
    }


    public static class ClaimsHelper
    {
        public static void GetPermissions(this List<RoleClaimsViewModel> allPermissions, Type policy, string roleId)
        {
            FieldInfo[] fields = policy.GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (FieldInfo fi in fields)
            {
                allPermissions.Add(new RoleClaimsViewModel { Value = fi.GetValue(null).ToString(), Type = "Permissions" });
            }
        }
        public static async Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string permission)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
            {
                await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
            }
        }
    }

    public class PermissionViewModel
    {
        public string RoleId { get; set; }
        public IList<RoleClaimsViewModel> RoleClaims { get; set; }
    }
    public class RoleClaimsViewModel
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
    }
    //public static class DefaultUsers
    //{
    //    //public static async Task SeedBasicUserAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    //    //{
    //    //    var defaultUser = new IdentityUser
    //    //    {
    //    //        UserName = "basicuser@gmail.com",
    //    //        Email = "basicuser@gmail.com",
    //    //        EmailConfirmed = true
    //    //    };
    //    //    if (userManager.Users.All(u => u.Id != defaultUser.Id))
    //    //    {
    //    //        var user = await userManager.FindByEmailAsync(defaultUser.Email);
    //    //        if (user == null)
    //    //        {
    //    //            await userManager.CreateAsync(defaultUser, "123Pa$$word!");
    //    //            await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
    //    //        }
    //    //    }
    //    //}
    //    //public static async Task SeedSuperAdminAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    //    //{
    //    //    var defaultUser = new IdentityUser
    //    //    {
    //    //        UserName = "superadmin@gmail.com",
    //    //        Email = "superadmin@gmail.com",
    //    //        EmailConfirmed = true
    //    //    };
    //    //    if (userManager.Users.All(u => u.Id != defaultUser.Id))
    //    //    {
    //    //        var user = await userManager.FindByEmailAsync(defaultUser.Email);
    //    //        if (user == null)
    //    //        {
    //    //            await userManager.CreateAsync(defaultUser, "123Pa$$word!");
    //    //            await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
    //    //            await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
    //    //            await userManager.AddToRoleAsync(defaultUser, Roles.SuperAdmin.ToString());
    //    //        }
    //    //        await roleManager.SeedClaimsForSuperAdmin();
    //    //    }
    //    }
    //    //private async static Task SeedClaimsForSuperAdmin(this RoleManager<IdentityRole> roleManager)
    //    //{
    //    //    var adminRole = await roleManager.FindByNameAsync("SuperAdmin");
    //    //    await roleManager.AddPermissionClaim(adminRole, "Products");
    //    //}
    //    //public static async Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
    //    //{
    //    //    var allClaims = await roleManager.GetClaimsAsync(role);
    //    //    var allPermissions = Permissions.GeneratePermissionsForModule(module);
    //    //    foreach (var permission in allPermissions)
    //    //    {
    //    //        if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
    //    //        {
    //    //            await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
    //    //        }
    //    //    }
    //    //}
    }


