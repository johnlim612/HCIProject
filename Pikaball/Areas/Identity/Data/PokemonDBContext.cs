using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Pikaball.Areas.Identity.Data;
using Pikaball.Models;

namespace Pikaball.Data
{
    /// <summary>
    /// Using default template so far
    /// </summary>
    public class PokemonDBContext : IdentityDbContext<PikaballUser>
    {
        public PokemonDBContext(DbContextOptions<PokemonDBContext> options)
            : base(options)
        {
        }
        public DbSet<PokemonCollection> PokemonCollections { get; set; }
        public DbSet<PikaballUser> PikaballUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PokemonCollection>()
                .HasKey(k => new { k.PokedexID, k.UserID });
            builder.Entity<PokemonCollection>()
                .HasOne(pCollection => pCollection.PikaballUser)
                .WithMany(pCollection => pCollection.PokemonCollections)
                .HasForeignKey(pCollection => pCollection.UserID)
                .IsRequired();
        }
    }

    /// <summary>
    /// This factory class is for bypassing the problem where no parameterless constructor existed for
    /// PokemonDBContext. This was an error that shouldn't have happened, however it did. 
    /// Probably due to an internal error from VisualStudio Asp.net core mvc
    /// trying to do something that it shouldn't have been doing.
    /// </summary>
    public class PokemonDBContextFactory : IDesignTimeDbContextFactory<PokemonDBContext>
    {
        public PokemonDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PokemonDBContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Pikaball;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new PokemonDBContext(optionsBuilder.Options);
        }
    }
}
