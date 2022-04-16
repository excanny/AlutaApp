using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlutaApp.Models;
using AlutaApp.Data;
using X.PagedList;
using AlutaApp.Models.AlutaApp.Models;
using AlutaApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using AlutaApp.DTO;
using Newtonsoft.Json;

namespace AlutaApp.Controllers
{
    [Authorize]
    public class AdministratorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public AdministratorsController(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public async Task<IActionResult> DeleteUser(int? id)
        {
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

        // POST: Administrators/Delete/5
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedUser(int id)
        {
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
            var administrator = await _context.Users.FindAsync(id);
            _context.Users.Remove(administrator);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(UsersAccount));
        }
       [HttpGet]
       public async Task<IActionResult> SearchInstitution(string searchText){
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
            var allInstitutions = await _context.Institutions.Where(e => e.Name.Contains(searchText) || e.Abbreviation.Contains(searchText)).ToListAsync();
            var institutions = await allInstitutions.OrderBy(s=>s.Name).ToPagedListAsync(pageIndex, pageSize);
           return PartialView("_InstitutionListPartial",institutions);
       }


       [HttpGet]
       public async Task<IActionResult> SearchUser(string searchText, string status){
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
            var allUsers = await _context.Users.Include(s=>s.Department).Include(s=>s.Institution).Where(e => e.FullName.Contains(searchText) 
            || e.Institution.Name.Contains(searchText)
            || e.Department.Name.Contains(searchText)
            || e.YearOfAdmission.ToString().Contains(searchText)
            || e.Gender.Contains(searchText)).ToListAsync();
            var users = await allUsers.OrderBy(s=>s.FullName).ToPagedListAsync(pageIndex, pageSize);
            return PartialView("_UserListPartial",users);
       }

       [HttpGet]
       public async Task<IActionResult> SearchPost(string searchText){
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
            var allUsers = await _context.Posts
            .Include(s=>s.User)
            .Include(s=>s.Comments)
            .Include(s=>s.Likes).Where(e => e.Title.Contains(searchText) 
            || e.Content.Contains(searchText)).ToListAsync();
            var users = await allUsers.OrderBy(s=>s.Title).ToPagedListAsync(pageIndex, pageSize);
            return PartialView("_PostListPartial",users);
       }


        [HttpGet]
       public async Task<IActionResult> SearchDepartment(string searchText){
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
            var allInstitutions = await _context.Departments.Where(e => e.Name.Contains(searchText)).ToListAsync();
            var institutions = await allInstitutions.OrderBy(s=>s.Name).ToPagedListAsync(pageIndex, pageSize);
           return PartialView("_DepartmentListPartial",institutions);
       }

        public async Task<IActionResult> EditUser(int? id)
        {
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

        // POST: Administrators/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(int id, [Bind("FullName,Gender,DateOfBirth,YearOfAdmission,Biography,InstitutionId,DepartmentId,GradePoint,Id,UserName,Email,PhoneNumber")] User user)
        {
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
            user.InstitutionId = user.InstitutionId;
            user.Institution = await _context.Institutions.Where(w=>w.Id == user.InstitutionId).FirstOrDefaultAsync();

            user.DepartmentId = user.DepartmentId;
            user.Department = await _context.Departments.Where(w => w.Id == user.DepartmentId).FirstOrDefaultAsync();

            if (id != user.Id)
            {
                return NotFound();
            }
            
            if (!ModelState.IsValid)
            {
                return View(ModelState);
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
                return RedirectToAction(nameof(UsersAccount));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", user.DepartmentId);
            ViewData["InstitutionId"] = new SelectList(_context.Institutions, "Id", "Abbreviation", user.InstitutionId);
            return View(user);
        }
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        public async Task<IActionResult> UsersAccount(int? page)
        {
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
            var allUsers = await _context.Users.Include(s=>s.Institution).Include(a=>a.Department).ToListAsync();
            var departments = await allUsers.OrderBy(s => s.DateOfBirth).ToPagedListAsync(pageIndex, pageSize);
            if (departments.Count == 0)
            {
                ViewBag.NoInstitition = "No User";
                return View(null);
            }

            return View(departments);
        }

        public async Task<IActionResult> Ban(int? id)
        {
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
            var getUser = _context.Users.Find(id);
            if (getUser == null)
            {
                return View(null);
            }
            if (getUser.IsBanned == false)
            {
                getUser.IsBanned = true;
                _context.SaveChanges();
                return RedirectToAction("ViewAccount", new { id = id });
            }
            getUser.IsBanned = false;
            _context.SaveChanges();
            return RedirectToAction("ViewAccount", new { id = id });
        }

        public async Task<IActionResult> ViewAccount(int? id)
        {
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
            var result = await _context.Users.Where(e => e.Id == id).Include(e => e.Department).Include(x=>x.Posts).Include(s => s.Institution).FirstOrDefaultAsync();
            return View(result);

        }

        public async Task<IActionResult> Points(int? page)
        {
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
            var allDepartments = await _context.Points2Earns.ToListAsync();
            var departments = await allDepartments.OrderBy(s => s.Points).ToPagedListAsync(pageIndex, pageSize);
            if (departments.Count == 0)
            {
                ViewBag.NoInstitition = "No Department";
                return View(null);
            }

            return View(departments);
        }

       [HttpGet]
       public async Task<IActionResult> SearchPoint(string searchText){
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
            var allPoints = await _context.Points2Earns.Where(e => e.Category.Contains(searchText)).ToListAsync();
            var points = await allPoints.OrderBy(s=>s.Category).ToPagedListAsync(pageIndex, pageSize);
           return PartialView("_PointListPartial",points);
       }

       
//
        public async Task<IActionResult> Posts(int? page)
        {
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
            var allDepartments = await _context.Posts.Include(r=>r.User).ToListAsync();
            var departments = await allDepartments.OrderByDescending(s => s.TimeCreated).ToPagedListAsync(pageIndex, pageSize);
            if (departments.Count == 0)
            {
                ViewBag.NoInstitition = "No Department";
                return View(null);
            }

            return View(departments);
        }

        public ActionResult PostChart()
        {
            var users = _context.Posts.Where(s => s.TimeCreated.Year == DateTime.Now.Year).ToList();
            var months = new Month
            {
                January = users.Where(s => s.TimeCreated.Month == 1).Count(),
                February = users.Where(s => s.TimeCreated.Month == 2).Count(),
                March = users.Where(s => s.TimeCreated.Month == 3).Count(),
                April = users.Where(s => s.TimeCreated.Month == 4).Count(),
                May = users.Where(s => s.TimeCreated.Month == 5).Count(),
                June = users.Where(s => s.TimeCreated.Month == 6).Count(),
                July = users.Where(s => s.TimeCreated.Month == 7).Count(),
                August = users.Where(s => s.TimeCreated.Month == 8).Count(),
                September = users.Where(s => s.TimeCreated.Month == 9).Count(),
                October = users.Where(s => s.TimeCreated.Month == 10).Count(),
                November = users.Where(s => s.TimeCreated.Month == 11).Count(),
                December = users.Where(s => s.TimeCreated.Month == 12).Count(),
            };
            return new JsonResult(months);
        }

        public async Task<IActionResult> ViewPost(int? id)
        {
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
            var result = await _context.Posts.Where(e => e.Id == id).Include(e=>e.User).Include(s => s.Comments).Include(w=>w.Likes).FirstOrDefaultAsync();
            ViewBag.CommentLikes =  _context.Comments.Where(r=>r.PostId == id).Include(w=>w.CommentLikes).ToList();
            int? page = 1;

             int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.Id =  id;
            var getLikes  = _context.PostLikes.Where(p=>p.PostId  == id).ToList();
           //var allDepartments = await _context.Points2Earns.ToListAsync();
            var likes = await getLikes.OrderByDescending(s => s.TimeCreated).ToPagedListAsync(pageIndex, pageSize);
            
            var getComments  = _context.Comments.Where(p=>p.PostId  == id).ToList();
           //var allDepartments = await _context.Points2Earns.ToListAsync();
            var comments = await getComments.OrderByDescending(s => s.TimeCreated).ToPagedListAsync(pageIndex, pageSize);
            
            ViewBag.PageList = likes;
            ViewBag.CommentList = comments;


            return View(result);

        }

        
        public async Task<IActionResult> GetPostLikes(int? id){
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
            int? page = 1;

             int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var getLikes  = _context.PostLikes.Where(p=>p.PostId  == id).ToList();
           //var allDepartments = await _context.Points2Earns.ToListAsync();
            var likes = await getLikes.OrderByDescending(s => s.TimeCreated).ToPagedListAsync(pageIndex, pageSize);
            return PartialView("_GetPostLikes",likes);
        }
        public ActionResult CommentChart()
        {
            var users = _context.Comments.Where(s => s.TimeCreated.Year == DateTime.Now.Year).ToList();
            var months = new Month
            {
                January = users.Where(s => s.TimeCreated.Month == 1).Count(),
                February = users.Where(s => s.TimeCreated.Month == 2).Count(),
                March = users.Where(s => s.TimeCreated.Month == 3).Count(),
                April = users.Where(s => s.TimeCreated.Month == 4).Count(),
                May = users.Where(s => s.TimeCreated.Month == 5).Count(),
                June = users.Where(s => s.TimeCreated.Month == 6).Count(),
                July = users.Where(s => s.TimeCreated.Month == 7).Count(),
                August = users.Where(s => s.TimeCreated.Month == 8).Count(),
                September = users.Where(s => s.TimeCreated.Month == 9).Count(),
                October = users.Where(s => s.TimeCreated.Month == 10).Count(),
                November = users.Where(s => s.TimeCreated.Month == 11).Count(),
                December = users.Where(s => s.TimeCreated.Month == 12).Count(),
            };
            return new JsonResult(months);
        }


        public ActionResult MessageChart()
        {
            var users = _context.Messages.Where(s => s.TimeCreated.Year == DateTime.Now.Year).ToList();
            var months = new Month
            {
                January = users.Where(s => s.TimeCreated.Month == 1).Count(),
                February = users.Where(s => s.TimeCreated.Month == 2).Count(),
                March = users.Where(s => s.TimeCreated.Month == 3).Count(),
                April = users.Where(s => s.TimeCreated.Month == 4).Count(),
                May = users.Where(s => s.TimeCreated.Month == 5).Count(),
                June = users.Where(s => s.TimeCreated.Month == 6).Count(),
                July = users.Where(s => s.TimeCreated.Month == 7).Count(),
                August = users.Where(s => s.TimeCreated.Month == 8).Count(),
                September = users.Where(s => s.TimeCreated.Month == 9).Count(),
                October = users.Where(s => s.TimeCreated.Month == 10).Count(),
                November = users.Where(s => s.TimeCreated.Month == 11).Count(),
                December = users.Where(s => s.TimeCreated.Month == 12).Count(),
            };
            return new JsonResult(months);
        }

        public ActionResult PromoChart()
        {
            var users = _context.Promotions.Where(s => s.DateCreated.Year == DateTime.Now.Year).ToList();
            var months = new Month
            {
                January = users.Where(s => s.DateCreated.Month == 1).Count(),
                February = users.Where(s => s.DateCreated.Month == 2).Count(),
                March = users.Where(s => s.DateCreated.Month == 3).Count(),
                April = users.Where(s => s.DateCreated.Month == 4).Count(),
                May = users.Where(s => s.DateCreated.Month == 5).Count(),
                June = users.Where(s => s.DateCreated.Month == 6).Count(),
                July = users.Where(s => s.DateCreated.Month == 7).Count(),
                August = users.Where(s => s.DateCreated.Month == 8).Count(),
                September = users.Where(s => s.DateCreated.Month == 9).Count(),
                October = users.Where(s => s.DateCreated.Month == 10).Count(),
                November = users.Where(s => s.DateCreated.Month == 11).Count(),
                December = users.Where(s => s.DateCreated.Month == 12).Count(),
            };
            return new JsonResult(months);
        }

        public ActionResult RevenueChart()
        {
            var users = _context.PropmotionPayments.Where(s => s.DateInitiated.Year == DateTime.Now.Year).ToList();
            var months = new Month
            {
                January = Convert.ToInt32(users.Where(s => s.DateInitiated.Month == 1).Select(e=>e.Amount).Sum()),
                February = Convert.ToInt32(users.Where(s => s.DateInitiated.Month == 2).Select(e => e.Amount).Sum()),
                March = Convert.ToInt32(users.Where(s => s.DateInitiated.Month == 3).Select(e => e.Amount).Sum()),
                April = Convert.ToInt32(users.Where(s => s.DateInitiated.Month == 4).Select(e => e.Amount).Sum()),
                May = Convert.ToInt32(users.Where(s => s.DateInitiated.Month == 5).Select(e => e.Amount).Sum()),
                June = Convert.ToInt32(users.Where(s => s.DateInitiated.Month == 6).Select(e => e.Amount).Sum()),
                July = Convert.ToInt32(users.Where(s => s.DateInitiated.Month == 7).Select(e => e.Amount).Sum()),
                August = Convert.ToInt32(users.Where(s => s.DateInitiated.Month == 8).Select(e => e.Amount).Sum()),
                September = Convert.ToInt32(users.Where(s => s.DateInitiated.Month == 9).Select(e => e.Amount).Sum()),
                October = Convert.ToInt32(users.Where(s => s.DateInitiated.Month == 10).Select(e => e.Amount).Sum()),
                November = Convert.ToInt32(users.Where(s => s.DateInitiated.Month == 11).Select(e => e.Amount).Sum()),
                December = Convert.ToInt32(users.Where(s => s.DateInitiated.Month == 12).Select(e => e.Amount).Sum()),
            };
            return new JsonResult(months);
        }


        public async Task<IActionResult> ViewPostComment(int? id)
        {
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
            var result = await _context.Comments.Where(e => e.PostId == id).Include(e => e.User).Include(w => w.CommentLikes).FirstOrDefaultAsync();
            return PartialView("_CommentListPartial", result);

        }


        public async Task<IActionResult> ViewPoint(int? id)
        {
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
            var result = await _context.Points2Earns.Where(e => e.Id == id).FirstOrDefaultAsync();
            return View(result);

        }

        public async Task<IActionResult> DeletePost(int? id)
        {
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
            if (id == null)
            {
                return NotFound();
            }

            var administrator = await _context.Posts.Include(s=>s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (administrator == null)
            {
                return NotFound();
            }

            return View(administrator);
        }

        // POST: Administrators/Delete/5
        [HttpPost, ActionName("DeletePost")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedPost(int id)
        {
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
            var administrator = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(administrator);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Posts));
        }

        private bool PostExists(long id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Documents(int? page)
        {
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
            var allDepartments = await _context.Documents.Include(s=>s.Category).Include(e=>e.Department).Include(a=>a.User).ToListAsync();
            var departments = await allDepartments.OrderByDescending(s => s.TimeCreated).ToPagedListAsync(pageIndex, pageSize);
            if (departments.Count == 0)
            {
                ViewBag.NoInstitition = "No Department";
                return View(null);
            }

            return View(departments);
        }




        public async Task<IActionResult> ViewDocument(int? id)
        {
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
            var document = await _context.Documents
                .Include(d => d.Category)
                .Include(d => d.Department)
                .Include(d => d.User)
                .Where(y => y.Id == id).FirstOrDefaultAsync();


            return View(document);
        }
        

        public async Task<IActionResult> DeleteDocument(int? id)
        {
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
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .Include(d => d.Category)
                .Include(d => d.Department)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }



        [HttpPost, ActionName("DeleteDocument")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmDocument(int? id)
        {
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
            var comment = await _context.Documents.FindAsync(id);
            _context.Documents.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Documents");
        }
        // POST: Documents/Delete/5
        public async Task<IActionResult> DeleteComment(int? id)
        {
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
            var comment = await _context.Comments.FindAsync(id);
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewPost", new { id = id});
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }


        public async Task<IActionResult> UpdatePoint(int? id)
        {
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
            if (id == null)
            {
                return NotFound();
            }

            var administrator = await _context.Points2Earns.FindAsync(id);
            if (administrator == null)
            {
                return NotFound();
            }
            return View(administrator);
        }

        // POST: Administrators/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePoint(int id, [Bind("Id,Points,Category")] Points2Earn administrator)
        {
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
            if (id != administrator.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(administrator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(administrator.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Points));
            }
            return View(administrator);
        }

        // GET: Administrators/Delete/5
        

        
        













        // GET: Administrators
        //public async Task<IActionResult> Index()
        //{
        //    var administrators = await _context.Administrator.Select(e=> new AdminstratorModel
        //    { 
        //    DateCreated = e.CreatedDate.ToLongDateString(),
        //    FirstName = e.FullName,
        //    RoleId = e.RoleId,
        //    RoleName = _context.Role.Where(d => d.Id == e.RoleId).Select(e => e.RoleName).FirstOrDefault(),
        //    UserName = e.UserName
        //    }).ToListAsync();
        //    List<PageModel> models = new List<PageModel>();
        //    var apps = await _context.Page.ToListAsync();
        //    var roleid = HttpContext.Session.GetInt32("RoleId");
        //    ViewBag.FullName = HttpContext.Session.GetString("FullName");

        //    foreach (var item in apps)
        //    {
        //        var userAccess = await _context.Permission.Where(e => e.RoleId == roleid && e.PageId == item.Id).FirstOrDefaultAsync();
        //        models.Add(new PageModel { PageName = item.PageName, PageUrl = item.PageUrl });
        //    }


        //    ViewBag.AccessPage = models;
        //    return View(administrators);
        //}

        //// GET: Administrators/Details/5
        //public async Task<IActionResult> Details(long? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var administrator = await _context.Administrator
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (administrator == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(administrator);
        //}

        //// GET: Administrators/Create
        //public IActionResult Create()
        //{
        //    ViewData["RoleId"] = new SelectList(_context.Role, "Id", "RoleName");
        //    return View();
        //}

        //// POST: Administrators/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("UserName,Password,RoleId,Id,CreatedDate,ModifiedDate,CreatedBy,ModifiedBy")] Administrator administrator)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        administrator.CreatedDate = DateTime.Now;
        //        _context.Add(administrator);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["RoleId"] = new SelectList(_context.Role, "Id", "RoleName");
        //    return View(administrator);
        //}

        //[HttpGet]
        //public async Task<IActionResult> Login()
        //{
        //    //if (HttpContext.Session.GetString("userName") == null)
        //    //{
        //    //    return RedirectToAction("Login");
        //    //}
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Login(LoginModel login)
        //{
        //    //if (HttpContext.Session.GetString("userName") == null)
        //    //{
        //    //    return RedirectToAction("Login");
        //    //}
        //    var user = await _context.Administrator.Where(d=>d.UserName == login.UserName).FirstOrDefaultAsync();
        //    if (user == null)
        //    {
        //        ViewBag.Message = "User Does not Exist";
        //        return View();
        //    }
        //    if (user.Password != login.Password)
        //    {
        //        ViewBag.Message = "Invalid username or password";
        //        return View();
        //    }
        //    //List<PageModel> models = new List<PageModel>();
        //    //var apps = await _context.Page.ToListAsync();
        //    HttpContext.Session.SetInt32("RoleId", user.RoleId);
        //    HttpContext.Session.SetString("FullName", user.FullName);
        //    //foreach (var item in apps)
        //    //{
        //    //    var userAccess = await _context.Permission.Where(e => e.RoleId == user.RoleId && e.PageId == item.Id).FirstOrDefaultAsync();
        //    //    models.Add(new PageModel { PageName = item.PageName, PageUrl = item.PageUrl});
        //    //}


        //    //ViewBag.AccessPage = models;
        //    return RedirectToAction("Dashboard");
        //}

        [Authorize]
        public async Task<ActionResult> Dashboard()
        {
            //var secondPartyId = _contextAccessor.HttpContext.Session.GetInt32("secondPartyId");
            var firstCount = 2;
            ViewBag.Count = _context.Notifications.Where(e => e.Clicked == false && e.Viewed == false).ToList().Count();
            ViewBag.Remaining = ViewBag.Count - firstCount;
            ViewBag.Notifications = _context.Notifications.Select(s=> new NotificationViewModel
            {
                Content = s.Content,
                User = _context.Users.Where(e=>e.Id == s.UserId).FirstOrDefault().FullName,
                NotificationId = s.Id,
                Clicked = s.Clicked,
                View = s.Viewed,
                TimeCreated = s.TimeCreated
            }).ToList().OrderByDescending(s => s.TimeCreated).Take(firstCount);
            ViewBag.ActiveUsers = _context.Users.Where(s=>s.Online  == true).ToList().Count();
            ViewBag.InActiveUsers= _context.Users.Where(s=>s.Online  == false).ToList().Count();
            ViewBag.DailyRevenue =  _context.PropmotionPayments.Where(s=>s.DateInitiated.Day == DateTime.Now.Day).ToList().Sum(e => e.Amount);
            ViewBag.TotalUsers = _context.Users.ToList().Count();
            var res = _contextAccessor.HttpContext.Session.GetString("userName");
            var getSubstring = res.IndexOf("@");
            var getLength = res.Length;
            var replaced = res.Replace(res.Substring(getSubstring, getLength-getSubstring),"");
            var getRevenue = _context.PropmotionPayments.Select(s => s.Amount).ToList().Sum();
            ViewBag.Revenue = getRevenue;
            ViewBag.UserName = replaced;
            //List<string> months = new List<string>();

            //months.Add("January");
            //months.Add("February");
            //months.Add("March");
            //months.Add("April");
            //months.Add("May");
            //months.Add("June" );
            //months.Add("July");
            //months.Add("August");
            //months.Add("September");
            //months.Add("October" );
            //months.Add("November" );
            //months.Add("December");
            //ViewBag.Months = JsonConvert.SerializeObject(months);
            //List<string> userCounts = new List<string>();

            var users = _context.Users.Where(s => s.TimeRegistered.Year == DateTime.Now.Year).ToList();
            //for (int i = 1; i <= 12; i++)
            //{
            //    var userCounters = users.Where(s => s.TimeRegistered.Month == i && s.TimeRegistered.Year == DateTime.Now.Year).ToList().Count();
            //    userCounts.Add(userCounters.ToString());
            //}

            //ViewBag.Counter = JsonConvert.SerializeObject(userCounts);

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

            return View();
        }

        

        public async Task<IActionResult> Departments(int? page)
        {
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
            var allDepartments = await _context.Departments.ToListAsync();
            var departments = await allDepartments.OrderBy(s => s.Name).ToPagedListAsync(pageIndex, pageSize);
            if (departments.Count == 0)
            {
                ViewBag.NoInstitition = "No Department";
                return View(null);
            }

            return View(departments);
        }


        public async Task<IActionResult> AddDepartment(Department department)
        {
            var secondPartyId = _contextAccessor.HttpContext.Session.GetInt32("secondPartyId");
            ViewBag.Count = _context.Notifications.Where(e => e.SecondPartyId == secondPartyId && e.Clicked == false && e.Viewed == false).ToList().Count();
            ViewBag.Notifications = _context.Notifications.Where(e => e.SecondPartyId == secondPartyId && e.Clicked == false && e.Viewed == false).ToList();
            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();
            return RedirectToAction("Departments");
        }

        public async Task<IActionResult> ViewDepartment(int? id)
        {
            var result = await _context.Departments.Where(e => e.Id == id).FirstOrDefaultAsync();
            return View(result);
        
        }


        public async Task<IActionResult> EditDepartment(int? id)
        {
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
            if (id == null)
            {
                return NotFound();
            }

            var administrator = await _context.Departments.FindAsync(id);
            if (administrator == null)
            {
                return NotFound();
            }
            return View(administrator);
        }

        // POST: Administrators/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDepartment(int id, [Bind("Id,Name")] Department administrator)
        {
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
            if (id != administrator.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(administrator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(administrator.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Department));
            }
            return View(administrator);
        }

        // GET: Administrators/Delete/5
        public async Task<IActionResult> DeleteDepartment(int? id)
        {
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
            if (id == null)
            {
                return NotFound();
            }

            var administrator = await _context.Departments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (administrator == null)
            {
                return NotFound();
            }

            return View(administrator);
        }

        // POST: Administrators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedDepartment(int id)
        {
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
            var administrator = await _context.Departments.FindAsync(id);
            _context.Departments.Remove(administrator);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Departments));
        }

        private bool DepartmentExists(long id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
        public async Task<IActionResult> Institutions(int? page)
        {
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
            var allInstitutions = await _context.Institutions.ToListAsync();
            var institutions = await allInstitutions.OrderBy(s=>s.Abbreviation).ToPagedListAsync(pageIndex, pageSize);
            if (institutions.Count == 0)
            {
                ViewBag.NoInstitition = "No Institutions";
                return View();
            }
            ViewBag.Institutions=  institutions;
            return View(institutions);
        }

        public async Task<IActionResult> Create(Institution institution)
        {
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
            await _context.Institutions.AddAsync(institution);
            await _context.SaveChangesAsync();
            return RedirectToAction("Institutions");
        }

        public async Task<IActionResult> ViewDetails(int? id)
        {
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
            var result = await _context.Institutions.Where(e => e.Id == id).FirstOrDefaultAsync();
            return View(result);
        }

        // GET: Administrators/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
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
            if (id == null)
            {
                return NotFound();
            }

            var administrator = await _context.Institutions.FindAsync(id);
            if (administrator == null)
            {
                return NotFound();
            }
            return View(administrator);
        }

        // POST: Administrators/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Abbreviation")] Institution administrator)
        {
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
            if (id != administrator.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                    _context.Update(administrator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstitutionExists(administrator.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Institutions));
            }
            return View(administrator);
        }

        // GET: Administrators/DeleteInstitution/5
        public async Task<IActionResult> DeleteInstitution(int? id)
        {
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
            if (id == null)
            {
                return NotFound();
            }

            var administrator = await _context.Institutions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (administrator == null)
            {
                return NotFound();
            }

            return View(administrator);
        }

        // POST: Administrators/Delete/5
        [HttpPost, ActionName("DeleteInstitution")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedInstitution(int id)
        {
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
            var administrator = await _context.Institutions.FindAsync(id);
            _context.Institutions.Remove(administrator);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Institutions));
        }

        private bool InstitutionExists(long id)
        {
            return _context.Institutions.Any(e => e.Id == id);
        }
    }
}
