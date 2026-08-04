namespace NatLib.DI.Exceptions;

public class DependencyResolutionException : Exception
{
    public Type ServiceType { get; }
        
    public DependencyResolutionException(Type serviceType, string message)
        : base(message)
    {
        ServiceType = serviceType;
    }

    public DependencyResolutionException(Type serviceType, string message, Exception innerException)
        : base(message, innerException)
    {
        ServiceType = serviceType;
    }
}