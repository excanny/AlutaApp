using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlutaApp.Data;
using AlutaApp.Models;
using X.PagedList;
using AlutaApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AlutaApp.Controllers
{
    [Authorize]
    public class TriviasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public TriviasController(ApplicationDbContext context, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: Trivias

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Trivias.View)]
        public async Task<IActionResult> Trivias(int? page)
        {

            //var alltrivias = await _context.Trivias.Include(a => a.UserResults).Include(a => a.Questions).Include(q => q.Attempts).ToListAsync();
            var alltrivias = await _context.Trivias.ToListAsync();

            return View(alltrivias);
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Trivias.View)]
        public async Task<IActionResult> TriviaAttempts(int? page, int triviaId)
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
            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            //var alltrivias = await _context.Trivias.Where(e=>e.Id == triviaId).Include(q => q.Attempts).ToListAsync();
            //var trivias = await alltrivias.ToPagedListAsync(pageIndex, pageSize);
            return View();
        }
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Trivias.ToListAsync());
        //}

        // GET: Trivias/Details/5
        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Trivias.View)]
        public async Task<IActionResult> Details(int? id)
        {
           
            if (id == null)
            {
                return NotFound();
            }

            //var trivia = await _context.Trivias.Include(s=>s.Questions).Include(s=>s.UserResults).Include(s=>s.Attempts)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (trivia == null)
            //{
            //    return NotFound();
            //}

            return View();
        }

        // GET: Trivias/Create
        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Trivias.Edit)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Trivias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        
        [Authorize(Policy = Permissions.Permissions.Trivias.Create)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PlayDate")] Trivia trivia)
        {
           
            if (ModelState.IsValid)
            {
                _context.Add(trivia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Trivias));
            }
           return View(trivia);
        }

        // GET: Trivias/Edit/5
        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Trivias.Edit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trivia = await _context.Trivias.FindAsync(id);
            if (trivia == null)
            {
                return NotFound();
            }
            return View(trivia);
        }

        // POST: Trivias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        [Authorize(Policy = Permissions.Permissions.Trivias.Edit)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PlayDate")] Trivia trivia)
        {
            if (id != trivia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                var triviaDetails = await _context.Trivias.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
                if (triviaDetails != null)
                    triviaDetails.PlayDate = trivia.PlayDate;

                try
                {

                    _context.Update(trivia);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Trivias));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TriviaExists(trivia.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Trivias));
            }
            return View(trivia);
        }

        // GET: Trivias/Delete/5
        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Trivias.Delete)]
        public async Task<IActionResult> Delete(int? id)
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

            var trivia = await _context.Trivias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trivia == null)
            {
                return NotFound();
            }

            return View(trivia);
        }

        // POST: Trivias/Delete/5
        [HttpPost, ActionName("Delete")]
       
        [Authorize(Policy = Permissions.Permissions.Trivias.Delete)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trivia = await _context.Trivias.FindAsync(id);
            _context.Trivias.Remove(trivia);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Trivias));
        }

        private bool TriviaExists(int id)
        {
            return _context.Trivias.Any(e => e.Id == id);
        }
    }
}
