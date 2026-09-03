using NatLib.EntityFramework.Tests;
using NatLib.EntityFramework.Tests.Models;
using NatLib.EntityFramework.Tests.UpdateMaps;

var dbContext = new AppDbContext();

dbContext.Database.EnsureCreated();

dbContext.Dispose();

dbContext = new AppDbContext();
var repo = new UserEntityRepository(dbContext);

repo.UpdateAsync(e => e.Login == "testLogin", 
    new UserEntityUpdateMap("testLogin555", "testPass222"))
    .Wait();

repo.SaveChangesAsync().Wait();

dbContext.Dispose();

//
// dbContext.Users.Add(new UserEntity
// {
//     Login = "testLogin",
//     Password = "testPassword",
// });
//
// dbContext.SaveChanges();