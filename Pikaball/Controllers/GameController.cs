using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pikaball.Data;
using Pikaball.Models;

namespace Pikaball.Controllers
{
    public class GameController : Controller
    {
        private readonly PokemonDBContext _context;

        public GameController(PokemonDBContext context)
        {
            _context = context;
        }

        /*Coverpage*/
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        /*Checks if user has the pokemon, if not: post to create pokemon, else: put to update pokemon level*/
        [HttpGet]
        [HttpPost]
        [HttpPut]
        public IActionResult Play()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Play([Bind("PokedexID,UserID,name,description,level,LastDrawn,HasNextEvolution,EvCondition,EvolutionUnlocked,SpriteUrl,Type1,Type2")] PokemonCollection pokemonCollection)
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Collection(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var pokemonDBContext = from s in _context.PokemonCollections
                                   select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                pokemonDBContext = pokemonDBContext.Where(s => s.name.Contains(searchString));
            }
            pokemonDBContext = pokemonDBContext.OrderByDescending(s => s.LastDrawn);
            return View(await pokemonDBContext.ToListAsync());
        }


        /*Error page*/
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
