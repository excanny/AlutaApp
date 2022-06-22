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

        // GET: Trivias/Edit/5
        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Trivias.Edit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trivia = await _context.TriviaQuestions.FindAsync(id);
            if (trivia == null)
            {
                return NotFound();
            }
            return View(trivia);
        }

        // GET: Trivias/Delete/5
        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Trivias.Delete)]
        public async Task<IActionResult> Delete(int? id)
        {
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

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Trivias.View)]
        public async Task<IActionResult> Trivias(int? page)
        {

            //var alltrivias = await _context.Trivias.Include(a => a.UserResults).Include(a => a.Questions).Include(q => q.Attempts).ToListAsync();
            var alltrivias = await _context.TriviaQuestions.ToListAsync();

            return View(alltrivias);
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Trivias.View)]
        public async Task<IActionResult> TriviaAttempts(int? page, int triviaId)
        {
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
        public async Task<IActionResult> Create(TriviaQuestion triviaQuestion)
        {

            if (ModelState.IsValid)
            {
                _context.Add(triviaQuestion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Trivias));
            }
           return View(triviaQuestion);
        }

        

        // POST: Trivias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        [Authorize(Policy = Permissions.Permissions.Trivias.Edit)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TriviaQuestion triviaQuestion)
        {
            if (id != triviaQuestion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                var triviaQuestionDetails = await _context.TriviaQuestions.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
                if (triviaQuestionDetails != null)
                {
                    triviaQuestionDetails.Question = triviaQuestion.Question;
                    triviaQuestionDetails.Option1 = triviaQuestion.Option1;
                    triviaQuestionDetails.Option2 = triviaQuestion.Option2;
                    triviaQuestionDetails.Option3 = triviaQuestion.Option3;
                    triviaQuestionDetails.Option4 = triviaQuestion.Option4;
                    triviaQuestionDetails.CorrectOption = triviaQuestion.CorrectOption;
                }
                    

                try
                {

                    _context.Update(triviaQuestionDetails);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Trivias));
                }
                catch (DbUpdateConcurrencyException)
                {
                   
                }
                return RedirectToAction(nameof(Trivias));
            }
            return View();
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
