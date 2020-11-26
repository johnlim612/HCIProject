using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pikaball.Data;
using Pikaball.Models;

namespace Pikaball.Controllers
{
    public class PokemonCollectionsController : Controller
    {
        private readonly PokemonDBContext _context;

        public PokemonCollectionsController(PokemonDBContext context)
        {
            _context = context;
        }

        // GET: PokemonCollections
        public async Task<IActionResult> Index()
        {
            var pokemonDBContext = _context.PokemonCollections.Include(p => p.PikaballUser);
            return View(await pokemonDBContext.ToListAsync());
        }

        // GET: PokemonCollections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pokemonCollection = await _context.PokemonCollections
                .Include(p => p.PikaballUser)
                .FirstOrDefaultAsync(m => m.PokedexID == id);
            if (pokemonCollection == null)
            {
                return NotFound();
            }

            return View(pokemonCollection);
        }

        // GET: PokemonCollections/Create
        public IActionResult Create()
        {
            ViewData["UserID"] = new SelectList(_context.PikaballUsers, "Id", "Id");
            return View();
        }

        // POST: PokemonCollections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PokedexID,UserID,name,description,level,LastDrawn,HasNextEvolution,EvCondition,EvolutionUnlocked,SpriteUrl,Type1,Type2")] PokemonCollection pokemonCollection)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pokemonCollection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.PikaballUsers, "Id", "Id", pokemonCollection.UserID);
            return View(pokemonCollection);
        }

        // GET: PokemonCollections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pokemonCollection = await _context.PokemonCollections.FindAsync(id);
            if (pokemonCollection == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.PikaballUsers, "Id", "Id", pokemonCollection.UserID);
            return View(pokemonCollection);
        }

        // POST: PokemonCollections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PokedexID,UserID,name,description,level,LastDrawn,HasNextEvolution,EvCondition,EvolutionUnlocked,SpriteUrl,Type1,Type2")] PokemonCollection pokemonCollection)
        {
            if (id != pokemonCollection.PokedexID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pokemonCollection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PokemonCollectionExists(pokemonCollection.PokedexID))
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
            ViewData["UserID"] = new SelectList(_context.PikaballUsers, "Id", "Id", pokemonCollection.UserID);
            return View(pokemonCollection);
        }

        // GET: PokemonCollections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pokemonCollection = await _context.PokemonCollections
                .Include(p => p.PikaballUser)
                .FirstOrDefaultAsync(m => m.PokedexID == id);
            if (pokemonCollection == null)
            {
                return NotFound();
            }

            return View(pokemonCollection);
        }

        // POST: PokemonCollections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pokemonCollection = await _context.PokemonCollections.FindAsync(id);
            _context.PokemonCollections.Remove(pokemonCollection);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PokemonCollectionExists(int id)
        {
            return _context.PokemonCollections.Any(e => e.PokedexID == id);
        }
    }
}
