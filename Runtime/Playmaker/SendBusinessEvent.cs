#if false

using System;
using GameAnalyticsSDK;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("GameAnalytics")]
	[Tooltip("Sends a business event message to the GameAnalytics server with no validation")]
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

		public override void Reset()
		{
			Currency = new FsmString() { UseVariable = false };
			Amount = new FsmInt() { UseVariable = false };
			ItemType = new FsmString() { UseVariable = false };
			ItemID = new FsmString() { UseVariable = false };
			CartType = new FsmString() { UseVariable = false };
		}

		public override void OnEnter()
		{
			GameAnalytics.NewBusinessEvent(Currency.Value, Amount.Value, ItemType.Value, ItemID.Value, CartType.Value);

			Finish();
		}
	}

	[ActionCategory("GameAnalytics")]
	[Tooltip("Sends a business event message to the GameAnalytics server, validated against the App Store (iOS, tvOS) or Google Play (Android)")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1163")]
	public class SendBusinessEventWithReceipt : FsmStateAction
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

		[Tooltip("Store the purchase was made in")]
		public GAStore Store;

		[Tooltip("App Store: transaction id of the purchase (StoreKit 1 transactionIdentifier or StoreKit 2 Transaction.id)")]
		public FsmString TransactionId;

		[Tooltip("Google Play: product id (SKU) of the purchase")]
		public FsmString ProductId;

		[Tooltip("Google Play: purchase token of the purchase")]
		public FsmString PurchaseToken;

		public override void Reset()
		{
			Currency = new FsmString() { UseVariable = false };
			Amount = new FsmInt() { UseVariable = false };
			ItemType = new FsmString() { UseVariable = false };
			ItemID = new FsmString() { UseVariable = false };
			CartType = new FsmString() { UseVariable = false };
			Store = GAStore.AppStore;
			TransactionId = new FsmString() { UseVariable = false };
			ProductId = new FsmString() { UseVariable = false };
			PurchaseToken = new FsmString() { UseVariable = false };
		}

		public override void OnEnter()
		{
			GAReceiptInfo receipt = Store == GAStore.GooglePlay
				? GAReceiptInfo.GooglePlay(ProductId.Value, PurchaseToken.Value)
				: GAReceiptInfo.AppStore(TransactionId.Value);
			GameAnalytics.NewBusinessEvent(Currency.Value, Amount.Value, ItemType.Value, ItemID.Value, CartType.Value, receipt);

			Finish();
		}
	}

#if UNITY_IOS || UNITY_TVOS
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
GameAnalytics.NewBusinessEventIOS(Currency.Value, Amount.Value, ItemType.Value, ItemID.Value, CartType.Value, Receipt.Value);

Finish();
}
}

[ActionCategory("GameAnalytics")]
[Tooltip("Sends an iOS business with auto fetch receipt event message to the GameAnalytics server")]
[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1163")]
public class SendBusinessEventIOSAutoFetchReceipt : FsmStateAction
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

public override void Reset()
{
Currency = new FsmString() { UseVariable = false };
Amount = new FsmInt() { UseVariable = false };
ItemType = new FsmString() { UseVariable = false };
ItemID = new FsmString() { UseVariable = false };
CartType = new FsmString() { UseVariable = false };
}

public override void OnEnter()
{
GameAnalytics.NewBusinessEventIOSAutoFetchReceipt(Currency.Value, Amount.Value, ItemType.Value, ItemID.Value, CartType.Value);

Finish();
}
}
#endif

#if UNITY_ANDROID
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
#endif
}

#endif
