using NatLib.Debug.EntityFrameworkTests;
using NatLib.Debug.EntityFrameworkTests.Models;
using NatLib.DI;

public static class EntityFrameworkTestsProgram
{
    public static async Task Run()
    {
        var builder = new ServiceBuilder();

        builder.AddScoped<AppDbContext>();

        builder.AddScoped<IUserEntityRepository, UserEntityRepository>();
        builder.AddScoped<ITestEntity1Repository, TestEntity1Repository>();

        var provider = builder.Build();

        using (var initScope = provider.CreateScope())
        {
            await initScope.GetRequiredService<AppDbContext>().Database.EnsureCreatedAsync();
        }

        using (var scope = provider.CreateScope())
        {
            var testEntityRepo = scope.GetRequiredService<ITestEntity1Repository>();

            await testEntityRepo.CreateAsync(new TestEntity1()
            {
                Name = "testName", Value = 10, TestId = null
            });
    
            await testEntityRepo.SaveChangesAsync();

            var result = await testEntityRepo.GetAsync(e => e.Name == "testName");
    
            Console.WriteLine($"Creation test result: {(result == null ? "FAIL" : "PASS")}");
        }

        provider.Dispose();
    }
}

