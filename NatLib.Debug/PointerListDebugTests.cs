using NatLib.Arrays;

namespace NatLib.Debug;

public static class PointerListDebugTests
{
    public static void Run()
    {
        var list = new PointerList<int>();

        for (var i = 0; i < 1000000; i++)
        {
            list.Add(i);
        }
        
        var span = list.AsSpan();

        foreach (var i in span)
        {
            Console.Write(i + " ");
        }
        
        Console.WriteLine($"CAP: {list.Capacity} LEN: {list.Length}");

        Console.ReadKey();
        
        for (var i = 0; i < 80; i++)
        {
            Console.WriteLine($"deleting {list[20]}");
            list.Delete(20);
        }
        
        Console.ReadKey();
        
        span = list.AsSpan();

        foreach (var i in span)
        {
            Console.Write(i + " ");
        }

        Console.WriteLine($"CAP: {list.Capacity} LEN: {list.Length}");
        
        Console.ReadKey();
        
        list.Clear();
        
        Console.ReadKey();
    }
}