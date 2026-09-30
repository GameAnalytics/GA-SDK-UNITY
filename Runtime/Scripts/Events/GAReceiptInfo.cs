namespace GameAnalyticsSDK
{
    /// <summary>
    /// Store identifiers of an in-app purchase, used to validate a business event server side.
    /// Create one with <see cref="AppStore"/> or <see cref="GooglePlay"/>.
    /// </summary>
    public readonly struct GAReceiptInfo
    {
        public GAStore Store { get; }

        /// <summary>App Store: SKPaymentTransaction.transactionIdentifier (StoreKit 1) or String(Transaction.id) (StoreKit 2).</summary>
        public string TransactionId { get; }

        /// <summary>Google Play: product id (SKU) of the purchase.</summary>
        public string ProductId { get; }

        /// <summary>Google Play: purchase token of the purchase.</summary>
        public string PurchaseToken { get; }

        private GAReceiptInfo(GAStore store, string transactionId, string productId, string purchaseToken)
        {
            Store = store;
            TransactionId = transactionId;
            ProductId = productId;
            PurchaseToken = purchaseToken;
        }

        /// <summary>Receipt of an App Store purchase (iOS, tvOS).</summary>
        public static GAReceiptInfo AppStore(string transactionId)
        {
            return new GAReceiptInfo(GAStore.AppStore, transactionId, null, null);
        }

        /// <summary>Receipt of a Google Play purchase (Android).</summary>
        public static GAReceiptInfo GooglePlay(string productId, string purchaseToken)
        {
            return new GAReceiptInfo(GAStore.GooglePlay, null, productId, purchaseToken);
        }
    }
}
