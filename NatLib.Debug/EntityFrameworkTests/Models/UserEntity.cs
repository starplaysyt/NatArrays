using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NatLib.EntityFramework.Application;
using NatLib.EntityFramework.Domain;
using NatLib.EntityFramework.Infrastructure;

namespace NatLib.Debug.EntityFrameworkTests.Models;

[Table("users"), Index(nameof(Login))]
public class UserEntity : DomainEntity<uint>
{
    [Column("login"), MaxLength(255)]
    public required string Login { get; set; }
    
    [Column("password"), MaxLength(255)]
    public required string Password { get; set; }
}

public record UserEntityUpdateMap(
    string? Login,
    string? Password);
    
public interface IUserEntityRepository : 
    IEntityRepository<UserEntity, uint, UserEntityUpdateMap>
{
}

public class UserEntityRepository(AppDbContext context) : 
    EntityRepository<UserEntity, uint, UserEntityUpdateMap>,
    IUserEntityRepository
{
    protected override DbContext Context => context;
    public override DbSet<UserEntity> DbSet => context.Users;
}