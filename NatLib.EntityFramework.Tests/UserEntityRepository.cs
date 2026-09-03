using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using NatLib.EntityFramework.Infrastructure;
using NatLib.EntityFramework.Tests.Models;
using NatLib.EntityFramework.Tests.UpdateMaps;

namespace NatLib.EntityFramework.Tests;

public class UserEntityRepository(AppDbContext context) : 
    EntityRepository<UserEntity, uint, UserEntityUpdateMap>
{
    protected override DbContext Context => context;
    public override DbSet<UserEntity> DbSet => context.Users;
}