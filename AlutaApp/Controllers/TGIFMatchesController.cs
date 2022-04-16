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

namespace AlutaApp.Controllers
{
    [Authorize]
    public class TGIFMatchesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TGIFMatchesController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> TGIFs(int? page)
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
            var alltrivias = await _context.TGIFMatches.Include(s => s.Messages).ToListAsync();
            var trivias = await alltrivias.ToPagedListAsync(pageIndex, pageSize);
            ViewData["FemaleId"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["MaleId"] = new SelectList(_context.Users, "Id", "FullName");
            return View(trivias);
        }
        // GET: TGIFMatches
        public async Task<IActionResult> Index()
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
            var applicationDbContext = _context.TGIFMatches.Include(t => t.Female).Include(t => t.Male);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TGIFMatches/Details/5
        public async Task<IActionResult> Details(int? id)
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

            var tGIFMatch = await _context.TGIFMatches
                .Include(t => t.Female)
                .Include(t => t.Male)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tGIFMatch == null)
            {
                return NotFound();
            }

            return View(tGIFMatch);
        }

        // GET: TGIFMatches/Create
        public IActionResult Create()
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
            ViewData["FemaleId"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["MaleId"] = new SelectList(_context.Users, "Id", "FullName");
            return View();
        }

        // POST: TGIFMatches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MaleId,FemaleId,MaleStatus,FemaleStatus,DateOfExpiry")] TGIFMatch tGIFMatch)
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
            if (ModelState.IsValid)
            {
                tGIFMatch.DateMatched = DateTime.Now;
                _context.Add(tGIFMatch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FemaleId"] = new SelectList(_context.Users, "Id", "FullName", tGIFMatch.FemaleId);
            ViewData["MaleId"] = new SelectList(_context.Users, "Id", "FullName", tGIFMatch.MaleId);
            return View(tGIFMatch);
        }

        // GET: TGIFMatches/Edit/5
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

            var tGIFMatch = await _context.TGIFMatches.FindAsync(id);
            if (tGIFMatch == null)
            {
                return NotFound();
            }
            ViewData["FemaleId"] = new SelectList(_context.Users, "Id", "FullName", tGIFMatch.FemaleId);
            ViewData["MaleId"] = new SelectList(_context.Users, "Id", "FullName", tGIFMatch.MaleId);
            return View(tGIFMatch);
        }

        // POST: TGIFMatches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MaleId,FemaleId,MaleStatus,FemaleStatus,DateMatched,DateOfExpiry")] TGIFMatch tGIFMatch)
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
            if (id != tGIFMatch.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tGIFMatch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TGIFMatchExists(tGIFMatch.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(TGIFs));
            }
            ViewData["FemaleId"] = new SelectList(_context.Users, "Id", "FullName", tGIFMatch.FemaleId);
            ViewData["MaleId"] = new SelectList(_context.Users, "Id", "FullName", tGIFMatch.MaleId);
            return View(tGIFMatch);
        }

        // GET: TGIFMatches/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            var tGIFMatch = await _context.TGIFMatches
                .Include(t => t.Female)
                .Include(t => t.Male)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tGIFMatch == null)
            {
                return NotFound();
            }

            return View(tGIFMatch);
        }

        // POST: TGIFMatches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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
            var tGIFMatch = await _context.TGIFMatches.FindAsync(id);
            _context.TGIFMatches.Remove(tGIFMatch);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(TGIFs));
        }

        private bool TGIFMatchExists(int id)
        {
            return _context.TGIFMatches.Any(e => e.Id == id);
        }
    }
}
