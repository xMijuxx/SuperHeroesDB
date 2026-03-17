using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuperHeroesDB.Models
{
    public class HeroPower
    {
        public int HeroId { get; set; } //klucz obcy do tabeli Hero
        public virtual Hero? Hero { get; set; }

        public int PowerId { get; set; } //klucz obcy do tabeli Power
        public virtual Power? Power { get; set; }

    }
}
