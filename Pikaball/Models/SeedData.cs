using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pikaball.Data;
using System;
using System.Linq;

namespace Pikaball.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new PokemonDBContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<PokemonDBContext>>()))
            {
                // Look for any movies.
                if (context.PokemonCollections.Any())
                {
                    return;   // DB has been seeded
                }
                var pokemons = new PokemonCollection[]
                {
                new PokemonCollection
                {
                    PokedexID=1,
                    UserID = "09294edb-4bd7-4ea9-ae2d-eee04166814e",
                    name="bulbasaur",
                    description = "While it is young, it uses the nutrients that are stored in the seed on its back in order to grow.",
                    level=1,
                    LastDrawn=DateTime.Now,
                    EvolutionCondition=16,
                    EvolutionUnlocked=false,
                    SpriteUrl="https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/1.png",
                    Type1="Grass",
                    Type2="Poison"
                },
               
                };
                foreach (PokemonCollection s in pokemons)
                {
                    context.PokemonCollections.Add(s);
                }
                context.SaveChanges();
            }
        }
    }
}
