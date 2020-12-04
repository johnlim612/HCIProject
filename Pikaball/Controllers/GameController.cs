using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pikaball.Data;
using Pikaball.Models;

namespace Pikaball.Controllers
{
    public class GameController : Controller
    {
        private readonly PokemonDBContext MyContext;

        public GameController(PokemonDBContext context)
        {
            MyContext = context;
        }

        /// <summary>
        /// Coverpage that user sees on default startup
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Returns the play view screen where the javascript/jquery functions will start when user
        /// clicks on the pokeball in the center
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Play()
        {
            return View();
        }

        /// <summary>
        /// API request for getting the array of user's pokemon collection.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Game/GetCollection")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> GetCollection()
        {
            var pokemonDBContext = from s in MyContext.PokemonCollections select s;
            pokemonDBContext = pokemonDBContext.Where(s => s.UserID.Contains(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Json(await pokemonDBContext.ToListAsync());
        }

        /// <summary>
        /// This is where the user's pokemon will be added into their collection
        /// The pokemon will either be added or updated depending on whether the pokemon has a
        /// userID
        /// Pokemon object or PokemonCollection is created from the json format sent from an ajax call
        /// in the javascript
        /// </summary>
        /// <param name="pokemon"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/UpdateCollection")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public PokemonCollection UpdateCollection([FromBody] PokemonCollection pokemon)
        {
            PokemonCollection newPokemon = pokemon;
            Console.WriteLine(pokemon);
            newPokemon.LastDrawn = DateTime.Now;
            if (!String.IsNullOrEmpty(newPokemon.UserID))
            {
                Console.WriteLine(newPokemon.name);
                MyContext.PokemonCollections.Update(newPokemon);
            }
            else
            {
                newPokemon.UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Console.WriteLine(newPokemon.name);
                MyContext.PokemonCollections.Add(newPokemon);
            }
            MyContext.SaveChanges();
            return newPokemon;
        }

        /// <summary>
        /// This collection takes in a search string that would search through the database for the
        /// searched pokemon. If the string is null or empty, the database would return all the
        /// pokemon in the user's collection and display them in the view in a grid format
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Collection(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var pokemonDBContext = from s in MyContext.PokemonCollections
                                   select s;
            //this part filters out pokemon owned by other users
            pokemonDBContext = pokemonDBContext.Where(s => s.UserID.Contains(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            
            if (!String.IsNullOrEmpty(searchString))
            {
                pokemonDBContext = pokemonDBContext.Where(s => s.name.Contains(searchString));
            }
            pokemonDBContext = pokemonDBContext.OrderByDescending(s => s.LastDrawn);
            return View(await pokemonDBContext.ToListAsync());
        }

        [AllowAnonymous]
        public IActionResult HowTo()
        {
            return View();
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
