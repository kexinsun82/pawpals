using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using pawpals.Models;

namespace pawpals.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public new DbSet<User> Users { get; set; }
    public DbSet<Pet> Pets { get; set; }
    public DbSet<Connection> Connections { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Identity tables
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("VARCHAR(255)");
                entity.ToTable("AspNetUsers");
            });

            modelBuilder.Entity<ApplicationRole>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("VARCHAR(255)");
                entity.ToTable("AspNetRoles");
            });

            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnType("VARCHAR(255)");
                entity.Property(e => e.RoleId).HasColumnType("VARCHAR(255)");
                entity.ToTable("AspNetUserRoles");
            });

            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnType("VARCHAR(255)");
                entity.ToTable("AspNetUserClaims");
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnType("VARCHAR(255)");
                entity.ToTable("AspNetUserLogins");
            });

            // Connection table relationships
            modelBuilder.Entity<Connection>()
                .HasOne(c => c.Follower)
                .WithMany(u => u.Followers)
                .HasForeignKey(c => c.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Connection>()
                .HasOne(c => c.Following)
                .WithMany(u => u.Following)
                .HasForeignKey(c => c.FollowingId)
                .OnDelete(DeleteBehavior.Restrict);
        }


}
