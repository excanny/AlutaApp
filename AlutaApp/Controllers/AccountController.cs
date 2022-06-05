using AlutaApp.Data;
using AlutaApp.DTO;
using AlutaApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

                        UserName = request.Email,
                        NormalizedUserName = request.Email,
                        Email = request.Email,
                        PhoneNumber = request.PhoneNumber,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                    };

                    var result = await _userManager.CreateAsync(user, request.Password);

                    if (result.Succeeded)
                    {
                        res = await _userManager.AddToRoleAsync(user, request.Role);

                        if (!res.Succeeded)
                        {
                            return View(request);
                        }

                        return RedirectToAction("Dashboard", "Administrators");
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

                try
                {

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                }



            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "ApplicationRole");
        }

    }
}
