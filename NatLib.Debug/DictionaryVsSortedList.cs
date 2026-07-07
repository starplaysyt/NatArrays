using System.Diagnostics;
using NatLib.UniConsole.Graphics;

namespace NatLib.Debug;

public class DictionaryVsSortedList
{
    public static void Run()
    {
                var random = Random.Shared;

        var res = new Dictionary<(int, int), ConsoleColorExt>
        {
            {
                (random.Next(0, 100), random.Next(0, 100)), ConsoleColorExt.Red
            },
            {
                (random.Next(0, 100), random.Next(0, 100)), ConsoleColorExt.Green
            },
            {
                (random.Next(0, 100), random.Next(0, 100)), ConsoleColorExt.Yellow
            },
            {
                (random.Next(0, 100), random.Next(0, 100)), ConsoleColorExt.Blue
            },
            {
                (random.Next(0, 100), random.Next(0, 100)), ConsoleColorExt.Cyan
            },
            {
                (random.Next(0, 100), random.Next(0, 100)), ConsoleColorExt.Magenta
            },
        };

        var sortedList = new SortedList<(int, int), ConsoleColorExt>(res);

        foreach (var item in sortedList)
        {
            Console.WriteLine($"sortedList {item.Key} {item.Value}");
        }

        foreach (var item in res)
        {
            Console.WriteLine($"dict {item.Key} {item.Value}");
        }

        var resultFound = 0;
        
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        for (int k = 0; k < 1000; k++)
        {
            for (int i = 0; i < 1000; i++)
            {
                for (int j = 0; j < 1000; j++)
                {
                    if (res.ContainsKey((i, j)))
                    {
                        resultFound++;
                        // Console.WriteLine($"dict found {i} {j} = {res[(i, j)]}");
                    }
                }
            }
        }
        
        stopwatch.Stop();

        Console.WriteLine($"Animal planet took {stopwatch.ElapsedMilliseconds}ms, found {resultFound} entries.");

        var resultFound2 = 0;
        stopwatch.Restart();
        
        var enumerator = sortedList.GetEnumerator();
        
        var currentElement = enumerator.Current;
        
        for (int k = 0; k < 1000; k++)
        {
            enumerator.Reset();
            enumerator.MoveNext();
            currentElement = enumerator.Current;
            for (var i = 0; i < 1000; i++)
            {
                for (var j = 0; j < 1000; j++)
                {
                    if (currentElement.Key.Item1 != i || currentElement.Key.Item2 != j) continue;
                    
                    resultFound2++;
                        
                    enumerator.MoveNext();
                    currentElement = enumerator.Current;
                }
            }
        }
        
        stopwatch.Stop();
        
        Console.WriteLine($"Animal planet took {stopwatch.ElapsedMilliseconds}ms, found {resultFound2} entries.");
    }
}