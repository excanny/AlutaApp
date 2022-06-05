using AlutaApp.Data;
using AlutaApp.DTO;
using AlutaApp.Models;
using AlutaApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AlutaApp.Controllers
{
    public class ApplicationRoleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _http;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public ApplicationRoleController(RoleManager<ApplicationRole> roleManager, IHttpContextAccessor http, ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _roleManager = roleManager;
            _http = http;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpGet]

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult AddUser()
        {
            UserRegistrationDto model = new UserRegistrationDto();
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Update()
        {


            string userId = HttpContext.Request.Query["userId"];
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);

            var role = roles[0];

            return View();
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Update(string userId, string Role)
        {

            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            var newRoleAdded = await _userManager.AddToRoleAsync(user, Role);
            return RedirectToAction("AllUsers");
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AllRoles()
        {
            var firstCount = 2;
            ViewBag.Count = _context.Notifications.Where(e => e.Clicked == false && e.Viewed == false).ToList().Count();
            ViewBag.Remaining = ViewBag.Count - firstCount;
            ViewBag.Notifications = _context.Notifications.Select(s => new NotificationViewModel
            {
                Content = s.Content,
                //User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
                NotificationId = s.Id,
                Clicked = s.Clicked,
                View = s.Viewed,
                TimeCreated = s.TimeCreated
            }).ToList().OrderByDescending(s => s.TimeCreated).Take(firstCount);
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (roleName != null)
            {
                await _roleManager.CreateAsync(new ApplicationRole(roleName.Trim()));
            }
            return RedirectToAction("AllRoles");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }

}
