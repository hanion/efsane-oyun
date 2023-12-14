using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using efsane_oyun.Data;
using efsane_oyun.Models;
using Microsoft.AspNetCore.Authorization;

namespace efsane_oyun.Controllers
{
    public class GameTagsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GameTagsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GameTags
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.GameTags.ToListAsync());
        }

        // GET: GameTags/Details/5
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameTags = await _context.GameTags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gameTags == null)
            {
                return NotFound();
            }

            return View(gameTags);
        }

        // GET: GameTags/Create
        [Authorize(Roles = "Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: GameTags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Create([Bind("Id,GameId,TagId")] GameTags gameTags)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gameTags);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gameTags);
        }

        // GET: GameTags/Edit/5
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameTags = await _context.GameTags.FindAsync(id);
            if (gameTags == null)
            {
                return NotFound();
            }
            return View(gameTags);
        }

        // POST: GameTags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GameId,TagId")] GameTags gameTags)
        {
            if (id != gameTags.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gameTags);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameTagsExists(gameTags.Id))
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
            return View(gameTags);
        }

        // GET: GameTags/Delete/5
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameTags = await _context.GameTags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gameTags == null)
            {
                return NotFound();
            }

            return View(gameTags);
        }

        // POST: GameTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gameTags = await _context.GameTags.FindAsync(id);
            if (gameTags != null)
            {
                _context.GameTags.Remove(gameTags);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameTagsExists(int id)
        {
            return _context.GameTags.Any(e => e.Id == id);
        }
    }
}
