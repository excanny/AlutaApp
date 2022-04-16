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
using Microsoft.AspNetCore.Authorization;

namespace AlutaApp.Controllers
{
    [Authorize]
    public class ChatGroupsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChatGroupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetMessagesByChatGroup(int? id)
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
            var getIt = await _context.GroupMessages.Include(d=>d.Sender).ToListAsync();

            GroupChatList chatList = new GroupChatList();
            var gets = _context.ChatGroups.Where(s => s.Id == id).ToList();


            chatList.ChatGroupId = gets.DistinctBy(s => new { chatGroupId = s.Id }).Select(s => s.Id).ToList();


            var mess = _context.ChatGroups.ToList();

            chatList.MessageInfos = await _context.GroupMessages.Include(w=>w.Sender).Where(e => e.ChatGroupId == id).Select(s => new MessageInfo { Contents = s.Content, Senders = s.Sender.UserName, TimeCreated = s.TimeCreated}).ToListAsync();
            chatList.ChatGroupName = mess.DistinctBy(s => new { chatGroupId = s.Id }).Select(s => s.Name).ToList();

            //List<ChatGroupMessage> groupMessages = getIt.DistinctBy(s=> new {chatGroupId = s.ChatGroupId}).ToList();

            return View(chatList);
        }


        public ActionResult BarChart()
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
            return View();
        }
        public async Task<IActionResult> GetMessagesByUser(int? id)
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

            var getMessages = await _context.GroupMessages.Where(s => s.UserId == id).Include(d=>d.Sender).ToListAsync();
            return View(getMessages);
        }

        // GET: ChatGroups
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
            return View(await _context.ChatGroups.Include(s=>s.Department).Include(d=>d.Institution).ToListAsync());
        }

        // GET: ChatGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatGroup = await _context.ChatGroups.Include(s=>s.Messages).Include(s=>s.Users)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatGroup == null)
            {
                return NotFound();
            }

            return View(chatGroup);
        }

        // GET: ChatGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ChatGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,InstitutionId,DepartmentId,YearOfAdmission,Name,GroupPhotoLink")] ChatGroup chatGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chatGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chatGroup);
        }

        // GET: ChatGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatGroup = await _context.ChatGroups.FindAsync(id);
            if (chatGroup == null)
            {
                return NotFound();
            }
            return View(chatGroup);
        }

        // POST: ChatGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InstitutionId,DepartmentId,YearOfAdmission,Name,GroupPhotoLink")] ChatGroup chatGroup)
        {
            if (id != chatGroup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chatGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChatGroupExists(chatGroup.Id))
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
            return View(chatGroup);
        }

        // GET: ChatGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatGroup = await _context.ChatGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatGroup == null)
            {
                return NotFound();
            }

            return View(chatGroup);
        }

        // POST: ChatGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chatGroup = await _context.ChatGroups.FindAsync(id);
            _context.ChatGroups.Remove(chatGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChatGroupExists(int id)
        {
            return _context.ChatGroups.Any(e => e.Id == id);
        }
    }
}
