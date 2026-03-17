using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuperHeroesDB.Models

{
    public class Team
    {
        public int TeamId { get; set; } //klucz główny

        [Required]
        [MaxLength(50)]
        public string TeamName { get; set; }

        public virtual ICollection<Hero>? Heroes { get; set; }
    }
}
