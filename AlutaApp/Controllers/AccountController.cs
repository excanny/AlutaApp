using Aluta.Constants;
using AlutaApp.Data;
using AlutaApp.DTO;
using AlutaApp.Helpers;
using AlutaApp.Models;
using AlutaApp.Permissions;
using AlutaApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AlutaApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _http;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(RoleManager<ApplicationRole> roleManager, IHttpContextAccessor http, ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _roleManager = roleManager;
            _http = http;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            UserRegistrationDto model = new UserRegistrationDto();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserRegistrationDto request)
        {
            if (ModelState.IsValid)
            {
                var userCheck = await _userManager.FindByEmailAsync(request.Email);
                IdentityResult? res;
                if (userCheck == null)
                {
                    var user = new ApplicationUser
                    {

                        UserName = request.Name,
                        NormalizedUserName = request.Name,
                        Email = request.Email,
                        EmailConfirmed = true,
                        
                    };

                    var result = await _userManager.CreateAsync(user, request.Password);

                    if (result.Succeeded)
                    {
                        var defaultRoles = DefaultApplicationRoles.GetDefaultRoles();
                        
                        res = await _userManager.AddToRoleAsync(user, defaultRoles[3].ToString());

                        if (!res.Succeeded)
                        {
                            return View(request);
                        }

                        return RedirectToAction("AllUsers", "Account");
                    }
                    else
                    {
                        if (result.Errors.Count() > 0)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("message", error.Description);
                            }
                        }
                        return View(request);
                    }
                }
                else
                {
                    ModelState.AddModelError("message", "Email already exists.");
                    return View(request);
                }
            }
            return View(request);

        }

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Users.View)]
        public async Task<IActionResult> AllUsers()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var role = await _userManager.GetRolesAsync(currentUser);
            var allUsersExceptCurrentUser = await _userManager.Users.Where(a => a.Id != currentUser.Id).ToListAsync();
            return View(allUsersExceptCurrentUser);

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            UserLoginDto model = new UserLoginDto();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var role = await _userManager.GetRolesAsync(user);
                    if(!role.Contains("SuperAdmin"))
                    {
                        var claims = await _userManager.GetClaimsAsync(user);

                        if (claims.Count < 1)
                        {
                            ModelState.AddModelError("message", "Sorry, Invalid login");
                            return View(model);
                        }
                    }

             
                    if (!user.EmailConfirmed)
                    {
                        ModelState.AddModelError("message", "Email not confirmed yet");
                        return View(model);

                    }

                    if (await _userManager.CheckPasswordAsync(user, model.Password) == false)
                    {
                        ModelState.AddModelError("message", "Invalid credentials");
                        return View(model);
                    }

                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, true);

                    if (result.Succeeded)
                    {
                        _http.HttpContext.Session.SetString("userName", user.Email);
                        return RedirectToAction("Dashboard", "Administrators");
                    }
                    else if (result.IsLockedOut)
                    {
                        return View("AccountLocked");
                    }
                    else
                    {
                        ModelState.AddModelError("message", "Invalid login attempt");
                        return View();
                    }

                }

            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("/");
        }
        
        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.ApplicationUsers.ManageRoles)]
        public async Task<IActionResult> ManageRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return View();
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();
            var allRolesViewModel = allRoles.Select(role => new ManageRoleViewModel
            { Name = role.Name, Description = role.Description }).ToList();
            foreach (var roleViewModel in allRolesViewModel.Where(roleViewModel =>
                userRoles.Contains(roleViewModel.Name)))
            {
                roleViewModel.Checked = true;
            }
            var manageUserRolesViewModel = new ManageUserRolesViewModel
            {
                UserId = userId,
                UserName = user.UserName,
                ManageRolesViewModel = allRolesViewModel
            };
            return View(manageUserRolesViewModel);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Permissions.ApplicationUsers.ManageRoles)]
        public async Task<IActionResult> ManageRoles(ManageUserRolesViewModel manageUserRolesViewModel)
        {
            if (!ModelState.IsValid) return View(manageUserRolesViewModel);
            var user = await _userManager.FindByIdAsync(manageUserRolesViewModel.UserId);
            if (user == null)
                return View(manageUserRolesViewModel);
            var existingRoles = await _userManager.GetRolesAsync(user);
            foreach (var roleViewModel in manageUserRolesViewModel.ManageRolesViewModel)
            {
                var roleExists = existingRoles.FirstOrDefault(x => x == roleViewModel.Name);
                switch (roleViewModel.Checked)
                {
                    case true when roleExists == null:
                        await _userManager.AddToRoleAsync(user, roleViewModel.Name);
                        break;
                    case false when roleExists != null:
                        await _userManager.RemoveFromRoleAsync(user, roleViewModel.Name);
                        break;
                }
            }
            return RedirectToAction("Index", "User", new { id = manageUserRolesViewModel.UserId, succeeded = true, message = "Succeeded" });

        }

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.ApplicationUsers.ManageClaims)]
        public async Task<IActionResult> ManagePermissions(string userId, string permissionValue, int? pageNumber, int? pageSize)
        {

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return RedirectToAction("Index");
            var userPermissions = await _userManager.GetClaimsAsync(user);
            var allPermissions = PermissionHelper.GetAllPermissions();
            if (!string.IsNullOrWhiteSpace(permissionValue))
            {
                allPermissions = allPermissions.Where(x => x.Value.ToLower().Contains(permissionValue.Trim().ToLower()))
                    .ToList();
            }
            var managePermissionsClaim = new List<ManageUserClaimViewModel>();
            foreach (var permission in allPermissions)
            {
                var managePermissionClaim = new ManageUserClaimViewModel { Type = permission.Type, Value = permission.Value };
                if (userPermissions.Any(x => x.Value == permission.Value))
                {
                    managePermissionClaim.Checked = true;
                }
                managePermissionsClaim.Add(managePermissionClaim);
            }

            var paginatedList = PaginatedList<ManageUserClaimViewModel>.CreateFromLinqQueryable(managePermissionsClaim.AsQueryable(),
                pageNumber ?? 1, pageSize ?? 12);
            var manageUserPermissionsViewModel = new ManageUserPermissionsViewModel
            {
                UserId = userId,
                UserName = user.UserName,
                PermissionValue = permissionValue,
                ManagePermissionsViewModel = paginatedList
            };

            return View(manageUserPermissionsViewModel);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Permissions.ApplicationUsers.ManageClaims)]
        public async Task<IActionResult> ManageClaims(ManageUserClaimViewModel manageUserClaimViewModel)
        {

            var userById = await _userManager.FindByIdAsync(manageUserClaimViewModel.UserId);
            var userByName = await _userManager.FindByNameAsync(manageUserClaimViewModel.UserName);

            if (userById != userByName)
                return Json(new { Succeeded = false, Message = "Fail" });

            var allClaims = await _userManager.GetClaimsAsync(userById);
            var claimExists =
                allClaims.Where(x => x.Type == manageUserClaimViewModel.Type && x.Value == manageUserClaimViewModel.Value).ToList();
            switch (manageUserClaimViewModel.Checked)
            {
                case true when claimExists.Count == 0:
                    {
                        await _userManager.AddClaimAsync(userById,
                            new Claim(manageUserClaimViewModel.Type, manageUserClaimViewModel.Value));
                        break;
                    }
                case false when claimExists.Count > 0:
                    {
                        await _userManager.RemoveClaimsAsync(userById, claimExists);
                        break;
                    }
            }
            return Json(new { Succeeded = true, Message = "Success" });
        }

    }
}
