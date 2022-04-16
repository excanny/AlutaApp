using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlutaApp.Data;
using AlutaApp.Models.AlutaApp.Models;
using X.PagedList;
using AlutaApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using AlutaApp.DTO;
using Newtonsoft.Json;

namespace AlutaApp.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult UserChart()
        {
            var users = _context.Users.Where(s => s.TimeRegistered.Year == DateTime.Now.Year).ToList();
            var months = new Month
            {
                January = users.Where(s => s.TimeRegistered.Month == 1).Count(),
                February = users.Where(s => s.TimeRegistered.Month == 2).Count(),
                March = users.Where(s => s.TimeRegistered.Month == 3).Count(),
                April = users.Where(s => s.TimeRegistered.Month == 4).Count(),
                May = users.Where(s => s.TimeRegistered.Month == 5).Count(),
                June = users.Where(s => s.TimeRegistered.Month == 6).Count(),
                July = users.Where(s => s.TimeRegistered.Month == 7).Count(),
                August = users.Where(s => s.TimeRegistered.Month == 8).Count(),
                September = users.Where(s => s.TimeRegistered.Month == 9).Count(),
                October = users.Where(s => s.TimeRegistered.Month == 10).Count(),
                November = users.Where(s => s.TimeRegistered.Month == 11).Count(),
                December = users.Where(s => s.TimeRegistered.Month == 12).Count(),
            };
            return new JsonResult(months);
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Users.Include(u => u.Department).Include(u => u.Institution);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Department)
                .Include(u => u.Institution)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

    
        public async Task<IActionResult> ActiveUsers(int? page){
           var firstCount = 2;
            ViewBag.Count = _context.Notifications.Where(e => e.Clicked == false && e.Viewed == false).ToList().Count();
            ViewBag.Remaining = ViewBag.Count - firstCount;
            ViewBag.Notifications = _context.Notifications.Select(s => new NotificationViewModel
            {
                Content = s.Content,
                User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
                NotificationId = s.Id,
                Clicked = s.Clicked,
                View = s.Viewed,
                TimeCreated = s.TimeCreated
            }).ToList().OrderByDescending(s => s.TimeCreated).Take(firstCount);
            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var allUsers = await _context.Users.Where(q=>q.Online == true && q.IsBanned == false).Include(s=>s.Institution).Include(a=>a.Department).ToListAsync();
            var departments = await allUsers.OrderBy(s => s.DateOfBirth).ToPagedListAsync(pageIndex, pageSize);
            return View(departments);
        }

        public async Task<IActionResult> InActiveUsers(int? page){
           var firstCount = 2;
            ViewBag.Count = _context.Notifications.Where(e => e.Clicked == false && e.Viewed == false).ToList().Count();
            ViewBag.Remaining = ViewBag.Count - firstCount;
            ViewBag.Notifications = _context.Notifications.Select(s => new NotificationViewModel
            {
                Content = s.Content,
                User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
                NotificationId = s.Id,
                Clicked = s.Clicked,
                View = s.Viewed,
                TimeCreated = s.TimeCreated
            }).ToList().OrderByDescending(s => s.TimeCreated).Take(firstCount);
            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var allUsers = await _context.Users.Where(q=>q.Online == false && q.IsBanned == true).Include(s=>s.Institution).Include(a=>a.Department).ToListAsync();
            var departments = await allUsers.OrderBy(s => s.DateOfBirth).ToPagedListAsync(pageIndex, pageSize);
            return View(departments);
        }


        [HttpGet]
       public async Task<IActionResult> SearchActiveUsers(string searchText){
           int? page = 1;
           var firstCount = 2;
            ViewBag.Count = _context.Notifications.Where(e => e.Clicked == false && e.Viewed == false).ToList().Count();
            ViewBag.Remaining = ViewBag.Count - firstCount;
            ViewBag.Notifications = _context.Notifications.Select(s => new NotificationViewModel
            {
                Content = s.Content,
                User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
                NotificationId = s.Id,
                Clicked = s.Clicked,
                View = s.Viewed,
                TimeCreated = s.TimeCreated
            }).ToList().OrderByDescending(s => s.TimeCreated).Take(firstCount);
             int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var allUsers = await _context.Users.Where(q=>q.Online == true && q.IsBanned == false).Include(s=>s.Institution).Include(a=>a.Department).ToListAsync();
            var departments = await allUsers.OrderBy(s => s.DateOfBirth).ToPagedListAsync(pageIndex, pageSize);
            //return View(departments);
           return PartialView("_UserListPartial",departments);
       }


       [HttpGet]
       public async Task<IActionResult> SearchInActiveUsers(string searchText){
           int? page = 1;
           var firstCount = 2;
            ViewBag.Count = _context.Notifications.Where(e => e.Clicked == false && e.Viewed == false).ToList().Count();
            ViewBag.Remaining = ViewBag.Count - firstCount;
            ViewBag.Notifications = _context.Notifications.Select(s => new NotificationViewModel
            {
                Content = s.Content,
                User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
                NotificationId = s.Id,
                Clicked = s.Clicked,
                View = s.Viewed,
                TimeCreated = s.TimeCreated
            }).ToList().OrderByDescending(s => s.TimeCreated).Take(firstCount);
             int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var allUsers = await _context.Users.Where(q=>q.Online == false && q.IsBanned == true).Include(s=>s.Institution).Include(a=>a.Department).ToListAsync();
            var departments = await allUsers.OrderBy(s => s.DateOfBirth).ToPagedListAsync(pageIndex, pageSize);
            //return View(departments);
           return PartialView("_UserListPartial",departments);
       }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            ViewData["InstitutionId"] = new SelectList(_context.Institutions, "Id", "Abbreviation");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FullName,Gender,DateOfBirth,YearOfAdmission,ProfilePhoto,Biography,IsBanned,IsVerified,Referrer,InstitutionId,DepartmentId,GradePoint,Online,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", user.DepartmentId);
            ViewData["InstitutionId"] = new SelectList(_context.Institutions, "Id", "Abbreviation", user.InstitutionId);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", user.DepartmentId);
            ViewData["InstitutionId"] = new SelectList(_context.Institutions, "Id", "Abbreviation", user.InstitutionId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FullName,Gender,DateOfBirth,YearOfAdmission,ProfilePhoto,Biography,IsBanned,IsVerified,Referrer,InstitutionId,DepartmentId,GradePoint,Online,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", user.DepartmentId);
            ViewData["InstitutionId"] = new SelectList(_context.Institutions, "Id", "Abbreviation", user.InstitutionId);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Department)
                .Include(u => u.Institution)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
