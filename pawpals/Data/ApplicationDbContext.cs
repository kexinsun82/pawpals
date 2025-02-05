using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using pawpals.Models;

namespace pawpals.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Member> Members { get; set; }
    public DbSet<Pet> Pets { get; set; }
    public DbSet<Connection> Connections { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
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