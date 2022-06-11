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
using X.PagedList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AlutaApp.Controllers
{
    [Authorize]
    public class DocumentCommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public DocumentCommentsController(ApplicationDbContext context, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: DocumentComments
        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.DocumentComments.View)]
        public async Task<IActionResult> Index()
        {
           
            var documentComments = _context.DocumentComments.ToList();
            

            return View(documentComments);
        }


        // GET: DocumentComments/Details/5
        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.DocumentComments.View)]
        public async Task<IActionResult> Details(int? id)
        {
           
            var likers = _context.DocumentCommentLikes.Where(e=>e.DocumentCommentId  == id).ToList();

            //var getDocumentLikers = likers.Select(d=> new Likers{
            //    Name = _context.Users.Where(e=>e.Id == d.UserId).Select(e=>e.FullName).FirstOrDefault(),
            //    TimeLiked = d.TimeCreated
            //}).ToList();

            //var documentLikers = await getDocumentLikers.OrderByDescending(s => s.TimeLiked).ToPagedListAsync(pageIndex, pageSize);
            //ViewBag.Likers = documentLikers;
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var documentComment = await _context.DocumentComments
            //    .Include(d => d.User).Include(e=>e.Likes)
            //    .FirstOrDefaultAsync(m => m.Id == id);

            var documentComment = await _context.DocumentComments
                
                .FirstOrDefaultAsync(m => m.Id == id);

            if (documentComment == null)
            {
                return NotFound();
            }

            return View(documentComment);
        }

        // GET: DocumentComments/Create
        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.DocumentComments.Create)]
        public IActionResult Create()
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName");
            return View();
        }

        // POST: DocumentComments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       
        [Authorize(Policy = Permissions.Permissions.DocumentComments.Create)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,DocumentId,Content,TimeCreated")] DocumentComment documentComment)
        {
            var firstCount = 2;
            ViewBag.Count = _context.Notifications.Where(e => e.Clicked == false && e.Viewed == false).ToList().Count();
            ViewBag.Remaining = ViewBag.Count - firstCount;
            ViewBag.Notifications = _context.Notifications.Select(s => new NotificationViewModel
            {
                Content = s.Content,
               // User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
                NotificationId = s.Id,
                Clicked = s.Clicked,
                View = s.Viewed,
                TimeCreated = s.TimeCreated
            }).ToList().OrderByDescending(s => s.TimeCreated).Take(firstCount);
            if (ModelState.IsValid)
            {
                _context.Add(documentComment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", documentComment.UserId);
            return View(documentComment);
        }

        // GET: DocumentComments/Edit/5
        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.DocumentComments.Edit)]
        public async Task<IActionResult> Edit(int? id)
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
            if (id == null)
            {
                return NotFound();
            }

            var documentComment = await _context.DocumentComments.FindAsync(id);
            if (documentComment == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", documentComment.UserId);
            return View(documentComment);
        }

        // POST: DocumentComments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        [Authorize(Policy = Permissions.Permissions.DocumentComments.Edit)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,DocumentId,Content,TimeCreated")] DocumentComment documentComment)
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
            if (id != documentComment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(documentComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentCommentExists(documentComment.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", documentComment.UserId);
            return View(documentComment);
        }

        // GET: DocumentComments/Delete/5
        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.DocumentComments.Delete)]
        public async Task<IActionResult> Delete(int? id)
        {
            var firstCount = 2;
            ViewBag.Count = _context.Notifications.Where(e => e.Clicked == false && e.Viewed == false).ToList().Count();
            ViewBag.Remaining = ViewBag.Count - firstCount;
            ViewBag.Notifications = _context.Notifications.Select(s => new NotificationViewModel
            {
                Content = s.Content,
               // User = _context.Users.Where(e => e.Id == s.UserId).FirstOrDefault().FullName,
                NotificationId = s.Id,
                Clicked = s.Clicked,
                View = s.Viewed,
                TimeCreated = s.TimeCreated
            }).ToList().OrderByDescending(s => s.TimeCreated).Take(firstCount);
            if (id == null)
            {
                return NotFound();
            }

            var documentComment = await _context.DocumentComments
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (documentComment == null)
            {
                return NotFound();
            }

            return View(documentComment);
        }

        // POST: DocumentComments/Delete/5
        [HttpPost, ActionName("Delete")]
        
        [Authorize(Policy = Permissions.Permissions.DocumentComments.Delete)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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
            var documentComment = await _context.DocumentComments.FindAsync(id);
            _context.DocumentComments.Remove(documentComment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentCommentExists(int id)
        {
            return _context.DocumentComments.Any(e => e.Id == id);
        }
    }
}
