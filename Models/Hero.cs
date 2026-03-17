using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuperHeroesDB.Models
{
    public class Hero
    {
        public int HeroId { get; set; } //klucz główny

        [Required]
        [MaxLength(50)]
        public string HeroName { get; set; }

        [Required]
        [MaxLength(50)]
        public required string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public required string LastName { get; set; }

        [Required]
        public int TeamId  { get; set; } //klucz obcy
        public virtual Team? Team { get; set; }

        public virtual ICollection<HeroPower>? HeroPowers { get; set; }
    }
}
