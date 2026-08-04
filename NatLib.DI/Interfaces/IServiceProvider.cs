namespace NatLib.DI.Interfaces;

public interface IServiceProvider
{
    object? GetService(Type serviceType);
    
    object GetRequiredService(Type serviceType);
    
    T? GetService<T>() where T : class;
    
    T GetRequiredService<T>() where T : class;
    
    IServiceScope CreateScope();
}