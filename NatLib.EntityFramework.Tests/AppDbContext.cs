using Microsoft.EntityFrameworkCore;
using NatLib.EntityFramework.Tests.Models;

namespace NatLib.EntityFramework.Tests;

public class AppDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; } 
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=app.db");
    }
}