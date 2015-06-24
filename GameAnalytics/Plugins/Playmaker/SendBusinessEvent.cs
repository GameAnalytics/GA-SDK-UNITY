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
		[Tooltip("Cart Type")]
		public FsmString CartType;

		[Tooltip("App Store Receipt, used for purchase validation")]
		public FsmString Receipt;
		
		[RequiredField]
		[Tooltip("If true the SDK will ignore the receipt parameter and attempt to automatically get the receipt.")]
		public FsmBool AutoFetchReceipt;
		
		public override void Reset()
		{
			Currency = new FsmString() { UseVariable = false };
			Amount = new FsmInt() { UseVariable = false };
			ItemType = new FsmString() { UseVariable = false };
			ItemID = new FsmString() { UseVariable = false };
			CartType = new FsmString() { UseVariable = false };
			Receipt = new FsmString() { UseVariable = false };
			AutoFetchReceipt = new FsmBool() { UseVariable = false };
		}
		
		public override void OnEnter()
		{
			GameAnalytics.NewBusinessEvent(Currency.Value, Amount.Value, ItemType.Value, ItemID.Value, CartType.Value, Receipt.Value, AutoFetchReceipt.Value);
			
			Finish();
		}
	}

	[ActionCategory("GameAnalytics")]
	[Tooltip("Sends an iOS business event message to the GameAnalytics server")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1163")]
	public class SendBusinessEventIOS : FsmStateAction
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
		[Tooltip("Cart Type")]
		public FsmString CartType;

		[Tooltip("App Store Receipt, used for purchase validation")]
		public FsmString Receipt;

		[RequiredField]
		[Tooltip("If true the SDK will ignore the receipt parameter and attempt to automatically get the receipt.")]
		public FsmBool AutoFetchReceipt;

		public override void Reset()
		{
			Currency = new FsmString() { UseVariable = false };
			Amount = new FsmInt() { UseVariable = false };
			ItemType = new FsmString() { UseVariable = false };
			ItemID = new FsmString() { UseVariable = false };
			CartType = new FsmString() { UseVariable = false };
			Receipt = new FsmString() { UseVariable = false };
			AutoFetchReceipt = new FsmBool() { UseVariable = false };
		}

		public override void OnEnter()
		{
			GA_Business.NewEventIOS(Currency.Value, Amount.Value, ItemType.Value, ItemID.Value, CartType.Value, Receipt.Value, AutoFetchReceipt.Value);

			Finish();
		}
	}

	[ActionCategory("GameAnalytics")]
	[Tooltip("Sends a Google Play business event message to the GameAnalytics server")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1163")]
	public class SendBusinessEventGooglePlay : FsmStateAction
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
		[Tooltip("Cart Type")]
		public FsmString CartType;

		[Tooltip("App Store Receipt, used for purchase validation")]
		public FsmString Receipt;

		[RequiredField]
		[Tooltip("Signature for In-App purchase, used for purchase validation")]
		public FsmString Signature;

		public override void Reset()
		{
			Currency = new FsmString() { UseVariable = false };
			Amount = new FsmInt() { UseVariable = false };
			ItemType = new FsmString() { UseVariable = false };
			ItemID = new FsmString() { UseVariable = false };
			CartType = new FsmString() { UseVariable = false };
			Receipt = new FsmString() { UseVariable = false };
			Signature = new FsmString() { UseVariable = false };
		}

		public override void OnEnter()
		{
			GameAnalytics.NewBusinessEventGooglePlay(Currency.Value, Amount.Value, ItemType.Value, ItemID.Value, CartType.Value, Receipt.Value, Signature.Value);

			Finish();
		}
	}
}

#endif