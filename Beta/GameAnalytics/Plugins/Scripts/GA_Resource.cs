using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameAnalyticsSDK
{
	public static class GA_Resource
	{
		public enum GAResourceFlowType
		{
			// Source: Used when adding resource to a user
			GAResourceFlowTypeSource = 1,
			// Sink: Used when removing a resource from a user
			GAResourceFlowTypeSink = 2
		}

		#region public methods

		public static void NewEvent(GAResourceFlowType flowType, string currency, float amount, string itemType, string itemId)
		{
			GA_Wrapper.AddResourceEvent(flowType, currency, amount, itemType, itemId);
		}

		#endregion
	}
}