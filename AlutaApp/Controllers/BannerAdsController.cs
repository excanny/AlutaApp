﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlutaApp.Data;
using AlutaApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace AlutaApp.Controllers
{
    [Authorize]
    public class BannerAdsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BannerAdsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BannerAds
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BannerAds.Include(b => b.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BannerAds/Details/5
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

        // GET: BannerAds/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName");
            return View();
        }

        // POST: BannerAds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", bannerAd.UserId);
            return View(bannerAd);
        }

        // POST: BannerAds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,BannerLink,InstitutionId,DepartmentId,Gender,StartDate,EndDate,Status")] BannerAd bannerAd)
        {
            if (id != bannerAd.Id)
            {
                return NotFound();
            }
            bannerAd.User = _context.Users.Where(d=>d.Id == bannerAd.UserId).FirstOrDefault();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bannerAd);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BannerAdExists(bannerAd.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", bannerAd.UserId);
            return View(bannerAd);
        }

        // GET: BannerAds/Delete/5
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