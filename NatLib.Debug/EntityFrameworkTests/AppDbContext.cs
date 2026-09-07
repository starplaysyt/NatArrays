using Microsoft.EntityFrameworkCore;
using NatLib.Debug.EntityFrameworkTests.Models;

namespace NatLib.Debug.EntityFrameworkTests;

public class AppDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<TestEntity1> TestEntities { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=app.db");
    }
}