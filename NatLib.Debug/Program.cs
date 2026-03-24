using NatLib.UniConsole.Arguments;
using NatLib.UniConsole.Conversations;
using NatLib.UniConsole.Graphics;
using NatLib.UniConsole.Interfaces;

namespace NatLib.Debug;

public class Program
{
    public static void Main(string[] args)
    {
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
        // 						CanBeQuit = true,
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
        // 							CanBeQuit = true,
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
        // 								CanBeQuit = true,
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

        var convInt1 = 0;
        var userEnter = "";

        // TODO: Implement message element with variables support

        // var conversation = new ConversationQuery(
        // 	staticElements: new Dictionary<string, IConversationElement> {
        // 		{
        // 			"messageHeader",
        // 			new MessageConversationElement(
        // 				new MessageElementArgs
        // 				{
        // 					Message = "HeaderMessage",
        // 					ClearBeforeNext = false,
        // 					WaitForUserKey = false,
        // 					ClearAtTheEnd = false
        // 				},
        // 			null)
        // 		}
        // 	},
        // 	rootElement: 
        // 	// new MessageConversationElement(
        // 	// 	requestArgs: new MessageElementArgs
        // 	// 	{
        // 	// 		Message = "TestMessage1",
        // 	// 		ClearBeforeNext = false,
        // 	// 		WaitForUserKey = false
        // 	// 	},
        // 	// 	nextElement: new MessageConversationElement(
        // 	// 		new MessageElementArgs
        // 	// 		{
        // 	// 			Message = "TestMessage2",
        // 	// 			ClearBeforeNext = false,
        // 	// 			WaitForUserKey = false, ClearAtTheEnd = false
        // 	// 		},
        // 	// 		new MessageConversationElement(
        // 	// 			new MessageElementArgs
        // 	// 			{
        // 	// 				Message = "TestMessage3",
        // 	// 				ClearBeforeNext = false,
        // 	// 				WaitForUserKey = true
        // 	// 			}, 
        // 	// 		null)
        // 	// 	)
        // 	// )
        // 	new IntRequestConversationElement(
        // 		new RequestElementArgs<int>
        // 		{
        // 			Message = () => "Insert int (1 - 9): ",
        // 			RetryMessage = () => $"Incorrect input - ({userEnter})",
        // 			
        // 			RawValueBinding = val => userEnter = val,
        // 			ConvertedValueBinding = val => convInt1 = val,
        // 			
        // 			CanBeQuit = true,
        // 			ValidationDelegate = val => val is < 10 and > 0,
        // 			ClearAtTheEnd = true,
        // 			ReferencePresenter = null,
        // 			ClearBeforeNext = false,
        // 			ClearBeforeQuit = false,
        // 			QuitCreatesExecutionBranch = false,
        // 			QuitElement = new MessageConversationElement(
        // 				new MessageElementArgs
        // 				{
        // 					Message = "QuitMessage",
        // 					ClearBeforeNext = false,
        // 					WaitForUserKey = true,
        // 				}, 
        // 				null
        // 				)
        // 		},
        // 		new MessageConversationElement(
        // 			new MessageElementArgs
        // 			{
        // 				Message = "NextGlobalMessage",
        // 				ClearBeforeNext = false,
        // 				WaitForUserKey = true,
        // 			}, 
        // 			null
        // 		)
        // 	)
        // );
        //
        // conversation.Run();

        // var val = "null";
        //
        // var entry = new MessageConversationElement(
        //     new MessageElementArgs
        //     {
        //         ClearAtTheEnd = true, DistinctAfterUsage = false, Message = "TestMessage1", WaitForUserKey = true
        //     },
        //     new MessageConversationElement(
        //         new MessageElementArgs
        //         {
        //             ClearAtTheEnd = true, DistinctAfterUsage = true, Message = "TestMessage2", WaitForUserKey = true
        //         },
        //         new ValueRequestConversationElement<string>(
        //             new RequestElementArgs<string>
        //             {
        //                 Message = () => "TestMessage3", ConvertedValueBinding = value => val = value
        //             },
        //             null)
        //     )
        // );
        // entry.Start();
        
        // ConsoleRenderer.WriteTopBorder();
        // ConsoleRenderer.WriteMessageWrap("abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmn");
        // ConsoleRenderer.WriteMessageWrap("abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmn" +
        //                                       "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmn" + "");
        
        ConsoleRendererManualTests.WriteMessageLinesTest();
    }
}