using NatLib.EntityFramework.Tests;
using NatLib.EntityFramework.Tests.Models;

using var dbContext = new AppDbContext();

dbContext.Database.EnsureCreated();

dbContext.Users.Add(new UserEntity
{
    Login = "testLogin",
    Password = "testPassword",
});

dbContext.SaveChanges();



