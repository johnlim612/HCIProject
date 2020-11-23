using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pikaball.Models;

namespace Pikaball.Controllers
{
    public class GameController : Controller
    {
        private readonly ILogger<GameController> _logger;

        public GameController(ILogger<GameController> logger)
        {
            _logger = logger;
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

        /*Collection page, get request for user pokemon collection, Requires login*/
        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Collection()
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
