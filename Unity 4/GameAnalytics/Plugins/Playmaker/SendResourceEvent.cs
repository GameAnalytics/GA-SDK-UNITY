#if false

using System;
using GameAnalyticsSDK;
using GameAnalyticsSDK.Events;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("GameAnalytics")]
	[Tooltip("Sends a resource event message to the GameAnalytics server")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1164")]
	public class SendResourceEvent : FsmStateAction
	{
		[Tooltip("The resource flow type as string: 'source', 'sink' case insensitive, any other values are invalid")]
		public FsmString ResourceFlowTypeAsString;

		[Tooltip("The resource flow type: add (source) or remove (sink) resource")]
		public GAResourceFlowType ResourceFlowType;
		
		[RequiredField]
		[Tooltip("Type of virtual currency used (E.g. gold, lives)")]
		public FsmString ResourceCurrency;

		[RequiredField]
		[Tooltip("Amount of virtual currency used/gained in this event")]
		public FsmFloat Amount;
		
		[RequiredField]
		[Tooltip("Type of item purchased/used with virtual currency (E.g. boost, gameplay)")]
		public FsmString ItemType;
		
		[RequiredField]
		[Tooltip("Specific item purchased/used with virtual currency (E.g. rainbowboost, gamestart)")]
		public FsmString ItemID;
		
		public override void Reset()
		{
			ResourceFlowType = GAResourceFlowType.Source;
			ResourceCurrency = new FsmString() { UseVariable = false };
			Amount = new FsmFloat() { UseVariable = false };
			ItemType = new FsmString() { UseVariable = false };
			ItemID = new FsmString() { UseVariable = false };
		}
		
		public override void OnEnter()
		{
			GA_Resource.NewEvent(ResourceFlowType, ResourceCurrency.Value, Amount.Value, ItemType.Value, ItemID.Value);
			
			Finish();
		}
	}
}

#endif