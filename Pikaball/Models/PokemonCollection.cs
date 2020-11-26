using Pikaball.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pikaball.Models
{
    public class PokemonCollection
    {
        //Pokedex number of the pokemon
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PokedexID { get; set; }

        //Foreign Key from PikaballUser ID
        [ForeignKey("PikaballUserID")]
        public string UserID { get; set; }

        public PikaballUser PikaballUser { get; set; }

        [Required]
        public string name { get; set; }
        public string description { get; set; }

        [Required]
        public int level { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime LastDrawn { get; set; }

        [Required]
        public Boolean HasNextEvolution {get; set;}
        public int? EvCondition { get; set; }
        public Boolean? EvolutionUnlocked { get; set; }
        public string SpriteUrl { get; set; }
        public PokemonType Type1 { get; set; }
        public PokemonType? Type2 { get; set; }

    }

    public enum PokemonType
    {
        Normal,
        Fire,
        Fighting,
        Water,
        Flying,
        Grass,
        Poison,
        Electric,
        Ground,
        Psychic,
        Rock,
        Ice,
        Bug,
        Dragon,
        Ghost,
        Dark,
        Steel,
        Fairy
    }
}
