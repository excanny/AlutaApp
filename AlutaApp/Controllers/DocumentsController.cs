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

namespace AlutaApp.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DocumentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Documents
        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Documents.View)]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Documents.Include(d => d.Category).Include(d => d.Department).Include(d => d.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Documents/Details/5
        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Documents.View)]
        public async Task<IActionResult> Details(int? id)
        {
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

        // GET: Documents/Create
        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Documents.Create)]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.DocumentCategories, "Id", "Id");
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName");
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        [Authorize(Policy = Permissions.Permissions.Documents.Create)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,DepartmentId,CategoryId,Title,DocumentLink,DocumentThumbnailLink,TotalDownloads,TimeCreated,LastUpdated")] Document document)
        {
            if (ModelState.IsValid)
            {
                _context.Add(document);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.DocumentCategories, "Id", "Id", document.CategoryId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", document.DepartmentId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", document.UserId);
            return View(document);
        }

        // GET: Documents/Edit/5
        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Documents.Edit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.DocumentCategories, "Id", "Id", document.CategoryId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", document.DepartmentId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", document.UserId);
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       
        [Authorize(Policy = Permissions.Permissions.Documents.Edit)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,DepartmentId,CategoryId,Title,DocumentLink,DocumentThumbnailLink,TotalDownloads,TimeCreated,LastUpdated")] Document document)
        {
            if (id != document.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.DocumentCategories, "Id", "Id", document.CategoryId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", document.DepartmentId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", document.UserId);
            return View(document);
        }

        // GET: Documents/Delete/5
        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Documents.Delete)]
        public async Task<IActionResult> Delete(int? id)
        {
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

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
       
        [Authorize(Policy = Permissions.Permissions.Documents.Delete)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentExists(int id)
        {
            return _context.Documents.Any(e => e.Id == id);
        }
    }
}
