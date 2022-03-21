using Domination_WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Domination_WebAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<GameNode> GameNodes { get; set; }
        public DbSet<GameZone> GameZones { get; set; }
        public DbSet<NodeImprovement> NodeImprovement { get; set; }
        public DbSet<PlayerResearchTech> PlayerResearchTechs { get; set; }
        public DbSet<ResearchTech> ResearchTech { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Bonus> Bonuses { get; set; }
        public DbSet<PlayerBonus> PlayerBonus { get; set; }
        public DbSet<GameZoneClaim> GameZoneClaims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<GameNode>()
                .HasOne(e => e.GameZone)
                .WithMany(f => f.Nodes)
                .HasForeignKey(e => e.GameZoneId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<GameZone>()
                .HasMany(x => x.Nodes)
                .WithOne(y => y.GameZone)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<GameNode>()
                .HasOne(e => e.NodeImprovement)
                .WithOne()
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
