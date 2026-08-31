using NatLib.DI;

namespace NatLib.Debug;

public static class DependencyInjectionDebug
{
    public interface ILogger
    {
        void Log(string message);
    }

    public class ConsoleLogger : ILogger, IDisposable
    {
        private readonly Guid _id = Guid.NewGuid();

        public void Log(string message)
        {
            Console.WriteLine($"[{_id}] {DateTime.Now:HH:mm:ss} | {message}");
        }

        public void Dispose()
        {
            Console.WriteLine($"[{_id}] Logger disposed");
        }
    }

    public interface IRepository
    {
        void Save(string data);
    }

    public class DatabaseRepository : IRepository
    {
        private readonly ILogger _logger;
        private readonly Guid _id = Guid.NewGuid();
        
        public DatabaseRepository(ILogger logger)
        {
            _logger = logger;
            _logger.Log($"DatabaseRepository {_id} created");
        }

        public void Save(string data)
        {
            _logger.Log($"[Repo {_id}] Saving: {data}");
        }
    }

    public interface IUserService
    {
        void CreateUser(string name);
    }

    public class UserService : IUserService
    {
        private readonly IRepository _repository;
        private readonly ILogger _logger;
        
        public UserService(IRepository repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public void CreateUser(string name)
        {
            _logger.Log($"Creating user: {name}");
            _repository.Save($"User:{name}");
        }
    }

    public static void Run()
    {
        var services = new ServiceBuilder();
        
        services.AddSingleton<ILogger, ConsoleLogger>();
        services.AddScoped<IRepository, DatabaseRepository>();
        services.AddTransient<IUserService, UserService>();
        
        var serviceProvider = services.Build();

        Console.WriteLine("Singleton: ");
        var logger1 = serviceProvider.GetRequiredService<ILogger>();
        var logger2 = serviceProvider.GetRequiredService<ILogger>();
        logger1.Log("Hello from logger1");
        logger2.Log("Hello from logger2");
        Console.WriteLine($"Same instance? {ReferenceEquals(logger1, logger2)}"); // true

        Console.WriteLine("Transient: ");
        using (var scope = serviceProvider.CreateScope())
        {
            var userService1 = scope.GetRequiredService<IUserService>();
            var userService2 = scope.GetRequiredService<IUserService>();
            Console.WriteLine($"Same instance? {ReferenceEquals(userService1, userService2)}"); // false
        }

        Console.WriteLine("Scoped: ");
        using (var scope = serviceProvider.CreateScope())
        {
            var repo1 = scope.GetRequiredService<IRepository>();
            var repo2 = scope.GetRequiredService<IRepository>();
            Console.WriteLine($"Same in scope? {ReferenceEquals(repo1, repo2)}"); // True

            repo1.Save("scoped data");
        }
        
        using (var scope2 = serviceProvider.CreateScope())
        {
            var repo3 = scope2.GetRequiredService<IRepository>();
            repo3.Save("another scope data");
        }

        Console.WriteLine("Full scenario: ");
        using (var scope = serviceProvider.CreateScope())
        {
            var userSvc = scope.GetRequiredService<IUserService>();
            userSvc.CreateUser("Alice");
            userSvc.CreateUser("Bob");
        }
        
        // !!! IMPORTANT - DISPOSE ROOT PROVIDER!
        Console.WriteLine("\nDisposing root provider");
        ((IDisposable)serviceProvider).Dispose();
    }
}