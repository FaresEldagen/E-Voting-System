using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Voting_System.Entities;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Custom configurations
        modelBuilder.Entity<User>().Property(u => u.Id)
            .ValueGeneratedNever()
            .HasMaxLength(14);
            
        modelBuilder.Entity<User>().Property(u => u.Vote)
            .HasColumnType("INT")
            .HasDefaultValue(0);
    }
}

