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
using Microsoft.AspNetCore.Identity;

namespace AlutaApp.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public MessagesController(ApplicationDbContext context, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: Messages/Create
        [HttpGet]
        public IActionResult GetMessages()
        {
            var vm = new UserListViewModel();
            vm.Users = _context.Users
                                  .Select(a => new SelectListItem()
                                  {
                                      Value = a.Id.ToString(),
                                      Text = a.FullName
                                  })
                                  .ToList();
            return View(vm);

        }


        [HttpPost]
        [Authorize(Policy = Permissions.Permissions.Messages.View)]
        public async Task<IActionResult> Messages(int userId1, int userId2)
        {
            ViewBag.UserId1 = userId1;
            ViewBag.UserId2 = userId2;
            var allMessages = await _context.Messages
                .Where(x => ((x.SenderId == userId1 && x.RecieverId == userId2) || (x.RecieverId == userId1 && x.SenderId == userId2)))
                .Include(x => x.ParentMessage)
                .ThenInclude(x => x.Sender)
                .Include(x => x.Post.Video)
                .Select(x => new MessageSingleDTO
                {
                    Id = x.Id,
                    Content = x.Content,
                    MediaLink = x.MediaLink,
                    IsReel = x.Post.Video != null,
                    SenderId = x.SenderId,
                    RecieverId = x.RecieverId,
                    ParentMessage = x.ParentMessage != null ? new ParentMessageDTO
                    {
                        Id = x.ParentMessage.Id,
                        UserName = x.ParentMessage.Sender.FullName,
                        Content = x.ParentMessage.Content,
                        MediaLink = x.ParentMessage.MediaLink,
                        PostId = x.ParentMessage.PostId,
                        Deleted = x.ParentMessage.Deleted
                    } : null,
                    PostId = x.PostId,
                    TimeCreated = x.TimeCreated.ToUniversalTime(),
                    Delivered = x.Delivered,
                    Read = x.Read,
                    Deleted = x.Deleted
                }).OrderByDescending(x => x.Id).ToListAsync();

            return View(allMessages);

        }
        // GET: Messages
        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Messages.View)]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Messages.Include(m => m.Reciever).Include(m => m.Sender);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Messages/Details/5
        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Messages.View)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages
                .Include(m => m.Reciever)
                .Include(m => m.Sender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // GET: Messages/Create
        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Messages.Create)]
        public IActionResult Create()
        {
            ViewData["RecieverId"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["SenderId"] = new SelectList(_context.Users, "Id", "FullName");
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        [Authorize(Policy = Permissions.Permissions.Messages.Create)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content,SenderId,RecieverId,Delivered,Read,TimeCreated,Deleted")] Message message)
        {
            if (ModelState.IsValid)
            {
                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RecieverId"] = new SelectList(_context.Users, "Id", "FullName", message.RecieverId);
            ViewData["SenderId"] = new SelectList(_context.Users, "Id", "FullName", message.SenderId);
            return View(message);
        }

        // GET: Messages/Edit/5
        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Messages.Edit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            ViewData["RecieverId"] = new SelectList(_context.Users, "Id", "FullName", message.RecieverId);
            ViewData["SenderId"] = new SelectList(_context.Users, "Id", "FullName", message.SenderId);
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        [Authorize(Policy = Permissions.Permissions.Messages.Edit)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,SenderId,RecieverId,Delivered,Read,TimeCreated,Deleted")] Message message)
        {
            if (id != message.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message.Id))
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
            ViewData["RecieverId"] = new SelectList(_context.Users, "Id", "FullName", message.RecieverId);
            ViewData["SenderId"] = new SelectList(_context.Users, "Id", "FullName", message.SenderId);
            return View(message);
        }

        // GET: Messages/Delete/5
        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Messages.Delete)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages
                .Include(m => m.Reciever)
                .Include(m => m.Sender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
       
        [Authorize(Policy = Permissions.Permissions.Messages.Delete)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageExists(int id)
        {
            return _context.Messages.Any(e => e.Id == id);
        }
    }
}
