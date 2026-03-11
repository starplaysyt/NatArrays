using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using NatLib.Arrays;
using NatLib.Core.Enums;
using NatLib.Core.Presenters;
using NatLib.Core.Utils;
using NatLib.UniConsole.Arguments;
using NatLib.UniConsole.Conversations;
using NatLib.UniConsole.Graphics;
using NatLib.UniConsole.Interfaces;

namespace NatLib.Debug;

public class Program
{
	public class TestingClassChild : IParsable<TestingClassChild>
	{
		public int A { get; set; }
		public int B { get; set; }
		
		public TestingClassChild(int a, int b) => (A, B) = (a, b);

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
	
    public class TestingStruct (string stringProp, float floatProp, int intProp, bool boolProp, DateTime dateTimeProp, TestingClassChild child)
    {
	    public string StringProp { get; set; } = stringProp;
		public float FloatProp { get; set; } = floatProp;
		public int IntProp { get; set; } = intProp;
		public bool BoolProp { get; set; } = boolProp;
		public DateTime DateTimeProp { get; set; } = dateTimeProp;
		public TestingClassChild Child { get; set; } = child;

		public TestingStruct() : this("", 0, 0, true, new DateTime(), new TestingClassChild(0,0))
		{
			
		}
    }
    
    public static void Main(string[] args)
    {
	 //    var list = new List<TestingStruct>
		// {
		//     new() { StringProp = "Alpha",       FloatProp = 1.5f,    IntProp = 10,   BoolProp = true,  DateTimeProp = new DateTime(2023, 1, 15),  Child = new TestingClassChild(1, 2) },
		//     new() { StringProp = "Bravo",       FloatProp = 2.7f,    IntProp = 25,   BoolProp = false, DateTimeProp = new DateTime(2023, 2, 20),  Child = new TestingClassChild(3, 4) },
		//     new() { StringProp = "Charlie",     FloatProp = 3.14f,   IntProp = 42,   BoolProp = true,  DateTimeProp = new DateTime(2023, 3, 10),  Child = new TestingClassChild(5, 6) },
		//     new() { StringProp = "Delta",       FloatProp = 4.0f,    IntProp = 100,  BoolProp = false, DateTimeProp = new DateTime(2023, 4, 5),   Child = new TestingClassChild(7, 8) },
		//     new() { StringProp = "Echo",        FloatProp = 5.55f,   IntProp = 3,    BoolProp = true,  DateTimeProp = new DateTime(2023, 5, 25),  Child = new TestingClassChild(9, 10) },
		//     new() { StringProp = "Foxtrot",     FloatProp = 6.1f,    IntProp = 77,   BoolProp = false, DateTimeProp = new DateTime(2023, 6, 30),  Child = new TestingClassChild(11, 12) },
		//     new() { StringProp = "Golf",        FloatProp = 7.89f,   IntProp = 55,   BoolProp = true,  DateTimeProp = new DateTime(2023, 7, 4),   Child = new TestingClassChild(13, 14) },
		//     new() { StringProp = "Hotel",       FloatProp = 8.2f,    IntProp = 200,  BoolProp = false, DateTimeProp = new DateTime(2023, 8, 18),  Child = new TestingClassChild(15, 16) },
		//     new() { StringProp = "India",       FloatProp = 9.99f,   IntProp = 0,    BoolProp = true,  DateTimeProp = new DateTime(2023, 9, 1),   Child = new TestingClassChild(17, 18) },
		//     new() { StringProp = "Juliet",      FloatProp = 10.0f,   IntProp = 33,   BoolProp = false, DateTimeProp = new DateTime(2023, 10, 12), Child = new TestingClassChild(19, 20) },
		//     new() { StringProp = "Kilo",        FloatProp = 11.11f,  IntProp = 8,    BoolProp = true,  DateTimeProp = new DateTime(2023, 11, 7),  Child = new TestingClassChild(21, 22) },
		//     new() { StringProp = "Lima",        FloatProp = 12.34f,  IntProp = 999,  BoolProp = false, DateTimeProp = new DateTime(2023, 12, 25), Child = new TestingClassChild(23, 24) },
		//     new() { StringProp = "Mike",        FloatProp = 0.5f,    IntProp = 64,   BoolProp = true,  DateTimeProp = new DateTime(2024, 1, 1),   Child = new TestingClassChild(25, 26) },
		//     new() { StringProp = "November",    FloatProp = 13.37f,  IntProp = 128,  BoolProp = false, DateTimeProp = new DateTime(2024, 2, 14),  Child = new TestingClassChild(27, 28) },
		//     new() { StringProp = "Oscar",       FloatProp = 99.9f,   IntProp = 7,    BoolProp = true,  DateTimeProp = new DateTime(2024, 3, 17),  Child = new TestingClassChild(29, 30) },
		//     new() { StringProp = "Papa",        FloatProp = 15.75f,  IntProp = 512,  BoolProp = false, DateTimeProp = new DateTime(2024, 4, 22),  Child = new TestingClassChild(31, 32) },
		//     new() { StringProp = "Quebec",      FloatProp = 16.0f,   IntProp = 48,   BoolProp = true,  DateTimeProp = new DateTime(2024, 5, 9),   Child = new TestingClassChild(33, 34) },
		//     new() { StringProp = "Romeo",       FloatProp = 17.17f,  IntProp = 256,  BoolProp = false, DateTimeProp = new DateTime(2024, 6, 15),  Child = new TestingClassChild(35, 36) },
		//     new() { StringProp = "Sierra",      FloatProp = 18.88f,  IntProp = 1024, BoolProp = true,  DateTimeProp = new DateTime(2024, 7, 20),  Child = new TestingClassChild(37, 38) },
		//     new() { StringProp = "Tango",       FloatProp = 19.01f,  IntProp = 15,   BoolProp = false, DateTimeProp = new DateTime(2024, 8, 31),  Child = new TestingClassChild(39, 40) },
		// };
	 //    
	 //    var bindingCollection = new BindingList<TestingStruct>(list);
	 //    
	 //    var presenter = new CollectionTablePresenter<TestingStruct>(bindingCollection);
	 //
	 //    presenter.ShowNumbers = true;
	 //    var res = presenter.BuildTable();
	 //    Console.WriteLine(res);
	 
		var bindingCollection = new BindingList<int> {1, 2, 3, 4, 5, 6, 7, 8};

		var presenter = new CollectionTablePresenter<int>(bindingCollection);
		var res = presenter.BuildTable();
		Console.WriteLine(res);


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
		//
		// var result = CollectionTablePresenter.BuildTable<PhoneCaller>(callers);
		//
		// Console.WriteLine(result);

		// var mat4 = new Mat4F();
		//
		// for (var i = 0; i < Mat4F.Size; i++)
		// {
		//     for (var j = 0; j < Mat4F.Size; j++)
		//     {
		//         mat4[i, j] = j * Mat4F.Size + i;
		//     }
		// }
		//
		// for (var i = 0; i < Mat4F.Size; i++)
		// {
		//     for (var j = 0; j < Mat4F.Size; j++)
		//     {
		//         Console.WriteLine(mat4[i, j]);
		//     }
		// }
		//
		// Console.WriteLine($"Float size: {sizeof(float)}");
		//
		// Console.WriteLine(mat4.ToString());

		// var mat1 = new Mat4F();
		// mat1[0, 0] = 1;
		// mat1[1, 1] = 2;
		// mat1[2, 2] = 3;
		// mat1[3, 3] = 4;
		//
		// var mat2 = new Mat4F();
		// mat2[0, 0] = 1;
		// mat2[1, 1] = 2;
		// mat2[2, 2] = 3;
		// mat2[3, 3] = 4;
		//
		// var mat3 = new Mat4F();
		// mat3[0, 0] = 1;
		// mat3[1, 1] = 2;
		// mat3[2, 2] = 3;
		//
		// var mat4 = new Mat4F();
		//
		// var fillArr = new float[16];
		// Array.Fill(fillArr, 0);
		// var mat5 = new Mat4F(fillArr);
		//
		// Console.WriteLine($"Result: {mat1.Equals(mat2)}");
		// Console.WriteLine($"Result: {mat2.Equals(mat1)}");
		//
		// Console.WriteLine($"Result: {mat3.Equals(mat1)}");
		// Console.WriteLine($"Result: {mat1.Equals(mat3)}");

		// var callersBinding = new BindingList<PhoneCaller>(callers);
		//
		// Console.ReadKey();
		//
		// var callersTable = new CollectionTablePresenter<PhoneCaller>(callersBinding)
		// {
		//     ShowNumbers = true
		// };
		//
		// var result = callersTable.BuildTable();
		//
		// Console.WriteLine(result);
		//
		// callersTable.Invalidate();
		// Console.WriteLine(callersTable.BuildTable());
		// Console.ReadKey();
		//
		// int[] lengths = [2, 15, 5, 15, 11];
		// string[] strs = ["str1", "str2", "str2", "str2", "str2"];
		//
		// Console.WriteLine(StringUtils.WrapJoin(strs, lengths, '|'));
		// Console.WriteLine(StringUtils.GenerateJoin('[', ']', '-', '|', lengths));
		//     
		// Console.ReadKey();

		// Stopwatch watch = Stopwatch.StartNew();

		//for (int i = 0; i < 100000; i++)
		//{
		//ConsoleRenderer.WriteFixedStringNext("string", 7, '=');
		//ConsoleRenderer.WriteTopBorder();
		//ConsoleRenderer.WriteMessageInBounds("testMessage");

		// ConsoleRenderer.WriteTopBorder();
		// ConsoleRenderer.WriteMessageInBounds("testMessage");
		// ConsoleRenderer.WriteSeparator();
		// ConsoleRenderer.WriteMessageInBounds("akjsjkdbjabskbdkjbjkabsjkbdjkbakjsbdjkbjkabsjkbdjkbaskjbdjbjakbsdjkbjkabskd");
		//ConsoleRenderer.WriteBottomBorder();
		//}
		// Console.WriteLine();
		// watch.Stop();
		// Console.WriteLine(watch.ElapsedMilliseconds);
		//
		// var str = "teststring";
		// Span<char> charspan = stackalloc char[45];
		//
		// int intval = 0;
		// float floatval = 10.3434f;
		// DateTime dt = DateTime.Now;
		// ConsoleColor enumval = ConsoleColor.Green;
		// int ptr = 0;
		//
		// str.TryCopyTo(charspan);
		// ptr += str.Length;
		//
		// intval.TryFormat(charspan[ptr..], out var written, "0000");
		// ptr += written;
		//
		// floatval.TryFormat(charspan[ptr..], out written, "F2");
		// ptr += written;
		//
		// dt.TryFormat(charspan[ptr..], out written, "yy-MM-dd");
		// ptr += written;
		//
		// Enum.TryFormat(enumval, charspan[ptr..], out written, "F");
		//
		//
		// Console.WriteLine(charspan);

		// var defs = ReflectionUtils.GetPropertyInfos(typeof(PhoneCaller)).FirstOrDefault(i => i.Name == "Phone");
		// var delegGet = ReflectionUtils.GetPropertyGetterDelegate(defs);
		// var delegSet = ReflectionUtils.GetPropertySetterDelegate(defs);
		//
		//
		// var caller = new PhoneCaller() { Phone = "testPhone"};
		//
		// delegSet.Invoke(caller, "+213123123123");
		//
		// var result = (string)delegGet.Invoke(caller) ?? "Error";
		//
		// Console.WriteLine(result);

		//var phoneCallerList = new BindingList<PhoneCaller>(new List<PhoneCaller>());

		//var tableCollectionPresenter = new CollectionTablePresenter<PhoneCaller>(phoneCallerList);
		// var tableCollectionMaxRecords = 10;
		//
		// var reqRecordId = 0;
		// var reqPropertyId = 0;
		// var reqPropertyValue = "";
		// object? reqPropertyConversionResult = null;
		//
		// var propertiesPresenter = new TypePropertiesPresenter(typeof(TestingStruct));

		// var conversation = new ConversationQuery(
		// 	new() {
		// 		{"tableElement", new TableConversationElement<PhoneCaller>(tableCollectionPresenter)},
		// 		{"infoElement", new MessageConversationElement("TestInfo: ")},
		// 	},
		// 	new MultipleChoiceConversationElement(
		// 		[
		// 			(
		// 				"Set record value", 
		// 				new IntRequestConversationElement(
		// 					requestArgs: new RequestElementArgs<int> {
		// 						MainMessage = "Select record id",
		// 						Quitable = true,
		// 						ValidationDelegate = value => {
		// 							return value <= tableCollectionMaxRecords && value > 0;
		// 						},
		// 						ValueSetterDelegate = value => {
		// 							reqRecordId = value;
		// 						},
		// 						RetryMessage = "Entry exception. Press Enter to retry or Space to leave..."
		// 					},
		// 					nextOperation: new IntRequestConversationElement(
		// 						requestArgs: new RequestElementArgs<int>() {
		// 							MainMessage = "Select property id",
		// 							Quitable = true,
		// 							ReferencePresenter = propertiesPresenter,
		// 							ReferenceNumeration = true,
		// 							ValidationDelegate = value => {
		// 								return value <= propertiesPresenter.Count && value > 0;
		// 							},
		// 							ValueSetterDelegate = value => {
		// 								reqPropertyId = value;
		// 							},
		// 							RetryMessage = "Entry exception. Press Enter to retry or Space to leave..."
		// 						},
		// 						nextOperation: new StringRequestConversationElement(
		// 							requestArgs: new RequestElementArgs<string> {
		// 								MainMessage = "Insert property value",
		// 								Quitable = true,
		// 								ValueSetterDelegate = value => reqPropertyValue = value,
		// 								ValidationDelegate = value => 
		// 									ConvertingUtils.TryReflectionConvert(
		// 										value, 
		// 										propertiesPresenter[reqPropertyId].PropertyType, 
		// 										out reqPropertyConversionResult),
		// 								RetryMessage = "Entry exception. Press Enter to retry or Space to leave..."
		// 							},
		// 							nextOperation: new OperationConversationElement(
		// 								requestArgs: new OperationElementArgs
		// 								{
		// 									MainMessage = "Setting property...",
		// 									OperationDelegate = 
		// 										() => propertiesPresenter.TrySetPropertyValue(
		// 												phoneCallerList[reqRecordId - 1], 
		// 												reqPropertyId, 
		// 												reqPropertyConversionResult
		// 												),
		// 									SuccessMessage = "Property setting complete.",
		// 									FailureMessage = "Property setting failed."
		// 								},
		// 								null
		// 							)
		// 						)
		// 					)
		// 				)
		// 			)
		// 		]
		// 	)
		// );

		// var conversation = new ConversationQuery(
		// 	staticElements: new Dictionary<string, IConversationElement> {
		// 		{
		// 			"messageHeader",
		// 			new MessageConversationElement(
		// 				new MessageElementArgs
		// 				{
		// 					Message = "HeaderMessage",
		// 					DistinctAfterUsage = false,
		// 					WaitForUserKey = false,
		// 				},
		// 			null)
		// 		}
		// 	},
		// 	rootElement: new MessageConversationElement(
		// 		requestArgs: new MessageElementArgs
		// 		{
		// 			Message = "TestMessage1",
		// 			DistinctAfterUsage = false,
		// 			WaitForUserKey = false
		// 		},
		// 		nextElement: new MessageConversationElement(
		// 			new MessageElementArgs
		// 			{
		// 				Message = "TestMessage2",
		// 				DistinctAfterUsage = false,
		// 				WaitForUserKey = false
		// 			},
		// 			new MessageConversationElement(
		// 				new MessageElementArgs
		// 				{
		// 					Message = "TestMessage3",
		// 					DistinctAfterUsage = false,
		// 					WaitForUserKey = true
		// 				}, 
		// 			null)
		// 		)
		// 	)
		// );
		//
		// conversation.Run();
    }
}

