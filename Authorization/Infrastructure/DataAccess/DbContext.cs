using Authorization.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Infrastructure.DataAccess;

public class AppDbContext:DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<PasswordEntity> Passwords { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>().HasKey(x => x.Id);
        modelBuilder.Entity<UserEntity>().HasOne(x => x.PasswordEntity).WithOne(y=>y.User).HasForeignKey<PasswordEntity>(x=>x.UserId);
        modelBuilder.Entity<PasswordEntity>().HasKey(x=>x.Id);
    }
}