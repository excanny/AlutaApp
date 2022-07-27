using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlutaApp.Data;
using AlutaApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AlutaApp.ViewModels;

namespace AlutaApp.Controllers
{
    [Authorize]
    public class BannerAdsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public BannerAdsController(ApplicationDbContext context, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.BannerAds.View)]
        public async Task<IActionResult> Index()
        {
            var allBannerAds = await _context.BannerAds.Include(b => b.User).Include(b => b.Department).Include(b => b.Institution).ToListAsync();

            return View(allBannerAds);
        }

        // GET: BannerAds/Details/5

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.BannerAds.View)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bannerAd = await _context.BannerAds
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bannerAd == null)
            {
                return NotFound();
            }

            return View(bannerAd);
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.BannerAds.Create)]
        public async Task<IActionResult> Create()
        {
            ViewBag.UsersList = await _context.Users.Where(q => !q.Deleted).ToListAsync();
            return View();
        }

        // POST: BannerAds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

      
        [Authorize(Policy = Permissions.Permissions.BannerAds.Create)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,BannerLink,InstitutionId,DepartmentId,Gender,StartDate,EndDate,Status")] BannerAd bannerAd)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bannerAd);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", bannerAd.UserId);
            return View(bannerAd);
        }

        // GET: BannerAds/Edit/5

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.BannerAds.Edit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bannerAd = await _context.BannerAds.FindAsync(id);
            if (bannerAd == null)
            {
                return NotFound();
            }
            
            return View(bannerAd);
        }

        // POST: BannerAds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = Permissions.Permissions.BannerAds.Edit)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,BannerLink,InstitutionId,DepartmentId,Gender,StartDate,EndDate,Status")] BannerAdViewModel? bannerAd)
        {
            if (id != bannerAd.Id)
            {
                return NotFound();
            }
            var newbannerAd = await _context.BannerAds.Where(d => d.Id == bannerAd.Id).FirstOrDefaultAsync();
            if (ModelState.IsValid)
            {
                try
                {
                    if (newbannerAd == null) return NotFound();

                     newbannerAd.BannerLink = bannerAd.BannerLink;
                     newbannerAd.InstitutionId = bannerAd.InstitutionId;
                     newbannerAd.DepartmentId = bannerAd.DepartmentId;
                     newbannerAd.Gender = bannerAd.Gender;
                     newbannerAd.StartDate = bannerAd.StartDate;
                     newbannerAd.EndDate = bannerAd.EndDate;
                     newbannerAd.Status = bannerAd.Status;

                    _context.Update(newbannerAd);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) { }
                
                return RedirectToAction(nameof(Index));
            }
            
            return View(bannerAd);
        }

        // GET: BannerAds/Delete/5

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.BannerAds.Delete)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bannerAd = await _context.BannerAds
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bannerAd == null)
            {
                return NotFound();
            }

            return View(bannerAd);
        }

        // POST: BannerAds/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = Permissions.Permissions.BannerAds.Delete)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bannerAd = await _context.BannerAds.FindAsync(id);
            _context.BannerAds.Remove(bannerAd);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BannerAdExists(int id)
        {
            return _context.BannerAds.Any(e => e.Id == id);
        }
    }
}
