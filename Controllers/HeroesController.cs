using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuperHeroesDB.Data;
using SuperHeroesDB.Models;

namespace SuperHeroesDB.Controllers
{
    public class HeroesController : Controller
    {
        private readonly HeroesContext _context;

        public HeroesController(HeroesContext context)
        {
            _context = context;
        }

        // GET: Heroes

        //wyszukiwarka
        public async Task<IActionResult> Index(string searchString)
        {
            var heroes = _context.Heroes.Include(h => h.Team).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                heroes = heroes.Where(h => h.HeroName.Contains(searchString));
            }

            return View(await heroes.ToListAsync());
        }

        // GET: Heroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hero = await _context.Heroes
                .Include(h => h.Team)
                .FirstOrDefaultAsync(m => m.HeroId == id);
            if (hero == null)
            {
                return NotFound();
            }

            return View(hero);
        }

        // GET: Heroes/Create
        public IActionResult Create()
        {
            if (!Request.Cookies.ContainsKey("UserLogin"))
            {
                return RedirectToAction("Login", "Authentication");
            }

            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "TeamName");
            return View();
        }

        // POST: Heroes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HeroId,HeroName,FirstName,LastName,TeamId")] Hero hero)
        {
            if (!Request.Cookies.ContainsKey("UserLogin"))
            {
                return RedirectToAction("Login", "Authentication");
            }
                
            if (ModelState.IsValid)
            {
                _context.Add(hero);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "TeamName", hero.TeamId);
            return View(hero);
        }

        // GET: Heroes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!Request.Cookies.ContainsKey("UserLogin"))
            {
                return RedirectToAction("Login", "Authentication");
            }

            if (id == null)
            {
                return NotFound();
            }

            var hero = await _context.Heroes.FindAsync(id);
            if (hero == null)
            {
                return NotFound();
            }
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "TeamName", hero.TeamId);
            return View(hero);
        }

        // POST: Heroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HeroId,HeroName,FirstName,LastName,TeamId")] Hero hero)
        {
            if (!Request.Cookies.ContainsKey("UserLogin"))
            {
                return RedirectToAction("Login", "Authentication");
            }

            if (id != hero.HeroId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hero);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HeroExists(hero.HeroId))
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
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "TeamName", hero.TeamId);
            return View(hero);
        }

        // GET: Heroes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!Request.Cookies.ContainsKey("UserLogin"))
            {
                return RedirectToAction("Login", "Authentication");
            }

            if (id == null)
            {
                return NotFound();
            }

            var hero = await _context.Heroes
                .Include(h => h.Team)
                .FirstOrDefaultAsync(m => m.HeroId == id);
            if (hero == null)
            {
                return NotFound();
            }

            return View(hero);
        }

        // POST: Heroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!Request.Cookies.ContainsKey("UserLogin"))
            {
                return RedirectToAction("Login", "Authentication");
            }

            var hero = await _context.Heroes.FindAsync(id);
            if (hero != null)
            {
                _context.Heroes.Remove(hero);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HeroExists(int id)
        {
            return _context.Heroes.Any(e => e.HeroId == id);
        }
    }
}
