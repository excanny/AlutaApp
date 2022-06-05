using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlutaApp.Data;
using AlutaApp.Models;
using AlutaApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AlutaApp.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public NotificationsController(ApplicationDbContext context, IHttpContextAccessor contextAccessor, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> ClickNotification(int id)
        {
            ViewBag.Notifications = _context.Notifications.Select(s => new NotificationViewModel
            {
                Content = s.Content,
                //User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
                NotificationId = s.Id,
                Clicked = s.Clicked,
                View = s.Viewed,
                TimeCreated = s.TimeCreated
            }).ToList().Take(10).OrderByDescending(s => s.TimeCreated);
            var notification = _context.Notifications.Where(e => e.Id == id).FirstOrDefault();
            notification.Clicked = true;
            notification.Viewed = true;
            _context.SaveChanges();
            return View();
        }

        public async Task<IActionResult> ViewNotification(int id)
        {
            ViewBag.Notifications = _context.Notifications.Select(s => new NotificationViewModel
            {
                Content = s.Content,
                //User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
                NotificationId = s.Id,
                Clicked = s.Clicked,
                View = s.Viewed,
                TimeCreated = s.TimeCreated
            }).ToList().Take(10).OrderByDescending(s => s.TimeCreated);
            var notification = _context.Notifications.Where(e => e.Id == id).FirstOrDefault();
            notification.Clicked = true;
            notification.Viewed = true;
            _context.SaveChanges();
            return View(notification);
        }

        public async Task<IActionResult> AllNotification()
        {
            var notification = _context.Notifications.ToList();
            return View(notification);
        }
        public async Task<IActionResult> NewNotification()
        {
            var secondPartyId  = _contextAccessor.HttpContext.Session.GetInt32("secondPartyId");
            ViewBag.Count = _context.Notifications.Where(e => e.SecondPartyId == secondPartyId && e.Clicked == false && e.Viewed == false).ToList().Count();
            ViewBag.Notifications = _context.Notifications.Where(e => e.SecondPartyId == secondPartyId && e.Clicked == false && e.Viewed == false).ToList();
            return View();
        }

        public async Task<IActionResult> ReadNotification()
        {
            var secondPartyId = _contextAccessor.HttpContext.Session.GetInt32("secondPartyId");
            ViewBag.ReadCount = _context.Notifications.Where(e => e.SecondPartyId == secondPartyId && e.Clicked == true && e.Viewed == true).ToList().Count();
            ViewBag.ReadNotifications = _context.Notifications.Where(e => e.SecondPartyId == secondPartyId && e.Clicked == true && e.Viewed == true).ToList();
            return View();
        }
        // GET: Notifications
        public async Task<IActionResult> Index()
        {
            var allNotifications = await _context.Notifications.ToListAsync();
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var role = await _userManager.GetRolesAsync(currentUser);

            return View(allNotifications);
           
        }

        // GET: Notifications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.Notifications
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notification == null)
            {
                return NotFound();
            }

            return View(notification);
        }

        // GET: Notifications/Create
        public async Task<IActionResult> Create()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var role = await _userManager.GetRolesAsync(currentUser);

            return View();
        }

        // POST: Notifications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Content,Category,CategoryId,SecondPartyId,ThirdPartyId,Viewed,Clicked,TimeCreated,Active")] Notification notification)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(notification);
        }

        // GET: Notifications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }
            return View(notification);
        }

        // POST: Notifications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Content,Category,CategoryId,SecondPartyId,ThirdPartyId,Viewed,Clicked,TimeCreated,Active")] Notification notification)
        {
            if (id != notification.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notification);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationExists(notification.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(notification);
        }

        // GET: Notifications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.Notifications
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notification == null)
            {
                return NotFound();
            }

            return View(notification);
        }

        // POST: Notifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotificationExists(int id)
        {
            return _context.Notifications.Any(e => e.Id == id);
        }
    }
}
