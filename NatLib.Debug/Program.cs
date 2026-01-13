using NatLib.Core.Enums;
using NatLib.Core.Operations;
using NatLib.Core.Utils;
using NatLib.UniConsole.Utils;

namespace NatLib.Debug;

public class Program
{
    public class PhoneCaller
    {
        public string Phone { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }
        
        public ConsoleColor Color { get; set; }
    }
    
    public static void Main(string[] args)
    {
        // Console.WriteLine("Hello World!");
        //
        // const string str1 = "1234567890";
        // const string str2 = "12345";
        //
        // Console.WriteLine($"+{str1.FixCenter(7, '_')}+");
        // Console.WriteLine($"+{str2.FixCenter(7, '_')}+");
        //
        // Console.WriteLine($"+{str1.FixCenter(7, '_')}+");
        // Console.WriteLine($"+{str2.FixCenter(7, '_')}+");
        //
        // // Console.WriteLine("ABC".Fix(5));
        // // Console.WriteLine("ABC".FixRight(5, '_'));
        // // Console.WriteLine("ABCDEFG".FixRight(4));
        // // Console.WriteLine("AB".FixRight(2));
        // // Console.WriteLine("AB".FixRight(0));
        //
        // string[] stringArray =
        // [
        //     "2",
        //     "5",
        //     "",
        //     "10",
        //     "d",
        //     "",
        //     "string7aass",
        //     "string8asdasd",
        //     "string9asdasdsadsd"
        // ];
        //
        // var sizes = new int[stringArray.Length];
        //
        // Array.Fill(sizes, 15);
        //
        // Console.WriteLine(StringUtils.WrapJoin(stringArray, sizes, '+', Alignment.Center));
        // Console.WriteLine(StringUtils.WrapJoin(stringArray, sizes, '+', Alignment.Center));
        // Console.WriteLine(StringUtils.WrapJoin(stringArray, sizes, '+', Alignment.Center));
        // Console.WriteLine(StringUtils.WrapJoin(stringArray, sizes, '+', Alignment.Center));
        // Console.WriteLine(StringUtils.WrapJoin(stringArray, sizes, '+', Alignment.Center));

        // var conv = new GetIntValueConversation();
        // conv.RunConversation();

        // RequestingUtils.RequestEnter<int>("Insert int value: ");
        //
        // RequestingUtils.RequestEnterWithExit("Insert enum:", typeof(ConsoleColor), out var value);
        //
        // Console.WriteLine((ConsoleColor)value);
        //RequestingUtils.RequestEnter<ConsoleColor>("Insert enum value: ");

        var callers = new List<PhoneCaller>
        {
            new() { Phone = "+1-202-555-0101", Count = 5,  Name = "Alice Johnson",     Color = ConsoleColor.Cyan },
            new() { Phone = "+1-202-555-0102", Count = 12, Name = "Bob Smith",         Color = ConsoleColor.Yellow },
            new() { Phone = "+1-202-555-0103", Count = 3,  Name = "Charlie Brown",     Color = ConsoleColor.Green },
            new() { Phone = "+1-202-555-0104", Count = 7,  Name = "Diana Prince",      Color = ConsoleColor.Magenta },
            new() { Phone = "+1-202-555-0105", Count = 18, Name = "Edward Wilson",     Color = ConsoleColor.Blue },
            new() { Phone = "+1-202-555-0106", Count = 1,  Name = "Fiona Gallagher",   Color = ConsoleColor.Red },
            new() { Phone = "+1-202-555-0107", Count = 9,  Name = "George Miller",     Color = ConsoleColor.Gray },
            new() { Phone = "+1-202-555-0108", Count = 14, Name = "Hannah Davis",      Color = ConsoleColor.DarkYellow },
            new() { Phone = "+1-202-555-0109", Count = 2,  Name = "Ian Thompson",      Color = ConsoleColor.DarkCyan },
            new() { Phone = "+1-202-555-0110", Count = 11, Name = "Julia Roberts",     Color = ConsoleColor.DarkGreen },

            new() { Phone = "+1-202-555-0111", Count = 4,  Name = "Kevin Anderson",    Color = ConsoleColor.White },
            new() { Phone = "+1-202-555-0112", Count = 6,  Name = "Laura Martinez",    Color = ConsoleColor.DarkMagenta },
            new() { Phone = "+1-202-555-0113", Count = 10, Name = "Michael Scott",     Color = ConsoleColor.DarkGray },
            new() { Phone = "+1-202-555-0114", Count = 8,  Name = "Natalie Portman",   Color = ConsoleColor.DarkBlue },
            new() { Phone = "+1-202-555-0115", Count = 20, Name = "Oliver Stone",      Color = ConsoleColor.Yellow },
            new() { Phone = "+1-202-555-0116", Count = 13, Name = "Paula Walker",      Color = ConsoleColor.Green },
            new() { Phone = "+1-202-555-0117", Count = 16, Name = "Quentin Blake",     Color = ConsoleColor.Cyan },
            new() { Phone = "+1-202-555-0118", Count = 15, Name = "Rachel Adams",      Color = ConsoleColor.Blue },
            new() { Phone = "+1-202-555-0119", Count = 19, Name = "Steven King",       Color = ConsoleColor.Red },
            new() { Phone = "+1-202-555-0120", Count = 17, Name = "Tina Turner",       Color = ConsoleColor.Magenta }
        };

        var result = CollectionTableBuilder.BuildTable<PhoneCaller>(callers);
        
        Console.WriteLine(result);
    }
}

