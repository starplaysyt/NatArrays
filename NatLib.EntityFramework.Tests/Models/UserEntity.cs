using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NatLib.EntityFramework.Domain;

namespace NatLib.EntityFramework.Tests.Models;

[Table("users"), Index(nameof(Login))]
public class UserEntity : DomainEntity<uint>
{
    [Column("login"), MaxLength(255)]
    public required string Login { get; set; }
    
    [Column("password"), MaxLength(255)]
    public required string Password { get; set; }
}