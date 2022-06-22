using AlutaApp.Data;
using AlutaApp.DTO;
using AlutaApp.Models;
using AlutaApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace AlutaApp.Controllers
{
    [Authorize]
    public class AdministratorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public AdministratorsController(ApplicationDbContext context, IHttpContextAccessor contextAccessor, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Users.Delete)]
        public async Task<IActionResult> DeleteUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                 .Include(u => u.Department)
                 .Include(u => u.Institution)
                 .FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Administrators/Delete/5
      
        [Authorize(Policy = Permissions.Permissions.Users.Delete)]
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if(user != null)
            {
                user.Deleted = true;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
               
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
                //////User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
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
                //////User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
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
                //////User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
                NotificationId = s.Id,
                Clicked = s.Clicked,
                View = s.Viewed,
                TimeCreated = s.TimeCreated
            }).ToList().OrderByDescending(s => s.TimeCreated).Take(firstCount);
            // int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var allUsers = await _context.Posts
            .Include(s=>s.User)
            .Include(s=>s.Comments)
            .ToListAsync();
            return PartialView("_PostListPartial");
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
                //////User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
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

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Users.Edit)]
        public async Task<IActionResult> EditUser(int? id)
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

        // POST: Administrators/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = Permissions.Permissions.Users.Edit)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(int id, [Bind("FullName,Gender,DateOfBirth,YearOfAdmission, Biography,InstitutionId,DepartmentId,GradePoint")] EditUserDTO user)
        //public async Task<IActionResult> EditUser(int id, User user)
        {

            //var errors = ModelState
            //.Where(x => x.Value.Errors.Count > 0)
            //.Select(x => new { x.Key, x.Value.Errors })
            //.ToArray();

            User? userData = await _context.Users.FindAsync(id);
            if (userData != null)
            {
                userData.FullName = user.FullName;
                userData.Gender = user.Gender;
                userData.DateOfBirth = user.DateOfBirth;
                userData.YearOfAdmission = user.YearOfAdmission;
                userData.InstitutionId = user.InstitutionId;    
                userData.DepartmentId = user.DepartmentId;
                userData.GradePoint = user.GradePoint;  
     
            }


            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userData);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(UsersAccount));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                  
                }
                return RedirectToAction(nameof(UsersAccount));
            }

            //return RedirectToAction("Index");

            return View(user);
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Users.View)]
        public async Task<IActionResult> UsersAccount()
        {
           
            var allUsers = await _context.Users.Where(u => !u.Deleted).Include(s=>s.Institution).Include(a=>a.Department).ToListAsync();
            return View(allUsers);

        }

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Users.Edit)]
        public async Task<IActionResult> Ban(int? id)
        {
            var firstCount = 2;
            ViewBag.Count = _context.Notifications.Where(e => e.Clicked == false && e.Viewed == false).ToList().Count();
            ViewBag.Remaining = ViewBag.Count - firstCount;
            ViewBag.Notifications = _context.Notifications.Select(s => new NotificationViewModel
            {
                Content = s.Content,
                //////User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
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
            
            _context.SaveChanges();
            return RedirectToAction("ViewAccount", new { id = id });
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Users.View)]
        public async Task<IActionResult> ViewAccount(int id)
        {

            var userDetails = await _context.Users.Where(e => e.Id == id).Include(e => e.Department).Include(x=>x.Posts).Include(s => s.Institution).FirstOrDefaultAsync();
            return View(userDetails);

        }

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Points.View)]
        public async Task<IActionResult> Points()
        {
            var firstCount = 2;
            ViewBag.Count = _context.Notifications.Where(e => e.Clicked == false && e.Viewed == false).ToList().Count();
            ViewBag.Remaining = ViewBag.Count - firstCount;
            ViewBag.Notifications = _context.Notifications.Select(s => new NotificationViewModel
            {
                Content = s.Content,
                //////User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
                NotificationId = s.Id,
                Clicked = s.Clicked,
                View = s.Viewed,
                TimeCreated = s.TimeCreated
            }).ToList().OrderByDescending(s => s.TimeCreated).Take(firstCount);
            int pageSize = 10;
            int pageIndex = 1;

            //pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var allPoints = await _context.Points2Earns.ToListAsync();
            //var departments = await allDepartments.OrderBy(s => s.Points).ToPagedListAsync(pageIndex, pageSize);
            //if (departments.Count == 0)
            //{
            //    ViewBag.NoInstitition = "No Department";
            //    return View(null);
            //}

            //return View(allDepartments);
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var role = await _userManager.GetRolesAsync(currentUser);

            return View(allPoints);
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
               // ////User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
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


        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Posts.View)]
        public async Task<IActionResult> Posts()
        {
           
            var allPosts = await _context.Posts.Include(r=>r.User).ToListAsync();

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var role = await _userManager.GetRolesAsync(currentUser);

            return View(allPosts);
        }

        public async Task<IActionResult> ViewPost(int? id)
        {
            var firstCount = 2;
            ViewBag.Count = _context.Notifications.Where(e => e.Clicked == false && e.Viewed == false).ToList().Count();
            ViewBag.Remaining = ViewBag.Count - firstCount;
            ViewBag.Notifications = _context.Notifications.Select(s => new NotificationViewModel
            {
                Content = s.Content,
                //////User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
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
                //////User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
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

        public async Task<IActionResult> ViewPostComment(int? id)
        {
            var firstCount = 2;
            ViewBag.Count = _context.Notifications.Where(e => e.Clicked == false && e.Viewed == false).ToList().Count();
            ViewBag.Remaining = ViewBag.Count - firstCount;
            ViewBag.Notifications = _context.Notifications.Select(s => new NotificationViewModel
            {
                Content = s.Content,
                //////User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
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
                //////User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
                NotificationId = s.Id,
                Clicked = s.Clicked,
                View = s.Viewed,
                TimeCreated = s.TimeCreated
            }).ToList().OrderByDescending(s => s.TimeCreated).Take(firstCount);
            var result = await _context.Points2Earns.Where(e => e.Id == id).FirstOrDefaultAsync();
            return View(result);

        }


        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Posts.Delete)]
        public async Task<IActionResult> DeletePost(int? id)
        {
            var firstCount = 2;
            ViewBag.Count = _context.Notifications.Where(e => e.Clicked == false && e.Viewed == false).ToList().Count();
            ViewBag.Remaining = ViewBag.Count - firstCount;
            ViewBag.Notifications = _context.Notifications.Select(s => new NotificationViewModel
            {
                Content = s.Content,
                ////User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
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
        
        [Authorize(Policy = Permissions.Permissions.Posts.Delete)]
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
                ////User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
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

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Documents.View)]
        public async Task<IActionResult> Documents()
        {
          
            var allDocuments = await _context.Documents.Include(s=>s.Category).Include(e=>e.Department).Include(a=>a.User).ToListAsync();

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var role = await _userManager.GetRolesAsync(currentUser);

            return View(allDocuments);

           
        }


        public async Task<IActionResult> ViewDocument(int? id)
        {
            var firstCount = 2;
            ViewBag.Count = _context.Notifications.Where(e => e.Clicked == false && e.Viewed == false).ToList().Count();
            ViewBag.Remaining = ViewBag.Count - firstCount;
            ViewBag.Notifications = _context.Notifications.Select(s => new NotificationViewModel
            {
                Content = s.Content,
                ////User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
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


        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Documents.Delete)]
        public async Task<IActionResult> DeleteDocument(int? id)
        {
            var firstCount = 2;
            ViewBag.Count = _context.Notifications.Where(e => e.Clicked == false && e.Viewed == false).ToList().Count();
            ViewBag.Remaining = ViewBag.Count - firstCount;
            ViewBag.Notifications = _context.Notifications.Select(s => new NotificationViewModel
            {
                Content = s.Content,
                ////User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
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


       
        [Authorize(Policy = Permissions.Permissions.Documents.Delete)]
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
                ////User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
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
                ////User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
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

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Points.Edit)]
        public async Task<IActionResult> UpdatePoint(int? id)
        {
           
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
        [Authorize(Policy = Permissions.Permissions.Points.Edit)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePoint(int id, [Bind("Id,Points,Category")] Points2Earn administrator)
        {
            var firstCount = 2;
            ViewBag.Count = _context.Notifications.Where(e => e.Clicked == false && e.Viewed == false).ToList().Count();
            ViewBag.Remaining = ViewBag.Count - firstCount;
            ViewBag.Notifications = _context.Notifications.Select(s => new NotificationViewModel
            {
                Content = s.Content,
                ////User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
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

        [Authorize]
        public async Task<ActionResult> Dashboard()
        {
           
            ViewBag.TotalApprovedBannerAds = _context.BannerAds.ToList().Count();
            var totalBannerAdsCost = _context.BannerAds.Where(a => a.Status == 3).Sum(e => e.Cost);
            //var totalpromotionPayments = _context.PromotionPayments.Where(a => a.Status == 3).Sum(e => e.Amount);
            ViewBag.TotalRevenue = totalBannerAdsCost;
            ViewBag.NoOfpromotions = _context.Promotions.ToList().Count();

            return View();
        }

        public JsonResult GetUsersChartJSON()
        {
            var user_data = _context.Users.GroupBy(user => user.Online).Select(group => new
            {
                Count = group.Count()
            });

            return Json(user_data);
        }

        public JsonResult GetPostsChartJSON()
        {
            var past = DateTime.Now.AddMonths(-7);

            //var post_data = _context.Posts.Where(c => c.TimeCreated > DateTime.Now.AddYears(-1))
            var post_data = _context.Posts
               .GroupBy(c => c.TimeCreated.Month)
            .Select(g => new { Month = g.Key, Count = g.Count() });


            return Json(post_data);
        }

        public async Task<JsonResult> GetCommentsChartJSON()
        {
            //var post_data = _context.Posts.Where(c => c.TimeCreated > DateTime.Now.AddYears(-1))
            var comment_data = await _context.Comments
               .GroupBy(c => c.TimeCreated.Month)
            .Select(g => new { Month = g.Key, Count = g.Count() }).ToArrayAsync();


            return Json(comment_data);
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Departments.View)]
        public async Task<IActionResult> Departments(int? page)
        {
            var allDepartments = await _context.Departments.ToListAsync();
          
            return View(allDepartments);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Permissions.Departments.Create)]
        public async Task<IActionResult> AddDepartment(Department department)
        {
            var secondPartyId = _contextAccessor.HttpContext.Session.GetInt32("secondPartyId");
            ViewBag.Count = _context.Notifications.Where(e => e.SecondPartyId == secondPartyId && e.Clicked == false && e.Viewed == false).ToList().Count();
            ViewBag.Notifications = _context.Notifications.Where(e => e.SecondPartyId == secondPartyId && e.Clicked == false && e.Viewed == false).ToList();
            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();
            return RedirectToAction("Departments");
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Departments.View)]
        public async Task<IActionResult> ViewDepartment(int? id)
        {
            var result = await _context.Departments.Where(e => e.Id == id).FirstOrDefaultAsync();
            return View(result);
        
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Departments.Edit)]
        public async Task<IActionResult> EditDepartment(int? id)
        {
     
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

        [HttpPost]
        [Authorize(Policy = Permissions.Permissions.Departments.Edit)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDepartment(int id, [Bind("Id,Name")] Department administrator)
        {
            var firstCount = 2;
            ViewBag.Count = _context.Notifications.Where(e => e.Clicked == false && e.Viewed == false).ToList().Count();
            ViewBag.Remaining = ViewBag.Count - firstCount;
            ViewBag.Notifications = _context.Notifications.Select(s => new NotificationViewModel
            {
                Content = s.Content,
                ////User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
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
                return RedirectToAction(nameof(Departments));
            }
            return View(administrator);
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Departments.Delete)]
        public async Task<IActionResult> DeleteDepartment(int? id)
        {
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
        [HttpPost, ActionName("DeleteDepartment")]
       
        [Authorize(Policy = Permissions.Permissions.Departments.Delete)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedDepartment(int id)
        {
            var firstCount = 2;
            ViewBag.Count = _context.Notifications.Where(e => e.Clicked == false && e.Viewed == false).ToList().Count();
            ViewBag.Remaining = ViewBag.Count - firstCount;
            ViewBag.Notifications = _context.Notifications.Select(s => new NotificationViewModel
            {
                Content = s.Content,
                ////User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
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

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Institutions.View)]
        public async Task<IActionResult> Institutions(int? page)
        {
           
            var allInstitutions = await _context.Institutions.ToListAsync();

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            return View(allInstitutions);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Permissions.Institutions.Create)]
        public async Task<IActionResult> Create(Institution institution)
        {
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
                ////User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
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
                ////User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
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
        
        [Authorize(Policy = Permissions.Permissions.Institutions.Edit)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Abbreviation")] Institution administrator)
        {
            var firstCount = 2;
            ViewBag.Count = _context.Notifications.Where(e => e.Clicked == false && e.Viewed == false).ToList().Count();
            ViewBag.Remaining = ViewBag.Count - firstCount;
            ViewBag.Notifications = _context.Notifications.Select(s => new NotificationViewModel
            {
                Content = s.Content,
                ////User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
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
        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Institutions.Delete)]
        public async Task<IActionResult> DeleteInstitution(int? id)
        {

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
       
        [Authorize(Policy = Permissions.Permissions.Institutions.Delete)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedInstitution(int id)
        {
            var institution = await _context.Institutions.FindAsync(id);
            if(institution != null)
                _context.Institutions.Remove(institution);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Institutions));
        }

        private bool InstitutionExists(long id)
        {
            return _context.Institutions.Any(e => e.Id == id);
        }
    }
}
