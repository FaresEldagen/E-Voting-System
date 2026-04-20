using Microsoft.EntityFrameworkCore;

namespace E_Voting_System.Entities;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("");
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<User>().Property(u => u.Id)
            .ValueGeneratedNever();
        modelBuilder.Entity<User>().Property(u => u.Vote)
            .HasColumnType("INT")
            .IsRequired(false);
    }
}

