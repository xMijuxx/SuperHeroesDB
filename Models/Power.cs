using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuperHeroesDB.Models
{
    public class Power
    {
        public int PowerId { get; set; } //klucz główny

        [Required]
        [MaxLength(50)]
        public string PowerName { get; set; }

        public virtual ICollection<HeroPower>? HeroPowers { get; set; }
    }
}
