using Microsoft.EntityFrameworkCore;
using SuperHeroesDB.Models;

namespace SuperHeroesDB.Data
{
    public class HeroesContext : DbContext
    {
        public HeroesContext(DbContextOptions<HeroesContext> options) : base(options)
        {
        }

        public DbSet<Hero> Heroes { get; set; }
        public DbSet<Power> Powers { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<HeroPower> HeroPowers { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HeroPower>()
                .HasKey(hp => new { hp.HeroId, hp.PowerId });

            modelBuilder.Entity<HeroPower>()
                .HasOne(hp => hp.Hero)
                .WithMany(h => h.HeroPowers)
                .HasForeignKey(hp => hp.HeroId);

            modelBuilder.Entity<HeroPower>()
                .HasOne(hp => hp.Power)
                .WithMany(p => p.HeroPowers)
                .HasForeignKey(hp => hp.PowerId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
