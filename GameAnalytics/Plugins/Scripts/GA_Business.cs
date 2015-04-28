/// <summary>
/// This class handles business events, such as ingame purchases.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameAnalyticsSDK
{
	public static class GA_Business 
	{
		#region public methods

		public static void NewEvent(string currency, int amount, string itemType, string itemId, string cartType, string receipt, bool autoFetchReceipt)
		{
			CreateNewEvent(currency, amount, itemType, itemId, cartType, receipt, autoFetchReceipt);
		}
		
		#endregion
		
		#region private methods

		private static void CreateNewEvent(string currency, int amount, string itemType, string itemId, string cartType, string receipt, bool autoFetchReceipt)
		{
			if (autoFetchReceipt)
			{
				GA_iOSWrapper.AddBusinessEventAndAutoFetchReceipt(currency, amount, itemType, itemId, cartType);
			}
			else
			{
				GA_iOSWrapper.AddBusinessEvent(currency, amount, itemType, itemId, cartType, receipt);
			}
		}
		
		#endregion
	}
}