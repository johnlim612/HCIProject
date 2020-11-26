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
                context.SaveChanges();
            }
        }
    }
}
