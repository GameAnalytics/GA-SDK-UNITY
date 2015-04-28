#if false

using System;
using GameAnalyticsSDK;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("GameAnalytics")]
	[Tooltip("Sends a business event message to the GameAnalytics server")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1163")]
	public class SendBusinessEvent : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Abbreviation of the currency used for the transaction. F.x. USD (U.S. Dollars)")]
		public FsmString Currency;
		
		[RequiredField]
		[Tooltip("Amount of real currency, in cents")]
		public FsmInt Amount;

		[RequiredField]
		[Tooltip("Type of IAP item purchased (e.g. Coins)")]
		public FsmString ItemType;
		
		[RequiredField]
		[Tooltip("Specific item purchased (e.g. CoinPack001)")]
		public FsmString ItemID;

		[RequiredField]
		[Tooltip("")]
		public FsmString CartType;
		
		[RequiredField]
		[Tooltip("App Store Receipt, used for purchase validation")]
		public FsmString Receipt;
		
		public override void Reset()
		{
			Currency = new FsmString() { UseVariable = false };
			Amount = new FsmInt() { UseVariable = false };
			ItemType = new FsmString() { UseVariable = false };
			ItemID = new FsmString() { UseVariable = false };
			CartType = new FsmString() { UseVariable = false };
			Receipt = new FsmString() { UseVariable = false };
		}
		
		public override void OnEnter()
		{
			GA_Business.NewEvent(Currency.Value, Amount.Value, ItemType.Value, ItemID.Value, CartType.Value, Receipt.Value);
			
			Finish();
		}
	}
}

#endif