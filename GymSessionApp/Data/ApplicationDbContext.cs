using GymSessionApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GymSessionApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<GymSession> GymSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure GymSession entity
            modelBuilder.Entity<GymSession>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SessionName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.TrainerName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(500);
            });
        }
    }
}
