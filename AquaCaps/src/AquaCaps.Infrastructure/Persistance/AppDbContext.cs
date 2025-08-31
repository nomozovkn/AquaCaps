using AquaCaps.Domain.Entity;
using Microsoft.EntityFrameworkCore;


namespace AquaCaps.Infrastructure.Persistance;

public class AppDbContext:DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderAssignment> OrderAssignments { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
