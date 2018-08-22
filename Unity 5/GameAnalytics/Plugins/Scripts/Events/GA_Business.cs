/// <summary>
/// This class handles business events, such as ingame purchases.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK.Wrapper;

namespace GameAnalyticsSDK.Events
{
	public static class GA_Business
	{
        #region public methods

#if (UNITY_IOS || UNITY_TVOS)
		public static void NewEvent(string currency, int amount, string itemType, string itemId, string cartType, string receipt, bool autoFetchReceipt, IDictionary<string, object> fields)
		{
			if(autoFetchReceipt)
			{
				GA_Wrapper.AddBusinessEventAndAutoFetchReceipt(currency, amount, itemType, itemId, cartType, fields);
			}
			else
			{
				GA_Wrapper.AddBusinessEvent(currency, amount, itemType, itemId, cartType, receipt, fields);
			}
		}

		public static void NewEvent(string currency, int amount, string itemType, string itemId, string cartType, IDictionary<string, object> fields)
		{
			NewEvent(currency, amount, itemType, itemId, cartType, null, false, fields);
		}
#endif

#if (UNITY_ANDROID)
		public static void NewEventGooglePlay(string currency, int amount, string itemType, string itemId, string cartType, string receipt, string signature, IDictionary<string, object> fields)
		{
			GA_Wrapper.AddBusinessEventWithReceipt(currency, amount, itemType, itemId, cartType, receipt, "google_play", signature, fields);
		}
#endif

#if (!UNITY_IOS && !UNITY_TVOS)
        public static void NewEvent(string currency, int amount, string itemType, string itemId, string cartType, IDictionary<string, object> fields)
		{
			GA_Wrapper.AddBusinessEvent(currency, amount, itemType, itemId, cartType, fields);
		}
		#endif
		#endregion
	}
}