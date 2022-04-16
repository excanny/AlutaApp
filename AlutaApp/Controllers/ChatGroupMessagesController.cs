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
    public class ChatGroupMessagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChatGroupMessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ChatGroupMessages
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GroupMessages.Include(c => c.Sender);
            return View(await applicationDbContext.ToListAsync());
        }


        
        // GET: ChatGroupMessages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatGroupMessage = await _context.GroupMessages
                .Include(c => c.Sender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatGroupMessage == null)
            {
                return NotFound();
            }

            return View(chatGroupMessage);
        }

        // GET: ChatGroupMessages/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName");
            return View();
        }

        // POST: ChatGroupMessages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ChatGroupId,UserId,Content,Deleted,TimeCreated")] ChatGroupMessage chatGroupMessage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chatGroupMessage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", chatGroupMessage.UserId);
            return View(chatGroupMessage);
        }

        // GET: ChatGroupMessages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatGroupMessage = await _context.GroupMessages.FindAsync(id);
            if (chatGroupMessage == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", chatGroupMessage.UserId);
            return View(chatGroupMessage);
        }

        // POST: ChatGroupMessages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ChatGroupId,UserId,Content,Deleted,TimeCreated")] ChatGroupMessage chatGroupMessage)
        {
            if (id != chatGroupMessage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chatGroupMessage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChatGroupMessageExists(chatGroupMessage.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", chatGroupMessage.UserId);
            return View(chatGroupMessage);
        }

        // GET: ChatGroupMessages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatGroupMessage = await _context.GroupMessages
                .Include(c => c.Sender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatGroupMessage == null)
            {
                return NotFound();
            }

            return View(chatGroupMessage);
        }

        // POST: ChatGroupMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chatGroupMessage = await _context.GroupMessages.FindAsync(id);
            _context.GroupMessages.Remove(chatGroupMessage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChatGroupMessageExists(int id)
        {
            return _context.GroupMessages.Any(e => e.Id == id);
        }
    }
}
