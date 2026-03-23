using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using NatLib.Core.Utils;

namespace NatLib.Debug;

public static class CollectionTablePresenterDebug
{
    public class TestingClassChild : IParsable<TestingClassChild>
    {
        public int A { get; set; }
        public int B { get; set; }

        public TestingClassChild(int a, int b)
        {
            (A, B) = (a, b);
        }

        public override string ToString() => "(A : B)";

        public static TestingClassChild Parse(string s, IFormatProvider? provider)
        {
            var span = s.AsSpan()[1..^2];
            var sep = span.IndexOf(':');
            var a = int.Parse(span[..sep].Trim());
            var b = int.Parse(span[(sep + 1)..].Trim());

            return new TestingClassChild(a, b);
        }

        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out TestingClassChild result)
        {
            try
            {
                var span = s.AsSpan()[1..^2];
                var sep = span.IndexOf(':');
                var a = int.Parse(span[..sep].Trim());
                var b = int.Parse(span[(sep + 1)..].Trim());

                result = new TestingClassChild(a, b);
                return true;
            }
            catch (Exception)
            {
                result = null;
                return false;
            }
        }
    }

    public class TestingStruct(string stringProp, float floatProp, int intProp, bool boolProp, DateTime dateTimeProp, TestingClassChild child)
    {
        public string StringProp { get; set; } = stringProp;
        public float FloatProp { get; set; } = floatProp;
        public int IntProp { get; set; } = intProp;
        public bool BoolProp { get; set; } = boolProp;
        public DateTime DateTimeProp { get; set; } = dateTimeProp;
        public TestingClassChild Child { get; set; } = child;

        public TestingStruct() : this("", 0, 0, true, new DateTime(), new TestingClassChild(0, 0))
        {

        }
    }

    public static void MainMethod()
    {
        var list = new List<TestingStruct>
		{
		    new() { StringProp = "Alpha",       FloatProp = 1.5f,    IntProp = 10,   BoolProp = true,  DateTimeProp = new DateTime(2023, 1, 15),  Child = new TestingClassChild(1, 2) },
		    new() { StringProp = "Bravo",       FloatProp = 2.7f,    IntProp = 25,   BoolProp = false, DateTimeProp = new DateTime(2023, 2, 20),  Child = new TestingClassChild(3, 4) },
		    new() { StringProp = "Charlie",     FloatProp = 3.14f,   IntProp = 42,   BoolProp = true,  DateTimeProp = new DateTime(2023, 3, 10),  Child = new TestingClassChild(5, 6) },
		    new() { StringProp = "Delta",       FloatProp = 4.0f,    IntProp = 100,  BoolProp = false, DateTimeProp = new DateTime(2023, 4, 5),   Child = new TestingClassChild(7, 8) },
		    new() { StringProp = "Echo",        FloatProp = 5.55f,   IntProp = 3,    BoolProp = true,  DateTimeProp = new DateTime(2023, 5, 25),  Child = new TestingClassChild(9, 10) },
		    new() { StringProp = "Foxtrot",     FloatProp = 6.1f,    IntProp = 77,   BoolProp = false, DateTimeProp = new DateTime(2023, 6, 30),  Child = new TestingClassChild(11, 12) },
		    new() { StringProp = "Golf",        FloatProp = 7.89f,   IntProp = 55,   BoolProp = true,  DateTimeProp = new DateTime(2023, 7, 4),   Child = new TestingClassChild(13, 14) },
		    new() { StringProp = "Hotel",       FloatProp = 8.2f,    IntProp = 200,  BoolProp = false, DateTimeProp = new DateTime(2023, 8, 18),  Child = new TestingClassChild(15, 16) },
		    new() { StringProp = "India",       FloatProp = 9.99f,   IntProp = 0,    BoolProp = true,  DateTimeProp = new DateTime(2023, 9, 1),   Child = new TestingClassChild(17, 18) },
		    new() { StringProp = "Juliet",      FloatProp = 10.0f,   IntProp = 33,   BoolProp = false, DateTimeProp = new DateTime(2023, 10, 12), Child = new TestingClassChild(19, 20) },
		    new() { StringProp = "Kilo",        FloatProp = 11.11f,  IntProp = 8,    BoolProp = true,  DateTimeProp = new DateTime(2023, 11, 7),  Child = new TestingClassChild(21, 22) },
		    new() { StringProp = "Lima",        FloatProp = 12.34f,  IntProp = 999,  BoolProp = false, DateTimeProp = new DateTime(2023, 12, 25), Child = new TestingClassChild(23, 24) },
		    new() { StringProp = "Mike",        FloatProp = 0.5f,    IntProp = 64,   BoolProp = true,  DateTimeProp = new DateTime(2024, 1, 1),   Child = new TestingClassChild(25, 26) },
		    new() { StringProp = "November",    FloatProp = 13.37f,  IntProp = 128,  BoolProp = false, DateTimeProp = new DateTime(2024, 2, 14),  Child = new TestingClassChild(27, 28) },
		    new() { StringProp = "Oscar",       FloatProp = 99.9f,   IntProp = 7,    BoolProp = true,  DateTimeProp = new DateTime(2024, 3, 17),  Child = new TestingClassChild(29, 30) },
		    new() { StringProp = "Papa",        FloatProp = 15.75f,  IntProp = 512,  BoolProp = false, DateTimeProp = new DateTime(2024, 4, 22),  Child = new TestingClassChild(31, 32) },
		    new() { StringProp = "Quebec",      FloatProp = 16.0f,   IntProp = 48,   BoolProp = true,  DateTimeProp = new DateTime(2024, 5, 9),   Child = new TestingClassChild(33, 34) },
		    new() { StringProp = "Romeo",       FloatProp = 17.17f,  IntProp = 256,  BoolProp = false, DateTimeProp = new DateTime(2024, 6, 15),  Child = new TestingClassChild(35, 36) },
		    new() { StringProp = "Sierra",      FloatProp = 18.88f,  IntProp = 1024, BoolProp = true,  DateTimeProp = new DateTime(2024, 7, 20),  Child = new TestingClassChild(37, 38) },
		    new() { StringProp = "Tango",       FloatProp = 19.01f,  IntProp = 15,   BoolProp = false, DateTimeProp = new DateTime(2024, 8, 31),  Child = new TestingClassChild(39, 40) },
		};
	    
	    var bindingCollection = new BindingList<TestingStruct>(list);
	    
	    var presenter = new CollectionTablePresenter<TestingStruct>(bindingCollection);
	 
	    presenter.ShowNumbers = true;
	    var res = presenter.BuildTable();
	    Console.WriteLine(res);
    }
}