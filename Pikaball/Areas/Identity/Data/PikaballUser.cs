using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Pikaball.Models;

namespace Pikaball.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the PikaballUser class
    /// <summary>
    /// The user class along with the model for pokemon collection model 
    /// as they are connected to each other
    /// </summary>
    public class PikaballUser : IdentityUser
    {
        public List<PokemonCollection> PokemonCollections { get; set; }
    }
}
