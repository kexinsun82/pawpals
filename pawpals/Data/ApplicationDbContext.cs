using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using pawpals.Models;

namespace pawpals.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Pet> Pets { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}
