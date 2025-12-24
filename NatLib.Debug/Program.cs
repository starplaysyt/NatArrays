using NatLib.Core.Enums;
using NatLib.Core.Operations;

namespace NatLib.Debug;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");

        const string str1 = "1234567890";
        const string str2 = "12345";

        Console.WriteLine($"+{str1.FixCenter(7, '_')}+");
        Console.WriteLine($"+{str2.FixCenter(7, '_')}+");
        
        Console.WriteLine($"+{str1.FixCenter(7, '_')}+");
        Console.WriteLine($"+{str2.FixCenter(7, '_')}+");
        
        //
        // Console.WriteLine("ABC".Fix(5));
        // Console.WriteLine("ABC".FixRight(5, '_'));
        // Console.WriteLine("ABCDEFG".FixRight(4));
        // Console.WriteLine("AB".FixRight(2));
        // Console.WriteLine("AB".FixRight(0));

        string[] stringArray =
        [
            "2",
            "5",
            "",
            "10",
            "d",
            "",
            "string7aass",
            "string8asdasd",
            "string9asdasdsadsd"
        ];
        
        int[] sizes = new int[stringArray.Length];

        Array.Fill(sizes, 15);

        Console.WriteLine(StringExtensions.WrapJoin(stringArray, sizes, '+', SimpleAlignment.Center));
        Console.WriteLine(StringExtensions.WrapJoin(stringArray, sizes, '+', SimpleAlignment.Center));
        Console.WriteLine(StringExtensions.WrapJoin(stringArray, sizes, '+', SimpleAlignment.Center));
        Console.WriteLine(StringExtensions.WrapJoin(stringArray, sizes, '+', SimpleAlignment.Center));
        Console.WriteLine(StringExtensions.WrapJoin(stringArray, sizes, '+', SimpleAlignment.Center));
    }
}

